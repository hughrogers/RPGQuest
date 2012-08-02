
using UnityEngine;
using System.Collections;

[AddComponentMenu("RPG Kit/Scenes/Destroy After Time")]
public class DestroyAfterTime : MonoBehaviour
{
	public float time = 1;
	
	void Update()
	{
		if(!GameHandler.IsGamePaused())
		{
			this.time -= GameHandler.DeltaTime;
			if(this.time <= 0) GameObject.Destroy(this.gameObject);
		}
	}
}
