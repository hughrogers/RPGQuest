
using System.Collections;
using UnityEngine;

public class ActorEventMover : MonoBehaviour
{
	private Transform actor;
	private Transform target;
	private Vector3 waypoint;
	
	private CharacterController controller;
	private bool applyGravity;
	
	private float time;
	private float time2;
	private float speed;
	
	private Function interpolate;
	private Vector3 startPos;
	private Vector3 distancePos;
	
	private float startY;
	private float distanceY;
	
	private bool speedToObject = false;
	private bool moveToWP = false;
	private bool moveToDir = false;
	private bool rotateToWP = false;
	private bool rotateDirection = false;
	
	private Vector3 direction = Vector3.zero;
	
	private BaseEvent callback = null;
	private int next = 0;
	
	public void StopMoving()
	{
		this.speedToObject = false;
		this.moveToWP = false;
		this.moveToDir = false;
		this.rotateToWP = false;
		this.rotateDirection = false;
		this.callback = null;
		this.interpolate = null;
	}
	
	/*
	============================================================================
	Move functions
	============================================================================
	*/
	public IEnumerator SpeedToObject(Transform a, bool ic, bool g, float s, float d, Transform t, BaseEvent cb, int n)
	{
		this.StopMoving();
		
		this.actor = a;
		if(ic) this.controller = (CharacterController)this.actor.GetComponent(typeof(CharacterController));
		this.applyGravity = g;
		this.speed = s;
		this.distanceY = d;
		this.target = t;
		this.callback = cb;
		this.next = n;
		
		yield return null;
		this.speedToObject = true;
	}
	
	public IEnumerator MoveToPosition(Transform a, bool ic, bool g, bool fd, Vector3 wp, EaseType et, float t)
	{
		this.StopMoving();
		
		this.actor = a;
		if(ic) this.controller = (CharacterController)this.actor.GetComponent(typeof(CharacterController));
		this.applyGravity = g;
		this.waypoint = wp;
		this.interpolate = Interpolate.Ease(et);
		this.time = 0;
		this.time2 = t;
		
		yield return null;
		if(fd) this.actor.LookAt(new Vector3(this.waypoint.x, this.actor.position.y, this.waypoint.z));
		this.startPos = this.actor.position;
		this.distancePos = this.waypoint - this.startPos;
		this.moveToWP = true;
	}
	
	public IEnumerator MoveToObject(Transform a, bool ic, bool g, bool fd, Transform wp, EaseType et, float t)
	{
		this.StopMoving();
		
		this.actor = a;
		if(ic) this.controller = (CharacterController)this.actor.GetComponent(typeof(CharacterController));
		this.applyGravity = g;
		this.waypoint = wp.position;
		this.interpolate = Interpolate.Ease(et);
		this.time = 0;
		this.time2 = t;
		
		yield return null;
		if(fd) this.actor.LookAt(new Vector3(this.waypoint.x, this.actor.position.y, this.waypoint.z));
		this.startPos = this.actor.position;
		this.distancePos = this.waypoint - this.startPos;
		this.moveToWP = true;
	}
	
	public IEnumerator MoveToDirection(Transform a, bool ic, Vector3 d, bool fd, float s, float t)
	{
		this.StopMoving();
		
		this.actor = a;
		if(ic) this.controller = (CharacterController)this.actor.GetComponent(typeof(CharacterController));
		this.waypoint = d;
		this.speed = s;
		this.time = 0;
		this.time2 = t;
		
		yield return null;
		if(fd) this.actor.LookAt(new Vector3(this.waypoint.x+this.actor.position.x, this.actor.position.y, this.waypoint.z+this.actor.position.z));
		this.moveToDir = true;
	}
	
	/*
	============================================================================
	Rotate functions
	============================================================================
	*/
	public IEnumerator RotateToObject(Transform a, Transform wp, EaseType et, float t)
	{
		this.StopMoving();
		
		this.actor = a;
		this.waypoint = wp.position;
		this.interpolate = Interpolate.Ease(et);
		this.time = 0;
		this.time2 = t;
		
		yield return null;
		Transform tmp = new GameObject().transform;
		tmp.position = this.actor.position;
		tmp.rotation = this.actor.rotation;
		tmp.LookAt(new Vector3(this.waypoint.x, tmp.position.y, this.waypoint.z));
		this.startY = this.actor.eulerAngles.y;
		this.distanceY = tmp.eulerAngles.y - this.startY;
		if(this.distanceY >= 190) this.distanceY -= 360;
		GameObject.Destroy(tmp.gameObject);
		this.rotateToWP = true;
	}
	
	public IEnumerator Rotation(Transform a, float s, float t, Vector3 d)
	{
		this.StopMoving();
		
		this.actor = a;
		this.speed = s;
		this.time = 0;
		this.time2 = t;
		this.direction = d*this.speed;
		
		yield return null;
		this.rotateDirection = true;
	}
	
	public IEnumerator Rotation(Transform a, float s, float t, Vector3 d, EaseType et)
	{
		this.StopMoving();
		
		this.actor = a;
		this.interpolate = Interpolate.Ease(et);
		this.time = 0;
		this.time2 = t;
		this.distancePos = d*s;
		this.startPos = this.actor.eulerAngles;
		
		yield return null;
		this.rotateDirection = true;
	}
	
	/*
	============================================================================
	Callbacks functions
	============================================================================
	*/
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if(this.speedToObject && 
			hit.gameObject == this.target.gameObject && 
			this.callback != null)
		{
			this.speedToObject = false;
			if(this.controller) this.controller.Move(Vector3.zero);
			this.callback.StepFinished(this.next);
		}
	}
	
	void OnCollisionEnter(Collision collision)
	{
		if(this.speedToObject && 
			collision.gameObject == this.target.gameObject && 
			this.callback != null)
		{
			this.speedToObject = false;
			if(this.controller) this.controller.Move(Vector3.zero);
			this.callback.StepFinished(this.next);
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(this.speedToObject && 
			other.gameObject == this.target.gameObject && 
			this.callback != null)
		{
			this.speedToObject = false;
			if(this.controller) this.controller.Move(Vector3.zero);
			this.callback.StepFinished(this.next);
		}
	}
	
	/*
	============================================================================
	Update functions
	============================================================================
	*/
	void Update()
	{
		if(!GameHandler.IsGamePaused())
		{
			if(this.speedToObject)
			{
				if(this.target != null)
				{
					this.actor.LookAt(new Vector3(this.target.position.x, this.actor.position.y, this.target.position.z));
					
					if(this.controller)
					{
						Vector3 moveDirection = Vector3.forward*this.speed;
						if(this.applyGravity) moveDirection.y = Physics.gravity.y;
						moveDirection = this.actor.TransformDirection(moveDirection*GameHandler.DeltaTime);
						this.controller.Move(moveDirection);
					}
					else
					{
						this.actor.position = Vector3.MoveTowards(this.actor.position, this.target.position, this.speed*GameHandler.DeltaTime);
					}
				}
				
				bool endMovement = target == null;
				if(!endMovement)
				{
					Vector3 v1 = actor.transform.position;
					v1.y = 0;
					Vector3 v2 = target.transform.position;
					v2.y = 0;
					endMovement = Vector3.Distance(v1, v2)-this.distanceY <= 0.2f;
				}
				
				if(endMovement)
				{
					this.speedToObject = false;
					if(this.callback != null)
					{
						if(this.controller) this.controller.Move(Vector3.zero);
						this.callback.StepFinished(this.next);
					}
				}
			}
			else if(this.moveToWP)
			{
				this.time += GameHandler.DeltaTime;
				if(this.controller)
				{
					Vector3 moveDirection = Interpolate.Ease(this.interpolate, this.startPos, this.distancePos, this.time, this.time2) - this.actor.position;
					if(this.applyGravity) moveDirection.y = Physics.gravity.y;
					this.controller.Move(moveDirection);
				}
				else
				{
					this.actor.position = Interpolate.Ease(this.interpolate, this.startPos, this.distancePos, this.time, this.time2);
				}
				if(this.time >= this.time2)
				{
					this.moveToWP = false;
					if(this.controller) this.controller.Move(Vector3.zero);
				}
			}
			else if(this.moveToDir)
			{
				this.time += GameHandler.DeltaTime;
				if(this.controller)
				{
					this.controller.Move(this.waypoint * this.speed * GameHandler.DeltaTime);
				}
				else
				{
					this.actor.position += this.waypoint * this.speed * GameHandler.DeltaTime;
				}
				if(this.time >= this.time2)
				{
					this.moveToDir = false;
					if(this.controller) this.controller.Move(Vector3.zero);
				}
			}
			else if(this.rotateToWP)
			{
				this.time += GameHandler.DeltaTime;
				this.actor.eulerAngles = new Vector3(this.actor.eulerAngles.x, 
						Interpolate.Ease(this.interpolate, this.startY, this.distanceY, this.time, this.time2),
						this.actor.eulerAngles.z);
				if(this.time >= this.time2)
				{
					this.rotateToWP = false;
				}
			}
			else if(this.rotateDirection)
			{
				this.time += GameHandler.DeltaTime;
				if(this.interpolate == null)
				{
					this.actor.Rotate(this.direction*GameHandler.DeltaTime);
				}
				else
				{
					this.actor.eulerAngles = Interpolate.Ease(this.interpolate, this.startPos, this.distancePos, this.time, this.time2);
				}
				if(this.time >= this.time2)
				{
					this.rotateDirection = false;
				}
			}
		}
	}
}