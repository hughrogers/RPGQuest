
using System.Collections;
using UnityEngine;

public class Equipment
{
	// prefab
	public string prefabName = "";
	
	// settings
	public int minimumLevel = 0;
	public int minimumClassLevel = 0;
	public EquipType equipType = EquipType.SINGLE;
	public bool[] equipPart;
	public bool[] blockPart;
	public int buyPrice = 0;
	public bool sellable = false;
	public int sellPrice = 50;
	public ValueSetter sellSetter = ValueSetter.PERCENT;
	public bool dropable = false;
	
	// element effectiveness
	public int[] elementValue;
	public SimpleOperator[] elementOperator;
	
	// race damage factor
	public int[] raceValue;
	// size damage factor
	public int[] sizeValue;
	
	// status effects
	public SkillEffect[] skillEffect;
	
	// skills
	public EquipmentSkill[] equipmentSkill = new EquipmentSkill[0];
	
	public BonusSettings bonus = new BonusSettings();
	
	public Equipment()
	{
		
	}
	
	public virtual string GetPrefabPath() { return ""; }
	public GameObject GetPrefabInstance()
	{
		GameObject prefab = null;
		if("" != this.prefabName)
		{
			GameObject tmp = (GameObject)Resources.Load(this.GetPrefabPath()+this.prefabName, typeof(GameObject));
			if(tmp) prefab = (GameObject)GameObject.Instantiate(tmp);
		}
		return prefab;
	}
	
	public bool IsSingle()
	{
		return EquipType.SINGLE.Equals(this.equipType);
	}
	
	public bool IsMulti()
	{
		return EquipType.MULTI.Equals(this.equipType);
	}
	
	public void AddEquipmentSkill()
	{
		equipmentSkill = ArrayHelper.Add(new EquipmentSkill(DataHolder.StatusValueCount), equipmentSkill);
	}
	
	public void RemoveEquipmentSkill(int index)
	{
		equipmentSkill = ArrayHelper.Remove(index, equipmentSkill);
	}
	
	public bool IsEquipable(bool[] ep)
	{
		bool val = false;
		
		if(EquipType.SINGLE.Equals(equipType))
		{
			for(int i=0; i<ep.Length; i++)
			{
				if(ep[i] == true && equipPart[i] == true)
				{
					val = true;
					break;
				}
			}
		}
		else
		{
			val = true;
			for(int i=0; i<ep.Length; i++)
			{
				if(equipPart[i] && !ep[i])
				{
					val = false;
					break;
				}
			}
		}
		
		return val;
	}
	
	public bool CanEquip(int eID, Character c)
	{
		bool can = true;
		if(c.currentLevel < this.minimumLevel || 
			c.currentClassLevel < this.minimumClassLevel || 
			!this.CanClassEquip(eID, c.currentClass))
		{
			can = false;
		}
		return can;
	}
	
	public virtual bool CanClassEquip(int eID, int classID)
	{
		return true;
	}
	
	public bool CanApplyEffect(int effectID)
	{
		return !SkillEffect.REMOVE.Equals(this.skillEffect[effectID]);
	}
	
	public bool CanRemoveEffect(int effectID)
	{
		return !SkillEffect.ADD.Equals(this.skillEffect[effectID]);
	}
	
	public int GetSellPrice()
	{
		int p = this.sellPrice;
		if(ValueSetter.PERCENT.Equals(this.sellSetter))
		{
			p = (this.buyPrice*this.sellPrice)/100;
		}
		return p;
	}
}
