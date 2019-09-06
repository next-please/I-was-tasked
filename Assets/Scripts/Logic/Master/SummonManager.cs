using System.Collections;
using UnityEngine;

public class SummonManager : MonoBehaviour
{
    public int numPlayers = 1;
    public BoardManager boardManager;

    EnemyGenerator enemyGenerator = new EnemyGenerator();
    public void GenerateAndSummonEnemies(int currentRound)
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

}
