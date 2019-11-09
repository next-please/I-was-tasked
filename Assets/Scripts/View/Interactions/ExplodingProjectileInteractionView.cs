using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingProjectileInteractionView : InteractionView
{
    public GameObject Explosion;

    public override void CleanUpInteraction()
    {
        if (interaction.ticksRemaining <= 0)
           Instantiate(Explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
