
using System.Collections;
using UnityEngine;

public class SetToPositionStep : EventStep
{
	public SetToPositionStep(GameEventType t) : base(t)
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
		if(actor != null && (!this.show || gameEvent.waypoint[this.waypointID] != null))
		{
			Vector3 position = this.v3;
			if(this.show) position += gameEvent.waypoint[this.waypointID].position;
			actor.transform.position = position;
		}
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("actor", this.actorID.ToString());
		ht.Add("waypoint", this.waypointID.ToString());
		ht.Add("show", this.show.ToString());
		ht.Add("show3", this.show3.ToString());
		
		ArrayList subs = new ArrayList();
		Hashtable s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "vector3");
		s.Add("x", this.v3.x.ToString());
		s.Add("y", this.v3.y.ToString());
		s.Add("z", this.v3.z.ToString());
		subs.Add(s);
		
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}

public class MoveToWaypointStep : EventStep
{
	public MoveToWaypointStep(GameEventType t) : base(t)
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
			ActorEventMover comp = actor.gameObject.GetComponent<ActorEventMover>();
			if(comp == null)
			{
				comp = actor.gameObject.AddComponent<ActorEventMover>();
			}
			comp.StartCoroutine(comp.MoveToObject(actor.transform, this.controller, this.show2, this.show, 
					gameEvent.waypoint[this.waypointID], this.interpolate, this.time));
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
		ht.Add("controller", this.controller.ToString());
		ht.Add("show3", this.show3.ToString());
		ht.Add("show2", this.show2.ToString());
		ht.Add("show", this.show.ToString());
		ht.Add("interpolate", this.interpolate.ToString());
		return ht;
	}
}

public class MoveToDirectionStep : EventStep
{
	public MoveToDirectionStep(GameEventType t) : base(t)
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
			ActorEventMover comp = actor.gameObject.GetComponent<ActorEventMover>();
			if(comp == null)
			{
				comp = actor.gameObject.AddComponent<ActorEventMover>();
			}
			Vector3 target = this.v3;
			if(this.show2) target = actor.transform.TransformDirection(target);
			comp.StartCoroutine(comp.MoveToDirection(actor.transform, this.controller, target, this.show, this.speed, this.time));
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
		ht.Add("controller", this.controller.ToString());
		ht.Add("speed", this.speed.ToString());
		ht.Add("show", this.show.ToString());
		ht.Add("show2", this.show2.ToString());
		ht.Add("show3", this.show3.ToString());
		
		ArrayList subs = new ArrayList();
		Hashtable s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "vector3");
		s.Add("x", this.v3.x.ToString());
		s.Add("y", this.v3.y.ToString());
		s.Add("z", this.v3.z.ToString());
		subs.Add(s);
		
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}

public class MoveToPrefabStep : EventStep
{
	public MoveToPrefabStep(GameEventType t) : base(t)
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
		GameObject pref = (GameObject)gameEvent.spawnedPrefabs[this.prefabID];
		
		if(actor != null && pref != null)
		{
			ActorEventMover comp = actor.gameObject.GetComponent<ActorEventMover>();
			if(comp == null)
			{
				comp = actor.gameObject.AddComponent<ActorEventMover>();
			}
			comp.StartCoroutine(comp.MoveToObject(actor.transform, this.controller, this.show2, this.show, 
					pref.transform, this.interpolate, this.time));
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
		ht.Add("prefab", this.prefabID.ToString());
		ht.Add("time", this.time.ToString());
		ht.Add("wait", this.wait.ToString());
		ht.Add("controller", this.controller.ToString());
		ht.Add("show3", this.show3.ToString());
		ht.Add("show2", this.show2.ToString());
		ht.Add("show", this.show.ToString());
		ht.Add("interpolate", this.interpolate.ToString());
		return ht;
	}
}
