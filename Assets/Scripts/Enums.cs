using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Fireblast,
        ForestSpirits,
        FrostArmour,
        GreaterHeal,
        MagicMissile,
        MarkForDeath,
        Moonfire,
        ProtectAlly,
        Rampage,
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

    public static string[] JobSynergyDescription = new string[]
    {
        "Druids become immobile and gain range",
        "Knights returns damage to opponent",
        "Mages cast each spell twice, and start with extra mana",
        "A random ally is buffed",
        "Rogues gain massive damage but lose massive health"
    };

    public static string[] RaceSynergyDescription = new string[]
    {
        "Humans gain extra gold",
        "Elves gain range",
        "Orcs gain health",
        "Undead gain lifesteal"
    };
}
