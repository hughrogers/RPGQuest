
using UnityEngine;
using System.Collections;

public class BonusSettings
{
	public float hitBonus = 0;
	public float counterBonus = 0;
	public float criticalBonus = 0;
	public float blockBonus = 0;
	public float escapeBonus = 0;
	public float speedBonus = 0;
	public float itemStealBonus = 0;
	public float moneyStealBonus = 0;
	
	public int[] statusBonus = new int[0];
	
	public DifficultyBonus[] difficultyBonus = new DifficultyBonus[0];
	
	// XML
	private static string STATUS = "status";
	private static string DIFFICULTYBONUS = "difficultybonus";
	
	public BonusSettings()
	{
		this.statusBonus = new int[DataHolder.StatusValueCount];
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
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
				Hashtable ht2 = HashtableHelper.GetTitleHashtable(BonusSettings.STATUS, i);
				ht2.Add("value", this.statusBonus[i].ToString());
				s.Add(ht2);
			}
		}
		if(this.difficultyBonus.Length > 0)
		{
			ht.Add("difficultybonuses", this.difficultyBonus.Length.ToString());
			
			for(int i=0; i<this.difficultyBonus.Length; i++)
			{
				s.Add(this.difficultyBonus[i].GetData(
						HashtableHelper.GetTitleHashtable(BonusSettings.DIFFICULTYBONUS, i)));
			}
		}
		if(s.Count > 0) ht.Add(XMLHandler.NODES, s);
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		this.hitBonus = float.Parse((string)ht["hit"]);
		this.counterBonus = float.Parse((string)ht["counter"]);
		this.criticalBonus = float.Parse((string)ht["critical"]);
		if(ht.ContainsKey("block"))
		{
			this.blockBonus = float.Parse((string)ht["block"]);
		}
		this.escapeBonus = float.Parse((string)ht["escape"]);
		this.speedBonus = float.Parse((string)ht["speed"]);
		if(ht.ContainsKey("itemsteal"))
		{
			this.itemStealBonus = float.Parse((string)ht["block"]);
		}
		if(ht.ContainsKey("moneysteal"))
		{
			this.moneyStealBonus = float.Parse((string)ht["block"]);
		}
		
		if(ht.ContainsKey("difficultybonuses"))
		{
			this.difficultyBonus = new DifficultyBonus[int.Parse((string)ht["difficultybonuses"])];
			for(int i=0; i<this.difficultyBonus.Length; i++)
			{
				this.difficultyBonus[i] = new DifficultyBonus();
			}
		}
		
		if(ht.ContainsKey(XMLHandler.NODES))
		{
			ArrayList s = ht[XMLHandler.NODES] as ArrayList;
			foreach(Hashtable ht2 in s)
			{
				if(ht2[XMLHandler.NODE_NAME] as string == BonusSettings.STATUS)
				{
					int id = int.Parse((string)ht2["id"]);
					if(id < this.statusBonus.Length) this.statusBonus[id] = int.Parse((string)ht2["value"]);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == BonusSettings.DIFFICULTYBONUS)
				{
					int id = int.Parse((string)ht2["id"]);
					if(id < this.difficultyBonus.Length)
					{
						this.difficultyBonus[id].SetData(ht2);
					}
				}
			}
		}
	}
	
	/*
	============================================================================
	Editor functions
	============================================================================
	*/
	public void AddStatusValue()
	{
		this.statusBonus = ArrayHelper.Add(0, this.statusBonus);
	}
	
	public void RemoveStatusValue(int index)
	{
		this.statusBonus = ArrayHelper.Remove(index, this.statusBonus);
	}
	
	public void SetStatusValueType(int index, StatusValueType val)
	{
		if(StatusValueType.CONSUMABLE.Equals(val) || 
			StatusValueType.EXPERIENCE.Equals(val))
		{
			this.statusBonus[index] = 0;
		}
	}
	
	public void RemoveDifficulty(int id)
	{
		DifficultyBonus db = this.GetDifficultyBonus(id);
		if(db != null)
		{
			this.difficultyBonus = ArrayHelper.Remove(db, this.difficultyBonus);
		}
		for(int i=0; i<this.difficultyBonus.Length; i++)
		{
			if(this.difficultyBonus[i].difficultyID > id)
			{
				this.difficultyBonus[i].difficultyID--;
			}
		}
	}
	
	public void AddDifficultyBonus()
	{
		this.difficultyBonus = ArrayHelper.Add(new DifficultyBonus(), this.difficultyBonus);
	}
	
	public void RemoveDifficultyBonus(int index)
	{
		this.difficultyBonus = ArrayHelper.Remove(index, this.difficultyBonus);
	}
	
	public void AddElement()
	{
		for(int i=0; i<this.difficultyBonus.Length; i++)
		{
			this.difficultyBonus[i].AddElement();
		}
	}
	
	public void RemoveElement(int index)
	{
		for(int i=0; i<this.difficultyBonus.Length; i++)
		{
			this.difficultyBonus[i].RemoveElement(index);
		}
	}
	
	public void AddRace()
	{
		for(int i=0; i<this.difficultyBonus.Length; i++)
		{
			this.difficultyBonus[i].AddRace();
		}
	}
	
	public void RemoveRace(int index)
	{
		for(int i=0; i<this.difficultyBonus.Length; i++)
		{
			this.difficultyBonus[i].RemoveRace(index);
		}
	}
	
	public void AddSize()
	{
		for(int i=0; i<this.difficultyBonus.Length; i++)
		{
			this.difficultyBonus[i].AddSize();
		}
	}
	
	public void RemoveSize(int index)
	{
		for(int i=0; i<this.difficultyBonus.Length; i++)
		{
			this.difficultyBonus[i].RemoveSize(index);
		}
	}
	
	/*
	============================================================================
	In-game functions
	============================================================================
	*/
	public DifficultyBonus GetDifficultyBonus(int id)
	{
		DifficultyBonus db = null;
		for(int i=0; i<this.difficultyBonus.Length; i++)
		{
			if(this.difficultyBonus[i].difficultyID == id)
			{
				db = this.difficultyBonus[i];
				break;
			}
		}
		return db;
	}
	
	public float GetHitBonus()
	{
		float b = this.hitBonus;
		DifficultyBonus db = this.GetDifficultyBonus(GameHandler.GetDifficulty());
		if(db != null)
		{
			b += db.hitBonus;
		}
		return b;
	}
	
	public float GetCounterBonus()
	{
		float b = this.counterBonus;
		DifficultyBonus db = this.GetDifficultyBonus(GameHandler.GetDifficulty());
		if(db != null)
		{
			b += db.counterBonus;
		}
		return b;
	}
	
	public float GetCriticalBonus()
	{
		float b = this.criticalBonus;
		DifficultyBonus db = this.GetDifficultyBonus(GameHandler.GetDifficulty());
		if(db != null)
		{
			b += db.criticalBonus;
		}
		return b;
	}
	
	public float GetBlockBonus()
	{
		float b = this.blockBonus;
		DifficultyBonus db = this.GetDifficultyBonus(GameHandler.GetDifficulty());
		if(db != null)
		{
			b += db.blockBonus;
		}
		return b;
	}
	
	public float GetEscapeBonus()
	{
		float b = this.escapeBonus;
		DifficultyBonus db = this.GetDifficultyBonus(GameHandler.GetDifficulty());
		if(db != null)
		{
			b += db.escapeBonus;
		}
		return b;
	}
	
	public float GetSpeedBonus()
	{
		float b = this.speedBonus;
		DifficultyBonus db = this.GetDifficultyBonus(GameHandler.GetDifficulty());
		if(db != null)
		{
			b += db.speedBonus;
		}
		return b;
	}
	
	public float GetItemStealBonus()
	{
		float b = this.itemStealBonus;
		DifficultyBonus db = this.GetDifficultyBonus(GameHandler.GetDifficulty());
		if(db != null)
		{
			b += db.itemStealBonus;
		}
		return b;
	}
	
	public float GetMoneyStealBonus()
	{
		float b = this.moneyStealBonus;
		DifficultyBonus db = this.GetDifficultyBonus(GameHandler.GetDifficulty());
		if(db != null)
		{
			b += db.moneyStealBonus;
		}
		return b;
	}
	
	public int[] GetStatusBonus()
	{
		int[] b = new int[this.statusBonus.Length];
		for(int i=0; i<b.Length; i++) b[i] = this.statusBonus[i];
		DifficultyBonus db = this.GetDifficultyBonus(GameHandler.GetDifficulty());
		if(db != null)
		{
			for(int i=0; i<b.Length; i++) b[i] += db.statusBonus[i];
		}
		return b;
	}
	
	public int GetElementDefence(int index)
	{
		int def = 0;
		DifficultyBonus db = this.GetDifficultyBonus(GameHandler.GetDifficulty());
		if(db != null)
		{
			def = db.elementBonus[index];
		}
		return def;
	}
	
	public int GetRaceDamageFactor(int index)
	{
		int factor = 0;
		DifficultyBonus db = this.GetDifficultyBonus(GameHandler.GetDifficulty());
		if(db != null)
		{
			factor = db.raceBonus[index];
		}
		return factor;
	}
	
	public int GetSizeDamageFactor(int index)
	{
		int factor = 0;
		DifficultyBonus db = this.GetDifficultyBonus(GameHandler.GetDifficulty());
		if(db != null)
		{
			factor = db.sizeBonus[index];
		}
		return factor;
	}
}
