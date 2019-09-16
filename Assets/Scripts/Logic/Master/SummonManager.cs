using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonManager : MonoBehaviour
{
    public BoardManager boardManager;
    public InventoryManager inventoryManager;

    EnemyGenerator enemyGenerator = new EnemyGenerator();

    public void SummonEnemies(List<List<Piece>> enemies)
    {
        int i = 0;
        foreach (List<Piece> enemyPieces in enemies)
        {
            Player player = (Player) i;
            for (int _i = 0; _i < enemyPieces.Count; _i++)
            {
                boardManager.AddPieceToBoard(player,(Piece)enemyPieces[_i], 7 - (_i / 8), 7 - (_i % 8));
            }
            i++;
        }
    }

    public List<List<Piece>> GenerateEnemies(int currentRound, int numPlayers = 1)
    {
        List<List<Piece>> enemies = new List<List<Piece>>();
        for (int i = 0; i < numPlayers; ++i)
        {
            Player player = (Player) i;
            List<Piece> enemyPieces = enemyGenerator.generateEnemies(currentRound);
            enemies.Add(enemyPieces);
        }
        return enemies;
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
                if (!inventoryManager.AddToBench(player, piece))
                {
                    inventoryManager.AddGold(player, piece.GetPrice());
                }
                inventoryManager.RemoveFromArmy(player, piece);
            }
        }
    }

    public void RemoveAllEnemyPieces(int numPlayers = 1)
    {
        for (int i = 0; i < numPlayers; ++i)
        {
            Player player = (Player) i;
            boardManager.RemoveAllEnemies(player);
        }
    }
}
