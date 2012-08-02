
using System.Collections;

public class Class
{
	public bool[] equipPart;
	
	public bool[] weapon;
	public bool[] armor;
	
	// element defence
	public int[] elementValue;
	// race attack factor
	public int[] raceValue;
	// size attack factor
	public int[] sizeValue;
	
	public bool useClassLevel = false;
	public Development development = new Development();
	
	public BonusSettings bonus = new BonusSettings();
	
	public Class()
	{
		
	}
	
	public ArrayList GetEquipableWeapons()
	{
		ArrayList equipable = new ArrayList();
		for(int i=0; i<weapon.Length; i++)
		{
			if(weapon[i])
			{
				equipable.Add(i);
			}
		}
		return equipable;
	}
	
	public string[] GetEquipableWeaponNames(bool showIDs)
	{
		ArrayList e = this.GetEquipableWeapons();
		string[] names = new string[e.Count];
		for(int i=0; i<e.Count; i++)
		{
			if(showIDs) names[i] = e[i]+": "+DataHolder.Weapons().GetName((int)e[i]);
			else names[i] = DataHolder.Weapons().GetName((int)e[i]);
		}
		return names;
	}
	
	public ArrayList GetEquipableWeapons(int partID)
	{
		ArrayList equipable = new ArrayList();
		ArrayList equips = this.GetEquipableWeapons();
		for(int i=0; i<equips.Count; i++)
		{
			if(DataHolder.Weapon((int)equips[i]).equipPart[partID])
			{
				equipable.Add((int)equips[i]);
			}
		}
		return equipable;
	}
	
	public string[] GetEquipableWeaponNames(int partID, bool showIDs)
	{
		ArrayList e = this.GetEquipableWeapons(partID);
		string[] names = new string[e.Count];
		for(int i=0; i<e.Count; i++)
		{
			if(showIDs) names[i] = e[i]+": "+DataHolder.Weapons().GetName((int)e[i]);
			else names[i] = DataHolder.Weapons().GetName((int)e[i]);
		}
		return names;
	}
	
	public ArrayList GetEquipableArmors()
	{
		ArrayList equipable = new ArrayList();
		for(int i=0; i<armor.Length; i++)
		{
			if(armor[i])
			{
				equipable.Add(i);
			}
		}
		return equipable;
	}
	
	public string[] GetEquipableArmorNames(bool showIDs)
	{
		ArrayList e = this.GetEquipableArmors();
		string[] names = new string[e.Count];
		for(int i=0; i<e.Count; i++)
		{
			if(showIDs) names[i] = e[i]+": "+DataHolder.Armors().GetName((int)e[i]);
			else names[i] = DataHolder.Armors().GetName((int)e[i]);
		}
		return names;
	}
	
	public ArrayList GetEquipableArmors(int partID)
	{
		ArrayList equipable = new ArrayList();
		ArrayList equips = this.GetEquipableArmors();
		for(int i=0; i<equips.Count; i++)
		{
			if(DataHolder.Armor((int)equips[i]).equipPart[partID])
			{
				equipable.Add(equips[i]);
			}
		}
		return equipable;
	}
	
	public string[] GetEquipableArmorNames(int partID, bool showIDs)
	{
		ArrayList e = this.GetEquipableArmors(partID);
		string[] names = new string[e.Count];
		for(int i=0; i<e.Count; i++)
		{
			if(showIDs) names[i] = e[i]+": "+DataHolder.Armors().GetName((int)e[i]);
			else names[i] = DataHolder.Armors().GetName((int)e[i]);
		}
		return names;
	}
	
	public ArrayList GetEquipmentParts()
	{
		ArrayList parts = new ArrayList();
		for(int i=0; i<equipPart.Length; i++)
		{
			if(equipPart[i])
			{
				parts.Add(i);
			}
		}
		return parts;
	}
	
	public string[] GetEquipmentPartNames(bool showIDs)
	{
		ArrayList e = this.GetEquipmentParts();
		string[] names = new string[e.Count];
		for(int i=0; i<e.Count; i++)
		{
			if(showIDs) names[i] = e[i]+": "+DataHolder.EquipmentParts().GetName((int)e[i]);
			else names[i] = DataHolder.EquipmentParts().GetName((int)e[i]);
		}
		return names;
	}
	
	public bool IsWeaponEquipable(int id)
	{
		return this.weapon[id];
	}
	
	public bool IsArmorEquipable(int id)
	{
		return this.armor[id];
	}
}