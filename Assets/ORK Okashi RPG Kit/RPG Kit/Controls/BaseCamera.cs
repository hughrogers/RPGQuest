
using UnityEngine;
using System.Collections;

public abstract class BaseCamera : MonoBehaviour
{
	void LateUpdate()
	{
		if(!GameHandler.IsGamePaused() && GameHandler.IsControlField())
		{
			GameObject obj = GameHandler.Party().GetPlayer();
			if(obj != null) this.ExecuteCamera(obj.transform);
		}
		else this.DontExecuteCamera();
	}
	
	protected abstract void ExecuteCamera(Transform target);
	
	protected abstract void DontExecuteCamera();
}
