
using UnityEngine;
using System.Collections;

public class StatusTimeChange
{
	public int statusID = 0;
	
	public bool showText = false;
	public bool forcePerSecond = false;
	
	public bool useFormula = false;
	public int formulaID = 0;
	public float value = 0;
	
	public bool minCheck = false;
	public int minValue = 0;
	
	public bool maxCheck = false;
	public int maxValue = 9999;
	
	// ingame
	public float valueChange = 0;
	public float time = 0;
	
	public StatusTimeChange()
	{
		
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		ht.Add("statusid", this.statusID.ToString());
		ht.Add("showtext", this.showText.ToString());
		ht.Add("forcepersecond", this.forcePerSecond.ToString());
		
		if(this.useFormula) ht.Add("formulaid", this.formulaID.ToString());
		else ht.Add("value", this.value.ToString());
		
		if(this.minCheck) ht.Add("minvalue", this.minValue.ToString());
		if(this.maxCheck) ht.Add("maxvalue", this.maxValue.ToString());
		
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		IntHelper.FromHashtable(ht, "statusid", ref this.statusID);
		BoolHelper.FromHashtable(ht, "showtext", ref this.showText);
		BoolHelper.FromHashtable(ht, "forcepersecond", ref this.forcePerSecond);
		IntHelper.FromHashtable(ht, "formulaid", ref this.formulaID, ref this.useFormula);
		FloatHelper.FromHashtable(ht, "value", ref this.value);
		IntHelper.FromHashtable(ht, "minvalue", ref this.minValue, ref this.minCheck);
		IntHelper.FromHashtable(ht, "maxvalue", ref this.maxValue, ref this.maxCheck);
	}
	
	public StatusTimeChange GetCopy()
	{
		StatusTimeChange stc = new StatusTimeChange();
		stc.SetData(this.GetData(new Hashtable()));
		return stc;
	}
	
	/*
	============================================================================
	Ingame functions
	============================================================================
	*/
	public void Tick(float t, Combatant c)
	{
		if((!this.minCheck || c.status[this.statusID].GetValue() >= this.minValue) &&
			(!this.maxCheck || c.status[this.statusID].GetValue() <= this.maxValue) && 
			!c.status[this.statusID].MaxReached())
		{
			if(this.forcePerSecond)
			{
				this.time += t;
				if(this.time >= 1)
				{
					this.Calculate(1, c);
					this.time -= 1;
				}
			}
			else this.Calculate(t, c);
		}
	}
	
	private void Calculate(float t, Combatant c)
	{
		if(this.useFormula) this.valueChange += DataHolder.Formula(this.formulaID).Calculate(c, c)*t;
		else this.valueChange += this.value*t;
		
		int change = 0;
		if(this.valueChange < 0) change = (int)Mathf.Ceil(this.valueChange);
		else if(this.valueChange > 0) change = (int)Mathf.Floor(this.valueChange);
		if(change != 0)
		{
			this.valueChange -= change;
			c.status[this.statusID].AddValue(change, true, false, this.showText);
		}
	}
}
