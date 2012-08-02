
using UnityEngine;
using System.Collections;

public class GameStatistic
{
	// enemies
	public bool logKilledEnemies = false;
	public bool logSingleEnemies = false;
	
	// items
	public bool logUsedItems = false;
	public bool logSingleItems = false;
	public bool logCreatedItems = false;
	public bool logSingleCreated = false;
	
	// battles
	public bool logBattles = false;
	public bool logWonBattles = false;
	public bool logLostBattles = false;
	public bool logEscapedBattles = false;
	
	// custom
	public bool logCustom = false;
	
	// failed creation? total, single
	// log moved distance
	// log visited scenes
	// statistic clearing settings (new game? game load? etc)
	
	// ingame
	// enemies
	private int killedEnemies = 0; // #*ke
	private int[] singleEnemies = new int[0]; // #*seX#
	
	// items
	private int usedItems = 0; // #*ui
	private int[] singleItems = new int[0]; // #*siX#
	private int createdItems = 0; // #*cr
	private int[] singleCreatedItems = new int[0]; // #*ciX#
	private int[] singleCreatedWeapons = new int[0]; // #*cwX#
	private int[] singleCreatedArmors = new int[0]; // #*caX#
	
	// battles
	private int battles = 0; // #*ba
	private int wonBattles = 0; // #*bw
	private int lostBattles = 0; // #*bl
	private int escapedBattles = 0; // #*be
	
	// custom
	private int[] custom = new int[0]; // #*cuX#
	
	// XML
	private static string ENEMY = "enemy";
	private static string ITEM = "item";
	private static string CREATEDITEM = "createditem";
	private static string CREATEDWEAPON = "createdweapon";
	private static string CREATEDARMOR = "createdarmor";
	private static string CUSTOM = "custom";
	
	
	public GameStatistic()
	{
		
	}
	
	public void Clear()
	{
		if(this.logKilledEnemies) this.killedEnemies = 0;
		if(this.logSingleEnemies) this.singleEnemies = new int[DataHolder.EnemyCount];
		if(this.logUsedItems) this.usedItems = 0;
		if(this.logSingleItems) this.singleItems = new int[DataHolder.ItemCount];
		if(this.logUsedItems) this.createdItems = 0;
		if(this.logSingleItems) this.singleCreatedItems = new int[DataHolder.ItemCount];
		if(this.logSingleItems) this.singleCreatedWeapons = new int[DataHolder.WeaponCount];
		if(this.logSingleItems) this.singleCreatedArmors = new int[DataHolder.ArmorCount];
		if(this.logBattles) this.battles = 0;
		if(this.logWonBattles) this.wonBattles = 0;
		if(this.logLostBattles) this.lostBattles = 0;
		if(this.logEscapedBattles) this.escapedBattles = 0;
		if(this.logCustom) this.custom = new int[0];
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		// enemies
		if(this.logKilledEnemies) ht.Add("logkilledenemies", "true");
		if(this.logSingleEnemies) ht.Add("logsingleenemies", "true");
		// items
		if(this.logUsedItems) ht.Add("loguseditems", "true");
		if(this.logSingleItems) ht.Add("logsingleitems", "true");
		if(this.logCreatedItems) ht.Add("logcreateditems", "true");
		if(this.logSingleCreated) ht.Add("logsinglecreated", "true");
		// items
		if(this.logBattles) ht.Add("logbattles", "true");
		if(this.logWonBattles) ht.Add("logwonbattles", "true");
		if(this.logLostBattles) ht.Add("loglostbattles", "true");
		if(this.logEscapedBattles) ht.Add("logescapedbattles", "true");
		// custom
		if(this.logCustom) ht.Add("logcustom", "true");
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		// enemies
		if(ht.ContainsKey("logkilledenemies")) this.logKilledEnemies = true;
		if(ht.ContainsKey("logsingleenemies")) this.logSingleEnemies = true;
		// items
		if(ht.ContainsKey("loguseditems")) this.logUsedItems = true;
		if(ht.ContainsKey("logsingleitems")) this.logSingleItems = true;
		if(ht.ContainsKey("logcreateditems")) this.logCreatedItems = true;
		if(ht.ContainsKey("logsinglecreated")) this.logSingleCreated = true;
		// battles
		if(ht.ContainsKey("logbattles")) this.logBattles = true;
		if(ht.ContainsKey("logwonbattles")) this.logWonBattles = true;
		if(ht.ContainsKey("loglostbattles")) this.logLostBattles = true;
		if(ht.ContainsKey("logescapedbattles")) this.logEscapedBattles = true;
		// custom
		if(ht.ContainsKey("logcustom")) this.logCustom = true;
	}
	
	/*
	============================================================================
	Save game functions
	============================================================================
	*/
	public Hashtable GetSaveData(Hashtable ht)
	{
		ArrayList s = new ArrayList();
		// enemies
		if(this.logKilledEnemies) ht.Add("killedenemies", this.killedEnemies.ToString());
		if(this.logSingleEnemies)
		{
			this.AddIntArray(ref s, this.singleEnemies, GameStatistic.ENEMY);
		}
		// items
		if(this.logUsedItems) ht.Add("useditems", this.usedItems.ToString());
		if(this.logSingleItems)
		{
			this.AddIntArray(ref s, this.singleItems, GameStatistic.ITEM);
		}
		if(this.logCreatedItems) ht.Add("createditems", this.createdItems.ToString());
		if(this.logSingleCreated)
		{
			this.AddIntArray(ref s, this.singleCreatedItems, GameStatistic.CREATEDITEM);
			this.AddIntArray(ref s, this.singleCreatedWeapons, GameStatistic.CREATEDWEAPON);
			this.AddIntArray(ref s, this.singleCreatedArmors, GameStatistic.CREATEDARMOR);
		}
		// battles
		if(this.logBattles) ht.Add("battles", this.battles.ToString());
		if(this.logWonBattles) ht.Add("wonbattles", this.wonBattles.ToString());
		if(this.logLostBattles) ht.Add("lostbattles", this.lostBattles.ToString());
		if(this.logEscapedBattles) ht.Add("escapedbattles", this.escapedBattles.ToString());
		// custom
		if(this.logCustom)
		{
			ht.Add("customs", this.custom.Length.ToString());
			this.AddIntArray(ref s, this.custom, GameStatistic.CUSTOM);
		}
		
		if(s.Count > 0) ht.Add(XMLHandler.NODES, s);
		return ht;
	}
	
	private void AddIntArray(ref ArrayList s, int[] data, string title)
	{
		for(int i=0; i<data.Length; i++)
		{
			if(data[i] > 0)
			{
				Hashtable ht2 = HashtableHelper.GetTitleHashtable(title, i);
				ht2.Add("value", data[i].ToString());
				s.Add(ht2);
			}
		}
	}
	
	public void SetSaveData(Hashtable ht)
	{
		this.Clear();
		if(ht.ContainsKey("killedenemies"))
		{
			this.killedEnemies = int.Parse((string)ht["killedenemies"]);
		}
		if(ht.ContainsKey("useditems"))
		{
			this.usedItems = int.Parse((string)ht["useditems"]);
		}
		if(ht.ContainsKey("battles"))
		{
			this.battles = int.Parse((string)ht["battles"]);
		}
		if(ht.ContainsKey("wonbattles"))
		{
			this.wonBattles = int.Parse((string)ht["battles"]);
		}
		if(ht.ContainsKey("lostbattles"))
		{
			this.lostBattles = int.Parse((string)ht["lostbattles"]);
		}
		if(ht.ContainsKey("escapedbattles"))
		{
			this.escapedBattles = int.Parse((string)ht["escapedbattles"]);
		}
		if(ht.ContainsKey("customs"))
		{
			this.custom = new int[int.Parse((string)ht["customs"])];
		}
		
		if(ht.ContainsKey(XMLHandler.NODES))
		{
			ArrayList s = ht[XMLHandler.NODES] as ArrayList;
			foreach(Hashtable ht2 in s)
			{
				if(ht2[XMLHandler.NODE_NAME] as string == GameStatistic.ENEMY)
				{
					this.SetIntArray(ref this.singleEnemies, ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == GameStatistic.ITEM)
				{
					this.SetIntArray(ref this.singleItems, ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == GameStatistic.CREATEDITEM)
				{
					this.SetIntArray(ref this.singleCreatedItems, ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == GameStatistic.CREATEDWEAPON)
				{
					this.SetIntArray(ref this.singleCreatedWeapons, ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == GameStatistic.CREATEDARMOR)
				{
					this.SetIntArray(ref this.singleCreatedArmors, ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == GameStatistic.CUSTOM)
				{
					this.SetIntArray(ref this.custom, ht2);
				}
			}
		}
	}
	
	private void SetIntArray(ref int[] data, Hashtable ht)
	{
		int id = int.Parse((string)ht["id"]);
		if(id < data.Length)
		{
			data[id] = int.Parse((string)ht["value"]);
		}
	}
	
	/*
	============================================================================
	String functions
	============================================================================
	*/
	public string GetStatisticText(string text)
	{
		// enemies
		text = text.Replace("#*ke", this.GetKilledEnemies().ToString());
		
		int replace = MultiLabel.NextSpecial(text, "#*se");
		while(replace != -1)
		{
			text = text.Replace("#*se"+replace+"#", this.GetKilledEnemy(replace).ToString());
			replace = MultiLabel.NextSpecial(text, "#*se");
		}
		
		// items
		text = text.Replace("#*ui", this.GetUsedItems().ToString());
		
		replace = MultiLabel.NextSpecial(text, "#*si");
		while(replace != -1)
		{
			text = text.Replace("#*si"+replace+"#", this.GetUsedItem(replace).ToString());
			replace = MultiLabel.NextSpecial(text, "#*si");
		}
		
		text = text.Replace("#*cr", this.GetCreatedItems().ToString());
		replace = MultiLabel.NextSpecial(text, "#*ci");
		while(replace != -1)
		{
			text = text.Replace("#*ci"+replace+"#", this.GetCreatedItem(replace).ToString());
			replace = MultiLabel.NextSpecial(text, "#*ci");
		}
		replace = MultiLabel.NextSpecial(text, "#*cw");
		while(replace != -1)
		{
			text = text.Replace("#*cw"+replace+"#", this.GetCreatedWeapon(replace).ToString());
			replace = MultiLabel.NextSpecial(text, "#*cw");
		}
		replace = MultiLabel.NextSpecial(text, "#*ca");
		while(replace != -1)
		{
			text = text.Replace("#*ca"+replace+"#", this.GetCreatedArmor(replace).ToString());
			replace = MultiLabel.NextSpecial(text, "#*ca");
		}
		
		// battles
		text = text.Replace("#*ba", this.GetBattles().ToString());
		text = text.Replace("#*bw", this.GetWonBattles().ToString());
		text = text.Replace("#*bl", this.GetLostBattles().ToString());
		text = text.Replace("#*be", this.GetEscapedBattles().ToString());
		
		// custom
		replace = MultiLabel.NextSpecial(text, "#*cu");
		while(replace != -1)
		{
			text = text.Replace("#*cu"+replace+"#", this.GetCustom(replace).ToString());
			replace = MultiLabel.NextSpecial(text, "#*cu");
		}
		
		return text;
	}
	
	/*
	============================================================================
	Enemy functions
	============================================================================
	*/
	public void EnemyKilled(int id)
	{
		if(this.logKilledEnemies) this.killedEnemies++;
		if(this.logSingleEnemies && id < this.singleEnemies.Length)
		{
			this.singleEnemies[id]++;
		}
	}
	
	public int GetKilledEnemies()
	{
		return this.killedEnemies;
	}
	
	public int GetKilledEnemy(int id)
	{
		if(id < this.singleEnemies.Length) return this.singleEnemies[id];
		else return 0;
	}
	
	/*
	============================================================================
	Item functions
	============================================================================
	*/
	public void ItemUsed(int id)
	{
		if(this.logUsedItems) this.usedItems++;
		if(this.logSingleItems && id < this.singleItems.Length)
		{
			this.singleItems[id]++;
		}
	}
	
	public int GetUsedItems()
	{
		return this.usedItems;
	}
	
	public int GetUsedItem(int id)
	{
		if(id < this.singleItems.Length) return this.singleItems[id];
		else return 0;
	}
	
	public void ItemCreated(int id, ItemDropType t)
	{
		if(this.logCreatedItems) this.createdItems++;
		if(this.logSingleCreated)
		{
			if(ItemDropType.ITEM.Equals(t) &&
				id <this.singleCreatedItems.Length)
			{
				this.singleCreatedItems[id]++;
			}
			else if(ItemDropType.WEAPON.Equals(t) &&
				id < this.singleCreatedWeapons.Length)
			{
				this.singleCreatedWeapons[id]++;
			}
			else if(ItemDropType.ARMOR.Equals(t) &&
				id < this.singleCreatedArmors.Length)
			{
				this.singleCreatedArmors[id]++;
			}
		}
	}
	
	public int GetCreatedItems()
	{
		return this.createdItems;
	}
	
	public int GetCreatedItem(int id)
	{
		if(id < this.singleCreatedItems.Length) return this.singleCreatedItems[id];
		else return 0;
	}
	
	public int GetCreatedWeapon(int id)
	{
		if(id < this.singleCreatedWeapons.Length) return this.singleCreatedWeapons[id];
		else return 0;
	}
	
	public int GetCreatedArmor(int id)
	{
		if(id < this.singleCreatedArmors.Length) return this.singleCreatedArmors[id];
		else return 0;
	}
	
	/*
	============================================================================
	Battle functions
	============================================================================
	*/
	public void BattleStarted()
	{
		if(this.logBattles) this.battles++;
	}
	
	public void BattleWon()
	{
		if(this.logWonBattles) this.wonBattles++;
	}
	
	public void BattleLost()
	{
		if(this.logLostBattles) this.lostBattles++;
	}
	
	public void BattleEscaped()
	{
		if(this.logEscapedBattles) this.escapedBattles++;
	}
	
	public int GetBattles()
	{
		return this.battles;
	}
	
	public int GetWonBattles()
	{
		return this.wonBattles;
	}
	
	public int GetLostBattles()
	{
		return this.lostBattles;
	}
	
	public int GetEscapedBattles()
	{
		return this.escapedBattles;
	}
	
	/*
	============================================================================
	Custom functions
	============================================================================
	*/
	public void CustomChanged(int index, int add)
	{
		if(this.logCustom)
		{
			if(index >= this.custom.Length)
			{
				int[] tmp = this.custom;
				this.custom = new int[index+1];
				System.Array.Copy(tmp, this.custom, tmp.Length);
			}
			this.custom[index] += add;
		}
	}
	
	public int GetCustom(int index)
	{
		if(index < this.custom.Length) return this.custom[index];
		else return 0;
	}
}
