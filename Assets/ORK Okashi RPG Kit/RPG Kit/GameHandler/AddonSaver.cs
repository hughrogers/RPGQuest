
using UnityEngine;
using System.Collections;

public abstract class AddonSaver
{
	public abstract Hashtable GetData();
	
	public abstract void SetData(Hashtable ht);
}
