using UnityEngine;

public class SkillState : State
{
    public override void OnStart(Piece piece, Board board)
    {
        // Need to fetch the correct spell.
        // Lewis: Bring in your if-else blocks here, and construct the correct skill.
        Interaction skill = new TestFishBallSkill(piece, piece.GetTarget());
        board.AddInteractionToProcess(skill);
        ticksRemaining = skill.ticksTotal; // Channelling/Casting Duration of the Spell.
        piece.SetCurrentManaPoints(0);
        Debug.Log(piece.GetName() + " has casted a skill!");
    }

    public override void OnViewStart(PieceView pieceView)
    {
        Piece target = pieceView.piece.GetTarget();
        Tile targetTile = target.GetCurrentTile();
        if (targetTile == null)
        {
            target.GetLockedTile();
        }
        if (targetTile == null)
        {
            Debug.Log("No target to look at, See AttackState.cs");
            return;
        }
        Vector3 tilePos = ViewManager.CalculateTileWorldPosition(targetTile);
        tilePos.y = 0.5f;
        pieceView.transform.LookAt(tilePos);
        pieceView.animator.Play("Cast", 0);
    }

    public override void OnViewFinish(PieceView pieceView)
    {
        pieceView.animator.Play("Idle", 0);
    }
}
