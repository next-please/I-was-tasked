using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Simulator : Tickable
{
    public PhaseManager phaseManager;
    public bool shouldRun = false;

    private Player player;
    private Board gameBoard;

    bool IsResolved()
    {
        List<Piece> piecesOnBoard = gameBoard.GetActivePiecesOnBoard();
        int numEnemies = piecesOnBoard.Where(piece => piece.IsEnemy()).Count();
        int numFriends = piecesOnBoard.Where(piece => !piece.IsEnemy()).Count();
        if (numEnemies == 0 || numFriends == 0)
        {
            Debug.Log("Game has been resolved");
            return true;
        }
        return false;
    }

    public void SetGameBoard(Board board, Player player = Player.Zero)
    {
        gameBoard = board;
    }

    public void StartSim()
    {
        shouldRun = true;
    }

    public override void Tick(long tick)
    {
        if (!shouldRun)
        {
            return;
        }

        // copy because list gets sorted
        List<Piece> activePiecesOnBoard = new List<Piece>(gameBoard.GetActivePiecesOnBoard());
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
}
