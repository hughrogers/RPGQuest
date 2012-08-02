
using UnityEngine;
using System.Collections;

public class StatusEffectData : BaseLangData
{
	public StatusEffect[] effect = new StatusEffect[0];
	
	// XML data
	private string filename = "statusEffects";
	
	private static string STATUSEFFECTS = "statuseffects";
	private static string EFFECT = "effect";
	private static string CONDITION = "condition";
	private static string ELEMENT = "element";
	private static string AUTOAPPLY = "autoapply";
	private static string AUTOREMOVE = "autoremove";
	private static string PREFAB = "prefab";
	private static string ENDEFFECT = "endeffect";
	private static string BONUSSETTINGS = "bonussettings";
	private static string RACE = "race";
	private static string SIZE = "size";
	private static string SKILLTYPEBLOCK = "skilltypeblock";

	public StatusEffectData()
	{
		LoadData();
	}
	
	public override string GetIconPath() { return "Icons/StatusEffect/"; }
	
	public void LoadData()
	{
		ArrayList data = XMLHandler.LoadXML(dir+filename);
		
		if(data.Count > 0)
		{
			foreach(Hashtable entry in data)
			{
				if(entry[XMLHandler.NODE_NAME] as string == StatusEffectData.STATUSEFFECTS)
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList subs = entry[XMLHandler.NODES] as ArrayList;
						effect = new StatusEffect[subs.Count];
						icon = new string[subs.Count];
						
						foreach(Hashtable val in subs)
						{
							if(val[XMLHandler.NODE_NAME] as string == StatusEffectData.EFFECT)
							{
								int i = int.Parse((string)val["id"]);
								icon[i] = "";
								
								effect[i] = new StatusEffect();
								int count = int.Parse((string)val["conditions"]);
								effect[i].condition = new StatusCondition[count];
								for(int j=0; j<count; j++)
								{
									effect[i].condition[j] = new StatusCondition();
								}
								effect[i].endWithBattle = bool.Parse((string)val["endwithbattle"]);
								effect[i].end = (StatusEffectEnd)System.Enum.Parse(typeof(StatusEffectEnd), (string)val["end"]);
								effect[i].endValue = int.Parse((string)val["endvalue"]);
								effect[i].endChance = int.Parse((string)val["endchance"]);
								
								if(val.ContainsKey("stopmove")) effect[i].stopMove = true;
								if(val.ContainsKey("stopmovement")) effect[i].stopMovement = true;
								if(val.ContainsKey("autoattack")) effect[i].autoAttack = true;
								if(val.ContainsKey("attackfriends")) effect[i].attackFriends = true;
								if(val.ContainsKey("blockattack")) effect[i].blockAttack = true;
								if(val.ContainsKey("blockskills")) effect[i].blockSkills = true;
								if(val.ContainsKey("blockitems")) effect[i].blockItems = true;
								if(val.ContainsKey("blockdefend")) effect[i].blockDefend = true;
								if(val.ContainsKey("blockescape")) effect[i].blockEscape = true;
								if(val.ContainsKey("endonattack")) effect[i].endOnAttack = true;
								if(val.ContainsKey("reflectskills")) effect[i].reflectSkills = true;
								
								if(val.ContainsKey("movespeedreduction"))
								{
									effect[i].bonus.speedBonus = -float.Parse((string)val["movespeedreduction"]);
								}
								
								if(val.ContainsKey("attackelement"))
								{
									effect[i].setElement = true;
									effect[i].attackElement = int.Parse((string)val["attackelement"]);
								}
								if(val.ContainsKey("hitchance"))
								{
									effect[i].hitChance = true;
									effect[i].hitFormula = int.Parse((string)val["hitchance"]);
								}
								if(val.ContainsKey("reducehit"))
								{
									effect[i].bonus.hitBonus = -int.Parse((string)val["reducehit"]);
								}
								int elements = int.Parse((string)val["elements"]);
								effect[i].elementValue = new int[elements];
								effect[i].elementOperator = new SimpleOperator[elements];
								
								if(val.ContainsKey("races"))
								{
									count = int.Parse((string)val["races"]);
								}
								else count = DataHolder.Races().GetDataCount();
								effect[i].raceValue = new int[count];
								if(val.ContainsKey("sizes"))
								{
									count = int.Parse((string)val["sizes"]);
								}
								else count = DataHolder.Sizes().GetDataCount();
								effect[i].sizeValue = new int[count];
								
								if(val.ContainsKey("autoapply"))
								{
									effect[i].autoApply = true;
									effect[i].applyRequirement = new StatusRequirement[int.Parse((string)val["autoapply"])];
									effect[i].applyNeeded = (AIConditionNeeded)System.Enum.Parse(typeof(AIConditionNeeded), (string)val["applyneeded"]);
								}
								if(val.ContainsKey("autoremove"))
								{
									effect[i].autoRemove = true;
									effect[i].removeRequirement = new StatusRequirement[int.Parse((string)val["autoremove"])];
									effect[i].removeNeeded = (AIConditionNeeded)System.Enum.Parse(typeof(AIConditionNeeded), (string)val["removeneeded"]);
								}
								if(val.ContainsKey("prefabs"))
								{
									effect[i].prefab = new EffectPrefab[int.Parse((string)val["prefabs"])];
								}
								if(val.ContainsKey("endeffects"))
								{
									count = int.Parse((string)val["endeffects"]);
									effect[i].effectChangeID = new int[count];
									effect[i].endEffectChanges = new SkillEffect[count];
								}
								
								if(val.ContainsKey("blockbaseattacks"))
								{
									this.effect[i].blockBaseAttacks = true;
								}
								
								ArrayList s = val[XMLHandler.NODES] as ArrayList;
								foreach(Hashtable ht in s)
								{
									this.LoadLanguages(ht, i, subs.Count);
									if(ht[XMLHandler.NODE_NAME] as string == StatusEffectData.CONDITION)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < this.effect[i].condition.Length)
										{
											this.effect[i].condition[id].SetData(ht);
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == StatusEffectData.ELEMENT)
									{
										int j = int.Parse((string)ht["id"]);
										effect[i].elementValue[j] = int.Parse((string)ht["value"]);
										effect[i].elementOperator[j] = (SimpleOperator)System.Enum.Parse(typeof(SimpleOperator), (string)ht["operator"]);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == StatusEffectData.AUTOAPPLY)
									{
										int j = int.Parse((string)ht["id"]);
										if(j < effect[i].applyRequirement.Length)
										{
											effect[i].applyRequirement[j] = new StatusRequirement(ht);
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == StatusEffectData.AUTOREMOVE)
									{
										int j = int.Parse((string)ht["id"]);
										if(j < effect[i].removeRequirement.Length)
										{
											effect[i].removeRequirement[j] = new StatusRequirement(ht);
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == StatusEffectData.PREFAB)
									{
										int j = int.Parse((string)ht["id"]);
										if(j < effect[i].prefab.Length)
										{
											effect[i].prefab[j] = new EffectPrefab(ht);
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == StatusEffectData.ENDEFFECT)
									{
										int j = int.Parse((string)ht["id"]);
										if(j < effect[i].endEffectChanges.Length)
										{
											effect[i].effectChangeID[j] = int.Parse((string)ht["effect"]);
											effect[i].endEffectChanges[j] = (SkillEffect)System.Enum.Parse(
													typeof(SkillEffect), (string)ht["change"]);
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == StatusEffectData.BONUSSETTINGS)
									{
										effect[i].bonus.SetData(ht);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == StatusEffectData.RACE)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < effect[i].raceValue.Length)
										{
											effect[i].raceValue[id] = int.Parse((string)ht["value"]);
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == StatusEffectData.SIZE)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < effect[i].sizeValue.Length)
										{
											effect[i].sizeValue[id] = int.Parse((string)ht["value"]);
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == StatusEffectData.SKILLTYPEBLOCK)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < effect[i].skillTypeBlock.Length)
										{
											effect[i].skillTypeBlock[id] = true;
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
		if(name.Length > 0)
		{
			ArrayList data = new ArrayList();
			ArrayList subs = new ArrayList();
			
			Hashtable sv = new Hashtable();
			sv.Add(XMLHandler.NODE_NAME, StatusEffectData.STATUSEFFECTS);
			
			for(int i=0; i<name[0].Count(); i++)
			{
				Hashtable val = new Hashtable();
				ArrayList s = new ArrayList();
				
				val.Add(XMLHandler.NODE_NAME, StatusEffectData.EFFECT);
				val.Add("id", i.ToString());
				if(effect[i].stopMove) val.Add("stopmove", "true");
				if(effect[i].stopMovement) val.Add("stopmovement", "true");
				if(effect[i].autoAttack) val.Add("autoattack", "true");
				if(effect[i].attackFriends) val.Add("attackfriends", "true");
				if(effect[i].blockAttack) val.Add("blockattack", "true");
				if(effect[i].blockSkills) val.Add("blockskills", "true");
				if(effect[i].blockItems) val.Add("blockitems", "true");
				if(effect[i].blockDefend) val.Add("blockdefend", "true");
				if(effect[i].blockEscape) val.Add("blockescape", "true");
				if(effect[i].endOnAttack) val.Add("endonattack", "true");
				if(effect[i].reflectSkills) val.Add("reflectskills", "true");
				val.Add("endwithbattle", effect[i].endWithBattle.ToString());
				val.Add("end", effect[i].end.ToString());
				val.Add("endvalue", effect[i].endValue.ToString());
				val.Add("endchance", effect[i].endChance.ToString());
				val.Add("conditions", effect[i].condition.Length.ToString());
				
				if(effect[i].setElement) val.Add("attackelement", effect[i].attackElement.ToString());
				
				if(effect[i].hitChance)
				{
					val.Add("hitchance", effect[i].hitFormula.ToString());
				}
				
				s = this.SaveLanguages(s, i);
				
				for(int j=0; j<effect[i].condition.Length; j++)
				{
					if(effect[i].condition[j].apply)
					{
						s.Add(this.effect[i].condition[j].GetData(
								HashtableHelper.GetTitleHashtable(StatusEffectData.CONDITION, j)));
					}
				}
				
				val.Add("elements", effect[i].elementValue.Length.ToString());
				for(int j=0; j<effect[i].elementValue.Length; j++)
				{
					if(effect[i].elementValue[j] != 0 || SimpleOperator.SET.Equals(effect[i].elementOperator[j]))
					{
						Hashtable e = new Hashtable();
						e.Add(XMLHandler.NODE_NAME, StatusEffectData.ELEMENT);
						e.Add("id", j.ToString());
						e.Add("value", effect[i].elementValue[j].ToString());
						e.Add("operator", effect[i].elementOperator[j].ToString());
						s.Add(e);
					}
				}
				
				val.Add("races", effect[i].raceValue.Length.ToString());
				for(int j=0; j<effect[i].raceValue.Length; j++)
				{
					if(effect[i].raceValue[j] != 0)
					{
						Hashtable e = HashtableHelper.GetTitleHashtable(StatusEffectData.RACE, j);
						e.Add("value", effect[i].raceValue[j].ToString());
						s.Add(e);
					}
				}
				
				val.Add("sizes", effect[i].sizeValue.Length.ToString());
				for(int j=0; j<effect[i].sizeValue.Length; j++)
				{
					if(effect[i].sizeValue[j] != 0)
					{
						Hashtable e = HashtableHelper.GetTitleHashtable(StatusEffectData.SIZE, j);
						e.Add("value", effect[i].sizeValue[j].ToString());
						s.Add(e);
					}
				}
				
				if(effect[i].autoApply)
				{
					val.Add("autoapply", effect[i].applyRequirement.Length.ToString());
					val.Add("applyneeded", effect[i].applyNeeded.ToString());
					for(int j=0; j<effect[i].applyRequirement.Length; j++)
					{
						s.Add(effect[i].applyRequirement[j].GetData(
								HashtableHelper.GetTitleHashtable(StatusEffectData.AUTOAPPLY, j)));
					}
				}
				if(effect[i].autoRemove)
				{
					val.Add("autoremove", effect[i].removeRequirement.Length.ToString());
					val.Add("removeneeded", effect[i].removeNeeded.ToString());
					for(int j=0; j<effect[i].removeRequirement.Length; j++)
					{
						s.Add(effect[i].removeRequirement[j].GetData(
								HashtableHelper.GetTitleHashtable(StatusEffectData.AUTOREMOVE, j)));
					}
				}
				
				if(effect[i].prefab.Length > 0)
				{
					val.Add("prefabs", effect[i].prefab.Length.ToString());
					for(int j=0; j<effect[i].prefab.Length; j++)
					{
						s.Add(effect[i].prefab[j].GetData(
								HashtableHelper.GetTitleHashtable(StatusEffectData.PREFAB, j)));
					}
				}
				
				if(effect[i].endEffectChanges.Length > 0)
				{
					val.Add("endeffects", effect[i].endEffectChanges.Length.ToString());
					for(int j=0; j<effect[i].endEffectChanges.Length; j++)
					{
						Hashtable e = HashtableHelper.GetTitleHashtable(StatusEffectData.ENDEFFECT, j);
						e.Add("effect", effect[i].effectChangeID[j].ToString());
						e.Add("change", effect[i].endEffectChanges[j].ToString());
						s.Add(e);
					}
				}
				
				if(this.effect[i].blockBaseAttacks) val.Add("blockbaseattacks", "true");
				for(int j=0; j<this.effect[i].skillTypeBlock.Length; j++)
				{
					if(this.effect[i].skillTypeBlock[j])
					{
						s.Add(HashtableHelper.GetTitleHashtable(StatusEffectData.SKILLTYPEBLOCK, j));
					}
				}
				
				s.Add(effect[i].bonus.GetData(HashtableHelper.GetTitleHashtable(StatusEffectData.BONUSSETTINGS)));
				
				val.Add(XMLHandler.NODES, s);
				subs.Add(val);
			}
			sv.Add(XMLHandler.NODES, subs);
			data.Add(sv);
			
			XMLHandler.SaveXML(dir, filename, data);
		}
	}
	
	public void AddEffect(string n, string d, int langs, int values, int elCount)
	{
		base.AddBaseData(n, d, langs);
		effect = ArrayHelper.Add(new StatusEffect(), effect);
		
		effect[effect.Length-1].condition = new StatusCondition[values];
		for(int i=0; i<values; i++)
		{
			effect[effect.Length-1].condition[i] = new StatusCondition();
		}
		effect[effect.Length-1].elementValue = new int[elCount];
		effect[effect.Length-1].elementOperator = new SimpleOperator[elCount];
		effect[effect.Length-1].raceValue = new int[DataHolder.Races().GetDataCount()];
		effect[effect.Length-1].sizeValue = new int[DataHolder.Sizes().GetDataCount()];
	}
	
	public override void RemoveData(int index)
	{
		base.RemoveData(index);
		effect = ArrayHelper.Remove(index, effect);
	}
	
	public StatusEffect GetCopy(int index)
	{
		StatusEffect e = new StatusEffect();
		e.realID = index;
		e.stopMove = effect[index].stopMove;
		e.stopMovement = effect[index].stopMovement;
		e.autoAttack = effect[index].autoAttack;
		e.attackFriends = effect[index].attackFriends;
		e.blockAttack = effect[index].blockAttack;
		e.blockSkills = effect[index].blockSkills;
		e.blockItems = effect[index].blockItems;
		e.blockDefend = effect[index].blockDefend;
		e.blockEscape = effect[index].blockEscape;
		e.reflectSkills = effect[index].reflectSkills;
		e.endWithBattle = effect[index].endWithBattle;
		e.endOnAttack = effect[index].endOnAttack;
		e.end = effect[index].end;
		e.endValue = effect[index].endValue;
		e.endChance = effect[index].endChance;
		e.setElement = effect[index].setElement;
		e.attackElement = effect[index].attackElement;
		e.hitChance = effect[index].hitChance;
		e.hitFormula = effect[index].hitFormula;
		
		
		e.blockBaseAttacks = effect[index].blockBaseAttacks;
		for(int i=0; i<effect[index].skillTypeBlock.Length; i++)
		{
			e.skillTypeBlock[i] = effect[index].skillTypeBlock[i];
		}
		
		e.condition = new StatusCondition[effect[index].condition.Length];
		for(int i=0; i<effect[index].condition.Length; i++)
		{
			e.condition[i] = new StatusCondition();
			e.condition[i].apply = effect[index].condition[i].apply;
			e.condition[i].stopChange = effect[index].condition[i].stopChange;
			e.condition[i].simpleOperator = effect[index].condition[i].simpleOperator;
			e.condition[i].value = effect[index].condition[i].value;
			e.condition[i].setter = effect[index].condition[i].setter;
			e.condition[i].execution = effect[index].condition[i].execution;
			e.condition[i].time = effect[index].condition[i].time;
		}
		
		e.elementValue = new int[effect[index].elementValue.Length];
		e.elementOperator = new SimpleOperator[effect[index].elementOperator.Length];
		for(int i=0; i<effect[index].elementValue.Length; i++)
		{
			e.elementValue[i] = effect[index].elementValue[i];
			e.elementOperator[i] = effect[index].elementOperator[i];
		}
		
		e.raceValue = new int[effect[index].raceValue.Length];
		for(int i=0; i<effect[index].raceValue.Length; i++)
		{
			e.raceValue[i] = effect[index].raceValue[i];
		}
		
		e.sizeValue = new int[effect[index].sizeValue.Length];
		for(int i=0; i<effect[index].sizeValue.Length; i++)
		{
			e.sizeValue[i] = effect[index].sizeValue[i];
		}
		
		e.autoApply = effect[index].autoApply;
		e.applyNeeded = effect[index].applyNeeded;
		e.applyRequirement = new StatusRequirement[effect[index].applyRequirement.Length];
		for(int i=0; i<effect[index].applyRequirement.Length; i++)
		{
			e.applyRequirement[i] = new StatusRequirement(
					effect[index].applyRequirement[i].GetData(new Hashtable()));
		}
		
		e.autoRemove = effect[index].autoRemove;
		e.removeNeeded = effect[index].removeNeeded;
		e.removeRequirement = new StatusRequirement[effect[index].removeRequirement.Length];
		for(int i=0; i<effect[index].removeRequirement.Length; i++)
		{
			e.removeRequirement[i] = new StatusRequirement(
					effect[index].removeRequirement[i].GetData(new Hashtable()));
		}
		
		e.prefab = new EffectPrefab[effect[index].prefab.Length];
		for(int i=0; i<e.prefab.Length; i++)
		{
			e.prefab[i] = new EffectPrefab(
					effect[index].prefab[i].GetData(new Hashtable()));
		}
		
		e.effectChangeID = new int[effect[index].effectChangeID.Length];
		e.endEffectChanges = new SkillEffect[effect[index].endEffectChanges.Length];
		for(int i=0; i<e.effectChangeID.Length; i++)
		{
			e.effectChangeID[i] = effect[index].effectChangeID[i];
			e.endEffectChanges[i] = effect[index].endEffectChanges[i];
		}
		
		e.bonus.SetData(effect[index].bonus.GetData(new Hashtable()));
		
		return e;
	}
	
	public override void Copy(int index)
	{
		base.Copy(index);
		effect = ArrayHelper.Add(this.GetCopy(index), effect);
	}
	
	public void AddStatusValue(int index)
	{
		for(int i=0; i<effect.Length; i++)
		{
			effect[i].bonus.AddStatusValue();
			effect[i].condition = ArrayHelper.Add(new StatusCondition(), effect[i].condition);
		}
	}
	
	public void RemoveStatusValue(int index)
	{
		for(int i=0; i<effect.Length; i++)
		{
			effect[i].bonus.RemoveStatusValue(index);
			effect[i].condition = ArrayHelper.Remove(index, effect[i].condition);
			
			for(int j=0; j<effect[i].applyRequirement.Length; j++)
			{
				if(StatusNeeded.STATUS_VALUE.Equals(effect[i].applyRequirement[j].statusNeeded))
				{
					effect[i].applyRequirement[j].statID = this.CheckForIndex(index, effect[i].applyRequirement[j].statID);
				}
			}
			for(int j=0; j<effect[i].removeRequirement.Length; j++)
			{
				if(StatusNeeded.STATUS_VALUE.Equals(effect[i].removeRequirement[j].statusNeeded))
				{
					effect[i].removeRequirement[j].statID = this.CheckForIndex(index, effect[i].removeRequirement[j].statID);
				}
			}
		}
	}
	
	public void SetStatusValueType(int index, StatusValueType val)
	{
		if(StatusValueType.CONSUMABLE.Equals(val) || 
			StatusValueType.EXPERIENCE.Equals(val))
		{
			for(int i=0; i<effect.Length; i++)
			{
				effect[i].bonus.SetStatusValueType(index, val);
			}
		}
	}
	
	public void AddElement(int index)
	{
		for(int i=0; i<effect.Length; i++)
		{
			effect[i].bonus.AddElement();
			effect[i].elementValue = ArrayHelper.Add(0, effect[i].elementValue);
			effect[i].elementOperator = ArrayHelper.Add(SimpleOperator.ADD, effect[i].elementOperator);
		}
	}
	
	public void RemoveElement(int index)
	{
		for(int i=0; i<effect.Length; i++)
		{
			effect[i].bonus.RemoveElement(index);
			if(effect[i].attackElement == index)
			{
				effect[i].attackElement = 0;
			}
			else if(effect[i].attackElement > index)
			{
				effect[i].attackElement -= 1;
			}
			
			effect[i].elementValue = ArrayHelper.Remove(index, effect[i].elementValue);
			effect[i].elementOperator = ArrayHelper.Remove(index, effect[i].elementOperator);
			
			for(int j=0; j<effect[i].applyRequirement.Length; j++)
			{
				if(StatusNeeded.ELEMENT.Equals(effect[i].applyRequirement[j].statusNeeded))
				{
					effect[i].applyRequirement[j].statID = this.CheckForIndex(index, effect[i].applyRequirement[j].statID);
				}
			}
			for(int j=0; j<effect[i].removeRequirement.Length; j++)
			{
				if(StatusNeeded.ELEMENT.Equals(effect[i].removeRequirement[j].statusNeeded))
				{
					effect[i].removeRequirement[j].statID = this.CheckForIndex(index, effect[i].removeRequirement[j].statID);
				}
			}
		}
	}
	
	public void AddRace()
	{
		for(int i=0; i<effect.Length; i++)
		{
			effect[i].bonus.AddRace();
			effect[i].raceValue = ArrayHelper.Add(0, effect[i].raceValue);
		}
	}
	
	public void RemoveRace(int index)
	{
		for(int i=0; i<effect.Length; i++)
		{
			effect[i].bonus.RemoveRace(index);
			effect[i].raceValue = ArrayHelper.Remove(index, effect[i].raceValue);
			for(int j=0; j<effect[i].applyRequirement.Length; j++)
			{
				if(StatusNeeded.RACE.Equals(effect[i].applyRequirement[j].statusNeeded))
				{
					effect[i].applyRequirement[j].statID = this.CheckForIndex(index, effect[i].applyRequirement[j].statID);
				}
			}
			for(int j=0; j<effect[i].removeRequirement.Length; j++)
			{
				if(StatusNeeded.RACE.Equals(effect[i].removeRequirement[j].statusNeeded))
				{
					effect[i].removeRequirement[j].statID = this.CheckForIndex(index, effect[i].removeRequirement[j].statID);
				}
			}
		}
	}
	
	public void AddSize()
	{
		for(int i=0; i<effect.Length; i++)
		{
			effect[i].bonus.AddSize();
			effect[i].sizeValue = ArrayHelper.Add(0, effect[i].sizeValue);
		}
	}
	
	public void RemoveSize(int index)
	{
		for(int i=0; i<effect.Length; i++)
		{
			effect[i].bonus.RemoveSize(index);
			effect[i].sizeValue = ArrayHelper.Remove(index, effect[i].sizeValue);
			for(int j=0; j<effect[i].applyRequirement.Length; j++)
			{
				if(StatusNeeded.SIZE.Equals(effect[i].applyRequirement[j].statusNeeded))
				{
					effect[i].applyRequirement[j].statID = this.CheckForIndex(index, effect[i].applyRequirement[j].statID);
				}
			}
			for(int j=0; j<effect[i].removeRequirement.Length; j++)
			{
				if(StatusNeeded.SIZE.Equals(effect[i].removeRequirement[j].statusNeeded))
				{
					effect[i].removeRequirement[j].statID = this.CheckForIndex(index, effect[i].removeRequirement[j].statID);
				}
			}
		}
	}
	
	public void RemoveSkill(int index)
	{
		for(int i=0; i<effect.Length; i++)
		{
			for(int j=0; j<effect[i].applyRequirement.Length; j++)
			{
				if(StatusNeeded.SKILL.Equals(effect[i].applyRequirement[j].statusNeeded))
				{
					effect[i].applyRequirement[j].statID = this.CheckForIndex(index, effect[i].applyRequirement[j].statID);
				}
			}
			for(int j=0; j<effect[i].removeRequirement.Length; j++)
			{
				if(StatusNeeded.SKILL.Equals(effect[i].removeRequirement[j].statusNeeded))
				{
					effect[i].removeRequirement[j].statID = this.CheckForIndex(index, effect[i].removeRequirement[j].statID);
				}
			}
		}
	}
	
	public void RemoveDifficulty(int index)
	{
		for(int i=0; i<effect.Length; i++)
		{
			effect[i].bonus.RemoveDifficulty(index);
		}
	}
	
	public void AddSkillType()
	{
		for(int i=0; i<this.effect.Length; i++)
		{
			this.effect[i].skillTypeBlock = ArrayHelper.Add(false, this.effect[i].skillTypeBlock);
		}
	}
	
	public void RemoveSkillType(int index)
	{
		for(int i=0; i<this.effect.Length; i++)
		{
			this.effect[i].skillTypeBlock = ArrayHelper.Remove(index, this.effect[i].skillTypeBlock);
		}
	}
	
	public void CheckAutoApply(Combatant c)
	{
		for(int i=0; i<effect.Length; i++)
		{
			if(effect[i].CheckAutoApply(c))
			{
				c.AddEffect(i, c);
			}
		}
	}
}