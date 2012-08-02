
using UnityEngine;
using System.Collections;

[AddComponentMenu("RPG Kit/Controls/Click Player Controller")]
public class ClickPlayerController : MonoBehaviour
{
	public bool moveDead = true;
	public MouseTouchControl mouseTouch = new MouseTouchControl(true);
	// click
	public float raycastDistance = 100.0f;
	public LayerMask layerMask = 1;
	
	// move
	public GameObject cursorObject;
	public Vector3 cursorOffset = Vector3.zero;
	
	public float speedSmoothing = 10.0f;
	public bool useCharacterSpeed = false;
	public float runSpeed = 8.0f;
	public float gravity =  Physics.gravity.y;
	public float minimumMoveDistance = 0.2f;
	public bool ignoreYDistance = true;
	public bool useEventMover = false;
	
	// outside field
	public bool autoRemoveCursor = true;
	public bool autoStopMove = true;
	
	// ingame
	private Vector3 targetPosition = Vector3.zero;
	private CharacterController controller;
	private GameObject targetCursor;
	
	private bool moveToTarget = false;
	private Vector3 moveDirection = Vector3.zero;
	private bool moveStopped = false;
	private float moveSpeed = 0.0f;
	
	private ActorEventMover mover = null;
	private Combatant combatant = null;
	
	void Start()
	{
		this.controller = (CharacterController)this.GetComponent("CharacterController");
		if(this.useCharacterSpeed || !this.moveDead)
		{
			CombatantClick cc = (CombatantClick)this.transform.root.GetComponent(typeof(CombatantClick));
			if(cc == null)
			{
				cc = (CombatantClick)this.transform.root.GetComponentInChildren(typeof(CombatantClick));
			}
			if(cc != null)
			{
				this.combatant = cc.combatant;
			}
		}
	}
	
	void Update()
	{
		if(GameHandler.IsControlField() && !GameHandler.IsBlockControl() && 
			(this.moveDead || !this.combatant.isDead) && !this.combatant.IsStopMovement())
		{
			if(this.useCharacterSpeed && this.combatant != null)
			{
				this.runSpeed = this.combatant.GetMoveSpeed();
			}
			
			// get click
			Vector3 point = Vector3.zero;
			if(this.mouseTouch.Interacted(ref point))
			{
				bool doMove = true;
				Ray ray = Camera.main.ScreenPointToRay(point);
				RaycastHit hit;
				if(Physics.Raycast(ray, out hit, this.raycastDistance, this.layerMask))
				{
					if(GameHandler.WithinNoClickMove(hit.point))
					{
						this.ClearCursor();
						doMove = false;
					}
					else
					{
						BaseInteraction[] interactions = hit.transform.gameObject.GetComponentsInChildren<BaseInteraction>();
						foreach(BaseInteraction interaction in interactions)
						{
							if(interaction != null && EventStartType.INTERACT.Equals(interaction.startType))
							{
								doMove = false;
								break;
							}
						}
						
						if(doMove && this.useEventMover)
						{
							this.moveStopped = false;
							this.targetPosition = hit.point;
							if(this.cursorObject != null)
							{
								if(this.targetCursor == null)
								{
									this.targetCursor = (GameObject)GameObject.Instantiate(this.cursorObject);
								}
								this.targetCursor.transform.position = this.targetPosition+this.cursorOffset;
							}
							float distance = Vector3.Distance(this.targetPosition, this.transform.position);
							if(this.mover == null)
							{
								this.mover = (ActorEventMover)this.gameObject.GetComponent("ActorEventMover");
								if(this.mover == null) this.mover = (ActorEventMover)this.gameObject.AddComponent("ActorEventMover");
							}
							this.mover.StartCoroutine(this.mover.MoveToPosition(
									this.transform, true, true, true, this.targetPosition, 
									EaseType.Linear, distance/this.runSpeed));
						}
					}
				}
				if(doMove && !this.useEventMover)
				{
					this.moveStopped = false;
					this.targetPosition = hit.point;
					this.moveToTarget = true;
					if(this.cursorObject != null)
					{
						if(this.targetCursor == null)
						{
							this.targetCursor = (GameObject)GameObject.Instantiate(this.cursorObject);
						}
						this.targetCursor.transform.position = this.targetPosition+this.cursorOffset;
					}
				}
				else
				{
					this.moveToTarget = false;
				}
			}
			// move to target
			if(this.moveToTarget && this.controller != null && !this.useEventMover)
			{
				Vector3 v1 = this.transform.position;
				Vector3 v2 = this.targetPosition;
				if(this.ignoreYDistance) v1.y = v2.y;
				if(this.minimumMoveDistance < Vector3.Distance(v1, v2))
				{
					this.SetDirection(this.targetPosition);
					float curSmooth = speedSmoothing * GameHandler.DeltaMovementTime;
					this.moveSpeed = Mathf.Lerp(this.moveSpeed, this.runSpeed, curSmooth);
					Vector3 movement = this.moveDirection * this.moveSpeed + new Vector3(0, this.gravity, 0);
					movement *= GameHandler.DeltaMovementTime;
					this.controller.Move(movement);
				}
				else
				{
					this.moveSpeed = 0;
					this.controller.Move(Vector3.zero);
				}
			}
		}
		else
		{
			if(this.autoRemoveCursor && this.targetCursor != null)
			{
				GameObject.Destroy(this.targetCursor);
			}
			if(this.autoStopMove && !this.moveStopped)
			{
				if(this.mover != null) this.mover.StopMoving();
				this.moveSpeed = 0;
				this.moveStopped = true;
				this.moveToTarget = false;
				this.controller.Move(Vector3.zero);
			}
		}
	}
	
	void SetDirection(Vector3 pos)
	{
		pos.y = transform.position.y;
		transform.LookAt(pos);
		moveDirection = transform.TransformDirection(Vector3.forward);
	}
	
	public void ClearCursor()
	{
		if(this.targetCursor != null)
		{
			GameObject.Destroy(this.targetCursor);
		}
		if(this.mover != null) this.mover.StopMoving();
		this.moveSpeed = 0;
		this.moveStopped = true;
		this.moveToTarget = false;
		if(this.controller != null) this.controller.Move(Vector3.zero);
	}
}
