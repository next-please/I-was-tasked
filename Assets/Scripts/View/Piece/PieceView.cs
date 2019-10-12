using UnityEngine;
using TMPro;

public class PieceView : MonoBehaviour
{
    [HideInInspector]
    public Animator animator;
    public GameObject statusBars;
    public GameObject currentHPBar;
    public GameObject currentMPBar;
    public TextMeshPro nameText; // todo: remove later
    public Piece piece; // The piece being displayed.
    private IViewState prevViewAction;
    private float velocityHP = 0.0f;
    private float velocityMP = 0.0f;
    private float smoothTime = 0.01f;

    private void Start()
    {
        Vector3 lookAtPosition = new Vector3(statusBars.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        statusBars.transform.LookAt(lookAtPosition);
        currentMPBar.transform.localScale = new Vector3(0, 1, 1);
    }

    public void TrackPiece(Piece piece)
    {
        this.piece = piece;

        // todo: remove later
        if (nameText != null)
        {
            nameText.text = piece.GetRace().ToString() + " " + piece.GetClass().ToString();
        }

        piece.SetPieceView(this);
    }

    public void InstantiateModelPrefab(GameObject characterModel)
    {
        GameObject modelPrefab = Instantiate(characterModel) as GameObject;
        modelPrefab.transform.SetParent(this.transform);
        modelPrefab.transform.localPosition = Vector3.zero;
        modelPrefab.transform.rotation = transform.rotation;

        animator = modelPrefab.GetComponent<Animator>();
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

    void OnDrawGizmos()
    {
        if (piece != null)
        {
            GUIStyle style = new GUIStyle();
            style.fontStyle = FontStyle.Bold;
            style.fontSize = 24;
            // Handles.Label(transform.position + Vector3.up * 0.5f, piece.GetViewState().ToString(), style);
        }
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
            transform.rotation = Quaternion.identity; // there's a bug with this.. where the rotation drifts...
        }
    }

    public void UpdateCurrentHPBar()
    {
        float currHPScale = Mathf.SmoothDamp(currentHPBar.transform.localScale.x, (float) piece.GetCurrentHitPoints() / piece.GetMaximumHitPoints(), ref velocityHP, smoothTime);
        currentHPBar.transform.localScale = new Vector3(currHPScale, 1, 1);
    }

    public void UpdateCurrentMPBar()
    {
        float currMPScale = Mathf.SmoothDamp(currentMPBar.transform.localScale.x, (float)piece.GetCurrentManaPoints() / piece.GetMaximumManaPoints(), ref velocityMP, smoothTime);
        currentMPBar.transform.localScale = new Vector3(currMPScale, 1, 1);
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
