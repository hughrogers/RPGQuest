
using System.Collections;

public class LevelUpStep : EventStep
{
	public LevelUpStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		if(this.show)
		{
			Character[] cs = null;
			if(this.show2)
			{
				cs = GameHandler.Party().GetBattleParty();
			}
			else
			{
				cs = GameHandler.Party().GetParty();
			}
			for(int i=0; i<cs.Length; i++)
			{
				cs[i].ForceLevelUp();
			}
		}
		else
		{
			Character c = GameHandler.Party().GetCharacter(this.characterID);
			if(c != null)
			{
				if(this.show3) c.ForceClassLevelUp();
				else c.ForceLevelUp();
			}
		}
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("character", this.characterID.ToString());
		ht.Add("show", this.show.ToString());
		ht.Add("show2", this.show2.ToString());
		ht.Add("show3", this.show3.ToString());
		return ht;
	}
}

public class CheckLevelStep : EventStep
{
	public CheckLevelStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		bool has = true;
		if(this.show)
		{
			Character[] cs = null;
			if(this.show2)
			{
				cs = GameHandler.Party().GetBattleParty();
			}
			else
			{
				cs = GameHandler.Party().GetParty();
			}
			for(int i=0; i<cs.Length; i++)
			{
				if(cs[i] == null || 
					(this.show3 && cs[i].currentClassLevel < this.number) ||
					(!this.show3 && cs[i].currentLevel < this.number))
				{
					has = false;
					break;
				}
			}
		}
		else
		{
			Character c = GameHandler.Party().GetCharacter(this.characterID);
			if(c == null || 
				(this.show3 && c.currentClassLevel < this.number) ||
				(!this.show3 && c.currentLevel < this.number))
			{
				has = false;
			}
		}
		if(has) gameEvent.StepFinished(this.next);
		else gameEvent.StepFinished(this.nextFail);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("character", this.characterID.ToString());
		ht.Add("show", this.show.ToString());
		ht.Add("show2", this.show2.ToString());
		ht.Add("show3", this.show3.ToString());
		ht.Add("number", this.number.ToString());
		ht.Add("nextfail", this.nextFail.ToString());
		return ht;
	}
}
