
using System.Collections;

public class AddItemAStep : AnimationStep
{
	public AddItemAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		if(battleAnimation.battleAction.user is Character) GameHandler.AddItem(this.itemID, this.number);
		battleAnimation.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("item", this.itemID.ToString());
		ht.Add("number", this.number.ToString());
		return ht;
	}
}

public class RemoveItemAStep : AnimationStep
{
	public RemoveItemAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		if(battleAnimation.battleAction.user is Character) GameHandler.RemoveItem(this.itemID, this.number);
		battleAnimation.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("item", this.itemID.ToString());
		ht.Add("number", this.number.ToString());
		return ht;
	}
}

public class CheckItemAStep : AnimationStep
{
	public CheckItemAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		if(GameHandler.HasItem(this.itemID, this.number)) battleAnimation.StepFinished(this.next);
		else battleAnimation.StepFinished(this.nextFail);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("item", this.itemID.ToString());
		ht.Add("number", this.number.ToString());
		ht.Add("nextfail", this.nextFail.ToString());
		return ht;
	}
}