
using UnityEngine;
using System.Collections;

public class Skill : Useable
{
	public int realID = -1;
	
	// skill settings
	public bool isPassive = false;
	public int skilltype = 0;
	
	// skill level
	public SkillLevel[] level = new SkillLevel[] {new SkillLevel()};
	
	public Skill()
	{
		
	}
	
	/*
	============================================================================
	Level handling functions
	============================================================================
	*/
	public void AddLevel()
	{
		this.level = ArrayHelper.Add(this.level[this.level.Length-1].GetCopy(), this.level);
	}
	
	public void CopyLevel(int index)
	{
		this.level = ArrayHelper.Add(this.level[index].GetCopy(), this.level);
	}
	
	public void RemoveLevel(int index)
	{
		this.level = ArrayHelper.Remove(index, this.level);
	}
	
	/*
	============================================================================
	Effect functions
	============================================================================
	*/
	public bool CanApplyEffect(int effectID, int lvl)
	{
		return !SkillEffect.REMOVE.Equals(this.level[lvl].skillEffect[effectID]);
	}
	
	public bool CanRemoveEffect(int effectID, int lvl)
	{
		return !SkillEffect.ADD.Equals(this.level[lvl].skillEffect[effectID]);
	}
	
	/*
	============================================================================
	Use functions
	============================================================================
	*/
	public void UserConsume(Combatant user, int lvl)
	{
		this.level[lvl].UserConsume(user);
	}
	
	public CombatantAnimation[] Use(Combatant user, Combatant[] target, BattleAction ba, 
			bool uc, int lvl, float damageFactor, float damageMultiplier)
	{
		return this.level[lvl].Use(user, target, ba, uc, this.realID, damageFactor, damageMultiplier);
	}
	
	/*
	============================================================================
	Check functions
	============================================================================
	*/
	public bool InRange(Combatant user, Combatant target, int lvl)
	{
		return this.level[lvl].useRange.InRange(user, target);
	}
	
	public bool CanUse(Combatant c, int lvl)
	{
		return this.level[lvl].CanUse(c, this.realID);
	}
	
	public string GetSkillCostString(Combatant c, int lvl)
	{
		return this.level[lvl].GetSkillCostString(c);
	}
}