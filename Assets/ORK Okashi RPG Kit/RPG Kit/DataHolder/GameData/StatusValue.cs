
public class StatusValue
{
	// settings
	public int minValue = 0;
	public int maxValue  = 99999;
	public StatusValueType type = StatusValueType.NORMAL;
	
	// type consumable
	public int maxStatus = 0;
	public bool killChar = false;
	
	// type experience
	public bool levelUp = false;
	public bool levelUpClass = false;
	
	// battle text display
	public BattleTextSettings addText = new BattleTextSettings();
	public BattleTextSettings subText = new BattleTextSettings();
	
	// ingame
	private int baseValue = 0;
	private int currentValue = 0;
	public int[] lastValueHUD;
	
	public int realID = -1;
	private Combatant owner = null;
	private int lastCurrentValue = 0;
	
	public StatusValue()
	{
		
	}
	
	/*
	============================================================================
	Init functions
	============================================================================
	*/
	public void InitValue(int val)
	{
		this.baseValue = val;
		this.currentValue = val;
	}
	
	public void SetOwner(Combatant c)
	{
		this.owner = c;
	}
	
	/*
	============================================================================
	Get functions
	============================================================================
	*/
	public int GetValue()
	{
		return this.currentValue;
	}
	
	public int GetBaseValue()
	{
		return this.baseValue;
	}
	
	/*
	============================================================================
	Change functions
	============================================================================
	*/
	public void SetValue(int val, bool checkDeath, bool checkLevelUp, bool showText)
	{
		if(this.owner.CanChangeStatusValue(this.realID))
		{
			if(showText)
			{
				int add = val - this.currentValue;
				if(add < 0 && this.subText.active)
				{
					if(this.subText.IsShowNumber()) this.subText.ShowNumber(-add, this.owner);
					else this.subText.ShowText((-add).ToString(), this.owner);
				}
				else if(add >= 0 && this.addText.active)
				{
					if(this.addText.IsShowNumber()) this.addText.ShowNumber(add, this.owner);
					else this.addText.ShowText(add.ToString(), this.owner);
				}
			}
			this.currentValue = val;
			this.CheckBounds(checkDeath, checkLevelUp);
		}
		else
		{
			DataHolder.BattleSystemData().blockTextSettings.ShowText("", this.owner);
		}
	}
	
	public string AddValue(int add, bool checkDeath, bool checkLevelUp, bool showText)
	{
		if(this.owner.CanChangeStatusValue(this.realID))
		{
			if(showText)
			{
				if(add < 0 && this.subText.active)
				{
					if(this.subText.IsShowNumber()) this.subText.ShowNumber(-add, this.owner);
					else this.subText.ShowText((-add).ToString(), this.owner);
				}
				else if(add >= 0 && this.addText.active)
				{
					if(this.addText.IsShowNumber()) this.addText.ShowNumber(add, this.owner);
					else this.addText.ShowText(add.ToString(), this.owner);
				}
			}
			this.currentValue += add;
			return this.CheckBounds(checkDeath, checkLevelUp);
		}
		else
		{
			DataHolder.BattleSystemData().blockTextSettings.ShowText("", this.owner);
			return "";
		}
	}
	
	public void ResetValue()
	{
		this.currentValue = this.baseValue;
	}
	
	public void SetBaseValue(int val)
	{
		this.baseValue = val;
	}
	
	public void AddBaseValue(int add)
	{
		this.baseValue += add;
	}
	
	/*
	============================================================================
	Check functions
	============================================================================
	*/
	public string CheckBounds()
	{
		return this.CheckBounds(true, true);
	}
	
	public string CheckBounds(bool checkDeath, bool checkLevelUp)
	{
		string txt = "";
		if(this.currentValue < this.minValue) this.currentValue = this.minValue;
		else if(this.currentValue > this.maxValue) this.currentValue = this.maxValue;
		if(this.baseValue < this.minValue) this.baseValue = this.minValue;
		else if(this.baseValue > this.maxValue) this.baseValue = this.maxValue;
		
		if(this.IsConsumable())
		{
			if(this.currentValue > this.owner.status[this.maxStatus].currentValue)
			{
				this.currentValue = this.owner.status[this.maxStatus].currentValue;
			}
			if(this.killChar && checkDeath) this.owner.CheckDeath();
		}
		else if(this.IsExperience() && this.levelUp && this.owner is Character && checkLevelUp)
		{
			txt = ((Character)this.owner).CheckLevelUp();
		}
		if(this.currentValue != this.lastCurrentValue)
		{
			this.lastCurrentValue = this.currentValue;
			this.owner.StatusChanged(this.realID);
		}
		return txt;
	}
	
	public bool MaxReached()
	{
		bool max = false;
		if(this.IsConsumable() && 
			this.currentValue >= this.owner.status[this.maxStatus].currentValue)
		{
			max = true;
		}
		else if(this.IsNormal() &&
			this.currentValue >= this.maxValue)
		{
			max = true;
		}
		return max;
	}
	
	public bool IsNormal()
	{
		return StatusValueType.NORMAL.Equals(type);
	}
	
	public bool IsConsumable()
	{
		return StatusValueType.CONSUMABLE.Equals(type);
	}
	
	public bool IsExperience()
	{
		return StatusValueType.EXPERIENCE.Equals(type);
	}
	
	public bool CompareTo(int checkValue, ValueCheck comparison, ValueSetter setter, Combatant c)
	{
		bool check = false;
		
		int value = this.currentValue;
		if(ValueSetter.PERCENT.Equals(setter))
		{
			float v = value;
			float mv = this.maxValue;
			if(this.IsConsumable())
			{
				mv = c.status[this.maxStatus].currentValue;
			}
			v /= (mv/100.0f);
			value = (int)v;
		}
		
		if((ValueCheck.EQUALS.Equals(comparison) && value == checkValue) ||
			(ValueCheck.LESS.Equals(comparison) && value < checkValue) ||
			(ValueCheck.GREATER.Equals(comparison) && value > checkValue))
		{
			check = true;
		}
		
		return check;
	}
}