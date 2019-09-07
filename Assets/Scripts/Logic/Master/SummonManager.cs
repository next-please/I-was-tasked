using System.Collections;
using UnityEngine;

public class SummonManager : MonoBehaviour
{
    public BoardManager boardManager;
    public InventoryManager inventoryManager;

    EnemyGenerator enemyGenerator = new EnemyGenerator();
    public void GenerateAndSummonEnemies(int currentRound, int numPlayers = 1)
    {
        for (int i = 0; i < numPlayers; ++i)
        {
            Player player = (Player) i;
            boardManager.RemoveAllEnemies(player);
            ArrayList enemyPieces = enemyGenerator.generateEnemies(currentRound);
            for (int _i = 0; _i < enemyPieces.Count; _i++)
            {
                boardManager.AddPieceToBoard(player,(Piece)enemyPieces[_i], 7 - (_i / 8), 7 - (_i % 8));
            }
        }
    }

    public void RemoveExcessPlayerPieces(int numPlayers = 1)
    {
        for (int i = 0; i < numPlayers; ++i)
        {
            Player player = (Player) i;
            var excess = inventoryManager.GetExcessPieces(player);
            foreach (Piece piece in excess)
            {
                boardManager.RemovePieceFromBoard(player, piece);
                inventoryManager.AddToBench(player, piece);
                inventoryManager.RemoveFromArmy(player, piece);
            }
        }
    }
}
