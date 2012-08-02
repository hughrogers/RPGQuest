
using System.Collections;
using UnityEngine;

public class FadeObjectStep : EventStep
{
	public FadeObjectStep(GameEventType t) : base(t)
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
		if(actor)
		{
			EventFader comp = actor.gameObject.GetComponent<EventFader>();
			if(comp == null)
			{
				comp = actor.gameObject.AddComponent<EventFader>();
			}
			if(this.show7)
			{
				if(this.show8)
				{
					comp.StartCoroutine(comp.FlashCurrent(this.show6, this.show, this.float8, this.show2, this.float2, 
							this.show4, this.float4, this.show5, this.float6, this.interpolate, this.time, this.show9, 
							this.materialProperty, this.show11));
				}
				else
				{
					comp.StartCoroutine(comp.Flash(this.show6, this.show, this.float7, this.float8, this.show2, this.float1, this.float2, 
							this.show4, this.float3, this.float4, this.show5, this.float5, this.float6, this.interpolate, this.time, this.show9, 
							this.materialProperty, this.show11));
				}
			}
			else if(this.show8)
			{
				comp.StartCoroutine(comp.FadeCurrent(this.show6, this.show, this.float8, this.show2, this.float2, 
						this.show4, this.float4, this.show5, this.float6, this.interpolate, this.time, this.show9, 
						this.materialProperty, this.show11));
			}
			else
			{
				comp.StartCoroutine(comp.Fade(this.show6, this.show, this.float7, this.float8, this.show2, this.float1, this.float2, 
						this.show4, this.float3, this.float4, this.show5, this.float5, this.float6, this.interpolate, this.time, this.show9, 
						this.materialProperty, this.show11));
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
		ht.Add("wait", this.wait.ToString());
		ht.Add("time", this.time.ToString());
		ht.Add("float1", this.float1.ToString());
		ht.Add("float2", this.float2.ToString());
		ht.Add("float3", this.float3.ToString());
		ht.Add("float4", this.float4.ToString());
		ht.Add("float5", this.float5.ToString());
		ht.Add("float6", this.float6.ToString());
		ht.Add("float7", this.float7.ToString());
		ht.Add("float8", this.float8.ToString());
		ht.Add("show", this.show.ToString());
		ht.Add("show2", this.show2.ToString());
		ht.Add("show3", this.show3.ToString());
		ht.Add("show4", this.show4.ToString());
		ht.Add("show5", this.show5.ToString());
		ht.Add("show6", this.show6.ToString());
		ht.Add("show7", this.show7.ToString());
		ht.Add("show8", this.show8.ToString());
		ht.Add("show9", this.show9.ToString());
		ht.Add("show10", this.show10.ToString());
		ht.Add("show11", this.show11.ToString());
		ht.Add("actor", this.actorID.ToString());
		ht.Add("interpolate", this.interpolate.ToString());
		
		if(this.materialProperty.IndexOf(" ", 0) != -1)
		{
			this.materialProperty = "_Color";
		}
		if(this.materialProperty != "_Color")
		{
			ht.Add("materialproperty", this.materialProperty);
		}
		return ht;
	}
}

public class FadeCameraStep : EventStep
{
	public FadeCameraStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		bool finished = false;
		if(this.show7)
		{
			GameHandler.GetLevelHandler().screenFader.FlashScreen(this.show, this.float7, this.float8, this.show2, this.float1, this.float2, 
					this.show4, this.float3, this.float4, this.show5, this.float5, this.float6, this.interpolate, this.time);
		}
		else
		{
			GameHandler.GetLevelHandler().screenFader.FadeScreen(this.show, this.float7, this.float8, this.show2, this.float1, this.float2, 
					this.show4, this.float3, this.float4, this.show5, this.float5, this.float6, this.interpolate, this.time);
		}
		if(this.wait)
		{
			finished = true;
			gameEvent.StartTime(this.time, this.next);
		}
		if(!finished)
		{
			gameEvent.StepFinished(this.next);
		}
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("wait", this.wait.ToString());
		ht.Add("time", this.time.ToString());
		ht.Add("float1", this.float1.ToString());
		ht.Add("float2", this.float2.ToString());
		ht.Add("float3", this.float3.ToString());
		ht.Add("float4", this.float4.ToString());
		ht.Add("float5", this.float5.ToString());
		ht.Add("float6", this.float6.ToString());
		ht.Add("float7", this.float7.ToString());
		ht.Add("float8", this.float8.ToString());
		ht.Add("show", this.show.ToString());
		ht.Add("show2", this.show2.ToString());
		ht.Add("show4", this.show4.ToString());
		ht.Add("show5", this.show5.ToString());
		ht.Add("show7", this.show7.ToString());
		ht.Add("interpolate", this.interpolate.ToString());
		return ht;
	}
}