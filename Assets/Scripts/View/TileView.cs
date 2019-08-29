using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileView : MonoBehaviour
{
    public ViewManager vm;
    public Tile tile;

    public void TrackTile(Tile tile)
    {
        this.tile = tile;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0) && ShouldSpawnPiece())
        {
            vm.AddPiece(EventManager.Instance.draggedPiece, tile.GetRow(), tile.GetCol());

            // TODO: use event listener?
            EventManager.Instance.draggedPiece = null;
            EventManager.Instance.isPieceDropped = true;
        }
    }

    private bool ShouldSpawnPiece()
    {
        return EventManager.Instance.draggedPiece != null && !tile.IsOccupied();
    }
}
