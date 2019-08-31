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
            this.state = this.state.TransitNextState(this);
            // on start
            this.state.OnStart(this, board);
            while (this.state.hasFinished())
            {
                this.state.OnFinish(this, board);
                this.state = this.state.TransitNextState(this);
                this.state.OnStart(this, board);
            }
        }
    }

    public virtual State CreateState()
    {
        FindNewTargetState findTarget = new FindNewTargetState();
        MoveState move = new MoveState();
        AttackState attack = new AttackState();
        InfiniteState inf = new InfiniteState();

        WaitState wait = new WaitState(1);

        HasTarget hasTarget = new HasTarget();
        InRange canAttack = new InRange();
        WillBeInRange canAttackSoon = new WillBeInRange();

        Transition foundTarget = new Transition(hasTarget);
        Transition inRange = new Transition(canAttack);
        Transition stillHasTarget = new Transition(hasTarget);
        Transition willBeInRange = new Transition(canAttackSoon);

        findTarget.SetNextState(foundTarget);

        foundTarget.SetNextStates(
            inRange, // check if in range
            inf // do nothing
        );

        inRange.SetNextStates(
            attack, // attack
            willBeInRange // move to target
        );

        willBeInRange.SetNextStates(
            wait, // wait until in range
            move
        );

        wait.SetNextState(inRange);

        attack.SetNextState(stillHasTarget); // do we still have a target?
        move.SetNextState(findTarget); // find a new target

        stillHasTarget.SetNextStates(
            inRange, // check if still in range
            findTarget // find new target
        );

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
