
using UnityEngine;
using System.Collections;

public class BattleAdvantage
{
	public bool enabled = false;
	public float chance = 0;
	public bool blockEscape = false;
	
	public bool overrideText = false;
	public int textColor = 0;
	public int shadowColor = 1;
	public string[] text;
	
	public GroupCondition partyCondition = new GroupCondition();
	public GroupCondition enemiesCondition = new GroupCondition();
	
	// advantage index
	public static int NONE = 0;
	public static int PARTY = 1;
	public static int ENEMIES = 2;
	
	public BattleAdvantage()
	{
		this.text = new string[DataHolder.LanguageCount];
		for(int i=0; i<this.text.Length; i++)
		{
			this.text[i] = "";
		}
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		if(this.enabled)
		{
			ArrayList s = new ArrayList();
			
			ht.Add("chance", this.chance.ToString());
			if(this.blockEscape) ht.Add("blockescape", "true");
			
			if(this.overrideText)
			{
				ht.Add("textcolor", this.textColor.ToString());
				ht.Add("shadowcolor", this.shadowColor.ToString());
				HashtableHelper.AddContentHashtables(ref s, XMLName.TEXT, this.text);
			}
			
			s.Add(this.partyCondition.GetData(
					HashtableHelper.GetTitleHashtable(XMLName.PARTY)));
			s.Add(this.enemiesCondition.GetData(
					HashtableHelper.GetTitleHashtable(XMLName.ENEMIES)));
			
			if(s.Count > 0) ht.Add(XMLHandler.NODES, s);
		}
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		if(ht.ContainsKey("chance"))
		{
			this.enabled = true;
			this.chance = float.Parse((string)ht["chance"]);
			
			if(ht.ContainsKey("blockescape")) this.blockEscape = true;
			
			if(ht.ContainsKey("textcolor"))
			{
				this.overrideText = true;
				this.textColor = int.Parse((string)ht["textcolor"]);
				this.shadowColor = int.Parse((string)ht["shadowcolor"]);
			}
			
			if(ht.ContainsKey(XMLHandler.NODES))
			{
				ArrayList s = ht[XMLHandler.NODES] as ArrayList;
				foreach(Hashtable ht2 in s)
				{
					if(ht2[XMLHandler.NODE_NAME] as string == XMLName.TEXT)
					{
						HashtableHelper.GetContentString(ht2, ref this.text);
					}
					else if(ht2[XMLHandler.NODE_NAME] as string == XMLName.PARTY)
					{
						this.partyCondition.SetData(ht2);
					}
					else if(ht2[XMLHandler.NODE_NAME] as string == XMLName.ENEMIES)
					{
						this.enemiesCondition.SetData(ht2);
					}
				}
			}
		}
	}
	
	/*
	============================================================================
	Text functions
	============================================================================
	*/
	public string GetText()
	{
		string txt = "";
		int lang = GameHandler.GetLanguage();
		if(lang < this.text.Length && this.text[lang] != "")
		{
			txt = this.text[lang];
		}
		else txt = this.text[0];
		return txt;
	}
	
	public void ShowText()
	{
		if(this.overrideText)
		{
			GameHandler.GetLevelHandler().ShowBattleMessage(this.GetText(), this.textColor, this.shadowColor);
		}
	}
}
