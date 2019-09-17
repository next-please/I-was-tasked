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

public class BenchItem : InteractablePiece
{
    private readonly float distanceOffset = 10f;
    private readonly float scaleOffset = 10f;

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
        modelPrefab.transform.rotation = Camera.main.transform.rotation;
        modelPrefab.transform.Rotate(0, 180, 0); // face forward

        animator = modelPrefab.GetComponent<Animator>();

        // TODO: remove later
        nameText.text = piece.GetRace().ToString() + " " + piece.GetClass().ToString();
        nameText.transform.rotation = Camera.main.transform.rotation;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        // gameObject.GetComponent<Collider>().enabled = false;
        transform.localScale /= scaleOffset; // update to world scale
    }

    public override void OnDrag(PointerEventData eventData)
    {
        SetDraggedState();
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
        if (tile.IsOccupied() || !IsTileDropAllowed())
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
        var screenPoint = Input.mousePosition;
        screenPoint.z = distanceOffset;
        transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
        animator.Play("Walk");
    }

    private void SetBenchState()
    {
        transform.localScale *= scaleOffset; // update to ui scale
        transform.localPosition = Vector3.zero;
        // gameObject.GetComponent<Collider>().enabled = true;

        animator.Play("Idle");
    }

    bool IsTileDropAllowed()
    {
        return PhaseManager.GetCurrentPhase() == Phase.Market;
    }
}
