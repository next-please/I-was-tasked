using System.Collections;
using UnityEngine;

using Com.Nextplease.IWT;

public class RoundSummaryUIManager : MonoBehaviour
{
    public Canvas roundSummaryCanvas;
    public SoundManager soundManager;
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
        bool win = true;
        for (int i = 0; i < 3; i++)
        {
            if (PhaseManager.damageResults[i])
            {
                playerBadgeAnimators[i].Play("Lose");
                win = false;
            }
            else
            {
                playerBadgeAnimators[i].Play("Win");
            }
            yield return new WaitForSecondsRealtime(0.3f);
        }
        soundManager.PlayEndWaveSound(win);

        yield return new WaitForSeconds(3.3f);
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
