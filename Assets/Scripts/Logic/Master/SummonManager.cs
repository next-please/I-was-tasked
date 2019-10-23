using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
                boardManager.AddPieceToBoard(player, enemyPieces[_i], enemyPieces[_i].startingSpot.Item1, enemyPieces[_i].startingSpot.Item2);
            }
            i++;
        }
    }

    public int GenerateRandomIndex(int currentRound)
    {
        return enemyGenerator.getWaveRandomIndex(currentRound);
    }

    public List<List<Piece>> GenerateEnemies(int currentRound, int index, int numPlayers = 1)
    {
        List<List<Piece>> enemies = new List<List<Piece>>();
        for (int i = 0; i < numPlayers; ++i)
        {
            Player player = (Player) i;
            List<Piece> enemyPieces = enemyGenerator.generateEnemies(currentRound, index);
            enemies.Add(enemyPieces);
        }
        return enemies;
    }

    public String GetWaveName(int currentRound, int index)
    {
        return enemyGenerator.generateWaveNames(currentRound, index);
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
