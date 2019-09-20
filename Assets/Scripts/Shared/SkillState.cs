using UnityEngine;

public class SkillState : State
{
    public override void OnStart(Piece piece, Board board)
    {
        Interaction skill;
        piece.SetCurrentManaPoints(0);

        skill = new ShapeshiftLingeringEffect(piece);
        if (piece.GetRace() == Enums.Race.Human && piece.GetClass() == Enums.Job.Druid)
            skill = new ShapeshiftSkill(piece, board);
        else if (piece.GetRace() == Enums.Race.Elf && piece.GetClass() == Enums.Job.Knight)
            skill = new ProtectAllySkill(piece, board);
        else if (piece.GetRace() == Enums.Race.Orc && piece.GetClass() == Enums.Job.Druid)
            skill = new BarkskinSkill(piece, board);
        else if (piece.GetRace() == Enums.Race.Elf && piece.GetClass() == Enums.Job.Priest)
            skill = new BlessingOfNatureSkill(piece, board);

        board.AddInteractionToProcess(skill);
        ticksRemaining = skill.ticksTotal; // Channelling/Casting Duration of the Spell.
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
