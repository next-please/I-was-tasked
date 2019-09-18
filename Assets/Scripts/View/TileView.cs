using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileView : MonoBehaviour
{
    Tile tile;

    public void TrackTile(Tile tile)
    {
        this.tile = tile;
    }

    public Tile GetTile()
    {
        return tile;
    }
}
