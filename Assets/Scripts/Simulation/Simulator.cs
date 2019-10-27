using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Simulator : Tickable
{
    public PhaseManager phaseManager;
    public IncomeManager incomeManager;
    public bool shouldRun = false;

    private Player player;
    private Board gameBoard;
    private int bonusGold = 1; // Earn 1 Bonus Gold for winning the round.

    public void SetGameBoard(Board board, Player player)
    {
        gameBoard = board;
        this.player = player;
    }

    public void StartSim()
    {
        gameBoard.ClearInteractionsToProcess();
        shouldRun = true;
        incomeManager.SetIncomeGeneratedByPlayer(player, 0); // Reset any bonus gold from winning.
    }

    public override void Tick(long tick)
    {
        if (!shouldRun)
        {
            return;
        }

        List<Piece> activePiecesOnBoard = new List<Piece>(gameBoard.GetActivePiecesOnBoard()); // Copy because list gets sorted
        if (IsResolved())
        {
            shouldRun = false;
            foreach (Piece activePiece in activePiecesOnBoard)
            {
                // this forces all states to end (move will finish)
                activePiece.TransitIntoState(gameBoard, new InfiniteState());
            }
            phaseManager.SimulationEnded(player, activePiecesOnBoard);
            gameBoard.ClearInteractionsToProcess();
            return;
        }

        List<Piece> piecesOnBoard = gameBoard.GetPiecesOnBoard();
        foreach (Piece currentPiece in piecesOnBoard)
        {
            currentPiece.ProcessState(gameBoard, tick);
        }

        Queue<Interaction> interactionsToProcess = gameBoard.GetInteractionsToProcess();
        int numInteractionsToProcess = interactionsToProcess.Count;
        for (int i = 0; i < numInteractionsToProcess; i++)
        {
            Interaction interaction = interactionsToProcess.Dequeue();
            if (interaction.ProcessInteraction()) {
                interactionsToProcess.Enqueue(interaction);
            }
        }

        foreach (Piece currentPiece in gameBoard.GetPiecesOnBoard())
        {
            if (currentPiece.IsDead())
            {
                gameBoard.DeactivatePieceOnBoard(currentPiece);
            }
        }
    }

    private bool IsResolved()
    {
        List<Piece> piecesOnBoard = gameBoard.GetActivePiecesOnBoard();
        int numEnemies = piecesOnBoard.Where(piece => piece.IsEnemy()).Count();
        int numFriends = piecesOnBoard.Where(piece => !piece.IsEnemy()).Count();
        if (numEnemies == 0 || numFriends == 0)
        {
            Debug.Log("Game has been resolved.");

            // Increase the rounds survived count for each piece.
            if (numFriends > 0)
            {
                foreach (Piece piece in piecesOnBoard.Where(piece => !piece.IsEnemy()))
                {
                    piece.SetRoundsSurvived(piece.GetRoundsSurvived() + 1);
                    // Pieces no longer upgrade in rarity and attributes.
                    // phaseManager.marketManager.characterGenerator.TryUpgradeCharacterRoundsSurvived(piece);
                }
                incomeManager.SetIncomeGeneratedByPlayer(player, bonusGold);
            }

            return true;
        }
        return false;
    }
}
