
using UnityEngine;
using System.Collections;

public class StatusRequirement
{
	public StatusNeeded statusNeeded = StatusNeeded.STATUS_VALUE;
	public int statID = 0;
	public ValueCheck comparison = ValueCheck.EQUALS;
	public int value = 0;
	public ValueSetter setter = ValueSetter.VALUE;
	public bool classLevel = false;
	
	public StatusRequirement()
	{
		
	}
	
	public StatusRequirement(Hashtable ht)
	{
		this.SetData(ht);
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		ht.Add("statusneeded", this.statusNeeded.ToString());
		ht.Add("statid", this.statID.ToString());
		ht.Add("comparison", this.comparison.ToString());
		ht.Add("value", this.value.ToString());
		ht.Add("setter", this.setter.ToString());
		if(this.classLevel && StatusNeeded.LEVEL.Equals(this.statusNeeded))
		{	
			ht.Add("classlevel", "true");
		}
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		this.statusNeeded = (StatusNeeded)System.Enum.Parse(typeof(StatusNeeded), (string)ht["statusneeded"]);
		this.comparison = (ValueCheck)System.Enum.Parse(typeof(ValueCheck), (string)ht["comparison"]);
		this.setter = (ValueSetter)System.Enum.Parse(typeof(ValueSetter), (string)ht["setter"]);
		this.statID = int.Parse((string)ht["statid"]);
		this.value = int.Parse((string)ht["value"]);
		if(ht.ContainsKey("classlevel")) this.classLevel = true;
	}
	
	/*
	============================================================================
	Check functions
	============================================================================
	*/
	public bool CheckRequirement(Combatant c)
	{
		bool check = false;
		if(StatusNeeded.STATUS_VALUE.Equals(this.statusNeeded) &&
			c.status[this.statID].CompareTo(this.value, this.comparison, this.setter, c))
		{
			check = true;
		}
		else if(StatusNeeded.ELEMENT.Equals(this.statusNeeded))
		{
			int element = c.GetElementDefence(this.statID);
			if((ValueCheck.EQUALS.Equals(this.comparison) && element == this.value) ||
				(ValueCheck.LESS.Equals(this.comparison) && element < this.value) ||
				(ValueCheck.GREATER.Equals(this.comparison) && element > this.value))
			{
				check = true;
			}
		}
		else if(StatusNeeded.SKILL.Equals(this.statusNeeded) && c.HasSkill(this.statID, 0))
		{
			check = true;
		}
		else if(StatusNeeded.RACE.Equals(this.statusNeeded) && c.raceID == this.statID)
		{
			check = true;
		}
		else if(StatusNeeded.SIZE.Equals(this.statusNeeded) && c.sizeID == this.statID)
		{
			check = true;
		}
		else if(StatusNeeded.LEVEL.Equals(this.statusNeeded))
		{
			int lvl = c.currentLevel;
			if(this.classLevel) lvl = c.currentClassLevel;
			if((ValueCheck.EQUALS.Equals(this.comparison) && lvl == this.value) ||
				(ValueCheck.LESS.Equals(this.comparison) && lvl < this.value) ||
				(ValueCheck.GREATER.Equals(this.comparison) && lvl > this.value))
			{
				check = true;
			}
		}
		return check;
	}
}
