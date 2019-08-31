using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece
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
    private int rarity;

    private State state;
    private State entryState;

    // Placeholder Constructor; actual Melee Piece would be more complex in attributes.
    public Piece(string name, int hitPoints, int attackDamage, int attackRange, bool isEnemy)
    {
        SetName(name);
        SetHitPoints(hitPoints);
        SetAttackDamage(attackDamage);
        SetAttackRange(attackRange);
        SetIsEnemy(isEnemy);
        SetMovementSpeed(1);
        this.state = CreateState();
        this.entryState = this.state;
    }

    public virtual void ProcessState(Board board, long tick)
    {
        if (IsDead()) return;
        ISimState simAction = this.state;
        simAction.OnTick(this, board);
        if (simAction.hasFinished())
        {
            simAction.OnFinish(this, board);
            // find new action
            this.state = this.state.TransitNextAction(this);
            // on start
            this.state.OnStart(this, board);
        }
    }

    public virtual State CreateState()
    {
        FindNewTargetState findTarget = new FindNewTargetState();
        MoveState move = new MoveState();
        AttackState attack = new AttackState();
        InfiniteState inf = new InfiniteState();

        findTarget.AddNextAction(attack); // after finding, we try to attack
        findTarget.AddNextAction(move); // if we cant attack, we try to move towards target
        findTarget.AddNextAction(inf); // we cant find anything

        attack.AddNextAction(attack); // attack same target
        // attack.AddNextAction(move); // uncomment to chase
        attack.AddNextAction(findTarget); // find new target

        // uncomment the top 2 for chasing behaviour
        // move.AddNextAction(attack); // attack same target
        // move.AddNextAction(move); // we may have to chase
        move.AddNextAction(findTarget); // find new target

        return findTarget; // our initial action is find
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
    public IViewState GetViewState()
    {
        return state;
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

    public void SetRarity(int rarity)
    {
        this.rarity = rarity;
    }

    public int GetRarity()
    {
        return rarity;
    }

    public void Reset()
    {
        SetHitPoints(100);
        this.state = entryState;
    }
}
