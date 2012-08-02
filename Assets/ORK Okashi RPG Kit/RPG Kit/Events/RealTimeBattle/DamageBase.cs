
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class DamageBase : MonoBehaviour
{
	// ingame
	protected Combatant combatant = null;
	
	void Start()
	{
		CombatantClick cc = (CombatantClick)this.transform.root.GetComponent(typeof(CombatantClick));
		if(cc == null)
		{
			cc = (CombatantClick)this.transform.root.GetComponentInChildren(typeof(CombatantClick));
		}
		if(cc != null)
		{
			this.combatant = cc.combatant;
		}
	}
	
	public int GetBattleID()
	{
		int id = -1;
		if(this.combatant != null) id = this.combatant.battleID;
		return id;
	}
	
	public Combatant GetCombatant()
	{
		return this.combatant;
	}
}
