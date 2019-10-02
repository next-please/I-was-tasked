﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPrefabLoader : MonoBehaviour
{
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

    private Dictionary<Enums.InteractionPrefab, GameObject> interactionPrefabMap;

    private void Awake()
    {
        interactionPrefabMap = new Dictionary<Enums.InteractionPrefab, GameObject>()
        {
            { Enums.InteractionPrefab.ProjectileTestBlue, ProjectileTestBluePrefab },
            { Enums.InteractionPrefab.ProjectileTestRed, ProjectileTestRedPrefab },
            { Enums.InteractionPrefab.FishBallTest, FishBallTestPrefab },
            { Enums.InteractionPrefab.ProjectileTestYellow, ProjectileTestYellowPrefab},
            { Enums.InteractionPrefab.ProjectileTestGreen, ProjectileTestGreenPrefab},
            { Enums.InteractionPrefab.ProjectileTestBloodRed, ProjectileTestBloodRedPrefab},
            { Enums.InteractionPrefab.ProjectileTestBlack, ProjectileTestBlackPrefab},
            { Enums.InteractionPrefab.ProjectileTestLightBlue, ProjectileTestLightBluePrefab},
            { Enums.InteractionPrefab.ProjectileTestFireRed, ProjectileTestFireRedPrefab},
            { Enums.InteractionPrefab.ProjectileTestArcanePurple, ProjectileTestArcanePurplePrefab},
            { Enums.InteractionPrefab.ProjectileTestSicklyGreen, ProjectileTestSicklyGreenPrefab},
            { Enums.InteractionPrefab.CylinderTestSicklyGreen, CylinderTestSicklyGreenPrefab}
        };
    }

    public GameObject GetPrefab(Enums.InteractionPrefab interactionPrefab)
    {
        return interactionPrefabMap[interactionPrefab];
    }
}
