using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderstormSkill : Interaction
{
    private Piece caster;
    private Tile targetSpace;
    private Board board;
    private Vector3 attackSource;
    private int initialStartUp = 25;
    private int ticksTilActivation = 50;
    private int countRemaining;
    public int thunderStormDefaultRadius = 1;
    public int thunderStormDefaultCount = 5;

    public ThunderstormSkill(Piece caster, Piece target, Board board)
    {
        this.caster = caster;
        this.board = board;
        this.targetSpace = target.GetCurrentTile();
        this.countRemaining = thunderStormDefaultCount;
        this.ticksTotal = 50;
        this.ticksRemaining = initialStartUp;
        interactionPrefab = Enums.InteractionPrefab.ThunderStorm;

        attackSource = ViewManager.CalculateTileWorldPosition(target.GetCurrentTile());
        attackSource.y = 5.0f;
    }

    public override bool ProcessInteraction()
    {
        if (ticksRemaining > 0)
        {
            ticksRemaining--;
            if (ticksRemaining == 0)
            {
                ApplyDamageToInflict();
                if (countRemaining > 0 && board.GetActiveEnemiesOnBoard().Count > 0)
                {
                    countRemaining--;
                    ticksRemaining = ticksTilActivation;
                }
            }
            return true;
        }
        else
        {
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
        List<Piece> enemiesWithinRange = board.GetActiveEnemiesWithinRadiusOfTile(targetSpace, thunderStormDefaultRadius);
        int numEnemiesWithinRange = enemiesWithinRange.Count;
        if (numEnemiesWithinRange > 0)
        {
            Piece target = enemiesWithinRange[board.GetRNGesus().Next(0, numEnemiesWithinRange)];
            Interaction skill = new ThunderstormBoltEffect(target);
            board.AddInteractionToProcess(skill);
        }
    }
}

public class ThunderstormBoltEffect : Interaction
{
    private Piece target;
    private Vector3 attackSource;
    private int ticksTilActivation = 25;
    public int thunderStormDefaultDamage = 28;

    public ThunderstormBoltEffect(Piece target)
    {
        this.target = target;
        this.ticksTotal = 50;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.ThunderStormLightning;
        attackSource = ViewManager.CalculateTileWorldPosition(target.GetCurrentTile());
        attackSource.y = 5.0f;
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

