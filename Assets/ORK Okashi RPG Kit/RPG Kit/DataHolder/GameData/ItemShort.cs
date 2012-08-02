
using UnityEngine;
using System.Collections;

public class ItemShort
{
	public bool isMoney = false;
	public ItemDropType type = ItemDropType.ITEM;
	public int id = 0;
	public int quantity = 1;
	
	
	public ItemShort()
	{
		
	}
	
	public ItemShort(Hashtable ht)
	{
		this.SetData(ht);
	}
	
	public void AddToInventory()
	{
		if(this.isMoney)
		{
			GameHandler.AddMoney(this.quantity);
		}
		else if(ItemDropType.ITEM.Equals(this.type))
		{
			GameHandler.AddItem(this.id, this.quantity);
		}
		else if(ItemDropType.WEAPON.Equals(this.type))
		{
			GameHandler.AddWeapon(this.id, this.quantity);
		}
		else if(ItemDropType.ARMOR.Equals(this.type))
		{
			GameHandler.AddArmor(this.id, this.quantity);
		}
	}
	
	public void RemoveFromInventory()
	{
		if(this.isMoney)
		{
			GameHandler.SubMoney(this.quantity);
		}
		else if(ItemDropType.ITEM.Equals(this.type))
		{
			GameHandler.RemoveItem(this.id, this.quantity);
		}
		else if(ItemDropType.WEAPON.Equals(this.type))
		{
			GameHandler.RemoveWeapon(this.id, this.quantity);
		}
		else if(ItemDropType.ARMOR.Equals(this.type))
		{
			GameHandler.RemoveArmor(this.id, this.quantity);
		}
	}
	
	public bool InInventory()
	{
		bool has = false;
		if(this.isMoney)
		{
			has = GameHandler.HasEnoughMoney(this.quantity);
		}
		else if(ItemDropType.ITEM.Equals(this.type))
		{
			has = GameHandler.HasItem(this.id, this.quantity);
		}
		else if(ItemDropType.WEAPON.Equals(this.type))
		{
			has = GameHandler.HasWeapon(this.id, this.quantity);
		}
		else if(ItemDropType.ARMOR.Equals(this.type))
		{
			has = GameHandler.HasArmor(this.id, this.quantity);
		}
		return has;
	}
	
	public bool Equals(ItemShort item)
	{
		return (this.type == item.type && this.id == item.id && this.quantity == item.quantity);
	}
	
	public Hashtable GetData(Hashtable ht)
	{
		if(this.isMoney) ht.Add("ismoney", "true");
		else
		{
			ht.Add("type", this.type.ToString());
			ht.Add("itemid", this.id.ToString());
		}
		ht.Add("quantity", this.quantity.ToString());
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		if(ht.ContainsKey("ismoney")) this.isMoney = true;
		else
		{
			this.type = (ItemDropType)System.Enum.Parse(typeof(ItemDropType), (string)ht["type"]);
			this.id = int.Parse((string)ht["itemid"]);
		}
		this.quantity = int.Parse((string)ht["quantity"]);
	}
}
