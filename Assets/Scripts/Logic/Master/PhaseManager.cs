using Com.Nextplease.IWT;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public enum Phase
{
    NIL,
    Initialization, // Happens once
    Market,
    PreCombat,
    Combat,
    PostCombat,
    Tutorial
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

    public readonly int marketDuration = GameLogicManager.Inst.Data.MarketDuration;
    public readonly int preCombatDuration = GameLogicManager.Inst.Data.PreCombatDuration;
    public readonly int combatDuration = GameLogicManager.Inst.Data.CombatDuration;

    static Phase currentPhase = Phase.NIL;

    private int round = 0;
    public int randomRoundIndex = 0;
    private int RoundsNeededToSurvive = 15;
    private int simulationPlayerCount = 0;

    private int numPlayers = 1;
    private bool phasesRunning = false;

    // TUTORIAL
    private int _tutorialRounds = 2;

    private HashSet<string> playerReadySet = new HashSet<string>();

    void Awake()
    {
        SwordImage.enabled = false;
        damageResults = new bool[3];
        CurrentPhaseText.text = "Loading...";
    }

    private void Start()
    {
        EventManager.Instance.AddListener<AddPieceToBoardEvent>(OnPieceAdded);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener<AddPieceToBoardEvent>(OnPieceAdded);
    }

    public bool IsMovablePhase()
    {
        return currentPhase == Phase.Market || currentPhase == Phase.PreCombat;
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

    private void OnTutorialOver()
    {
        phasesRunning = false;
        boardManager.RemoveAllPieces();
        EventManager.Instance.RemoveListener<AddPieceToBoardEvent>(OnPieceAdded);
        // StopAllCoroutines();
        StartCoroutine(TransitionToFullGame());
    }

    IEnumerator TransitionToFullGame()
    {
        round = 0;
        marketManager.CalculateAndApplyDamageToCastle(MarketManager.StartingCastleHealth);
        EventManager.Instance.Raise(new DamageTakenEvent { currentHealth = marketManager.market.CastleHealth });
        CurrentRoundText.text = "Transition";
        for (int i = 0; i < numPlayers; ++i)
            incomeManager.SetIncomeGeneratedByPlayer((Player)i, 0);
        yield return Countdown(10);
        roomManager.SetFullGameMode();
        inventoryManager.ResetInventories();
        TryStartRound();
    }

    void OnGameOver()
    {
        StartCoroutine(PlayGameOverScreen());
    }

    IEnumerator PlayGameOverScreen()
    {
        bool win = round > RoundsNeededToSurvive;
        if (win)
        {
            GameOverScreen.GetComponent<Animator>().Play("Win");
        }
        else
        {
            GameOverScreen.GetComponent<Animator>().Play("Lose");
        }
        SoundManager.Instance.PlayEndGameSound(win);
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

            // Play sounds at the correct time.
            if (GetCurrentPhase() == Phase.PreCombat)
            {
                if (time == 4)
                {
                    SoundManager.Instance.PlayRoundPreStartSound();
                }
            }

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
        Request req = new Request(ActionTypes.ROUND_START, data);
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
        if (roomManager.IsTutorial)
        {
            if (round > _tutorialRounds)
            {
                OnTutorialOver();
                yield break;
            }
            CurrentRoundText.text = "Tutorial";
            CurrentTimeText.text = "-";
            summonManager.RemoveAllEnemyPieces(numPlayers);
            TryMarketPhase();
        }
        else
        {
            if (round > RoundsNeededToSurvive)
            {
                OnGameOver();
                yield break;
            }
            else
            {
                StartCoroutine(ShowNewRoundPopUpScreen());
                CurrentRoundText.text = "Round " + round;
                summonManager.RemoveAllEnemyPieces(numPlayers);
                TryMarketPhase();
            }
        }
    }

    IEnumerator ShowNewRoundPopUpScreen()
    {
        string roundText = "Round " + round + " begins!"
            + "<size=50%>\n" + incomeManager.GetIncome(round, roomManager.GetLocalPlayerIndex()) + " Gold Received!";
        roundText += "<size=30%>\n+ " + incomeManager.GetIncomeFromRound(round, roomManager.GetLocalPlayerIndex()) + " Gold From Round";
        if (incomeManager.GetIncomeFromSynergy(round, roomManager.GetLocalPlayerIndex()) > 0)
        {
            roundText += "\n+ " + incomeManager.GetIncomeFromSynergy(round, roomManager.GetLocalPlayerIndex()) + " Gold From Synergy";
        }
        if (incomeManager.GetIncomeFromVictory(round, roomManager.GetLocalPlayerIndex()) > 0)
        {
            roundText += "\n+ " + incomeManager.GetIncomeFromVictory(round, roomManager.GetLocalPlayerIndex()) + " Gold From Win";
        }

        roundText += "</size>";
        PopUpScreen.GetComponentInChildren<TextMeshProUGUI>().text = roundText;
        PopUpScreen.enabled = true;
        yield return new WaitForSeconds(4f);
        PopUpScreen.enabled = false;
    }

    void TryMarketPhase()
    {
        List<Piece> newMarketPieces = marketManager.GenerateMarketItems();
        if (roomManager.IsTutorial)
        {
            switch (round)
            {
                case 1:
                    newMarketPieces = newMarketPieces.Skip(0).Take(3).ToList();
                    break;
                case 2:
                    newMarketPieces = new List<int>() { 0, 0, 0 }.Select(
                        placeholder => new Piece(
                            NameGenerator.GenerateName(Enums.Job.Mage, Enums.Race.Human),
                            NameGenerator.GetTitle(Enums.Race.Human, Enums.Job.Mage),
                            Enums.Race.Human,
                            Enums.Job.Mage,
                            1,
                            false,
                            140,
                            100,
                            9,
                            5,
                            4,
                            2,
                            Enums.Spell.Fireblast)).ToList();
                    break;
                default:
                    break;
            }
        }
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

        if (roomManager.IsTutorial)
        {
            for (int i = 0; i < numPlayers; ++i)
                inventoryManager.SetGold((Player)i, 1);
        }
        else
        {
            Debug.Log(round + " market");
            incomeManager.GenerateIncome(round);
            yield return Countdown(marketDuration);
            TryPreCombat();
        }
    }

    void TryPreCombat()
    {
        int tutorialRound = 0, unusedIndex = 0;
        randomRoundIndex = summonManager.GenerateRandomIndex(round);
        var enemies = roomManager.IsTutorial
            ? summonManager.GenerateEnemies(tutorialRound, unusedIndex, numPlayers)
            : summonManager.GenerateEnemies(round, randomRoundIndex, numPlayers);
        Data data = new PreCombatData(enemies, randomRoundIndex);
        Request req = new Request(ActionTypes.PRECOMBAT_PHASE, data); // TODO: replace with proper codes
        requestHandler.SendRequest(req);
    }

    public void StartPreCombat(List<List<Piece>> enemies)
    {
        Debug.Log(summonManager.GetWaveName(round, randomRoundIndex));
        StartCoroutine(ShowRoundWavePopUpScreen());
        StartCoroutine(PreCombatToCombat(enemies));
    }

    IEnumerator ShowRoundWavePopUpScreen()
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
        StartCoroutine(TryCombatWhenReady());
    }

    IEnumerator TryCombatWhenReady()
    {
        while (!requestHandler.ReadyToGo())
            yield return null;
        TryCombat();
    }

    void TryCombat()
    {
        Data data = new CombatData();
        Request req = new Request(ActionTypes.COMBAT_PHASE, data);
        requestHandler.SendRequest(req);
    }

    public void StartCombat()
    {
        StartCoroutine(Combat());
    }

    IEnumerator Combat()
    {
        ChangePhase(Phase.Combat);
        summonManager.RemoveExcessPlayerPieces(numPlayers);
        synergyManager.ApplySynergiesToArmies(numPlayers);
        CurrentTimeText.text = "";
        SwordImage.enabled = true;
        simulationPlayerCount = 0;
        boardManager.StartSim(numPlayers);
        yield return Countdown(combatDuration);
        if (currentPhase == Phase.Combat)
            boardManager.ForceAllBoardsToResolve(numPlayers);
    }

    void TryPostCombat()
    {
        int[] gold = inventoryManager.GetAllPlayerGold();
        int health = marketManager.GetCastleHealth();
        Data data = new PostCombatData(health, gold);
        Request req = new Request(ActionTypes.POSTCOMBAT_PHASE, data);
        requestHandler.SendRequest(req);
    }

    public void SetPlayerReadyForNextPhase(string playerID)
    {
        playerReadySet.Add(playerID);
    }

    public bool PlayersReadyForNextPhase()
    {
        return playerReadySet.Count == roomManager.NumPlayersToStart;
    }

    public void ClearPlayerReadySet()
    {
        playerReadySet.Clear();
    }

    public void SetPostCombatData(int health, int[] gold)
    {
        inventoryManager.CheckAndSetAllPlayerGold(gold);
        marketManager.CalculateAndApplyDamageToCastle(health);
    }

    public void StartPostCombat()
    {
        StopAllCoroutines();
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

    public void OnPieceAdded(AddPieceToBoardEvent e)
    {
        if (!roomManager.IsTutorial)
            return;

        if (currentPhase != Phase.Market)
            return;

        if (e.piece.IsEnemy())
            return;

        int totalPiecesOnBoard = 0;
        for (int i = 0; i < numPlayers; ++i)
            totalPiecesOnBoard += inventoryManager.GetPlayerInventory((Player)i).GetArmyCount();

        int piecesNecessary = 0;
        switch (round)
        {
            case 1:
                piecesNecessary = numPlayers;
                break;
            case 2:
                piecesNecessary = 2 * numPlayers;
                break;
            default:
                break;
        }

        if (totalPiecesOnBoard >= piecesNecessary)
            TryPreCombat();
    }

    private IEnumerator StartFailSafe(int failSafeRound, Phase fsPhase)
    {
        int failSafeTime = GameLogicManager.Inst.Data.CombatDuration;
        switch (fsPhase)
        {
            case Phase.Market:
                failSafeTime = GameLogicManager.Inst.Data.MarketDuration;
                break;
            case Phase.PreCombat:
                failSafeTime = GameLogicManager.Inst.Data.PreCombatDuration;
                break;
            case Phase.Combat:
                failSafeTime = GameLogicManager.Inst.Data.CombatDuration;
                break;
            case Phase.PostCombat:
                failSafeTime = 10;
                break;
            default:
                break;
        }
        yield return new WaitForSecondsRealtime(failSafeTime);

        if (failSafeRound == this.round && fsPhase == currentPhase)
        {
            Debug.LogFormat("{0}: Fail Safe Activated for {1}", "PhaseManager", fsPhase.ToString());
            switch (fsPhase)
            {
                case Phase.Market:
                    TryPreCombat();
                    break;
                case Phase.PreCombat:
                    TryCombat();
                    break;
                case Phase.Combat:
                    TryPostCombat();
                    break;
                case Phase.PostCombat:
                    TryMarketPhase();
                    break;
            }

        }
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
