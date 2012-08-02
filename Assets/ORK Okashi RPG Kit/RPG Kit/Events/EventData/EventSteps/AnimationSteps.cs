
using System.Collections;
using UnityEngine;

public class PlayAnimationStep : EventStep
{
	public PlayAnimationStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		GameObject actor = null;
		if(this.show3)
		{
			actor = (GameObject)gameEvent.spawnedPrefabs[this.actorID];
		}
		else
		{
			actor = gameEvent.actor[this.actorID].GetActor();
		}
		if(actor != null)
		{
			Animation animation = actor.GetComponent<Animation>();
			if(animation == null) animation = actor.GetComponentInChildren<Animation>();
			if(animation != null)
			{
				if(this.playOptions[this.number] == "Play")
				{
					animation.Play(this.value, this.playMode);
				}
				else if(this.playOptions[this.number] == "CrossFade")
				{
					animation.CrossFade(this.value, this.time, this.playMode);
				}
				else if(this.playOptions[this.number] == "Blend")
				{
					animation.Blend(this.value, this.speed, this.time);
				}
				else if(this.playOptions[this.number] == "PlayQueued")
				{
					animation.PlayQueued(this.value, this.queueMode, this.playMode);
				}
				else if(this.playOptions[this.number] == "CrossFadeQueued")
				{
					animation.CrossFadeQueued(this.value, this.time, this.queueMode, this.playMode);
				}
				if(this.wait)
				{
					gameEvent.StartTime(AnimationHelper.GetLength(actor.animation, this.value), this.next);
				}
			}
		}
		if(!wait || actor == null || actor.animation == null)
		{
			gameEvent.StepFinished(this.next);
		}
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("actor", this.actorID.ToString());
		ht.Add("number", this.number.ToString());
		ht.Add("wait", this.wait.ToString());
		ht.Add("show3", this.show3.ToString());
		
		// play mode
		if(this.playOptions[this.number] == "Play" || this.playOptions[this.number] == "CrossFade" ||
				this.playOptions[this.number] == "PlayQueued" || this.playOptions[this.number] == "CrossFadeQueued")
		{
			ht.Add("playmode", this.playMode.ToString());
		}
		// fade length
		if(this.playOptions[this.number] == "CrossFade" || this.playOptions[this.number] == "Blend" ||
				this.playOptions[this.number] == "CrossFadeQueued")
		{
			ht.Add("time", this.time.ToString());
		}
		// target weight
		if(this.playOptions[this.number] == "Blend")
		{
			ht.Add("speed", this.speed.ToString());
		}
		// queue mode
		if(this.playOptions[this.number] == "PlayQueued" || this.playOptions[this.number] == "CrossFadeQueued")
		{
			ht.Add("queuemode", this.queueMode.ToString());
		}
		
		ArrayList subs = new ArrayList();
		Hashtable s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "value");
		s.Add(XMLHandler.CONTENT, this.value);
		subs.Add(s);
		
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}

public class StopAnimationStep : EventStep
{
	public StopAnimationStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		GameObject actor = null;
		if(this.show3)
		{
			actor = (GameObject)gameEvent.spawnedPrefabs[this.actorID];
		}
		else
		{
			actor = gameEvent.actor[this.actorID].GetActor();
		}
		Animation animation = actor.GetComponent<Animation>();
		if(animation == null) animation = actor.GetComponentInChildren<Animation>();
		if(actor != null && animation != null)
		{
			if(this.show) animation.Stop();
			else animation.Stop(this.value);
		}
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("actor", this.actorID.ToString());
		ht.Add("show", this.show.ToString());
		ht.Add("show3", this.show3.ToString());
		if(!this.show)
		{
			ArrayList subs = new ArrayList();
			Hashtable s = new Hashtable();
			s.Add(XMLHandler.NODE_NAME, "value");
			s.Add(XMLHandler.CONTENT, this.value);
			subs.Add(s);
			
			ht.Add(XMLHandler.NODES, subs);
		}
		return ht;
	}
}