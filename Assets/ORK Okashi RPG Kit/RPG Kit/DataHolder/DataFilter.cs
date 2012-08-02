
using UnityEngine;
using System.Collections;

public class DataFilter
{
	public bool[] useFilter;
	public int[] filterID;
	
	public string[] nameList = new string[0];
	public int[] realID = new int[0];
	
	public DataFilter(int size)
	{
		this.useFilter = new bool[size];
		this.filterID = new int[size];
	}
	
	public int GetFakeID(int id)
	{
		int result = 0;
		for(int i=0; i<realID.Length; i++)
		{
			if(realID[i] == id)
			{
				result = i;
				break;
			}
		}
		return result;
	}
	
	public void Deactivate()
	{
		for(int i=0; i<this.useFilter.Length; i++)
		{
			this.useFilter[i] = false;
		}
	}
}
