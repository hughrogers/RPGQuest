
using UnityEngine;
using System.Collections;

public class SkillLevel
{
	public int skillElement = 0;
	public bool revive = false;
	public bool counterable = false;
	public bool reflectable = false;
	public SkillEffect[] skillEffect;
	public int orderChange = 0;
	
	public string audioName = "";
	
	// animation
	public bool battleAnimation = false;
	public int animationID = 0;
	
	public bool hitChance = false;
	public int hitFormula = 0;
	// active time battle settings
	public bool endTurn = true;
	public float timebarUse = 1000;
	
	// passive skill
	public BonusSettings bonus = new BonusSettings();
	public int[] elementValue;
	public SimpleOperator[] elementOperator;
	public int[] raceValue;
	public int[] sizeValue;
	
	// user settings
	public ValueChange[] userConsume;
	
	// target settings
	public ValueChange[] targetConsume;
	
	public int[] skillCombo = new int[0];
	
	// skill reuse
	public StatusEffectEnd skillReuse = StatusEffectEnd.NONE;
	public float reuseTime = 0;
	
	public float castTime = 0;
	public bool cancelable = false;
	
	public UseRange useRange = new UseRange();
	
	public StealChance stealChance = new StealChance();
	
	public SkillLevel()
	{
		int svCount = DataHolder.StatusValueCount;
		int seCount = DataHolder.Effects().GetDataCount();
		int elCount = DataHolder.Elements().GetDataCount();
		this.userConsume = new ValueChange[svCount];
		this.targetConsume = new ValueChange[svCount];
		for(int i=0; i<svCount; i++)
		{
			this.userConsume[i] = new ValueChange();
			this.targetConsume[i] = new ValueChange();
		}
		this.skillEffect = new SkillEffect[seCount];
		this.elementValue = new int[elCount];
		this.elementOperator = new SimpleOperator[elCount];
		this.raceValue = new int[DataHolder.Races().GetDataCount()];
		this.sizeValue = new int[DataHolder.Sizes().GetDataCount()];
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht, Skill skill)
	{
		ArrayList s = new ArrayList();
		
		ht.Add("element", this.skillElement.ToString());
		if(this.revive) ht.Add("revive", "true");
		if(this.battleAnimation) ht.Add("animation", this.animationID.ToString());
		if(!this.endTurn)
		{
			ht.Add("timebaruse", this.timebarUse.ToString());
		}
		if(this.orderChange != 0)
		{
			ht.Add("orderchange", this.orderChange);
		}
		
		if(this.hitChance)
		{
			ht.Add("hitchance", this.hitFormula.ToString());
		}
		if(this.counterable) ht.Add("counterable", "true");
		if(this.reflectable) ht.Add("reflectable", "true");
		
		ht = this.useRange.GetData(ht);
		
		ht.Add("consumes", this.userConsume.Length.ToString());
		for(int j=0; j<this.userConsume.Length; j++)
		{
			if(this.userConsume[j].active)
			{
				s.Add(this.userConsume[j].GetData(HashtableHelper.GetTitleHashtable(SkillData.USERCONSUME, j)));
			}
			if(this.targetConsume[j].active)
			{
				s.Add(this.targetConsume[j].GetData(HashtableHelper.GetTitleHashtable(SkillData.TARGETCONSUME, j)));
			}
		}
		
		ht.Add("effects", this.skillEffect.Length.ToString());
		for(int j=0; j<this.skillEffect.Length; j++)
		{
			if(this.skillEffect[j] > 0)
			{
				Hashtable e = HashtableHelper.GetTitleHashtable(SkillData.EFFECT, j);
				e.Add("value", this.skillEffect[j].ToString());
				s.Add(e);
			}
		}
		
		ht.Add("skillcombos", this.skillCombo.Length.ToString());
		for(int j=0; j<this.skillCombo.Length; j++)
		{
			Hashtable e = HashtableHelper.GetTitleHashtable(SkillData.SKILLCOMBO, j);
			e.Add("skill", this.skillCombo[j].ToString());
			s.Add(e);
		}
		
		// passive skill
		if(skill.isPassive)
		{
			s.Add(this.bonus.GetData(HashtableHelper.GetTitleHashtable(SkillData.BONUSSETTINGS)));
			
			for(int j=0; j<this.elementValue.Length; j++)
			{
				if(this.elementValue[j] != 0 || SimpleOperator.SET.Equals(this.elementOperator[j]))
				{
					Hashtable e = HashtableHelper.GetTitleHashtable(SkillData.ELEMENT, j);
					e.Add("value", this.elementValue[j].ToString());
					e.Add("operator", this.elementOperator[j].ToString());
					s.Add(e);
				}
			}
			
			for(int j=0; j<this.raceValue.Length; j++)
			{
				if(this.raceValue[j] != 0)
				{
					Hashtable e = HashtableHelper.GetTitleHashtable(SkillData.RACE, j);
					e.Add("value", this.raceValue[j].ToString());
					s.Add(e);
				}
			}
			
			for(int j=0; j<this.sizeValue.Length; j++)
			{
				if(this.raceValue[j] != 0)
				{
					Hashtable e = HashtableHelper.GetTitleHashtable(SkillData.SIZE, j);
					e.Add("value", this.sizeValue[j].ToString());
					s.Add(e);
				}
			}
		}
		else
		{
			s.Add(this.stealChance.GetData(HashtableHelper.GetTitleHashtable(SkillData.STEALCHANCE)));
		}
		
		if(!StatusEffectEnd.NONE.Equals(this.skillReuse))
		{
			ht.Add("skillreuse", this.skillReuse.ToString());
			ht.Add("reusetime", this.reuseTime.ToString());
		}
		if(this.castTime > 0)
		{
			ht.Add("casttime", this.castTime.ToString());
			if(this.cancelable) ht.Add("cancelable", "true");
		}
		if(this.audioName != "") s.Add(HashtableHelper.GetContentHashtable(SkillData.AUDIOCLIP, this.audioName));
		
		if(s.Count > 0) ht.Add(XMLHandler.NODES, s);
		return ht;
	}
	
	public void SetData(Hashtable val)
	{
		this.elementValue = new int[DataHolder.Elements().GetDataCount()];
		this.elementOperator = new SimpleOperator[DataHolder.Elements().GetDataCount()];
		this.raceValue = new int[DataHolder.Races().GetDataCount()];
		this.sizeValue = new int[DataHolder.Sizes().GetDataCount()];
		
		this.skillElement = int.Parse((string)val["element"]);
		if(val.ContainsKey("revive")) this.revive = true;
		if(val.ContainsKey("animation"))
		{
			this.battleAnimation = true;
			this.animationID = int.Parse((string)val["animation"]);
		}
		if(val.ContainsKey("timebaruse"))
		{
			this.endTurn = false;
			this.timebarUse = float.Parse((string)val["timebaruse"]);
		}
		if(val.ContainsKey("orderchange"))
		{
			this.orderChange = int.Parse((string)val["orderchange"]);
		}
		this.useRange.SetData(val);
		
		int cl = int.Parse((string)val["consumes"]);
		this.userConsume = new ValueChange[cl];
		this.targetConsume = new ValueChange[cl];
		this.skillEffect = new SkillEffect[int.Parse((string)val["effects"])];
		for(int j=0; j<cl; j++)
		{
			this.userConsume[j] = new ValueChange();
			this.targetConsume[j] = new ValueChange();
		}
		
		if(val.ContainsKey("hitchance"))
		{
			this.hitChance = true;
			this.hitFormula = int.Parse((string)val["hitchance"]);
		}
		
		if(val.ContainsKey("counterable")) this.counterable = true;
		if(val.ContainsKey("reflectable")) this.reflectable = true;
		
		if(val.ContainsKey("skillcombos"))
		{
			this.skillCombo = new int[int.Parse((string)val["skillcombos"])];
		}
		
		if(val.ContainsKey("counter"))
		{
			this.bonus.counterBonus = int.Parse((string)val["counter"]);
		}
		if(val.ContainsKey("escape"))
		{
			this.bonus.escapeBonus = int.Parse((string)val["escape"]);
		}
		if(val.ContainsKey("hitbonus"))
		{
			this.bonus.hitBonus = int.Parse((string)val["hitbonus"]);
		}
		
		if(val.ContainsKey("skillreuse"))
		{
			this.skillReuse = (StatusEffectEnd)System.Enum.Parse(typeof(StatusEffectEnd), (string)val["skillreuse"]);
		}
		if(val.ContainsKey("reusetime"))
		{
			this.reuseTime = float.Parse((string)val["reusetime"]);
		}
		
		if(val.ContainsKey("casttime"))
		{
			this.castTime = float.Parse((string)val["casttime"]);
			if(val.ContainsKey("cancelable")) this.cancelable = true;
		}
		
		if(val.ContainsKey(XMLHandler.NODES))
		{
			ArrayList s = val[XMLHandler.NODES] as ArrayList;
			foreach(Hashtable ht in s)
			{
				if(ht[XMLHandler.NODE_NAME] as string == SkillData.USERCONSUME)
				{
					int j = int.Parse((string)ht["id"]);
					if(j < this.userConsume.Length) this.userConsume[j].SetData(ht);
				}
				else if(ht[XMLHandler.NODE_NAME] as string == SkillData.TARGETCONSUME)
				{
					int j = int.Parse((string)ht["id"]);
					if(j < this.targetConsume.Length) this.targetConsume[j].SetData(ht);
				}
				else if(ht[XMLHandler.NODE_NAME] as string == SkillData.EFFECT)
				{
					int j = int.Parse((string)ht["id"]);
					if(j < this.skillEffect.Length)
					{
						this.skillEffect[j] = (SkillEffect)System.Enum.Parse(typeof(SkillEffect), (string)ht["value"]);
					}
				}
				else if(ht[XMLHandler.NODE_NAME] as string == SkillData.SKILLCOMBO)
				{
					int j = int.Parse((string)ht["id"]);
					if(j < this.skillCombo.Length)
					{
						this.skillCombo[j] = int.Parse((string)ht["skill"]);
					}
				}
				else if(ht[XMLHandler.NODE_NAME] as string == SkillData.BONUS)
				{
					int j = int.Parse((string)ht["id"]);
					if(j < this.bonus.statusBonus.Length)
					{
						this.bonus.statusBonus[j] = int.Parse((string)ht["value"]);
					}
				}
				else if(ht[XMLHandler.NODE_NAME] as string == SkillData.ELEMENT)
				{
					int j = int.Parse((string)ht["id"]);
					if(j < this.elementValue.Length)
					{
						this.elementValue[j] = int.Parse((string)ht["value"]);
						this.elementOperator[j] = (SimpleOperator)System.Enum.Parse(
								typeof(SimpleOperator), (string)ht["operator"]);
					}
				}
				else if(ht[XMLHandler.NODE_NAME] as string == SkillData.BONUSSETTINGS)
				{
					this.bonus.SetData(ht);
				}
				else if(ht[XMLHandler.NODE_NAME] as string == SkillData.AUDIOCLIP)
				{
					this.audioName = ht[XMLHandler.CONTENT] as string;
				}
				else if(ht[XMLHandler.NODE_NAME] as string == SkillData.RACE)
				{
					int j = int.Parse((string)ht["id"]);
					if(j < this.raceValue.Length)
					{
						this.raceValue[j] = int.Parse((string)ht["value"]);
					}
				}
				else if(ht[XMLHandler.NODE_NAME] as string == SkillData.STEALCHANCE)
				{
					this.stealChance.SetData(ht);
				}
				else if(ht[XMLHandler.NODE_NAME] as string == SkillData.SIZE)
				{
					int j = int.Parse((string)ht["id"]);
					if(j < this.sizeValue.Length)
					{
						this.sizeValue[j] = int.Parse((string)ht["value"]);
					}
				}
			}
		}
	}
	
	public SkillLevel GetCopy()
	{
		SkillLevel s = new SkillLevel();
		
		s.skillElement = this.skillElement;
		s.hitChance = this.hitChance;
		s.hitFormula = this.hitFormula;
		s.counterable = this.counterable;
		s.reflectable = this.reflectable;
		s.revive = this.revive;
		s.battleAnimation = this.battleAnimation;
		s.animationID = this.animationID;
		s.endTurn = this.endTurn;
		s.timebarUse = this.timebarUse;
		s.audioName = this.audioName;
		s.useRange.SetData(this.useRange.GetData(new Hashtable()));
		
		s.userConsume = new ValueChange[this.userConsume.Length];
		s.targetConsume = new ValueChange[this.targetConsume.Length];
		for(int i=0; i<this.userConsume.Length; i++)
		{
			if(this.userConsume[i].active)
			{
				s.userConsume[i] = new ValueChange(this.userConsume[i].GetData(new Hashtable()));
			}
			else s.userConsume[i] = new ValueChange();
			if(this.targetConsume[i].active)
			{
				s.targetConsume[i] = new ValueChange(this.targetConsume[i].GetData(new Hashtable()));
			}
			else s.targetConsume[i] = new ValueChange();
		}
		
		s.skillEffect = new SkillEffect[this.skillEffect.Length];
		for(int i=0; i<this.skillEffect.Length; i++)
		{
			s.skillEffect[i] = this.skillEffect[i];
		}
		
		s.skillCombo = new int[this.skillCombo.Length];
		for(int i=0; i<this.skillCombo.Length; i++)
		{
			s.skillCombo[i] = this.skillCombo[i];
		}
		
		s.elementValue = new int[this.elementValue.Length];
		s.elementOperator = new SimpleOperator[this.elementOperator.Length];
		for(int i=0; i<this.elementValue.Length; i++)
		{
			s.elementValue[i] = this.elementValue[i];
			s.elementOperator[i] = this.elementOperator[i];
		}
		
		s.raceValue = new int[this.raceValue.Length];
		for(int i=0; i<this.raceValue.Length; i++)
		{
			s.raceValue[i] = this.raceValue[i];
		}
		
		s.sizeValue = new int[this.sizeValue.Length];
		for(int i=0; i<this.sizeValue.Length; i++)
		{
			s.sizeValue[i] = this.sizeValue[i];
		}
		
		s.skillReuse = this.skillReuse;
		s.reuseTime = this.reuseTime;
		s.castTime = this.castTime;
		s.cancelable = this.cancelable;
		s.bonus.SetData(this.bonus.GetData(new Hashtable()));
		s.stealChance.SetData(this.stealChance.GetData(new Hashtable()));
		return s;
	}
	
	/*
	============================================================================
	Skill combo functions
	============================================================================
	*/
	public void AddSkillCombo()
	{
		this.skillCombo = ArrayHelper.Add(0, this.skillCombo);
	}
	
	public void RemoveSkillCombo(int index)
	{
		this.skillCombo = ArrayHelper.Remove(index, this.skillCombo);
	}
	
	public bool CanUseSkillCombo(int lastSkill)
	{
		bool can = true;
		if(this.skillCombo.Length > 0)
		{
			can = false;
			for(int i=0; i<this.skillCombo.Length; i++)
			{
				if(this.skillCombo[i] == lastSkill)
				{
					can = true;
					break;
				}
			}
		}
		return can;
	}
	
	/*
	============================================================================
	Effect functions
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
	
	public bool CanApplyEffect(int effectID)
	{
		return !SkillEffect.REMOVE.Equals(this.skillEffect[effectID]);
	}
	
	public bool CanRemoveEffect(int effectID)
	{
		return !SkillEffect.ADD.Equals(this.skillEffect[effectID]);
	}
	
	/*
	============================================================================
	Use functions
	============================================================================
	*/
	public void UserConsume(Combatant user)
	{
		for(int i=0; i<this.userConsume.Length; i++)
		{
			if(this.userConsume[i].active)
			{
				this.userConsume[i].ChangeValue(i, -1, user, user, DataHolder.BattleSystemData().showUserDamage, 1, 1);
			}
		}
	}
	
	public CombatantAnimation[] Use(Combatant user, Combatant[] target, BattleAction ba, 
			bool uc, int realID, float damageFactor, float damageMultiplier)
	{
		AudioClip clip = null;
		if(this.audioName != "")
		{
			clip = (AudioClip)Resources.Load(SkillData.AUDIO_PATH+this.audioName, typeof(AudioClip));
		}
		
		CombatantAnimation[] anims = new CombatantAnimation[target.Length];
		if(uc) this.UserConsume(user);
		
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
			
			if(target[j].IsSkillTypeBlocked(DataHolder.Skill(realID).skilltype))
			{
				DataHolder.BattleSystemData().blockTextSettings.ShowText("", target[j]);
			}
			else if(!this.hitChance || DataHolder.GameSettings().GetRandom() <= 
					(DataHolder.Formulas().formula[this.hitFormula].Calculate(user, target[j]) + user.GetHitBonus()))
			{
				if(!target[j].isDead || (target[j].isDead && this.revive && !target[j].noRevive))
				{
					if(this.revive && target[j].isDead && !target[j].noRevive)
					{
						target[j].isDead = false;
						anims[j] = CombatantAnimation.REVIVE;
					}
					for(int i=0; i<this.targetConsume.Length; i++)
					{
						if(this.targetConsume[i].active)
						{
							int oldVal = target[j].status[i].GetValue();
							int change = this.targetConsume[i].ChangeValue(i, this.skillElement, user, target[j], true, damageFactor, damageMultiplier);
							if(!(this.revive && target[j].isDead && !target[j].noRevive) && ((this.targetConsume[i].IsSub() && change > 0) ||
								(this.targetConsume[i].IsAdd() && change < 0) ||
								(this.targetConsume[i].IsSet() && change < oldVal)))
							{
								anims[j] = CombatantAnimation.DAMAGE;
							}
						}
					}
					this.ApplyEffects(user, target[j]);
					if(this.orderChange != 0)
					{
						DataHolder.BattleSystem().OrderChange(target[j].battleID, this.orderChange);
					}
					
					this.stealChance.Steal(user, target[j]);
				}
			}
			else
			{
				DataHolder.BattleSystemData().missTextSettings.ShowText("", target[j]);
				anims[j] = CombatantAnimation.EVADE;
			}
		}
		
		for(int j=0; j<target.Length; j++)
		{
			if(this.counterable && user.battleID != target[j].battleID)
			{
				if(DataHolder.BattleSystem().IsRealTime()) target[j].UseCounter(user);
				else if(ba != null) ba.doCounter[j] = target[j].UseCounter(user);
			}
		}
		
		if(!StatusEffectEnd.NONE.Equals(this.skillReuse))
		{
			user.AddSkillBlock(new SkillBlock(realID, this.reuseTime, this.skillReuse));
		}
		
		return anims;
	}
	
	/*
	============================================================================
	Check functions
	============================================================================
	*/
	public bool CanUse(Combatant c, int realID)
	{
		bool can = this.CanUseSkillCombo(c.lastSkill);
		if(can)
		{
			for(int i=0; i<this.userConsume.Length; i++)
			{
				if(this.userConsume[i].active)
				{
					int change = this.userConsume[i].GetChange(c, c, 1, 1);
					if((this.userConsume[i].IsAdd() && change < 0 && c.status[i].GetValue() < change) ||
							(this.userConsume[i].IsSub() && change > 0 && c.status[i].GetValue() < change))
					{
						can = false;
						break;
					}
				}
			}
		}
		if(can) can = !c.IsSkillBlocked(realID);
		return can;
	}
	
	public string GetSkillCostString(Combatant c)
	{
		string costs = "";
		for(int i=0; i<this.userConsume.Length; i++)
		{
			if(this.userConsume[i].active)
			{
				int change = this.userConsume[i].GetChange(c, c, 1, 1);
				if("" != costs) costs += " ";
				costs += DataHolder.StatusValues().GetName(i);
				if((this.userConsume[i].IsAdd() && change > 0) ||
					(this.userConsume[i].IsSub() && change < 0))
				{
					costs += "+";
				}
				else if(this.userConsume[i].IsSet()) costs += "=";
				else costs += " ";
				costs += change.ToString();
			}
		}
		return costs;
	}
}
