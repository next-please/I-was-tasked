using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnholyAuraSkill : Interaction
{
    private Piece caster;
    private Board board;
    private Vector3 attackSource;
    private int ticksTilActivation = 10;
    private int countRemaining;
    public int unholyAuraDefaultRadius = 4;
    public int unholyAuraDefaultCount = 7;
    public int unholyAuraDefaultDamage = 8;

    public UnholyAuraSkill(Piece caster, Board board)
    {
        this.caster = caster;
        this.board = board;
        this.countRemaining = unholyAuraDefaultCount;
        this.ticksTotal = 50;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.UnholyAura;

        attackSource = ViewManager.CalculateTileWorldPosition(caster.GetCurrentTile());
        attackSource.y += 1f;
    }

    public UnholyAuraSkill(Piece caster, Board board, int countRemaining)
    {
        this.caster = caster;
        this.board = board;
        this.countRemaining = countRemaining;
        this.ticksTotal = 50;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.UnholyAura;

        attackSource = ViewManager.CalculateTileWorldPosition(caster.GetCurrentTile());
        attackSource.y += 1f;
    }

    public override bool ProcessInteraction()
    {
        ticksRemaining--;
        if (ticksRemaining == 0)
        {
            ApplyDamageToInflict();
            if (countRemaining > 0 &&
                caster.GetState().GetType() != typeof(InfiniteState) &&
                board.GetActiveEnemiesOnBoard().Count > 0)
            {
                countRemaining--;
                ticksRemaining = ticksTilActivation;
            }

        }
        return ticksRemaining > 0;
    }

    public override void CleanUpInteraction()
    {
        interactionView.CleanUpInteraction();
    }

    public override bool ProcessInteractionView()
    {
        GameObject projectile = interactionView.gameObject;
        projectile.transform.position = attackSource;
        return (ticksRemaining > 0 && !caster.IsDead());
    }

    private void ApplyDamageToInflict()
    {
        if (caster.IsDead())
        {
            return;
        }

        foreach (Piece target in board.GetActiveEnemiesWithinRadiusOfTile(caster.GetCurrentTile(), unholyAuraDefaultRadius))
        {
            target.SetCurrentHitPoints(target.GetCurrentHitPoints() - unholyAuraDefaultDamage);
        }

        Debug.Log(caster.GetName() + " has UnholyAura-ed targets around it for " + unholyAuraDefaultDamage + " DMG.");
    }
}
