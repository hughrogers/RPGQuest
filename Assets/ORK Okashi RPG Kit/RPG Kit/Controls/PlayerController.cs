
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("RPG Kit/Controls/Player Controller")]
public class PlayerController : MonoBehaviour
{
	public bool moveDead = true;
	public bool useCharacterSpeed = false;
	public float runSpeed = 8.0f;
	
	// The gravity for the character
	public float gravity = Physics.gravity.y;
	// The gravity in controlled descent mode
	public float speedSmoothing = 10.0f;
	public float rotateSpeed = 500.0f;
	
	public bool firstPerson = false;
	public bool useCamDirection = true;
	public string verticalAxis = "Vertical";
	public string horizontalAxis = "Horizontal";
	
	// The current move direction in x-z
	private Vector3 moveDirection = Vector3.zero;
	// The current vertical speed
	private float verticalSpeed = 0.0f;
	// The current x-z move speed
	private float moveSpeed = 0.0f;
	private float lastMoveSpeed = 0.0f;
	
	// The last collision flags returned from controller.Move
	//private CollisionFlags collisionFlags;
	private bool blockMove = false;
	private Combatant combatant = null;
	private CharacterController controller = null;
	private Vector3 targetDirection = Vector3.zero;
	
	// jump settings
	public bool useJump = false;
	public string jumpKey = "";
	public float jumpDuration = 0.5f;
	public float jumpSpeed = -Physics.gravity.y;
	public EaseType jumpInterpolation = EaseType.EaseOutQuad;
	public float inAirModifier = 0.5f;
	public float jumpMaxGroundAngle = 45;
	private Function interpolate;
	
	private bool isJumping = false;
	private float jumpTime = 0;
	
	// sprint settings
	public bool useSprint = false;
	public string sprintKey = "";
	public float sprintFactor = 2.0f;
	
	public bool useEnergy = false;
	
	public bool maxEFormula = false;
	public float maxEnergy = 10.0f;
	public int meFormula = 0;
	
	public bool energyCFormula = false;
	public float energyConsume = 1.0f;
	public int ecFormula = 0;
	
	public bool energyRFormula = false;
	public float energyRegeneration = 1.0f;
	public int erFormula = 0;
	
	void Start()
	{
		this.moveDirection = transform.TransformDirection(Vector3.forward);
		if(this.useCharacterSpeed || !this.moveDead || 
			(this.useSprint && this.useEnergy))
		{
			CombatantClick cc = (CombatantClick)this.transform.root.GetComponent(typeof(CombatantClick));
			if(cc == null)
			{
				cc = (CombatantClick)this.transform.root.GetComponentInChildren(typeof(CombatantClick));
			}
			if(cc != null)
			{
				this.combatant = cc.combatant;
				if(this.maxEFormula)
				{
					this.combatant.sprintEnergyMax = DataHolder.Formula(this.meFormula).Calculate(this.combatant, this.combatant);
					this.combatant.sprintEnergy = this.combatant.sprintEnergyMax;
				}
				else
				{
					this.combatant.sprintEnergyMax = this.maxEnergy;
					this.combatant.sprintEnergy = this.maxEnergy;
				}
			}
		}
		this.interpolate = Interpolate.Ease(this.jumpInterpolation);
	}
	
	void Update()
	{
		if(this.controller == null)
		{
			this.controller = this.GetComponent<CharacterController>();
		}
		
		if(this.jumpTime < this.jumpDuration) this.jumpTime += GameHandler.DeltaMovementTime;
		else
		{
			this.jumpTime = this.jumpDuration;
			this.isJumping = false;
		}
		if(GameHandler.IsControlField() && !GameHandler.IsBlockControl())
		{
			this.blockMove = false;
			
			this.UpdateSmoothedMovementDirection();
			this.ApplyGravity();
			
			if(!(this.lastMoveSpeed == 0 && this.moveSpeed == 0))
			{
				// Calculate actual motion
				Vector3 movement;
				if(this.firstPerson)
				{
					movement = this.targetDirection * moveSpeed + new Vector3(0, this.verticalSpeed, 0);
				}
				else
				{
					movement = this.moveDirection * moveSpeed + new Vector3(0, this.verticalSpeed, 0);
				}
				movement *= GameHandler.DeltaMovementTime;
				
				// Move the controller
				this.controller.Move(movement);
			}
			this.lastMoveSpeed = moveSpeed;
			
			if(!this.firstPerson && this.useCamDirection)
			{
				this.transform.rotation = Quaternion.LookRotation(this.moveDirection);
			}
		}
		else
		{
			if(!this.blockMove)
			{
				this.blockMove = true;
				this.controller.Move(Vector3.zero);
			}
			this.moveDirection = transform.TransformDirection(Vector3.forward);
		}
	}
	
	private void UpdateSmoothedMovementDirection()
	{
		// Forward vector relative to the camera along the x-z plane	
		Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
		forward.y = 0;
		forward = forward.normalized;
	
		// Right vector relative to the camera
		// Always orthogonal to the forward vector
		Vector3 right = new Vector3(forward.z, 0, -forward.x);
		
		if(this.firstPerson || !this.useCamDirection)
		{
			forward = transform.TransformDirection(Vector3.forward);
			right = transform.TransformDirection(Vector3.right);
		}
		
		float v = 0.0f;
		float h = 0.0f;
		float speedMod = 1;
		
		if(GameHandler.IsControlField() && (this.combatant == null || 
			((this.moveDead || !this.combatant.isDead) && !this.combatant.IsStopMovement())))
		{
			v = ControlHandler.GetAxis(this.verticalAxis);
			h = ControlHandler.GetAxis(this.horizontalAxis);
			
			if(this.useCharacterSpeed && this.combatant != null)
			{
				this.runSpeed = this.combatant.GetMoveSpeed();
			}
			
			// jump
			if(this.useJump && this.controller.isGrounded && 
				ControlHandler.IsPressed(this.jumpKey))
			{
				RaycastHit hit;
				if(Physics.Raycast(this.controller.transform.position, -Vector3.up, out hit))
				{
					if(Vector3.Angle(Vector3.up, hit.normal) < this.jumpMaxGroundAngle)
					{
						this.isJumping = true;
						this.jumpTime = 0;
					}
				}
			}
			
			// sprint
			if(this.combatant != null)
			{
				if(this.useSprint && this.controller.isGrounded &&
					ControlHandler.IsHeld(this.sprintKey))
				{
					if(this.EnergyHandling(true)) speedMod = this.sprintFactor;
				}
				else this.EnergyHandling(false);
			}
		}
		// Target direction relative to the camera
		if(this.useCamDirection || this.firstPerson) this.targetDirection = h*right+v*forward;
		else if(!this.useCamDirection)
		{
			this.transform.Rotate(Vector3.up*h*this.rotateSpeed*GameHandler.DeltaMovementTime);
			this.targetDirection = v*this.transform.TransformDirection(Vector3.forward);
		}
		
		if(!this.controller.isGrounded) this.targetDirection *= this.inAirModifier;
		
		// We store speed and direction seperately,
		// so that when the character stands still we still have a valid forward direction
		// moveDirection is always normalized, and we only update it if there is user input.
		if(this.targetDirection != Vector3.zero)
		{
			// If we are really slow, just snap to the target direction
			if(this.moveSpeed < (runSpeed / 2) * 0.9f)
			{
				this.moveDirection = this.targetDirection.normalized;
			}
			else if(!this.firstPerson)
			{
				this.moveDirection = Vector3.RotateTowards(moveDirection, this.targetDirection, 
						this.rotateSpeed * Mathf.Deg2Rad * GameHandler.DeltaMovementTime, 1000).normalized;
			}
		}
		
		// Smooth the speed based on the current target direction
		float curSmooth = speedSmoothing * GameHandler.DeltaMovementTime;
		
		// Choose target speed
		//* We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
		float targetSpeed = Mathf.Min(this.targetDirection.magnitude, 1.0f);
		if(targetSpeed < 0.2) targetSpeed = 0;
		targetSpeed = targetSpeed*runSpeed*speedMod;
		this.moveSpeed = Mathf.Lerp(this.moveSpeed, targetSpeed, curSmooth);
	}
	
	private void ApplyGravity()
	{
		if(this.isJumping)
		{
			this.verticalSpeed = Interpolate.Ease(this.interpolate, 
					this.jumpSpeed, -this.jumpSpeed+0.1f, this.jumpTime, this.jumpDuration);
		}
		else this.verticalSpeed = gravity;
	}
	
	private bool EnergyHandling(bool use)
	{
		bool ok = true;
		if(this.useSprint && this.useEnergy)
		{
			// set max
			if(this.maxEFormula)
			{
				this.combatant.sprintEnergyMax = DataHolder.Formula(this.meFormula).Calculate(this.combatant, this.combatant);
			}
			// regenerate
			if(this.energyRFormula)
			{
				this.combatant.sprintEnergy += DataHolder.Formula(this.erFormula).Calculate(this.combatant, this.combatant)*GameHandler.DeltaMovementTime;
			}
			else this.combatant.sprintEnergy += this.energyRegeneration*GameHandler.DeltaMovementTime;
			// use
			if(use)
			{
				float tmp = 0;
				if(this.energyCFormula)
				{
					tmp = DataHolder.Formula(this.ecFormula).Calculate(this.combatant, this.combatant)*GameHandler.DeltaMovementTime;
				}
				else tmp = this.energyConsume*GameHandler.DeltaMovementTime;
				if(tmp > this.combatant.sprintEnergy) ok = false;
				else this.combatant.sprintEnergy -= tmp;
			}
			// max bounds
			if(this.combatant.sprintEnergy < 0) this.combatant.sprintEnergy = 0;
			else if(this.combatant.sprintEnergy > this.combatant.sprintEnergyMax)
			{
				this.combatant.sprintEnergy = this.combatant.sprintEnergyMax;
			}
		}
		return ok;
	}
}
