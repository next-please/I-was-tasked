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
            vm.AddPiece(DragEventManager.Instance.draggedPiece, tile.GetRow(), tile.GetCol());

            // TODO: use event listener?
            DragEventManager.Instance.draggedPiece = null;
            DragEventManager.Instance.isPieceDropped = true;
        }
    }

    private bool ShouldSpawnPiece()
    {
        return DragEventManager.Instance.draggedPiece != null && !tile.IsOccupied();
    }
}
