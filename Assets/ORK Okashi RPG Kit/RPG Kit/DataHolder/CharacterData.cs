
using System.Collections;

public class CharacterData : BaseLangData
{
	public Character[] character = new Character[0];
	
	public static string PREFAB_PATH = "Prefabs/Characters/";
	
	// XML data
	private string filename = "characters";
	
	private static string CHARACTERS = "characters";
	private static string CHARACTER = "character";
	public static string STATUSVALUE = "statusvalue";
	public static string SKILL = "skill";
	private static string ANIMATIONS = "animations";
	private static string PREFAB = "prefab";
	private static string PREFABROOT = "prefabroot";
	private static string AUDIOCLIP = "audioclip";
	private static string AUTOATTACK = "autoattack";
	private static string AIMOVER = "aimover";
	private static string AIBEHAVIOUR = "aibehaviour";
	private static string BASEATTACK = "baseattack";
	private static string ATTACK = "attack";
	private static string CONTROLMAP = "controlmap";
	private static string FIELDANIMATIONS = "fieldanimations";
	private static string BONUSSETTINGS = "bonussettings";
	private static string DEVELOPMENT = "development";
	private static string CUSTOMANIMATIONS = "customanimations";
	private static string FIELDSTATUSCHANGE = "fieldstatuschange";
	private static string BATTLESTATUSCHANGE = "battlestatuschange";

	public CharacterData()
	{
		this.filter = new DataFilter(2);
		LoadData();
	}
	
	public override string GetIconPath() { return "Icons/Character/"; }
	
	public void LoadData()
	{
		ArrayList data = XMLHandler.LoadXML(dir+filename);
		
		if(data.Count > 0)
		{
			foreach(Hashtable entry in data)
			{
				if(entry[XMLHandler.NODE_NAME] as string == CharacterData.CHARACTERS)
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList subs = entry[XMLHandler.NODES] as ArrayList;
						icon = new string[subs.Count];
						character = new Character[subs.Count];
						
						int count = DataHolder.StatusValueCount;
						
						foreach(Hashtable val in subs)
						{
							if(val[XMLHandler.NODE_NAME] as string == CharacterData.CHARACTER)
							{
								// old attack conversion
								BaseAttack[] tmpAtk = new BaseAttack[0];
								
								int i = int.Parse((string)val["id"]);
								icon[i] = "";
								
								character[i] = new Character();
								character[i].currentClass = int.Parse((string)val["class"]);
								if(val.ContainsKey("startlevel"))
								{
									character[i].development.startLevel = int.Parse((string)val["startlevel"]);
								}
								if(val.ContainsKey("maxlevel"))
								{
									character[i].development.maxLevel = int.Parse((string)val["maxlevel"]);
									character[i].development.Init(count);
								}
								character[i].baseCounter = int.Parse((string)val["basecounter"]);
								if(val.ContainsKey("basecritical"))
								{
									character[i].baseCritical = int.Parse((string)val["basecritical"]);
								}
								if(val.ContainsKey("baseblock"))
								{
									character[i].baseBlock = int.Parse((string)val["baseblock"]);
								}
								
								if(val.ContainsKey("raceid"))
								{
									character[i].raceID = int.Parse((string)val["raceid"]);
								}
								if(val.ContainsKey("sizeid"))
								{
									character[i].sizeID = int.Parse((string)val["sizeid"]);
								}
								
								character[i].baseElement = int.Parse((string)val["baseelement"]);
								
								if(val.ContainsKey("attacks"))
								{
									character[i].baseAttack = new int[int.Parse((string)val["attacks"])];
								}
								else
								{
									// old attack conversion
									if(val.ContainsKey("baseattacks"))
									{
										character[i].baseAttack = new int[int.Parse((string)val["baseattacks"])];
										tmpAtk = new BaseAttack[character[i].baseAttack.Length];
										for(int j=0; j<tmpAtk.Length; j++)
										{
											tmpAtk[j] = new BaseAttack(count);
										}
									}
									else
									{
										tmpAtk = new BaseAttack[1];
										tmpAtk[0] = new BaseAttack(count);
										tmpAtk[0].SetData(val);
									}
								}
								
								if(val.ContainsKey("skills"))
								{
									character[i].development.skill = new SkillLearn[int.Parse((string)val["skills"])];
									for(int j=0; j<character[i].development.skill.Length; j++)
									{
										character[i].development.skill[j] = new SkillLearn();
									}
								}
								
								if(val.ContainsKey("norevive")) character[i].noRevive = true;
								if(val.ContainsKey("leaveondeath")) character[i].leaveOnDeath = true;
								if(val.ContainsKey("movespeed"))
								{
									character[i].moveSpeed = float.Parse((string)val["movespeed"]);
								}
								if(val.ContainsKey("movespeedformula"))
								{
									character[i].useMoveSpeedFormula = true;
									character[i].moveSpeedFormula = int.Parse((string)val["movespeedformula"]);
								}
								if(val.ContainsKey("minmovespeed"))
								{
									character[i].minMoveSpeed = float.Parse((string)val["minmovespeed"]);
								}
								
								if(val.ContainsKey("aicontrolled"))
								{
									character[i].aiControlled = bool.Parse((string)val["aicontrolled"]);
									if(val.ContainsKey("attackpartytarget")) character[i].attackPartyTarget = true;
									if(val.ContainsKey("attacklasttarget")) character[i].attackLastTarget = true;
									if(val.ContainsKey("ainearesttarget")) character[i].aiNearestTarget = true;
									if(val.ContainsKey("aitimeout")) character[i].aiTimeout = float.Parse((string)val["aitimeout"]);
								}
								if(val.ContainsKey("ais"))
								{
									character[i].aiBehaviour = new AIBehaviour[int.Parse((string)val["ais"])];
									for(int j=0; j<character[i].aiBehaviour.Length; j++)
									{
										character[i].aiBehaviour[j] = new AIBehaviour();
									}
								}
								
								if(val.ContainsKey("fieldstatuschanges"))
								{
									character[i].fieldStatusChange = new StatusTimeChange[int.Parse((string)val["fieldstatuschanges"])];
									for(int j=0; j<character[i].fieldStatusChange.Length; j++)
									{
										character[i].fieldStatusChange[j] = new StatusTimeChange();
									}
								}
								if(val.ContainsKey("battlestatuschanges"))
								{
									character[i].battleStatusChange = new StatusTimeChange[int.Parse((string)val["battlestatuschanges"])];
									for(int j=0; j<character[i].battleStatusChange.Length; j++)
									{
										character[i].battleStatusChange[j] = new StatusTimeChange();
									}
								}
								
								ArrayList s = val[XMLHandler.NODES] as ArrayList;
								foreach(Hashtable ht in s)
								{
									this.LoadLanguages(ht, i, subs.Count);
									if(ht[XMLHandler.NODE_NAME] as string == CharacterData.PREFAB)
									{
										character[i].prefabName = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == CharacterData.PREFABROOT)
									{
										character[i].prefabRoot = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == CharacterData.STATUSVALUE)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < character[i].development.statusValue.Length)
										{
											for(int j=0; j<character[i].development.maxLevel; j++)
											{
												character[i].development.statusValue[id].levelValue[j] = int.Parse((string)ht[j.ToString()]);
											}
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == CharacterData.SKILL)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < character[i].development.skill.Length)
										{
											character[i].development.skill[id].SetData(ht, false);
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == CharacterData.ANIMATIONS)
									{
										character[i].battleAnimations = new BattleAnimationSettings(ht);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == CharacterData.AUDIOCLIP)
									{
										character[i].audioClipName[int.Parse((string)ht["id"])] = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == CharacterData.AUTOATTACK)
									{
										character[i].autoAttack.SetData(ht);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == CharacterData.AIMOVER)
									{
										character[i].aiMoverSettings.SetData(ht);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == CharacterData.AIBEHAVIOUR)
									{
										int j = int.Parse((string)ht["id"]);
										if(j < character[i].aiBehaviour.Length)
										{
											character[i].aiBehaviour[j].SetData(ht);
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == CharacterData.BASEATTACK)
									{
										int j = int.Parse((string)ht["id"]);
										if(j < tmpAtk.Length)
										{
											tmpAtk[j].SetData(ht);
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == CharacterData.CONTROLMAP)
									{
										character[i].controlMap.SetData(ht);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == CharacterData.FIELDANIMATIONS)
									{
										character[i].fieldAnimations.SetData(ht);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == CharacterData.BONUSSETTINGS)
									{
										character[i].bonus.SetData(ht);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == CharacterData.ATTACK)
									{
										int j = int.Parse((string)ht["id"]);
										if(j < character[i].baseAttack.Length)
										{
											character[i].baseAttack[j] = int.Parse((string)ht["id2"]);
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == CharacterData.DEVELOPMENT)
									{
										character[i].development.SetData(ht);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == CharacterData.CUSTOMANIMATIONS)
									{
										character[i].customAnimations.SetData(ht);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == CharacterData.FIELDSTATUSCHANGE)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < character[i].fieldStatusChange.Length)
										{
											character[i].fieldStatusChange[id].SetData(ht);
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == CharacterData.BATTLESTATUSCHANGE)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < character[i].battleStatusChange.Length)
										{
											character[i].battleStatusChange[id].SetData(ht);
										}
									}
								}
								
								// old attack import
								if(tmpAtk.Length > 0)
								{
									character[i].baseAttack = DataHolder.BaseAttacks().ImportOldAttack(tmpAtk, "Character");
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
		
		sv.Add(XMLHandler.NODE_NAME, CharacterData.CHARACTERS);
		
		for(int i=0; i<name[0].Count(); i++)
		{
			Hashtable ht = new Hashtable();
			ArrayList s = new ArrayList();
			
			ht.Add(XMLHandler.NODE_NAME, CharacterData.CHARACTER);
			ht.Add("id", i.ToString());
			ht.Add("raceid", character[i].raceID.ToString());
			ht.Add("sizeid", character[i].sizeID.ToString());
			ht.Add("class", character[i].currentClass.ToString());
			ht.Add("basecounter", character[i].baseCounter.ToString());
			ht.Add("basecritical", character[i].baseCritical.ToString());
			ht.Add("baseblock", character[i].baseBlock.ToString());
			
			if(character[i].noRevive) ht.Add("norevive", "true");
			if(character[i].leaveOnDeath) ht.Add("leaveondeath", "true");
			if(character[i].useMoveSpeedFormula)
			{
				ht.Add("movespeedformula", character[i].moveSpeedFormula.ToString());
			}
			else if(character[i].moveSpeed != 5)
			{
				ht.Add("movespeed", character[i].moveSpeed.ToString());
			}
			if(character[i].minMoveSpeed != 1)
			{
				ht.Add("minmovespeed", character[i].minMoveSpeed.ToString());
			}
			
			if("" != character[i].prefabName)
			{
				s.Add(HashtableHelper.GetContentHashtable(
						CharacterData.PREFAB, character[i].prefabName));
			}
			
			if("" != character[i].prefabRoot)
			{
				s.Add(HashtableHelper.GetContentHashtable(
						CharacterData.PREFABROOT, character[i].prefabRoot));
			}
			
			Hashtable anims = new Hashtable();
			anims.Add(XMLHandler.NODE_NAME, CharacterData.ANIMATIONS);
			anims = character[i].battleAnimations.GetData(anims);
			s.Add(anims);
			
			s = this.SaveLanguages(s, i);
			
			for(int j=0; j<character[i].audioClipName.Length; j++)
			{
				if(character[i].audioClipName[j] != null && character[i].audioClipName[j] != "")
				{
					s.Add(HashtableHelper.GetContentHashtable(
							CharacterData.AUDIOCLIP, character[i].audioClipName[j], j));
				}
			}
			
			if(character[i].autoAttack.active)
			{
				s.Add(character[i].autoAttack.GetData(HashtableHelper.GetTitleHashtable(
						CharacterData.AUTOATTACK)));
			}
			
			if(character[i].aiMoverSettings.useAIMover)
			{
				s.Add(character[i].aiMoverSettings.GetData(HashtableHelper.GetTitleHashtable(
						CharacterData.AIMOVER)));
			}
			
			if(character[i].aiControlled)
			{
				ht.Add("aicontrolled", "true");
				if(character[i].attackPartyTarget) ht.Add("attackpartytarget", "true");
				if(character[i].attackLastTarget) ht.Add("attacklasttarget", "true");
				if(character[i].aiNearestTarget) ht.Add("ainearesttarget", "true");
				if(character[i].aiTimeout > 0) ht.Add("aitimeout", character[i].aiTimeout.ToString());
			}
			if(character[i].aiBehaviour.Length > 0)
			{
				ht.Add("ais", character[i].aiBehaviour.Length.ToString());
				for(int j=0; j<character[i].aiBehaviour.Length; j++)
				{
					s.Add(character[i].aiBehaviour[j].GetData(
							HashtableHelper.GetTitleHashtable(CharacterData.AIBEHAVIOUR, j)));
				}
			}
			
			ht.Add("baseelement", character[i].baseElement.ToString());
			ht.Add("attacks", character[i].baseAttack.Length.ToString());
			for(int j=0; j<character[i].baseAttack.Length; j++)
			{
				Hashtable ht2 = HashtableHelper.GetTitleHashtable(CharacterData.ATTACK, j);
				ht2.Add("id2", character[i].baseAttack[j].ToString());
				s.Add(ht2);
			}
			
			s.Add(character[i].controlMap.GetData(
					HashtableHelper.GetTitleHashtable(CharacterData.CONTROLMAP)));
			
			s.Add(character[i].fieldAnimations.GetData(
					HashtableHelper.GetTitleHashtable(CharacterData.FIELDANIMATIONS)));
			
			s.Add(character[i].bonus.GetData(
					HashtableHelper.GetTitleHashtable(CharacterData.BONUSSETTINGS)));
			
			s.Add(character[i].development.GetData(
					HashtableHelper.GetTitleHashtable(CharacterData.DEVELOPMENT), true));
			
			s.Add(character[i].customAnimations.GetData(
					HashtableHelper.GetTitleHashtable(CharacterData.CUSTOMANIMATIONS)));
			
			if(character[i].fieldStatusChange.Length > 0)
			{
				ht.Add("fieldstatuschanges", character[i].fieldStatusChange.Length.ToString());
				for(int j=0; j<character[i].fieldStatusChange.Length; j++)
				{
					s.Add(character[i].fieldStatusChange[j].GetData(
							HashtableHelper.GetTitleHashtable(CharacterData.FIELDSTATUSCHANGE, j)));
				}
			}
			if(character[i].battleStatusChange.Length > 0)
			{
				ht.Add("battlestatuschanges", character[i].battleStatusChange.Length.ToString());
				for(int j=0; j<character[i].battleStatusChange.Length; j++)
				{
					s.Add(character[i].battleStatusChange[j].GetData(
							HashtableHelper.GetTitleHashtable(CharacterData.BATTLESTATUSCHANGE, j)));
				}
			}
			
			ht.Add(XMLHandler.NODES, s);
			subs.Add(ht);
		}
		sv.Add(XMLHandler.NODES, subs);
		data.Add(sv);
		XMLHandler.SaveXML(dir, filename, data);
	}
	
	public void AddCharacter(string n, string d, int count, StatusValue[] value)
	{
		base.AddBaseData(n, d, count);
		character = ArrayHelper.Add(new Character(), character);
		character[character.Length-1].development.BaseInit(value);
	}
	
	public override void RemoveData(int index)
	{
		base.RemoveData(index);
		character = ArrayHelper.Remove(index, character);
	}
	
	public Character GetCopy(int index)
	{
		Character c = new Character();
		c.realID = index;
		c.raceID = character[index].raceID;
		c.sizeID = character[index].sizeID;
		c.prefabName = character[index].prefabName;
		c.prefabRoot = character[index].prefabRoot;
		c.currentClass = character[index].currentClass;
		c.baseCounter = character[index].baseCounter;
		c.baseCritical = character[index].baseCritical;
		c.baseBlock = character[index].baseBlock;
		c.noRevive = character[index].noRevive;
		c.leaveOnDeath = character[index].leaveOnDeath;
		c.useMoveSpeedFormula = character[index].useMoveSpeedFormula;
		c.moveSpeedFormula = character[index].moveSpeedFormula;
		c.moveSpeed = character[index].moveSpeed;
		c.minMoveSpeed = character[index].minMoveSpeed;
		
		c.development = character[index].development.GetCopy();
		
		// animations
		c.battleAnimations = new BattleAnimationSettings(character[index].battleAnimations.GetData(new Hashtable()));
		
		for(int i=0; i<c.audioClipName.Length; i++)
		{
			c.audioClipName[i] = character[index].audioClipName[i];
		}
		
		c.autoAttack.SetData(character[index].autoAttack.GetData(new Hashtable()));
		c.aiMoverSettings.SetData(character[index].aiMoverSettings.GetData(new Hashtable()));
		
		c.aiControlled = character[index].aiControlled;
		c.attackPartyTarget = character[index].attackPartyTarget;
		c.attackLastTarget = character[index].attackLastTarget;
		c.aiNearestTarget = character[index].aiNearestTarget;
		c.aiTimeout = character[index].aiTimeout;
		c.aiBehaviour = new AIBehaviour[character[index].aiBehaviour.Length];
		for(int i=0; i<character[index].aiBehaviour.Length; i++)
		{
			c.aiBehaviour[i] = new AIBehaviour();
			c.aiBehaviour[i].SetData(character[index].aiBehaviour[i].GetData(new Hashtable()));
		}
		
		c.baseElement = character[index].baseElement;
		c.baseAttack = new int[character[index].baseAttack.Length];
		for(int i=0; i<character[index].baseAttack.Length; i++)
		{
			c.baseAttack[i] = character[index].baseAttack[i];
		}
		
		c.controlMap.SetData(character[index].controlMap.GetData(new Hashtable()));
		c.fieldAnimations.SetData(character[index].fieldAnimations.GetData(new Hashtable()));
		c.customAnimations.SetData(character[index].customAnimations.GetData(new Hashtable()));
		c.bonus.SetData(character[index].bonus.GetData(new Hashtable()));
		
		c.fieldStatusChange = new StatusTimeChange[character[index].fieldStatusChange.Length];
		for(int i=0; i<c.fieldStatusChange.Length; i++)
		{
			c.fieldStatusChange[i] = new StatusTimeChange();
			c.fieldStatusChange[i].SetData(character[index].fieldStatusChange[i].GetData(new Hashtable()));
		}
		c.battleStatusChange = new StatusTimeChange[character[index].battleStatusChange.Length];
		for(int i=0; i<c.battleStatusChange.Length; i++)
		{
			c.battleStatusChange[i] = new StatusTimeChange();
			c.battleStatusChange[i].SetData(character[index].battleStatusChange[i].GetData(new Hashtable()));
		}
		
		return c;
	}
	
	public override void Copy(int index)
	{
		base.Copy(index);
		character = ArrayHelper.Add(this.GetCopy(index), character);
	}
	
	public void AddStatusValue(int index, StatusValue val)
	{
		for(int i=0; i<character.Length; i++)
		{
			character[i].development.AddStatusValue(index, val);
			character[i].bonus.AddStatusValue();
		}
	}
	
	public void SetStatusValueType(int index, StatusValueType type, StatusValue val)
	{
		for(int i=0; i<character.Length; i++)
		{
			character[i].development.SetStatusValueType(index, type, val);
			character[i].bonus.SetStatusValueType(index, type);
			if(!StatusValueType.CONSUMABLE.Equals(type))
			{
				this.RemoveStatusTimeChange(i, index);
			}
		}
	}
	
	public void RemoveStatusValue(int index)
	{
		for(int i=0; i<character.Length; i++)
		{
			character[i].development.RemoveStatusValue(index);
			character[i].bonus.RemoveStatusValue(index);
			this.RemoveStatusTimeChange(i, index);
		}
	}
	
	private void RemoveStatusTimeChange(int i, int index)
	{
		for(int j=0; j<character[i].fieldStatusChange.Length; j++)
		{
			if(character[i].fieldStatusChange[j].statusID == index)
			{
				character[i].fieldStatusChange = ArrayHelper.Remove(index, character[i].fieldStatusChange);
				j--;
			}
		}
		for(int j=0; j<character[i].fieldStatusChange.Length; j++)
		{
			if(character[i].fieldStatusChange[i].statusID > index)
			{
				character[i].fieldStatusChange[i].statusID--;
			}
		}
		for(int j=0; j<character[i].battleStatusChange.Length; j++)
		{
			if(character[i].battleStatusChange[j].statusID == index)
			{
				character[i].battleStatusChange = ArrayHelper.Remove(index, character[i].battleStatusChange);
				j--;
			}
		}
		for(int j=0; j<character[i].battleStatusChange.Length; j++)
		{
			if(character[i].battleStatusChange[i].statusID > index)
			{
				character[i].battleStatusChange[i].statusID--;
			}
		}
	}
	
	public void StatusValueMinMaxChanged(int index, int min, int max)
	{
		for(int i=0; i<character.Length; i++)
		{
			character[i].development.StatusValueMinMaxChanged(index, min, max);
		}
	}
	
	public void RemoveClass(int index)
	{
		for(int i=0; i<character.Length; i++)
		{
			character[i].currentClass = this.CheckForIndex(index, character[i].currentClass);
		}
	}
	
	public void RemoveFormula(int index)
	{
		for(int i=0; i<character.Length; i++)
		{
			character[i].baseCounter = this.CheckForIndex(index, character[i].baseCounter);
			character[i].baseCritical = this.CheckForIndex(index, character[i].baseCritical);
			character[i].baseBlock = this.CheckForIndex(index, character[i].baseBlock);
			character[i].moveSpeedFormula = this.CheckForIndex(index, character[i].moveSpeedFormula);
			for(int j=0; j<character[i].fieldStatusChange.Length; j++)
			{
				if(character[i].fieldStatusChange[j].useFormula)
				{
					character[i].fieldStatusChange[j].formulaID = this.CheckForIndex(index, character[i].fieldStatusChange[j].formulaID);
				}
			}
			for(int j=0; j<character[i].battleStatusChange.Length; j++)
			{
				if(character[i].battleStatusChange[j].useFormula)
				{
					character[i].battleStatusChange[j].formulaID = this.CheckForIndex(index, character[i].battleStatusChange[j].formulaID);
				}
			}
		}
	}
	
	public void RemoveSkill(int index)
	{
		for(int i=0; i<character.Length; i++)
		{
			for(int j=0; j<character[i].development.skill.Length; j++)
			{
				character[i].development.skill[j].skillID = this.CheckForIndex(index, character[i].development.skill[j].skillID);
			}
		}
	}
	
	public void AddLearnSkill(int index)
	{
		character[index].development.skill = ArrayHelper.Add(new SkillLearn(), character[index].development.skill);
	}
	
	public void RemoveLearnSkill(int index, int s)
	{
		character[index].development.skill = ArrayHelper.Remove(s, character[index].development.skill);
	}
	
	public void RemoveBaseAttack(int index)
	{
		for(int i=0; i<character.Length; i++)
		{
			for(int j=0; j<character[i].baseAttack.Length; j++)
			{
				character[i].baseAttack[j] = this.CheckForIndex(index, character[i].baseAttack[j]);
			}
		}
	}
	
	public void AddElement()
	{
		for(int i=0; i<character.Length; i++)
		{
			character[i].bonus.AddElement();
		}
	}
	
	public void RemoveElement(int index)
	{
		for(int i=0; i<character.Length; i++)
		{
			character[i].bonus.RemoveElement(index);
		}
	}
	
	public void AddRace()
	{
		for(int i=0; i<character.Length; i++)
		{
			character[i].bonus.AddRace();
		}
	}
	
	public void RemoveRace(int index)
	{
		for(int i=0; i<character.Length; i++)
		{
			character[i].bonus.RemoveRace(index);
			character[i].raceID = this.CheckForIndex(index, character[i].raceID);
		}
	}
	
	public void AddSize()
	{
		for(int i=0; i<character.Length; i++)
		{
			character[i].bonus.AddSize();
		}
	}
	
	public void RemoveSize(int index)
	{
		for(int i=0; i<character.Length; i++)
		{
			character[i].bonus.RemoveSize(index);
			character[i].sizeID = this.CheckForIndex(index, character[i].sizeID);
		}
	}
	
	public void RemoveDifficulty(int index)
	{
		for(int i=0; i<character.Length; i++)
		{
			character[i].bonus.RemoveDifficulty(index);
			for(int j=0; j<character[i].aiBehaviour.Length; j++)
			{
				character[i].aiBehaviour[j].RemoveDifficulty(index);
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
				if((!this.filter.useFilter[0] || this.character[i].raceID == this.filter.filterID[0]) &&
					(!this.filter.useFilter[1] || this.character[i].sizeID == this.filter.filterID[1]))
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