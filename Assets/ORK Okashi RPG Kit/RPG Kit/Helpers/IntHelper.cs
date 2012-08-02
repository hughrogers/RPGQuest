
using UnityEngine;
using System.Collections;

public class IntHelper
{
	/*
	============================================================================
	Hashtable functions
	============================================================================
	*/
	public static void FromHashtable(Hashtable ht, string key, ref int data)
	{
		if(ht.ContainsKey(key))
		{
			data = int.Parse((string)ht[key]);
		}
	}
	
	public static void FromHashtable(Hashtable ht, string key, ref int data, ref bool check)
	{
		if(ht.ContainsKey(key))
		{
			check = true;
			data = int.Parse((string)ht[key]);
		}
	}
	
	public static void ToHashtable(ref Hashtable ht, string key, int data, bool check)
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
	public static void Limit(ref int value, int min, int max)
	{
		if(value < min) value = min;
		else if(value > max) value = max;
	}
}
