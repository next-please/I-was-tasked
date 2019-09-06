using System.Collections;
using System.Collections.Generic;
using System;
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
    public Canvas WinScreen;

    public IncomeManager incomeManager;
    public BoardManager boardManager;
    public MarketManager marketManager;
    public InventoryManager inventoryManager;
    public SummonManager summonManager;

    Phase currentPhase = Phase.NIL;
    int round = 0;
    private int RoundsNeededToSurvive = 15;
    float countdown = 0;

    void OnEnable()
    {
        EventManager.Instance.AddListener<SimulationEndedEvent>(OnSimulationEnd);
        EventManager.Instance.AddListener<GameOverEvent>(OnGameOver);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<SimulationEndedEvent>(OnSimulationEnd);
        EventManager.Instance.RemoveListener<GameOverEvent>(OnGameOver);
    }

    void Start()
    {
        round = 0;
        Initialize();
        StartCoroutine(MarketToCombat());
    }

    void OnGameOver(GameOverEvent e)
    {
        if (round <= RoundsNeededToSurvive)
        {
            WinScreen.GetComponentInChildren<Text>().text = "You Lose!";
            EventManager.Instance.Raise(new GlobalMessageEvent { message = "Game is over, you lost!" });
        }
        else
        {
            WinScreen.GetComponentInChildren<Text>().text = "You Win!";
            EventManager.Instance.Raise(new GlobalMessageEvent { message = "Game is over, you won!" });
        }
        WinScreen.enabled = true;
    }

    IEnumerator MarketToCombat()
    {
        round++;
        Debug.Log("Rounds remaining: " + (round - RoundsNeededToSurvive));
        EventManager.Instance.Raise(new GlobalMessageEvent { message = "Round " + round + " begins!" });
        if (round > RoundsNeededToSurvive)
        {
            EventManager.Instance.Raise(new GameOverEvent { });
        }
        CurrentRoundText.text = "Round " + round;
        yield return MarketPhase();
        yield return PreCombat();
        Combat();
    }

    IEnumerator PostCombatToCombat()
    {
        yield return PostCombat();
        yield return MarketToCombat();
    }

    void OnSimulationEnd(SimulationEndedEvent e)
    {
        Assert.IsTrue(currentPhase == Phase.Combat);
        StartCoroutine(PostCombatToCombat());
    }

    IEnumerator Countdown(int time)
    {
        while (time > 0)
        {
            CurrentTimeText.text = time.ToString();
            yield return new WaitForSecondsRealtime(1);
            time -= 1;
        }
    }

    // PHASES
    void Initialize()
    {
        ChangePhase(Phase.Initialization);
        boardManager.CreateBoards();
        inventoryManager.ResetInventories();
    }

    IEnumerator MarketPhase()
    {
        ChangePhase(Phase.Market);
        boardManager.ResetBoards();
        incomeManager.GenerateIncome(round);
        marketManager.GenerateMarketItems();
        yield return Countdown(5);
    }

    IEnumerator PreCombat()
    {
        ChangePhase(Phase.PreCombat);
        summonManager.GenerateAndSummonEnemies(round);
        yield return Countdown(2);
    }

    void Combat()
    {
        ChangePhase(Phase.Combat);
        CurrentTimeText.text = "Combat In Progress";
        boardManager.StartSim();
    }

    IEnumerator PostCombat()
    {
        ChangePhase(Phase.PostCombat);
        yield return Countdown(5);
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