using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurseOfAgonySkill : Interaction
{
    private Piece caster;
    private Piece target;
    private Board board;
    public int curseOfAgonyDefaultCurseAmount = 20;
    public int ticksTilActivation = 0;

    public CurseOfAgonySkill(Piece caster, Piece target, Board board)
    {
        this.caster = caster;
        this.target = target;
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
        interactionView.CleanUpInteraction();
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
        target.SetCurseDamageAmount(target.GetCurseDamageAmount() + curseOfAgonyDefaultCurseAmount);
        int curseChange = curseOfAgonyDefaultCurseAmount;

        Interaction skill = new CurseOfAgonyLingeringEffect(target, curseChange);
        board.AddInteractionToProcess(skill);

        Debug.Log(caster.GetName() + " has CurseOfAgony-ed " + target.GetName() + " to selfharm " + target.GetCurseDamageAmount() + " damage on each attack.");
    }
}

public class CurseOfAgonyLingeringEffect : Interaction
{
    private Piece target;
    private double curseChange;
    private Vector3 attackDestination;
    public int ticksTilActivation = 250;

    public CurseOfAgonyLingeringEffect(Piece target, int curseChange)
    {
        this.target = target;
        this.curseChange = curseChange;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.CurseOfAgony;
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
        if (!target.IsDead())
        {
            attackDestination = ViewManager.CalculateTileWorldPosition(target.GetCurrentTile());
            attackDestination.y = 2f;
        }

        projectile.transform.position = attackDestination;

        if (ticksRemaining <= 0 || target.IsDead())
        {
            return false;
        }
        return true;
    }

    private void ApplyEffect()
    {
        if (target.IsDead())
        {
            return;
        }
        target.SetArmourPercentage(target.GetArmourPercentage() - curseChange);

        Debug.Log(target.GetName() + "'s Curse of Agony has expired.");
    }

}