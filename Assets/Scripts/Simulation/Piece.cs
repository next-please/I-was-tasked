using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization;

[Serializable]
public class Piece : ISerializable
{
    #region Serializable Fields
    private string guid;
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
    private Tile initialTile;
    private Tile currentTile;
    private Tile lockedTile;
    #endregion

    private Piece target;
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
        this.guid = Guid.NewGuid().ToString();
    }

    public override bool Equals(object obj)
    {
        if ((obj == null) || ! this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            Piece p = (Piece) obj;
            return p.guid == this.guid;
        }
    }

    public override int GetHashCode()
    {
        return guid.GetHashCode();
    }

    public static bool operator ==(Piece a, Piece b)
    {
        if (object.ReferenceEquals(a, null))
        {
            return object.ReferenceEquals(b, null);
        }
        return a.Equals(b);
    }

    public static bool operator !=(Piece a, Piece b)
    {
        return !(a == b);
    }

    // The special constructor is used to deserialize values.
    public Piece(SerializationInfo info, StreamingContext context)
    {
        // In Order of Declaration
        guid = (string) info.GetValue("guid", typeof(string));
        name = (string) info.GetValue("name", typeof(string));
        race = (Enums.Race) info.GetValue("race", typeof(Enums.Race));
        job = (Enums.Job) info.GetValue("job", typeof(Enums.Job));
        currentHitPoints = (int) info.GetValue("currentHitPoints", typeof(int));
        maximumHitPoints = (int) info.GetValue("maximumHitPoints", typeof(int));
        currentManaPoints = (int) info.GetValue("currentManaPoints", typeof(int));
        maximumManaPoints = (int) info.GetValue("maximumManaPoints", typeof(int));
        attackDamage = (int) info.GetValue("attackDamage", typeof(int));
        attackRange = (int) info.GetValue("attackRange", typeof(int));
        attackSpeed = (int) info.GetValue("attackSpeed", typeof(int));
        movementSpeed = (int) info.GetValue("movementSpeed", typeof(int));
        isEnemy = (bool) info.GetValue("isEnemy", typeof(bool));
        rarity = (int) info.GetValue("rarity", typeof(int));
        damageIfSurvive = (int) info.GetValue("damageIfSurvive", typeof(int));
        initialTile = (Tile) info.GetValue("initialTile", typeof(Tile));
        currentTile = (Tile) info.GetValue("currentTile", typeof(Tile));
        lockedTile = (Tile) info.GetValue("lockedTile", typeof(Tile));
        this.state = CreateState();
        this.entryState = this.state;
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        // In Order of Declaration
        info.AddValue("guid", guid, typeof(string));
        info.AddValue("name", name, typeof(string));
        info.AddValue("race", race, typeof(Enums.Race));
        info.AddValue("job", job, typeof(Enums.Job));
        info.AddValue("currentHitPoints", currentHitPoints, typeof(int));
        info.AddValue("maximumHitPoints", maximumHitPoints, typeof(int));
        info.AddValue("currentManaPoints", currentManaPoints, typeof(int));
        info.AddValue("maximumManaPoints", maximumManaPoints, typeof(int));
        info.AddValue("attackDamage", attackDamage, typeof(int));
        info.AddValue("attackRange", attackRange, typeof(int));
        info.AddValue("attackSpeed", attackSpeed, typeof(int));
        info.AddValue("movementSpeed", movementSpeed, typeof(int));
        info.AddValue("isEnemy", isEnemy, typeof(bool));
        info.AddValue("rarity", rarity, typeof(int));
        info.AddValue("damageIfSurvive", damageIfSurvive, typeof(int));
        info.AddValue("initialTile", initialTile, typeof(Tile));
        info.AddValue("currentTile", currentTile, typeof(Tile));
        info.AddValue("lockedTile", lockedTile, typeof(Tile));
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
