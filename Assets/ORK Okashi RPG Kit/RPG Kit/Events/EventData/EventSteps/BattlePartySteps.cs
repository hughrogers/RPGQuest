
using System.Collections;

public class JoinBattlePartyStep : EventStep
{
	public JoinBattlePartyStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		GameHandler.Party().JoinBattleParty(this.characterID);
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("character", this.characterID.ToString());
		return ht;
	}
}

public class LeaveBattlePartyStep : EventStep
{
	public LeaveBattlePartyStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		GameHandler.Party().LeaveBattleParty(this.characterID);
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("character", this.characterID.ToString());
		return ht;
	}
}

public class IsInBattlePartyStep : EventStep
{
	public IsInBattlePartyStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		if(GameHandler.Party().HasJoinedBattleParty(this.characterID)) gameEvent.StepFinished(this.next);
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

public class LockBattlePartyMemberStep : EventStep
{
	public LockBattlePartyMemberStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		GameHandler.Party().LockBattlePartyMember(this.characterID);
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("character", this.characterID.ToString());
		return ht;
	}
}

public class UnlockBattlePartyMemberStep : EventStep
{
	public UnlockBattlePartyMemberStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		GameHandler.Party().UnlockBattlePartyMember(this.characterID);
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("character", this.characterID.ToString());
		return ht;
	}
}

public class IsLockedBattlePartyMemberStep : EventStep
{
	public IsLockedBattlePartyMemberStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		if(GameHandler.Party().IsLockedBattlePartyMember(this.characterID)) gameEvent.StepFinished(this.next);
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

public class SetCharacterNameStep : EventStep
{
	public SetCharacterNameStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		Character c = GameHandler.Party().GetCharacter(this.characterID);
		if(c != null)
		{
			if(this.show)
			{
				string n = GameHandler.GetVariable(this.key);
				c.SetName(n);
			}
			else
			{
				c.SetName(this.key);
			}
		}
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("character", this.characterID.ToString());
		ht.Add("show", this.show.ToString());
		
		ArrayList subs = new ArrayList();
		
		Hashtable s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "key");
		s.Add(XMLHandler.CONTENT, this.key);
		subs.Add(s);
		
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}