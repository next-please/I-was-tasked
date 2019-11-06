using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThunderstormSkill : Interaction
{
    private Piece caster;
    private Tile targetSpace;
    private Board board;
    private Vector3 attackSource;
    private int initialStartUp = GameLogicManager.Inst.Data.Skills.ThunderStormInitialTick;
    private int ticksTilActivation = GameLogicManager.Inst.Data.Skills.ThunderStormSubsequentTick;
    private int countRemaining;
    public int thunderStormDefaultRadius = GameLogicManager.Inst.Data.Skills.ThunderStormRadius;
    public int thunderStormDefaultCount = GameLogicManager.Inst.Data.Skills.ThunderStormCount;

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
            Interaction skill = new ThunderstormBoltEffect(target, caster);
            board.AddInteractionToProcess(skill);
        }
    }
}

public class ThunderstormBoltEffect : Interaction
{
    private Piece target;
    private Piece caster;
    private Vector3 attackSource;
    private int ticksTilActivation = 25;
    public int thunderStormDefaultDamage = GameLogicManager.Inst.Data.Skills.ThunderStormBoltDamage;

    public ThunderstormBoltEffect(Piece target, Piece caster)
    {
        this.target = target;
        this.caster = caster;
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
            if (!caster.IsDead())
            {
                caster.GetPieceView().pieceSounds.PlaySkillSubCastSound();
            }
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

        int damage = (int)Math.Floor(thunderStormDefaultDamage * Math.Pow(GameLogicManager.Inst.Data.Skills.ThunderStormRarityMultiplier, caster.GetRarity()));
        if (!target.invulnerable)
        {
            target.SetCurrentHitPoints(target.GetCurrentHitPoints() - damage);
        }

        Debug.Log(target.GetName() + " has been ThunderStorm-ed for " + damage + " DMG.");
    }
}

