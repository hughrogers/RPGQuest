
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combatant
{
	public int raceID = 0;
	public int sizeID = 0;
	
	public string prefabName = "";
	public string prefabRoot = "";
	public int baseCounter = 0;
	public int baseCritical = 0;
	public int baseBlock = 0;
	public BonusSettings bonus = new BonusSettings();
	
	public bool noRevive = false;
	
	public StatusValue[] status;
	public StatusEffect[] effect = new StatusEffect[0];
	protected int[] element;
	protected int[] raceDmgFactor;
	protected int[] sizeDmgFactor;
	
	public StatusTimeChange[] fieldStatusChange = new StatusTimeChange[0];
	public StatusTimeChange[] battleStatusChange = new StatusTimeChange[0];
	
	// attack settings
	public int baseElement = 0;
	public int[] baseAttack = new int[1];
	
	// battle settings
	public FieldAnimationSettings fieldAnimations = new FieldAnimationSettings();
	public BattleAnimationSettings battleAnimations = new BattleAnimationSettings();
	public CustomAnimationSettings customAnimations = new CustomAnimationSettings();
	public AutoAttack autoAttack = new AutoAttack();
	
	public AggressiveType aggressiveType = AggressiveType.ALWAYS;
	private bool isAggressive = true;
	
	public bool useMoveSpeedFormula = false;
	public int moveSpeedFormula = 0;
	public float moveSpeed = 5;
	public float minMoveSpeed = 1;
	
	// audio settings
	public string[] audioClipName = new string[20];
	public AudioClip[] audioClip = new AudioClip[20];
	
	public float sprintEnergy = 10;
	public float sprintEnergyMax = 10;
	
	// ingame
	public int currentLevel = 1;
	public int currentClassLevel = 1;
	public int realID = -1;
	
	public int battleID = -1;
	public int lastTurnIndex = -1;
	public bool turnPerformed = false;
	public int currentTurn = 0;
	
	protected Queue<BattleAction> actionQueue = new Queue<BattleAction>();
	protected bool actionFired = false;
	protected bool finishAttackQueue = false;
	protected bool turnEnded = false;
	
	public float timeBar = 0.0f;
	public float[] timeBarHUD;
	public float usedTimeBar = 0.0f;
	public float[] usedTimeBarHUD;
	
	public bool isDead = false;
	public bool isDefending = false;
	public bool effectHUD = false;
	public int effectCheckCounter = 0;
	
	public GameObject prefabInstance;
	private Transform battleSpot;
	public GameObject cursorInstance;
	
	// skill ingame
	public int lastSkill = -1;
	protected SkillBlock[] skillReuseBlock = new SkillBlock[0];
	public bool skillBlockChanged = false;
	// cast time
	public bool castingSkill = false;
	public BattleAction castSkill = null;
	public float[] castTimeHUD;
	
	protected bool isChoosingAction = false;
	protected bool inAction = false;
	protected bool waitForAction = false;
	public int lastTargetBattleID = BattleAction.NONE;
	
	public bool autoAttackStarted = false;
	public float autoAttackTime = 0;
	
	public bool blockedControl = false;
	
	// ai settings
	public AIMoverSettings aiMoverSettings = new AIMoverSettings();
	public AIBehaviour[] aiBehaviour = new AIBehaviour[0];
	public bool attackLastTarget = false;
	public bool attackPartyTarget = false;
	public bool aiNearestTarget = false;
	public float aiTimeout = 0;
	public float aiTimeoutTimer = 0;
	
	private AIMover aiMover = null;
	public bool respawnFlag = false;
	public BattleAction aiAction = null;
	protected int rtCounterFlag = -1;
	
	public int baIndex = 0;
	protected float baTimeout = -10;
	
	public Combatant()
	{
		
	}
	
	public virtual string GetName() { return ""; }
	
	public bool SameType(Combatant c)
	{
		return (this is Character && c is Character) || (this is Enemy && c is Enemy);
	}
	
	public virtual void Init()
	{
		this.status = new StatusValue[DataHolder.StatusValueCount];
		for(int i=0; i<this.status.Length; i++)
		{
			this.status[i] = DataHolder.StatusValues().GetCopy(i);
			this.status[i].SetOwner(this);
		}
		
		this.timeBarHUD = new float[DataHolder.HUDs().GetDataCount()];
		this.usedTimeBarHUD = new float[DataHolder.HUDs().GetDataCount()];
		this.castTimeHUD = new float[DataHolder.HUDs().GetDataCount()];
		
		if(!AggressiveType.ALWAYS.Equals(this.aggressiveType))
		{
			this.isAggressive = false;
		}
	}
	
	/*
	============================================================================
	Prefab functions
	============================================================================
	*/
	public virtual string GetPrefabPath() { return ""; }
	
	public GameObject CreatePrefabInstance()
	{
		this.DestroyPrefab();
		if("" != this.prefabName)
		{
			GameObject tmp = (GameObject)Resources.Load(this.GetPrefabPath()+this.prefabName, typeof(GameObject));
			if(tmp)
			{
				this.AddComponents((GameObject)GameObject.Instantiate(tmp));
			}
		}
		return this.prefabInstance;
	}
	
	public void AddComponents(GameObject obj)
	{
		if(obj != null)
		{
			CombatantClick cc = (CombatantClick)obj.AddComponent("CombatantClick");
			cc.combatant = this;
			this.aiMover = this.aiMoverSettings.AddAIMover(obj);
			for(int i=0; i<this.effect.Length; i++)
			{
				this.effect[i].AddPrefabs(this);
			}
			this.prefabInstance = obj;
			if(this.fieldAnimations.baseAnimator)
			{
				this.fieldAnimations.AddAnimations(obj);
				this.fieldAnimations.SetAnimationLayers(this);
			}
			this.battleAnimations.SetAnimationLayers(this);
			this.customAnimations.SetAnimationLayers(this);
			
			this.prefabInstance = TransformHelper.GetChild(
					this.prefabRoot, obj.transform).gameObject;
		}
	}
	
	public GameObject GetRealRootObject()
	{
		if(this.prefabInstance != null)
		{
			return this.prefabInstance.transform.root.gameObject;
		}
		else return null;
	}
	
	public void DestroyPrefab()
	{
		if(this.prefabInstance != null)
		{
			GameObject.Destroy(this.GetRealRootObject());
		}
	}
	
	/*
	============================================================================
	Div functions
	============================================================================
	*/
	public AudioSource GetAudioSource()
	{
		AudioSource s = null;
		if(this.prefabInstance != null)
		{
			if(this.prefabInstance.audio != null)
			{
				s = this.prefabInstance.audio;
			}
			else
			{
				s = this.prefabInstance.GetComponentInChildren<AudioSource>();
			}
			if(s == null) s = this.prefabInstance.AddComponent<AudioSource>();
		}
		return s;
	}
	
	public void SetRoute(float t, GameObject[] wp)
	{
		if(this.aiMover != null)
		{
			this.aiMover.SetRoute(t, wp);
		}
	}
	
	public void SetBattleSpot(Transform t)
	{
		this.battleSpot = t;
	}
	
	public Transform GetBattleSpot()
	{
		if(this.battleSpot == null && this.prefabInstance != null)
		{
			return this.prefabInstance.transform;
		}
		else return this.battleSpot;
	}
	
	public AudioClip GetAudioClip(int index)
	{
		AudioClip clip = null;
		if(index < this.audioClip.Length)
		{
			if(this.audioClip[index] == null && 
				this.audioClipName[index] != null && this.audioClipName[index] != "")
			{
				this.audioClip[index] = (AudioClip)Resources.Load(
						BattleSystemData.AUDIO_PATH+this.audioClipName[index], typeof(AudioClip));
			}
			clip = this.audioClip[index];
		}
		return clip;
	}
	
	public void CheckAggressive(AggressiveType t)
	{
		if(this.aggressiveType.Equals(t))
		{
			this.isAggressive = true;
		}
	}
	
	public void StatusChanged(int id)
	{
		this.UpdateAnimations();
	}
	
	/*
	============================================================================
	Movement functions
	============================================================================
	*/
	protected virtual float GetInternMoveSpeed()
	{
		float speed = this.moveSpeed;
		if(this.useMoveSpeedFormula)
		{
			speed = DataHolder.Formula(this.moveSpeedFormula).Calculate(this, this);
		}
		speed += this.bonus.GetSpeedBonus();
		for(int i=0; i<this.effect.Length; i++)
		{
			speed += this.effect[i].bonus.GetSpeedBonus();
		}
		return speed;
	}
	
	public float GetMoveSpeed()
	{
		float speed = this.GetInternMoveSpeed();
		if(speed < this.minMoveSpeed) speed = this.minMoveSpeed;
		return speed;
	}
	
	public bool IsInAir()
	{
		bool inAir = false;
		if(this.prefabInstance != null)
		{
			CharacterController controller = this.prefabInstance.GetComponent<CharacterController>();
			if(controller != null && !controller.isGrounded)
			{
				inAir = true;
			}
		}
		return inAir;
	}
	
	public float GetCurrentSpeed()
	{
		float currentSpeed = 0;
		if(this.prefabInstance != null)
		{
			CharacterController controller = this.prefabInstance.GetComponent<CharacterController>();
			if(controller != null)
			{
				Vector3 horizontalVelocity = controller.velocity;
				horizontalVelocity.y = 0;
				currentSpeed = horizontalVelocity.magnitude;
			}
		}
		return currentSpeed;
	}
	
	/*
	============================================================================
	Base attack functions
	============================================================================
	*/
	public virtual bool InAttackRange(Combatant c)
	{
		return DataHolder.BaseAttack(this.baseAttack[this.baIndex]).useRange.InRange(this, c);
	}
	
	public void AddBaseAttack()
	{
		this.baseAttack = ArrayHelper.Add(0, this.baseAttack);
	}
	
	public void RemoveBaseAttack(int index)
	{
		this.baseAttack = ArrayHelper.Remove(index, this.baseAttack);
	}
	
	public virtual void NextBaseAttack()
	{
		this.baIndex++;
		if(this.baIndex >= this.baseAttack.Length)
		{
			this.baIndex = 0;
		}
		if(DataHolder.BaseAttack(this.baseAttack[this.baIndex]).availableTime > 0)
		{
			this.baTimeout = DataHolder.BaseAttack(this.baseAttack[this.baIndex]).availableTime;
		}
		else this.baTimeout = -10;
	}
	
	public void ResetBaseAttack()
	{
		this.baIndex = 0;
		this.baTimeout = -10;
	}
	
	public virtual bool UseBaseAttack(Combatant target, BattleAction ba, 
			bool counter, float damageFactor, float damageMultiplier)
	{
		return false;
	}
	
	public virtual bool UseCounter(Combatant target)
	{
		bool use = false;
		if(!this.isDead)
		{
			float chance = DataHolder.Formulas().formula[this.baseCounter].Calculate(this, target);
			chance += this.GetCounterBonus();
			if(DataHolder.GameSettings().GetRandom() <= chance)
			{
				use = true;
			}
		}
		if(DataHolder.BattleSystem().IsRealTime() && use)
		{
			this.rtCounterFlag = target.battleID;
		}
		return use;
	}
	
	/*
	============================================================================
	Skill functions
	============================================================================
	*/
	public void AddSkillBlock(SkillBlock block)
	{
		this.skillReuseBlock = ArrayHelper.Add(block, this.skillReuseBlock);
		this.skillBlockChanged = true;
	}
	
	public void RemoveSkillBlock(SkillBlock block)
	{
		this.skillReuseBlock = ArrayHelper.Remove(block, this.skillReuseBlock);
		this.skillBlockChanged = true;
	}
	
	public bool IsSkillBlocked(int id)
	{
		bool blocked = false;
		for(int i=0; i<this.skillReuseBlock.Length; i++)
		{
			if(this.skillReuseBlock[i].skillID == id)
			{
				blocked = true;
				break;
			}
		}
		return blocked;
	}
	
	public virtual bool HasSkill(int id, int lvl)
	{
		return false;
	}
	
	/*
	============================================================================
	Battle functions
	============================================================================
	*/
	public virtual void StartBattle(int bID)
	{
		this.battleID = bID;
		this.actionQueue.Clear();
		this.actionFired = false;
		this.finishAttackQueue = false;
		this.turnEnded = false;
		this.turnPerformed = true;
		this.isChoosingAction = false;
		this.waitForAction = false;
		this.currentTurn = 0;
		this.timeBar = 0;
		this.usedTimeBar = 0;
		this.lastTargetBattleID = BattleAction.NONE;
		this.SetInAction(false);
		this.autoAttackStarted = false;
		this.autoAttackTime = Random.Range(0.0f, this.autoAttack.interval);
		this.aiAction = null;
		this.SetStartEffects();
	}
	
	public virtual void EndBattle()
	{
		for(int i=0; i<this.effect.Length; i++)
		{
			this.effect[i].EndBattle();
		}
		for(int i=0; i<this.skillReuseBlock.Length; i++)
		{
			this.skillReuseBlock[i].EndBattle(this);
		}
		this.actionQueue.Clear();
		this.actionFired = false;
		this.finishAttackQueue = false;
		this.turnEnded = false;
		this.castingSkill = false;
		this.castSkill = null;
		this.SetInAction(false);
		this.lastTurnIndex = -1;
		this.turnPerformed = false;
		this.isChoosingAction = false;
		this.waitForAction = false;
		this.lastTargetBattleID = BattleAction.NONE;
		this.autoAttackStarted = false;
		this.aiAction = null;
	}
	
	public bool HasAutoAttack()
	{
		return this.autoAttack.active && 
				(DataHolder.BattleSystem().dynamicCombat ||
				DataHolder.BattleControl().UseAutoAttackTarget(this));
	}
	
	public bool IsInAction()
	{
		return this.inAction || this.castingSkill;
	}
	
	public void SetInAction(bool ia)
	{
		this.inAction = ia;
		if(DataHolder.BattleSystem().IsRealTime() &&
			DataHolder.BattleSystem().blockControlAction &&
			GameHandler.Party().IsPlayerCharacter(this) &&
			this.inAction)
		{
			GameHandler.SetBlockControl(1);
			this.blockedControl = true;
		}
		
		if(!this.inAction)
		{
			if(this.blockedControl)
			{
				this.blockedControl = false;
				GameHandler.SetBlockControl(-1);
			}
			if(this.autoAttackStarted)
			{
				this.autoAttackStarted = false;
				this.autoAttackTime = this.autoAttack.interval;
			}
			else this.waitForAction = false;
		}
	}
	
	public bool IsChoosingAction()
	{
		return this.isChoosingAction;
	}
	
	public bool IsWaitingForAction()
	{
		return this.waitForAction;
	}
	
	public bool AllowPerformingAction()
	{
		return !this.IsInAction() && 
			(!DataHolder.BattleSystem().IsActiveTime() || (this.timeBar >= this.usedTimeBar &&
			(this.finishAttackQueue || 
			((UseTimebarAction.ACTION_BORDER.Equals(DataHolder.BattleSystem().useTimebarAction) && 
				this.timeBar >= DataHolder.BattleSystem().actionBorder) ||
			(UseTimebarAction.MAX_TIMEBAR.Equals(DataHolder.BattleSystem().useTimebarAction) && 
				this.timeBar >= DataHolder.BattleSystem().maxTimebar) ||
			(UseTimebarAction.END_TURN.Equals(DataHolder.BattleSystem().useTimebarAction) && this.turnEnded)))));
	}
	
	public bool CanChooseAction()
	{
		return !this.isDead && !this.turnEnded && 
				!this.IsChoosingAction() &&
				(!this.IsInAction() || this.autoAttackStarted) &&
				!this.IsStopMove() &&
				!this.IsAutoAttack() && 
				!this.IsAttackFriends() && 
				((DataHolder.BattleSystem().IsActiveTime() && DataHolder.BattleSystem().enableMultiChoice) ||
				!this.IsWaitingForAction()) && 
				(!DataHolder.BattleSystem().IsActiveTime() || (!this.finishAttackQueue && 
				this.usedTimeBar < DataHolder.BattleSystem().maxTimebar));
	}
	
	public void InitNewTurn()
	{
		this.currentTurn++;
		this.turnPerformed = true;
		this.waitForAction = true;
		this.isDefending = false;
		for(int i=0; i<this.skillReuseBlock.Length; i++)
		{
			this.skillReuseBlock[i].ReduceTurn(this);
		}
		this.CheckEffects();
		if(DataHolder.BattleSystem().turnBonuses)
		{
			for(int i=0; i<DataHolder.BattleSystem().statusBonus.Length; i++)
			{
				if(this.status[i].IsConsumable() && DataHolder.BattleSystem().statusBonus[i] != 0)
				{
					this.status[i].AddValue(DataHolder.BattleSystem().statusBonus[i], true, false, true);
				}
			}
		}
	}
	
	public virtual void ChooseAction()
	{
		this.InitNewTurn();
	}
	
	public virtual void CheckRealTimeAction()
	{
		this.ChooseAction();
	}
	
	public virtual void EndTurn()
	{
		if(this.actionQueue.Count > 0 || this.actionFired)
		{
			this.turnEnded = true;
		}
		else if(DataHolder.BattleSystem().IsActiveTime())
		{
			this.timeBar = 0;
		}
	}
	
	public bool IsTurnEnded()
	{
		return this.turnEnded;
	}
	
	public virtual void Escape() {}
	
	/*
	============================================================================
	Action functions
	============================================================================
	*/
	public void AddAction(BattleAction action)
	{
		if(action != null && DataHolder.BattleSystem().IsActiveTime() && 
			DataHolder.BattleSystem().enableMultiChoice)
		{
			bool add = true;
			if(!this.isDead && !action.IsCounter() && !action.autoAttackFlag)
			{
				float tmp = action.GetTimeUse();
				if(this.usedTimeBar+tmp <= DataHolder.BattleSystem().maxTimebar)
				{
					this.usedTimeBar += tmp;
				}
				else
				{
					add = false;
					DataHolder.BattleSystem().AddAction(null);
				}
			}
			
			if(add)
			{
				if(this.actionQueue.Count > 0 || this.actionFired)
				{
					this.actionQueue.Enqueue(action);
				}
				else
				{
					this.actionFired = true;
					DataHolder.BattleSystem().AddAction(action);
				}
			}
		}
		else
		{
			this.actionFired = true;
			DataHolder.BattleSystem().AddAction(action);
		}
	}
	
	public void NextAction()
	{
		if(this.actionQueue.Count > 0)
		{
			this.actionFired = true;
			this.waitForAction = true;
			this.isDefending = false;
			DataHolder.BattleSystem().AddAction(this.actionQueue.Dequeue());
		}
		else
		{
			this.actionFired = false;
			this.finishAttackQueue = false;
			this.turnEnded = false;
		}
	}
	
	public void FinishAttackQueue()
	{
		this.finishAttackQueue = true;
	}
	
	public bool IsFinishingAttackQueue()
	{
		return this.finishAttackQueue;
	}
	
	public bool AddAttackAction(int targetID, bool newTurn)
	{
		bool add = false;
		if(!this.IsStopMove() && !this.IsBlockAttack() &&
			this.InAttackRange(DataHolder.BattleSystem().
				GetCombatantForBattleID(targetID)))
		{
			if(newTurn) this.InitNewTurn();
			this.AddAction(new BattleAction(
					AttackSelection.ATTACK, this, targetID, -1, 0));
			add = true;
		}
		return add;
	}
	
	public bool AddSkillAction(int skillID, int skillLevel, int targetID, bool newTurn)
	{
		bool add = false;
		if(!this.IsStopMove() && !this.IsBlockSkills() &&
			DataHolder.Skill(skillID).InRange(this, DataHolder.BattleSystem().
				GetCombatantForBattleID(targetID), skillLevel))
		{
			if(newTurn) this.InitNewTurn();
			BattleAction action = new BattleAction(AttackSelection.SKILL, this, targetID, skillID, skillLevel);
			if(DataHolder.Skill(skillID).targetRaycast.active && 
				!DataHolder.Skill(skillID).targetRaycast.NeedInteraction())
			{
				action.rayTargetSet = true;
				action.rayPoint = DataHolder.Skill(skillID).targetRaycast.GetRayPoint(
						this.prefabInstance, VectorHelper.GetScreenCenter());
			}
			action.CheckRevive();
			this.AddAction(action);
			add = true;
		}
		return add;
	}
	
	public bool AddItemAction(int itemID, int targetID, bool newTurn)
	{
		bool add = false;
		if(!this.IsStopMove() && !this.IsBlockItems() &&
			DataHolder.Item(itemID).useRange.InRange(this, 
				DataHolder.BattleSystem().GetCombatantForBattleID(targetID)))
		{
			if(newTurn) this.InitNewTurn();
			BattleAction action = new BattleAction(AttackSelection.ITEM, this, targetID, itemID, 0);
			if(DataHolder.Item(itemID).targetRaycast.active && 
				!DataHolder.Item(itemID).targetRaycast.NeedInteraction())
			{
				action.rayTargetSet = true;
				action.rayPoint = DataHolder.Item(itemID).targetRaycast.GetRayPoint(
						this.prefabInstance, VectorHelper.GetScreenCenter());
			}
			action.CheckRevive();
			this.AddAction(action);
			add = true;
		}
		return add;
	}
	
	/*
	============================================================================
	Time functions
	============================================================================
	*/
	public void Tick()
	{
		if(!this.isDead && DataHolder.BattleSystem().DoCombatantTick())
		{
			float bt = GameHandler.DeltaBattleTime;
			// status time changes
			if(GameHandler.IsControlBattle())
			{
				for(int i=0; i<this.battleStatusChange.Length; i++)
				{
					this.battleStatusChange[i].Tick(bt, this);
				}
			}
			else if(GameHandler.IsControlField())
			{
				float dt = GameHandler.DeltaTime;
				for(int i=0; i<this.fieldStatusChange.Length; i++)
				{
					this.fieldStatusChange[i].Tick(dt, this);
				}
			}
			
			// effects
			this.effectCheckCounter++;
			for(int i=0; i<this.effect.Length; i++)
			{
				this.effect[i].TimeCheck();
			}
			if(this.effectCheckCounter >= 10)
			{
				DataHolder.Effects().CheckAutoApply(this);
				for(int i=0; i<this.effect.Length; i++)
				{
					this.effect[i].CheckAutoRemove(this);
				}
				this.effectCheckCounter = 0;
			}
			
			// skill reuse
			for(int i=0; i<this.skillReuseBlock.Length; i++)
			{
				this.skillReuseBlock[i].ReduceTime(this, bt);
			}
			
			// skill casting
			if(this.castingSkill && this.castSkill != null && !this.autoAttackStarted)
			{
				this.castSkill.castTime += bt;
				if(this.castSkill.castTime >= this.castSkill.castTimeMax)
				{
					this.castSkill.casted = true;
					DataHolder.BattleSystem().PerformAction(this.castSkill);
					this.castingSkill = false;
					this.castSkill = null;
				}
			}
			
			// auto attack
			if(!this.autoAttackStarted && !this.isDefending &&
				this.HasAutoAttack() && !this.IsInAction() &&
				DataHolder.BattleSystem().IsBattleRunning() &&
				(!this.isChoosingAction || !DataHolder.BattleSystem().blockAutoAttackMenu))
			{
				if(this.autoAttackTime > 0)
				{
					this.autoAttackTime -= bt;
				}
				else if(!this.IsStopMove() && 
					!(this.autoAttack.useSkill && this.IsBlockSkills()) &&
					!(!this.autoAttack.useSkill && this.IsBlockAttack()))
				{
					this.autoAttackStarted = true;
					this.AddAction(this.autoAttack.GetAction(this));
				}
			}
			
			// real time AI
			if(DataHolder.BattleSystem().IsRealTime() && 
				!DataHolder.BattleSystem().battleEnd &&  
				GameHandler.IsInBattleArea() && !this.isDead && this.isAggressive)
			{
				if(this.aiAction == null && this.CanChooseAction())
				{
					if(this.aiTimeoutTimer > 0) this.aiTimeoutTimer -= bt;
					if(this.aiTimeoutTimer <= 0 || this.rtCounterFlag >= 0)
					{
						this.CheckRealTimeAction();
					}
				}
				else if(this.aiAction != null && this.aiAction.InRange())
				{
					this.aiMover.StopFollow(true);
					this.AddAction(this.aiAction);
					this.aiAction = null;
				}
			}
			
			if(!this.inAction && this.baTimeout != -10)
			{
				this.baTimeout -= bt;
				if(this.baTimeout <= 0) this.ResetBaseAttack();
			}
		}
	}
	
	/*
	============================================================================
	Skill cast functions
	============================================================================
	*/
	public float GetSkillCastTime()
	{
		float time = -1;
		if(this.castingSkill && this.castSkill != null)
		{
			time = this.castSkill.castTime;
		}
		return time;
	}
	
	public float GetSkillCastTimeMax()
	{
		float time = -1;
		if(this.castingSkill && this.castSkill != null)
		{
			time = this.castSkill.castTimeMax;
		}
		return time;
	}
	
	public void CancelSkillCast()
	{
		if(this.castingSkill && this.castSkill != null && 
			this.castSkill.CancelSkillCast())
		{
			DataHolder.BattleSystemData().castCancelTextSettings.ShowText(
					DataHolder.Skills().GetName(this.castSkill.useID), this);
			this.castingSkill = false;
			this.waitForAction = false;
			if(!this.castSkill.autoAttackFlag)
			{
				this.timeBar -= this.castSkill.GetTimeUse();
				this.usedTimeBar -= this.castSkill.GetTimeUse();
			}
			this.castSkill = null;
			this.NextAction();
		}
	}
	
	/*
	============================================================================
	Status effect functions
	============================================================================
	*/
	public virtual bool CanApplyEffect(int effectID)
	{
		return true;
	}
	
	public virtual bool CanRemoveEffect(int effectID)
	{
		return true;
	}
	
	public void CheckEffects()
	{
		for(int i=0; i<this.effect.Length; i++)
		{
			this.effect[i].CheckEffect();
		}
	}
	
	public void CheckEffectsAttack()
	{
		ArrayList tmp = new ArrayList();
		for(int i=0; i<this.effect.Length; i++)
		{
			if(this.effect[i].endOnAttack)
			{
				tmp.Add(this.effect[i].realID);
			}
		}
		for(int i=0; i<tmp.Count; i++)
		{
			this.RemoveEffect((int)tmp[i]);
		}
	}
	
	public void AddEffect(int effectID, Combatant user)
	{
		if(this.IsEffectSet(effectID))
		{
			for(int i=0; i<this.effect.Length; i++)
			{
				if(this.effect[i].realID == effectID)
				{
					this.effect[i].ReApply();
				}
			}
		}
		else
		{
			StatusEffect se = DataHolder.Effects().GetCopy(effectID);
			if(se.ApplyEffect(user, this))
			{
				this.effect = ArrayHelper.Add(se, this.effect);
				DataHolder.BattleSystemData().effectTextSettings.ShowText(se.GetName(), this);
				se.AddPrefabs(this);
				this.ResetStatus();
				this.ResetEffects();
				this.effectHUD = true;
			}
		}
	}
	
	public void RemoveEffect(int effectID)
	{
		for(int i=0; i<this.effect.Length; i++)
		{
			if(this.effect[i].realID == effectID)
			{
				this.effect[i].RemoveEffect(this);
				break;
			}
		}
		this.ResetStatus();
		this.ResetEffects();
		this.effectHUD = true;
	}
	
	public void RemoveEffect(StatusEffect e)
	{
		this.effect = ArrayHelper.Remove(e, this.effect);
		e.DestroyPrefabs();
	}
	
	public void ResetEffects()
	{
		for(int i=0; i<this.effect.Length; i++)
		{
			this.effect[i].ResetChange(this);
		}
	}
	
	public bool HasReflect()
	{
		bool reflect = false;
		for(int i=0; i<this.effect.Length; i++)
		{
			if(this.effect[i].reflectSkills)
			{
				reflect = true;
				break;
			}
		}
		return reflect;
	}
	
	public virtual void ResetStatus()
	{
		for(int i=0; i<this.status.Length; i++)
		{
			if(this.status[i].IsNormal())
			{
				this.status[i].ResetValue();
			}
		}
		int[] statusBonus = this.bonus.GetStatusBonus();
		for(int i=0; i<statusBonus.Length; i++)
		{
			if(statusBonus[i] != 0)
			{
				this.status[i].AddValue(statusBonus[i], false, false, false);
			}
		}
		for(int i=0; i<this.effect.Length; i++)
		{
			int[] effectBonus = this.effect[i].bonus.GetStatusBonus();
			for(int j=0; j<effectBonus.Length; j++)
			{
				if(effectBonus[j] != 0)
				{
					this.status[j].AddValue(effectBonus[j], false, false, false);
				}
			}
		}
	}
	
	public virtual void SetStartEffects() {}
	
	public bool IsEffectSet(int effectID)
	{
		bool has = false;
		for(int i=0; i<this.effect.Length; i++)
		{
			if(this.effect[i].realID == effectID)
			{
				has = true;
				break;
			}
		}
		return has;
	}
	
	public bool IsStopMove()
	{
		bool stop = false;
		for(int i=0; i<this.effect.Length; i++)
		{
			if(this.effect[i].stopMove)
			{
				stop = true;
				break;
			}
		}
		return stop;
	}
	
	public bool IsStopMovement()
	{
		bool stop = false;
		for(int i=0; i<this.effect.Length; i++)
		{
			if(this.effect[i].stopMovement)
			{
				stop = true;
				break;
			}
		}
		return stop;
	}
	
	public bool IsAutoAttack()
	{
		bool attack = false;
		for(int i=0; i<this.effect.Length; i++)
		{
			if(this.effect[i].autoAttack)
			{
				attack = true;
				break;
			}
		}
		return attack;
	}
	
	public bool IsAttackFriends()
	{
		bool attack = false;
		for(int i=0; i<this.effect.Length; i++)
		{
			if(this.effect[i].attackFriends)
			{
				attack = true;
				break;
			}
		}
		return attack;
	}
	
	public virtual bool IsBlockAttack()
	{
		bool block = false;
		for(int i=0; i<this.effect.Length; i++)
		{
			if(this.effect[i].blockAttack)
			{
				block = true;
				break;
			}
		}
		return block;
	}
	
	public bool IsBlockSkills()
	{
		bool block = false;
		for(int i=0; i<this.effect.Length; i++)
		{
			if(this.effect[i].blockSkills)
			{
				block = true;
				break;
			}
		}
		return block;
	}
	
	public bool IsBlockItems()
	{
		bool block = false;
		for(int i=0; i<this.effect.Length; i++)
		{
			if(this.effect[i].blockItems)
			{
				block = true;
				break;
			}
		}
		return block;
	}
	
	public bool IsBlockDefend()
	{
		bool block = false;
		for(int i=0; i<this.effect.Length; i++)
		{
			if(this.effect[i].blockDefend)
			{
				block = true;
				break;
			}
		}
		return block;
	}
	
	public bool IsBlockEscape()
	{
		bool block = false;
		for(int i=0; i<this.effect.Length; i++)
		{
			if(this.effect[i].blockEscape)
			{
				block = true;
				break;
			}
		}
		return block;
	}
	
	public int GetEffectAttackElement()
	{
		int eID = -1;
		for(int i=0; i<this.effect.Length; i++)
		{
			if(this.effect[i].setElement)
			{
				eID = this.effect[i].attackElement;
				break;
			}
		}
		return eID;
	}
	
	public bool CanChangeStatusValue(int index)
	{
		bool can = true;
		for(int i=0; i<this.effect.Length; i++)
		{
			if(this.effect[i].condition[index].apply && 
				this.effect[i].condition[index].stopChange)
			{
				can = false;
				break;
			}
		}
		return can;
	}
	
	public bool IsSkillTypeBlocked(int index)
	{
		bool blocked = false;
		for(int i=0; i<this.effect.Length; i++)
		{
			if(this.effect[i].skillTypeBlock[index])
			{
				blocked = true;
				break;
			}
		}
		return blocked;
	}
	
	public bool IsBlockBaseAttacks()
	{
		bool blocked = false;
		for(int i=0; i<this.effect.Length; i++)
		{
			if(this.effect[i].blockBaseAttacks)
			{
				blocked = true;
				break;
			}
		}
		return blocked;
	}
	
	/*
	============================================================================
	Death functions
	============================================================================
	*/
	public void CheckDeath()
	{
		if(!this.isDead)
		{
			for(int i=0; i<this.status.Length; i++)
			{
				if(this.status[i].IsConsumable() && this.status[i].killChar && 
						this.status[i].GetValue() <= this.status[i].minValue)
				{
					this.Death();
				}
			}
		}
	}
	
	public void Death()
	{
		if(!this.isDead)
		{
			this.isDead = true;
			this.lastSkill = -1;
			this.timeBar = 0;
			this.usedTimeBar = 0;
			this.actionFired = false;
			this.finishAttackQueue = false;
			this.turnEnded = false;
			this.actionQueue.Clear();
			this.castingSkill = false;
			this.castSkill = null;
			this.aiAction = null;
			for(int i=0; i<this.effect.Length; i++)
			{
				this.effect[i].StopEffect(this);
			}
			DataHolder.BattleSystem().UnshiftAction(new BattleAction(AttackSelection.DEATH, this, this.battleID, -1, 0));
		}
	}
	
	public virtual void Died()
	{
		this.SetInAction(false);
	}
	
	/*
	============================================================================
	Element functions
	============================================================================
	*/
	public int[] GetElements()
	{
		int[] def = new int[this.element.Length];
		for(int i=0; i<def.Length; i++) def[i] = this.GetElementDefence(i);
		return def;
	}
	
	public virtual int GetElementDefence(int index)
	{
		return this.element[index]+this.bonus.GetElementDefence(index);
	}
	
	/*
	============================================================================
	Race damage functions
	============================================================================
	*/
	public virtual int GetRaceDamageFactor(int index)
	{
		int factor = this.raceDmgFactor[index]+this.bonus.GetRaceDamageFactor(index);
		for(int i=0; i<this.effect.Length; i++)
		{
			factor += this.effect[i].raceValue[index];
		}
		return factor;
	}
	
	/*
	============================================================================
	Size damage functions
	============================================================================
	*/
	public virtual int GetSizeDamageFactor(int index)
	{
		int factor = this.sizeDmgFactor[index]+this.bonus.GetSizeDamageFactor(index);
		for(int i=0; i<this.effect.Length; i++)
		{
			factor += this.effect[i].sizeValue[index];
		}
		return factor;
	}
	
	/*
	============================================================================
	Bonus functions
	============================================================================
	*/
	public virtual float GetHitBonus()
	{
		float bonus = this.bonus.GetHitBonus();
		for(int i=0; i<this.effect.Length; i++)
		{
			bonus += this.effect[i].bonus.GetHitBonus();
		}
		return bonus;
	}
	
	public virtual float GetCounterBonus()
	{
		float bonus = this.bonus.GetCounterBonus();
		for(int i=0; i<this.effect.Length; i++)
		{
			bonus += this.effect[i].bonus.GetCounterBonus();
		}
		return bonus;
	}
	
	public virtual float GetCriticalBonus()
	{
		float bonus = this.bonus.GetCriticalBonus();
		for(int i=0; i<this.effect.Length; i++)
		{
			bonus += this.effect[i].bonus.GetCriticalBonus();
		}
		return bonus;
	}
	
	public virtual float GetEscapeBonus()
	{
		float bonus = this.bonus.GetEscapeBonus();
		for(int i=0; i<this.effect.Length; i++)
		{
			bonus += this.effect[i].bonus.GetEscapeBonus();
		}
		return bonus;
	}
	
	public virtual float GetBlockBonus()
	{
		float bonus = this.bonus.GetBlockBonus();
		for(int i=0; i<this.effect.Length; i++)
		{
			bonus += this.effect[i].bonus.GetBlockBonus();
		}
		return bonus;
	}
	
	public virtual float GetItemStealBonus()
	{
		float bonus = this.bonus.GetItemStealBonus();
		for(int i=0; i<this.effect.Length; i++)
		{
			bonus += this.effect[i].bonus.GetItemStealBonus();
		}
		return bonus;
	}
	
	public virtual float GetMoneyStealBonus()
	{
		float bonus = this.bonus.GetMoneyStealBonus();
		for(int i=0; i<this.effect.Length; i++)
		{
			bonus += this.effect[i].bonus.GetMoneyStealBonus();
		}
		return bonus;
	}
	
	/*
	============================================================================
	Animation functions
	============================================================================
	*/
	public Animation GetAnimationComponent()
	{
		Animation a = null;
		if(this.prefabInstance != null)
		{
			if(this.prefabInstance.animation != null)
			{
				a = this.prefabInstance.animation;
			}
			else
			{
				a = this.prefabInstance.GetComponentInChildren<Animation>();
			}
		}
		return a;
	}
	
	public void UpdateAnimations()
	{
		if(this.fieldAnimations.baseAnimator)
		{
			this.fieldAnimations.SetAnimationLayers(this);
		}
		this.battleAnimations.SetAnimationLayers(this);
		this.customAnimations.SetAnimationLayers(this);
	}
	
	public virtual string GetAnimationName(CombatantAnimation type)
	{
		return this.battleAnimations.GetAnimationName(type);
	}
	
	public void PlayAnimation(CombatantAnimation type, PlayMode mode)
	{
		if(this.prefabInstance && this.prefabInstance.animation)
		{
			string name = this.GetAnimationName(type);
			if("" != name)
			{
				this.prefabInstance.animation.CrossFade(name, 0.1f, mode);
			}
		}
	}
	
	public virtual int GetBaseAttackAnimation()
	{
		int id = this.battleAnimations.GetBaseAttackAnimation();
		if(DataHolder.BaseAttack(this.baseAttack[this.baIndex]).overrideAnimation)
		{
			id = DataHolder.BaseAttack(this.baseAttack[this.baIndex]).animationID;
		}
		return id;
	}
	
	public virtual int GetDefendAnimation()
	{
		return this.battleAnimations.GetDefendAnimation();
	}
	
	public virtual int GetEscapeAnimation()
	{
		return this.battleAnimations.GetEscapeAnimation();
	}
	
	public virtual int GetDeathAnimation()
	{
		return this.battleAnimations.GetDeathAnimation();
	}
	
	/*
	============================================================================
	Target functions
	============================================================================
	*/
	public void Blink(bool blink)
	{
		if(this.prefabInstance)
		{
			TargetBlinker comp = this.prefabInstance.GetComponent<TargetBlinker>();
			if(blink)
			{
				BattleMenu bm = DataHolder.BattleMenu();
				if(bm.useTargetCursor && "" != bm.cursorPrefabName)
				{
					if(!this.cursorInstance)
					{
						GameObject tmp = (GameObject)Resources.Load(BattleSystemData.PREFAB_PATH+bm.cursorPrefabName, typeof(GameObject));
						if(tmp) this.cursorInstance = (GameObject)GameObject.Instantiate(tmp);
					}
					if(this.cursorInstance)
					{
						this.cursorInstance.SetActiveRecursively(true);
						Transform t = TransformHelper.GetChild(bm.cursorChildName, this.prefabInstance.transform);
						this.cursorInstance.transform.position = t.position;
						this.cursorInstance.transform.parent = t;
						this.cursorInstance.transform.localPosition = bm.cursorOffset;
					}
				}
				if(bm.useTargetBlink)
				{
					if(!comp)
					{
						comp = this.prefabInstance.AddComponent<TargetBlinker>();
					}
					
					if(bm.fromCurrent)
					{
						comp.StartCoroutine(comp.BlinkCurrent(bm.blinkChildren, bm.aBlink, bm.aEnd, bm.rBlink, bm.rEnd,
								bm.gBlink, bm.gEnd, bm.bBlink, bm.bEnd, bm.blinkInterpolation, bm.blinkTime));
					}
					else
					{
						comp.StartCoroutine(comp.Blink(bm.blinkChildren, bm.aBlink, bm.aStart, bm.aEnd, bm.rBlink, bm.rStart, bm.rEnd,
								bm.gBlink, bm.gStart, bm.gEnd, bm.bBlink, bm.bStart, bm.bEnd, bm.blinkInterpolation, bm.blinkTime));
					}
				}
			}
			else
			{
				if(comp) comp.StopBlink();
				if(this.cursorInstance) this.cursorInstance.SetActiveRecursively(false);
			}
		}
	}
	
	public void LookAt(Combatant c)
	{
		if(c != null && c.prefabInstance != null)
		{
			this.LookAt(new Vector3(
					c.prefabInstance.transform.position.x,
					this.prefabInstance.transform.position.y,
					c.prefabInstance.transform.position.z));
		}
	}
	
	public void LookAt(Vector3 v)
	{
		if(this.prefabInstance != null)
		{
			this.prefabInstance.transform.LookAt(v);
		}
	}
	
	public int GetRandomTarget(Combatant[] cs)
	{
		int id = BattleAction.NONE;
		ArrayList targets = new ArrayList();
		for(int i=0; i<cs.Length; i++)
		{
			if(!cs[i].isDead)
			{
				targets.Add(cs[i]);
			}
		}
		if(targets.Count > 0)
		{
			id = ((Combatant)targets[Random.Range(0, targets.Count)]).battleID;
		}
		return id;
	}
	
	public GameObject GetRandomTargetObject(Combatant[] cs)
	{
		GameObject t = null;
		ArrayList targets = new ArrayList();
		for(int i=0; i<cs.Length; i++)
		{
			if(!cs[i].isDead)
			{
				targets.Add(cs[i]);
			}
		}
		if(targets.Count > 0)
		{
			t = ((Combatant)targets[Random.Range(0, targets.Count)]).prefabInstance;
		}
		return t;
	}
	
	public int GetNearestTarget(Combatant[] cs)
	{
		int id = BattleAction.NONE;
		float distance = Mathf.Infinity;
		for(int i=0; i<cs.Length; i++)
		{
			if(cs[i] != null && !cs[i].isDead &&
				cs[i] != this && cs[i].prefabInstance != null &&
				this.prefabInstance != null)
			{
				float tmp = Vector3.Distance(
						this.prefabInstance.transform.position, 
						cs[i].prefabInstance.transform.position);
				if(tmp < distance)
				{
					distance = tmp;
					id = cs[i].battleID;
				}
			}
		}
		return id;
	}
	
	public GameObject GetNearestTargetObject(Combatant[] cs)
	{
		GameObject t = null;
		float distance = Mathf.Infinity;
		for(int i=0; i<cs.Length; i++)
		{
			if(cs[i] != null && !cs[i].isDead &&
				cs[i] != this && cs[i].prefabInstance != null &&
				this.prefabInstance != null)
			{
				float tmp = Vector3.Distance(
						this.prefabInstance.transform.position, 
						cs[i].prefabInstance.transform.position);
				if(tmp < distance)
				{
					distance = tmp;
					t = cs[i].prefabInstance;
				}
			}
		}
		return t;
	}
	
	public Combatant[] GetCombatantsInRange(Combatant[] cs)
	{
		Combatant[] inRange = new Combatant[0];
		for(int i=0; i<cs.Length; i++)
		{
			if(cs[i] != null)
			{
				UseRange ur = new UseRange();
				if(ur.InRange(this, cs[i]))
				{
					inRange = ArrayHelper.Add(cs[i], inRange);
				}
			}
		}
		return inRange;
	}
	
	/*
	============================================================================
	AI functions
	============================================================================
	*/
	public void AddAIBehaviour()
	{
		this.aiBehaviour = ArrayHelper.Add(new AIBehaviour(), this.aiBehaviour);
	}
	
	public void RemoveAIBehaviour(int index)
	{
		this.aiBehaviour = ArrayHelper.Remove(index, this.aiBehaviour);
	}
	
	public void MoveAIBehaviour(int index, int change)
	{
		AIBehaviour tmp = this.aiBehaviour[index+change];
		this.aiBehaviour[index+change] = this.aiBehaviour[index];
		this.aiBehaviour[index] = tmp;
	}
	
	public BattleAction GetAIBehaviourAction(Combatant[] allies, Combatant[] enemies)
	{
		// get in battle range
		allies = this.GetCombatantsInRange(allies);
		enemies = this.GetCombatantsInRange(enemies);
		
		BattleAction action = new BattleAction();
		
		if(UseRange.InPlayerRange(this.prefabInstance, DataHolder.BattleSystem().aiRange))
		{
			if(this.rtCounterFlag >= 0)
			{
				action.type = AttackSelection.COUNTER;
				action.targetID = this.rtCounterFlag;
			}
			else
			{
				bool found = false;
				for(int i=0; i<this.aiBehaviour.Length; i++)
				{
					int valid = DataHolder.BattleAI(this.aiBehaviour[i].battleAI).IsValid(this.battleID, allies, enemies);
					if(valid != -1)
					{
						BattleAction action2 = this.aiBehaviour[i].GetAction(this, valid, allies, enemies);
						if(action2 != null)
						{
							action = action2;
							found = true;
							break;
						}
					}
				}
				
				// set up base attack if no condition occured
				if(!found)
				{
					if(this.IsBlockAttack() || enemies.Length == 0)
					{
						action.type = AttackSelection.NONE;
						action.targetID = this.battleID;
					}
					else
					{
						action.type = AttackSelection.ATTACK;
						if(this.attackPartyTarget && DataHolder.BattleControl().HasPartyTarget())
						{
							action.targetID = DataHolder.BattleControl().GetPartyTargetID();
						}
						else if(this.attackLastTarget && this.lastTargetBattleID >= 0 &&
							DataHolder.BattleSystem().CheckForID(this.lastTargetBattleID, enemies, true))
						{
							action.targetID = this.lastTargetBattleID;
						}
						else if(this.aiNearestTarget)
						{
							action.targetID = this.GetNearestTarget(enemies);
						}
						else
						{
							action.targetID = this.GetRandomTarget(enemies);
						}
					}
				}
			}
			
			action.user = this;
			if(!AttackSelection.NONE.Equals(action.type))
			{
				action.CheckRevive();
				if(!action.TargetAlive())
				{
					action.type = AttackSelection.NONE;
					action.targetID = this.battleID;
				}
				else if(DataHolder.BattleSystem().IsRealTime() && !action.InRange())
				{
					if(DataHolder.BattleSystem().IsRealTime() && action.InBattleRange() && 
						this.aiMover != null)
					{
						this.aiMover.SetTarget(this.GetNearestTargetObject(action.GetTargets(false)));
						this.aiAction = action;
						action = null;
					}
					else
					{
						action.type = AttackSelection.NONE;
						action.targetID = this.battleID;
					}
				}
				else if(this.aiMover != null) this.aiMover.StopFollow(true);
			}
			if(this.rtCounterFlag >= 0)
			{
				this.rtCounterFlag = -1;
				this.aiTimeoutTimer = 0;
			}
			else this.aiTimeoutTimer = this.aiTimeout;
		}
		else
		{
			action.type = AttackSelection.NONE;
			action.user = this;
			action.targetID = this.battleID;
			this.aiTimeoutTimer = DataHolder.BattleSystem().aiRecheckTime;
		}
		
		if(action != null && DataHolder.BattleSystem().IsActiveTime() && 
			this.usedTimeBar+action.GetTimeUse() > DataHolder.BattleSystem().maxTimebar)
		{
			action.type = AttackSelection.NONE;
			action.user = this;
			action.targetID = this.battleID;
		}
		
		if(action == null || AttackSelection.NONE.Equals(action.type))
		{
			this.EndTurn();
		}
		
		return action;
	}
	
	public void ClearAIAction()
	{
		if(this.aiAction != null)
		{
			this.aiAction = null;
			this.aiTimeoutTimer = this.aiTimeout;
			this.waitForAction = false;
		}
	}
}