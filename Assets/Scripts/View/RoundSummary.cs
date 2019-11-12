using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundSummary : MonoBehaviour
{
    public RoundSummaryUIManager roundSummaryUIManager;

    public void PlayEndWaveSound()
    {
        roundSummaryUIManager.PlayEndWaveSound();
    }
}
