
using UnityEngine;
using System.Collections;

public class Language
{
	public string[] text = new string[0];
	
	public Language()
	{
		
	}
	
	public Language(string add)
	{
		this.Add(add);
	}
	
	public Language(string[] add)
	{
		this.text = new string[add.Length];
		System.Array.Copy(add, this.text, add.Length);
	}
	
	public Language(int size)
	{
		this.text = new string[size];
		for(int i=0; i<size; i++)  this.text[i] = "";
	}
	
	/*
	============================================================================
	Utility functions
	============================================================================
	*/
	public void Add(string add)
	{
		this.text = ArrayHelper.Add(add, this.text);
	}
	
	public void Remove(int index)
	{
		this.text = ArrayHelper.Remove(index, this.text);
	}
	
	public void Copy(int index)
	{
		if(index >= 0 && index < this.text.Length)
		{
			this.Add(this.text[index]);
		}
	}
	
	public int Count()
	{
		return this.text.Length;
	}
}
