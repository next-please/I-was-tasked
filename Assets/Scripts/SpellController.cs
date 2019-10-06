using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    public readonly string[,] SpellNames = new string[,]
    {
        { "Shapeshift", "Charge", "Fireblast", "Greater Heal", "Cheap Shot" },
        { "Forest Spirits", "Protect Ally", "Magic Missile", "Blessing of Nature", "Mark for Death" },
        { "Barkskin", "Rampage", "Thunderstorm", "Curse of Agony", "Evicerate" },
        { "Moonfire", "Rot", "Frost Armour", "Unholy Aura", "Shadow Strike" }
    };

    public string GetSpellName(Enums.Race race, Enums.Job job)
    {
        return SpellNames[(int)race, (int)job];
    }
    /* reference cs script
        //Innervate
        private void ExecuteOldElfDruidSpell(Piece caster, Piece originalTarget, Board board, System.Random rngesus)
        {
            int newTargetIndex = rngesus.Next(0, board.GetActiveFriendliesOnBoard().Count - 1);
            Piece newTarget;
            if (board.GetActiveFriendliesOnBoard()[newTargetIndex].Equals(caster))
            {
                newTarget = board.GetActiveFriendliesOnBoard()[board.GetActiveFriendliesOnBoard().Count - 1];
            }
            else
            {
                newTarget = board.GetActiveFriendliesOnBoard()[newTargetIndex];
            }
            int overflowMana = caster.GetMaximumManaPoints();
            int newTargetMana = newTarget.GetCurrentManaPoints() + overflowMana;
            if (newTargetMana > newTarget.GetMaximumManaPoints())
            {
                overflowMana = newTargetMana - newTarget.GetMaximumManaPoints();
                newTarget.SetMaximumManaPoints(newTarget.GetMaximumManaPoints());
                caster.SetCurrentManaPoints(overflowMana);
            }
            else
            {
                newTarget.SetCurrentManaPoints(newTarget.GetCurrentManaPoints() + overflowMana);
            }
        }
*/
}
