
using UnityEngine;
using System.Collections;

public class BattleAnimationSettings
{
	// battle  clip names
	public AnimationData idle = new AnimationData(-2);
	public AnimationData walk = new AnimationData(0);
	public AnimationData run = new AnimationData(0);
	public AnimationData sprint = new AnimationData(0);
	public AnimationData jump = new AnimationData(1);
	public AnimationData fall = new AnimationData(1);
	public AnimationData land = new AnimationData(0);
	public AnimationData attack = new AnimationData(2);
	public AnimationData defend = new AnimationData(2);
	public AnimationData item = new AnimationData(2);
	public AnimationData skill = new AnimationData(2);
	public AnimationData damage = new AnimationData(3);
	public AnimationData evade = new AnimationData(3);
	public AnimationData death = new AnimationData(4);
	public AnimationData revive = new AnimationData(4);
	// character only
	public AnimationData victory = new AnimationData(4);
	public AnimationData idleAfter = new AnimationData(-2);
	
	// battle animations
	public bool animateBaseAttack = false;
	public int baseAttackAnimationID = 0;
	
	public bool animateDefend = false;
	public int defendAnimationID = 0;
	
	public bool animateEscape = false;
	public int escapeAnimationID = 0;
	
	public bool animateDeath = false;
	public int deathAnimationID = 0;
	
	private static string IDLE = "idle";
	private static string WALK = "walk";
	private static string RUN = "run";
	private static string SPRINT = "sprint";
	private static string JUMP = "jump";
	private static string FALL = "fall";
	private static string LAND = "land";
	private static string ATTACK = "attack";
	private static string DEFEND = "defend";
	private static string ITEM = "item";
	private static string SKILL = "skill";
	private static string DAMAGE = "damage";
	private static string EVADE = "evade";
	private static string DEATH = "death";
	private static string REVIVE = "revive";
	private static string VICTORY = "victory";
	private static string IDLEAFTER = "idleafter";
	
	public BattleAnimationSettings()
	{
		
	}
	
	public BattleAnimationSettings(Hashtable ht)
	{
		this.SetData(ht);
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		if(this.animateBaseAttack) ht.Add("attackid", this.baseAttackAnimationID.ToString());
		if(this.animateDefend) ht.Add("defendid", this.defendAnimationID.ToString());
		if(this.animateEscape) ht.Add("escapeid", this.escapeAnimationID.ToString());
		if(this.animateDeath) ht.Add("deathid", this.deathAnimationID.ToString());
		
		ArrayList s = new ArrayList();
		if(this.idle.name != "") s.Add(this.idle.GetData(HashtableHelper.GetTitleHashtable(BattleAnimationSettings.IDLE)));
		if(this.walk.name != "") s.Add(this.walk.GetData(HashtableHelper.GetTitleHashtable(BattleAnimationSettings.WALK)));
		if(this.run.name != "") s.Add(this.run.GetData(HashtableHelper.GetTitleHashtable(BattleAnimationSettings.RUN)));
		if(this.sprint.name != "") s.Add(this.sprint.GetData(HashtableHelper.GetTitleHashtable(BattleAnimationSettings.SPRINT)));
		if(this.jump.name != "") s.Add(this.jump.GetData(HashtableHelper.GetTitleHashtable(BattleAnimationSettings.JUMP)));
		if(this.fall.name != "") s.Add(this.fall.GetData(HashtableHelper.GetTitleHashtable(BattleAnimationSettings.FALL)));
		if(this.land.name != "") s.Add(this.land.GetData(HashtableHelper.GetTitleHashtable(BattleAnimationSettings.LAND)));
		if(this.attack.name != "") s.Add(this.attack.GetData(HashtableHelper.GetTitleHashtable(BattleAnimationSettings.ATTACK)));
		if(this.defend.name != "") s.Add(this.defend.GetData(HashtableHelper.GetTitleHashtable(BattleAnimationSettings.DEFEND)));
		if(this.item.name != "") s.Add(this.item.GetData(HashtableHelper.GetTitleHashtable(BattleAnimationSettings.ITEM)));
		if(this.skill.name != "") s.Add(this.skill.GetData(HashtableHelper.GetTitleHashtable(BattleAnimationSettings.SKILL)));
		if(this.damage.name != "") s.Add(this.damage.GetData(HashtableHelper.GetTitleHashtable(BattleAnimationSettings.DAMAGE)));
		if(this.evade.name != "") s.Add(this.evade.GetData(HashtableHelper.GetTitleHashtable(BattleAnimationSettings.EVADE)));
		if(this.death.name != "") s.Add(this.death.GetData(HashtableHelper.GetTitleHashtable(BattleAnimationSettings.DEATH)));
		if(this.revive.name != "") s.Add(this.revive.GetData(HashtableHelper.GetTitleHashtable(BattleAnimationSettings.REVIVE)));
		if(this.victory.name != "") s.Add(this.victory.GetData(HashtableHelper.GetTitleHashtable(BattleAnimationSettings.VICTORY)));
		if(this.idleAfter.name != "") s.Add(this.idleAfter.GetData(HashtableHelper.GetTitleHashtable(BattleAnimationSettings.IDLEAFTER)));
		
		if(s.Count > 0) ht.Add(XMLHandler.NODES, s);
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		if(ht.ContainsKey("attackid"))
		{
			this.animateBaseAttack = true;
			this.baseAttackAnimationID = int.Parse((string)ht["attackid"]);
		}
		if(ht.ContainsKey("defendid"))
		{
			this.animateDefend = true;
			this.defendAnimationID = int.Parse((string)ht["defendid"]);
		}
		if(ht.ContainsKey("escapeid"))
		{
			this.animateEscape = true;
			this.escapeAnimationID = int.Parse((string)ht["escapeid"]);
		}
		if(ht.ContainsKey("deathid"))
		{
			this.animateDeath = true;
			this.deathAnimationID = int.Parse((string)ht["deathid"]);
		}
		
		if(ht.ContainsKey(XMLHandler.NODES))
		{
			ArrayList s = ht[XMLHandler.NODES] as ArrayList;
			foreach(Hashtable ht2 in s)
			{
				if(ht2[XMLHandler.NODE_NAME] as string == BattleAnimationSettings.IDLE)
				{
					this.idle.SetData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == BattleAnimationSettings.WALK)
				{
					this.walk.SetData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == BattleAnimationSettings.RUN)
				{
					this.run.SetData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == BattleAnimationSettings.SPRINT)
				{
					this.sprint.SetData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == BattleAnimationSettings.JUMP)
				{
					this.jump.SetData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == BattleAnimationSettings.FALL)
				{
					this.fall.SetData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == BattleAnimationSettings.LAND)
				{
					this.land.SetData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == BattleAnimationSettings.ATTACK)
				{
					this.attack.SetData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == BattleAnimationSettings.DEFEND)
				{
					this.defend.SetData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == BattleAnimationSettings.ITEM)
				{
					this.item.SetData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == BattleAnimationSettings.SKILL)
				{
					this.skill.SetData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == BattleAnimationSettings.DAMAGE)
				{
					this.damage.SetData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == BattleAnimationSettings.EVADE)
				{
					this.evade.SetData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == BattleAnimationSettings.DEATH)
				{
					this.death.SetData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == BattleAnimationSettings.REVIVE)
				{
					this.revive.SetData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == BattleAnimationSettings.VICTORY)
				{
					this.victory.SetData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == BattleAnimationSettings.IDLEAFTER)
				{
					this.idleAfter.SetData(ht2);
				}
			}
		}
		else
		{
			if(ht.ContainsKey("idle")) this.idle.name = ht["idle"] as string;
			if(ht.ContainsKey("run")) this.run.name = ht["run"] as string;
			if(ht.ContainsKey("attack")) this.attack.name = ht["attack"] as string;
			if(ht.ContainsKey("defend")) this.defend.name = ht["defend"] as string;
			if(ht.ContainsKey("item")) this.item.name = ht["item"] as string;
			if(ht.ContainsKey("skill")) this.skill.name = ht["skill"] as string;
			if(ht.ContainsKey("damage")) this.damage.name = ht["damage"] as string;
			if(ht.ContainsKey("evade")) this.evade.name = ht["evade"] as string;
			if(ht.ContainsKey("death")) this.death.name = ht["death"] as string;
			if(ht.ContainsKey("revive")) this.revive.name = ht["revive"] as string;
			if(ht.ContainsKey("victory")) this.victory.name = ht["victory"] as string;
			if(ht.ContainsKey("idleafter")) this.idleAfter.name = ht["idleafter"] as string;
		}
	}
	
	/*
	============================================================================
	Animation handling functions
	============================================================================
	*/
	public void SetAnimationLayers(Combatant c)
	{
		Animation a = c.GetAnimationComponent();
		if(a != null)
		{
			this.idle.Init(a, c);
			this.walk.Init(a, c);
			this.run.Init(a, c);
			this.sprint.Init(a, c);
			this.jump.Init(a, c);
			this.fall.Init(a, c);
			this.land.Init(a, c);
			this.attack.Init(a, c);
			this.defend.Init(a, c);
			this.item.Init(a, c);
			this.skill.Init(a, c);
			this.damage.Init(a, c);
			this.evade.Init(a, c);
			this.death.Init(a, c);
			this.revive.Init(a, c);
			this.victory.Init(a, c);
			this.idleAfter.Init(a, c);
		}
	}
	
	public string GetAnimationName(CombatantAnimation type)
	{
		string name = "";
		if(CombatantAnimation.IDLE.Equals(type)) name = this.idle.name;
		else if(CombatantAnimation.WALK.Equals(type)) name = this.walk.name;
		else if(CombatantAnimation.RUN.Equals(type)) name = this.run.name;
		else if(CombatantAnimation.SPRINT.Equals(type)) name = this.sprint.name;
		else if(CombatantAnimation.JUMP.Equals(type)) name = this.jump.name;
		else if(CombatantAnimation.FALL.Equals(type)) name = this.fall.name;
		else if(CombatantAnimation.LAND.Equals(type)) name = this.land.name;
		else if(CombatantAnimation.ATTACK.Equals(type)) name = this.attack.name;
		else if(CombatantAnimation.DEFEND.Equals(type)) name = this.defend.name;
		else if(CombatantAnimation.ITEM.Equals(type)) name = this.item.name;
		else if(CombatantAnimation.SKILL.Equals(type)) name = this.skill.name;
		else if(CombatantAnimation.DAMAGE.Equals(type)) name = this.damage.name;
		else if(CombatantAnimation.EVADE.Equals(type)) name = this.evade.name;
		else if(CombatantAnimation.DEATH.Equals(type)) name = this.death.name;
		else if(CombatantAnimation.REVIVE.Equals(type)) name = this.revive.name;
		return name;
	}
	
	public int GetBaseAttackAnimation()
	{
		int id = -1;
		if(this.animateBaseAttack) id = this.baseAttackAnimationID;
		return id;
	}
	
	public int GetDefendAnimation()
	{
		int id = -1;
		if(this.animateDefend) id = this.defendAnimationID;
		return id;
	}
	
	public int GetEscapeAnimation()
	{
		int id = -1;
		if(this.animateEscape) id = this.escapeAnimationID;
		return id;
	}
	
	public int GetDeathAnimation()
	{
		int id = -1;
		if(this.animateDeath) id = this.deathAnimationID;
		return id;
	}
}
