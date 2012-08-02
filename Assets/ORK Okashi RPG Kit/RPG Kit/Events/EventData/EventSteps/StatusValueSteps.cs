
using System.Collections;

public class RegenerateStep : EventStep
{
	public RegenerateStep(GameEventType t) : base(t)
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
				for(int j=0; j<cs[i].status.Length; j++)
				{
					if(cs[i].status[j].IsConsumable())
					{
						cs[i].status[j].SetValue(cs[i].status[cs[i].status[j].maxStatus].GetValue(), true, false, true);
					}
				}
			}
		}
		else
		{
			Character c = GameHandler.Party().GetCharacter(this.characterID);
			if(c != null)
			{
				for(int i=0; i<c.status.Length; i++)
				{
					if(c.status[i].IsConsumable())
					{
						c.status[i].SetValue(c.status[c.status[i].maxStatus].GetValue(), true, false, true);
					}
				}
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
		return ht;
	}
}

public class CheckStatusValueStep : EventStep
{
	public CheckStatusValueStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		bool ok = true;
		Character[] cs = new Character[0];
		if(this.show)
		{
			if(this.show2) cs = GameHandler.Party().GetBattleParty();
			else cs = GameHandler.Party().GetParty();
		}
		else
		{
			cs = ArrayHelper.Add(GameHandler.Party().GetCharacter(this.characterID), cs);
		}
		for(int i=0; i<cs.Length; i++)
		{
			if(cs[i] != null)
			{
				int val = cs[i].status[this.itemID].GetValue();
				if((ValueCheck.EQUALS.Equals(this.valueCheck) && val != this.number) ||
					(ValueCheck.LESS.Equals(this.valueCheck) && val >= this.number) ||
					(ValueCheck.GREATER.Equals(this.valueCheck) && val <= this.number))
				{
					ok = false;
					break;
				}
			}
			else ok = false;
		}
		if(ok) gameEvent.StepFinished(this.next);
		else gameEvent.StepFinished(this.nextFail);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("nextfail", this.nextFail.ToString());
		ht.Add("character", this.characterID.ToString());
		ht.Add("show", this.show.ToString());
		ht.Add("show2", this.show2.ToString());
		ht.Add("item", this.itemID.ToString());
		ht.Add("valuecheck", this.valueCheck.ToString());
		ht.Add("number", this.number.ToString());
		return ht;
	}
}

public class SetStatusValueStep : EventStep
{
	public SetStatusValueStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		Character[] cs = new Character[0];
		if(this.show)
		{
			if(this.show2) cs = GameHandler.Party().GetBattleParty();
			else cs = GameHandler.Party().GetParty();
		}
		else
		{
			cs = ArrayHelper.Add(GameHandler.Party().GetCharacter(this.characterID), cs);
		}
		for(int i=0; i<cs.Length; i++)
		{
			if(cs[i] != null)
			{
				if(SimpleOperator.ADD.Equals(this.simpleOperator))
				{
					if(cs[i].status[this.itemID].IsNormal()) cs[i].status[this.itemID].AddBaseValue(this.number);
					cs[i].status[this.itemID].AddValue(this.number, false, false, true);
				}
				else if(SimpleOperator.SUB.Equals(this.simpleOperator))
				{
					if(cs[i].status[this.itemID].IsNormal()) cs[i].status[this.itemID].AddBaseValue(-this.number);
					cs[i].status[this.itemID].AddValue(-this.number, false, false, true);
				}
				else if(SimpleOperator.SET.Equals(this.simpleOperator))
				{
					if(cs[i].status[this.itemID].IsNormal()) cs[i].status[this.itemID].SetBaseValue(this.number);
					cs[i].status[this.itemID].SetValue(this.number, false, false, true);
				}
				for(int j=0; j<cs[i].status.Length; j++) cs[i].status[j].CheckBounds();
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
		ht.Add("item", this.itemID.ToString());
		ht.Add("simpleoperator", this.simpleOperator.ToString());
		ht.Add("number", this.number.ToString());
		return ht;
	}
}