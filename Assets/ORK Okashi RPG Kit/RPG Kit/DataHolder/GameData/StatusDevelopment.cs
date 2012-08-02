
using UnityEngine;
using System.Collections;

public class StatusDevelopment
{
	public int[] levelValue = new int[0];
	
	public StatusDevelopment()
	{
		
	}
	
	public StatusDevelopment(int[] add)
	{
		this.levelValue = new int[add.Length];
		System.Array.Copy(add, this.levelValue, add.Length);
	}
	
	public StatusDevelopment(int size)
	{
		this.levelValue = new int[size];
	}
	
	/*
	============================================================================
	Utility functions
	============================================================================
	*/
	public void Add(int add)
	{
		this.levelValue = ArrayHelper.Add(add, this.levelValue);
	}
	
	public void Remove(int index)
	{
		this.levelValue = ArrayHelper.Remove(index, this.levelValue);
	}
	
	public void Copy(int index)
	{
		if(index >= 0 && index < this.levelValue.Length)
		{
			this.Add(this.levelValue[index]);
		}
	}
	
	public int Count()
	{
		return this.levelValue.Length;
	}
}
