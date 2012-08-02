
using System.Collections;
using UnityEngine;

public class SetCamPosStep : EventStep
{
	public SetCamPosStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		Transform cam = gameEvent.GetCamera();
		GameObject actor = null;
		if(this.show3)
		{
			actor = (GameObject)gameEvent.spawnedPrefabs[this.actorID];
		}
		else
		{
			actor = gameEvent.actor[this.actorID].GetActor();
		}
		if(cam != null && actor != null)
		{
			if(this.show4)
			{
				if(this.show5) cam.position = actor.transform.position;
				if(this.show6) cam.rotation = actor.transform.rotation;
				if(this.show7) cam.camera.fieldOfView = this.float1;
			}
			else
			{
				DataHolder.CameraPosition(this.posID).Use(cam, actor.transform);
			}
		}
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("actor", this.actorID.ToString());
		ht.Add("show3", this.show3.ToString());
		ht.Add("show4", this.show4.ToString());
		ht.Add("show5", this.show5.ToString());
		ht.Add("show6", this.show6.ToString());
		ht.Add("show7", this.show7.ToString());
		if(this.show7) ht.Add("float1", this.float1.ToString());
		ht.Add("campos", this.posID.ToString());
		return ht;
	}
}

public class FadeCamPosStep : EventStep
{
	public FadeCamPosStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		bool finished = false;
		Transform cam = gameEvent.GetCamera();
		GameObject actor = null;
		if(this.show3)
		{
			actor = (GameObject)gameEvent.spawnedPrefabs[this.actorID];
		}
		else
		{
			actor = gameEvent.actor[this.actorID].GetActor();
		}
		if(cam != null && actor != null)
		{
			CameraEventMover comp = (CameraEventMover)cam.gameObject.GetComponent("CameraEventMover");
			if(comp == null)
			{
				comp = (CameraEventMover)cam.gameObject.AddComponent("CameraEventMover");
			}
			
			if(this.show4)
			{
				Vector3 pos = cam.position;
				if(this.show5) pos = actor.transform.position;
				Quaternion rot = cam.rotation;
				if(this.show6) rot = actor.transform.rotation;
				float fov = cam.camera.fieldOfView;
				if(this.show7) fov = this.float1;
				comp.StartCoroutine(comp.SetTargetData(pos, rot, fov, cam, this.interpolate, this.time));
			}
			else
			{
				comp.StartCoroutine(comp.SetTargetData(DataHolder.CameraPosition(this.posID), cam, actor.transform, this.interpolate, this.time));
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
		ht.Add("show3", this.show3.ToString());
		ht.Add("show4", this.show4.ToString());
		ht.Add("show5", this.show5.ToString());
		ht.Add("show6", this.show6.ToString());
		ht.Add("show7", this.show7.ToString());
		if(this.show7) ht.Add("float1", this.float1.ToString());
		ht.Add("campos", this.posID.ToString());
		ht.Add("time", this.time.ToString());
		ht.Add("interpolate", this.interpolate.ToString());
		ht.Add("wait", this.wait.ToString());
		return ht;
	}
}

public class SetInitialCamPosStep : EventStep
{
	public SetInitialCamPosStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		gameEvent.ResetCameraPosition();
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		return ht;
	}
}

public class FadeToInitialCamPosStep : EventStep
{
	public FadeToInitialCamPosStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		bool finished = false;
		Transform cam = gameEvent.GetCamera();
		if(cam != null)
		{
			CameraEventMover comp = (CameraEventMover)cam.gameObject.GetComponent("CameraEventMover");
			if(comp == null)
			{
				comp = (CameraEventMover)cam.gameObject.AddComponent("CameraEventMover");
			}
			comp.StartCoroutine(comp.SetTargetData(gameEvent.initialCamPosition, gameEvent.initialCamRotation, 
					gameEvent.initialFieldOfView, cam, this.interpolate, this.time));
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
		ht.Add("time", this.time.ToString());
		ht.Add("interpolate", this.interpolate.ToString());
		ht.Add("wait", this.wait.ToString());
		return ht;
	}
}

public class ShakeCameraStep : EventStep
{
	public ShakeCameraStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		bool finished = false;
		Transform cam = gameEvent.GetCamera();
		if(cam)
		{
			CameraEventMover comp = (CameraEventMover)cam.gameObject.GetComponent("CameraEventMover");
			if(comp == null)
			{
				comp = (CameraEventMover)cam.gameObject.AddComponent("CameraEventMover");
			}
			comp.CameraShake(cam, this.time, this.intensity, this.speed);
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
		ht.Add("time", this.time.ToString());
		ht.Add("intensity", this.intensity.ToString());
		ht.Add("speed", this.speed.ToString());
		ht.Add("wait", this.wait.ToString());
		return ht;
	}
}

public class RotateCamAroundStep : EventStep
{
	public RotateCamAroundStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		bool finished = false;
		Transform cam = gameEvent.GetCamera();
		GameObject actor = null;
		if(this.show3)
		{
			actor = (GameObject)gameEvent.spawnedPrefabs[this.actorID];
		}
		else
		{
			actor = gameEvent.actor[this.actorID].GetActor();
		}
		if(cam != null && actor != null)
		{
			CameraEventMover comp = (CameraEventMover)cam.gameObject.GetComponent("CameraEventMover");
			if(comp == null)
			{
				comp = (CameraEventMover)cam.gameObject.AddComponent("CameraEventMover");
			}
			comp.CameraRotate(cam, actor.transform, this.v3, this.time, this.speed);
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
		ht.Add("show3", this.show3.ToString());
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
		
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}