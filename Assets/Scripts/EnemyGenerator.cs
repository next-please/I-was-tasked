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
    public readonly int upgradePowerModifier = 5;
    public readonly int upgradeHealthModifier = 50;
    public readonly int upgradePieceModifier = 1;
    public readonly int defaultDamage = 1;

    private int currentPieces;
    private int currentHitPoints;
    private int currentManaPoints;
    private int currentAttackDamage;
    private int currentAttackRange;
    private int currentAttackSpeed;
    private int currentMovementSpeed;
    private int currentDamage;
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
        currentDamage = defaultDamage;
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
                    currentHitPoints += upgradeHealthModifier;
                    break;
                case 2:
                    currentAttackDamage += upgradePowerModifier;
                    break;
                case 3:
                    currentPieces += upgradePieceModifier;
                    break;
                default:
                    break;
            }
        }
        for (int i=0; i<currentPieces; i++)
        {
            // To Lewis: May want to do some RNG team compositions for Race and Job of Enemies.
            // Current placeholder Race and Job is Undead-Knight.
            Piece enemy = new Piece("Enemy #" + (i + 1), Enums.Race.Undead, Enums.Job.Knight, -1, true,
                                     defaultHitPoints, defaultManaPoints,
                                     defaultAttackDamage, defaultAttackRange,
                                     defaultAttackSpeed, defaultMovementSpeed);
            enemy.SetDamageIfSurvive(defaultDamage);
            enemyPieces.Add(enemy);
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
        currentDamage = defaultDamage;
    }
}
