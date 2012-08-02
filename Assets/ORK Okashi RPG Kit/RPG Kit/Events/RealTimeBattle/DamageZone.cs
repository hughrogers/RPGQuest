
using UnityEngine;
using System.Collections;

[AddComponentMenu("RPG Kit/Real Time Battles/Damage Zone")]
public class DamageZone : DamageBase
{
	public float damageFactor = 1.0f;
	
	public int Damage(BattleAction action)
	{
		int id = this.GetBattleID();
		if(this.combatant != null && 
			(!this.combatant.isDead || action.reviveFlag))
		{
			CombatantAnimation[] anims = action.Calculate(
					new Combatant[] {this.combatant}, this.damageFactor);
			if(anims.Length > 0)
			{
				string name = this.combatant.GetAnimationName(anims[0]);
				if(name != "" && this.combatant.prefabInstance != null && 
					this.combatant.prefabInstance.animation != null && 
					this.combatant.prefabInstance.animation[name] && 
					(DataHolder.BattleSystem().playDamageAnim || 
					!this.combatant.IsInAction() ||
					!(DataHolder.BattleSystem().dynamicCombat || 
					DataHolder.BattleSystem().IsRealTime())))
				{
					this.combatant.prefabInstance.animation.CrossFade(name, 0.3f, PlayMode.StopSameLayer);
				}
			}
		}
		return id;
	}
}
