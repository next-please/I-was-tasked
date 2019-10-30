using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameLogicData", order = 1)]
public class GameLogicData : ScriptableObject
{
    public string Version = "master";
    public PieceBalanceData Piece;
    public SkillData Skills;
    public GeneratorData Gen;
    public SynergyData Synergy;
}

[Serializable]
public class SynergyData
{
    [Header("Job Synergy Requirement")]
    public int DruidRequirement = 4;
    public int KnightRequirement = 5;
    public int MageRequirement = 8;
    public int PriestRequirement = 6;
    public int RogueRequirement = 7;

    [Header("Race Synergy Requirement")]
    public int HumanRequirement = 10;
    public int ElfRequirement = 8;
    public int OrcRequirement = 10;
    public int UndeadRequirement = 9;

    [Header("Synergy Power Variables")]
    public int HumanGoldAmount = 10;
    public double RogueDamageMultiplier = 10;
    public double RogueHitPointMultiplier = 0.1;
    public double MageStartingManaPercentage = 0.25;
    public int UndeadTicksToDie = 500;
    public int PriestRetributionRadius = 2;
    public int PriestRetributionHealing = 100;
    public int PriestRetributionDamage = 75;
    public double OrcRampageHealthThreshold = 0.5;
    public int OrcRampageAttackSpeed = 3;
    public double OrcRampageArmourPercentage = -0.25;
    public int ElfGuidingSpiritAttackDamage = 10;
    public int ElfGuidingSpiritAttackSpeed = 1;
    public int GuidingSpiritLingerTicks = 50;
}

[Serializable]
public class PieceBalanceData
{
    [Header("Mana Point Logic")]
    public int ManaPointsGainedOnAttack = 10;
    public int ManaPointsGainedOnHit = 4;
}

[Serializable]
public class GeneratorData
{
    [Header("HP/MP")]
    public int DefaultHitPoints = 150;
    public int DefaultManaPoints = 100;
    public double HitPointMultiplier = 1.5;

    [Header("Attack Damage")]
    public int DefaultAttackDamage = 10;
    public double AttackDamageMultiplier = 1.5;

    [Header("Attack Range")]
    public int DefaultAttackRange = 1;

    [Header("Attack Speed")]
    public int DefaultAttackSpeed = 5;
    public int MinAttackSpeed = 1;
    public int MaxAttackSpeed = 10;

    [Header("Movement Speed")]
    public int DefaulMovementSpeed = 5;
    public int MinMovementSpeed = 1;
    public int MaxMovementSpeed = 10;

    [Header("Human")]
    public double HumanHitPointMultiplier = 1.1;
    public double HumanAttackDamageMultiplier = 1.1;
    public int HumanManaPointAdditor = -5;

    [Header("Orc")]
    public int OrcFlatMovementSpeedAdditor = -1;
    public double OrcHitPointMultipler = 1.25;
    public double OrcAttackDamageMultiplier = 1.25;
    public int OrcFlatAttackSpeedAdditor = -1;

    [Header("Elf")]
    public int ElfFlatMovementSpeedAdditor = 1;
    public int ElfFlatAttackSpeedAdditor = 1;
    public int ElfFlatConditionalAttackRangeAdditor = 1;
    public double ElfAttackDamageMultiplier = 1.05;
    public int ElfManaPointAdditor = -8;

    [Header("Undead")]
    public int UndeadFlatMovementSpeedAdditor = -3;
    public int UndeadFlatAttackSpeedAdditor = -2;
    public double UndeadHitPointMultiplier = 1.35;

    [Header("Mage")]
    public int MageFlatAttackRangeAdditor = 2;
    public int MageManaPointAdditor = -8;
    public double MageAttackDamageMultiplier = 1.05;

    [Header("Rogue")]
    public int RogueFlatMovementSpeedAdditor = 1;
    public int RogueFlatAttackSpeedAdditor = 1;
    public double RogueHitPointMultiplier = 1.05;
    public double RogueAttackDamageMultiplier = 1.1;

    [Header("Druid")]
    public double DruidHitPointMultiplier = 1.15;
    public double DruidAttackDamageMultiplier = 1.15;
    public int DruidManaPointAdditor = -3;

    [Header("Knight")]
    public double KnightHitPointMultiplier = 1.2;
    public double KnightAttackDamageMultiplier = 1.15;

    [Header("Priest")]
    public int PriestFlatAttackRangeAdditor = 4;
    public int PriestManaPointAdditor = -5;
    public double PriestHitPointMultiplier = 1.1;

    [Header("Human Priest")]
    public double HumanPriestManaMultiplier = 1.3;
}

[Serializable]
public class SkillData
{
    [Header("Bark Skin")]
    public int BarkSkinBlockAmount = 6;
    public int BarkSkinLingerTicks = 250;

    [Header("Berserk")]
    public double BerserkMultiplerIncrease = 2;

    [Header("Blessing Of Nature")]
    public double BlessingOfNatureMultiplierIncrease = 0.6;
    public int BlessingOfNatureLingerTicks = 250;

    [Header("Charge")]
    public double ChargeStackingPercentIncrease = 1.2;
    public double ChargeBaseDamage = 40;

    [Header("Cheap Shot")]
    public int CheapShotStunTicks = 250;
    public int CheapShotLingerTicks = 250;

    [Header("Curse of Agony")]
    public int CurseOfAgonyCurseAmount = 20;
    public int CurseOfAgonyLingerTicks = 250;

    [Header("Eviscerate")]
    public int EviscerateInitialDamage = 5;
    public int EviscerateBleedCount = 10;
    public int EviscerateBleedDamage = 1;
    public int EviscerateLingerTicks = 30;

    [Header("Fireblast")]
    public int FireblastDamage = 120;
    public int FireblastTicks = 120;

    [Header("Forest Spirits")]
    public int ForestSpiritsInitialTicks = 120;
    public int ForestSpiritsSubsequentTicks = 40;
    public int ForestSpiritsCount = 5;
    public int ForestSpiritsSecondaryTicks = 70;
    public int ForestSpiritsHeal = 30;

    [Header("Frost Armour")]
    public double FrostArmourPercentage = 0.3;
    public int FrostArmourLingerTicks = 250;

    [Header("Greater Heal")]
    public int GreaterHealInitialTicks = 100;
    public int GreaterHealAmount = 120;
    public int GreaterHealLingerTicks = 200; //NOTE: alert this is purely visual @Rinder 5

    [Header("Magic Missile")]
    public int MagicMissileCount = 3;
    public int MagicMissileDamage = 35;
    public int MagicMissileTicks = 30;

    [Header("Mark For Death")]
    public int MarkForDeathTicks = 300;

    [Header("Moonfire")]
    public int MoonfireInitialTicks = 20;
    public double MoonfireManaRetainPercentage = 0.5;
    public int MoonfireDefaultDamage = 58;
    public int MoonfireLingerTicks = 20;

    [Header("Protect Ally")]
    public int ProtectAllyLingerTicks = 100;

    [Header("Rampage")]
    public int RampageAttackSpeed = 3;
    public double RampageArmourPercentage  = -0.25;
    public int RampageLingerTicks  = 250;

    [Header("Rot")]
    public int RotTickPerCount = 20;
    public int RotRadius = 1;
    public int RotCount = 20;
    public int RotDamage = 6;

    [Header("Shadow Strike")]
    public int ShadowStrikeDamage = 90;
    public int ShadowStrikeInitialTick = 40;

    [Header("Shape Shift")]
    public double ShapeShiftMultiplierIncrease = 1.2;
    public int ShapeShiftDefaultAttackSpeedIncrease = 1;
    public int ShapeShiftLingerTicks = 200;

    [Header("Thunderstorm")]
    public int ThunderStormInitialTick = 25;
    public int ThunderStormSubsequentTick = 50;
    public int ThunderStormRadius = 1;
    public int ThunderStormCount = 5;
    public int ThunderStormBoltDamage = 28;

    [Header("Unholy Aura")]
    public int UnholyAuraTickPerCount = 25;
    public int UnholyAuraRadius = 4;
    public int UnholyAuraCount = 15;
    public int UnholyAuraDamage = 6;
}
