using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece
{
	public string name;
	public int hitPoints;
	public int attackDamage;
	public int attackSpeed;
	public bool isEnemy;
	public long readyByTick;

	// Probably may not use these methdods.
	public abstract void ProcessAction();
	public abstract void MoveTo();
	public abstract void Attack();
}
