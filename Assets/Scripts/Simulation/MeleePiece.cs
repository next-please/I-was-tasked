using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleePiece : Piece
{
	public MeleePiece(string name, int hitPoints, int attackDamage, int attackSpeed, bool isEnemy)
	{
		this.name = name;
		this.hitPoints = hitPoints;
		this.attackDamage = attackDamage;
		this.attackSpeed = attackSpeed;
		this.isEnemy = isEnemy;
		this.readyByTick = 0;
	}

	public override void ProcessAction()
	{
		// To be completed.
	}

	public override void MoveTo()
	{
		// To be completed.
	}

	public override void Attack()
	{
		// To be completed.
	}
}
