using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Implement these to allow view to update
// Floating Point Math is allowed
// Update ran every MonoBehaviour.Update
public interface IViewAction
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
public interface ISimAction
{
    void OnStart(Piece piece, Board board);
    void OnTick(Piece piece, Board board);
    void OnFinish(Piece piece, Board board);
    bool hasFinished();
}

// Idea: Action is just State in FSM that can only transit after its duration ends
public abstract class Action : IViewAction, ISimAction
{
    public int ticksRemaining;
    private Piece piece; // Piece the action belongs to
    private List<Action> nextActions = new List<Action>();

    private bool shouldCallViewFinish = false;
    private bool shouldCallViewStart = true;

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

    public void AddNextAction(Action action)
    {
        // perhaps do priority sorting here
        nextActions.Add(action);
    }

    public Action TransitNextAction(Piece piece)
    {
        shouldCallViewFinish = true;
        foreach (Action action in nextActions)
        {
            if (action.ShouldTransitInto(piece))
            {
                action.shouldCallViewStart = true;
                return action;
            }
        }
        return null;
    }

    public virtual bool ShouldTransitInto(Piece piece)
    {
        // we could do a checking, eg: Mana Check before spell
        return true;
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
