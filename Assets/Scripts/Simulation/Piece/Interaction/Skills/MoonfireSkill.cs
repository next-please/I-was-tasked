using System.Collections;
using System.Collections.Generic;
using UnityEngine;using System;

public class MoonfireSkill : Interaction
{
    private Piece caster;
    private Piece target;
    private Board board;
    private Vector3 attackSource;
    private int ticksTilActivation = GameLogicManager.Inst.Data.Skills.MoonfireInitialTicks;
    public static double moonfireDefaultManaRetainPercentage = GameLogicManager.Inst.Data.Skills.MoonfireManaRetainPercentage;
    private int moonfireDefaultDamage = GameLogicManager.Inst.Data.Skills.MoonfireDefaultDamage;

    public MoonfireSkill(Piece caster, Piece target, Board board)
    {
        this.caster = caster;
        this.target = target;
        this.board = board;
        this.ticksTotal = 50;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.ProjectileTestLightBlue;

        attackSource = ViewManager.CalculateTileWorldPosition(target.GetCurrentTile());
        attackSource.y += 0.65f;
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
            ApplyDamageToInflict();
            return false;
        }
    }

    public override void CleanUpInteraction()
    {
        interactionView.CleanUpInteraction();
    }

    public override bool ProcessInteractionView()
    {
        GameObject projectile = interactionView.gameObject;
        projectile.transform.position = attackSource;
        return ticksRemaining > 0;
    }

    private void ApplyDamageToInflict()
    {
        if (target.IsDead())
        {
            return;
        }

        int damage = (int)Math.Floor(moonfireDefaultDamage * Math.Pow(GameLogicManager.Inst.Data.Skills.MoonfireRarityMultiplier, caster.GetRarity()));
        if (!target.invulnerable)
        {
            target.SetCurrentHitPoints(target.GetCurrentHitPoints() - damage);
        }

        Interaction skill = new MoonfireLingeringEffect(attackSource);
        board.AddInteractionToProcess(skill);

        Debug.Log(caster.GetName() + " has Moonfire-ed " + target.GetName() + " for " + damage + " DMG, whose HP has dropped to " + target.GetCurrentHitPoints() + " HP.");
    }
}
public class MoonfireLingeringEffect : Interaction
{
    private Vector3 effectPosition;
    public int ticksTilActivation = GameLogicManager.Inst.Data.Skills.MoonfireLingerTicks;

    public MoonfireLingeringEffect(Vector3 effectPosition)
    {
        this.effectPosition = effectPosition;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.Moonfire;
    }

    public override bool ProcessInteraction()
    {
        ticksRemaining--;
        return ticksRemaining >= 0;
    }

    public override void CleanUpInteraction()
    {
        interactionView.CleanUpInteraction();
    }

    public override bool ProcessInteractionView()
    {
        GameObject projectile = interactionView.gameObject;
        projectile.transform.position = effectPosition;
        return ticksRemaining > 0;
    }
}
