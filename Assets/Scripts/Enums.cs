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
        "Druids gain infinite range",
        string.Format("({0}) Knights taunt enemies away from allies \n\n({1}) Knights are also invulnerable for a short duration when battle begins", GameLogicManager.Inst.Data.Synergy.KnightRequirement1, GameLogicManager.Inst.Data.Synergy.KnightRequirement2),
        string.Format("({0}) Mages start with extra mana\n\n({1}) Mages also cast each spell twice", GameLogicManager.Inst.Data.Synergy.MageRequirement1, GameLogicManager.Inst.Data.Synergy.MageRequirement2),
        string.Format("({0}) Priests purge surrounding area on death \n\n({1}) Priest's area of purge is larger", GameLogicManager.Inst.Data.Synergy.PriestRequirement1, GameLogicManager.Inst.Data.Synergy.PriestRequirement2),
        string.Format("(<b>Exactly</b> {0} or {1}) Rogues have massively amplified damage but severly reduced health at the start of battle", GameLogicManager.Inst.Data.Synergy.RogueRequirement1, GameLogicManager.Inst.Data.Synergy.RogueRequirement2)
    };

    public static string[] RaceSynergyDescription = new string[]
    {
        string.Format("({0}) All players gain {1} extra gold per turn \n\n({2}) All player gain {3} extra gold per turn", GameLogicManager.Inst.Data.Synergy.HumanRequirement1, GameLogicManager.Inst.Data.Synergy.HumanGoldAmount1, GameLogicManager.Inst.Data.Synergy.HumanRequirement2, GameLogicManager.Inst.Data.Synergy.HumanGoldAmount2),
        "Elves transfer some of their attributes to another elf upon death",
        string.Format("({0}) Orcs rampage after taking significant damage, gaining attack speed \n\n({1}) Orcs begin their rampage after taking lesser damage", GameLogicManager.Inst.Data.Synergy.OrcRequirement1, GameLogicManager.Inst.Data.Synergy.OrcRequirement2),
        string.Format("Undead gain <b>Immortality</b> for {0} seconds after the battle starts\n\n Units with <b>Immortality</b> can lose health but cannot die", GameLogicManager.Inst.Data.Synergy.UndeadTicksToDie / 50)
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
        ForestSpirits,
        NoSkill,
        InfiniteMagicMissile,
        Berserk
    }
}
