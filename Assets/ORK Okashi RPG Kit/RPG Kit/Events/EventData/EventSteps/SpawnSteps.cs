
using System.Collections;
using UnityEngine;

public class SpawnPlayerStep : EventStep
{
	public SpawnPlayerStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		GameHandler.SpawnPlayer(this.spawnID);
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("spawn", this.spawnID.ToString());
		return ht;
	}
}

public class DestroyPlayerStep : EventStep
{
	public DestroyPlayerStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		if(this.show)
		{
			GameHandler.Party().DestroyInstances();
		}
		else
		{
			GameHandler.Party().DestroyPlayer();
		}
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("show", this.show.ToString());
		return ht;
	}
}

public class SpawnPrefabStep : EventStep
{
	public SpawnPrefabStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		Transform actor = null;
		if(this.show)
		{
			GameObject tmp = gameEvent.actor[this.spawnID].GetActor();
			if(tmp != null)
			{
				actor = tmp.transform;
			}
		}
		else
		{
			actor = gameEvent.waypoint[this.spawnID];
		}
		if(gameEvent.prefab[this.prefabID] != null && actor != null)
		{
			if(gameEvent.spawnedPrefabs.ContainsKey(this.number))
			{
				GameObject.Destroy((GameObject)gameEvent.spawnedPrefabs[this.number]);
			}
			gameEvent.spawnedPrefabs[this.number] = GameObject.Instantiate(
					gameEvent.prefab[this.prefabID], actor.transform.position+this.v3, Quaternion.identity);
		}
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("prefab", this.prefabID.ToString());
		ht.Add("spawn", this.spawnID.ToString());
		ht.Add("show", this.show.ToString());
		ht.Add("number", this.number.ToString());
		
		ArrayList subs = new ArrayList();
		Hashtable s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "vector3");
		s.Add("x", this.v3.x);
		s.Add("y", this.v3.y);
		s.Add("z", this.v3.z);
		subs.Add(s);
		
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}

public class DestroyPrefabStep : EventStep
{
	public DestroyPrefabStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		if(gameEvent.spawnedPrefabs[this.number] != null)
		{
			GameObject.Destroy((GameObject)gameEvent.spawnedPrefabs[this.number]);
		}
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("number", this.number.ToString());
		return ht;
	}
}

public class TeleportStep : EventStep
{
	public TeleportStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		gameEvent.SetTeleportID(this.spawnID);
		gameEvent.StepFinished(gameEvent.step.Length);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("spawn", this.spawnID.ToString());
		return ht;
	}
}