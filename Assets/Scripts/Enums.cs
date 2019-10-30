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
        "Knights taunt enemies",
        "Mages cast each spell twice, and start with extra mana",
        "Priests cast divine judgement on death",
        "Rogues gain massive damage but lose massive health"
    };

    public static string[] RaceSynergyDescription = new string[]
    {
        "Humans gain extra gold",
        "Elves guide a friendly unit in death",
        "Orcs rule the world! And they're angry!",
        "Undead gain immortality. Then they die."
    };
}
