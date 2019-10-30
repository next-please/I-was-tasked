using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotSkill : Interaction
{
    private Piece caster;
    private Board board;
    private Vector3 attackSource;
    private int ticksTilActivation = GameLogicManager.Inst.Data.Skills.RotTickPerCount;
    public int countRemaining;
    public int rotDefaultRadius = GameLogicManager.Inst.Data.Skills.RotRadius;
    public static int rotDefaultCount = GameLogicManager.Inst.Data.Skills.RotCount;
    public int rotDefaultDamage = GameLogicManager.Inst.Data.Skills.RotDamage;

    public RotSkill(Piece caster, Board board)
    {
        if (caster.interactions.Find(x => x.identifier.Equals("Rot")) != null)
        {
            RotSkill skill = (RotSkill)caster.interactions.Find(x => x.identifier.Equals("Rot"));
            skill.countRemaining = RotSkill.rotDefaultCount;
        }
        else
        {
            this.identifier = "Rot";
            this.caster = caster;
            this.board = board;
            this.countRemaining = rotDefaultCount;
            this.ticksTotal = 50;
            this.ticksRemaining = ticksTilActivation;
            interactionPrefab = Enums.InteractionPrefab.Rot;

            attackSource = ViewManager.CalculateTileWorldPosition(caster.GetCurrentTile());
            attackSource.y = 0.5f;
            caster.interactions.Add(this);
        }
    }

    public RotSkill(Piece caster, Board board, int countRemaining)
    {
        
        this.caster = caster;
        this.board = board;
        this.countRemaining = countRemaining;
        this.ticksTotal = 50;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.Rot;

        attackSource = ViewManager.CalculateTileWorldPosition(caster.GetCurrentTile());
        attackSource.y = 0.5f;
    }

    public override bool ProcessInteraction()
    {
        ticksRemaining--;
        if (ticksRemaining == 0 && countRemaining > 0 && !caster.IsDead())
        {
            countRemaining--;
            ApplyDamageToInflict();
            if (caster.GetState().GetType() != typeof(InfiniteState) &&
                board.GetActiveEnemiesOnBoard().Count > 0)
            {
                ticksRemaining = ticksTilActivation;
            }
        }
        if (countRemaining < 0)
        {
            caster.interactions.Remove(caster.interactions.Find(x => x.identifier.Equals("Rot")));
        }
        return ticksRemaining >= 0 && !caster.IsDead();
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

        // Projectile chases the Target. If the Target is dead, the Projectile will go to the Tile the Target was previously on.

        if (!caster.IsDead())
        {
            attackSource = ViewManager.CalculateTileWorldPosition(caster.GetCurrentTile());
            attackSource.y = 0.5f;
        }
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

        if (!caster.IsEnemy())
        {
            foreach (Piece target in board.GetActiveEnemiesWithinRadiusOfTile(caster.GetCurrentTile(), rotDefaultRadius))
            {
                if (!target.invulnerable)
                {
                    target.SetCurrentHitPoints(target.GetCurrentHitPoints() - rotDefaultDamage);
                }
            }
        }
        else
        {
            foreach (Piece target in board.GetActiveFriendliesWithinRadiusOfTile(caster.GetCurrentTile(), rotDefaultRadius))
            {
                if (!target.invulnerable)
                {
                    target.SetCurrentHitPoints(target.GetCurrentHitPoints() - rotDefaultDamage);
                }
            }
        }

        Debug.Log(caster.GetName() + " has Rot-ed targets around it for " + rotDefaultDamage + " DMG.");
    }
}
