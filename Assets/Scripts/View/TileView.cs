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

    private void OnMouseDown()
    {
        if (tile.IsOccupied())
        {
            return;
        }

        MeleePiece lewis_enemy = new MeleePiece("Lewis the Jesus Koh", 100, 1, true);
        MeleePiece jolyn_player = new MeleePiece("Jo Jo Lyn", 100, 3, false);

        vm.AddPiece(lewis_enemy, tile.GetRow(), tile.GetCol());
        vm.AddPiece(jolyn_player, tile.GetRow() + 1, tile.GetCol() + 1);
    }
}
