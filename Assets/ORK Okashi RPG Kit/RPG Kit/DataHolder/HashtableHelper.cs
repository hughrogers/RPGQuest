
using UnityEngine;
using System.Collections;

public class HashtableHelper
{
	/*
	============================================================================
	Title functions
	============================================================================
	*/
	public static Hashtable GetTitleHashtable(string title)
	{
		Hashtable ht = new Hashtable();
		ht.Add(XMLHandler.NODE_NAME, title);
		return ht;
	}
	
	public static Hashtable GetTitleHashtable(string title, int id)
	{
		Hashtable ht = HashtableHelper.GetTitleHashtable(title);
		ht.Add("id", id.ToString());
		return ht;
	}
	
	/*
	============================================================================
	Content functions
	============================================================================
	*/
	public static Hashtable GetContentHashtable(string title, string content)
	{
		Hashtable ht = HashtableHelper.GetTitleHashtable(title);
		ht.Add(XMLHandler.CONTENT, content);
		return ht;
	}
	
	public static Hashtable GetContentHashtable(string title, string content, int id)
	{
		Hashtable ht = HashtableHelper.GetTitleHashtable(title, id);
		ht.Add(XMLHandler.CONTENT, content);
		return ht;
	}
	
	public static void AddContentHashtables(ref ArrayList s, string title, string[] data)
	{
		for(int i=0; i<data.Length; i++)
		{
			if(data[i] != "") s.Add(HashtableHelper.GetContentHashtable(title, data[i], i));
		}
	}
	
	public static void GetContentString(Hashtable ht, ref string[] data)
	{
		HashtableHelper.GetContentString(ht, ref data, "id");
	}
	
	public static void GetContentString(Hashtable ht, ref string[] data, string idStr)
	{
		int id = int.Parse((string)ht[idStr]);
		if(id < data.Length)
		{
			data[id] = ht[XMLHandler.CONTENT] as string;
		}
	}
}
