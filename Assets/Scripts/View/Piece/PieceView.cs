using UnityEngine;
using TMPro;

public class PieceHealthChangeEvent : GameEvent
{
    public Piece piece;
}

public class PieceView : MonoBehaviour
{
    [HideInInspector]
    public Animator animator;
    public GameObject statusBars;
    public GameObject currentHPBar;
    public GameObject currentMPBar;
    public GameObject[] rarities;
    public Piece piece; // The piece being displayed.
    public PieceSounds pieceSounds;
    private IViewState prevViewAction;
    private float velocityHP = 0.0f;
    private float velocityMP = 0.0f;
    private float smoothTime = 0.01f;
    private Player boardOwner;


    private void Start()
    {
        Vector3 lookAtPosition = new Vector3(statusBars.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        statusBars.transform.LookAt(lookAtPosition);
        pieceSounds = GetComponentInChildren<PieceSounds>();
        pieceSounds.InstantiateSounds(this.transform);
        foreach (GameObject rarity in rarities)
        {
            rarity.SetActive(false);
        }
        rarities[piece.GetRarity() - 1].SetActive(true);
    }

    public void TrackPiece(Piece piece)
    {
        this.piece = piece;
        piece.SetPieceView(this);
    }

    public void InstantiateModelPrefab(GameObject characterModel, Player boardOwner)
    {
        GameObject modelPrefab = Instantiate(characterModel) as GameObject;
        modelPrefab.transform.SetParent(this.transform);
        modelPrefab.transform.localPosition = Vector3.zero;
        modelPrefab.transform.rotation = transform.rotation;
        animator = modelPrefab.GetComponent<Animator>();
        this.boardOwner = boardOwner;
        float rotation = (int) boardOwner * 45;
        rotation += piece.IsEnemy() ? 180 : 0;
        modelPrefab.transform.parent.Rotate(new Vector3(0, 1, 0), rotation);
    }

    void OnEnable()
    {
        EventManager.Instance.AddListener<RemovePieceFromBoardEvent>(OnPieceRemoved);
        EventManager.Instance.AddListener<PieceMoveEvent>(OnPieceMove);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<RemovePieceFromBoardEvent>(OnPieceRemoved);
        EventManager.Instance.RemoveListener<PieceMoveEvent>(OnPieceMove);
    }

    private void Update()
    {
        if (piece == null)
        {
            return;
        }

        if (piece.IsDead())
        {
            if (currentHPBar.transform.localScale.x > 0)
            {
                UpdateCurrentHPBar();
            }
            if (statusBars.activeSelf && currentHPBar.transform.localScale.x <= 0)
            {
                statusBars.SetActive(false);
                animator.Play("Death", 0);
            }
            return;
        }
        else
        {
            if (!statusBars.activeSelf)
            {
                statusBars.SetActive(true);
            }
            UpdateCurrentHPBar();
            UpdateCurrentMPBar();
        }

        IViewState viewAction = piece.GetViewState();
        if (prevViewAction != null)
        {
            prevViewAction.CallViewFinishIfNeeded(this);
        }
        viewAction.CallViewStartIfNeeded(this);
        viewAction.OnViewUpdate(this);
        prevViewAction = viewAction;

        Vector3 lookAtPosition = statusBars.transform.position - Camera.main.transform.forward;
        statusBars.transform.LookAt(lookAtPosition);
    }

    void OnPieceRemoved(RemovePieceFromBoardEvent e)
    {
        if (e.piece == piece)
        {
            DestroyPieceView();
        }
    }

    void OnPieceMove(PieceMoveEvent e)
    {
        if (e.piece == piece)
        {
            Vector3 piecePosition = ViewManager.CalculateTileWorldPosition(e.tile);
            piecePosition.y = 0.5f;
            transform.position = piecePosition;
            transform.rotation = Quaternion.identity;
            float rotation = (int) boardOwner * 45;
            rotation += piece.IsEnemy() ? 180 : 0;
            transform.Rotate(new Vector3(0, 1, 0), rotation);
        }
    }

    public void UpdateCurrentHPBar()
    {
        float currHPScale = Mathf.SmoothDamp(currentHPBar.transform.localScale.x, (float) piece.GetCurrentHitPoints() / piece.GetMaximumHitPoints(), ref velocityHP, smoothTime);
        currentHPBar.transform.localScale = new Vector3(currHPScale, currentHPBar.transform.localScale.y, currentHPBar.transform.localScale.z);
        EventManager.Instance.Raise(new PieceHealthChangeEvent { piece = piece });
    }

    public void UpdateCurrentMPBar()
    {
        float currMPScale = Mathf.SmoothDamp(currentMPBar.transform.localScale.x, (float)piece.GetCurrentManaPoints() / piece.GetMaximumManaPoints(), ref velocityMP, smoothTime);
        currentMPBar.transform.localScale = new Vector3(currMPScale, currentMPBar.transform.localScale.y, currentMPBar.transform.localScale.z);
    }

    public void DestroyPieceView()
    {
        Destroy(gameObject);
        piece.SetPieceView(null);
    }

    public void TransportToTile(Tile tile)
    {
        Vector3 pos = ViewManager.CalculateTileWorldPosition(tile);
        float y = transform.position.y;
        transform.position = new Vector3(pos.x, y, pos.z);
    }

    public void LookAtTile(Tile tile)
    {
        Vector3 pos = ViewManager.CalculateTileWorldPosition(tile);
        pos.y = 0.5f;
        transform.LookAt(pos);
    }
}
