
using System.Collections;
using UnityEngine;

public class DifficultyData : BaseLangData
{
	public Difficulty[] difficulty = new Difficulty[0];
	
	// XML data
	private string filename = "difficulties";
	
	private static string DIFFICULTIES = "difficulties";
	private static string DIFFICULTY = "difficulty";
	private static string DATA = "data";

	public DifficultyData()
	{
		LoadData();
	}
	
	public override string GetIconPath() { return "Icons/Difficulty/"; }
	
	public void LoadData()
	{
		ArrayList data = XMLHandler.LoadXML(dir+filename);
		
		if(data.Count > 0)
		{
			foreach(Hashtable entry in data)
			{
				if(entry[XMLHandler.NODE_NAME] as string == DifficultyData.DIFFICULTIES)
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList subs = entry[XMLHandler.NODES] as ArrayList;
						this.icon = new string[subs.Count];
						this.difficulty = new Difficulty[subs.Count];
						
						foreach(Hashtable val in subs)
						{
							if(val[XMLHandler.NODE_NAME] as string == DifficultyData.DIFFICULTY)
							{
								int i = int.Parse((string)val["id"]);
								this.icon[i] = "";
								
								this.difficulty[i] = new Difficulty();
								
								ArrayList s = val[XMLHandler.NODES] as ArrayList;
								foreach(Hashtable ht in s)
								{
									this.LoadLanguages(ht, i, subs.Count);
									if(ht[XMLHandler.NODE_NAME] as string == DifficultyData.DATA)
									{
										this.difficulty[i].SetData(ht);
									}
								}
							}
						}
					}
				}
			}
		}
		else
		{
			this.AddDifficulty("Default Difficulty", "", DataHolder.LanguageCount);
		}
	}
	
	public void SaveData()
	{
		if(name.Length > 0)
		{
			ArrayList data = new ArrayList();
			ArrayList subs = new ArrayList();
			
			Hashtable sv = HashtableHelper.GetTitleHashtable(DifficultyData.DIFFICULTIES);
			
			for(int i=0; i<name[0].Count(); i++)
			{
				ArrayList s = new ArrayList();
				Hashtable val = HashtableHelper.GetTitleHashtable(DifficultyData.DIFFICULTY, i);
				
				s.Add(this.difficulty[i].GetData(HashtableHelper.GetTitleHashtable(DifficultyData.DATA)));
				s = this.SaveLanguages(s, i);
				
				val.Add(XMLHandler.NODES, s);
				subs.Add(val);
			}
			sv.Add(XMLHandler.NODES, subs);
			data.Add(sv);
			
			XMLHandler.SaveXML(dir, filename, data);
		}
	}
	
	/*
	============================================================================
	Add/remove/copy functions
	============================================================================
	*/
	public void AddDifficulty(string n, string d, int count)
	{
		base.AddBaseData(n, d, count);
		this.difficulty = ArrayHelper.Add(new Difficulty(), this.difficulty);
	}
	
	public override void RemoveData(int index)
	{
		base.RemoveData(index);
		this.difficulty = ArrayHelper.Remove(index, this.difficulty);
	}
	
	public override void Copy(int index)
	{
		base.Copy(index);
		this.difficulty = ArrayHelper.Add(this.difficulty[index].GetCopy(), this.difficulty);
	}
	
	/*
	============================================================================
	Status value functions
	============================================================================
	*/
	public void AddStatusValue(int index)
	{
		for(int i=0; i<this.difficulty.Length; i++)
		{
			this.difficulty[i].statusMultiplier = ArrayHelper.Add(1, this.difficulty[i].statusMultiplier);
		}
	}
	
	public void SetStatusValueType(int index, StatusValueType type)
	{
		for(int i=0; i<this.difficulty.Length; i++)
		{
			if(StatusValueType.CONSUMABLE.Equals(type))
			{
				this.difficulty[i].statusMultiplier[index] = 1;
			}
		}
	}
	
	public void RemoveStatusValue(int index)
	{
		for(int i=0; i<this.difficulty.Length; i++)
		{
			this.difficulty[i].statusMultiplier = ArrayHelper.Remove(index, this.difficulty[i].statusMultiplier);
		}
	}
	
	/*
	============================================================================
	Menu functions
	============================================================================
	*/
	public ChoiceContent[] GetDifficultyChoice(bool addCancel)
	{
		ChoiceContent[] choice = new ChoiceContent[this.difficulty.Length];
		for(int i=0; i<choice.Length; i++)
		{
			choice[i] = this.GetChoiceContent(i, HUDContentType.BOTH);
		}
		if(addCancel)
		{
			choice = ArrayHelper.Add(new ChoiceContent(new GUIContent(DataHolder.MainMenu().GetCancelText())), choice);
		}
		return choice;
	}
	
	/*
	============================================================================
	In-game functions
	============================================================================
	*/
	public float GetTimeFactor()
	{
		float factor = 1;
		if(GameHandler.GetDifficulty() >= 0 && 
			GameHandler.GetDifficulty() < this.difficulty.Length)
		{
			factor = this.difficulty[GameHandler.GetDifficulty()].timeFactor;
		}
		return factor;
	}
	
	public float GetMovementFactor()
	{
		float factor = 1;
		if(GameHandler.GetDifficulty() >= 0 && 
			GameHandler.GetDifficulty() < this.difficulty.Length)
		{
			factor = this.difficulty[GameHandler.GetDifficulty()].movementFactor;
		}
		return factor;
	}
	
	public float GetBattleFactor()
	{
		float factor = 1;
		if(GameHandler.GetDifficulty() >= 0 && 
			GameHandler.GetDifficulty() < this.difficulty.Length)
		{
			factor = this.difficulty[GameHandler.GetDifficulty()].battleFactor;
		}
		return factor;
	}
	
	public float GetAnimationFactor()
	{
		float factor = 1;
		if(GameHandler.GetDifficulty() >= 0 && 
			GameHandler.GetDifficulty() < this.difficulty.Length)
		{
			factor = this.difficulty[GameHandler.GetDifficulty()].animationFactor;
		}
		return factor;
	}
}