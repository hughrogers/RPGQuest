
using UnityEngine;

public class StatusEffect
{
	// effect settings
	public bool stopMove = false;
	public bool stopMovement = false;
	public bool autoAttack = false;
	public bool attackFriends = false;
	public bool blockAttack = false;
	public bool blockSkills = false;
	public bool blockItems = false;
	public bool blockDefend = false;
	public bool blockEscape = false;
	public bool reflectSkills = false;
	
	public bool hitChance = false;
	public int hitFormula = 0;
	
	// effect end
	public bool endWithBattle = true;
	public bool endOnAttack = false;
	public StatusEffectEnd end = StatusEffectEnd.NONE;
	public int endValue = 0;
	public int endChance = 0;
	
	public int[] effectChangeID = new int[0];
	public SkillEffect[] endEffectChanges = new SkillEffect[0];
	
	// element cast
	public bool setElement = false;
	public int attackElement = 0;
	
	public StatusCondition[] condition;
	
	public bool blockBaseAttacks = false;
	public bool[] skillTypeBlock = new bool[DataHolder.SkillTypeCount];
	
	// element effectiveness
	public int[] elementValue;
	public SimpleOperator[] elementOperator;
	
	// race damage factor
	public int[] raceValue;
	// size damage factor
	public int[] sizeValue;
	
	public BonusSettings bonus = new BonusSettings();
	
	// auto apply
	public bool autoApply = false;
	public AIConditionNeeded applyNeeded = AIConditionNeeded.ALL;
	public StatusRequirement[] applyRequirement = new StatusRequirement[0];
	
	// auto remove
	public bool autoRemove = false;
	public AIConditionNeeded removeNeeded = AIConditionNeeded.ALL;
	public StatusRequirement[] removeRequirement = new StatusRequirement[0];
	
	// prefab
	public EffectPrefab[] prefab = new EffectPrefab[0];
	
	// ingame
	public int realID = 0;
	public int endAfter = 0;
	private Combatant combatant;
	private bool initialized = false;
	
	public StatusEffect()
	{
		
	}
	
	/*
	============================================================================
	Status requirement functions
	============================================================================
	*/
	public void AddAutoApply()
	{
		this.applyRequirement = ArrayHelper.Add(new StatusRequirement(), this.applyRequirement);
	}
	
	public void RemoveAutoApply(int index)
	{
		this.applyRequirement = ArrayHelper.Remove(index, this.applyRequirement);
	}
	
	public void AddAutoRemove()
	{
		this.removeRequirement = ArrayHelper.Add(new StatusRequirement(), this.removeRequirement);
	}
	
	public void RemoveAutoRemove(int index)
	{
		this.removeRequirement = ArrayHelper.Remove(index, this.removeRequirement);
	}
	
	public bool CheckAutoApply(Combatant c)
	{
		return this.CheckRequirements(c, this.autoApply, this.applyRequirement, this.applyNeeded);
	}
	
	public void CheckAutoRemove(Combatant c)
	{
		if(this.CheckRequirements(c, this.autoRemove, this.removeRequirement, this.removeNeeded))
		{
			c.RemoveEffect(this.realID);
		}
	}
	
	private bool CheckRequirements(Combatant c, bool doCheck, StatusRequirement[] req, AIConditionNeeded needed)
	{
		bool check = false;
		if(doCheck)
		{
			check = true;
			bool any = false;
			for(int i=0; i<req.Length; i++)
			{
				if(req[i].CheckRequirement(c))
				{
					any = true;
				}
				else if(AIConditionNeeded.ALL.Equals(needed))
				{
					check = false;
					break;
				}
			}
			if(AIConditionNeeded.ONE.Equals(needed) && !any && 
				req.Length > 0)
			{
				check = false;
			}
		}
		return check;
	}
	
	/*
	============================================================================
	Prefab functions
	============================================================================
	*/
	public void AddPrefab()
	{
		this.prefab = ArrayHelper.Add(new EffectPrefab(), this.prefab);
	}
	
	public void RemovePrefab(int index)
	{
		this.prefab = ArrayHelper.Remove(index, this.prefab);
	}
	
	public void AddPrefabs(Combatant c)
	{
		for(int i=0; i<this.prefab.Length; i++)
		{
			this.prefab[i].Create(c);
		}
	}
	
	public void DestroyPrefabs()
	{
		for(int i=0; i<this.prefab.Length; i++)
		{
			this.prefab[i].Destroy();
		}
	}
	
	/*
	============================================================================
	End effect change functions
	============================================================================
	*/
	public void AddEndChange()
	{
		this.effectChangeID = ArrayHelper.Add(0, this.effectChangeID);
		this.endEffectChanges = ArrayHelper.Add(SkillEffect.NONE, this.endEffectChanges);
	}
	
	public void RemoveEndChange(int index)
	{
		this.effectChangeID = ArrayHelper.Remove(index, this.effectChangeID);
		this.endEffectChanges = ArrayHelper.Remove(index, this.endEffectChanges);
	}
	
	public void CheckEndEffectChanges(Combatant target)
	{
		for(int i=0; i<this.effectChangeID.Length; i++)
		{
			if(SkillEffect.ADD.Equals(this.endEffectChanges[i]))
			{
				target.AddEffect(this.effectChangeID[i], target);
			}
			else if(SkillEffect.REMOVE.Equals(this.endEffectChanges[i]))
			{
				target.RemoveEffect(this.effectChangeID[i]);
			}
		}
	}
	
	/*
	============================================================================
	Ingame functions
	============================================================================
	*/
	public void ReApply()
	{
		this.endAfter = this.endValue;
		if(StatusEffectEnd.TIME.Equals(this.end))
		{
			this.endAfter *= 1000;
		}
	}
	
	public bool ApplyEffect(Combatant user, Combatant target)
	{
		bool applied = false;
		if(target.CanApplyEffect(this.realID) &&
				(!this.hitChance ||
				DataHolder.GameSettings().GetRandom() <= DataHolder.Formulas().formula[this.hitFormula].Calculate(user, target)))
		{
			for(int i=0; i<this.condition.Length; i++)
			{
				if(this.condition[i].apply && !this.condition[i].stopChange)
				{
					this.condition[i].InitChange(i, target);
					if(this.condition[i].OnCast())
					{
						this.condition[i].SetChange(i, target);
					}
				}
			}
			
			if(StatusEffectEnd.TIME.Equals(this.end)) this.endAfter = this.endValue*1000;
			else this.endAfter = this.endValue;
			this.combatant = target;
			applied = true;
			this.initialized = true;
		}
		return applied;
	}
	
	public void RemoveEffect(Combatant target)
	{
		if(target.CanRemoveEffect(this.realID))
		{
			this.StopEffect(target);
		}
	}
	
	public void StopEffect(Combatant target)
	{
		target.RemoveEffect(this);
		this.CheckEndEffectChanges(target);
	}
	
	public void ResetChange(Combatant target)
	{
		for(int i=0; i<this.condition.Length; i++)
		{
			if(this.condition[i].apply && !this.condition[i].stopChange)
			{
				this.condition[i].ResetChange(i, target);
			}
		}
	}
	
	public void CheckEffect()
	{
		if(StatusEffectEnd.TURN.Equals(this.end))
		{
			this.endAfter--;
			if(this.endAfter <= 0 && DataHolder.GameSettings().GetRandom() <= this.endChance)
			{
				this.combatant.RemoveEffect(this.realID);
			}
		}
		for(int i=0; i<this.condition.Length; i++)
		{
			if(this.condition[i].apply && !this.condition[i].stopChange && this.condition[i].OnTurn())
			{
				this.condition[i].SetChange(i, this.combatant);
			}
		}
	}
	
	public void TimeCheck()
	{
		if(!GameHandler.IsGamePaused() && this.initialized)
		{
			if(StatusEffectEnd.TIME.Equals(this.end))
			{
				this.endAfter -= (int)(GameHandler.DeltaBattleTime*1000);
			}
			for(int i=0; i<this.condition.Length; i++)
			{
				if(this.condition[i].apply && !this.condition[i].stopChange && this.condition[i].OnTime())
				{
					this.condition[i].setAfter -= (int)(GameHandler.DeltaBattleTime*1000);
					if(this.condition[i].setAfter <= 0)
					{
						this.condition[i].SetChange(i, this.combatant);
					}
				}
			}
			
			if(StatusEffectEnd.TIME.Equals(this.end) && this.endAfter <= 0)
			{
				this.combatant.RemoveEffect(this.realID);
			}
		}
	}
	
	public void EndBattle()
	{
		if(this.endWithBattle)
		{
			this.StopEffect(this.combatant);
		}
	}
	
	public string GetName()
	{
		return DataHolder.Effects().GetName(this.realID);
	}
}
