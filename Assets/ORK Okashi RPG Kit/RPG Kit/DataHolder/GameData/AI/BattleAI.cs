
using UnityEngine;
using System.Collections;

public class BattleAI
{
	public AIConditionNeeded needed = AIConditionNeeded.ALL;
	
	public AICondition[] condition = new AICondition[0];
	
	public BattleAI()
	{
		
	}
	
	public int IsValid(int id, Combatant[] allies, Combatant[] enemies)
	{
		int valid = -1;
		if(AIConditionNeeded.ALL.Equals(this.needed))
		{
			for(int i=0; i<this.condition.Length; i++)
			{
				valid = this.condition[i].IsValid(id, allies, enemies);
				if(valid == -1) break;
			}
		}
		else if(AIConditionNeeded.ONE.Equals(this.needed))
		{
			for(int i=0; i<this.condition.Length; i++)
			{
				valid = this.condition[i].IsValid(id, allies, enemies);
				if(valid != -1) break;
			}
		}
		return valid;
	}
}
