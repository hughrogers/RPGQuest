
using System.Collections;
using UnityEngine;

public enum BattleAnimationType {
		// base steps
		WAIT, GO_TO, RANDOM_STEP, CHECK_RANDOM, CHECK_FORMULA, 
		WAIT_FOR_BUTTON, CHECK_DIFFICULTY, CHECK_USER, 
		// spawn steps
		SPAWN_PREFAB, DESTROY_PREFAB, 
		// battle system steps
		CALCULATE, DAMAGE_MULTIPLIER, ACTIVATE_DAMAGE, RESTORE_CONTROL, 
		// function steps
		SEND_MESSAGE, BROADCAST_MESSAGE, 
		ADD_COMPONENT, REMOVE_COMPONENT, 
		// statistic steps
		CUSTOM_STATISTIC, CLEAR_STATISTIC, 
		// camera steps
		SET_CAMERA_POSITION, FADE_CAMERA_POSITION, 
		SET_INITIAL_CAMPOS, FADE_TO_INITIAL_CAMPOS, 
		SHAKE_CAMERA, ROTATE_CAMERA_AROUND, MOUNT_CAMERA, 
		// move steps
		SET_TO_POSITION, 
		MOVE_TO, MOVE_TO_DIRECTION, 
		// rotate steps
		ROTATE_TO, ROTATION, LOOK_AT, 
		// animation steps
		PLAY_ANIMATION, STOP_ANIMATION, CALL_ANIMATION, 
		// audio steps
		PLAY_SOUND, STOP_SOUND, 
		// fade steps
		FADE_OBJECT, FADE_CAMERA, 
		// item steps
		ADD_ITEM, REMOVE_ITEM, CHECK_ITEM, 
		// variable steps
		SET_VARIABLE, REMOVE_VARIABLE, CHECK_VARIABLE,
		SET_NUMBER_VARIABLE, REMOVE_NUMBER_VARIABLE, CHECK_NUMBER_VARIABLE, 
		SET_PLAYERPREFS, GET_PLAYERPREFS, HAS_PLAYERPREFS
		};

public class BattleAnimation : BaseEvent
{
	public int realID = -1;
	public AnimationStep[] step = new AnimationStep[0];
	
	// editor
	public bool hideButtons = false;
	
	// ingame
	public EventInteraction interaction;
	public bool executing = false;
	public int currentStep = 0;
	
	// time handling
	public bool timeRunning = false;
	public bool stepFinished = false;
	public float time = 0.0f;
	
	// camera settings
	public bool returnToBaseCamPos = true;
	public bool returnLooks = true;
	public Transform cam;
	public Vector3 initialCamPosition;
	public Quaternion initialCamRotation;
	public float initialFieldOfView;
	
	// prefabs
	public bool autoDestroyPrefabs = true;
	public string[] prefabName = new string[0];
	public GameObject[] prefab = new GameObject[0];
	public Hashtable spawnedPrefabs = new Hashtable();
	
	// audio clips
	public string[] audioName = new string[0];
	public AudioClip[] audioClip = new AudioClip[0];
	
	public BattleAction battleAction;
	public bool calculated = false;
	public bool calculationNeeded = true;
	
	public bool camBlocked = false;
	public bool resetBlock = false;
	
	public bool stopFlag = false;
	
	// button press
	private string button = "";
	private int buttonNext = 0;
	
	// call other battle animation
	private BattleAnimation calledAnimation = null;
	private BattleAnimation parentAnimation = null;
	
	public BattleAnimation()
	{
		
	}
	
	public AnimationStep TypeToStep(BattleAnimationType t)
	{
		AnimationStep s = null;
		
		// base steps
		if(BattleAnimationType.WAIT.Equals(t)) s = new WaitAStep(t);
		else if(BattleAnimationType.GO_TO.Equals(t)) s = new GoToAStep(t);
		else if(BattleAnimationType.RANDOM_STEP.Equals(t)) s = new RandomAStep(t);
		else if(BattleAnimationType.CHECK_RANDOM.Equals(t)) s = new CheckRandomAStep(t);
		else if(BattleAnimationType.CHECK_FORMULA.Equals(t)) s = new CheckFormulaAStep(t);
		else if(BattleAnimationType.WAIT_FOR_BUTTON.Equals(t)) s = new WaitForButtonAStep(t);
		else if(BattleAnimationType.CHECK_DIFFICULTY.Equals(t)) s = new CheckDifficultyAStep(t);
		else if(BattleAnimationType.CHECK_USER.Equals(t)) s = new CheckUserAStep(t);
		// spawn steps
		else if(BattleAnimationType.SPAWN_PREFAB.Equals(t)) s = new SpawnPrefabAStep(t);
		else if(BattleAnimationType.DESTROY_PREFAB.Equals(t)) s = new DestroyPrefabAStep(t);
		// battle system steps
		else if(BattleAnimationType.CALCULATE.Equals(t)) s = new CalculateAStep(t);
		else if(BattleAnimationType.DAMAGE_MULTIPLIER.Equals(t)) s = new DamageMultiplierAStep(t);
		else if(BattleAnimationType.ACTIVATE_DAMAGE.Equals(t)) s = new ActivateDamageAStep(t);
		else if(BattleAnimationType.RESTORE_CONTROL.Equals(t)) s = new RestoreControlAStep(t);
		// function steps
		else if(BattleAnimationType.SEND_MESSAGE.Equals(t)) s = new SendMessageAStep(t);
		else if(BattleAnimationType.BROADCAST_MESSAGE.Equals(t)) s = new BroadcastMessageAStep(t);
		else if(BattleAnimationType.ADD_COMPONENT.Equals(t)) s = new AddComponentAStep(t);
		else if(BattleAnimationType.REMOVE_COMPONENT.Equals(t)) s = new RemoveComponentAStep(t);
		// statistic steps
		else if(BattleAnimationType.CUSTOM_STATISTIC.Equals(t)) s = new CustomStatisticAStep(t);
		else if(BattleAnimationType.CLEAR_STATISTIC.Equals(t)) s = new ClearStatisticAStep(t);
		// camera steps
		else if(BattleAnimationType.SET_CAMERA_POSITION.Equals(t)) s = new SetCamPosAStep(t);
		else if(BattleAnimationType.FADE_CAMERA_POSITION.Equals(t)) s = new FadeCamPosAStep(t);
		else if(BattleAnimationType.SET_INITIAL_CAMPOS.Equals(t)) s = new SetInitialCamPosAStep(t);
		else if(BattleAnimationType.FADE_TO_INITIAL_CAMPOS.Equals(t)) s = new FadeToInitialCamPosAStep(t);
		else if(BattleAnimationType.SHAKE_CAMERA.Equals(t)) s = new ShakeCameraAStep(t);
		else if(BattleAnimationType.ROTATE_CAMERA_AROUND.Equals(t)) s = new RotateCamAroundAStep(t);
		else if(BattleAnimationType.MOUNT_CAMERA.Equals(t)) s = new MountCameraAStep(t);
		// move steps
		else if(BattleAnimationType.SET_TO_POSITION.Equals(t)) s = new SetToPositionAStep(t);
		else if(BattleAnimationType.MOVE_TO.Equals(t)) s = new MoveToAStep(t);
		else if(BattleAnimationType.MOVE_TO_DIRECTION.Equals(t)) s = new MoveToDirectionAStep(t);
		// rotate steps
		else if(BattleAnimationType.ROTATE_TO.Equals(t)) s = new RotateToAStep(t);
		else if(BattleAnimationType.ROTATION.Equals(t)) s = new RotationAStep(t);
		else if(BattleAnimationType.LOOK_AT.Equals(t)) s = new LookAtAStep(t);
		// animation steps
		else if(BattleAnimationType.PLAY_ANIMATION.Equals(t)) s = new PlayAnimationAStep(t);
		else if(BattleAnimationType.STOP_ANIMATION.Equals(t)) s = new StopAnimationAStep(t);
		else if(BattleAnimationType.CALL_ANIMATION.Equals(t)) s = new CallAnimationAStep(t);
		// audio steps
		else if(BattleAnimationType.PLAY_SOUND.Equals(t)) s = new PlaySoundAStep(t);
		else if(BattleAnimationType.STOP_SOUND.Equals(t)) s = new StopSoundAStep(t);
		// fade steps
		else if(BattleAnimationType.FADE_OBJECT.Equals(t)) s = new FadeObjectAStep(t);
		else if(BattleAnimationType.FADE_CAMERA.Equals(t)) s = new FadeCameraAStep(t);
		//  item steps
		else if(BattleAnimationType.ADD_ITEM.Equals(t)) s = new AddItemAStep(t);
		else if(BattleAnimationType.REMOVE_ITEM.Equals(t)) s = new RemoveItemAStep(t);
		else if(BattleAnimationType.CHECK_ITEM.Equals(t)) s = new CheckItemAStep(t);
		//  variable steps
		else if(BattleAnimationType.SET_VARIABLE.Equals(t)) s = new SetVariableAStep(t);
		else if(BattleAnimationType.REMOVE_VARIABLE.Equals(t)) s = new RemoveVariableAStep(t);
		else if(BattleAnimationType.CHECK_VARIABLE.Equals(t)) s = new CheckVariableAStep(t);
		else if(BattleAnimationType.SET_NUMBER_VARIABLE.Equals(t)) s = new SetNumberVariableAStep(t);
		else if(BattleAnimationType.REMOVE_NUMBER_VARIABLE.Equals(t)) s = new RemoveNumberVariableAStep(t);
		else if(BattleAnimationType.CHECK_NUMBER_VARIABLE.Equals(t)) s = new CheckNumberVariableAStep(t);
		else if(BattleAnimationType.SET_PLAYERPREFS.Equals(t)) s = new SetPlayerPrefsAStep(t);
		else if(BattleAnimationType.GET_PLAYERPREFS.Equals(t)) s = new GetPlayerPrefsAStep(t);
		else if(BattleAnimationType.HAS_PLAYERPREFS.Equals(t)) s = new HasPlayerPrefsAStep(t);
		
		return s;
	}
	
	public void AddStep(BattleAnimationType t, int pos)
	{
		this.InsertStep(this.TypeToStep(t), pos);
	}
	
	public void InsertStep(AnimationStep s, int pos)
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
		AnimationStep s = this.step[index-1];
		this.step[index-1] = this.step[index];
		this.step[index] = s;
	}
	
	public void MoveStepDown(int index)
	{
		this.step[index+1].next--;
		this.step[index].next++;
		AnimationStep s = this.step[index+1];
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
	Prefab functions
	============================================================================
	*/
	public void AddPrefab()
	{
		this.prefab = ArrayHelper.Add(null, this.prefab);
		this.prefabName = ArrayHelper.Add("", this.prefabName);
	}
	
	public void RemovePrefab(int index)
	{
		this.prefab = ArrayHelper.Remove(index, this.prefab);
		this.prefabName = ArrayHelper.Remove(index, this.prefabName);
	}
	
	public string[] GetPrefabList()
	{
		string[] list = new string[this.prefab.Length];
		for(int i=0; i<this.prefab.Length; i++)
		{
			list[i] = "Prefab "+i.ToString();
		}
		return list;
	}
	
	/*
	============================================================================
	Audio functions
	============================================================================
	*/
	public void AddAudioClip()
	{
		this.audioClip = ArrayHelper.Add(null, this.audioClip);
		this.audioName = ArrayHelper.Add("", this.audioName);
	}
	
	public void RemoveAudioClip(int index)
	{
		this.audioClip = ArrayHelper.Remove(index, this.audioClip);
		this.audioName = ArrayHelper.Remove(index, this.audioName);
	}
	
	public string[] GetAudioClipList()
	{
		string[] list = new string[this.audioClip.Length];
		for(int i=0; i<this.audioClip.Length; i++)
		{
			list[i] = "Audio clip "+i.ToString();
		}
		return list;
	}
	
	public void LoadResources()
	{
		for(int i=0; i<this.audioName.Length; i++)
		{
			if("" != this.audioName[i] && this.audioClip[i] == null)
			{
				this.audioClip[i] = (AudioClip)Resources.Load(BattleSystemData.AUDIO_PATH+this.audioName[i], typeof(AudioClip));
			}
		}
		for(int i=0; i<this.prefabName.Length; i++)
		{
			if("" != this.prefabName[i] && this.prefab[i] == null)
			{
				this.prefab[i] = (GameObject)Resources.Load(BattleSystemData.PREFAB_PATH+this.prefabName[i], typeof(GameObject));
			}
		}
	}
	
	/*
	============================================================================
	Start/end functions
	============================================================================
	*/
	public void StartFromAnimation(BattleAnimation ba)
	{
		this.parentAnimation = ba;
		this.StartEvent(this.parentAnimation.battleAction);
	}
	
	public void StartEvent(BattleAction action)
	{
		if(this.step.Length > 0 && !this.executing)
		{
			this.LoadResources();
			this.battleAction = action;
			this.stopFlag = false;
			this.calculated = false;
			this.executing = true;
			this.currentStep = 0;
			if(this.GetCamera())
			{
				this.initialCamPosition = this.cam.position;
				this.initialCamRotation = this.cam.rotation;
				this.initialFieldOfView = this.cam.camera.fieldOfView;
			}
			if(DataHolder.BattleSystem().IsRealTime())
			{
				this.camBlocked = true;
			}
			else
			{
				this.camBlocked = DataHolder.BattleCam().IsAnimationCamBlocked(this.realID);
				if(!this.camBlocked)
				{
					DataHolder.BattleCam().BlockedByAnimation(true);
					this.resetBlock = true;
				}
			}
			this.ExecuteNextStep();
		}
	}
	
	public void StopEvent()
	{
		if(!this.battleAction.IsDeath()) this.stopFlag = true;
	}
	
	public void EndEvent()
	{
		this.currentStep = this.step.Length+1;
		this.stepFinished = true;
		this.timeRunning = false;
		// calculate if it hasn't been done yet
		if(this.calculationNeeded && !this.calculated && this.battleAction.target != null)
		{
			bool allDead = true;
			for(int i=0; i<this.battleAction.target.Length; i++)
			{
				if(this.battleAction.target[i] != null && 
					(!this.battleAction.target[i].isDead || this.battleAction.reviveFlag))
				{
					allDead = false;
					break;
				}
			}
			if(!allDead) this.battleAction.Calculate(this.battleAction.target, 1);
		}
		if(this.autoDestroyPrefabs)
		{
			foreach(DictionaryEntry entry in this.spawnedPrefabs)
			{
				GameObject.Destroy((GameObject)entry.Value);
			}
		}
		if(this.battleAction != null && DataHolder.BattleSystem().GetArena() != null)
		{
			if(this.returnToBaseCamPos && this.cam && !this.camBlocked)
			{
				DataHolder.BattleSystem().GetArena().SetBaseCameraPosition(this.cam);
			}
			if(this.returnLooks)
			{
				DataHolder.BattleSystem().DoLookAt(
						GameHandler.Party().GetBattleParty(), 
						DataHolder.BattleSystem().enemies);
				if(this.battleAction.user is Character)
				{
					DataHolder.BattleSystem().DoLookAt(
							this.battleAction.user, DataHolder.BattleSystem().enemies);
				}
				else if(this.battleAction.user is Enemy)
				{
					DataHolder.BattleSystem().DoLookAt(
							this.battleAction.user, GameHandler.Party().GetBattleParty());
				}
			}
		}
		if(this.resetBlock) DataHolder.BattleCam().BlockedByAnimation(false);
		this.executing = false;
		this.calculated = false;
		if(this.parentAnimation != null) this.parentAnimation.CalledAnimationFinished();
		else this.battleAction.AnimationFinished();
	}
	
	public bool IsLatestActiveAction()
	{
		return DataHolder.BattleSystem().IsLatestActiveAction(this.battleAction);
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
			if(DataHolder.BattleSystem().battleEnd ||
				(!this.battleAction.IsDeath() && this.battleAction.user.isDead))
			{
				this.calculated = true;
				this.stopFlag = true;
			}
			
			if(this.step[this.currentStep].stepEnabled && 
				(this.battleAction.IsDeath() || !this.stopFlag || 
				this.step[this.currentStep] is DestroyPrefabAStep ||
				this.step[this.currentStep] is StopSoundAStep ||
				this.step[this.currentStep] is StopAnimationAStep))
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
		this.stepFinished = true;
		this.StartTime(0.01f, next);
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
		if(this.calledAnimation != null)
		{
			this.calledAnimation.TimeTick(t);
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
	Call animation functions
	============================================================================
	*/
	public void CallAnimation(int id, int next)
	{
		this.currentStep = next;
		if(id < DataHolder.BattleAnimations().GetDataCount())
		{
			this.calledAnimation = DataHolder.BattleAnimations().GetCopy(id);
			this.calledAnimation.StartFromAnimation(this);
		}
		else this.CalledAnimationFinished();
	}
	
	public void CalledAnimationFinished()
	{
		this.calledAnimation = null;
		this.StepFinished(this.currentStep);
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
			Camera c = Camera.main;
			if(c != null)
			{
				this.cam = c.transform;
			}
		}
		return this.cam;
	}
	
	public void ResetCameraPosition()
	{
		if(this.cam && !this.camBlocked)
		{
			this.cam.position = this.initialCamPosition;
			this.cam.rotation = this.initialCamRotation;
			this.cam.camera.fieldOfView = this.initialFieldOfView;
		}
	}
	
	/*
	============================================================================
	GameObject functions
	============================================================================
	*/
	public GameObject[] GetAnimationObjects(BattleAnimationObject obj, int pID)
	{
		GameObject[] list = new GameObject[0];
		if(BattleAnimationObject.USER.Equals(obj) &&
			this.battleAction.user != null && 
			this.battleAction.user.prefabInstance != null)
		{
			list = ArrayHelper.Add(this.battleAction.user.prefabInstance, list);
		}
		else if(BattleAnimationObject.TARGET.Equals(obj))
		{
			
			if(this.battleAction.target != null &&
				this.battleAction.target.Length > 0)
			{
				if(this.battleAction.rayTargetSet)
				{
					if(this.battleAction.rayObject == null)
					{
						this.battleAction.rayObject = new GameObject("RaySpot");
						this.battleAction.rayObject.transform.position = this.battleAction.rayPoint;
					}
					list = ArrayHelper.Add(this.battleAction.rayObject, list);
				}
				else for(int i=0; i<this.battleAction.target.Length; i++)
				{
					if(this.battleAction.target[i] != null &&
						this.battleAction.target[i].prefabInstance != null)
					{
						list = ArrayHelper.Add(this.battleAction.target[i].prefabInstance, list);
					}
				}
			}
		}
		else if(BattleAnimationObject.ARENA.Equals(obj))
		{
			list = ArrayHelper.Add(DataHolder.BattleSystem().GetArenaCenter(), list);
		}
		else if(BattleAnimationObject.PREFAB.Equals(obj) &&
			this.spawnedPrefabs[pID] != null)
		{
			list = ArrayHelper.Add((GameObject)this.spawnedPrefabs[pID], list);
		}
		return list;
	}
	
	public GameObject GetAnimationObject(BattleAnimationObject obj, int pID)
	{
		GameObject actor = null;
		if(BattleAnimationObject.USER.Equals(obj) &&
			this.battleAction.user != null && 
			this.battleAction.user.prefabInstance != null)
		{
			actor = this.battleAction.user.prefabInstance;
		}
		else if(BattleAnimationObject.TARGET.Equals(obj))
		{
			if(this.battleAction.rayTargetSet)
			{
				if(this.battleAction.rayObject == null)
				{
					this.battleAction.rayObject = new GameObject("RaySpot");
					this.battleAction.rayObject.transform.position = this.battleAction.rayPoint;
				}
				actor = this.battleAction.rayObject;
			}
			else if(this.battleAction.target != null &&
				this.battleAction.target.Length > 0)
			{
				actor = DataHolder.BattleSystem().GetGroupCenter(this.battleAction.target);
			}
		}
		else if(BattleAnimationObject.ARENA.Equals(obj))
		{
			actor = DataHolder.BattleSystem().GetArenaCenter();
		}
		else if(BattleAnimationObject.PREFAB.Equals(obj) &&
			this.spawnedPrefabs[pID] != null)
		{
			actor = (GameObject)this.spawnedPrefabs[pID];
		}
		return actor;
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetEventData()
	{
		Hashtable val = new Hashtable();
		//val.Add(XMLHandler.NODE_NAME, "gameevent"); -> this is added in BattleAnimationData save handling
		val.Add("steps", this.step.Length.ToString());
		val.Add("returntobasecampos", this.returnToBaseCamPos.ToString());
		val.Add("autodestroyprefabs", this.autoDestroyPrefabs.ToString());
		val.Add("calculationneeded", this.calculationNeeded.ToString());
		
		ArrayList s = new ArrayList();
		if(this.prefab.Length > 0)
		{
			val.Add("prefabs", this.prefab.Length.ToString());
			for(int i=0; i<this.prefabName.Length; i++)
			{
				if("" != this.prefabName[i])
				{
					Hashtable ht = new Hashtable();
					ht.Add("id", i.ToString());
					ht.Add(XMLHandler.NODE_NAME, "prefab");
					ht.Add(XMLHandler.CONTENT, this.prefabName[i]);
					s.Add(ht);
				}
			}
		}
		if(this.audioClip.Length > 0)
		{
			val.Add("audioclips", this.audioClip.Length.ToString());
			for(int i=0; i<this.audioName.Length; i++)
			{
				if("" != this.audioName[i])
				{
					Hashtable ht = new Hashtable();
					ht.Add("id", i.ToString());
					ht.Add(XMLHandler.NODE_NAME, "audioclip");
					ht.Add(XMLHandler.CONTENT, this.audioName[i]);
					s.Add(ht);
				}
			}
		}
		for(int i=0; i<this.step.Length; i++)
		{
			Hashtable ht = this.step[i].GetData();
			ht.Add("id", i.ToString());
			ht.Add(XMLHandler.NODE_NAME, "step");
			s.Add(ht);
		}
		val.Add(XMLHandler.NODES, s);
		return val;
	}
	
	public void LoadEventData(Hashtable val)
	{
		this.step = new AnimationStep[int.Parse((string)val["steps"])];
		if(val.ContainsKey("autodestroyprefabs"))
		{
			this.autoDestroyPrefabs = bool.Parse((string)val["autodestroyprefabs"]);
		}
		if(val.ContainsKey("calculationneeded"))
		{
			this.calculationNeeded = bool.Parse((string)val["calculationneeded"]);
		}
		if(val.ContainsKey("prefabs"))
		{
			int count = int.Parse((string)val["prefabs"]);
			this.prefabName = new string[count];
			this.prefab = new GameObject[count];
			for(int i=0; i<count; i++) this.prefabName[i] = "";
		}
		if(val.ContainsKey("audioclips"))
		{
			int count = int.Parse((string)val["audioclips"]);
			this.audioClip = new AudioClip[count];
			this.audioName = new string[count];
			for(int i=0; i<count; i++) this.audioName[i] = "";
		}
		if(val.ContainsKey("returntobasecampos")) this.returnToBaseCamPos = bool.Parse((string)val["returntobasecampos"]);
		
		if(val.ContainsKey(XMLHandler.NODES))
		{
			ArrayList subs = val[XMLHandler.NODES] as ArrayList;
			foreach(Hashtable ht in subs)
			{
				if(ht[XMLHandler.NODE_NAME] as string == "prefab")
				{
					int i = int.Parse((string)ht["id"]);
					this.prefabName[i] = ht[XMLHandler.CONTENT] as string;
				}
				else if(ht[XMLHandler.NODE_NAME] as string == "audioclip")
				{
					int i = int.Parse((string)ht["id"]);
					this.audioName[i] = ht[XMLHandler.CONTENT] as string;
				}
				else if(ht[XMLHandler.NODE_NAME] as string == "step")
				{
					int i = int.Parse((string)ht["id"]);
					BattleAnimationType t = (BattleAnimationType)System.Enum.Parse(typeof(BattleAnimationType), (string)ht["type"]);
					AnimationStep s = this.TypeToStep(t);
					
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
					if(ht.ContainsKey("prefab2")) s.prefabID2 = int.Parse((string)ht["prefab2"]);
					if(ht.ContainsKey("skill")) s.skillID = int.Parse((string)ht["skill"]);
					if(ht.ContainsKey("audio")) s.audioID = int.Parse((string)ht["audio"]);
					if(ht.ContainsKey("formula")) s.formulaID = int.Parse((string)ht["formula"]);
					
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
					if(ht.ContainsKey("statusorigin")) s.statusOrigin = (StatusOrigin)System.Enum.Parse(typeof(StatusOrigin), (string)ht["statusorigin"]);
					if(ht.ContainsKey("statusorigin2")) s.statusOrigin2 = (StatusOrigin)System.Enum.Parse(typeof(StatusOrigin), (string)ht["statusorigin2"]);
					if(ht.ContainsKey("combatantanimation")) s.combatantAnimation = (CombatantAnimation)System.Enum.Parse(typeof(CombatantAnimation), (string)ht["combatantanimation"]);
					if(ht.ContainsKey("movetotarget")) s.moveToTarget = (BattleMoveToTarget)System.Enum.Parse(typeof(BattleMoveToTarget), (string)ht["movetotarget"]);
					if(ht.ContainsKey("audiorolloffmode")) s.audioRolloffMode = (AudioRolloffMode)System.Enum.Parse(typeof(AudioRolloffMode), (string)ht["audiorolloffmode"]);
					if(ht.ContainsKey("simpleoperator")) s.simpleOperator = (SimpleOperator)System.Enum.Parse(typeof(SimpleOperator), (string)ht["simpleoperator"]);
					if(ht.ContainsKey("valuecheck")) s.valueCheck = (ValueCheck)System.Enum.Parse(typeof(ValueCheck), (string)ht["valuecheck"]);
					if(ht.ContainsKey("animationobject"))
					{
						s.animationObject = (BattleAnimationObject)System.Enum.Parse(typeof(BattleAnimationObject), (string)ht["animationobject"]);
					}
					else
					{
						if(StatusOrigin.USER.Equals(s.statusOrigin)) s.animationObject = BattleAnimationObject.USER;
						else if(StatusOrigin.TARGET.Equals(s.statusOrigin)) s.animationObject = BattleAnimationObject.TARGET;
						else if((s is SpawnPrefabAStep && s.show) || s.show2) s.animationObject = BattleAnimationObject.ARENA;
						else if(s.show3) s.animationObject = BattleAnimationObject.PREFAB;
					}
					
					if(ht.ContainsKey("materialproperty")) s.materialProperty = ht["materialproperty"] as string;
					
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
							else if(ht2[XMLHandler.NODE_NAME] as string == "statuseffect")
							{
								int id = int.Parse((string)ht2["id"]);
								if(id < s.effect.Length)
								{
									s.effect[id] = (SkillEffect)System.Enum.Parse(typeof(SkillEffect), (string)ht2["effect"]);
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
