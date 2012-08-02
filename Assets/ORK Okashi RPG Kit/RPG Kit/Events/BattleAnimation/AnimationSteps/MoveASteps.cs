
using System.Collections;
using UnityEngine;

public class SetToPositionAStep : AnimationStep
{
	public SetToPositionAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		GameObject actor = battleAnimation.GetAnimationObject(this.animationObject, this.prefabID2);
		
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
			actor.transform.position = target.transform.position+this.v3;
		}
		battleAnimation.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("animationobject", this.animationObject.ToString());
		ht.Add("prefab2", this.prefabID2);
		ht.Add("movetotarget", this.moveToTarget.ToString());
		
		ArrayList subs = new ArrayList();
		Hashtable s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "vector3");
		s.Add("x", this.v3.x.ToString());
		s.Add("y", this.v3.y.ToString());
		s.Add("z", this.v3.z.ToString());
		subs.Add(s);
		
		subs.Add(HashtableHelper.GetContentHashtable("pathtochild", this.pathToChild));
		
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}

public class MoveToAStep : AnimationStep
{
	public MoveToAStep(BattleAnimationType t) : base(t)
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
		if(this.show7)
		{
			target = (GameObject)battleAnimation.spawnedPrefabs[this.number];
		}
		else if(BattleMoveToTarget.TARGET.Equals(this.moveToTarget) &&
			battleAnimation.battleAction.target != null &&
			battleAnimation.battleAction.target.Length > 0)
		{
			if(battleAnimation.battleAction.target.Length == 1 && 
				battleAnimation.battleAction.target[0] != null && 
				battleAnimation.battleAction.target[0].prefabInstance != null)
			{
				target = battleAnimation.battleAction.target[0].prefabInstance;
			}
			else if(battleAnimation.battleAction.target.Length > 1)
			{
				target = DataHolder.BattleSystem().GetGroupCenter(battleAnimation.battleAction.target);
			}
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
			
			ActorEventMover comp = actor.gameObject.GetComponent<ActorEventMover>();
			if(comp == null)
			{
				comp = actor.gameObject.AddComponent<ActorEventMover>();
			}
			
			// move by speed
			if(this.show5)
			{
				float sp = this.speed;
				if(this.show6) sp = battleAnimation.battleAction.user.GetMoveSpeed();
				float d = 0;
				if(this.show4) d = this.float1;
				comp.StartCoroutine(comp.SpeedToObject(actor.transform, this.controller, this.show2, 
						sp, d, target.transform, this.wait ? battleAnimation : null, this.next));
				if(this.wait) finished = true;
			}
			// move by interpolation
			else
			{
				if(this.show4)
				{
					float distanceLength = Vector3.Distance(actor.transform.position, target.transform.position);
					GameObject obj = new GameObject();
					obj.transform.position = target.transform.position;
					target = obj;
					target.transform.position = Vector3.Lerp(target.transform.position, actor.transform.position, this.float1 / distanceLength);
				}
				comp.StartCoroutine(comp.MoveToObject(actor.transform, this.controller, this.show2, this.show, 
						target.transform, this.interpolate, this.time));
				if(this.wait)
				{
					finished = true;
					battleAnimation.StartTime(this.time, this.next);
				}
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
		ht.Add("controller", this.controller.ToString());
		ht.Add("movetotarget", this.moveToTarget.ToString());
		ht.Add("float1", this.float1.ToString());
		ht.Add("show4", this.show4.ToString());
		ht.Add("show3", this.show3.ToString());
		ht.Add("show2", this.show2.ToString());
		ht.Add("show", this.show.ToString());
		ht.Add("interpolate", this.interpolate.ToString());
		ht.Add("show5", this.show5.ToString());
		ht.Add("show6", this.show6.ToString());
		ht.Add("speed", this.speed.ToString());
		ht.Add("show7", this.show7.ToString());
		ht.Add("number", this.number.ToString());
		
		ArrayList subs = new ArrayList();
		subs.Add(HashtableHelper.GetContentHashtable("pathtochild", this.pathToChild));
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}

public class MoveToDirectionAStep : AnimationStep
{
	public MoveToDirectionAStep(BattleAnimationType t) : base(t)
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
