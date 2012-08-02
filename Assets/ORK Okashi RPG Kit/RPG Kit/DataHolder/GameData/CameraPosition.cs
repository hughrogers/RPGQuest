
using UnityEngine;

public class CameraPosition
{
	public Vector3 position = Vector3.zero;
	public Vector3 rotation = Vector3.zero;
	
	public bool lookAt = false;
	public bool localPoint = false;
	
	public bool targetChild = false;
	public string childName = "";
	
	public bool ignoreXRotation = false;
	public bool ignoreYRotation = false;
	public bool ignoreZRotation = false;
	
	public bool setFoV = false;
	public float fieldOfView = 40.0f;
	
	public CameraPosition()
	{
		
	}
	
	public void Use(Transform camera, Transform target)
	{
		if(this.targetChild)
		{
			Transform t = target.Find(this.childName);
			if(t != null)
			{
				target = t;
			}
		}
		if(this.localPoint)
		{
			camera.position = target.TransformPoint(this.position);
		}
		else
		{
			camera.position = target.position+this.position;
		}
		if(this.setFoV && camera.camera != null)
		{
			camera.camera.fieldOfView = this.fieldOfView;
		}
		if(this.lookAt)
		{
			camera.LookAt(target);
			Vector3 v = Vector3.zero;
			if(this.ignoreXRotation) v.x = this.rotation.x;
			if(this.ignoreYRotation) v.y = this.rotation.y;
			if(this.ignoreZRotation) v.z = this.rotation.z;
			camera.Rotate(v);
		}
		else
		{
			camera.rotation = Quaternion.Euler(this.rotation.x, this.rotation.y, this.rotation.z);
		}
	}
}