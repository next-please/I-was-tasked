using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPrefabLoader : MonoBehaviour
{
    public GameObject ProjectileTestBluePrefab;
    public GameObject ProjectileTestRedPrefab;
    public GameObject FishBallTestPrefab;
    public GameObject ProjectileTestYellowPrefab;
    public GameObject ProjectileTestGreenPrefab;

    private Dictionary<Enums.InteractionPrefab, GameObject> interactionPrefabMap;

    private void Awake()
    {
        interactionPrefabMap = new Dictionary<Enums.InteractionPrefab, GameObject>()
        {
            { Enums.InteractionPrefab.ProjectileTestBlue, ProjectileTestBluePrefab },
            { Enums.InteractionPrefab.ProjectileTestRed, ProjectileTestRedPrefab },
            { Enums.InteractionPrefab.FishBallTest, FishBallTestPrefab },
            { Enums.InteractionPrefab.ProjectileTestYellow, ProjectileTestYellowPrefab}
        };
    }

    public GameObject GetPrefab(Enums.InteractionPrefab interactionPrefab)
    {
        return interactionPrefabMap[interactionPrefab];
    }
}
