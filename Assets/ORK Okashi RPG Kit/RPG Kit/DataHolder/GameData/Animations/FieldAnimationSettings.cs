
using UnityEngine;
using System.Collections;

public class FieldAnimationSettings
{
	public bool baseAnimator = true;
	public float walkSpeed = 3.0f;
	public float runSpeed = 8.0f;
	public float fadeLength = 0.1f;
	public float minFallTime = 0.3f;
	
	public AnimationData idle = new AnimationData(-2);
	public AnimationData walk = new AnimationData(0);
	public AnimationData run = new AnimationData(0);
	public AnimationData sprint = new AnimationData(0);
	public AnimationData jump = new AnimationData(1);
	public AnimationData fall = new AnimationData(1);
	public AnimationData land = new AnimationData(0);
	
	private static string IDLE = "idle";
	private static string WALK = "walk";
	private static string RUN = "run";
	private static string SPRINT = "sprint";
	private static string JUMP = "jump";
	private static string FALL = "fall";
	private static string LAND = "land";
	
	public FieldAnimationSettings()
	{
		
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		ht.Add("baseanimator", this.baseAnimator.ToString());
		ht.Add("walkspeed", this.walkSpeed.ToString());
		ht.Add("runspeed", this.runSpeed.ToString());
		ht.Add("fadelength", this.fadeLength.ToString());
		ht.Add("minfalltime", this.minFallTime.ToString());
		
		ArrayList s = new ArrayList();
		if(this.idle.name != "") s.Add(this.idle.GetData(HashtableHelper.GetTitleHashtable(FieldAnimationSettings.IDLE)));
		if(this.walk.name != "") s.Add(this.walk.GetData(HashtableHelper.GetTitleHashtable(FieldAnimationSettings.WALK)));
		if(this.run.name != "") s.Add(this.run.GetData(HashtableHelper.GetTitleHashtable(FieldAnimationSettings.RUN)));
		if(this.sprint.name != "") s.Add(this.sprint.GetData(HashtableHelper.GetTitleHashtable(FieldAnimationSettings.SPRINT)));
		if(this.jump.name != "") s.Add(this.jump.GetData(HashtableHelper.GetTitleHashtable(FieldAnimationSettings.JUMP)));
		if(this.fall.name != "") s.Add(this.fall.GetData(HashtableHelper.GetTitleHashtable(FieldAnimationSettings.FALL)));
		if(this.land.name != "") s.Add(this.land.GetData(HashtableHelper.GetTitleHashtable(FieldAnimationSettings.LAND)));
		if(s.Count > 0) ht.Add(XMLHandler.NODES, s);
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		this.baseAnimator = bool.Parse((string)ht["baseanimator"]);
		this.walkSpeed = float.Parse((string)ht["walkspeed"]);
		this.runSpeed = float.Parse((string)ht["runspeed"]);
		this.fadeLength = float.Parse((string)ht["fadelength"]);
		
		if(ht.ContainsKey("minfalltime"))
		{
			this.minFallTime = float.Parse((string)ht["minfalltime"]);
		}
		
		if(ht.ContainsKey(XMLHandler.NODES))
		{
			ArrayList s = ht[XMLHandler.NODES] as ArrayList;
			foreach(Hashtable ht2 in s)
			{
				if(ht2[XMLHandler.NODE_NAME] as string == FieldAnimationSettings.IDLE)
				{
					this.idle.SetData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == FieldAnimationSettings.WALK)
				{
					this.walk.SetData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == FieldAnimationSettings.RUN)
				{
					this.run.SetData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == FieldAnimationSettings.SPRINT)
				{
					this.sprint.SetData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == FieldAnimationSettings.JUMP)
				{
					this.jump.SetData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == FieldAnimationSettings.FALL)
				{
					this.fall.SetData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == FieldAnimationSettings.LAND)
				{
					this.land.SetData(ht2);
				}
			}
		}
	}
	
	/*
	============================================================================
	Animation handling functions
	============================================================================
	*/
	public void SetAnimationLayers(Combatant c)
	{
		Animation a = c.GetAnimationComponent();
		if(a != null)
		{
			this.idle.Init(a, c);
			this.walk.Init(a, c);
			this.run.Init(a, c);
			this.sprint.Init(a, c);
			this.jump.Init(a, c);
			this.fall.Init(a, c);
			this.land.Init(a, c);
		}
	}
	
	public void AddAnimations(GameObject obj)
	{
		if(this.baseAnimator)
		{
			BaseAnimator rem = obj.GetComponent<BaseAnimator>();
			if(rem != null) GameObject.Destroy(rem);
			FieldAnimator animator = obj.GetComponent<FieldAnimator>();
			if(animator == null)
			{
				animator = obj.AddComponent<FieldAnimator>();
			}
			animator.walkSpeed = this.walkSpeed;
			animator.runSpeed = this.runSpeed;
			animator.fadeLength = this.fadeLength;
			animator.minFallTime = this.minFallTime;
			
			animator.idleAnimation = this.idle.name;
			animator.walkAnimation = this.walk.name;
			animator.runAnimation = this.run.name;
			animator.sprintAnimation = this.sprint.name;
			animator.jumpAnimation = this.jump.name;
			animator.fallAnimation = this.fall.name;
			animator.landAnimation = this.land.name;
		}
	}
}
