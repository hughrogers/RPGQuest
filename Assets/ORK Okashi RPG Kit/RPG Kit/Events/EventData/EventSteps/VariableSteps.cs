
using System.Collections;
using UnityEngine;

public class SetVariableStep : EventStep
{
	public SetVariableStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		GameHandler.SetVariable(this.key, this.value);
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		
		ArrayList subs = new ArrayList();
		Hashtable s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "value");
		s.Add(XMLHandler.CONTENT, this.value);
		subs.Add(s);
		
		s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "key");
		s.Add(XMLHandler.CONTENT, this.key);
		subs.Add(s);
		
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}

public class RemoveVariableStep : EventStep
{
	public RemoveVariableStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		GameHandler.RemoveVariable(this.key);
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		
		ArrayList subs = new ArrayList();
		Hashtable s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "key");
		s.Add(XMLHandler.CONTENT, this.key);
		subs.Add(s);
		
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}

public class CheckVariableStep : EventStep
{
	public CheckVariableStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		if(GameHandler.CheckVariable(this.key, this.value)) gameEvent.StepFinished(this.next);
		else gameEvent.StepFinished(this.nextFail);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("nextfail", this.nextFail.ToString());
		
		ArrayList subs = new ArrayList();
		Hashtable s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "value");
		s.Add(XMLHandler.CONTENT, this.value);
		subs.Add(s);
		
		s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "key");
		s.Add(XMLHandler.CONTENT, this.key);
		subs.Add(s);
		
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}

public class SetNumberVariableStep : EventStep
{
	public SetNumberVariableStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		if(SimpleOperator.ADD.Equals(this.simpleOperator))
		{
			GameHandler.SetNumberVariable(this.key, 
					GameHandler.GetNumberVariable(this.key)+this.float1);
		}
		else if(SimpleOperator.SUB.Equals(this.simpleOperator))
		{
			GameHandler.SetNumberVariable(this.key, 
					GameHandler.GetNumberVariable(this.key)-this.float1);
		}
		else if(SimpleOperator.SET.Equals(this.simpleOperator))
		{
			GameHandler.SetNumberVariable(this.key, this.float1);
		}
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		
		ht.Add("float1", this.float1);
		ht.Add("simpleoperator", this.simpleOperator);
		
		ArrayList subs = new ArrayList();
		Hashtable s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "key");
		s.Add(XMLHandler.CONTENT, this.key);
		subs.Add(s);
		
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}

public class RemoveNumberVariableStep : EventStep
{
	public RemoveNumberVariableStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		GameHandler.RemoveNumberVariable(this.key);
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		
		ArrayList subs = new ArrayList();
		Hashtable s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "key");
		s.Add(XMLHandler.CONTENT, this.key);
		subs.Add(s);
		
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}

public class CheckNumberVariableStep : EventStep
{
	public CheckNumberVariableStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		if(GameHandler.CheckNumberVariable(this.key, this.float1, this.valueCheck)) gameEvent.StepFinished(this.next);
		else gameEvent.StepFinished(this.nextFail);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("nextfail", this.nextFail.ToString());
		ht.Add("valuecheck", this.valueCheck.ToString());
		ht.Add("float1", this.float1.ToString());
		
		ArrayList subs = new ArrayList();
		Hashtable s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "key");
		s.Add(XMLHandler.CONTENT, this.key);
		subs.Add(s);
		
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}

public class SetPlayerPrefsStep : EventStep
{
	public SetPlayerPrefsStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		if(this.show)
		{
			PlayerPrefs.SetFloat(this.value, GameHandler.GetNumberVariable(this.key));
		}
		else
		{
			PlayerPrefs.SetString(this.value, GameHandler.GetVariable(this.key));
		}
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		
		ht.Add("show", this.show.ToString());
		
		ArrayList subs = new ArrayList();
		Hashtable s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "value");
		s.Add(XMLHandler.CONTENT, this.value);
		subs.Add(s);
		
		s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "key");
		s.Add(XMLHandler.CONTENT, this.key);
		subs.Add(s);
		
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}

public class GetPlayerPrefsStep : EventStep
{
	public GetPlayerPrefsStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		if(this.show)
		{
			GameHandler.SetNumberVariable(this.key, PlayerPrefs.GetFloat(this.value));
		}
		else
		{
			GameHandler.SetVariable(this.key, PlayerPrefs.GetString(this.value));
		}
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		
		ht.Add("show", this.show.ToString());
		
		ArrayList subs = new ArrayList();
		Hashtable s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "value");
		s.Add(XMLHandler.CONTENT, this.value);
		subs.Add(s);
		
		s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "key");
		s.Add(XMLHandler.CONTENT, this.key);
		subs.Add(s);
		
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}

public class HasPlayerPrefsStep : EventStep
{
	public HasPlayerPrefsStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		if(PlayerPrefs.HasKey(this.key)) gameEvent.StepFinished(this.next);
		else gameEvent.StepFinished(this.nextFail);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("nextfail", this.nextFail.ToString());
		
		ArrayList subs = new ArrayList();
		Hashtable s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "key");
		s.Add(XMLHandler.CONTENT, this.key);
		subs.Add(s);
		
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}