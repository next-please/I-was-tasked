﻿using UnityEngine;
using UnityEngine.UI;

public class SkillState : State
{
    private Interaction skill;
    public override void OnStart(Piece piece, Board board)
    {
        piece.SetCurrentManaPoints(0);
        skill = new ShapeshiftLingeringEffect(piece);
        if (!piece.IsEnemy())
        {
            if (piece.GetRace() == Enums.Race.Human && piece.GetClass() == Enums.Job.Druid)
                skill = new ShapeshiftSkill(piece, board);
            else if (piece.GetRace() == Enums.Race.Elf && piece.GetClass() == Enums.Job.Knight)
                skill = new ProtectAllySkill(piece, board);
            else if (piece.GetRace() == Enums.Race.Orc && piece.GetClass() == Enums.Job.Druid)
                skill = new BarkskinSkill(piece, board);
            else if (piece.GetRace() == Enums.Race.Elf && piece.GetClass() == Enums.Job.Priest)
                skill = new BlessingOfNatureSkill(piece, board);
            else if (piece.GetRace() == Enums.Race.Orc && piece.GetClass() == Enums.Job.Knight)
                skill = new RampageSkill(piece, board);
            else if (piece.GetRace() == Enums.Race.Elf && piece.GetClass() == Enums.Job.Rogue && !piece.GetTarget().IsDead())
                skill = new MarkForDeathSkill(piece, piece.GetTarget(), board);
            else if (piece.GetRace() == Enums.Race.Undead && piece.GetClass() == Enums.Job.Mage && board.GetActiveFriendliesOnBoard().Count > 0)
                skill = new FrostArmourSkill(piece, board.GetActiveFriendliesOnBoard()[board.GetRNGesus().Next(0, board.GetActiveFriendliesOnBoard().Count)], board);
            else if (piece.GetRace() == Enums.Race.Orc && piece.GetClass() == Enums.Job.Priest && board.GetActiveEnemiesOnBoard().Count > 0)
                skill = new CurseOfAgonySkill(piece, board.GetActiveEnemiesOnBoard()[board.GetRNGesus().Next(0, board.GetActiveEnemiesOnBoard().Count)], board);
            else if (piece.GetRace() == Enums.Race.Orc && piece.GetClass() == Enums.Job.Rogue && !piece.GetTarget().IsDead())
                skill = new EvicerateSkill(piece, piece.GetTarget(), board);
            else if (piece.GetRace() == Enums.Race.Human && piece.GetClass() == Enums.Job.Mage && !piece.GetTarget().IsDead())
                skill = new FireblastSkill(piece, piece.GetTarget());
            else if (piece.GetRace() == Enums.Race.Elf && piece.GetClass() == Enums.Job.Mage && board.GetActiveEnemiesOnBoard().Count > 0)
                skill = new MagicMissileSkill(piece, board.GetActiveEnemiesOnBoard()[board.GetRNGesus().Next(0, board.GetActiveEnemiesOnBoard().Count)], board);
            else if (piece.GetRace() == Enums.Race.Undead && piece.GetClass() == Enums.Job.Knight)
                skill = new RotSkill(piece, board);
            else if (piece.GetRace() == Enums.Race.Undead && piece.GetClass() == Enums.Job.Priest)
                skill = new UnholyAuraSkill(piece, board);
            else if (piece.GetRace() == Enums.Race.Orc && piece.GetClass() == Enums.Job.Mage && !piece.GetTarget().IsDead())
                skill = new ThunderstormSkill(piece, piece.GetTarget(), board);
            else if (piece.GetRace() == Enums.Race.Undead && piece.GetClass() == Enums.Job.Druid && !piece.GetTarget().IsDead())
                skill = new MoonfireSkill(piece, piece.GetTarget(), board);
            else if (piece.GetRace() == Enums.Race.Human && piece.GetClass() == Enums.Job.Priest)
                skill = new GreaterHealSkill(piece, board);
            else if (piece.GetRace() == Enums.Race.Human && piece.GetClass() == Enums.Job.Rogue && !piece.GetTarget().IsDead())
                skill = new CheapShotSkill(piece, piece.GetTarget(), board);
            else if (piece.GetRace() == Enums.Race.Undead && piece.GetClass() == Enums.Job.Rogue && board.GetActiveEnemiesOnBoard().Count > 0)
                skill = new ShadowStrikeSkill(piece, board.FindFarthestTarget(piece), board);
            else if (piece.GetRace() == Enums.Race.Human && piece.GetClass() == Enums.Job.Knight && board.GetActiveEnemiesOnBoard().Count > 0)
                skill = new ChargeSkill(piece, board.FindFarthestTarget(piece), board);
            else if (piece.GetRace() == Enums.Race.Elf && piece.GetClass() == Enums.Job.Druid)
                skill = new ForestSpiritsSkill(piece, board);
        }
        if (piece.IsEnemy())
        {
            if (piece.GetRace() == Enums.Race.Human && piece.GetClass() == Enums.Job.Rogue && !piece.GetTarget().IsDead())
                skill = new CheapShotSkill(piece, piece.GetTarget(), board);
            else if (piece.GetRace() == Enums.Race.Orc && piece.GetClass() == Enums.Job.Rogue && !piece.GetTarget().IsDead())
                skill = new EvicerateSkill(piece, piece.GetTarget(), board);
            else if (piece.GetRace() == Enums.Race.Human && piece.GetClass() == Enums.Job.Druid)
                skill = new ShapeshiftSkill(piece, board);
            else if (piece.GetRace() == Enums.Race.Human && piece.GetClass() == Enums.Job.Mage && !piece.GetTarget().IsDead())
                skill = new FireblastSkill(piece, piece.GetTarget());
            else if (piece.GetRace() == Enums.Race.Human && piece.GetClass() == Enums.Job.Priest)
                skill = new GreaterHealSkill(piece, board);
            else if (piece.GetRace() == Enums.Race.Undead && piece.GetClass() == Enums.Job.Rogue && board.GetActiveFriendliesOnBoard().Count > 0)
                skill = new ShadowStrikeSkill(piece, board.FindFarthestTarget(piece), board, (int)(ShadowStrikeSkill.shadowStrikeDefaultDamage * 0.3));
            else if (piece.GetRace() == Enums.Race.Undead && piece.GetClass() == Enums.Job.Priest)
                skill = new RaiseDeadSkill(piece, board);
            else if (piece.GetRace() == Enums.Race.Elf && piece.GetClass() == Enums.Job.Druid)
                skill = new ForestSpiritsSkill(piece, board);
            else if (piece.GetRace() == Enums.Race.Elf && piece.GetClass() == Enums.Job.Priest)
                skill = new BlessingOfNatureSkill(piece, board);
            else if (piece.GetRace() == Enums.Race.Elf && piece.GetClass() == Enums.Job.Knight)
                skill = new ProtectAllySkill(piece, board);
            else if (piece.GetRace() == Enums.Race.Undead && piece.GetClass() == Enums.Job.Knight)
                skill = new RotSkill(piece, board);
            else if (piece.GetRace() == Enums.Race.Elf && piece.GetClass() == Enums.Job.Rogue && board.GetActiveFriendliesOnBoard().Count > 0)
                skill = new MagicMissileSkill(piece, board.GetActiveFriendliesOnBoard()[board.GetRNGesus().Next(0, board.GetActiveFriendliesOnBoard().Count)], board);
            else if (piece.GetRace() == Enums.Race.Elf && piece.GetClass() == Enums.Job.Mage && board.GetActiveFriendliesOnBoard().Count > 0)
                skill = new MagicMissileSkill(piece, board.GetActiveFriendliesOnBoard()[board.GetRNGesus().Next(0, board.GetActiveFriendliesOnBoard().Count)], board);
            else if (piece.GetRace() == Enums.Race.Orc && piece.GetClass() == Enums.Job.Druid)
                skill = new BarkskinSkill(piece, board);
            else if (piece.GetRace() == Enums.Race.Undead && piece.GetClass() == Enums.Job.Druid && !piece.GetTarget().IsDead())
                skill = new MoonfireSkill(piece, piece.GetTarget(), board);
            else if (piece.GetRace() == Enums.Race.Undead && piece.GetClass() == Enums.Job.Mage)
                skill = new BerserkSkill(piece, board);
        }
        board.AddInteractionToProcess(skill);
        if (piece.multicast)
        {
            if (piece.GetRace() == Enums.Race.Undead && piece.GetClass() == Enums.Job.Mage && board.GetActiveFriendliesOnBoard().Count > 0)
                skill = new FrostArmourSkill(piece, board.GetActiveFriendliesOnBoard()[board.GetRNGesus().Next(0, board.GetActiveFriendliesOnBoard().Count)], board);
            else if (piece.GetRace() == Enums.Race.Human && piece.GetClass() == Enums.Job.Mage && !piece.GetTarget().IsDead())
                skill = new FireblastSkill(piece, piece.GetTarget());
            else if (piece.GetRace() == Enums.Race.Elf && piece.GetClass() == Enums.Job.Mage && board.GetActiveEnemiesOnBoard().Count > 0)
                skill = new MagicMissileSkill(piece, board.GetActiveEnemiesOnBoard()[board.GetRNGesus().Next(0, board.GetActiveEnemiesOnBoard().Count)], board);
            else if (piece.GetRace() == Enums.Race.Orc && piece.GetClass() == Enums.Job.Mage && !piece.GetTarget().IsDead())
                skill = new ThunderstormSkill(piece, piece.GetTarget(), board);
            board.AddInteractionToProcess(skill);
        }
        ticksRemaining = skill.ticksTotal; // Channelling/Casting Duration of the Spell.
        Debug.Log(piece.GetName() + " has casted a skill!");
    }

    public override void OnFinish(Piece piece, Board board)
    {
        // Implementation would change depending on the type of skill.
        Piece target = piece.GetTarget();
        if (target.IsDead())
        {
            board.DeactivatePieceOnBoard(target);
            Debug.Log(target.GetName() + " has died and " + piece.GetName() + " is no longer attacking it.");
        }
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
