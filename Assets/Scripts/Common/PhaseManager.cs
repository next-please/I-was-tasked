using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public enum Phase
{
    NIL,
    Initialization, // Happens once
    Market,
    PreCombat,
    Combat,
    PostCombat,
};

public class PhaseManager : MonoBehaviour
{
    public Text CurrentPhaseText;
    public Text CurrentTimeText;
    public Text CurrentRoundText;

    Phase currentPhase = Phase.NIL;
    int round = 0;
    float countdown = 0;

    void OnEnable()
    {
        EventManager.Instance.AddListener<SimulationEndedEvent>(OnSimulationEnd);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<SimulationEndedEvent>(OnSimulationEnd);
    }

    void Start()
    {
        round = 0;
        ChangePhase(Phase.Initialization);
        StartCoroutine(MarketToCombat());
    }

    IEnumerator MarketToCombat()
    {
        ChangePhase(Phase.Market);
        SetTime(5);
        yield return new WaitForSecondsRealtime(5);
        ChangePhase(Phase.PreCombat);
        SetTime(2);
        yield return new WaitForSecondsRealtime(2);
        ChangePhase(Phase.Combat);
    }

    IEnumerator PostCombatToCombat()
    {
        ChangePhase(Phase.PostCombat);
        SetTime(5);
        yield return new WaitForSecondsRealtime(5);
        yield return MarketToCombat();
    }

    void OnSimulationEnd(SimulationEndedEvent e)
    {
        Assert.IsTrue(currentPhase == Phase.Combat);
        StartCoroutine(PostCombatToCombat());
    }

    void Update()
    {
        // Not the best...
        if (countdown <= 0)
        {
            countdown = 0;
            CurrentTimeText.text = countdown.ToString();
            return;
        }
        countdown -= Time.deltaTime;
        CurrentTimeText.text = countdown.ToString();
    }

    void SetTime(float time)
    {
        countdown = time;
    }

    void ChangePhase(Phase phase)
    {
        Debug.Log("Exiting Phase " + currentPhase);
        EventManager.Instance.Raise(new ExitPhaseEvent { phase = currentPhase });
        currentPhase = phase;
        Debug.Log("Entering Phase " + currentPhase);
        EventManager.Instance.Raise(new EnterPhaseEvent{ phase = currentPhase, round = this.round  });
        CurrentPhaseText.text = currentPhase.ToString();
    }
}

public class EnterPhaseEvent : GameEvent
{
    public Phase phase;
    public int round;
}

public class ExitPhaseEvent : GameEvent
{
    public Phase phase;
}
