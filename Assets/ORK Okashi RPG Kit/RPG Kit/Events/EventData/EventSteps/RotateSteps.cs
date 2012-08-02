
using UnityEngine;
using System.Collections;

public class RotateToWaypointStep : EventStep
{
	public RotateToWaypointStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		bool finished = false;
		GameObject actor = null;
		if(this.show3)
		{
			actor = (GameObject)gameEvent.spawnedPrefabs[this.actorID];
		}
		else
		{
			actor = gameEvent.actor[this.actorID].GetActor();
		}
		if(actor != null && gameEvent.waypoint[this.waypointID] != null)
		{
			ActorEventMover comp = (ActorEventMover)actor.gameObject.GetComponent("ActorEventMover");
			if(comp == null)
			{
				comp = (ActorEventMover)actor.gameObject.AddComponent("ActorEventMover");
			}
			comp.StartCoroutine(comp.RotateToObject(actor.transform, gameEvent.waypoint[this.waypointID], this.interpolate, this.time));
			if(this.wait)
			{
				finished = true;
				gameEvent.StartTime(this.time, this.next);
			}
		}
		if(!finished)
		{
			gameEvent.StepFinished(this.next);
		}
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("actor", this.actorID.ToString());
		ht.Add("waypoint", this.waypointID.ToString());
		ht.Add("time", this.time.ToString());
		ht.Add("wait", this.wait.ToString());
		ht.Add("interpolate", this.interpolate.ToString());
		ht.Add("show3", this.show3.ToString());
		return ht;
	}
}

public class RotationStep : EventStep
{
	public RotationStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		bool finished = false;
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
			Vector3 dir = new Vector3(0, 1, 0);
			if(this.show) dir = this.v3;
			
			ActorEventMover comp =  (ActorEventMover)actor.gameObject.GetComponent("ActorEventMover");
			if(comp == null)
			{
				comp = (ActorEventMover)actor.gameObject.AddComponent("ActorEventMover");
			}
			if(this.show2)
			{
				comp.StartCoroutine(comp.Rotation(actor.transform, this.speed, this.time, dir, this.interpolate));
			}
			else
			{
				comp.StartCoroutine(comp.Rotation(actor.transform, this.speed, this.time, dir));
			}
			if(this.wait)
			{
				finished = true;
				gameEvent.StartTime(this.time, this.next);
			}
		}
		if(!finished)
		{
			gameEvent.StepFinished(this.next);
		}
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("actor", this.actorID.ToString());
		ht.Add("time", this.time.ToString());
		ht.Add("wait", this.wait.ToString());
		ht.Add("speed", this.speed.ToString());
		ht.Add("show3", this.show3.ToString());
		ht.Add("show2", this.show2.ToString());
		ht.Add("interpolate", this.interpolate.ToString());
		
		if(this.show)
		{
			ht.Add("show", this.show.ToString());
			
			ArrayList subs = new ArrayList();
			Hashtable s = new Hashtable();
			s.Add(XMLHandler.NODE_NAME, "vector3");
			s.Add("x", this.v3.x.ToString());
			s.Add("y", this.v3.y.ToString());
			s.Add("z", this.v3.z.ToString());
			subs.Add(s);
			
			ht.Add(XMLHandler.NODES, subs);
		}
		return ht;
	}
}
