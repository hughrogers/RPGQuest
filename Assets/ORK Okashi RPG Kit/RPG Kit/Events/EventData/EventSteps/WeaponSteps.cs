
using System.Collections;

public class AddWeaponStep : EventStep
{
	public AddWeaponStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		GameHandler.AddWeapon(this.weaponID, this.number);
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("weapon", this.weaponID.ToString());
		ht.Add("number", this.number.ToString());
		return ht;
	}
}

public class RemoveWeaponStep : EventStep
{
	public RemoveWeaponStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		GameHandler.RemoveWeapon(this.weaponID, this.number);
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("weapon", this.weaponID.ToString());
		ht.Add("number", this.number.ToString());
		return ht;
	}
}

public class CheckWeaponStep : EventStep
{
	public CheckWeaponStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		if(GameHandler.HasWeapon(this.weaponID, this.number)) gameEvent.StepFinished(this.next);
		else gameEvent.StepFinished(this.nextFail);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("weapon", this.weaponID.ToString());
		ht.Add("number", this.number.ToString());
		ht.Add("nextfail", this.nextFail.ToString());
		return ht;
	}
}