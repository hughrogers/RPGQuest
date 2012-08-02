
using System.Collections;
using UnityEngine;

public class SendMessageStep : EventStep
{
	public SendMessageStep(GameEventType t) : base(t)
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
			actor.SendMessage(this.key, this.value, SendMessageOptions.DontRequireReceiver);
		}
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("actor", this.actorID.ToString());
		ht.Add("show3", this.show3.ToString());
		
		ArrayList subs = new ArrayList();
		Hashtable s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "value");
		s.Add(XMLHandler.CONTENT, this.value);
		subs.Add(s);
		
		s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "key");
		s.Add(XMLHandler.CONTENT, this.key);
		subs.Add(s);
		
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}

public class BroadcastMessageStep : EventStep
{
	public BroadcastMessageStep(GameEventType t) : base(t)
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
			actor.BroadcastMessage(this.key, this.value, SendMessageOptions.DontRequireReceiver);
		}
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("actor", this.actorID.ToString());
		ht.Add("show3", this.show3.ToString());
		
		ArrayList subs = new ArrayList();
		Hashtable s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "value");
		s.Add(XMLHandler.CONTENT, this.value);
		subs.Add(s);
		
		s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "key");
		s.Add(XMLHandler.CONTENT, this.key);
		subs.Add(s);
		
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}

public class AddComponentStep : EventStep
{
	public AddComponentStep(GameEventType t) : base(t)
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
			actor.AddComponent(this.key);
		}
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("actor", this.actorID.ToString());
		ht.Add("show3", this.show3.ToString());
		
		ArrayList subs = new ArrayList();
		Hashtable s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "key");
		s.Add(XMLHandler.CONTENT, this.key);
		subs.Add(s);
		
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}

public class RemoveComponentStep : EventStep
{
	public RemoveComponentStep(GameEventType t) : base(t)
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
			Component comp = actor.GetComponent(this.key);
			if(comp != null)
			{
				GameObject.Destroy(comp);
			}
		}
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("actor", this.actorID.ToString());
		ht.Add("show3", this.show3.ToString());
		
		ArrayList subs = new ArrayList();
		Hashtable s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "key");
		s.Add(XMLHandler.CONTENT, this.key);
		subs.Add(s);
		
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}

public class ActivateObjectStep : EventStep
{
	public ActivateObjectStep(GameEventType t) : base(t)
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
			actor.SetActiveRecursively(this.show);
		}
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("actor", this.actorID.ToString());
		ht.Add("show3", this.show3.ToString());
		ht.Add("show", this.show.ToString());
		return ht;
	}
}

public class ObjectVisibleStep : EventStep
{
	public ObjectVisibleStep(GameEventType t) : base(t)
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
			Component[] comps = actor.GetComponentsInChildren(typeof(Renderer));
			for(int i=0; i<comps.Length; i++)
			{
				((Renderer)comps[i]).enabled = this.show;
			}
			Component[] comps2 = actor.GetComponentsInChildren(typeof(Projector));
			for(int i=0; i<comps2.Length; i++)
			{
				((Projector)comps2[i]).enabled = this.show;
			}
		}
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("actor", this.actorID.ToString());
		ht.Add("show3", this.show3.ToString());
		ht.Add("show", this.show.ToString());
		return ht;
	}
}

public class ParentObjectStep : EventStep
{
	public ParentObjectStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		GameObject actor = null;
		if(this.show2)
		{
			actor = (GameObject)gameEvent.spawnedPrefabs[this.actorID];
		}
		else
		{
			actor = gameEvent.actor[this.actorID].GetActor();
		}
		
		if(actor != null)
		{
			if(this.show)
			{
				GameObject actor2 = null;
				if(this.show3)
				{
					actor2 = (GameObject)gameEvent.spawnedPrefabs[this.prefabID];
				}
				else
				{
					actor2 = gameEvent.actor[this.prefabID].GetActor();
				}
				if(actor2 != null)
				{
					TransformHelper.Mount(TransformHelper.GetChild(this.pathToChild, actor2.transform), actor.transform, 
							this.show4, this.show6, this.v3, this.show5, this.v3_2);
				}
			}
			else
			{
				actor.transform.parent = null;
			}
		}
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("actor", this.actorID.ToString());
		ht.Add("show", this.show.ToString());
		ht.Add("show2", this.show2.ToString());
		ht.Add("show3", this.show3.ToString());
		ht.Add("show4", this.show4.ToString());
		ht.Add("show5", this.show5.ToString());
		ht.Add("show6", this.show6.ToString());
		ht.Add("prefab", this.prefabID.ToString());
		
		ArrayList subs = new ArrayList();
		Hashtable s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "vector3");
		s.Add("x", this.v3.x.ToString());
		s.Add("y", this.v3.y.ToString());
		s.Add("z", this.v3.z.ToString());
		subs.Add(s);
		
		s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "vector3_2");
		s.Add("x", this.v3_2.x.ToString());
		s.Add("y", this.v3_2.y.ToString());
		s.Add("z", this.v3_2.z.ToString());
		subs.Add(s);
		
		subs.Add(HashtableHelper.GetContentHashtable("pathtochild", this.pathToChild));
		
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}

public class CallGlobalEventStep : EventStep
{
	public CallGlobalEventStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		gameEvent.CallGlobalEvent(this.number, this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("number", this.number.ToString());
		return ht;
	}
}