using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceView : MonoBehaviour
{
    public Piece piece = null; // piece that I'm trying to display
    IViewAction currentViewAction;
    public void TrackPiece(Piece piece)
    {
        this.piece = piece;
    }

    void Update()
    {
        if (piece == null)
        {
            return;
        }

        IViewAction viewAction = piece.GetViewAction();
        if (currentViewAction != viewAction)
        {
            if (currentViewAction != null)
            {
                // previous action finished
                currentViewAction.OnViewFinish(this);
            }
            currentViewAction = viewAction;
            // new action starts
            currentViewAction.OnViewStart(this);
        }

        // action update
        currentViewAction.OnViewUpdate(this);
    }
}

public class TileView
{

}

public class BoardView
{

}
