using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PrefabEnum = Enums.InteractionPrefab;

public class InteractionPrefabLoader : MonoBehaviour
{
    [Header("Skills")]
    public GameObject EviscerateBleed;
    public GameObject Fireblast;
    public GameObject ForestSpirits;
    public GameObject FrostArmour;
    public GameObject GreaterHeal;
    public GameObject MagicMissile;
    public GameObject MarkForDeath;
    public GameObject Moonfire;
    public GameObject Rampage;
    public GameObject Rot;
    public GameObject ShadowStrike;
    public GameObject UnholyAura;

    [Header("Debug SKills")]
    public GameObject ProjectileTestBluePrefab;
    public GameObject ProjectileTestRedPrefab;
    public GameObject FishBallTestPrefab;
    public GameObject ProjectileTestYellowPrefab;
    public GameObject ProjectileTestGreenPrefab;
    public GameObject ProjectileTestBloodRedPrefab;
    public GameObject ProjectileTestBlackPrefab;
    public GameObject ProjectileTestLightBluePrefab;
    public GameObject ProjectileTestFireRedPrefab;
    public GameObject ProjectileTestArcanePurplePrefab;
    public GameObject ProjectileTestSicklyGreenPrefab;
    public GameObject CylinderTestSicklyGreenPrefab;
    public GameObject CylinderTestLightBluePrefab;

    private Dictionary<Enums.InteractionPrefab, GameObject> interactionPrefabMap;

    private void Awake()
    {
        interactionPrefabMap = new Dictionary<Enums.InteractionPrefab, GameObject>()
        {
            { PrefabEnum.EviscerateBleed, EviscerateBleed },
            { PrefabEnum.Fireblast, Fireblast },
            { PrefabEnum.ForestSpirits, ForestSpirits },
            { PrefabEnum.FrostArmour, FrostArmour },
            { PrefabEnum.GreaterHeal, GreaterHeal },
            { PrefabEnum.MagicMissile, MagicMissile },
            { PrefabEnum.MarkForDeath, MarkForDeath },
            { PrefabEnum.Moonfire, Moonfire },
            { PrefabEnum.Rampage, Rampage },
            { PrefabEnum.Rot, Rot },
            { PrefabEnum.ShadowStrike, ShadowStrike },
            { PrefabEnum.UnholyAura, UnholyAura },
            { PrefabEnum.ProjectileTestBlue, ProjectileTestBluePrefab },
            { PrefabEnum.ProjectileTestRed, ProjectileTestRedPrefab },
            { PrefabEnum.FishBallTest, FishBallTestPrefab },
            { PrefabEnum.ProjectileTestYellow, ProjectileTestYellowPrefab},
            { PrefabEnum.ProjectileTestGreen, ProjectileTestGreenPrefab},
            { PrefabEnum.ProjectileTestBloodRed, ProjectileTestBloodRedPrefab},
            { PrefabEnum.ProjectileTestBlack, ProjectileTestBlackPrefab},
            { PrefabEnum.ProjectileTestLightBlue, ProjectileTestLightBluePrefab},
            { PrefabEnum.ProjectileTestFireRed, ProjectileTestFireRedPrefab},
            { PrefabEnum.ProjectileTestArcanePurple, ProjectileTestArcanePurplePrefab},
            { PrefabEnum.ProjectileTestSicklyGreen, ProjectileTestSicklyGreenPrefab},
            { PrefabEnum.CylinderTestSicklyGreen, CylinderTestSicklyGreenPrefab},
            { PrefabEnum.CylinderTestLightBlue, CylinderTestLightBluePrefab}
        };
    }

    public GameObject GetPrefab(Enums.InteractionPrefab interactionPrefab)
    {
        var prefab = interactionPrefabMap[interactionPrefab];
        if (prefab == null)
            Debug.Log(interactionPrefab + " Not Found!");
        return prefab;
    }
}
