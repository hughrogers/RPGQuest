
using System.Collections;

public class EquipWeaponStep : EventStep
{
	public EquipWeaponStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		Character c = GameHandler.Party().GetCharacter(this.characterID);
		if(c != null)
		{
			c.Equip(this.number, this.weaponID, EquipSet.WEAPON);
		}
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("character", this.characterID.ToString());
		ht.Add("weapon", this.weaponID.ToString());
		ht.Add("number", this.number.ToString());
		return ht;
	}
}

public class EquipArmorStep : EventStep
{
	public EquipArmorStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		Character c = GameHandler.Party().GetCharacter(this.characterID);
		if(c != null)
		{
			c.Equip(this.number, this.armorID, EquipSet.ARMOR);
		}
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("character", this.characterID.ToString());
		ht.Add("armor", this.armorID.ToString());
		ht.Add("number", this.number.ToString());
		return ht;
	}
}

public class UnequipStep : EventStep
{
	public UnequipStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		Character c = GameHandler.Party().GetCharacter(this.characterID);
		if(c != null)
		{
			if(this.show) c.UnequipAll();
			else c.Unequip(this.number);
		}
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("character", this.characterID.ToString());
		ht.Add("number", this.number.ToString());
		ht.Add("show", this.show.ToString());
		return ht;
	}
}