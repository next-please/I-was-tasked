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
        "Druids becomes copies of the strongest Druid",
        "Knights returns damage to opponent",
        "Mages starts with Mana",
        "A random ally is buffed",
        "A random enemy is damaged"
    };

    public static string[] RaceSynergyDescription = new string[]
    {
        "Humans gain a random racial synergy",
        "Elves gain range",
        "Orcs gain health",
        "Undead gain lifesteal"
    };
}
