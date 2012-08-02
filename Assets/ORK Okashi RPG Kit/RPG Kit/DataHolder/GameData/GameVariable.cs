
using UnityEngine;
using System.Collections;

public class GameVariable
{
	// game variable
	public bool[] remove = new bool[0];
	public string[] variableKey  = new string[0];
	public string[] variableValue = new string[0];
	// number variables
	public bool[] removeNumber = new bool[0];
	public SimpleOperator[] changeType = new SimpleOperator[0];
	public string[] numberVarKey = new string[0];
	public float[] numberVarValue = new float[0];
	
	private static string VARIABLEKEY = "variablekey";
	private static string VARIABLEVALUE = "variablevalue";
	private static string NUMBERVARIABLE = "numbervariable";
	
	public GameVariable()
	{
		
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		ArrayList s = new ArrayList();
		ht.Add("variables", this.variableKey.Length.ToString());
		for(int j=0; j<this.variableKey.Length; j++)
		{
			Hashtable c = new Hashtable();
			c.Add(XMLHandler.NODE_NAME, GameVariable.VARIABLEKEY);
			c.Add("id", j.ToString());
			c.Add(XMLHandler.CONTENT, this.variableKey[j]);
			if(this.remove[j]) c.Add("remove", "true");
			s.Add(c);
			
			if(!this.remove[j])
			{
				c = new Hashtable();
				c.Add(XMLHandler.NODE_NAME, GameVariable.VARIABLEVALUE);
				c.Add("id", j.ToString());
				c.Add(XMLHandler.CONTENT, this.variableValue[j]);
				s.Add(c);
			}
		}
		
		ht.Add("numbervariables", this.numberVarKey.Length.ToString());
		for(int j=0; j<this.numberVarKey.Length; j++)
		{
			Hashtable c = new Hashtable();
			c.Add(XMLHandler.NODE_NAME, GameVariable.NUMBERVARIABLE);
			c.Add("id", j.ToString());
			c.Add(XMLHandler.CONTENT, this.numberVarKey[j]);
			c.Add("value", this.numberVarValue[j].ToString());
			c.Add("type", this.changeType[j].ToString());
			if(this.removeNumber[j]) c.Add("remove", "true");
			s.Add(c);
		}
		if(s.Count > 0) ht.Add(XMLHandler.NODES, s);
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		if(ht.ContainsKey("variables"))
		{
			int count = int.Parse((string)ht["variables"]);
			this.remove = new bool[count];
			this.variableKey = new string[count];
			this.variableValue = new string[count];
		}
		if(ht.ContainsKey("numbervariables"))
		{
			int count = int.Parse((string)ht["numbervariables"]);
			this.removeNumber = new bool[count];
			this.changeType = new SimpleOperator[count];
			this.numberVarKey = new string[count];
			this.numberVarValue = new float[count];
		}
		
		if(ht.ContainsKey(XMLHandler.NODES))
		{
			ArrayList s = ht[XMLHandler.NODES] as ArrayList;
			foreach(Hashtable ht2 in s)
			{
				if(ht2[XMLHandler.NODE_NAME] as string == GameVariable.VARIABLEKEY)
				{
					int j = int.Parse((string)ht2["id"]);
					if(j < this.variableKey.Length)
					{
						this.variableKey[j] = ht2[XMLHandler.CONTENT] as string;
						if(ht2.ContainsKey("remove"))
						{
							this.remove[j] = true;
							this.variableValue[j] = "";
						}
					}
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == GameVariable.VARIABLEVALUE)
				{
					int j = int.Parse((string)ht2["id"]);
					if(j < this.variableValue.Length)
					{
						this.variableValue[j] = ht2[XMLHandler.CONTENT] as string;
					}
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == GameVariable.NUMBERVARIABLE)
				{
					int j = int.Parse((string)ht2["id"]);
					if(j < this.numberVarKey.Length)
					{
						this.numberVarKey[j] = ht2[XMLHandler.CONTENT] as string;
						this.numberVarValue[j] = float.Parse((string)ht2["value"]);
						this.changeType[j] = (SimpleOperator)System.Enum.Parse(
								typeof(SimpleOperator), (string)ht2["type"]);
						if(ht2.ContainsKey("remove"))
						{
							this.removeNumber[j] = true;
						}
					}
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
		this.remove = ArrayHelper.Add(false, this.remove);
		this.variableKey = ArrayHelper.Add("key", this.variableKey);
		this.variableValue = ArrayHelper.Add("value", this.variableValue);
	}
	
	public void RemoveVariable(int index)
	{
		this.remove = ArrayHelper.Remove(index, this.remove);
		this.variableKey = ArrayHelper.Remove(index, this.variableKey);
		this.variableValue = ArrayHelper.Remove(index, this.variableValue);
	}
	
	public void AddNumberVariable()
	{
		this.removeNumber = ArrayHelper.Add(false, this.removeNumber);
		this.changeType = ArrayHelper.Add(SimpleOperator.ADD, this.changeType);
		this.numberVarKey = ArrayHelper.Add("key", this.numberVarKey);
		this.numberVarValue = ArrayHelper.Add(0, this.numberVarValue);
	}
	
	public void RemoveNumberVariable(int index)
	{
		this.removeNumber = ArrayHelper.Remove(index, this.removeNumber);
		this.changeType = ArrayHelper.Remove(index, this.changeType);
		this.numberVarKey = ArrayHelper.Remove(index, this.numberVarKey);
		this.numberVarValue = ArrayHelper.Remove(index, this.numberVarValue);
	}
	
	/*
	============================================================================
	Set functions
	============================================================================
	*/
	public void SetVariables()
	{
		// vars
		for(int i=0; i<this.variableKey.Length; i++)
		{
			if(this.variableKey[i] != null && this.variableKey[i] != "")
			{
				if(this.remove[i])
				{
					GameHandler.RemoveVariable(this.variableKey[i]);
				}
				else if(this.variableValue[i] != null)
				{
					GameHandler.SetVariable(this.variableKey[i], this.variableValue[i]);
				}
			}
		}
		// number vars
		for(int i=0; i<this.numberVarKey.Length; i++)
		{
			if(this.numberVarKey[i] != null && this.numberVarKey[i] != "")
			{
				if(this.removeNumber[i])
				{
					GameHandler.RemoveNumberVariable(this.numberVarKey[i]);
				}
				else if(SimpleOperator.ADD.Equals(this.changeType[i]))
				{
					GameHandler.SetNumberVariable(this.numberVarKey[i], 
							GameHandler.GetNumberVariable(this.numberVarKey[i])+this.numberVarValue[i]);
				}
				else if(SimpleOperator.SUB.Equals(this.changeType[i]))
				{
					GameHandler.SetNumberVariable(this.numberVarKey[i], 
							GameHandler.GetNumberVariable(this.numberVarKey[i])-this.numberVarValue[i]);
				}
				else if(SimpleOperator.SET.Equals(this.changeType[i]))
				{
					GameHandler.SetNumberVariable(this.numberVarKey[i], this.numberVarValue[i]);
				}
			}
		}
	}
}
