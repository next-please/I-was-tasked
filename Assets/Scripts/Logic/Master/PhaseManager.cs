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
    public int NumPlayers = 1;
    public Text CurrentPhaseText;
    public Text CurrentTimeText;
    public Text CurrentRoundText;
    public Canvas WinScreen;

    public IncomeManager incomeManager;
    public BoardManager boardManager;
    public MarketManager marketManager;
    public InventoryManager inventoryManager;
    public SummonManager summonManager;
    public SynergyManager synergyManager;

    Phase currentPhase = Phase.NIL;
    private int round = 0;
    private int RoundsNeededToSurvive = 15;
    private float countdown = 0;
    private int simulationPlayerCount = 0;

    void Start()
    {
        round = 0;
        Initialize();
        StartCoroutine(MarketToCombat());
    }

    void OnGameOver()
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
            OnGameOver();
            yield break;
        }
        CurrentRoundText.text = "Round " + round;
        yield return MarketPhase();
        yield return PreCombat();
        Combat();
    }

    public void SimulationEnded(Player player, List<Piece> piecesOnBoard)
    {
        simulationPlayerCount++;
        Assert.IsTrue(currentPhase == Phase.Combat);
        marketManager.CalculateAndApplyDamageToCastle(piecesOnBoard);
        if (marketManager.GetCastleHealth() < 0)
        {
            OnGameOver();
        }
        else if (simulationPlayerCount == NumPlayers)
        {
            simulationPlayerCount = 0;
            StartCoroutine(PostCombatToCombat());
        }
    }

    IEnumerator PostCombatToCombat()
    {
        yield return PostCombat();
        yield return MarketToCombat();
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
        boardManager.CreateBoards(NumPlayers);
        inventoryManager.ResetInventories();
    }

    IEnumerator MarketPhase()
    {
        ChangePhase(Phase.Market);
        boardManager.ResetBoards(NumPlayers);
        incomeManager.GenerateIncome(round);
        marketManager.GenerateMarketItems();
        yield return Countdown(5);
    }

    IEnumerator PreCombat()
    {
        ChangePhase(Phase.PreCombat);
        summonManager.GenerateAndSummonEnemies(round, NumPlayers);
        summonManager.RemoveExcessPlayerPieces(NumPlayers);
        synergyManager.ApplySynergiesToArmies(NumPlayers);
        yield return Countdown(2);
    }

    void Combat()
    {
        ChangePhase(Phase.Combat);
        CurrentTimeText.text = "Combat In Progress";
        boardManager.StartSim(NumPlayers);
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
