
using UnityEngine;
using System.Collections;

public class BattleControl
{
	// keys
	public string attackKey = "";
	public bool attackPartyTarget = false;
	public bool aptRange = false;
	
	public string battleMenuKey = "";
	public string skillMenuKey = "";
	public string itemMenuKey = "";
	public bool keyCloses = false;
	public bool allowMenuSwitch = false;
	
	public string nextTargetKey = "";
	public string previousTargetKey = "";
	public string nearestTargetKey = "";
	public string clearTargetKey = "";
	
	// party target
	public bool usePartyTarget = false;
	public bool onlyInBattleRange = false;
	public MouseTouchControl mouseTouch = new MouseTouchControl();
	public bool allowTargetRemove = false;
	public bool ptNoActionOnly = false;
	public bool autoSelectTarget = false;
	public bool autoUseOnTarget = false;
	public bool useTargetCursor = false;
	public string cursorPrefabName = "";
	public string cursorChildName = "";
	public Vector3 cursorOffset = Vector3.zero;
	
	// auto attack
	public bool autoAttackTarget = false;
	public bool aaPlayerOnly = false;
	
	public static string ATTACK = "attack";
	public static string MENU = "menu";
	public static string SKILLMENU = "skillmenu";
	public static string ITEMMENU = "itemmenu";
	public static string NEXTTARGET = "nexttarget";
	public static string PREVIOUSTARGET = "previoustarget";
	public static string NEARESTTARGET = "nearesttarget";
	public static string CLEARTARGET = "cleartarget";
	public static string CURSOR = "cursor";
	public static string CURSORCHILD = "cursorchild";
	
	// ingame
	public Combatant partyTarget = null;
	public Transform ptObject = null;
	public GameObject cursorInstance = null;
	
	public BattleControl()
	{
		
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		ArrayList s = new ArrayList();
		
		if(this.usePartyTarget)
		{
			ht.Add("usepartytarget", "true");
			s.Add(HashtableHelper.GetContentHashtable(BattleControl.NEXTTARGET, this.nextTargetKey));
			s.Add(HashtableHelper.GetContentHashtable(BattleControl.PREVIOUSTARGET, this.previousTargetKey));
			s.Add(HashtableHelper.GetContentHashtable(BattleControl.NEARESTTARGET, this.nearestTargetKey));
			if(this.onlyInBattleRange) ht.Add("onlyinbattlerange", "true");
			if(this.autoSelectTarget) ht.Add("autoselecttarget", "true");
			else s.Add(HashtableHelper.GetContentHashtable(BattleControl.CLEARTARGET, this.clearTargetKey));
			ht = this.mouseTouch.GetData(ht);
			if(this.allowTargetRemove) ht.Add("allowtargetremove", "true");
			if(this.ptNoActionOnly) ht.Add("ptnoactiononly", "true");
			if(this.autoUseOnTarget) ht.Add("autouseontarget", "true");
			if(this.attackPartyTarget) ht.Add("attackpartytarget", "true");
			if(this.aptRange) ht.Add("aptrange", "true");
			if(this.autoAttackTarget)
			{
				ht.Add("autoattacktarget", "true");
				if(this.aaPlayerOnly) ht.Add("aaplayeronly", "true");
			}
			if(this.useTargetCursor)
			{
				Hashtable ht2 = HashtableHelper.GetContentHashtable(BattleControl.CURSOR, this.cursorPrefabName);
				ht2.Add("x", this.cursorOffset.x.ToString());
				ht2.Add("y", this.cursorOffset.y.ToString());
				ht2.Add("z", this.cursorOffset.z.ToString());
				s.Add(ht2);
				if(this.cursorChildName != "")
				{
					s.Add(HashtableHelper.GetContentHashtable(BattleControl.CURSORCHILD, this.cursorChildName));
				}
			}
		}
		
		if(this.keyCloses) ht.Add("keycloses", "true");
		if(this.allowMenuSwitch) ht.Add("allowmenuswitch", "true");
		s.Add(HashtableHelper.GetContentHashtable(BattleControl.ATTACK, this.attackKey));
		s.Add(HashtableHelper.GetContentHashtable(BattleControl.MENU, this.battleMenuKey));
		s.Add(HashtableHelper.GetContentHashtable(BattleControl.SKILLMENU, this.skillMenuKey));
		s.Add(HashtableHelper.GetContentHashtable(BattleControl.ITEMMENU, this.itemMenuKey));
		ht.Add(XMLHandler.NODES, s);
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		if(ht.ContainsKey("keycloses")) this.keyCloses = true;
		if(ht.ContainsKey("allowmenuswitch")) this.allowMenuSwitch = true;
		if(ht.ContainsKey("usepartytarget"))
		{
			this.usePartyTarget = true;
			if(ht.ContainsKey("onlyinbattlerange")) this.onlyInBattleRange = true;
			if(ht.ContainsKey("autoselecttarget")) this.autoSelectTarget = true;
			this.mouseTouch.SetData(ht);
			if(ht.ContainsKey("allowtargetremove")) this.allowTargetRemove = true;
			if(ht.ContainsKey("ptnoactiononly")) this.ptNoActionOnly = true;
			if(ht.ContainsKey("autouseontarget")) this.autoUseOnTarget = true;
			if(ht.ContainsKey("autoattacktarget")) this.autoAttackTarget = true;
			if(ht.ContainsKey("aaplayeronly")) this.aaPlayerOnly = true;
			if(ht.ContainsKey("attackpartytarget")) this.attackPartyTarget = true;
			if(ht.ContainsKey("aptrange")) this.aptRange = true;
		}
		ArrayList s = ht[XMLHandler.NODES] as ArrayList;
		foreach(Hashtable ht2 in s)
		{
			if(ht2[XMLHandler.NODE_NAME] as string == BattleControl.ATTACK)
			{
				this.attackKey = ht2[XMLHandler.CONTENT] as string;
			}
			else if(ht2[XMLHandler.NODE_NAME] as string == BattleControl.MENU)
			{
				this.battleMenuKey = ht2[XMLHandler.CONTENT] as string;
			}
			else if(ht2[XMLHandler.NODE_NAME] as string == BattleControl.SKILLMENU)
			{
				this.skillMenuKey = ht2[XMLHandler.CONTENT] as string;
			}
			else if(ht2[XMLHandler.NODE_NAME] as string == BattleControl.ITEMMENU)
			{
				this.itemMenuKey = ht2[XMLHandler.CONTENT] as string;
			}
			else if(ht2[XMLHandler.NODE_NAME] as string == BattleControl.NEXTTARGET)
			{
				this.nextTargetKey = ht2[XMLHandler.CONTENT] as string;
			}
			else if(ht2[XMLHandler.NODE_NAME] as string == BattleControl.PREVIOUSTARGET)
			{
				this.previousTargetKey = ht2[XMLHandler.CONTENT] as string;
			}
			else if(ht2[XMLHandler.NODE_NAME] as string == BattleControl.NEARESTTARGET)
			{
				this.nearestTargetKey = ht2[XMLHandler.CONTENT] as string;
			}
			else if(ht2[XMLHandler.NODE_NAME] as string == BattleControl.CLEARTARGET)
			{
				this.clearTargetKey = ht2[XMLHandler.CONTENT] as string;
			}
			else if(ht2[XMLHandler.NODE_NAME] as string == BattleControl.CURSOR)
			{
				this.useTargetCursor = true;
				this.cursorPrefabName = ht2[XMLHandler.CONTENT] as string;
				this.cursorOffset = new Vector3(
						float.Parse((string)ht2["x"]), 
						float.Parse((string)ht2["y"]), 
						float.Parse((string)ht2["z"]));
			}
			else if(ht2[XMLHandler.NODE_NAME] as string == BattleControl.CURSORCHILD)
			{
				this.cursorChildName = ht2[XMLHandler.CONTENT] as string;
			}
		}
	}
	
	/*
	============================================================================
	Control functions
	============================================================================
	*/
	public void Tick(bool blockKeys)
	{
		if(GameHandler.IsInBattleArea() &&
			DataHolder.BattleSystem().IsRealTime())
		{
			DataHolder.BattleSystem().Tick();
			Character c = GameHandler.Party().GetPlayerCharacter();
			if(c != null && !c.isDead)
			{
				if(!blockKeys)
				{
					c.controlMap.Tick(this, c);
					if(ControlHandler.IsPressed(this.attackKey))
					{
						if(c.CanChooseAction() && !c.autoAttackStarted)
						{
							if(this.attackPartyTarget && this.partyTarget != null && 
								(!this.aptRange || c.InAttackRange(this.partyTarget)))
							{
								c.AddAttackAction(BattleAction.PARTY_TARGET, true);
							}
							else c.AddAttackAction(BattleAction.NONE, true);
						}
					}
				}
				if(ControlHandler.IsPressed(this.battleMenuKey))
				{
					if(c.CanChooseAction())
					{
						c.ChooseAction();
					}
					else if(this.allowMenuSwitch &&
						!c.battleMenu.IsBaseMenu() && c.IsChoosingAction())
					{
						GameHandler.GetLevelHandler().SwitchBattleMenu(BattleMenuMode.BASE);
					}
					else if(this.keyCloses && 
						(c.IsCalledMenuMode(BattleMenuMode.BASE) ||
						c.battleMenu.IsBaseMenu()))
					{
						GameHandler.GetLevelHandler().CloseBattleMenu();
					}
				}
				else if(ControlHandler.IsPressed(this.skillMenuKey))
				{
					if(c.CanChooseAction())
					{
						c.CallSkillMenu();
					}
					else if(this.allowMenuSwitch &&
						!c.battleMenu.IsSkillMenu() && c.IsChoosingAction())
					{
						GameHandler.GetLevelHandler().SwitchBattleMenu(BattleMenuMode.SKILL);
					}
					else if(this.keyCloses && 
						(c.IsCalledMenuMode(BattleMenuMode.SKILL) ||
						c.battleMenu.IsSkillMenu()))
					{
						GameHandler.GetLevelHandler().CloseBattleMenu();
					}
				}
				else if(ControlHandler.IsPressed(this.itemMenuKey))
				{
					if(c.CanChooseAction())
					{
						c.CallItemMenu();
					}
					else if(this.allowMenuSwitch &&
						!c.battleMenu.IsItemMenu() && c.IsChoosingAction())
					{
						GameHandler.GetLevelHandler().SwitchBattleMenu(BattleMenuMode.ITEM);
					}
					else if(this.keyCloses && 
						(c.IsCalledMenuMode(BattleMenuMode.ITEM) ||
						c.battleMenu.IsItemMenu()))
					{
						GameHandler.GetLevelHandler().CloseBattleMenu();
					}
				}
			}
			
			if(this.usePartyTarget && (!this.ptNoActionOnly || c.CanChooseAction()))
			{
				if(this.partyTarget != null && 
					(this.partyTarget.isDead || this.partyTarget.prefabInstance == null))
				{
					this.SetPartyTarget(null);
				}
				// click
				Vector3 point = Vector3.zero;
				if(this.mouseTouch.Interacted(ref point))
				{
					this.TargetSelectionClick(point);
				}
				// keys
				if(!blockKeys)
				{
					if(ControlHandler.IsPressed(this.nextTargetKey))
					{
						this.SetPartyTarget(DataHolder.BattleSystem().GetEnemyOffset(
								this.partyTarget, 1, this.onlyInBattleRange));
					}
					else if(ControlHandler.IsPressed(this.previousTargetKey))
					{
						this.SetPartyTarget(DataHolder.BattleSystem().GetEnemyOffset(
								this.partyTarget, -1, this.onlyInBattleRange));
					}
					else if(ControlHandler.IsPressed(this.nearestTargetKey))
					{
						this.SetPartyTarget(DataHolder.BattleSystem().GetNearestEnemy(
								GameHandler.Party().GetPlayerCharacter()));
					}
					else if(!this.autoSelectTarget && ControlHandler.IsPressed(this.clearTargetKey))
					{
						this.SetPartyTarget(null);
					}
				}
				
				if(this.autoSelectTarget && 
					(this.partyTarget == null || this.partyTarget.isDead || 
					this.partyTarget.prefabInstance == null))
				{
					this.SetPartyTarget(DataHolder.BattleSystem().GetNearestEnemy(
							GameHandler.Party().GetPlayerCharacter()));
				}
				
				// cursor
				if(this.useTargetCursor && "" != this.cursorPrefabName && 
					this.partyTarget != null && !this.partyTarget.isDead && 
					this.partyTarget.prefabInstance != null)
				{
					if(this.cursorInstance == null)
					{
						GameObject tmp = (GameObject)Resources.Load(BattleSystemData.PREFAB_PATH+
								this.cursorPrefabName, typeof(GameObject));
						if(tmp) this.cursorInstance = (GameObject)GameObject.Instantiate(tmp);
					}
					this.cursorInstance.transform.position = this.ptObject.position+this.cursorOffset;
				}
				else if(this.cursorInstance != null)
				{
					GameObject.Destroy(this.cursorInstance);
				}
			}
		}
	}
	
	private void TargetSelectionClick(Vector3 point)
	{
		bool found = false;
		Ray ray = Camera.main.ScreenPointToRay(point);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit))
		{
			CombatantClick click = hit.transform.root.transform.GetComponentInChildren<CombatantClick>();
			if(click != null && click.combatant != null && click.combatant is Enemy)
			{
				this.SetPartyTarget(click.combatant);
				found = true;
			}
		}
		if(!found && this.allowTargetRemove)
		{
			this.SetPartyTarget(null);
		}
	}
	
	public bool IsAutoUseOnTarget()
	{
		bool isSet = false;
		if(GameHandler.IsInBattleArea() && this.autoUseOnTarget && 
			DataHolder.BattleSystem().IsRealTime() && 
			this.partyTarget != null && !this.partyTarget.isDead && 
			this.partyTarget.prefabInstance != null)
		{
			isSet = true;
		}
		return isSet;
	}
	
	public bool UseAutoAttackTarget(Combatant c)
	{
		bool use = false;
		if(GameHandler.IsInBattleArea() && 
			DataHolder.BattleSystem().IsRealTime() &&
			this.autoAttackTarget && 
			this.partyTarget != null && !this.partyTarget.isDead && 
			this.partyTarget.prefabInstance != null &&
			(!this.aaPlayerOnly || c == GameHandler.Party().GetPlayerCharacter()))
		{
			use = true;
		}
		return use;
	}
	
	public bool HasPartyTarget()
	{
		return this.partyTarget != null && !this.partyTarget.isDead && 
				this.partyTarget.prefabInstance != null;
	}
	
	public int GetPartyTargetID()
	{
		int id = -2;
		if(this.HasPartyTarget())
		{
			id = this.partyTarget.battleID;
		}
		return id;
	}
	
	public void SetPartyTarget(Combatant c)
	{
		this.partyTarget = c;
		if(this.partyTarget == null)
		{
			this.ptObject = null;
		}
		else if(this.partyTarget.prefabInstance != null)
		{
			this.partyTarget.CheckAggressive(AggressiveType.SELECTION);
			this.ptObject = TransformHelper.GetChild(
					this.cursorChildName, this.partyTarget.prefabInstance.transform);
		}
	}
}
