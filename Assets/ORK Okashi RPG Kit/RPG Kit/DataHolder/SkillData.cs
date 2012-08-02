
using System.Collections;

public class SkillData : BaseLangData
{
	public Skill[] skill = new Skill[0];
	
	// XML data
	private string filename = "skills";
	
	public static string AUDIO_PATH = "Audio/Skills/";
	
	public static string SKILLS = "skills";
	public static string SKILL = "skill";
	public static string USERCONSUME = "userconsume";
	public static string TARGETCONSUME = "targetconsume";
	public static string EFFECT = "effect";
	public static string SKILLCOMBO = "skillcombo";
	public static string BONUS = "bonus";
	public static string ELEMENT = "element";
	public static string LEVEL = "level";
	public static string TARGETRAYCAST = "targetraycast";
	public static string BONUSSETTINGS = "bonussettings";
	public static string AUDIOCLIP = "audioclip";
	public static string RACE = "race";
	public static string STEALCHANCE = "stealchance";
	public static string SIZE = "size";

	public SkillData()
	{
		this.filter = new DataFilter(1);
		LoadData();
	}
	
	public override string GetIconPath() { return "Icons/Skill/"; }
	
	public void LoadData()
	{
		ArrayList data = XMLHandler.LoadXML(dir+filename);
		
		if(data.Count > 0)
		{
			foreach(Hashtable entry in data)
			{
				if(entry[XMLHandler.NODE_NAME] as string == SkillData.SKILLS)
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList subs = entry[XMLHandler.NODES] as ArrayList;
						icon = new string[subs.Count];
						skill = new Skill[subs.Count];
						
						foreach(Hashtable val in subs)
						{
							if(val[XMLHandler.NODE_NAME] as string == SkillData.SKILL)
							{
								int i = int.Parse((string)val["id"]);
								icon[i] = "";
								
								skill[i] = new Skill();
								skill[i].realID = i;
								
								skill[i].skilltype = int.Parse((string)val["skilltype"]);
								skill[i].targetType = (TargetType)System.Enum.Parse(typeof(TargetType), (string)val["targettype"]);
								skill[i].skillTarget = (SkillTarget)System.Enum.Parse(typeof(SkillTarget), (string)val["skilltarget"]);
								if(val.ContainsKey("useinbattle")) skill[i].useInBattle = true;
								if(val.ContainsKey("useinfield")) skill[i].useInField = true;
								if(val.ContainsKey("passive"))
								{
									skill[i].isPassive = true;
									skill[i].useInBattle = false;
									skill[i].useInField = false;
								}
								
								if(val.ContainsKey("consumes")) skill[i].level[0].SetData(val);
								
								if(val.ContainsKey("levels"))
								{
									skill[i].level = new SkillLevel[int.Parse((string)val["levels"])];
								}
								
								ArrayList s = val[XMLHandler.NODES] as ArrayList;
								foreach(Hashtable ht in s)
								{
									this.LoadLanguages(ht, i, subs.Count);
									if(ht[XMLHandler.NODE_NAME] as string == SkillData.LEVEL)
									{
										int j = int.Parse((string)ht["id"]);
										if(j < skill[i].level.Length)
										{
											skill[i].level[j] = new SkillLevel();
											skill[i].level[j].SetData(ht);
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == SkillData.TARGETRAYCAST)
									{
										skill[i].targetRaycast.SetData(ht);
									}
								}
							}
						}
					}
				}
			}
		}
	}
	
	public void SaveData()
	{
		if(name.Length == 0) return;
		ArrayList data = new ArrayList();
		ArrayList subs = new ArrayList();
		Hashtable sv = new Hashtable();
		
		sv.Add(XMLHandler.NODE_NAME, SkillData.SKILLS);
		
		for(int i=0; i<skill.Length; i++)
		{
			Hashtable ht = HashtableHelper.GetTitleHashtable(SkillData.SKILL, i);
			ArrayList s = new ArrayList();
			s = this.SaveLanguages(s, i);
			
			ht.Add("skilltype", skill[i].skilltype.ToString());
			ht.Add("targettype", skill[i].targetType.ToString());
			ht.Add("skilltarget", skill[i].skillTarget.ToString());
			if(skill[i].useInBattle) ht.Add("useinbattle", "true");
			if(skill[i].useInField) ht.Add("useinfield", "true");
			if(skill[i].isPassive) ht.Add("passive", "true");
			
			ht.Add("levels", skill[i].level.Length.ToString());
			for(int j=0; j<skill[i].level.Length; j++)
			{
				s.Add(skill[i].level[j].GetData(HashtableHelper.GetTitleHashtable(SkillData.LEVEL, j), skill[i]));
			}
			s.Add(skill[i].targetRaycast.GetData(HashtableHelper.GetTitleHashtable(SkillData.TARGETRAYCAST)));
			
			ht.Add(XMLHandler.NODES, s);
			subs.Add(ht);
		}
		sv.Add(XMLHandler.NODES, subs);
		data.Add(sv);
		XMLHandler.SaveXML(dir, filename, data);
	}
	
	public void AddSkill(string n, string d, int count)
	{
		base.AddBaseData(n, d, count);
		skill = ArrayHelper.Add(new Skill(), skill);
	}
	
	public override void RemoveData(int index)
	{
		base.RemoveData(index);
		skill = ArrayHelper.Remove(index, skill);
		
		for(int i=0; i<skill.Length; i++)
		{
			for(int j=0; j<skill[i].level.Length; j++)
			{
				for(int k=0; k<skill[i].level[j].skillCombo.Length; k++)
				{
					if(skill[i].level[j].skillCombo[k] == index) skill[i].level[j].skillCombo[k] = 0;
				}
			}
		}
	}
	
	public Skill GetCopy(int index)
	{
		Skill s = new Skill();
		s.skilltype = skill[index].skilltype;
		s.targetType = skill[index].targetType;
		s.skillTarget = skill[index].skillTarget;
		s.useInBattle = skill[index].useInBattle;
		s.useInField = skill[index].useInField;
		s.isPassive = skill[index].isPassive;
		
		s.level = new SkillLevel[skill[index].level.Length];
		for(int i=0; i<skill[index].level.Length; i++)
		{
			s.level[i] = skill[index].level[i].GetCopy();
		}
		
		s.targetRaycast.SetData(skill[index].targetRaycast.GetData(new Hashtable()));
		
		return s;
	}
	
	public override void Copy(int index)
	{
		base.Copy(index);
		skill = ArrayHelper.Add(this.GetCopy(index), skill);
	}
	
	public void RemoveSkillType(int index)
	{
		for(int i=0; i<skill.Length; i++)
		{
			if(skill[i].skilltype == index)
			{
				skill[i].skilltype = 0;
			}
			else if(skill[i].skilltype > index)
			{
				skill[i].skilltype -= 1;
			}
		}
	}
	
	public void AddStatusValue(int index)
	{
		for(int i=0; i<skill.Length; i++)
		{
			for(int j=0; j<skill[i].level.Length; j++)
			{
				skill[i].level[j].userConsume = ArrayHelper.Add(new ValueChange(), skill[i].level[j].userConsume);
				skill[i].level[j].targetConsume = ArrayHelper.Add(new ValueChange(), skill[i].level[j].targetConsume);
				skill[i].level[j].bonus.AddStatusValue();
			}
		}
	}
	
	public void RemoveStatusValue(int index)
	{
		for(int i=0; i<skill.Length; i++)
		{
			for(int j=0; j<skill[i].level.Length; j++)
			{
				skill[i].level[j].userConsume = ArrayHelper.Remove(index, skill[i].level[j].userConsume);
				skill[i].level[j].targetConsume = ArrayHelper.Remove(index, skill[i].level[j].targetConsume);
				skill[i].level[j].bonus.RemoveStatusValue(index);
				
				for(int k=index; k<skill[i].level[j].userConsume.Length-1; k++)
				{
					if(skill[i].level[j].userConsume[k].status == index)
					{
						skill[i].level[j].userConsume[k].status = 0;
					}
					else if(skill[i].level[j].userConsume[k].status > index)
					{
						skill[i].level[j].userConsume[k].status -= 1;
					}
					if(skill[i].level[j].targetConsume[k].status == index)
					{
						skill[i].level[j].targetConsume[k].status = 0;
					}
					else if(skill[i].level[j].targetConsume[k].status > index)
					{
						skill[i].level[j].targetConsume[k].status -= 1;
					}
				}
			}
		}
	}
	
	public void SetStatusValueType(int index, StatusValueType val)
	{
		if(!StatusValueType.CONSUMABLE.Equals(val))
		{
			for(int i=0; i<skill.Length; i++)
			{
				for(int j=0; j<skill[i].level.Length; j++)
				{
					skill[i].level[j].userConsume[index] = new ValueChange();
					skill[i].level[j].targetConsume[index] = new ValueChange();
				}
			}
		}
		if(StatusValueType.CONSUMABLE.Equals(val) || StatusValueType.EXPERIENCE.Equals(val))
		{
			for(int i=0; i<skill.Length; i++)
			{
				for(int j=0; j<skill[i].level.Length; j++)
				{
					skill[i].level[j].bonus.SetStatusValueType(index, val);
				}
			}
		}
	}
	
	public void AddStatusEffect(int index)
	{
		for(int i=0; i<skill.Length; i++)
		{
			for(int j=0; j<skill[i].level.Length; j++)
			{
				skill[i].level[j].skillEffect = ArrayHelper.Add(SkillEffect.NONE, skill[i].level[j].skillEffect);
			}
		}
	}
	
	public void RemoveStatusEffect(int index)
	{
		for(int i=0; i<skill.Length; i++)
		{
			for(int j=0; j<skill[i].level.Length; j++)
			{
				skill[i].level[j].skillEffect = ArrayHelper.Remove(index, skill[i].level[j].skillEffect);
			}
		}
	}
	
	public void AddElement(int index)
	{
		for(int i=0; i<skill.Length; i++)
		{
			for(int j=0; j<skill[i].level.Length; j++)
			{
				skill[i].level[j].bonus.AddElement();
				skill[i].level[j].elementValue = ArrayHelper.Add(0, skill[i].level[j].elementValue);
				skill[i].level[j].elementOperator = ArrayHelper.Add(SimpleOperator.ADD, skill[i].level[j].elementOperator);
			}
		}
	}
	
	public void RemoveElement(int index)
	{
		for(int i=0; i<skill.Length; i++)
		{
			for(int j=0; j<skill[i].level.Length; j++)
			{
				skill[i].level[j].bonus.RemoveElement(index);
				skill[i].level[j].elementValue = ArrayHelper.Remove(index, skill[i].level[j].elementValue);
				skill[i].level[j].elementOperator = ArrayHelper.Remove(index, skill[i].level[j].elementOperator);
				if(skill[i].level[j].skillElement == index)
				{
					skill[i].level[j].skillElement = 0;
				}
				else if(skill[i].level[j].skillElement > index)
				{
					skill[i].level[j].skillElement -= 1;
				}
			}
		}
	}
	
	public void RemoveFormula(int index)
	{
		for(int i=0; i<skill.Length; i++)
		{
			for(int j=0; j<skill[i].level.Length; j++)
			{
				for(int k=0; k<skill[i].level[j].userConsume.Length; k++)
				{
					skill[i].level[j].userConsume[k].formula = this.CheckForIndex(index, skill[i].level[j].userConsume[k].formula);
					skill[i].level[j].targetConsume[k].formula = this.CheckForIndex(index, skill[i].level[j].targetConsume[k].formula);
					skill[i].level[j].hitFormula = this.CheckForIndex(index, skill[i].level[j].hitFormula);
				}
			}
		}
	}
	
	public void AddRace()
	{
		for(int i=0; i<skill.Length; i++)
		{
			for(int j=0; j<skill[i].level.Length; j++)
			{
				skill[i].level[j].bonus.AddRace();
				skill[i].level[j].raceValue = ArrayHelper.Add(0, skill[i].level[j].raceValue);
			}
		}
	}
	
	public void RemoveRace(int index)
	{
		for(int i=0; i<skill.Length; i++)
		{
			for(int j=0; j<skill[i].level.Length; j++)
			{
				skill[i].level[j].bonus.RemoveRace(index);
				skill[i].level[j].raceValue = ArrayHelper.Remove(index, skill[i].level[j].raceValue);
			}
		}
	}
	
	public void AddSize()
	{
		for(int i=0; i<skill.Length; i++)
		{
			for(int j=0; j<skill[i].level.Length; j++)
			{
				skill[i].level[j].bonus.AddSize();
				skill[i].level[j].sizeValue = ArrayHelper.Add(0, skill[i].level[j].sizeValue);
			}
		}
	}
	
	public void RemoveSize(int index)
	{
		for(int i=0; i<skill.Length; i++)
		{
			for(int j=0; j<skill[i].level.Length; j++)
			{
				skill[i].level[j].bonus.RemoveSize(index);
				skill[i].level[j].sizeValue = ArrayHelper.Remove(index, skill[i].level[j].sizeValue);
			}
		}
	}
	
	public void RemoveItem(int index)
	{
		for(int i=0; i<skill.Length; i++)
		{
			for(int j=0; j<skill[i].level.Length; j++)
			{
				if(ItemDropType.ITEM.Equals(skill[i].level[j].stealChance.itemType))
				{
					skill[i].level[j].stealChance.itemID = this.CheckForIndex(index, skill[i].level[j].stealChance.itemID);
				}
			}
		}
	}
	
	public void RemoveWeapon(int index)
	{
		for(int i=0; i<skill.Length; i++)
		{
			for(int j=0; j<skill[i].level.Length; j++)
			{
				if(ItemDropType.WEAPON.Equals(skill[i].level[j].stealChance.itemType))
				{
					skill[i].level[j].stealChance.itemID = this.CheckForIndex(index, skill[i].level[j].stealChance.itemID);
				}
			}
		}
	}
	
	public void RemoveArmor(int index)
	{
		for(int i=0; i<skill.Length; i++)
		{
			for(int j=0; j<skill[i].level.Length; j++)
			{
				if(ItemDropType.ARMOR.Equals(skill[i].level[j].stealChance.itemType))
				{
					skill[i].level[j].stealChance.itemID = this.CheckForIndex(index, skill[i].level[j].stealChance.itemID);
				}
			}
		}
	}
	
	public void RemoveDifficulty(int index)
	{
		for(int i=0; i<skill.Length; i++)
		{
			for(int j=0; j<skill[i].level.Length; j++)
			{
				skill[i].level[j].bonus.RemoveDifficulty(index);
			}
		}
	}
	
	// filter override
	public override void CreateFilterList(bool showIDs)
	{
		ArrayList names = new ArrayList();
		ArrayList ids = new ArrayList();
		if(name != null)
		{
			for(int i=0; i<name[0].Count(); i++)
			{
				if(this.skill[i].skilltype == this.filter.filterID[0])
				{
					if(showIDs)
					{
						names.Add(i.ToString() + ": " + name[0].text[i]);
					}
					else
					{
						names.Add(name[0].text[i]);
					}
					ids.Add(i);
				}
			}
		}
		this.filter.nameList = names.ToArray(typeof(string)) as string[];
		this.filter.realID = ids.ToArray(typeof(int)) as int[];
	}
}