using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CurseOfAgonySkill : Interaction
{
    private Piece caster;
    private Piece target;
    private Board board;
    public int curseOfAgonyDefaultCurseAmount = GameLogicManager.Inst.Data.Skills.CurseOfAgonyCurseAmount;
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

        if (caster.interactions.Find(x => x.identifier == Enums.Interaction.CurseOfAgonyLingering) != null)
        {
            CurseOfAgonyLingeringEffect skill = (CurseOfAgonyLingeringEffect)caster.interactions.Find(x => x.identifier == Enums.Interaction.CurseOfAgonyLingering);
            skill.ticksRemaining = CurseOfAgonyLingeringEffect.ticksTilActivation;
            int curseChange = curseOfAgonyDefaultCurseAmount;
            curseChange = (int)Math.Floor(curseChange * Math.Pow(GameLogicManager.Inst.Data.Skills.CurseOfAgonyRarityMultiplier, caster.GetRarity()));
            if (curseChange > skill.curseChange)
            {
                target.SetCurseDamageAmount(target.GetCurseDamageAmount() + (curseChange - skill.curseChange));
                skill.curseChange = curseChange;
            }
        }
        else
        {
            int curseChange = curseOfAgonyDefaultCurseAmount;
            curseChange = (int)Math.Floor(curseChange * Math.Pow(GameLogicManager.Inst.Data.Skills.CurseOfAgonyRarityMultiplier, caster.GetRarity()));
            target.SetCurseDamageAmount(target.GetCurseDamageAmount() + curseChange);

            Interaction skill = new CurseOfAgonyLingeringEffect(target, curseChange);
            board.AddInteractionToProcess(skill);
            target.interactions.Add(skill);
        }

        Debug.Log(caster.GetName() + " has CurseOfAgony-ed " + target.GetName() + " to selfharm " + target.GetCurseDamageAmount() + " damage on each attack.");
    }
}

public class CurseOfAgonyLingeringEffect : Interaction
{
    private Piece target;
    public int curseChange;
    public static int ticksTilActivation = GameLogicManager.Inst.Data.Skills.CurseOfAgonyLingerTicks;

    public CurseOfAgonyLingeringEffect(Piece target, int curseChange)
    {
        this.identifier = Enums.Interaction.CurseOfAgonyLingering;
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
            Transform targetT = target.GetPieceView().transform;
            projectile.transform.parent =  targetT;
            Vector3 pos = Vector3.zero;
            pos.y = 2;
            projectile.transform.localPosition = pos;
        }

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
        target.interactions.Remove(target.interactions.Find(x => x.identifier == Enums.Interaction.CurseOfAgonyLingering));

        Debug.Log(target.GetName() + "'s Curse of Agony has expired.");
    }

}