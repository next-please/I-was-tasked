﻿using System.Collections;
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
        }
    }

    private bool ShouldSpawnPiece()
    {
        return EventManager.Instance.draggedPiece != null && !tile.IsOccupied();
    }
}
