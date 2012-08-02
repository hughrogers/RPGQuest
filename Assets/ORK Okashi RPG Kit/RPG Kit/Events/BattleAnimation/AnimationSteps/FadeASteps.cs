
using System.Collections;
using UnityEngine;

public class FadeObjectAStep : AnimationStep
{
	public FadeObjectAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		bool finished = false;
		
		GameObject[] list = battleAnimation.GetAnimationObjects(this.animationObject, this.prefabID2);
		if(list != null && list.Length > 0)
		{
			for(int i=0; i<list.Length; i++)
			{
				if(list[i] != null && this.pathToChild != "")
				{
					Transform t = list[i].transform.Find(this.pathToChild);
					if(t != null) list[i] = t.gameObject;
				}
				if(list[i] != null && this.DoFade(list[i]))
				{
					finished = true;
				}
			}
		}
		
		if(this.wait && finished)
		{
			battleAnimation.StartTime(this.time, this.next);
		}
		else
		{
			battleAnimation.StepFinished(this.next);
		}
	}
	
	private bool DoFade(GameObject actor)
	{
		bool fade = false;
		if(actor != null)
		{
			EventFader comp = (EventFader)actor.gameObject.GetComponent("EventFader");
			if(comp == null)
			{
				actor.gameObject.AddComponent("EventFader");
				comp = (EventFader)actor.gameObject.GetComponent("EventFader");
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
			fade = true;
		}
		return fade;
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("animationobject", this.animationObject.ToString());
		ht.Add("prefab2", this.prefabID2);
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
		ht.Add("show6", this.show6.ToString());
		ht.Add("show7", this.show7.ToString());
		ht.Add("show8", this.show8.ToString());
		ht.Add("show9", this.show9.ToString());
		ht.Add("show10", this.show10.ToString());
		ht.Add("show11", this.show11.ToString());
		ht.Add("interpolate", this.interpolate.ToString());
		
		if(this.materialProperty.IndexOf(" ", 0) != -1)
		{
			this.materialProperty = "_Color";
		}
		if(this.materialProperty != "_Color")
		{
			ht.Add("materialproperty", this.materialProperty);
		}
		
		ArrayList subs = new ArrayList();
		subs.Add(HashtableHelper.GetContentHashtable("pathtochild", this.pathToChild));
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}

public class FadeCameraAStep : AnimationStep
{
	public FadeCameraAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
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
			battleAnimation.StartTime(this.time, this.next);
		}
		if(!finished)
		{
			battleAnimation.StepFinished(this.next);
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