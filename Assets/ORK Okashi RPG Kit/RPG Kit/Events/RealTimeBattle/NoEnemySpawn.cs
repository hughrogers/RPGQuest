
using UnityEngine;
using System.Collections;

[AddComponentMenu("RPG Kit/Real Time Battles/No Enemy Spawn")]
public class NoEnemySpawn : BaseColliderZone
{
	void Awake()
	{
		this.gameObject.layer = 2;
		if(this.collider == null) GameObject.Destroy(this.gameObject);
		else GameHandler.AddNoSpawn(this);
	}
}
