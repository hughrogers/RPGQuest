
using UnityEngine;

using System.Collections;

public class CameraEventMover : MonoBehaviour
{
	private Transform cam;
	private float time;
	private float time2;
	private float hitTime;

	// mover
	private bool running = false;
	private CameraPosition camPos;
	private Transform actor;
	
	private Function interpolate;
	private Vector3 startPos;
	private Vector3 distancePos;
	private Quaternion startRot;
	private Quaternion endRot;
	private float startFoV;
	private float distanceFov;
	
	// shaker
	private bool shaking = false;
	private float intensity;
	private float originalPosition;
	private float shakeSpeed;
	
	// rotater
	private bool rotating= false;
	private float rotateSpeed ;
	private Vector3 rotateAngle;
	
	public IEnumerator SetTargetData(CameraPosition cp, Transform c, Transform a, EaseType et, float t)
	{
		this.shaking = false;
		this.running = false;
		this.rotating = false;
		
		this.camPos = cp;
		this.cam = c;
		this.actor = a;
		this.interpolate = Interpolate.Ease(et);
		this.time = 0;
		this.time2 = t;
		
		Transform tmp = new GameObject().transform;
		this.camPos.Use(tmp, this.actor);
		yield return null;
		this.startPos = this.cam.position;
		this.distancePos = tmp.position - this.startPos;
		this.startRot = this.cam.rotation;
		this.endRot = tmp.rotation;
		this.startFoV = this.cam.camera.fieldOfView;
		this.distanceFov = this.camPos.fieldOfView - this.startFoV;
		GameObject.Destroy(tmp.gameObject);
		this.running = true;
	}
	
	public IEnumerator SetTargetData(Vector3 pos, Quaternion rot, float fov, Transform c, EaseType et, float t)
	{
		this.shaking = false;
		this.running = false;
		this.rotating = false;
		
		this.cam = c;
		this.interpolate = Interpolate.Ease(et);
		this.time = 0;
		this.time2 = t;
		this.camPos = null;
		
		yield return null;
		this.startPos = this.cam.position;
		this.distancePos = pos - this.startPos;
		this.startRot = this.cam.rotation;
		this.endRot = rot;
		this.startFoV = this.cam.camera.fieldOfView;
		this.distanceFov = fov - this.startFoV;
		this.running = true;
	}
	
	public void CameraShake(Transform c, float t, float i, float s)
	{
		this.shaking = false;
		this.running = false;
		this.rotating = false;
		
		this.cam = c;
		this.time = 0;
		this.time2 = t;
		this.intensity = i;
		this.shakeSpeed = s;
		
		this.hitTime = Time.time;
		this.originalPosition = this.cam.localPosition.x;
		this.shaking = true;
	}
	
	public void CameraRotate(Transform c, Transform a, Vector3 ra, float t, float s)
	{
		this.shaking = false;
		this.running = false;
		this.rotating = false;
		
		this.cam = c;
		this.actor = a;
		this.rotateAngle = ra;
		this.time = 0;
		this.time2 = t;
		this.rotateSpeed = s;
		
		this.rotating = true;
	}
	
	void Update()
	{
		if(this.running && !GameHandler.IsGamePaused())
		{
			this.time += GameHandler.DeltaTime;
			this.cam.position = Interpolate.Ease(this.interpolate, this.startPos, this.distancePos, this.time, this.time2);
			this.cam.rotation = Interpolate.Ease(this.interpolate, this.startRot, this.endRot, this.time, this.time2);
			this.cam.camera.fieldOfView = Interpolate.Ease(this.interpolate, this.startFoV, this.distanceFov, this.time, this.time2);
			if(this.time >= this.time2)
			{
				this.running = false;
			}
		}
		else if(this.shaking && !GameHandler.IsGamePaused())
		{
			this.time += GameHandler.DeltaTime;
			float timer = (Time.time - hitTime) * shakeSpeed;
			float factor =  (1-((this.time2-this.time)/this.time2));
			this.cam.localPosition = new Vector3(originalPosition + Mathf.Sin(timer) * this.intensity * factor, this.cam.localPosition.y, this.cam.localPosition.z);
			if (timer > Mathf.PI * 2) hitTime = Time.time;
			if(this.time >= this.time2)
			{
				this.cam.localPosition = new Vector3(originalPosition, this.cam.localPosition.y, this.cam.localPosition.z);
				this.shaking = false;
			}
		}
		else if(this.rotating && !GameHandler.IsGamePaused())
		{
			this.time += GameHandler.DeltaTime;
			this.cam.RotateAround(this.actor.position, this.rotateAngle, this.rotateSpeed * GameHandler.DeltaTime);
			if(this.time >= this.time2)
			{
				this.rotating = false;
			}
		}
	}
}