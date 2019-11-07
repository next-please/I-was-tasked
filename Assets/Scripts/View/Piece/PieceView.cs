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
    public GameObject dividerHP;
    public GameObject currentHPBar;
    public GameObject midgroundHPBar;
    public GameObject currentMPBar;
    public GameObject[] rarities;
    public Piece piece; // The piece being displayed.
    public PieceSounds pieceSounds;
    private IViewState prevViewAction;
    private float velocityHP = 0.0f;
    private float velocityMP = 0.0f;
    private float smoothTimeHP = 0.2f;
    private float smoothTimeMP = 0.01f;
    private Player boardOwner;
    private GameObject[] dividers;
    private int maximumHP = -1;


    private void Start()
    {
        statusBars.transform.LookAt(statusBars.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        pieceSounds = GetComponentInChildren<PieceSounds>();
        pieceSounds.InstantiateSounds(transform);
    }

    public void TrackPiece(Piece piece)
    {
        this.piece = piece;
        piece.SetPieceView(this);

        if (!piece.IsEnemy())
        {
            foreach (GameObject rarity in rarities)
            {
                rarity.SetActive(false);
            }
            rarities[piece.GetRarity() - 1].SetActive(true);
        }
        SetHealthDividers();
        maximumHP = piece.GetMaximumHitPoints();
    }

    private void SetHealthDividers()
    {
        if (maximumHP == piece.GetMaximumHitPoints())
        {
            return;
        }

        // Remove any existing dividers.
        if (dividers != null)
        {
            for (int i = 0; i < dividers.Length; i++)
            {
                Destroy(dividers[i]);
            }
        }

        int numDividers = Mathf.CeilToInt(piece.GetMaximumHitPoints() / 25.0f);
        float spacingWidth = 11.5f / numDividers;
        int incrementCount = 1;

        dividers = new GameObject[numDividers];
        for (int i = 0; i < numDividers; i++)
        {
            dividers[i] = Instantiate(dividerHP) as GameObject;
            dividers[i].transform.SetParent(currentHPBar.transform.parent);
            dividers[i].transform.localScale = dividerHP.transform.localScale;
            dividers[i].transform.localPosition = Vector3.zero;

            if (numDividers % 2 == 0)
            {
                if (i <= 1)
                {
                    float widthX = spacingWidth / 2.0f * ((i % 2 == 0) ? 1 : -1);
                    dividers[i].transform.localPosition = new Vector3(widthX, 0.0f, 0.0f);
                }
                else
                {
                    float widthX = (spacingWidth / 2.0f + spacingWidth * incrementCount) * ((i % 2 == 0) ? 1 : -1);
                    dividers[i].transform.localPosition = new Vector3(widthX, 0.0f, 0.0f);
                }

                if (i > 1 && i % 2 == 1)
                {
                    incrementCount++;
                }
            }
            else
            {
                if (i != 0)
                {
                    float widthX = (spacingWidth + spacingWidth * (incrementCount - 1)) * ((i % 2 == 0) ? 1 : -1);
                    dividers[i].transform.localPosition = new Vector3(widthX, 0.0f, 0.0f);
                }

                if (i > 1 && i % 2 == 0)
                {
                    incrementCount++;
                }
            }

            if (spacingWidth < 1)
            {
                dividers[i].transform.localScale = new Vector3(dividers[i].transform.localScale.x * spacingWidth, dividers[i].transform.localScale.y, dividers[i].transform.localScale.z);
            }
        }
        dividerHP.SetActive(false);
    }

    public void InstantiateModelPrefab(GameObject characterModel, Player boardOwner)
    {
        GameObject modelPrefab = Instantiate(characterModel) as GameObject;
        modelPrefab.transform.SetParent(this.transform);
        modelPrefab.transform.localPosition = Vector3.zero;
        if (piece.GetTitle().Equals("Swarm"))
        {
            modelPrefab.transform.localPosition = Vector3.zero + Vector3.up*0.9f;
        }
        if (piece.spell == Enums.Spell.Berserk)
        {
            modelPrefab.transform.localPosition = Vector3.zero + Vector3.down*0.8f;
        }
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
                pieceSounds.PlayDeathSound();
            }

            if (transform.position.y + 3 > 0.01)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, -3, transform.position.z), Time.deltaTime * 0.0325f);
            }
            return;
        }
        else
        {
            if (!statusBars.activeSelf)
            {
                statusBars.SetActive(true);
            }
            SetHealthDividers();
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

        statusBars.transform.LookAt(statusBars.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
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
        float currHPScale = Mathf.SmoothDamp(midgroundHPBar.transform.localScale.x, (float) piece.GetCurrentHitPoints() / piece.GetMaximumHitPoints(), ref velocityHP, smoothTimeHP);
        currentHPBar.transform.localScale = new Vector3((float)piece.GetCurrentHitPoints() / piece.GetMaximumHitPoints(), currentHPBar.transform.localScale.y, currentHPBar.transform.localScale.z);
        midgroundHPBar.transform.localScale = new Vector3(currHPScale, midgroundHPBar.transform.localScale.y, midgroundHPBar.transform.localScale.z);
        EventManager.Instance.Raise(new PieceHealthChangeEvent { piece = piece });
    }

    public void UpdateCurrentMPBar()
    {
        float currMPScale = Mathf.SmoothDamp(currentMPBar.transform.localScale.x, (float)piece.GetCurrentManaPoints() / piece.GetMaximumManaPoints(), ref velocityMP, smoothTimeMP);
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
