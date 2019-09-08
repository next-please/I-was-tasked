using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Piece
{
    private Piece target;
    private Tile initialTile;
    private Tile currentTile;
    private Tile lockedTile;
    private string name;
    private Enums.Race race;
    private Enums.Job job;
    private int defaultMaximumHitPoints;
    private int currentHitPoints;
    private int maximumHitPoints;
    private int currentManaPoints;
    private int defaultMaximumManaPoints;
    private int maximumManaPoints;
    private int defaultAttackDamage;
    private int attackDamage;
    private int attackRange;
    private int defaultAttackRange;
    private int attackSpeed;
    private int movementSpeed;
    private int defaultMovementSpeed;
    private bool isEnemy;
    private double lifestealPercentage = 0;
    private double defaultLifestealPercentage = 0;
    private double recoilPercentage = 0;
    private double defaultRecoilPercentage = 0;
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
        SetDefaultMaximumHitPoints(maximumHitPoints);
        SetCurrentManaPoints(0);
        SetAttackDamage(attackDamage);
        SetDefaultAttackDamage(attackDamage);
        SetAttackRange(attackRange);
        SetDefaultAttackRange(attackRange);
        SetMovementSpeed(1);
        SetDefaultMovementSpeed(1);
        SetIsEnemy(isEnemy);
        this.state = CreateState();
        this.entryState = this.state;
    }

    public Piece(string name, int maximumHitPoints, int attackDamage, int attackRange, int movementSpeed, bool isEnemy)
    {
        SetName(name);
        SetCurrentHitPoints(maximumHitPoints);
        SetMaximumHitPoints(maximumHitPoints);
        SetDefaultMaximumHitPoints(maximumHitPoints);
        SetCurrentManaPoints(0);
        SetAttackDamage(attackDamage);
        SetDefaultAttackDamage(attackDamage);
        SetAttackRange(attackRange);
        SetDefaultAttackRange(attackRange);
        SetMovementSpeed(movementSpeed);
        SetDefaultMovementSpeed(movementSpeed);
        SetIsEnemy(isEnemy);
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
            State nextState = this.state.TransitNextState(this);
            TransitIntoState(board, nextState);
            while (this.state.hasFinished())
            {
                nextState = this.state.TransitNextState(this);
                TransitIntoState(board, nextState);
            }
        }
    }

    public void TransitIntoState(Board board, State state)
    {
        this.state.OnFinish(this, board);
        this.state = state;
        this.state.OnStart(this, board);
    }

    public virtual State CreateState()
    {
        FindNewTargetState findTarget = new FindNewTargetState();
        MoveState move = new MoveState();
        AttackState attack = new AttackState();
        InfiniteState inf = new InfiniteState();

        WaitState waitOneTick = new WaitState(1);
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
            waitOneTick
        );

        waitOneTick.SetNextState(findTarget);
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

    public int GetDefaultMaximumHitPoints()
    {
        return defaultMaximumHitPoints;
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

    public double GetLifestealPercentage()
    {
        return lifestealPercentage;
    }

    public double GetRecoilPercentage()
    {
        return recoilPercentage;
    }

    public double GetDefaultRecoilPercentage()
    {
        return defaultRecoilPercentage;
    }

    public double GetDefaultLifestealPercentage()
    {
        return defaultLifestealPercentage;
    }

    public int GetDefaultAttackDamage()
    {
        return defaultAttackDamage;
    }

    public int GetAttackRange()
    {
        return attackRange;
    }

    public int GetDefaultAttackRange()
    {
        return defaultAttackRange;
    }

    public int GetAttackSpeed()
    {
        return attackSpeed;
    }

    public int GetMovementSpeed()
    {
        return movementSpeed;
    }

    public int GetDefaultMovementSpeed()
    {
        return defaultMovementSpeed;
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

    public void SetDefaultMaximumHitPoints(int defaultMaximumHitPoints)
    {
        this.defaultMaximumHitPoints = defaultMaximumHitPoints;
    }

    public void SetCurrentManaPoints(int currentManaPoints)
    {
        if (currentManaPoints < 0)
        {
            this.currentManaPoints = 0;
        }
        else if (currentManaPoints >= this.maximumManaPoints)
        {
            this.currentManaPoints = this.maximumManaPoints;
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

    public void SetLifestealPercentage(double lifestealPercentage)
    {
        this.lifestealPercentage = lifestealPercentage;
    }

    public void SetRecoilPercentage(double recoilPercentage)
    {
        this.recoilPercentage = recoilPercentage;
    }
    
    public void SetDefaultAttackDamage(int defaultAttackDamage)
    {
        this.defaultAttackDamage = defaultAttackDamage;
    }

    public void SetAttackRange(int attackRange)
    {
        this.attackRange = attackRange;
    }

    public void SetDefaultAttackRange(int defaultAttackRange)
    {
        this.defaultAttackRange = defaultAttackRange;
    }

    public void SetAttackSpeed(int attackSpeed)
    {
        this.attackSpeed = attackSpeed;
    }

    public void SetMovementSpeed(int movementSpeed)
    {
        this.movementSpeed = movementSpeed;
    }

    public void SetDefaultMovementSpeed(int defaultMovementSpeed)
    {
        this.defaultMovementSpeed = defaultMovementSpeed;
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
        SetMaximumHitPoints(GetDefaultMaximumHitPoints());
        SetCurrentHitPoints(GetMaximumHitPoints());
        SetAttackDamage(GetDefaultAttackDamage());
        SetLifestealPercentage(GetDefaultLifestealPercentage());
        SetRecoilPercentage(GetDefaultRecoilPercentage());
        SetAttackRange(GetDefaultAttackRange());
        SetMovementSpeed(GetDefaultMovementSpeed());
        SetCurrentManaPoints(0);
        this.state = entryState;
    }

    public int GetPrice()
    {
        return (int)Math.Pow(2, rarity-1);
    }
}
