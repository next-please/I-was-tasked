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

    private State state;
    private State entryState;

    private string name;
    private Enums.Race race;
    private Enums.Job job;
    private int rarity;
    private bool isEnemy;

    private int defaultMaximumHitPoints;
    private int currentHitPoints;
    private int maximumHitPoints;

    private int currentManaPoints;
    private int maximumManaPoints;
    private int manaPointsGainedOnAttack;
    private int manaPointsGainedOnDamaged;

    private int defaultAttackDamage;
    private int attackDamage;

    private int defaultAttackRange;
    private int attackRange;

    private int defaultAttackSpeed;
    private int attackSpeed;

    private int defaultMovementSpeed;
    private int movementSpeed;

    private double defaultLifestealPercentage;
    private double lifestealPercentage;

    private double defaultRecoilPercentage;
    private double recoilPercentage;

    private int damageIfSurvive;

    // Constructor for All Pieces; remember to set isEnemy accordingly.
    public Piece(string name, Enums.Race race, Enums.Job job, int rarity, bool isEnemy,
                 int defaultMaximumHitPoints, int maximumManaPoints,
                 int defaultAttackDamage, int defaultAttackRange,
                 int defaultAttackSpeed, int defaultMovementSpeed)
    {
        SetName(name);
        SetRace(race);
        SetClass(job);
        SetRarity(rarity);
        SetIsEnemy(isEnemy);

        SetDefaultMaximumHitPoints(defaultMaximumHitPoints);
        SetCurrentHitPoints(defaultMaximumHitPoints);
        SetMaximumHitPoints(defaultMaximumHitPoints);

        SetMaximumManaPoints(maximumManaPoints);
        SetCurrentManaPoints(0);

        // Placeholder Mana Gain Values.
        if (!isEnemy)
        {
            SetManaPointsGainedOnAttack(20);
            SetManaPointsGainedOnDamaged(20);
        }
        else
        {
            SetManaPointsGainedOnAttack(10);
            SetManaPointsGainedOnDamaged(10);
        }

        SetDefaultAttackDamage(defaultAttackDamage);
        SetAttackDamage(defaultAttackDamage);

        SetDefaultAttackRange(defaultAttackRange);
        SetAttackRange(defaultAttackRange);

        SetDefaultAttackSpeed(defaultAttackSpeed);
        SetAttackSpeed(defaultAttackSpeed);

        SetDefaultMovementSpeed(defaultMovementSpeed);
        SetMovementSpeed(defaultMovementSpeed);

        SetDefaultLifestealPercentage(0);
        SetLifestealPercentage(0);

        SetDefaultRecoilPercentage(0);
        SetRecoilPercentage(0);

        SetDamageIfSurvive(0);

        this.state = CreateState();
        this.entryState = this.state;
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

    public void Reset()
    {
        SetMaximumHitPoints(GetDefaultMaximumHitPoints());
        SetCurrentHitPoints(GetMaximumHitPoints());
        SetCurrentManaPoints(0);
        SetAttackDamage(GetDefaultAttackDamage());
        SetAttackRange(GetDefaultAttackRange());
        SetMovementSpeed(GetDefaultMovementSpeed());

        SetLifestealPercentage(GetDefaultLifestealPercentage());
        SetRecoilPercentage(GetDefaultRecoilPercentage());

        this.state = entryState;
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

    public int GetRarity()
    {
        return rarity;
    }

    public int GetDefaultMaximumHitPoints()
    {
        return defaultMaximumHitPoints;
    }

    public int GetCurrentHitPoints()
    {
        return currentHitPoints;
    }

    public int GetMaximumHitPoints()
    {
        return maximumHitPoints;
    }

    public int GetCurrentManaPoints()
    {
        return currentManaPoints;
    }

    public int GetMaximumManaPoints()
    {
        return maximumManaPoints;
    }

    public int GetManaPointsGainedOnAttack()
    {
        return manaPointsGainedOnAttack;
    }

    public int GetManaPointsGainedOnDamaged()
    {
        return manaPointsGainedOnDamaged;
    }

    public int GetDefaultAttackDamage()
    {
        return defaultAttackDamage;
    }

    public int GetAttackDamage()
    {
        return attackDamage;
    }

    public int GetDefaultAttackRange()
    {
        return defaultAttackRange;
    }

    public int GetAttackRange()
    {
        return attackRange;
    }

    public int GetDefaultAttackSpeed()
    {
        return defaultAttackSpeed;
    }

    public int GetAttackSpeed()
    {
        return attackSpeed;
    }

    public int GetDefaultMovementSpeed()
    {
        return defaultMovementSpeed;
    }

    public int GetMovementSpeed()
    {
        return movementSpeed;
    }

    public double GetDefaultLifestealPercentage()
    {
        return defaultLifestealPercentage;
    }

    public double GetLifestealPercentage()
    {
        return lifestealPercentage;
    }

    public double GetDefaultRecoilPercentage()
    {
        return defaultRecoilPercentage;
    }

    public double GetRecoilPercentage()
    {
        return recoilPercentage;
    }

    public int GetDamageIfSurvive()
    {
        return damageIfSurvive;
    }

    public int GetPrice()
    {
        return (int)Math.Pow(2, rarity - 1);
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

    public void SetRarity(int rarity)
    {
        this.rarity = rarity;
    }

    public void SetIsEnemy(bool isEnemy)
    {
        this.isEnemy = isEnemy;
    }

    public void SetDefaultMaximumHitPoints(int defaultMaximumHitPoints)
    {
        this.defaultMaximumHitPoints = defaultMaximumHitPoints;
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

    public void SetManaPointsGainedOnAttack(int manaPointsGainedOnAttack)
    {
        this.manaPointsGainedOnAttack = manaPointsGainedOnAttack;
    }

    public void SetManaPointsGainedOnDamaged(int manaPointsGainedOnDamage)
    {
        this.manaPointsGainedOnDamaged = manaPointsGainedOnDamage;
    }

    public void SetDefaultAttackDamage(int defaultAttackDamage)
    {
        this.defaultAttackDamage = defaultAttackDamage;
    }

    public void SetAttackDamage(int attackDamage)
    {
        this.attackDamage = attackDamage;
    }

    public void SetDefaultAttackRange(int defaultAttackRange)
    {
        this.defaultAttackRange = defaultAttackRange;
    }

    public void SetAttackRange(int attackRange)
    {
        this.attackRange = attackRange;
    }

    public void SetDefaultAttackSpeed(int defaultAttackSpeed)
    {
        this.defaultAttackSpeed = defaultAttackSpeed;
    }

    public void SetAttackSpeed(int attackSpeed)
    {
        this.attackSpeed = attackSpeed;
    }

    public void SetDefaultMovementSpeed(int defaultMovementSpeed)
    {
        this.defaultMovementSpeed = defaultMovementSpeed;
    }

    public void SetMovementSpeed(int movementSpeed)
    {
        this.movementSpeed = movementSpeed;
    }

    public void SetDefaultLifestealPercentage(double defaultLifestealPercentage)
    {
        this.defaultLifestealPercentage = defaultLifestealPercentage;
    }

    public void SetLifestealPercentage(double lifestealPercentage)
    {
        this.lifestealPercentage = lifestealPercentage;
    }

    public void SetDefaultRecoilPercentage(double defaultRecoilPercentage)
    {
        this.defaultRecoilPercentage = defaultRecoilPercentage;
    }

    public void SetRecoilPercentage(double recoilPercentage)
    {
        this.recoilPercentage = recoilPercentage;
    }

    public void SetDamageIfSurvive(int damageIfSurvive)
    {
        this.damageIfSurvive = damageIfSurvive;
    }
}
