using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    //timer variables
    public Text timerText;
    public readonly int ticksPerSecond = 50;

    //player variables
    ArrayList player1Garrison;
    ArrayList player2Garrison;
    ArrayList player3Garrison;
    public readonly int boardWidth = 8;
    public readonly int boardLength = 8;
    int[,] player1Board;
    int[,] player2Board;
    int[,] player3Board;
    int player1ArmySize;
    int player2ArmySize;
    int player3ArmySize;
    public readonly int startingArmySize = 0;

    //game state variables
    public Text currentRoundText;
    public Text currentPhaseText;
    public Canvas winScreenCanvas;
    enum Phase {Initialization, Market, PreCombat, Combat, PostCombat};
    Phase currentPhase;
    int countdownTicks;
    int currentRound;
    public readonly int maxRounds = 3; 
    int initializationRoundLength = 10;
    int marketRoundLength = 50;
    int preCombatRoundLength = 50;
    int combatRoundLength = 50;
    int postCombatRoundLength = 50;

    //gold variables
    public readonly int maxFixedIncome = 5;
    public readonly decimal InterestRate = 1m / 10;
    int incomeFromUpgrades;
    int player1Gold;
    public Text player1GoldText;
    int player2Gold;
    public Text player2GoldText;
    int player3Gold;
    public Text player3GoldText;
    public readonly int startingGold = 0;

    // Start is called before the first frame update
    void Start()
    {
        player1Garrison = new ArrayList();
        player2Garrison = new ArrayList();
        player3Garrison = new ArrayList();
        player1Board = new int[boardLength, boardWidth];
        player2Board = new int[boardLength, boardWidth];
        player3Board = new int[boardLength, boardWidth];

        Initialization();
    }

    void Initialization()
    {
        currentPhase = Phase.Initialization;
        countdownTicks = initializationRoundLength;
        currentRound = 0;
        incomeFromUpgrades = 0;
        player1Gold = startingGold;
        player1ArmySize = startingArmySize;
        player2Gold = startingGold;
        player2ArmySize = startingArmySize;
        player3Gold = startingGold;
        player3ArmySize = startingArmySize;

        currentRoundText.text = "Round " + currentRound;
        currentPhaseText.text = "Initializing...";
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        countdownTicks -= 1;
        decimal remainingSeconds = Math.Ceiling(Convert.ToDecimal(countdownTicks) / ticksPerSecond);
        timerText.text = remainingSeconds.ToString();

        if (countdownTicks == 0)
        {
            ChangePhase();
        }
    }

    void ChangePhase()
    {
        switch (currentPhase)
        {
            case Phase.Initialization:
                InitializeNewRound();
                InitializeMarketPhase();
                break;
            case Phase.Market:
                InitializePreCombatPhase();
                break;
            case Phase.PreCombat:
                InitializeCombatPhase();
                break;
            case Phase.Combat:
                InitializePostCombatPhase();
                break;
            case Phase.PostCombat:
                DespawnEnemiesAndRespawnAllies();
                InitializeNewRound();
                InitializeMarketPhase();
                break;
            default:
                Debug.Log("Phase Error! Non-recognised phase currently set: " + currentPhase);
                break;
        }

    }

    private void InitializeNewRound()
    {
        currentRound++;
        if (currentRound > maxRounds)
        {
            WinGame();
        }
        currentRoundText.text = "Round " + currentRound;
        generateIncome();
    }

    private void generateIncome()
    {
        int currentIncome = Math.Min(currentRound, maxFixedIncome);
        currentIncome += incomeFromUpgrades;

        decimal incomeFromInterest = Math.Floor(player1Gold * InterestRate);
        player1Gold += currentIncome + Convert.ToInt32(incomeFromInterest);
        player1GoldText.text = player1Gold + " Gold";

        incomeFromInterest = Math.Floor(player2Gold * InterestRate);
        player2Gold += currentIncome + Convert.ToInt32(incomeFromInterest);
        player2GoldText.text = player2Gold + " Gold";

        incomeFromInterest = Math.Floor(player3Gold * InterestRate);
        player3Gold += currentIncome + Convert.ToInt32(incomeFromInterest);
        player3GoldText.text = player3Gold + " Gold";
    }

    private void InitializeMarketPhase()
    {
        countdownTicks = marketRoundLength;
        currentPhaseText.text = "Market Phase";
        currentPhase = Phase.Market;

        GenerateMarket();
    }

    private void GenerateMarket()
    {
        throw new NotImplementedException();
    }

    private void InitializePreCombatPhase()
    {
        countdownTicks = preCombatRoundLength;
        currentPhaseText.text = "Pre-Combat";
        currentPhase = Phase.PreCombat;

        SummonEnemies();
    }

    private void SummonEnemies()
    {
        throw new NotImplementedException();
    }

    private void InitializeCombatPhase()
    {
        countdownTicks = combatRoundLength;
        currentPhaseText.text = "Combat";
        currentPhase = Phase.Combat;
        throw new NotImplementedException();
    }

    private void InitializePostCombatPhase()
    {
        countdownTicks = postCombatRoundLength;
        currentPhaseText.text = "Post-Combat";
        currentPhase = Phase.PostCombat;

        CalculateDamageToCastle();
    }

    private void CalculateDamageToCastle()
    {
        throw new NotImplementedException();
    }

    private void DespawnEnemiesAndRespawnAllies()
    {
        //throw new NotImplementedException();
    }

    private void WinGame()
    {
        winScreenCanvas.enabled = true;
    }
}
