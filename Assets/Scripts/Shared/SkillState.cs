using UnityEngine;
using UnityEngine.UI;
using System;

public class SkillState : State
{
    private Interaction skill;
    public override void OnStart(Piece piece, Board board)
    {
        skill = null;
        ticksRemaining = 1; // Channelling/Casting Duration of the Spell.

        if (!piece.IsEnemy())
        {
            if (piece.spell == Enums.Spell.Shapeshift)
                skill = new ShapeshiftSkill(piece, board);
            else if (piece.spell == Enums.Spell.ProtectAlly)
                skill = new ProtectAllySkill(piece, board);
            else if (piece.spell == Enums.Spell.Barkskin)
                skill = new BarkskinSkill(piece, board);
            else if (piece.spell == Enums.Spell.BlessingOfNature)
                skill = new BlessingOfNatureSkill(piece, board);
            else if (piece.spell == Enums.Spell.Rampage)
                skill = new RampageSkill(piece, board);
            else if (piece.spell == Enums.Spell.MarkForDeath)
                skill = new MarkForDeathSkill(piece, board.FindNearestTarget(piece), board);
            else if (piece.spell == Enums.Spell.FrostArmour)
                skill = new FrostArmourSkill(piece, board);
            else if (piece.spell == Enums.Spell.CurseOfAgony)
                skill = new CurseOfAgonySkill(piece, board.GetActiveEnemiesOnBoard()[board.GetRNGesus().Next(0, board.GetActiveEnemiesOnBoard().Count)], board);
            else if (piece.spell == Enums.Spell.Evicerate)
                skill = new EvicerateSkill(piece, board.FindNearestTarget(piece), board);
            else if (piece.spell == Enums.Spell.Fireblast)
                skill = new FireblastSkill(piece, board.FindNearestTarget(piece));
            else if (piece.spell == Enums.Spell.MagicMissile)
                skill = new MagicMissileSkill(piece, board.GetActiveEnemiesOnBoard()[board.GetRNGesus().Next(0, board.GetActiveEnemiesOnBoard().Count)], board);
            else if (piece.spell == Enums.Spell.Rot)
                skill = new RotSkill(piece, board);
            else if (piece.spell == Enums.Spell.UnholyAura)
                skill = new UnholyAuraSkill(piece, board);
            else if (piece.spell == Enums.Spell.Thunderstorm)
                skill = new ThunderstormSkill(piece, board.FindNearestTarget(piece), board);
            else if (piece.spell == Enums.Spell.Moonbeam)
                skill = new MoonfireSkill(piece, board.FindNearestTarget(piece), board);
            else if (piece.spell == Enums.Spell.GreaterHeal)
                skill = new GreaterHealSkill(piece, board);
            else if (piece.spell == Enums.Spell.CheapShot)
                skill = new CheapShotSkill(piece, board.FindNearestTarget(piece), board);
            else if (piece.spell == Enums.Spell.ShadowStrike)
                skill = new ShadowStrikeSkill(piece, board.FindFarthestTarget(piece), board);
            else if (piece.spell == Enums.Spell.Charge)
                skill = new ChargeSkill(piece, board.FindFarthestTarget(piece), board);
            else if (piece.spell == Enums.Spell.ForestSpirits)
                skill = new ForestSpiritsSkill(piece, board);
        }
        if (piece.IsEnemy())
        {
            if (piece.spell == Enums.Spell.CheapShot)
                skill = new CheapShotSkill(piece, board.FindNearestTarget(piece), board);
            else if (piece.spell == Enums.Spell.Evicerate)
                skill = new EvicerateSkill(piece, board.FindNearestTarget(piece), board);
            else if (piece.spell == Enums.Spell.Shapeshift)
                skill = new ShapeshiftSkill(piece, board);
            else if (piece.spell == Enums.Spell.Fireblast)
                skill = new FireblastSkill(piece, board.FindNearestTarget(piece));
            else if (piece.spell == Enums.Spell.GreaterHeal)
                skill = new GreaterHealSkill(piece, board);
            else if (piece.spell == Enums.Spell.ShadowStrike)
                skill = new ShadowStrikeSkill(piece, board.FindFarthestTarget(piece), board, (int)(ShadowStrikeSkill.shadowStrikeDefaultDamage * 0.9));
            else if (piece.spell == Enums.Spell.UnholyAura)
                skill = new UnholyAuraSkill(piece, board);
            //skill = new RaiseDeadSkill(piece, board);
            else if (piece.spell == Enums.Spell.ForestSpirits)
                skill = new ForestSpiritsSkill(piece, board);
            else if (piece.spell == Enums.Spell.BlessingOfNature)
                skill = new BlessingOfNatureSkill(piece, board);
            else if (piece.spell == Enums.Spell.ProtectAlly)
                skill = new ProtectAllySkill(piece, board);
            else if (piece.spell == Enums.Spell.Rot)
                skill = new RotSkill(piece, board);
            else if (piece.spell == Enums.Spell.InfiniteMagicMissile)
                skill = new MagicMissileSkill(piece, board.GetActiveFriendliesOnBoard()[board.GetRNGesus().Next(0, board.GetActiveFriendliesOnBoard().Count)], board, true);
            else if (piece.spell == Enums.Spell.MagicMissile)
                skill = new MagicMissileSkill(piece, board.GetActiveFriendliesOnBoard()[board.GetRNGesus().Next(0, board.GetActiveFriendliesOnBoard().Count)], board);
            else if (piece.spell == Enums.Spell.Barkskin)
                skill = new BarkskinSkill(piece, board);
            else if (piece.spell == Enums.Spell.Moonbeam)
                skill = new MoonfireSkill(piece, board.FindNearestTarget(piece), board);
            else if (piece.spell == Enums.Spell.Berserk)
                skill = new BerserkSkill(piece, board);
        }
        if (skill != null)
        {
            board.AddInteractionToProcess(skill);
            ticksRemaining = skill.ticksTotal; // Channelling/Casting Duration of the Spell.
        }
        if (piece.multicast && !piece.IsEnemy())
        {
            if (piece.spell == Enums.Spell.FrostArmour)
                skill = new FrostArmourSkill(piece, board);
            else if (piece.spell == Enums.Spell.Fireblast)
                skill = new FireblastSkill(piece, board.FindFarthestTarget(piece));
            else if (piece.spell == Enums.Spell.MagicMissile)
                skill = new MagicMissileSkill(piece, board.GetActiveEnemiesOnBoard()[board.GetRNGesus().Next(0, board.GetActiveEnemiesOnBoard().Count)], board);
            else if (piece.spell == Enums.Spell.Thunderstorm)
                skill = new ThunderstormSkill(piece, board.FindFarthestTarget(piece), board);
            board.AddInteractionToProcess(skill);
        }
        Debug.Log(piece.GetName() + " has casted a skill!");
    }

    public override void OnFinish(Piece piece, Board board)
    {
        // Implementation would change depending on the type of skill.
        piece.SetCurrentManaPoints(0);
        if (piece.spell == Enums.Spell.Moonbeam)
        {
            piece.SetCurrentManaPoints((int)Math.Floor(piece.GetMaximumManaPoints() * MoonfireSkill.moonfireDefaultManaRetainPercentage));
        }
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
        pieceView.pieceSounds.PlaySkillCastSound();
    }

    public override void OnViewFinish(PieceView pieceView)
    {
        pieceView.animator.Play("Idle", 0);
    }
}
