public static class Enums
{
    public enum Job
    {
        Druid,
        Knight,
        Mage,
        Priest,
        Rogue
    }
    public enum Race
    {
        Human,
        Elf,
        Orc,
        Undead
    }

    public enum InteractionPrefab
    {
        None,
        BarkSkin,
        BlessingOfNature,
        Charge,
        CheapShot,
        CurseOfAgony,
        EviscerateBleed,
        DivineJudgement,
        Fireblast,
        ForestSpirits,
        FrostArmour,
        GreaterHeal,
        GuidingSpirits,
        MagicMissile,
        MarkForDeath,
        Moonfire,
        ProtectAlly,
        Rampage,
        RampageSynergy,
        Rot,
        ShadowStrike,
        ShapeShift,
        ThunderStorm,
        ThunderStormLightning,
        UnholyAura,
        ProjectileTestBlue,
        ProjectileTestRed,
        FishBallTest,
        ProjectileTestYellow,
        ProjectileTestGreen,
        ProjectileTestBloodRed,
        ProjectileTestBlack,
        ProjectileTestLightBlue,
        ProjectileTestFireRed,
        ProjectileTestArcanePurple,
        ProjectileTestSicklyGreen,
        CylinderTestSicklyGreen,
        CylinderTestLightBlue,
    }

    public static int[][] JobSynergyRequirements = new int[][]
    {
        new int[] { GameLogicManager.Inst.Data.Synergy.DruidRequirement },
        new int[] { GameLogicManager.Inst.Data.Synergy.KnightRequirement1, GameLogicManager.Inst.Data.Synergy.KnightRequirement2 },
        new int[] { GameLogicManager.Inst.Data.Synergy.MageRequirement1, GameLogicManager.Inst.Data.Synergy.MageRequirement2 },
        new int[] { GameLogicManager.Inst.Data.Synergy.PriestRequirement1, GameLogicManager.Inst.Data.Synergy.PriestRequirement2 },
        new int[] { GameLogicManager.Inst.Data.Synergy.RogueRequirement1, GameLogicManager.Inst.Data.Synergy.RogueRequirement2 }
    };

    public static int[][] RaceSynergyRequirements = new int[][]
    {
        new int[] { GameLogicManager.Inst.Data.Synergy.HumanRequirement1, GameLogicManager.Inst.Data.Synergy.HumanRequirement2 },
        new int[] {GameLogicManager.Inst.Data.Synergy.ElfRequirement1,},
        new int[] {GameLogicManager.Inst.Data.Synergy.OrcRequirement1, GameLogicManager.Inst.Data.Synergy.OrcRequirement2},
        new int[] {GameLogicManager.Inst.Data.Synergy.UndeadRequirement1},
    };

    public static string[] JobSynergyDescription = new string[]
    {
        "Druids become immobile and gain range",
        "(4) Knights taunt enemies away from allies \n(11) Knights are invulnerable to damage for a short while when the match begins",
        "(3) Mages start with extra mana\n(8) In addition, Mages cast each spell twice",
        "(4) Priests create a divine explosion on death \n(6) Priests create a bigger divine explosion on death",
        "(<b>Exactly</b> 2) Rogues gain massive damage but lose massive health\n(7) Rogues gain massive damage but lose massive health"
    };

    public static string[] RaceSynergyDescription = new string[]
    {
        "(5) Everyone generates some extra gold per turn \n(10) Everyone generates a lot of extra gold per turn",
        "Elves guide a friendly unit in death",
        "(4) Orcs enrage after taking significant damage \n(9) Orcs enrage after a bit of damage. They also appear more often.",
        "Undead gain immortality. Then they die."
    };

    public enum Spell
    {
        Shapeshift,
        ProtectAlly,
        Barkskin,
        BlessingOfNature,
        Rampage,
        MarkForDeath,
        FrostArmour,
        CurseOfAgony,
        Evicerate,
        Fireblast,
        MagicMissile,
        Rot,
        UnholyAura,
        Thunderstorm,
        Moonfire,
        GreaterHeal,
        CheapShot,
        ShadowStrike,
        Charge,
        ForestSpirits
    }
}
