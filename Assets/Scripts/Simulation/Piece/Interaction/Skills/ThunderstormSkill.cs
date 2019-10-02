using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderstormSkill : Interaction
{
    private Piece caster;
    private Tile targetSpace;
    private Board board;
    private Vector3 attackSource;
    private int ticksTilActivation = 70;
    private int countRemaining;
    public int thunderStormDefaultRadius = 4;
    public int thunderStormDefaultCount = 3;

    public ThunderstormSkill(Piece caster, Piece target, Board board)
    {
        this.caster = caster;
        this.board = board;
        this.targetSpace = target.GetCurrentTile();
        this.countRemaining = thunderStormDefaultCount;
        this.ticksTotal = 50;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.CylinderTestLightBlue;

        attackSource = ViewManager.CalculateTileWorldPosition(target.GetCurrentTile());
        attackSource.y = 2.0f;
    }

    public ThunderstormSkill(Piece caster, Tile targetSpace, Board board, int countRemaining)
    {
        this.caster = caster;
        this.countRemaining = countRemaining;
        this.targetSpace = targetSpace;
        this.ticksTotal = 50;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.CylinderTestLightBlue;

        attackSource = ViewManager.CalculateTileWorldPosition(targetSpace);
        attackSource.y = 2.0f;
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

        if (ticksRemaining <= 0)
        {
            return false;
        }
        return true;
    }

    private void ApplyDamageToInflict()
    {
        List<Piece> enemiesWithinRange = board.GetActiveEnemiesWithinRadiusOfTile(caster.GetCurrentTile(), thunderStormDefaultRadius);
        int numEnemiesWithinRange = enemiesWithinRange.Count;
        if (numEnemiesWithinRange > 0)
        {
            Piece target = enemiesWithinRange[board.GetRNGesus().Next(0, numEnemiesWithinRange)];
            Interaction skill = new ThunderstormBoltEffect(target);
            board.AddInteractionToProcess(skill);
        }

        if (countRemaining > 0)
        {
            if (board.GetActiveEnemiesOnBoard().Count > 0)
            {
                Interaction skill = new ThunderstormSkill(caster, targetSpace, board, countRemaining - 1);
                board.AddInteractionToProcess(skill);
            }
        }
    }
}

public class ThunderstormBoltEffect : Interaction
{
    private Piece target;
    private Vector3 attackSource;
    private int ticksTilActivation = 9;
    public int thunderStormDefaultDamage = 60;

    public ThunderstormBoltEffect(Piece target)
    {
        this.target = target;
        this.ticksTotal = 50;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.ProjectileTestYellow;

        attackSource = ViewManager.CalculateTileWorldPosition(target.GetCurrentTile());
        attackSource.y = 1.0f;
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

        if (ticksRemaining <= 0)
        {
            return false;
        }
        return true;
    }

    private void ApplyDamageToInflict()
    {
        if (target.IsDead())
        {
            return;
        }
        target.SetCurrentHitPoints(target.GetCurrentHitPoints() - thunderStormDefaultDamage);

        Debug.Log(target.GetName() + " has been ThunderStorm-ed for " + thunderStormDefaultDamage + " DMG.");
    }
}

