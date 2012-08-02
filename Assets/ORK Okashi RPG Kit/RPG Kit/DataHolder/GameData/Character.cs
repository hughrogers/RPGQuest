
using System.Collections;
using UnityEngine;

public class Character : Combatant
{
	public int currentClass = 0;
	public Development development = new Development();
	
	public bool leaveOnDeath = false;
	
	public bool aiControlled = false;
	public CharacterControlMap controlMap = new CharacterControlMap();
	
	// ingame
	public EquipShort[] equipment;
	private SkillLearn[] skill = new SkillLearn[0];
	private bool skillsChanged = true;
	private SkillLearn[] skillList = new SkillLearn[0];
	
	// in battle
	public ActiveBattleMenu battleMenu = new ActiveBattleMenu();
	
	private string newName = "";
	
	public Character()
	{
		
	}
	
	public override string GetPrefabPath() { return CharacterData.PREFAB_PATH; }
	
	public Character(int id)
	{
		this.realID = id;
	}
	
	protected override float GetInternMoveSpeed()
	{
		float speed = base.GetInternMoveSpeed();
		speed += DataHolder.Class(this.currentClass).bonus.GetSpeedBonus();
		
		ArrayList checkedWeapons = new ArrayList();
		ArrayList checkedArmors = new ArrayList();
		for(int i=0; i<this.equipment.Length; i++)
		{
			if(!this.equipment[i].IsNone())
			{
				Equipment eqp = null;
				bool add = true;
				if(this.equipment[i].IsWeapon())
				{
					eqp = DataHolder.Weapon(this.equipment[i].equipID);
					if(eqp.IsMulti() && checkedWeapons.Contains(this.equipment[i].equipID))
					{
						add = false;
					}
					else
					{
						checkedWeapons.Add(this.equipment[i].equipID);
					}
				}
				else if(this.equipment[i].IsArmor())
				{
					eqp = DataHolder.Armor(this.equipment[i].equipID);
					if(eqp.IsMulti() && checkedArmors.Contains(this.equipment[i].equipID))
					{
						add = false;
					}
					else
					{
						checkedArmors.Add(this.equipment[i].equipID);
					}
				}
				if(add)
				{
					speed += eqp.bonus.GetSpeedBonus();
				}
			}
		}
		// skills
		SkillLearn[] s = this.GetSkills();
		for(int i=0; i<s.Length; i++)
		{
			Skill sk = DataHolder.Skill(s[i].skillID);
			if(sk.isPassive)
			{
				speed += sk.level[s[i].GetLevel()].bonus.GetSpeedBonus();
			}
		}
		return speed;
	}
	
	public override bool InAttackRange(Combatant c)
	{
		bool inRange = true;
		bool found = false;
		for(int i=0; i<this.equipment.Length; i++)
		{
			if(this.equipment[i].IsWeapon() && 
				DataHolder.Weapon(this.equipment[i].equipID).ownAttack && 
				DataHolder.BaseAttack(DataHolder.Weapon(this.equipment[i].equipID).baseAttack[this.baIndex]).useRange.active)
			{
				inRange = DataHolder.BaseAttack(DataHolder.Weapon(this.equipment[i].equipID).
						baseAttack[this.baIndex]).useRange.InRange(this, c);
				found = true;
				break;
			}
		}
		if(!found) inRange = base.InAttackRange(c);
		return inRange;
	}
	
	/*
	============================================================================
	Init functions
	============================================================================
	*/
	public override void Init()
	{
		this.Init(this.development.startLevel);
	}
	
	public void Init(int lvl)
	{
		base.Init();
		this.currentLevel = lvl;
		Class c = DataHolder.Class(this.currentClass);
		if(c.useClassLevel)
		{
			this.currentClassLevel = c.development.startLevel;
		}
		
		for(int i=0; i<this.status.Length; i++)
		{
			int value = 0;
			if(this.status[i].IsConsumable())
			{
				value += this.GetValueAtLevel(this.status[i].maxStatus, this.currentLevel);
				if(c.useClassLevel) value += c.development.GetValueAtLevel(
						this.status[i].maxStatus, this.currentClassLevel);
			}
			else if(c.useClassLevel && 
				this.status[i].IsExperience() && this.status[i].levelUpClass)
			{
				value += c.development.GetValueAtLevel(i, this.currentClassLevel);
			}
			else
			{
				value += this.GetValueAtLevel(i, this.currentLevel);
				if(c.useClassLevel) value += c.development.GetValueAtLevel(i, this.currentClassLevel);
			}
			this.status[i].InitValue(value);
		}
		
		this.equipment = new EquipShort[c.equipPart.Length];
		for(int i=0; i<c.equipPart.Length; i++)
		{
			this.equipment[i] = new EquipShort();
		}
		this.element = new int[c.elementValue.Length];
		for(int i=0; i<c.elementValue.Length; i++)
		{
			this.element[i] = c.elementValue[i];
		}
		this.raceDmgFactor = new int[c.raceValue.Length];
		for(int i=0; i<c.raceValue.Length; i++)
		{
			this.raceDmgFactor[i] = c.raceValue[i];
		}
		this.sizeDmgFactor = new int[c.sizeValue.Length];
		for(int i=0; i<c.sizeValue.Length; i++)
		{
			this.sizeDmgFactor[i] = c.sizeValue[i];
		}
		
		// init level skills
		this.development.InitSkills(this, this.currentLevel);
		// init class skills
		if(c.useClassLevel) c.development.InitSkills(this, this.currentClassLevel);
		else c.development.InitSkills(this, this.currentLevel);
		
		this.ResetStatus();
		for(int i=0; i<this.status.Length; i++)
		{
			if(this.status[i].IsConsumable())
			{
				this.status[i].SetValue(this.status[this.status[i].maxStatus].GetValue(), false, false, false);
			}
		}
	}
	
	public int GetValueAtLevel(int vID, int lvl)
	{
		return this.development.GetValueAtLevel(vID, lvl);
	}
	
	public void ChangeClass(int classID, bool forgetSkills, bool learnSkills, 
			bool resetClassLevel, bool removeOldBonus, bool statusBonus)
	{
		this.UnequipAll();
		
		Class oC = DataHolder.Class(this.currentClass);
		this.currentClass = classID;
		Class c = DataHolder.Class(this.currentClass);
		
		// skills
		if(forgetSkills)
		{
			if(c.useClassLevel) oC.development.ForgetSkills(this, this.currentClassLevel);
			else oC.development.ForgetSkills(this, this.currentLevel);
		}
		
		// status+level
		if(c.useClassLevel)
		{
			if(removeOldBonus)
			{
				for(int i=0; i<this.status.Length; i++)
				{
					if(this.status[i].IsNormal())
					{
						int value = -oC.development.GetValueAtLevel(i, this.currentClassLevel);
						this.status[i].AddBaseValue(value);
						this.status[i].AddValue(value, true, true, true);
					}
				}
			}
			
			if(resetClassLevel)
			{
				this.currentClassLevel = c.development.startLevel;
			}
			
			for(int i=0; i<this.status.Length; i++)
			{
				int value = 0;
				if(this.status[i].IsExperience() && this.status[i].levelUpClass)
				{
					value += c.development.GetValueAtLevel(i, this.currentClassLevel);
				}
				else if(this.status[i].IsNormal() && statusBonus)
				{
					value += c.development.GetValueAtLevel(i, this.currentClassLevel);
				}
				if(value != 0)
				{
					this.status[i].AddBaseValue(value);
					this.status[i].AddValue(value, true, true, true);
				}
			}
		}
		
		if(learnSkills)
		{
			if(c.useClassLevel) c.development.InitSkills(this, this.currentClassLevel);
			else c.development.InitSkills(this, this.currentLevel);
		}
		
		this.equipment = new EquipShort[c.equipPart.Length];
		for(int i=0; i<c.equipPart.Length; i++)
		{
			this.equipment[i] = new EquipShort();
		}
		this.element = new int[c.elementValue.Length];
		for(int i=0; i<c.elementValue.Length; i++)
		{
			this.element[i] = c.elementValue[i];
		}
		this.raceDmgFactor = new int[c.raceValue.Length];
		for(int i=0; i<c.raceValue.Length; i++)
		{
			this.raceDmgFactor[i] = c.raceValue[i];
		}
		this.sizeDmgFactor = new int[c.sizeValue.Length];
		for(int i=0; i<c.sizeValue.Length; i++)
		{
			this.sizeDmgFactor[i] = c.sizeValue[i];
		}
		this.ResetStatus();
		this.ResetEffects();
		this.skillsChanged = true;
	}
	
	/*
	============================================================================
	Equip functions
	============================================================================
	*/
	public bool Equip(int partID, int equipID, EquipSet type)
	{
		bool equipped = false;
		Equipment eqp = null;
		if(EquipSet.WEAPON.Equals(type))
		{
			eqp = DataHolder.Weapon(equipID);
		}
		else if(EquipSet.ARMOR.Equals(type))
		{
			eqp = DataHolder.Armor(equipID);
		}
		if(eqp != null)
		{
			// check partID, if -1 get first partID that matches
			if(partID == -1)
			{
				for(int i=0; i<eqp.equipPart.Length; i++)
				{
					if(eqp.equipPart[i] && DataHolder.Class(this.currentClass).equipPart[i])
					{
						partID = i;
						break;
					}
				}
			}
			if(eqp.equipPart[partID] && eqp.CanEquip(equipID, this))
			{
				equipped = true;
				this.Unequip(partID, false);
				if(eqp.IsSingle())
				{
					this.equipment[partID].Change(type, equipID);
				}
				else if(eqp.IsMulti())
				{
					for(int i=0; i<eqp.equipPart.Length; i++)
					{
						if(eqp.equipPart[i])
						{
							this.Unequip(i, false);
							this.equipment[i].Change(type, equipID);
						}
					}
				}
				for(int i=0; i<eqp.blockPart.Length; i++)
				{
					if(eqp.blockPart[i])
					{
						this.Unequip(i, false);
						this.equipment[i].Change(EquipSet.NONE, partID, true);
					}
				}
				if(this.equipment[partID].IsWeapon())
				{
					GameHandler.RemoveWeapon(this.equipment[partID].equipID);
				}
				else if(this.equipment[partID].IsArmor())
				{
					GameHandler.RemoveArmor(this.equipment[partID].equipID);
				}
				this.ResetStatus();
				this.ResetEffects();
			}
		}
		this.skillsChanged = true;
		return equipped;
	}
	
	public void UnequipAll()
	{
		for(int i=0; i<this.equipment.Length; i++)
		{
			if(i < this.equipment.Length-1) this.Unequip(i, false);
			else this.Unequip(i);
		}
		this.skillsChanged = true;
	}
	
	public void Unequip(int partID)
	{
		this.Unequip(partID, true);
	}
	
	public void Unequip(int partID, bool reset)
	{
		if(this.equipment[partID].IsNone() && this.equipment[partID].blocked)
		{
			partID = this.equipment[partID].equipID;
		}
		if(!this.equipment[partID].IsNone())
		{
			Equipment eqp = null;
			if(this.equipment[partID].IsWeapon())
			{
				eqp = DataHolder.Weapon(this.equipment[partID].equipID);
				GameHandler.AddWeapon(this.equipment[partID].equipID);
			}
			else if(this.equipment[partID].IsArmor())
			{
				eqp = DataHolder.Armor(this.equipment[partID].equipID);
				GameHandler.AddArmor(this.equipment[partID].equipID);
			}
			if(eqp.IsSingle())
			{
				this.equipment[partID].Change(EquipSet.NONE, 0);
			}
			else if(eqp.IsMulti())
			{
				for(int i=0; i<eqp.equipPart.Length; i++)
				{
					if(eqp.equipPart[i])
					{
						this.equipment[i].Change(EquipSet.NONE, 0);
					}
				}
			}
			for(int i=0; i<eqp.blockPart.Length; i++)
			{
				if(eqp.blockPart[i])
				{
					this.equipment[i].Change(EquipSet.NONE, 0);
				}
			}
			if(reset)
			{
				this.ResetStatus();
				this.ResetEffects();
			}
		}
		this.skillsChanged = true;
	}
	
	public EquipShort[] GetFakeEquip(int partID, int equipID, EquipSet type)
	{
		EquipShort[] equip = new EquipShort[this.equipment.Length];
		for(int i=0; i<equip.Length; i++)
		{
			equip[i] = new EquipShort(this.equipment[i].type, this.equipment[i].equipID);
			equip[i].blocked = this.equipment[i].blocked;
		}
		
		Equipment eqp = null;
		if(EquipSet.WEAPON.Equals(type))
		{
			eqp = DataHolder.Weapon(equipID);
		}
		else if(EquipSet.ARMOR.Equals(type))
		{
			eqp = DataHolder.Armor(equipID);
		}
		// unequip old
		if(partID >= 0)
		{
			if(equip[partID].IsNone() && equip[partID].blocked)
			{
				partID = equip[partID].equipID;
			}
			if(!equip[partID].IsNone())
			{
				Equipment eqp2 = null;
				if(equip[partID].IsWeapon())
				{
					eqp2 = DataHolder.Weapon(equip[partID].equipID);
				}
				else if(equip[partID].IsArmor())
				{
					eqp2 = DataHolder.Armor(equip[partID].equipID);
				}
				if(eqp2.IsSingle())
				{
					equip[partID].Change(EquipSet.NONE, 0);
				}
				else if(eqp2.IsMulti())
				{
					for(int i=0; i<eqp2.equipPart.Length; i++)
					{
						if(eqp2.equipPart[i])
						{
							equip[i].Change(EquipSet.NONE, 0);
						}
					}
				}
				for(int i=0; i<eqp2.blockPart.Length; i++)
				{
					if(eqp2.blockPart[i])
					{
						equip[i].Change(EquipSet.NONE, 0);
					}
				}
			}
		}
		// equip new
		if(eqp != null && eqp.CanEquip(equipID, this))
		{
			// check partID, if -1 get first partID that matches
			if(partID == -1)
			{
				for(int i=0; i<eqp.equipPart.Length; i++)
				{
					if(eqp.equipPart[i] && DataHolder.Class(this.currentClass).equipPart[i])
					{
						partID = i;
						break;
					}
				}
			}
			if(eqp.IsSingle())
			{
				equip[partID].Change(type, equipID);
			}
			else if(eqp.IsMulti())
			{
				for(int i=0; i<eqp.equipPart.Length; i++)
				{
					if(eqp.equipPart[i])
					{
						equip[i].Change(type, equipID);
					}
				}
			}
			for(int i=0; i<eqp.blockPart.Length; i++)
			{
				if(eqp.blockPart[i])
				{
					equip[i].Change(EquipSet.NONE, partID, true);
				}
			}
		}
		return equip;
	}
	
	/*
	============================================================================
	Status value functions
	============================================================================
	*/
	public override void ResetStatus()
	{
		base.ResetStatus();
		int[] classBonus = DataHolder.Class(this.currentClass).bonus.GetStatusBonus();
		for(int i=0; i<classBonus.Length; i++)
		{
			if(classBonus[i] != 0)
			{
				this.status[i].AddValue(classBonus[i], false, false, false);
			}
		}
		
		int[] equipChanges = this.GetEquipChanges(this.equipment);
		for(int i=0; i<equipChanges.Length; i++)
		{
			if(equipChanges[i] != 0)
			{
				this.status[i].AddValue(equipChanges[i], false, false, false);
			}
		}
		
		int[] skillChanges = this.GetSkillStatusChanges();
		for(int i=0; i<skillChanges.Length; i++)
		{
			if(skillChanges[i] != 0)
			{
				this.status[i].AddValue(skillChanges[i], false, false, false);
			}
		}
		
		for(int i=0; i<this.status.Length; i++)
		{
			this.status[i].CheckBounds(true, false);
		}
		this.skillsChanged = true;
	}
	
	public int[] GetEquipChanges(EquipShort[] equips)
	{
		int[] changes = new int[this.status.Length];
		ArrayList checkedWeapons = new ArrayList();
		ArrayList checkedArmors = new ArrayList();
		for(int i=0; i<equips.Length; i++)
		{
			if(!equips[i].IsNone())
			{
				Equipment eqp = null;
				bool add = true;
				if(equips[i].IsWeapon())
				{
					eqp = DataHolder.Weapon(equips[i].equipID);
					if(eqp.IsMulti() && checkedWeapons.Contains(equips[i].equipID))
					{
						add = false;
					}
					else
					{
						checkedWeapons.Add(equips[i].equipID);
					}
				}
				else if(equips[i].IsArmor())
				{
					eqp = DataHolder.Armor(equips[i].equipID);
					if(eqp.IsMulti() && checkedArmors.Contains(equips[i].equipID))
					{
						add = false;
					}
					else
					{
						checkedArmors.Add(equips[i].equipID);
					}
				}
				if(add)
				{
					int[] b = eqp.bonus.GetStatusBonus();
					for(int j=0; j<b.Length; j++)
					{
						if(b[j] != 0)
						{
							changes[j] += b[j];
						}
					}
				}
			}
		}
		return changes;
	}
	
	public int[] GetSkillStatusChanges()
	{
		int[] changes = new int[this.status.Length];
		
		SkillLearn[] s = this.GetSkills();
		for(int i=0; i<s.Length; i++)
		{
			Skill sk = DataHolder.Skill(s[i].skillID);
			if(sk.isPassive)
			{
				int[] b = sk.level[s[i].GetLevel()].bonus.GetStatusBonus();
				for(int j=0; j<b.Length; j++)
				{
					if(b[j] != 0)
					{
						changes[j] += b[j];
					}
				}
			}
		}
		
		return changes;
	}
	
	/*
	============================================================================
	Effect functions
	============================================================================
	*/
	public override void SetStartEffects()
	{
		for(int i=0; i<this.equipment.Length; i++)
		{
			if(!this.equipment[i].IsNone())
			{
				Equipment eqp = null;
				if(this.equipment[i].IsWeapon())
				{
					eqp = DataHolder.Weapon(this.equipment[i].equipID);
				}
				else if(this.equipment[i].IsArmor())
				{
					eqp = DataHolder.Armor(this.equipment[i].equipID);
				}
				for(int j=0; j<eqp.skillEffect.Length; j++)
				{
					if(SkillEffect.ADD.Equals(eqp.skillEffect[j]))
					{
						this.AddEffect(j, this);
					}
				}
			}
		}
		// skills
		SkillLearn[] s = this.GetSkills();
		for(int i=0; i<s.Length; i++)
		{
			Skill sk = DataHolder.Skill(s[i].skillID);
			if(sk.isPassive)
			{
				for(int j=0; j<sk.level[s[i].GetLevel()].skillEffect.Length; j++)
				{
					if(SkillEffect.ADD.Equals(sk.level[s[i].GetLevel()].skillEffect[j]))
					{
						this.AddEffect(j, this);
					}
				}
			}
		}
	}
	
	public override bool CanApplyEffect(int effectID)
	{
		bool apply = true;
		for(int i=0; i<this.equipment.Length; i++)
		{
			if(this.equipment[i].IsWeapon())
			{
				if(!DataHolder.Weapon(this.equipment[i].equipID).CanApplyEffect(effectID))
				{
					apply = false;
					break;
				}
			}
			else if(this.equipment[i].IsArmor())
			{
				if(!DataHolder.Armor(this.equipment[i].equipID).CanApplyEffect(effectID))
				{
					apply = false;
					break;
				}
			}
		}
		// skills
		if(apply)
		{
			SkillLearn[] s = this.GetSkills();
			for(int i=0; i<s.Length; i++)
			{
				Skill sk = DataHolder.Skill(s[i].skillID);
				if(sk.isPassive && !sk.CanApplyEffect(effectID, s[i].GetLevel()))
				{
					apply = false;
					break;
				}
			}
		}
		return apply;
	}
	
	public override bool CanRemoveEffect(int effectID)
	{
		bool remove = true;
		for(int i=0; i<this.equipment.Length; i++)
		{
			if(this.equipment[i].IsWeapon())
			{
				if(!DataHolder.Weapon(this.equipment[i].equipID).CanRemoveEffect(effectID))
				{
					remove = false;
					break;
				}
			}
			else if(this.equipment[i].IsArmor())
			{
				if(!DataHolder.Armor(this.equipment[i].equipID).CanRemoveEffect(effectID))
				{
					remove = false;
					break;
				}
			}
		}
		// skills
		if(remove)
		{
			SkillLearn[] s = this.GetSkills();
			for(int i=0; i<s.Length; i++)
			{
				Skill sk = DataHolder.Skill(s[i].skillID);
				if(sk.isPassive && !sk.CanRemoveEffect(effectID, s[i].GetLevel()))
				{
					remove = false;
					break;
				}
			}
		}
		return remove;
	}
	
	/*
	============================================================================
	Skill functions
	============================================================================
	*/
	public SkillLearn[] GetSkills()
	{
		if(this.skillsChanged)
		{
			this.skillList = new SkillLearn[0];
			for(int i=0; i<this.skill.Length; i++)
			{
				this.skillList = ArrayHelper.Add(this.skill[i].GetCopy(), this.skillList);
			}
			for(int i=0; i<this.equipment.Length; i++)
			{
				if(this.equipment[i].IsWeapon())
				{
					Weapon wpn = DataHolder.Weapon(this.equipment[i].equipID);
					for(int j=0; j<wpn.equipmentSkill.Length; j++)
					{
						if(wpn.equipmentSkill[j].CanUseSkill(this))
						{
							bool add = true;
							for(int k=0; k<this.skillList.Length; k++)
							{
								if(this.skillList[k].skillID == wpn.equipmentSkill[j].skill)
								{
									add = false;
									if(this.skillList[k].skillLevel < wpn.equipmentSkill[j].skillLevel)
									{
										this.skillList[k].skillLevel = wpn.equipmentSkill[j].skillLevel;
									}
								}
							}
							if(add)
							{
								this.skillList = ArrayHelper.Add(new SkillLearn(
										wpn.equipmentSkill[j].skill, wpn.equipmentSkill[j].skillLevel), this.skillList);
							}
						}
					}
				}
				else if(this.equipment[i].IsArmor())
				{
					Armor arm = DataHolder.Armor(this.equipment[i].equipID);
					for(int j=0; j<arm.equipmentSkill.Length; j++)
					{
						if(arm.equipmentSkill[j].CanUseSkill(this))
						{
							bool add = true;
							for(int k=0; k<this.skillList.Length; k++)
							{
								if(this.skillList[k].skillID == arm.equipmentSkill[j].skill)
								{
									add = false;
									if(this.skillList[k].skillLevel < arm.equipmentSkill[j].skillLevel)
									{
										this.skillList[k].skillLevel = arm.equipmentSkill[j].skillLevel;
									}
								}
							}
							if(add)
							{
								 this.skillList = ArrayHelper.Add(new SkillLearn(
										arm.equipmentSkill[j].skill, arm.equipmentSkill[j].skillLevel), this.skillList);
							}
						}
					}
				}
			}
			this.skillsChanged = false;
		}
		return this.skillList;
	}
	
	public SkillLearn GetLearnedSkill(int id)
	{
		SkillLearn sk = null;
		for(int i=0; i<this.skill.Length; i++)
		{
			if(this.skill[i].skillID == id) sk = this.skill[i];
		}
		return sk;
	}
	
	public SkillLearn GetSkill(int id)
	{
		SkillLearn sk = null;
		SkillLearn[] s = this.GetSkills();
		for(int i=0; i<s.Length; i++)
		{
			if(s[i].skillID == id) sk = s[i];
		}
		return sk;
	}
	
	public void ChangeSkillUseLevel(int id, int change)
	{
		for(int i=0; i<this.skill.Length; i++)
		{
			if(this.skill[i].skillID == id) this.skill[i].ChangeUseLevel(change);
		}
		for(int i=0; i<this.skillList.Length; i++)
		{
			if(this.skillList[i].skillID == id) this.skillList[i].ChangeUseLevel(change);
		}
	}
	
	public bool ForgetSkill(int id)
	{
		bool forget = false;
		if(this.HasLearnedSkill(id, 0))
		{
			this.skill = ArrayHelper.Remove(this.GetLearnedSkill(id), this.skill);
			forget = true;
		}
		this.skillsChanged = true;
		this.ResetStatus();
		this.ResetEffects();
		return forget;
	}
	
	public bool LearnSkill(int id, int lvl)
	{
		bool learn = false;
		if(!this.HasLearnedSkill(id, lvl))
		{
			bool add = true;
			for(int i=0; i<this.skill.Length; i++)
			{
				if(this.skill[i].skillID == id)
				{
					this.skill[i].skillLevel = lvl;
					this.skill[i].SetUseLevel(lvl);
					add = false;
					break;
				}
			}
			if(add)
			{
				this.skill = ArrayHelper.Add(new SkillLearn(id, lvl), this.skill);
			}
			learn = true;
		}
		this.skillsChanged = true;
		this.ResetStatus();
		this.ResetEffects();
		return learn;
	}
	
	public bool HasLearnedSkill(int id, int lvl)
	{
		bool found = false;
		for(int i=0; i<this.skill.Length; i++)
		{
			if(this.skill[i].skillID == id &&
				this.skill[i].skillLevel >= lvl)
			{
				found = true;
				break;
			}
		}
		return found;
	}
	
	public override bool HasSkill(int id, int lvl)
	{
		bool found  = false;
		SkillLearn[] s = this.GetSkills();
		for(int i=0; i<s.Length; i++)
		{
			if(s[i].skillID == id &&
				s[i].skillLevel >= lvl)
			{
				found = true;
				break;
			}
		}
		return found;
	}
	
	public bool HasSkillType(int index)
	{
		bool has = false;
		SkillLearn[] s = this.GetSkills();
		for(int i=0; i<s.Length; i++)
		{
			if(DataHolder.Skill(s[i].skillID).skilltype == index)
			{
				has = true;
				break;
			}
		}
		return has;
	}
	
	public SkillLearn[] GetSkillsByType(int index)
	{
		SkillLearn[] sk = new SkillLearn[0];
		SkillLearn[] s = this.GetSkills();
		for(int i=0; i<s.Length; i++)
		{
			if(DataHolder.Skill(s[i].skillID).skilltype == index)
			{
				sk = ArrayHelper.Add(s[i].GetCopy(), sk);
			}
		}
		return sk;
	}
	
	/*
	============================================================================
	Element functions
	============================================================================
	*/
	public override int GetElementDefence(int index)
	{
		int def = base.GetElementDefence(index);
		def += DataHolder.Class(this.currentClass).bonus.GetElementDefence(index);
		bool isSet = false;
		// effects
		for(int i=0; i<this.effect.Length; i++)
		{
			def += this.effect[i].bonus.GetElementDefence(index);
			if(SimpleOperator.ADD.Equals(this.effect[i].elementOperator[index]))
			{
				def += this.effect[i].elementValue[index];
			}
			else if(SimpleOperator.SUB.Equals(this.effect[i].elementOperator[index]))
			{
				def -= this.effect[i].elementValue[index];
			}
			else if(SimpleOperator.SET.Equals(this.effect[i].elementOperator[index]))
			{
				def = this.effect[i].elementValue[index];
				isSet = true;
				break;
			}
		}
		// equipment
		if(!isSet)
		{
			for(int i=0; i<this.equipment.Length; i++)
			{
				Equipment e = this.equipment[i].GetEquipment();
				if(e != null)
				{
					def += e.bonus.GetElementDefence(index);
					if(SimpleOperator.ADD.Equals(e.elementOperator[index]))
					{
						def += e.elementValue[index];
					}
					else if(SimpleOperator.SUB.Equals(e.elementOperator[index]))
					{
						def -= e.elementValue[index];
					}
					else if(SimpleOperator.SET.Equals(e.elementOperator[index]))
					{
						def = e.elementValue[index];
						isSet = true;
						break;
					}
				}
			}
		}
		// skills
		if(!isSet)
		{
			SkillLearn[] s = this.GetSkills();
			for(int i=0; i<s.Length; i++)
			{
				Skill sk = DataHolder.Skill(s[i].skillID);
				if(sk.isPassive)
				{
					int lvl = s[i].GetLevel();
					def += sk.level[lvl].bonus.GetElementDefence(index);
					if(SimpleOperator.ADD.Equals(sk.level[lvl].elementOperator[index]))
					{
						def += sk.level[lvl].elementValue[index];
					}
					else if(SimpleOperator.SUB.Equals(sk.level[lvl].elementOperator[index]))
					{
						def -= sk.level[lvl].elementValue[index];
					}
					else if(SimpleOperator.SET.Equals(sk.level[lvl].elementOperator[index]))
					{
						def = sk.level[lvl].elementValue[index];
						isSet = true;
						break;
					}
				}
			}
		}
		return def;
	}
	
	public int[] GetElementChanges(EquipShort[] equips)
	{
		int[] def = new int[this.element.Length];
		ArrayList set = new ArrayList();
		for(int i=0; i<this.element.Length; i++)
		{
			for(int j=0; j<equips.Length; j++)
			{
				if(!set.Contains(i))
				{
					Equipment e = equips[j].GetEquipment();
					if(e != null)
					{
						def[i] += e.bonus.GetElementDefence(i);
						if(SimpleOperator.ADD.Equals(e.elementOperator[i]))
						{
							def[i]+= e.elementValue[i];
						}
						else if(SimpleOperator.SUB.Equals(e.elementOperator[i]))
						{
							def[i] -= e.elementValue[i];
						}
						else if(SimpleOperator.SET.Equals(e.elementOperator[i]))
						{
							def[i] = e.elementValue[i];
							set.Add(i);
						}
					}
				}
			}
		}
		return def;
	}
	
	/*
	============================================================================
	Race damage functions
	============================================================================
	*/
	public override int GetRaceDamageFactor(int index)
	{
		// base including effects
		int factor = base.GetRaceDamageFactor(index);
		factor += DataHolder.Class(this.currentClass).bonus.GetRaceDamageFactor(index);
		// equipment
		for(int i=0; i<this.equipment.Length; i++)
		{
			Equipment e = this.equipment[i].GetEquipment();
			if(e != null)
			{
				factor += e.raceValue[index]+e.bonus.GetRaceDamageFactor(index);
			}
		}
		// skills
		SkillLearn[] s = this.GetSkills();
		for(int i=0; i<s.Length; i++)
		{
			Skill sk = DataHolder.Skill(s[i].skillID);
			if(sk.isPassive)
			{
				int lvl = s[i].GetLevel();
				factor += sk.level[lvl].raceValue[index]+sk.level[lvl].bonus.GetRaceDamageFactor(index);
			}
		}
		return factor;
	}
	
	/*
	============================================================================
	Size damage functions
	============================================================================
	*/
	public override int GetSizeDamageFactor(int index)
	{
		// base including effects
		int factor = base.GetSizeDamageFactor(index);
		factor += DataHolder.Class(this.currentClass).bonus.GetSizeDamageFactor(index);
		// equipment
		for(int i=0; i<this.equipment.Length; i++)
		{
			Equipment e = this.equipment[i].GetEquipment();
			if(e != null)
			{
				factor += e.sizeValue[index]+e.bonus.GetSizeDamageFactor(index);
			}
		}
		// skills
		SkillLearn[] s = this.GetSkills();
		for(int i=0; i<s.Length; i++)
		{
			Skill sk = DataHolder.Skill(s[i].skillID);
			if(sk.isPassive)
			{
				int lvl = s[i].GetLevel();
				factor += sk.level[lvl].sizeValue[index]+sk.level[lvl].bonus.GetSizeDamageFactor(index);
			}
		}
		return factor;
	}
	
	/*
	============================================================================
	Bonus functions
	============================================================================
	*/
	public override float GetHitBonus()
	{
		float bonus = base.GetHitBonus();
		bonus += DataHolder.Class(this.currentClass).bonus.GetHitBonus();
		
		ArrayList checkedWeapons = new ArrayList();
		ArrayList checkedArmors = new ArrayList();
		for(int i=0; i<this.equipment.Length; i++)
		{
			if(!this.equipment[i].IsNone())
			{
				Equipment eqp = null;
				bool add = true;
				if(this.equipment[i].IsWeapon())
				{
					eqp = DataHolder.Weapon(this.equipment[i].equipID);
					if(eqp.IsMulti() && checkedWeapons.Contains(this.equipment[i].equipID))
					{
						add = false;
					}
					else
					{
						checkedWeapons.Add(this.equipment[i].equipID);
					}
				}
				else if(this.equipment[i].IsArmor())
				{
					eqp = DataHolder.Armor(this.equipment[i].equipID);
					if(eqp.IsMulti() && checkedArmors.Contains(this.equipment[i].equipID))
					{
						add = false;
					}
					else
					{
						checkedArmors.Add(this.equipment[i].equipID);
					}
				}
				if(add)
				{
					bonus += eqp.bonus.GetHitBonus();
				}
			}
		}
		// skills
		SkillLearn[] s = this.GetSkills();
		for(int i=0; i<s.Length; i++)
		{
			Skill sk = DataHolder.Skill(s[i].skillID);
			if(sk.isPassive)
			{
				bonus += sk.level[s[i].GetLevel()].bonus.GetHitBonus();
			}
		}
		return bonus;
	}
	
	public override float GetCounterBonus()
	{
		float bonus = base.GetCounterBonus();
		bonus += DataHolder.Class(this.currentClass).bonus.GetCounterBonus();
		
		ArrayList checkedWeapons = new ArrayList();
		ArrayList checkedArmors = new ArrayList();
		for(int i=0; i<this.equipment.Length; i++)
		{
			if(!this.equipment[i].IsNone())
			{
				Equipment eqp = null;
				bool add = true;
				if(this.equipment[i].IsWeapon())
				{
					eqp = DataHolder.Weapon(this.equipment[i].equipID);
					if(eqp.IsMulti() && checkedWeapons.Contains(this.equipment[i].equipID))
					{
						add = false;
					}
					else
					{
						checkedWeapons.Add(this.equipment[i].equipID);
					}
				}
				else if(this.equipment[i].IsArmor())
				{
					eqp = DataHolder.Armor(this.equipment[i].equipID);
					if(eqp.IsMulti() && checkedArmors.Contains(this.equipment[i].equipID))
					{
						add = false;
					}
					else
					{
						checkedArmors.Add(this.equipment[i].equipID);
					}
				}
				if(add)
				{
					bonus += eqp.bonus.GetCounterBonus();
				}
			}
		}
		// skills
		SkillLearn[] s = this.GetSkills();
		for(int i=0; i<s.Length; i++)
		{
			Skill sk = DataHolder.Skill(s[i].skillID);
			if(sk.isPassive)
			{
				bonus += sk.level[s[i].GetLevel()].bonus.GetCounterBonus();
			}
		}
		return bonus;
	}
	
	public override float GetCriticalBonus()
	{
		float bonus = base.GetCriticalBonus();
		bonus += DataHolder.Class(this.currentClass).bonus.GetCriticalBonus();
		
		ArrayList checkedWeapons = new ArrayList();
		ArrayList checkedArmors = new ArrayList();
		for(int i=0; i<this.equipment.Length; i++)
		{
			if(!this.equipment[i].IsNone())
			{
				Equipment eqp = null;
				bool add = true;
				if(this.equipment[i].IsWeapon())
				{
					eqp = DataHolder.Weapon(this.equipment[i].equipID);
					if(eqp.IsMulti() && checkedWeapons.Contains(this.equipment[i].equipID))
					{
						add = false;
					}
					else
					{
						checkedWeapons.Add(this.equipment[i].equipID);
					}
				}
				else if(this.equipment[i].IsArmor())
				{
					eqp = DataHolder.Armor(this.equipment[i].equipID);
					if(eqp.IsMulti() && checkedArmors.Contains(this.equipment[i].equipID))
					{
						add = false;
					}
					else
					{
						checkedArmors.Add(this.equipment[i].equipID);
					}
				}
				if(add)
				{
					bonus += eqp.bonus.GetCriticalBonus();
				}
			}
		}
		// skills
		SkillLearn[] s = this.GetSkills();
		for(int i=0; i<s.Length; i++)
		{
			Skill sk = DataHolder.Skill(s[i].skillID);
			if(sk.isPassive)
			{
				bonus += sk.level[s[i].GetLevel()].bonus.GetCriticalBonus();
			}
		}
		return bonus;
	}
	
	public override float GetEscapeBonus()
	{
		float bonus = base.GetEscapeBonus();
		bonus += DataHolder.Class(this.currentClass).bonus.GetEscapeBonus();
		
		ArrayList checkedWeapons = new ArrayList();
		ArrayList checkedArmors = new ArrayList();
		for(int i=0; i<this.equipment.Length; i++)
		{
			if(!this.equipment[i].IsNone())
			{
				Equipment eqp = null;
				bool add = true;
				if(this.equipment[i].IsWeapon())
				{
					eqp = DataHolder.Weapon(this.equipment[i].equipID);
					if(eqp.IsMulti() && checkedWeapons.Contains(this.equipment[i].equipID))
					{
						add = false;
					}
					else
					{
						checkedWeapons.Add(this.equipment[i].equipID);
					}
				}
				else if(this.equipment[i].IsArmor())
				{
					eqp = DataHolder.Armor(this.equipment[i].equipID);
					if(eqp.IsMulti() && checkedArmors.Contains(this.equipment[i].equipID))
					{
						add = false;
					}
					else
					{
						checkedArmors.Add(this.equipment[i].equipID);
					}
				}
				if(add)
				{
					bonus += eqp.bonus.GetEscapeBonus();
				}
			}
		}
		// skills
		SkillLearn[] s = this.GetSkills();
		for(int i=0; i<s.Length; i++)
		{
			Skill sk = DataHolder.Skill(s[i].skillID);
			if(sk.isPassive)
			{
				bonus += sk.level[s[i].GetLevel()].bonus.GetEscapeBonus();
			}
		}
		return bonus;
	}
	
	public override float GetBlockBonus()
	{
		float bonus = base.GetBlockBonus();
		bonus += DataHolder.Class(this.currentClass).bonus.GetBlockBonus();
		
		ArrayList checkedWeapons = new ArrayList();
		ArrayList checkedArmors = new ArrayList();
		for(int i=0; i<this.equipment.Length; i++)
		{
			if(!this.equipment[i].IsNone())
			{
				Equipment eqp = null;
				bool add = true;
				if(this.equipment[i].IsWeapon())
				{
					eqp = DataHolder.Weapon(this.equipment[i].equipID);
					if(eqp.IsMulti() && checkedWeapons.Contains(this.equipment[i].equipID))
					{
						add = false;
					}
					else
					{
						checkedWeapons.Add(this.equipment[i].equipID);
					}
				}
				else if(this.equipment[i].IsArmor())
				{
					eqp = DataHolder.Armor(this.equipment[i].equipID);
					if(eqp.IsMulti() && checkedArmors.Contains(this.equipment[i].equipID))
					{
						add = false;
					}
					else
					{
						checkedArmors.Add(this.equipment[i].equipID);
					}
				}
				if(add)
				{
					bonus += eqp.bonus.GetBlockBonus();
				}
			}
		}
		// skills
		SkillLearn[] s = this.GetSkills();
		for(int i=0; i<s.Length; i++)
		{
			Skill sk = DataHolder.Skill(s[i].skillID);
			if(sk.isPassive)
			{
				bonus += sk.level[s[i].GetLevel()].bonus.GetBlockBonus();
			}
		}
		return bonus;
	}
	
	public override float GetItemStealBonus()
	{
		float bonus = base.GetItemStealBonus();
		bonus += DataHolder.Class(this.currentClass).bonus.GetItemStealBonus();
		
		ArrayList checkedWeapons = new ArrayList();
		ArrayList checkedArmors = new ArrayList();
		for(int i=0; i<this.equipment.Length; i++)
		{
			if(!this.equipment[i].IsNone())
			{
				Equipment eqp = null;
				bool add = true;
				if(this.equipment[i].IsWeapon())
				{
					eqp = DataHolder.Weapon(this.equipment[i].equipID);
					if(eqp.IsMulti() && checkedWeapons.Contains(this.equipment[i].equipID))
					{
						add = false;
					}
					else
					{
						checkedWeapons.Add(this.equipment[i].equipID);
					}
				}
				else if(this.equipment[i].IsArmor())
				{
					eqp = DataHolder.Armor(this.equipment[i].equipID);
					if(eqp.IsMulti() && checkedArmors.Contains(this.equipment[i].equipID))
					{
						add = false;
					}
					else
					{
						checkedArmors.Add(this.equipment[i].equipID);
					}
				}
				if(add)
				{
					bonus += eqp.bonus.GetItemStealBonus();
				}
			}
		}
		// skills
		SkillLearn[] s = this.GetSkills();
		for(int i=0; i<s.Length; i++)
		{
			Skill sk = DataHolder.Skill(s[i].skillID);
			if(sk.isPassive)
			{
				bonus += sk.level[s[i].GetLevel()].bonus.GetItemStealBonus();
			}
		}
		return bonus;
	}
	
	public override float GetMoneyStealBonus()
	{
		float bonus = base.GetMoneyStealBonus();
		bonus += DataHolder.Class(this.currentClass).bonus.GetMoneyStealBonus();
		
		ArrayList checkedWeapons = new ArrayList();
		ArrayList checkedArmors = new ArrayList();
		for(int i=0; i<this.equipment.Length; i++)
		{
			if(!this.equipment[i].IsNone())
			{
				Equipment eqp = null;
				bool add = true;
				if(this.equipment[i].IsWeapon())
				{
					eqp = DataHolder.Weapon(this.equipment[i].equipID);
					if(eqp.IsMulti() && checkedWeapons.Contains(this.equipment[i].equipID))
					{
						add = false;
					}
					else
					{
						checkedWeapons.Add(this.equipment[i].equipID);
					}
				}
				else if(this.equipment[i].IsArmor())
				{
					eqp = DataHolder.Armor(this.equipment[i].equipID);
					if(eqp.IsMulti() && checkedArmors.Contains(this.equipment[i].equipID))
					{
						add = false;
					}
					else
					{
						checkedArmors.Add(this.equipment[i].equipID);
					}
				}
				if(add)
				{
					bonus += eqp.bonus.GetMoneyStealBonus();
				}
			}
		}
		// skills
		SkillLearn[] s = this.GetSkills();
		for(int i=0; i<s.Length; i++)
		{
			Skill sk = DataHolder.Skill(s[i].skillID);
			if(sk.isPassive)
			{
				bonus += sk.level[s[i].GetLevel()].bonus.GetMoneyStealBonus();
			}
		}
		return bonus;
	}
	
	/*
	============================================================================
	Attack functions
	============================================================================
	*/
	public override bool IsBlockAttack()
	{
		bool block = base.IsBlockAttack();
		
		if(!block)
		{
			bool found = false;
			for(int i=0; i<this.equipment.Length; i++)
			{
				if(this.equipment[i].IsWeapon())
				{
					Weapon wpn = DataHolder.Weapon(this.equipment[i].equipID);
					if(wpn.ownAttack)
					{
						if(!DataHolder.BaseAttack(wpn.baseAttack[this.baIndex]).CheckItems())
						{
							block = true;
						}
						found = true;
					}
				}
			}
			
			if(!found && !DataHolder.BaseAttack(this.baseAttack[this.baIndex]).CheckItems())
			{
				block = true;
			}
		}
		return block;
	}
	
	public override void NextBaseAttack()
	{
		this.baIndex++;
		bool found = false;
		for(int i=0; i<this.equipment.Length; i++)
		{
			if(this.equipment[i].IsWeapon())
			{
				Weapon wpn = DataHolder.Weapon(this.equipment[i].equipID);
				if(wpn.ownAttack)
				{
					if(this.baIndex >= wpn.baseAttack.Length)
					{
						this.baIndex = 0;
					}
					if(DataHolder.BaseAttack(wpn.baseAttack[this.baIndex]).availableTime > 0)
					{
						this.baTimeout = DataHolder.BaseAttack(wpn.baseAttack[this.baIndex]).availableTime;
					}
					else this.baTimeout = -10;
					found = true;
				}
			}
		}
		if(!found && this.baIndex >= this.baseAttack.Length)
		{
			this.baIndex = 0;
		}
		if(!found)
		{
			if(DataHolder.BaseAttack(this.baseAttack[this.baIndex]).availableTime > 0)
			{
				this.baTimeout = DataHolder.BaseAttack(this.baseAttack[this.baIndex]).availableTime;
			}
			else this.baTimeout = -10;
		}
	}
	
	public override bool UseBaseAttack(Combatant target, BattleAction ba, 
			bool counter, float damageFactor, float damageMultiplier)
	{
		bool hit = false;
		bool found = false;
		for(int i=0; i<this.equipment.Length; i++)
		{
			if(this.equipment[i].IsWeapon())
			{
				Weapon wpn = DataHolder.Weapon(this.equipment[i].equipID);
				if(wpn.ownAttack)
				{
					if(wpn.UseBaseAttack(this, target, damageFactor, damageMultiplier))
					{
						hit = true;
					}
					found = true;
				}
			}
		}
		
		if(!found)
		{
			hit = DataHolder.BaseAttack(this.baseAttack[this.baIndex]).Use(this, this.baseElement, target, damageFactor, damageMultiplier);
		}
		
		if(!this.isDead && counter && this.battleID != target.battleID)
		{
			if(DataHolder.BattleSystem().IsRealTime()) target.UseCounter(this);
			else if(ba != null) ba.doCounter = new bool[] {target.UseCounter(this)};
		}
		
		return hit;
	}
	
	/*
	============================================================================
	Info functions
	============================================================================
	*/
	public override string GetName()
	{
		string n = "";
		if(this.newName != "") n = this.newName;
		else if(this.realID >= 0) n = DataHolder.Characters().GetName(this.realID);
		return n;
	}
	
	public Texture2D GetIcon()
	{
		Texture2D i = null;
		if(this.realID >= 0) i = DataHolder.Characters().GetIcon(this.realID);
		return i;
	}
	
	public GUIContent GetContent()
	{
		GUIContent g = null;
		if(this.realID >= 0) g = DataHolder.Characters().GetContent(this.realID);
		if(this.newName != "" && g != null)
		{
			g.text = this.newName;
		}
		return g;
	}
	
	public void SetName(string n)
	{
		this.newName = n;
	}
	
	/*
	============================================================================
	Escape functions
	============================================================================
	*/
	public override void Escape()
	{
		base.Escape();
		DataHolder.BattleSystem().BattleEscaped();
	}
	
	/*
	============================================================================
	Death functions
	============================================================================
	*/
	public override void Died()
	{
		base.Died();
		if(this.leaveOnDeath)
		{
			DataHolder.BattleSystem().RemoveCharacter(this);
			GameHandler.Party().RemoveFromParty(this.realID);
		}
		DataHolder.BattleSystem().CheckBattleEnd();
	}
	
	/*
	============================================================================
	Level up functions
	============================================================================
	*/
	public string CheckLevelUp()
	{
		string changes = "";
		string tmp = "";
		Class c = DataHolder.Class(this.currentClass);
		do
		{
			tmp = "";
			for(int i=0; i<this.status.Length; i++)
			{
				// base level up
				if(this.status[i].IsExperience() && this.status[i].levelUp &&
					this.currentLevel < this.development.maxLevel && 
					this.status[i].GetValue() >= this.GetValueAtLevel(i, this.currentLevel+1))
				{
					tmp += this.LevelUp();
				}
				// class level up
				if(c.useClassLevel && this.status[i].IsExperience() && this.status[i].levelUpClass &&
					this.currentClassLevel < c.development.maxLevel && 
					this.status[i].GetValue() >= c.development.GetValueAtLevel(i, this.currentClassLevel+1))
				{
					if("" != tmp) tmp += "\n";
					tmp += this.ClassLevelUp();
				}
			}
			if("" != changes) changes += "\n";
			if("" != tmp) changes += tmp;
		} while(tmp != "");
		return changes;
	}
	
	public string ForceLevelUp()
	{
		string changes = "";
		for(int i=0; i<this.status.Length; i++)
		{
			if(this.status[i].IsExperience() && this.status[i].levelUp)
			{
				this.status[i].SetValue(this.GetValueAtLevel(i, this.currentLevel+1), true, false, false);
				string tmp = this.LevelUp();
				if("" != tmp) changes = tmp;
			}
		}
		return changes;
	}
	
	public string ForceClassLevelUp()
	{
		string changes = "";
		Class c = DataHolder.Class(this.currentClass);
		if(c.useClassLevel)
		{
			for(int i=0; i<this.status.Length; i++)
			{
				if(this.status[i].IsExperience() && this.status[i].levelUpClass)
				{
					this.status[i].SetValue(this.GetValueAtLevel(i, this.currentClassLevel+1), true, false, false);
					string tmp = this.ClassLevelUp();
					if("" != tmp) changes = tmp;
				}
			}
		}
		return changes;
	}
	
	public string LevelUp()
	{
		string changes = "";
		if(this.currentLevel < this.development.maxLevel)
		{
			this.currentLevel++;
			DataHolder.BattleSystemData().levelUpTextSettings.ShowText(
					this.currentLevel.ToString(), this);
			
			string levelText = DataHolder.BattleEnd().GetLevelUpText(this.realID, this.currentLevel);
			if("" != levelText) levelText += "\n";
			
			string statusText = this.development.IncreaseStatus(this, this.currentLevel);
			string skillText = this.development.LearnSkills(this, this.currentLevel);
			
			if(!DataHolder.Class(this.currentClass).useClassLevel)
			{
				string tmp = DataHolder.Class(this.currentClass).development.LearnSkills(this, this.currentLevel);
				if("" != tmp) skillText += tmp;
			}
			
			for(int i=0; i<DataHolder.BattleEnd().levelUpOrder.Length; i++)
			{
				if(DataHolder.BattleEnd().levelUpOrder[i] == BattleEnd.LEVELUP)
				{
					changes += levelText;
				}
				else if(DataHolder.BattleEnd().levelUpOrder[i] == BattleEnd.STATUS)
				{
					changes += statusText;
				}
				else if(DataHolder.BattleEnd().levelUpOrder[i] == BattleEnd.SKILL)
				{
					changes += skillText;
				}
			}
		}
		this.skillsChanged = true;
		return changes;
	}
	
	public string ClassLevelUp()
	{
		string changes = "";
		Class c = DataHolder.Class(this.currentClass);
		if(this.currentClassLevel < c.development.maxLevel)
		{
			this.currentClassLevel++;
			DataHolder.BattleSystemData().classLevelUpTextSettings.ShowText(
					this.currentClassLevel.ToString(), this);
			
			string levelText = DataHolder.BattleEnd().GetClassLevelUpText(
					this.realID, this.currentClass, this.currentClassLevel);
			if("" != levelText) levelText += "\n";
			
			string statusText = c.development.IncreaseStatus(this, this.currentClassLevel);
			string skillText = c.development.LearnSkills(this, this.currentClassLevel);
			
			for(int i=0; i<DataHolder.BattleEnd().levelUpOrder.Length; i++)
			{
				if(DataHolder.BattleEnd().levelUpOrder[i] == BattleEnd.LEVELUP)
				{
					changes += levelText;
				}
				else if(DataHolder.BattleEnd().levelUpOrder[i] == BattleEnd.STATUS)
				{
					changes += statusText;
				}
				else if(DataHolder.BattleEnd().levelUpOrder[i] == BattleEnd.SKILL)
				{
					changes += skillText;
				}
			}
		}
		this.skillsChanged = true;
		return changes;
	}
	
	/*
	============================================================================
	Animation functions
	============================================================================
	*/
	public override string GetAnimationName(CombatantAnimation type)
	{
		string name = base.GetAnimationName(type);
		for(int i=0; i<this.equipment.Length; i++)
		{
			if(this.equipment[i].IsWeapon())
			{
				Weapon wpn = DataHolder.Weapon(this.equipment[i].equipID);
				if(wpn.ownBaseAnimations)
				{
					name = wpn.battleAnimations.GetAnimationName(type);
					break;
				}
			}
		}
		return name;
	}
	
	public string GetVictoryAnimationName()
	{
		string name = this.battleAnimations.victory.name;
		for(int i=0; i<this.equipment.Length; i++)
		{
			if(this.equipment[i].IsWeapon())
			{
				Weapon wpn = DataHolder.Weapon(this.equipment[i].equipID);
				if(wpn.ownBaseAnimations)
				{
					name = wpn.battleAnimations.victory.name;
					break;
				}
			}
		}
		return name;
	}
	
	public string GetIdleAfterAnimationName()
	{
		string name = this.battleAnimations.idleAfter.name;
		for(int i=0; i<this.equipment.Length; i++)
		{
			if(this.equipment[i].IsWeapon())
			{
				Weapon wpn = DataHolder.Weapon(this.equipment[i].equipID);
				if(wpn.ownBaseAnimations)
				{
					name = wpn.battleAnimations.idleAfter.name;
					break;
				}
			}
		}
		return name;
	}
	
	public override int GetBaseAttackAnimation()
	{
		int id = -1;
		for(int i=0; i<this.equipment.Length; i++)
		{
			if(this.equipment[i].IsWeapon())
			{
				Weapon wpn = DataHolder.Weapon(this.equipment[i].equipID);
				id = wpn.battleAnimations.GetBaseAttackAnimation();
				if(wpn.ownAttack && DataHolder.BaseAttack(wpn.baseAttack[this.baIndex]).overrideAnimation)
				{
					id = DataHolder.BaseAttack(wpn.baseAttack[this.baIndex]).animationID;
				}
				if(id != -1) break;
			}
		}
		if(id == -1)
		{
			id = base.GetBaseAttackAnimation();
		}
		return id;
	}
	
	public override int GetDefendAnimation()
	{
		int id = -1;
		for(int i=0; i<this.equipment.Length; i++)
		{
			if(this.equipment[i].IsWeapon())
			{
				id = DataHolder.Weapon(this.equipment[i].equipID).battleAnimations.GetDefendAnimation();
				if(id != -1) break;
			}
		}
		if(id == -1) id = base.GetDefendAnimation();
		return id;
	}
	
	public override int GetEscapeAnimation()
	{
		int id = -1;
		for(int i=0; i<this.equipment.Length; i++)
		{
			if(this.equipment[i].IsWeapon())
			{
				id = DataHolder.Weapon(this.equipment[i].equipID).battleAnimations.GetEscapeAnimation();
				if(id != -1) break;
			}
		}
		if(id == -1) id = base.GetEscapeAnimation();
		return id;
	}
	
	public override int GetDeathAnimation()
	{
		int id = -1;
		for(int i=0; i<this.equipment.Length; i++)
		{
			if(this.equipment[i].IsWeapon())
			{
				id = DataHolder.Weapon(this.equipment[i].equipID).battleAnimations.GetDeathAnimation();
				if(id != -1) break;
			}
		}
		if(id == -1) id = base.GetDeathAnimation();
		return id;
	}
	
	/*
	============================================================================
	Battle functions
	============================================================================
	*/
	public override void StartBattle(int bID)
	{
		base.StartBattle(bID);
		this.battleMenu.Clear();
	}
	
	public override void EndBattle()
	{
		base.EndBattle();
		this.battleMenu.Clear();
	}
	
	public override void ChooseAction()
	{
		base.ChooseAction();
		if(this.IsStopMove() || (this.IsAutoAttack() && this.IsBlockAttack()) ||
				(this.IsAttackFriends() && this.IsBlockAttack()))
		{
			this.AddAction(new BattleAction(AttackSelection.NONE, this, this.battleID, -1, 0));
		}
		else if(this.IsAutoAttack())
		{
			this.AddAction(new BattleAction(AttackSelection.ATTACK, this, 
					this.GetRandomTarget(DataHolder.BattleSystem().enemies), -1, 0));
		}
		else if(this.IsAttackFriends())
		{
			this.AddAction(new BattleAction(AttackSelection.ATTACK, this, 
					this.GetRandomTarget(GameHandler.Party().GetBattleParty()), -1, 0));
		}
		else if(this.aiControlled && this != GameHandler.Party().GetPlayerCharacter())
		{
			this.AddAction(this.GetAIBehaviourAction(
					GameHandler.Party().GetBattleParty(), DataHolder.BattleSystem().enemies));
		}
		else
		{
			this.ShowBattleMenu(BattleMenuMode.BASE, null);
		}
	}
	
	public override void CheckRealTimeAction()
	{
		if(this.aiControlled && 
			this != GameHandler.Party().GetPlayerCharacter())
		{
			this.ChooseAction();
		}
	}
	
	public override void EndTurn()
	{
		if(this.isChoosingAction) this.EndBattleMenu(false);
		base.EndTurn();
	}
	
	public void CallSkillMenu()
	{
		if(!this.IsBlockSkills())
		{
			base.ChooseAction();
			this.ShowBattleMenu(BattleMenuMode.SKILL, null);
		}
	}
	
	public void CallItemMenu()
	{
		if(!this.IsBlockItems())
		{
			base.ChooseAction();
			this.ShowBattleMenu(BattleMenuMode.ITEM, null);
		}
	}
	
	public void CallTargetMenu(string type, int id, int useLevel)
	{
		base.ChooseAction();
		this.ShowBattleMenu(BattleMenuMode.TARGET, 
				new BattleMenuItem(new GUIContent(""), id, useLevel, type, true, "", null));
	}
	
	public bool IsCalledMenuMode(BattleMenuMode mode)
	{
		bool same = false;
		if(this.isChoosingAction && this.battleMenu.callMode.Equals(mode))
		{
			same = true;
		}
		return same;
	}
	
	/*
	============================================================================
	Battle menu functions
	============================================================================
	*/
	public void ShowBattleMenu(BattleMenuMode mode, BattleMenuItem bmi)
	{
		if(DataHolder.BattleSystem().IsRealTime())
		{
			if(DataHolder.BattleSystem().blockControlMenu) GameHandler.SetBlockControl(1);
			if(DataHolder.BattleSystem().freezeAction) GameHandler.FreezeTime(true);
		}
		this.battleMenu.SetOwner(this);
		this.battleMenu.CallMenu(mode, bmi);
		this.isChoosingAction = true;
		GameHandler.GetLevelHandler().AddBattleMenuUser(this);
	}
	
	public void EndBattleMenu(bool cancel)
	{
		GameHandler.GetLevelHandler().RemoveBattleMenuUser(this);
		DataHolder.BattleSystem().BattleMenuFinished();
		this.battleMenu.Clear();
		this.isChoosingAction = false;
		if(cancel) this.waitForAction = false;
		if(DataHolder.BattleSystem().IsRealTime())
		{
			if(DataHolder.BattleSystem().blockControlMenu) GameHandler.SetBlockControl(-1);
			if(DataHolder.BattleSystem().freezeAction) GameHandler.FreezeTime(false);
		}
		DataHolder.BattleSystem().TargetSelectionOff();
	}
	
	/*
	============================================================================
	Savegame functions
	============================================================================
	*/
	private static string STATUS = "status";
	private static string SKILL = "skill";
	private static string EQUIPMENT = "equipment";
	public static string NEWNAME = "newname";
	
	public Hashtable GetSaveData(Hashtable ht)
	{
		ArrayList s = new ArrayList();
		
		ht.Add("realid", this.realID.ToString());
		ht.Add("class", this.currentClass.ToString());
		ht.Add("level", this.currentLevel.ToString());
		ht.Add("classlevel", this.currentClassLevel.ToString());
		if(this.isDead)
		{
			ht.Add("dead", "true");
		}
		
		for(int i=0; i<this.status.Length; i++)
		{
			Hashtable ht2 = new Hashtable();
			ht2.Add(XMLHandler.NODE_NAME, Character.STATUS);
			ht2.Add("id", i.ToString());
			ht2.Add("base", this.status[i].GetBaseValue().ToString());
			ht2.Add("current", this.status[i].GetValue().ToString());
			s.Add(ht2);
		}
		for(int i=0; i<this.skill.Length; i++)
		{
			s.Add(this.skill[i].GetData(HashtableHelper.GetTitleHashtable(Character.SKILL), true));
		}
		for(int i=0; i<this.equipment.Length; i++)
		{
			if(!EquipSet.NONE.Equals(this.equipment[i].type))
			{
				Hashtable ht2 = new Hashtable();
				ht2.Add(XMLHandler.NODE_NAME, Character.EQUIPMENT);
				ht2.Add("id", i.ToString());
				ht2.Add("type", this.equipment[i].type.ToString());
				ht2.Add("equipid", this.equipment[i].equipID.ToString());
				s.Add(ht2);
			}
		}
		if(this.prefabInstance && 
			DataHolder.LoadSaveHUD().savePartyPosition)
		{
			ht.Add("posx", this.prefabInstance.transform.position.x.ToString());
			ht.Add("posy", this.prefabInstance.transform.position.y.ToString());
			ht.Add("posz", this.prefabInstance.transform.position.z.ToString());
			ht.Add("rotx", this.prefabInstance.transform.rotation.x.ToString());
			ht.Add("roty", this.prefabInstance.transform.rotation.y.ToString());
			ht.Add("rotz", this.prefabInstance.transform.rotation.z.ToString());
			ht.Add("rotw", this.prefabInstance.transform.rotation.w.ToString());
		}
		if(this.newName != "")
		{
			s.Add(HashtableHelper.GetContentHashtable(Character.NEWNAME, this.newName));
		}
		
		if(s.Count > 0) ht.Add(XMLHandler.NODES, s);
		return ht;
	}
	
	public void SetSaveData(Hashtable ht)
	{
		if(ht.ContainsKey("level")) this.Init(int.Parse((string)ht["level"]));
		else this.Init();
		if(ht.ContainsKey("class"))
		{
			this.currentClass = int.Parse((string)ht["class"]);
		}
		if(ht.ContainsKey("dead")) this.isDead = true;
		if(ht.ContainsKey("classlevel"))
		{
			this.currentClassLevel = int.Parse((string)ht["classlevel"]);
		}
		this.skill = new SkillLearn[0];
		
		if(ht.ContainsKey(XMLHandler.NODES))
		{
			ArrayList s = ht[XMLHandler.NODES] as ArrayList;
			foreach(Hashtable ht2 in s)
			{
				if(ht2[XMLHandler.NODE_NAME] as string == Character.STATUS)
				{
					int i = int.Parse((string)ht2["id"]);
					this.status[i].SetBaseValue(int.Parse((string)ht2["base"]));
					this.status[i].SetValue(int.Parse((string)ht2["current"]), true, false, false);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == Character.SKILL)
				{
					this.skill = ArrayHelper.Add(new SkillLearn(ht2, true), this.skill);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == Character.EQUIPMENT)
				{
					this.equipment[int.Parse((string)ht2["id"])].Change(
							(EquipSet)System.Enum.Parse(typeof(EquipSet), (string)ht2["type"]), int.Parse((string)ht2["equipid"]));
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == Character.NEWNAME)
				{
					this.newName = ht2[XMLHandler.CONTENT] as string;
				}
			}
		}
		for(int i=0; i<this.status.Length; i++) this.status[i].CheckBounds();
		this.ResetStatus();
		this.ResetEffects();
		if(ht.ContainsKey("posx"))
		{
			this.CreatePrefabInstance();
			if(this.prefabInstance)
			{
				this.prefabInstance.transform.position = new Vector3(
						float.Parse((string)ht["posx"]),
						float.Parse((string)ht["posy"]),
						float.Parse((string)ht["posz"]));
				this.prefabInstance.transform.rotation = new Quaternion(
						float.Parse((string)ht["rotx"]),
						float.Parse((string)ht["roty"]),
						float.Parse((string)ht["rotz"]),
						float.Parse((string)ht["rotw"]));
			}
		}
	}
}