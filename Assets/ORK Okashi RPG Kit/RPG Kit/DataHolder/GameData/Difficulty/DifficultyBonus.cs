using UnityEngine;
using System.Collections;

public class DifficultyBonus
{
	public int difficultyID = 0;
	
	public float hitBonus = 0;
	public float counterBonus = 0;
	public float criticalBonus = 0;
	public float blockBonus = 0;
	public float escapeBonus = 0;
	public float speedBonus = 0;
	public float itemStealBonus = 0;
	public float moneyStealBonus = 0;
	
	public int[] statusBonus = new int[0];
	
	public int[] elementBonus = new int[0];
	public int[] raceBonus = new int[0];
	public int[] sizeBonus = new int[0];
	// effects, ...
	
	// XML
	private static string STATUS = "status";
	private static string ELEMENT = "element";
	private static string RACE = "race";
	private static string SIZE = "size";
	
	public DifficultyBonus()
	{
		this.statusBonus = new int[DataHolder.StatusValueCount];
		this.elementBonus = new int[DataHolder.ElementCount];
		this.raceBonus = new int[DataHolder.RaceCount];
		this.sizeBonus = new int[DataHolder.SizeCount];
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		ht.Add("difficulty", this.difficultyID.ToString());
		
		ht.Add("hit", this.hitBonus.ToString());
		ht.Add("counter", this.counterBonus.ToString());
		ht.Add("critical", this.criticalBonus.ToString());
		ht.Add("block", this.blockBonus.ToString());
		ht.Add("escape", this.escapeBonus.ToString());
		ht.Add("speed", this.speedBonus.ToString());
		ht.Add("itemsteal", this.itemStealBonus.ToString());
		ht.Add("moneysteal", this.moneyStealBonus.ToString());
		
		ArrayList s = new ArrayList();
		for(int i=0; i<this.statusBonus.Length; i++)
		{
			if(this.statusBonus[i] != 0)
			{
				Hashtable ht2 = HashtableHelper.GetTitleHashtable(DifficultyBonus.STATUS, i);
				ht2.Add("value", this.statusBonus[i].ToString());
				s.Add(ht2);
			}
		}
		
		for(int i=0; i<this.elementBonus.Length; i++)
		{
			if(this.elementBonus[i] != 0)
			{
				Hashtable ht2 = HashtableHelper.GetTitleHashtable(DifficultyBonus.ELEMENT, i);
				ht2.Add("value", this.elementBonus[i].ToString());
				s.Add(ht2);
			}
		}
		
		for(int i=0; i<this.raceBonus.Length; i++)
		{
			if(this.raceBonus[i] != 0)
			{
				Hashtable ht2 = HashtableHelper.GetTitleHashtable(DifficultyBonus.RACE, i);
				ht2.Add("value", this.raceBonus[i].ToString());
				s.Add(ht2);
			}
		}
		
		for(int i=0; i<this.sizeBonus.Length; i++)
		{
			if(this.sizeBonus[i] != 0)
			{
				Hashtable ht2 = HashtableHelper.GetTitleHashtable(DifficultyBonus.SIZE, i);
				ht2.Add("value", this.sizeBonus[i].ToString());
				s.Add(ht2);
			}
		}
		
		if(s.Count > 0) ht.Add(XMLHandler.NODES, s);
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		this.difficultyID = int.Parse((string)ht["difficulty"]);
		
		this.hitBonus = float.Parse((string)ht["hit"]);
		this.counterBonus = float.Parse((string)ht["counter"]);
		this.criticalBonus = float.Parse((string)ht["critical"]);
		this.blockBonus = float.Parse((string)ht["block"]);
		this.escapeBonus = float.Parse((string)ht["escape"]);
		this.speedBonus = float.Parse((string)ht["speed"]);
		this.itemStealBonus = float.Parse((string)ht["block"]);
		this.moneyStealBonus = float.Parse((string)ht["block"]);
		
		if(ht.ContainsKey(XMLHandler.NODES))
		{
			ArrayList s = ht[XMLHandler.NODES] as ArrayList;
			foreach(Hashtable ht2 in s)
			{
				if(ht2[XMLHandler.NODE_NAME] as string == DifficultyBonus.STATUS)
				{
					int id = int.Parse((string)ht2["id"]);
					if(id < this.statusBonus.Length) this.statusBonus[id] = int.Parse((string)ht2["value"]);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == DifficultyBonus.ELEMENT)
				{
					int id = int.Parse((string)ht2["id"]);
					if(id < this.elementBonus.Length) this.elementBonus[id] = int.Parse((string)ht2["value"]);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == DifficultyBonus.RACE)
				{
					int id = int.Parse((string)ht2["id"]);
					if(id < this.raceBonus.Length) this.raceBonus[id] = int.Parse((string)ht2["value"]);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == DifficultyBonus.SIZE)
				{
					int id = int.Parse((string)ht2["id"]);
					if(id < this.sizeBonus.Length) this.sizeBonus[id] = int.Parse((string)ht2["value"]);
				}
			}
		}
	}
	
	/*
	============================================================================
	Editor functions
	============================================================================
	*/
	public void AddElement()
	{
		this.elementBonus = ArrayHelper.Add(0, this.elementBonus);
	}
	
	public void RemoveElement(int index)
	{
		this.elementBonus = ArrayHelper.Remove(index, this.elementBonus);
	}
	
	public void AddRace()
	{
		this.raceBonus = ArrayHelper.Add(0, this.raceBonus);
	}
	
	public void RemoveRace(int index)
	{
		this.raceBonus = ArrayHelper.Remove(index, this.raceBonus);
	}
	
	public void AddSize()
	{
		this.sizeBonus = ArrayHelper.Add(0, this.sizeBonus);
	}
	
	public void RemoveSize(int index)
	{
		this.sizeBonus = ArrayHelper.Remove(index, this.sizeBonus);
	}
}
