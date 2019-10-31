using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using TMPro;

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
    public static bool[] damageResults;

    public TextMeshProUGUI CurrentPhaseText;
    public TextMeshProUGUI CurrentTimeText;
    public TextMeshProUGUI CurrentRoundText;
    public Image SwordImage;
    public Canvas PopUpScreen;
    public Canvas GameOverScreen;

    public Canvas tutorialPopUp;

    public IncomeManager incomeManager;
    public BoardManager boardManager;
    public MarketManager marketManager;
    public InventoryManager inventoryManager;
    public SummonManager summonManager;

    public RequestHandler requestHandler;
    public RoomManager roomManager;
    public SynergyManager synergyManager;

    public const int marketDuration = 20;
    public const int preCombatDuration = 10;

    static Phase currentPhase = Phase.NIL;

    private int round = 0;
    public int randomRoundIndex = 0;
    private int RoundsNeededToSurvive = 15;
    private float countdown = 0;
    private int simulationPlayerCount = 0;

    private int numPlayers = 1;
    private bool phasesRunning = false;


    private HashSet<string> playerReadySet = new HashSet<string>();

    void Awake()
    {
        SwordImage.enabled = false;
        damageResults = new bool[3];
    }

    public void StartPhases(int numPlayers = 1)
    {
        this.numPlayers = numPlayers;
        if (phasesRunning) return;
        phasesRunning = true;
        roomManager.NumPlayersToStart = numPlayers;
        TryIntialize();
    }

    public static Phase GetCurrentPhase()
    {
        return currentPhase;
    }

    void OnGameOver()
    {
        StartCoroutine(PlayGameOverScreen());
    }

    IEnumerator PlayGameOverScreen()
    {
        if (round <= RoundsNeededToSurvive)
        {
            GameOverScreen.GetComponent<Animator>().Play("Lose");
        }
        else
        {
            GameOverScreen.GetComponent<Animator>().Play("Win");
        }
        yield return null;
    }

    public void SimulationEnded(Player player, List<Piece> piecesOnBoard)
    {
        simulationPlayerCount++;
        Assert.IsTrue(currentPhase == Phase.Combat);
        damageResults[(int)player] = marketManager.CalculateAndApplyDamageToCastle(piecesOnBoard);
        if (marketManager.GetCastleHealth() <= 0)
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
        SwordImage.enabled = false;
        while (time > 0)
        {
            CurrentTimeText.text = time.ToString();
            yield return new WaitForSecondsRealtime(1);
            time -= 1;
            if (time == 20 && round == 1)
            {
                tutorialPopUp.enabled = false;
            }
        }
    }

    public void SetNumPlayers(int numPlayers)
    {
        this.numPlayers = numPlayers;
    }

    // PHASES
    public void TryIntialize()
    {
        System.Random random = new System.Random();
        int[] seeds = new int[1 + numPlayers]; // For Synergy Manager and the numPlayers (3) Simulators.
        for (int i = 0; i < seeds.Length; i++)
        {
            seeds[i] = random.Next();
        }
        Data data = new InitPhaseData(numPlayers, seeds);
        Request req = new Request(ActionTypes.INIT_PHASE, data); // TODO: replace with proper codes
        requestHandler.SendRequest(req);
    }

    public void Initialize(int numPlayers, int[] seeds)
    {
        this.round = 0;
        this.numPlayers = numPlayers;
        ChangePhase(Phase.Initialization);
        synergyManager.SetSeed(seeds[0]);
        boardManager.CreateBoards(seeds, numPlayers);
        inventoryManager.ResetInventories();
        TryStartRound();
    }

    void TryStartRound()
    {
        Data data = new PhaseManagementData(this.numPlayers, round);
        Request req = new Request(ActionTypes.ROUND_START, data); // TODO: replace with proper codes
        requestHandler.SendRequest(req);
    }

    public void StartRound()
    {
        StartCoroutine(StartRoundToMarket());
    }

    IEnumerator StartRoundToMarket()
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
        summonManager.RemoveAllEnemyPieces(numPlayers);
        TryMarketPhase();
    }

    void TryMarketPhase()
    {
        List<Piece> newMarketPieces = marketManager.GenerateMarketItems();
        Data data = new MarketManagementData(this.numPlayers, round, newMarketPieces);
        Request req = new Request(ActionTypes.MARKET_PHASE, data); // TODO: replace with proper codes
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
        incomeManager.GenerateIncome();
        if (round == 1)
        {
            yield return Countdown(marketDuration + 10);
        }
        else
        {
            yield return Countdown(marketDuration);
        }
        TryPreCombat();
    }

    void TryPreCombat()
    {
        randomRoundIndex = summonManager.GenerateRandomIndex(round);
        var enemies = summonManager.GenerateEnemies(round, randomRoundIndex, numPlayers);
        Data data = new PreCombatData(enemies, randomRoundIndex);
        Request req = new Request(ActionTypes.PRECOMBAT_PHASE, data); // TODO: replace with proper codes
        requestHandler.SendRequest(req);
    }

    public void StartPreCombat(List<List<Piece>> enemies)
    {
        Debug.Log(summonManager.GetWaveName(round, randomRoundIndex));
        StartCoroutine(ShowRoundPopUpScreen());
        StartCoroutine(PreCombatToCombat(enemies));
    }

    IEnumerator ShowRoundPopUpScreen()
    {
        PopUpScreen.GetComponentInChildren<TextMeshProUGUI>().text = "Round " + round + ":\n" + summonManager.GetWaveName(round, randomRoundIndex);
        PopUpScreen.enabled = true;
        yield return new WaitForSeconds(2f);
        PopUpScreen.enabled = false;
    }

    IEnumerator PreCombatToCombat(List<List<Piece>> enemies)
    {
        ChangePhase(Phase.PreCombat);
        summonManager.SummonEnemies(enemies);
        yield return Countdown(preCombatDuration);
        Combat();
    }

    void Combat()
    {
        ChangePhase(Phase.Combat);
        summonManager.RemoveExcessPlayerPieces(numPlayers);
        synergyManager.ApplySynergiesToArmies(numPlayers);
        CurrentTimeText.text = "";
        SwordImage.enabled = true;
        simulationPlayerCount = 0;
        boardManager.StartSim(numPlayers);
    }

    void TryPostCombat()
    {
        Data data = new PhaseManagementData(this.numPlayers, round);
        Request req = new Request(ActionTypes.POSTCOMBAT_PHASE, data); // TODO: replace with proper codes
        requestHandler.SendRequest(req);
    }

    public void SetPlayerReadyForPostCombat(string playerID)
    {
        playerReadySet.Add(playerID);
    }

    public bool PlayersReadyForPostCombat()
    {
        return playerReadySet.Count == roomManager.NumPlayersToStart;
    }

    public void ClearPlayerReadySet()
    {
        playerReadySet.Clear();
    }

    public void StartPostCombat()
    {
        StartCoroutine(PostCombatToStartRound());
    }

    IEnumerator PostCombatToStartRound()
    {
        ChangePhase(Phase.PostCombat);
        yield return Countdown(7);
        TryStartRound();
    }

    void ChangePhase(Phase phase)
    {
        Debug.Log("Exiting Phase " + currentPhase);
        EventManager.Instance.Raise(new ExitPhaseEvent { phase = currentPhase });
        currentPhase = phase;
        Debug.Log("Entering Phase " + currentPhase);
        EventManager.Instance.Raise(new EnterPhaseEvent { phase = currentPhase, round = this.round });
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
