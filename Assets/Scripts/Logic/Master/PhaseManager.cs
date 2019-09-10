using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

using Com.Nextplease.IWT;

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
    public RequestHandler requestHandler;

    Phase currentPhase = Phase.NIL;
    private int round = 0;
    private int RoundsNeededToSurvive = 15;
    private float countdown = 0;
    private int simulationPlayerCount = 0;

    private int numPlayers = 1;
    private bool phasesRunning = false;

    public void StartPhases(int numPlayers = 1)
    {
        this.numPlayers = numPlayers;
        if (phasesRunning) return;
        phasesRunning = true;
        round = 0;
        TryIntialize();
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

    public void SimulationEnded(Player player, List<Piece> piecesOnBoard)
    {
        simulationPlayerCount++;
        Assert.IsTrue(currentPhase == Phase.Combat);
        marketManager.CalculateAndApplyDamageToCastle(piecesOnBoard);
        if (marketManager.GetCastleHealth() < 0)
        {
            OnGameOver();
        }
        else if (simulationPlayerCount == numPlayers)
        {
            simulationPlayerCount = 0;
            TryPostCombat();
        }
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
    public void TryIntialize()
    {
        Data data = new PhaseManagementData(this.numPlayers, 0);
        Request req = new Request(10, data); // TODO: replace with proper codes
        requestHandler.SendRequest(req);
    }

    public void SetNumPlayers(int numPlayers)
    {
        this.numPlayers = numPlayers;
    }

    public void Initialize(int numPlayers) {
        this.numPlayers = numPlayers;
        ChangePhase(Phase.Initialization);
        boardManager.CreateBoards(numPlayers);
        inventoryManager.ResetInventories();
        StartCoroutine(RoundStartToMarket());
    }

    IEnumerator RoundStartToMarket()
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
        TryMarketPhase();
    }

    void TryMarketPhase()
    {
        Data data = new PhaseManagementData(this.numPlayers, round);
        Request req = new Request(11, data); // TODO: replace with proper codes
        requestHandler.SendRequest(req);
    }

    public void StartMarketPhase()
    {
        StartCoroutine(MarketToPreCombat());
    }

    IEnumerator MarketToPreCombat()
    {
        ChangePhase(Phase.Market);
        boardManager.ResetBoards(numPlayers);
        incomeManager.GenerateIncome(round);
        marketManager.GenerateMarketItems();
        yield return Countdown(5);
        TryPreCombat();
    }

    void TryPreCombat()
    {
        Data data = new PhaseManagementData(this.numPlayers, round);
        Request req = new Request(12, data); // TODO: replace with proper codes
        requestHandler.SendRequest(req);
    }

    public void StartPreCombat()
    {
        StartCoroutine(PreCombatToCombat());
    }

    IEnumerator PreCombatToCombat()
    {
        ChangePhase(Phase.PreCombat);
        summonManager.GenerateAndSummonEnemies(round, numPlayers);
        summonManager.RemoveExcessPlayerPieces(numPlayers);
        yield return Countdown(2);
        Combat();
    }

    void Combat()
    {
        ChangePhase(Phase.Combat);
        CurrentTimeText.text = "Combat In Progress";
        simulationPlayerCount = 0;
        boardManager.StartSim(numPlayers);
    }

    void TryPostCombat()
    {
        Data data = new PhaseManagementData(this.numPlayers, round);
        Request req = new Request(13, data); // TODO: replace with proper codes
        requestHandler.SendRequest(req);
    }

    public void StartPostCombat()
    {
        StartCoroutine(PostCombatToMarket());
    }

    IEnumerator PostCombatToMarket()
    {
        ChangePhase(Phase.PostCombat);
        yield return Countdown(5);
        TryMarketPhase();
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