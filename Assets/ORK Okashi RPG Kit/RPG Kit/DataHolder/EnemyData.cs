
using System.Collections;

public class EnemyData : BaseLangData
{
	public Enemy[] enemy = new Enemy[0];
	
	public static string PREFAB_PATH = "Prefabs/Enemies/";

	// XML data
	private string filename = "enemies";
	
	private static string ENEMIES = "enemies";
	private static string ENEMY = "enemy";
	private static string STATUSVALUE = "statusvalue";
	private static string EFFECT = "effect";
	private static string ELEMENT = "element";
	private static string ITEMDROP = "itemdrop";
	private static string AICONDITION = "aicondition";
	private static string ANIMATIONS = "animations";
	private static string PREFAB = "prefab";
	private static string PREFABROOT = "prefabroot";
	private static string AUDIOCLIP = "audioclip";
	private static string AIMOVER = "aimover";
	private static string AIBEHAVIOUR = "aibehaviour";
	private static string BASEATTACK = "baseattack";
	private static string ATTACK = "attack";
	private static string FIELDANIMATIONS = "fieldanimations";
	private static string VARIABLES = "variables";
	private static string BONUSSETTINGS = "bonussettings";
	private static string RACE = "race";
	private static string SIZE = "size";
	private static string CUSTOMANIMATIONS = "customanimations";
	private static string BATTLESTATUSCHANGE = "battlestatuschange";

	public EnemyData()
	{
		this.filter = new DataFilter(2);
		LoadData();
	}
	
	public override string GetIconPath() { return "Icons/Enemy/"; }
	
	public void LoadData()
	{
		ArrayList data = XMLHandler.LoadXML(dir+filename);
		
		if(data.Count > 0)
		{
			foreach(Hashtable entry in data)
			{
				if(entry[XMLHandler.NODE_NAME] as string == EnemyData.ENEMIES)
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList subs = entry[XMLHandler.NODES] as ArrayList;
						icon = new string[subs.Count];
						enemy = new Enemy[subs.Count];
						
						foreach(Hashtable val in subs)
						{
							if(val[XMLHandler.NODE_NAME] as string == EnemyData.ENEMY)
							{
								// old attack conversion
								BaseAttack[] tmpAtk = new BaseAttack[0];
								
								int i = int.Parse((string)val["id"]);
								icon[i] = "";
								
								enemy[i] = new Enemy();
								enemy[i].level = int.Parse((string)val["level"]);
								if(val.ContainsKey("classlevel")) enemy[i].classLevel = int.Parse((string)val["classlevel"]);
								enemy[i].money = int.Parse((string)val["money"]);
								int values = int.Parse((string)val["values"]);
								enemy[i].value = new int[values];
								enemy[i].skillEffect = new SkillEffect[int.Parse((string)val["effects"])];
								enemy[i].itemDrop = new ItemDrop[int.Parse((string)val["drops"])];
								enemy[i].baseCounter = int.Parse((string)val["basecounter"]);
								if(val.ContainsKey("basecritical"))
								{
									enemy[i].baseCritical = int.Parse((string)val["basecritical"]);
								}
								if(val.ContainsKey("baseblock"))
								{
									enemy[i].baseBlock = int.Parse((string)val["baseblock"]);
								}
								
								if(val.ContainsKey("raceid"))
								{
									enemy[i].raceID = int.Parse((string)val["raceid"]);
								}
								if(val.ContainsKey("sizeid"))
								{
									enemy[i].sizeID = int.Parse((string)val["sizeid"]);
								}
								
								int count;
								if(val.ContainsKey("races"))
								{
									count = int.Parse((string)val["races"]);
								}
								else count = DataHolder.Races().GetDataCount();
								enemy[i].raceValue = new int[count];
								for(int j=0; j<count; j++)
								{
									enemy[i].raceValue[j] = 100;
								}
								if(val.ContainsKey("sizes"))
								{
									count = int.Parse((string)val["sizes"]);
								}
								else count = DataHolder.Sizes().GetDataCount();
								enemy[i].sizeValue = new int[count];
								for(int j=0; j<count; j++)
								{
									enemy[i].sizeValue[j] = 100;
								}
								
								if(val.ContainsKey("counter"))
								{
									enemy[i].bonus.counterBonus = int.Parse((string)val["counter"]);
								}
								if(val.ContainsKey("escape"))
								{
									enemy[i].bonus.escapeBonus = int.Parse((string)val["escape"]);
								}
								if(val.ContainsKey("hitbonus"))
								{
									enemy[i].bonus.hitBonus = int.Parse((string)val["hitbonus"]);
								}
								
								if(val.ContainsKey("isaggressive"))
								{
									enemy[i].aggressiveType = AggressiveType.ALWAYS;
								}
								if(val.ContainsKey("aggressivetype"))
								{
									enemy[i].aggressiveType = (AggressiveType)System.Enum.Parse(
											typeof(AggressiveType), (string)val["aggressivetype"]);
								}
								
								enemy[i].baseElement = int.Parse((string)val["baseelement"]);
								
								if(val.ContainsKey("attacks"))
								{
									enemy[i].baseAttack = new int[int.Parse((string)val["attacks"])];
								}
								else
								{
									// old attack conversion
									if(val.ContainsKey("baseattacks"))
									{
										enemy[i].baseAttack = new int[int.Parse((string)val["baseattacks"])];
										tmpAtk = new BaseAttack[enemy[i].baseAttack.Length];
										for(int j=0; j<tmpAtk.Length; j++)
										{
											tmpAtk[j] = new BaseAttack(values);
										}
									}
									else
									{
										tmpAtk = new BaseAttack[1];
										tmpAtk[0] = new BaseAttack(values);
										tmpAtk[0].SetData(val);
									}
								}
								
								count = int.Parse((string)val["elements"]);
								enemy[i].elementValue = new int[count];
								for(int j=0; j<count; j++)
								{
									enemy[i].elementValue[j] = 100;
								}
								
								if(val.ContainsKey("ais"))
								{
									enemy[i].aiBehaviour = new AIBehaviour[int.Parse((string)val["ais"])];
									for(int j=0; j<enemy[i].aiBehaviour.Length; j++)
									{
										enemy[i].aiBehaviour[j] = new AIBehaviour();
									}
								}
								
								if(val.ContainsKey("movespeed"))
								{
									enemy[i].moveSpeed = float.Parse((string)val["movespeed"]);
								}
								if(val.ContainsKey("movespeedformula"))
								{
									enemy[i].useMoveSpeedFormula = true;
									enemy[i].moveSpeedFormula = int.Parse((string)val["movespeedformula"]);
								}
								if(val.ContainsKey("minmovespeed"))
								{
									enemy[i].minMoveSpeed = float.Parse((string)val["minmovespeed"]);
								}
								if(val.ContainsKey("attacklasttarget")) enemy[i].attackLastTarget = true;
								if(val.ContainsKey("ainearesttarget")) enemy[i].aiNearestTarget = true;
								if(val.ContainsKey("aitimeout")) enemy[i].aiTimeout = float.Parse((string)val["aitimeout"]);
								
								if(val.ContainsKey("stealitem"))
								{
									enemy[i].stealItem = true;
									enemy[i].stealItemFactor = float.Parse((string)val["stealitemfactor"]);
									enemy[i].stealItemID = int.Parse((string)val["stealitem"]);
									enemy[i].stealItemType = (ItemDropType)System.Enum.Parse(
											typeof(ItemDropType), (string)val["stealitemtype"]);
									if(val.ContainsKey("stealitemonce"))
									{
										enemy[i].stealItemOnce = true;
									}
								}
								if(val.ContainsKey("stealmoney"))
								{
									enemy[i].stealMoney = true;
									enemy[i].stealMoneyFactor = float.Parse((string)val["stealmoneyfactor"]);
									enemy[i].stealMoneyAmount = int.Parse((string)val["stealmoney"]);
									if(val.ContainsKey("stealmoneyonce"))
									{
										enemy[i].stealMoneyOnce = true;
									}
								}
								
								if(val.ContainsKey("battlestatuschanges"))
								{
									enemy[i].battleStatusChange = new StatusTimeChange[int.Parse((string)val["battlestatuschanges"])];
									for(int j=0; j<enemy[i].battleStatusChange.Length; j++)
									{
										enemy[i].battleStatusChange[j] = new StatusTimeChange();
									}
								}
								
								ArrayList s = val[XMLHandler.NODES] as ArrayList;
								foreach(Hashtable ht in s)
								{
									this.LoadLanguages(ht, i, subs.Count);
									if(ht[XMLHandler.NODE_NAME] as string == EnemyData.PREFAB)
									{
										enemy[i].prefabName = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == EnemyData.PREFABROOT)
									{
										enemy[i].prefabRoot = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == EnemyData.STATUSVALUE)
									{
										enemy[i].value[int.Parse((string)ht["id"])] = int.Parse((string)ht["value"]);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == EnemyData.EFFECT)
									{
										enemy[i].skillEffect[int.Parse((string)ht["id"])] = (SkillEffect)System.Enum.Parse(typeof(SkillEffect), (string)ht["value"]);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == EnemyData.ELEMENT)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < enemy[i].elementValue.Length)
										{
											enemy[i].elementValue[id] = int.Parse((string)ht["value"]);
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == EnemyData.ITEMDROP)
									{
										int j = int.Parse((string)ht["id"]);
										enemy[i].itemDrop[j] = new ItemDrop();
										enemy[i].itemDrop[j].type = (ItemDropType)System.Enum.Parse(typeof(ItemDropType), (string)ht["type"]);
										enemy[i].itemDrop[j].itemID = int.Parse((string)ht["item"]);
										if(ht.ContainsKey("quantity")) enemy[i].itemDrop[j].quantity = int.Parse((string)ht["quantity"]);
										enemy[i].itemDrop[j].chance = float.Parse((string)ht["chance"]);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == EnemyData.AICONDITION)
									{
										int j = int.Parse((string)ht["id"]);
										if(j < enemy[i].aiBehaviour.Length)
										{
											enemy[i].aiBehaviour[j].battleAI = int.Parse((string)ht["ai"]);
											enemy[i].aiBehaviour[j].attackSelection[0] = (AttackSelection)
													System.Enum.Parse(typeof(AttackSelection), (string)ht["attackselection"]);
											if(ht.ContainsKey("skill")) enemy[i].aiBehaviour[j].useID[0] = int.Parse((string)ht["skill"]);
											if(ht.ContainsKey("slvl")) enemy[i].aiBehaviour[j].useLevel[0] = int.Parse((string)ht["slvl"]);
											if(ht.ContainsKey("item")) enemy[i].aiBehaviour[j].useID[0] = int.Parse((string)ht["item"]);
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == EnemyData.ANIMATIONS)
									{
										enemy[i].battleAnimations = new BattleAnimationSettings(ht);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == EnemyData.AUDIOCLIP)
									{
										enemy[i].audioClipName[int.Parse((string)ht["id"])] = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == EnemyData.AIMOVER)
									{
										enemy[i].aiMoverSettings.SetData(ht);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == EnemyData.AIBEHAVIOUR)
									{
										int j = int.Parse((string)ht["id"]);
										if(j < enemy[i].aiBehaviour.Length)
										{
											enemy[i].aiBehaviour[j].SetData(ht);
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == EnemyData.BASEATTACK)
									{
										int j = int.Parse((string)ht["id"]);
										if(j < tmpAtk.Length)
										{
											tmpAtk[j].SetData(ht);
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == EnemyData.FIELDANIMATIONS)
									{
										enemy[i].fieldAnimations.SetData(ht);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == EnemyData.VARIABLES)
									{
										enemy[i].variables.SetData(ht);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == EnemyData.BONUSSETTINGS)
									{
										enemy[i].bonus.SetData(ht);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == EnemyData.ATTACK)
									{
										int j = int.Parse((string)ht["id"]);
										if(j < enemy[i].baseAttack.Length)
										{
											enemy[i].baseAttack[j] = int.Parse((string)ht["id2"]);;
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == EnemyData.RACE)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < enemy[i].raceValue.Length)
										{
											enemy[i].raceValue[id] = int.Parse((string)ht["value"]);
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == EnemyData.SIZE)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < enemy[i].sizeValue.Length)
										{
											enemy[i].sizeValue[id] = int.Parse((string)ht["value"]);
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == EnemyData.CUSTOMANIMATIONS)
									{
										enemy[i].customAnimations.SetData(ht);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == EnemyData.BATTLESTATUSCHANGE)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < enemy[i].battleStatusChange.Length)
										{
											enemy[i].battleStatusChange[id].SetData(ht);
										}
									}
								}
								
								// old attack import
								if(tmpAtk.Length > 0)
								{
									enemy[i].baseAttack = DataHolder.BaseAttacks().ImportOldAttack(tmpAtk, "Enemy");
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
		
		sv.Add(XMLHandler.NODE_NAME, EnemyData.ENEMIES);
		
		for(int i=0; i<name[0].Count(); i++)
		{
			Hashtable ht = new Hashtable();
			ArrayList s = new ArrayList();
			
			ht.Add(XMLHandler.NODE_NAME, EnemyData.ENEMY);
			ht.Add("id", i.ToString());
			ht.Add("raceid", enemy[i].raceID.ToString());
			ht.Add("sizeid", enemy[i].sizeID.ToString());
			
			if("" != enemy[i].prefabName)
			{
				s.Add(HashtableHelper.GetContentHashtable(
						EnemyData.PREFAB, enemy[i].prefabName));
			}
			
			if("" != enemy[i].prefabRoot)
			{
				s.Add(HashtableHelper.GetContentHashtable(
						EnemyData.PREFABROOT, enemy[i].prefabRoot));
			}
			
			ht.Add("level", enemy[i].level.ToString());
			ht.Add("classlevel", enemy[i].classLevel.ToString());
			ht.Add("money", enemy[i].money.ToString());
			
			ht.Add("basecounter", enemy[i].baseCounter.ToString());
			ht.Add("basecritical", enemy[i].baseCritical.ToString());
			ht.Add("baseblock", enemy[i].baseBlock.ToString());
			ht.Add("aggressivetype", enemy[i].aggressiveType.ToString());
			
			if(enemy[i].useMoveSpeedFormula)
			{
				ht.Add("movespeedformula", enemy[i].moveSpeedFormula.ToString());
			}
			else if(enemy[i].moveSpeed != 5)
			{
				ht.Add("movespeed", enemy[i].moveSpeed.ToString());
			}
			if(enemy[i].minMoveSpeed != 1)
			{
				ht.Add("minmovespeed", enemy[i].minMoveSpeed.ToString());
			}
			
			if(enemy[i].stealItem)
			{
				ht.Add("stealitemfactor", enemy[i].stealItemFactor.ToString());
				ht.Add("stealitem", enemy[i].stealItemID.ToString());
				ht.Add("stealitemtype", enemy[i].stealItemType.ToString());
				if(enemy[i].stealItemOnce)
				{
					ht.Add("stealitemonce", "true");
				}
			}
			if(enemy[i].stealMoney)
			{
				ht.Add("stealmoneyfactor", enemy[i].stealMoneyFactor.ToString());
				ht.Add("stealmoney", enemy[i].stealMoneyAmount.ToString());
				if(enemy[i].stealMoneyOnce)
				{
					ht.Add("stealmoneyonce", "true");
				}
			}
			
			Hashtable anims = new Hashtable();
			anims.Add(XMLHandler.NODE_NAME, EnemyData.ANIMATIONS);
			anims = enemy[i].battleAnimations.GetData(anims);
			s.Add(anims);
			
			s = this.SaveLanguages(s, i);
			
			ht.Add("values", enemy[i].value.Length.ToString());
			for(int j=0; j<enemy[i].value.Length; j++)
			{
				Hashtable val = new Hashtable();
				val.Add(XMLHandler.NODE_NAME, EnemyData.STATUSVALUE);
				val.Add("id", j.ToString());
				val.Add("value", enemy[i].value[j].ToString());
				s.Add(val);
			}
			
			ht.Add("effects", enemy[i].skillEffect.Length.ToString());
			for(int j=0; j<enemy[i].skillEffect.Length; j++)
			{
				if(enemy[i].skillEffect[j] > 0)
				{
					Hashtable e = new Hashtable();
					e.Add(XMLHandler.NODE_NAME, EnemyData.EFFECT);
					e.Add("id", j.ToString());
					e.Add("value", enemy[i].skillEffect[j].ToString());
					s.Add(e);
				}
			}
			
			ht.Add("elements", enemy[i].elementValue.Length.ToString());
			for(int j=0; j<enemy[i].elementValue.Length; j++)
			{
				if(enemy[i].elementValue[j] != 100)
				{
					Hashtable e = new Hashtable();
					e.Add(XMLHandler.NODE_NAME, EnemyData.ELEMENT);
					e.Add("id", j.ToString());
					e.Add("value", enemy[i].elementValue[j].ToString());
					s.Add(e);
				}
			}
			
			ht.Add("races", enemy[i].raceValue.Length.ToString());
			for(int j=0; j<enemy[i].raceValue.Length; j++)
			{
				if(enemy[i].raceValue[j] != 100)
				{
					Hashtable e = HashtableHelper.GetTitleHashtable(EnemyData.RACE, j);
					e.Add("value", enemy[i].raceValue[j].ToString());
					s.Add(e);
				}
			}
			
			ht.Add("sizes", enemy[i].sizeValue.Length.ToString());
			for(int j=0; j<enemy[i].sizeValue.Length; j++)
			{
				if(enemy[i].sizeValue[j] != 100)
				{
					Hashtable e = HashtableHelper.GetTitleHashtable(EnemyData.SIZE, j);
					e.Add("value", enemy[i].sizeValue[j].ToString());
					s.Add(e);
				}
			}
			
			ht.Add("drops", enemy[i].itemDrop.Length.ToString());
			for(int j=0; j<enemy[i].itemDrop.Length; j++)
			{
				Hashtable d = new Hashtable();
				d.Add(XMLHandler.NODE_NAME, EnemyData.ITEMDROP);
				d.Add("id", j.ToString());
				d.Add("type", enemy[i].itemDrop[j].type.ToString());
				d.Add("item", enemy[i].itemDrop[j].itemID.ToString());
				if(enemy[i].itemDrop[j].quantity > 1) d.Add("quantity", enemy[i].itemDrop[j].quantity.ToString());
				d.Add("chance", enemy[i].itemDrop[j].chance.ToString());
				s.Add(d);
			}
			
			for(int j=0; j<enemy[i].audioClipName.Length; j++)
			{
				if(enemy[i].audioClipName[j] != null && enemy[i].audioClipName[j] != "")
				{
					s.Add(HashtableHelper.GetContentHashtable(
							EnemyData.AUDIOCLIP, enemy[i].audioClipName[j], j));
				}
			}
			
			if(enemy[i].aiMoverSettings.useAIMover)
			{
				s.Add(enemy[i].aiMoverSettings.GetData(HashtableHelper.GetTitleHashtable(
						EnemyData.AIMOVER)));
			}
			
			if(enemy[i].attackLastTarget) ht.Add("attacklasttarget", "true");
			if(enemy[i].aiNearestTarget) ht.Add("ainearesttarget", "true");
			if(enemy[i].aiTimeout > 0) ht.Add("aitimeout", enemy[i].aiTimeout.ToString());
			if(enemy[i].aiBehaviour.Length > 0)
			{
				ht.Add("ais", enemy[i].aiBehaviour.Length.ToString());
				for(int j=0; j<enemy[i].aiBehaviour.Length; j++)
				{
					s.Add(enemy[i].aiBehaviour[j].GetData(
							HashtableHelper.GetTitleHashtable(EnemyData.AIBEHAVIOUR, j)));
				}
			}
			
			ht.Add("baseelement", enemy[i].baseElement.ToString());
			ht.Add("attacks", enemy[i].baseAttack.Length.ToString());
			for(int j=0; j<enemy[i].baseAttack.Length; j++)
			{
				Hashtable ht2 = HashtableHelper.GetTitleHashtable(EnemyData.ATTACK, j);
				ht2.Add("id2", enemy[i].baseAttack[j].ToString());
				s.Add(ht2);
			}
			
			s.Add(enemy[i].fieldAnimations.GetData(
					HashtableHelper.GetTitleHashtable(EnemyData.FIELDANIMATIONS)));
			
			s.Add(enemy[i].variables.GetData(
					HashtableHelper.GetTitleHashtable(EnemyData.VARIABLES)));
			
			s.Add(enemy[i].bonus.GetData(
					HashtableHelper.GetTitleHashtable(EnemyData.BONUSSETTINGS)));
			
			s.Add(enemy[i].customAnimations.GetData(
					HashtableHelper.GetTitleHashtable(EnemyData.CUSTOMANIMATIONS)));
			
			if(enemy[i].battleStatusChange.Length > 0)
			{
				ht.Add("battlestatuschanges", enemy[i].battleStatusChange.Length.ToString());
				for(int j=0; j<enemy[i].battleStatusChange.Length; j++)
				{
					s.Add(enemy[i].battleStatusChange[j].GetData(
							HashtableHelper.GetTitleHashtable(EnemyData.BATTLESTATUSCHANGE, j)));
				}
			}
			
			ht.Add(XMLHandler.NODES, s);
			subs.Add(ht);
		}
		sv.Add(XMLHandler.NODES, subs);
		data.Add(sv);
		XMLHandler.SaveXML(dir, filename, data);
	}
	
	public void AddEnemy(string n, string d, int count, int seCount, int svCount, int elCount)
	{
		base.AddBaseData(n, d, count);
		enemy = ArrayHelper.Add(new Enemy(), enemy);
		
		enemy[enemy.Length-1].value = new int[svCount];
		enemy[enemy.Length-1].skillEffect = new SkillEffect[seCount];
		enemy[enemy.Length-1].elementValue = new int[elCount];
		for(int i=0; i<elCount; i++)
		{
			enemy[enemy.Length-1].elementValue[i] = 100;
		}
		enemy[enemy.Length-1].raceValue = new int[DataHolder.Races().GetDataCount()];
		for(int i=0; i<enemy[enemy.Length-1].raceValue.Length; i++)
		{
			enemy[enemy.Length-1].raceValue[i] = 100;
		}
		enemy[enemy.Length-1].sizeValue = new int[DataHolder.Sizes().GetDataCount()];
		for(int i=0; i<enemy[enemy.Length-1].sizeValue.Length; i++)
		{
			enemy[enemy.Length-1].sizeValue[i] = 100;
		}
	}
	
	public override void RemoveData(int index)
	{
		base.RemoveData(index);
		enemy = ArrayHelper.Remove(index, enemy);
	}
	
	public Enemy GetCopy(int index)
	{
		Enemy e = new Enemy();
		e.realID = index;
		e.raceID = enemy[index].raceID;
		e.sizeID = enemy[index].sizeID;
		e.prefabName = enemy[index].prefabName;
		e.prefabRoot = enemy[index].prefabRoot;
		e.level = enemy[index].level;
		e.classLevel = enemy[index].classLevel;
		e.money = enemy[index].money;
		e.baseCounter = enemy[index].baseCounter;
		e.baseCritical = enemy[index].baseCritical;
		e.baseBlock = enemy[index].baseBlock;
		e.useMoveSpeedFormula = enemy[index].useMoveSpeedFormula;
		e.moveSpeedFormula = enemy[index].moveSpeedFormula;
		e.moveSpeed = enemy[index].moveSpeed;
		e.minMoveSpeed = enemy[index].minMoveSpeed;
		e.aggressiveType = enemy[index].aggressiveType;
		e.stealItem = enemy[index].stealItem;
		e.stealItemFactor = enemy[index].stealItemFactor;
		e.stealItemType = enemy[index].stealItemType;
		e.stealItemID = enemy[index].stealItemID;
		e.stealItemOnce = enemy[index].stealItemOnce;
		e.stealMoney = enemy[index].stealMoney;
		e.stealMoneyFactor = enemy[index].stealMoneyFactor;
		e.stealMoneyAmount = enemy[index].stealMoneyAmount;
		e.stealMoneyOnce = enemy[index].stealMoneyOnce;
		
		e.value = new int[enemy[index].value.Length];
		for(int i=0; i<enemy[index].value.Length; i++)
		{
			e.value[i] = enemy[index].value[i];
		}
		
		e.skillEffect = new SkillEffect[enemy[index].skillEffect.Length];
		for(int i=0; i<enemy[index].skillEffect.Length; i++)
		{
			e.skillEffect[i] = enemy[index].skillEffect[i];
		}
		
		e.elementValue = new int[enemy[index].elementValue.Length];
		for(int i=0; i<enemy[index].elementValue.Length; i++)
		{
			e.elementValue[i] = enemy[index].elementValue[i];
		}
		
		e.raceValue = new int[enemy[index].raceValue.Length];
		for(int i=0; i<enemy[index].raceValue.Length; i++)
		{
			e.raceValue[i] = enemy[index].raceValue[i];
		}
		
		e.sizeValue = new int[enemy[index].sizeValue.Length];
		for(int i=0; i<enemy[index].sizeValue.Length; i++)
		{
			e.sizeValue[i] = enemy[index].sizeValue[i];
		}
		
		e.itemDrop = new ItemDrop[enemy[index].itemDrop.Length];
		for(int i=0; i<enemy[index].itemDrop.Length; i++)
		{
			e.itemDrop[i] = new ItemDrop();
			e.itemDrop[i].type = enemy[index].itemDrop[i].type;
			e.itemDrop[i].itemID = enemy[index].itemDrop[i].itemID;
			e.itemDrop[i].quantity = enemy[index].itemDrop[i].quantity;
			e.itemDrop[i].chance = enemy[index].itemDrop[i].chance;
		}
		
		// animations
		e.battleAnimations = new BattleAnimationSettings(enemy[index].battleAnimations.GetData(new Hashtable()));
		
		for(int i=0; i<e.audioClipName.Length; i++)
		{
			e.audioClipName[i] = enemy[index].audioClipName[i];
		}
		
		e.aiMoverSettings.SetData(enemy[index].aiMoverSettings.GetData(new Hashtable()));
		
		e.attackLastTarget = enemy[index].attackLastTarget;
		e.aiNearestTarget = enemy[index].aiNearestTarget;
		e.aiTimeout = enemy[index].aiTimeout;
		e.aiBehaviour = new AIBehaviour[enemy[index].aiBehaviour.Length];
		for(int i=0; i<enemy[index].aiBehaviour.Length; i++)
		{
			e.aiBehaviour[i] = new AIBehaviour();
			e.aiBehaviour[i].SetData(enemy[index].aiBehaviour[i].GetData(new Hashtable()));
		}
		
		e.baseElement = enemy[index].baseElement;
		e.baseAttack = new int[enemy[index].baseAttack.Length];
		for(int i=0; i<enemy[index].baseAttack.Length; i++)
		{
			e.baseAttack[i] = enemy[index].baseAttack[i];
		}
		
		e.fieldAnimations.SetData(enemy[index].fieldAnimations.GetData(new Hashtable()));
		e.customAnimations.SetData(enemy[index].customAnimations.GetData(new Hashtable()));
		e.variables.SetData(enemy[index].variables.GetData(new Hashtable()));
		e.bonus.SetData(enemy[index].bonus.GetData(new Hashtable()));
		
		e.battleStatusChange = new StatusTimeChange[enemy[index].battleStatusChange.Length];
		for(int i=0; i<e.battleStatusChange.Length; i++)
		{
			e.battleStatusChange[i] = new StatusTimeChange();
			e.battleStatusChange[i].SetData(enemy[index].battleStatusChange[i].GetData(new Hashtable()));
		}
		
		return e;
	}
	
	public override void Copy(int index)
	{
		base.Copy(index);
		enemy = ArrayHelper.Add(this.GetCopy(index), enemy);
	}
	
	public void AddStatusValue(int index)
	{
		for(int i=0; i<enemy.Length; i++)
		{
			enemy[i].value = ArrayHelper.Add(0, enemy[i].value);
			enemy[i].bonus.AddStatusValue();
		}
	}
	
	public void RemoveStatusValue(int index)
	{
		for(int i=0; i<enemy.Length; i++)
		{
			enemy[i].value = ArrayHelper.Remove(index, enemy[i].value);
			enemy[i].bonus.RemoveStatusValue(index);
			this.RemoveStatusTimeChange(i, index);
		}
	}
	
	public void SetStatusValueType(int index, StatusValueType type)
	{
		for(int i=0; i<enemy.Length; i++)
		{
			enemy[i].value[index] = 0;
			enemy[i].bonus.SetStatusValueType(index, type);
			if(!StatusValueType.CONSUMABLE.Equals(type))
			{
				this.RemoveStatusTimeChange(i, index);
			}
		}
	}
	
	private void RemoveStatusTimeChange(int i, int index)
	{
		for(int j=0; j<enemy[i].battleStatusChange.Length; j++)
		{
			if(enemy[i].battleStatusChange[j].statusID == index)
			{
				enemy[i].battleStatusChange = ArrayHelper.Remove(index, enemy[i].battleStatusChange);
				j--;
			}
		}
		for(int j=0; j<enemy[i].battleStatusChange.Length; j++)
		{
			if(enemy[i].battleStatusChange[i].statusID > index)
			{
				enemy[i].battleStatusChange[i].statusID--;
			}
		}
	}
	
	public void AddStatusEffect(int index)
	{
		for(int i=0; i<enemy.Length; i++)
		{
			enemy[i].skillEffect = ArrayHelper.Add(SkillEffect.NONE, enemy[i].skillEffect);
		}
	}
	
	public void RemoveStatusEffect(int index)
	{
		for(int i=0; i<enemy.Length; i++)
		{
			enemy[i].skillEffect = ArrayHelper.Remove(index, enemy[i].skillEffect);
		}
	}
	
	public void AddElement(int index)
	{
		for(int i=0; i<enemy.Length; i++)
		{
			enemy[i].bonus.AddElement();
			enemy[i].elementValue = ArrayHelper.Add(100, enemy[i].elementValue);
		}
	}
	
	public void RemoveElement(int index)
	{
		for(int i=0; i<enemy.Length; i++)
		{
			enemy[i].bonus.RemoveElement(index);
			enemy[i].elementValue = ArrayHelper.Remove(index, enemy[i].elementValue);
			enemy[i].baseElement = this.CheckForIndex(index, enemy[i].baseElement);
		}
	}
	
	public void RemoveItem(int index)
	{
		for(int i=0; i<enemy.Length; i++)
		{
			if(ItemDropType.ITEM.Equals(enemy[i].stealItemType))
			{
				enemy[i].stealItemID = this.CheckForIndex(index, enemy[i].stealItemID);
			}
			for(int j=0; j<enemy[i].itemDrop.Length; j++)
			{
				if(ItemDropType.ITEM.Equals(enemy[i].itemDrop[j].type))
				{
					enemy[i].itemDrop[j].itemID = this.CheckForIndex(index, enemy[i].itemDrop[j].itemID);
				}
			}
			for(int j=0; j<enemy[i].aiBehaviour.Length; j++)
			{
				for(int k=0; k<enemy[i].aiBehaviour[j].useID.Length; k++)
				{
					if(AttackSelection.ITEM.Equals(enemy[i].aiBehaviour[j].attackSelection[k]))
					{
						enemy[i].aiBehaviour[j].useID[k] = this.CheckForIndex(index, enemy[i].aiBehaviour[j].useID[k]);
					}
				}
			}
		}
	}
	
	public void RemoveWeapon(int index)
	{
		for(int i=0; i<enemy.Length; i++)
		{
			if(ItemDropType.WEAPON.Equals(enemy[i].stealItemType))
			{
				enemy[i].stealItemID = this.CheckForIndex(index, enemy[i].stealItemID);
			}
			for(int j=0; j<enemy[i].itemDrop.Length; j++)
			{
				if(ItemDropType.WEAPON.Equals(enemy[i].itemDrop[j].type))
				{
					enemy[i].itemDrop[j].itemID = this.CheckForIndex(index, enemy[i].itemDrop[j].itemID);
				}
			}
		}
	}
	
	public void RemoveArmor(int index)
	{
		for(int i=0; i<enemy.Length; i++)
		{
			if(ItemDropType.ARMOR.Equals(enemy[i].stealItemType))
			{
				enemy[i].stealItemID = this.CheckForIndex(index, enemy[i].stealItemID);
			}
			for(int j=0; j<enemy[i].itemDrop.Length; j++)
			{
				if(ItemDropType.ARMOR.Equals(enemy[i].itemDrop[j].type))
				{
					enemy[i].itemDrop[j].itemID = this.CheckForIndex(index, enemy[i].itemDrop[j].itemID);
				}
			}
		}
	}
	
	public void RemoveFormula(int index)
	{
		for(int i=0; i<enemy.Length; i++)
		{
			enemy[i].baseCounter = this.CheckForIndex(index, enemy[i].baseCounter);
			enemy[i].baseCritical = this.CheckForIndex(index, enemy[i].baseCritical);
			enemy[i].baseBlock = this.CheckForIndex(index, enemy[i].baseBlock);
			enemy[i].moveSpeedFormula = this.CheckForIndex(index, enemy[i].moveSpeedFormula);
			for(int j=0; j<enemy[i].battleStatusChange.Length; j++)
			{
				if(enemy[i].battleStatusChange[j].useFormula)
				{
					enemy[i].battleStatusChange[j].formulaID = this.CheckForIndex(index, enemy[i].battleStatusChange[j].formulaID);
				}
			}
		}
	}
	
	public void RemoveBaseAttack(int index)
	{
		for(int i=0; i<enemy.Length; i++)
		{
			for(int j=0; j<enemy[i].baseAttack.Length; j++)
			{
				enemy[i].baseAttack[j] = this.CheckForIndex(index, enemy[i].baseAttack[j]);
			}
		}
	}
	
	public void AddItemDrop(int index)
	{
		enemy[index].itemDrop = ArrayHelper.Add(new ItemDrop(), enemy[index].itemDrop);
	}
	
	public void RemoveItemDrop(int index, int item)
	{
		for(int i=item; i<enemy[index].itemDrop.Length-1; i++)
		{
			enemy[index].itemDrop[i] = enemy[index].itemDrop[i+1];
		}
		enemy[index].itemDrop = ArrayHelper.Remove(enemy[index].itemDrop.Length-1, enemy[index].itemDrop);
	}
	
	public void AddRace()
	{
		for(int i=0; i<enemy.Length; i++)
		{
			enemy[i].bonus.AddRace();
			enemy[i].raceValue = ArrayHelper.Add(100, enemy[i].raceValue);
		}
	}
	
	public void RemoveRace(int index)
	{
		for(int i=0; i<enemy.Length; i++)
		{
			enemy[i].bonus.RemoveRace(index);
			enemy[i].raceValue = ArrayHelper.Remove(index, enemy[i].raceValue);
			enemy[i].raceID = this.CheckForIndex(index, enemy[i].raceID);
		}
	}
	
	public void AddSize()
	{
		for(int i=0; i<enemy.Length; i++)
		{
			enemy[i].bonus.AddSize();
			enemy[i].sizeValue = ArrayHelper.Add(100, enemy[i].sizeValue);
		}
	}
	
	public void RemoveSize(int index)
	{
		for(int i=0; i<enemy.Length; i++)
		{
			enemy[i].bonus.RemoveSize(index);
			enemy[i].sizeValue = ArrayHelper.Remove(index, enemy[i].sizeValue);
			enemy[i].sizeID = this.CheckForIndex(index, enemy[i].sizeID);
		}
	}
	
	public void RemoveDifficulty(int index)
	{
		for(int i=0; i<enemy.Length; i++)
		{
			enemy[i].bonus.RemoveDifficulty(index);
			for(int j=0; j<enemy[i].aiBehaviour.Length; j++)
			{
				enemy[i].aiBehaviour[j].RemoveDifficulty(index);
			}
		}
	}
	
	/*
	============================================================================
	Filter functions
	============================================================================
	*/
	public override void CreateFilterList(bool showIDs)
	{
		ArrayList names = new ArrayList();
		ArrayList ids = new ArrayList();
		if(name != null)
		{
			for(int i=0; i<name[0].Count(); i++)
			{
				if((!this.filter.useFilter[0] || this.enemy[i].raceID == this.filter.filterID[0]) &&
					(!this.filter.useFilter[1] || this.enemy[i].sizeID == this.filter.filterID[1]))
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