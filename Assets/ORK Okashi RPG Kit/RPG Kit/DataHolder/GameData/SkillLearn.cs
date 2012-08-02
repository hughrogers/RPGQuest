
using UnityEngine;
using System.Collections;

public class SkillLearn
{
	public int atLevel = 1;
	public int skillID = 0;
	public int skillLevel = 1;
	
	// ingame
	private int useLevel = 1;
	
	public SkillLearn()
	{
		
	}
	
	public SkillLearn(int id, int lvl)
	{
		this.skillID = id;
		this.skillLevel = lvl;
		this.useLevel = lvl;
	}
	
	public SkillLearn(Hashtable ht, bool saveGame)
	{
		this.SetData(ht, saveGame);
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht, bool saveGame)
	{
		if(saveGame)
		{
			ht.Add("id", this.skillID.ToString());
			ht.Add("lvl", this.skillLevel.ToString());
		}
		else
		{
			ht.Add("level", this.atLevel.ToString());
			ht.Add("skill", this.skillID.ToString());
			ht.Add("slvl", this.skillLevel.ToString());
		}
		return ht;
	}
	
	public void SetData(Hashtable ht, bool saveGame)
	{
		if(saveGame)
		{
			this.skillID = int.Parse((string)ht["id"]);
			if(ht.ContainsKey("lvl")) this.skillLevel = int.Parse((string)ht["lvl"]);
		}
		else
		{
			this.atLevel = int.Parse((string)ht["level"]);
			this.skillID = int.Parse((string)ht["skill"]);
			if(ht.ContainsKey("slvl")) this.skillLevel = int.Parse((string)ht["slvl"]);
		}
	}
	
	public SkillLearn GetCopy()
	{
		SkillLearn s = new SkillLearn();
		
		s.atLevel = this.atLevel;
		s.skillID = this.skillID;
		s.skillLevel = this.skillLevel;
		s.useLevel = this.useLevel;
		
		return s;
	}
	
	/*
	============================================================================
	Use level functions
	============================================================================
	*/
	public int GetLevel()
	{
		int lvl = 0;
		if(DataHolder.Skill(this.skillID).isPassive) lvl = this.skillLevel-1;
		else lvl = this.useLevel-1;
		return lvl;
	}
	
	public void SetUseLevel(int lvl)
	{
		this.useLevel = lvl;
	}
	
	public void ChangeUseLevel(int change)
	{
		this.useLevel += change;
		if(DataHolder.GameSettings().loopSkillLevels)
		{
			if(this.useLevel < 1) this.useLevel = this.skillLevel;
			else if(this.useLevel > this.skillLevel) this.useLevel = 1;
		}
		else
		{
			if(this.useLevel < 1) this.useLevel = 1;
			else if(this.useLevel > this.skillLevel) this.useLevel = this.skillLevel;
		}
	}
	
	/*
	============================================================================
	Utility functions
	============================================================================
	*/
	public string GetName()
	{
		return DataHolder.GameSettings().GetSkillLevelName(
				DataHolder.Skills().GetName(this.skillID), this.GetLevel()+1);
	}
	
	public string GetDescription()
	{
		return DataHolder.Skills().GetDescription(this.skillID);
	}
	
	public Texture2D GetIcon()
	{
		return DataHolder.Skills().GetIcon(this.skillID);
	}
	
	public GUIContent GetContent()
	{
		GUIContent gc = DataHolder.Skills().GetContent(this.skillID);
		gc.text = this.GetName();
		return gc;
	}
	
	public ChoiceContent GetChoiceContent(HUDContentType type)
	{
		ChoiceContent cc = DataHolder.Skills().GetChoiceContent(this.skillID, type);
		cc.content.text = this.GetName();
		return cc;
	}
	
	/*
	============================================================================
	Use functions
	============================================================================
	*/
	public bool CanUse(Character c)
	{
		return !DataHolder.Skill(this.skillID).isPassive && 
				DataHolder.Skill(this.skillID).CanUse(c, this.useLevel-1);
	}
	
	public void SetHighestUseLevel(Character c)
	{
		if(!DataHolder.Skill(this.skillID).isPassive)
		{
			int lvl = this.skillLevel;
			for(int i=lvl; i>0; i--)
			{
				if(DataHolder.Skill(this.skillID).CanUse(c, i-1))
				{
					lvl = i;
					break;
				}
			}
			this.useLevel = lvl;
		}
	}
}
