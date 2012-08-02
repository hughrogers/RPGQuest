
using System.Collections;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
	// dialogue handling
	protected GameEvent dlgEvent;
	protected int nextStep = 0;
	protected bool showDialogue = false;
	protected bool showItemCollection = false;
	protected string dialogueName = "";
	protected string dialogueMessage = "";
	protected DialoguePosition dialoguePosition;
	protected ItemCollector itemCollector;
	protected bool dialogueWait = false;
	protected bool dialogueBlockAccept = false;
	protected float dialogueTime = 0;
	protected float dialogueTime2 = 0;
	protected SpeakerPortrait speakerPortrait = null;
	
	// choice handling
	protected bool showChoice = false;
	protected ChoiceContent[] contentChoice;
	protected float choiceTimeout = 0;
	
	// info handling
	protected bool showInfo = false;
	protected float infoTime = 0;
	protected string infoMessage = "";
	protected DialoguePosition infoPosition;
	
	// battle info handling
	protected bool showBattleInfo = false;
	protected float battleInfoTime = 0;
	
	// battle message handling
	protected bool showBattleMessage = false;
	protected string battleMessageText = "";
	protected DialoguePosition battleMessagePosition;
	protected float battleMessageTime = 0;
	protected int battleMessageColor = 0;
	protected int battleMessageSColor = 1;
	
	// battle menu handling
	protected bool showBattleMenu = false;
	protected DialoguePosition battleMenuPosition;
	protected int bmUserIndex = 0;
	protected Character[] currentBMUser = new Character[0];
	protected ArrayList lastBMSelections = new ArrayList();
	protected bool getLastBMIndex = false;
	protected bool markBackBM = false;
	protected bool markSwitchBM = false;
	protected BattleMenuMode switchModeBM = BattleMenuMode.BASE;
	
	// battle end
	protected bool showBattleGains = false;
	protected string[] battleEndMessages;
	protected int currentBattleEndPos = 0;
	
	// save point
	protected bool showSavePoint = false;
	protected bool showSaveMenu = false;
	protected bool showLoadMenu = false;
	protected bool showSaveQuestion = false;
	protected bool showLoadQuestion = false;
	protected int selectedSaveFile = 0;
	
	// main menu
	protected bool showMainMenu = false;
	protected bool showLanguageMenu = false;
	protected bool showAboutInfo = false;
	protected bool showDifficultyMenu = false;
	
	protected bool eventOkPressed = false;
	protected bool interactFlag = false;
	
	// question
	protected bool showQuestion = false;
	protected QuestionInterface currentQuestion = null;
	
	// matrix
	public Matrix4x4 defaultMatrix;
	public Matrix4x4 guiMatrix;
	public Matrix4x4 revertMatrix;
	
	// huds
	public HUD[] hud = new HUD[0];
	
	// global events
	public GlobalEvent[] globalEvent = new GlobalEvent[0];
	public GlobalEvent[] calledGlobalEvents = new GlobalEvent[0];
	
	// screen fade
	public ScreenFader screenFader;
	
	// interaction list
	public ArrayList interactionList = new ArrayList();
	public ArrayList areaNameList = new ArrayList();
	
	void Awake()
	{
		DontDestroyOnLoad(transform);
		if(GUISystemType.ORK.Equals(DataHolder.GameSettings().guiSystemType))
		{
			this.useGUILayout = false;
			if(!this.screenFader) this.screenFader = (ScreenFader)this.gameObject.AddComponent("ScreenFader");
			if(DataHolder.GameSettings().preloadFonts) DataHolder.Fonts().PreloadFonts();
			if(DataHolder.GameSettings().preloadBoxes) DataHolder.DialoguePositions().PreloadBoxes();
			if(DataHolder.GameSettings().autoRemoveLayer) this.RemoveGUILayer();
		}
		else if(!this.screenFader) 
		{
			this.screenFader = (ScreenFaderGUI)this.gameObject.AddComponent("ScreenFaderGUI");
		}
		
		// GUI scaling
		this.CreateGUIMatrix();
		
		// audio
		if(!this.audio) this.gameObject.AddComponent("AudioSource");
		
		// load huds
		this.hud = new HUD[DataHolder.HUDs().GetDataCount()];
		for(int i=0; i<this.hud.Length; i++)
		{
			this.hud[i] = DataHolder.HUDs().GetCopy(i);
		}
		
		
		Camera cam = Camera.main;
		if(cam != null) DataHolder.GameSettings().cameraControlSettings.AddCameraControl(cam.gameObject);
		
		this.StartCoroutine(this.SceneCheck());
	}
	
	private void CreateGUIMatrix()
	{
		Vector3 guiScale = new Vector3(1, 1, 1);
		Vector3 revertGuiScale = new Vector3(1, 1, 1);
		
		guiScale.x = Screen.width / DataHolder.GameSettings().defaultScreen.x;
		guiScale.y = Screen.height / DataHolder.GameSettings().defaultScreen.y;
		revertGuiScale.x = DataHolder.GameSettings().defaultScreen.x / Screen.width;
		revertGuiScale.y = DataHolder.GameSettings().defaultScreen.y / Screen.height;
		
		if(GUIScaleMode.SCALE_AND_CROP.Equals(DataHolder.GameSettings().guiScaleMode))
		{
			if(guiScale.x > guiScale.y)
			{
				guiScale.y = guiScale.x;
				revertGuiScale.y = revertGuiScale.x;
			}
			else
			{
				guiScale.x = guiScale.y;
				revertGuiScale.x = revertGuiScale.y;
			}
		}
		else if(GUIScaleMode.SCALE_TO_FIT.Equals(DataHolder.GameSettings().guiScaleMode))
		{
			if(guiScale.x < guiScale.y)
			{
				guiScale.y = guiScale.x;
				revertGuiScale.y = revertGuiScale.x;
			}
			else
			{
				guiScale.x = guiScale.y;
				revertGuiScale.x = revertGuiScale.y;
			}
		}
		else if(GUIScaleMode.NO_SCALE.Equals(DataHolder.GameSettings().guiScaleMode))
		{
			guiScale.x = 1;
			guiScale.y = 1;
			revertGuiScale.x = 1;
			revertGuiScale.y = 1;
		}
		
		this.guiMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, guiScale);
		this.revertMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, revertGuiScale);
	}
	
	private IEnumerator SceneCheck()
	{
		if(DataHolder.MainMenu().autoCall &&
			Application.loadedLevelName == DataHolder.MainMenu().mainMenuScene)
		{
			if(DataHolder.MainMenu().waitTime > 0)
			{
				yield return new WaitForSeconds(DataHolder.MainMenu().waitTime);
			}
			else yield return null;
			this.CallMainMenu();
		}
	}
	
	public void CallGameOverChoice()
	{
		if(DataHolder.GameSettings().gameOver.showChoice)
		{
			this.StartCoroutine(this.CallGameOverChoice2());
		}
	}
	
	private IEnumerator CallGameOverChoice2()
	{
		if(DataHolder.GameSettings().gameOver.waitTime > 0)
		{
			yield return new WaitForSeconds(DataHolder.GameSettings().gameOver.waitTime);
		}
		else yield return null;
		this.ShowQuestion(DataHolder.GameSettings().gameOver);
	}
	
	public void ClearGlobalEvents()
	{
		this.globalEvent = new GlobalEvent[0];
		this.calledGlobalEvents = new GlobalEvent[0];
	}
	
	public void InitGlobalEvents()
	{
		this.globalEvent = new GlobalEvent[DataHolder.GlobalEvents().GetDataCount()];
		for(int i=0; i<this.globalEvent.Length; i++)
		{
			this.globalEvent[i] = DataHolder.GlobalEvent(i).GetCopy();
			this.globalEvent[i].LoadEvent();
		}
	}
	
	public void CallGlobalEvent(int id)
	{
		if(id < DataHolder.GlobalEventCount)
		{
			GlobalEvent evt = DataHolder.GlobalEvent(id).GetCopy();
			evt.LoadEvent();
			if(evt.Call())
			{
				this.calledGlobalEvents = ArrayHelper.Add(evt, this.calledGlobalEvents);
			}
		}
	}
	
	void OnLevelWasLoaded(int level)
	{
		DataHolder.BattleSystem().ClearBattle(true);
		GameHandler.ClearNoSpawns();
		GameHandler.ClearNoClickMoves();
		GameHandler.RestoreControl();
		this.interactionList = new ArrayList();
		this.areaNameList = new ArrayList();
		if(GUISystemType.ORK.Equals(DataHolder.GameSettings().guiSystemType))
		{
			if(DataHolder.GameSettings().autoRemoveLayer) this.RemoveGUILayer();
			if(TextureDelete.SCENE.Equals(DataHolder.GameSettings().battleTextureDelete))
			{
				GameHandler.Party().DeleteBattleTextures();
			}
		}
		Camera cam = Camera.main;
		if(cam != null) DataHolder.GameSettings().cameraControlSettings.AddCameraControl(cam.gameObject);
		this.StartCoroutine(this.SceneCheck());
	}
	
	public void RemoveGUILayer()
	{
		Camera[] cam = Camera.allCameras;
		for(int i=0; i<cam.Length; i++)
		{
			if(cam[i].name != "GUICamera")
			{
				cam[i].cullingMask -= 1 << 31;
			}
		}
	}
	
	public void RegisterPreloadAN(AreaName an)
	{
		this.areaNameList.Add(an);
	}
	
	public bool InteractionListContains(GameObject other)
	{
		bool contains = false;
		for(int i=0; i<this.interactionList.Count; i++)
		{
			if(this.interactionList[i] == other)
			{
				contains = true;
				break;
			}
		}
		return contains;
	}
	
	private bool CheckDialogueAccept()
	{
		bool val = false;
		if((this.showBattleGains || this.showBattleMenu) &&
			this.battleMenuPosition.GetAcceptPressed())
		{
			val = true;
		}
		else if((this.showMainMenu || this.showDialogue || this.showChoice ||
			this.showSavePoint || this.showSaveMenu || this.showLoadMenu ||
			this.showSaveQuestion || this.showLoadQuestion) && 
			this.dialoguePosition.GetAcceptPressed())
		{
			val = true;
		}
		return val;
	}
	
	public bool StartInteraction()
	{
		bool interact = false;
		if(GameHandler.GetPlayer() && GameHandler.IsControlField() && 
			!this.IsControlShown())
		{
			InteractionController ic = (InteractionController)GameHandler.GetPlayer().
					GetComponentInChildren(typeof(InteractionController));
			if(ic != null && ic.Interact(this.interactionList))
			{
				interact = true;
				this.interactFlag = true;
			}
		}
		return interact;
	}
	
	public bool IsControlShown()
	{
		return this.showDialogue || this.showChoice || this.showSavePoint || 
				this.showSaveMenu || this.showLoadMenu || this.showSaveQuestion || 
				this.showLoadQuestion || this.showMainMenu || this.showBattleMessage || 
				this.showBattleMenu || this.showBattleGains;
	}
	
	public bool CheckHUDClick(Vector2 mp)
	{
		bool clicked = false;
		for(int i=0; i<this.hud.Length; i++)
		{
			if(this.hud[i].ClickHUD(mp, this.interactionList.Count))
			{
				clicked = true;
				break;
			}
		}
		return clicked;
	}
	
	void Update()
	{
		float t = GameHandler.DeltaTime;
		// preloading
		if(GUISystemType.ORK.Equals(DataHolder.GameSettings().guiSystemType))
		{
			if(DataHolder.GameSettings().preloadBoxes && 
				DataHolder.DialoguePositions().HasNextPreload())
			{
				DataHolder.DialoguePositions().PreloadNext();
			}
			if(this.areaNameList.Count > 0)
			{
				((AreaName)this.areaNameList[0]).Preload();
				this.areaNameList.RemoveAt(0);
			}
		}
		
		if(ControlHandler.IsPressed(DataHolder.GameSettings().pauseKey))
		{
			GameHandler.PauseGame(!GameHandler.IsGamePaused());
		}
		
		for(int i=0; i<this.globalEvent.Length; i++)
		{
			this.globalEvent[i].Tick(t);
		}
		for(int i=0; i<this.calledGlobalEvents.Length; i++)
		{
			if(this.calledGlobalEvents[i].IsFinished())
			{
				this.calledGlobalEvents = ArrayHelper.Remove(i--, this.calledGlobalEvents);
			}
			else this.calledGlobalEvents[i].Tick(t);
		}
		
		// generate gui matrix
		this.CreateGUIMatrix();
		
		// clearings
		if(!GameHandler.IsControlBattle()) this.showBattleMenu = false;
		if(!DataHolder.GameSettings().pauseTime) GameHandler.AddGameTime(Time.deltaTime);
		GameHandler.WindowHandler().Tick(Time.deltaTime);
		
		// fading huds
		for(int i=0; i<this.hud.Length; i++)
		{
			this.hud[i].DoFade(Time.deltaTime, this.interactionList.Count);
		}
		
		if(!GameHandler.IsGamePaused())
		{
			if(GameHandler.IsControlField() || GameHandler.IsControlBattle())
			{
				GameHandler.Party().Tick();
			}
			if(GameHandler.IsControlBattle())
			{
				DataHolder.BattleCam().Tick();
			}
			if(DataHolder.GameSettings().pauseTime) GameHandler.AddGameTime(Time.deltaTime);
			
			// dialogue auto close time
			if(this.showDialogue && this.dialogueWait && this.dialogueTime2 > 0)
			{
				this.dialogueTime2 -= t;
				if(this.dialogueTime2 <= 0) this.PressOk();
			}
			
			// fading dialogues etc.
			if(this.useGUILayout)
			{
				// fading dialogue positions
				if(this.showDialogue || this.showChoice || this.showBattleInfo ||
					this.showSavePoint || this.showSaveMenu || this.showLoadMenu ||
					this.showSaveQuestion || this.showLoadQuestion || this.showMainMenu)
				{
					this.dialoguePosition.DoFade(Time.deltaTime);
				}
				if(this.showBattleMessage)
				{
					this.battleMessagePosition.DoFade(Time.deltaTime);
				}
				if(this.showBattleMenu || this.showBattleGains)
				{
					this.battleMenuPosition.DoFade(Time.deltaTime);
				}
			}
			// ORK gui fade out checks
			else this.FadeOutChecking();
			
			// info time
			if(this.showInfo)
			{
				this.infoPosition.DoFade(Time.deltaTime);
				this.infoTime -= t;
				if(!this.infoPosition.IsFadingOut() && this.infoTime <= this.infoPosition.fadeOutTime)
				{
					this.infoPosition.InitOut();
				}
				if(this.infoTime <= 0)
				{
					this.showInfo = false;
				}
			}
			// battle info time
			if(this.showBattleMessage)
			{
				this.battleMessageTime -= t;
				if(!this.battleMessagePosition.IsFadingOut() && this.battleMessageTime <= this.battleMessagePosition.fadeOutTime)
				{
					this.battleMessagePosition.InitOut();
				}
				if(this.battleMessageTime <= 0)
				{
					this.showBattleMessage = false;
				}
			}
			if(GameHandler.IsControlBattle() && DataHolder.BattleSystem().battleRunning)
			{
				if(this.showBattleInfo)
				{
					this.battleInfoTime -= t;
					if(!this.dialoguePosition.IsFadingOut() && this.battleInfoTime <= this.dialoguePosition.fadeOutTime)
					{
						this.dialoguePosition.InitOut();
					}
					if(this.battleInfoTime <= 0)
					{
						this.showBattleInfo = false;
					}
				}
			}
			
			// keyboard inputs
			if(DataHolder.GameSettings().IsKeyboardAllowed())
			{
				// accept
				if(ControlHandler.IsAcceptPressed() || this.CheckDialogueAccept())
				{
					if(GameHandler.GetPlayer() && GameHandler.IsControlField())
					{
						if(!this.StartInteraction() && !(this.dialogueWait && this.dialogueBlockAccept))
						{
							this.PressOk();
						}
					}
					else if(!(this.dialogueWait && this.dialogueBlockAccept))
					{
						this.PressOk();
					}
				}
				
				// cancel
				if(ControlHandler.IsCancelPressed())
				{
					if((GameHandler.IsControlBattle() && this.showBattleMenu) && 
						this.battleMenuPosition.CanControl() &&
						(this.lastBMSelections.Count > 0 || DataHolder.BattleSystem().IsRealTime()))
					{
						DataHolder.GameSettings().PlayCancelAudio(this.audio);
						this.battleMenuPosition.InitOut();
						this.markBackBM = true;
					}
					else if(((GameHandler.IsControlSave() && 
							(this.showSavePoint || this.showSaveMenu || this.showLoadMenu ||
							this.showSaveQuestion || this.showLoadQuestion)) ||
						(GameHandler.IsControlMenu() && 
							(this.showLanguageMenu || this.showAboutInfo || this.showDifficultyMenu))) &&
							!this.dialoguePosition.isFading && !this.dialoguePosition.isMoving)
					{
						DataHolder.GameSettings().PlayCancelAudio(this.audio);
						if(this.showAboutInfo) this.dialogueMessage = "";
						else this.dialoguePosition.multiLabel.SetSelection(this.contentChoice.Length-1);
						this.dialoguePosition.InitOut();
					}
				}
				
				if(GameHandler.IsControlField() && DataHolder.GameSettings().switchPlayer &&
					((DataHolder.GameSettings().switchOnlyBP && GameHandler.Party().GetBattlePartySize() > 1) ||
					(!DataHolder.GameSettings().switchOnlyBP && GameHandler.Party().GetPartySize() > 1)) &&
					!GameHandler.IsBlockControl())
				{
					if(ControlHandler.IsCharPlusPressed())
					{
						Character c = GameHandler.Party().GetCharacterOffset(
								GameHandler.Party().GetPlayerID(), 1, 
								DataHolder.GameSettings().switchOnlyBP);
						if(c != null)
						{
							Character p = GameHandler.Party().GetPlayerCharacter();
							if(p.blockedControl)
							{
								p.blockedControl = false;
								GameHandler.SetBlockControl(-1);
							}
							if(c.IsInAction())
							{
								c.blockedControl = true;
								GameHandler.SetBlockControl(1);
							}
							GameHandler.Party().SetPlayer(c);
						}
					}
					else if(ControlHandler.IsCharMinusPressed())
					{
						Character c = GameHandler.Party().GetCharacterOffset(
								GameHandler.Party().GetPlayerID(), -1, 
								DataHolder.GameSettings().switchOnlyBP);
						if(c != null)
						{
							Character p = GameHandler.Party().GetPlayerCharacter();
							if(p.blockedControl)
							{
								p.blockedControl = false;
								GameHandler.SetBlockControl(-1);
							}
							if(c.IsInAction())
							{
								c.blockedControl = true;
								GameHandler.SetBlockControl(1);
							}
							GameHandler.Party().SetPlayer(c);
						}
					}
				}
				
				// choice interaction
				if(((GameHandler.IsControlEvent() && this.showChoice) ||
					(GameHandler.IsControlSave() && (this.showSavePoint || this.showSaveMenu ||
					this.showLoadMenu || this.showSaveQuestion || this.showLoadQuestion)) ||
					(GameHandler.IsControlMenu() && this.showMainMenu) ||
					(this.showQuestion && this.currentQuestion != null && 
					GameHandler.IsControlType(this.currentQuestion.controlType))) && 
						(Time.time - this.choiceTimeout) > DataHolder.GameSettings().cursorTimeout &&
						this.dialoguePosition.CanControl())
				{
					float v = ControlHandler.GetVertical();
					float h = ControlHandler.GetHorizontal();
					if(v < -0.3)
					{
						DataHolder.GameSettings().PlayCursorMoveAudio(this.audio);
						this.dialoguePosition.multiLabel.ChangeSelectionVertical(1);
						this.choiceTimeout = Time.time;
					}
					else if(v > 0.3)
					{
						DataHolder.GameSettings().PlayCursorMoveAudio(this.audio);
						this.dialoguePosition.multiLabel.ChangeSelectionVertical(-1);
						this.choiceTimeout = Time.time;
					}
					else if(h > 0.3)
					{
						DataHolder.GameSettings().PlayCursorMoveAudio(this.audio);
						this.dialoguePosition.multiLabel.ChangeSelectionHorizontal(1);
						this.choiceTimeout = Time.time;
					}
					else if(h < -0.3)
					{
						DataHolder.GameSettings().PlayCursorMoveAudio(this.audio);
						this.dialoguePosition.multiLabel.ChangeSelectionHorizontal(-1);
						this.choiceTimeout = Time.time;
					}
				}
				// battle menu interaction
				else if(GameHandler.IsControlBattle() && this.showBattleMenu && 
					!DataHolder.BattleSystem().battleEnd && this.battleMenuPosition.CanControl())
				{
					// battle menu update tick
					if(this.currentBMUser.Length > 0)
					{
						this.currentBMUser[this.bmUserIndex].battleMenu.Tick();
					}
					
					// menu user change
					if(this.currentBMUser.Length > 1 || 
						(DataHolder.BattleSystem().IsRealTime() && 
						((DataHolder.GameSettings().switchOnlyBP && GameHandler.Party().GetBattlePartySize() > 1) ||
						(!DataHolder.GameSettings().switchOnlyBP && GameHandler.Party().GetPartySize() > 1))))
					{
						if(ControlHandler.IsCharPlusPressed())
						{
							if(DataHolder.BattleSystem().IsRealTime())
							{
								Character c = GameHandler.Party().GetCharacterOffset(
										GameHandler.Party().GetPlayerID(), 1, 
										DataHolder.GameSettings().switchOnlyBP);
								if(c != null)
								{
									Character p = GameHandler.Party().GetPlayerCharacter();
									if(p.blockedControl)
									{
										p.blockedControl = false;
										GameHandler.SetBlockControl(-1);
									}
									if(c.IsInAction())
									{
										c.blockedControl = true;
										GameHandler.SetBlockControl(1);
									}
									this.currentBMUser[this.bmUserIndex].EndBattleMenu(true);
									GameHandler.Party().SetPlayer(c);
									c.ChooseAction();
								}
							}
							else
							{
								this.currentBMUser[this.bmUserIndex].battleMenu.Reset(
										this.battleMenuPosition.multiLabel.GetSelection());
								this.bmUserIndex++;
								if(this.bmUserIndex >= this.currentBMUser.Length) this.bmUserIndex = 0;
								this.lastBMSelections = new ArrayList();
								this.currentBMUser[this.bmUserIndex].battleMenu.Reset(
										this.battleMenuPosition.multiLabel.GetSelection());
								this.ExitBattleMenu();
							}
						}
						else if(ControlHandler.IsCharMinusPressed())
						{
							if(DataHolder.BattleSystem().IsRealTime())
							{
								Character c = GameHandler.Party().GetCharacterOffset(
										GameHandler.Party().GetPlayerID(), -1, 
										DataHolder.GameSettings().switchOnlyBP);
								if(c != null)
								{
									Character p = GameHandler.Party().GetPlayerCharacter();
									if(p.blockedControl)
									{
										p.blockedControl = false;
										GameHandler.SetBlockControl(-1);
									}
									if(c.IsInAction())
									{
										c.blockedControl = true;
										GameHandler.SetBlockControl(1);
									}
									this.currentBMUser[this.bmUserIndex].EndBattleMenu(true);
									GameHandler.Party().SetPlayer(c);
									c.ChooseAction();
								}
							}
							else
							{
								this.currentBMUser[this.bmUserIndex].battleMenu.Reset(
										this.battleMenuPosition.multiLabel.GetSelection());
								this.bmUserIndex--;
								if(this.bmUserIndex < 0) this.bmUserIndex = this.currentBMUser.Length-1;
								this.lastBMSelections = new ArrayList();
								this.currentBMUser[this.bmUserIndex].battleMenu.Reset(
										this.battleMenuPosition.multiLabel.GetSelection());
								this.ExitBattleMenu();
							}
						}
					}
					
					if(this.currentBMUser.Length > 0 && 
						this.currentBMUser[this.bmUserIndex].battleMenu.choices.Length > 0)
					{
						// skill level changes
						if(ControlHandler.IsSkillPlusPressed() &&
							this.currentBMUser[this.bmUserIndex].battleMenu.IsSkillMenu())
						{
							DataHolder.GameSettings().PlaySkillLevelAudio(this.audio);
							this.currentBMUser[this.bmUserIndex].battleMenu.ChangeSkillLevel(
									this.battleMenuPosition.multiLabel.GetSelection(), 1);
							this.battleMenuPosition.multiLabel.newContent = true;
						}
						else if(ControlHandler.IsSkillMinusPressed() &&
							this.currentBMUser[this.bmUserIndex].battleMenu.IsSkillMenu())
						{
							DataHolder.GameSettings().PlaySkillLevelAudio(this.audio);
							this.currentBMUser[this.bmUserIndex].battleMenu.ChangeSkillLevel(
									this.battleMenuPosition.multiLabel.GetSelection(), -1);
							this.battleMenuPosition.multiLabel.newContent = true;
						}
						
						if((Time.time - this.choiceTimeout) > DataHolder.GameSettings().cursorTimeout)
						{
							float v = ControlHandler.GetVertical();
							float h = ControlHandler.GetHorizontal();
							if(v < -0.3)
							{
								DataHolder.GameSettings().PlayCursorMoveAudio(this.audio);
								this.battleMenuPosition.multiLabel.ChangeSelectionVertical(1);
								this.choiceTimeout = Time.time;
								if(DataHolder.BattleMenu().useTargetBlink || 
									DataHolder.BattleMenu().useTargetCursor)
								{
									this.currentBMUser[this.bmUserIndex].battleMenu.
											SetSelectedTarget(this.battleMenuPosition.multiLabel.GetSelection());
								}
							}
							else if(v > 0.3)
							{
								DataHolder.GameSettings().PlayCursorMoveAudio(this.audio);
								this.battleMenuPosition.multiLabel.ChangeSelectionVertical(-1);
								this.choiceTimeout = Time.time;
								if(DataHolder.BattleMenu().useTargetBlink || 
									DataHolder.BattleMenu().useTargetCursor)
								{
									this.currentBMUser[this.bmUserIndex].battleMenu.
											SetSelectedTarget(this.battleMenuPosition.multiLabel.GetSelection());
								}
							}
							else if(h > 0.3)
							{
								DataHolder.GameSettings().PlayCursorMoveAudio(this.audio);
								this.battleMenuPosition.multiLabel.ChangeSelectionHorizontal(1);
								this.choiceTimeout = Time.time;
								if(DataHolder.BattleMenu().useTargetBlink || 
									DataHolder.BattleMenu().useTargetCursor)
								{
									this.currentBMUser[this.bmUserIndex].battleMenu.
											SetSelectedTarget(this.battleMenuPosition.multiLabel.GetSelection());
								}
							}
							else if(h < -0.3)
							{
								DataHolder.GameSettings().PlayCursorMoveAudio(this.audio);
								this.battleMenuPosition.multiLabel.ChangeSelectionHorizontal(-1);
								this.choiceTimeout = Time.time;
								if(DataHolder.BattleMenu().useTargetBlink || 
									DataHolder.BattleMenu().useTargetCursor)
								{
									this.currentBMUser[this.bmUserIndex].battleMenu.
											SetSelectedTarget(this.battleMenuPosition.multiLabel.GetSelection());
								}
							}
						}
					}
				}
				// dialogue scroll
				else if(GameHandler.IsControlEvent() && this.showDialogue && 
						this.dialoguePosition.CanControl())
				{
					float v = ControlHandler.GetVertical();
					if(v < -0.3)
					{
						this.dialoguePosition.multiLabel.ChangeScroll(DataHolder.GameSettings().scrollSpeed);
					}
					else if(v > 0.3)
					{
						this.dialoguePosition.multiLabel.ChangeScroll(-DataHolder.GameSettings().scrollSpeed);
					}
				}
			}
			if(GameHandler.IsControlBattle())
			{
				DataHolder.BattleControl().Tick(this.interactFlag || this.IsControlShown());
			}
			this.interactFlag = false;
		}
	}
	
	/*
	============================================================================
	Dialogue functions
	============================================================================
	*/
	public void ShowDialogue(string n, string m, int dp, GameEvent gameEvent, int ns, 
			bool wait, float time, bool block, SpeakerPortrait sp)
	{
		this.dlgEvent = gameEvent;
		this.dialogueName = n;
		this.dialogueMessage = m;
		this.nextStep = ns;
		this.dialogueWait = wait;
		this.speakerPortrait = sp;
		this.dialogueBlockAccept = block;
		this.dialogueTime = time;
		this.dialogueTime2 = time;
		if(this.dialoguePosition != null) this.dialoguePosition.SetOutDone();
		this.dialoguePosition = DataHolder.DialoguePositions().GetCopy(dp);
		if(this.useGUILayout) this.dialoguePosition.InitIn();
		else this.dialoguePosition.Show(this.dialogueName, this.dialogueMessage, null, sp, null, true);
		this.showDialogue = true;
	}
	
	public void ShowItemCollection(string m, string[] ch, ItemCollector ic)
	{
		this.itemCollector = ic;
		this.dialogueName = "";
		this.dialogueMessage = m;
		if(DataHolder.GameSettings().itemCollectionChoice) this.contentChoice = MultiLabel.CreateChoiceContents(ch);
		else this.contentChoice = null;
		if(this.dialoguePosition != null) this.dialoguePosition.SetOutDone();
		this.dialoguePosition = DataHolder.DialoguePositions().GetCopy(DataHolder.GameSettings().itemCollectionPosition);
		if(this.useGUILayout) this.dialoguePosition.InitIn();
		else this.dialoguePosition.Show(this.dialogueName, 
				this.dialogueMessage, this.contentChoice, null, null, true);
		if(DataHolder.GameSettings().itemCollectionChoice) this.showChoice = true;
		else this.showDialogue = true;
		this.showItemCollection = true;
	}
	
	public void ShowChoice(string n, string m, string[] ch, int dp, GameEvent gameEvent, SpeakerPortrait sp)
	{
		this.dlgEvent = gameEvent;
		this.dialogueName = n;
		this.dialogueMessage = m;
		this.contentChoice = MultiLabel.CreateChoiceContents(ch);
		this.speakerPortrait = sp;
		if(this.dialoguePosition != null) this.dialoguePosition.SetOutDone();
		this.dialoguePosition = DataHolder.DialoguePositions().GetCopy(dp);
		if(this.useGUILayout) this.dialoguePosition.InitIn();
		else this.dialoguePosition.Show(this.dialogueName, 
				this.dialogueMessage, this.contentChoice, sp, null, true);
		this.showChoice = true;
	}
	
	public void ShowQuestion(QuestionInterface question)
	{
		this.currentQuestion = question;
		this.currentQuestion.CreateChoices();
		GameHandler.SetControlType(this.currentQuestion.controlType);
		this.dialogueName = "";
		this.dialogueMessage = this.currentQuestion.message;
		this.contentChoice = this.currentQuestion.choices;
		if(this.dialoguePosition != null) this.dialoguePosition.SetOutDone();
		this.dialoguePosition = DataHolder.DialoguePositions().GetCopy(this.currentQuestion.dialoguePosition);
		if(this.useGUILayout) this.dialoguePosition.InitIn();
		else this.dialoguePosition.Show(this.dialogueName, 
				this.dialogueMessage, this.contentChoice, null, null, true);
		this.showQuestion = true;
		this.showChoice = true;
	}
	
	/*
	============================================================================
	Info display functions
	============================================================================
	*/
	public void ShowInfo(string m, int dp, float t)
	{
		this.infoMessage = m;
		this.infoTime = t;
		if(this.infoPosition != null) this.infoPosition.SetOutDone();
		this.infoPosition = DataHolder.DialoguePositions().GetCopy(dp);
		if(this.useGUILayout) this.infoPosition.InitIn();
		else this.infoPosition.Show("", this.infoMessage, null, null, null, true);
		this.showInfo = true;
	}
	
	public void ShowInfo(DialoguePosition dp, float t)
	{
		this.infoTime = t;
		if(this.infoPosition != null) this.infoPosition.SetOutDone();
		this.infoPosition = dp;
		this.infoPosition.Reset();
		GameHandler.GUIHandler().AddDPSprite(this.infoPosition);
		this.showInfo = true;
	}
	
	/*
	============================================================================
	Battle functions
	============================================================================
	*/
	public void ShowBattleInfo(string m, int dp)
	{
		this.dialogueMessage = m;
		this.battleInfoTime = DataHolder.BattleSystemData().infoShowTime;
		if(this.dialoguePosition != null) this.dialoguePosition.SetOutDone();
		this.dialoguePosition = DataHolder.DialoguePositions().GetCopy(dp);
		if(this.useGUILayout) this.dialoguePosition.InitIn();
		else this.dialoguePosition.Show("", this.dialogueMessage, null, null, null, true);
		this.showBattleInfo = true;
	}
	
	public void ShowBattleMessage(string m, int c1, int c2)
	{
		this.showInfo = false;
		this.showBattleInfo = false;
		this.battleMessageText = m;
		this.battleMessageTime = DataHolder.BattleSystemData().battleMessageShowTime;
		this.battleMessageColor = c1;
		this.battleMessageSColor = c2;
		if(this.battleMessagePosition != null) this.battleMessagePosition.SetOutDone();
		this.battleMessagePosition = DataHolder.DialoguePositions().GetCopy(
				DataHolder.BattleSystemData().battleMessagePosition);
		if(this.useGUILayout) this.battleMessagePosition.InitIn();
		else
		{
			this.battleMessageText = MultiLabel.GetColorString(c1)+
					MultiLabel.GetShadowColorString(c2)+this.battleMessageText;
			this.battleMessagePosition.Show("", this.battleMessageText, null, null, null, true);
			this.battleMessagePosition.SetFocus();
		}
		this.showBattleMessage = true;
	}
	
	public void ShowBattleEnd(string[] messages, bool add)
	{
		this.showBattleMenu = false;
		this.currentBattleEndPos = 0;
		if(add && this.battleEndMessages != null && 
			this.battleEndMessages.Length > 0)
		{
			ArrayList s = new ArrayList();
			for(int i=0; i<this.battleEndMessages.Length; i++) s.Add(this.battleEndMessages[i]);
			for(int i=0; i<messages.Length; i++) s.Add(messages[i]);
			this.battleEndMessages = s.ToArray(typeof(string)) as string[];
		}
		else this.battleEndMessages = messages;
		if(this.battleMenuPosition != null) this.battleMenuPosition.SetOutDone();
		this.battleMenuPosition = DataHolder.DialoguePositions().GetCopy(DataHolder.BattleEnd().dialoguePosition);
		if(this.useGUILayout) this.battleMenuPosition.InitIn();
		else this.battleMenuPosition.Show("", this.battleEndMessages[0], null, null, null, true);
		this.showBattleGains = true;
	}
	
	public void ExitBattleMenu()
	{
		this.showBattleMenu = false;
		this.battleMenuPosition.InitOut();
	}
	
	public ChoiceContent BattleDragStarted(Vector2 pos)
	{
		ChoiceContent cc = null;
		if(this.showBattleMenu) cc = this.battleMenuPosition.multiLabel.GetDragOnPosition(pos);
		return cc;
	}
	
	public void BattleMenuBack(bool removeFirst)
	{
		if(removeFirst) this.lastBMSelections.RemoveAt(0);
		if(this.lastBMSelections.Count > 0)
		{
			if(!this.useGUILayout)
			{
				this.currentBMUser[this.bmUserIndex].battleMenu.StoreMenu(this.battleMenuPosition);
				this.battleMenuPosition.SetOutDone();
			}
			this.currentBMUser[this.bmUserIndex].battleMenu.Back();
			this.getLastBMIndex = true;
			this.showBattleMenu = false;
		}
		else if(DataHolder.BattleSystem().IsRealTime())
		{
			this.currentBMUser[this.bmUserIndex].EndBattleMenu(true);
		}
	}
	
	public void SetLastBMIndex()
	{
		this.getLastBMIndex = true;
	}
	
	public void AddBattleMenuUser(Character c)
	{
		if(this.currentBMUser.Length == 0) this.lastBMSelections = new ArrayList();
		this.currentBMUser = ArrayHelper.Add(c, this.currentBMUser);
	}
	
	public void RemoveBattleMenuUser(Character c)
	{
		this.currentBMUser = ArrayHelper.Remove(c, this.currentBMUser);
		this.showBattleMenu = false;
		if(DataHolder.BattleCam().blockAnimationCams && this.currentBMUser.Length == 0)
		{
			DataHolder.BattleCam().SetMenuUser(null);
		}
	}
	
	public void ClearBattleMenuUsers()
	{
		this.currentBMUser = new Character[0];
		this.showBattleMenu = false;
		if(DataHolder.BattleCam().blockAnimationCams)
		{
			DataHolder.BattleCam().SetMenuUser(null);
		}
	}
	
	public bool BattleMenuActive()
	{
		return this.currentBMUser.Length > 0;
	}
	
	public void BattleTargetClicked(int battleID)
	{
		if(this.currentBMUser.Length > 0 && this.currentBMUser[this.bmUserIndex].battleMenu.IsTargetMenu())
		{
			this.currentBMUser[this.bmUserIndex].battleMenu.TargetClicked(battleID);
		}
	}
	
	public void SetBattleMenuIndex(int index, Character c)
	{
		if(this.battleMenuPosition != null && this.bmUserIndex >= 0 && 
			this.bmUserIndex < this.currentBMUser.Length && 
			this.currentBMUser[this.bmUserIndex] == c)
		{
			this.battleMenuPosition.multiLabel.SetSelection(index);
		}
	}
	
	public void CloseBattleMenu()
	{
		if(this.battleMenuPosition != null &&
			this.battleMenuPosition.CanControl())
		{
			DataHolder.GameSettings().PlayCancelAudio(this.audio);
			this.lastBMSelections = new ArrayList();
			this.markBackBM = true;
			this.battleMenuPosition.InitOut();
		}
	}
	
	public void SwitchBattleMenu(BattleMenuMode mode)
	{
		if(this.battleMenuPosition != null &&
			this.battleMenuPosition.CanControl())
		{
			DataHolder.GameSettings().PlayCancelAudio(this.audio);
			this.lastBMSelections = new ArrayList();
			this.switchModeBM = mode;
			this.markSwitchBM = true;
			this.battleMenuPosition.InitOut();
		}
	}
	
	/*
	============================================================================
	Load/save functions
	============================================================================
	*/
	public void CallSavePointMenu()
	{
		this.dialogueName = "";
		this.dialogueMessage = "";
		this.contentChoice = DataHolder.LoadSaveHUD().GetSavePointChoice();
		if(this.dialoguePosition != null) this.dialoguePosition.SetOutDone();
		this.dialoguePosition = DataHolder.DialoguePositions().GetCopy(DataHolder.LoadSaveHUD().spPosition);
		if(this.useGUILayout) this.dialoguePosition.InitIn();
		else this.dialoguePosition.Show(this.dialogueName, this.dialogueMessage, this.contentChoice, null, null, true);
		this.showSavePoint = true;
	}
	
	public void CallSaveMenu()
	{
		this.dialogueName = "";
		this.dialogueMessage = DataHolder.LoadSaveHUD().GetSaveString();
		this.contentChoice = DataHolder.LoadSaveHUD().GetSaveFileChoice();
		if(this.dialoguePosition != null) this.dialoguePosition.SetOutDone();
		this.dialoguePosition = DataHolder.DialoguePositions().GetCopy(DataHolder.LoadSaveHUD().savePosition);
		if(this.useGUILayout) this.dialoguePosition.InitIn();
		else this.dialoguePosition.Show(this.dialogueName, this.dialogueMessage, this.contentChoice, null, null, true);
		this.showSaveMenu = true;
	}
	
	public void CallLoadMenu()
	{
		this.dialogueName = "";
		this.dialogueMessage = DataHolder.LoadSaveHUD().GetLoadstring();
		this.contentChoice = DataHolder.LoadSaveHUD().GetLoadFileChoice();
		if(this.dialoguePosition != null) this.dialoguePosition.SetOutDone();
		this.dialoguePosition = DataHolder.DialoguePositions().GetCopy(DataHolder.LoadSaveHUD().savePosition);
		if(this.useGUILayout) this.dialoguePosition.InitIn();
		else this.dialoguePosition.Show(this.dialogueName, this.dialogueMessage, this.contentChoice, null, null, true);
		this.showLoadMenu = true;
	}
	
	public void CallSaveQuestion(int index)
	{
		this.dialogueName = "";
		this.dialogueMessage = DataHolder.LoadSaveHUD().GetSaveQuestionString(index);
		this.contentChoice = DataHolder.LoadSaveHUD().GetYesNoChoice();
		if(this.dialoguePosition != null) this.dialoguePosition.SetOutDone();
		this.dialoguePosition = DataHolder.DialoguePositions().GetCopy(DataHolder.LoadSaveHUD().saveQuestionPosition);
		if(this.useGUILayout) this.dialoguePosition.InitIn();
		else this.dialoguePosition.Show(this.dialogueName, this.dialogueMessage, this.contentChoice, null, null, true);
		this.showSaveQuestion = true;
	}
	
	public void CallLoadQuestion(int index)
	{
		this.dialogueName = "";
		this.dialogueMessage = DataHolder.LoadSaveHUD().GetLoadQuestionString(index);
		this.contentChoice = DataHolder.LoadSaveHUD().GetYesNoChoice();
		if(this.dialoguePosition != null) this.dialoguePosition.SetOutDone();
		this.dialoguePosition = DataHolder.DialoguePositions().GetCopy(DataHolder.LoadSaveHUD().saveQuestionPosition);
		if(this.useGUILayout) this.dialoguePosition.InitIn();
		else this.dialoguePosition.Show(this.dialogueName, this.dialogueMessage, this.contentChoice, null, null, true);
		this.showLoadQuestion = true;
	}
	
	public void SaveCanceled()
	{
		this.showSavePoint = false;
		this.showSaveMenu = false;
		this.showLoadMenu = false;
		this.showSaveQuestion = false;
		this.showLoadQuestion = false;
		if(this.showMainMenu) this.CallMainMenu();
		else if(this.showQuestion && this.currentQuestion != null)
		{
			this.ShowQuestion(this.currentQuestion);
		}
		else GameHandler.SetControlType(ControlType.FIELD);
	}
	
	/*
	============================================================================
	Main menu functions
	============================================================================
	*/
	public void CallMainMenu()
	{
		GameHandler.SetControlType(ControlType.MENU);
		this.dialogueName = "";
		this.dialogueMessage = "";
		this.contentChoice = DataHolder.MainMenu().GetMainMenuChoice();
		this.dialoguePosition = DataHolder.DialoguePositions().GetCopy(DataHolder.MainMenu().menuPosition);
		if(this.useGUILayout) this.dialoguePosition.InitIn();
		else this.dialoguePosition.Show(this.dialogueName, this.dialogueMessage, this.contentChoice, null, null, true);
		this.showMainMenu = true;
	}
	
	public void CallLanguageMenu()
	{
		this.dialogueName = "";
		this.dialogueMessage = "";
		this.contentChoice = DataHolder.Languages().GetLanguageChoice();
		this.dialoguePosition = DataHolder.DialoguePositions().GetCopy(DataHolder.MainMenu().menuPosition);
		this.dialoguePosition.multiLabel.SetSelection(GameHandler.GetLanguage());
		if(this.useGUILayout) this.dialoguePosition.InitIn();
		else this.dialoguePosition.Show(this.dialogueName, this.dialogueMessage, this.contentChoice, null, null, true);
		this.showLanguageMenu = true;
	}
	
	public void CallAboutInfo()
	{
		this.dialogueName = "";
		this.dialogueMessage = DataHolder.MainMenu().GetAboutInfo();
		this.dialoguePosition = DataHolder.DialoguePositions().GetCopy(DataHolder.MainMenu().aboutPosition);
		if(this.useGUILayout) this.dialoguePosition.InitIn();
		else this.dialoguePosition.Show(this.dialogueName, this.dialogueMessage, null, null, null, true);
		this.showAboutInfo = true;
	}
	
	public void CallDifficultyMenu()
	{
		this.dialogueName = "";
		this.dialogueMessage = DataHolder.MainMenu().GetDifficultyQuestion();
		this.contentChoice = DataHolder.Difficulties().GetDifficultyChoice(true);
		this.dialoguePosition = DataHolder.DialoguePositions().GetCopy(DataHolder.MainMenu().menuPosition);
		this.dialoguePosition.multiLabel.SetSelection(GameHandler.GetDifficulty());
		if(this.useGUILayout) this.dialoguePosition.InitIn();
		else this.dialoguePosition.Show(this.dialogueName, this.dialogueMessage, this.contentChoice, null, null, true);
		this.showDifficultyMenu = true;
	}
	
	/*
	============================================================================
	Ok/choice functions
	============================================================================
	*/
	public void PressOk()
	{
		if(this.useGUILayout)
		{
			this.eventOkPressed = true;
		}
		// ORK gui handling
		else
		{
			// battle gains
			if(this.showBattleGains && !this.battleMenuPosition.outDone && !this.battleMenuPosition.IsFadingOut())
			{
				DataHolder.GameSettings().PlayAcceptAudio(this.audio);
				this.battleEndMessages[this.currentBattleEndPos] = this.battleMenuPosition.multiLabel.GetUnshownText();
				if(this.battleEndMessages[this.currentBattleEndPos].Length == 0)
				{
					this.battleMenuPosition.InitOut();
				}
			}
			// battle menu
			else if(GameHandler.IsControlBattle() && 
				DataHolder.BattleSystem().battleRunning && !this.showBattleGains)
			{
				if(this.bmUserIndex < 0) this.bmUserIndex = this.currentBMUser.Length-1;
				else if(this.bmUserIndex >= this.currentBMUser.Length) this.bmUserIndex = 0;
				if(this.currentBMUser.Length > 0 && this.currentBMUser[this.bmUserIndex].IsChoosingAction())
				{
					if(!this.currentBMUser[this.bmUserIndex].isDead &&
						!this.battleMenuPosition.outDone && !this.battleMenuPosition.IsFadingOut())
					{
						if(this.currentBMUser[this.bmUserIndex].battleMenu.choices[
							this.battleMenuPosition.multiLabel.GetSelection()].active)
						{
							this.lastBMSelections.Insert(0, this.battleMenuPosition.multiLabel.GetSelection());
							DataHolder.GameSettings().PlayAcceptAudio(this.audio);
							this.battleMenuPosition.InitOut();
						}
						else
						{
							DataHolder.GameSettings().PlayFailAudio(this.audio);
						}
					}
				}
			}
			// main menu handling
			else if(GameHandler.IsControlMenu() && this.showMainMenu &&
				!this.dialoguePosition.outDone && !this.dialoguePosition.IsFadingOut())
			{
				if(this.showAboutInfo)
				{
					DataHolder.GameSettings().PlayAcceptAudio(this.audio);
					this.dialogueMessage = this.dialoguePosition.multiLabel.GetUnshownText();
					if(this.dialogueMessage.Length == 0) this.dialoguePosition.InitOut();
					else this.dialoguePosition.multiLabel.ReShow(this.dialogueMessage);
				}
				else
				{
					if(this.contentChoice[this.dialoguePosition.multiLabel.GetSelection()].active)
					{
						DataHolder.GameSettings().PlayAcceptAudio(this.audio);
						this.dialoguePosition.InitOut();
					}
					else
					{
						DataHolder.GameSettings().PlayFailAudio(this.audio);
					}
				}
			}
			// save handling
			else if(GameHandler.IsControlSave() && 
				!this.dialoguePosition.outDone && !this.dialoguePosition.IsFadingOut() &&
				(this.showSavePoint || this.showSaveMenu || this.showLoadMenu ||
				this.showSaveQuestion || this.showLoadQuestion))
			{
				if(this.contentChoice[this.dialoguePosition.multiLabel.GetSelection()].active)
				{
					DataHolder.GameSettings().PlayAcceptAudio(this.audio);
					this.dialoguePosition.InitOut();
				}
				else
				{
					DataHolder.GameSettings().PlayFailAudio(this.audio);
				}
			}
			// dialogue
			else if(this.showDialogue && !this.dialoguePosition.outDone && !this.dialoguePosition.IsFadingOut())
			{
				if(!this.dialogueWait || this.dialogueTime2 > 0)
				{
					DataHolder.GameSettings().PlayAcceptAudio(this.audio);
				}
				this.dialogueTime2 = this.dialogueTime;
				this.dialogueMessage = this.dialoguePosition.multiLabel.GetUnshownText();
				if(this.dialogueMessage.Length == 0)
				{
					this.dialogueWait = false;
					this.dialoguePosition.InitOut();
				}
				else this.dialoguePosition.multiLabel.ReShow(this.dialogueMessage);
			}
			// choice
			else if(this.showChoice && !this.dialoguePosition.outDone && !this.dialoguePosition.IsFadingOut())
			{
				if(this.contentChoice[this.dialoguePosition.multiLabel.GetSelection()].active)
				{
					DataHolder.GameSettings().PlayAcceptAudio(this.audio);
					this.dialoguePosition.InitOut();
				}
				else
				{
					DataHolder.GameSettings().PlayFailAudio(this.audio);
				}
			}
		}
	}
	
	private void FadeOutChecking()
	{
		// battle gains
		if(this.showBattleGains && this.battleMenuPosition.outDone && 
			this.battleEndMessages[this.currentBattleEndPos].Length == 0)
		{
			this.currentBattleEndPos++;
			if(this.battleEndMessages.Length == this.currentBattleEndPos)
			{
				this.showBattleGains = false;
				this.battleEndMessages = null;
				DataHolder.BattleSystem().GainsAccepted();
			}
			else
			{
				this.ShowBattleEnd(ArrayHelper.Remove(0, this.battleEndMessages), false);
			}
		}
		// battle menu
		else if(GameHandler.IsControlBattle() && 
			DataHolder.BattleSystem().battleRunning && !this.showBattleGains)
		{
			if(this.bmUserIndex < 0) this.bmUserIndex = this.currentBMUser.Length-1;
			else if(this.bmUserIndex >= this.currentBMUser.Length) this.bmUserIndex = 0;
			if(this.currentBMUser.Length > 0 && this.currentBMUser[this.bmUserIndex].IsChoosingAction())
			{
				if(this.currentBMUser[this.bmUserIndex].isDead)
				{
					this.showBattleMenu = false;
					this.battleMenuPosition.SetOutDone();
					this.currentBMUser[this.bmUserIndex].EndBattleMenu(false);
					DataHolder.BattleSystem().AddAction(null);
				}
				else
				{
					if(!this.showBattleMenu)
					{
						this.battleMenuPosition = this.currentBMUser[this.bmUserIndex].battleMenu.GetStoredMenu();
						if(this.battleMenuPosition == null)
						{
							this.battleMenuPosition = DataHolder.DialoguePositions().GetCopy(
									this.currentBMUser[this.bmUserIndex].battleMenu.bmPosition);
						}
						else this.battleMenuPosition.Reset();
						this.battleMenuPosition.multiLabel.SetSelection(0);
						if(this.getLastBMIndex)
						{
							this.battleMenuPosition.multiLabel.SetSelection((int)this.lastBMSelections[0]);
							this.lastBMSelections.RemoveAt(0);
							this.getLastBMIndex = false;
						}
						else if(DataHolder.BattleCam().blockAnimationCams &&
							this.currentBMUser[this.bmUserIndex].prefabInstance != null)
						{
							DataHolder.BattleCam().SetMenuUser(
									this.currentBMUser[this.bmUserIndex].prefabInstance.transform);
						}
						this.showBattleMenu = true;
						if(this.currentBMUser[this.bmUserIndex].battleMenu.IsBaseMenu())
						{
							this.lastBMSelections = new ArrayList();
						}
						else if((DataHolder.BattleMenu().useTargetBlink ||
							DataHolder.BattleMenu().useTargetCursor) &&
							this.currentBMUser[this.bmUserIndex].battleMenu.IsTargetMenu())
						{
							this.currentBMUser[this.bmUserIndex].battleMenu.SetSelectedTarget(this.battleMenuPosition.multiLabel.GetSelection());
						}
						if(!this.currentBMUser[this.bmUserIndex].battleMenu.hide && 
							(!this.currentBMUser[this.bmUserIndex].battleMenu.IsTargetMenu() || 
							DataHolder.BattleMenu().useTargetMenu))
						{
							this.battleMenuPosition.Show(this.currentBMUser[this.bmUserIndex].battleMenu.bmTitle, 
									"", this.currentBMUser[this.bmUserIndex].battleMenu.choices, null, null, false);
						}
						else
						{
							this.battleMenuPosition.multiLabel.maxSelection = 
									this.currentBMUser[this.bmUserIndex].battleMenu.choices.Length;
						}
						this.currentBMUser[this.bmUserIndex].battleMenu.listUpdated = false;
					}
					
					if(this.currentBMUser[this.bmUserIndex].skillBlockChanged)
					{
						this.currentBMUser[this.bmUserIndex].battleMenu.ReloadSkillMenu();
						this.battleMenuPosition.Show(this.currentBMUser[this.bmUserIndex].battleMenu.bmTitle, 
								"", this.currentBMUser[this.bmUserIndex].battleMenu.choices, null, null, false);
					}
					else if(this.currentBMUser[this.bmUserIndex].battleMenu.listUpdated)
					{
						this.currentBMUser[this.bmUserIndex].battleMenu.listUpdated = false;
						this.battleMenuPosition.Show(this.currentBMUser[this.bmUserIndex].battleMenu.bmTitle, 
								"", this.currentBMUser[this.bmUserIndex].battleMenu.choices, null, null, false);
					}
					
					if(this.showBattleMenu && this.battleMenuPosition.outDone)
					{
						this.currentBMUser[this.bmUserIndex].battleMenu.StoreMenu(this.battleMenuPosition);
						this.showBattleMenu = false;
						this.battleMenuPosition.SetOutDone();
						if(this.markBackBM)
						{
							this.markBackBM = false;
							this.BattleMenuBack(false);
						}
						else
						{
							this.currentBMUser[this.bmUserIndex].battleMenu.Select(this.battleMenuPosition.multiLabel.GetSelection());
						}
					}
				}
			}
		}
		// main menu handling
		else if(GameHandler.IsControlMenu() && this.showMainMenu && this.dialoguePosition.outDone)
		{
			if(this.showAboutInfo && this.dialogueMessage.Length == 0)
			{
				this.showAboutInfo = false;
				this.CallMainMenu();
			}
			else if(this.showLanguageMenu)
			{
				this.showLanguageMenu = false;
				if(this.dialoguePosition.multiLabel.GetSelection() < this.contentChoice.Length-1)
				{
					GameHandler.SetLanguage(this.dialoguePosition.multiLabel.GetSelection());
				}
				if(this.showMainMenu)
				{
					this.CallMainMenu();
				}
			}
			else if(this.showDifficultyMenu)
			{
				this.showDifficultyMenu = false;
				if(this.dialoguePosition.multiLabel.GetSelection() < this.contentChoice.Length-1)
				{
					GameHandler.SetDifficulty(this.dialoguePosition.multiLabel.GetSelection());
					if(this.showMainMenu)
					{
						this.showMainMenu = false;
						GameHandler.StartNewGame();
					}
				}
				else if(this.showMainMenu)
				{
					this.CallMainMenu();
				}
			}
			else
			{
				if(this.dialoguePosition.multiLabel.GetSelection() == 0)
				{
					if(DataHolder.MainMenu().showDifficulty)
					{
						this.CallDifficultyMenu();
					}
					else
					{
						this.showMainMenu = false;
						GameHandler.StartNewGame();
					}
				}
				else if(DataHolder.MainMenu().IsChoiceLoad(this.dialoguePosition.multiLabel.GetSelection()))
				{
					GameHandler.SetControlType(ControlType.SAVE);
					this.CallLoadMenu();
				}
				else if(DataHolder.MainMenu().IsChoiceLanguage(this.dialoguePosition.multiLabel.GetSelection()))
				{
					this.CallLanguageMenu();
				}
				else if(DataHolder.MainMenu().IsChoiceAbout(this.dialoguePosition.multiLabel.GetSelection()))
				{
					this.CallAboutInfo();
				}
				else if(DataHolder.MainMenu().IsChoiceExit(this.dialoguePosition.multiLabel.GetSelection()))
				{
					this.showMainMenu = false;
					Application.Quit();
				}
			}
		}
		// save handling
		else if(GameHandler.IsControlSave() && this.dialoguePosition.outDone &&
			(this.showSavePoint || this.showSaveMenu || this.showLoadMenu ||
			this.showSaveQuestion || this.showLoadQuestion))
		{
			if(this.showSavePoint)
			{
				this.showSavePoint = false;
				if(this.dialoguePosition.multiLabel.GetSelection() == 0)
				{
					this.CallSaveMenu();
				}
				else if(this.dialoguePosition.multiLabel.GetSelection() == 1 && this.contentChoice.Length == 3)
				{
					this.CallLoadMenu();
				}
				else if(this.dialoguePosition.multiLabel.GetSelection() == this.contentChoice.Length-1)
				{
					this.SaveCanceled();
				}
			}
			else if(this.showSaveMenu)
			{
				this.showSaveMenu = false;
				if(this.dialoguePosition.multiLabel.GetSelection() == this.contentChoice.Length-1)
				{
					this.SaveCanceled();
				}
				else
				{
					this.selectedSaveFile = this.dialoguePosition.multiLabel.GetSelection();
					this.CallSaveQuestion(this.dialoguePosition.multiLabel.GetSelection());
				}
			}
			else if(this.showSaveQuestion)
			{
				this.showSaveQuestion = false;
				if(this.dialoguePosition.multiLabel.GetSelection() == this.contentChoice.Length-1)
				{
					this.CallSaveMenu();
				}
				else
				{
					this.SaveCanceled();
					SaveHandler.SaveGame(this.selectedSaveFile);
				}
			}
			else if(this.showLoadMenu)
			{
				this.showLoadMenu = false;
				if(this.dialoguePosition.multiLabel.GetSelection() == this.contentChoice.Length-1)
				{
					this.SaveCanceled();
				}
				else
				{
					this.selectedSaveFile = this.dialoguePosition.multiLabel.GetSelection();
					if(SaveHandler.FileExists(-1)) this.selectedSaveFile -= 1;
					this.CallLoadQuestion(this.selectedSaveFile);
				}
			}
			else if(this.showLoadQuestion)
			{
				this.showLoadQuestion = false;
				if(this.dialoguePosition.multiLabel.GetSelection() == this.contentChoice.Length-1)
				{
					this.CallLoadMenu();
				}
				else
				{
					this.showMainMenu = false;
					this.showQuestion = false;
					this.currentQuestion = null;
					this.SaveCanceled();
					SaveHandler.LoadGame(this.selectedSaveFile);
				}
			}
		}
		// dialogue
		else if(this.showDialogue && this.dialoguePosition.outDone && this.dialogueMessage.Length == 0)
		{
			this.showDialogue = false;
			if(this.showItemCollection)
			{
				this.showItemCollection = false;
				this.itemCollector.CollectionFinished(true);
				this.itemCollector = null;
			}
			else
			{
				this.dlgEvent.StepFinished(this.nextStep);
				this.dlgEvent = null;
			}
		}
		// choice
		else if(this.showChoice && this.dialoguePosition.outDone)
		{
			this.showChoice = false;
			if(this.showItemCollection)
			{
				this.showItemCollection = false;
				this.itemCollector.CollectionFinished(this.dialoguePosition.multiLabel.GetSelection() == 0);
				this.itemCollector = null;
			}
			else if(this.showQuestion)
			{
				if(this.currentQuestion.ChoiceSelected(this.dialoguePosition.multiLabel.GetSelection()))
				{
					this.showQuestion = false;
					this.currentQuestion = null;
				}
			}
			else
			{
				this.dlgEvent.ChoiceSelected(this.dialoguePosition.multiLabel.GetSelection());
				this.dlgEvent = null;
			}
		}
	}
}