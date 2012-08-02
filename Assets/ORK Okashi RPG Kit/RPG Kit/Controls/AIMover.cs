
using UnityEngine;
using System.Collections;

public class AIMover : MonoBehaviour
{
	public bool useCombatantSpeed = false;
	public float runSpeed = 8.0f;
	public float gravity = Physics.gravity.y;
	public float speedSmoothing = 10.0f;
	public bool ignoreYDistance = false;
	
	// party follow
	public float minFollowDistance = 3.0f;
	public bool giveWay = false;
	public float giveWayDistance = 1.5f;
	
	public bool moveAway = false;
	
	// enemy random patrol
	public bool randomPatrol = false;
	public float patrolRadius = 20.0f;
	public float patrolTime = 4.0f;
	
	public float changeTimeout = 1.0f;
	
	public float playerDistance = 120.0f;
	
	public bool autoRespawn = false;
	public float respawnDistance = 100.0f;
	
	public bool moveDead = false;
	
	// ingame
	private CharacterController controller = null;
	private bool partyFollow = false;
	private Combatant combatant = null;
	private float timeout = 0;
	
	private float moveSpeed = 0.0f;
	private Vector3 moveDirection = Vector3.zero;
	private GameObject target = null;
	private bool moveStopped = false;
	private bool stopFollow = false;
	
	public float waypointTime = 5.0f;
	private Vector3[] waypoint = new Vector3[0];
	private int wpIndex = 0;
	private float wpTimeout = 0;
	
	private Vector3 initialPosition = Vector3.zero;
	
	private bool isCharacter = false;
	
	void Start()
	{
		this.controller = this.GetComponent<CharacterController>();
		CombatantClick cc = (CombatantClick)this.transform.root.GetComponent(typeof(CombatantClick));
		if(cc == null)
		{
			cc = (CombatantClick)this.transform.root.GetComponentInChildren(typeof(CombatantClick));
		}
		if(cc != null)
		{
			this.combatant = cc.combatant;
			if(this.combatant is Character)
			{
				this.isCharacter = true;
				if(DataHolder.GameSettings().partyFollow)
				{
					this.partyFollow = true;
				}
			}
		}
		this.timeout = Random.Range(0, this.changeTimeout);
		this.initialPosition = this.transform.position;
		if(this.randomPatrol) this.RandomWaypoint();
		
	}
	
	void Update()
	{
		if(GameHandler.IsControlField() && 
			this.combatant != null && (this.moveDead || !this.combatant.isDead) &&
			!this.combatant.IsInAction() && !this.combatant.autoAttackStarted && 
			this.combatant != GameHandler.Party().GetPlayerCharacter() && 
			DataHolder.BattleSystem().DoCombatantTick() && 
			!this.combatant.IsStopMovement())
		{
			GameObject player = GameHandler.Party().GetPlayer();
			if(this.useCombatantSpeed)
			{
				this.runSpeed = this.combatant.GetMoveSpeed();
			}
			
			if(this.combatant.aiAction == null && 
				!this.combatant.IsWaitingForAction())
			{
				if(this.timeout > 0) this.timeout -= GameHandler.DeltaMovementTime;
				else if(this.partyFollow)
				{
					if(player != null)
					{
						float distance = VectorHelper.Distance(this.transform.position, player.transform.position, this.ignoreYDistance);
						if(this.autoRespawn && distance >= this.respawnDistance && !this.combatant.respawnFlag)
						{
							this.combatant.respawnFlag = true;
							this.target = null;
							this.moveStopped = false;
							this.stopFollow = false;
							this.moveAway = false;
							GameHandler.Party().RespawnParty();
						}
						else if(distance >= this.minFollowDistance && (this.target == null || this.moveAway))
						{
							this.target = player;
							if(this.moveAway) this.moveAway = false;
							else this.timeout = this.changeTimeout;
							this.moveStopped = false;
							this.stopFollow = false;
						}
						else if(this.giveWay && distance < this.giveWayDistance && this.target == null)
						{
							this.target = player;
							this.moveAway = true;
						}
						else if(!this.moveAway && distance < this.minFollowDistance && this.target == player)
						{
							this.target = null;
							this.timeout = this.changeTimeout;
						}
					}
				}
			}
			
			// target outside battle range, reset target/action
			if(this.target != null && (!this.isCharacter || this.target != player) && 
				VectorHelper.Distance(this.transform.position, this.target.transform.position, 
				this.ignoreYDistance) > DataHolder.BattleSystem().battleRange)
			{
				this.target = null;
				this.combatant.ClearAIAction();
				this.StopMove();
			}
			
			if(this.wpTimeout > 0) this.wpTimeout -= GameHandler.DeltaMovementTime;
			if((player == null || VectorHelper.Distance(this.transform.position, player.transform.position, 
				this.ignoreYDistance) < this.playerDistance) && 
				(this.target != null || (this.waypoint.Length > 0 && this.wpTimeout <= 0)) && 
				this.controller != null && !this.stopFollow)
			{
				if(this.target == null)
				{
					this.SetDirection(this.waypoint[this.wpIndex]);
					this.CheckNextWaypoint();
				}
				else this.SetDirection(this.target.transform.position);
				float curSmooth = this.speedSmoothing*GameHandler.DeltaMovementTime;
				this.moveSpeed = Mathf.Lerp(this.moveSpeed, this.runSpeed, curSmooth);
				Vector3 movement = this.moveDirection*this.moveSpeed+new Vector3(0, this.gravity, 0);
				movement *= GameHandler.DeltaMovementTime;
				this.controller.Move(movement);
			}
			else if(!this.moveStopped && this.controller != null)
			{
				this.StopMove();
			}
			if(this.target == null && this.stopFollow)
			{
				this.timeout = this.changeTimeout;
				this.stopFollow = false;
			}
		}
		else if(!this.moveStopped && this.controller != null)
		{
			this.StopMove();
		}
	}
	
	/*
	============================================================================
	Utility functions
	============================================================================
	*/
	public void StopMove()
	{
		this.controller.Move(Vector3.zero);
		this.moveSpeed = 0;
		this.moveStopped = true;
		this.timeout = this.changeTimeout;
		this.wpTimeout = this.waypointTime;
	}
	
	public void SetDirection(Vector3 pos)
	{
		pos.y = this.transform.position.y;
		this.transform.LookAt(pos);
		if(this.moveAway) this.transform.Rotate(new Vector3(0, 180, 0));
		this.moveDirection = this.transform.TransformDirection(Vector3.forward);
	}
	
	public void SetTarget(GameObject t)
	{
		this.target = t;
		this.moveStopped = false;
		this.stopFollow = false;
		this.moveAway = false;
		this.timeout = this.combatant.aiTimeout*1.5f;
	}
	
	public void StopFollow(bool stop)
	{
		this.stopFollow = stop;
		this.target = null;
		this.moveStopped = false;
		this.moveAway = false;
		this.timeout = this.combatant.aiTimeout*1.5f;
	}
	
	public void SetRoute(float t, GameObject[] wp)
	{
		this.waypointTime = t;
		this.waypoint = new Vector3[0];
		for(int i=0; i<wp.Length; i++)
		{
			if(wp[i] != null)
			{
				this.waypoint = ArrayHelper.Add(wp[i].transform.position, this.waypoint);
			}
		}
		this.wpIndex = 0;
		this.randomPatrol = false;
	}
	
	public void CheckNextWaypoint()
	{
		if(VectorHelper.Distance(this.transform.position, this.waypoint[this.wpIndex], this.ignoreYDistance) < 0.5f)
		{
			if(this.randomPatrol) this.RandomWaypoint();
			else this.wpIndex++;
			if(this.wpIndex >= this.waypoint.Length) this.wpIndex = 0;
			this.wpTimeout = this.waypointTime;
			this.moveStopped = false;
		}
	}
	
	public void RandomWaypoint()
	{
		this.waypoint = new Vector3[1];
		this.waypoint[0] = this.initialPosition;
		this.waypoint[0].x += Random.Range(-this.patrolRadius, this.patrolRadius);
		this.waypoint[0].y += Random.Range(-this.patrolRadius, this.patrolRadius);
	}
}
