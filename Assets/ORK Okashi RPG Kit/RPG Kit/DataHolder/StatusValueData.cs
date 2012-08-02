
using System.Collections;
using UnityEngine;

public class StatusValueData : BaseLangData
{
	public StatusValue[] value = new StatusValue[0];
	
	// XML data
	private string filename = "statusValues";
	
	private static string STATUSVALUES = "statusvalues";
	private static string VALUE = "value";
	private static string ADDTEXT = "addtext";
	private static string SUBTEXT = "subtext";

	public StatusValueData()
	{
		LoadData();
	}
	
	public override string GetIconPath() { return "Icons/StatusValue/"; }
	
	public void LoadData()
	{
		ArrayList data = XMLHandler.LoadXML(dir+filename);
		
		if(data.Count > 0)
		{
			foreach(Hashtable entry in data)
			{
				if(entry[XMLHandler.NODE_NAME] as string == StatusValueData.STATUSVALUES)
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList subs = entry[XMLHandler.NODES] as ArrayList;
						value = new StatusValue[subs.Count];
						icon = new string[subs.Count];
						
						foreach(Hashtable val in subs)
						{
							if(val[XMLHandler.NODE_NAME] as string == StatusValueData.VALUE)
							{
								int i = int.Parse((string)val["id"]);
								icon[i] = "";
								value[i] = new StatusValue();
								value[i].minValue = int.Parse((string)val["minvalue"]);
								value[i].maxValue = int.Parse((string)val["maxvalue"]);
								value[i].type = (StatusValueType)System.Enum.Parse(typeof(StatusValueType), (string)val["type"]);
								
								if(StatusValueType.CONSUMABLE.Equals(value[i].type))
								{
									value[i].maxStatus = int.Parse((string)val["maxstatus"]);
									value[i].killChar = bool.Parse((string)val["killchar"]);
								}
								else if(StatusValueType.EXPERIENCE.Equals(value[i].type))
								{
									value[i].levelUp = bool.Parse((string)val["levelup"]);
									if(val.ContainsKey("levelupclass")) value[i].levelUpClass = true;
								}
								
								ArrayList s = val[XMLHandler.NODES] as ArrayList;
								foreach(Hashtable ht in s)
								{
									this.LoadLanguages(ht, i, subs.Count);
									if(ht[XMLHandler.NODE_NAME] as string == StatusValueData.ADDTEXT)
									{
										value[i].addText.SetData(ht);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == StatusValueData.SUBTEXT)
									{
										value[i].subText.SetData(ht);
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
			sv.Add(XMLHandler.NODE_NAME, StatusValueData.STATUSVALUES);
			
			for(int i=0; i<name[0].Count(); i++)
			{
				Hashtable val = new Hashtable();
				ArrayList s = new ArrayList();
				
				val.Add(XMLHandler.NODE_NAME, StatusValueData.VALUE);
				val.Add("id", i.ToString());
				val.Add("minvalue", value[i].minValue.ToString());
				val.Add("maxvalue", value[i].maxValue.ToString());
				val.Add("type", value[i].type.ToString());
				
				if(StatusValueType.CONSUMABLE.Equals(value[i].type))
				{
					val.Add("maxstatus", value[i].maxStatus.ToString());
					val.Add("killchar", value[i].killChar.ToString());
				}
				else if(StatusValueType.EXPERIENCE.Equals(value[i].type))
				{
					val.Add("levelup", value[i].levelUp.ToString());
					if(value[i].levelUpClass) val.Add("levelupclass", "true");
				}
				
				s = this.SaveLanguages(s, i);
				if(value[i].addText.active) s.Add(value[i].addText.GetData(StatusValueData.ADDTEXT));
				if(value[i].subText.active) s.Add(value[i].subText.GetData(StatusValueData.SUBTEXT));
				val.Add(XMLHandler.NODES, s);
				subs.Add(val);
			}
			sv.Add(XMLHandler.NODES, subs);
			data.Add(sv);
			
			XMLHandler.SaveXML(dir, filename, data);
		}
	}
	
	public StatusValue GetStatusValueData(int index)
	{
		return value[index];
	}
	
	public StatusValue[] GetStatusValuesData()
	{
		return value;
	}
	
	public void AddValue(string n, string d, int count)
	{
		base.AddBaseData(n, d, count);
		value = ArrayHelper.Add(new StatusValue(), value);
	}
	
	public override void RemoveData(int index)
	{
		base.RemoveData(index);
		value = ArrayHelper.Remove(index, value);
	}
	
	public override void Copy(int index)
	{
		base.Copy(index);
		value = ArrayHelper.Add(this.GetCopy(index), value);
	}
	
	public StatusValue GetCopy(int index)
	{
		StatusValue v = new StatusValue();
		v.realID = index;
		
		v.minValue = value[index].minValue;
		v.maxValue = value[index].maxValue;
		v.type = value[index].type;
		v.maxStatus = value[index].maxStatus;
		v.killChar = value[index].killChar;
		v.levelUp = value[index].levelUp;
		v.levelUpClass = value[index].levelUpClass;
		
		v.addText.SetData(value[index].addText.GetData(StatusValueData.ADDTEXT));
		v.subText.SetData(value[index].subText.GetData(StatusValueData.SUBTEXT));
		
		return v;
	}
	
	public int GetFirstConsumable()
	{
		int id = -1;
		for(int i=0; i<this.value.Length; i++)
		{
			if(this.value[i].IsConsumable())
			{
				id = i;
				break;
			}
		}
		return id;
	}
	
	public void ClearCopiedLevelUp()
	{
		value[value.Length-1].levelUp = false;
		value[value.Length-1].levelUpClass = false;
	}
	
	public void RemoveStatusValue(int index)
	{
		for(int i=0; i<value.Length; i++)
		{
			if(value[i].maxStatus == index)
			{
				value[i].maxStatus = 0;
			}
			else if(value[i].maxStatus > index)
			{
				value[i].maxStatus -= 1;
			}
		}
	}
	
	public void AddRefreshTextSettings(int id, Hashtable ht)
	{
		if(id < value.Length)
		{
			value[id].addText.SetData(ht);
		}
	}
	
	public void AddDamageTextSettings(int id, Hashtable ht)
	{
		if(id < value.Length)
		{
			value[id].subText.SetData(ht);
		}
	}
	
	new public void AddLanguage(int lang)
	{
		base.AddLanguage(lang);
		for(int i=0; i<value.Length; i++)
		{
			value[i].addText.text = ArrayHelper.Add("%", value[i].addText.text);
			value[i].subText.text = ArrayHelper.Add("%", value[i].subText.text);
		}
	}
	
	new public void RemoveLanguage(int lang)
	{
		base.RemoveLanguage(lang);
		for(int i=0; i<value.Length; i++)
		{
			value[i].addText.text = ArrayHelper.Remove(lang, value[i].addText.text);
			value[i].subText.text = ArrayHelper.Remove(lang, value[i].subText.text);
		}
	}
}