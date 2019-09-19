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

    private Tile initialTile;
    private Tile currentTile;
    private Tile lockedTile;

    private string name;
    private string title;
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
    private int minimumAttackSpeed = 1;

    private int defaultMovementSpeed;
    private int movementSpeed;
    private int minimumMovementSpeed = 1;

    private double defaultLifestealPercentage;
    private double lifestealPercentage;

    private double defaultRecoilPercentage;
    private double recoilPercentage;

    private int defaultBlockAmount;
    private int blockAmount;

    private double defaultArmourPercentage;
    private double armourPercentage;

    private int defaultCurseDamageAmount;
    private int curseDamageAmount;

    private Piece defaultLinkedProtectingPiece;
    private Piece linkedProtectingPiece;

    private int damageIfSurvive;
#endregion

    private Piece target;
    private State state;
    private State entryState;

    // Constructor for All Pieces; remember to set isEnemy accordingly.
    public Piece(string name, string title, Enums.Race race, Enums.Job job, int rarity, bool isEnemy,
                 int defaultMaximumHitPoints, int maximumManaPoints,
                 int defaultAttackDamage, int defaultAttackRange,
                 int defaultAttackSpeed, int defaultMovementSpeed)
    {
        this.guid = Guid.NewGuid().ToString();

        SetName(name);
        SetRace(race);
        SetClass(job);
        SetTitle(title);
        SetRarity(rarity);
        SetIsEnemy(isEnemy);
        SetMovementSpeed(1);

        SetDefaultMaximumHitPoints(defaultMaximumHitPoints);
        SetMaximumHitPoints(defaultMaximumHitPoints);
        SetCurrentHitPoints(defaultMaximumHitPoints);

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

        SetDefaultBlockAmount(0);
        SetBlockAmount(0);

        SetDefaultArmourPercentage(0);
        SetArmourPercentage(0);

        SetDefaultCurseDamageAmount(0);
        SetCurseDamageAmount(0);

        SetDefaultLinkedProtectingPiece(null);
        SetLinkedProtectingPiece(null);

        SetDamageIfSurvive(0);

        this.state = CreateState();
        this.entryState = this.state;
    }
     // The special constructor is used to deserialize values.
    public Piece(SerializationInfo info, StreamingContext context)
    {
        // In Order of Declaration
        guid = (string) info.GetValue("guid", typeof(string));

        initialTile = (Tile) info.GetValue("initialTile", typeof(Tile));
        currentTile = (Tile) info.GetValue("currentTile", typeof(Tile));
        lockedTile = (Tile) info.GetValue("lockedTile", typeof(Tile));

        name = (string) info.GetValue("name", typeof(string));
        title = (string) info.GetValue("title", typeof(string));
        race = (Enums.Race) info.GetValue("race", typeof(Enums.Race));
        job = (Enums.Job) info.GetValue("job", typeof(Enums.Job));
        rarity = (int) info.GetValue("rarity", typeof(int));
        isEnemy = (bool) info.GetValue("isEnemy", typeof(bool));

        defaultMaximumHitPoints = (int) info.GetValue("defaultMaximumHitPoints", typeof(int));
        maximumHitPoints = (int) info.GetValue("maximumHitPoints", typeof(int));
        currentHitPoints = (int)info.GetValue("currentHitPoints", typeof(int));

        currentManaPoints = (int) info.GetValue("currentManaPoints", typeof(int));
        maximumManaPoints = (int) info.GetValue("maximumManaPoints", typeof(int));
        manaPointsGainedOnAttack = (int) info.GetValue("manaPointsGainedOnAttack", typeof(int));
        manaPointsGainedOnDamaged = (int) info.GetValue("manaPointsGainedOnDamaged", typeof(int));

        defaultAttackDamage = (int) info.GetValue("defaultAttackDamage", typeof(int));
        attackDamage = (int) info.GetValue("attackDamage", typeof(int));

        defaultAttackRange = (int) info.GetValue("defaultAttackRange", typeof(int));
        attackRange = (int) info.GetValue("attackRange", typeof(int));

        defaultAttackSpeed = (int) info.GetValue("defaultAttackSpeed", typeof(int));
        attackSpeed = (int) info.GetValue("attackSpeed", typeof(int));

        defaultMovementSpeed = (int) info.GetValue("defaultMovementSpeed", typeof(int));
        movementSpeed = (int) info.GetValue("movementSpeed", typeof(int));

        defaultLifestealPercentage = (double) info.GetValue("defaultLifestealPercentage", typeof(double));
        lifestealPercentage = (double) info.GetValue("lifestealPercentage", typeof(double));

        defaultRecoilPercentage = (double) info.GetValue("defaultRecoilPercentage", typeof(double));
        recoilPercentage = (double) info.GetValue("recoilPercentage", typeof(double));

        defaultBlockAmount = (int)info.GetValue("defaultBlockAmount", typeof(int));
        blockAmount = (int)info.GetValue("blockAmount", typeof(int));

        defaultArmourPercentage = (double)info.GetValue("defaultArmourPercentage", typeof(double));
        armourPercentage = (double)info.GetValue("armourPercentage", typeof(double));

        defaultCurseDamageAmount = (int)info.GetValue("defaultCurseDamageAmount", typeof(int));
        curseDamageAmount = (int)info.GetValue("curseDamageAmount", typeof(int));

        defaultLinkedProtectingPiece = null;
        linkedProtectingPiece = null;

        damageIfSurvive = (int) info.GetValue("damageIfSurvive", typeof(int));

        this.state = CreateState();
        this.entryState = this.state;
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        // In Order of Declaration
        info.AddValue("guid", guid, typeof(string));

        info.AddValue("initialTile", initialTile, typeof(Tile));
        info.AddValue("currentTile", currentTile, typeof(Tile));
        info.AddValue("lockedTile", lockedTile, typeof(Tile));

        info.AddValue("name", name, typeof(string));
        info.AddValue("title", title, typeof(string));
        info.AddValue("race", race, typeof(Enums.Race));
        info.AddValue("job", job, typeof(Enums.Job));
        info.AddValue("rarity", rarity, typeof(int));
        info.AddValue("isEnemy", isEnemy, typeof(bool));

        info.AddValue("defaultMaximumHitPoints", defaultMaximumHitPoints, typeof(int));
        info.AddValue("currentHitPoints", currentHitPoints, typeof(int));
        info.AddValue("maximumHitPoints", maximumHitPoints, typeof(int));

        info.AddValue("currentManaPoints", currentManaPoints, typeof(int));
        info.AddValue("maximumManaPoints", maximumManaPoints, typeof(int));
        info.AddValue("manaPointsGainedOnAttack", manaPointsGainedOnAttack, typeof(int));
        info.AddValue("manaPointsGainedOnDamaged", manaPointsGainedOnDamaged, typeof(int));

        info.AddValue("defaultAttackDamage", defaultAttackDamage, typeof(int));
        info.AddValue("attackDamage", attackDamage, typeof(int));

        info.AddValue("defaultAttackRange", defaultAttackRange, typeof(int));
        info.AddValue("attackRange", attackRange, typeof(int));

        info.AddValue("defaultAttackSpeed", defaultAttackSpeed, typeof(int));
        info.AddValue("attackSpeed", attackSpeed, typeof(int));

        info.AddValue("defaultMovementSpeed", defaultMovementSpeed, typeof(int));
        info.AddValue("movementSpeed", movementSpeed, typeof(int));

        info.AddValue("defaultLifestealPercentage", defaultLifestealPercentage, typeof(double));
        info.AddValue("lifestealPercentage", lifestealPercentage, typeof(double));

        info.AddValue("defaultRecoilPercentage", defaultRecoilPercentage, typeof(double));
        info.AddValue("recoilPercentage", recoilPercentage, typeof(double));

        info.AddValue("defaultBlockAmount", defaultBlockAmount, typeof(int));
        info.AddValue("blockAmount", blockAmount, typeof(int));

        info.AddValue("defaultArmourPercentage", defaultArmourPercentage, typeof(double));
        info.AddValue("armourPercentage", armourPercentage, typeof(double));

        info.AddValue("defaultCurseDamageAmount", defaultCurseDamageAmount, typeof(int));
        info.AddValue("curseDamageAmount", curseDamageAmount, typeof(int));

        info.AddValue("damageIfSurvive", damageIfSurvive, typeof(int));
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


    public virtual State CreateState()
    {
        FindNewTargetState findTarget = new FindNewTargetState();
        MoveState move = new MoveState();
        SkillState skill = new SkillState();
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

    public State GetState()
    {
        return state;
    }

    public virtual void ProcessState(Board board, long tick)
    {
        if (IsDead()) return;
        ISimState simAction = this.state;
        simAction.OnTick(this, board);
        if (simAction.hasFinished())
        {
            State nextState = this.state.TransitNextState(this);
            
            // Always checking if we can cast a skill.
            // To-do: A "Stunned" State.
            SkillState skill = new SkillState();
            HasFullMP hasFullMP = new HasFullMP();
            Transition canCastSkill = new Transition(hasFullMP);
            canCastSkill.SetNextStates(
                skill,
                nextState
            );

            skill.SetNextState(nextState);
            TransitIntoState(board, canCastSkill);
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
        SetBlockAmount(GetDefaultBlockAmount());
        SetArmourPercentage(GetDefaultArmourPercentage());
        SetCurseDamageAmount(GetDefaultCurseDamageAmount());
        SetLinkedProtectingPiece(GetDefaultLinkedProtectingPiece());

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

    public string GetTitle()
    {
        return title;
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

    public int GetDefaultBlockAmount()
    {
        return defaultBlockAmount;
    }

    public int GetBlockAmount()
    {
        return blockAmount;
    }

    public double GetDefaultArmourPercentage()
    {
        return defaultArmourPercentage;
    }

    public double GetArmourPercentage()
    {
        return armourPercentage;
    }

    public int GetDefaultCurseDamageAmount()
    {
        return defaultCurseDamageAmount;
    }

    public int GetCurseDamageAmount()
    {
        return curseDamageAmount;
    }

    public Piece GetDefaultLinkedProtectingPiece()
    {
        return defaultLinkedProtectingPiece;
    }

    public Piece GetLinkedProtectingPiece()
    {
        return linkedProtectingPiece;
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

    public void SetTitle(string title)
    {
        this.title = title;
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
            this.currentHitPoints = Math.Min(currentHitPoints, maximumHitPoints);
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
        this.defaultAttackSpeed = Math.Max(defaultAttackSpeed, minimumAttackSpeed);
    }

    public void SetAttackSpeed(int attackSpeed)
    {
        this.attackSpeed = Math.Max(attackSpeed, minimumAttackSpeed);
    }

    public void SetDefaultMovementSpeed(int defaultMovementSpeed)
    {
        this.defaultMovementSpeed = Math.Max(defaultMovementSpeed, minimumMovementSpeed);
    }

    public void SetMovementSpeed(int movementSpeed)
    {
        this.movementSpeed = Math.Max(movementSpeed, minimumMovementSpeed);
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

    public void SetDefaultBlockAmount(int defaultBlockAmount)
    {
        this.defaultBlockAmount = defaultBlockAmount;
    }

    public void SetBlockAmount(int blockAmount)
    {
        this.blockAmount = blockAmount;
    }

    public void SetDefaultArmourPercentage(double defaultArmourPercentage)
    {
        this.defaultArmourPercentage = defaultArmourPercentage;
    }

    public void SetArmourPercentage(double armourPercentage)
    {
        this.armourPercentage = armourPercentage;
    }

    public void SetDefaultCurseDamageAmount(int defaultCurseDamageAmount)
    {
        this.defaultCurseDamageAmount = defaultCurseDamageAmount;
    }

    public void SetCurseDamageAmount(int curseDamageAmount)
    {
        this.curseDamageAmount = curseDamageAmount;
    }

    public void SetDefaultLinkedProtectingPiece(Piece defaultLinkedProtectingPiece)
    {
        this.defaultLinkedProtectingPiece = defaultLinkedProtectingPiece;
    }

    public void SetLinkedProtectingPiece(Piece linkedProtectingPiece)
    {
        this.linkedProtectingPiece = linkedProtectingPiece;
    }

    public void SetDamageIfSurvive(int damageIfSurvive)
    {
        this.damageIfSurvive = damageIfSurvive;
    }
}
