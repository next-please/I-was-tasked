using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyGenerator
{
    public CharacterGenerator characterGenerator;
    private System.Random rngesus;

    // Constructor
    public EnemyGenerator()
    {
        rngesus = new System.Random();
        characterGenerator = new CharacterGenerator();
    }

    public String generateWaveNames(int roundNumber, int index)
    {
        switch (roundNumber)
        {
            case 1:
                return wave1Names(index);
            case 2:
                return wave2Names(index);
            case 3:
                return wave3Names(index);
            case 4:
                return wave4Names(index);
            case 5:
                return wave5Names(index);
            case 6:
                return wave6Names(index);
            case 7:
                return wave7Names(index);
            case 8:
                return wave8Names(index);
            case 9:
                return wave9Names(index);
            case 10:
                return wave10Names(index);
            case 11:
                return wave11Names(index);
            case 12:
                return wave12Names(index);
            case 13:
                return wave13Names(index);
            case 14:
                return wave14Names(index);
            case 15:
                return wave15Names(index);
            default:
                return "";
        }
    }
    public List<Piece> generateEnemies(int roundNumber, int index)
    {
        switch (roundNumber)
        {
            case 0:
                return tutorialEnemies();
            case 1:
                return wave1Enemies(index);
            case 2:
                return wave2Enemies(index);
            case 3:
                return wave3Enemies(index);
            case 4:
                return wave4Enemies(index);
            case 5:
                return wave5Enemies(index);
            case 6:
                return wave6Enemies(index);
            case 7:
                return wave7Enemies(index);
            case 8:
                return wave8Enemies(index);
            case 9:
                return wave9Enemies(index);
            case 10:
                return wave10Enemies(index);
            case 11:
                return wave11Enemies(index);
            case 12:
                return wave12Enemies(index);
            case 13:
                return wave13Enemies(index);
            case 14:
                return wave14Enemies(index);
            case 15:
                return wave15Enemies(index);
            default:
                return new List<Piece>();
        }
    }

    public int getWaveRandomIndex(int waveNumber)
    {
        switch (waveNumber)
        {
            case 1:
                return rngesus.Next(0, 1);
            case 2:
                return rngesus.Next(0, 1);
            case 3:
                return rngesus.Next(0, 1);
            case 4:
                return rngesus.Next(0, 1);
            case 5:
                return rngesus.Next(0, 1);
            case 6:
                return rngesus.Next(0, 1);
            case 7:
                return rngesus.Next(0, 1);
            case 8:
                return rngesus.Next(0, 1);
            case 9:
                return rngesus.Next(0, 1);
            case 10:
                return rngesus.Next(0, 1);
            case 11:
                return rngesus.Next(0, 1);
            case 12:
                return rngesus.Next(0, 1);
            case 13:
                return rngesus.Next(0, 1);
            case 14:
                return rngesus.Next(0, 1);
            case 15:
                return rngesus.Next(0, 1);
            default:
                return rngesus.Next(0, 1);
        }
    }

    public List<Piece> tutorialEnemies()
    {
        Piece enemy = new Piece("Enemy #1)", "", Enums.Race.Orc, Enums.Job.Knight, 1, true, 300, 100, 30, 1, 4, 5, Enums.Spell.NoSkill);
        enemy.SetDamageIfSurvive(1);
        enemy.startingSpot = new Tuple<int, int>(3, 4);
        return new List<Piece>() { enemy };
    }

    public String wave1Names(int index)
    {
        switch (index)
        {
            case 0:
                return "Training Round";
            default:
                return "";
        }
    }
    public List<Piece> wave1Enemies(int index)
    {
        List<Piece> enemies = new List<Piece>();
        Piece enemy;
        switch (index)
        {
            case 0:
                enemy = new Piece("Enemy #1)", "", Enums.Race.Orc, Enums.Job.Knight, 1, true, 200, 100, 10, 1, 4, 5, Enums.Spell.NoSkill);
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(3, 4);
                enemies.Add(enemy);
                break;
        }
        return enemies;
    }
    public String wave2Names(int index)
    {
        switch (index)
        {
            case 0:
                return "Roaming Monsters";
            default:
                return "";
        }
    }
    public List<Piece> wave2Enemies(int index)
    {
        List<Piece> enemies = new List<Piece>();
        Piece enemy;
        switch (index)
        {
            case 0:
                enemy = new Piece("Enemy #1)", "", Enums.Race.Orc, Enums.Job.Knight, 1, true, 120, 100, 10, 1, 4, 5, Enums.Spell.NoSkill);
                enemy.SetDamageIfSurvive(0);
                enemy.startingSpot = new Tuple<int, int>(3, 4);
                enemies.Add(enemy);
                enemy = new Piece("Enemy #2)", "", Enums.Race.Orc, Enums.Job.Knight, 1, true, 100, 100, 12, 1, 4, 5, Enums.Spell.NoSkill);
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(5, 4);
                enemies.Add(enemy);
                enemy = new Piece("Enemy #1)", "", Enums.Race.Orc, Enums.Job.Priest, 1, true, 90, 100, 5, 2, 4, 5, Enums.Spell.NoSkill);
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(4, 6);
                enemies.Add(enemy);
                break;
        }
        return enemies;
    }
    public String wave3Names(int index)
    {
        switch (index)
        {
            case 0:
                return "Bandits";
            default:
                return "";
        }
    }
    public List<Piece> wave3Enemies(int index)
    {
        List<Piece> enemies = new List<Piece>();
        Piece enemy;
        switch (index)
        {
            case 0:
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Rogue, Enums.Race.Human, true);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #1");
                enemy.SetAttackDamage(18);
                enemy.SetMaximumHitPoints(190);
                enemy.SetCurrentHitPoints(enemy.GetMaximumHitPoints());
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(6, 4);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(0, Enums.Job.Rogue, Enums.Race.Orc, true);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #2");
                enemy.SetAttackDamage(12);
                enemy.SetTitle("");
                enemy.SetMaximumHitPoints(190);
                enemy.SetCurrentHitPoints(190);
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(3, 4);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(0, Enums.Job.Rogue, Enums.Race.Orc, true);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #3");
                enemy.SetAttackDamage(12);
                enemy.SetTitle("");
                enemy.SetMaximumHitPoints(190);
                enemy.SetCurrentHitPoints(190);
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(4, 4);
                enemies.Add(enemy);
                break;
        }
        return enemies;
    }
    public String wave4Names(int index)
    {
        switch (index)
        {
            case 0:
                return "Goblin Swarm";
            default:
                return "";
        }
    }
    public List<Piece> wave4Enemies(int index)
    {
        List<Piece> enemies = new List<Piece>();
        Piece enemy;
        switch (index)
        {
            case 0:
                enemy = new Piece("Enemy #1)", "Swarm", Enums.Race.Orc, Enums.Job.Knight, 1, true, 90, 100, 10, 1, 4, 5, Enums.Spell.NoSkill);
                enemy.SetDamageIfSurvive(0);
                enemy.startingSpot = new Tuple<int, int>(1, 4);
                enemy.size = Enums.Size.Small;
                enemies.Add(enemy);
                enemy = new Piece("Enemy #2)", "Swarm", Enums.Race.Orc, Enums.Job.Knight, 1, true, 90, 100, 8, 1, 3, 5, Enums.Spell.NoSkill);
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(2, 5);
                enemy.size = Enums.Size.Small;
                enemies.Add(enemy);
                enemy = new Piece("Enemy #3)", "Swarm", Enums.Race.Orc, Enums.Job.Mage, 1, true, 60, 100, 9, 6, 4, 5, Enums.Spell.NoSkill);
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(3, 7);
                enemy.size = Enums.Size.Small;
                enemies.Add(enemy);
                enemy = new Piece("Enemy #4)", "Swarm", Enums.Race.Orc, Enums.Job.Mage, 1, true, 60, 100, 9, 6, 5, 5, Enums.Spell.NoSkill);
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(4, 7);
                enemy.size = Enums.Size.Small;
                enemies.Add(enemy);
                enemy = new Piece("Enemy #5)", "Swarm", Enums.Race.Orc, Enums.Job.Knight, 1, true, 90, 100, 11, 1, 5, 5, Enums.Spell.NoSkill);
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(5, 5);
                enemy.size = Enums.Size.Small;
                enemies.Add(enemy);
                enemy = new Piece("Enemy #6)", "Swarm", Enums.Race.Orc, Enums.Job.Knight, 1, true, 90, 100, 10, 1, 4, 5, Enums.Spell.NoSkill);
                enemy.SetDamageIfSurvive(0);
                enemy.startingSpot = new Tuple<int, int>(6, 4);
                enemy.size = Enums.Size.Small;
                enemies.Add(enemy);
                break;
        }
        return enemies;
    }
    public String wave5Names(int index)
    {
        switch (index)
        {
            case 0:
                return "Rival Nation";
            default:
                return "";
        }
    }
    public List<Piece> wave5Enemies(int index)
    {
        List<Piece> enemies = new List<Piece>();
        Piece enemy;
        switch (index)
        {
            case 0:
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Knight, Enums.Race.Human, true);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #1");
                enemy.SetCurrentHitPoints(enemy.GetMaximumHitPoints());
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(3, 5);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Knight, Enums.Race.Human, true);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #2");
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(4, 6);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Druid, Enums.Race.Human, true);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #3");
                enemy.SetMaximumHitPoints(270);
                enemy.SetCurrentHitPoints(enemy.GetMaximumHitPoints());
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(4, 5);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(0, Enums.Job.Priest, Enums.Race.Human, true);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #4");
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(3, 7);
                enemies.Add(enemy);
                break;
        }
        return enemies;
    }
    public String wave6Names(int index)
    {
        switch (index)
        {
            case 0:
                return "Assassins";
            default:
                return "";
        }
    }
    public List<Piece> wave6Enemies(int index)
    {
        List<Piece> enemies = new List<Piece>();
        Piece enemy;
        switch (index)
        {
            case 0:
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Rogue, Enums.Race.Undead, true);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #1");
                enemy.spell = Enums.Spell.ShadowStrike;
                enemy.SetMaximumManaPoints((int)(enemy.GetMaximumManaPoints() * 0.7));
                enemy.SetTitle("");
                enemy.SetMaximumHitPoints(235);
                enemy.SetCurrentHitPoints(235);
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(0, 7);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(0, Enums.Job.Rogue, Enums.Race.Human, true);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #2");
                enemy.spell = Enums.Spell.ShadowStrike;
                enemy.SetMaximumManaPoints((int)(enemy.GetMaximumManaPoints() * 0.7));
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(1, 4);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Rogue, Enums.Race.Undead, true);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #3");
                enemy.spell = Enums.Spell.ShadowStrike;
                enemy.SetMaximumManaPoints((int)(enemy.GetMaximumManaPoints() * 0.7));
                enemy.SetTitle("");
                enemy.SetMaximumHitPoints(235);
                enemy.SetCurrentHitPoints(235);
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(3, 7);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Rogue, Enums.Race.Orc, true);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #4");
                enemy.spell = Enums.Spell.ShadowStrike;
                enemy.SetMaximumManaPoints((int)(enemy.GetMaximumManaPoints() * 0.7));
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(5, 5);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(0, Enums.Job.Rogue, Enums.Race.Orc, true);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #5");
                enemy.spell = Enums.Spell.ShadowStrike;
                enemy.SetMaximumManaPoints((int)(enemy.GetMaximumManaPoints() * 0.7));
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(6, 6);
                enemies.Add(enemy);
                break;
        }
        return enemies;
    }
    public String wave7Names(int index)
    {
        switch (index)
        {
            case 0:
                return "Rotting Shambles";
            default:
                return "";
        }
    }
    public List<Piece> wave7Enemies(int index)
    {
        List<Piece> enemies = new List<Piece>();
        Piece enemy;
        switch (index)
        {
            case 0:
                enemy = characterGenerator.GenerateCharacter(2, Enums.Job.Priest, Enums.Race.Undead, false);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #1");
                enemy.SetMaximumHitPoints(770);
                enemy.SetCurrentHitPoints(770);
                enemy.SetAttackDamage((int)(enemy.GetAttackDamage() * 2.3));
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(2);
                enemy.startingSpot = new Tuple<int, int>(2, 7);
                enemy.interactions.Add(new DecayingSynergyEffect(enemy));
                enemy.rotting = true;
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(0, Enums.Job.Druid, Enums.Race.Undead, false);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #2");
                enemy.SetMaximumHitPoints(630);
                enemy.SetCurrentHitPoints(630);
                enemy.SetAttackDamage((int)(enemy.GetAttackDamage() * 2.3));
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(2);
                enemy.startingSpot = new Tuple<int, int>(6, 6);
                enemy.interactions.Add(new DecayingSynergyEffect(enemy));
                enemy.rotting = true;
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(4, Enums.Job.Knight, Enums.Race.Undead, false);
                enemy.SetIsEnemy(true);
                enemy.spell = Enums.Spell.NoSkill;
                enemy.SetName("Enemy #3");
                enemy.SetMaximumHitPoints(1050);
                enemy.SetCurrentHitPoints(1050);
                enemy.SetAttackDamage((int)(enemy.GetAttackDamage() * 2.3));
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(2, 6);
                enemy.interactions.Add(new DecayingSynergyEffect(enemy));
                enemy.rotting = true;
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(4, Enums.Job.Druid, Enums.Race.Undead, false);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #4");
                enemy.SetMaximumHitPoints(910);
                enemy.SetCurrentHitPoints(910);
                enemy.SetAttackDamage((int)(enemy.GetAttackDamage() * 2.3));
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(3, 6);
                enemy.interactions.Add(new DecayingSynergyEffect(enemy));
                enemy.rotting = true;
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Rogue, Enums.Race.Undead, false);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #5");
                enemy.spell = Enums.Spell.NoSkill;
                enemy.SetMaximumHitPoints(490);
                enemy.SetCurrentHitPoints(490);
                enemy.SetAttackDamage((int)(enemy.GetAttackDamage() * 2.3));
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(4, 6);
                enemy.interactions.Add(new DecayingSynergyEffect(enemy));
                enemy.rotting = true;
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(3, Enums.Job.Priest, Enums.Race.Undead, false);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #6");
                enemy.SetMaximumHitPoints(1190);
                enemy.SetCurrentHitPoints(1190);
                enemy.SetAttackDamage((int)(enemy.GetAttackDamage() * 2.3));
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(5, 7);
                enemy.interactions.Add(new DecayingSynergyEffect(enemy));
                enemy.rotting = true;
                enemies.Add(enemy);
                break;
        }
        return enemies;
    }
    public String wave8Names(int index)
    {
        switch (index)
        {
            case 0:
                return "Elven Ambush";
            default:
                return "";
        }
    }
    public List<Piece> wave8Enemies(int index)
    {
        List<Piece> enemies = new List<Piece>();
        Piece enemy;
        switch (index)
        {
            case 0:
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Druid, Enums.Race.Elf, true);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #1");
                enemy.SetTitle("");
                enemy.SetRarity(1);
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(2, 4);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Knight, Enums.Race.Elf, true);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #2");
                enemy.SetTitle("");
                enemy.SetRarity(1);
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(3, 4);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(2, Enums.Job.Rogue, Enums.Race.Elf, true);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #3");
                enemy.SetTitle("");
                enemy.SetRarity(2);
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(4, 5);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(2, Enums.Job.Mage, Enums.Race.Elf, true);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #4");
                enemy.SetTitle("");
                enemy.SetRarity(2);
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(7, 4);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Mage, Enums.Race.Elf, true);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #5");
                enemy.SetTitle("");
                enemy.SetRarity(1);
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(1, 6);
                enemies.Add(enemy);
                break;
        }
        return enemies;
    }
    public String wave9Names(int index)
    {
        switch (index)
        {
            case 0:
                return "Undead Horde";
            default:
                return "";
        }
    }
    public List<Piece> wave9Enemies(int index)
    {
        List<Piece> enemies = new List<Piece>();
        Piece enemy;
        switch (index)
        {
            case 0:
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Knight, Enums.Race.Undead, true);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #1");
                enemy.SetTitle("");
                enemy.SetMaximumHitPoints(enemy.GetMaximumHitPoints() * 6);
                enemy.SetCurrentHitPoints(enemy.GetMaximumHitPoints());
                enemy.SetMaximumManaPoints((int)(enemy.GetAttackDamage() * 0.6));
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(0, 4);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Knight, Enums.Race.Undead, true);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #2");
                enemy.SetTitle("");
                enemy.SetMaximumHitPoints(enemy.GetMaximumHitPoints() * 6);
                enemy.SetCurrentHitPoints(enemy.GetMaximumHitPoints());
                enemy.SetMaximumManaPoints((int)(enemy.GetAttackDamage() * 0.4));
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(1, 4);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Knight, Enums.Race.Undead, true);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #3");
                enemy.SetTitle("");
                enemy.SetMaximumHitPoints(enemy.GetMaximumHitPoints() * 6);
                enemy.SetCurrentHitPoints(enemy.GetMaximumHitPoints());
                enemy.SetMaximumManaPoints((int)(enemy.GetAttackDamage() * 0.8));
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(2, 4);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Knight, Enums.Race.Undead, true);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #4");
                enemy.SetTitle("");
                enemy.SetMaximumHitPoints(enemy.GetMaximumHitPoints() * 6);
                enemy.SetCurrentHitPoints(enemy.GetMaximumHitPoints());
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(3, 4);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Knight, Enums.Race.Undead, true);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #5");
                enemy.SetTitle("");
                enemy.SetMaximumHitPoints(enemy.GetMaximumHitPoints() * 6);
                enemy.SetCurrentHitPoints(enemy.GetMaximumHitPoints());
                enemy.SetMaximumManaPoints((int)(enemy.GetAttackDamage() * 0.8));
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(4, 4);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Knight, Enums.Race.Undead, true);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #6");
                enemy.SetTitle("");
                enemy.SetMaximumHitPoints(enemy.GetMaximumHitPoints() * 6);
                enemy.SetCurrentHitPoints(enemy.GetMaximumHitPoints());
                enemy.SetMaximumManaPoints((int)(enemy.GetAttackDamage() * 0.6));
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(5, 4);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Knight, Enums.Race.Undead, true);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #7");
                enemy.SetTitle("");
                enemy.SetMaximumHitPoints(enemy.GetMaximumHitPoints() * 6);
                enemy.SetCurrentHitPoints(enemy.GetMaximumHitPoints());
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(6, 4);
                enemies.Add(enemy);
                break;
        }
        return enemies;
    }
    public String wave10Names(int index)
    {
        switch (index)
        {
            case 0:
                return "Ritual Circle";
            default:
                return "";
        }
    }
    public List<Piece> wave10Enemies(int index)
    {
        List<Piece> enemies = new List<Piece>();
        Piece enemy;
        switch (index)
        {
            case 0:
                enemy = characterGenerator.GenerateCharacter(3, Enums.Job.Mage, Enums.Race.Elf, true);
                enemy.SetIsEnemy(true);
                enemy.spell = Enums.Spell.InfiniteMagicMissile;
                enemy.SetAttackRange(10);
                enemy.SetMaximumHitPoints(440);
                enemy.SetCurrentHitPoints(440);
                enemy.SetCurrentManaPoints(80);
                enemy.SetName("Enemy #1");
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(2);
                enemy.startingSpot = new Tuple<int, int>(3, 7);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(2, Enums.Job.Priest, Enums.Race.Orc, false);
                enemy.SetIsEnemy(true);
                enemy.spell = Enums.Spell.NoSkill;
                enemy.SetAttackRange(10);
                enemy.SetMaximumManaPoints(150);
                enemy.SetName("Enemy #2");
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(2, 5);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(2, Enums.Job.Priest, Enums.Race.Orc, false);
                enemy.SetIsEnemy(true);
                enemy.spell = Enums.Spell.NoSkill;
                enemy.SetAttackRange(10);
                enemy.SetMaximumManaPoints(150);
                enemy.SetName("Enemy #3");
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(3, 4);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(2, Enums.Job.Priest, Enums.Race.Orc, false);
                enemy.SetIsEnemy(true);
                enemy.spell = Enums.Spell.NoSkill;
                enemy.SetAttackRange(10);
                enemy.SetMaximumManaPoints(150);
                enemy.SetName("Enemy #4");
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(4, 4);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(2, Enums.Job.Priest, Enums.Race.Orc, false);
                enemy.SetIsEnemy(true);
                enemy.spell = Enums.Spell.NoSkill;
                enemy.SetAttackRange(10);
                enemy.SetMaximumManaPoints(150);
                enemy.SetName("Enemy #5");
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(5, 5);
                enemies.Add(enemy);
                break;
        }
        return enemies;
    }
    public String wave11Names(int index)
    {
        switch (index)
        {
            case 0:
                return "Force of Nature";
            default:
                return "";
        }
    }
    public List<Piece> wave11Enemies(int index)
    {
        List<Piece> enemies = new List<Piece>();
        Piece enemy;
        switch (index)
        {
            case 0:
                enemy = characterGenerator.GenerateCharacter(2, Enums.Job.Druid, Enums.Race.Human, false);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #1");
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(1, 5);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(2, Enums.Job.Druid, Enums.Race.Undead, false);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #2");
                enemy.SetTitle("");
                enemy.SetMaximumHitPoints(250);
                enemy.SetCurrentHitPoints(250);
                enemy.SetDamageIfSurvive(1);
                enemy.SetAttackRange(3);
                enemy.startingSpot = new Tuple<int, int>(3, 6);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(2, Enums.Job.Druid, Enums.Race.Elf, false);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #3");
                enemy.SetTitle("");
                enemy.SetMaximumManaPoints((int)(enemy.GetMaximumManaPoints() * 1.5));
                enemy.SetDamageIfSurvive(1);
                enemy.SetAttackRange(3);
                enemy.startingSpot = new Tuple<int, int>(4, 6);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Druid, Enums.Race.Orc, false);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #4");
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(2, 4);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(2, Enums.Job.Druid, Enums.Race.Orc, false);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #5");
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(5, 4);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Druid, Enums.Race.Human, false);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #6");
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(6, 5);
                enemies.Add(enemy);
                break;
        }
        return enemies;
    }
    public String wave12Names(int index)
    {
        switch (index)
        {
            case 0:
                return "Lumbering Ogre";
            default:
                return "";
        }
    }
    public List<Piece> wave12Enemies(int index)
    {
        List<Piece> enemies = new List<Piece>();
        Piece enemy;
        switch (index)
        {
            case 0:
                enemy = characterGenerator.GenerateCharacter(4, Enums.Job.Knight, Enums.Race.Undead, false);
                enemy.SetIsEnemy(true);
                enemy.SetRace(Enums.Race.Orc);
                enemy.spell = Enums.Spell.Berserk;
                enemy.size = Enums.Size.Big;
                enemy.SetMaximumHitPoints(3800);
                enemy.SetCurrentHitPoints(3800);
                enemy.SetName("Enemy #1");
                enemy.SetMaximumManaPoints(250);
                enemy.SetMovementSpeed(2);
                enemy.SetDefaultMovementSpeed(2);
                enemy.SetAttackSpeed(2);
                enemy.SetDefaultAttackSpeed(2);
                enemy.SetAttackDamage(140);
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(5);
                enemy.startingSpot = new Tuple<int, int>(0, 7);
                enemies.Add(enemy);
                break;
        }
        return enemies;
    }
    public String wave13Names(int index)
    {
        switch (index)
        {
            case 0:
                return "Organised Army";
            default:
                return "";
        }
    }
    public List<Piece> wave13Enemies(int index)
    {
        List<Piece> enemies = new List<Piece>();
        Piece enemy;
        switch (index)
        {
            case 0:
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Knight, Enums.Race.Human, true);
                enemy.SetAttackDamage(5);
                enemy.SetAttackSpeed(3);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #1");
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(2, 4);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Knight, Enums.Race.Human, true);
                enemy.SetAttackDamage(5);
                enemy.SetAttackSpeed(3);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #2");
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(3, 4);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(0, Enums.Job.Knight, Enums.Race.Human, true);
                enemy.SetAttackDamage(5);
                enemy.SetAttackSpeed(3);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #3");
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(4, 4);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(0, Enums.Job.Knight, Enums.Race.Human, true);
                enemy.SetAttackDamage(5);
                enemy.SetAttackSpeed(3);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #4");
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(5, 4);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Spearman, Enums.Race.Human, true);
                enemy.SetClass(Enums.Job.Spearman);
                enemy.spell = Enums.Spell.Charge;
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #5");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(3, 5);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Spearman, Enums.Race.Human, true);
                enemy.SetClass(Enums.Job.Spearman);
                enemy.spell = Enums.Spell.Charge;
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #6");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(4, 5);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Archer, Enums.Race.Human, true);
                enemy.SetClass(Enums.Job.Archer);
                enemy.spell = Enums.Spell.Evicerate;
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #7");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(3, 6);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Archer, Enums.Race.Human, true);
                enemy.SetClass(Enums.Job.Archer);
                enemy.spell = Enums.Spell.Evicerate;
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #8");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(4, 6);
                enemies.Add(enemy);
                break;
        }
        return enemies;
    }
    public String wave14Names(int index)
    {
        switch (index)
        {
            case 0:
                return "The Four Horsemen";
            default:
                return "";
        }
    }
    public List<Piece> wave14Enemies(int index)
    {
        List<Piece> enemies = new List<Piece>();
        Piece enemy;
        switch (index)
        {
            case 0:
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Druid, Enums.Race.Orc, false);
                enemy.SetMaximumHitPoints(400);
                enemy.SetCurrentHitPoints(enemy.GetMaximumHitPoints());
                enemy.SetAttackDamage(40);
                enemy.SetArmourPercentage(100);
                enemy.SetIsEnemy(true);
                enemy.SetName("Horseman of Essence");
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(2);
                enemy.startingSpot = new Tuple<int, int>(0, 4);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(0, Enums.Job.Druid, Enums.Race.Orc, false);
                enemy.SetMaximumHitPoints(4000);
                enemy.SetCurrentHitPoints(enemy.GetMaximumHitPoints());
                enemy.SetAttackDamage(40);
                enemy.SetArmourPercentage(-900);
                enemy.SetIsEnemy(true);
                enemy.SetName("Horseman of War");
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(2);
                enemy.startingSpot = new Tuple<int, int>(3, 4);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(0, Enums.Job.Druid, Enums.Race.Orc, false);
                enemy.SetMaximumHitPoints(200);
                enemy.SetCurrentHitPoints(enemy.GetMaximumHitPoints());
                enemy.SetLifestealPercentage(500);
                enemy.SetAttackDamage(40);
                enemy.SetIsEnemy(true);
                enemy.SetName("Horseman of Life");
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(2);
                enemy.startingSpot = new Tuple<int, int>(7, 4);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Druid, Enums.Race.Orc, false);
                enemy.SetMaximumHitPoints(400);
                enemy.SetCurrentHitPoints(enemy.GetMaximumHitPoints());
                enemy.SetAttackDamage(40);
                enemy.SetIsEnemy(true);
                enemy.SetName("Horseman of Unity3D");
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(2);
                enemy.startingSpot = new Tuple<int, int>(4, 7);
                enemies.Add(enemy);
                //enemy.interactions.Add(new horsemanInvulnerabilityEffect(enemies));
                break;
        }
        return enemies;
    }
    public String wave15Names(int index)
    {
        switch (index)
        {
            case 0:
                return "Demon Invasion (unimplemented)";
            default:
                return "";
        }
    }
    public List<Piece> wave15Enemies(int index)
    {
        List<Piece> enemies = new List<Piece>();
        Piece enemy;
        switch (index)
        {
            case 0:
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Rogue, Enums.Race.Undead, false);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #1");
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(0, 7);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Rogue, Enums.Race.Undead, false);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #2");
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(1, 4);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(0, Enums.Job.Rogue, Enums.Race.Undead, false);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #3");
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(2, 5);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(0, Enums.Job.Rogue, Enums.Race.Undead, false);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #4");
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(3, 7);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Rogue, Enums.Race.Undead, false);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #5");
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(5, 5);
                enemies.Add(enemy);
                enemy = characterGenerator.GenerateCharacter(1, Enums.Job.Rogue, Enums.Race.Undead, false);
                enemy.SetIsEnemy(true);
                enemy.SetName("Enemy #6");
                enemy.SetTitle("");
                enemy.SetDamageIfSurvive(1);
                enemy.startingSpot = new Tuple<int, int>(6, 6);
                enemies.Add(enemy);
                break;
        }
        return enemies;
    }
}