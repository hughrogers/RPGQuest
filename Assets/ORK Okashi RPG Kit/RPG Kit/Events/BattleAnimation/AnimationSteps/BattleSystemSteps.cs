
using System.Collections;
using UnityEngine;

public class CalculateAStep : AnimationStep
{
	public CalculateAStep(BattleAnimationType t) : base (t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		bool finished = false;
		battleAnimation.calculated = true;
		
		bool allDead = true;
		for(int i=0; i<battleAnimation.battleAction.target.Length; i++)
		{
			if(battleAnimation.battleAction.target[i] != null && 
				(!battleAnimation.battleAction.target[i].isDead || battleAnimation.battleAction.reviveFlag))
			{
				allDead = false;
				break;
			}
		}
		
		if(!allDead)
		{
			float dmgFactor = 1;
			if(this.show2) dmgFactor = this.float1;
			CombatantAnimation[] anims = battleAnimation.battleAction.Calculate(battleAnimation.battleAction.target, dmgFactor);
			if(this.show)
			{
				float t = 0;
				for(int i=0; i<battleAnimation.battleAction.target.Length; i++)
				{
					if(battleAnimation.battleAction.target[i] != null && 
						battleAnimation.battleAction.target[i].prefabInstance != null &&
						(DataHolder.BattleSystem().playDamageAnim || 
						!battleAnimation.battleAction.target[i].IsInAction() ||
						!(DataHolder.BattleSystem().dynamicCombat || DataHolder.BattleSystem().IsRealTime())))
					{
						string name = battleAnimation.battleAction.target[i].GetAnimationName(anims[i]);
						if(name != "" && this.PlayAnimation(battleAnimation.battleAction.target[i].prefabInstance, name))
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
					finished = true;
					battleAnimation.StartTime(t, this.next);
				}
			}
		}
		
		if(!finished)
		{
			battleAnimation.StepFinished(this.next);
		}
	}
	
	private bool PlayAnimation(GameObject actor, string name)
	{
		bool b = false;
		if(actor != null && actor.animation != null && name != "" && actor.animation[name])
		{
			if(this.playOptions[this.number] == "Play")
			{
				actor.animation.Play(name, this.playMode);
			}
			else if(this.playOptions[this.number] == "CrossFade")
			{
				actor.animation.CrossFade(name, this.time, this.playMode);
			}
			else if(this.playOptions[this.number] == "Blend")
			{
				actor.animation.Blend(name, this.speed, this.time);
			}
			else if(this.playOptions[this.number] == "PlayQueued")
			{
				actor.animation.PlayQueued(name, this.queueMode, this.playMode);
			}
			else if(this.playOptions[this.number] == "CrossFadeQueued")
			{
				actor.animation.CrossFadeQueued(name, this.time, this.queueMode, this.playMode);
			}
			b = true;
		}
		return b;
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("number", this.number.ToString());
		ht.Add("wait", this.wait.ToString());
		ht.Add("show", this.show.ToString());
		if(this.show2)
		{
			ht.Add("show2", this.show2.ToString());
			ht.Add("float1", this.float1.ToString());
		}
		
		// play mode
		if(this.playOptions[this.number] == "Play" || this.playOptions[this.number] == "CrossFade" ||
				this.playOptions[this.number] == "PlayQueued" || this.playOptions[this.number] == "CrossFadeQueued")
		{
			ht.Add("playmode", this.playMode.ToString());
		}
		// fade length
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

public class DamageMultiplierAStep : AnimationStep
{
	public DamageMultiplierAStep(BattleAnimationType t) : base (t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		battleAnimation.battleAction.damageMultiplier = this.float1;
		battleAnimation.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("float1", this.float1.ToString());
		return ht;
	}
}

public class ActivateDamageAStep : AnimationStep
{
	public ActivateDamageAStep(BattleAnimationType t) : base (t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		GameObject obj = null;
		if(this.show3 && battleAnimation.spawnedPrefabs[this.prefabID] != null)
		{
			obj = (GameObject)battleAnimation.spawnedPrefabs[this.prefabID];
		}
		else if(battleAnimation.battleAction.user != null &&
			battleAnimation.battleAction.user.prefabInstance != null)
		{
			obj = battleAnimation.battleAction.user.prefabInstance;
		}
		if(obj != null)
		{
			DamageDealer[] damage = obj.GetComponentsInChildren<DamageDealer>();
			BattleAction action = null;
			if(show) action = battleAnimation.battleAction;
			for(int i=0; i<damage.Length; i++)
			{
				damage[i].SetAction(action);
				damage[i].SetDamageActive(this.show);
				if(this.show2)
				{
					AudioClip clip = null;
					if(this.show4) clip = battleAnimation.battleAction.user.GetAudioClip(this.audioID);
					else clip = battleAnimation.audioClip[this.audioID];
					damage[i].SetAudioClip(clip, this.volume, this.float1, this.float2, this.speed, this.audioRolloffMode);
				}
				if(this.show5)
				{
					damage[i].SetPrefab(battleAnimation.prefab[this.prefabID2], this.float3);
				}
			}
		}
		battleAnimation.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("show", this.show.ToString());
		ht.Add("show2", this.show2.ToString());
		ht.Add("show3", this.show3.ToString());
		ht.Add("show4", this.show4.ToString());
		ht.Add("show5", this.show5.ToString());
		ht.Add("audio", this.audioID.ToString());
		ht.Add("prefab", this.prefabID.ToString());
		ht.Add("prefab2", this.prefabID2.ToString());
		ht.Add("float1", this.float1.ToString());
		ht.Add("float2", this.float2.ToString());
		ht.Add("float3", this.float3.ToString());
		ht.Add("volume", this.volume.ToString());
		ht.Add("speed", this.speed.ToString());
		ht.Add("audiorolloffmode", this.audioRolloffMode.ToString());
		return ht;
	}
}

public class RestoreControlAStep : AnimationStep
{
	public RestoreControlAStep(BattleAnimationType t) : base (t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		if(GameHandler.Party().IsPlayerCharacter(battleAnimation.battleAction.user))
		{
			GameHandler.RestoreControl();
		}
		battleAnimation.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		return ht;
	}
}