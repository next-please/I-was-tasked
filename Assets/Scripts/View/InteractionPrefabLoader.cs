using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPrefabLoader : MonoBehaviour
{
    public GameObject ProjectileTestBluePrefab;
    public GameObject ProjectileTestRedPrefab;

    private Dictionary<Enums.InteractionPrefab, GameObject> interactionPrefabMap;

    private void Awake()
    {
        interactionPrefabMap = new Dictionary<Enums.InteractionPrefab, GameObject>()
        {
            { Enums.InteractionPrefab.ProjectileTestBlue, ProjectileTestBluePrefab },
            { Enums.InteractionPrefab.ProjectileTestRed, ProjectileTestRedPrefab }
        };
    }

    public GameObject GetPrefab(Enums.InteractionPrefab interactionPrefab)
    {
        return interactionPrefabMap[interactionPrefab];
    }
}
