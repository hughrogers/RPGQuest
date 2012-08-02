
using UnityEngine;
using System.Collections;

public class UseRange
{
	public bool active = false;
	public float range = 5.0f;
	public bool ignoreYDistance = false;
	
	public UseRange()
	{
		
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		if(this.active)
		{
			ht.Add("userange", this.range.ToString());
			if(this.ignoreYDistance) ht.Add("ignoreydistance", "true");
		}
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		if(ht.ContainsKey("userange"))
		{
			this.active = true;
			this.range = float.Parse((string)ht["userange"]);
			if(ht.ContainsKey("ignoreydistance")) this.ignoreYDistance = true;
		}
	}
	
	public bool CompareTo(UseRange ur)
	{
		bool same = false;
		if(this.active == ur.active && 
			this.range == ur.range && 
			this.ignoreYDistance == ur.ignoreYDistance)
		{
			same = true;
		}
		return same;
	}
	
	/*
	============================================================================
	Utility functions
	============================================================================
	*/
	public bool InRange(Combatant user, Combatant target)
	{
		bool ok = true;
		if(DataHolder.BattleSystem().IsRealTime() &&
			user != null && target != null && 
			user.prefabInstance != null && 
			target.prefabInstance != null)
		{
			Vector3 v1 = user.prefabInstance.transform.position;
			if(this.ignoreYDistance) v1.y = target.prefabInstance.transform.position.y;
			ok = this.InRange(Vector3.Distance(v1, 
					target.prefabInstance.transform.position));
		}
		return ok;
	}
	
	public bool InRange(float distance)
	{
		bool ok = true;
		if(DataHolder.BattleSystem().IsRealTime())
		{
			if(distance > DataHolder.BattleSystem().battleRange ||
				(this.active && distance > this.range))
			{
				ok = false;
			}
		}
		return ok;
	}
	
	public static bool InPlayerRange(GameObject obj, float distance)
	{
		bool ok = false;
		GameObject player = GameHandler.Party().GetPlayer();
		if(player != null && obj != null && 
			Vector3.Distance(player.transform.position, obj.transform.position) < distance)
		{
			ok = true;
		}
		return ok;
	}
}
