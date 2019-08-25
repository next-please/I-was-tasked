using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleePiece : Piece
{
	// Placeholder Constructor; actual Melee Piece would be more complex in attributes.
	public MeleePiece(string name, int hitPoints, int attackDamage, bool isEnemy)
	{
		SetName(name);
		SetHitPoints(hitPoints);
		SetAttackDamage(attackDamage);
		SetAttackRange(1);
		SetIsEnemy(isEnemy);
	}

	public override void AttackTarget()
	{
		Piece target = GetTarget();
		target.SetHitPoints(target.GetHitPoints() - GetAttackDamage());
	}
}
