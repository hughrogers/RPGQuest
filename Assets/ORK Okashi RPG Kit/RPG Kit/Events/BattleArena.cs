
using System.Collections;
using UnityEngine;

[AddComponentMenu("RPG Kit/Battles/Battle Arena")]
public class BattleArena : BaseInteraction
{
	// editor only
	public int gizmoMode = 0;
	
	// battle setup
	public bool canEscape = true;
	public bool noGameOver = false;
	public bool removeParty = true;
	public bool resetPartyPosition = true;
	
	// audio
	public AudioClip startSound;
	public float startVolume = 1;
	
	public bool playBattleMusic = true;
	public int battleMusic = 0;
	public MusicPlayType bmPlayType = MusicPlayType.PLAY;
	public float bmFadeTime = 1;
	public EaseType bmInterpolate = EaseType.Linear;
	
	public bool playVictoryMusic = true;
	public int victoryMusic = 0;
	public MusicPlayType vPlayType = MusicPlayType.PLAY;
	public float vFadeTime = 1;
	public EaseType vInterpolate = EaseType.Linear;
	
	public bool restoreLastMusic = true;
	public MusicPlayType lPlayType = MusicPlayType.PLAY;
	public float lFadeTime = 1;
	public EaseType lInterpolate = EaseType.Linear;
	
	// start cam setup
	public int baseCamPos = 0;
	
	public float startWait = 0;
	public bool startFadeScreen = true;
	public float startFadeOutTime = 0.3f;
	public float startFadeInTime = 1;
	public EaseType startFadeInterpolation = EaseType.Linear;
	
	public bool startFadeAlpha = true;
	public float startFadeAStart = 0;
	public float startFadeAEnd = 1;
	
	public bool startFadeRed = false;
	public float startFadeRStart = 0;
	public float startFadeREnd = 0;
	
	public bool startFadeGreen = false;
	public float startFadeGStart = 0;
	public float startFadeGEnd = 0;
	
	public bool startFadeBlue = false;
	public float startFadeBStart = 0;
	public float startFadeBEnd = 0;
	
	public bool fadeCamPos = false;
	public bool setStartCamPos = false;
	public int startCamPos = 0;
	public float startPosTime = 1;
	public EaseType startCamInterpolation = EaseType.Linear;
	
	// end cam setup
	public float endWait = 0;
	public bool endFadeScreen = true;
	public float endFadeOutTime = 0.3f;
	public float endFadeInTime = 1;
	public EaseType endFadeInterpolation = EaseType.Linear;
	
	public bool endFadeAlpha = true;
	public float endFadeAStart = 0;
	public float endFadeAEnd = 1;
	
	public bool endFadeRed = false;
	public float endFadeRStart = 0;
	public float endFadeREnd = 0;
	
	public bool endFadeGreen = false;
	public float endFadeGStart = 0;
	public float endFadeGEnd = 0;
	
	public bool endFadeBlue = false;
	public float endFadeBStart = 0;
	public float endFadeBEnd = 0;
	
	public bool fadeEndCamPos = false;
	public bool setEndCamPos = false;
	public int endCamPos = 0;
	public float endPosTime = 1;
	public EaseType endCamInterpolation = EaseType.Linear;
	
	// party
	public Transform[] partySpot = new Transform[0];
	public Transform[] partySpotPA = new Transform[0];
	public Transform[] partySpotEA = new Transform[0];
	// enemies
	public int[] enemy = new int[1];
	public Transform[] enemySpot = new Transform[1];
	public Transform[] enemySpotPA = new Transform[1];
	public Transform[] enemySpotEA = new Transform[1];
	public bool[] spawnEnemy = new bool[] {true};
	public GameObject[] enemyObject = new GameObject[1];
	
	// money gain
	public int moneyGain = 0;
	// item drops
	public ItemDropType[] gainType = new ItemDropType[0];
	public int[] gainID = new int[0];
	public int[] gainQuantity = new int[0];
	public float[] gainChance = new float[0];
	
	// battle advantages
	public bool enablePartyAdvantage = true;
	public bool overridePAChance = false;
	public float paChance = 0;
	public bool enableEnemiesAdvantage = true;
	public bool overrideEAChance = false;
	public float eaChance = 0;
	
	// ingame
	private bool battleRunning = false;
	
	public Vector3 initialCamPosition;
	public Quaternion initialCamRotation;
	public float initialFieldOfView;
	
	private Transform[] usedPartySpot;
	private Transform[] usedEnemySpot;
	
	// reset pos
	private bool[] resetPos;
	private Vector3[] partyPos;
	private Quaternion[] partyQuat;
	
	private float battleStartTime = 0;
	
	/*
	============================================================================
	Inspector functions
	============================================================================
	*/
	public void AddEnemy()
	{
		this.enemy = ArrayHelper.Add(0, this.enemy);
		this.enemySpot = ArrayHelper.Add(null, this.enemySpot);
		this.enemySpotPA = ArrayHelper.Add(null, this.enemySpotPA);
		this.enemySpotEA = ArrayHelper.Add(null, this.enemySpotEA);
		this.spawnEnemy = ArrayHelper.Add(true, this.spawnEnemy);
		this.enemyObject = ArrayHelper.Add(null, this.enemyObject);
	}
	
	public void RemoveEnemy(int index)
	{
		this.enemy = ArrayHelper.Remove(index, this.enemy);
		this.enemySpot = ArrayHelper.Remove(index, this.enemySpot);
		this.enemySpotEA = ArrayHelper.Remove(index, this.enemySpotPA);
		this.spawnEnemy = ArrayHelper.Remove(index, this.spawnEnemy);
		this.enemyObject = ArrayHelper.Remove(index, this.enemyObject);
	}
	
	public void AddGain()
	{
		this.gainType = ArrayHelper.Add(ItemDropType.ITEM, this.gainType);
		this.gainID = ArrayHelper.Add(0, this.gainID);
		this.gainQuantity = ArrayHelper.Add(1, this.gainQuantity);
		this.gainChance = ArrayHelper.Add(100, this.gainChance);
	}
	
	public void RemoveGain(int index)
	{
		this.gainType = ArrayHelper.Remove(index, this.gainType);
		this.gainID = ArrayHelper.Remove(index, this.gainID);
		this.gainQuantity = ArrayHelper.Remove(index, this.gainQuantity);
		this.gainChance = ArrayHelper.Remove(index, this.gainChance);
	}
	
	/*
	============================================================================
	Spot functions
	============================================================================
	*/
	public Vector3 GetPartySpotPosition(int index, int advantage)
	{
		if((advantage == BattleAdvantage.PARTY && 
			(!DataHolder.BattleSystem().enablePASpots || !this.enablePartyAdvantage)) ||
			(advantage == BattleAdvantage.ENEMIES && 
			(!DataHolder.BattleSystem().enableEASpots || !this.enableEnemiesAdvantage)))
		{
			advantage = BattleAdvantage.NONE;
		}
		
		if(index < this.partySpot.Length && 
			((advantage == BattleAdvantage.NONE && this.partySpot[index] != null) ||
			(advantage == BattleAdvantage.PARTY && this.partySpotPA[index] != null) ||
			(advantage == BattleAdvantage.ENEMIES && this.partySpotEA[index] != null)))
		{
			if(advantage == BattleAdvantage.PARTY) return this.partySpotPA[index].position;
			else if(advantage == BattleAdvantage.ENEMIES) return this.partySpotEA[index].position;
			else return this.partySpot[index].position;
		}
		else if(index < DataHolder.BattleSystem().partySpot.Length)
		{
			if(advantage == BattleAdvantage.PARTY)
			{
				return this.transform.TransformPoint(DataHolder.BattleSystem().partySpotPA[index]);
			}
			else if(advantage == BattleAdvantage.ENEMIES)
			{
				return this.transform.TransformPoint(DataHolder.BattleSystem().partySpotEA[index]);
			}
			else
			{
				return this.transform.TransformPoint(DataHolder.BattleSystem().partySpot[index]);
			}
		}
		else return this.transform.position;
	}
	
	public Vector3 GetEnemySpotPosition(int index, int advantage)
	{
		if((advantage == BattleAdvantage.PARTY && 
			(!DataHolder.BattleSystem().enablePASpots || !this.enablePartyAdvantage)) ||
			(advantage == BattleAdvantage.ENEMIES && 
			(!DataHolder.BattleSystem().enableEASpots || !this.enableEnemiesAdvantage)))
		{
			advantage = BattleAdvantage.NONE;
		}
		
		if(index < this.enemySpot.Length && 
			((advantage == BattleAdvantage.NONE && this.enemySpot[index] != null) ||
			(advantage == BattleAdvantage.PARTY && this.enemySpotPA[index] != null) ||
			(advantage == BattleAdvantage.ENEMIES && this.enemySpotEA[index] != null)))
		{
			if(advantage == BattleAdvantage.PARTY) return this.enemySpotPA[index].position;
			else if(advantage == BattleAdvantage.ENEMIES) return this.enemySpotEA[index].position;
			else return this.enemySpot[index].position;
		}
		else if(index < DataHolder.BattleSystem().enemySpot.Length)
		{
			if(advantage == BattleAdvantage.PARTY)
			{
				return this.transform.TransformPoint(DataHolder.BattleSystem().enemySpotPA[index]);
			}
			else if(advantage == BattleAdvantage.ENEMIES)
			{
				return this.transform.TransformPoint(DataHolder.BattleSystem().enemySpotEA[index]);
			}
			else
			{
				return this.transform.TransformPoint(DataHolder.BattleSystem().enemySpot[index]);
			}
		}
		else return this.transform.position;
	}
	
	/*
	============================================================================
	Enemies functions
	============================================================================
	*/
	public void SetEnemyTeam(int[] id)
	{
		this.enemy = id;
		this.enemySpot = this.CopySpots(this.enemySpot, this.enemy.Length);
		this.enemySpotPA = this.CopySpots(this.enemySpotPA, this.enemy.Length);
		this.enemySpotEA = this.CopySpots(this.enemySpotEA, this.enemy.Length);
		this.spawnEnemy = new bool[this.enemy.Length];
		this.enemyObject = new GameObject[this.enemy.Length];
		
		for(int i=0; i<this.enemy.Length; i++)
		{
			this.spawnEnemy[i] = true;
		}
	}
	
	private Transform[] CopySpots(Transform[] spots, int length)
	{
		Transform[] tmp = spots;
		spots = new Transform[length];
		System.Array.Copy(tmp, spots, Mathf.Min(tmp.Length, length));
		return spots;
	}
	
	/*
	============================================================================
	Start/end functions
	============================================================================
	*/
	void Awake()
	{
		if(this.autoDestroyOnVariables && !this.CheckVariables())
		{
			for(int i=0; i<this.enemyObject.Length; i++)
			{
				if(this.enemyObject[i] != null)
				{
					GameObject.Destroy(this.enemyObject[i]);
				}
			}
			GameObject.Destroy(this.gameObject);
		}
		else
		{
			for(int i=0; i<this.enemyObject.Length; i++)
			{
				if(this.enemyObject[i] != null)
				{
					Enemy e = DataHolder.Enemies().GetCopy(this.enemy[i]);
					e.Init();
					e.AddComponents(this.enemyObject[i]);
				}
			}
		}
	}
	
	void Update()
	{
		if(this.KeyPress()) StartCoroutine(StartBattle());
		if(!GameHandler.IsGamePaused() && this.battleRunning)
		{
			DataHolder.BattleSystem().Tick();
		}
	}
	
	void Start()
	{
		// start battle when autostart
		if(EventStartType.AUTOSTART.Equals(this.startType) && this.CheckVariables())
		{
			StartCoroutine(StartBattle());
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(this.CheckTriggerEnter(other))
		{
			StartCoroutine(StartBattle());
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if(this.CheckTriggerExit(other))
		{
			StartCoroutine(StartBattle());
		}
	}
	
	public override void TouchInteract()
	{
		this.OnMouseUp();
	}
	
	void OnMouseUp()
	{
		if(EventStartType.INTERACT.Equals(this.startType) &&
			this.CheckVariables() && this.gameObject.active && DataHolder.GameSettings().IsMouseAllowed())
		{
			GameObject p = GameHandler.GetPlayer();
			if(p && Vector3.Distance(p.transform.position, this.transform.position) < this.maxMouseDistance)
			{
				StartCoroutine(StartBattle());
			}
		}
	}
	
	public override bool Interact()
	{
		bool val = false;
		// start event on interaction here
		if(EventStartType.INTERACT.Equals(this.startType) && GameHandler.IsControlField() &&
			this.CheckVariables() && this.gameObject.active)
		{
			StartCoroutine(StartBattle());
			val = true;
		}
		return val;
	}
	
	public override bool DropInteract(ChoiceContent drop)
	{
		bool val = false;
		if(EventStartType.DROP.Equals(this.startType) && GameHandler.IsControlField() &&
			this.CheckVariables() && this.gameObject.active && this.CheckDrop(drop))
		{
			StartCoroutine(StartBattle());
			val = true;
		}
		return val;
	}
	
	public void CallStart()
	{
		if(this.CheckVariables())
		{
			StartCoroutine(StartBattle());
		}
	}
	
	public IEnumerator StartBattle()
	{
		if(!this.battleRunning)
		{
			GameHandler.SetControlType(ControlType.NONE);
			DataHolder.BattleSystem().SetBattleArena(this);
			
			this.usedPartySpot = new Transform[this.partySpot.Length];
			this.usedEnemySpot = new Transform[this.enemySpot.Length];
			
			for(int i=0; i<this.usedPartySpot.Length; i++)
			{
				Vector3 pos = this.GetPartySpotPosition(i, DataHolder.BattleSystem().battleAdvantage);
				GameObject sp = new GameObject("partySpot"+i);
				this.usedPartySpot[i] = sp.transform;
				this.usedPartySpot[i].position = pos;
				this.usedPartySpot[i].parent = this.transform;
				if(DataHolder.BattleSystem().spotOnGround)
				{
					PlaceOnGround pog = sp.AddComponent<PlaceOnGround>();
					pog.distance = DataHolder.BattleSystem().spotDistance;
					pog.layerMask = 1 << DataHolder.BattleSystem().spotLayer;
					pog.offset = DataHolder.BattleSystem().spotOffset;
				}
			}
			for(int i=0; i<this.usedEnemySpot.Length; i++)
			{
				Vector3 pos = this.GetEnemySpotPosition(i, DataHolder.BattleSystem().battleAdvantage);
				GameObject sp = new GameObject("enemySpot"+i);
				this.usedEnemySpot[i] = sp.transform;
				this.usedEnemySpot[i].position = pos;
				this.usedEnemySpot[i].parent = this.transform;
				if(DataHolder.BattleSystem().spotOnGround)
				{
					PlaceOnGround pog = sp.AddComponent<PlaceOnGround>();
					pog.distance = DataHolder.BattleSystem().spotDistance;
					pog.layerMask = 1 << DataHolder.BattleSystem().spotLayer;
					pog.offset = DataHolder.BattleSystem().spotOffset;
					pog.Place();
				}
			}
			
			this.SetMessageTime();
			DataHolder.BattleSystem().ShowBattleStartMessage();
			
			if(this.startSound)
			{
				if(!this.audio)
				{
					this.gameObject.AddComponent("AudioSource");
				}
				this.audio.PlayOneShot(this.startSound, this.startVolume);
			}
			
			if(this.restoreLastMusic)
			{
				GameHandler.GetMusicHandler().StoreCurrent();
			}
			if(this.playBattleMusic)
			{
				if(MusicPlayType.PLAY.Equals(this.bmPlayType))
				{
					GameHandler.GetMusicHandler().Play(this.battleMusic);
				}
				else if(MusicPlayType.FADE_IN.Equals(this.bmPlayType))
				{
					GameHandler.GetMusicHandler().FadeIn(this.battleMusic, this.bmFadeTime, this.bmInterpolate);
				}
				else if(MusicPlayType.FADE_TO.Equals(this.bmPlayType))
				{
					GameHandler.GetMusicHandler().FadeTo(this.battleMusic, this.bmFadeTime, this.bmInterpolate);
				}
			}
			
			Transform cam = null;
			if(Camera.main)
			{
				cam = Camera.main.transform;
				this.initialCamPosition = cam.position;
				this.initialCamRotation = cam.rotation;
				this.initialFieldOfView = cam.camera.fieldOfView;
			}
			
			float tmpWait = 0;
			
			if(this.startWait > 0)
			{
				this.battleStartTime -= this.startWait;
				yield return new WaitForSeconds(this.startWait);
			}
			
			if(this.startFadeScreen)
			{
				GameHandler.GetLevelHandler().screenFader.FadeScreen(this.startFadeAlpha, this.startFadeAStart, this.startFadeAEnd, 
										this.startFadeRed, this.startFadeRStart, this.startFadeREnd, 
										this.startFadeGreen, this.startFadeGStart, this.startFadeGEnd, 
										this.startFadeBlue, this.startFadeBStart, this.startFadeBEnd, this.startFadeInterpolation, this.startFadeOutTime);
				this.battleStartTime -= this.startFadeOutTime;
				yield return new WaitForSeconds(this.startFadeOutTime);
			}
			GameHandler.SetControlType(ControlType.BATTLE);
			
			yield return null;
			// whole party pos handling
			Character[] party = GameHandler.Party().GetParty();
			if(this.resetPartyPosition)
			{
				this.resetPos = new bool[party.Length];
				this.partyPos = new Vector3[party.Length];
				this.partyQuat = new Quaternion[party.Length];
				for(int i=0; i<party.Length; i++)
				{
					if(party[i].prefabInstance)
					{
						this.resetPos[i] = true;
						this.partyPos[i] = party[i].prefabInstance.transform.position;
						this.partyQuat[i] = party[i].prefabInstance.transform.rotation;
						if(!GameHandler.Party().HasJoinedBattleParty(party[i].realID))
						{
							party[i].DestroyPrefab();
						}
					}
				}
			}
			
			// battle party pos handling
			party = GameHandler.Party().GetBattleParty();
			for(int i=0; i<party.Length; i++)
			{
				if(party[i].prefabInstance == null)
				{
					party[i].CreatePrefabInstance();
				}
				if(party[i].prefabInstance)
				{
					if(this.usedPartySpot[i])
					{
						party[i].prefabInstance.transform.position = this.usedPartySpot[i].position;
						party[i].SetBattleSpot(this.usedPartySpot[i]);
					}
					if(party[i].isDead) party[i].PlayAnimation(CombatantAnimation.DEATH, PlayMode.StopAll);
				}
			}
			
			Enemy[] enemies = new Enemy[this.enemy.Length];
			for(int i=0; i<this.enemy.Length; i++)
			{
				enemies[i] = DataHolder.Enemies().GetCopy(this.enemy[i]);
				enemies[i].Init();
				if(this.spawnEnemy[i])
				{
					this.enemyObject[i] = enemies[i].CreatePrefabInstance();
				}
				else if(this.enemyObject[i])
				{
					if(!this.enemyObject[i].active) this.enemyObject[i].SetActiveRecursively(true);
					enemies[i].prefabInstance = this.enemyObject[i];
					CombatantClick cc = this.enemyObject[i].GetComponent<CombatantClick>();
					if(cc != null)
					{
						enemies[i] = cc.combatant as Enemy;
					}
					else
					{
						FieldAnimator ba = enemies[i].prefabInstance.GetComponent<FieldAnimator>();
						if(ba != null) ba.SetCombatant(enemies[i]);
					}
				}
				if(this.enemyObject[i])
				{
					if(this.enemyObject[i].GetComponent("CombatantClick") == null)
					{
						CombatantClick cc = (CombatantClick)this.enemyObject[i].AddComponent("CombatantClick");
						cc.combatant = enemies[i];
					}
					if(this.usedEnemySpot[i])
					{
						this.enemyObject[i].transform.position = this.usedEnemySpot[i].position;
						enemies[i].SetBattleSpot(this.usedEnemySpot[i]);
					}
				}
			}
			// look setup
			DataHolder.BattleSystem().DoLookAt(party, enemies);
			DataHolder.BattleSystem().SetupBattle(party, enemies, this.canEscape);
			
			yield return null;
			
			if(cam && this.fadeCamPos)
			{
				if(this.setStartCamPos) DataHolder.CameraPosition(this.startCamPos).Use(cam, this.transform);
				CameraEventMover moveComp = (CameraEventMover)cam.gameObject.GetComponent("CameraEventMover");
				if(moveComp == null)
				{
					moveComp = (CameraEventMover)cam.gameObject.AddComponent("CameraEventMover");
				}
				moveComp.StartCoroutine(moveComp.SetTargetData(DataHolder.CameraPosition(this.baseCamPos), cam, this.transform, this.startCamInterpolation, this.startPosTime));
				tmpWait += this.startPosTime;
			}
			else if(cam)
			{
				this.SetBaseCameraPosition(cam);
			}
			
			if(this.startFadeScreen)
			{
				GameHandler.GetLevelHandler().screenFader.FadeScreen(this.startFadeAlpha, this.startFadeAEnd, this.startFadeAStart, 
										this.startFadeRed, this.startFadeREnd, this.startFadeRStart, 
										this.startFadeGreen, this.startFadeGEnd, this.startFadeGStart, 
										this.startFadeBlue, this.startFadeBEnd, this.startFadeBStart, this.startFadeInterpolation, this.startFadeInTime);
				if(tmpWait < this.startFadeInTime) tmpWait = this.startFadeInTime;
			}
			if(tmpWait > this.battleStartTime) this.battleStartTime = tmpWait;
			yield return new WaitForSeconds(this.battleStartTime);
			
			this.battleRunning = true;
			DataHolder.BattleSystem().StartBattle();
		}
	}
	
	public IEnumerator BattleFinished()
	{
		this.SetVariables();
		this.battleStartTime = 0;
		float tmpWait = 0;
		if(this.endWait > 0)
		{
			yield return new WaitForSeconds(this.endWait);
		}
		
		if(this.restoreLastMusic)
		{
			if(MusicPlayType.PLAY.Equals(this.lPlayType))
			{
				GameHandler.GetMusicHandler().PlayStored();
			}
			else if(MusicPlayType.FADE_IN.Equals(this.lPlayType))
			{
				GameHandler.GetMusicHandler().FadeInStored(this.lFadeTime, this.lInterpolate);
			}
			else if(MusicPlayType.FADE_TO.Equals(this.lPlayType))
			{
				GameHandler.GetMusicHandler().FadeToStored(this.lFadeTime, this.lInterpolate);
			}
		}
		
		Transform cam = null;
		if(Camera.main)
		{
			cam = Camera.main.transform;
		}
		if(this.endFadeScreen)
		{
			GameHandler.GetLevelHandler().screenFader.FadeScreen(this.endFadeAlpha, this.endFadeAStart, this.endFadeAEnd, 
									this.endFadeRed, this.endFadeRStart, this.endFadeREnd, 
									this.endFadeGreen, this.endFadeGStart, this.endFadeGEnd, 
									this.endFadeBlue, this.endFadeBStart, this.endFadeBEnd, this.endFadeInterpolation, this.endFadeOutTime);
			this.battleStartTime -= this.endFadeOutTime;
			yield return new WaitForSeconds(this.endFadeOutTime);
		}
		
		GameHandler.SetControlType(ControlType.NONE);
		for(int i=0; i<this.enemyObject.Length; i++)
		{
			if(this.enemyObject[i])
			{
				GameObject.Destroy(this.enemyObject[i]);
			}
		}
		
		Character[] party = GameHandler.Party().GetBattleParty();
		for(int i=0; i<party.Length; i++)
		{
			if(party[i].prefabInstance)
			{
				if(this.removeParty && party[i].realID != GameHandler.Party().GetPlayerID())
				{
					party[i].DestroyPrefab();
				}
			}
			if(!party[i].isDead && DataHolder.BattleSystem().endBattleStatusSettings)
			{
				for(int j=0; j<DataHolder.BattleSystem().endSetStatus.Length; j++)
				{
					if(DataHolder.BattleSystem().endSetStatus[j])
					{
						party[i].status[j].SetValue(DataHolder.BattleSystem().endStatus[j], true, false, false);
					}
				}
			}
			if(party[i].isDead && DataHolder.BattleSystem().reviveAfterBattle)
			{
				for(int j=0; j<DataHolder.BattleSystem().reviveSetStatus.Length; j++)
				{
					if(DataHolder.BattleSystem().reviveSetStatus[j])
					{
						party[i].status[j].SetValue(DataHolder.BattleSystem().reviveStatus[j], true, false, false);
					}
				}
				party[i].isDead = false;
			}
		}
		
		// whole party pos handling
		if(this.resetPartyPosition)
		{
			party = GameHandler.Party().GetParty();
			for(int i=0; i<this.resetPos.Length; i++)
			{
				if(this.resetPos[i])
				{
					if(party[i].prefabInstance == null)
					{
						party[i].CreatePrefabInstance();
					}
					party[i].prefabInstance.transform.position = this.partyPos[i];
					party[i].prefabInstance.transform.rotation = this.partyQuat[i];
				}
			}
		}
		
		if(cam && this.fadeEndCamPos)
		{
			if(this.setEndCamPos) DataHolder.CameraPosition(this.endCamPos).Use(cam, this.transform);
			CameraEventMover moveComp = (CameraEventMover)cam.gameObject.GetComponent("CameraEventMover");
			if(moveComp == null)
			{
				moveComp = (CameraEventMover)cam.gameObject.AddComponent("CameraEventMover");
			}
			moveComp.StartCoroutine(moveComp.SetTargetData(this.initialCamPosition, this.initialCamRotation, this.initialFieldOfView, cam, this.endCamInterpolation, this.endPosTime));
			tmpWait += this.endPosTime;
		}
		else if(cam)
		{
			this.ResetCameraPosition(cam);
		}
		
		if(this.endFadeScreen)
		{
			GameHandler.GetLevelHandler().screenFader.FadeScreen(this.endFadeAlpha, this.endFadeAEnd, this.endFadeAStart, 
									this.endFadeRed, this.endFadeREnd, this.endFadeRStart, 
									this.endFadeGreen, this.endFadeGEnd, this.endFadeGStart, 
									this.endFadeBlue, this.endFadeBEnd, this.endFadeBStart, this.endFadeInterpolation, this.endFadeInTime);
			if(tmpWait < this.endFadeInTime) tmpWait = this.endFadeInTime;
		}
		if(tmpWait > this.battleStartTime) this.battleStartTime = tmpWait;
		yield return new WaitForSeconds(this.battleStartTime);
		
		// destroy temporary battle spots
		for(int i=0; i<this.usedPartySpot.Length; i++)
		{
			if(this.usedPartySpot[i] != null)
			{
				GameObject.Destroy(this.usedPartySpot[i].gameObject);
			}
		}
		for(int i=0; i<this.usedEnemySpot.Length; i++)
		{
			if(this.usedEnemySpot[i] != null)
			{
				GameObject.Destroy(this.usedEnemySpot[i].gameObject);
			}
		}
		
		this.battleRunning = false;
		GameHandler.SetControlType(ControlType.FIELD);
		if(this.deactivateAfter)
		{
			this.gameObject.SetActiveRecursively(false);
		}
		Resources.UnloadUnusedAssets();
	}
	
	/*
	============================================================================
	Camera control functions
	============================================================================
	*/
	public void SetBaseCameraPosition(Transform cam)
	{
		DataHolder.CameraPosition(this.baseCamPos).Use(cam, this.transform);
	}
	
	public void ResetCameraPosition(Transform cam)
	{
		cam.position = this.initialCamPosition;
		cam.rotation = this.initialCamRotation;
		cam.camera.fieldOfView = this.initialFieldOfView;
	}
	
	/*
	============================================================================
	Battle message functions
	============================================================================
	*/
	public void SetMessageTime()
	{
		this.battleStartTime = DataHolder.BattleSystemData().battleMessageShowTime;
	}
	
	public void BattleVictory()
	{
		this.SetMessageTime();
		
		if(this.playVictoryMusic)
		{
			if(MusicPlayType.PLAY.Equals(this.vPlayType))
			{
				GameHandler.GetMusicHandler().Play(this.victoryMusic);
			}
			else if(MusicPlayType.FADE_IN.Equals(this.vPlayType))
			{
				GameHandler.GetMusicHandler().FadeIn(this.victoryMusic, this.vFadeTime, this.vInterpolate);
			}
			else if(MusicPlayType.FADE_TO.Equals(this.vPlayType))
			{
				GameHandler.GetMusicHandler().FadeTo(this.victoryMusic, this.vFadeTime, this.vInterpolate);
			}
		}
	}
	
	public IEnumerator BattleLost()
	{
		this.SetMessageTime();
		GameHandler.GetMusicHandler().Stop();
		yield return new WaitForSeconds(DataHolder.BattleSystemData().battleMessageShowTime);
		if(this.noGameOver)
		{
			this.BattleFinished();
		}
		else
		{
			GameHandler.GameOver();
		}
	}
	
	/*
	============================================================================
	Gizmos functions
	============================================================================
	*/
	void OnApplicationQuit()
	{
		DataHolder.BattleSystem().EndBattle();
	}
	
	void OnDrawGizmosSelected()
	{
		for(int i=0; i<this.spawnEnemy.Length; i++)
		{
			if(!this.spawnEnemy[i] && this.enemyObject[i] && 
				(this.enemySpot[i] != null || i < DataHolder.BattleSystem().enemySpot.Length))
			{
				Gizmos.color = new Color(1, 0, 0);
				Gizmos.DrawLine(this.GetEnemySpotPosition(i, this.gizmoMode), this.enemyObject[i].transform.position);
			}
		}
		for(int i=0; i<this.enemySpot.Length; i++)
		{
			if(this.enemySpot[i] != null ||
				i < DataHolder.BattleSystem().enemySpot.Length)
			{
				Gizmos.color = new Color(1, 0, 0);
				Gizmos.DrawWireCube(this.GetEnemySpotPosition(i, this.gizmoMode), new Vector3(0.5f, 0.5f, 0.5f));
			}
		}
		for(int i=0; i<this.partySpot.Length; i++)
		{
			if(this.partySpot[i] != null ||
				i < DataHolder.BattleSystem().partySpot.Length)
			{
				Gizmos.color = new Color(0, 1, 1);
				Gizmos.DrawWireCube(this.GetPartySpotPosition(i, this.gizmoMode), new Vector3(0.5f, 0.5f, 0.5f));
			}
		}
	}
	
	void OnDrawGizmos()
	{
		Gizmos.DrawIcon(transform.position, "BattleArena.psd");
	}
}