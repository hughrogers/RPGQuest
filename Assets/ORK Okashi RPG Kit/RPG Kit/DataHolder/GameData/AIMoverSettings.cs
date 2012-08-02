
using UnityEngine;
using System.Collections;

public class AIMoverSettings
{
	public bool useAIMover = false;
	// speed settings
	public bool useCombatantSpeed = false;
	public float runSpeed = 8.0f;
	public float gravity = Physics.gravity.y;
	public float speedSmoothing = 10.0f;
	
	public bool ignoreYDistance = false;
	
	// party follow settings
	public float minFollowDistance = 3.0f;
	public bool giveWay = false;
	public float giveWayDistance = 1.5f;
	public bool autoRespawn = false;
	public float respawnDistance = 100.0f;
	
	// enemy random patrol
	public bool randomPatrol = false;
	public float patrolRadius = 20.0f;
	public float waypointTime = 5.0f;
	
	public float changeTimeout = 1.0f;
	
	public float playerDistance = 120.0f;
	
	// ingame
	private bool added = false;
	
	public AIMoverSettings()
	{
		
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		if(this.useAIMover)
		{
			ht.Add("usecombatantspeed", this.useCombatantSpeed.ToString());
			ht.Add("runspeed", this.runSpeed.ToString());
			ht.Add("gravity", this.gravity.ToString());
			ht.Add("speedsmoothing", this.speedSmoothing.ToString());
			if(this.ignoreYDistance) ht.Add("ignoreydistance", "true");
			ht.Add("minfollowdistance", this.minFollowDistance.ToString());
			ht.Add("changetimeout", this.changeTimeout.ToString());
			ht.Add("playerdistance", this.playerDistance.ToString());
			if(this.giveWay) ht.Add("giveway", this.giveWayDistance.ToString());
			if(this.autoRespawn) ht.Add("respawn", this.respawnDistance.ToString());
			if(this.randomPatrol)
			{
				ht.Add("patrolradius", this.patrolRadius.ToString());
				ht.Add("waypointtime", this.waypointTime.ToString());
			}
		}
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		if(ht.ContainsKey("usecombatantspeed"))
		{
			this.useAIMover = true;
			this.useCombatantSpeed = bool.Parse((string)ht["usecombatantspeed"]);
			this.runSpeed = float.Parse((string)ht["runspeed"]);
			this.gravity = float.Parse((string)ht["gravity"]);
			this.speedSmoothing = float.Parse((string)ht["speedsmoothing"]);
			if(ht.ContainsKey("ignoreydistance")) this.ignoreYDistance = true;
			this.minFollowDistance = float.Parse((string)ht["minfollowdistance"]);
			this.changeTimeout = float.Parse((string)ht["changetimeout"]);
			this.playerDistance = float.Parse((string)ht["playerdistance"]);
			if(ht.ContainsKey("giveway"))
			{
				this.giveWay = true;
				this.giveWayDistance = float.Parse((string)ht["giveway"]);
			}
			if(ht.ContainsKey("respawn"))
			{
				this.autoRespawn = true;
				this.respawnDistance = float.Parse((string)ht["respawn"]);
			}
			if(ht.ContainsKey("patrolradius"))
			{
				this.randomPatrol = true;
				this.patrolRadius = float.Parse((string)ht["patrolradius"]);
				this.waypointTime = float.Parse((string)ht["waypointtime"]);
			}
		}
		else this.useAIMover = false;
	}
	
	/*
	============================================================================
	Ingame functions
	============================================================================
	*/
	public AIMover AddAIMover(GameObject combatant)
	{
		AIMover comp = null;
		if(this.useAIMover)
		{
			comp = combatant.GetComponent<AIMover>();
			if(comp == null)
			{
				comp = combatant.AddComponent<AIMover>();
				this.added = true;
				comp.useCombatantSpeed = this.useCombatantSpeed;
				comp.runSpeed = this.runSpeed;
				comp.gravity = this.gravity;
				comp.speedSmoothing = this.speedSmoothing;
				comp.ignoreYDistance = this.ignoreYDistance;
				comp.minFollowDistance = this.minFollowDistance;
				comp.changeTimeout = this.changeTimeout;
				comp.giveWay = this.giveWay;
				comp.giveWayDistance = this.giveWayDistance;
				comp.autoRespawn = this.autoRespawn;
				comp.respawnDistance = this.respawnDistance;
				comp.playerDistance = this.playerDistance;
				comp.randomPatrol = this.randomPatrol;
				comp.patrolRadius = this.patrolRadius;
				comp.waypointTime = this.waypointTime;
			}
		}
		return comp;
	}
	
	public void RemoveAIMover(GameObject combatant)
	{
		if(this.added)
		{
			AIMover comp = combatant.GetComponent<AIMover>();
			if(comp != null)
			{
				GameObject.Destroy(comp);
				this.added = false;
			}
		}
	}
}
