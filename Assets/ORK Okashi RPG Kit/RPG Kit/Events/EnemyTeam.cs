
using UnityEngine;
using System.Collections;

[System.Serializable]
public class EnemyTeam
{
	public float chance = 10.0f;
	
	public int[] enemyID = new int[1];
	
	// inspector
	public bool fold = true;
	
	/*
	============================================================================
	Inspector functions
	============================================================================
	*/
	public void AddEnemy()
	{
		this.enemyID = ArrayHelper.Add(0, this.enemyID);
	}
	
	public void RemoveEnemy(int index)
	{
		this.enemyID = ArrayHelper.Remove(index, this.enemyID);
	}
}
