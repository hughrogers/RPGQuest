
using UnityEngine;
using System.Collections;

public class Weapon : Equipment
{
	// attack settings
	public bool ownAttack = false;
	public int element = 0;
	public int[] baseAttack = new int[1];
	
	// animation
	public bool ownBaseAnimations = false;
	public BattleAnimationSettings battleAnimations = new BattleAnimationSettings();
	
	public Weapon()
	{
		
	}
	
	public override string GetPrefabPath() { return "Prefabs/Weapons/"; }
	
	public bool UseBaseAttack(Combatant user, Combatant target, float damageFactor, float damageMultiplier)
	{
		return DataHolder.BaseAttack(this.baseAttack[user.baIndex]).Use(
				user, this.element, target, damageFactor, damageMultiplier);
	}
	
	public override bool CanClassEquip(int eID, int classID)
	{
		return DataHolder.Class(classID).IsWeaponEquipable(eID);
	}
	
	public void AddBaseAttack()
	{
		this.baseAttack = ArrayHelper.Add(0, this.baseAttack);
	}
	
	public void RemoveBaseAttack(int index)
	{
		this.baseAttack = ArrayHelper.Remove(index, this.baseAttack);
	}
}
