using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DecayingSynergyEffect : Interaction
{
    private Piece caster;
    public int ticksTilActivation = 0;

    public DecayingSynergyEffect(Piece caster)
    {
        this.caster = caster;
        this.ticksRemaining = ticksTilActivation;
        this.ticksTotal = 50;
    }

    public override bool ProcessInteraction()
    {
        if (!caster.IsDead())
        {
            caster.SetCurrentHitPoints(caster.GetCurrentHitPoints() - 1);
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void CleanUpInteraction()
    {
        //interactionView.CleanUpInteraction();
    }

    public override bool ProcessInteractionView()
    {
        return false;
    }

}
