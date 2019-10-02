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
        interactionPrefab = Enums.InteractionPrefab.CylinderTestSicklyGreen;

        attackSource = ViewManager.CalculateTileWorldPosition(caster.GetCurrentTile());
        attackSource.y = 0.5f;
    }

    public UnholyAuraSkill(Piece caster, Board board, int countRemaining)
    {
        this.caster = caster;
        this.countRemaining = countRemaining;
        this.ticksTotal = 50;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.CylinderTestSicklyGreen;

        attackSource = ViewManager.CalculateTileWorldPosition(caster.GetCurrentTile());
        attackSource.y = 0.5f;
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

        // Projectile chases the Target. If the Target is dead, the Projectile will go to the Tile the Target was previously on.
        projectile.transform.position = attackSource;

        if (ticksRemaining <= 0 || caster.IsDead())
        {
            return false;
        }
        return true;
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

        if (countRemaining > 0)
        {
            if (caster.GetState().GetType() != typeof(InfiniteState) && board.GetActiveEnemiesOnBoard().Count > 0)
            {
                Interaction skill = new UnholyAuraSkill(caster, board, countRemaining - 1);
                board.AddInteractionToProcess(skill);
            }
        }

        Debug.Log(caster.GetName() + " has UnholyAura-ed targets around it for " + unholyAuraDefaultDamage + " DMG.");
    }
}
