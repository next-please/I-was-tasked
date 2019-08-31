using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Implement these to allow view to update
// Floating Point Math is allowed
// Update ran every MonoBehaviour.Update
public interface IViewState
{
    void OnViewStart(PieceView pieceView);
    void OnViewUpdate(PieceView pieceView);
    void OnViewFinish(PieceView pieceView);
    void CallViewStartIfNeeded(PieceView pieceView);
    void CallViewFinishIfNeeded(PieceView pieceView);
}

// Implement these for simulation
// Floating Point Math is not allowed
// Update ran every FixedClock.Tick
public interface ISimState
{
    void OnStart(Piece piece, Board board);
    void OnTick(Piece piece, Board board);
    void OnFinish(Piece piece, Board board);
    bool hasFinished();
}

public abstract class State : IViewState, ISimState
{
    public int ticksRemaining;
    protected State nextState;

    protected bool shouldCallViewFinish = false;
    protected bool shouldCallViewStart = true;

    public virtual void OnStart(Piece piece, Board board)
    {
        ticksRemaining = 1;
    }

    public virtual void OnTick(Piece piece, Board board)
    {
        ticksRemaining--;
    }

    public bool hasFinished()
    {
        return ticksRemaining <= 0;
    }

    public void SetNextState(State state)
    {
        nextState = state;
    }

    public State TransitNextState(Piece piece)
    {
        shouldCallViewFinish = true;
        if (nextState == null)
        {
            return null;
        }
        nextState.shouldCallViewStart = true;
        return nextState;
    }

    public void CallViewFinishIfNeeded(PieceView pieceView)
    {
        if (shouldCallViewFinish)
        {
            OnViewFinish(pieceView);
            shouldCallViewFinish = false;
        }
    }

    public void CallViewStartIfNeeded(PieceView pieceView)
    {
        if (shouldCallViewStart)
        {
            OnViewStart(pieceView);
            shouldCallViewStart = false;
        }
    }

    public virtual void OnFinish(Piece piece, Board board) { }
    public virtual void OnViewStart(PieceView pieceView) { }
    public virtual void OnViewUpdate(PieceView pieceView) { }
    public virtual void OnViewFinish(PieceView pieceView) { }
}
