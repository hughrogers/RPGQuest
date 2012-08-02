
using System.Collections;
using UnityEngine;

[AddComponentMenu("RPG Kit/Events/Event Interaction")]
public class EventInteraction : BaseInteraction
{
	public string eventFile = "";
	public bool fileOk = false;
	public bool turnToEvent = true;
	public bool turnToPlayer = false;
	public Transform cam;
	public Transform[] actor = new Transform[0];
	public Transform[] waypoint = new Transform[0];
	public GameObject[] prefab = new GameObject[0];
	public AudioClip[] audioClip = new AudioClip[0];
	
	public GameEvent gameEvent = new GameEvent();
	
	// time settings
	public bool eventStarted = false;
	public float timeBefore = 0.0f;
	public float timeAfter = 0.0f;
	
	void Awake()
	{
		// load data from file
		this.LoadEvent();
	}
	
	void Start()
	{
		// start event when autostart
		this.CheckAutoStart();
	}
	
	private void CheckAutoStart()
	{
		if(EventStartType.AUTOSTART.Equals(this.startType))
		{
			if(this.CheckVariables())
			{
				this.StartEvent();
			}
			else if(this.repeatExecution)
			{
				StartCoroutine(CheckAutoStart2());
			}
		}
	}
	
	private IEnumerator CheckAutoStart2()
	{
		if(this.timeAfter > 0)
		{
			yield return new WaitForSeconds(this.timeAfter);
		}
		else
		{
			yield return new WaitForSeconds(0.1f);
		}
		this.CheckAutoStart();
	}
	
	void Update()
	{
		if(this.KeyPress()) this.StartEvent();
		if(!GameHandler.IsGamePaused() && this.gameEvent.executing)
		{
			this.gameEvent.TimeTick(GameHandler.DeltaTime);
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(this.CheckTriggerEnter(other))
		{
			this.StartEvent();
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if(this.CheckTriggerExit(other))
		{
			this.StartEvent();
		}
	}
	
	public override void TouchInteract()
	{
		this.OnMouseUp();
	}
	
	void OnMouseUp()
	{
		if(EventStartType.INTERACT.Equals(this.startType) &&
			this.CheckVariables() && this.gameObject.active && DataHolder.GameSettings().IsMouseAllowed())
		{
			GameObject p = GameHandler.GetPlayer();
			if(p && Vector3.Distance(p.transform.position, this.transform.position) < this.maxMouseDistance)
			{
				this.StartEvent();
			}
		}
	}
	
	public override bool Interact()
	{
		bool val = false;
		// start event on interaction here
		if(EventStartType.INTERACT.Equals(this.startType) &&
			this.CheckVariables() && this.gameObject.active)
		{
			this.StartEvent();
			val = true;
		}
		return val;
	}
	
	public override bool DropInteract(ChoiceContent drop)
	{
		bool val = false;
		if(EventStartType.DROP.Equals(this.startType) &&
			this.CheckVariables() && this.gameObject.active && this.CheckDrop(drop))
		{
			this.StartEvent();
			val = true;
		}
		return val;
	}
	
	public void LoadEvent()
	{
		if(this.eventFile != "" && this.gameEvent.LoadEventData(this.GetEventFile()))
		{
			if(Application.isPlaying)
			{
				this.gameEvent.cam = this.cam;
				this.gameEvent.waypoint = this.waypoint;
				this.gameEvent.prefab = this.prefab;
				this.gameEvent.audioClip = this.audioClip;
				for(int i=0; i<this.actor.Length; i++)
				{
					if(this.actor[i] != null)
					{
						this.gameEvent.actor[i].actor = this.actor[i].gameObject;
					}
				}
			}
			this.fileOk = true;
		}
	}
	
	public void DoTurns()
	{
		if(EventStartType.INTERACT.Equals(this.startType))
		{
			GameObject p = GameHandler.GetPlayer();
			if(this.turnToEvent && p)
			{
				p.transform.LookAt(new Vector3(this.transform.position.x, p.transform.position.y, this.transform.position.z));
			}
			if(this.turnToPlayer && p)
			{
				this.transform.LookAt(new Vector3(p.transform.position.x, this.transform.position.y, p.transform.position.z));
			}
		}
	}
	
	public void StartEvent()
	{
		StartCoroutine(StartEvent2());
	}
	
	private IEnumerator StartEvent2()
	{
		if(!this.eventStarted && !GameHandler.IsControlMenu() &&
			(!GameHandler.IsControlEvent() || !this.gameEvent.blockControls))
		{
			this.DoTurns();
			this.eventStarted = true;
			if(this.timeBefore > 0)
			{
				yield return new WaitForSeconds(this.timeBefore);
			}
			this.gameEvent.StartEvent(this);
		}
	}
	
	public void EventFinished()
	{
		StartCoroutine(EventFinished2());
	}
	
	private IEnumerator EventFinished2()
	{
		if(this.deactivateAfter &&
			!(EventStartType.AUTOSTART.Equals(this.startType) && this.repeatExecution))
		{
			GameHandler.GetLevelHandler().interactionList.Remove(this.gameObject);
		}
		if(this.timeAfter > 0)
		{
			yield return new WaitForSeconds(this.timeAfter);
		}
		else
		{
			yield return new WaitForSeconds(0.1f);
		}
		this.eventStarted = false;
		if(EventStartType.AUTOSTART.Equals(this.startType) && this.repeatExecution)
		{
			this.CheckAutoStart();
		}
		else if(this.deactivateAfter)
		{
			this.gameObject.SetActiveRecursively(false);
		}
	}
	
	public string GetEventFile()
	{
		return "Assets/Resources/Events/"+this.eventFile+".xml";
	}
	
	void OnDrawGizmos()
	{
		Gizmos.DrawIcon(transform.position, "GameEvent.psd");
	}
}