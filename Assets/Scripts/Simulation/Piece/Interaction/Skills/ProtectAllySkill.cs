using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProtectAllySkill : Interaction
{
    private Piece caster;
    private Piece target;
    private Board board;
    public double shapeshiftDefaultMultiplierIncrease = 1.2;
    public int shapeshiftDefaultAttackSpeedIncrease = 1;
    public int ticksTilActivation = 0;

    public ProtectAllySkill(Piece caster, Board board)
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
        interactionView.CleanUpInteraction();
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
            tempList = board.GetActiveFriendliesOnBoard().FindAll(x => x != caster);
        }
        else
        {
            tempList = board.GetActiveEnemiesOnBoard().FindAll(x => x != caster);
        }

        if (caster.IsDead() || tempList.Count == 0)
        {
            return;
        }

        tempList.Sort((x, y) => (int)(100 * ((double)x.GetCurrentHitPoints() / x.GetMaximumHitPoints() - (double)y.GetCurrentHitPoints() / y.GetMaximumHitPoints())));
        target = tempList[0];
        
        target.SetLinkedProtectingPiece(ref caster);
        if (target.interactions.Find(x => x.identifier.Equals("ProtectAlly")) != null)
        {
            Interaction skill = target.interactions.Find(x => x.identifier.Equals("ProtectAlly"));
            skill.ticksRemaining = ticksTilActivation;
        }
        else
        {
            Interaction skill = new ProtectAllyLingeringEffect(target);
            board.AddInteractionToProcess(skill);
            target.interactions.Add(skill);
        }

        Debug.Log(caster.GetName() + " has ProtectAlly-ed " + target.GetName() + " to take damage from attacks instead of them.");

    }
}

public class ProtectAllyLingeringEffect : Interaction
{
    private Piece target;
    private Vector3 attackDestination;
    public int ticksTilActivation = GameLogicManager.Inst.Data.Skills.ProtectAllyLingerTicks;

    public ProtectAllyLingeringEffect(Piece target)
    {
        this.identifier = "ProtectAlly";
        this.target = target;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.ProtectAlly;
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
            Fizzle();
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
        PieceView pv = target.GetPieceView();
        projectile.transform.parent = pv.transform;
        Vector3 pos = pv.transform.position;
        pos.y = 1.8f;

        projectile.transform.position = pos;

        if (ticksRemaining <= 0)
        {
            return false;
        }
        return true;
    }

    private void Fizzle()
    {
        target.RemoveLinkedProtectingPiece();
        target.interactions.Remove(this);
    }

}
