
using UnityEngine;

[AddComponentMenu("RPG Kit/Battles/Battle Agent")]
public class BattleAgent : MonoBehaviour
{
	public GameObject battleArena;
	
	public bool huntPlayer = false;
	public GameObject moveObject;
	public bool controllerMove = true;
	public bool moveToStartPoint = true;
	public bool applyGravity = true;
	public float moveSpeed = 8;
	public float huntingRange = 20;
	public bool useMaxDistance = true;
	public float maxDistanceToArena = 30;
	public float maxMovementRange = 100;
	public float waitTime = 0.5f;
	public float changeTime = 0.5f;
	
	private CharacterController controller;
	
	private Vector3 startPoint;
	private bool activated = false;
	
	private float waitTimeout = 0;
	private float changeTimeout = 0;
	private int mode = 0;
	private int nextMode = 0;
	
	void Start()
	{
		this.startPoint = this.moveObject.transform.position;
		if(this.huntPlayer && this.controllerMove && this.moveObject)
		{
			this.controller = (CharacterController)this.moveObject.GetComponent(typeof(CharacterController));
		}
	}
	
	void Update()
	{
		if(this.huntPlayer && !GameHandler.IsGamePaused() && 
			GameHandler.IsControlField() && this.moveObject && this.battleArena)
		{
			this.waitTimeout -= GameHandler.DeltaMovementTime;
			if(this.waitTimeout <= 0)
			{
				GameObject player = GameHandler.GetPlayer();
				if(player != null && 
					Vector3.Distance(this.moveObject.transform.position, player.transform.position) <= this.huntingRange &&
					(!this.useMaxDistance ||
					Vector3.Distance(this.battleArena.transform.position, player.transform.position) <= this.maxDistanceToArena))
				{
					this.SwitchMode(1);
				}
				else if(player != null && this.moveToStartPoint && 
					Vector3.Distance(this.moveObject.transform.position, this.startPoint) > 1)
				{
					this.SwitchMode(2);
				}
				else if(this.controller)
				{
					this.nextMode = 0;
					this.CheckMode(0);
				}
				
				if(this.changeTimeout > 0)
				{
					this.changeTimeout -= GameHandler.DeltaMovementTime;
					if(this.changeTimeout <= 0) this.CheckMode(this.nextMode);
				}
				
				if(this.mode == 0)
				{
					this.controller.Move(Vector3.zero);
				}
				else if(this.mode == 1)
				{
					this.moveObject.transform.LookAt(new Vector3(player.transform.position.x, this.moveObject.transform.position.y, player.transform.position.z));
					if(this.controller)
					{
						Vector3 moveDirection = Vector3.forward * this.moveSpeed * GameHandler.DeltaMovementTime;
						moveDirection = this.moveObject.transform.TransformDirection(moveDirection);
						if(this.applyGravity) moveDirection.y = Physics.gravity.y;
						this.controller.Move(moveDirection);
					}
					else
					{
						this.moveObject.transform.position = Vector3.MoveTowards(this.moveObject.transform.position, 
								player.transform.position, this.moveSpeed*GameHandler.DeltaMovementTime);
					}
				}
				else if(this.mode == 2)
				{
					this.moveObject.transform.LookAt(new Vector3(this.startPoint.x, this.moveObject.transform.position.y, this.startPoint.z));
					if(this.controller)
					{
						Vector3 moveDirection = Vector3.forward * this.moveSpeed * GameHandler.DeltaMovementTime;
						moveDirection = this.moveObject.transform.TransformDirection(moveDirection);
						if(this.applyGravity) moveDirection.y = Physics.gravity.y;
						this.controller.Move(moveDirection);
					}
					else
					{
						this.moveObject.transform.position = Vector3.MoveTowards(this.moveObject.transform.position, 
								this.startPoint, this.moveSpeed*GameHandler.DeltaMovementTime);
					}
				}
			}
			else
			{
				this.controller.Move(Vector3.zero);
			}
		}
	}
	
	private void SwitchMode(int m)
	{
		if(this.nextMode != m)
		{
			this.changeTimeout = this.changeTime;
		}
		this.nextMode = m;
	}
	
	private bool CheckMode(int m)
	{
		bool ok = true;
		if(this.mode != m)
		{
			this.waitTimeout = this.waitTime;
			ok = false;
		}
		this.mode = m;
		return ok;
	}
	
	void OnCollisionEnter(Collision collision)
	{
		if(GameHandler.IsControlField() && collision.gameObject == GameHandler.GetPlayer())
		{
			this.StartBattle();
		}
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if(GameHandler.IsControlField() && hit.gameObject == GameHandler.GetPlayer())
		{
			this.StartBattle();
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(GameHandler.IsControlField() && other.gameObject == GameHandler.GetPlayer())
		{
			this.StartBattle();
		}
	}
	
	public void StartBattle()
	{
		if(this.battleArena && !this.activated)
		{
			this.activated = true;
			BattleArena ba = this.battleArena.GetComponent<BattleArena>();
			if(ba)
			{
				if(this.controller) this.controller.Move(Vector3.zero);
				ba.CallStart();
				this.gameObject.SetActiveRecursively(false);
			}
		}
	}
	
	void OnDrawGizmosSelected()
	{
		if(this.huntPlayer)
		{
			Gizmos.color = new Color(0, 1, 1);
			Gizmos.DrawWireSphere(this.transform.position, this.huntingRange);
			if(this.battleArena)
			{
				Gizmos.color = new Color(1, 0, 0);
				if(this.useMaxDistance) Gizmos.DrawWireSphere(this.battleArena.transform.position, this.maxDistanceToArena);
				Gizmos.DrawWireCube(this.battleArena.transform.position, new Vector3(0.5f, 0.5f, 0.5f));
			}
		}
	}
	
	void OnDrawGizmos()
	{
		Gizmos.DrawIcon(transform.position, "BattleAgent.psd");
	}
}