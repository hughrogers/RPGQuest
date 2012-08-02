
using System.Collections;

public class AddItemStep : EventStep
{
	public AddItemStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		GameHandler.AddItem(this.itemID, this.number);
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("item", this.itemID.ToString());
		ht.Add("number", this.number.ToString());
		return ht;
	}
}

public class RemoveItemStep : EventStep
{
	public RemoveItemStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		GameHandler.RemoveItem(this.itemID, this.number);
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("item", this.itemID.ToString());
		ht.Add("number", this.number.ToString());
		return ht;
	}
}

public class CheckItemStep : EventStep
{
	public CheckItemStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		if(GameHandler.HasItem(this.itemID, this.number)) gameEvent.StepFinished(this.next);
		else gameEvent.StepFinished(this.nextFail);
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

public class LearnItemRecipeStep : EventStep
{
	public LearnItemRecipeStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		GameHandler.LearnRecipe(this.itemID);
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("item", this.itemID.ToString());
		return ht;
	}
}

public class ItemRecipeKnownStep : EventStep
{
	public ItemRecipeKnownStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		if(GameHandler.RecipeKnown(this.itemID)) gameEvent.StepFinished(this.next);
		else gameEvent.StepFinished(this.nextFail);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("item", this.itemID.ToString());
		ht.Add("nextfail", this.nextFail.ToString());
		return ht;
	}
}