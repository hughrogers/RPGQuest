
using System.Collections;
using UnityEngine;

public class SetCamPosAStep : AnimationStep
{
	public SetCamPosAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		Transform cam = battleAnimation.GetCamera();
		GameObject actor = battleAnimation.GetAnimationObject(this.animationObject, this.prefabID);
		if(cam != null && actor != null && 
			!battleAnimation.camBlocked && 
			(battleAnimation.IsLatestActiveAction() || 
			!DataHolder.BattleSystem().dynamicCombat))
		{
			DataHolder.CameraPosition(this.posID).Use(cam, actor.transform);
		}
		battleAnimation.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("animationobject", this.animationObject.ToString());
		ht.Add("prefab2", this.prefabID2);
		ht.Add("campos", this.posID.ToString());
		return ht;
	}
}

public class FadeCamPosAStep : AnimationStep
{
	public FadeCamPosAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		bool finished = false;
		Transform cam = battleAnimation.GetCamera();
		GameObject actor = battleAnimation.GetAnimationObject(this.animationObject, this.prefabID);
		if(cam != null && actor != null && 
			!battleAnimation.camBlocked && 
			(battleAnimation.IsLatestActiveAction() || 
			!DataHolder.BattleSystem().dynamicCombat))
		{
			CameraEventMover comp = (CameraEventMover)cam.gameObject.GetComponent("CameraEventMover");
			if(comp == null)
			{
				cam.gameObject.AddComponent("CameraEventMover");
				comp = (CameraEventMover)cam.gameObject.GetComponent("CameraEventMover");
			}
			comp.StartCoroutine(comp.SetTargetData(DataHolder.CameraPosition(this.posID), cam, actor.transform, this.interpolate, this.time));
			if(this.wait)
			{
				finished = true;
				battleAnimation.StartTime(this.time, this.next);
			}
		}
		if(!finished)
		{
			if(this.wait) battleAnimation.StartTime(this.time, this.next);
			else battleAnimation.StepFinished(this.next);
		}
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("animationobject", this.animationObject.ToString());
		ht.Add("prefab2", this.prefabID2);
		ht.Add("campos", this.posID.ToString());
		ht.Add("time", this.time.ToString());
		ht.Add("interpolate", this.interpolate.ToString());
		ht.Add("wait", this.wait.ToString());
		return ht;
	}
}

public class SetInitialCamPosAStep : AnimationStep
{
	public SetInitialCamPosAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		if(!battleAnimation.camBlocked && 
			(battleAnimation.IsLatestActiveAction() || 
			!DataHolder.BattleSystem().dynamicCombat))
		{
			battleAnimation.ResetCameraPosition();
		}
		battleAnimation.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		return ht;
	}
}

public class FadeToInitialCamPosAStep : AnimationStep
{
	public FadeToInitialCamPosAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		bool finished = false;
		Transform cam = battleAnimation.GetCamera();
		if(cam != null && 
			!battleAnimation.camBlocked && 
			(battleAnimation.IsLatestActiveAction() || 
			!DataHolder.BattleSystem().dynamicCombat))
		{
			CameraEventMover comp = (CameraEventMover)cam.gameObject.GetComponent("CameraEventMover");
			if(comp == null)
			{
				cam.gameObject.AddComponent("CameraEventMover");
				comp = (CameraEventMover)cam.gameObject.GetComponent("CameraEventMover");
			}
			comp.StartCoroutine(comp.SetTargetData(battleAnimation.initialCamPosition, battleAnimation.initialCamRotation, 
					battleAnimation.initialFieldOfView, cam, this.interpolate, this.time));
			if(this.wait)
			{
				finished = true;
				battleAnimation.StartTime(this.time, this.next);
			}
		}
		if(!finished)
		{
			if(this.wait) battleAnimation.StartTime(this.time, this.next);
			else battleAnimation.StepFinished(this.next);
		}
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("time", this.time.ToString());
		ht.Add("interpolate", this.interpolate.ToString());
		ht.Add("wait", this.wait.ToString());
		return ht;
	}
}

public class ShakeCameraAStep : AnimationStep
{
	public ShakeCameraAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		bool finished = false;
		Transform cam = battleAnimation.GetCamera();
		if(cam != null && 
			!battleAnimation.camBlocked && 
			(battleAnimation.IsLatestActiveAction() || 
			!DataHolder.BattleSystem().dynamicCombat))
		{
			CameraEventMover comp = (CameraEventMover)cam.gameObject.GetComponent("CameraEventMover");
			if(comp == null)
			{
				cam.gameObject.AddComponent("CameraEventMover");
				comp = (CameraEventMover)cam.gameObject.GetComponent("CameraEventMover");
			}
			comp.CameraShake(cam, this.time, this.intensity, this.speed);
			if(this.wait)
			{
				finished = true;
				battleAnimation.StartTime(this.time, this.next);
			}
		}
		if(!finished)
		{
			if(this.wait) battleAnimation.StartTime(this.time, this.next);
			else battleAnimation.StepFinished(this.next);
		}
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("time", this.time.ToString());
		ht.Add("intensity", this.intensity.ToString());
		ht.Add("speed", this.speed.ToString());
		ht.Add("wait", this.wait.ToString());
		return ht;
	}
}

public class RotateCamAroundAStep : AnimationStep
{
	public RotateCamAroundAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		bool finished = false;
		Transform cam = battleAnimation.GetCamera();
		GameObject actor = battleAnimation.GetAnimationObject(this.animationObject, this.prefabID);
		if(cam != null && actor != null && 
			!battleAnimation.camBlocked && 
			(battleAnimation.IsLatestActiveAction() || 
			!DataHolder.BattleSystem().dynamicCombat))
		{
			actor = TransformHelper.GetChild(this.pathToChild, actor.transform).gameObject;
			
			CameraEventMover comp = (CameraEventMover)cam.gameObject.GetComponent("CameraEventMover");
			if(comp == null)
			{
				cam.gameObject.AddComponent("CameraEventMover");
				comp = (CameraEventMover)cam.gameObject.GetComponent("CameraEventMover");
			}
			comp.CameraRotate(cam, actor.transform, this.v3, this.time, this.speed);
			if(this.wait)
			{
				finished = true;
				battleAnimation.StartTime(this.time, this.next);
			}
		}
		if(!finished)
		{
			if(this.wait) battleAnimation.StartTime(this.time, this.next);
			else battleAnimation.StepFinished(this.next);
		}
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("animationobject", this.animationObject.ToString());
		ht.Add("prefab2", this.prefabID2);
		ht.Add("time", this.time.ToString());
		ht.Add("wait", this.wait.ToString());
		ht.Add("speed", this.speed.ToString());
		
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

public class MountCameraAStep : AnimationStep
{
	public MountCameraAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		Transform cam = battleAnimation.GetCamera();
		if(cam != null && 
			!battleAnimation.camBlocked && 
			(battleAnimation.IsLatestActiveAction() || 
			!DataHolder.BattleSystem().dynamicCombat))
		{
			if(this.show)
			{
				GameObject actor = battleAnimation.GetAnimationObject(this.animationObject, this.prefabID);
				if(actor != null)
				{
					TransformHelper.Mount(TransformHelper.GetChild(this.pathToChild, actor.transform), cam, 
							this.show2, this.show4, this.v3, this.show3, this.v3_2);
				}
			}
			else
			{
				cam.parent = null;
			}
		}
		battleAnimation.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("animationobject", this.animationObject.ToString());
		ht.Add("prefab2", this.prefabID2);
		ht.Add("show", this.show);
		ht.Add("show2", this.show2);
		ht.Add("show3", this.show3);
		ht.Add("show4", this.show4);
		
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