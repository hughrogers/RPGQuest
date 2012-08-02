
using System.Collections;
using UnityEngine;

public class CheckClassStep : EventStep
{
	public CheckClassStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		Character c = GameHandler.Party().GetCharacter(this.characterID);
		if(c != null && c.currentClass == this.number) gameEvent.StepFinished(this.next);
		else gameEvent.StepFinished(this.nextFail);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("nextfail", this.nextFail.ToString());
		ht.Add("character", this.characterID.ToString());
		ht.Add("number", this.number.ToString());
		return ht;
	}
}


public class ChangeClassStep : EventStep
{
	public ChangeClassStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		Character c = GameHandler.Party().GetCharacter(this.characterID);
		if(c != null)
		{
			c.ChangeClass(this.number, this.show, this.show2, this.show3, this.show4, this.show5);
		}
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("character", this.characterID.ToString());
		ht.Add("number", this.number.ToString());
		ht.Add("show", this.show.ToString());
		ht.Add("show2", this.show2.ToString());
		ht.Add("show3", this.show3.ToString());
		ht.Add("show4", this.show4.ToString());
		ht.Add("show5", this.show5.ToString());
		return ht;
	}
}
