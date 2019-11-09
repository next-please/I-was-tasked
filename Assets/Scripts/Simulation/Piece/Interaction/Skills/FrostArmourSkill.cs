using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostArmourSkill : Interaction
{
    private Piece caster;
    private Board board;
    public double frostArmourDefaultArmourPercentage = GameLogicManager.Inst.Data.Skills.FrostArmourPercentage;
    public int ticksTilActivation = 0;

    public FrostArmourSkill(Piece caster, Board board)
    {
        this.caster = caster;
        this.board = board;
        this.ticksRemaining = ticksTilActivation;
        this.ticksTotal = 50;
    }

    public override bool ProcessInteraction()
    {
        if (ticksRemaining > 0)
        {
            ticksRemaining--;
            return true;
        }
        else
        {
            ApplyEffect();
            return false;
        }
    }

    public override void CleanUpInteraction()
    {
        if (interactionView != null)
        {
            interactionView.CleanUpInteraction();
        }
    }

    public override bool ProcessInteractionView()
    {
        return false;
    }

    private void ApplyEffect()
    {
        Piece target;
        List<Piece> tempList;
        if (!caster.IsEnemy())
        {
            tempList = board.GetActiveFriendliesOnBoard().FindAll(x => x.interactions.Find(y => y.identifier == Enums.Interaction.FrostArmourLingering) == null);
        }
        else
        {
            tempList = board.GetActiveEnemiesOnBoard().FindAll(x => x.interactions.Find(y => y.identifier == Enums.Interaction.FrostArmourLingering) == null);
        }

        if (tempList.Count < 1)
        {
            if (!caster.IsEnemy())
            {
                tempList = board.GetActiveFriendliesOnBoard();
            }
            else
            {
                tempList = board.GetActiveEnemiesOnBoard();
            }
        }

        if (caster.IsDead())
        {
            return;
        }

        tempList.Sort((x, y) => (int)(100 * ((double)x.GetCurrentHitPoints() / x.GetMaximumHitPoints() - (double)y.GetCurrentHitPoints() / y.GetMaximumHitPoints())));
        target = tempList[0];

        if (target.interactions.Find(x => x.identifier == Enums.Interaction.FrostArmourLingering) != null)
        {
            target.interactions.Find(x => x.identifier == Enums.Interaction.FrostArmourLingering).ticksRemaining = FrostArmourLingeringEffect.ticksTilActivation;
        }
        else
        {
            target.SetArmourPercentage(target.GetArmourPercentage() + frostArmourDefaultArmourPercentage);
            double armourChange = frostArmourDefaultArmourPercentage;

            Interaction skill = new FrostArmourLingeringEffect(target, armourChange);
            board.AddInteractionToProcess(skill);
            target.interactions.Add(skill);
        }

        Debug.Log(caster.GetName() + " has FrostArmour-ed " + target.GetName() + " to increase armour to " + target.GetArmourPercentage() + ".");
    }
}

public class FrostArmourLingeringEffect : Interaction
{
    private Piece target;
    private double armourChange;
    private Vector3 attackDestination;
    public static int ticksTilActivation = GameLogicManager.Inst.Data.Skills.FrostArmourLingerTicks;
    public int blockAmount;

    public FrostArmourLingeringEffect(Piece target, double armourChange)
    {
        this.identifier = Enums.Interaction.FrostArmourLingering;
        this.target = target;
        this.armourChange = armourChange;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.FrostArmour;
    }

    public override bool ProcessInteraction()
    {
        if (ticksRemaining > 0)
        {
            ticksRemaining--;
            return true;
        }
        else
        {
            ApplyEffect();
            return false;
        }
    }

    public override void CleanUpInteraction()
    {
        if (interactionView != null)
        {
            interactionView.CleanUpInteraction();
        }
    }

    public override bool ProcessInteractionView()
    {
        if (ticksRemaining <= 0 || target.IsDead())
        {
            return false;
        }
        GameObject projectile = interactionView.gameObject;
        Transform targetT = target.GetPieceView().transform;
        projectile.transform.parent =  targetT;
        Vector3 pos = Vector3.zero;
        pos.y = 1;
        projectile.transform.localPosition = pos;
        return true;
    }

    private void ApplyEffect()
    {
        if (target.IsDead())
        {
            return;
        }
        target.SetArmourPercentage(target.GetArmourPercentage() - armourChange);
        target.interactions.Remove(target.interactions.Find(x => x.identifier == Enums.Interaction.FrostArmourLingering));

        Debug.Log(target.GetName() + "'s Frost Armour has expired.");
    }

}