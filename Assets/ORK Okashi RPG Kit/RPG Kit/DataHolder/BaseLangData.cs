
using System.Collections;
using UnityEngine;

public class BaseLangData
{
	public Language[] name = new Language[0];
	public Language[] description = new Language[0];
	public string[] icon = new string[0];
	
	public string skinPath = "HUD/";
	
	protected string dir = "ProjectSettings/";
	
	public DataFilter filter;
	
	protected static string NAME = "name";
	protected static string DESCRIPTION = "description";
	protected static string ICON = "icon";
	
	public BaseLangData()
	{
		
	}
	
	public int GetDataCount()
	{
		int val = 0;
		if(this.name.Length > 0)
		{
			val = this.name[0].Count();
		}
		return val;
	}
	
	public string GetName(int index, int lang)
	{
		string n = "";
		if(index >= 0 && index < this.name[lang].Count())
		{
			n = this.name[lang].text[index];
		}
		return n;
	}
	
	public string GetName(int index)
	{
		return this.GetName(index, GameHandler.GetLanguage());
	}
	
	public string GetDescription(int index, int lang)
	{
		string n = "";
		if(index >= 0 && index < this.description[lang].Count())
		{
			n = this.description[lang].text[index];
		}
		return n;
	}
	
	public string GetDescription(int index)
	{
		return this.GetDescription(index, GameHandler.GetLanguage());
	}
	
	public ChoiceContent GetChoiceContent(int id, HUDContentType type)
	{
		ChoiceContent cc = null;
		if(HUDContentType.TEXT.Equals(type))
		{
			cc = new ChoiceContent(new GUIContent(this.GetName(id)));
		}
		else if(HUDContentType.ICON.Equals(type))
		{
			cc = new ChoiceContent(new GUIContent(this.GetIcon(id)));
		}
		else if(HUDContentType.BOTH.Equals(type))
		{
			cc = new ChoiceContent(this.GetContent(id));
		}
		return cc;
	}
	
	public virtual string GetIconPath() { return ""; }
	
	public Texture2D GetIcon(int index)
	{
		Texture2D tex = null;
		if(this.icon[index] != null && "" != this.icon[index])
		{
			tex = (Texture2D)Resources.Load(this.GetIconPath()+this.icon[index], typeof(Texture2D));
		}
		return tex;
	}
	
	public GUIContent GetContent(int index)
	{
		return new GUIContent(this.GetName(index), this.GetIcon(index));
	}
	
	public string[] GetNameList(bool showIDs)
	{
		string[] result = new string[0];
		if(this.name.Length > 0)
		{
			result = new string[name[0].Count()];
			for(int i=0; i<name[0].Count(); i++)
			{
				if(showIDs)
				{
					result[i] = i.ToString() + ": " + name[0].text[i];
				}
				else
				{
					result[i] = name[0].text[i];
				}
			}
		}
		return result;
	}
	
	public virtual void CreateFilterList(bool showIDs)
	{
		ArrayList names = new ArrayList();
		ArrayList ids = new ArrayList();
		if(this.name.Length > 0)
		{
			for(int i=0; i<name[0].Count(); i++)
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
		this.filter.nameList = names.ToArray(typeof(string)) as string[];
		this.filter.realID = ids.ToArray(typeof(int)) as int[];
	}
	
	public void AddBaseData(string n, string d, int count)
	{
		if(this.name.Length == 0)
		{
			for(int i=0; i<count; i++)
			{
				this.name = ArrayHelper.Add(new Language(n), this.name);
				this.description = ArrayHelper.Add(new Language(d), this.description);
			}
		}
		else
		{
			for(int i=0; i<count; i++)
			{
				this.name[i].Add(n);
				this.description[i].Add(d);
			}			
		}
		icon = ArrayHelper.Add("", icon);
	}
	
	public virtual void RemoveData(int index)
	{
		for(int i=0; i<this.name.Length; i++)
		{
			this.name[i].Remove(index);
			this.description[i].Remove(index);
		}
		icon = ArrayHelper.Remove(index, icon);
	}
	
	public virtual void Copy(int index)
	{
		for(int i=0; i<this.name.Length; i++)
		{
			this.name[i].Copy(index);
			this.description[i].Copy(index);
		}
		icon = ArrayHelper.Add(icon[index], icon);
	}
	
	public void AddLanguage(int lang)
	{
		if(this.name.Length > 0)
		{
			this.name = ArrayHelper.Add(new Language(this.name[0].text), this.name);
			this.description = ArrayHelper.Add(new Language(this.description[0].text), this.name);
		}
	}
	
	public void RemoveLanguage(int lang)
	{
		if(this.name.Length > 0)
		{
			this.name = ArrayHelper.Remove(lang, this.name);
			this.description = ArrayHelper.Remove(lang, this.description);
		}
	}
	
	public int CheckForIndex(int index, int check)
	{
		if(check == index) check = 0;
		else if(check > index) check -= 1;
		return check;
	}
	
	/*
	============================================================================
	Utility functions
	============================================================================
	*/
	public ArrayList SaveLanguages(ArrayList s, int i)
	{
		if(icon[i] != null && "" != icon[i])
		{
			Hashtable ic = new Hashtable();
			ic.Add(XMLHandler.NODE_NAME, BaseLangData.ICON);
			ic.Add(XMLHandler.CONTENT, icon[i]);
			s.Add(ic);
		}
		for(int j=0; j<name.Length; j++)
		{
			Hashtable n = new Hashtable();
			string text = name[j].text[i];
			if(text == "")
			{
				text = name[0].text[i];
			}
			n.Add(XMLHandler.NODE_NAME, BaseLangData.NAME);
			n.Add("lang", j.ToString());
			n.Add(XMLHandler.CONTENT, text);
			s.Add(n);
			
			Hashtable d = new Hashtable();
			text = description[j].text[i];
			if(text == "")
			{
				text = description[0].text[i];
			}
			d.Add(XMLHandler.NODE_NAME, BaseLangData.DESCRIPTION);
			d.Add("lang", j.ToString());
			d.Add(XMLHandler.CONTENT, text);
			s.Add(d);
		}
		return s;
	}
	
	public void LoadLanguages(Hashtable ht, int i, int subs)
	{
		if(ht[XMLHandler.NODE_NAME] as string == BaseLangData.NAME)
		{
			int lang = int.Parse((string)ht["lang"]);
			if(lang >= name.Length)
			{
				name = ArrayHelper.Add(new Language(subs), name);
			}
			name[lang].text[i] = ht[XMLHandler.CONTENT] as string;
		}
		else if(ht[XMLHandler.NODE_NAME] as string == BaseLangData.DESCRIPTION)
		{
			int lang = int.Parse((string)ht["lang"]);
			if(lang >= description.Length)
			{
				description = ArrayHelper.Add(new Language(subs), description);
			}
			description[lang].text[i] = ht[XMLHandler.CONTENT] as string;
		}
		else if(ht[XMLHandler.NODE_NAME] as string == BaseLangData.ICON)
		{
			icon[i] = ht[XMLHandler.CONTENT] as string;
		}
	}
}