
using UnityEngine;
using System.Collections;

[AddComponentMenu("RPG Kit/Controls/Touch Camera")]
public class TouchCamera : BaseCamera
{
	public float distance = 10.0f;
	public float minHeight = 5.0f;
	public float height = 5.0f;
	public float maxHeight = 30.0f;
	public float heightDamping = 2.0f;
	
	public bool allowRotation = true;
	public bool allowZoom = true;
	public float rotation = 0;
	public float rotationDamping = 3.0f;
	
	public float rotationFactor = 1;
	public float zoomFactor = 1;
	
	public string zoomPlusKey = "";
	public string zoomMinusKey = "";
	public float zoomKeyChange = 3;
	
	public MouseTouchControl mouseTouch = new MouseTouchControl(false, 1, true, 2, 1);
	
	protected override void ExecuteCamera(Transform target)
	{
		if(target)
		{
			Vector3 add = Vector3.zero;
			if((this.allowRotation || this.allowZoom) &&
				this.mouseTouch.Interacted(ref add))
			{
				add = this.mouseTouch.GetLastChange();
				if(this.allowRotation) this.rotation += add.x*this.rotationFactor;
				if(this.allowZoom) this.height += add.y*this.zoomFactor;
				if(this.rotation < 0) this.rotation += 360;
				else if(this.rotation > 360) this.rotation -= 360;
			}
			if(this.allowZoom && ControlHandler.IsPressed(this.zoomPlusKey))
			{
				this.height -= this.zoomKeyChange;
			}
			else if(this.allowZoom && ControlHandler.IsPressed(this.zoomMinusKey))
			{
				this.height += this.zoomKeyChange;
			}
			
			if(this.height < this.minHeight) this.height = this.minHeight;
			else if(this.height > this.maxHeight) this.height = this.maxHeight;
			
			float wantedHeight = target.position.y + this.height;
			float currentHeight = transform.position.y;
			if(this.heightDamping != 0)
			{
				currentHeight = Mathf.Lerp(currentHeight, wantedHeight, this.heightDamping * Time.deltaTime);
			}
			else currentHeight = wantedHeight;
			
			Vector3 pos = target.position - Vector3.forward * distance;
			pos.y = currentHeight;
			transform.position = pos;
			
			float wantedRotation = this.rotation;
			if(this.rotationDamping != 0)
			{
				wantedRotation = Mathf.LerpAngle(transform.eulerAngles.y, this.rotation, this.rotationDamping * Time.deltaTime);
			}
			transform.RotateAround(target.position, Vector3.up, wantedRotation);
			transform.LookAt(target);
		}
	}
	
	protected override void DontExecuteCamera()
	{
		
	}
}