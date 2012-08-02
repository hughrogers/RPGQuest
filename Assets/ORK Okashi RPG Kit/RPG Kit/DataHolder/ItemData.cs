
using System.Collections;

public class ItemData : BaseLangData
{
	public Item[] item = new Item[0];

	// XML data
	private string filename = "items";
	
	public static string AUDIO_PATH = "Audio/Items/";
	
	private static string ITEMS = "items";
	private static string ITEM = "item";
	private static string EFFECT = "effect";
	private static string VALUECHANGE = "valuechange";
	private static string PREFAB = "prefab";
	private static string VARIABLEKEY = "variablekey";
	private static string VARIABLEVALUE = "variablevalue";
	public static string TARGETRAYCAST = "targetraycast";
	public static string AUDIOCLIP = "audioclip";

	public ItemData()
	{
		this.filter = new DataFilter(1);
		LoadData();
	}
	
	public override string GetIconPath() { return "Icons/Item/"; }
	public string GetPrefabPath() { return "Prefabs/Items/"; }
	
	public void LoadData()
	{
		ArrayList data = XMLHandler.LoadXML(dir+filename);
		
		if(data.Count > 0)
		{
			foreach(Hashtable entry in data)
			{
				if(entry[XMLHandler.NODE_NAME] as string == ItemData.ITEMS)
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList subs = entry[XMLHandler.NODES] as ArrayList;
						icon = new string[subs.Count];
						item = new Item[subs.Count];
						
						foreach(Hashtable val in subs)
						{
							if(val[XMLHandler.NODE_NAME] as string == ItemData.ITEM)
							{
								int i = int.Parse((string)val["id"]);
								icon[i] = "";
								
								item[i] = new Item();
								item[i].skillEffect = new SkillEffect[int.Parse((string)val["effects"])];
								
								item[i].itemType = int.Parse((string)val["itemtype"]);
								item[i].useable = bool.Parse((string)val["useable"]);
								item[i].consume = bool.Parse((string)val["consume"]);
								item[i].targetType = (TargetType)System.Enum.Parse(typeof(TargetType), (string)val["targettype"]);
								item[i].skillTarget = (SkillTarget)System.Enum.Parse(typeof(SkillTarget), (string)val["skilltarget"]);
								
								int values = int.Parse((string)val["values"]);
								item[i].valueChange = new ValueChange[values];
								for(int j=0; j<values; j++)
								{
									item[i].valueChange[j] = new ValueChange();
								}
								if(val.ContainsKey("dropable")) item[i].dropable = true;
								if(val.ContainsKey("stealable")) item[i].stealable = true;
								if(val.ContainsKey("useinbattle")) item[i].useInBattle = true;
								if(val.ContainsKey("useinfield")) item[i].useInField = true;
								if(val.ContainsKey("revive")) item[i].revive = true;
								if(val.ContainsKey("recipeid"))
								{
									item[i].learnRecipe = true;
									item[i].recipeID = int.Parse((string)val["recipeid"]);
								}
								if(val.ContainsKey("animation"))
								{
									item[i].battleAnimation = true;
									item[i].animationID = int.Parse((string)val["animation"]);
								}
								
								item[i].buyPrice = int.Parse((string)val["buyprice"]);
								if(val.ContainsKey("sellprice"))
								{
									item[i].sellable = true;
									item[i].sellPrice = int.Parse((string)val["sellprice"]);
									item[i].sellSetter = (ValueSetter)System.Enum.Parse(typeof(ValueSetter), (string)val["sellsetter"]);
								}
								
								if(val.ContainsKey("itemskill"))
								{
									item[i].itemSkill = (ItemSkillType)System.Enum.Parse(typeof(ItemSkillType), (string)val["itemskill"]);
									item[i].skillID = int.Parse((string)val["skill"]);
									if(val.ContainsKey("slvl"))
									{
										item[i].skillLevel = int.Parse((string)val["slvl"]);
									}
								}
								if(val.ContainsKey("itemvariable")) 
								{
									item[i].itemVariable = (ItemVariableType)System.Enum.Parse(typeof(ItemVariableType), (string)val["itemvariable"]);
								}
								item[i].useRange.SetData(val);
								
								IntHelper.FromHashtable(val, "globaleventid", ref item[i].globalEventID, ref item[i].callGlobalEvent);
								
								ArrayList s = val[XMLHandler.NODES] as ArrayList;
								foreach(Hashtable ht in s)
								{
									this.LoadLanguages(ht, i, subs.Count);
									if(ht[XMLHandler.NODE_NAME] as string == ItemData.PREFAB)
									{
										item[i].prefabName = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == ItemData.EFFECT)
									{
										item[i].skillEffect[int.Parse((string)ht["id"])] = (SkillEffect)System.Enum.Parse(typeof(SkillEffect), (string)ht["value"]);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == ItemData.VALUECHANGE)
									{
										int j = int.Parse((string)ht["id"]);
										if(j < item[i].valueChange.Length) item[i].valueChange[j].SetData(ht);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == ItemData.VARIABLEKEY)
									{
										item[i].variableKey = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == ItemData.VARIABLEVALUE)
									{
										item[i].variableValue = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == ItemData.TARGETRAYCAST)
									{
										item[i].targetRaycast.SetData(ht);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == ItemData.AUDIOCLIP)
									{
										item[i].audioName = ht[XMLHandler.CONTENT] as string;
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
		
		sv.Add(XMLHandler.NODE_NAME, ItemData.ITEMS);
		
		for(int i=0; i<name[0].Count(); i++)
		{
			Hashtable ht = new Hashtable();
			ArrayList s = new ArrayList();
			
			ht.Add(XMLHandler.NODE_NAME, ItemData.ITEM);
			ht.Add("id", i.ToString());
			ht.Add("itemtype", item[i].itemType.ToString());
			if(item[i].useInBattle) ht.Add("useinbattle", "true");
			if(item[i].useInField) ht.Add("useinfield", "true");
			if(item[i].battleAnimation) ht.Add("animation", item[i].animationID.ToString());
			ht = item[i].useRange.GetData(ht);
			
			ht.Add("buyprice", item[i].buyPrice.ToString());
			if(item[i].sellable)
			{
				ht.Add("sellprice", item[i].sellPrice.ToString());
				ht.Add("sellsetter", item[i].sellSetter.ToString());
			}
			
			if(item[i].dropable) ht.Add("dropable", "true");
			if(item[i].stealable) ht.Add("stealable", "true");
			ht.Add("useable", item[i].useable.ToString());
			ht.Add("consume", item[i].consume.ToString());
			ht.Add("targettype", item[i].targetType.ToString());
			ht.Add("skilltarget", item[i].skillTarget.ToString());
			if(item[i].revive) ht.Add("revive", "true");
			if(item[i].learnRecipe) ht.Add("recipeid", item[i].recipeID.ToString());
			
			IntHelper.ToHashtable(ref ht, "globaleventid", item[i].globalEventID, item[i].callGlobalEvent);
			
			if(item[i].itemSkill > 0)
			{
				ht.Add("itemskill", item[i].itemSkill);
				ht.Add("skill", item[i].skillID);
				ht.Add("slvl", item[i].skillLevel);
			}
			if("" != item[i].prefabName)
			{
				Hashtable pref = new Hashtable();
				pref.Add(XMLHandler.NODE_NAME, ItemData.PREFAB);
				pref.Add(XMLHandler.CONTENT, item[i].prefabName);
				s.Add(pref);
			}
			
			s = this.SaveLanguages(s, i);
			
			ht.Add("effects", item[i].skillEffect.Length.ToString());
			for(int j=0; j<item[i].skillEffect.Length; j++)
			{
				if(item[i].skillEffect[j] > 0)
				{
					Hashtable e = new Hashtable();
					e.Add(XMLHandler.NODE_NAME, ItemData.EFFECT);
					e.Add("id", j.ToString());
					e.Add("value", item[i].skillEffect[j].ToString());
					s.Add(e);
				}
			}
			
			ht.Add("values", item[i].valueChange.Length.ToString());
			for(int j=0; j<item[i].valueChange.Length; j++)
			{
				if(item[i].valueChange[j].active)
				{
					s.Add(item[i].valueChange[j].GetData(HashtableHelper.GetTitleHashtable(ItemData.VALUECHANGE, j)));
				}
			}
			
			// variable settings
			ht.Add("itemvariable", item[i].itemVariable.ToString());
			if(!ItemVariableType.NONE.Equals(item[i].itemVariable))
			{
				Hashtable iv = new Hashtable();
				iv.Add(XMLHandler.NODE_NAME, ItemData.VARIABLEKEY);
				iv.Add(XMLHandler.CONTENT, item[i].variableKey);
				s.Add(iv);
				
				if(ItemVariableType.SET.Equals(item[i].itemVariable))
				{
					iv = new Hashtable();
					iv.Add(XMLHandler.NODE_NAME, ItemData.VARIABLEVALUE);
					iv.Add(XMLHandler.CONTENT, item[i].variableValue);
					s.Add(iv);
				}
			}
			
			s.Add(item[i].targetRaycast.GetData(HashtableHelper.GetTitleHashtable(ItemData.TARGETRAYCAST)));
			if(item[i].audioName != "") s.Add(HashtableHelper.GetContentHashtable(ItemData.AUDIOCLIP, item[i].audioName));
			
			ht.Add(XMLHandler.NODES, s);
			subs.Add(ht);
		}
		sv.Add(XMLHandler.NODES, subs);
		data.Add(sv);
		XMLHandler.SaveXML(dir, filename, data);
	}
	
	public void AddItem(string n, string d, int count, int seCount, int svCount)
	{
		base.AddBaseData(n, d, count);
		item = ArrayHelper.Add(new Item(), item);
		
		item[item.Length-1].skillEffect = new SkillEffect[seCount];
		item[item.Length-1].valueChange = new ValueChange[svCount];
		for(int i=0; i<svCount; i++)
		{
			item[item.Length-1].valueChange[i] = new ValueChange();
		}
	}
	
	public override void RemoveData(int index)
	{
		base.RemoveData(index);
		item = ArrayHelper.Remove(index, item);
	}
	
	public override void Copy(int index)
	{
		base.Copy(index);
		item = ArrayHelper.Add(new Item(), item);
		
		item[item.Length-1].prefabName = item[index].prefabName;
		item[item.Length-1].itemType = item[index].itemType;
		item[item.Length-1].buyPrice = item[index].buyPrice;
		item[item.Length-1].sellable = item[index].sellable;
		item[item.Length-1].sellPrice = item[index].sellPrice;
		item[item.Length-1].sellSetter = item[index].sellSetter;
		item[item.Length-1].useable = item[index].useable;
		item[item.Length-1].consume = item[index].consume;
		item[item.Length-1].itemSkill = item[index].itemSkill;
		item[item.Length-1].skillID = item[index].skillID;
		item[item.Length-1].skillLevel = item[index].skillLevel;
		item[item.Length-1].targetType = item[index].targetType;
		item[item.Length-1].skillTarget = item[index].skillTarget;
		item[item.Length-1].useInBattle = item[index].useInBattle;
		item[item.Length-1].useInField = item[index].useInField;
		item[item.Length-1].revive = item[index].revive;
		item[item.Length-1].battleAnimation = item[index].battleAnimation;
		item[item.Length-1].animationID = item[index].animationID;
		item[item.Length-1].itemVariable = item[index].itemVariable;
		item[item.Length-1].variableKey = item[index].variableKey;
		item[item.Length-1].variableValue = item[index].variableValue;
		item[item.Length-1].learnRecipe = item[index].learnRecipe;
		item[item.Length-1].recipeID = item[index].recipeID;
		item[item.Length-1].dropable = item[index].dropable;
		item[item.Length-1].stealable = item[index].stealable;
		item[item.Length-1].audioName = item[index].audioName;
		item[item.Length-1].callGlobalEvent = item[index].callGlobalEvent;
		item[item.Length-1].globalEventID = item[index].globalEventID;
		item[item.Length-1].useRange.SetData(item[index].useRange.GetData(new Hashtable()));
		
		item[item.Length-1].valueChange = new ValueChange[item[index].valueChange.Length];
		for(int i=0; i<item[index].valueChange.Length; i++)
		{
			if(item[index].valueChange[i].active)
			{
				item[item.Length-1].valueChange[i] = new ValueChange(item[index].valueChange[i].GetData(new Hashtable()));
			}
			else item[item.Length-1].valueChange[i] = new ValueChange();
		}
		
		item[item.Length-1].skillEffect = new SkillEffect[item[index].skillEffect.Length];
		for(int i=0; i<item[index].skillEffect.Length; i++)
		{
			item[item.Length-1].skillEffect[i] = item[index].skillEffect[i];
		}
		
		item[item.Length-1].targetRaycast.SetData(item[index].targetRaycast.GetData(new Hashtable()));
	}
	
	public void AddStatusEffect(int index)
	{
		for(int i=0; i<item.Length; i++)
		{
			item[i].skillEffect = ArrayHelper.Add(SkillEffect.NONE, item[i].skillEffect);
		}
	}
	
	public void RemoveStatusEffect(int index)
	{
		for(int i=0; i<item.Length; i++)
		{
			for(int j=index; j<item[i].skillEffect.Length-1; j++)
			{
				item[i].skillEffect[j] = item[i].skillEffect[j+1];
			}
			item[i].skillEffect = ArrayHelper.Remove(item[i].skillEffect.Length-1, item[i].skillEffect);
		}
	}
	
	public void RemoveItemType(int index)
	{
		for(int i=0; i<item.Length; i++)
		{
			if(item[i].itemType == index)
			{
				item[i].itemType = 0;
			}
			else if(item[i].itemType > index)
			{
				item[i].itemType -= 1;
			}
		}
	}
	
	public void RemoveSkill(int index)
	{
		for(int i=0; i<item.Length; i++)
		{
			if(item[i].skillID == index)
			{
				item[i].skillID = 0;
			}
			else if(item[i].skillID > index)
			{
				item[i].skillID -= 1;
			}
		}
	}
	
	public void AddStatusValue(int index)
	{
		for(int i=0; i<item.Length; i++)
		{
			item[i].valueChange = ArrayHelper.Add(new ValueChange(), item[i].valueChange);
		}
	}
	
	public void RemoveStatusValue(int index)
	{
		for(int i=0; i<item.Length; i++)
		{
			for(int j=index; j<item[i].valueChange.Length-1; j++)
			{
				item[i].valueChange[j] = item[i].valueChange[j+1];
			}
			item[i].valueChange = ArrayHelper.Remove(item[i].valueChange.Length-1, item[i].valueChange);
			
			for(int j=index; j<item[i].valueChange.Length-1; j++)
			{
				item[i].valueChange[j].status = this.CheckForIndex(index, item[i].valueChange[j].status);
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
				if(this.item[i].itemType == this.filter.filterID[0])
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