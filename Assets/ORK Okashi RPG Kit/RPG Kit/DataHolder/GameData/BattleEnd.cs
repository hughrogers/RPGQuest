
using System.Collections;

public class BattleEnd
{
	// general settings
	public int dialoguePosition = 0;
	public bool splitExp = false;
	public bool getImmediately = false;
	public bool dropItems = false;
	public bool dropMoney = false;
	
	// victory gains
	public bool showGains = false;
	public ArrayList moneyText = new ArrayList();
	public ArrayList itemText = new ArrayList();
	public ArrayList experienceText = new ArrayList();
	
	public string[] gainOrder = new string[] {BattleEnd.MONEY, BattleEnd.ITEM, BattleEnd.EXPERIENCE};
	public static string MONEY = "Money";
	public static string ITEM = "Item";
	public static string EXPERIENCE = "Experience";
	
	// level up
	public bool showLevelUp = false;
	public ArrayList levelUpText = new ArrayList();
	public ArrayList classLevelUpText = new ArrayList();
	public ArrayList statusValueText = new ArrayList();
	public ArrayList skillText = new ArrayList();
	
	public string[] levelUpOrder = new string[] {BattleEnd.LEVELUP, BattleEnd.STATUS, BattleEnd.SKILL};
	public static string LEVELUP = "Levelup";
	public static string STATUS = "Status";
	public static string SKILL = "Skill";
	
	// replacement strings
	public static string REPLACE_NUMBER = "%";
	public static string REPLACE_NAME = "%n";
	public static string REPLACE_CLASS = "%c";
	
	public BattleEnd()
	{
		for(int i=0; i<DataHolder.Languages().GetDataCount(); i++)
		{
			this.moneyText.Add("You got % money!");
			this.itemText.Add("You found % %n.");
			this.experienceText.Add("% %n gained.");
			this.levelUpText.Add("%n reached level %!");
			this.classLevelUpText.Add("%n reached class level %!");
			this.statusValueText.Add("%n +%");
			this.skillText.Add("%n learned!");
		}
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		ArrayList s = new ArrayList();
		ht.Add("pos", this.dialoguePosition.ToString());
		if(this.splitExp) ht.Add("splitexp", "true");
		if(this.getImmediately)
		{
			ht.Add("getimmediately", "true");
			if(this.dropItems) ht.Add("dropitems", "true");
			if(this.dropMoney) ht.Add("dropmoney", "true");
		}
		if(this.showGains)
		{
			ht.Add("gainorder", this.GetGainOrder());
			for(int i=0; i<this.moneyText.Count; i++)
			{
				s.Add(HashtableHelper.GetContentHashtable(BattleSystemData.MONEY, 
						this.moneyText[i] as string, i));
			}
			for(int i=0; i<this.itemText.Count; i++)
			{
				s.Add(HashtableHelper.GetContentHashtable(BattleSystemData.ITEM, 
						this.itemText[i] as string, i));
			}
		}
		if(this.showLevelUp)
		{
			ht.Add("levelorder", this.GetLevelOrder());
			for(int i=0; i<this.levelUpText.Count; i++)
			{
				s.Add(HashtableHelper.GetContentHashtable(BattleSystemData.LEVELUP, 
						this.levelUpText[i] as string, i));
			}
			for(int i=0; i<this.classLevelUpText.Count; i++)
			{
				s.Add(HashtableHelper.GetContentHashtable(BattleSystemData.CLASSLEVELUP, 
						this.classLevelUpText[i] as string, i));
			}
			for(int i=0; i<this.statusValueText.Count; i++)
			{
				s.Add(HashtableHelper.GetContentHashtable(BattleSystemData.STATUS, 
						this.statusValueText[i] as string, i));
			}
			for(int i=0; i<this.skillText.Count; i++)
			{
				s.Add(HashtableHelper.GetContentHashtable(BattleSystemData.SKILL, 
						this.skillText[i] as string, i));
			}
			for(int i=0; i<this.experienceText.Count; i++)
			{
				s.Add(HashtableHelper.GetContentHashtable(BattleSystemData.EXPERIENCE, 
						this.experienceText[i] as string, i));
			}
		}
		ht.Add(XMLHandler.NODES, s);
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		this.dialoguePosition = int.Parse((string)ht["pos"]);
		if(ht.ContainsKey("splitexp")) this.splitExp = true;
		if(ht.ContainsKey("getimmediately"))
		{
			this.getImmediately = true;
			if(ht.ContainsKey("dropitems")) this.dropItems = true;
			if(ht.ContainsKey("dropmoney")) this.dropMoney = true;
		}
		if(ht.ContainsKey("gainorder"))
		{
			this.showGains = true;
			this.SetGainOrder(ht["gainorder"] as string);
		}
		if(ht.ContainsKey("levelorder"))
		{
			this.showLevelUp = true;
			this.SetLevelOrder(ht["levelorder"] as string);
		}
		
		if(ht.ContainsKey(XMLHandler.NODES))
		{
			ArrayList s = ht[XMLHandler.NODES] as ArrayList;
			foreach(Hashtable ht2 in s)
			{
				if(ht2[XMLHandler.NODE_NAME] as string == BattleSystemData.MONEY)
				{
					int id = int.Parse((string)ht2["id"]);
					if(id < this.moneyText.Count) this.moneyText[id] = ht2[XMLHandler.CONTENT];
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == BattleSystemData.ITEM)
				{
					int id = int.Parse((string)ht2["id"]);
					if(id < this.itemText.Count) this.itemText[id] = ht2[XMLHandler.CONTENT];
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == BattleSystemData.LEVELUP)
				{
					int id = int.Parse((string)ht2["id"]);
					if(id < this.levelUpText.Count) this.levelUpText[id] = ht2[XMLHandler.CONTENT];
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == BattleSystemData.CLASSLEVELUP)
				{
					int id = int.Parse((string)ht2["id"]);
					if(id < this.classLevelUpText.Count) this.classLevelUpText[id] = ht2[XMLHandler.CONTENT];
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == BattleSystemData.STATUS)
				{
					int id = int.Parse((string)ht2["id"]);
					if(id < this.statusValueText.Count) this.statusValueText[id] = ht2[XMLHandler.CONTENT];
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == BattleSystemData.SKILL)
				{
					int id = int.Parse((string)ht2["id"]);
					if(id < this.skillText.Count) this.skillText[id] = ht2[XMLHandler.CONTENT];
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == BattleSystemData.EXPERIENCE)
				{
					int id = int.Parse((string)ht2["id"]);
					if(id < this.experienceText.Count) this.experienceText[id] = ht2[XMLHandler.CONTENT];
				}
			}
		}
	}
	
	/*
	============================================================================
	Order functions
	============================================================================
	*/
	public string GetGainOrder()
	{
		string text = "";
		for(int i=0; i<this.gainOrder.Length; i++)
		{
			if(i>0) text += ":";
			text += this.gainOrder[i];
		}
		return text;
	}
	
	public void SetGainOrder(string text)
	{
		this.gainOrder = text.Split(new char[] {':'});
	}
	
	public string GetLevelOrder()
	{
		string text = "";
		for(int i=0; i<this.levelUpOrder.Length; i++)
		{
			if(i>0) text += ":";
			text += this.levelUpOrder[i];
		}
		return text;
	}
	
	public void SetLevelOrder(string text)
	{
		this.levelUpOrder = text.Split(new char[] {':'});
	}
	
	public void GainMoveUp(int index)
	{
		string tmp = this.gainOrder[index-1];
		this.gainOrder[index-1] = this.gainOrder[index];
		this.gainOrder[index] = tmp;
	}
	
	public void GainMoveDown(int index)
	{
		string tmp = this.gainOrder[index+1];
		this.gainOrder[index+1] = this.gainOrder[index];
		this.gainOrder[index] = tmp;
	}
	
	public void LevelMoveUp(int index)
	{
		string tmp = this.levelUpOrder[index-1];
		this.levelUpOrder[index-1] = this.levelUpOrder[index];
		this.levelUpOrder[index] = tmp;
	}
	
	public void LevelMoveDown(int index)
	{
		string tmp = this.levelUpOrder[index+1];
		this.levelUpOrder[index+1] = this.levelUpOrder[index];
		this.levelUpOrder[index] = tmp;
	}
	
	/*
	============================================================================
	Text functions
	============================================================================
	*/
	public string GetItemText(int itemID, int count, ItemDropType type)
	{
		string text = this.itemText[GameHandler.GetLanguage()] as string;
		if(ItemDropType.ITEM.Equals(type))
		{
			text = text.Replace(BattleEnd.REPLACE_NAME, DataHolder.Items().GetName(itemID));
		}
		else if(ItemDropType.WEAPON.Equals(type))
		{
			text = text.Replace(BattleEnd.REPLACE_NAME, DataHolder.Weapons().GetName(itemID));
		}
		else if(ItemDropType.ARMOR.Equals(type))
		{
			text = text.Replace(BattleEnd.REPLACE_NAME, DataHolder.Armors().GetName(itemID));
		}
		text = text.Replace(BattleEnd.REPLACE_NUMBER, count.ToString());
		return text;
	}
	
	public string GetMoneyText(int count)
	{
		string text = this.moneyText[GameHandler.GetLanguage()] as string;
		text = text.Replace(BattleEnd.REPLACE_NUMBER, count.ToString());
		return text;
	}
	
	public string GetExperienceText(int statusID, int count)
	{
		string text = this.experienceText[GameHandler.GetLanguage()] as string;
		text = text.Replace(BattleEnd.REPLACE_NAME, DataHolder.StatusValues().GetName(statusID));
		text = text.Replace(BattleEnd.REPLACE_NUMBER, count.ToString());
		return text;
	}
	
	public string GetLevelUpText(int charID, int level)
	{
		string text = this.levelUpText[GameHandler.GetLanguage()] as string;
		text = text.Replace(BattleEnd.REPLACE_NAME, DataHolder.Characters().GetName(charID));
		text = text.Replace(BattleEnd.REPLACE_NUMBER, level.ToString());
		return text;
	}
	
	public string GetClassLevelUpText(int charID, int classID, int level)
	{
		string text = this.classLevelUpText[GameHandler.GetLanguage()] as string;
		text = text.Replace(BattleEnd.REPLACE_NAME, DataHolder.Characters().GetName(charID));
		text = text.Replace(BattleEnd.REPLACE_NUMBER, level.ToString());
		text = text.Replace(BattleEnd.REPLACE_CLASS, DataHolder.Classes().GetName(classID));
		return text;
	}
	
	public string GetStatusValueText(int statusID, int count)
	{
		string text = this.statusValueText[GameHandler.GetLanguage()] as string;
		text = text.Replace(BattleEnd.REPLACE_NAME, DataHolder.StatusValues().GetName(statusID));
		text = text.Replace(BattleEnd.REPLACE_NUMBER, count.ToString());
		return text;
	}
	
	public string GetSkillText(int skillID, int skillLevel)
	{
		string text = this.skillText[GameHandler.GetLanguage()] as string;
		text = text.Replace(BattleEnd.REPLACE_NAME, 
				DataHolder.GameSettings().GetSkillLevelName(
				DataHolder.Skills().GetName(skillID), skillLevel));
		return text;
	}
}