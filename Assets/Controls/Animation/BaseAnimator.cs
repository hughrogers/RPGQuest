
using UnityEngine;
using System.Collections;

[AddComponentMenu("RPG Kit/Controls/Base Animator")]
public class BaseAnimator : MonoBehaviour
{
	public float walkSpeed = 2.5f;
	private float runSpeedScale = 1.0f;
	private float walkSpeedScale = 1.0f;
	
	public string walkAnimation = "walk";
	public string runAnimation = "run";
	public string idleAnimation = "idle";
	
	public float fadeLength = 0.1f;
	
	private float lastTimeFactor = 1;
	
	private Animation anim = null;
	private CharacterController controller = null;
	
	private bool isMoving = false;
	
	void Start ()
	{
		this.controller = this.GetComponent<CharacterController>();
		this.anim = this.GetComponent<Animation>();
		if(this.anim == null) this.anim = this.GetComponentInChildren<Animation>();
		
		if(!GameHandler.IsControlBattle())
		{
			this.anim[runAnimation].layer = -1;
			this.anim[walkAnimation].layer = -1;
			this.anim[idleAnimation].layer = -2;
			this.anim.SyncLayer(-1);
	
			// We are in full control here - don't let any other animations play when we start
			this.anim.Stop();
			this.anim.Play(idleAnimation);
		}
	}
	
	void Update()
	{
		if(!GameHandler.IsControlBattle() && 
			this.controller != null && this.anim != null)
		{
			Vector3 horizontalVelocity = controller.velocity;
			horizontalVelocity.y = 0;
			float currentSpeed = horizontalVelocity.magnitude;
			if(currentSpeed > 0.2f) Debug.Log(this.gameObject.name+" speed="+currentSpeed);
			if(currentSpeed > this.walkSpeed && this.HasAnimation(this.runAnimation))
			{
				this.anim.CrossFade(this.runAnimation, this.fadeLength);
				this.isMoving = true;
			}
			else if(currentSpeed > 0.2f && this.HasAnimation(this.walkAnimation))
			{
				this.anim.CrossFade(this.walkAnimation, this.fadeLength);
				this.isMoving = true;
			}
			else if(this.isMoving)
			{
				this.isMoving = false;
				this.StartCoroutine(this.FadeOut(this.walkAnimation, this.fadeLength));
				this.StartCoroutine(this.FadeOut(this.runAnimation, this.fadeLength));
			}
			
			this.anim[this.runAnimation].normalizedSpeed = this.runSpeedScale;
			this.anim[this.walkAnimation].normalizedSpeed = this.walkSpeedScale;
			
			if(this.lastTimeFactor != GameHandler.AnimationFactor)
			{
				this.ChangeAnimationSpeed(GameHandler.AnimationFactor);
			}
		}
	}
	
	private void ChangeAnimationSpeed(float speed)
	{
		foreach(AnimationState state in this.anim)
		{
			state.speed = speed;
		}
		this.lastTimeFactor = speed;
	}
	
	private IEnumerator FadeOut(string name, float time)
	{
		if(this.HasAnimation(name))
		{
			this.anim.Blend(name, 0, time);
			if(time > 0) yield return new WaitForSeconds(time);
			this.anim.Stop(name);
		}
	}
	
	public bool HasAnimation(string name)
	{
		return name != "" && this.anim[name] != null;
	}
}