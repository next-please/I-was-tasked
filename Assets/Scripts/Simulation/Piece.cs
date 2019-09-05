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
    private Enums.Race race;
    private Enums.Job job;
    private int currentHitPoints;
    private int maximumHitPoints;
    private int currentManaPoints;
    private int maximumManaPoints;
    private int attackDamage;
    private int attackRange;
    private int attackSpeed;
    private int movementSpeed;
    private bool isEnemy;
    private int rarity;
    private int damageIfSurvive = 0;
    private State state;
    private State entryState;

    // Placeholder Constructor; actual Melee Piece would be more complex in attributes.
    public Piece(string name, int maximumHitPoints, int attackDamage, int attackRange, bool isEnemy)
    {
        SetName(name);
        SetCurrentHitPoints(maximumHitPoints);
        SetMaximumHitPoints(maximumHitPoints);
        SetCurrentManaPoints(0);
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

        WaitState waitOneSecond = new WaitState(50);
        WaitState waitOneFifthSeconds = new WaitState(10);

        HasTarget hasTarget = new HasTarget();
        InRange inRange = new InRange();
        WillBeInRange willBeInRange = new WillBeInRange();
        CanFindNextTile canFindNextTile = new CanFindNextTile();

        Transition wasTargetFound = new Transition(hasTarget);
        Transition currentlyInRange = new Transition(inRange);
        Transition willTargetBeInRange = new Transition(willBeInRange);
        Transition stillHasTarget = new Transition(hasTarget);
        Transition tryToFindNextTile = new Transition(canFindNextTile);

        findTarget.SetNextState(wasTargetFound);

        wasTargetFound.SetNextStates(
            currentlyInRange, // check if in range
            inf // do nothing
        );

        currentlyInRange.SetNextStates(
            attack, // attack
            willTargetBeInRange // move to target
        );

        willTargetBeInRange.SetNextStates(
            waitOneFifthSeconds, // wait until in range
            tryToFindNextTile
        );

        tryToFindNextTile.SetNextStates(
            move,
            waitOneSecond
        );

        waitOneSecond.SetNextState(findTarget);
        waitOneFifthSeconds.SetNextState(currentlyInRange);

        attack.SetNextState(stillHasTarget); // do we still have a target?
        move.SetNextState(findTarget); // find a new target

        stillHasTarget.SetNextStates(
            currentlyInRange, // check if still in range
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

    public Enums.Race GetRace()
    {
        return race;
    }

    public Enums.Job GetClass()
    {
        return job;
    }

    public int GetMaximumHitPoints()
    {
        return maximumHitPoints;
    }

    public int GetCurrentHitPoints()
    {
        return currentHitPoints;
    }

    public int GetCurrentManaPoints()
    {
        return currentManaPoints;
    }

    public int GetMaximumManaPoints()
    {
        return maximumManaPoints;
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
        return (currentHitPoints <= 0);
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

    public void SetRace(Enums.Race race)
    {
        this.race = race;
    }

    public void SetClass(Enums.Job job)
    {
        this.job = job;
    }

    public void SetCurrentHitPoints(int currentHitPoints)
    {
        if (currentHitPoints < 0)
        {
            this.currentHitPoints = 0;
        }
        else
        {
            this.currentHitPoints = currentHitPoints;
        }
    }

    public void SetMaximumHitPoints(int maximumHitPoints)
    {
        this.maximumHitPoints = maximumHitPoints;
    }

    public void SetCurrentManaPoints(int currentManaPoints)
    {
        if (currentManaPoints < 0)
        {
            this.currentManaPoints = 0;
        }
        else
        {
            this.currentManaPoints = currentManaPoints;
        }
    }

    public void SetMaximumManaPoints(int maximumManaPoints)
    {
        this.maximumManaPoints = maximumManaPoints;
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

    public void SetDamageIfSurvive(int damageIfSurvive)
    {
        this.damageIfSurvive = damageIfSurvive;
    }

    public int GetDamageIfSurvive()
    {
        return damageIfSurvive;
    }

    public void Reset()
    {
        SetCurrentHitPoints(GetMaximumHitPoints());
        SetCurrentManaPoints(0);
        this.state = entryState;
    }
}
