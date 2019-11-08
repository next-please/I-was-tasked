using UnityEngine;
using UnityEngine.EventSystems;

public class MoveFromBenchToBoardEvent : GameEvent
{
    public Piece piece;
    public Tile tile;
    public int slotIndex;
}

public class MoveOnBenchEvent : GameEvent
{
    public Piece piece;
    public int slotIndex;
}

public class TrashPieceOnBenchEvent : GameEvent
{
    public Piece piece;
}

public class ShowTrashCanEvent : GameEvent
{
    public bool showTrashCan;
}

public class BenchItem : InteractablePiece
{
    private Animator animator;

    [HideInInspector]
    public int index;

    public void AdjustRarityRotationView(Player player)
    {
        foreach (GameObject rarity in rarities)
        {
            rarity.SetActive(false);
        }
        rarityParent.transform.LookAt(rarityParent.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        rarityParent.transform.eulerAngles = new Vector3(rarityParent.transform.eulerAngles.x, 45.0f * (int) player, 0.0f);
        rarities[piece.GetRarity() - 1].SetActive(true);
    }

    public void InstantiateModelPrefab(GameObject characterModel)
    {
        GameObject modelPrefab = Instantiate(characterModel) as GameObject;
        modelPrefab.transform.SetParent(this.transform);
        modelPrefab.transform.localPosition = Vector3.zero;
        modelPrefab.transform.localScale = Vector3.one;
        animator = modelPrefab.GetComponent<Animator>();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (IsDragAllowed())
        {
            SetDraggedState();
            EventManager.Instance.Raise(new ShowTrashCanEvent { showTrashCan = true });
        }
        else
        {
            eventData.pointerDrag = null;
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        rarityParent.transform.LookAt(rarityParent.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        transform.position = GetMouseWorldPosition();
        HitTarget target = GetHitTarget();
        if (target == HitTarget.Tile || target == HitTarget.BenchSlot)
        {
            EventManager.Instance.Raise(new DragOverTileEvent {
                hitTarget = target,
                targetObject = this.targetObject
            });
        }
    }

    public override void OnBenchDrop(BenchSlot targetSlot)
    {
        if (targetSlot.isOccupied)
        {
            OnEmptyDrop();
            return;
        }
        rarityParent.transform.LookAt(rarityParent.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        EventManager.Instance.Raise(new MoveOnBenchEvent { piece = piece, slotIndex = targetSlot.index });
        Destroy(gameObject);
    }

    public override void OnTileDrop(Tile tile)
    {
        if (tile.IsOccupied() | !tile.IsEnemyTile() || !IsTileDropAllowed())
        {
            OnEmptyDrop();
            return;
        }
        EventManager.Instance.Raise(new MoveFromBenchToBoardEvent { piece = piece, tile = tile, slotIndex = index });
        Destroy(gameObject);
    }

    public override void OnTrashDrop()
    {
        EventManager.Instance.Raise(new TrashPieceOnBenchEvent { piece = piece });
        Destroy(gameObject);
    }

    // Returns piece to bench
    public override void OnEmptyDrop()
    {
        SetBenchState();
    }

    private void SetDraggedState()
    {
        animator.Play("Walk");
    }

    private void SetBenchState()
    {
        transform.localPosition = Vector3.up / 2;
        animator.Play("Idle");
    }

    bool IsTileDropAllowed()
    {
        Phase currentPhase = PhaseManager.GetCurrentPhase();
        return (currentPhase == Phase.Market || currentPhase == Phase.PreCombat);
    }

    private bool IsDragAllowed()
    {
        return CameraController.IsViewingOwnBoard();
    }
}
