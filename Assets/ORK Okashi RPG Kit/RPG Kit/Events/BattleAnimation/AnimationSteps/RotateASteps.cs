
using UnityEngine;
using System.Collections;

public class RotateToAStep : AnimationStep
{
	public RotateToAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		bool finished = false;
		GameObject actor = null;
		if(this.show3)
		{
			actor = (GameObject)battleAnimation.spawnedPrefabs[this.prefabID];
		}
		else if(battleAnimation.battleAction.user != null && battleAnimation.battleAction.user.prefabInstance != null)
		{
			actor = battleAnimation.battleAction.user.prefabInstance;
		}
		
		GameObject target = null;
		if(BattleMoveToTarget.TARGET.Equals(this.moveToTarget) &&
			battleAnimation.battleAction.target != null && 
			battleAnimation.battleAction.target.Length > 0)
		{
			target = DataHolder.BattleSystem().GetGroupCenter(battleAnimation.battleAction.target);
		}
		else if(BattleMoveToTarget.BASE.Equals(this.moveToTarget))
		{
			target = battleAnimation.battleAction.user.GetBattleSpot().gameObject;
		}
		else if(BattleMoveToTarget.CENTER.Equals(this.moveToTarget))
		{
			target = DataHolder.BattleSystem().GetArenaCenter();
		}
		if(actor != null && target != null)
		{
			if(this.pathToChild != "")
			{
				Transform t = target.transform.Find(this.pathToChild);
				if(t != null) target = t.gameObject;
			}
			
			ActorEventMover comp = (ActorEventMover)actor.gameObject.GetComponent("ActorEventMover");
			if(comp == null)
			{
				actor.gameObject.AddComponent("ActorEventMover");
				comp = (ActorEventMover)actor.gameObject.GetComponent("ActorEventMover");
			}
			comp.StartCoroutine(comp.RotateToObject(actor.transform, target.transform, this.interpolate, this.time));
			if(this.wait)
			{
				finished = true;
				battleAnimation.StartTime(this.time, this.next);
			}
		}
		if(!finished)
		{
			battleAnimation.StepFinished(this.next);
		}
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("prefab", this.prefabID.ToString());
		ht.Add("movetotarget", this.moveToTarget.ToString());
		ht.Add("time", this.time.ToString());
		ht.Add("wait", this.wait.ToString());
		ht.Add("interpolate", this.interpolate.ToString());
		ht.Add("show3", this.show3.ToString());
		
		ArrayList subs = new ArrayList();
		subs.Add(HashtableHelper.GetContentHashtable("pathtochild", this.pathToChild));
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}

public class RotationAStep : AnimationStep
{
	public RotationAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		bool finished = false;
		GameObject actor = null;
		if(this.show3)
		{
			actor = (GameObject)battleAnimation.spawnedPrefabs[this.prefabID];
		}
		else if(battleAnimation.battleAction.user != null && battleAnimation.battleAction.user.prefabInstance != null)
		{
			actor = battleAnimation.battleAction.user.prefabInstance;
		}
		if(actor != null)
		{
			Vector3 dir = new Vector3(0, 1, 0);
			if(this.show) dir = this.v3;
			
			ActorEventMover comp = (ActorEventMover)actor.gameObject.GetComponent("ActorEventMover");
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
				battleAnimation.StartTime(this.time, this.next);
			}
		}
		if(!finished)
		{
			battleAnimation.StepFinished(this.next);
		}
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("prefab", this.prefabID.ToString());
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

public class LookAtAStep : AnimationStep
{
	public LookAtAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		GameObject actor = null;
		if(this.show3)
		{
			actor = (GameObject)battleAnimation.spawnedPrefabs[this.prefabID];
		}
		else if(battleAnimation.battleAction.user != null && battleAnimation.battleAction.user.prefabInstance != null)
		{
			actor = battleAnimation.battleAction.user.prefabInstance;
		}
		
		GameObject target = null;
		if(BattleMoveToTarget.TARGET.Equals(this.moveToTarget) &&
			battleAnimation.battleAction.target != null && 
			battleAnimation.battleAction.target.Length > 0)
		{
			target = DataHolder.BattleSystem().GetGroupCenter(battleAnimation.battleAction.target);
		}
		else if(BattleMoveToTarget.BASE.Equals(this.moveToTarget))
		{
			target = battleAnimation.battleAction.user.GetBattleSpot().gameObject;
		}
		else if(BattleMoveToTarget.CENTER.Equals(this.moveToTarget))
		{
			target = DataHolder.BattleSystem().GetArenaCenter();
		}
		if(actor != null && target != null)
		{
			if(this.pathToChild != "")
			{
				Transform t = target.transform.Find(this.pathToChild);
				if(t != null) target = t.gameObject;
			}
			
			Vector3 lookAt = target.transform.position;
			if(this.show) lookAt.y = actor.transform.position.y;
			actor.transform.LookAt(lookAt);
		}
		battleAnimation.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("prefab", this.prefabID.ToString());
		ht.Add("movetotarget", this.moveToTarget.ToString());
		ht.Add("show", this.show.ToString());
		ht.Add("show3", this.show3.ToString());
		
		ArrayList subs = new ArrayList();
		subs.Add(HashtableHelper.GetContentHashtable("pathtochild", this.pathToChild));
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}
