
using UnityEngine;
using System.Collections;

[AddComponentMenu("RPG Kit/Controls/Smooth Look At Camera")]
public class SmoothLookAt : BaseCamera
{
	public float damping = 6.0f;
	public bool smooth = true;
	
	void Start()
	{
	   	if(rigidbody) rigidbody.freezeRotation = true;
	}
	
	protected override void ExecuteCamera(Transform target)
	{
		if(target)
		{
			if (smooth)
			{
				// Look at and dampen the rotation
				Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
				transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
			}
			else
			{
				// Just lookat
				transform.LookAt(target);
			}
		}
	}
	
	protected override void DontExecuteCamera()
	{
		
	}
}