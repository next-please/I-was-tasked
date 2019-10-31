using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseDeadSkill : Interaction
{
    private Piece caster;
    private Board board;
    private Tile targetTile;
    private bool failedToFindSpot;
    public int ticksTilActivation = 0;

    public RaiseDeadSkill(Piece caster, Board board)
    {
        this.caster = caster;
        this.board = board;
        this.ticksRemaining = ticksTilActivation;
        this.ticksTotal = 50;

        failedToFindSpot = false;
        int targetRow = caster.GetCurrentTile().GetRow();
        int targetCol = caster.GetCurrentTile().GetCol();
        if (!board.GetTile(targetRow, targetCol - 1).IsLocked() && !board.GetTile(targetRow, targetCol - 1).IsOccupied())
        {
            targetTile = board.GetTile(targetRow, targetCol - 1);
        }
        else if (!board.GetTile(targetRow - 1, targetCol - 1).IsLocked() && !board.GetTile(targetRow - 1, targetCol - 1).IsOccupied())
        {
            targetTile = board.GetTile(targetRow - 1, targetCol - 1);
        }
        else if (!board.GetTile(targetRow + 1, targetCol - 1).IsLocked() && !board.GetTile(targetRow + 1, targetCol - 1).IsOccupied())
        {
            targetTile = board.GetTile(targetRow + 1, targetCol - 1);
        }
        else if (!board.GetTile(targetRow + 1, targetCol).IsLocked() && !board.GetTile(targetRow + 1, targetCol).IsOccupied())
        {
            targetTile = board.GetTile(targetRow + 1, targetCol);
        }
        else if (!board.GetTile(targetRow - 1, targetCol).IsLocked() && !board.GetTile(targetRow - 1, targetCol).IsOccupied())
        {
            targetTile = board.GetTile(targetRow - 1, targetCol);
        }
        else if (!board.GetTile(targetRow, targetCol + 1).IsLocked() && !board.GetTile(targetRow, targetCol + 1).IsOccupied())
        {
            targetTile = board.GetTile(targetRow, targetCol + 1);
        }
        else if (!board.GetTile(targetRow - 1, targetCol + 1).IsLocked() && !board.GetTile(targetRow - 1, targetCol + 1).IsOccupied())
        {
            targetTile = board.GetTile(targetRow - 1, targetCol + 1);
        }
        else if (!board.GetTile(targetRow + 1, targetCol + 1).IsLocked() && !board.GetTile(targetRow + 1, targetCol + 1).IsOccupied())
        {
            targetTile = board.GetTile(targetRow + 1, targetCol + 1);
        }
        else
        {
            failedToFindSpot = true;
            targetTile = board.GetTile(targetRow, targetCol);
        }
    }

    public override bool ProcessInteraction()
    {
        if (ticksRemaining > 0)
        {
            ticksRemaining--;
            return true;
        }
        else
        {
            ApplyEffect();
            return false;
        }
    }

    public override void CleanUpInteraction()
    {
        interactionView.CleanUpInteraction();
    }

    public override bool ProcessInteractionView()
    {
        return false;
    }

    private void ApplyEffect()
    {
        if (caster.IsDead() || caster.GetState().GetType() == typeof(InfiniteState) || failedToFindSpot)
        {
            return;
        }

        Piece raisedEnemy = new CharacterGenerator().GenerateCharacter(0, Enums.Job.Rogue, Enums.Race.Human);
        raisedEnemy.SetClass(Enums.Job.Knight);
        raisedEnemy.SetRace(Enums.Race.Orc);
        raisedEnemy.SetIsEnemy(true);
        raisedEnemy.SetName("Undead");
        raisedEnemy.SetTitle("");
        raisedEnemy.SetRarity(1);
        raisedEnemy.SetDamageIfSurvive(0);

        board.AddPieceToBoard(raisedEnemy, targetTile.GetRow(), targetTile.GetCol());

        Debug.Log(caster.GetName() + " has RaiseDead-ed.");
    }
}
