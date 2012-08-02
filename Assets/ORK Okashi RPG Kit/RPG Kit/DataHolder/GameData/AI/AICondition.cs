
using UnityEngine;
using System.Collections;

public class AICondition
{
	public AIConditionTarget target = AIConditionTarget.SELF;
	public AIConditionType type = AIConditionType.STATUS;
	
	// status value
	public int statusID = 0;
	public ValueSetter statusSetter = ValueSetter.VALUE;
	public int statusValue = 0;
	public ValueCheck statusCheck = ValueCheck.EQUALS;
	public int checkStatusID = 0;
	
	// effect
	public int effectID = 0;
	public ActiveSelection effectActive = ActiveSelection.INACTIVE;
	
	// element
	public int elementID = 0;
	public int elementValue = 0;
	public ValueCheck elementCheck = ValueCheck.EQUALS;
	
	// turn
	public int turn = 1;
	public bool everyTurn = false;
	
	// chance
	public int chance = 0;
	
	// death
	public bool isDead = false;
	
	// race
	public int raceID = 0;
	
	// size
	public int sizeID = 0;
	
	public AICondition()
	{
		
	}
	
	// -1 = invalid
	// -2 = valid, but no target
	// >=0 = battleID of the target
	public int IsValid(int id, Combatant[] allies, Combatant[] enemies)
	{
		int valid = -1;
		// check for status value
		if(AIConditionType.STATUS.Equals(this.type))
		{
			if(AIConditionTarget.SELF.Equals(this.target))
			{
				valid = this.CheckStatus(this.GetSelf(id, allies));
			}
			else if(AIConditionTarget.ALLY.Equals(this.target))
			{
				for(int i=0; i<allies.Length; i++)
				{
					valid = this.CheckStatus(allies[i]);
					if(valid != -1)
					{
						break;
					}
				}
			}
			else if(AIConditionTarget.ENEMY.Equals(this.target))
			{
				for(int i=0; i<enemies.Length; i++)
				{
					valid = this.CheckStatus(enemies[i]);
					if(valid != -1)
					{
						break;
					}
				}
			}
		}
		// check for effect
		else if(AIConditionType.EFFECT.Equals(this.type))
		{
			if(AIConditionTarget.SELF.Equals(this.target))
			{
				valid = this.CheckEffect(this.GetSelf(id, allies));
			}
			else if(AIConditionTarget.ALLY.Equals(this.target))
			{
				for(int i=0; i<allies.Length; i++)
				{
					valid = this.CheckEffect(allies[i]);
					if(valid != -1)
					{
						break;
					}
				}
			}
			else if(AIConditionTarget.ENEMY.Equals(this.target))
			{
				for(int i=0; i<enemies.Length; i++)
				{
					valid = this.CheckEffect(enemies[i]);
					if(valid != -1)
					{
						break;
					}
				}
			}
		}
		// check for element
		else if(AIConditionType.ELEMENT.Equals(this.type))
		{
			if(AIConditionTarget.SELF.Equals(this.target))
			{
				Combatant c = this.GetSelf(id, allies);
				if(!c.isDead)
				{
					int val = c.GetElementDefence(elementID);
					if(this.CheckValue(val, this.elementValue, this.elementCheck))
					{
						valid = id;
					}
				}
			}
			else if(AIConditionTarget.ALLY.Equals(this.target))
			{
				for(int i=0; i<allies.Length; i++)
				{
					int val = allies[i].GetElementDefence(this.elementID);
					if(!allies[i].isDead && this.CheckValue(val, this.elementValue, this.elementCheck))
					{
						valid = allies[i].battleID;
						break;
					}
				}
			}
			else if(AIConditionTarget.ENEMY.Equals(this.target))
			{
				for(int i=0; i<enemies.Length; i++)
				{
					int val = enemies[i].GetElementDefence(this.elementID);
					if(!enemies[i].isDead && this.CheckValue(val, this.elementValue, this.elementCheck))
					{
						valid = enemies[i].battleID;
						break;
					}
				}
			}
		}
		// check for turn
		else if(AIConditionType.TURN.Equals(this.type))
		{
			int currentTurn = this.GetSelf(id, allies).currentTurn;
			if((this.everyTurn && (currentTurn % this.turn) == 0) ||
				(!this.everyTurn && this.turn == currentTurn))
			{
				valid = -2;
			}
		}
		// check for chance
		else if(AIConditionType.CHANCE.Equals(this.type))
		{
			if(DataHolder.GameSettings().GetRandom() <= this.chance)
			{
				valid = -2;
			}
		}
		// check for death
		else if(AIConditionType.DEATH.Equals(this.type))
		{
			if(AIConditionTarget.SELF.Equals(this.target))
			{
				valid = this.CheckDeath(this.GetSelf(id, allies));
			}
			else if(AIConditionTarget.ALLY.Equals(this.target))
			{
				for(int i=0; i<allies.Length; i++)
				{
					valid = this.CheckDeath(allies[i]);
					if(valid != -1)
					{
						break;
					}
				}
			}
			else if(AIConditionTarget.ENEMY.Equals(this.target))
			{
				for(int i=0; i<enemies.Length; i++)
				{
					valid = this.CheckDeath(enemies[i]);
					if(valid != -1)
					{
						break;
					}
				}
			}
		}
		// check for race
		else if(AIConditionType.RACE.Equals(this.type))
		{
			if(AIConditionTarget.SELF.Equals(this.target))
			{
				valid = this.CheckRace(this.GetSelf(id, allies));
			}
			else if(AIConditionTarget.ALLY.Equals(this.target))
			{
				for(int i=0; i<allies.Length; i++)
				{
					valid = this.CheckRace(allies[i]);
					if(valid != -1)
					{
						break;
					}
				}
			}
			else if(AIConditionTarget.ENEMY.Equals(this.target))
			{
				for(int i=0; i<enemies.Length; i++)
				{
					valid = this.CheckRace(enemies[i]);
					if(valid != -1)
					{
						break;
					}
				}
			}
		}
		// check for size
		else if(AIConditionType.SIZE.Equals(this.type))
		{
			if(AIConditionTarget.SELF.Equals(this.target))
			{
				valid = this.CheckSize(this.GetSelf(id, allies));
			}
			else if(AIConditionTarget.ALLY.Equals(this.target))
			{
				for(int i=0; i<allies.Length; i++)
				{
					valid = this.CheckSize(allies[i]);
					if(valid != -1)
					{
						break;
					}
				}
			}
			else if(AIConditionTarget.ENEMY.Equals(this.target))
			{
				for(int i=0; i<enemies.Length; i++)
				{
					valid = this.CheckSize(enemies[i]);
					if(valid != -1)
					{
						break;
					}
				}
			}
		}
		return valid;
	}
	
	private Combatant GetSelf(int id, Combatant[] allies)
	{
		Combatant self = null;
		for(int i=0; i<allies.Length; i++)
		{
			if(allies[i].battleID == id)
			{
				self = allies[i];
			}
		}
		return self;
	}
	
	private bool CheckValue(int val, int cVal, ValueCheck check)
	{
		bool valid = false;
		if(ValueCheck.EQUALS.Equals(check) && val == cVal)
		{
			valid = true;
		}
		else if(ValueCheck.LESS.Equals(check) && val < cVal)
		{
			valid = true;
		}
		else if(ValueCheck.GREATER.Equals(check) && val > cVal)
		{
			valid = true;
		}
		return valid;
	}
	
	private int CheckEffect(Combatant combatant)
	{
		int valid = -1;
		if(!combatant.isDead)
		{
			if((ActiveSelection.INACTIVE.Equals(this.effectActive) && !combatant.IsEffectSet(this.effectID)) ||
				(ActiveSelection.ACTIVE.Equals(this.effectActive) && combatant.IsEffectSet(this.effectID)))
			{
				valid = combatant.battleID;
			}
		}
		return valid;
	}
	
	private int CheckStatus(Combatant combatant)
	{
		int valid = -1;
		if(!combatant.isDead)
		{
			int val = combatant.status[this.statusID].GetValue();
			int cVal = 0;
			if(ValueSetter.PERCENT.Equals(this.statusSetter))
			{
				cVal = combatant.status[this.checkStatusID].GetValue();
				cVal *= this.statusValue;
				cVal /= 100;
			}
			else if(ValueSetter.VALUE.Equals(this.statusSetter))
			{
				cVal = this.statusValue;
			}
			if(this.CheckValue(val, cVal, this.statusCheck))
			{
				valid = combatant.battleID;
			}
		}
		return valid;
	}
	
	private int CheckDeath(Combatant combatant)
	{
		int valid = -1;
		if(combatant.isDead == this.isDead)
		{
			valid = combatant.battleID;
		}
		return valid;
	}
	
	private int CheckRace(Combatant combatant)
	{
		int valid = -1;
		if(combatant.raceID == this.raceID)
		{
			valid = combatant.battleID;
		}
		return valid;
	}
	
	private int CheckSize(Combatant combatant)
	{
		int valid = -1;
		if(combatant.sizeID == this.sizeID)
		{
			valid = combatant.battleID;
		}
		return valid;
	}
}