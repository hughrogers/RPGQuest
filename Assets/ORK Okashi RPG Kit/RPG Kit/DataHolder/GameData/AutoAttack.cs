
using UnityEngine;
using System.Collections;

public class AutoAttack
{
	public bool active = false;
	
	public bool useSkill = false;
	public int skillID = 0;
	public int skillLevel = 1;
	
	public float interval = 0;
	
	public AutoAttack()
	{
		
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		if(this.active)
		{
			ht.Add("interval", this.interval.ToString());
			if(this.useSkill)
			{
				ht.Add("skill", this.skillID.ToString());
				ht.Add("slvl", this.skillLevel.ToString());
			}
		}
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		if(ht.ContainsKey("interval"))
		{
			this.active = true;
			this.interval = float.Parse((string)ht["interval"]);
			if(ht.ContainsKey("skill"))
			{
				this.useSkill = true;
				this.skillID = int.Parse((string)ht["skill"]);
				this.skillLevel = int.Parse((string)ht["slvl"]);
			}
		}
		else this.active = false;
	}
	
	/*
	============================================================================
	Use functions
	============================================================================
	*/
	public BattleAction GetAction(Combatant c)
	{
		BattleAction action = null;
		bool partyTarget = DataHolder.BattleControl().UseAutoAttackTarget(c);
		if(this.active && !c.IsStopMove() &&
			(DataHolder.BattleSystem().dynamicCombat || partyTarget))
		{
			int random = c.lastTargetBattleID;
			if(this.useSkill)
			{
				Skill s = DataHolder.Skill(this.skillID);
				if(s.CanUse(c, this.skillLevel-1) && !c.IsBlockSkills() && !s.isPassive)
				{
					if(s.TargetSelf())
					{
						action = new BattleAction(AttackSelection.SKILL, c, c.battleID, this.skillID, this.skillLevel-1);
					}
					else if(s.TargetNone())
					{
						action = new BattleAction(AttackSelection.SKILL, c, BattleAction.NONE, this.skillID, this.skillLevel-1);
					}
					else if(s.TargetSingleAlly())
					{
						if(c is Character && (random < 0 || 
							DataHolder.BattleSystem().GetCombatantForBattleID(random) is Enemy))
						{
							random = BattleAction.RANDOM_CHARACTER;
						}
						else if(c is Enemy && (random < 0 || 
							DataHolder.BattleSystem().GetCombatantForBattleID(random) is Character))
						{
							random = BattleAction.RANDOM_ENEMY;
						}
						action = new BattleAction(AttackSelection.SKILL, c, random, this.skillID, this.skillLevel-1);
					}
					else if(s.TargetSingleEnemy())
					{
						if(c is Character && (random < 0 || 
							DataHolder.BattleSystem().GetCombatantForBattleID(random) is Character))
						{
							random = BattleAction.RANDOM_ENEMY;
						}
						else if(c is Enemy && (random < 0 || 
							DataHolder.BattleSystem().GetCombatantForBattleID(random) is Enemy))
						{
							random = BattleAction.RANDOM_CHARACTER;
						}
						action = new BattleAction(AttackSelection.SKILL, c, random, this.skillID, this.skillLevel-1);
					}
					else if(s.TargetAllyGroup())
					{
						action = new BattleAction(AttackSelection.SKILL, c, BattleAction.ALL_CHARACTERS, this.skillID, this.skillLevel-1);
					}
					else if(s.TargetEnemyGroup())
					{
						action = new BattleAction(AttackSelection.SKILL, c, BattleAction.ALL_ENEMIES, this.skillID, this.skillLevel-1);
					}
					// party target
					if(partyTarget && !s.TargetSelf() && !s.TargetAlly() && !s.TargetGroup())
					{
						action.targetID = BattleAction.PARTY_TARGET;
					}
				}
			}
			// use attack
			else if(!c.IsBlockAttack())
			{
				if(c is Character && (random < 0 || 
					DataHolder.BattleSystem().GetCombatantForBattleID(random) is Character))
				{
					random = BattleAction.RANDOM_ENEMY;
				}
				else if(c is Enemy && (random < 0 || 
					DataHolder.BattleSystem().GetCombatantForBattleID(random) is Enemy))
				{
					random = BattleAction.RANDOM_CHARACTER;
				}
				action = new BattleAction(AttackSelection.ATTACK, c, random, -1, 0);
				// party target
				if(partyTarget)
				{
					action.targetID = BattleAction.PARTY_TARGET;
				}
			}
			if(action != null)
			{
				action.autoAttackFlag = true;
				// not in range, clear
				if(!action.InRange())
				{
					action.type = AttackSelection.NONE;
				}
			}
		}
		if(action == null) action = new BattleAction(AttackSelection.NONE, c, BattleAction.NONE, -1, 0);
		return action;
	}
}
