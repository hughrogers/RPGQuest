
using UnityEngine;
using System.Collections;

[AddComponentMenu("RPG Kit/Controls/First Person Camera")]
public class FirstPersonCamera : BaseCamera
{
	public string onChild = "";
	public Vector3 offset = Vector3.zero;
	
	public string horizontalAxis = "";
	public string verticalAxis = "";
	
	public Vector2 sensitivity = new Vector2(15, 15);
	
	public bool lockCursor = false;
	
	protected override void ExecuteCamera(Transform target)
	{
		if(target)
		{
			if(this.lockCursor)
			{
				Screen.lockCursor = true;
			}
			
			float rotX = ControlHandler.GetAxis(this.horizontalAxis)*this.sensitivity.x;
			float rotY = ControlHandler.GetAxis(this.verticalAxis)*this.sensitivity.y;
			
			// X on player
			target.Rotate(0, rotX, 0);
			
			//  Y on camera
			Vector3 tmp = this.transform.eulerAngles;
			tmp.y = target.eulerAngles.y;
			tmp.z = 0;
			tmp.x -= rotY;
			this.transform.eulerAngles = tmp;
			
			if(this.onChild != "")
			{
				Transform t = target.Find(this.onChild);
				if(t != null) target = t;
			}
			this.transform.position = target.position+target.TransformDirection(this.offset);
		}
	}
	
	protected override void DontExecuteCamera()
	{
		if(this.lockCursor)
		{
			Screen.lockCursor = false;
			Screen.showCursor = true;
		}
	}
}
