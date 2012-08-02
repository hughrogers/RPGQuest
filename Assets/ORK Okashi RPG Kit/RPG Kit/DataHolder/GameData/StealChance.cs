
using UnityEngine;
using System.Collections;

public class StealChance
{
	// item
	public bool stealItem = false;
	public int itemChance = 0;
	public float itemBonus = 0;
	public bool fixItem = false;
	public ItemDropType itemType = ItemDropType.ITEM;
	public int itemID = 0;
	
	// money
	public bool stealMoney = false;
	public int moneyChance = 0;
	public float moneyBonus = 0;
	public bool fixMoney = false;
	public int money = 0;
	
	public StealChance()
	{
		
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		if(this.stealItem)
		{
			ht.Add("itemchance", this.itemChance.ToString());
			ht.Add("itembonus", this.itemBonus.ToString());
			if(this.fixItem)
			{
				ht.Add("itemtype", this.itemType.ToString());
				ht.Add("item", this.itemID.ToString());
			}
		}
		if(this.stealMoney)
		{
			ht.Add("moneychance", this.moneyChance.ToString());
			ht.Add("moneybonus", this.moneyBonus.ToString());
			if(this.fixMoney)
			{
				ht.Add("money", this.money.ToString());
			}
		}
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		if(ht.ContainsKey("itemchance"))
		{
			this.stealItem = true;
			this.itemChance = int.Parse((string)ht["itemchance"]);
			this.itemBonus = float.Parse((string)ht["itembonus"]);
			if(ht.ContainsKey("item"))
			{
				this.fixItem = true;
				this.itemType = (ItemDropType)System.Enum.Parse(
						typeof(ItemDropType), (string)ht["itemtype"]);
				this.itemID = int.Parse((string)ht["item"]);
			}
		}
		if(ht.ContainsKey("moneychance"))
		{
			this.stealMoney = true;
			this.moneyChance = int.Parse((string)ht["moneychance"]);
			this.moneyBonus = float.Parse((string)ht["moneybonus"]);
			if(ht.ContainsKey("money"))
			{
				this.fixMoney = true;
				this.money = int.Parse((string)ht["money"]);
			}
		}
	}
	
	/*
	============================================================================
	Steal functions
	============================================================================
	*/
	public void Steal(Combatant user, Combatant target)
	{
		if(user is Character && target is Enemy) this.StealFromEnemy(user as Character, target as Enemy);
		else if(user is Enemy && target is Character) this.StealFromParty(user as Enemy, target as Character);
	}
	
	public void StealFromEnemy(Character user, Enemy target)
	{
		ItemDropType t = ItemDropType.ITEM;
		int id = -1;
		int m = -1;
		
		// steal item
		if(this.stealItem && target.stealItem && !target.itemStolen && 
			DataHolder.GameSettings().GetRandom() <= 
			(DataHolder.Formula(this.itemChance).Calculate(user, target)+
				user.GetItemStealBonus()+this.itemBonus)*target.stealItemFactor)
		{
			if(this.fixItem)
			{
				GameHandler.AddToInventory(this.itemType, this.itemID, 1);
				t = this.itemType;
				id = this.itemID;
			}
			else
			{
				GameHandler.AddToInventory(target.stealItemType, target.stealItemID, 1);
				t = target.stealItemType;
				id = target.stealItemID;
			}
			if(target.stealItemOnce) target.itemStolen = true;
			if(id >= 0)
			{
				// TODO: info text display
			}
		}
		
		// steal money
		if(this.stealMoney && target.stealMoney && !target.moneyStolen && 
			DataHolder.GameSettings().GetRandom() <= 
			(DataHolder.Formula(this.moneyChance).Calculate(user, target)+
				user.GetMoneyStealBonus()+this.moneyBonus)*target.stealMoneyFactor)
		{
			if(this.fixMoney)
			{
				GameHandler.AddMoney(this.money);
				m = this.money;
			}
			else
			{
				GameHandler.AddMoney(target.stealMoneyAmount);
				m = target.stealMoneyAmount;
			}
			if(target.stealMoneyOnce) target.moneyStolen = true;
		}
		this.ShowStealInfo(user.GetName(), t, id, m);
	}
	
	public void StealFromParty(Enemy user, Character target)
	{
		ItemDropType t = ItemDropType.ITEM;
		int id = -1;
		int m = -1;
		
		// steal item
		if(this.stealItem && 
			DataHolder.GameSettings().GetRandom() <= 
			(DataHolder.Formula(this.itemChance).Calculate(user, target)+
				user.GetItemStealBonus()+this.itemBonus))
		{
			if(this.fixItem && GameHandler.HasInInventory(this.itemType, this.itemID, 1))
			{
				GameHandler.RemoveFromInventory(this.itemType, this.itemID, 1);
				t = this.itemType;
				id = this.itemID;
			}
			else if(!this.fixItem)
			{
				ArrayList list = GameHandler.GetStealableItems();
				if(list.Count > 0)
				{
					id = (int)list[Random.Range(0, list.Count-1)];
					GameHandler.RemoveItem(id);
				}
			}
			if(id >= 0) user.AddItemDrop(t, id, 1, 100);
		}
		
		// steal money
		if(this.stealMoney && 
			DataHolder.GameSettings().GetRandom() <= 
			(DataHolder.Formula(this.moneyChance).Calculate(user, target)+
				user.GetMoneyStealBonus()+this.moneyBonus))
		{
			if(this.fixMoney)
			{
				m = this.money;
				if(!GameHandler.HasEnoughMoney(m))
				{
					m = GameHandler.GetMoney();
				}
				GameHandler.SubMoney(m);
			}
			else
			{
				m = user.money;
				if(!GameHandler.HasEnoughMoney(m))
				{
					m = GameHandler.GetMoney();
				}
				GameHandler.SubMoney(m);
			}
			if(m > 0) user.money += m;
		}
		this.ShowStealInfo(user.GetName(), t, id, m);
	}
	
	private void ShowStealInfo(string user, ItemDropType t, int id, int m)
	{
		if(DataHolder.BattleSystemData().showInfo)
		{
			// item text display
			if(id >= 0 && DataHolder.BattleSystemData().showStealItem)
			{
				string name = "";
				if(ItemDropType.ITEM.Equals(t)) name = DataHolder.Items().GetName(id);
				else if(ItemDropType.WEAPON.Equals(t)) name = DataHolder.Weapons().GetName(id);
				else if(ItemDropType.ARMOR.Equals(t)) name = DataHolder.Armors().GetName(id);
				GameHandler.GetLevelHandler().ShowBattleInfo(
						DataHolder.BattleSystemData().GetStealItemText(user, name), 
						DataHolder.BattleSystemData().infoPosition);
			}
			else if(this.stealItem && DataHolder.BattleSystemData().showStealItemFail)
			{
				GameHandler.GetLevelHandler().ShowBattleInfo(
						DataHolder.BattleSystemData().GetStealItemFailText(user), 
						DataHolder.BattleSystemData().infoPosition);
			}
			// money text display
			if(m > 0 && DataHolder.BattleSystemData().showStealMoney)
			{
				GameHandler.GetLevelHandler().ShowBattleInfo(
						DataHolder.BattleSystemData().GetStealMoneyText(user, m.ToString()), 
						DataHolder.BattleSystemData().infoPosition);
			}
			else if(this.stealMoney && DataHolder.BattleSystemData().showStealMoneyFail)
			{
				GameHandler.GetLevelHandler().ShowBattleInfo(
						DataHolder.BattleSystemData().GetStealMoneyFailText(user), 
						DataHolder.BattleSystemData().infoPosition);
			}
		}
	}
}
