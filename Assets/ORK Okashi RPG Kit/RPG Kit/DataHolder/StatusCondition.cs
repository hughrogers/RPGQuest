
using UnityEngine;
using System.Collections;

public class StatusCondition
{
	public bool apply = false;
	public bool stopChange = false;
	
	public SimpleOperator simpleOperator = SimpleOperator.ADD;
	public int value = 0;
	public ValueSetter setter = ValueSetter.PERCENT;
	public StatusConditionExecution execution = StatusConditionExecution.CAST;
	public int time = 0;
	
	// ingame
	public int change = 0;
	public int count = 0;
	public int setAfter = 0;
	
	public StatusCondition()
	{
		
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		if(this.stopChange) ht.Add("stopchange", "true");
		ht.Add("operator", this.simpleOperator.ToString());
		ht.Add("value", this.value.ToString());
		ht.Add("setter", this.setter.ToString());
		ht.Add("execution", this.execution.ToString());
		ht.Add("time", this.time.ToString());
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		this.apply = true;
		if(ht.ContainsKey("stopchange")) this.stopChange = true;
		
		this.simpleOperator = (SimpleOperator)System.Enum.Parse(
				typeof(SimpleOperator), (string)ht["operator"]);
		this.value = int.Parse((string)ht["value"]);
		this.setter = (ValueSetter)System.Enum.Parse(
				typeof(ValueSetter), (string)ht["setter"]);
		this.execution = (StatusConditionExecution)System.Enum.Parse(
				typeof(StatusConditionExecution), (string)ht["execution"]);
		this.time = int.Parse((string)ht["time"]);
	}
	
	/*
	============================================================================
	Check functions
	============================================================================
	*/
	public bool OnCast()
	{
		return StatusConditionExecution.CAST.Equals(this.execution);
	}
	
	public bool OnTurn()
	{
		return StatusConditionExecution.TURN.Equals(this.execution);
	}
	
	public bool OnTime()
	{
		return StatusConditionExecution.TIME.Equals(this.execution);
	}
	
	/*
	============================================================================
	Change functions
	============================================================================
	*/
	public void InitChange(int index, Combatant target)
	{
		this.setAfter = this.time*1000;
		if(ValueSetter.PERCENT.Equals(this.setter))
		{
			if(target.status[index].IsConsumable())
			{
				this.change = target.status[target.status[index].maxStatus].GetValue();
			}
			else
			{
				this.change = target.status[index].GetValue();
			}
			this.change *= this.value;
			this.change /= 100;
		}
		else if(ValueSetter.VALUE.Equals(this.setter))
		{
			this.change = this.value;
		}
	}
	
	public void SetChange(int index, Combatant target)
	{
		this.count++;
		this.setAfter = this.time*1000;
		if(SimpleOperator.ADD.Equals(this.simpleOperator))
		{
			target.status[index].AddValue(this.change, false, false, true);
		}
		else if(SimpleOperator.SUB.Equals(this.simpleOperator))
		{
			target.status[index].AddValue(-this.change, false, false, true);
		}
		else if(SimpleOperator.SET.Equals(this.simpleOperator))
		{
			target.status[index].SetValue(this.change, false, false, true);
		}
		for(int i=0; i<target.status.Length; i++)
		{
			target.status[i].CheckBounds();
		}
	}
	
	public void ResetChange(int index, Combatant target)
	{
		if(!target.status[index].IsConsumable())
		{
			if(SimpleOperator.ADD.Equals(this.simpleOperator))
			{
				target.status[index].AddValue(this.change*count, false, false, false);
			}
			else if(SimpleOperator.SUB.Equals(this.simpleOperator))
			{
				target.status[index].AddValue(-this.change*count, false, false, false);
			}
			else if(SimpleOperator.SET.Equals(this.simpleOperator))
			{
				target.status[index].SetValue(this.change, false, false, false);
			}
		}
		for(int i=0; i<target.status.Length; i++)
		{
			target.status[i].CheckBounds();
		}
	}
}
