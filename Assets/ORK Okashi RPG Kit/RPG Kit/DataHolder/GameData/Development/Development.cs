
using UnityEngine;
using System.Collections;

public class Development
{
	public int startLevel = 1;
	public int maxLevel = 10;
	
	public StatusDevelopment[] statusValue;
	
	public SkillLearn[] skill = new SkillLearn[0];
	
	// XML
	private static string STATUSVALUE = "statusvalue";
	private static string SKILL = "skill";
	
	public Development()
	{
		
	}
	
	public void Init(int statCount)
	{
		this.statusValue = new StatusDevelopment[statCount];
		for(int i=0; i<this.statusValue.Length; i++)
		{
			this.statusValue[i] = new StatusDevelopment(this.maxLevel);
		}
	}
	
	public void BaseInit(StatusValue[] value)
	{
		this.Init(value.Length);
		
		for(int i=0; i<value.Length; i++)
		{
			float increase = value[i].maxValue - value[i].minValue;
			increase /= this.maxLevel;
			this.statusValue[i].levelValue[0] = value[i].minValue;
			this.statusValue[i].levelValue[this.maxLevel-1] = value[i].maxValue;
			for(int j=2; j<this.maxLevel; j++)
			{
				this.statusValue[i].levelValue[j-1] = 
						(int)(this.statusValue[i].levelValue[0] + increase*j);
			}
		}
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht, bool saveStats)
	{
		ArrayList s = new ArrayList();
		if(saveStats)
		{
			ht.Add("startlevel", this.startLevel.ToString());
			ht.Add("maxlevel", this.maxLevel.ToString());
			
			for(int j=0; j<this.statusValue.Length; j++)
			{
				if(this.statusValue[j].Count() > 0)
				{
					Hashtable val = HashtableHelper.GetTitleHashtable(Development.STATUSVALUE, j);
					for(int k=0; k<this.maxLevel; k++)
					{
						val.Add(k.ToString(), this.statusValue[j].levelValue[k].ToString());
					}
					s.Add(val);
				}
			}
		}
		
		if(this.skill.Length > 0)
		{
			ht.Add("skills", this.skill.Length.ToString());
			for(int j=0; j<this.skill.Length; j++)
			{
				s.Add(this.skill[j].GetData(
						HashtableHelper.GetTitleHashtable(Development.SKILL, j), false));
			}
		}
		
		if(s.Count > 0) ht.Add(XMLHandler.NODES, s);
		
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		if(ht.ContainsKey("startlevel"))
		{
			this.startLevel = int.Parse((string)ht["startlevel"]);
			this.maxLevel = int.Parse((string)ht["maxlevel"]);
			
			this.Init(DataHolder.StatusValueCount);
		}
		if(ht.ContainsKey("skills"))
		{
			this.skill = new SkillLearn[int.Parse((string)ht["skills"])];
			for(int j=0; j<this.skill.Length; j++)
			{
				this.skill[j] = new SkillLearn();
			}
		}
		
		ArrayList s = ht[XMLHandler.NODES] as ArrayList;
		foreach(Hashtable ht2 in s)
		{
			if(ht2[XMLHandler.NODE_NAME] as string == Development.STATUSVALUE)
			{
				int id = int.Parse((string)ht2["id"]);
				if(id < this.statusValue.Length)
				{
					for(int j=0; j<this.maxLevel; j++)
					{
						this.statusValue[id].levelValue[j] = int.Parse((string)ht2[j.ToString()]);
					}
				}
			}
			else if(ht2[XMLHandler.NODE_NAME] as string == Development.SKILL)
			{
				int id = int.Parse((string)ht2["id"]);
				if(id < this.skill.Length)
				{
					this.skill[id].SetData(ht2, false);
				}
			}
		}
	}
	
	public Development GetCopy()
	{
		Development d = new Development();
		d.Init(this.statusValue.Length);
		d.SetData(this.GetData(new Hashtable(), true));
		return d;
	}
	
	/*
	============================================================================
	Editor functions
	============================================================================
	*/
	public void MaxLevelChanged()
	{
		if(this.statusValue != null)
		{
			for(int i=0; i< this.statusValue.Length; i++)
			{
				int difference = this.statusValue[i].Count() - this.maxLevel;
				// add values
				if(difference < 0)
				{
					difference *= -1;
					int last = this.statusValue[i].levelValue[this.statusValue[i].Count()-1];
					for(int j=0; j<difference; j++)
					{
						this.statusValue[i].Add(last);
					}
				}
				// remove values
				else
				{
					for(int j=difference; j>0; j--)
					{
						this.statusValue[i].Remove(this.statusValue[i].Count()-1);
					}
				}
			}
		}
	}
	
	public void AddStatusValue(int index, StatusValue val)
	{
		if(this.statusValue != null)
		{
			StatusDevelopment sd = new StatusDevelopment(this.maxLevel);
			float increase = val.maxValue - val.minValue;
			increase /= this.maxLevel;
			sd.levelValue[0] = val.minValue;
			sd.levelValue[sd.Count()-1] = val.maxValue;
			for(int j=2; j<this.maxLevel; j++)
			{
				sd.levelValue[j-1] = (int)(sd.levelValue[0] + increase*j);
			}
			this.statusValue = ArrayHelper.Add(sd, this.statusValue);
		}
	}
	
	public void SetStatusValueType(int index, StatusValueType type, StatusValue val)
	{
		if(this.statusValue != null)
		{
			if(StatusValueType.CONSUMABLE.Equals(type))
			{
				this.statusValue[index] = new StatusDevelopment();
			}
			else
			{
				this.statusValue[index] = new StatusDevelopment(this.maxLevel);
				float increase = val.maxValue - val.minValue;
				increase /= this.maxLevel;
				this.statusValue[index].levelValue[0] = val.minValue;
				this.statusValue[index].levelValue[this.maxLevel-1] = val.maxValue;
				for(int j=2; j<this.maxLevel; j++)
				{
					this.statusValue[index].levelValue[j-1] = (int)(
							this.statusValue[index].levelValue[0] + increase*j);
				}
			}
		}
	}
	
	public void RemoveStatusValue(int index)
	{
		if(this.statusValue != null)
		{
			this.statusValue = ArrayHelper.Remove(index, this.statusValue);
		}
	}
	
	public void StatusValueMinMaxChanged(int index, int min, int max)
	{
		if(this.statusValue != null)
		{
			for(int k=0; k<this.statusValue[index].Count(); k++)
			{
				if(this.statusValue[index].levelValue[k] < min)
				{
					this.statusValue[index].levelValue[k] = min;
				}
				else if(this.statusValue[index].levelValue[k] > max)
				{
					this.statusValue[index].levelValue[k] = max;
				}
			}
		}
	}
	
	/*
	============================================================================
	Ingame functions
	============================================================================
	*/
	public int GetValueAtLevel(int vID, int lvl)
	{
		return this.statusValue[vID].levelValue[lvl-1];
	}
	
	public int GetValueDifference(int vID, int lvl, int lvl2)
	{
		return Mathf.Abs(this.GetValueAtLevel(vID, lvl) - this.GetValueAtLevel(vID, lvl2));
	}
	
	/*
	============================================================================
	Status functions
	============================================================================
	*/
	public string IncreaseStatus(Character character, int lvl)
	{
		string statusText = "";
		for(int i=0; i<character.status.Length; i++)
		{
			if(character.status[i].IsNormal())
			{
				int val = this.GetValueDifference(i, lvl, lvl-1);
				if(val != 0)
				{
					string tmp = DataHolder.BattleEnd().GetStatusValueText(i, val);
					if("" != tmp) statusText += tmp+"\n";
					character.status[i].AddBaseValue(val);
					character.status[i].AddValue(val, true, false, true);
				}
			}
		}
		return statusText;
	}
	
	/*
	============================================================================
	Skill functions
	============================================================================
	*/
	public void InitSkills(Character character, int lvl)
	{
		for(int i=0; i<this.skill.Length; i++)
		{
			if(this.skill[i].atLevel <= lvl)
			{
				character.LearnSkill(this.skill[i].skillID, this.skill[i].skillLevel);
			}
		}
	}
	
	public void ForgetSkills(Character character, int lvl)
	{
		for(int i=0; i<this.skill.Length; i++)
		{
			if(this.skill[i].atLevel <= lvl)
			{
				character.ForgetSkill(this.skill[i].skillID);
			}
		}
	}
	
	public string LearnSkills(Character character, int lvl)
	{
		string skillText = "";
		for(int i=0; i<this.skill.Length; i++)
		{
			if(this.skill[i].atLevel == lvl)
			{
				if(character.LearnSkill(this.skill[i].skillID, this.skill[i].skillLevel))
				{
					string tmp = DataHolder.BattleEnd().GetSkillText(this.skill[i].skillID, this.skill[i].skillLevel);
					if("" != tmp) skillText += tmp+"\n";
				}
			}
		}
		return skillText;
	}
}
