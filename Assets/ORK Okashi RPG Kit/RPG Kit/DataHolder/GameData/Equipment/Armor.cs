
using UnityEngine;
using System.Collections;

public class Armor : Equipment
{
	public Armor()
	{
		
	}
	
	public override string GetPrefabPath() { return "Prefabs/Armors/"; }
	
	public override bool CanClassEquip(int eID, int classID)
	{
		return DataHolder.Class(classID).IsArmorEquipable(eID);
	}
}
