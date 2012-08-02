
using System.Collections;

public class ClassData : BaseLangData
{
	public Class[] classes = new Class[0];
	
	// XML data
	private string filename = "classes";
	
	private static string CLASSES = "classes";
	private static string CLASS = "class";
	private static string EQUIPPART = "equippart";
	private static string WEAPON = "weapon";
	private static string ARMOR = "armor";
	private static string ELEMENT = "element";
	private static string SKILL = "skill";
	private static string RACE = "race";
	private static string SIZE = "size";
	private static string DEVELOPMENT = "development";
	private static string BONUS = "bonus";

	public ClassData()
	{
		LoadData();
	}
	
	public override string GetIconPath() { return "Icons/Class/"; }
	
	public void LoadData()
	{
		ArrayList data = XMLHandler.LoadXML(dir+filename);
		
		if(data.Count > 0)
		{
			foreach(Hashtable entry in data)
			{
				if(entry[XMLHandler.NODE_NAME] as string == ClassData.CLASSES)
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList subs = entry[XMLHandler.NODES] as ArrayList;
						icon = new string[subs.Count];
						classes = new Class[subs.Count];
						
						foreach(Hashtable val in subs)
						{
							if(val[XMLHandler.NODE_NAME] as string == ClassData.CLASS)
							{
								int i = int.Parse((string)val["id"]);
								icon[i] = "";
								
								classes[i] = new Class();
								classes[i].equipPart = new bool[int.Parse((string)val["equipparts"])];
								classes[i].weapon = new bool[int.Parse((string)val["weapons"])];
								classes[i].armor = new bool[int.Parse((string)val["armors"])];
								
								int count = int.Parse((string)val["elements"]);
								classes[i].elementValue = new int[count];
								for(int j=0; j<count; j++)
								{
									classes[i].elementValue[j] = 100;
								}
								if(val.ContainsKey("races"))
								{
									count = int.Parse((string)val["races"]);
								}
								else count = DataHolder.Races().GetDataCount();
								classes[i].raceValue = new int[count];
								for(int j=0; j<count; j++)
								{
									classes[i].raceValue[j] = 100;
								}
								if(val.ContainsKey("sizes"))
								{
									count = int.Parse((string)val["sizes"]);
								}
								else count = DataHolder.Sizes().GetDataCount();
								classes[i].sizeValue = new int[count];
								for(int j=0; j<count; j++)
								{
									classes[i].sizeValue[j] = 100;
								}
								
								if(val.ContainsKey("skills"))
								{
									classes[i].development.skill = new SkillLearn[int.Parse((string)val["skills"])];
									for(int j=0; j<classes[i].development.skill.Length; j++)
									{
										classes[i].development.skill[j] = new SkillLearn();
									}
								}
								
								if(val.ContainsKey("useclasslevel")) classes[i].useClassLevel = true;
								
								ArrayList s = val[XMLHandler.NODES] as ArrayList;
								foreach(Hashtable ht in s)
								{
									this.LoadLanguages(ht, i, subs.Count);
									if(ht[XMLHandler.NODE_NAME] as string == ClassData.EQUIPPART)
									{
										classes[i].equipPart[int.Parse((string)ht["id"])] = true;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == ClassData.WEAPON)
									{
										classes[i].weapon[int.Parse((string)ht["id"])] = true;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == ClassData.ARMOR)
									{
										classes[i].armor[int.Parse((string)ht["id"])] = true;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == ClassData.ELEMENT)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < classes[i].elementValue.Length)
										{
											classes[i].elementValue[id] = int.Parse((string)ht["value"]);
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == ClassData.SKILL)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < classes[i].development.skill.Length)
										{
											classes[i].development.skill[id].SetData(ht, false);
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == ClassData.RACE)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < classes[i].raceValue.Length)
										{
											classes[i].raceValue[id] = int.Parse((string)ht["value"]);
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == ClassData.SIZE)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < classes[i].sizeValue.Length)
										{
											classes[i].sizeValue[id] = int.Parse((string)ht["value"]);
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == ClassData.DEVELOPMENT)
									{
										classes[i].development.SetData(ht);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == ClassData.BONUS)
									{
										classes[i].bonus.SetData(ht);
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
		
		sv.Add(XMLHandler.NODE_NAME, ClassData.CLASSES);
		
		for(int i=0; i<name[0].Count(); i++)
		{
			Hashtable ht = new Hashtable();
			ArrayList s = new ArrayList();
			
			ht.Add(XMLHandler.NODE_NAME, ClassData.CLASS);
			ht.Add("id", i.ToString());
			ht.Add("equipparts", classes[i].equipPart.Length.ToString());
			ht.Add("weapons", classes[i].weapon.Length.ToString());
			ht.Add("armors", classes[i].armor.Length.ToString());
			
			s = this.SaveLanguages(s, i);
			
			for(int j=0; j<classes[i].equipPart.Length; j++)
			{
				if(classes[i].equipPart[j])
				{
					Hashtable ep = new Hashtable();
					ep.Add(XMLHandler.NODE_NAME, ClassData.EQUIPPART);
					ep.Add("id", j.ToString());
					s.Add(ep);
				}
			}
			
			for(int j=0; j<classes[i].weapon.Length; j++)
			{
				if(classes[i].weapon[j])
				{
					Hashtable ep = new Hashtable();
					ep.Add(XMLHandler.NODE_NAME, ClassData.WEAPON);
					ep.Add("id", j.ToString());
					s.Add(ep);
				}
			}
			
			for(int j=0; j<classes[i].armor.Length; j++)
			{
				if(classes[i].armor[j])
				{
					Hashtable ep = new Hashtable();
					ep.Add(XMLHandler.NODE_NAME, ClassData.ARMOR);
					ep.Add("id", j.ToString());
					s.Add(ep);
				}
			}
			
			ht.Add("elements", classes[i].elementValue.Length.ToString());
			for(int j=0; j<classes[i].elementValue.Length; j++)
			{
				if(classes[i].elementValue[j] != 100)
				{
					Hashtable ep = new Hashtable();
					ep.Add(XMLHandler.NODE_NAME, ClassData.ELEMENT);
					ep.Add("id", j.ToString());
					ep.Add("value", classes[i].elementValue[j].ToString());
					s.Add(ep);
				}
			}
			
			ht.Add("races", classes[i].raceValue.Length.ToString());
			for(int j=0; j<classes[i].raceValue.Length; j++)
			{
				if(classes[i].raceValue[j] != 100)
				{
					Hashtable ep = HashtableHelper.GetTitleHashtable(ClassData.RACE, j);
					ep.Add("value", classes[i].raceValue[j].ToString());
					s.Add(ep);
				}
			}
			
			ht.Add("sizes", classes[i].sizeValue.Length.ToString());
			for(int j=0; j<classes[i].sizeValue.Length; j++)
			{
				if(classes[i].sizeValue[j] != 100)
				{
					Hashtable ep = HashtableHelper.GetTitleHashtable(ClassData.SIZE, j);
					ep.Add("value", classes[i].sizeValue[j].ToString());
					s.Add(ep);
				}
			}
			
			if(classes[i].useClassLevel) ht.Add("useclasslevel", "true");
			s.Add(classes[i].development.GetData(
					HashtableHelper.GetTitleHashtable(ClassData.DEVELOPMENT), classes[i].useClassLevel));
			
			s.Add(classes[i].bonus.GetData(HashtableHelper.GetTitleHashtable(ClassData.BONUS)));
			
			ht.Add(XMLHandler.NODES, s);
			subs.Add(ht);
		}
		sv.Add(XMLHandler.NODES, subs);
		data.Add(sv);
		XMLHandler.SaveXML(dir, filename, data);
	}
	
	public void AddClass(string n, string d, int count, int epCount, int wpCount, int arCount, int elCount)
	{
		base.AddBaseData(n, d, count);
		classes = ArrayHelper.Add(new Class(), classes);
		
		classes[classes.Length-1].equipPart = new bool[epCount];
		classes[classes.Length-1].weapon = new bool[wpCount];
		classes[classes.Length-1].armor = new bool[arCount];
		classes[classes.Length-1].elementValue = new int[elCount];
		for(int i=0; i<elCount; i++)
		{
			classes[classes.Length-1].elementValue[i] = 100;
		}
		classes[classes.Length-1].raceValue = new int[DataHolder.Races().GetDataCount()];
		for(int i=0; i<classes[classes.Length-1].raceValue.Length; i++)
		{
			classes[classes.Length-1].raceValue[i] = 100;
		}
		classes[classes.Length-1].sizeValue = new int[DataHolder.Sizes().GetDataCount()];
		for(int i=0; i<classes[classes.Length-1].sizeValue.Length; i++)
		{
			classes[classes.Length-1].sizeValue[i] = 100;
		}
		classes[classes.Length-1].development.BaseInit(DataHolder.StatusValues().value);
	}
	
	public override void RemoveData(int index)
	{
		base.RemoveData(index);
		classes = ArrayHelper.Remove(index, classes);
	}
	
	public override void Copy(int index)
	{
		base.Copy(index);
		classes = ArrayHelper.Add(new Class(), classes);
		
		classes[classes.Length-1].equipPart = new bool[classes[index].equipPart.Length];
		for(int i=0; i<classes[index].equipPart.Length; i++)
		{
			classes[classes.Length-1].equipPart[i] = classes[index].equipPart[i];
		}
		
		classes[classes.Length-1].weapon = new bool[classes[index].weapon.Length];
		for(int i=0; i<classes[index].weapon.Length; i++)
		{
			classes[classes.Length-1].weapon[i] = classes[index].weapon[i];
		}
		
		classes[classes.Length-1].armor = new bool[classes[index].armor.Length];
		for(int i=0; i<classes[index].armor.Length; i++)
		{
			classes[classes.Length-1].armor[i] = classes[index].armor[i];
		}
		
		classes[classes.Length-1].elementValue = new int[classes[index].elementValue.Length];
		for(int i=0; i<classes[index].elementValue.Length; i++)
		{
			classes[classes.Length-1].elementValue[i] = classes[index].elementValue[i];
		}
		
		classes[classes.Length-1].raceValue = new int[classes[index].raceValue.Length];
		for(int i=0; i<classes[index].raceValue.Length; i++)
		{
			classes[classes.Length-1].raceValue[i] = classes[index].raceValue[i];
		}
		
		classes[classes.Length-1].sizeValue = new int[classes[index].sizeValue.Length];
		for(int i=0; i<classes[index].sizeValue.Length; i++)
		{
			classes[classes.Length-1].sizeValue[i] = classes[index].sizeValue[i];
		}
		
		classes[classes.Length-1].useClassLevel = classes[index].useClassLevel;
		classes[classes.Length-1].development = classes[index].development.GetCopy();
		classes[classes.Length-1].bonus.SetData(classes[index].bonus.GetData(new Hashtable()));
	}
	
	public void AddStatusValue(int index, StatusValue val)
	{
		for(int i=0; i<classes.Length; i++)
		{
			classes[i].bonus.AddStatusValue();
			classes[i].development.AddStatusValue(index, val);
		}
	}
	
	public void SetStatusValueType(int index, StatusValueType type, StatusValue val)
	{
		for(int i=0; i<classes.Length; i++)
		{
			classes[i].bonus.SetStatusValueType(index, type);
			classes[i].development.SetStatusValueType(index, type, val);
		}
	}
	
	public void RemoveStatusValue(int index)
	{
		for(int i=0; i<classes.Length; i++)
		{
			classes[i].bonus.RemoveStatusValue(index);
			classes[i].development.RemoveStatusValue(index);
		}
	}
	
	public void StatusValueMinMaxChanged(int index, int min, int max)
	{
		for(int i=0; i<classes.Length; i++)
		{
			classes[i].development.StatusValueMinMaxChanged(index, min, max);
		}
	}
	
	public void AddElement(int index)
	{
		for(int i=0; i<classes.Length; i++)
		{
			classes[i].bonus.AddElement();
			classes[i].elementValue = ArrayHelper.Add(100, classes[i].elementValue);
		}
	}
	
	public void RemoveElement(int index)
	{
		for(int i=0; i<classes.Length; i++)
		{
			classes[i].bonus.RemoveElement(index);
			classes[i].elementValue = ArrayHelper.Remove(index, classes[i].elementValue);
		}
	}
	
	public void AddEquipmentPart(int index)
	{
		for(int i=0; i<classes.Length; i++)
		{
			classes[i].equipPart = ArrayHelper.Add(false, classes[i].equipPart);
		}
	}
	
	public void RemoveEquipmentPart(int index)
	{
		for(int i=0; i<classes.Length; i++)
		{
			classes[i].equipPart = ArrayHelper.Remove(index, classes[i].equipPart);
		}
	}
	
	public void AddWeapon(int index)
	{
		for(int i=0; i<classes.Length; i++)
		{
			classes[i].weapon = ArrayHelper.Add(false, classes[i].weapon);
		}
	}
	
	public void RemoveWeapon(int index)
	{
		for(int i=0; i<classes.Length; i++)
		{
			classes[i].weapon = ArrayHelper.Remove(index, classes[i].weapon);
		}
	}
	
	public void AddArmor(int index)
	{
		for(int i=0; i<classes.Length; i++)
		{
			classes[i].armor = ArrayHelper.Add(false, classes[i].armor);
		}
	}
	
	public void RemoveArmor(int index)
	{
		for(int i=0; i<classes.Length; i++)
		{
			classes[i].armor = ArrayHelper.Remove(index, classes[i].armor);
		}
	}
	
	public void RemoveSkill(int index)
	{
		for(int i=0; i<classes.Length; i++)
		{
			for(int j=0; j<classes[i].development.skill.Length; j++)
			{
				classes[i].development.skill[j].skillID = this.CheckForIndex(index, classes[i].development.skill[j].skillID);
			}
		}
	}
	
	public void AddLearnSkill(int index)
	{
		classes[index].development.skill = ArrayHelper.Add(new SkillLearn(), classes[index].development.skill);
	}
	
	public void RemoveLearnSkill(int index, int s)
	{
		classes[index].development.skill = ArrayHelper.Remove(s, classes[index].development.skill);
	}
	
	public void AddRace()
	{
		for(int i=0; i<classes.Length; i++)
		{
			classes[i].bonus.AddRace();
			classes[i].raceValue = ArrayHelper.Add(100, classes[i].raceValue);
		}
	}
	
	public void RemoveRace(int index)
	{
		for(int i=0; i<classes.Length; i++)
		{
			classes[i].bonus.RemoveRace(index);
			classes[i].raceValue = ArrayHelper.Remove(index, classes[i].raceValue);
		}
	}
	
	public void AddSize()
	{
		for(int i=0; i<classes.Length; i++)
		{
			classes[i].bonus.AddSize();
			classes[i].sizeValue = ArrayHelper.Add(100, classes[i].sizeValue);
		}
	}
	
	public void RemoveSize(int index)
	{
		for(int i=0; i<classes.Length; i++)
		{
			classes[i].bonus.RemoveSize(index);
			classes[i].sizeValue = ArrayHelper.Remove(index, classes[i].sizeValue);
		}
	}
	
	public void RemoveDifficulty(int index)
	{
		for(int i=0; i<classes.Length; i++)
		{
			classes[i].bonus.RemoveDifficulty(index);
		}
	}
}