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
    int player1ArmySize; //also used as price for army size increase
    int player2ArmySize; //also used as price for army size increase
    int player3ArmySize; //also used as price for army size increase
    public Text player1ArmySizeText;
    public Text player2ArmySizeText;
    public Text player3ArmySizeText;
    public readonly int startingArmySize = 1;

    //game state variables
    public Text currentRoundText;
    public Text currentPhaseText;
    public Canvas winScreenCanvas;
    enum Phase {Initialization, Market, PreCombat, Combat, PostCombat};
    Phase currentPhase;
    int countdownTicks;
    int currentRound;
    public readonly int maxRounds = 3; 
    int initializationRoundLength = 100;
    int marketRoundLength = 100;
    int preCombatRoundLength = 100;
    int combatRoundLength = 100;
    int postCombatRoundLength = 100;

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
    public readonly int upgradeIncomeCost = 10;
    public readonly int upgradeIncomeAmount = 1;
    public Text passiveIncomeText;

    //market variables
    int marketTier = 1; //also used as price for market increase
    public Text[] marketTierUpgradeText;
    int marketSize;
    public readonly int startingMarketSize = 5;
    public readonly int marketSizeUpgradeCost = 2;
    public Text player1UpgradeArmySizeCostText;
    public Text player2UpgradeArmySizeCostText;
    public Text player3UpgradeArmySizeCostText;
    public Text marketSizeText;
    public Text marketRarityText;

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
        marketSize = startingMarketSize;

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
        int currentIncome = generateIncome();
    }

    private int generateIncome()
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

        return currentIncome;
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

    /**
     * 
     * Player Buttons and Actions
     *
    **/

    public void TestButton()
    {
        winScreenCanvas.enabled = true;
    }

    public void Player1UpgradeArmy()
    {
        if (player1Gold >= player1ArmySize)
        {
            player1Gold -= player1ArmySize;
            player1ArmySize++;
            player1GoldText.text = player1Gold + " Gold";
            player1ArmySizeText.text = "Army Size: " + player1ArmySize;
            player1UpgradeArmySizeCostText.text = "Upgrade Army Size ($" + player1ArmySize + ")";
        }
    }

    public void Player1UpgradeMarketSize()
    {
        if (player1Gold >= marketSizeUpgradeCost)
        {
            player1Gold -= marketSizeUpgradeCost;
            marketSize++;
            player1GoldText.text = player1Gold + " Gold";
            marketSizeText.text = "Market Size: " + marketSize;
        }
    }

    public void Player1UpgradeMarketRarity()
    {
        if (player1Gold >= marketTier)
        {
            player1Gold -= marketTier;
            marketTier++;
            player1GoldText.text = player1Gold + " Gold";

            marketRarityText.text = "Market Rarity: Tier " + marketTier;
            foreach (Text text in marketTierUpgradeText)
            {
                text.text = "Upgrade Market Rarity ($" + marketTier + ")";
            }
        }
    }

    public void Player1UpgradePassiveIncome()
    {
        if (player1Gold >= upgradeIncomeCost)
        {
            player1Gold -= upgradeIncomeCost;
            incomeFromUpgrades++;
            player1GoldText.text = player1Gold + " Gold";
            passiveIncomeText.text = "Additional Passive Income: " + incomeFromUpgrades;
        }
    }

    public void Player2UpgradeArmy()
    {
        Debug.Log("test");
        if (player2Gold >= player2ArmySize)
        {
            Debug.Log("Success");
            player2Gold -= player2ArmySize;
            player2ArmySize++;
            player2GoldText.text = player2Gold + " Gold";
            Debug.Log(player2Gold);
            player2ArmySizeText.text = "Army Size: " + player2ArmySize;
            Debug.Log(player2ArmySize);
            player2UpgradeArmySizeCostText.text = "Upgrade Army Size ($" + player2ArmySize + ")";
        }
    }

    public void Player2UpgradeMarketSize()
    {
        if (player2Gold >= marketSizeUpgradeCost)
        {
            player2Gold -= marketSizeUpgradeCost;
            marketSize++;
            player2GoldText.text = player2Gold + " Gold";
            marketSizeText.text = "Market Size: " + marketSize;
        }
    }

    public void Player2UpgradeMarketRarity()
    {
        if (player2Gold >= marketTier)
        {
            player2Gold -= marketTier;
            marketTier++;
            player2GoldText.text = player2Gold + " Gold";

            marketRarityText.text = "Market Rarity: Tier " + marketTier;
            foreach (Text text in marketTierUpgradeText)
            {
                text.text = "Upgrade Market Rarity ($" + marketTier + ")";
            }
        }
    }

    public void Player2UpgradePassiveIncome()
    {
        if (player2Gold >= upgradeIncomeCost)
        {
            player2Gold -= upgradeIncomeCost;
            incomeFromUpgrades++;
            player2GoldText.text = player2Gold + " Gold";
            passiveIncomeText.text = "Additional Passive Income: " + incomeFromUpgrades;
        }
    }

    public void Player3UpgradeArmy()
    {
        if (player3Gold >= player3ArmySize)
        {
            player3Gold -= player3ArmySize;
            player3ArmySize++;
            player3GoldText.text = player3Gold + " Gold";
            player3ArmySizeText.text = "Army Size: " + player3ArmySize;
            player3UpgradeArmySizeCostText.text = "Upgrade Army Size ($" + player3ArmySize + ")";
        }
    }

    public void Player3UpgradeMarketSize()
    {
        if (player3Gold >= marketSizeUpgradeCost)
        {
            player3Gold -= marketSizeUpgradeCost;
            marketSize++;
            player3GoldText.text = player3Gold + " Gold";
            marketSizeText.text = "Market Size: " + marketSize;
        }
    }

    public void Player3UpgradeMarketRarity()
    {
        if (player3Gold >= marketTier)
        {
            player3Gold -= marketTier;
            marketTier++;
            player3GoldText.text = player3Gold + " Gold";

            marketRarityText.text = "Market Rarity: Tier " + marketTier;
            foreach (Text text in marketTierUpgradeText)
            {
                text.text = "Upgrade Market Rarity ($" + marketTier + ")";
            }
        }
    }

    public void Player3UpgradePassiveIncome()
    {
        if (player3Gold >= upgradeIncomeCost)
        {
            player3Gold -= upgradeIncomeCost;
            incomeFromUpgrades++;
            player3GoldText.text = player3Gold + " Gold";
            passiveIncomeText.text = "Additional Passive Income: " + incomeFromUpgrades;
        }
    }
}
