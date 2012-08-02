
using UnityEngine;
using System.Collections;

[AddComponentMenu("RPG Kit/Controls/No Click Move")]
public class NoClickMove : BaseColliderZone
{
	void Awake()
	{
		this.gameObject.layer = 2;
		if(this.collider == null) GameObject.Destroy(this.gameObject);
		else GameHandler.AddNoClickMove(this);
	}
}
