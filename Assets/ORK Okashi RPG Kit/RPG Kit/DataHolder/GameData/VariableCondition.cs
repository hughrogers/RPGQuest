
using UnityEngine;
using System.Collections;

public class VariableCondition
{
	// game variable
	public AIConditionNeeded needed = AIConditionNeeded.ALL;
	public string[] variableKey  = new string[0];
	public string[] variableValue = new string[0];
	public bool[] checkType = new bool[0];
	// number variables
	public string[] numberVarKey = new string[0];
	public float[] numberVarValue = new float[0];
	public bool[] numberCheckType = new bool[0];
	public ValueCheck[] numberValueCheck = new ValueCheck[0];
	
	private static string VARIABLEKEY = "variablekey";
	private static string VARIABLEVALUE = "variablevalue";
	private static string NUMBERVARIABLE = "numbervariable";
	
	public VariableCondition()
	{
		
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(string title)
	{
		ArrayList s = new ArrayList();
		Hashtable ht = HashtableHelper.GetTitleHashtable(title);
		ht.Add("needed", this.needed.ToString());
		ht.Add("variables", this.variableKey.Length.ToString());
		for(int j=0; j<this.variableKey.Length; j++)
		{
			Hashtable c = new Hashtable();
			c.Add(XMLHandler.NODE_NAME, VariableCondition.VARIABLEKEY);
			c.Add("id", j.ToString());
			c.Add(XMLHandler.CONTENT, this.variableKey[j]);
			c.Add("type", this.checkType[j].ToString());
			s.Add(c);
			c = new Hashtable();
			c.Add(XMLHandler.NODE_NAME, VariableCondition.VARIABLEVALUE);
			c.Add("id", j.ToString());
			c.Add(XMLHandler.CONTENT, this.variableValue[j]);
			s.Add(c);
		}
		
		ht.Add("numbervariables", this.numberVarKey.Length.ToString());
		for(int j=0; j<this.numberVarKey.Length; j++)
		{
			Hashtable c = new Hashtable();
			c.Add(XMLHandler.NODE_NAME, VariableCondition.NUMBERVARIABLE);
			c.Add("id", j.ToString());
			c.Add(XMLHandler.CONTENT, this.numberVarKey[j]);
			c.Add("value", this.numberVarValue[j].ToString());
			c.Add("type", this.numberCheckType[j].ToString());
			c.Add("check", this.numberValueCheck[j].ToString());
			s.Add(c);
		}
		if(s.Count > 0) ht.Add(XMLHandler.NODES, s);
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		this.needed = (AIConditionNeeded)System.Enum.Parse(typeof(AIConditionNeeded), (string)ht["needed"]);
		if(ht.ContainsKey("variables"))
		{
			int count = int.Parse((string)ht["variables"]);
			this.variableKey = new string[count];
			this.variableValue = new string[count];
			this.checkType = new bool[count];
		}
		if(ht.ContainsKey("numbervariables"))
		{
			int count = int.Parse((string)ht["numbervariables"]);
			this.numberVarKey = new string[count];
			this.numberVarValue = new float[count];
			this.numberCheckType = new bool[count];
			this.numberValueCheck = new ValueCheck[count];
		}
		
		if(ht.ContainsKey(XMLHandler.NODES))
		{
			ArrayList s = ht[XMLHandler.NODES] as ArrayList;
			foreach(Hashtable ht2 in s)
			{
				if(ht2[XMLHandler.NODE_NAME] as string == VariableCondition.VARIABLEKEY)
				{
					int j = int.Parse((string)ht2["id"]);
					this.variableKey[j] = ht2[XMLHandler.CONTENT] as string;
					this.checkType[j] = bool.Parse((string)ht2["type"]);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == VariableCondition.VARIABLEVALUE)
				{
					int j = int.Parse((string)ht2["id"]);
					this.variableValue[j] = ht2[XMLHandler.CONTENT] as string;
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == VariableCondition.NUMBERVARIABLE)
				{
					int j = int.Parse((string)ht2["id"]);
					this.numberVarKey[j] = ht2[XMLHandler.CONTENT] as string;
					this.numberVarValue[j] = float.Parse((string)ht2["value"]);
					this.numberCheckType[j] = bool.Parse((string)ht2["type"]);
					this.numberValueCheck[j] = (ValueCheck)System.Enum.Parse(
							typeof(ValueCheck), (string)ht2["check"]);
				}
			}
		}
	}
	
	/*
	============================================================================
	Add/remove functions
	============================================================================
	*/
	public void AddVariable()
	{
		this.variableKey = ArrayHelper.Add("key", this.variableKey);
		this.variableValue = ArrayHelper.Add("value", this.variableValue);
		this.checkType = ArrayHelper.Add(true, this.checkType);
	}
	
	public void RemoveVariable(int index)
	{
		this.variableKey = ArrayHelper.Remove(index, this.variableKey);
		this.variableValue = ArrayHelper.Remove(index, this.variableValue);
		this.checkType = ArrayHelper.Remove(index, this.checkType);
	}
	
	public void AddNumberVariable()
	{
		this.numberVarKey = ArrayHelper.Add("key", this.numberVarKey);
		this.numberVarValue = ArrayHelper.Add(0, this.numberVarValue);
		this.numberCheckType = ArrayHelper.Add(true, this.numberCheckType);
		this.numberValueCheck = ArrayHelper.Add(ValueCheck.EQUALS, this.numberValueCheck);
	}
	
	public void RemoveNumberVariable(int index)
	{
		this.numberVarKey = ArrayHelper.Remove(index, this.numberVarKey);
		this.numberVarValue = ArrayHelper.Remove(index, this.numberVarValue);
		this.numberCheckType = ArrayHelper.Remove(index, this.numberCheckType);
		this.numberValueCheck = ArrayHelper.Remove(index, this.numberValueCheck);
	}
	
	/*
	============================================================================
	Check functions
	============================================================================
	*/
	public bool CheckVariables()
	{
		bool apply = true;
		bool any = false;
		for(int i=0; i<this.variableKey.Length; i++)
		{
			bool check = GameHandler.CheckVariable(this.variableKey[i], this.variableValue[i]);
			
			if((check && this.checkType[i]) || (!check && !this.checkType[i]))
			{
				any = true;
			}
			else if(AIConditionNeeded.ALL.Equals(this.needed))
			{
				apply = false;
				break;
			}
		}
		if(apply)
		{
			for(int i=0; i<this.numberVarKey.Length; i++)
			{
				bool check = GameHandler.CheckNumberVariable(this.numberVarKey[i], 
						this.numberVarValue[i], this.numberValueCheck[i]);
				
				if((check && this.numberCheckType[i]) || (!check && !this.numberCheckType[i]))
				{
					any = true;
				}
				else if(AIConditionNeeded.ALL.Equals(this.needed))
				{
					apply = false;
					break;
				}
			}
		}
		if(AIConditionNeeded.ONE.Equals(this.needed) && !any && 
			(this.variableKey.Length > 0 || this.numberVarKey.Length > 0))
		{
			apply = false;
		}
		return apply;
	}
}
