
using System.Collections;
using UnityEngine;

public class SetVariableAStep : AnimationStep
{
	public SetVariableAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		GameHandler.SetVariable(this.key, this.value);
		battleAnimation.StepFinished(this.next);
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

public class RemoveVariableAStep : AnimationStep
{
	public RemoveVariableAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		GameHandler.RemoveVariable(this.key);
		battleAnimation.StepFinished(this.next);
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

public class CheckVariableAStep : AnimationStep
{
	public CheckVariableAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		if(GameHandler.CheckVariable(this.key, this.value)) battleAnimation.StepFinished(this.next);
		else battleAnimation.StepFinished(this.nextFail);
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

public class SetNumberVariableAStep : AnimationStep
{
	public SetNumberVariableAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
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
		battleAnimation.StepFinished(this.next);
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

public class RemoveNumberVariableAStep : AnimationStep
{
	public RemoveNumberVariableAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		GameHandler.RemoveNumberVariable(this.key);
		battleAnimation.StepFinished(this.next);
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

public class CheckNumberVariableAStep : AnimationStep
{
	public CheckNumberVariableAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		if(GameHandler.CheckNumberVariable(this.key, this.float1, this.valueCheck)) battleAnimation.StepFinished(this.next);
		else battleAnimation.StepFinished(this.nextFail);
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

public class SetPlayerPrefsAStep : AnimationStep
{
	public SetPlayerPrefsAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		if(this.show)
		{
			PlayerPrefs.SetFloat(this.value, GameHandler.GetNumberVariable(this.key));
		}
		else
		{
			PlayerPrefs.SetString(this.value, GameHandler.GetVariable(this.key));
		}
		battleAnimation.StepFinished(this.next);
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

public class GetPlayerPrefsAStep : AnimationStep
{
	public GetPlayerPrefsAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		if(this.show)
		{
			GameHandler.SetNumberVariable(this.key, PlayerPrefs.GetFloat(this.value));
		}
		else
		{
			GameHandler.SetVariable(this.key, PlayerPrefs.GetString(this.value));
		}
		battleAnimation.StepFinished(this.next);
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

public class HasPlayerPrefsAStep : AnimationStep
{
	public HasPlayerPrefsAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		if(PlayerPrefs.HasKey(this.key)) battleAnimation.StepFinished(this.next);
		else battleAnimation.StepFinished(this.nextFail);
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