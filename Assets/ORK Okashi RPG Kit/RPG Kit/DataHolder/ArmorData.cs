
using System.Collections;

public class ArmorData : BaseLangData
{
	public Armor[] armor = new Armor[0];
	
	// XML data
	private string filename = "armors";
	
	private static string ARMORS = "armors";
	private static string ARMOR = "armor";
	private static string EQUIPPART = "equippart";
	private static string BLOCKPART = "blockpart";
	private static string EFFECT = "effect";
	private static string BONUS = "bonus";
	private static string SKILL = "skill";
	private static string STATUS = "status";
	private static string ELEMENT = "element";
	private static string PREFAB = "prefab";
	private static string BONUSSETTINGS = "bonussettings";
	private static string RACE = "race";
	private static string SIZE = "size";

	public ArmorData()
	{
		this.filter = new DataFilter(1);
		LoadData();
	}
	
	public override string GetIconPath() { return "Icons/Armor/"; }
	
	public void LoadData()
	{
		ArrayList data = XMLHandler.LoadXML(dir+filename);
		
		if(data.Count > 0)
		{
			foreach(Hashtable entry in data)
			{
				if(entry[XMLHandler.NODE_NAME] as string == ArmorData.ARMORS)
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList subs = entry[XMLHandler.NODES] as ArrayList;
						icon = new string[subs.Count];
						armor = new Armor[subs.Count];
						
						foreach(Hashtable val in subs)
						{
							if(val[XMLHandler.NODE_NAME] as string == ArmorData.ARMOR)
							{
								int i = int.Parse((string)val["id"]);
								icon[i] = "";
								
								armor[i] = new Armor();
								if(val.ContainsKey("minimumlevel"))
								{
									armor[i].minimumLevel = int.Parse((string)val["minimumlevel"]);
								}
								if(val.ContainsKey("minimumclasslevel"))
								{
									armor[i].minimumClassLevel = int.Parse((string)val["minimumclasslevel"]);
								}
								if(val.ContainsKey("dropable")) armor[i].dropable = true;
								armor[i].equipPart = new bool[int.Parse((string)val["equipparts"])];
								armor[i].blockPart = new bool[int.Parse((string)val["equipparts"])];
								armor[i].equipType = (EquipType)System.Enum.Parse(typeof(EquipType), (string)val["equiptype"]);
								
								armor[i].skillEffect = new SkillEffect[int.Parse((string)val["effects"])];
								if(val.ContainsKey("skills"))
								{
									armor[i].equipmentSkill = new EquipmentSkill[int.Parse((string)val["skills"])];
								}
								
								armor[i].buyPrice = int.Parse((string)val["buyprice"]);
								if(val.ContainsKey("sellprice"))
								{
									armor[i].sellable = true;
									armor[i].sellPrice = int.Parse((string)val["sellprice"]);
									armor[i].sellSetter = (ValueSetter)System.Enum.Parse(typeof(ValueSetter), (string)val["sellsetter"]);
								}
								if(val.ContainsKey("counter"))
								{
									armor[i].bonus.counterBonus = int.Parse((string)val["counter"]);
								}
								if(val.ContainsKey("escape"))
								{
									armor[i].bonus.escapeBonus = int.Parse((string)val["escape"]);
								}
								if(val.ContainsKey("hitbonus"))
								{
									armor[i].bonus.hitBonus = int.Parse((string)val["hitbonus"]);
								}
								if(val.ContainsKey("movespeedreduction"))
								{
									armor[i].bonus.speedBonus = -float.Parse((string)val["movespeedreduction"]);
								}
								
								int elements = int.Parse((string)val["elements"]);
								armor[i].elementValue = new int[elements];
								armor[i].elementOperator = new SimpleOperator[elements];
								
								int count;
								if(val.ContainsKey("races"))
								{
									count = int.Parse((string)val["races"]);
								}
								else count = DataHolder.Races().GetDataCount();
								armor[i].raceValue = new int[count];
								
								if(val.ContainsKey("sizes"))
								{
									count = int.Parse((string)val["sizes"]);
								}
								else count = DataHolder.Sizes().GetDataCount();
								armor[i].sizeValue = new int[count];
								
								ArrayList s  = val[XMLHandler.NODES] as ArrayList;
								foreach(Hashtable ht in s)
								{
									this.LoadLanguages(ht, i, subs.Count);
									if(ht[XMLHandler.NODE_NAME] as string == ArmorData.EQUIPPART)
									{
										armor[i].equipPart[int.Parse((string)ht["id"])] = bool.Parse((string)ht["enabled"]);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == ArmorData.BLOCKPART)
									{
										armor[i].blockPart[int.Parse((string)ht["id"])] = bool.Parse((string)ht["blocked"]);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == ArmorData.BONUS)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < armor[i].bonus.statusBonus.Length)
										{
											armor[i].bonus.statusBonus[id] = int.Parse((string)ht["value"]);
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == ArmorData.EFFECT)
									{
										armor[i].skillEffect[int.Parse((string)ht["id"])] = 
												(SkillEffect)System.Enum.Parse(typeof(SkillEffect), (string)ht["value"]);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == ArmorData.PREFAB)
									{
										armor[i].prefabName = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == ArmorData.SKILL)
									{
										int j = int.Parse((string)ht["id"]);
										armor[i].equipmentSkill[j] = new EquipmentSkill(DataHolder.StatusValueCount);
										armor[i].equipmentSkill[j].skill = int.Parse((string)ht["skill"]);
										if(ht.ContainsKey("slvl"))
										{
											armor[i].equipmentSkill[j].skillLevel = int.Parse((string)ht["slvl"]);
										}
										if(ht.ContainsKey("level"))
										{
											armor[i].equipmentSkill[j].requireLevel = true;
											armor[i].equipmentSkill[j].level = int.Parse((string)ht["level"]);
										}
										if(ht.ContainsKey("classlevel"))
										{
											armor[i].equipmentSkill[j].requireClassLevel = true;
											armor[i].equipmentSkill[j].classLevel = int.Parse((string)ht["classlevel"]);
										}
										if(ht.ContainsKey("class"))
										{
											armor[i].equipmentSkill[j].requireClass = true;
											armor[i].equipmentSkill[j].classNumber = int.Parse((string)ht["class"]);
										}
										if(ht.ContainsKey(XMLHandler.NODES))
										{
											ArrayList ss = ht[XMLHandler.NODES] as ArrayList;
											foreach(Hashtable skill in ss)
											{
												if(skill[XMLHandler.NODE_NAME] as string == ArmorData.STATUS)
												{
													int k = int.Parse((string)skill["id"]);
													armor[i].equipmentSkill[j].requireStatus[k] = true;
													armor[i].equipmentSkill[j].statusValue[k] = int.Parse((string)skill["value"]);
													armor[i].equipmentSkill[j].statusRequirement[k] = 
															(ValueCheck)System.Enum.Parse(typeof(ValueCheck), (string)skill["requirement"]);
												}
											}
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == ArmorData.ELEMENT)
									{
										int j = int.Parse((string)ht["id"]);
										armor[i].elementValue[j] = int.Parse((string)ht["value"]);
										armor[i].elementOperator[j] = (SimpleOperator)System.Enum.Parse(
												typeof(SimpleOperator), (string)ht["operator"]);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == ArmorData.BONUSSETTINGS)
									{
										armor[i].bonus.SetData(ht);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == ArmorData.RACE)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < armor[i].raceValue.Length)
										{
											armor[i].raceValue[id] = int.Parse((string)ht["value"]);
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == ArmorData.SIZE)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < armor[i].sizeValue.Length)
										{
											armor[i].sizeValue[id] = int.Parse((string)ht["value"]);
										}
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
		
		sv.Add(XMLHandler.NODE_NAME, ArmorData.ARMORS);
		
		for(int i=0; i<name[0].Count(); i++)
		{
			Hashtable ht = new Hashtable();
			ArrayList s = new ArrayList();
			
			ht.Add(XMLHandler.NODE_NAME, ArmorData.ARMOR);
			ht.Add("id", i.ToString());
			if(armor[i].minimumLevel != 0)
			{
				ht.Add("minimumlevel", armor[i].minimumLevel.ToString());
			}
			if(armor[i].minimumClassLevel != 0)
			{
				ht.Add("minimumclasslevel", armor[i].minimumClassLevel.ToString());
			}
			if(armor[i].dropable) ht.Add("dropable", "true");
			ht.Add("equipparts", armor[i].equipPart.Length.ToString());
			ht.Add("equiptype", armor[i].equipType.ToString());
			ht.Add("buyprice", armor[i].buyPrice.ToString());
			if(armor[i].sellable)
			{
				ht.Add("sellprice", armor[i].sellPrice.ToString());
				ht.Add("sellsetter", armor[i].sellSetter.ToString());
			}
			if("" != armor[i].prefabName)
			{
				Hashtable pref = new Hashtable();
				pref.Add(XMLHandler.NODE_NAME, ArmorData.PREFAB);
				pref.Add(XMLHandler.CONTENT, armor[i].prefabName);
				s.Add(pref);
			}
			
			s = this.SaveLanguages(s, i);
			
			for(int j=0; j<armor[i].equipPart.Length; j++)
			{
				if(armor[i].equipPart[j])
				{
					Hashtable ep = HashtableHelper.GetTitleHashtable(ArmorData.EQUIPPART, j);
					ep.Add("enabled", armor[i].equipPart[j].ToString());
					s.Add(ep);
				}
				if(armor[i].blockPart[j])
				{
					Hashtable ep = HashtableHelper.GetTitleHashtable(ArmorData.BLOCKPART, j);
					ep.Add("blocked", armor[i].blockPart[j].ToString());
					s.Add(ep);
				}
			}
			
			ht.Add("effects", armor[i].skillEffect.Length.ToString());
			for(int j=0; j<armor[i].skillEffect.Length; j++)
			{
				if(armor[i].skillEffect[j] > 0)
				{
					Hashtable e = HashtableHelper.GetTitleHashtable(ArmorData.EFFECT, j);
					e.Add("value", armor[i].skillEffect[j].ToString());
					s.Add(e);
				}
			}
			
			if(armor[i].equipmentSkill.Length > 0)
			{
				ht.Add("skills", armor[i].equipmentSkill.Length.ToString());
				for(int j=0; j<armor[i].equipmentSkill.Length; j++)
				{
					Hashtable ep = new Hashtable();
					ep.Add(XMLHandler.NODE_NAME, ArmorData.SKILL);
					ep.Add("id", j.ToString());
					ep.Add("skill", armor[i].equipmentSkill[j].skill.ToString());
					ep.Add("slvl", armor[i].equipmentSkill[j].skillLevel.ToString());
					if(armor[i].equipmentSkill[j].requireLevel)
					{
						ep.Add("level", armor[i].equipmentSkill[j].level.ToString());
					}
					if(armor[i].equipmentSkill[j].requireClassLevel)
					{
						ep.Add("classlevel", armor[i].equipmentSkill[j].classLevel.ToString());
					}
					if(armor[i].equipmentSkill[j].requireClass)
					{
						ep.Add("class", armor[i].equipmentSkill[j].classNumber.ToString());
					}
					
					ArrayList ss = new ArrayList();
					for(int k=0; k<armor[i].equipmentSkill[j].requireStatus.Length; k++)
					{
						if(armor[i].equipmentSkill[j].requireStatus[k])
						{
							Hashtable skill = new Hashtable();
							skill.Add(XMLHandler.NODE_NAME, ArmorData.STATUS);
							skill.Add("id", k.ToString());
							skill.Add("value", armor[i].equipmentSkill[j].statusValue[k]);
							skill.Add("requirement", armor[i].equipmentSkill[j].statusRequirement[k]);
							ss.Add(skill);
						}
					}
					if(ss.Count > 0)
					{
						ep.Add(XMLHandler.NODES, ss);
					}
					s.Add(ep);
				}
			}
			
			ht.Add("elements", armor[i].elementValue.Length.ToString());
			for(int j=0; j<armor[i].elementValue.Length; j++)
			{
				if(armor[i].elementValue[j] != 0 || SimpleOperator.SET.Equals(armor[i].elementOperator[j]))
				{
					Hashtable e = new Hashtable();
					e.Add(XMLHandler.NODE_NAME, ArmorData.ELEMENT);
					e.Add("id", j.ToString());
					e.Add("value", armor[i].elementValue[j].ToString());
					e.Add("operator", armor[i].elementOperator[j].ToString());
					s.Add(e);
				}
			}
				
			ht.Add("races", armor[i].raceValue.Length.ToString());
			for(int j=0; j<armor[i].raceValue.Length; j++)
			{
				if(armor[i].raceValue[j] != 0)
				{
					Hashtable e = HashtableHelper.GetTitleHashtable(ArmorData.RACE, j);
					e.Add("value", armor[i].raceValue[j].ToString());
					s.Add(e);
				}
			}
				
			ht.Add("sizes", armor[i].sizeValue.Length.ToString());
			for(int j=0; j<armor[i].sizeValue.Length; j++)
			{
				if(armor[i].sizeValue[j] != 0)
				{
					Hashtable e = HashtableHelper.GetTitleHashtable(ArmorData.SIZE, j);
					e.Add("value", armor[i].sizeValue[j].ToString());
					s.Add(e);
				}
			}
			
			s.Add(armor[i].bonus.GetData(HashtableHelper.GetTitleHashtable(ArmorData.BONUSSETTINGS)));
			
			ht.Add(XMLHandler.NODES, s);
			subs.Add(ht);
		}
		sv.Add(XMLHandler.NODES, subs);
		data.Add(sv);
		XMLHandler.SaveXML(dir, filename, data);
	}
	
	public void AddArmor(string n, string d, int count, int epCount, int seCount, int elCount)
	{
		base.AddBaseData(n, d, count);
		armor = ArrayHelper.Add(new Armor(), armor);
		armor[armor.Length-1].equipPart = new bool[epCount];
		armor[armor.Length-1].blockPart = new bool[epCount];
		armor[armor.Length-1].skillEffect = new SkillEffect[seCount];
		armor[armor.Length-1].elementValue = new int[elCount];
		armor[armor.Length-1].elementOperator = new SimpleOperator[elCount];
		armor[armor.Length-1].raceValue = new int[DataHolder.Races().GetDataCount()];
		armor[armor.Length-1].sizeValue = new int[DataHolder.Sizes().GetDataCount()];
	}
	
	public override void RemoveData(int index)
	{
		base.RemoveData(index);
		armor = ArrayHelper.Remove(index, armor);
	}
	
	public override void Copy(int index)
	{
		base.Copy(index);
		armor = ArrayHelper.Add(this.GetCopy(index), armor);
	}
	
	public Armor GetCopy(int index)
	{
		Armor arm = new Armor();
		
		arm.minimumLevel = armor[index].minimumLevel;
		arm.minimumClassLevel = armor[index].minimumClassLevel;
		arm.equipType = armor[index].equipType;
		arm.buyPrice = armor[index].buyPrice;
		arm.sellable = armor[index].sellable;
		arm.sellPrice = armor[index].sellPrice;
		arm.sellSetter = armor[index].sellSetter;
		arm.prefabName = armor[index].prefabName;
		arm.dropable = armor[index].dropable;
		
		arm.equipPart = new bool[armor[index].equipPart.Length];
		arm.blockPart = new bool[armor[index].blockPart.Length];
		for(int i=0; i<armor[index].equipPart.Length; i++)
		{
			arm.equipPart[i] = armor[index].equipPart[i];
			arm.blockPart[i] = armor[index].blockPart[i];
		}
		
		arm.skillEffect = new SkillEffect[armor[index].skillEffect.Length];
		for(int i=0; i<armor[index].skillEffect.Length; i++)
		{
			arm.skillEffect[i] = armor[index].skillEffect[i];
		}
		arm.equipmentSkill = new EquipmentSkill[armor[index].equipmentSkill.Length];
		for(int i=0; i<armor[index].equipmentSkill.Length; i++)
		{
			arm.equipmentSkill[i] = new EquipmentSkill(armor[index].equipmentSkill[i].requireStatus.Length);
			arm.equipmentSkill[i].skill = armor[index].equipmentSkill[i].skill;
			arm.equipmentSkill[i].skillLevel = armor[index].equipmentSkill[i].skillLevel;
			arm.equipmentSkill[i].requireLevel = armor[index].equipmentSkill[i].requireLevel;
			arm.equipmentSkill[i].level = armor[index].equipmentSkill[i].level;
			arm.equipmentSkill[i].requireClassLevel = armor[index].equipmentSkill[i].requireClassLevel;
			arm.equipmentSkill[i].classLevel = armor[index].equipmentSkill[i].classLevel;
			arm.equipmentSkill[i].requireClass = armor[index].equipmentSkill[i].requireClass;
			arm.equipmentSkill[i].classNumber = armor[index].equipmentSkill[i].classNumber;
			
			for(int j=0; j<armor[index].equipmentSkill[i].requireStatus.Length; j++)
			{
				arm.equipmentSkill[i].requireStatus[j] = armor[index].equipmentSkill[i].requireStatus[j];
				arm.equipmentSkill[i].statusValue[j] = armor[index].equipmentSkill[i].statusValue[j];
				arm.equipmentSkill[i].statusRequirement[j] = armor[index].equipmentSkill[i].statusRequirement[j];
			}
		}
		
		arm.elementValue = new int[armor[index].elementValue.Length];
		arm.elementOperator = new SimpleOperator[armor[index].elementOperator.Length];
		for(int i=0; i<armor[index].elementValue.Length; i++)
		{
			arm.elementValue[i] = armor[index].elementValue[i];
			arm.elementOperator[i] = armor[index].elementOperator[i];
		}
		arm.bonus.SetData(armor[index].bonus.GetData(new Hashtable()));
		
		arm.raceValue = new int[armor[index].raceValue.Length];
		for(int i=0; i<armor[index].raceValue.Length; i++)
		{
			arm.raceValue[i] = armor[index].raceValue[i];
		}
		
		arm.sizeValue = new int[armor[index].sizeValue.Length];
		for(int i=0; i<armor[index].sizeValue.Length; i++)
		{
			arm.sizeValue[i] = armor[index].sizeValue[i];
		}
		
		return arm;
	}
	
	public void AddEquipmentPart(int index)
	{
		for(int i=0; i<armor.Length; i++)
		{
			armor[i].equipPart = ArrayHelper.Add(false, armor[i].equipPart);
			armor[i].blockPart = ArrayHelper.Add(false, armor[i].blockPart);
		}
	}
	
	public void RemoveEquipmentPart(int index)
	{
		for(int i=0; i<armor.Length; i++)
		{
			armor[i].equipPart = ArrayHelper.Remove(index, armor[i].equipPart);
			armor[i].blockPart = ArrayHelper.Remove(index, armor[i].blockPart);
		}
	}
	
	public void AddStatusValue(int index)
	{
		for(int i=0; i<armor.Length; i++)
		{
			armor[i].bonus.AddStatusValue();
		}
	}
	
	public void RemoveStatusValue(int index)
	{
		for(int i=0; i<armor.Length; i++)
		{
			armor[i].bonus.RemoveStatusValue(index);
		}
	}
	
	public void SetStatusValueType(int index, StatusValueType val)
	{
		if(StatusValueType.CONSUMABLE.Equals(val) || StatusValueType.EXPERIENCE.Equals(val))
		{
			for(int i=0; i<armor.Length; i++)
			{
				armor[i].bonus.SetStatusValueType(index, val);
			}
		}
	}
	
	public void AddStatusEffect(int index)
	{
		for(int i=0; i<armor.Length; i++)
		{
			armor[i].skillEffect = ArrayHelper.Add(SkillEffect.NONE, armor[i].skillEffect);
		}
	}
	
	public void RemoveStatusEffect(int index)
	{
		for(int i=0; i<armor.Length; i++)
		{
			for(int j=index; j<armor[i].skillEffect.Length-1; j++)
			{
				armor[i].skillEffect[j] = armor[i].skillEffect[j+1];
			}
			armor[i].skillEffect = ArrayHelper.Remove(armor[i].skillEffect.Length-1, armor[i].skillEffect);
		}
	}
	
	public void AddElement(int index)
	{
		for(int i=0; i<armor.Length; i++)
		{
			armor[i].bonus.AddElement();
			armor[i].elementValue = ArrayHelper.Add(0, armor[i].elementValue);
			armor[i].elementOperator = ArrayHelper.Add(SimpleOperator.ADD, armor[i].elementOperator);
		}
	}
	
	public void RemoveElement(int index)
	{
		for(int i=0; i<armor.Length; i++)
		{
			armor[i].bonus.RemoveElement(index);
			armor[i].elementValue = ArrayHelper.Remove(index, armor[i].elementValue);
			armor[i].elementOperator = ArrayHelper.Remove(index, armor[i].elementOperator);
		}
	}
	
	public void RemoveSkill(int index)
	{
		for(int i=0; i<armor.Length; i++)
		{
			for(int j=index; j<armor[i].equipmentSkill.Length-1; j++)
			{
				if(armor[i].equipmentSkill[j].skill == index)
				{
					armor[i].equipmentSkill[j].skill = 0;
				}
				else if(armor[i].equipmentSkill[j].skill > index)
				{
					armor[i].equipmentSkill[j].skill -= 1;
				}
			}
		}
	}
	
	public void AddRace()
	{
		for(int i=0; i<armor.Length; i++)
		{
			armor[i].bonus.AddRace();
			armor[i].raceValue = ArrayHelper.Add(0, armor[i].raceValue);
		}
	}
	
	public void RemoveRace(int index)
	{
		for(int i=0; i<armor.Length; i++)
		{
			armor[i].bonus.RemoveRace(index);
			armor[i].raceValue = ArrayHelper.Remove(index, armor[i].raceValue);
		}
	}
	
	public void AddSize()
	{
		for(int i=0; i<armor.Length; i++)
		{
			armor[i].bonus.AddSize();
			armor[i].sizeValue = ArrayHelper.Add(0, armor[i].sizeValue);
		}
	}
	
	public void RemoveSize(int index)
	{
		for(int i=0; i<armor.Length; i++)
		{
			armor[i].bonus.RemoveSize(index);
			armor[i].sizeValue = ArrayHelper.Remove(index, armor[i].sizeValue);
		}
	}
	
	public void RemoveDifficulty(int index)
	{
		for(int i=0; i<armor.Length; i++)
		{
			armor[i].bonus.RemoveDifficulty(index);
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
				if(this.armor[i].equipPart[this.filter.filterID[0]])
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