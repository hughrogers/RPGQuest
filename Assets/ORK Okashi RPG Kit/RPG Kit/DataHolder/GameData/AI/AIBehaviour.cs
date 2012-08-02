
using UnityEngine;
using System.Collections;

public class AIBehaviour
{
	public int battleAI = 0;
	public int difficultyID = 0;
	
	public AttackSelection[] attackSelection = new AttackSelection[] {AttackSelection.ATTACK};
	public int[] useID = new int[] {0};
	public int[] useLevel = new int[] {1};
	public int[] actionDifficultyID = new int[] {0};
	
	public static string ACTION = "action";
	
	public AIBehaviour()
	{
		
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		ht.Add("battleai", this.battleAI.ToString());
		ht.Add("difficulty", this.difficultyID.ToString());
		if(this.attackSelection.Length > 0)
		{
			ht.Add("actions", this.attackSelection.Length.ToString());
			ArrayList s = new ArrayList();
			for(int i=0; i<this.attackSelection.Length; i++)
			{
				Hashtable ht2 = HashtableHelper.GetTitleHashtable(AIBehaviour.ACTION, i);
				ht2.Add("attackselection", this.attackSelection[i].ToString());
				ht2.Add("useid", this.useID[i].ToString());
				ht2.Add("lvl", this.useLevel[i].ToString());
				ht2.Add("difficulty", this.actionDifficultyID[i].ToString());
				s.Add(ht2);
			}
			ht.Add(XMLHandler.NODES, s);
		}
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		this.battleAI = int.Parse((string)ht["battleai"]);
		if(ht.ContainsKey("difficulty"))
		{
			this.difficultyID = int.Parse((string)ht["difficulty"]);
		}
		
		if(ht.ContainsKey("actions"))
		{
			int count = int.Parse((string)ht["actions"]);
			this.attackSelection = new AttackSelection[count];
			this.useID = new int[count];
			this.useLevel = new int[count];
			this.actionDifficultyID = new int[count];
			
			ArrayList s = ht[XMLHandler.NODES] as ArrayList;
			foreach(Hashtable ht2 in s)
			{
				if(ht2[XMLHandler.NODE_NAME] as string == AIBehaviour.ACTION)
				{
					int id = int.Parse((string)ht2["id"]);
					if(id < this.attackSelection.Length)
					{
						this.attackSelection[id] = (AttackSelection)System.Enum.Parse(
								typeof(AttackSelection), (string)ht2["attackselection"]);
						this.useID[id] = int.Parse((string)ht2["useid"]);
						this.useLevel[id] = int.Parse((string)ht2["lvl"]);
						if(ht2.ContainsKey("difficulty"))
						{
							this.actionDifficultyID[id] = int.Parse((string)ht2["difficulty"]);
						}
					}
				}
			}
		}
	}
	
	/*
	============================================================================
	Editor functions
	============================================================================
	*/
	public void AddAction()
	{
		this.attackSelection = ArrayHelper.Add(AttackSelection.ATTACK, this.attackSelection);
		this.useID = ArrayHelper.Add(0, this.useID);
		this.useLevel = ArrayHelper.Add(1, this.useLevel);
		this.actionDifficultyID = ArrayHelper.Add(0, this.actionDifficultyID);
	}
	
	public void RemoveAction(int index)
	{
		if(this.attackSelection.Length > 0)
		{
			this.attackSelection = ArrayHelper.Remove(index, this.attackSelection);
			this.useID = ArrayHelper.Remove(index, this.useID);
			this.useLevel = ArrayHelper.Remove(index, this.useLevel);
			this.actionDifficultyID = ArrayHelper.Remove(index, this.actionDifficultyID);
		}
	}
	
	public void CopyAction(int index)
	{
		this.attackSelection = ArrayHelper.Add(this.attackSelection[index], this.attackSelection);
		this.useID = ArrayHelper.Add(this.useID[index], this.useID);
		this.useLevel = ArrayHelper.Add(this.useLevel[index], this.useLevel);
		this.actionDifficultyID = ArrayHelper.Add(this.actionDifficultyID[index], this.actionDifficultyID);
	}
	
	public void RemoveDifficulty(int index)
	{
		if(this.difficultyID == index) this.difficultyID = 0;
		else if(this.difficultyID > index) this.difficultyID--;
		for(int i=0; i<this.actionDifficultyID.Length; i++)
		{
			if(this.actionDifficultyID[i] == index) this.actionDifficultyID[i] = 0;
			else if(this.actionDifficultyID[i] > index) this.actionDifficultyID[i]--;
		}
	}
	
	/*
	============================================================================
	Action handling functions
	============================================================================
	*/
	
	public BattleAction GetAction(Combatant c, int valid, Combatant[] allies, Combatant[] enemies)
	{
		BattleAction action = new BattleAction();
		bool found = false;
		bool targetEnemy = false;
		int difID = GameHandler.GetDifficulty();
		if(this.difficultyID <= difID)
		{
			for(int i=0; i<this.attackSelection.Length; i++)
			{
				if(this.actionDifficultyID[i] <= difID)
				{
					if(valid == -2 && c.attackPartyTarget && DataHolder.BattleControl().HasPartyTarget())
					{
						action.targetID = DataHolder.BattleControl().GetPartyTargetID();
					}
					else if(valid == -2 && c.attackLastTarget && c.lastTargetBattleID >= 0 &&
						DataHolder.BattleSystem().CheckForID(c.lastTargetBattleID, enemies, true))
					{
						action.targetID = c.lastTargetBattleID;
					}
					else
					{
						action.targetID = valid;
					}
					action.type = this.attackSelection[i];
					// set up base attack
					if(action.IsAttack() && !c.IsBlockAttack())
					{
						targetEnemy = true;
						if(valid == -2)
						{
							if(c.aiNearestTarget)
							{
								action.targetID = c.GetNearestTarget(enemies);
							}
							else
							{
								action.targetID = c.GetRandomTarget(enemies);
							}
						}
						found = true;
					}
					// set up skill
					else if(action.IsSkill() && !c.IsBlockSkills() && 
						c.HasSkill(this.useID[i], this.useLevel[i]) && 
						DataHolder.Skill(this.useID[i]).CanUse(c, this.useLevel[i]-1))
					{
						action.useID = this.useID[i];
						action.useLevel = this.useLevel[i]-1;
						if(DataHolder.Skill(action.useID).TargetSelf())
						{
							action.targetID = c.battleID;
						}
						else if(DataHolder.Skill(action.useID).TargetNone())
						{
							action.targetID = BattleAction.NONE;
							if(DataHolder.Skill(action.useID).targetRaycast.active)
							{
								GameObject target = null;
								if(c.aiNearestTarget)
								{
									if(DataHolder.Skill(action.useID).TargetEnemy())
									{
										target = c.GetNearestTargetObject(enemies);
									}
									else if(DataHolder.Skill(action.useID).TargetAlly())
									{
										target = c.GetNearestTargetObject(allies);
									}
								}
								else
								{
									if(DataHolder.Skill(action.useID).TargetEnemy())
									{
										target = c.GetRandomTargetObject(enemies);
									}
									else if(DataHolder.Skill(action.useID).TargetAlly())
									{
										target = c.GetRandomTargetObject(allies);
									}
								}
								if(target != null)
								{
									action.rayTargetSet = true;
									action.rayPoint = DataHolder.Skill(action.useID).targetRaycast.GetAIPoint(
											c.prefabInstance, target);
								}
							}
						}
						else if(DataHolder.Skill(action.useID).TargetAllyGroup() && c is Enemy)
						{
							action.targetID = BattleAction.ALL_ENEMIES;
						}
						else if(DataHolder.Skill(action.useID).TargetAllyGroup() && c is Character)
						{
							action.targetID = BattleAction.ALL_CHARACTERS;
						}
						else if(DataHolder.Skill(action.useID).TargetEnemyGroup() && c is Enemy)
						{
							action.targetID = BattleAction.ALL_CHARACTERS;
							targetEnemy = true;
						}
						else if(DataHolder.Skill(action.useID).TargetEnemyGroup() && c is Character)
						{
							action.targetID = BattleAction.ALL_ENEMIES;
							targetEnemy = true;
						}
						else if(DataHolder.Skill(action.useID).TargetSingleAlly())
						{
							if(valid == -2 || !DataHolder.BattleSystem().CheckForID(valid, allies, 
								!DataHolder.Skill(action.useID).level[action.useLevel].revive))
							{
								if(c.aiNearestTarget)
								{
									action.targetID = c.GetNearestTarget(allies);
								}
								else
								{
									action.targetID = c.GetRandomTarget(allies);
								}
							}
						}
						else if(DataHolder.Skill(action.useID).TargetSingleEnemy())
						{
							if(valid == -2 || !DataHolder.BattleSystem().CheckForID(valid, enemies, true))
							{
								if(c.aiNearestTarget)
								{
									action.targetID = c.GetNearestTarget(enemies);
								}
								else
								{
									action.targetID = c.GetRandomTarget(enemies);
								}
							}
							targetEnemy = true;
						}
						found = true;
					}
					// set up item
					else if(action.IsItem() && !c.IsBlockItems() &&
						(!(c is Character) || GameHandler.HasItem(this.useID[i], 1)))
					{
						action.useID = this.useID[i];
						if(DataHolder.Item(action.useID).TargetSelf())
						{
							action.targetID = c.battleID;
						}
						else if(DataHolder.Item(action.useID).TargetNone())
						{
							action.targetID = BattleAction.NONE;
							if(DataHolder.Item(action.useID).targetRaycast.active)
							{
								GameObject target = null;
								if(c.aiNearestTarget)
								{
									if(DataHolder.Item(action.useID).TargetEnemy())
									{
										target = c.GetNearestTargetObject(enemies);
									}
									else if(DataHolder.Item(action.useID).TargetAlly())
									{
										target = c.GetNearestTargetObject(allies);
									}
								}
								else
								{
									if(DataHolder.Item(action.useID).TargetEnemy())
									{
										target = c.GetRandomTargetObject(enemies);
									}
									else if(DataHolder.Item(action.useID).TargetAlly())
									{
										target = c.GetRandomTargetObject(allies);
									}
								}
								if(target != null)
								{
									action.rayTargetSet = true;
									action.rayPoint = DataHolder.Item(action.useID).targetRaycast.GetAIPoint(
											c.prefabInstance, target);
								}
							}
						}
						else if(DataHolder.Item(action.useID).TargetAllyGroup() && c is Enemy)
						{
							action.targetID = BattleAction.ALL_ENEMIES;
						}
						else if(DataHolder.Item(action.useID).TargetAllyGroup() && c is Character)
						{
							action.targetID = BattleAction.ALL_CHARACTERS;
						}
						else if(DataHolder.Item(action.useID).TargetEnemyGroup() && c is Enemy)
						{
							action.targetID = BattleAction.ALL_CHARACTERS;
							targetEnemy = true;
						}
						else if(DataHolder.Item(action.useID).TargetEnemyGroup() && c is Character)
						{
							action.targetID = BattleAction.ALL_ENEMIES;
							targetEnemy = true;
						}
						else if(DataHolder.Item(action.useID).TargetSingleAlly())
						{
							if(valid == -2 || !DataHolder.BattleSystem().CheckForID(valid, allies, 
								!DataHolder.Item(action.useID).revive))
							{
								if(DataHolder.BattleSystem().IsRealTime())
								{
									action.targetID = c.GetNearestTarget(allies);
								}
								else
								{
									action.targetID = c.GetRandomTarget(allies);
								}
							}
						}
						else if(DataHolder.Item(action.useID).TargetSingleEnemy())
						{
							if(valid == -2 || !DataHolder.BattleSystem().CheckForID(valid, enemies, true))
							{
								if(DataHolder.BattleSystem().IsRealTime())
								{
									action.targetID = c.GetNearestTarget(enemies);
								}
								else
								{
									action.targetID = c.GetRandomTarget(enemies);
								}
							}
							targetEnemy = true;
						}
						found = true;
					}
					else if(action.IsDefend() && !c.IsBlockDefend())
					{
						action.targetID = c.battleID;
						found = true;
					}
					// set up escape
					else if(action.IsEscape() && !c.IsBlockEscape())
					{
						action.targetID = c.battleID;
						found = true;
					}
					else if(action.IsDeath() || action.IsNone())
					{
						action.targetID = c.battleID;
						found = true;
					}
					
					// check active time
					if(found)
					{
						if((DataHolder.BattleSystem().IsActiveTime() && 
							c.usedTimeBar+action.GetTimeUse() > DataHolder.BattleSystem().maxTimebar) ||
							(targetEnemy && enemies.Length == 0))
						{
							found = false;
							targetEnemy = false;
							action = new BattleAction();
						}
						else break;
					}
				}
			}
		}
		
		if(!found) action = null;
		return action;
	}
}
