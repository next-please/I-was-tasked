using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece
{
    private Piece target;
    private Tile initialTile;
    private Tile currentTile;
    private Tile lockedTile;
    private string name;
    private string race;
    private string job;
    private int hitPoints;
    private int manaPoints;
    private int attackDamage;
    private int attackRange;
    private int attackSpeed;
    private int movementSpeed;
    private bool isEnemy;

    protected Action action;
    public IViewAction GetViewAction()
    {
        return action;
    }

    public abstract void AttackTarget();

    public Piece FindNearestTarget(List<Piece> activePiecesOnBoard)
    {
        List<Piece> enemyPiecesOnBoard = new List<Piece>();

        // Get all enemy Pieces on the Board.
        foreach (Piece piece in activePiecesOnBoard)
        {
            if (piece.IsEnemy() != this.IsEnemy())
            {
                enemyPiecesOnBoard.Add(piece);
            }
        }

        // Determine the nearest enemy Piece.
        Piece nearestEnemyPiece = null;

        foreach (Piece enemyPiece in enemyPiecesOnBoard)
        {
            if (nearestEnemyPiece == null)
            {
                nearestEnemyPiece = enemyPiece;
                continue;
            }

            Tile nearestTile = nearestEnemyPiece.GetCurrentTile();
            Tile checkTile = enemyPiece.GetCurrentTile();
            if (currentTile.DistanceToTile(nearestTile) > currentTile.DistanceToTile(checkTile))
            {
                nearestEnemyPiece = enemyPiece;
            }
        }
        return nearestEnemyPiece;
    }

    public void ProcessAction(Board board, long tick)
    {
        if (IsDead()) return;
        ISimAction simAction = this.action;
        simAction.OnTick(this, board);
        if (simAction.hasFinished())
        {
            simAction.OnFinish(this, board);
            // find new action
            this.action = this.action.TransitNextAction(this);
            // on start
            this.action.OnStart(this, board);
        }
    }

    // For now this only checks whether the piece is within attacking range of the Target Piece.
    public bool CanAttackTarget()
    {
        Tile targetTile = target.GetCurrentTile();
        return (currentTile.DistanceToTile(targetTile) <= attackRange);
    }

    public Piece GetTarget()
    {
        return target;
    }

    public Tile GetInitialTile()
    {
        return initialTile;
    }

    public Tile GetCurrentTile()
    {
        return currentTile;
    }

    public Tile GetLockedTile()
    {
        return lockedTile;
    }

    public string GetName()
    {
        return name;
    }

    public string GetRace()
    {
        return race;
    }

    public string GetClass()
    {
        return job;
    }

    public int GetHitPoints()
    {
        return hitPoints;
    }

    public int GetManaPoints()
    {
        return manaPoints;
    }

    public int GetAttackDamage()
    {
        return attackDamage;
    }

    public int GetAttackRange()
    {
        return attackRange;
    }

    public int GetAttackSpeed()
    {
        return attackSpeed;
    }

    public int GetMovementSpeed()
    {
        return movementSpeed;
    }

    public bool HasLockedTile()
    {
        return (lockedTile != null);
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public bool IsDead()
    {
        return (hitPoints <= 0);
    }

    public bool IsOnBoard()
    {
        return (currentTile != null);
    }

    public void SetTarget(Piece piece)
    {
        target = piece;
    }

    public void SetInitialTile(Tile initialTile)
    {
        this.initialTile = initialTile;
    }

    public void SetCurrentTile(Tile currentTile)
    {
        this.currentTile = currentTile;
    }

    public void SetLockedTile(Tile lockedTile)
    {
        this.lockedTile = lockedTile;
    }

    public void SetName(string name)
    {
        this.name = name;
    }

    public void SetRace(string race)
    {
        this.race = race;
    }

    public void SetClass(string job)
    {
        this.job = job;
    }

    public void SetHitPoints(int hitPoints)
    {
        if (hitPoints < 0)
        {
            this.hitPoints = 0;
        }
        else
        {
            this.hitPoints = hitPoints;
        }
    }

    public void SetManaPoints(int manaPoints)
    {
        this.manaPoints = manaPoints;
    }

    public void SetAttackDamage(int attackDamage)
    {
        this.attackDamage = attackDamage;
    }

    public void SetAttackRange(int attackRange)
    {
        this.attackRange = attackRange;
    }

    public void SetAttackSpeed(int attackSpeed)
    {
        this.attackSpeed = attackSpeed;
    }

    public void SetMovementSpeed(int movementSpeed)
    {
        this.movementSpeed = movementSpeed;
    }

    public void SetIsEnemy(bool isEnemy)
    {
        this.isEnemy = isEnemy;
    }
}
