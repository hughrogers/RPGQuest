
using UnityEngine;

public class Item : Useable
{
	public string prefabName = "";
	public int itemType = 0;
	
	public int buyPrice = 0;
	public bool sellable = false;
	public int sellPrice = 50;
	public ValueSetter sellSetter = ValueSetter.PERCENT;
	
	public bool dropable = false;
	public bool useable = false;
	public bool consume = true;
	public bool stealable = false;
	
	public string audioName = "";
	
	public bool revive = false;
	public SkillEffect[] skillEffect;
	
	// animation
	public bool battleAnimation = false;
	public int animationID = 0;
	
	public ItemSkillType itemSkill = ItemSkillType.NONE;
	public int skillID = 0;
	public int skillLevel = 1;
	
	public ValueChange[] valueChange;
	
	public ItemVariableType itemVariable = ItemVariableType.NONE;
	public string variableKey = "";
	public string variableValue = "";
	
	public bool learnRecipe = false;
	public int recipeID = 0;
	
	public UseRange useRange = new UseRange();
	
	// global event
	public bool callGlobalEvent = false;
	public int globalEventID = 0;
	
	public Item()
	{
		
	}
	
	/*
	============================================================================
	Utility functions
	============================================================================
	*/
	public void ApplyEffects(Combatant user, Combatant target)
	{
		for(int i=0; i<this.skillEffect.Length; i++)
		{
			if(SkillEffect.ADD.Equals(this.skillEffect[i]))
			{
				target.AddEffect(i, user);
			}
			else if(SkillEffect.REMOVE.Equals(this.skillEffect[i]))
			{
				target.RemoveEffect(i);
			}
		}
	}
	
	public GameObject GetPrefabInstance()
	{
		GameObject prefab = null;
		if("" != this.prefabName)
		{
			GameObject tmp = (GameObject)Resources.Load(DataHolder.Items().GetPrefabPath()+this.prefabName, typeof(GameObject));
			if(tmp) prefab = (GameObject)GameObject.Instantiate(tmp);
		}
		return prefab;
	}
	
	public int GetSellPrice()
	{
		int p = this.sellPrice;
		if(ValueSetter.PERCENT.Equals(this.sellSetter))
		{
			p = (this.buyPrice*this.sellPrice)/100;
		}
		return p;
	}
	
	/*
	============================================================================
	Use functions
	============================================================================
	*/
	public CombatantAnimation[] Use(Combatant user, Combatant[] target, BattleAction ba, int id, float damageFactor, float damageMultiplier)
	{
		AudioClip clip = null;
		if(this.audioName != "")
		{
			clip = (AudioClip)Resources.Load(ItemData.AUDIO_PATH+this.audioName, typeof(AudioClip));
		}
		
		CombatantAnimation[] anims = new CombatantAnimation[target.Length];
		// global event
		if(this.callGlobalEvent)
		{
			GameHandler.GetLevelHandler().CallGlobalEvent(this.globalEventID);
		}
		// use item
		else
		{
			for(int j=0; j<target.Length; j++)
			{
				if(clip != null && target[j].prefabInstance != null)
				{
					AudioSource s = target[j].prefabInstance.GetComponentInChildren<AudioSource>();
					if(s == null) s = target[j].prefabInstance.AddComponent<AudioSource>();
					if(s != null)
					{
						s.PlayOneShot(clip);
					}
				}
				
				anims[j] = CombatantAnimation.NONE;
				for(int i=0; i<this.valueChange.Length; i++)
				{
					if(!target[j].isDead || (target[j].isDead && this.revive && !target[j].noRevive))
					{
						if(this.revive && target[j].isDead && !target[j].noRevive)
						{
							target[j].isDead = false;
							anims[j] = CombatantAnimation.REVIVE;
						}
						if(this.valueChange[i].active)
						{
							int oldVal = target[j].status[i].GetValue();
							int change = this.valueChange[i].ChangeValue(i, -1, user, target[j], true, damageFactor, damageMultiplier);
							if(!(this.revive && target[j].isDead && !target[j].noRevive) && ((this.valueChange[i].IsSub() && change > 0) ||
								(this.valueChange[i].IsAdd() && change < 0) ||
								(this.valueChange[i].IsSet() && change < oldVal)))
							{
								anims[j] = CombatantAnimation.DAMAGE;
							}
						}
					}
				}
			}
			if(ItemSkillType.USE.Equals(this.itemSkill))
			{
				anims = DataHolder.Skill(this.skillID).Use(user, target, ba, false, this.skillLevel-1, damageFactor, damageMultiplier);
			}
			else if(ItemSkillType.LEARN.Equals(this.itemSkill))
			{
				for(int j=0; j<target.Length; j++)
				{
					if(target[j] is Character)
					{
						Character t = (Character)target[j];
						t.LearnSkill(this.skillID, this.skillLevel);
					}
				}
			}
			if(ItemVariableType.SET.Equals(this.itemVariable))
			{
				GameHandler.SetVariable(this.variableKey, this.variableValue);
			}
			else if(ItemVariableType.REMOVE.Equals(this.itemVariable))
			{
				GameHandler.RemoveVariable(this.variableKey);
			}
			for(int j=0; j<target.Length; j++)
			{
				this.ApplyEffects(user, target[j]);
			}
		}
		// consume
		if(user is Character)
		{
			DataHolder.Statistic.ItemUsed(id);
			if(this.learnRecipe) GameHandler.LearnRecipe(this.recipeID);
			if(this.consume) GameHandler.RemoveItem(id);
		}
		return anims;
	}
}