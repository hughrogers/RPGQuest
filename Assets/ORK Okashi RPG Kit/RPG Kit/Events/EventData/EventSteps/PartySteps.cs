
using System.Collections;

public class JoinPartyStep : EventStep
{
	public JoinPartyStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		GameHandler.Party().JoinParty(this.characterID);
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("character", this.characterID.ToString());
		return ht;
	}
}

public class LeavePartyStep : EventStep
{
	public LeavePartyStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		GameHandler.Party().LeaveParty(this.characterID);
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("character", this.characterID.ToString());
		return ht;
	}
}

public class IsInPartyStep : EventStep
{
	public IsInPartyStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		if(GameHandler.Party().HasJoinedParty(this.characterID)) gameEvent.StepFinished(this.next);
		else gameEvent.StepFinished(this.nextFail);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("character", this.characterID.ToString());
		ht.Add("nextfail", this.nextFail.ToString());
		return ht;
	}
}

public class HasLeftPartyStep : EventStep
{
	public HasLeftPartyStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		if(GameHandler.Party().HasLeftParty(this.characterID)) gameEvent.StepFinished(this.next);
		else gameEvent.StepFinished(this.nextFail);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("character", this.characterID.ToString());
		ht.Add("nextfail", this.nextFail.ToString());
		return ht;
	}
}

public class CheckPlayerStep : EventStep
{
	public CheckPlayerStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		Character c = GameHandler.Party().GetPlayerCharacter();
		if(c != null && c.realID == this.characterID) gameEvent.StepFinished(this.next);
		else gameEvent.StepFinished(this.nextFail);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("character", this.characterID.ToString());
		ht.Add("nextfail", this.nextFail.ToString());
		return ht;
	}
}

public class SetPlayerStep : EventStep
{
	public SetPlayerStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		GameHandler.Party().SetPlayer(this.characterID);
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("character", this.characterID.ToString());
		return ht;
	}
}