
using UnityEngine;
using System.Collections;

public class FieldAnimator : MonoBehaviour
{
	public float walkSpeed = 3.0f;
	public float runSpeed = 8.0f;
	public float fadeLength = 0.1f;
	public float minFallTime = 0.3f;
	
	public string idleAnimation = "idle";
	public string walkAnimation = "walk";
	public string runAnimation = "run";
	public string sprintAnimation = "sprint";
	
	public string jumpAnimation = "jump";
	public string fallAnimation = "fall";
	public string landAnimation = "land";
	private bool inAir = false;
	
	private Combatant combatant = null;
	private CharacterController controller = null;
	
	private bool stopped = true;
	
	private string lastBattleIdle = "";
	private string lastBattleWalk = "";
	private string lastBattleRun = "";
	private string lastBattleSprint = "";
	private string lastBattleJump = "";
	private string lastBattleFall = "";
	private string lastBattleLand = "";
	
	// 0 == default, 1 == battle
	private int mode = 0;
	private float updateCounter = 0;
	
	private Animation anim = null;
	
	private float lastTimeFactor = 1;
	private float fallTime = 0;
	
	void Start()
	{
		CombatantClick cc = (CombatantClick)this.transform.root.GetComponent(typeof(CombatantClick));
		if(cc == null)
		{
			cc = (CombatantClick)this.transform.root.GetComponentInChildren(typeof(CombatantClick));
		}
		if(cc != null)
		{
			this.combatant = cc.combatant;
			this.anim = this.combatant.GetAnimationComponent();
			this.controller = this.GetComponent<CharacterController>();
			this.anim.Stop();
			this.SetIdleAnimation(this.idleAnimation);
		}
		else
		{
			GameObject.Destroy(this);
		}
	}
	
	void Update()
	{
		if(this.controller)
		{
			Vector3 horizontalVelocity = this.controller.velocity;
			horizontalVelocity.y = 0;
			float currentSpeed = horizontalVelocity.magnitude;
			
			if(GameHandler.IsControlBattle() && this.combatant != null)
			{
				if(this.mode == 0)
				{
					this.UpdateBattleAnimations();
					this.SetIdleAnimation(this.lastBattleIdle);
					this.mode = 1;
				}
				
				this.updateCounter += Time.deltaTime;
				if(this.updateCounter >= 1) this.UpdateBattleAnimations();
				
				if(this.controller.isGrounded || this.controller.velocity == Vector3.zero)
				{
					if(this.inAir)
					{
						this.StartCoroutine(this.FadeOut(this.lastBattleJump, this.fadeLength));
						this.StartCoroutine(this.FadeOut(this.lastBattleFall, this.fadeLength));
						if(this.fallTime >= this.minFallTime && this.HasAnimation(this.lastBattleLand))
						{
							this.anim.CrossFade(this.lastBattleLand, this.fadeLength);
						}
						this.StartCoroutine(this.PlayIn(this.lastBattleIdle, this.fadeLength));
						this.inAir = false;
					}
					
					if(currentSpeed > runSpeed && this.HasAnimation(this.lastBattleSprint))
					{
						this.anim.CrossFade(this.lastBattleSprint, this.fadeLength);
						this.stopped = false;
					}
					else if(this.HasAnimation(this.lastBattleRun) &&
						(currentSpeed > walkSpeed || 
						(!this.HasAnimation(this.lastBattleWalk) && currentSpeed > 0.2f)))
					{
						this.anim.CrossFade(this.lastBattleRun, this.fadeLength);
						this.stopped = false;
					}
					else if(currentSpeed > 0.2f && this.HasAnimation(this.lastBattleWalk))
					{
						this.anim.CrossFade(this.lastBattleWalk, this.fadeLength);
						this.stopped = false;
					}
					else if(!this.stopped)
					{
						this.stopped = true;
						this.StartCoroutine(this.FadeOut(this.lastBattleSprint, this.fadeLength));
						this.StartCoroutine(this.FadeOut(this.lastBattleRun, this.fadeLength));
						this.StartCoroutine(this.FadeOut(this.lastBattleWalk, this.fadeLength));
					}
				}
				else
				{
					if(!this.inAir)
					{
						this.StartCoroutine(this.FadeOut(this.lastBattleSprint, this.fadeLength));
						this.StartCoroutine(this.FadeOut(this.lastBattleRun, this.fadeLength));
						this.StartCoroutine(this.FadeOut(this.lastBattleWalk, this.fadeLength));
						this.StartCoroutine(this.FadeOut(this.lastBattleIdle, this.fadeLength));
						this.inAir = true;
						this.fallTime = 0;
					}
					else this.fallTime += GameHandler.DeltaTime;
					if(this.controller.velocity.y > 0 && this.HasAnimation(this.lastBattleJump))
					{
						this.anim.CrossFade(this.lastBattleJump, this.fadeLength);
					}
					else if(this.controller.velocity.y < 0 && this.HasAnimation(this.lastBattleFall))
					{
						this.anim.CrossFade(this.lastBattleFall, this.fadeLength);
						this.StartCoroutine(this.FadeOut(this.lastBattleJump, this.fadeLength));
					}
				}
			}
			else
			{
				if(this.mode == 1)
				{
					this.SetIdleAnimation(this.idleAnimation);
					this.mode = 0;
				}
				
				if(this.controller.isGrounded || this.controller.velocity == Vector3.zero)
				{
					if(this.inAir)
					{
						this.StartCoroutine(this.FadeOut(this.jumpAnimation, this.fadeLength));
						this.StartCoroutine(this.FadeOut(this.fallAnimation, this.fadeLength));
						if(this.fallTime >= this.minFallTime && this.HasAnimation(this.landAnimation))
						{
							this.anim.CrossFade(this.landAnimation, this.fadeLength);
						}
						this.StartCoroutine(this.PlayIn(this.idleAnimation, this.fadeLength));
						this.inAir = false;
					}
					if(currentSpeed > runSpeed && this.HasAnimation(this.sprintAnimation))
					{
						this.anim.CrossFade(this.sprintAnimation, this.fadeLength);
						this.stopped = false;
					}
					else if(this.HasAnimation(this.runAnimation) &&
						(currentSpeed > walkSpeed || 
						(!this.HasAnimation(this.walkAnimation) && currentSpeed > 0.2f)))
					{
						this.anim.CrossFade(this.runAnimation, this.fadeLength);
						this.stopped = false;
					}
					else if(currentSpeed > 0.2f && this.HasAnimation(this.walkAnimation))
					{
						this.anim.CrossFade(this.walkAnimation, this.fadeLength);
						this.stopped = false;
					}
					else if(!this.stopped)
					{
						this.stopped = true;
						this.StartCoroutine(this.FadeOut(this.sprintAnimation, this.fadeLength));
						this.StartCoroutine(this.FadeOut(this.runAnimation, this.fadeLength));
						this.StartCoroutine(this.FadeOut(this.walkAnimation, this.fadeLength));
					}
				}
				else
				{
					if(!this.inAir)
					{
						this.StartCoroutine(this.FadeOut(this.sprintAnimation, this.fadeLength));
						this.StartCoroutine(this.FadeOut(this.runAnimation, this.fadeLength));
						this.StartCoroutine(this.FadeOut(this.walkAnimation, this.fadeLength));
						this.StartCoroutine(this.FadeOut(this.idleAnimation, this.fadeLength));
						this.inAir = true;
						this.fallTime = 0;
					}
					else this.fallTime += GameHandler.DeltaTime;
					if(this.controller.velocity.y > 0 && this.HasAnimation(this.jumpAnimation))
					{
						this.anim.CrossFade(this.jumpAnimation, this.fadeLength);
					}
					else if(this.controller.velocity.y < 0 && this.HasAnimation(this.fallAnimation))
					{
						this.anim.CrossFade(this.fallAnimation, this.fadeLength);
						this.StartCoroutine(this.FadeOut(this.jumpAnimation, this.fadeLength));
					}
				}
			}
			
			if(this.lastTimeFactor != GameHandler.AnimationFactor)
			{
				this.ChangeAnimationSpeed(GameHandler.AnimationFactor);
			}
		}
	}
	
	public bool HasAnimation(string name)
	{
		return name != "" && this.anim[name] != null;
	}
	
	public void SetCombatant(Combatant c)
	{
		this.combatant = c;
	}
	
	public void SetIdleAnimation(string name)
	{
		this.anim.Stop();
		if(this.HasAnimation(name))
		{
			this.anim.Play(name);
		}
	}
	
	private IEnumerator PlayIn(string name, float time)
	{
		if(this.HasAnimation(name))
		{
			if(time > 0) yield return new WaitForSeconds(time);
			this.anim.Play(name);
		}
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
	
	private void UpdateBattleAnimations()
	{
		this.lastBattleIdle = this.combatant.GetAnimationName(CombatantAnimation.IDLE);
		this.lastBattleWalk = this.combatant.GetAnimationName(CombatantAnimation.WALK);
		this.lastBattleRun = this.combatant.GetAnimationName(CombatantAnimation.RUN);
		this.lastBattleSprint = this.combatant.GetAnimationName(CombatantAnimation.SPRINT);
		this.lastBattleJump = this.combatant.GetAnimationName(CombatantAnimation.JUMP);
		this.lastBattleFall = this.combatant.GetAnimationName(CombatantAnimation.FALL);
		this.lastBattleLand = this.combatant.GetAnimationName(CombatantAnimation.LAND);
		this.updateCounter = 0;
	}
	
	private void ChangeAnimationSpeed(float speed)
	{
		foreach(AnimationState state in this.anim)
		{
			state.speed = speed;
		}
		if(this.combatant != null)
		{
			this.combatant.UpdateAnimations();
		}
		this.lastTimeFactor = speed;
	}
}