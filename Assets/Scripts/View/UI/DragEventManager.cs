using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragEventManager : MonoBehaviour
{
    private static DragEventManager instance;

    public static DragEventManager Instance
    {
        get { return instance; }
    }
    public Piece draggedPiece;
    public bool isPieceDropped = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
}
