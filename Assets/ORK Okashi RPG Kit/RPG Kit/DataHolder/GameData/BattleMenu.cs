
using System.Collections;
using UnityEngine;

public class BattleMenu
{
	public bool enableDrag = false;
	public bool enableDoubleClick = false;
	
	public int dialoguePosition = 0;
	public int targetPosition = 0;
	public int skillPosition = 0;
	public int itemPosition = 0;
	
	public float updateInterval = 0.5f;
	
	// back
	public ArrayList backName = new ArrayList();
	public bool addBack = false;
	public bool backFirst = false;
	public string backIconName = "";
	public Texture2D backIcon;
	
	// attack point
	public ArrayList attackName = new ArrayList();
	public bool showAttack = true;
	public string attackIconName = "";
	public Texture2D attackIcon;
	
	// skill point
	public ArrayList skillName = new ArrayList();
	public bool showSkills = true;
	public bool combineSkills = true;
	public string skillIconName = "";
	public Texture2D skillIcon;
	
	// item point
	public ArrayList itemName = new ArrayList();
	public bool showItems = true;
	public bool combineItems = true;
	public string itemIconName = "";
	public Texture2D itemIcon;
	
	// defend point
	public ArrayList defendName = new ArrayList();
	public bool showDefend = true;
	public string defendIconName = "";
	public Texture2D defendIcon;
	
	// escape point
	public ArrayList escapeName = new ArrayList();
	public bool showEscape = true;
	public string escapeIconName = "";
	public Texture2D escapeIcon;
	
	// end turn point
	public ArrayList endTurnName = new ArrayList();
	public bool showEndTurn = false;
	public string endTurnIconName = "";
	public Texture2D endTurnIcon;
	
	// menu point order
	public string[] order = new string[] {BattleMenu.ATTACK, BattleMenu.SKILL, BattleMenu.ITEM, 
			BattleMenu.DEFEND, BattleMenu.ESCAPE, BattleMenu.ENDTURN};
	public static string ATTACK = "Attack";
	public static string SKILL = "Skill";
	public static string ITEM = "Item";
	public static string DEFEND = "Defend";
	public static string ESCAPE = "Escape";
	public static string ENDTURN = "End";
	// special
	public static string BACK = "Back";
	
	// target selection
	public bool useTargetMenu = false;
	
	// target cursor
	public bool useTargetCursor = false;
	public string cursorPrefabName = "";
	public string cursorChildName = "";
	public Vector3 cursorOffset = Vector3.zero;
	
	// target blink
	public bool useTargetBlink = false;
	public bool fromCurrent = false;
	public bool blinkChildren = false;
	public float blinkTime = 0.5f;
	public EaseType blinkInterpolation = EaseType.Linear;
	
	public bool aBlink = false;
	public float aStart = 1;
	public float aEnd = 1;
	
	public bool rBlink = false;
	public float rStart = 1;
	public float rEnd = 1;
	
	public bool gBlink = false;
	public float gStart = 1;
	public float gEnd = 1;
	
	public bool bBlink = false;
	public float bStart = 1;
	public float bEnd = 1;
	
	// click combatant
	public MouseTouchControl mouseTouch = new MouseTouchControl();
	
	// ingame
	// ORK GUI stored menus
	public DialoguePosition itemDP;
	public DialoguePosition targetDP;
	
	public BattleMenu()
	{
		for(int i=0; i<DataHolder.LanguageCount; i++)
		{
			this.backName.Add(BattleMenu.BACK);
			this.attackName.Add(BattleMenu.ATTACK);
			this.skillName.Add(BattleMenu.SKILL);
			this.itemName.Add(BattleMenu.ITEM);
			this.defendName.Add(BattleMenu.DEFEND);
			this.escapeName.Add(BattleMenu.ESCAPE);
			this.endTurnName.Add(BattleMenu.ENDTURN);
		}
	}
	
	public void DeleteBattleTextures()
	{
		if(this.itemDP != null) this.itemDP.multiLabel.DeleteTextures();
		if(this.targetDP != null) this.targetDP.multiLabel.DeleteTextures();
	}
	
	public void LoadIcons()
	{
		if(this.backIcon == null && "" != this.backIconName)
		{
			this.backIcon = (Texture2D)Resources.Load(BattleSystemData.ICON_PATH+this.backIconName, typeof(Texture2D));
		}
		if(this.attackIcon == null && "" != this.attackIconName)
		{
			this.attackIcon = (Texture2D)Resources.Load(BattleSystemData.ICON_PATH+this.attackIconName, typeof(Texture2D));
		}
		if(this.skillIcon == null && "" != this.skillIconName)
		{
			this.skillIcon = (Texture2D)Resources.Load(BattleSystemData.ICON_PATH+this.skillIconName, typeof(Texture2D));
		}
		if(this.itemIcon == null && "" != this.itemIconName)
		{
			this.itemIcon = (Texture2D)Resources.Load(BattleSystemData.ICON_PATH+this.itemIconName, typeof(Texture2D));
		}
		if(this.defendIcon == null && "" != this.defendIconName)
		{
			this.defendIcon = (Texture2D)Resources.Load(BattleSystemData.ICON_PATH+this.defendIconName, typeof(Texture2D));
		}
		if(this.escapeIcon == null && "" != this.escapeIconName)
		{
			this.escapeIcon = (Texture2D)Resources.Load(BattleSystemData.ICON_PATH+this.escapeIconName, typeof(Texture2D));
		}
		if(this.endTurnIcon == null && "" != this.endTurnIconName)
		{
			this.endTurnIcon = (Texture2D)Resources.Load(BattleSystemData.ICON_PATH+this.endTurnIconName, typeof(Texture2D));
		}
	}
	
	public string GetOrder()
	{
		string text = "";
		for(int i=0; i<this.order.Length; i++)
		{
			if(i>0) text += ":";
			text += this.order[i];
		}
		return text;
	}
	
	public void SetOrder(string text)
	{
		this.order = text.Split(new char[] {':'});
		if(this.order.Length < 6)
		{
			this.order = ArrayHelper.Add(BattleMenu.ENDTURN, this.order);
		}
	}
	
	public void MoveUp(int index)
	{
		string tmp = this.order[index-1];
		this.order[index-1] = this.order[index];
		this.order[index] = tmp;
	}
	
	public void MoveDown(int index)
	{
		string tmp = this.order[index+1];
		this.order[index+1] = this.order[index];
		this.order[index] = tmp;
	}
	
	public BattleMenuItem GetBackItem()
	{
		return new BattleMenuItem(new GUIContent(
				this.backName[GameHandler.GetLanguage()].ToString(), this.backIcon),
				-10, BattleMenu.BACK, false, "", null);
	}
	
	public BattleMenuItem[] BattleMenuList(Character character)
	{
		BattleMenuItem[] list = new BattleMenuItem[0];
		this.LoadIcons();
		
		if(this.addBack && this.backFirst && DataHolder.BattleSystem().IsRealTime())
		{
			list = ArrayHelper.Add(this.GetBackItem(), list);
		}
		
		for(int i=0; i<this.order.Length; i++)
		{
			if(this.order[i] == BattleMenu.ATTACK && this.showAttack)
			{
				if(DataHolder.BattleControl().IsAutoUseOnTarget() &&
					character.InAttackRange(DataHolder.BattleControl().partyTarget))
				{
					list = ArrayHelper.Add(new BattleMenuItem(
							new GUIContent(this.attackName[GameHandler.GetLanguage()].ToString(), this.attackIcon),
							-1, BattleMenu.ATTACK, true, "", 
							new BattleAction(AttackSelection.ATTACK, character, BattleAction.PARTY_TARGET, -1, 0)), list);
				}
				else
				{
					list = ArrayHelper.Add(new BattleMenuItem(
							new GUIContent(this.attackName[GameHandler.GetLanguage()].ToString(), this.attackIcon),
							-1, BattleMenu.ATTACK, true, ""), list);
				}
			}
			else if(this.order[i] == BattleMenu.SKILL && this.showSkills)
			{
				SkillLearn[] skills = character.GetSkills();
				if(this.combineSkills)
				{
					if(skills.Length > 0)
					{
						list = ArrayHelper.Add(new BattleMenuItem(
							new GUIContent(this.skillName[GameHandler.GetLanguage()].ToString(), this.skillIcon),
							-1, BattleMenu.SKILL, false, ""), list);
					}
				}
				else
				{
					ArrayList types = new ArrayList();
					for(int j=0; j<skills.Length; j++)
					{
						if(DataHolder.Skill(skills[j].skillID).useInBattle)
						{
							int st = DataHolder.Skill(skills[j].skillID).skilltype;
							if(!types.Contains(st))
							{
								types.Add(st);
							}
						}
					}
					types.Sort(new SkillTypeSorter());
					for(int j=0; j<types.Count; j++)
					{
						list = ArrayHelper.Add(new BattleMenuItem(DataHolder.SkillTypes().GetContent(
								(int)types[j]), (int)types[j], BattleMenu.SKILL, false, ""), list);
					}
				}
			}
			else if(this.order[i] == BattleMenu.ITEM && this.showItems)
			{
				if(this.combineItems)
				{
					list = ArrayHelper.Add(new BattleMenuItem(
						new GUIContent(this.itemName[GameHandler.GetLanguage()].ToString(), this.itemIcon),
						-1, BattleMenu.ITEM, false, ""), list);
				}
				else
				{
					ArrayList types = new ArrayList();
					foreach(int key in GameHandler.Items().Keys)
					{
						if(DataHolder.Item(key).useInBattle)
						{
							int it = DataHolder.Item(key).itemType;
							if(!types.Contains(it))
							{
								types.Add(it);
							}
						}
					}
					types.Sort(new ItemTypeSorter());
					for(int j=0; j<types.Count; j++)
					{
						list = ArrayHelper.Add(new BattleMenuItem(DataHolder.ItemTypes().GetContent(
								(int)types[j]), (int)types[j], BattleMenu.ITEM, false, ""), list);
					}
				}
			}
			else if(this.order[i] == BattleMenu.DEFEND && this.showDefend)
			{
				list = ArrayHelper.Add(new BattleMenuItem(
					new GUIContent(this.defendName[GameHandler.GetLanguage()].ToString(), this.defendIcon),
					-1, BattleMenu.DEFEND, false, ""), list);
			}
			else if(this.order[i] == BattleMenu.ESCAPE && this.showEscape)
			{
				list = ArrayHelper.Add(new BattleMenuItem(
					new GUIContent(this.escapeName[GameHandler.GetLanguage()].ToString(), this.escapeIcon),
					-1, BattleMenu.ESCAPE, false, ""), list);
			}
			else if(this.order[i] == BattleMenu.ENDTURN && this.showEndTurn)
			{
				list = ArrayHelper.Add(new BattleMenuItem(
					new GUIContent(this.endTurnName[GameHandler.GetLanguage()].ToString(), this.endTurnIcon),
					-1, BattleMenu.ENDTURN, false, ""), list);
			}
		}
		
		if(this.addBack && !this.backFirst && DataHolder.BattleSystem().IsRealTime())
		{
			list = ArrayHelper.Add(this.GetBackItem(), list);
		}
		
		return list;
	}
	
	public BattleMenuItem[] GetTargetMenuList(BattleMenuItem bmi, Character character)
	{
		BattleMenuItem[] list = new BattleMenuItem[0];
		
		if(this.addBack && this.backFirst)
		{
			list = ArrayHelper.Add(this.GetBackItem(), list);
		}
		
		if(bmi.type == BattleMenu.ATTACK)
		{
			for(int i=0; i<DataHolder.BattleSystem().enemies.Length; i++)
			{
				Enemy e = DataHolder.BattleSystem().enemies[i];
				list = ArrayHelper.Add(new BattleMenuItem(e.GetContent(), -1, BattleMenu.ATTACK, true, "", 
						new BattleAction(AttackSelection.ATTACK, character, e.battleID, -1, bmi.useLevel)), list);
			}
		}
		else if(bmi.type == BattleMenu.SKILL)
		{
			Skill s = DataHolder.Skill(bmi.id);
			if(s.TargetSingleAlly())
			{
				Character[] party = GameHandler.Party().GetBattleParty();
				for(int i=0; i<party.Length; i++)
				{
					list = ArrayHelper.Add(new BattleMenuItem(party[i].GetContent(),
							-1, BattleMenu.SKILL, true, "", new BattleAction(AttackSelection.SKILL, character, 
							party[i].battleID, bmi.id, bmi.useLevel)), list);
				}
			}
			else if(s.TargetSingleEnemy())
			{
				for(int i=0; i<DataHolder.BattleSystem().enemies.Length; i++)
				{
					Enemy e = DataHolder.BattleSystem().enemies[i];
					list = ArrayHelper.Add(new BattleMenuItem(e.GetContent(), 
							-1, BattleMenu.SKILL, true, "", new BattleAction(AttackSelection.SKILL, character, 
							e.battleID, bmi.id, bmi.useLevel)), list);
				}
			}
			else if(s.TargetAllyGroup())
			{
				list = ArrayHelper.Add(new BattleMenuItem(new GUIContent(DataHolder.BattleSystemData().GetAllAlliesText()), 
						-1, BattleMenu.SKILL, true, "", new BattleAction(AttackSelection.SKILL, character, 
						BattleAction.ALL_CHARACTERS, bmi.id, bmi.useLevel)), list);
			}
			else if(s.TargetEnemyGroup())
			{
				list = ArrayHelper.Add(new BattleMenuItem(new GUIContent(DataHolder.BattleSystemData().GetAllEnemiesText()), 
						-1, BattleMenu.SKILL, true, "", new BattleAction(AttackSelection.SKILL, character, 
						BattleAction.ALL_ENEMIES, bmi.id, bmi.useLevel)), list);
			}
		}
		else if(bmi.type == BattleMenu.ITEM)
		{
			Item it = DataHolder.Item(bmi.id);
			if(it.TargetSingleAlly())
			{
				Character[] party = GameHandler.Party().GetBattleParty();
				for(int i=0; i<party.Length; i++)
				{
					list = ArrayHelper.Add(new BattleMenuItem(party[i].GetContent(), 
							-1, BattleMenu.ITEM, true, "", new BattleAction(AttackSelection.ITEM, character, 
							party[i].battleID, bmi.id, bmi.useLevel)), list);
				}
			}
			else if(it.TargetSingleEnemy())
			{
				for(int i=0; i<DataHolder.BattleSystem().enemies.Length; i++)
				{
					Enemy e = DataHolder.BattleSystem().enemies[i];
					list = ArrayHelper.Add(new BattleMenuItem(e.GetContent(), 
							-1, BattleMenu.ITEM, true, "", new BattleAction(AttackSelection.ITEM, character, 
							e.battleID, bmi.id, bmi.useLevel)), list);
				}
			}
			else if(it.TargetAllyGroup())
			{
				list = ArrayHelper.Add(new BattleMenuItem(new GUIContent("All allies"), -1, BattleMenu.ITEM, true, "", 
						new BattleAction(AttackSelection.ITEM, character, 
						BattleAction.ALL_CHARACTERS, bmi.id, bmi.useLevel)), list);
			}
			else if(it.TargetEnemyGroup())
			{
				list = ArrayHelper.Add(new BattleMenuItem(new GUIContent("All enemies"), -1, BattleMenu.ITEM, true, "", 
						new BattleAction(AttackSelection.ITEM, character, 
						BattleAction.ALL_ENEMIES, bmi.id, bmi.useLevel)), list);
			}
		}
		if(this.addBack && !this.backFirst)
		{
			list = ArrayHelper.Add(this.GetBackItem(), list);
		}
		return list;
	}
	
	public BattleMenuItem[] GetSkillMenuList(int type, Character character)
	{
		BattleMenuItem[] list = new BattleMenuItem[0];
		if(this.addBack && this.backFirst)
		{
			list = ArrayHelper.Add(this.GetBackItem(), list);
		}
		
		ArrayList skills = new ArrayList();
		SkillLearn[] s = character.GetSkills();
		for(int i=0; i<s.Length; i++)
		{
			if(DataHolder.Skill(s[i].skillID).useInBattle &&
				(this.combineSkills || DataHolder.Skill(s[i].skillID).skilltype == type))
			{
				skills.Add(s[i].skillID);
			}
		}
		skills.Sort(new SkillNameSorter());
		for(int i=0; i<skills.Count; i++)
		{
			SkillLearn sk = character.GetSkill((int)skills[i]);
			if(sk != null)
			{
				if(DataHolder.Skill(sk.skillID).TargetSelf())
				{
					list = ArrayHelper.Add(new BattleMenuItem(sk.GetContent(), sk.skillID, sk.GetLevel(), BattleMenu.SKILL, true, 
							DataHolder.Skill(sk.skillID).GetSkillCostString(character, sk.GetLevel()), 
							new BattleAction(AttackSelection.SKILL, character, 
							character.battleID, sk.skillID, sk.GetLevel())), list);
				}
				else if(DataHolder.Skill(sk.skillID).TargetNone())
				{
					list = ArrayHelper.Add(new BattleMenuItem(sk.GetContent(), sk.skillID, sk.GetLevel(), BattleMenu.SKILL, true, 
							DataHolder.Skill(sk.skillID).GetSkillCostString(character, sk.GetLevel()), 
							new BattleAction(AttackSelection.SKILL, character, 
							BattleAction.NONE, sk.skillID, sk.GetLevel())), list);
					list[list.Length-1].action.targetRaycast = DataHolder.Skill(sk.skillID).targetRaycast;
				}
				else if(DataHolder.Skill(sk.skillID).TargetSingleEnemy() && 
					DataHolder.BattleControl().IsAutoUseOnTarget() &&
					DataHolder.Skill(sk.skillID).InRange(character, DataHolder.BattleControl().partyTarget, sk.GetLevel()))
				{
					list = ArrayHelper.Add(new BattleMenuItem(sk.GetContent(), sk.skillID, sk.GetLevel(), BattleMenu.SKILL, true, 
							DataHolder.Skill(sk.skillID).GetSkillCostString(character, sk.GetLevel()), 
							new BattleAction(AttackSelection.SKILL, character, 
							BattleAction.PARTY_TARGET, sk.skillID, sk.GetLevel())), list);
				}
				else
				{
					list = ArrayHelper.Add(new BattleMenuItem(sk.GetContent(), sk.skillID, sk.GetLevel(), BattleMenu.SKILL, true, 
							DataHolder.Skill(sk.skillID).GetSkillCostString(character, sk.GetLevel()), null), list);
				}
			}
		}
		if(this.addBack && !this.backFirst)
		{
			list = ArrayHelper.Add(this.GetBackItem(), list);
		}
		return list;
	}
	
	public BattleMenuItem[] GetItemMenuList(int type, Character character)
	{
		BattleMenuItem[] list = new BattleMenuItem[0];
		if(this.addBack && this.backFirst)
		{
			list = ArrayHelper.Add(this.GetBackItem(), list);
		}
		
		ArrayList items = new ArrayList();
		foreach(int key in GameHandler.Items().Keys)
		{
			if(DataHolder.Item(key).useInBattle &&
					(this.combineItems || DataHolder.Item(key).itemType == type))
			{
				items.Add(key);
			}
		}
		items.Sort(new ItemNameSorter());
		for(int i=0; i<items.Count; i++)
		{
			int itemID = (int)items[i];
			if(DataHolder.Item(itemID).TargetSelf())
			{
				list = ArrayHelper.Add(new BattleMenuItem(DataHolder.Items().GetContent(itemID), itemID, 0, 
						BattleMenu.ITEM, true, GameHandler.GetItemCount(itemID).ToString(), 
						new BattleAction(AttackSelection.ITEM, character, character.battleID, itemID, 0)), list);
			}
			else if(DataHolder.Item(itemID).TargetNone())
			{
				list = ArrayHelper.Add(new BattleMenuItem(DataHolder.Items().GetContent(itemID), itemID, 0, 
						BattleMenu.ITEM, true, GameHandler.GetItemCount(itemID).ToString(), 
						new BattleAction(AttackSelection.ITEM, character, BattleAction.NONE, itemID, 0)), list);
				list[list.Length-1].action.targetRaycast = DataHolder.Item(itemID).targetRaycast;
			}
			else if(DataHolder.Item(itemID).TargetSingleEnemy() && 
				DataHolder.BattleControl().IsAutoUseOnTarget() &&
				DataHolder.Item(itemID).useRange.InRange(character, DataHolder.BattleControl().partyTarget))
			{
				list = ArrayHelper.Add(new BattleMenuItem(DataHolder.Items().GetContent(itemID), itemID, 0, 
						BattleMenu.ITEM, true, GameHandler.GetItemCount(itemID).ToString(), 
						new BattleAction(AttackSelection.ITEM, character, BattleAction.PARTY_TARGET, itemID, 0)), list);
			}
			else
			{
				list = ArrayHelper.Add(new BattleMenuItem(DataHolder.Items().GetContent(itemID), itemID, 
						BattleMenu.ITEM, true, GameHandler.GetItemCount(itemID).ToString()), list);
			}
			
		}
		if(this.addBack && !this.backFirst)
		{
			list = ArrayHelper.Add(this.GetBackItem(), list);
		}
		return list;
	}
}

public class BattleMenuItem
{
	public GUIContent content;
	public int id;
	public string type;
	public bool isTarget;
	public int useLevel = 0;
	public string info;
	
	public BattleAction action;
	
	public bool add = false;
	public bool lastAdd = false;
	public bool active = false;
	public bool lastActive = false;
	
	public BattleMenuItem(string n, int i, string t, bool it, string inf) : this(new GUIContent(n), i, t, it, inf)
	{
		
	}
	
	public BattleMenuItem(GUIContent c, int i, string t, bool it, string inf) : this(c, i, t, it, inf, null)
	{
		
	}
	
	public BattleMenuItem(GUIContent c, int i, string t, bool it, string inf, BattleAction a) : this(c, i, 0, t, it, inf, a)
	{
		
	}
	
	public BattleMenuItem(GUIContent c, int i, int ul, string t, bool it, string inf, BattleAction a)
	{
		this.content = c;
		this.id = i;
		this.useLevel = ul;
		this.type = t;
		this.isTarget = it;
		this.info = inf;
		this.action = a;
		if(this.action != null)
		{
			this.action.CheckRevive();
		}
	}
	
	public bool Check(Character c)
	{
		this.lastAdd = this.add;
		this.lastActive = this.active;
		bool targetChange = false;
		
		// add check
		this.add = true;
		if(this.action != null && (!this.action.InRange() || !this.action.TargetAlive()))
		{
			this.add = false;
		}
		if(this.action != null && this.action.RangeDifference())
		{
			targetChange = true;
		}
		
		// active check
		this.active = true;
		// blocked
		if((BattleMenu.ATTACK == this.type && c.IsBlockAttack()) ||
			(BattleMenu.SKILL == this.type && c.IsBlockSkills()) ||
			(BattleMenu.ITEM == this.type && c.IsBlockItems()) ||
			(BattleMenu.DEFEND == this.type && c.IsBlockDefend()) ||
			(BattleMenu.ESCAPE == this.type && (c.IsBlockEscape() || !DataHolder.BattleSystem().canEscape)))
		{
			this.active = false;
		}
		// skill
		else if(this.isTarget && BattleMenu.SKILL == this.type && 
			this.id >= 0 && !DataHolder.Skill(this.id).CanUse(c, this.useLevel))
		{
			this.active = false;
		}
		// no skills
		else if(!this.isTarget && BattleMenu.SKILL == this.type && c.GetSkills().Length == 0)
		{
			this.active = false;
		}
		// no items
		else if(!this.isTarget && BattleMenu.ITEM == this.type && !GameHandler.HasItems())
		{
			this.active = false;
		}
		
		// active time checks
		if(this.active && DataHolder.BattleSystem().IsActiveTime())
		{
			if(BattleMenu.ATTACK == this.type && this.isTarget && 
				((!DataHolder.BattleSystem().attackEndTurn && 
				c.usedTimeBar+DataHolder.BattleSystem().attackTimebarUse > DataHolder.BattleSystem().maxTimebar) ||
				(DataHolder.BattleSystem().attackEndTurn && c.usedTimeBar > 0)))
			{
				this.active = false;
			}
			else if(BattleMenu.SKILL == this.type && this.isTarget && this.id >= 0 && 
				((!DataHolder.Skill(this.id).level[this.useLevel].endTurn && 
				c.usedTimeBar+DataHolder.Skill(this.id).level[this.useLevel].timebarUse > DataHolder.BattleSystem().maxTimebar) ||
				(DataHolder.Skill(this.id).level[this.useLevel].endTurn && c.usedTimeBar > 0)))
			{
				this.active = false;
			}
			else if(BattleMenu.ITEM == this.type && this.isTarget && 
				((!DataHolder.BattleSystem().itemEndTurn && 
				c.usedTimeBar+DataHolder.BattleSystem().itemTimebarUse > DataHolder.BattleSystem().maxTimebar) ||
				(DataHolder.BattleSystem().itemEndTurn && c.usedTimeBar > 0)))
			{
				this.active = false;
			}
			else if(BattleMenu.DEFEND == this.type &&  
				((!DataHolder.BattleSystem().defendEndTurn && 
				c.usedTimeBar+DataHolder.BattleSystem().defendTimebarUse > DataHolder.BattleSystem().maxTimebar) ||
				(DataHolder.BattleSystem().defendEndTurn && c.usedTimeBar > 0)))
			{
				this.active = false;
			}
			else if(BattleMenu.ESCAPE == this.type &&  
				((!DataHolder.BattleSystem().escapeEndTurn && 
				c.usedTimeBar+DataHolder.BattleSystem().escapeTimebarUse > DataHolder.BattleSystem().maxTimebar) ||
				(DataHolder.BattleSystem().escapeEndTurn && c.usedTimeBar > 0)))
			{
				this.active = false;
			}
		}
		
		// should update check
		return this.lastAdd != this.add || this.lastActive != this.active || targetChange;
	}
}