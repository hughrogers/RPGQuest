
using UnityEngine;
using System.Collections;

public class SkillBlock
{
	public int skillID = -1;
	public float time = 0;
	public StatusEffectEnd type = StatusEffectEnd.NONE;
	
	public SkillBlock(int id, float t, StatusEffectEnd ty)
	{
		this.skillID = id;
		this.time = t;
		this.type = ty;
	}
	
	/*
	============================================================================
	Time functions
	============================================================================
	*/
	public void ReduceTurn(Combatant c)
	{
		if(StatusEffectEnd.TURN.Equals(this.type))
		{
			this.time -= 1;
		}
		this.CheckBlock(c);
	}
	
	public void ReduceTime(Combatant c, float t)
	{
		if(StatusEffectEnd.TIME.Equals(this.type))
		{
			this.time -= t;
		}
		this.CheckBlock(c);
	}
	
	public void CheckBlock(Combatant c)
	{
		if(this.time <= 0)
		{
			c.RemoveSkillBlock(this);
		}
	}
	
	public void EndBattle(Combatant c)
	{
		if(StatusEffectEnd.TURN.Equals(this.type))
		{
			c.RemoveSkillBlock(this);
		}
	}
}
