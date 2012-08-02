
using UnityEngine;
using System.Collections;

public class FloatHelper
{
	/*
	============================================================================
	Hashtable functions
	============================================================================
	*/
	public static void FromHashtable(Hashtable ht, string key, ref float data)
	{
		if(ht.ContainsKey(key))
		{
			data = float.Parse((string)ht[key]);
		}
	}
	
	public static void FromHashtable(Hashtable ht, string key, ref float data, ref bool check)
	{
		if(ht.ContainsKey(key))
		{
			check = true;
			data = float.Parse((string)ht[key]);
		}
	}
	
	public static void ToHashtable(ref Hashtable ht, string key, float data, bool check)
	{
		if(check)
		{
			ht.Add(key, data.ToString());
		}
	}
	
	/*
	============================================================================
	Limit functions
	============================================================================
	*/
	public static void Limit(ref float value, float min, float max)
	{
		if(value < min) value = min;
		else if(value > max) value = max;
	}
	
	public static void ChanceLimit(ref float value)
	{
		if(value < DataHolder.GameSettings().minRandomRange)
		{
			value = DataHolder.GameSettings().minRandomRange;
		}
		else if(value > DataHolder.GameSettings().maxRandomRange)
		{
			value = DataHolder.GameSettings().maxRandomRange;
		}
	}
}
