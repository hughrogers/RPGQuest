
using UnityEngine;

public class BattleAction
{
	public static int ALL_ENEMIES = -1;
	public static int ALL_CHARACTERS = -2;
	public static int RANDOM_ENEMY = -3;
	public static int RANDOM_CHARACTER = -4;
	public static int NONE = -5;
	public static int PARTY_TARGET = -6;
	
	public AttackSelection type = AttackSelection.ATTACK;
	// battle ID of the target
	public int targetID = 0;
	// skill or item ID
	public int useID = 0;
	// skill level
	public int useLevel = 0;
	
	public bool reviveFlag = false;
	public bool autoAttackFlag = false;
	
	public Combatant user;
	public Combatant[] target;
	public bool[] doCounter;
	
	public BattleAnimation activeAnimation;
	
	// skill casting
	public float castTime = -2;
	public float castTimeMax = -1;
	public bool casted = false;
	
	private bool userConsumeDone = false;
	
	private int lastInRange = 0;
	private int currentInRange = 0;
	
	// raycast target
	public TargetRaycast targetRaycast = new TargetRaycast();
	public bool rayTargetSet = false;
	public Vector3 rayPoint = Vector3.zero;
	public GameObject rayObject = null;
	
	// from battle animation step
	public float damageMultiplier = 1;
	
	private float timeUse = 0.0f;
	
	public BattleAction()
	{
		
	}
	
	public BattleAction(AttackSelection t, Combatant u, int tID, int id, int ul)
	{
		this.type = t;
		this.user = u;
		this.targetID = tID;
		this.useID = id;
		this.useLevel = ul;
	}
	
	public void CheckRevive()
	{
		if(this.IsSkill())
		{
			this.reviveFlag = DataHolder.Skill(this.useID).level[this.useLevel].revive;
		}
		else if(this.IsItem())
		{
			Item item = DataHolder.Item(this.useID);
			this.reviveFlag = item.revive;
			if(!this.reviveFlag && ItemSkillType.USE.Equals(item.itemSkill))
			{
				this.reviveFlag = DataHolder.Skill(item.skillID).level[item.skillLevel-1].revive;
			}
		}
	}
	
	public bool IsAttack()
	{
		return AttackSelection.ATTACK.Equals(this.type);
	}
	
	public bool IsSkill()
	{
		return AttackSelection.SKILL.Equals(this.type);
	}
	
	public bool IsItem()
	{
		return AttackSelection.ITEM.Equals(this.type);
	}
	
	public bool IsDefend()
	{
		return AttackSelection.DEFEND.Equals(this.type);
	}
	
	public bool IsEscape()
	{
		return AttackSelection.ESCAPE.Equals(this.type);
	}
	
	public bool IsDeath()
	{
		return AttackSelection.DEATH.Equals(this.type);
	}
	
	public bool IsCounter()
	{
		return AttackSelection.COUNTER.Equals(this.type);
	}
	
	public bool IsNone()
	{
		return AttackSelection.NONE.Equals(this.type);
	}
	
	public bool CheckDamageDealer(DamageDealer dealer)
	{
		bool ok = false;
		if(this.IsAttack() && dealer.baseAttack) ok = true;
		else if(this.IsSkill())
		{
			for(int i=0; i<dealer.skillID.Length; i++)
			{
				if(dealer.skillID[i] == this.useID) ok = true;
			}
		}
		else if(this.IsItem())
		{
			for(int i=0; i<dealer.itemID.Length; i++)
			{
				if(dealer.itemID[i] == this.useID) ok = true;
			}
		}
		return ok;
	}
	
	public bool CanDamage(Combatant c)
	{
		bool can = false;
		if(this.IsAttack() && 
			(!this.user.SameType(c) ||
			this.user.IsAttackFriends()))
		{
			can = true;
		}
		else if(this.IsSkill() &&
			((DataHolder.Skill(this.useID).TargetAlly() && this.user.SameType(c)) ||
			(DataHolder.Skill(this.useID).TargetEnemy() && !this.user.SameType(c))))
		{
			can = true;
		}
		else if(this.IsItem() &&
			((DataHolder.Item(this.useID).TargetAlly() && this.user.SameType(c)) ||
			(DataHolder.Item(this.useID).TargetEnemy() && !this.user.SameType(c))))
		{
			can = true;
		}
		if(can && c.isDead && !this.reviveFlag) can = false;
		return can;
	}
	
	/*
	============================================================================
	Skill cast functions
	============================================================================
	*/
	public bool IsCastingSkill()
	{
		bool casting = false;
		if(!this.casted && DataHolder.BattleSystem().CanUseSkillCasting() && 
			this.IsSkill() && DataHolder.Skill(this.useID).level[this.useLevel].castTime > 0)
		{
			casting = true;
			this.castTimeMax = DataHolder.Skill(this.useID).level[this.useLevel].castTime;
			this.castTime = 0;
			if(this.user != null)
			{
				this.user.castSkill = this;
				this.user.castingSkill = true;
			}
		}
		return casting;
	}
	
	public bool CancelSkillCast()
	{
		bool canceled = false;
		if(!this.casted && this.castTime > 0 && 
			DataHolder.Skill(this.useID).level[this.useLevel].cancelable)
		{
			canceled = true;
		}
		return canceled;
	}
	
	/*
	============================================================================
	Target selection functions
	============================================================================
	*/
	public void BlinkTargets(bool blink)
	{
		DataHolder.BattleSystem().TargetSelectionOff();
		if(blink)
		{
			Combatant[] t = new Combatant[0];
			if(this.targetID != BattleAction.PARTY_TARGET)
			{
				t = this.GetTargets(true);
			}
			for(int i=0; i<t.Length; i++)
			{
				t[i].Blink(true);
			}
			
			if(DataHolder.BattleCam().blockAnimationCams)
			{
				GameObject camTarget = null;
				if(t.Length == 1) camTarget = t[0].prefabInstance;
				else
				{
					camTarget = new GameObject();
					camTarget.transform.position = Vector3.zero;
					int count = 0;
					for(int i=0; i<t.Length; i++)
					{
						if(t[i] != null && t[i].prefabInstance != null)
						{
							camTarget.transform.position += t[i].prefabInstance.transform.position;
							count++;
						}
					}
					if(count > 0) camTarget.transform.position /= count;
					else GameObject.Destroy(camTarget);
				}
				if(camTarget != null)
				{
					DataHolder.BattleCam().SetSelection(camTarget.transform);
				}
			}
		}
	}
	
	public Combatant[] GetTargets(bool checkRange)
	{
		Combatant[] t = new Combatant[0];
		if(this.targetID == BattleAction.ALL_CHARACTERS)
		{
			t = GameHandler.Party().GetBattleParty();
		}
		else if(this.targetID == BattleAction.ALL_ENEMIES)
		{
			t = DataHolder.BattleSystem().enemies;
		}
		else if(this.targetID == BattleAction.RANDOM_CHARACTER)
		{
			t = new Combatant[] {DataHolder.BattleSystem().GetRandomCharacter()};
		}
		else if(this.targetID == BattleAction.RANDOM_ENEMY)
		{
			t = new Combatant[] {DataHolder.BattleSystem().GetRandomEnemy()};
		}
		else if(this.targetID == BattleAction.PARTY_TARGET)
		{
			t = new Combatant[] {DataHolder.BattleControl().partyTarget};
		}
		else if(this.targetID != BattleAction.NONE)
		{
			t = new Combatant[] {DataHolder.BattleSystem().GetCombatantForBattleID(this.targetID)};
		}
		
		if(checkRange)
		{
			Combatant[] t2 = new Combatant[0];
			for(int i=0; i<t.Length; i++)
			{
				if(this.InRange(t[i])) t2 = ArrayHelper.Add(t[i], t2);
			}
			t = t2;
		}
		
		return t;
	}
	
	public bool InRange(Combatant t)
	{
		bool inRange = true;
		if((this.user != null && t != null) && 
			(((this.IsAttack() || this.IsCounter()) &&
			!this.user.InAttackRange(t)) || 
		// skill
			(this.IsSkill() && 
			!DataHolder.Skill(this.useID).InRange(this.user, t, this.useLevel)) ||
		// item
			(this.IsItem() && !DataHolder.Item(this.useID).useRange.InRange(this.user, t))))
		{
			inRange = false;
		}
		return inRange;
	}
	
	public bool InRange()
	{
		bool inRange = true;
		this.lastInRange = this.currentInRange;
		this.currentInRange = 0;
		if(this.user != null)
		{
			Combatant[] t = this.GetTargets(false);
			for(int i=0; i<t.Length; i++)
			{
				if(!this.InRange(t[i])) this.currentInRange++;
			}
			// none in range
			if(t.Length > 0 && t.Length == this.currentInRange)
			{
				inRange = false;
			}
		}
		return inRange;
	}
	
	public bool RangeDifference()
	{
		return this.lastInRange != this.currentInRange;
	}
	
	public bool InBattleRange()
	{
		bool inRange = true;
		if(this.user != null)
		{
			Combatant[] t = this.GetTargets(false);
			int count = 0;
			UseRange ur = new UseRange();
			for(int i=0; i<t.Length; i++)
			{
				if(!ur.InRange(this.user, t[i]))
				{
					count++;
				}
			}
			// none in range
			if(t.Length > 0 && t.Length == count)
			{
				inRange = false;
			}
		}
		return inRange;
	}
	
	public bool TargetNone()
	{
		bool none = false;
		if((this.IsSkill() && DataHolder.Skill(this.useID).TargetNone()) ||
			(this.IsItem() && DataHolder.Item(this.useID).TargetNone()))
		{
			none = true;
		}
		return none;
	}
	
	public bool TargetAlive()
	{
		bool alive = true;
		if(!this.reviveFlag)
		{
			Combatant[] t = this.GetTargets(false);
			if(!this.TargetNone())
			{
				alive = false;
				for(int i=0; i<t.Length; i++)
				{
					if(t[i] != null && !t[i].isDead)
					{
						alive = true;
						break;
					}
				}
			}
		}
		return alive;
	}
	
	public void CheckTargetAggressive()
	{
		Combatant[] t = this.GetTargets(true);
		for(int i=0; i<t.Length; i++)
		{
			if(t[i] != null && !this.user.SameType(t[i]))
			{
				t[i].CheckAggressive(AggressiveType.SELECTION);
			}
		}
	}
	
	/*
	============================================================================
	Action performing functions
	============================================================================
	*/
	public void PerformAction()
	{
		DataHolder.BattleSystem().AddActiveAction(this);
		
		// init targets
		this.target = this.GetTargets(true);
		
		if(!DataHolder.BattleSystem().IsRealTime() && this.user != null && 
			(target.Length == 0 || (target.Length == 1 && 
			(target[0] == null || (target[0].isDead && !this.reviveFlag)))) &&
			(this.IsAttack() || (this.IsSkill() && DataHolder.Skill(this.useID).TargetSingleEnemy()) ||
			(this.IsItem() && DataHolder.Item(this.useID).TargetSingleEnemy())))
		{
			if((this.user is Character && !this.user.IsAttackFriends()) || 
				(this.user is Enemy && this.user.IsAttackFriends()))
			{
				target = new Combatant[] {DataHolder.BattleSystem().GetCombatantForBattleID(
						user.GetRandomTarget(DataHolder.BattleSystem().enemies))};
			}
			else if((this.user is Character && this.user.IsAttackFriends()) || 
				(this.user is Enemy && !this.user.IsAttackFriends()))
			{
				target = new Combatant[] {DataHolder.BattleSystem().GetCombatantForBattleID(
						user.GetRandomTarget(GameHandler.Party().GetBattleParty()))};
			}
		}
		
		if(!this.IsNone() && user != null && 
			(DataHolder.BattleSystem().IsRealTime() || 
			(target.Length > 0 && target[0] != null)) && 
			((!user.isDead && !this.IsDeath()) || this.IsDeath()))
		{
			for(int i=0; i<this.target.Length; i++)
			{
				if(this.target[i] != null && !this.user.SameType(this.target[i]))
				{
					this.target[i].CheckAggressive(AggressiveType.ACTION);
				}
			}
			
			if(this.IsCounter()) user.baIndex = 0;
			user.lastTargetBattleID = this.targetID;
			user.SetInAction(true);
			if(DataHolder.BattleCam().blockAnimationCams &&
				user.prefabInstance != null)
			{
				DataHolder.BattleCam().SetLatestUser(user.prefabInstance.transform);
			}
			if(Application.isPlaying)
			{
				// show info
				if(DataHolder.BattleSystemData().showInfo)
				{
					if(this.IsSkill() && DataHolder.BattleSystemData().showSkills)
					{
						GameHandler.GetLevelHandler().ShowBattleInfo(
								DataHolder.GameSettings().GetSkillLevelName(DataHolder.Skills().GetName(this.useID), this.useLevel+1), 
								DataHolder.BattleSystemData().infoPosition);
					}
					else if(this.IsItem() && DataHolder.BattleSystemData().showItems)
					{
						GameHandler.GetLevelHandler().ShowBattleInfo(DataHolder.Items().GetName(this.useID), 
								DataHolder.BattleSystemData().infoPosition);
					}
					else if(this.IsDefend() && DataHolder.BattleSystemData().showDefend)
					{
						GameHandler.GetLevelHandler().ShowBattleInfo(DataHolder.BattleMenu().defendName[GameHandler.GetLanguage()] as string, 
								DataHolder.BattleSystemData().infoPosition);
					}
					else if(this.IsEscape() && DataHolder.BattleSystemData().showEscape)
					{
						GameHandler.GetLevelHandler().ShowBattleInfo(DataHolder.BattleMenu().escapeName[GameHandler.GetLanguage()] as string, 
								DataHolder.BattleSystemData().infoPosition);
					}
					else if(this.IsCounter() && DataHolder.BattleSystemData().showCounter)
					{
						GameHandler.GetLevelHandler().ShowBattleInfo(DataHolder.BattleSystemData().GetCounterText(), 
								DataHolder.BattleSystemData().infoPosition);
					}
				}
				// get battle animation
				if((this.IsAttack() || this.IsCounter()))
				{
					int id = user.GetBaseAttackAnimation();
					if(id >= 0) this.activeAnimation = DataHolder.BattleAnimations().GetCopy(id);
				}
				else if(this.IsSkill())
				{
					if(this.target == null || this.target.Length == 0)
					{
						DataHolder.Skill(this.useID).UserConsume(this.user, this.useLevel);
						this.userConsumeDone = true;
					}
					if(DataHolder.Skill(this.useID).level[this.useLevel].battleAnimation)
					{
						this.activeAnimation = DataHolder.BattleAnimations().GetCopy(DataHolder.Skill(this.useID).level[this.useLevel].animationID);
					}
				}
				else if(this.IsItem() && DataHolder.Item(this.useID).battleAnimation)
				{
					this.activeAnimation = DataHolder.BattleAnimations().GetCopy(DataHolder.Item(this.useID).animationID);
				}
				else if(this.IsDefend() && user.battleAnimations.animateDefend)
				{
					this.activeAnimation = DataHolder.BattleAnimations().GetCopy(user.GetDefendAnimation());
				}
				else if(this.IsEscape() && user.battleAnimations.animateEscape)
				{
					this.activeAnimation = DataHolder.BattleAnimations().GetCopy(user.GetEscapeAnimation());
				}
				else if(this.IsDeath() && user.battleAnimations.animateDeath)
				{
					this.activeAnimation = DataHolder.BattleAnimations().GetCopy(user.GetDeathAnimation());
				}
			}
			
			if(this.activeAnimation != null)
			{
				this.activeAnimation.StartEvent(this);
			}
			else
			{
				if(target != null && target.Length > 0)
				{
					this.Calculate(target, 1);
				}
				this.AnimationFinished();
			}
		}
		else
		{
			this.AnimationFinished();
		}
	}
	
	public CombatantAnimation[] Calculate(Combatant[] ts, float damageFactor)
	{
		if(DataHolder.BattleCam().blockAnimationCams)
		{
			if(ts.Length == 1 && ts[0].prefabInstance != null)
			{
				DataHolder.BattleCam().SetLatestDamage(ts[0].prefabInstance.transform);
			}
			else if(ts.Length > 1)
			{
				DataHolder.BattleCam().SetLatestDamage(
							DataHolder.BattleSystem().GetGroupCenter(ts).transform);
			}
		}
		
		CombatantAnimation[] anims = new CombatantAnimation[ts.Length];
		doCounter = new bool[ts.Length];
		if(user != null && ts.Length > 0 && ts[0] != null && 
			((!user.isDead && !this.IsDeath()) || this.IsDeath()))
		{
			if((this.IsAttack() || this.IsCounter()) && !user.IsBlockAttack())
			{
				bool hit = false;
				if(this.IsAttack())
				{
					hit = user.UseBaseAttack(ts[0], this, true, damageFactor, this.damageMultiplier);
				}
				else if(this.IsCounter())
				{
					hit = user.UseBaseAttack(ts[0], this, false, damageFactor, this.damageMultiplier);
				}
				if(hit)
				{
					anims[0] = CombatantAnimation.DAMAGE;
					ts[0].CheckEffectsAttack();
				}
				else
				{
					anims[0] = CombatantAnimation.EVADE;
				}
			}
			else if(this.IsSkill() && !user.IsBlockSkills() && DataHolder.Skill(this.useID).CanUse(user, this.useLevel))
			{
				// check for reflect
				if(ts != null && ts.Length > 0 &&
					DataHolder.Skill(this.useID).level[this.useLevel].reflectable)
				{
					for(int i=0; i<ts.Length; i++)
					{
						if(ts[i].HasReflect())
						{
							ts[i] = user;
						}
					}
				}
				anims = DataHolder.Skill(this.useID).Use(user, ts, this, !this.userConsumeDone, 
						this.useLevel, damageFactor, this.damageMultiplier);
				this.userConsumeDone = true;
			}
			else if(this.IsItem() && !user.IsBlockItems())
			{
				anims = DataHolder.Item(this.useID).Use(user, ts, this, this.useID, damageFactor, this.damageMultiplier);
			}
			else if(this.IsDefend() && !user.IsBlockDefend())
			{
				user.isDefending = true;
			}
			else if(this.IsEscape() && !user.IsBlockEscape())
			{
				for(int i=0; i<ts.Length; i++)
				{
					if(DataHolder.GameSettings().GetRandom() <= 
							(DataHolder.Formula(DataHolder.BattleSystem().escapeFormula).
								Calculate(user, ts[i]) + user.GetEscapeBonus()))
					{
						user.Escape();
					}
				}
			}
			else if(this.IsDeath())
			{
				anims[0] = CombatantAnimation.DEATH;
			}
		}
		return anims;
	}
	
	public void CalcTimeUse()
	{
		if(this.IsAttack() && !DataHolder.BattleSystem().attackEndTurn)
		{
			this.timeUse = DataHolder.BattleSystem().attackTimebarUse;
		}
		else if(this.IsSkill() && !DataHolder.Skill(this.useID).level[this.useLevel].endTurn)
		{
			this.timeUse = DataHolder.Skill(this.useID).level[this.useLevel].timebarUse;
		}
		else if(this.IsItem() && !DataHolder.BattleSystem().itemEndTurn)
		{
			this.timeUse = DataHolder.BattleSystem().itemTimebarUse;
		}
		else if(this.IsDefend() && !DataHolder.BattleSystem().defendEndTurn)
		{
			this.timeUse = DataHolder.BattleSystem().defendTimebarUse;
		}
		else if(this.IsEscape() && !DataHolder.BattleSystem().escapeEndTurn)
		{
			this.timeUse = DataHolder.BattleSystem().escapeTimebarUse;
		}
		else
		{
			this.timeUse = DataHolder.BattleSystem().actionBorder;
		}
	}
	
	public float GetTimeUse()
	{
		if(this.timeUse == 0) this.CalcTimeUse();
		return this.timeUse;
	}
	
	public void AnimationFinished()
	{
		// set last skill
		if(user != null)
		{
			if(this.IsSkill()) user.lastSkill = this.useID;
			else user.lastSkill = -1;
			
			// battle animation callbacks
			if(this.IsDeath()) user.Died();
			else if(this.IsAttack()) user.NextBaseAttack();
			
			if(!DataHolder.BattleSystem().IsRealTime() &&
				doCounter != null && doCounter.Length == target.Length)
			{
				for(int i=0; i<target.Length; i++)
				{
					if(doCounter[i] && !target[i].IsInAction())
					{
						DataHolder.BattleSystem().UnshiftAction(new BattleAction(AttackSelection.COUNTER, target[i], user.battleID, -1, 0));
					}
				}
			}
		}
		
		if(this.activeAnimation != null) this.activeAnimation.battleAction = null;
		this.activeAnimation = null;
		if(DataHolder.BattleSystem().IsActiveTime() && user != null && 
			!user.isDead && !this.IsCounter() && !this.autoAttackFlag)
		{
			user.timeBar -= this.GetTimeUse();
			user.usedTimeBar -= this.GetTimeUse();
			if(user.usedTimeBar < 0) user.usedTimeBar = 0;
			if(user.timeBar >= DataHolder.BattleSystem().maxTimebar)
			{
				user.timeBar = DataHolder.BattleSystem().maxTimebar-0.1f;
			}
		}
		
		if(user != null)
		{
			user.SetInAction(false);
			user.NextAction();
		}
		DataHolder.BattleSystem().RemoveActiveAction(this);
		DataHolder.BattleSystem().CheckDeath();
		if(!DataHolder.BattleSystem().dynamicCombat)
		{
			DataHolder.BattleSystem().ActionFinished();
		}
	}
	
	public void StopAction()
	{
		if(this.activeAnimation != null) this.activeAnimation.StopEvent();
	}
	
	public void Tick()
	{
		if(this.activeAnimation != null)
		{
			this.activeAnimation.TimeTick(GameHandler.DeltaTime);
		}
	}
}