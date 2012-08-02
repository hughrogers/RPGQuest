
using System.Collections;

public class WeaponData : BaseLangData
{
	public Weapon[] weapon = new Weapon[0];
	
	// XML data
	private string filename = "weapons";
	
	private static string WEAPONS = "weapons";
	private static string WEAPON = "weapon";
	private static string EQUIPPART = "equippart";
	private static string BLOCKPART = "blockpart";
	private static string EFFECT = "effect";
	private static string BONUS = "bonus";
	private static string SKILL = "skill";
	private static string STATUS = "status";
	private static string PREFAB = "prefab";
	private static string ANIMATIONS = "animations";
	private static string BASEATTACK = "baseattack";
	private static string ATTACK = "attack";
	private static string BONUSSETTINGS = "bonussettings";
	private static string RACE = "race";
	private static string ELEMENT = "element";
	private static string SIZE = "size";

	public WeaponData()
	{
		this.filter = new DataFilter(1);
		LoadData();
	}
	
	public override string GetIconPath() { return "Icons/Weapon/"; }
	
	public void LoadData()
	{
		ArrayList data = XMLHandler.LoadXML(dir+filename);
		
		if(data.Count > 0)
		{
			foreach(Hashtable entry in data)
			{
				if(entry[XMLHandler.NODE_NAME] as string == WeaponData.WEAPONS)
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList subs = entry[XMLHandler.NODES] as ArrayList;
						icon = new string[subs.Count];
						weapon = new Weapon[subs.Count];
						
						foreach(Hashtable val in subs)
						{
							if(val[XMLHandler.NODE_NAME] as string == WeaponData.WEAPON)
							{
								// old attack conversion
								BaseAttack[] tmpAtk = new BaseAttack[0];
								
								int i = int.Parse((string)val["id"]);
								icon[i] = "";
								
								weapon[i] = new Weapon();
								if(val.ContainsKey("minimumlevel"))
								{
									weapon[i].minimumLevel = int.Parse((string)val["minimumlevel"]);
								}
								if(val.ContainsKey("minimumclasslevel"))
								{
									weapon[i].minimumClassLevel = int.Parse((string)val["minimumclasslevel"]);
								}
								weapon[i].equipPart = new bool[int.Parse((string)val["equipparts"])];
								weapon[i].blockPart = new bool[int.Parse((string)val["equipparts"])];
								if(val.ContainsKey("dropable")) weapon[i].dropable = true;
								weapon[i].equipType = (EquipType)System.Enum.Parse(typeof(EquipType), (string)val["equiptype"]);
								weapon[i].skillEffect = new SkillEffect[int.Parse((string)val["effects"])];
								
								if(val.ContainsKey("element"))
								{
									weapon[i].ownAttack = true;
									weapon[i].element = int.Parse((string)val["element"]);
								}
								
								int count;
								if(val.ContainsKey("races"))
								{
									count = int.Parse((string)val["races"]);
								}
								else count = DataHolder.Races().GetDataCount();
								weapon[i].raceValue = new int[count];
								
								if(val.ContainsKey("sizes"))
								{
									count = int.Parse((string)val["sizes"]);
								}
								else count = DataHolder.Sizes().GetDataCount();
								weapon[i].sizeValue = new int[count];
								
								if(val.ContainsKey("elements"))
								{
									count = int.Parse((string)val["elements"]);
								}
								else count = DataHolder.Elements().GetDataCount();
								weapon[i].elementValue = new int[count];
								weapon[i].elementOperator = new SimpleOperator[count];
								
								if(val.ContainsKey("attacks"))
								{
									weapon[i].baseAttack = new int[int.Parse((string)val["attacks"])];
								}
								else
								{
									// old attack conversion
									if(val.ContainsKey("baseattacks"))
									{
										weapon[i].baseAttack = new int[int.Parse((string)val["baseattacks"])];
										tmpAtk = new BaseAttack[weapon[i].baseAttack.Length];
										for(int j=0; j<tmpAtk.Length; j++)
										{
											tmpAtk[j] = new BaseAttack(DataHolder.StatusValueCount);
										}
									}
									else
									{
										tmpAtk = new BaseAttack[1];
										tmpAtk[0] = new BaseAttack(DataHolder.StatusValueCount);
										tmpAtk[0].SetData(val);
									}
								}
								
								if(val.ContainsKey("skills"))
								{
									weapon[i].equipmentSkill = new EquipmentSkill[int.Parse((string)val["skills"])];
								}
								
								weapon[i].buyPrice = int.Parse((string)val["buyprice"]);
								if(val.ContainsKey("sellprice"))
								{
									weapon[i].sellable = true;
									weapon[i].sellPrice = int.Parse((string)val["sellprice"]);
									weapon[i].sellSetter = (ValueSetter)System.Enum.Parse(typeof(ValueSetter), (string)val["sellsetter"]);
								}
								if(val.ContainsKey("counter"))
								{
									weapon[i].bonus.counterBonus = int.Parse((string)val["counter"]);
								}
								if(val.ContainsKey("escape"))
								{
									weapon[i].bonus.escapeBonus = int.Parse((string)val["escape"]);
								}
								if(val.ContainsKey("hitbonus"))
								{
									weapon[i].bonus.hitBonus = int.Parse((string)val["hitbonus"]);
								}
								if(val.ContainsKey("animationid"))
								{
									weapon[i].battleAnimations.animateBaseAttack = true;
									weapon[i].battleAnimations.baseAttackAnimationID = int.Parse((string)val["animationid"]);
								}
								if(val.ContainsKey("movespeedreduction"))
								{
									weapon[i].bonus.speedBonus = -float.Parse((string)val["movespeedreduction"]);
								}
								
								ArrayList s = val[XMLHandler.NODES] as ArrayList;
								foreach(Hashtable ht in s)
								{
									this.LoadLanguages(ht, i, subs.Count);
									if(ht[XMLHandler.NODE_NAME] as string == WeaponData.EQUIPPART)
									{
										weapon[i].equipPart[int.Parse((string)ht["id"])] = bool.Parse((string)ht["enabled"]);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == WeaponData.BLOCKPART)
									{
										weapon[i].blockPart[int.Parse((string)ht["id"])] = bool.Parse((string)ht["blocked"]);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == WeaponData.BONUS)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < weapon[i].bonus.statusBonus.Length)
										{
											weapon[i].bonus.statusBonus[id] = int.Parse((string)ht["value"]);
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == WeaponData.EFFECT)
									{
										weapon[i].skillEffect[int.Parse((string)ht["id"])] = (SkillEffect)System.Enum.Parse(typeof(SkillEffect), (string)ht["value"]);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == WeaponData.PREFAB)
									{
										weapon[i].prefabName = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == WeaponData.SKILL)
									{
										int j = int.Parse((string)ht["id"]);
										weapon[i].equipmentSkill[j] = new EquipmentSkill(DataHolder.StatusValueCount);
										weapon[i].equipmentSkill[j].skill = int.Parse((string)ht["skill"]);
										if(ht.ContainsKey("slvl"))
										{
											weapon[i].equipmentSkill[j].skillLevel = int.Parse((string)ht["slvl"]);
										}
										if(ht.ContainsKey("level"))
										{
											weapon[i].equipmentSkill[j].requireLevel = true;
											weapon[i].equipmentSkill[j].level = int.Parse((string)ht["level"]);
										}
										if(ht.ContainsKey("classlevel"))
										{
											weapon[i].equipmentSkill[j].requireClassLevel = true;
											weapon[i].equipmentSkill[j].classLevel = int.Parse((string)ht["classlevel"]);
										}
										if(ht.ContainsKey("class"))
										{
											weapon[i].equipmentSkill[j].requireClass = true;
											weapon[i].equipmentSkill[j].classNumber = int.Parse((string)ht["class"]);
										}
										if(ht.ContainsKey(XMLHandler.NODES))
										{
											ArrayList ss = ht[XMLHandler.NODES] as ArrayList;
											foreach(Hashtable skill in ss)
											{
												if(skill[XMLHandler.NODE_NAME] as string == WeaponData.STATUS)
												{
													int k = int.Parse((string)skill["id"]);
													weapon[i].equipmentSkill[j].requireStatus[k] = true;
													weapon[i].equipmentSkill[j].statusValue[k] = int.Parse((string)skill["value"]);
													weapon[i].equipmentSkill[j].statusRequirement[k] = (ValueCheck)System.Enum.Parse(typeof(ValueCheck), (string)skill["requirement"]);
												}
											}
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == WeaponData.ANIMATIONS)
									{
										weapon[i].ownBaseAnimations = bool.Parse((string)ht["ownbase"]);
										weapon[i].battleAnimations = new BattleAnimationSettings(ht);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == WeaponData.BASEATTACK)
									{
										int j = int.Parse((string)ht["id"]);
										if(j < tmpAtk.Length)
										{
											tmpAtk[j].SetData(ht);
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == WeaponData.BONUSSETTINGS)
									{
										weapon[i].bonus.SetData(ht);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == WeaponData.ATTACK)
									{
										int j = int.Parse((string)ht["id"]);
										if(j < weapon[i].baseAttack.Length)
										{
											weapon[i].baseAttack[j] = int.Parse((string)ht["id2"]);;
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == WeaponData.RACE)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < weapon[i].raceValue.Length)
										{
											weapon[i].raceValue[id] = int.Parse((string)ht["value"]);
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == WeaponData.ELEMENT)
									{
										int j = int.Parse((string)ht["id"]);
										weapon[i].elementValue[j] = int.Parse((string)ht["value"]);
										weapon[i].elementOperator[j] = (SimpleOperator)System.Enum.Parse(
												typeof(SimpleOperator), (string)ht["operator"]);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == WeaponData.SIZE)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < weapon[i].sizeValue.Length)
										{
											weapon[i].sizeValue[id] = int.Parse((string)ht["value"]);
										}
									}
								}
								
								// old attack import
								if(weapon[i].ownAttack && tmpAtk.Length > 0)
								{
									weapon[i].baseAttack = DataHolder.BaseAttacks().ImportOldAttack(tmpAtk, "Weapon");
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
		
		sv.Add(XMLHandler.NODE_NAME, WeaponData.WEAPONS);
		
		for(int i=0; i<name[0].Count(); i++)
		{
			Hashtable ht = new Hashtable();
			ArrayList s = new ArrayList();
			
			ht.Add(XMLHandler.NODE_NAME, WeaponData.WEAPON);
			ht.Add("id", i.ToString());
			if(weapon[i].minimumLevel != 0)
			{
				ht.Add("minimumlevel", weapon[i].minimumLevel.ToString());
			}
			if(weapon[i].minimumClassLevel != 0)
			{
				ht.Add("minimumclasslevel", weapon[i].minimumClassLevel.ToString());
			}
			if(weapon[i].dropable) ht.Add("dropable", "true");
			ht.Add("equipparts", weapon[i].equipPart.Length.ToString());
			ht.Add("equiptype", weapon[i].equipType.ToString());
			ht.Add("buyprice", weapon[i].buyPrice.ToString());
			if(weapon[i].sellable)
			{
				ht.Add("sellprice", weapon[i].sellPrice.ToString());
				ht.Add("sellsetter", weapon[i].sellSetter.ToString());
			}
			if("" != weapon[i].prefabName)
			{
				Hashtable pref = new Hashtable();
				pref.Add(XMLHandler.NODE_NAME, WeaponData.PREFAB);
				pref.Add(XMLHandler.CONTENT, weapon[i].prefabName);
				s.Add(pref);
			}
			
			Hashtable anims = new Hashtable();
			anims.Add(XMLHandler.NODE_NAME, WeaponData.ANIMATIONS);
			anims.Add("ownbase", weapon[i].ownBaseAnimations.ToString());
			if(weapon[i].ownBaseAnimations)
			{
				anims = weapon[i].battleAnimations.GetData(anims);
			}
			s.Add(anims);
			
			s = this.SaveLanguages(s, i);
			
			for(int j=0; j<weapon[i].equipPart.Length; j++)
			{
				if(weapon[i].equipPart[j])
				{
					Hashtable ep = HashtableHelper.GetTitleHashtable(WeaponData.EQUIPPART, j);
					ep.Add("enabled", weapon[i].equipPart[j].ToString());
					s.Add(ep);
				}
				if(weapon[i].blockPart[j])
				{
					Hashtable ep = HashtableHelper.GetTitleHashtable(WeaponData.BLOCKPART, j);
					ep.Add("blocked", weapon[i].blockPart[j].ToString());
					s.Add(ep);
				}
			}
			
			ht.Add("effects", weapon[i].skillEffect.Length.ToString());
			for(int j=0; j<weapon[i].skillEffect.Length; j++)
			{
				if(weapon[i].skillEffect[j] > 0)
				{
					Hashtable e = HashtableHelper.GetTitleHashtable(WeaponData.EFFECT, j);
					e.Add("value", weapon[i].skillEffect[j].ToString());
					s.Add(e);
				}
			}
			
			if(weapon[i].equipmentSkill.Length > 0)
			{
				ht.Add("skills", weapon[i].equipmentSkill.Length.ToString());
				for(int j=0; j<weapon[i].equipmentSkill.Length; j++)
				{
					Hashtable ep = new Hashtable();
					ep.Add(XMLHandler.NODE_NAME, WeaponData.SKILL);
					ep.Add("id", j.ToString());
					ep.Add("skill", weapon[i].equipmentSkill[j].skill.ToString());
					ep.Add("slvl", weapon[i].equipmentSkill[j].skillLevel.ToString());
					if(weapon[i].equipmentSkill[j].requireLevel)
					{
						ep.Add("level", weapon[i].equipmentSkill[j].level.ToString());
					}
					if(weapon[i].equipmentSkill[j].requireClassLevel)
					{
						ep.Add("classlevel", weapon[i].equipmentSkill[j].classLevel.ToString());
					}
					if(weapon[i].equipmentSkill[j].requireClass)
					{
						ep.Add("class", weapon[i].equipmentSkill[j].classNumber.ToString());
					}
					
					ArrayList ss = new ArrayList();
					for(int k=0; k<weapon[i].equipmentSkill[j].requireStatus.Length; k++)
					{
						if(weapon[i].equipmentSkill[j].requireStatus[k])
						{
							Hashtable skill = new Hashtable();
							skill.Add(XMLHandler.NODE_NAME, WeaponData.STATUS);
							skill.Add("id", k.ToString());
							skill.Add("value", weapon[i].equipmentSkill[j].statusValue[k]);
							skill.Add("requirement", weapon[i].equipmentSkill[j].statusRequirement[k]);
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
			
			if(weapon[i].ownAttack)
			{
				ht.Add("element", weapon[i].element.ToString());
				ht.Add("attacks", weapon[i].baseAttack.Length.ToString());
				for(int j=0; j<weapon[i].baseAttack.Length; j++)
				{
					Hashtable ht2 = HashtableHelper.GetTitleHashtable(WeaponData.ATTACK, j);
					ht2.Add("id2", weapon[i].baseAttack[j].ToString());
					s.Add(ht2);
				}
			}
			
			ht.Add("elements", weapon[i].elementValue.Length.ToString());
			for(int j=0; j<weapon[i].elementValue.Length; j++)
			{
				if(weapon[i].elementValue[j] != 0 || SimpleOperator.SET.Equals(weapon[i].elementOperator[j]))
				{
					Hashtable e = new Hashtable();
					e.Add(XMLHandler.NODE_NAME, WeaponData.ELEMENT);
					e.Add("id", j.ToString());
					e.Add("value", weapon[i].elementValue[j].ToString());
					e.Add("operator", weapon[i].elementOperator[j].ToString());
					s.Add(e);
				}
			}
			
			ht.Add("races", weapon[i].raceValue.Length.ToString());
			for(int j=0; j<weapon[i].raceValue.Length; j++)
			{
				if(weapon[i].raceValue[j] != 0)
				{
					Hashtable e = HashtableHelper.GetTitleHashtable(WeaponData.RACE, j);
					e.Add("value", weapon[i].raceValue[j].ToString());
					s.Add(e);
				}
			}
			
			ht.Add("sizes", weapon[i].sizeValue.Length.ToString());
			for(int j=0; j<weapon[i].sizeValue.Length; j++)
			{
				if(weapon[i].sizeValue[j] != 0)
				{
					Hashtable e = HashtableHelper.GetTitleHashtable(WeaponData.SIZE, j);
					e.Add("value", weapon[i].sizeValue[j].ToString());
					s.Add(e);
				}
			}
			
			s.Add(weapon[i].bonus.GetData(HashtableHelper.GetTitleHashtable(WeaponData.BONUSSETTINGS)));
			
			ht.Add(XMLHandler.NODES, s);
			subs.Add(ht);
		}
		sv.Add(XMLHandler.NODES, subs);
		data.Add(sv);
		XMLHandler.SaveXML(dir, filename, data);
	}
	
	public void AddWeapon(string n, string d, int count, int epCount, int seCount)
	{
		base.AddBaseData(n, d, count);
		weapon = ArrayHelper.Add(new Weapon(), weapon);
		weapon[weapon.Length-1].equipPart = new bool[epCount];
		weapon[weapon.Length-1].blockPart = new bool[epCount];
		weapon[weapon.Length-1].skillEffect = new SkillEffect[seCount];
		weapon[weapon.Length-1].raceValue = new int[DataHolder.Races().GetDataCount()];
		weapon[weapon.Length-1].sizeValue = new int[DataHolder.Sizes().GetDataCount()];
		weapon[weapon.Length-1].elementValue = new int[DataHolder.Elements().GetDataCount()];
		weapon[weapon.Length-1].elementOperator = new SimpleOperator[DataHolder.Elements().GetDataCount()];
	}
	
	public override void RemoveData(int index)
	{
		base.RemoveData(index);
		weapon = ArrayHelper.Remove(index, weapon);
	}
	
	public override void Copy(int index)
	{
		base.Copy(index);
		weapon = ArrayHelper.Add(this.GetCopy(index), weapon);
	}
	
	public Weapon GetCopy(int index)
	{
		Weapon wpn = new Weapon();
		
		wpn.minimumLevel = weapon[index].minimumLevel;
		wpn.minimumClassLevel = weapon[index].minimumClassLevel;
		wpn.equipType = weapon[index].equipType;
		wpn.buyPrice = weapon[index].buyPrice;
		wpn.sellable = weapon[index].sellable;
		wpn.sellPrice = weapon[index].sellPrice;
		wpn.sellSetter = weapon[index].sellSetter;
		wpn.ownAttack = weapon[index].ownAttack;
		wpn.dropable = weapon[index].dropable;
		wpn.ownBaseAnimations = weapon[index].ownBaseAnimations;
		wpn.battleAnimations = new BattleAnimationSettings(weapon[index].battleAnimations.GetData(new Hashtable()));
		
		wpn.equipPart = new bool[weapon[index].equipPart.Length];
		wpn.blockPart = new bool[weapon[index].blockPart.Length];
		for(int i=0; i<weapon[index].equipPart.Length; i++)
		{
			wpn.equipPart[i] = weapon[index].equipPart[i];
			wpn.blockPart[i] = weapon[index].blockPart[i];
		}
		
		wpn.skillEffect = new SkillEffect[weapon[index].skillEffect.Length];
		for(int i=0; i<weapon[index].skillEffect.Length; i++)
		{
			wpn.skillEffect[i] = weapon[index].skillEffect[i];
		}
		wpn.equipmentSkill = new EquipmentSkill[weapon[index].equipmentSkill.Length];
		for(int i=0; i<weapon[index].equipmentSkill.Length; i++)
		{
			wpn.equipmentSkill[i] = new EquipmentSkill(weapon[index].equipmentSkill[i].requireStatus.Length);
			wpn.equipmentSkill[i].skill = weapon[index].equipmentSkill[i].skill;
			wpn.equipmentSkill[i].skillLevel = weapon[index].equipmentSkill[i].skillLevel;
			wpn.equipmentSkill[i].requireLevel = weapon[index].equipmentSkill[i].requireLevel;
			wpn.equipmentSkill[i].level = weapon[index].equipmentSkill[i].level;
			wpn.equipmentSkill[i].requireClassLevel = weapon[index].equipmentSkill[i].requireClassLevel;
			wpn.equipmentSkill[i].classLevel = weapon[index].equipmentSkill[i].classLevel;
			wpn.equipmentSkill[i].requireClass = weapon[index].equipmentSkill[i].requireClass;
			wpn.equipmentSkill[i].classNumber = weapon[index].equipmentSkill[i].classNumber;
			
			for(int j=0; j<weapon[index].equipmentSkill[i].requireStatus.Length; j++)
			{
				wpn.equipmentSkill[i].requireStatus[j] = weapon[index].equipmentSkill[i].requireStatus[j];
				wpn.equipmentSkill[i].statusValue[j] = weapon[index].equipmentSkill[i].statusValue[j];
				wpn.equipmentSkill[i].statusRequirement[j] = weapon[index].equipmentSkill[i].statusRequirement[j];
			}
		}
		
		wpn.element = weapon[index].element;
		wpn.baseAttack = new int[weapon[index].baseAttack.Length];
		for(int i=0; i<weapon[index].baseAttack.Length; i++)
		{
			wpn.baseAttack[i] = weapon[index].baseAttack[i];
		}
		
		wpn.bonus.SetData(weapon[index].bonus.GetData(new Hashtable()));
		
		wpn.raceValue = new int[weapon[index].raceValue.Length];
		for(int i=0; i<weapon[index].raceValue.Length; i++)
		{
			wpn.raceValue[i] = weapon[index].raceValue[i];
		}
		
		wpn.sizeValue = new int[weapon[index].sizeValue.Length];
		for(int i=0; i<weapon[index].sizeValue.Length; i++)
		{
			wpn.sizeValue[i] = weapon[index].sizeValue[i];
		}
		
		wpn.elementValue = new int[weapon[index].elementValue.Length];
		wpn.elementOperator = new SimpleOperator[weapon[index].elementOperator.Length];
		for(int i=0; i<weapon[index].elementValue.Length; i++)
		{
			wpn.elementValue[i] = weapon[index].elementValue[i];
			wpn.elementOperator[i] = weapon[index].elementOperator[i];
		}
		
		return wpn;
	}
	
	public void AddEquipmentPart(int index)
	{
		for(int i=0; i<weapon.Length; i++)
		{
			weapon[i].equipPart = ArrayHelper.Add(false, weapon[i].equipPart);
			weapon[i].blockPart = ArrayHelper.Add(false, weapon[i].blockPart);
		}
	}
	
	public void RemoveEquipmentPart(int index)
	{
		for(int i=0; i<weapon.Length; i++)
		{
			weapon[i].equipPart = ArrayHelper.Remove(index, weapon[i].equipPart);
			weapon[i].blockPart = ArrayHelper.Remove(index, weapon[i].blockPart);
		}
	}
	
	public void AddStatusValue(int index)
	{
		for(int i=0; i<weapon.Length; i++)
		{
			weapon[i].bonus.AddStatusValue();
		}
	}
	
	public void RemoveStatusValue(int index)
	{
		for(int i=0; i<weapon.Length; i++)
		{
			weapon[i].bonus.RemoveStatusValue(index);
		}
	}
	
	public void SetStatusValueType(int index, StatusValueType val)
	{
		if(StatusValueType.CONSUMABLE.Equals(val) || StatusValueType.EXPERIENCE.Equals(val))
		{
			for(int i=0; i<weapon.Length; i++)
			{
				weapon[i].bonus.SetStatusValueType(index, val);
			}
		}
	}
	
	public void AddStatusEffect(int index)
	{
		for(int i=0; i<weapon.Length; i++)
		{
			weapon[i].skillEffect = ArrayHelper.Add(SkillEffect.NONE, weapon[i].skillEffect);
		}
	}
	
	public void RemoveStatusEffect(int index)
	{
		for(int i=0; i<weapon.Length; i++)
		{
			for(int j=index; j<weapon[i].skillEffect.Length-1; j++)
			{
				weapon[i].skillEffect[j] = weapon[i].skillEffect[j+1];
			}
			weapon[i].skillEffect = ArrayHelper.Remove(weapon[i].skillEffect.Length-1, weapon[i].skillEffect);
		}
	}
	
	public void AddElement(int index)
	{
		for(int i=0; i<weapon.Length; i++)
		{
			weapon[i].bonus.AddElement();
			weapon[i].elementValue = ArrayHelper.Add(0, weapon[i].elementValue);
			weapon[i].elementOperator = ArrayHelper.Add(SimpleOperator.ADD, weapon[i].elementOperator);
		}
	}
	
	public void RemoveElement(int index)
	{
		for(int i=0; i<weapon.Length; i++)
		{
			weapon[i].bonus.RemoveElement(index);
			if(weapon[i].element == index)
			{
				weapon[i].element = 0;
			}
			else if(weapon[i].element > index)
			{
				weapon[i].element -= 1;
			}
			weapon[i].elementValue = ArrayHelper.Remove(index, weapon[i].elementValue);
			weapon[i].elementOperator = ArrayHelper.Remove(index, weapon[i].elementOperator);
		}
	}
	
	public void RemoveSkill(int index)
	{
		for(int i=0; i<weapon.Length; i++)
		{
			for(int j=index; j<weapon[i].equipmentSkill.Length-1; j++)
			{
				if(weapon[i].equipmentSkill[j].skill == index)
				{
					weapon[i].equipmentSkill[j].skill = 0;
				}
				else if(weapon[i].equipmentSkill[j].skill > index)
				{
					weapon[i].equipmentSkill[j].skill -= 1;
				}
			}
		}
	}
	
	public void RemoveBaseAttack(int index)
	{
		for(int i=0; i<weapon.Length; i++)
		{
			for(int j=0; j<weapon[i].baseAttack.Length; j++)
			{
				weapon[i].baseAttack[j] = this.CheckForIndex(index, weapon[i].baseAttack[j]);
			}
		}
	}
	
	public void AddRace()
	{
		for(int i=0; i<weapon.Length; i++)
		{
			weapon[i].bonus.AddRace();
			weapon[i].raceValue = ArrayHelper.Add(0, weapon[i].raceValue);
		}
	}
	
	public void RemoveRace(int index)
	{
		for(int i=0; i<weapon.Length; i++)
		{
			weapon[i].bonus.RemoveRace(index);
			weapon[i].raceValue = ArrayHelper.Remove(index, weapon[i].raceValue);
		}
	}
	
	public void AddSize()
	{
		for(int i=0; i<weapon.Length; i++)
		{
			weapon[i].bonus.AddSize();
			weapon[i].sizeValue = ArrayHelper.Add(0, weapon[i].sizeValue);
		}
	}
	
	public void RemoveSize(int index)
	{
		for(int i=0; i<weapon.Length; i++)
		{
			weapon[i].bonus.RemoveSize(index);
			weapon[i].sizeValue = ArrayHelper.Remove(index, weapon[i].sizeValue);
		}
	}
	
	public void RemoveDifficulty(int index)
	{
		for(int i=0; i<weapon.Length; i++)
		{
			weapon[i].bonus.RemoveDifficulty(index);
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
				if(this.weapon[i].equipPart[this.filter.filterID[0]])
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