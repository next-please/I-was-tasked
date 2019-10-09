using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampageInteractionView : InteractionView
{
    // Start is called before the first frame update
    public GameObject RampageParticlesEyes;
    public GameObject RampageParticlesHands;

    private List<GameObject> spawnedObjects;

    void Start()
    {
        spawnedObjects = new List<GameObject>();
        var rampage = interaction as RampageLingeringEffect;
        var pieceView = rampage.caster.GetPieceView();
        var warriorView = pieceView.GetComponentInChildren<WarriorView>();

        var rightEye = Instantiate(RampageParticlesEyes);
        var RightEyeTransform = warriorView.RightEye;
        SetAsChild(rightEye.transform,  RightEyeTransform);
        spawnedObjects.Add(rightEye);

        var leftEye = Instantiate(RampageParticlesEyes);
        var LeftEyeTransform = warriorView.LeftEye;
        SetAsChild(leftEye.transform, LeftEyeTransform);
        spawnedObjects.Add(leftEye);

        var rightHand = Instantiate(RampageParticlesHands);
        var RightHandTransform = warriorView.RightHand;
        SetAsChild(rightHand.transform, RightHandTransform);
        spawnedObjects.Add(rightHand);
    }

    void SetAsChild(Transform particle, Transform parent)
    {
        particle.transform.position = parent.transform.position;
        particle.parent = parent;
    }

    public override void CleanUpInteraction()
    {
        foreach (var o in spawnedObjects)
        {
            Destroy(o);
        }
    }
}
