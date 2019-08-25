using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : Tickable
{
	private Board gameBoard;
	private bool isResolved;

	private void Awake()
	{
		MeleePiece lewis_enemy = new MeleePiece("Lewis the Jesus Koh", 100, 1, true);
		MeleePiece junkai_enemy = new MeleePiece("Jun the Supreme Kai", 100, 2, true);
		MeleePiece jolyn_player = new MeleePiece("Jo Jo Lyn", 100, 3, false);
		MeleePiece nicholas_player = new MeleePiece("Nick Pepega Chua", 100, 4, false);
		gameBoard = new Board(8, 8);
		gameBoard.AddPieceToBoard(lewis_enemy, 7, 7);
		gameBoard.AddPieceToBoard(junkai_enemy, 4, 4);
		gameBoard.AddPieceToBoard(jolyn_player, 1, 2);
		gameBoard.AddPieceToBoard(nicholas_player, 0, 0);
		isResolved = false;
	}
	
	public override void Tick(long tick)
	{
		if (isResolved)
		{
			return;
		}

		List<Piece> piecesOnBoard = gameBoard.GetPiecesOnBoard();
		foreach (Piece currentPiece in piecesOnBoard)
		{
			if (currentPiece.IsDead()) // Self-explanatory.
			{
				continue;
			}

			if (!currentPiece.IsBusy()) // Not Attacking or Moving; attempt to find a Target.
			{
				List<Piece> activePiecesOnBoard = gameBoard.GetActivePiecesOnBoard();
				Piece nearestTarget = currentPiece.FindNearestTarget(activePiecesOnBoard); // Find a new Target (if any).
				Debug.Log(currentPiece.GetName() + " is Alive and Free.");
				if (nearestTarget == null) // There are no more enemies; game is Resolved.
				{
					Debug.Log("There are no more enemies for " + currentPiece.GetName() + " to target. Game is Resolved.");
					isResolved = true;
					return;
				}
				currentPiece.SetTarget(nearestTarget);
				currentPiece.SetIsMoving(true);
				Debug.Log(currentPiece.GetName() + " now has a Target of " + nearestTarget.GetName() + " and is going to move closer to it.");
			}
			else if (currentPiece.IsAttacking())
			{
				Piece target = currentPiece.GetTarget();
				if (!target.IsDead())
				{
					currentPiece.AttackTarget();
					Debug.Log(currentPiece.GetName() + " has attacked " + target.GetName() + " for " + currentPiece.GetAttackDamage() + " DMG, whose HP has dropped to " + target.GetHitPoints() + " HP."); ;
				}

				if (target.IsDead())
				{
					gameBoard.DeactivatePieceOnBoard(target);
					currentPiece.SetIsAttacking(false);
					Debug.Log(target.GetName() + " has died and " + currentPiece.GetName() + " is no longer attacking it.");
				}
			}
			else if (currentPiece.IsMoving())
			{
				Piece target = currentPiece.GetTarget();
				if (target.IsDead()) // If the Target dies before the current Piece actually attacks it; have to find a new Target.
				{
					currentPiece.SetIsAttacking(false);
					currentPiece.SetIsMoving(false);
					continue;
				}

				if (currentPiece.CanAttackTarget()) // The current Piece is in attacking range.
				{
					currentPiece.SetIsAttacking(true);
					currentPiece.SetIsMoving(false);
					continue;
				}

				if (currentPiece.HasLockedTile())
				{
					gameBoard.MovePieceToTile(currentPiece, currentPiece.GetLockedTile());
				}
				else
				{
					gameBoard.DeterminePieceLockedTile(currentPiece);
				}
			}
		}
	}
}
