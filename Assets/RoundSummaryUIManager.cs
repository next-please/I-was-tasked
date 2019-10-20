using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundSummaryUIManager : MonoBehaviour
{
    public Canvas roundSummaryCanvas;
    public Animator animator;

    void Awake()
    {
        roundSummaryCanvas.enabled = false;
    }

    void OnEnable()
    {
        EventManager.Instance.AddListener<EnterPhaseEvent>(OnEnterPhase);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<EnterPhaseEvent>(OnEnterPhase);
    }

    void OnEnterPhase(EnterPhaseEvent e)
    {
        if (e.phase == Phase.PostCombat)
        {
            StartCoroutine(ShowRoundSummary());
        }
    }

    IEnumerator ShowRoundSummary()
    {
        roundSummaryCanvas.enabled = true;
        animator.Play("Enter");
        yield return new WaitForSeconds(3f);
        animator.Play("Exit");
        yield return new WaitForSeconds(1f);
        roundSummaryCanvas.enabled = false;
    }
}
