
using UnityEngine;
using System.Collections;

public class CustomStatisticAStep : AnimationStep
{
	public CustomStatisticAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		DataHolder.Statistic.CustomChanged(this.number, this.itemID);
		battleAnimation.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("number", this.number.ToString());
		ht.Add("item", this.itemID.ToString());
		return ht;
	}
}

public class ClearStatisticAStep : AnimationStep
{
	public ClearStatisticAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		DataHolder.Statistic.Clear();
		battleAnimation.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("number", this.number.ToString());
		ht.Add("item", this.itemID.ToString());
		return ht;
	}
}
