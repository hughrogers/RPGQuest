
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem
{
	public BattleSystemType type = BattleSystemType.TURN;
	public EnemyCounting enemyCounting = EnemyCounting.NONE;
	
	public int turnCalc = 0;
	public int defendFormula = 0;
	public int escapeFormula = 0;
	
	public bool turnBonuses = false;
	public int[] statusBonus;
	
	public bool reviveAfterBattle = false;
	public bool[] reviveSetStatus;
	public int[] reviveStatus;
	
	public bool startBattleStatusSettings = false;
	public bool[] startSetStatus;
	public int[] startStatus;
	
	public bool endBattleStatusSettings = false;
	public bool[] endSetStatus;
	public int[] endStatus;
	
	// dynamic combat
	public bool dynamicCombat = false;
	public float minTimeBetween = 0;
	public bool playDamageAnim = false;
	public bool blockAutoAttackMenu = false;
	
	// default battle spots
	public bool spotOnGround = false;
	public float spotDistance = 100.0f;
	public int spotLayer = 1;
	public Vector3 spotOffset = Vector3.zero;
	
	public bool enablePASpots = false;
	public bool enableEASpots = false;
	
	public Vector3[] partySpot = new Vector3[DataHolder.GameSettings().maxBattleParty];
	public Vector3[] partySpotPA = new Vector3[DataHolder.GameSettings().maxBattleParty];
	public Vector3[] partySpotEA = new Vector3[DataHolder.GameSettings().maxBattleParty];
	public Vector3[] enemySpot = new Vector3[3];
	public Vector3[] enemySpotPA = new Vector3[3];
	public Vector3[] enemySpotEA = new Vector3[3];
	
	// battle advantages
	public int battleAdvantage = -1;
	public BattleAdvantage partyAdvantage = new BattleAdvantage();
	public BattleAdvantage enemiesAdvantage = new BattleAdvantage();
	public int firstMoveRounds = 1;
	
	// turn based
	public bool activeCommand = true;
	
	// active time
	public int menuBorder = 1000;
	public int actionBorder = 1000;
	public int maxTimebar = 1000;
	
	public bool enableMultiChoice = false;
	public bool useAllActions = false;
	public UseTimebarAction useTimebarAction = UseTimebarAction.ACTION_BORDER;
	
	public bool actionPause = true;
	public float atbTickInterval = 0.05f;
	public int startTimeCalc = 0;
	public bool attackEndTurn = true;
	public float attackTimebarUse = 1000;
	public bool itemEndTurn = true;
	public float itemTimebarUse = 1000;
	public bool defendEndTurn = true;
	public float defendTimebarUse = 1000;
	public bool escapeEndTurn = true;
	public float escapeTimebarUse = 1000;
	
	// ingame
	public Enemy[] enemies = new Enemy[0];
	private int orderIndex = 0;
	private Combatant[] actionOrder = new Combatant[0];
	private ArrayList actionStack = new ArrayList();
	
	// update
	private float time = 0;
	private int currentTick = 0;
	private bool timeRunning = false;
	public BattleAction[] activeBattleAction = new BattleAction[0];
	// test timers
	private bool nextCheck = false;
	private bool actionCheck = false;
	
	public bool battleRunning = false;
	public bool battleEnd = false;
	public bool canEscape = true;
	
	// victory gains
	private Hashtable itemGains;
	private Hashtable weaponGains;
	private Hashtable armorGains;
	private Hashtable expGains;
	private int moneyGain = 0;
	
	// battle arena
	private BattleArena battleArena;
	private GameObject groupCenter;
	private GameObject combatantCenter;
	
	// real time
	public float battleRange = 20.0f;
	public float aiRange = 100.0f;
	public float aiRecheckTime = 4.0f;
	public bool blockControlMenu = false;
	public bool blockControlAction = false;
	public bool blockMSE = false;
	public bool freezeAction = false;
	
	private bool showingGains = false;
	
	private int latestBattleID = 0;
	
	public BattleSystem()
	{
		
	}
	
	public bool IsTurnBased()
	{
		return BattleSystemType.TURN.Equals(this.type);
	}
	
	public bool IsActiveTime()
	{
		return BattleSystemType.ACTIVE.Equals(this.type);
	}
	
	public bool IsRealTime()
	{
		return BattleSystemType.REALTIME.Equals(this.type);
	}
	
	public bool IsBattleRunning()
	{
		return this.battleRunning && !this.battleEnd;
	}
	
	public bool CanUseSkillCasting()
	{
		return this.IsRealTime() || this.IsActiveTime() || this.dynamicCombat;
	}
	
	/*
	============================================================================
	Clearing functions
	============================================================================
	*/
	public void ClearBattle(bool clearEnemies)
	{
		GameHandler.ClearInBattleArea();
		this.EndBattle();
		for(int i=0; i<this.activeBattleAction.Length; i++)
		{
			this.activeBattleAction[i].StopAction();
		}
		Character[] party = GameHandler.Party().GetBattleParty();
		for(int i=0; i<party.Length; i++) party[i].EndBattle();
		if(clearEnemies) this.enemies = new Enemy[0];
		this.showingGains = false;
	}
	
	/*
	============================================================================
	Start functions
	============================================================================
	*/
	public void SetBattleArena(BattleArena arena)
	{
		this.battleArena = arena;
		this.battleAdvantage = BattleAdvantage.NONE;
		this.firstMoveRounds = 1;
		
		float rnd = DataHolder.GameSettings().GetRandom();
		
		float paChance = this.partyAdvantage.chance;
		if(this.battleArena.overridePAChance) paChance = this.battleArena.paChance;
		
		float eaChance = this.enemiesAdvantage.chance;
		if(this.battleArena.overrideEAChance) eaChance = this.battleArena.eaChance;
		
		if(this.partyAdvantage.enabled && this.battleArena.enablePartyAdvantage && 
			paChance > 0 && rnd <= paChance)
		{
			this.battleAdvantage = BattleAdvantage.PARTY;
		}
		else if(this.enemiesAdvantage.enabled && this.battleArena.enableEnemiesAdvantage && 
			eaChance > 0 && rnd >= DataHolder.GameSettings().maxRandomRange-eaChance)
		{
			this.battleAdvantage = BattleAdvantage.ENEMIES;
		}
	}
	
	public void AddEnemy(Enemy e)
	{
		if(this.battleRunning)
		{
			e.StartBattle(this.latestBattleID++);
			if(this.IsActiveTime())
			{
				e.timeBar = DataHolder.Formula(this.startTimeCalc).Calculate(
						e, e);
				if(e.timeBar > this.maxTimebar) e.timeBar = this.maxTimebar;
			}
			if(this.startBattleStatusSettings)
			{
				for(int j=0; j<this.startSetStatus.Length; j++)
				{
					if(this.startSetStatus[j])
					{
						e.status[j].SetValue(this.startStatus[j], true, false, false);
					}
				}
			}
		}
		this.enemies = ArrayHelper.Add(e, this.enemies);
	}
	
	public void SetupBattle(Character[] party, Enemy[] es, bool ce)
	{
		this.latestBattleID = 0;
		this.canEscape = ce;
		this.itemGains = new Hashtable();
		this.weaponGains = new Hashtable();
		this.armorGains = new Hashtable();
		this.expGains = new Hashtable();
		this.moneyGain = 0;
		this.battleEnd = false;
		this.nextCheck = false;
		this.actionCheck = false;
		
		if(this.battleArena != null)
		{
			this.moneyGain += this.battleArena.moneyGain;
			// additional item gains
			for(int i=0; i<this.battleArena.gainType.Length; i++)
			{
				if(DataHolder.GameSettings().GetRandom() <= this.battleArena.gainChance[i])
				{
					if(ItemDropType.ITEM.Equals(this.battleArena.gainType[i]))
					{
						this.AddGain(this.battleArena.gainQuantity[i], this.battleArena.gainID[i], this.itemGains);
					}
					else if(ItemDropType.WEAPON.Equals(this.battleArena.gainType[i]))
					{
						this.AddGain(this.battleArena.gainQuantity[i], this.battleArena.gainID[i], this.weaponGains);
					}
					else if(ItemDropType.ARMOR.Equals(this.battleArena.gainType[i]))
					{
						this.AddGain(this.battleArena.gainQuantity[i], this.battleArena.gainID[i], this.armorGains);
					}
				}
			}
		}
		
		if(this.IsRealTime()) this.battleAdvantage = BattleAdvantage.NONE;
		
		if(es != null && es.Length > 0) this.enemies = es;
		
		if((this.battleAdvantage == BattleAdvantage.PARTY && this.partyAdvantage.blockEscape) ||
			(this.battleAdvantage == BattleAdvantage.ENEMIES && this.enemiesAdvantage.blockEscape))
		{
			this.canEscape = false;
		}
		
		if(this.IsTurnBased())
		{
			if(this.battleAdvantage == BattleAdvantage.PARTY)
			{
				if(this.partyAdvantage.partyCondition.firstMove)
				{
					this.firstMoveRounds = this.partyAdvantage.partyCondition.firstMoveRounds;
				}
				else if(this.partyAdvantage.enemiesCondition.firstMove)
				{
					this.firstMoveRounds = this.partyAdvantage.enemiesCondition.firstMoveRounds;
				}
			}
			else if(this.battleAdvantage == BattleAdvantage.ENEMIES)
			{
				if(this.enemiesAdvantage.partyCondition.firstMove)
				{
					this.firstMoveRounds = this.enemiesAdvantage.partyCondition.firstMoveRounds;
				}
				else if(this.enemiesAdvantage.enemiesCondition.firstMove)
				{
					this.firstMoveRounds = this.enemiesAdvantage.enemiesCondition.firstMoveRounds;
				}
			}
		}
		
		for(int i=0; i<party.Length; i++)
		{
			party[i].StartBattle(this.latestBattleID++);
			
			if(this.battleAdvantage == BattleAdvantage.PARTY)
			{
				this.partyAdvantage.partyCondition.ApplyCondition(party[i]);
			}
			else if(this.battleAdvantage == BattleAdvantage.ENEMIES)
			{
				this.enemiesAdvantage.partyCondition.ApplyCondition(party[i]);
			}
			else
			{
				if(this.IsActiveTime())
				{
					party[i].timeBar = DataHolder.Formula(this.startTimeCalc).Calculate(
							party[i], party[i]);
					if(party[i].timeBar > this.maxTimebar) party[i].timeBar = this.maxTimebar;
				}
				if(this.startBattleStatusSettings)
				{
					for(int j=0; j<this.startSetStatus.Length; j++)
					{
						if(this.startSetStatus[j])
						{
							party[i].status[j].SetValue(this.startStatus[j], true, false, false);
						}
					}
				}
			}
		}
		
		Dictionary<int, int> counter = new Dictionary<int, int>();
		Dictionary<int, int> counter2 = new Dictionary<int, int>();
		if(!EnemyCounting.NONE.Equals(this.enemyCounting))
		{
			for(int i=0; i<this.enemies.Length; i++)
			{
				if(counter.ContainsKey(this.enemies[i].realID))
				{
					counter[this.enemies[i].realID]++;
				}
				else
				{
					counter.Add(this.enemies[i].realID, 1);
					counter2.Add(this.enemies[i].realID, 0);
				}
			}
		}
		
		for(int i=0; i<this.enemies.Length; i++)
		{
			if(!EnemyCounting.NONE.Equals(this.enemyCounting) && 
				counter[this.enemies[i].realID] > 1)
			{
				this.enemies[i].nameCount = this.GetEnemyCounting(counter2[this.enemies[i].realID]++);
			}
			
			this.enemies[i].StartBattle(this.latestBattleID++);
			
			if(this.battleAdvantage == BattleAdvantage.PARTY)
			{
				this.partyAdvantage.enemiesCondition.ApplyCondition(this.enemies[i]);
			}
			else if(this.battleAdvantage == BattleAdvantage.ENEMIES)
			{
				this.enemiesAdvantage.enemiesCondition.ApplyCondition(this.enemies[i]);
			}
			else
			{
				if(this.IsActiveTime())
				{
					this.enemies[i].timeBar = DataHolder.Formula(this.startTimeCalc).Calculate(
							this.enemies[i], this.enemies[i]);
					if(this.enemies[i].timeBar > this.maxTimebar) this.enemies[i].timeBar = this.maxTimebar;
				}
				if(this.startBattleStatusSettings)
				{
					for(int j=0; j<this.startSetStatus.Length; j++)
					{
						if(this.startSetStatus[j])
						{
							this.enemies[i].status[j].SetValue(this.startStatus[j], true, false, false);
						}
					}
				}
			}
		}
		this.actionStack = new ArrayList();
		this.actionOrder = new Combatant[0];
		this.orderIndex = 0;
		this.activeBattleAction = new BattleAction[0];
		this.showingGains = false;
		GameHandler.GetLevelHandler().ClearBattleMenuUsers();
	}
	
	private string GetEnemyCounting(int count)
	{
		string txt = "";
		if(EnemyCounting.LETTERS.Equals(this.enemyCounting))
		{
			txt += char.ConvertFromUtf32(count+65);
		}
		else if(EnemyCounting.NUMBERS.Equals(this.enemyCounting))
		{
			txt = (count+1).ToString();
		}
		return txt;
	}
	
	public void StartBattle()
	{
		if(!this.IsRealTime()) DataHolder.Statistic.BattleStarted();
		this.battleRunning = true;
		if(this.battleArena != null)
		{
			DataHolder.BattleCam().StartBattle(this.battleArena);
		}
		this.StartTime();
		if(this.IsTurnBased())
		{
			this.StartTurn();
		}
	}
	
	private void StartTurn()
	{
		if(!this.battleEnd)
		{
			this.orderIndex = 0;
			if(!this.dynamicCombat) this.actionStack = new ArrayList();
			// determine the order of actions
			ArrayList order = new ArrayList();
			ArrayList partyOrder = null;
			ArrayList enemiesOrder = null;
			if(this.battleAdvantage != BattleAdvantage.NONE && this.firstMoveRounds > 0)
			{
				partyOrder = new ArrayList();
				enemiesOrder = new ArrayList();
			}
			
			Character[] party = GameHandler.Party().GetBattleParty();
			for(int i=0; i<party.Length; i++)
			{
				if(!party[i].isDead)
				{
					Hashtable tmp = new Hashtable();
					tmp.Add("id", party[i].battleID.ToString());
					if(party[i].turnPerformed)
					{
						tmp.Add("value", 
								DataHolder.Formulas().formula[this.turnCalc].Calculate(
								party[i], party[i]).ToString());
						party[i].turnPerformed = false;
					}
					else
					{
						tmp.Add("value", (0-this.actionOrder.Length+party[i].lastTurnIndex).ToString());
					}
					if(this.battleAdvantage == BattleAdvantage.NONE || 
						this.firstMoveRounds < 1)
					{
						order.Add(tmp);
					}
					else partyOrder.Add(tmp);
				}
			}
			for(int i=0; i<this.enemies.Length; i++)
			{
				if(!this.enemies[i].isDead)
				{
					Hashtable tmp = new Hashtable();
					tmp.Add("id", this.enemies[i].battleID.ToString());
					if(this.enemies[i].turnPerformed)
					{
						tmp.Add("value", 
								DataHolder.Formulas().formula[this.turnCalc].Calculate(
								this.enemies[i], this.enemies[i]).ToString());
						this.enemies[i].turnPerformed = false;
					}
					else
					{
						tmp.Add("value", (0-this.actionOrder.Length+this.enemies[i].lastTurnIndex).ToString());
					}
					if(this.battleAdvantage == BattleAdvantage.NONE || 
						this.firstMoveRounds < 1)
					{
						order.Add(tmp);
					}
					else enemiesOrder.Add(tmp);
				}
			}
			
			if(this.battleAdvantage == BattleAdvantage.NONE || 
				this.firstMoveRounds < 1)
			{
				order.Sort(new TurnSorter());
			}
			else
			{
				partyOrder.Sort(new TurnSorter());
				enemiesOrder.Sort(new TurnSorter());
				if(this.battleAdvantage == BattleAdvantage.PARTY)
				{
					for(int i=0; i<partyOrder.Count; i++) order.Add(partyOrder[i]);
					for(int i=0; i<enemiesOrder.Count; i++) order.Add(enemiesOrder[i]);
				}
				else if(this.battleAdvantage == BattleAdvantage.ENEMIES)
				{
					for(int i=0; i<enemiesOrder.Count; i++) order.Add(enemiesOrder[i]);
					for(int i=0; i<partyOrder.Count; i++) order.Add(partyOrder[i]);
				}
				this.firstMoveRounds--;
			}
			
			this.actionOrder = new Combatant[order.Count];
			for(int i=0; i<this.actionOrder.Length; i++)
			{
				Hashtable ht = (Hashtable)order[i];
				this.actionOrder[i] = this.GetCombatantForBattleID(int.Parse(ht["id"] as string));
				this.actionOrder[i].lastTurnIndex = i;
			}
			
			this.GetNextAction();
		}
	}
	
	public void OrderChange(int battleID, int change)
	{
		if(this.IsTurnBased() && !this.dynamicCombat)
		{
			for(int i=this.orderIndex; i<this.actionOrder.Length; i++)
			{
				if(this.actionOrder[i] != null && 
					this.actionOrder[i].battleID == battleID)
				{
					change += i;
					change = Mathf.Max(change, this.orderIndex);
					change = Mathf.Min(change, this.actionOrder.Length-1);
					this.actionOrder = ArrayHelper.Change(this.actionOrder[i], this.actionOrder, change);
					break;
				}
			}
		}
	}
	
	private void StartTime()
	{
		if(this.IsActiveTime())
		{
			this.timeRunning = true;
		}
	}
	
	private void StopTime()
	{
		if(this.IsActiveTime())
		{
			this.timeRunning = false;
		}
	}
	
	/*
	============================================================================
	Tick functions
	============================================================================
	*/
	public void Tick()
	{
		if(this.IsActiveTime() && this.timeRunning)
		{
			time += GameHandler.DeltaBattleTime;
			if(time > this.atbTickInterval)
			{
				time -= this.atbTickInterval;
				if(this.currentTick == 0) this.TimeTick();
				else if(this.currentTick == 1) this.ActionTick();
				this.currentTick++;
				if(this.currentTick > 1) this.currentTick = 0;
			}
		}
		for(int i=0; i<this.activeBattleAction.Length; i++)
		{
			this.activeBattleAction[i].Tick();
		}
		if(this.IsRealTime())
		{
			this.PerformNextAction3();
		}
	}
	
	public bool DoCombatantTick()
	{
		bool tick = true;
		if((this.IsActiveTime() && this.timeRunning && 
			this.actionPause && this.nextCheck) ||
			(this.IsRealTime() && this.actionPause && 
			GameHandler.GetLevelHandler().BattleMenuActive()))
		{
			tick = false;
		}
		return tick;
	}
	
	private void TimeTick()
	{
		if(!GameHandler.IsGamePaused() && !this.battleEnd && !(this.actionPause && this.nextCheck))
		{
			this.TickGroup(GameHandler.Party().GetBattleParty());
			this.TickGroup(this.enemies);
		}
	}
	
	private void TickGroup(Combatant[] group)
	{
		for(int i=0; i<group.Length; i++)
		{
			group[i].CheckDeath();
			if(!group[i].isDead && !group[i].IsStopMove() && 
				!group[i].IsFinishingAttackQueue())
			{
				if(group[i].timeBar < this.maxTimebar)
				{
					group[i].timeBar += DataHolder.Formula(this.turnCalc).Calculate(group[i], group[i]);
				}
				if(group[i].timeBar > this.maxTimebar) group[i].timeBar = this.maxTimebar;
				
				if(group[i].timeBar >= this.menuBorder && 
				   group[i].CanChooseAction())
				{
					if(group[i] is Character) this.nextCheck = true;
					group[i].ChooseAction();
				}
				else if(!group[i].IsChoosingAction() && !group[i].IsTurnEnded() && 
					group[i].usedTimeBar >= this.maxTimebar)
				{
					group[i].EndTurn();
				}
			}
		}
	}
	
	private void ActionTick()
	{
		if(!GameHandler.IsGamePaused() && !this.battleEnd && 
			(this.dynamicCombat || !this.actionCheck))
		{
			this.PerformNextAction();
		}
	}
	
	public void BattleMenuFinished()
	{
		this.nextCheck = false;
	}
	
	/*
	============================================================================
	Action handling functions
	============================================================================
	*/
	private void GetNextAction()
	{
		this.battleArena.StartCoroutine(this.GetNextAction2());
	}
	
	private IEnumerator GetNextAction2()
	{
		yield return null;
		if(!this.battleEnd)
		{
			if(this.orderIndex < this.actionOrder.Length)
			{
				bool added = false;
				if(this.actionOrder[this.orderIndex] != null && 
					this.actionOrder[this.orderIndex].CanChooseAction())
				{
					added = true;
					this.actionOrder[this.orderIndex++].ChooseAction();
				}
				if(!added)
				{
					this.orderIndex++;
					this.AddAction(null);
				}
			}
			else if(this.IsTurnBased())
			{
				this.StartTurn();
			}
		}
	}
	
	public void UnshiftAction(BattleAction action)
	{
		this.actionStack.Insert(0, action);
	}
	
	public void AddAction(BattleAction action)
	{
		if(!this.battleEnd)
		{
			if(action != null)
			{
				action.CheckTargetAggressive();
				this.actionStack.Add(action);
			}
			if(this.IsTurnBased())
			{
				if(this.activeCommand || this.orderIndex >= this.actionOrder.Length)
				{
					this.PerformNextAction();
				}
				else 
				{
					this.GetNextAction();
				}
			}
		}
	}
	
	private void PerformNextAction()
	{
		this.battleArena.StartCoroutine(this.PerformNextAction2());
	}
	
	private IEnumerator PerformNextAction2()
	{
		yield return null;
		this.PerformNextAction3();
	}
	
	private void PerformNextAction3()
	{
		if(!this.battleEnd)
		{
			if(this.actionStack.Count > 0)
			{
				this.actionCheck = true;
				this.CheckDeath();
				BattleAction action = null;
				for(int i=0; i<this.actionStack.Count; i++)
				{
					action = this.actionStack[i] as BattleAction;
					// remove action from dead user
					if(action.user == null || 
					   (action.user.isDead && !action.IsDeath()))
					{
						this.actionStack.RemoveAt(i--);
						action = null;
					}
					// action found
					else if(!action.user.IsInAction() && 
						(action.IsDeath() || action.IsCounter() || 
						action.user.AllowPerformingAction()))
					{
						if(this.IsActiveTime() && this.useAllActions && 
							!action.IsDeath() && !action.IsCounter())
						{
							action.user.FinishAttackQueue();
						}
						this.actionStack.RemoveAt(i);
						break;
					}
					else
					{
						action = null;
					}
				}
				if(action != null && !action.IsCastingSkill())
				{
					this.PerformAction(action);
				}
				else this.PerformAction(null);
			}
			else if(this.IsTurnBased() && this.activeCommand)
			{
				this.GetNextAction();
			}
		}
	}
	
	public void PerformAction(BattleAction action)
	{
		if(action != null)
		{
			action.PerformAction();
			if(this.dynamicCombat)
			{
				if(this.minTimeBetween > 0)
				{
					this.battleArena.StartCoroutine(this.ActionFinished2());
				}
				else this.ActionFinished();
			}
		}
		else this.ActionFinished();
	}
	
	public IEnumerator ActionFinished2()
	{
		yield return new WaitForSeconds(this.minTimeBetween);
		this.ActionFinished();
	}
	
	public void ActionFinished()
	{
		if(!this.battleEnd)
		{
			if(this.IsTurnBased())
			{
				if(this.actionStack.Count > 0)
				{
					this.PerformNextAction();
				}
				else if(this.activeCommand)
				{
					this.GetNextAction();
				}
				else
				{
					this.StartTurn();
				}
			}
			else if(this.IsActiveTime() && this.actionStack.Count > 0)
			{
				this.PerformNextAction();
			}
			else
			{
				this.actionCheck = false;
			}
		}
	}
	
	public void AddActiveAction(BattleAction ba)
	{
		this.activeBattleAction = ArrayHelper.Add(ba, this.activeBattleAction);
	}
	
	public void RemoveActiveAction(BattleAction ba)
	{
		this.activeBattleAction = ArrayHelper.Remove(ba, this.activeBattleAction);
		if(DataHolder.BattleCam().blockAnimationCams && this.activeBattleAction.Length == 0)
		{
			DataHolder.BattleCam().SetLatestUser(null);
		}
		if(this.IsTurnBased() && this.dynamicCombat && this.actionStack.Count > 0)
		{
			this.PerformNextAction();
		}
	}
	
	public bool IsLatestActiveAction(BattleAction ba)
	{
		bool latest = false;
		if(this.activeBattleAction.Length > 0 && 
			this.activeBattleAction[this.activeBattleAction.Length-1] == ba)
		{
			latest = true;
		}
		return latest;
	}
	
	/*
	============================================================================
	Target functions
	============================================================================
	*/
	public void TargetSelectionOff()
	{
		Character[] party = GameHandler.Party().GetBattleParty();
		for(int i=0; i<party.Length; i++)
		{
			party[i].Blink(false);
		}
		for(int i=0; i<this.enemies.Length; i++)
		{
			this.enemies[i].Blink(false);
		}
		if(DataHolder.BattleCam().blockAnimationCams)
		{
			DataHolder.BattleCam().SetSelection(null);
		}
	}
	
	public Combatant GetEnemyOffset(Combatant c, int add, bool inbr)
	{
		Combatant next = null;
		if(this.enemies != null && this.enemies.Length > 0)
		{
			Character player = GameHandler.Party().GetPlayerCharacter();
			UseRange ur = new UseRange();
			Combatant[] e = new Combatant[0];
			for(int i=0; i<this.enemies.Length; i++)
			{
				if(!this.enemies[i].isDead && 
					(!inbr || ur.InRange(player, this.enemies[i])))
				{
					e = ArrayHelper.Add(this.enemies[i], e);
				}
			}
			
			for(int i=0; i<e.Length; i++)
			{
				if(e[i] == c)
				{
					if(i+add < 0) next = e[e.Length-1];
					else if(i+add >= e.Length) next = e[0];
					else next = e[i+add];
				}
			}
			if(e.Length > 0 && next == null) next = e[0];
		}
		return next;
	}
	
	public Combatant GetNearestEnemy(Combatant c)
	{
		Combatant nearest = null;
		float distance = Mathf.Infinity;
		if(c != null && c.prefabInstance != null &&
			this.enemies != null && this.enemies.Length > 0)
		{
			for(int i=0; i<this.enemies.Length; i++)
			{
				if(this.enemies[i] != null && !this.enemies[i].isDead &&
					this.enemies[i].prefabInstance != null)
				{
					float nd = Vector3.Distance(c.prefabInstance.transform.position,
						this.enemies[i].prefabInstance.transform.position);
					if(nd < distance)
					{
						nearest = this.enemies[i];
						distance = nd;
					}
				}
			}
		}
		return nearest;
	}
	
	public Combatant GetCombatantForBattleID(int bID)
	{
		Character[] party = GameHandler.Party().GetBattleParty();
		for(int i=0; i<party.Length; i++)
		{
			if(party[i].battleID == bID)
			{
				return party[i];
			}
		}
		for(int i=0; i<this.enemies.Length; i++)
		{
			if(this.enemies[i].battleID == bID)
			{
				return this.enemies[i];
			}
		}
		return null;
	}
	
	public Combatant GetRandomCombatant(Combatant[] c)
	{
		ArrayList list = new ArrayList();
		for(int i=0; i<c.Length; i++)
		{
			if(c[i] != null && !c[i].isDead)
			{
				list.Add(c[i]);
			}
		}
		if(list.Count > 0) return list[Random.Range(0, list.Count)] as Combatant;
		else return null;
	}
	
	public Combatant GetRandomCharacter()
	{
		return this.GetRandomCombatant(GameHandler.Party().GetBattleParty());
	}
	
	public Combatant GetRandomEnemy()
	{
		return this.GetRandomCombatant(this.enemies);
	}
	
	public bool CheckForID(int id, Combatant[] cs, bool checkDeath)
	{
		bool valid = false;
		for(int i=0; i<cs.Length; i++)
		{
			if(cs[i] != null && cs[i].battleID == id &&
				(!checkDeath || !cs[i].isDead))
			{
				valid = true;
				break;
			}
		}
		return valid;
	}
	
	/*
	============================================================================
	Death functions
	============================================================================
	*/
	public void CheckDeath()
	{
		Character[] party = GameHandler.Party().GetBattleParty();
		for(int i=0; i<party.Length; i++) party[i].CheckDeath();
		for(int i=0; i<this.enemies.Length; i++) this.enemies[i].CheckDeath();
	}
	
	public void EnemyDefeated(int bID)
	{
		// exp+items
		Enemy e = this.GetCombatantForBattleID(bID) as Enemy;
		if(e != null)
		{
			DataHolder.Statistic.EnemyKilled(e.realID);
			// victory gains
			// items
			for(int i=0; i<e.itemDrop.Length; i++)
			{
				if(DataHolder.GameSettings().GetRandom() <= e.itemDrop[i].chance)
				{
					if(DataHolder.BattleEnd().dropItems && 
						e.prefabInstance != null)
					{
						e.itemDrop[i].Drop(e.prefabInstance.transform.position);
					}
					else
					{
						if(e.itemDrop[i].IsItem())
						{
							this.AddGain(e.itemDrop[i].quantity, e.itemDrop[i].itemID, this.itemGains);
						}
						else if(e.itemDrop[i].IsWeapon())
						{
							this.AddGain(e.itemDrop[i].quantity, e.itemDrop[i].itemID, this.weaponGains);
						}
						else if(e.itemDrop[i].IsArmor())
						{
							this.AddGain(e.itemDrop[i].quantity, e.itemDrop[i].itemID, this.armorGains);
						}
					}
				}
			}
			
			// money
			if(DataHolder.BattleEnd().dropMoney && 
				e.prefabInstance != null)
			{
				GameHandler.DropHandler().Drop(e.prefabInstance.transform.position, e.money);
			}
			else this.moneyGain += e.money;
			
			// experience
			for(int i=0; i<e.status.Length; i++)
			{
				if(e.status[i].IsExperience())
				{
					int exp = e.status[i].GetBaseValue();
					if(exp != 0)
					{
						if(DataHolder.BattleEnd().splitExp)
						{
							exp /= GameHandler.Party().GetBattlePartySize();
						}
						this.AddGain(exp, i, this.expGains);
					}
				}
			}
			// variables
			e.variables.SetVariables();
			
			// remove enemy
			if(DataHolder.BattleEnd().getImmediately)
			{
				this.VictoryGains();
			}
			this.RemoveEnemy(e);
		}
	}
	
	public void RemoveCharacter(Character c)
	{
		c.EndBattle();
		if(c.prefabInstance)
		{
			c.DestroyPrefab();
		}
		if(c.cursorInstance) GameObject.Destroy(c.cursorInstance);
		c = null;
		this.CheckBattleEnd();
	}
	
	public void RemoveEnemy(Enemy e)
	{
		this.enemies = ArrayHelper.Remove(e, this.enemies);
		e.EndBattle();
		if(e.prefabInstance)
		{
			e.DestroyPrefab();
		}
		if(e.cursorInstance) GameObject.Destroy(e.cursorInstance);
		e = null;
		this.CheckBattleEnd();
	}
	
	/*
	============================================================================
	Battle end functions
	============================================================================
	*/
	public static void AddGain(int n, int id, Hashtable ht)
	{
		int count = 0;
		if(ht.ContainsKey(id))
		{
			count = (int)ht[id];
			ht.Remove(id);
		}
		count += n;
		ht.Add(id, count);
	}
	
	public void CheckBattleEnd()
	{
		if(this.enemies.Length == 0)
		{
			if(!DataHolder.BattleEnd().getImmediately) this.BattleVictory();
		}
		else
		{
			Character[] party = GameHandler.Party().GetBattleParty();
			bool alive = false;
			for(int i=0; i<party.Length; i++)
			{
				if(!party[i].isDead)
				{
					alive = true;
					break;
				}
			}
			if(!alive)
			{
				this.BattleLost();
			}
		}
	}
	
	public void EndBattle()
	{
		this.battleEnd = true;
		DataHolder.BattleCam().EndBattle();
		this.StopTime();
		GameHandler.GetLevelHandler().ClearBattleMenuUsers();
		if(GUISystemType.ORK.Equals(DataHolder.GameSettings().guiSystemType) &&
			TextureDelete.CLOSE.Equals(DataHolder.GameSettings().battleTextureDelete))
		{
			GameHandler.Party().DeleteBattleTextures();
		}
	}
	
	public void BattleVictory()
	{
		this.EndBattle();
		
		Character[] party = GameHandler.Party().GetBattleParty();
		for(int i=0; i<party.Length; i++)
		{
			party[i].EndBattle();
			if(!party[i].isDead && party[i].prefabInstance && party[i].prefabInstance.animation)
			{
				if("" != party[i].GetVictoryAnimationName())
				{
					party[i].prefabInstance.animation.CrossFade(party[i].GetVictoryAnimationName(), 0.3f, PlayMode.StopAll);
				}
			}
		}
		this.ShowBattleVictoryMessage();
		this.VictoryGains();
	}
	
	public void VictoryGains()
	{
		if(!this.IsRealTime()) DataHolder.Statistic.BattleWon();
		if(!this.showingGains)
		{
			GameHandler.SetBlockControl(1);
		}
		
		ArrayList endTexts = new ArrayList();
		string moneyText = "";
		if(this.moneyGain > 0)
		{
			GameHandler.AddMoney(this.moneyGain);
			moneyText = DataHolder.BattleEnd().GetMoneyText(this.moneyGain)+"\n";
		}
		string itemText = "";
		string expText = "";
		// item gains
		foreach(DictionaryEntry entry in this.itemGains)
		{
			GameHandler.AddItem((int)entry.Key, (int)entry.Value);
			string tmp = DataHolder.BattleEnd().GetItemText((int)entry.Key, (int)entry.Value, ItemDropType.ITEM);
			if("" != tmp) itemText += tmp+"\n";
		}
		foreach(DictionaryEntry entry in this.weaponGains)
		{
			GameHandler.AddWeapon((int)entry.Key, (int)entry.Value);
			string tmp = DataHolder.BattleEnd().GetItemText((int)entry.Key, (int)entry.Value, ItemDropType.WEAPON);
			if("" != tmp) itemText += tmp+"\n";
		}
		foreach(DictionaryEntry entry in this.armorGains)
		{
			GameHandler.AddArmor((int)entry.Key, (int)entry.Value);
			string tmp = DataHolder.BattleEnd().GetItemText((int)entry.Key, (int)entry.Value, ItemDropType.ARMOR);
			if("" != tmp) itemText += tmp+"\n";
		}
		
		Character[] party = GameHandler.Party().GetBattleParty();
		// experience gains
		foreach(DictionaryEntry entry in this.expGains)
		{
			string tmp = DataHolder.BattleEnd().GetExperienceText((int)entry.Key, (int)entry.Value);
			if("" != tmp) expText += tmp+"\n";
			for(int i=0; i<party.Length; i++)
			{
				if(!party[i].isDead)
				{
					tmp = party[i].status[(int)entry.Key].AddValue((int)entry.Value, true, false, true);
					if("" != tmp && DataHolder.BattleEnd().showLevelUp) endTexts.Add(tmp);
				}
			}
		}
		// check level up
		for(int i=0; i<party.Length; i++)
		{
			string tmp = party[i].CheckLevelUp();
			if("" != tmp && DataHolder.BattleEnd().showLevelUp) endTexts.Add(tmp);
		}
		string gainText = "";
		for(int i=0; i<DataHolder.BattleEnd().gainOrder.Length; i++)
		{
			if(DataHolder.BattleEnd().gainOrder[i] == BattleEnd.MONEY)
			{
				gainText += moneyText;
			}
			else if(DataHolder.BattleEnd().gainOrder[i] == BattleEnd.ITEM)
			{
				gainText += itemText;
			}
			else if(DataHolder.BattleEnd().gainOrder[i] == BattleEnd.EXPERIENCE)
			{
				gainText += expText;
			}
		}
		if(DataHolder.BattleEnd().showGains && "" != gainText) endTexts.Insert(0, gainText);
		
		this.itemGains = new Hashtable();
		this.weaponGains = new Hashtable();
		this.armorGains = new Hashtable();
		this.expGains = new Hashtable();
		this.moneyGain = 0;
		
		if(endTexts.Count > 0)
		{
			this.showingGains = true;
			GameHandler.GetLevelHandler().ShowBattleEnd(
					endTexts.ToArray(typeof(string)) as string[], true);
		}
		else if(!this.showingGains) this.GainsAccepted();
	}
	
	public void GainsAccepted()
	{
		GameHandler.SetBlockControl(-1);
		this.showingGains = false;
		if(this.battleEnd)
		{
			this.battleRunning = false;
			if(this.battleArena)
			{
				this.battleArena.StartCoroutine(this.battleArena.BattleFinished());
			}
			this.battleArena = null;
			this.ClearObjectsAndAnimations();
		}
	}
	
	public void BattleEscaped()
	{
		if(!this.IsRealTime()) DataHolder.Statistic.BattleEscaped();
		this.EndBattle();
		this.battleRunning = false;
		if(this.battleArena)
		{
			this.ShowBattleEscapeMessage();
			this.battleArena.StartCoroutine(this.battleArena.BattleFinished());
		}
		this.battleArena = null;
		this.ClearObjectsAndAnimations();
	}
	
	public void BattleLost()
	{
		if(!this.IsRealTime()) DataHolder.Statistic.BattleLost();
		this.EndBattle();
		Character[] party = GameHandler.Party().GetBattleParty();
		for(int i=0; i<party.Length; i++)
		{
			party[i].EndBattle();
		}
		for(int i=0; i<this.enemies.Length; i++)
		{
			this.enemies[i].EndBattle();
		}
		this.battleRunning = false;
		this.ShowBattleDefeatMessage();
		this.battleArena = null;
		this.ClearObjectsAndAnimations();
	}
	
	public void ClearObjectsAndAnimations()
	{
		if(this.groupCenter) GameObject.Destroy(this.groupCenter);
		if(this.combatantCenter) GameObject.Destroy(this.combatantCenter);
		Character[] party = GameHandler.Party().GetBattleParty();
		for(int i=0; i<party.Length; i++)
		{
			if(party[i].prefabInstance)
			{
				if(party[i].prefabInstance.animation && "" != party[i].GetIdleAfterAnimationName())
				{
					party[i].prefabInstance.animation.CrossFade(party[i].GetIdleAfterAnimationName(), 0.3f, PlayMode.StopAll);
				}
			}
		}
		if(this.IsRealTime()) GameHandler.ClearInBattleArea();
	}
	
	/*
	============================================================================
	Looking functions
	============================================================================
	*/
	public void DoLookAt(Combatant[] party, Combatant[] enemies)
	{
		if(enemies != null && enemies.Length > 0 &&
			party != null && party.Length > 0)
		{
			this.GroupLookAt(this.GetGroupCenter(enemies), party);
			this.GroupLookAt(this.GetGroupCenter(party), enemies);
		}
	}
	
	public void DoLookAt(Combatant user, Combatant[] targets)
	{
		if(user != null && targets.Length > 0)
		{
			this.UserLookAt(this.GetGroupCenter(targets), user);
		}
	}
	
	public GameObject GetGroupCenter(Combatant[] cs)
	{
		int count = 0;
		Vector3 cen = Vector3.zero;
		for(int i=0; i<cs.Length; i++)
		{
			if(cs[i] != null && cs[i].prefabInstance)
			{
				cen += cs[i].prefabInstance.transform.position;
				count++;
			}
		}
		if(count > 0)
		{
			cen /= count;
			if(this.groupCenter == null)
			{
				this.groupCenter = new GameObject();
			}
			this.groupCenter.transform.position = cen;
		}
		return this.groupCenter;
	}
	
	public void GroupLookAt(GameObject cen, Combatant[] cs)
	{
		for(int i=0; i<cs.Length; i++)
		{
			if(cs[i] != null && cs[i].prefabInstance && !cs[i].IsInAction())
			{
				cs[i].prefabInstance.transform.LookAt(new Vector3(cen.transform.position.x, 
						cs[i].prefabInstance.transform.position.y, cen.transform.position.z));
			}
		}
	}
	
	public void UserLookAt(GameObject cen, Combatant user)
	{
		if(user.prefabInstance)
		{
			user.prefabInstance.transform.LookAt(new Vector3(cen.transform.position.x, 
					user.prefabInstance.transform.position.y, cen.transform.position.z));
		}
	}
	
	public GameObject GetCombatantCenter()
	{
		if(this.combatantCenter == null)
		{
			this.combatantCenter = new GameObject();
		}
		int count = 0;
		this.combatantCenter.transform.position = Vector3.zero;
		Combatant[] party = GameHandler.Party().GetBattleParty();
		for(int i=0; i<party.Length; i++)
		{
			if(party[i] != null && party[i].prefabInstance != null)
			{
				this.combatantCenter.transform.position += party[i].prefabInstance.transform.position;
				count++;
			}
		}
		for(int i=0; i<this.enemies.Length; i++)
		{
			if(this.enemies[i] != null && this.enemies[i].prefabInstance != null)
			{
				this.combatantCenter.transform.position += this.enemies[i].prefabInstance.transform.position;
				count++;
			}
		}
		if(count > 0)
		{
			this.combatantCenter.transform.position /= count;
		}
		return this.combatantCenter;
	}
	
	public GameObject GetArenaCenter()
	{
		if(this.battleArena == null) return this.GetCombatantCenter();
		else return this.battleArena.gameObject;
	}
	
	public BattleArena GetArena()
	{
		return this.battleArena;
	}
	
	/*
	============================================================================
	Message functions
	============================================================================
	*/
	public void ShowBattleStartMessage()
	{
		if(this.battleArena != null) this.battleArena.SetMessageTime();
		if(this.battleAdvantage == BattleAdvantage.PARTY && this.partyAdvantage.overrideText)
		{
			this.partyAdvantage.ShowText();
		}
		else if(this.battleAdvantage == BattleAdvantage.ENEMIES && this.enemiesAdvantage.overrideText)
		{
			this.enemiesAdvantage.ShowText();
		}
		else
		{
			GameHandler.GetLevelHandler().ShowBattleMessage(DataHolder.BattleSystemData().GetBattleStartText(), 
					DataHolder.BattleSystemData().battleStartColor, DataHolder.BattleSystemData().battleStartSColor);
		}
	}
	
	public void ShowBattleVictoryMessage()
	{
		if(this.battleArena != null) this.battleArena.BattleVictory();
		GameHandler.GetLevelHandler().ShowBattleMessage(DataHolder.BattleSystemData().GetBattleVictoryText(), 
				DataHolder.BattleSystemData().battleVictoryColor, DataHolder.BattleSystemData().battleVictorySColor);
	}
	
	public void ShowBattleEscapeMessage()
	{
		if(this.battleArena != null) this.battleArena.SetMessageTime();
		GameHandler.GetLevelHandler().ShowBattleMessage(DataHolder.BattleSystemData().GetBattleEscapeText(), 
				DataHolder.BattleSystemData().battleEscapeColor, DataHolder.BattleSystemData().battleEscapeSColor);
	}
	
	public void ShowBattleDefeatMessage()
	{
		if(this.battleArena != null) this.battleArena.StartCoroutine(this.battleArena.BattleLost());
		else if(this.IsRealTime())
		{
			BattleArea area = GameHandler.GetBattleArea();
			if(area != null) area.StartCoroutine(area.BattleLost());
		}
		GameHandler.GetLevelHandler().ShowBattleMessage(DataHolder.BattleSystemData().GetBattleDefeatText(), 
				DataHolder.BattleSystemData().battleDefeatColor, DataHolder.BattleSystemData().battleDefeatSColor);
	}
}