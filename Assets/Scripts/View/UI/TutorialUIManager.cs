using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Nextplease.IWT;

public class TutorialUIManager : MonoBehaviour
{
    [Header("Canvas")]
    public RoomManager roomManager;
    public GameObject WelcomeCanvas;
    public GameObject PurchaseUnitCanvas;
    public GameObject PanToBoardCanvas;
    public GameObject DragToBoardCanvas;
    public GameObject EnemiesCanvas;
    public GameObject HealthCanvas;
    public GameObject PanToMarketCanvas;
    public GameObject SynergyCanvas;
    public GameObject EndCanvas;

    [Header("Game Board Items")]
    public GameObject PurchaseUnitArrows;
    public List<GameObject> PanToBoardArrows;
    public List<GameObject> DragToBoardArrows;

    private bool waitingForPurchase = false;
    private bool waitingForPan = false;
    private bool waitingForDrag = false;
    private bool waitingForPre = false;
    private bool waitingForCombat = false;
    private bool waitingForPost = false;
    private bool waitingForPanMarket = false;
    private bool waitingForSynergy = false;
    private bool waitingForEnd = false;

    void OnEnable()
    {
        EventManager.Instance.AddListener<EnterPhaseEvent>(OnPhaseEnter);
        EventManager.Instance.AddListener<InventoryChangeEvent>(OnInventoryChange);
        EventManager.Instance.AddListener<JobSynergyAppliedEvent>(OnJobSynergy);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<EnterPhaseEvent>(OnPhaseEnter);
        EventManager.Instance.RemoveListener<InventoryChangeEvent>(OnInventoryChange);
        EventManager.Instance.RemoveListener<JobSynergyAppliedEvent>(OnJobSynergy);
    }

    void OnPhaseEnter(EnterPhaseEvent e)
    {
        if (!roomManager.IsTutorial)
            return;

        if (e.phase == Phase.Initialization && e.round == 0)
        {
            ShowWelcome();
        }

        if (e.phase == Phase.PreCombat && waitingForPre)
        {
            ShowEnemies();
        }

        if (e.phase == Phase.Combat && waitingForCombat)
        {
            if (e.round == 1)
                CloseEnemies();
            else
                CloseSynergy();
        }

        if (e.phase == Phase.PostCombat && waitingForPost)
        {
            ShowHealth();
        }

        if (e.phase == Phase.PostCombat && waitingForEnd)
        {
            ShowEnd();
        }
    }

    void OnInventoryChange(InventoryChangeEvent e)
    {
        if (!roomManager.IsTutorial)
           return;
        if (e.inventory.GetOwner() != RoomManager.GetLocalPlayer())
            return;

        if (waitingForPurchase)
        {
            ClosePurchaseUnit();
            ShowPanToBoard();
        }

        if (waitingForDrag && e.inventory.GetArmyCount() > 0)
        {
            CloseDragToBoard();
            waitingForPre = true;
        }
    }

    void Update()
    {
        if (waitingForPan && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            ClosePanToBoard();
            ShowDragToBoard();
        }

        if (waitingForPanMarket && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            ClosePanToMarket();
        }
    }

    void OnJobSynergy(JobSynergyAppliedEvent e)
    {
        if (waitingForSynergy)
        {
            ShowSynergy();
        }
    }

    void ShowWelcome()
    {
        WelcomeCanvas.SetActive(true);
    }

    public void CloseWelcome()
    {
        WelcomeCanvas.SetActive(false);
        ShowPurchaseUnit();
    }

    void ShowPurchaseUnit()
    {
        PurchaseUnitCanvas.SetActive(true);
        PurchaseUnitArrows.SetActive(true);
        waitingForPurchase = true;
    }

    void ClosePurchaseUnit()
    {
        PurchaseUnitCanvas.SetActive(false);
        PurchaseUnitArrows.SetActive(false);
        waitingForPurchase = false;
    }

    void ShowPanToBoard()
    {
        GameObject arrow = PanToBoardArrows[(int)RoomManager.GetLocalPlayer()];
        PanToBoardCanvas.SetActive(true);
        arrow.SetActive(true);
        waitingForPan = true;
    }

    void ClosePanToBoard()
    {
        GameObject arrow = PanToBoardArrows[(int)RoomManager.GetLocalPlayer()];
        PanToBoardCanvas.SetActive(false);
        arrow.SetActive(false);
        waitingForPan = false;
    }

    void ShowDragToBoard()
    {
        GameObject arrows = DragToBoardArrows[(int)RoomManager.GetLocalPlayer()];
        DragToBoardCanvas.SetActive(true);
        arrows.SetActive(true);
        waitingForDrag = true;
    }

    void CloseDragToBoard()
    {
        GameObject arrows = DragToBoardArrows[(int)RoomManager.GetLocalPlayer()];
        DragToBoardCanvas.SetActive(false);
        arrows.SetActive(false);
        waitingForDrag = false;
    }

    void ShowEnemies()
    {
        waitingForPre = false;
        EnemiesCanvas.SetActive(true);
        waitingForCombat = true;
    }

    void CloseEnemies()
    {
        EnemiesCanvas.SetActive(false);
        waitingForCombat = false;
        waitingForPost = true;
    }

    void ShowHealth()
    {
        HealthCanvas.SetActive(true);
        waitingForPost = false;
    }

    public void CloseHealth()
    {
        HealthCanvas.SetActive(false);
    }

    public void ShowPanToMarket()
    {
        PanToMarketCanvas.SetActive(true);
        waitingForPanMarket = true;
    }

    void ClosePanToMarket()
    {
        PanToMarketCanvas.SetActive(false);
        waitingForPanMarket = false;
        waitingForSynergy = true;
        waitingForEnd = true;
    }

    void ShowSynergy()
    {
        waitingForSynergy = false;
        SynergyCanvas.SetActive(true);
        waitingForCombat = true;
    }

    void CloseSynergy()
    {
        SynergyCanvas.SetActive(false);
    }

    void ShowEnd()
    {
        EndCanvas.SetActive(true);
    }

    public void CloseTutorial()
    {
        Destroy(gameObject);
    }
}
