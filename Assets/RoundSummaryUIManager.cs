using System.Collections;
using UnityEngine;

using Com.Nextplease.IWT;

public class RoundSummaryUIManager : MonoBehaviour
{
    public Canvas roundSummaryCanvas;
    public Animator roundSummaryAnimator;
    public Animator[] playerBadgeAnimators;

    private int playerNum;
    private bool isLose;

    void Awake()
    {
        roundSummaryCanvas.enabled = false;
        playerNum = (int)RoomManager.GetLocalPlayer();
    }

    void OnEnable()
    {
        EventManager.Instance.AddListener<EnterPhaseEvent>(OnEnterPhase);
        EventManager.Instance.AddListener<DamageTakenEvent>(OnDamageTaken);
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

    void OnDamageTaken(DamageTakenEvent e)
    {
        isLose = true;
    }

    IEnumerator ShowRoundSummary()
    {
        yield return new WaitForSeconds(1f);

        roundSummaryCanvas.enabled = true;
        roundSummaryAnimator.Play("Enter");

        // play win/lose
        yield return new WaitForSeconds(playerNum * 0.3f);

        foreach (Animator animator in playerBadgeAnimators)
        {
            // fix this
        }


        if (isLose)
        {
            playerBadgeAnimators[playerNum].SetTrigger("Lose");
        }
        else
        {
            playerBadgeAnimators[playerNum].SetTrigger("Win");
        }
        isLose = false;

        yield return new WaitForSeconds(5f);
        roundSummaryAnimator.Play("Exit");
        yield return new WaitForSeconds(1f);

        // reset state
        roundSummaryCanvas.enabled = false;
        playerBadgeAnimators[playerNum].Play("Idle");
    }
}
