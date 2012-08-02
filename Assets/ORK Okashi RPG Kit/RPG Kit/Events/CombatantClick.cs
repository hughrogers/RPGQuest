
using UnityEngine;
using System.Collections;

public class CombatantClick : DropInteraction
{
	public Combatant combatant;
	
	void Update()
	{
		if(combatant is Enemy && !GameHandler.IsGamePaused() &&
			(GameHandler.IsControlField() || GameHandler.IsControlBattle()))
		{
			combatant.Tick();
		}
	}
	
	public override bool DropInteract(ChoiceContent drop)
	{
		bool dropped = false;
		if(drop == null)
		{
			GameHandler.GetLevelHandler().BattleTargetClicked(this.combatant.battleID);
		}
		else if(DragOrigin.INVENTORY.Equals(drop.dragOrigin))
		{
			if(DragType.ITEM.Equals(drop.dragType))
			{
				dropped = this.UseItem(drop.dragID);
			}
			else if(DragType.SKILL.Equals(drop.dragType))
			{
				dropped = this.UseSkill(drop.dragID, drop.characterID, drop.dragLevel);
			}
			else if(DragType.WEAPON.Equals(drop.dragType) ||
				DragType.ARMOR.Equals(drop.dragType))
			{
				dropped = this.Equip(drop.dragID, drop.dragType);
			}
		}
		else if(DragOrigin.BATTLE_MENU.Equals(drop.dragOrigin))
		{
			Combatant c = DataHolder.BattleSystem().GetCombatantForBattleID(drop.characterID);
			if(c != null)
			{
				if(DragType.ITEM.Equals(drop.dragType))
				{
					dropped = this.BattleItem(drop.dragID, c);
				}
				else if(DragType.SKILL.Equals(drop.dragType))
				{
					dropped = this.BattleSkill(drop.dragID, c, drop.dragLevel);
				}
				else if(DragType.ATTACK.Equals(drop.dragType))
				{
					dropped = this.BattleAttack(c);
				}
				if(dropped && c is Character)
				{
					((Character)c).EndBattleMenu(false);
					GameHandler.GetLevelHandler().ExitBattleMenu();
				}
			}
		}
		return dropped;
	}
	
	// field
	private bool UseItem(int id)
	{
		bool ok = false;
		Item item = DataHolder.Item(id);
		if(item.useable && item.useInField)
		{
			if(item.TargetAlly() && this.combatant is Character)
			{
				Combatant[] target = null;
				if(item.TargetGroup()) target = GameHandler.Party().GetBattleParty();
				else target = new Combatant[] {this.combatant};
				item.Use(this.combatant, target, null, id, 1, 1);
				ok = true;
			}
			else if((item.TargetEnemy() && this.combatant is Enemy) ||
				(item.TargetSelf() && this.combatant == GameHandler.Party().GetPlayerCharacter()))
			{
				item.Use(GameHandler.Party().GetPlayerCharacter(), new Combatant[] {this.combatant}, null, id, 1, 1);
				ok = true;
			}
		}
		return ok;
	}
	
	private bool UseSkill(int id, int userID, int ul)
	{
		bool ok = false;
		Combatant user = GameHandler.Party().GetCharacter(userID);
		Skill skill = DataHolder.Skill(id);
		if(user != null && skill.useInField)
		{
			if(skill.TargetAlly() && this.combatant is Character)
			{
				Combatant[] target = null;
				if(skill.TargetGroup()) target = GameHandler.Party().GetBattleParty();
				else target = new Combatant[] {this.combatant};
				skill.Use(user, target, null, true, ul, 1, 1);
				ok = true;
			}
			else if((skill.TargetEnemy() && this.combatant is Enemy) ||
				(skill.TargetSelf() && this.combatant == user))
			{
				skill.Use(user, new Combatant[] {this.combatant}, null, true, ul, 1, 1);
				ok = true;
			}
		}
		return ok;
	}
	
	private bool Equip(int id, DragType dt)
	{
		bool ok = false;
		if(this.combatant is Character)
		{
			EquipSet set = EquipSet.NONE;
			if(DragType.WEAPON.Equals(dt)) set = EquipSet.WEAPON;
			else if(DragType.ARMOR.Equals(dt)) set = EquipSet.ARMOR;
			((Character)combatant).Equip(-1, id, set);
			ok = true;
		}
		return ok;
	}
	
	// battle
	private bool BattleItem(int id, Combatant c)
	{
		bool ok = false;
		Item item = DataHolder.Item(id);
		if(item.useable && item.useInBattle)
		{
			if(item.TargetAlly() && this.combatant is Character)
			{
				if(item.TargetSingle())
				{
					c.AddAction(new BattleAction(
							AttackSelection.ITEM, c, this.combatant.battleID, id, 0));
				}
				else if(item.TargetGroup())
				{
					c.AddAction(new BattleAction(
							AttackSelection.ITEM, c, BattleAction.ALL_CHARACTERS, id, 0));
				}
				ok = true;
			}
			else if(item.TargetEnemy() && this.combatant is Enemy)
			{
				if(item.TargetSingle())
				{
					c.AddAction(new BattleAction(
							AttackSelection.ITEM, c, this.combatant.battleID, id, 0));
				}
				else if(item.TargetGroup())
				{
					c.AddAction(new BattleAction(
							AttackSelection.ITEM, c, BattleAction.ALL_ENEMIES, id, 0));
				}
				ok = true;
			}
			else if(item.TargetSelf() && this.combatant == c)
			{
				c.AddAction(new BattleAction(
						AttackSelection.ITEM, c, c.battleID, id, 0));
				ok = true;
			}
		}
		return ok;
	}
	
	private bool BattleSkill(int id, Combatant c, int ul)
	{
		bool ok = false;
		Skill skill = DataHolder.Skill(id);
		if(skill.useInBattle)
		{
			if(skill.TargetAlly() && this.combatant is Character)
			{
				if(skill.TargetSingle())
				{
					c.AddAction(new BattleAction(
							AttackSelection.SKILL, c, this.combatant.battleID, id, ul));
				}
				else if(skill.TargetGroup())
				{
					c.AddAction(new BattleAction(
							AttackSelection.SKILL, c, BattleAction.ALL_CHARACTERS, id, ul));
				}
				ok = true;
			}
			else if(skill.TargetEnemy() && this.combatant is Enemy)
			{
				if(skill.TargetSingle())
				{
					c.AddAction(new BattleAction(
							AttackSelection.SKILL, c, this.combatant.battleID, id, ul));
				}
				else if(skill.TargetGroup())
				{
					c.AddAction(new BattleAction(
							AttackSelection.SKILL, c, BattleAction.ALL_ENEMIES, id, ul));
				}
				ok = true;
			}
			else if(skill.TargetSelf() && this.combatant == c)
			{
				c.AddAction(new BattleAction(
						AttackSelection.SKILL, c, c.battleID, id, ul));
				ok = true;
			}
		}
		return ok;
	}
	
	private bool BattleAttack(Combatant c)
	{
		bool ok = false;
		if(this.combatant is Enemy)
		{
			c.AddAction(new BattleAction(
					AttackSelection.ATTACK, c, this.combatant.battleID, -1, 0));
			ok = true;
		}
		return ok;
	}
}
