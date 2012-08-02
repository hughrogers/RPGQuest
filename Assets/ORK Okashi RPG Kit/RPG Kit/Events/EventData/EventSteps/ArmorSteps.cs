
using System.Collections;
using UnityEngine;

public class AddArmorStep : EventStep
{
	public AddArmorStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		GameHandler.AddArmor(this.armorID, this.number);
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("armor", this.armorID.ToString());
		ht.Add("number", this.number.ToString());
		return ht;
	}
}

public class RemoveArmorStep : EventStep
{
	public RemoveArmorStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		GameHandler.RemoveArmor(this.armorID, this.number);
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("armor", this.armorID.ToString());
		ht.Add("number", this.number.ToString());
		return ht;
	}
}

public class CheckArmorStep : EventStep
{
	public CheckArmorStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		if(GameHandler.HasArmor(this.armorID, this.number)) gameEvent.StepFinished(this.next);
		else gameEvent.StepFinished(this.nextFail);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("armor", this.armorID.ToString());
		ht.Add("number", this.number.ToString());
		ht.Add("nextfail", this.nextFail.ToString());
		return ht;
	}
}