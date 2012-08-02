
using UnityEngine;

[AddComponentMenu("RPG Kit/Events/Variable Checker")]
public class VariableChecker : BaseInteraction
{
	public bool destroy = true;
	public bool stopAnimation = false;
	public bool stopAll = false;
	public string animationName  = "";
	public int number = 0;
	public float time = 0.3f;
	public float speed = 1.0f;
	public PlayMode playMode = PlayMode.StopSameLayer;
	public QueueMode queueMode = QueueMode.CompleteOthers;
	
	public string[] playOptions = new string[] {"Play", "CrossFade", "Blend", "PlayQueued", "CrossFadeQueued"};
	
	void Start()
	{
		// start event when autostart
		if(EventStartType.AUTOSTART.Equals(this.startType) && this.CheckVariables())
		{
			this.StartEvent();
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
		if(EventStartType.INTERACT.Equals(this.startType) && GameHandler.IsControlField() &&
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
		if(EventStartType.INTERACT.Equals(this.startType) && GameHandler.IsControlField() &&
			this.CheckVariables() && this.gameObject.active)
		{
			this.StartEvent();
			val = true;
		}
		return val;
	}
	
	public void StartEvent()
	{
		if(this.destroy)
		{
			GameObject.Destroy(this.gameObject);
		}
		else if(this.animation != null && this.stopAnimation)
		{
			if(this.stopAll) this.animation.Stop();
			else this.animation.Stop(this.animationName);
		}
		else if(this.animation != null)
		{
			if(this.playOptions[this.number] == "Play")
			{
				this.animation.Play(this.animationName, this.playMode);
			}
			else if(this.playOptions[this.number] == "CrossFade")
			{
				this.animation.CrossFade(this.animationName, this.time, this.playMode);
			}
			else if(this.playOptions[this.number] == "Blend")
			{
				this.animation.Blend(this.animationName, this.speed, this.time);
			}
			else if(this.playOptions[this.number] == "PlayQueued")
			{
				this.animation.PlayQueued(this.animationName, this.queueMode, this.playMode);
			}
			else if(this.playOptions[this.number] == "CrossFadeQueued")
			{
				this.animation.CrossFadeQueued(this.animationName, this.time, this.queueMode, this.playMode);
			}
		}
	}
}