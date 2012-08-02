
using UnityEngine;
using System.Collections;

public class EquipShort
{
	public EquipSet type = EquipSet.NONE;
	public int equipID = 0;
	public bool blocked = false;
	
	public EquipShort() {}
	
	public EquipShort(EquipSet t, int e)
	{
		this.type = t;
		this.equipID = e;
	}
	
	public void Change(EquipSet t, int e)
	{
		this.Change(t, e, false);
	}
	
	public void Change(EquipSet t, int e, bool b)
	{
		this.type = t;
		this.equipID = e;
		this.blocked = b;
	}
	
	public bool IsNone()
	{
		return EquipSet.NONE.Equals(this.type);
	}
	
	public bool IsWeapon()
	{
		return EquipSet.WEAPON.Equals(this.type);
	}
	
	public bool IsArmor()
	{
		return EquipSet.ARMOR.Equals(this.type);
	}
	
	public Equipment GetEquipment()
	{
		if(this.IsArmor()) return DataHolder.Armor(this.equipID);
		else if(this.IsWeapon()) return DataHolder.Weapon(this.equipID);
		else return null;
	}
	
	public string GetName()
	{
		string name = "";
		if(this.IsWeapon()) name = DataHolder.Weapons().GetName(this.equipID);
		else if(this.IsArmor()) name = DataHolder.Armors().GetName(this.equipID);
		return name;
	}
	
	public Texture2D GetIcon()
	{
		Texture2D tex = null;
		if(this.IsWeapon()) tex = DataHolder.Weapons().GetIcon(this.equipID);
		else if(this.IsArmor()) tex = DataHolder.Armors().GetIcon(this.equipID);
		return tex;
	}
	
	public GUIContent GetContent()
	{
		GUIContent gc = new GUIContent("");
		if(this.IsWeapon()) gc = DataHolder.Weapons().GetContent(this.equipID);
		else if(this.IsArmor()) gc = DataHolder.Armors().GetContent(this.equipID);
		return gc;
	}
}
