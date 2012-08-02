
using System.Collections;

public class LearnSkillStep : EventStep
{
	public LearnSkillStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		Character c = GameHandler.Party().GetCharacter(this.characterID);
		if(c != null)
		{
			c.LearnSkill(this.skillID, 1);
		}
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("character", this.characterID.ToString());
		ht.Add("skill", this.skillID.ToString());
		return ht;
	}
}

public class ForgetSkillStep : EventStep
{
	public ForgetSkillStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		Character c = GameHandler.Party().GetCharacter(this.characterID);
		if(c != null)
		{
			c.ForgetSkill(this.skillID);
		}
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("character", this.characterID.ToString());
		ht.Add("skill", this.skillID.ToString());
		return ht;
	}
}

public class HasSkillStep : EventStep
{
	public HasSkillStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		bool has = false;
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
				if((this.show3 && cs[i].HasLearnedSkill(this.skillID, 0)) ||
					(!this.show3 && cs[i].HasSkill(this.skillID, 0)))
				{
					has = true;
					break;
				}
			}
		}
		else
		{
			Character c = GameHandler.Party().GetCharacter(this.characterID);
			if(c != null)
			{
				if(this.show3) has = c.HasLearnedSkill(this.skillID, 0);
				else has = c.HasSkill(this.skillID, 0);
			}
		}
		if(has) gameEvent.StepFinished(this.next);
		else gameEvent.StepFinished(this.nextFail);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("character", this.characterID.ToString());
		ht.Add("skill", this.skillID.ToString());
		ht.Add("show", this.show.ToString());
		ht.Add("show2", this.show2.ToString());
		ht.Add("show3", this.show3.ToString());
		ht.Add("nextfail", this.nextFail.ToString());
		return ht;
	}
}