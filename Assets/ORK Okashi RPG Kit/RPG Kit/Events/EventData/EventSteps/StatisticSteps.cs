
using UnityEngine;
using System.Collections;

public class CustomStatisticStep : EventStep
{
	public CustomStatisticStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		DataHolder.Statistic.CustomChanged(this.number, this.itemID);
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("number", this.number.ToString());
		ht.Add("item", this.itemID.ToString());
		return ht;
	}
}

public class ClearStatisticStep : EventStep
{
	public ClearStatisticStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		DataHolder.Statistic.Clear();
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("number", this.number.ToString());
		ht.Add("item", this.itemID.ToString());
		return ht;
	}
}
