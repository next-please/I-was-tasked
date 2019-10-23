using System.Collections;
using UnityEngine;

using Com.Nextplease.IWT;

public class RoundSummaryUIManager : MonoBehaviour
{
    public Canvas roundSummaryCanvas;
    public Animator roundSummaryAnimator;
    public Animator[] playerBadgeAnimators;

    private int playerNum;

    void Awake()
    {
        roundSummaryCanvas.enabled = false;
        playerNum = (int)RoomManager.GetLocalPlayer();
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
        yield return new WaitForSeconds(1f);

        roundSummaryCanvas.enabled = true;
        roundSummaryAnimator.Play("Enter");

        // play win/lose
        for (int i = 0; i < 3; i++)
        {
            if (PhaseManager.damageResults[i])
            {
                playerBadgeAnimators[i].SetTrigger("Lose");
            }
            else
            {
                playerBadgeAnimators[i].SetTrigger("Win");
            }
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(5f);
        roundSummaryAnimator.Play("Exit");
        yield return new WaitForSeconds(1f);

        // reset state
        roundSummaryCanvas.enabled = false;
        resetBadges();
    }

    private void resetBadges()
    {
        foreach (Animator animator in playerBadgeAnimators)
        {
            animator.Play("Idle");
        }
    }
}
