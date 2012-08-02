
using UnityEngine;
using System.Collections;

public class BoolHelper
{
	/*
	============================================================================
	Hashtable functions
	============================================================================
	*/
	public static void FromHashtable(Hashtable ht, string key, ref bool data)
	{
		if(ht.ContainsKey(key))
		{
			data = bool.Parse((string)ht[key]);
		}
	}
}
