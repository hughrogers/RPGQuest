
using UnityEngine;
using System.Collections;

public class ValueChange
{
	public bool active = false;
	public SimpleOperator simpleOperator = SimpleOperator.ADD;
	public FormulaChooser formulaChooser = FormulaChooser.VALUE;
	public int value = 0;
	public int status = 0;
	public int formula = 0;
	public int randomMin = 0;
	public int randomMax = 0;
	public float efficiency = 1.0f;
	public bool cancelSkills = false;
	public bool blockable = false;
	public bool ignoreDefend = false;
	public bool ignoreElement = false;
	public bool ignoreRace = false;
	public bool ignoreSize = false;
	
	public ValueChange()
	{
		
	}
	
	public ValueChange(Hashtable ht)
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
		ht.Add("operator", this.simpleOperator.ToString());
		ht.Add("formulachooser", this.formulaChooser.ToString());
		if(this.cancelSkills) ht.Add("cancelskills", "true");
		if(this.blockable) ht.Add("blockable", "true");
		if(this.ignoreDefend) ht.Add("ignoredefend", "true");
		if(this.ignoreElement) ht.Add("ignoreelement", "true");
		if(this.ignoreRace) ht.Add("ignorerace", "true");
		if(this.ignoreSize) ht.Add("ignoresize", "true");
		if(this.efficiency != 1.0f) ht.Add("efficiency", this.efficiency.ToString());
		if(FormulaChooser.VALUE.Equals(this.formulaChooser))
		{
			ht.Add("value", this.value.ToString());
		}
		else if(FormulaChooser.STATUS.Equals(this.formulaChooser))
		{
			ht.Add("status", this.status.ToString());
		}
		else if(FormulaChooser.FORMULA.Equals(this.formulaChooser))
		{
			ht.Add("formula", this.formula.ToString());
		}
		else if(FormulaChooser.RANDOM.Equals(this.formulaChooser))
		{
			ht.Add("min", this.randomMin.ToString());
			ht.Add("max", this.randomMax.ToString());
		}
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		this.active = true;
		this.simpleOperator = (SimpleOperator)System.Enum.Parse(typeof(SimpleOperator), (string)ht["operator"]);
		this.formulaChooser = (FormulaChooser)System.Enum.Parse(typeof(FormulaChooser), (string)ht["formulachooser"]);
		if(ht.ContainsKey("cancelskills")) this.cancelSkills = true;
		if(ht.ContainsKey("blockable")) this.blockable = true;
		if(ht.ContainsKey("ignoredefend")) this.ignoreDefend = true;
		if(ht.ContainsKey("ignoreelement")) this.ignoreElement = true;
		if(ht.ContainsKey("ignorerace")) this.ignoreRace = true;
		if(ht.ContainsKey("ignoresize")) this.ignoreSize = true;
		if(ht.ContainsKey("efficiency")) this.efficiency = float.Parse((string)ht["efficiency"]);
		
		if(FormulaChooser.VALUE.Equals(this.formulaChooser))
		{
			this.value = int.Parse((string)ht["value"]);
		}
		else if(FormulaChooser.STATUS.Equals(this.formulaChooser))
		{
			this.status = int.Parse((string)ht["status"]);
		}
		else if(FormulaChooser.FORMULA.Equals(this.formulaChooser))
		{
			this.formula = int.Parse((string)ht["formula"]);
		}
		else if(FormulaChooser.RANDOM.Equals(this.formulaChooser))
		{
			this.randomMin = int.Parse((string)ht["min"]);
			this.randomMax = int.Parse((string)ht["max"]);
		}
	}
	
	public bool CompareTo(ValueChange vc)
	{
		bool same = false;
		if(this.active == vc.active && 
			this.simpleOperator.Equals(vc.simpleOperator) && 
			this.formulaChooser.Equals(vc.formulaChooser) && 
			this.value == vc.value && 
			this.status == vc.status && 
			this.formula == vc.formula && 
			this.randomMin == vc.randomMin && 
			this.randomMax == vc.randomMax && 
			this.efficiency == vc.efficiency && 
			this.cancelSkills == vc.cancelSkills && 
			this.blockable == vc.blockable && 
			this.ignoreDefend == vc.ignoreDefend && 
			this.ignoreElement == vc.ignoreElement && 
			this.ignoreRace == vc.ignoreRace && 
			this.ignoreSize == vc.ignoreSize)
		{
			same = true;
		}
		return same;
	}
	
	/*
	============================================================================
	Ingame functions
	============================================================================
	*/
	public int GetChange(Combatant user, Combatant target, float damageFactor, float damageMultiplier)
	{
		float change = 0;
		if(FormulaChooser.VALUE.Equals(this.formulaChooser))
		{
			change = value;
		}
		else if(FormulaChooser.STATUS.Equals(this.formulaChooser))
		{
			change = target.status[this.status].GetValue();
		}
		else if(FormulaChooser.FORMULA.Equals(this.formulaChooser))
		{
			change = DataHolder.Formulas().formula[this.formula].Calculate(user, target);
		}
		else if(FormulaChooser.RANDOM.Equals(this.formulaChooser))
		{
			change = Random.Range(this.randomMin, this.randomMax);
		}
		return (int)(change*this.efficiency*damageFactor*damageMultiplier);
	}
	
	private int GetDefendedChange(int change, int elementID, Combatant user, Combatant target)
	{
		// race damage
		if(!this.ignoreRace)
		{
			change *= user.GetRaceDamageFactor(target.raceID);
			change /= 100;
		}
		// size damage
		if(!this.ignoreSize)
		{
			change *= user.GetSizeDamageFactor(target.sizeID);
			change /= 100;
		}
		// element defence
		if(!this.ignoreRace && elementID >= 0)
		{
			change *= target.GetElementDefence(elementID);
			change /= 100;
		}
		// defend command
		if(!this.ignoreDefend && target.isDefending)
		{
			change = (int)(change * DataHolder.Formula(DataHolder.BattleSystem().defendFormula).Calculate(user, target));
			change /= 100;
		}
		return change;
	}
	
	public int ChangeValue(int statusID, int elementID, Combatant user, Combatant target, 
			bool showNumber, float damageFactor, float damageMultiplier)
	{
		int change = 0;
		if(this.active)
		{
			if(this.blockable && DataHolder.GameSettings().GetRandom() <= 
				(DataHolder.Formulas().formula[target.baseBlock].Calculate(user, target) + target.GetBlockBonus()))
			{
				DataHolder.BattleSystemData().blockTextSettings.ShowText("", target);
			}
			else
			{
				change = this.GetChange(user, target, damageFactor, damageMultiplier);
				int oldValue = target.status[statusID].GetValue();
				
				// set the new value to the target status
				if(this.IsAdd())
				{
					if(target.status[statusID].IsNormal())
					{
						target.status[statusID].AddBaseValue(change);
						target.status[statusID].AddValue(change, false, false, showNumber);
					}
					else
					{
						if(change < 0)
						{
							change = this.GetDefendedChange(change, elementID, user, target);
						}
						target.status[statusID].AddValue(change, false, false, showNumber);
					}
				}
				else if(this.IsSub())
				{
					if(target.status[statusID].IsNormal())
					{
						target.status[statusID].AddBaseValue(-change);
						target.status[statusID].AddValue(-change, false, false, showNumber);
					}
					else
					{
						if(change > 0)
						{
							change = this.GetDefendedChange(change, elementID, user, target);
						}
						target.status[statusID].AddValue(-change, false, false, showNumber);
					}
				}
				else if(this.IsSet())
				{
					if(target.status[statusID].IsNormal())
					{
						target.status[statusID].SetBaseValue(change);
						target.status[statusID].SetValue(change, false, false, showNumber);
					}
					else
					{
						target.status[statusID].SetValue(change, false, false, showNumber);
					}
				}
				for(int j=0; j<target.status.Length; j++) target.status[j].CheckBounds();
				// set aggressive if attacked
				if(target.status[statusID].IsConsumable() && 
					target.status[statusID].GetValue() < oldValue)
				{
					target.CheckAggressive(AggressiveType.DAMAGE);
				}
				// cancel skills
				if(this.cancelSkills)
				{
					target.CancelSkillCast();
				}
			}
		}
		return change;
	}
	
	public bool IsAdd()
	{
		return SimpleOperator.ADD.Equals(this.simpleOperator);
	}
	
	public bool IsSub()
	{
		return SimpleOperator.SUB.Equals(this.simpleOperator);
	}
	
	public bool IsSet()
	{
		return SimpleOperator.SET.Equals(this.simpleOperator);
	}
}