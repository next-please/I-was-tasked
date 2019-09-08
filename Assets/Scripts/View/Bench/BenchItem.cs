using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

public class BenchItem : Droppable
{
    private readonly float distanceOffset = 10f;
    private readonly float scaleOffset = 10f;

    public Piece piece;
    public int index;

    public void InstantiateModelPrefab(GameObject characterModel)
    {
        GameObject modelPrefab = Instantiate(characterModel) as GameObject;
        modelPrefab.transform.SetParent(this.transform);
        modelPrefab.transform.localPosition = Vector3.zero;
        modelPrefab.transform.localScale = Vector3.one;

        Quaternion cameraRotation = Camera.main.transform.rotation;
        modelPrefab.transform.rotation = Camera.main.transform.rotation;
        modelPrefab.transform.Rotate(0, 180, 0); // face forward
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        // gameObject.GetComponent<Collider>().enabled = false;
        transform.localScale /= scaleOffset; // update to world scale
    }

    public override void OnDrag(PointerEventData eventData)
    {
        var screenPoint = Input.mousePosition;
        screenPoint.z = distanceOffset;
        transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
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
        if (tile.IsOccupied())
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
        transform.localScale *= scaleOffset; // update to ui scale
        transform.localPosition = Vector3.zero;
        // gameObject.GetComponent<Collider>().enabled = true;
    }
}
