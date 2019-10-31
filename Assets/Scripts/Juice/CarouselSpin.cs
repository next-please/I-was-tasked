using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarouselSpin : MonoBehaviour
{
    public GameObject[] MarketSlots;
    private Vector3[] marketSlotPositions;
    private int[] toMove;

    void Start()
    {
        marketSlotPositions = new Vector3[MarketSlots.Length];
        toMove = new int[MarketSlots.Length];
        int i = 0;
        foreach (GameObject marketSlot in MarketSlots)
        {
            marketSlotPositions[i] = marketSlot.transform.position;
            toMove[i] = (i + 1 < MarketSlots.Length) ? i + 1 : 0;
            i++;
        }
    }


    void Update()
    {
        for (int i = 0; i < MarketSlots.Length; i++)
        {
            GameObject marketSlot = MarketSlots[i];
            marketSlot.transform.position = Vector3.MoveTowards(marketSlot.transform.position, marketSlotPositions[toMove[i]], 0.125f * Time.deltaTime);

            if (Vector3.Distance(marketSlot.transform.position, marketSlotPositions[toMove[i]]) < 0.001f)
            {
                toMove[i] = (toMove[i] + 1 < MarketSlots.Length) ? toMove[i] + 1 : 0;
            }
        }
    }
}
