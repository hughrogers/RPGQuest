
using UnityEngine;
using System.Collections;

public class ActiveBattleMenu
{
	public int bmPosition = 0;
	public string bmTitle = "";
	public ChoiceContent[] choices = new ChoiceContent[0];
	private int[] realID = new int[0];
	public bool listUpdated = false;
	
	private Character owner = null;
	
	public BattleMenuMode callMode = BattleMenuMode.BASE;
	private BattleMenuMode mode = BattleMenuMode.BASE;
	private BattleMenuItem[] list = new BattleMenuItem[0];
	
	private BattleMenuItem lastBMItem = null;
	private int blinkSet = -1;
	
	// ORK GUI
	private DialoguePosition baseDP;
	private DialoguePosition skillDP;
	
	private float lastUpdate = 0;
	
	private bool rayTarget = false;
	private BattleAction rayAction = null;
	
	public bool hide = false;
	
	public ActiveBattleMenu()
	{
		
	}
	
	public void SetOwner(Character c)
	{
		this.owner = c;
	}
	
	public void DeleteBattleTextures()
	{
		if(this.baseDP != null) this.baseDP.multiLabel.DeleteTextures();
		if(this.skillDP != null) this.skillDP.multiLabel.DeleteTextures();
	}
	
	public bool IsBaseMenu()
	{
		return BattleMenuMode.BASE.Equals(this.mode);
	}
	
	public bool IsSkillMenu()
	{
		return BattleMenuMode.SKILL.Equals(this.mode);
	}
	
	public bool IsItemMenu()
	{
		return BattleMenuMode.ITEM.Equals(this.mode);
	}
	
	public bool IsTargetMenu()
	{
		return BattleMenuMode.TARGET.Equals(this.mode);
	}
	
	/*
	============================================================================
	Call functions
	============================================================================
	*/
	public void CallMenu(BattleMenuMode m, BattleMenuItem bmi)
	{
		this.lastUpdate = Time.time;
		this.callMode = m;
		this.mode = m;
		this.Init(bmi);
	}
	
	public void Init(BattleMenuItem bmi)
	{
		this.bmTitle = this.owner.GetName();
		this.blinkSet = -1;
		if(BattleMenuMode.BASE.Equals(this.mode))
		{
			this.bmPosition = DataHolder.BattleMenu().dialoguePosition;
			this.list = DataHolder.BattleMenu().BattleMenuList(this.owner);
		}
		else if(BattleMenuMode.SKILL.Equals(this.mode))
		{
			this.bmPosition = DataHolder.BattleMenu().skillPosition;
			this.list = DataHolder.BattleMenu().GetSkillMenuList(-1, this.owner);
		}
		else if(BattleMenuMode.ITEM.Equals(this.mode))
		{
			this.bmPosition = DataHolder.BattleMenu().itemPosition;
			this.list = DataHolder.BattleMenu().GetItemMenuList(-1, this.owner);
		}
		else if(BattleMenuMode.TARGET.Equals(this.mode))
		{
			this.bmPosition = DataHolder.BattleMenu().targetPosition;
			if(BattleMenu.SKILL == bmi.type && 
				DataHolder.Skill(bmi.id).targetRaycast.NeedInteraction())
			{
				this.rayTarget = true;
				this.rayAction = new BattleAction(AttackSelection.SKILL, this.owner, 
							BattleAction.NONE, bmi.id, bmi.useLevel);
				this.rayAction.targetRaycast = DataHolder.Skill(bmi.id).targetRaycast;
				this.list = new BattleMenuItem[0];
				this.hide = true;
			}
			else if(BattleMenu.ITEM == bmi.type && 
				DataHolder.Item(bmi.id).targetRaycast.NeedInteraction())
			{
				this.rayTarget = true;
				this.rayAction = new BattleAction(AttackSelection.ITEM, this.owner, 
							BattleAction.NONE, bmi.id, bmi.useLevel);
				this.rayAction.targetRaycast = DataHolder.Item(bmi.id).targetRaycast;
				this.list = new BattleMenuItem[0];
				this.hide = true;
			}
			else this.list = DataHolder.BattleMenu().GetTargetMenuList(bmi, this.owner);
		}
		this.UpdateChoices(true);
	}
	
	public void Clear()
	{
		this.mode = BattleMenuMode.BASE;
		this.list = new BattleMenuItem[0];
		this.choices = new ChoiceContent[0];
		this.blinkSet = -1;
		this.lastBMItem = null;
	}
	
	public void Reset(int index)
	{
		// clear target selection
		if(this.list[this.realID[index]] != null && this.list[this.realID[index]].action != null)
		{
			this.list[this.realID[index]].action.BlinkTargets(false);
		}
		this.CallMenu(BattleMenuMode.BASE, null);
	}
	
	/*
	============================================================================
	Utility functions
	============================================================================
	*/
	public void ChangeSkillLevel(int index, int change)
	{
		if(BattleMenuMode.SKILL.Equals(this.mode) &&
			index >= 0 && index < this.realID.Length)
		{
			this.owner.ChangeSkillUseLevel(this.list[this.realID[index]].id, change);
			this.ReloadSkillMenu();
		}
	}
	
	public void ReloadSkillMenu()
	{
		this.owner.skillBlockChanged = false;
		if(BattleMenuMode.SKILL.Equals(this.mode))
		{
			this.list = DataHolder.BattleMenu().GetSkillMenuList(this.lastBMItem.id, this.owner);
			this.UpdateChoices(true);
		}
	}
	
	/*
	============================================================================
	Update functions
	============================================================================
	*/
	public void Tick()
	{
		if(this.rayTarget && this.rayAction != null)
		{
			this.rayAction.targetRaycast.Tick(this, this.owner.prefabInstance);
		}
		else if((Time.time-this.lastUpdate) > DataHolder.BattleMenu().updateInterval)
		{
			// check availability
			if(this.CheckList()) this.UpdateChoices(false);
			this.lastUpdate = Time.time;
		}
	}
	
	private bool CheckList()
	{
		bool update = false;
		for(int i=0; i<this.list.Length; i++)
		{
			if(this.list[i].Check(this.owner))
			{
				update = true;
			}
		}
		return update;
	}
	
	public void UpdateChoices(bool checkFirst)
	{
		if(this.list.Length > 0) this.hide = false;
		this.choices = new ChoiceContent[0];
		this.realID = new int[0];
		if(checkFirst) this.CheckList();
		int addIndex = 0;
		int newIndex = -1;
		for(int i=0; i<this.list.Length; i++)
		{
			if(this.list[i].add)
			{
				if(i == this.blinkSet) newIndex = addIndex;
				this.realID = ArrayHelper.Add(i, this.realID);
				if(this.list[i].type == BattleMenu.ITEM)
				{
					this.choices = ArrayHelper.Add(new ChoiceContent(this.list[i].content, 
							this.list[i].active, this.list[i].info), this.choices);
					this.choices[addIndex].SetDrag(DragType.ITEM, DragOrigin.BATTLE_MENU, this.list[i].id, 0);
					this.choices[addIndex].dragable = DataHolder.BattleMenu().enableDrag;
					this.choices[addIndex].doubleClick = DataHolder.BattleMenu().enableDoubleClick;
					this.choices[addIndex].characterID = this.owner.battleID;
				}
				else if(this.list[i].type == BattleMenu.SKILL)
				{
					this.choices = ArrayHelper.Add(new ChoiceContent(this.list[i].content, 
							this.list[i].active, this.list[i].info), this.choices);
					this.choices[addIndex].SetDrag(DragType.SKILL, DragOrigin.BATTLE_MENU, this.list[i].id, this.list[i].useLevel);
					this.choices[addIndex].dragable = DataHolder.BattleMenu().enableDrag;
					this.choices[addIndex].doubleClick = DataHolder.BattleMenu().enableDoubleClick;
					this.choices[addIndex].characterID = this.owner.battleID;
				}
				else
				{
					this.choices = ArrayHelper.Add(new ChoiceContent(this.list[i].content, 
							this.list[i].active), this.choices);
					if(BattleMenu.ATTACK == this.list[i].type && this.list[i].action == null)
					{
						this.choices[addIndex].SetDrag(DragType.ATTACK, DragOrigin.BATTLE_MENU, -1, 0);
						this.choices[addIndex].dragable = DataHolder.BattleMenu().enableDrag;
						this.choices[addIndex].doubleClick = DataHolder.BattleMenu().enableDoubleClick;
						this.choices[addIndex].characterID = this.owner.battleID;
					}
				}
				addIndex++;
			}
		}
		if(newIndex < 0) newIndex = 0;
		GameHandler.GetLevelHandler().SetBattleMenuIndex(newIndex, this.owner);
		DataHolder.BattleSystem().TargetSelectionOff();
		this.SetSelectedTarget(newIndex);
		this.lastUpdate = Time.time;
		this.listUpdated = true;
	}
	
	public void SetRayPoint(Vector3 point)
	{
		if(this.rayTarget && this.rayAction != null)
		{
			this.rayAction.rayTargetSet = true;
			this.rayAction.rayPoint = point;
			this.owner.EndBattleMenu(false);
			this.owner.AddAction(this.rayAction);
			this.rayAction = null;
			this.rayTarget = false;
		}
	}
	
	/*
	============================================================================
	Interaction functions
	============================================================================
	*/
	public void Select(int index)
	{
		BattleMenuItem prevItem = this.lastBMItem;
		this.lastBMItem = this.list[this.realID[index]];
		if(this.lastBMItem.isTarget)
		{
			if((this.lastBMItem.type == BattleMenu.ATTACK && this.owner.IsBlockAttack()) ||
					(this.lastBMItem.type == BattleMenu.SKILL && this.owner.IsBlockSkills()) ||
					(this.lastBMItem.type == BattleMenu.ITEM && this.owner.IsBlockItems()))
			{
				this.lastBMItem = null;
			}
			else if(this.lastBMItem.action == null)
			{
				if(this.lastBMItem.type == BattleMenu.SKILL && 
					!DataHolder.Skill(this.lastBMItem.id).CanUse(this.owner, this.lastBMItem.useLevel))
				{
					this.mode = BattleMenuMode.SKILL;
					this.list = DataHolder.BattleMenu().GetSkillMenuList(DataHolder.Skill(this.lastBMItem.id).skilltype, this.owner);
					this.bmPosition = DataHolder.BattleMenu().skillPosition;
					this.UpdateChoices(true);
					this.lastBMItem = new BattleMenuItem(new GUIContent(""), -1, BattleMenu.SKILL, false, "");
				}
				else
				{
					BattleMenuItem[] newList = DataHolder.BattleMenu().GetTargetMenuList(this.lastBMItem, this.owner);
					if((DataHolder.BattleMenu().addBack && newList.Length > 1) ||
						(!DataHolder.BattleMenu().addBack && newList.Length > 0))
					{
						this.mode = BattleMenuMode.TARGET;
						this.list = newList;
						this.bmPosition = DataHolder.BattleMenu().targetPosition;
						this.UpdateChoices(true);
					}
					else
					{
						this.lastBMItem = prevItem;
						GameHandler.GetLevelHandler().SetLastBMIndex();
					}
				}
			}
			else
			{
				BattleAction action = this.lastBMItem.action;
				action.BlinkTargets(false);
				if(action.targetRaycast.NeedInteraction())
				{
					this.rayTarget = true;
					this.rayAction = action;
					this.mode = BattleMenuMode.TARGET;
					this.list = new BattleMenuItem[0];
					this.UpdateChoices(true);
					this.hide = true;
				}
				else
				{
					if(action.targetRaycast.active)
					{
						action.rayTargetSet = true;
						action.rayPoint = action.targetRaycast.GetRayPoint(this.owner.prefabInstance, VectorHelper.GetScreenCenter());
					}
					this.owner.EndBattleMenu(false);
					this.owner.AddAction(action);
				}
			}
		}
		else
		{
			if(this.lastBMItem.type == BattleMenu.SKILL)
			{
				if(this.owner.IsBlockSkills())
				{
					this.lastBMItem = null;
				}
				else
				{
					this.mode = BattleMenuMode.SKILL;
					this.list = DataHolder.BattleMenu().GetSkillMenuList(this.lastBMItem.id, this.owner);
					this.bmPosition = DataHolder.BattleMenu().skillPosition;
					this.UpdateChoices(true);
				}
			}
			else if(this.lastBMItem.type == BattleMenu.ITEM)
			{
				if(this.owner.IsBlockItems())
				{
					this.lastBMItem = null;
				}
				else
				{
					this.mode = BattleMenuMode.ITEM;
					this.list = DataHolder.BattleMenu().GetItemMenuList(this.lastBMItem.id, this.owner);
					this.bmPosition = DataHolder.BattleMenu().itemPosition;
					this.UpdateChoices(true);
				}
			}
			else if(this.lastBMItem.type == BattleMenu.DEFEND)
			{
				if(this.owner.IsBlockDefend())
				{
					this.lastBMItem = null;
				}
				else
				{
					this.owner.EndBattleMenu(false);
					this.owner.AddAction(new BattleAction(AttackSelection.DEFEND, 
							this.owner, this.owner.battleID, -1, 0));
				}
			}
			else if(this.lastBMItem.type == BattleMenu.ESCAPE)
			{
				if(this.owner.IsBlockEscape() || !DataHolder.BattleSystem().canEscape)
				{
					this.lastBMItem = null;
				}
				else
				{
					this.owner.EndBattleMenu(false);
					this.owner.AddAction(new BattleAction(AttackSelection.ESCAPE, 
							this.owner, this.owner.battleID, -1, 0));
				}
			}
			else if(this.lastBMItem.type == BattleMenu.ENDTURN)
			{
				this.owner.EndTurn();
			}
			else if(this.lastBMItem.type == BattleMenu.BACK)
			{
				this.lastBMItem = prevItem;
				GameHandler.GetLevelHandler().BattleMenuBack(true);
			}
		}
	}
	
	public void Back()
	{
		DataHolder.BattleSystem().TargetSelectionOff();
		this.blinkSet = -1;
		if(this.lastBMItem == null || this.lastBMItem.type == BattleMenu.ATTACK || !this.lastBMItem.isTarget)
		{
			this.mode = BattleMenuMode.BASE;
			this.list = DataHolder.BattleMenu().BattleMenuList(this.owner);
			this.bmPosition = DataHolder.BattleMenu().dialoguePosition;
			this.UpdateChoices(true);
			this.lastBMItem = null;
		}
		else if(this.lastBMItem.type == BattleMenu.SKILL && this.lastBMItem.isTarget)
		{
			this.mode = BattleMenuMode.SKILL;
			this.list = DataHolder.BattleMenu().GetSkillMenuList(DataHolder.Skill(this.lastBMItem.id).skilltype, this.owner);
			this.bmPosition = DataHolder.BattleMenu().skillPosition;
			this.UpdateChoices(true);
			this.lastBMItem = new BattleMenuItem(new GUIContent(""), -1, BattleMenu.SKILL, false, "");
		}
		else if(this.lastBMItem.type == BattleMenu.ITEM && this.lastBMItem.isTarget)
		{
			this.mode = BattleMenuMode.ITEM;
			this.list = DataHolder.BattleMenu().GetItemMenuList(DataHolder.Item(this.lastBMItem.id).itemType, this.owner);
			this.bmPosition = DataHolder.BattleMenu().itemPosition;
			this.UpdateChoices(true);
			this.lastBMItem = new BattleMenuItem(new GUIContent(""), -1, BattleMenu.ITEM, false, "");
		}
	}
	
	public void SetSelectedTarget(int index)
	{
		if(this.list != null && this.realID.Length > 0 && 
			this.list[this.realID[index]].isTarget && this.list[this.realID[index]].action != null)
		{
			this.list[this.realID[index]].action.BlinkTargets(true);
			this.blinkSet = this.realID[index];
		}
		else if(this.list != null && this.list.Length > this.blinkSet && this.blinkSet >= 0 &&
			this.list[this.blinkSet] != null && this.list[this.blinkSet].action != null)
		{
			this.list[this.blinkSet].action.BlinkTargets(false);
			this.blinkSet = -1;
		}
	}
	
	public void TargetClicked(int battleID)
	{
		bool found = false;
		for(int i=0; i<this.list.Length; i++)
		{
			if(this.list[i].action != null && this.list[i].action.targetID == battleID)
			{
				this.lastBMItem = this.list[i];
				found = true;
				break;
			}
		}
		if(found)
		{
			DataHolder.GameSettings().PlayAcceptAudio(GameHandler.GetLevelHandler().audio);
			BattleAction action = this.lastBMItem.action;
			action.BlinkTargets(false);
			this.owner.EndBattleMenu(false);
			this.owner.AddAction(action);
		}
	}
	
	/*
	============================================================================
	ORK GUI functions
	============================================================================
	*/
	public void StoreMenu(DialoguePosition dp)
	{
		if(BattleMenuMode.BASE.Equals(this.mode)) this.baseDP = dp;
		else if(BattleMenuMode.SKILL.Equals(this.mode)) this.skillDP = dp;
		else if(BattleMenuMode.ITEM.Equals(this.mode)) DataHolder.BattleMenu().itemDP = dp;
		else if(BattleMenuMode.TARGET.Equals(this.mode)) DataHolder.BattleMenu().targetDP = dp;
	}
	
	public DialoguePosition GetStoredMenu()
	{
		DialoguePosition dp = null;
		if(BattleMenuMode.BASE.Equals(this.mode)) dp = this.baseDP;
		else if(BattleMenuMode.SKILL.Equals(this.mode)) dp = this.skillDP;
		else if(BattleMenuMode.ITEM.Equals(this.mode)) dp = DataHolder.BattleMenu().itemDP;
		else if(BattleMenuMode.TARGET.Equals(this.mode)) dp = DataHolder.BattleMenu().targetDP;
		return dp;
	}
}
