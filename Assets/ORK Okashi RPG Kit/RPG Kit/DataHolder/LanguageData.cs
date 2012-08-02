
using System.Collections;
using UnityEngine;

public class LanguageData : BaseData
{
	// XML data
	private string filename = "languages";
	
	private static string LANGUAGES = "languages";
	private static string LANGUAGE = "language";

	public LanguageData()
	{
		LoadData();
	}
	
	public void LoadData()
	{
		ArrayList data = XMLHandler.LoadXML(dir+filename);
		
		if(data.Count > 0)
		{
			foreach(Hashtable entry in data)
			{
				if(entry[XMLHandler.NODE_NAME] as string == LanguageData.LANGUAGES)
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList subs = entry[XMLHandler.NODES] as ArrayList;
						name = new string[subs.Count];
						
						foreach(Hashtable lang in subs)
						{
							if(lang[XMLHandler.NODE_NAME] as string == LanguageData.LANGUAGE)
							{
								int i = int.Parse((string)lang["id"]);
								name[i] = lang[XMLHandler.CONTENT] as string;
							}
						}
					}
				}
			}
		}
		else
		{
			this.AddLanguage("English");
		}
	}
	
	public void SaveData()
	{
		ArrayList data = new ArrayList();
		ArrayList subs = new ArrayList();
		Hashtable sv = new Hashtable();
		
		sv.Add(XMLHandler.NODE_NAME, LanguageData.LANGUAGES);
		
		for(int i=0; i<name.Length; i++)
		{
			Hashtable lang = new Hashtable();
			lang.Add(XMLHandler.NODE_NAME, LanguageData.LANGUAGE);
			lang.Add("id", i.ToString());
			lang.Add(XMLHandler.CONTENT, name[i]);
			subs.Add(lang);
		}
		sv.Add(XMLHandler.NODES, subs);
		
		data.Add(sv);
		XMLHandler.SaveXML(dir, filename, data);
	}
	
	public void AddLanguage(string n)
	{
		if(name == null)
		{
			name = new string[] {n};
		}
		else
		{
			name = ArrayHelper.Add(n, name);
		}
	}
	
	public override void RemoveData(int index)
	{
		name = ArrayHelper.Remove(index, name);
	}
	
	public override void Copy(int index)
	{
		this.AddLanguage(name[index]);
	}
	
	public ChoiceContent[] GetLanguageChoice()
	{
		ChoiceContent[] choice = new ChoiceContent[name.Length+1];
		for(int i=0; i<name.Length; i++)
		{
			choice[i] = new ChoiceContent(new GUIContent(this.GetName(i)));
		}
		choice[choice.Length-1] = new ChoiceContent(new GUIContent(DataHolder.MainMenu().GetCancelText()));
		return choice;
	}
}