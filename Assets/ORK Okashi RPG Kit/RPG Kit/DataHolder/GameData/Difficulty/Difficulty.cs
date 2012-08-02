
using UnityEngine;
using System.Collections;

public class Difficulty
{
	public float timeFactor = 1;
	public float movementFactor = 1;
	public float battleFactor = 1;
	public float animationFactor = 1;
	
	// bonuses
	public float[] statusMultiplier;
	public float[] elementMultiplier;
	public float[] raceMultiplier;
	public float[] sizeMultiplier;
	
	// XML
	private static string STATUSMULTIPLIER = "statusmultiplier";
	private static string ELEMENTMULTIPLIER = "elementmultiplier";
	private static string RACEMULTIPLIER = "racemultiplier";
	private static string SIZEMULTIPLIER = "sizemultiplier";
	
	public Difficulty()
	{
		this.statusMultiplier = new float[DataHolder.StatusValueCount];
		for(int i=0; i<this.statusMultiplier.Length; i++)
		{
			this.statusMultiplier[i] = 1;
		}
		this.elementMultiplier = new float[DataHolder.ElementCount];
		for(int i=0; i<this.elementMultiplier.Length; i++)
		{
			this.elementMultiplier[i] = 1;
		}
		this.raceMultiplier = new float[DataHolder.RaceCount];
		for(int i=0; i<this.raceMultiplier.Length; i++)
		{
			this.raceMultiplier[i] = 1;
		}
		this.sizeMultiplier = new float[DataHolder.SizeCount];
		for(int i=0; i<this.sizeMultiplier.Length; i++)
		{
			this.sizeMultiplier[i] = 1;
		}
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		if(this.timeFactor != 1) ht.Add("timefactor", this.timeFactor.ToString());
		if(this.movementFactor != 1) ht.Add("movementfactor", this.movementFactor.ToString());
		if(this.battleFactor != 1) ht.Add("battlefactor", this.battleFactor.ToString());
		if(this.animationFactor != 1) ht.Add("animationfactor", this.animationFactor.ToString());
		
		ArrayList s = new ArrayList();
		
		for(int i=0; i<this.statusMultiplier.Length; i++)
		{
			if(this.statusMultiplier[i] != 1)
			{
				Hashtable ht2 = HashtableHelper.GetTitleHashtable(Difficulty.STATUSMULTIPLIER, i);
				ht2.Add("value", this.statusMultiplier[i].ToString());
				s.Add(ht2);
			}
		}
		for(int i=0; i<this.elementMultiplier.Length; i++)
		{
			if(this.elementMultiplier[i] != 1)
			{
				Hashtable ht2 = HashtableHelper.GetTitleHashtable(Difficulty.ELEMENTMULTIPLIER, i);
				ht2.Add("value", this.elementMultiplier[i].ToString());
				s.Add(ht2);
			}
		}
		for(int i=0; i<this.raceMultiplier.Length; i++)
		{
			if(this.raceMultiplier[i] != 1)
			{
				Hashtable ht2 = HashtableHelper.GetTitleHashtable(Difficulty.RACEMULTIPLIER, i);
				ht2.Add("value", this.raceMultiplier[i].ToString());
				s.Add(ht2);
			}
		}
		for(int i=0; i<this.sizeMultiplier.Length; i++)
		{
			if(this.sizeMultiplier[i] != 1)
			{
				Hashtable ht2 = HashtableHelper.GetTitleHashtable(Difficulty.SIZEMULTIPLIER, i);
				ht2.Add("value", this.sizeMultiplier[i].ToString());
				s.Add(ht2);
			}
		}
		
		if(s.Count > 0) ht.Add(XMLHandler.NODES, s);
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		if(ht.ContainsKey("timefactor"))
		{
			this.timeFactor = float.Parse((string)ht["timefactor"]);
		}
		if(ht.ContainsKey("movementfactor"))
		{
			this.movementFactor = float.Parse((string)ht["movementfactor"]);
		}
		if(ht.ContainsKey("battlefactor"))
		{
			this.battleFactor = float.Parse((string)ht["battlefactor"]);
		}
		if(ht.ContainsKey("animationfactor"))
		{
			this.animationFactor = float.Parse((string)ht["animationfactor"]);
		}
		
		if(ht.ContainsKey(XMLHandler.NODES))
		{
			ArrayList s = ht[XMLHandler.NODES] as ArrayList;
			foreach(Hashtable ht2 in s)
			{
				if(ht2[XMLHandler.NODE_NAME] as string == Difficulty.STATUSMULTIPLIER)
				{
					int id = int.Parse((string)ht2["id"]);
					if(id < this.statusMultiplier.Length) this.statusMultiplier[id] = float.Parse((string)ht2["value"]);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == Difficulty.ELEMENTMULTIPLIER)
				{
					int id = int.Parse((string)ht2["id"]);
					if(id < this.elementMultiplier.Length) this.elementMultiplier[id] = float.Parse((string)ht2["value"]);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == Difficulty.RACEMULTIPLIER)
				{
					int id = int.Parse((string)ht2["id"]);
					if(id < this.raceMultiplier.Length) this.raceMultiplier[id] = float.Parse((string)ht2["value"]);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == Difficulty.SIZEMULTIPLIER)
				{
					int id = int.Parse((string)ht2["id"]);
					if(id < this.sizeMultiplier.Length) this.sizeMultiplier[id] = float.Parse((string)ht2["value"]);
				}
			}
		}
	}
	
	public Difficulty GetCopy()
	{
		Difficulty d = new Difficulty();
		d.SetData(this.GetData(new Hashtable()));
		return d;
	}
}
