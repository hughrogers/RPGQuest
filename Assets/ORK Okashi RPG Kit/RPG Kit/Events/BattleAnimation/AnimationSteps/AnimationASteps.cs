
using System.Collections;
using UnityEngine;

public class PlayAnimationAStep : AnimationStep
{
	public PlayAnimationAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		if(StatusOrigin.USER.Equals(this.statusOrigin) && 
			battleAnimation.battleAction.user != null && 
			battleAnimation.battleAction.user.prefabInstance != null)
		{
			string name = "";
			if(this.show2)
			{
				name = battleAnimation.battleAction.user.GetAnimationName(this.combatantAnimation);
			}
			else
			{
				name = this.value;
			}
			Animation animation = battleAnimation.battleAction.user.GetAnimationComponent();
			bool play = this.PlayAnimation(animation, name);
			if(this.wait && play)
			{
				battleAnimation.StartTime(AnimationHelper.GetLength(animation, name), this.next);
			}
			else
			{
				battleAnimation.StepFinished(this.next);
			}
		}
		else if(StatusOrigin.TARGET.Equals(this.statusOrigin))
		{
			float t = 0;
			for(int i=0; i<battleAnimation.battleAction.target.Length; i++)
			{
				if(battleAnimation.battleAction.target[i] != null && battleAnimation.battleAction.target[i].prefabInstance != null)
				{
					string name = "";
					if(this.show2) name = battleAnimation.battleAction.target[i].GetAnimationName(this.combatantAnimation);
					else name = this.value;
					if(this.PlayAnimation(battleAnimation.battleAction.target[i].GetAnimationComponent(), name))
					{
						float t2 = AnimationHelper.GetLength(battleAnimation.battleAction.target[i].prefabInstance.animation, name);
						if(t2 > t)
						{
							t = t2;
						}
					}
				}
			}
			if(this.wait && t > 0)
			{
				battleAnimation.StartTime(t, this.next);
			}
			else
			{
				battleAnimation.StepFinished(this.next);
			}
		}
		else battleAnimation.StepFinished(this.next);
	}
	
	private bool PlayAnimation(Animation animation, string name)
	{
		bool b = false;
		if(animation != null && name != "" && animation[name] != null)
		{
			if(this.show) animation[name].layer = this.min;
			if(this.playOptions[this.number] == "Play")
			{
				animation.Play(name, this.playMode);
			}
			else if(this.playOptions[this.number] == "CrossFade")
			{
				animation.CrossFade(name, this.time, this.playMode);
			}
			else if(this.playOptions[this.number] == "Blend")
			{
				animation.Blend(name, this.speed, this.time);
			}
			else if(this.playOptions[this.number] == "PlayQueued")
			{
				animation.PlayQueued(name, this.queueMode, this.playMode);
			}
			else if(this.playOptions[this.number] == "CrossFadeQueued")
			{
				animation.CrossFadeQueued(name, this.time, this.queueMode, this.playMode);
			}
			b = true;
		}
		return b;
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht  = base.GetData();
		ht.Add("number", this.number.ToString());
		ht.Add("wait", this.wait.ToString());
		ht.Add("statusorigin", this.statusOrigin.ToString());
		ht.Add("show2", this.show2.ToString());
		ht.Add("show", this.show.ToString());
		ht.Add("min", this.min.ToString());
		if(this.show2) ht.Add("combatantanimation", this.combatantAnimation.ToString());
		else
		{
			ArrayList subs = new ArrayList();
			Hashtable s = new Hashtable();
			s.Add(XMLHandler.NODE_NAME, "value");
			s.Add(XMLHandler.CONTENT, this.value);
			subs.Add(s);
			
			ht.Add(XMLHandler.NODES, subs);
		}
		
		// play mode
		if(this.playOptions[this.number] == "Play" || this.playOptions[this.number] == "CrossFade" ||
				this.playOptions[this.number] == "PlayQueued" || this.playOptions[this.number] == "CrossFadeQueued")
		{
			ht.Add("playmode", this.playMode.ToString());
		}
		// fade Length
		if(this.playOptions[this.number] == "CrossFade" || this.playOptions[this.number] == "Blend" ||
				this.playOptions[this.number] == "CrossFadeQueued")
		{
			ht.Add("time", this.time.ToString());
		}
		// target weight
		if(this.playOptions[this.number] == "Blend")
		{
			ht.Add("speed", this.speed.ToString());
		}
		// queue mode
		if(this.playOptions[this.number] == "PlayQueued" || this.playOptions[this.number] == "CrossFadeQueued")
		{
			ht.Add("queuemode", this.queueMode.ToString());
		}
		return ht;
	}
}

public class StopAnimationAStep : AnimationStep
{
	public StopAnimationAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		if(StatusOrigin.USER.Equals(this.statusOrigin) && 
			battleAnimation.battleAction.user != null && 
			battleAnimation.battleAction.user.prefabInstance != null)
		{
			if(this.show) battleAnimation.battleAction.user.prefabInstance.animation.Stop();
			else
			{
				string name = "";
				if(this.show2)
				{
					name = battleAnimation.battleAction.user.GetAnimationName(this.combatantAnimation);
				}
				else
				{
					name = this.value;
				}
				if(name != "") battleAnimation.battleAction.user.prefabInstance.animation.Stop(name);
			}
		}
		else if(StatusOrigin.TARGET.Equals(this.statusOrigin))
		{
			for(int i=0; i<battleAnimation.battleAction.target.Length; i++)
			{
				if(battleAnimation.battleAction.target[i] != null && battleAnimation.battleAction.target[i].prefabInstance != null)
				{
					if(this.show) battleAnimation.battleAction.target[i].prefabInstance.animation.Stop();
					else
					{
						string name = "";
						if(this.show2) name = battleAnimation.battleAction.target[i].GetAnimationName(this.combatantAnimation);
						else name = this.value;
						if(name != "") battleAnimation.battleAction.target[i].prefabInstance.animation.Stop(name);
					}
				}
			}
		}
		battleAnimation.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("show", this.show.ToString());
		ht.Add("statusorigin", this.statusOrigin.ToString());
		if(!this.show)
		{
			ht.Add("show2", this.show2.ToString());
			if(this.show2) ht.Add("combatantanimation", this.combatantAnimation.ToString());
			else
			{
				ArrayList subs = new ArrayList();
				Hashtable s = new Hashtable();
				s.Add(XMLHandler.NODE_NAME, "value");
				s.Add(XMLHandler.CONTENT, this.value);
				subs.Add(s);
				
				ht.Add(XMLHandler.NODES, subs);
			}
		}
		return ht;
	}
}

public class CallAnimationAStep : AnimationStep
{
	public CallAnimationAStep(BattleAnimationType t) : base (t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		battleAnimation.CallAnimation(this.number, this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("number", this.number.ToString());
		return ht;
	}
}