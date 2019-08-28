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
        if (!Input.GetMouseButtonUp(0)
            || tile.IsOccupied()
            || EventManager.Instance.draggedPiece == null)
        {
            return;
        }

        MeleePiece lewis_enemy = new MeleePiece("Lewis the Jesus Koh", 100, 1, true);

        vm.AddPiece(lewis_enemy, tile.GetRow(), tile.GetCol());
    }
}
