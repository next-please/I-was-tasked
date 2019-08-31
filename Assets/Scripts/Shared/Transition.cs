using System.Collections.Generic;

public class Transition : State
{
    Predicate predicate;
    State onTrue = null;
    State onFalse = null;

    public Transition(Predicate predicate)
    {
        this.predicate = predicate;
    }

    public void SetNextStates(State trueState, State falseState)
    {
        onTrue = trueState;
        onFalse = falseState;
    }

    public override void OnStart(Piece piece, Board board)
    {
        ticksRemaining = 0;
        nextState = predicate.IsTrue(piece) ? onTrue : onFalse;
    }
}
