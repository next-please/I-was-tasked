using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator
{
    public readonly int defaultStartingPieces = 1;
    public readonly int defaultHitPoints = 50;
    public readonly int defaultManaPoints = 100;
    public readonly int defaultAttackDamage = 5;
    public readonly int defaultAttackRange = 1;
    public readonly int defaultAttackSpeed = 5;
    public readonly int minimumAttackSpeed = 1;
    public readonly int maximumAttackSpeed = 10;
    public readonly int defaultMovementSpeed = 5;
    public readonly int minimumMovementSpeed = 1;
    public readonly int maximumMovementSpeed = 10;
    public readonly int upgradePowerModifier = 2;

    private int currentPieces;
    private int currentHitPoints;
    private int currentManaPoints;
    private int currentAttackDamage;
    private int currentAttackRange;
    private int currentAttackSpeed;
    private int currentMovementSpeed;
    private System.Random rngesus;

    // Constructor
    public EnemyGenerator()
    {
        rngesus = new System.Random();

        currentPieces = defaultStartingPieces;
        currentHitPoints = defaultHitPoints;
        currentManaPoints = defaultManaPoints;
        currentAttackDamage = defaultAttackDamage;
        currentAttackRange = defaultAttackRange;
        currentAttackSpeed = defaultAttackSpeed;
        currentMovementSpeed = defaultMovementSpeed;
    }

    public ArrayList generateEnemies(int roundNumber)
    {
        resetEnemyStats();

        ArrayList enemyPieces = new ArrayList();

        int randomValue;
        for (int i=1; i<roundNumber; i++)
        {
            randomValue = rngesus.Next(1, 4);
            switch (randomValue)
            {
                case 1:
                    currentHitPoints *= upgradePowerModifier;
                    break;
                case 2:
                    currentAttackDamage *= upgradePowerModifier;
                    break;
                case 3:
                    currentPieces *= upgradePowerModifier;
                    break;
                default:
                    break;
            }
        }
        for (int i=0; i<currentPieces; i++)
        {
            enemyPieces.Add(new Piece("Enemy #" + (i+1), currentHitPoints, currentAttackDamage, 1, true));
            ((Piece)enemyPieces[i]).SetMovementSpeed(defaultMovementSpeed);
        }

        return enemyPieces;
    }

    public void resetEnemyStats()
    {
        currentPieces = defaultStartingPieces;
        currentHitPoints = defaultHitPoints;
        currentManaPoints = defaultManaPoints;
        currentAttackDamage = defaultAttackDamage;
        currentAttackRange = defaultAttackRange;
        currentAttackSpeed = defaultAttackSpeed;
        currentMovementSpeed = defaultMovementSpeed;
    }
}
