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
    public int fireblastDefaultDamage = 100;
    public int cheapShotDefaultStunDuration = 200;
    public double chargeStackingDefaultPercentageIncrease = 1.2;
    public double chargeBaseDefaultDamage = 40;
    public double shapeShiftDefaultMultiplierIncrease = 1.2;
    public double blessingOfKingsDefaultMultiplierIncrease = 1.6;
    public int magicMissileDefaultCount = 3;
    public int magicMissileDefaultDamage = 40;
    public int greaterHealDefaultHeal = 150;
    public int barkskinDefaultBlockAmount = 7;
    public int forestSpiritsDefaultHealAmount = 9;
    public int forestSpiritsDefaultCount = 9;
    public int rampageDefaultAttackSpeedAmount = -3;
    public double rampageDefaultArmourPercentage = -0.25;
    public int thunderStormDefaultCount = 3;
    public int thunderStormDefaultDamage = 40;
    public int curseOfAgonyDefaultCurseAmount = 20;
    public int evicerateDefaultInitialDamage = 50;
    public int evicerateDefaultBleedCount = 5;
    public int evicerateDefaultBleedDamage = 30;
    public double moonfireDefaultManaRetainPercentage = 0.5;
    public int rotDefaultRadius = 1;
    public int rotDefaultCount = 10;
    public int rotDefaultDamage = 15;
    public double frostArmourDefaultArmourPercentage = 0.3;
    public int unholyAuraDefaultRadius = 4;
    public int unholyAuraDefaultCount = 7;
    public int unholyAuraDefaultDamage = 8;
    public int shadowStrikeDefaultDamage = 90;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetSpellName(Enums.Race race, Enums.Job job)
    {
        return SpellNames[(int)race, (int)job];
    }

    public void ChooseAndExecuteSpell(Piece caster, Piece originalTarget, Board board, System.Random rngesus)
    {
        if (caster.GetRace() == Enums.Race.Human && caster.GetClass() == Enums.Job.Druid)
            ExecuteHumanDruidSpell(caster, originalTarget, board);
        else if (caster.GetRace() == Enums.Race.Human && caster.GetClass() == Enums.Job.Knight)
            ExecuteHumanKnightSpell(caster, originalTarget, board);
        else if (caster.GetRace() == Enums.Race.Human && caster.GetClass() == Enums.Job.Mage)
            ExecuteHumanMageSpell(caster, originalTarget, board);
        else if (caster.GetRace() == Enums.Race.Human && caster.GetClass() == Enums.Job.Priest)
            ExecuteHumanPriestSpell(caster, originalTarget, board, rngesus);
        else if (caster.GetRace() == Enums.Race.Human && caster.GetClass() == Enums.Job.Rogue)
            ExecuteHumanRogueSpell(caster, originalTarget, board);
        else if (caster.GetRace() == Enums.Race.Elf && caster.GetClass() == Enums.Job.Druid)
            ExecuteElfDruidSpell(caster, originalTarget, board, rngesus);
        else if (caster.GetRace() == Enums.Race.Elf && caster.GetClass() == Enums.Job.Knight)
            ExecuteElfKnightSpell(caster, originalTarget, board, rngesus);
        else if (caster.GetRace() == Enums.Race.Elf && caster.GetClass() == Enums.Job.Mage)
            ExecuteElfMageSpell(caster, originalTarget, board, rngesus);
        else if (caster.GetRace() == Enums.Race.Elf && caster.GetClass() == Enums.Job.Priest)
            ExecuteElfPriestSpell(caster, originalTarget, board, rngesus);
        else if (caster.GetRace() == Enums.Race.Elf && caster.GetClass() == Enums.Job.Rogue)
            ExecuteElfRogueSpell(caster, originalTarget, board, rngesus);
        else if (caster.GetRace() == Enums.Race.Orc && caster.GetClass() == Enums.Job.Druid)
            ExecuteOrcDruidSpell(caster, originalTarget, board);
        else if (caster.GetRace() == Enums.Race.Orc && caster.GetClass() == Enums.Job.Knight)
            ExecuteOrcKnightSpell(caster, originalTarget, board);
        else if (caster.GetRace() == Enums.Race.Orc && caster.GetClass() == Enums.Job.Mage)
            ExecuteOrcMageSpell(caster, originalTarget, board, rngesus);
        else if (caster.GetRace() == Enums.Race.Orc && caster.GetClass() == Enums.Job.Priest)
            ExecuteOrcPriestSpell(caster, originalTarget, board);
        else if (caster.GetRace() == Enums.Race.Orc && caster.GetClass() == Enums.Job.Rogue)
            ExecuteOrcRogueSpell(caster, originalTarget, board);
        else if (caster.GetRace() == Enums.Race.Undead && caster.GetClass() == Enums.Job.Druid)
            ExecuteUndeadDruidSpell(caster, originalTarget, board);
        else if (caster.GetRace() == Enums.Race.Undead && caster.GetClass() == Enums.Job.Knight)
            ExecuteUndeadKnightSpell(caster, originalTarget, board);
        else if (caster.GetRace() == Enums.Race.Undead && caster.GetClass() == Enums.Job.Mage)
            ExecuteUndeadMageSpell(caster, originalTarget, board, rngesus);
        else if (caster.GetRace() == Enums.Race.Undead && caster.GetClass() == Enums.Job.Priest)
            ExecuteUndeadPriestSpell(caster, originalTarget, board);
        else if (caster.GetRace() == Enums.Race.Undead && caster.GetClass() == Enums.Job.Rogue)
            ExecuteUndeadRogueSpell(caster, originalTarget, board);
    }

    //shapeshift
    private void ExecuteHumanDruidSpell(Piece caster, Piece originalTarget, Board board)
    {
        caster.SetAttackDamage((int)Math.Floor(caster.GetAttackDamage() * shapeShiftDefaultMultiplierIncrease));
        caster.SetAttackSpeed((int)Math.Floor(caster.GetAttackSpeed() * shapeShiftDefaultMultiplierIncrease));
        caster.SetCurrentHitPoints((int)Math.Floor(caster.GetCurrentHitPoints() * shapeShiftDefaultMultiplierIncrease));
        caster.SetMaximumHitPoints((int)Math.Floor(caster.GetMaximumHitPoints() * shapeShiftDefaultMultiplierIncrease));
    }

    //Charge
    private void ExecuteHumanKnightSpell(Piece caster, Piece originalTarget, Board board)
    {
        Piece farthestTarget = board.FindFarthestTarget(caster); // Find a new Target (if any).
        if (farthestTarget == null) // There are no more enemies; game is Resolved.
        {
            Debug.Log("There are no more enemies for " + caster.GetName() + " to target. Game is Resolved.");
        }
        else
        {
            Debug.Log(caster.GetName() + " now has a Target of " + farthestTarget.GetName() + " and is going to charge to it.");
        }
        int distance = caster.GetCurrentTile().DistanceToTile(farthestTarget.GetCurrentTile());

        int newTargetRow = farthestTarget.GetCurrentTile().GetRow();
        int newTargetCol = farthestTarget.GetCurrentTile().GetCol();
        int selfRow = caster.GetCurrentTile().GetRow();
        int selfCol = caster.GetCurrentTile().GetCol();

        if (!board.GetTile(newTargetRow, newTargetCol-1).IsLocked() && !board.GetTile(newTargetRow, newTargetCol-1).IsOccupied() && selfCol<newTargetCol)
        {
            board.MovePieceToTile(caster, board.GetTile(newTargetRow, newTargetCol-1));
            farthestTarget.SetCurrentHitPoints((int)Math.Floor(farthestTarget.GetCurrentHitPoints() - chargeBaseDefaultDamage * Math.Pow(chargeStackingDefaultPercentageIncrease, distance)));
        }
        else if (!board.GetTile(newTargetRow, newTargetCol+1).IsLocked() && !board.GetTile(newTargetRow, newTargetCol+1).IsOccupied() && selfCol>newTargetCol)
        {
            board.MovePieceToTile(caster, board.GetTile(newTargetRow, newTargetCol+1));
            farthestTarget.SetCurrentHitPoints((int)Math.Floor(farthestTarget.GetCurrentHitPoints() - chargeBaseDefaultDamage * Math.Pow(chargeStackingDefaultPercentageIncrease, distance)));
        }
        else if (!board.GetTile(newTargetRow-1, newTargetCol).IsLocked() && !board.GetTile(newTargetRow-1, newTargetCol).IsOccupied() && selfRow<newTargetRow)
        {
            board.MovePieceToTile(caster, board.GetTile(newTargetRow-1, newTargetCol));
            farthestTarget.SetCurrentHitPoints((int)Math.Floor(farthestTarget.GetCurrentHitPoints() - chargeBaseDefaultDamage * Math.Pow(chargeStackingDefaultPercentageIncrease, distance)));
        }
        else if (!board.GetTile(newTargetRow+1, newTargetCol).IsLocked() && !board.GetTile(newTargetRow+1, newTargetCol).IsOccupied() && selfRow>newTargetRow)
        {
            board.MovePieceToTile(caster, board.GetTile(newTargetRow+1, newTargetCol));
            farthestTarget.SetCurrentHitPoints((int)Math.Floor(farthestTarget.GetCurrentHitPoints() - chargeBaseDefaultDamage * Math.Pow(chargeStackingDefaultPercentageIncrease, distance)));
        }
    }

    //Fireblast
    private void ExecuteHumanMageSpell(Piece caster, Piece originalTarget, Board board)
    {
        if (originalTarget.IsDead())
        {
            return;
        }

        originalTarget.SetCurrentHitPoints(originalTarget.GetCurrentHitPoints() - fireblastDefaultDamage);

        originalTarget.SetCurrentManaPoints(originalTarget.GetCurrentManaPoints() + originalTarget.GetManaPointsGainedOnDamaged());
        Debug.Log(caster.GetName() + " has fireblasted " + originalTarget.GetName() + " for " + fireblastDefaultDamage + " DMG, whose HP has dropped to " + originalTarget.GetCurrentHitPoints() + " HP.");
    }

    //Greater Heal
    private void ExecuteHumanPriestSpell(Piece caster, Piece originalTarget, Board board, System.Random rngesus)
    {
        List<Piece> damagedFriendlies = board.GetActiveFriendliesOnBoard().FindAll(piece => piece.GetCurrentHitPoints() < piece.GetMaximumHitPoints());
        Piece targetFriendly = damagedFriendlies[rngesus.Next(0, damagedFriendlies.Count)];
        targetFriendly.SetCurrentHitPoints(targetFriendly.GetCurrentHitPoints() + greaterHealDefaultHeal);
    }

    //Cheap Shot
    private void ExecuteHumanRogueSpell(Piece caster, Piece originalTarget, Board board)
    {
        originalTarget.GetState().ticksRemaining += cheapShotDefaultStunDuration;
    }

    //Forest Spirits
    private void ExecuteElfDruidSpell(Piece caster, Piece originalTarget, Board board, System.Random rngesus)
    {
        List<Piece> damagedFriendlies = board.GetActiveFriendliesOnBoard().FindAll(piece => piece.GetCurrentHitPoints() < piece.GetMaximumHitPoints());
        Piece targetFriendly;
        for (int i=0; i<forestSpiritsDefaultCount; i++)
        {
            targetFriendly = damagedFriendlies[rngesus.Next(0, damagedFriendlies.Count)];
            targetFriendly.SetCurrentHitPoints(targetFriendly.GetCurrentHitPoints() + forestSpiritsDefaultHealAmount);
        }
    }

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

    //Protect Ally
    private void ExecuteElfKnightSpell(Piece caster, Piece originalTarget, Board board, System.Random rngesus)
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
        newTarget.SetLinkedProtectingPiece(caster);
    }

    //Magic Missile
    private void ExecuteElfMageSpell(Piece caster, Piece originalTarget, Board board, System.Random rngesus)
    {
        Piece[] newTargets = new Piece[magicMissileDefaultCount];
        for (int i=0; i<newTargets.Length; i++)
        {
            newTargets[i] = board.GetActiveEnemiesOnBoard()[rngesus.Next(0, board.GetActiveEnemiesOnBoard().Count)];
            newTargets[i].SetCurrentHitPoints(newTargets[i].GetCurrentHitPoints() - magicMissileDefaultDamage);
        }
    }

    //Blessing of Nature
    private void ExecuteElfPriestSpell(Piece caster, Piece originalTarget, Board board, System.Random rngesus)
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
        newTarget.SetAttackDamage((int)Math.Floor(newTarget.GetAttackDamage() * blessingOfKingsDefaultMultiplierIncrease));
        newTarget.SetCurrentHitPoints((int)Math.Floor(newTarget.GetCurrentHitPoints() * blessingOfKingsDefaultMultiplierIncrease));
        newTarget.SetMaximumHitPoints((int)Math.Floor(newTarget.GetMaximumHitPoints() * blessingOfKingsDefaultMultiplierIncrease));
    }

    //Mark for Death
    private void ExecuteElfRogueSpell(Piece caster, Piece originalTarget, Board board, System.Random rngesus)
    {
        Piece targetEnemy = board.GetActiveEnemiesOnBoard()[rngesus.Next(0, board.GetActiveEnemiesOnBoard().Count)];
        targetEnemy.SetCurrentHitPoints(0);
    }

    //Barkskin
    private void ExecuteOrcDruidSpell(Piece caster, Piece originalTarget, Board board)
    {
        caster.SetBlockAmount(caster.GetBlockAmount() + barkskinDefaultBlockAmount);
    }

    //Rampage
    private void ExecuteOrcKnightSpell(Piece caster, Piece originalTarget, Board board)
    {
        caster.SetAttackSpeed(caster.GetAttackSpeed() + rampageDefaultAttackSpeedAmount);
        caster.SetArmourPercentage(caster.GetArmourPercentage() + rampageDefaultArmourPercentage);
    }

    //Thunderstorm
    private void ExecuteOrcMageSpell(Piece caster, Piece originalTarget, Board board, System.Random rngesus)
    {
        Piece[] newTargets = new Piece[thunderStormDefaultCount];
        for (int i = 0; i < newTargets.Length; i++)
        {
            newTargets[i] = board.GetActiveEnemiesOnBoard()[rngesus.Next(0, board.GetActiveEnemiesOnBoard().Count)];
            newTargets[i].SetCurrentHitPoints(newTargets[i].GetCurrentHitPoints() - thunderStormDefaultDamage);
        }
    }

    //Curse of Agony
    private void ExecuteOrcPriestSpell(Piece caster, Piece originalTarget, Board board)
    {
        originalTarget.SetCurseDamageAmount(originalTarget.GetCurseDamageAmount() + curseOfAgonyDefaultCurseAmount);
    }

    //Evicerate
    private void ExecuteOrcRogueSpell(Piece caster, Piece originalTarget, Board board)
    {
        originalTarget.SetCurrentHitPoints(originalTarget.GetCurrentHitPoints() - evicerateDefaultInitialDamage);
        for (int i=0; i<evicerateDefaultBleedCount; i++)
        {
            originalTarget.SetCurrentHitPoints(originalTarget.GetCurrentHitPoints() - evicerateDefaultBleedDamage);
        }
    }

    //Moonfire
    private void ExecuteUndeadDruidSpell(Piece caster, Piece originalTarget, Board board)
    {
        originalTarget.SetCurrentHitPoints(originalTarget.GetCurrentHitPoints() - evicerateDefaultInitialDamage);
        caster.SetCurrentManaPoints((int)(caster.GetMaximumManaPoints() * moonfireDefaultManaRetainPercentage));
    }

    //Rot
    private void ExecuteUndeadKnightSpell(Piece caster, Piece originalTarget, Board board)
    {
        for (int i=0; i<rotDefaultCount; i++)
        {
            List<Piece> targetEnemies = board.GetActiveEnemiesWithinRadiusOfTile(caster.GetCurrentTile(), rotDefaultRadius);
            foreach (Piece target in targetEnemies)
            {
                target.SetCurrentHitPoints(target.GetCurrentHitPoints() - rotDefaultDamage);
            }
        }
    }

    //Frost Armour
    private void ExecuteUndeadMageSpell(Piece caster, Piece originalTarget, Board board, System.Random rngesus)
    {
        Piece newTarget = board.GetActiveFriendliesOnBoard()[rngesus.Next(0, board.GetActiveFriendliesOnBoard().Count)];
        newTarget.SetArmourPercentage(newTarget.GetArmourPercentage() + frostArmourDefaultArmourPercentage);
    }

    //Unholy Aura
    private void ExecuteUndeadPriestSpell(Piece caster, Piece originalTarget, Board board)
    {
        for (int i = 0; i < unholyAuraDefaultCount; i++)
        {
            List<Piece> targetEnemies = board.GetActiveEnemiesWithinRadiusOfTile(caster.GetCurrentTile(), unholyAuraDefaultRadius);
            foreach (Piece target in targetEnemies)
            {
                target.SetCurrentHitPoints(target.GetCurrentHitPoints() - unholyAuraDefaultDamage);
            }
        }
    }

    //Shadow Strike
    private void ExecuteUndeadRogueSpell(Piece caster, Piece originalTarget, Board board)
    {
        Piece farthestTarget = board.FindFarthestTarget(caster); // Find a new Target (if any).
        if (farthestTarget == null) // There are no more enemies; game is Resolved.
        {
            Debug.Log("There are no more enemies for " + caster.GetName() + " to target. Game is Resolved.");
        }
        else
        {
            Debug.Log(caster.GetName() + " now has a Target of " + farthestTarget.GetName() + " and is going to shadow strike to it.");
        }
        int distance = caster.GetCurrentTile().DistanceToTile(farthestTarget.GetCurrentTile());

        int newTargetRow = farthestTarget.GetCurrentTile().GetRow();
        int newTargetCol = farthestTarget.GetCurrentTile().GetCol();
        int selfRow = caster.GetCurrentTile().GetRow();
        int selfCol = caster.GetCurrentTile().GetCol();

        if (!board.GetTile(newTargetRow + 1, newTargetCol).IsLocked() && !board.GetTile(newTargetRow + 1, newTargetCol).IsOccupied() && selfRow < newTargetRow)
        {
            board.MovePieceToTile(caster, board.GetTile(newTargetRow + 1, newTargetCol));
            farthestTarget.SetCurrentHitPoints(farthestTarget.GetCurrentHitPoints() - shadowStrikeDefaultDamage);
        }
        else if (!board.GetTile(newTargetRow - 1, newTargetCol).IsLocked() && !board.GetTile(newTargetRow - 1, newTargetCol).IsOccupied() && selfRow > newTargetRow)
        {
            board.MovePieceToTile(caster, board.GetTile(newTargetRow - 1, newTargetCol));
            farthestTarget.SetCurrentHitPoints(farthestTarget.GetCurrentHitPoints() - shadowStrikeDefaultDamage);
        }
        else if (!board.GetTile(newTargetRow, newTargetCol - 1).IsLocked() && !board.GetTile(newTargetRow, newTargetCol - 1).IsOccupied() && selfCol > newTargetCol)
        {
            board.MovePieceToTile(caster, board.GetTile(newTargetRow, newTargetCol - 1));
            farthestTarget.SetCurrentHitPoints(farthestTarget.GetCurrentHitPoints() - shadowStrikeDefaultDamage);
        }
        else if (!board.GetTile(newTargetRow, newTargetCol + 1).IsLocked() && !board.GetTile(newTargetRow, newTargetCol + 1).IsOccupied() && selfCol < newTargetCol)
        {
            board.MovePieceToTile(caster, board.GetTile(newTargetRow, newTargetCol + 1));
            farthestTarget.SetCurrentHitPoints(farthestTarget.GetCurrentHitPoints() - shadowStrikeDefaultDamage);
        }
    }
}
