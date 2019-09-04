using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Upgrades", order = 1)]
public class Upgrades : ScriptableObject
{
    public int MarketTier = 1; //also used as price for market increase, don't know if here or what
}
