
using System.Collections;
using UnityEngine;

public enum GameEventType {
		// base steps
		WAIT, GO_TO, RANDOM_STEP, CHECK_RANDOM, CHECK_FORMULA, 
		END_EVENT, LOAD_SCENE, WAIT_FOR_BUTTON, CHECK_DIFFICULTY, 
		// spawn steps
		SPAWN_PLAYER, DESTROY_PLAYER, SPAWN_PREFAB, DESTROY_PREFAB, 
		TELEPORT, 
		// dialogue steps
		SHOW_DIALOGUE, SHOW_CHOICE, TELEPORT_CHOICE, 
		// function steps
		SEND_MESSAGE, BROADCAST_MESSAGE, 
		ADD_COMPONENT, REMOVE_COMPONENT, 
		ACTIVATE_OBJECT, OBJECT_VISIBLE, 
		PARENT_OBJECT, CALL_GLOBAL_EVENT, 
		// statistic steps
		CUSTOM_STATISTIC, CLEAR_STATISTIC, 
		// camera steps
		SET_CAMERA_POSITION, FADE_CAMERA_POSITION, 
		SET_INITIAL_CAMPOS, FADE_TO_INITIAL_CAMPOS, 
		SHAKE_CAMERA, ROTATE_CAMERA_AROUND, 
		// move steps
		SET_TO_POSITION, 
		MOVE_TO_WAYPOINT, MOVE_TO_DIRECTION, MOVE_TO_PREFAB, 
		// rotate steps
		ROTATE_TO_WAYPOINT, ROTATION, 
		// animation steps
		PLAY_ANIMATION, STOP_ANIMATION, 
		// audio steps
		PLAY_SOUND, STOP_SOUND, PLAY_MUSIC, STORE_MUSIC, 
		// fade steps
		FADE_OBJECT, FADE_CAMERA, 
		// party steps
		JOIN_PARTY, LEAVE_PARTY, IS_IN_PARTY, HAS_LEFT_PARTY, 
		CHECK_PLAYER, SET_PLAYER, 
		SET_CHARACTER_NAME, 
		// battle party steps
		JOIN_BATTLE_PARTY, LEAVE_BATTLE_PARTY, IS_IN_BATTLE_PARTY, 
		LOCK_BP_MEMBER, UNLOCK_BP_MEMBER, IS_LOCKED_BP_MEMBER, 
		// status steps
		REGENERATE, CHECK_STATUS_VALUE, SET_STATUS_VALUE, 
		CHANGE_STATUS_EFFECT, 
		LEVEL_UP, CHECK_LEVEL, 
		// class steps
		CHECK_CLASS, CHANGE_CLASS, 
		// skill steps
		LEARN_SKILL, FORGET_SKILL, HAS_SKILL, 
		// equip steps
		EQUIP_WEAPON, EQUIP_ARMOR, UNEQUIP, 
		// item steps
		ADD_ITEM, REMOVE_ITEM, CHECK_ITEM, 
		LEARN_ITEM_RECIPE, ITEM_RECIPE_KNOWN, 
		// weapon steps
		ADD_WEAPON, REMOVE_WEAPON, CHECK_WEAPON, 
		// armor steps
		ADD_ARMOR, REMOVE_ARMOR, CHECK_ARMOR, 
		// money steps
		ADD_MONEY, REMOVE_MONEY, SET_MONEY, CHECK_MONEY, 
		// variable steps
		SET_VARIABLE, REMOVE_VARIABLE, CHECK_VARIABLE,
		SET_NUMBER_VARIABLE, REMOVE_NUMBER_VARIABLE, CHECK_NUMBER_VARIABLE, 
		SET_PLAYERPREFS, GET_PLAYERPREFS, HAS_PLAYERPREFS
		};

public class GameEvent : BaseEvent
{
	public bool blockControls = true;
	public bool mainCamera  = true;
	public EventActor[] actor = new EventActor[0];
	public EventStep[] step = new EventStep[0];
	
	// waypoints
	public int waypoints = 0;
	public Transform[] waypoint = new Transform[0];
	
	// prefabs
	public int prefabs = 0;
	public GameObject[] prefab = new GameObject[0];
	public Hashtable spawnedPrefabs = new Hashtable();
	
	// audio clips
	public int audioClips = 0;
	public AudioClip[] audioClip = new AudioClip[0];
	
	// editor
	public bool hideButtons = false;
	
	// ingame
	private EventInteraction interaction;
	private GlobalEvent globalEvent;
	private GlobalEvent calledEvent;
	public bool executing = false;
	public int currentStep = 0;
	
	// time handling
	public bool timeRunning = false;
	public bool stepFinished = false;
	public float time = 0.0f;
	
	// camera settings
	public Transform cam;
	public Vector3 initialCamPosition;
	public Quaternion initialCamRotation;
	public float initialFieldOfView;
	
	// teleport event
	private int[] teleportID = new int[0];
	private bool teleportChoice = false;
	private bool teleportCancel = false;
	
	// button press
	private string button = "";
	private int buttonNext = 0;
	
	public GameEvent()
	{
		
	}
	
	public EventStep TypeToStep(GameEventType t)
	{
		EventStep s = null;
		
		if(GameEventType.WAIT.Equals(t)) s = new WaitStep(t);
		else if(GameEventType.GO_TO.Equals(t)) s = new GoToStep(t);
		else if(GameEventType.RANDOM_STEP.Equals(t)) s = new RandomStep(t);
		else if(GameEventType.CHECK_RANDOM.Equals(t)) s = new CheckRandomStep(t);
		else if(GameEventType.CHECK_FORMULA.Equals(t)) s = new CheckFormulaStep(t);
		else if(GameEventType.END_EVENT.Equals(t)) s = new EndEventStep(t);
		else if(GameEventType.LOAD_SCENE.Equals(t)) s = new LoadSceneStep(t);
		else if(GameEventType.WAIT_FOR_BUTTON.Equals(t)) s = new WaitForButtonStep(t);
		else if(GameEventType.CHECK_DIFFICULTY.Equals(t)) s = new CheckDifficultyStep(t);
		// spawn steps
		else if(GameEventType.SPAWN_PLAYER.Equals(t)) s = new SpawnPlayerStep(t);
		else if(GameEventType.DESTROY_PLAYER.Equals(t)) s = new DestroyPlayerStep(t);
		else if(GameEventType.SPAWN_PREFAB.Equals(t)) s = new SpawnPrefabStep(t);
		else if(GameEventType.DESTROY_PREFAB.Equals(t)) s = new DestroyPrefabStep(t);
		else if(GameEventType.TELEPORT.Equals(t)) s = new TeleportStep(t);
		// dialogue steps
		else if(GameEventType.SHOW_DIALOGUE.Equals(t)) s = new ShowDialogueStep(t);
		else if(GameEventType.SHOW_CHOICE.Equals(t)) s = new ShowChoiceStep(t);
		else if(GameEventType.TELEPORT_CHOICE.Equals(t)) s = new TeleportChoiceStep(t);
		// function steps
		else if(GameEventType.SEND_MESSAGE.Equals(t)) s = new SendMessageStep(t);
		else if(GameEventType.BROADCAST_MESSAGE.Equals(t)) s = new BroadcastMessageStep(t);
		else if(GameEventType.ADD_COMPONENT.Equals(t)) s = new AddComponentStep(t);
		else if(GameEventType.REMOVE_COMPONENT.Equals(t)) s = new RemoveComponentStep(t);
		else if(GameEventType.ACTIVATE_OBJECT.Equals(t)) s = new ActivateObjectStep(t);
		else if(GameEventType.OBJECT_VISIBLE.Equals(t)) s = new ObjectVisibleStep(t);
		else if(GameEventType.PARENT_OBJECT.Equals(t)) s = new ParentObjectStep(t);
		else if(GameEventType.CALL_GLOBAL_EVENT.Equals(t)) s = new CallGlobalEventStep(t);
		// statistic steps
		else if(GameEventType.CUSTOM_STATISTIC.Equals(t)) s = new CustomStatisticStep(t);
		else if(GameEventType.CLEAR_STATISTIC.Equals(t)) s = new ClearStatisticStep(t);
		// camera steps
		else if(GameEventType.SET_CAMERA_POSITION.Equals(t)) s = new SetCamPosStep(t);
		else if(GameEventType.FADE_CAMERA_POSITION.Equals(t)) s = new FadeCamPosStep(t);
		else if(GameEventType.SET_INITIAL_CAMPOS.Equals(t)) s = new SetInitialCamPosStep(t);
		else if(GameEventType.FADE_TO_INITIAL_CAMPOS.Equals(t)) s = new FadeToInitialCamPosStep(t);
		else if(GameEventType.SHAKE_CAMERA.Equals(t)) s = new ShakeCameraStep(t);
		else if(GameEventType.ROTATE_CAMERA_AROUND.Equals(t)) s = new RotateCamAroundStep(t);
		// move steps
		else if(GameEventType.SET_TO_POSITION.Equals(t)) s = new SetToPositionStep(t);
		else if(GameEventType.MOVE_TO_WAYPOINT.Equals(t)) s = new MoveToWaypointStep(t);
		else if(GameEventType.MOVE_TO_DIRECTION.Equals(t)) s = new MoveToDirectionStep(t);
		else if(GameEventType.MOVE_TO_PREFAB.Equals(t)) s = new MoveToPrefabStep(t);
		// rotate steps
		else if(GameEventType.ROTATE_TO_WAYPOINT.Equals(t)) s = new RotateToWaypointStep(t);
		else if(GameEventType.ROTATION.Equals(t)) s = new RotationStep(t);
		// animation steps
		else if(GameEventType.PLAY_ANIMATION.Equals(t)) s = new PlayAnimationStep(t);
		else if(GameEventType.STOP_ANIMATION.Equals(t)) s = new StopAnimationStep(t);
		// audio steps
		else if(GameEventType.PLAY_SOUND.Equals(t)) s = new PlaySoundStep(t);
		else if(GameEventType.STOP_SOUND.Equals(t)) s = new StopSoundStep(t);
		else if(GameEventType.PLAY_MUSIC.Equals(t)) s = new PlayMusicStep(t);
		else if(GameEventType.STORE_MUSIC.Equals(t)) s = new StoreMusicStep(t);
		// fade steps
		else if(GameEventType.FADE_OBJECT.Equals(t)) s = new FadeObjectStep(t);
		else if(GameEventType.FADE_CAMERA.Equals(t)) s = new FadeCameraStep(t);
		// item steps
		else if(GameEventType.ADD_ITEM.Equals(t)) s = new AddItemStep(t);
		else if(GameEventType.REMOVE_ITEM.Equals(t)) s = new RemoveItemStep(t);
		else if(GameEventType.CHECK_ITEM.Equals(t)) s = new CheckItemStep(t);
		else if(GameEventType.LEARN_ITEM_RECIPE.Equals(t)) s = new LearnItemRecipeStep(t);
		else if(GameEventType.ITEM_RECIPE_KNOWN.Equals(t)) s = new ItemRecipeKnownStep(t);
		// weapon steps
		else if(GameEventType.ADD_WEAPON.Equals(t)) s = new AddWeaponStep(t);
		else if(GameEventType.REMOVE_WEAPON.Equals(t)) s = new RemoveWeaponStep(t);
		else if(GameEventType.CHECK_WEAPON.Equals(t)) s = new CheckWeaponStep(t);
		// armor steps
		else if(GameEventType.ADD_ARMOR.Equals(t)) s = new AddArmorStep(t);
		else if(GameEventType.REMOVE_ARMOR.Equals(t)) s = new RemoveArmorStep(t);
		else if(GameEventType.CHECK_ARMOR.Equals(t)) s = new CheckArmorStep(t);
		// money steps
		else if(GameEventType.ADD_MONEY.Equals(t)) s = new AddMoneyStep(t);
		else if(GameEventType.REMOVE_MONEY.Equals(t)) s = new RemoveMoneyStep(t);
		else if(GameEventType.SET_MONEY.Equals(t)) s = new SetMoneyStep(t);
		else if(GameEventType.CHECK_MONEY.Equals(t)) s = new CheckMoneyStep(t);
		// party steps
		else if(GameEventType.JOIN_PARTY.Equals(t)) s = new JoinPartyStep(t);
		else if(GameEventType.LEAVE_PARTY.Equals(t)) s = new LeavePartyStep(t);
		else if(GameEventType.IS_IN_PARTY.Equals(t)) s = new IsInPartyStep(t);
		else if(GameEventType.HAS_LEFT_PARTY.Equals(t)) s = new HasLeftPartyStep(t);
		else if(GameEventType.CHECK_PLAYER.Equals(t)) s = new CheckPlayerStep(t);
		else if(GameEventType.SET_PLAYER.Equals(t)) s = new SetPlayerStep(t);
		else if(GameEventType.SET_CHARACTER_NAME.Equals(t)) s = new SetCharacterNameStep(t);
		// battle party steps
		else if(GameEventType.JOIN_BATTLE_PARTY.Equals(t)) s = new JoinBattlePartyStep(t);
		else if(GameEventType.LEAVE_BATTLE_PARTY.Equals(t)) s = new LeaveBattlePartyStep(t);
		else if(GameEventType.IS_IN_BATTLE_PARTY.Equals(t)) s = new IsInBattlePartyStep(t);
		else if(GameEventType.LOCK_BP_MEMBER.Equals(t)) s = new LockBattlePartyMemberStep(t);
		else if(GameEventType.UNLOCK_BP_MEMBER.Equals(t)) s = new UnlockBattlePartyMemberStep(t);
		else if(GameEventType.IS_LOCKED_BP_MEMBER.Equals(t)) s = new IsLockedBattlePartyMemberStep(t);
		// skill steps
		else if(GameEventType.LEARN_SKILL.Equals(t)) s = new LearnSkillStep(t);
		else if(GameEventType.FORGET_SKILL.Equals(t)) s = new ForgetSkillStep(t);
		else if(GameEventType.HAS_SKILL.Equals(t)) s = new HasSkillStep(t);
		// equip steps
		else if(GameEventType.EQUIP_WEAPON.Equals(t)) s = new EquipWeaponStep(t);
		else if(GameEventType.EQUIP_ARMOR.Equals(t)) s = new EquipArmorStep(t);
		else if(GameEventType.UNEQUIP.Equals(t)) s = new UnequipStep(t);
		// status steps
		else if(GameEventType.REGENERATE.Equals(t)) s = new RegenerateStep(t);
		else if(GameEventType.CHECK_STATUS_VALUE.Equals(t)) s = new CheckStatusValueStep(t);
		else if(GameEventType.SET_STATUS_VALUE.Equals(t)) s = new SetStatusValueStep(t);
		else if(GameEventType.CHANGE_STATUS_EFFECT.Equals(t)) s = new ChangeStatusEffectStep(t);
		else if(GameEventType.LEVEL_UP.Equals(t)) s = new LevelUpStep(t);
		else if(GameEventType.CHECK_LEVEL.Equals(t)) s = new CheckLevelStep(t);
		// class steps
		else if(GameEventType.CHECK_CLASS.Equals(t)) s = new CheckClassStep(t);
		else if(GameEventType.CHANGE_CLASS.Equals(t)) s = new ChangeClassStep(t);
		//  variable steps
		else if(GameEventType.SET_VARIABLE.Equals(t)) s = new SetVariableStep(t);
		else if(GameEventType.REMOVE_VARIABLE.Equals(t)) s = new RemoveVariableStep(t);
		else if(GameEventType.CHECK_VARIABLE.Equals(t)) s = new CheckVariableStep(t);
		else if(GameEventType.SET_NUMBER_VARIABLE.Equals(t)) s = new SetNumberVariableStep(t);
		else if(GameEventType.REMOVE_NUMBER_VARIABLE.Equals(t)) s = new RemoveNumberVariableStep(t);
		else if(GameEventType.CHECK_NUMBER_VARIABLE.Equals(t)) s = new CheckNumberVariableStep(t);
		else if(GameEventType.SET_PLAYERPREFS.Equals(t)) s = new SetPlayerPrefsStep(t);
		else if(GameEventType.GET_PLAYERPREFS.Equals(t)) s = new GetPlayerPrefsStep(t);
		else if(GameEventType.HAS_PLAYERPREFS.Equals(t)) s = new HasPlayerPrefsStep(t);
		
		return s;
	}
	
	public void AddStep(GameEventType t, int pos)
	{
		this.InsertStep(this.TypeToStep(t), pos);
	}
	
	public void InsertStep(EventStep s, int pos)
	{
		step = ArrayHelper.Add(s, step);
		s.SetNextIndex(step.Length);
		this.MoveStepTo(pos, step.Length-1);
	}
	
	public void RemoveStep(int index)
	{
		step = ArrayHelper.Remove(index, step);
		for(int i=index; i<step.Length; i++)
		{
			this.step[i].next--;
		}
	}
	
	public void MoveStepUp(int index)
	{
		this.step[index-1].next++;
		this.step[index].next--;
		var s = this.step[index-1];
		this.step[index-1] = this.step[index];
		this.step[index] = s;
	}
	
	public void MoveStepDown(int index)
	{
		this.step[index+1].next--;
		this.step[index].next++;
		var s = this.step[index+1];
		this.step[index+1] = this.step[index];
		this.step[index] = s;
	}
	
	public void MoveStepTo(int index, int oldIndex)
	{
		if(oldIndex > index)
		{
			while(index != oldIndex) this.MoveStepUp(oldIndex--);
		}
		else if(index > oldIndex)
		{
			while(index != oldIndex) this.MoveStepDown(oldIndex++);
		}
	}
	
	/*
	============================================================================
	Actor functions
	============================================================================
	*/
	public void AddActor()
	{
		actor = ArrayHelper.Add(new EventActor(), actor);
	}
	
	public void RemoveActor(int index)
	{
		actor = ArrayHelper.Remove(index, actor);
	}
	
	public string[] GetActorList()
	{
		string[] list = new string[this.actor.Length];
		for(int i=0; i<this.actor.Length; i++)
		{
			if(this.actor[i].isPlayer)
			{
				list[i] = i.ToString()+": Player";
			}
			else if(this.actor[i].showName && this.actor[i].dialogName[GameHandler.GetLanguage()] != "")
			{
				list[i] = i.ToString()+": "+this.actor[i].dialogName[GameHandler.GetLanguage()];
			}
			else
			{
				list[i] = i.ToString()+": Actor "+i.ToString();
			}
		}
		return list;
	}
	
	public string GetActorName(int index)
	{
		string name = "";
		if(index < this.actor.Length)
		{
			name = this.actor[index].GetName();
		}
		return name;
	}
	
	/*
	============================================================================
	Resource functions
	============================================================================
	*/
	public string[] GetWaypointList()
	{
		string[] list = new string[this.waypoints];
		for(int i=0; i<this.waypoints; i++)
		{
			list[i] = "Waypoint "+i.ToString();
		}
		return list;
	}
	
	public string[] GetPrefabList()
	{
		string[] list = new string[this.prefabs];
		for(int i=0; i<this.prefabs; i++)
		{
			list[i] = "Prefab "+i.ToString();
		}
		return list;
	}
	
	public string[] GetAudioClipList()
	{
		string[] list = new string[this.audioClips];
		for(int i=0; i<this.audioClips; i++)
		{
			list[i] = "Audio clip "+i.ToString();
		}
		return list;
	}
	
	/*
	============================================================================
	Start/end functions
	============================================================================
	*/
	public void StartEvent(EventInteraction interact)
	{
		if(this.step.Length > 0 && !this.executing)
		{
			this.interaction = interact;
			this.StartEvent();
		}
	}
	
	public void StartEvent(GlobalEvent ge)
	{
		if(this.step.Length > 0 && !this.executing)
		{
			this.globalEvent = ge;
			this.StartEvent();
		}
	}
	
	private void StartEvent()
	{
		this.executing = true;
		this.currentStep = 0;
		this.teleportID = new int[0];
		this.teleportChoice = false;
		this.teleportCancel = false;
		if(this.blockControls)
		{
			GameHandler.SetControlType(ControlType.EVENT);
		}
		if(this.GetCamera())
		{
			this.initialCamPosition = this.cam.position;
			this.initialCamRotation = this.cam.rotation;
			this.initialFieldOfView = this.cam.camera.fieldOfView;
		}
		this.ExecuteNextStep();
	}
	
	public void EndEvent()
	{
		this.RestoreControls();
		this.executing = false;
		if(this.interaction != null && this.interaction.gameObject.active)
		{
			this.interaction.EventFinished();
		}
		else if(this.globalEvent != null)
		{
			this.globalEvent.EventFinished();
		}
		if(this.teleportID.Length > 0)
		{
			DataHolder.Teleport(this.teleportID[0]).Use();
		}
	}
	
	public void RestoreControls()
	{
		if(this.blockControls)
		{
			GameHandler.SetControlType(ControlType.FIELD);
		}
	}
	
	/*
	============================================================================
	Step functions
	============================================================================
	*/
	public void ExecuteNextStep()
	{
		this.stepFinished = false;
		if(this.currentStep < this.step.Length)
		{
			if(this.step[this.currentStep].stepEnabled)
			{
				this.step[this.currentStep].Execute(this);
			}
			else
			{
				this.StepFinished(this.currentStep+1);
			}
		}
		else if(this.currentStep == this.step.Length)
		{
			this.StartTime(0.1f, this.currentStep+1);
		}
		else
		{
			this.EndEvent();
		}
	}
	
	public override void StepFinished(int next)
	{
		if(this.teleportChoice)
		{
			this.teleportChoice = false;
			if(this.teleportCancel && next == this.teleportID.Length-1)
			{
				next = this.teleportID[next];
				this.teleportID = new int[0];
				this.teleportCancel = false;
			}
			else
			{
				this.teleportID = new int[] {this.teleportID[next]};
				next = this.step.Length;
			}
		}
		
		this.stepFinished = true;
		this.StartTime(0.01f, next);
	}
	
	public void ChoiceSelected(int index)
	{
		if(this.currentStep < this.step.Length)
		{
			this.step[this.currentStep].ChoiceSelected(index, this);
		}
	}
	
	public void WaitForButton(string b, float t, int n, int nf)
	{
		this.button = b;
		this.buttonNext = n;
		this.StartTime(t, nf);
	}
	
	/*
	============================================================================
	Time functions
	============================================================================
	*/
	public void StartTime(float t, int next)
	{
		this.time = t;
		this.currentStep = next;
		this.timeRunning = true;
	}
	
	public void TimeTick(float t)
	{
		if(this.calledEvent != null)
		{
			this.calledEvent.Tick(t);
		}
		else if(this.timeRunning)
		{
			this.time -= t;
			
			if(this.button != "" && ControlHandler.IsPressed(this.button))
			{
				this.currentStep = this.buttonNext;
				this.time = 0;
			}
			
			if(this.time <= 0.0)
			{
				this.timeRunning = false;
				this.button = "";
				if(this.stepFinished) this.ExecuteNextStep();
				else this.StepFinished(this.currentStep);
			}
		}
	}
	
	/*
	============================================================================
	Camera functions
	============================================================================
	*/
	public Transform GetCamera()
	{
		if(this.cam == null)
		{
			if(this.mainCamera)
			{
				Camera c = Camera.main;
				if(c != null)
				{
					this.cam = c.transform;
				}
			}
		}
		return this.cam;
	}
	
	public void ResetCameraPosition()
	{
		if(this.cam)
		{
			this.cam.position = this.initialCamPosition;
			this.cam.rotation = this.initialCamRotation;
			this.cam.camera.fieldOfView = this.initialFieldOfView;
		}
	}
	
	/*
	============================================================================
	Call global event functions
	============================================================================
	*/
	public void CallGlobalEvent(int id, int next)
	{
		this.currentStep = next;
		bool started = false;
		if(id < DataHolder.GlobalEvents().GetDataCount())
		{
			this.calledEvent = DataHolder.GlobalEvent(id).GetCopy();
			if(this.calledEvent.LoadEvent() && 
				this.calledEvent.StartFromEvent(this))
			{
				started = true;
			}
		}
		if(!started)
		{
			this.calledEvent = null;
			this.StepFinished(this.currentStep);
		}
	}
	
	public void GlobalEventFinished()
	{
		this.calledEvent = null;
		this.StepFinished(this.currentStep);
	}
	
	/*
	============================================================================
	Teleport functions
	============================================================================
	*/
	public void SetTeleportID(int id)
	{
		this.teleportID = new int[] {id};
	}
	
	public void SetTeleportIDs(int[] ids, bool addCancel)
	{
		this.teleportChoice = true;
		this.teleportID = ids;
		this.teleportCancel = addCancel;
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public void SaveEventData(string filename)
	{
		if(filename.Contains("Assets/Resources/Events/"))
		{
			filename = filename.Substring(filename.IndexOf("Assets/Resources/Events/"));
			filename = filename.Replace("Assets/Resources/Events/", "");
		}
		filename = filename.Replace(".xml", "");
		ArrayList data = new ArrayList();
		ArrayList subs = new ArrayList();
		
		Hashtable sv = new Hashtable();
		sv.Add(XMLHandler.NODE_NAME, "gameevents");
		
		Hashtable val = new Hashtable();
		val.Add(XMLHandler.NODE_NAME, "gameevent");
		val.Add("waypoints", this.waypoints.ToString());
		val.Add("prefabs", this.prefabs.ToString());
		val.Add("audioclips", this.audioClips.ToString());
		val.Add("actors", this.actor.Length.ToString());
		val.Add("steps", this.step.Length.ToString());
		val.Add("block", this.blockControls.ToString());
		val.Add("maincam", this.mainCamera.ToString());
		
		ArrayList s = new ArrayList();
		for(int i=0; i<this.actor.Length; i++)
		{
			Hashtable ht = this.actor[i].GetData();
			ht.Add("id", i.ToString());
			ht.Add(XMLHandler.NODE_NAME, "actor");
			s.Add(ht);
		}
		for(int i=0; i<this.step.Length; i++)
		{
			Hashtable ht = this.step[i].GetData();
			ht.Add("id", i.ToString());
			ht.Add(XMLHandler.NODE_NAME, "step");
			s.Add(ht);
		}
		val.Add(XMLHandler.NODES, s);
		subs.Add(val);
		
		sv.Add(XMLHandler.NODES, subs);
		data.Add(sv);
		XMLHandler.SaveXML("Events/", filename, data);
	}
	
	public bool LoadEventData(string filename)
	{
		bool fileOk = false;
		if(filename.Contains("Assets/Resources/"))
		{
			filename = filename.Substring(filename.IndexOf("Assets/Resources/"));
			filename = filename.Replace("Assets/Resources/", "");
		}
		filename = filename.Replace(".xml", "");
		ArrayList data = XMLHandler.LoadXML(filename);
		
		if(data.Count > 0)
		{
			foreach(Hashtable entry in data)
			{
				if(entry[XMLHandler.NODE_NAME] as string == "gameevents")
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList eSub = entry[XMLHandler.NODES] as ArrayList;
						foreach(Hashtable val in eSub)
						{
							if(val[XMLHandler.NODE_NAME] as string == "gameevent")
							{
								fileOk = true;
								this.blockControls = bool.Parse((string)val["block"]);
								this.mainCamera = bool.Parse((string)val["maincam"]);
								
								if(val.ContainsKey("waypoints")) this.waypoints = int.Parse((string)val["waypoints"]);
								if(val.ContainsKey("prefabs")) this.prefabs = int.Parse((string)val["prefabs"]);
								if(val.ContainsKey("audioclips")) this.audioClips = int.Parse((string)val["audioclips"]);
								this.actor = new EventActor[int.Parse((string)val["actors"])];
								this.step = new EventStep[int.Parse((string)val["steps"])];
								
								if(val.ContainsKey(XMLHandler.NODES))
								{
									ArrayList subs = val[XMLHandler.NODES] as ArrayList;
									foreach(Hashtable ht in subs)
									{
										if(ht[XMLHandler.NODE_NAME] as string == "actor")
										{
											int i = int.Parse((string)ht["id"]);
											this.actor[i] = new EventActor();
											this.actor[i].SetData(ht);
										}
										else if(ht[XMLHandler.NODE_NAME] as string == "step")
										{
											int i = int.Parse((string)ht["id"]);
											GameEventType t = (GameEventType)System.Enum.Parse(typeof(GameEventType), (string)ht["type"]);
											EventStep s = this.TypeToStep(t);
											
											if(ht.ContainsKey("time")) s.time = float.Parse((string)ht["time"]);
											if(ht.ContainsKey("intensity")) s.intensity = float.Parse((string)ht["intensity"]);
											if(ht.ContainsKey("speed")) s.speed = float.Parse((string)ht["speed"]);
											if(ht.ContainsKey("volume")) s.volume = float.Parse((string)ht["volume"]);
											if(ht.ContainsKey("float1")) s.float1 = float.Parse((string)ht["float1"]);
											if(ht.ContainsKey("float2")) s.float2 = float.Parse((string)ht["float2"]);
											if(ht.ContainsKey("float3")) s.float3 = float.Parse((string)ht["float3"]);
											if(ht.ContainsKey("float4")) s.float4 = float.Parse((string)ht["float4"]);
											if(ht.ContainsKey("float5")) s.float5 = float.Parse((string)ht["float5"]);
											if(ht.ContainsKey("float6")) s.float6 = float.Parse((string)ht["float6"]);
											if(ht.ContainsKey("float7")) s.float7 = float.Parse((string)ht["float7"]);
											if(ht.ContainsKey("float8")) s.float8 = float.Parse((string)ht["float8"]);
											
											if(ht.ContainsKey("next")) s.next = int.Parse((string)ht["next"]);
											if(ht.ContainsKey("nextfail")) s.nextFail = int.Parse((string)ht["nextfail"]);
											if(ht.ContainsKey("actor")) s.actorID = int.Parse((string)ht["actor"]);
											if(ht.ContainsKey("campos")) s.posID = int.Parse((string)ht["campos"]);
											if(ht.ContainsKey("spawn")) s.spawnID = int.Parse((string)ht["spawn"]);
											if(ht.ContainsKey("min")) s.min = int.Parse((string)ht["min"]);
											if(ht.ContainsKey("max")) s.max = int.Parse((string)ht["max"]);
											if(ht.ContainsKey("item")) s.itemID = int.Parse((string)ht["item"]);
											if(ht.ContainsKey("weapon")) s.weaponID = int.Parse((string)ht["weapon"]);
											if(ht.ContainsKey("armor")) s.armorID = int.Parse((string)ht["armor"]);
											if(ht.ContainsKey("number")) s.number = int.Parse((string)ht["number"]);
											if(ht.ContainsKey("character")) s.characterID = int.Parse((string)ht["character"]);
											if(ht.ContainsKey("waypoint")) s.waypointID = int.Parse((string)ht["waypoint"]);
											if(ht.ContainsKey("prefab")) s.prefabID = int.Parse((string)ht["prefab"]);
											if(ht.ContainsKey("skill")) s.skillID = int.Parse((string)ht["skill"]);
											if(ht.ContainsKey("audio")) s.audioID = int.Parse((string)ht["audio"]);
											if(ht.ContainsKey("formula")) s.formulaID = int.Parse((string)ht["formula"]);
											if(ht.ContainsKey("music")) s.musicID = int.Parse((string)ht["music"]);
											
											if(ht.ContainsKey("stepenabled")) s.stepEnabled = false;
											if(ht.ContainsKey("wait")) s.wait = bool.Parse((string)ht["wait"]);
											if(ht.ContainsKey("default")) s.useDefault = bool.Parse((string)ht["default"]);
											if(ht.ContainsKey("default2")) s.useDefault2 = bool.Parse((string)ht["default2"]);
											if(ht.ContainsKey("show")) s.show = bool.Parse((string)ht["show"]);
											if(ht.ContainsKey("show2")) s.show2 = bool.Parse((string)ht["show2"]);
											if(ht.ContainsKey("show3")) s.show3 = bool.Parse((string)ht["show3"]);
											if(ht.ContainsKey("show4")) s.show4 = bool.Parse((string)ht["show4"]);
											if(ht.ContainsKey("show5")) s.show5 = bool.Parse((string)ht["show5"]);
											if(ht.ContainsKey("show6")) s.show6 = bool.Parse((string)ht["show6"]);
											if(ht.ContainsKey("show7")) s.show7 = bool.Parse((string)ht["show7"]);
											if(ht.ContainsKey("show8")) s.show8 = bool.Parse((string)ht["show8"]);
											if(ht.ContainsKey("show9")) s.show9 = bool.Parse((string)ht["show9"]);
											if(ht.ContainsKey("show10")) s.show10 = bool.Parse((string)ht["show10"]);
											if(ht.ContainsKey("show11")) s.show11 = bool.Parse((string)ht["show11"]);
											if(ht.ContainsKey("showshadow")) s.showShadow = bool.Parse((string)ht["showshadow"]);
											if(ht.ContainsKey("controller")) s.controller = bool.Parse((string)ht["controller"]);
											
											if(ht.ContainsKey("interpolate")) s.interpolate = (EaseType)System.Enum.Parse(typeof(EaseType), (string)ht["interpolate"]);
											if(ht.ContainsKey("playmode")) s.playMode = (PlayMode)System.Enum.Parse(typeof(PlayMode), (string)ht["playmode"]);
											if(ht.ContainsKey("queuemode")) s.queueMode = (QueueMode)System.Enum.Parse(typeof(QueueMode), (string)ht["queuemode"]);
											if(ht.ContainsKey("audiorolloffmode")) s.audioRolloffMode = (AudioRolloffMode)System.Enum.Parse(typeof(AudioRolloffMode), (string)ht["audiorolloffmode"]);
											if(ht.ContainsKey("musicplaytype")) s.playType = (MusicPlayType)System.Enum.Parse(typeof(MusicPlayType), (string)ht["musicplaytype"]);
											if(ht.ContainsKey("simpleoperator")) s.simpleOperator = (SimpleOperator)System.Enum.Parse(typeof(SimpleOperator), (string)ht["simpleoperator"]);
											if(ht.ContainsKey("valuecheck")) s.valueCheck = (ValueCheck)System.Enum.Parse(typeof(ValueCheck), (string)ht["valuecheck"]);
											
											if(ht.ContainsKey("materialproperty")) s.materialProperty = ht["materialproperty"] as string;
											
											if(ht.ContainsKey("choices"))
											{
												int choices = int.Parse((string)ht["choices"]);
												s.choiceNext = new int[choices];
												s.choice = new ArrayList();
												s.addVariableCondition = new bool[choices];
												s.variableCondition = new VariableCondition[choices];
												s.addItem = new bool[choices];
												s.itemChoiceType = new ItemDropType[choices];
												s.itemChoice = new int[choices];
												s.itemChoiceQuantity = new int[choices];
												for(int i2=0; i2<s.choiceNext.Length; i2++)
												{
													s.choice.Add(new string[s.message.Length]);
													for(int j=0; j<((string[])s.choice[i2]).Length; j++)
													{
														((string[])s.choice[i2])[j] = "";
													}
													s.variableCondition[i2] = new VariableCondition();
												}
											}
											
											if(ht.ContainsKey(XMLHandler.NODES))
											{
												ArrayList subs2 = ht[XMLHandler.NODES] as ArrayList;
												foreach(Hashtable ht2 in subs2)
												{
													if(ht2[XMLHandler.NODE_NAME] as string == "rect")
													{
														s.rect = new Rect(float.Parse((string)ht2["x"]), float.Parse((string)ht2["y"]), 
																float.Parse((string)ht2["width"]), float.Parse((string)ht2["height"]));
													}
													else if(ht2[XMLHandler.NODE_NAME] as string == "rect2")
													{
														s.rect2 = new Rect(float.Parse((string)ht2["x"]), float.Parse((string)ht2["y"]), 
																float.Parse((string)ht2["width"]), float.Parse((string)ht2["height"]));
													}
													else if(ht2[XMLHandler.NODE_NAME] as string == "vector2")
													{
														s.v2 = new Vector2(float.Parse((string)ht2["x"]), float.Parse((string)ht2["y"]));
													}
													else if(ht2[XMLHandler.NODE_NAME] as string == "vector3")
													{
														s.v3 = new Vector3(float.Parse((string)ht2["x"]), float.Parse((string)ht2["y"]), float.Parse((string)ht2["z"]));
													}
													else if(ht2[XMLHandler.NODE_NAME] as string == "vector3_2")
													{
														s.v3_2 = new Vector3(float.Parse((string)ht2["x"]), float.Parse((string)ht2["y"]), float.Parse((string)ht2["z"]));
													}
													else if(ht2[XMLHandler.NODE_NAME] as string == "vector4")
													{
														s.v4 = new Vector4(float.Parse((string)ht2["x"]), float.Parse((string)ht2["y"]), 
																float.Parse((string)ht2["z"]), float.Parse((string)ht2["w"]));
													}
													else if(ht2[XMLHandler.NODE_NAME] as string == "vector4_2")
													{
														s.v4_2 = new Vector4(float.Parse((string)ht2["x"]), float.Parse((string)ht2["y"]), 
																float.Parse((string)ht2["z"]), float.Parse((string)ht2["w"]));
													}
													else if(ht2[XMLHandler.NODE_NAME] as string == "message")
													{
														int id = int.Parse((string)ht2["id"]); 
														if(id < s.message.Length) s.message[id] = ht2[XMLHandler.CONTENT] as string;
													}
													else if(ht2[XMLHandler.NODE_NAME] as string == "key")
													{
														s.key = ht2[XMLHandler.CONTENT] as string;
													}
													else if(ht2[XMLHandler.NODE_NAME] as string == "value")
													{
														s.value = ht2[XMLHandler.CONTENT] as string;
													}
													else if(ht2[XMLHandler.NODE_NAME] as string == "scene")
													{
														s.scene = ht2[XMLHandler.CONTENT] as string;
													}
													else if(ht2[XMLHandler.NODE_NAME] as string == "statuseffect")
													{
														int id = int.Parse((string)ht2["id"]);
														if(id < s.effect.Length)
														{
															s.effect[id] = (SkillEffect)System.Enum.Parse(typeof(SkillEffect), (string)ht2["effect"]);
														}
													}
													else if(ht2[XMLHandler.NODE_NAME] as string == "choice")
													{
														int id = int.Parse((string)ht2["id"]);
														s.choiceNext[id] = int.Parse((string)ht2["next"]);
														if(ht2.ContainsKey(XMLHandler.NODES))
														{
															ArrayList subs3 = ht2[XMLHandler.NODES] as ArrayList;
															foreach(Hashtable ht3 in subs3)
															{
																if(ht3[XMLHandler.NODE_NAME] as string == "choicetext")
																{
																	int id2 = int.Parse((string)ht3["id"]);
																	if(id2 < ((string[])s.choice[id]).Length) ((string[])s.choice[id])[id2] = ht3[XMLHandler.CONTENT] as string;
																}
																else if(ht3[XMLHandler.NODE_NAME] as string == "variablecondition")
																{
																	s.addVariableCondition[id] = true;
																	s.variableCondition[id].SetData(ht3);
																}
																else if(ht3[XMLHandler.NODE_NAME] as string == "item")
																{
																	s.addItem[id] = true;
																	s.itemChoiceType[id] = (ItemDropType)System.Enum.Parse(
																			typeof(ItemDropType), (string)ht3["type"]);
																	s.itemChoice[id] = int.Parse((string)ht3["id"]);
																	s.itemChoiceQuantity[id] = int.Parse((string)ht3["quantity"]);
																}
															}
														}
													}
													else if(ht2[XMLHandler.NODE_NAME] as string == "pathtochild")
													{
														s.pathToChild = ht2[XMLHandler.CONTENT] as string;
													}
												}
											}
											this.step[i] = s;
										}
									}
								}
							}
						}
					}
				}
			}
		}
		return fileOk;
	}
}
