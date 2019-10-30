using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkskinSkill : Interaction
{
    private Piece caster;
    private Board board;
    public int barkskinDefaultBlockAmount = GameLogicManager.Inst.Data.Skills.BarkSkinBlockAmount;
    public int ticksTilActivation = 0;

    public BarkskinSkill(Piece caster, Board board)
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
        if (caster.IsDead())
        {
            return;
        }

        if (caster.interactions.Find(x => x.identifier.Equals("Barkskin")) != null)
        {
            caster.interactions.Find(x => x.identifier.Equals("Barkskin")).ticksRemaining = BarkskinLingeringEffect.ticksTilActivation;
        }
        else
        {
            caster.SetBlockAmount(caster.GetBlockAmount() + barkskinDefaultBlockAmount);

            Interaction skill = new BarkskinLingeringEffect(caster, barkskinDefaultBlockAmount);
            board.AddInteractionToProcess(skill);
            caster.interactions.Add(skill);
        }

        Debug.Log(caster.GetName() + " has Barkskin-ed " + caster.GetName() + " to increase block to " + caster.GetBlockAmount() + ".");
    }
}

public class BarkskinLingeringEffect : Interaction
{
    public Piece caster;
    private Vector3 attackDestination;
    public static int ticksTilActivation = GameLogicManager.Inst.Data.Skills.BarkSkinLingerTicks;
    public int blockAmount;

    public BarkskinLingeringEffect(Piece caster, int blockAmount)
    {
        this.identifier = "Barkskin";
        this.caster = caster;
        this.blockAmount = blockAmount;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.BarkSkin;
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
        GameObject projectile = interactionView.gameObject;

        if (!caster.IsDead())
        {
            attackDestination = ViewManager.CalculateTileWorldPosition(caster.GetCurrentTile());
            attackDestination.y += 3.5f;
        }

        projectile.transform.position = attackDestination;

        if (ticksRemaining <= 0)
        {
            return false;
        }
        return true;
    }

    private void ApplyEffect()
    {
        if (caster.IsDead())
        {
            return;
        }
        caster.SetBlockAmount(caster.GetBlockAmount() - blockAmount);
        caster.interactions.Remove(caster.interactions.Find(x => x.identifier.Equals("Barkskin")));

        Debug.Log(caster.GetName() + "'s Barkskin has expired, block decreases to " + caster.GetBlockAmount() + ".");
    }

}
