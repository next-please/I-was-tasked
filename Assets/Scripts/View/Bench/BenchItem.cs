using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

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
    public TextMeshPro nameText; // TODO: prob remove later

    public void InstantiateModelPrefab(GameObject characterModel)
    {
        GameObject modelPrefab = Instantiate(characterModel) as GameObject;
        modelPrefab.transform.SetParent(this.transform);
        modelPrefab.transform.localPosition = Vector3.zero;
        modelPrefab.transform.localScale = Vector3.one;

        animator = modelPrefab.GetComponent<Animator>();

        // TODO: remove later
        nameText.text = piece.GetRace().ToString() + " " + piece.GetClass().ToString();
        nameText.transform.rotation = Camera.main.transform.rotation;
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
        transform.position = GetMouseWorldPosition();
    }

    public override void OnBenchDrop(BenchSlot targetSlot)
    {
        if (targetSlot.isOccupied)
        {
            OnEmptyDrop();
            return;
        }
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
