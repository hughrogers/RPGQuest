
using UnityEngine;
using System.Collections;

public class LevelHandlerGUI : LevelHandler
{
	void OnGUI()
	{
		// exit on game menu conditions
		if(GameHandler.IsGamePaused() && GameHandler.IsControlMenu())
		{
			GameHandler.DragHandler().DrawDrag(this.guiMatrix);
			return;
		}
		
		// set matrix to fit GUI on all resolutions
		this.defaultMatrix = GUI.matrix;
		GUI.matrix = this.guiMatrix;
		if(!this.showMainMenu)
		{
			for(int i=0; i<this.hud.Length; i++)
			{
				this.hud[i].ShowHUD(this.guiMatrix, this.interactionList.Count);
			}
		}
		
		if(this.showInfo && !this.showBattleMessage)
		{
			this.infoPosition.multiLabel.ShowOneLine(this.infoPosition, this.infoMessage);
		}
		
		// battle
		if(this.showBattleGains)
		{
			string hlpText = "";
			if(this.battleMenuPosition.oneline)
			{
				hlpText = this.battleMenuPosition.multiLabel.ShowOneLine(this.battleMenuPosition, this.battleEndMessages[this.currentBattleEndPos]);
			}
			else
			{
				hlpText = this.battleMenuPosition.multiLabel.ShowText(this.battleMenuPosition, this.battleEndMessages[this.currentBattleEndPos], "");
			}
			
			if(eventOkPressed && !this.battleMenuPosition.outDone && !this.battleMenuPosition.IsFadingOut())
			{
				DataHolder.GameSettings().PlayAcceptAudio(this.audio);
				this.battleEndMessages[this.currentBattleEndPos] = hlpText;
				this.battleMenuPosition.multiLabel.newContent = true;
				if(this.battleEndMessages[this.currentBattleEndPos].Length == 0)
				{
					this.battleMenuPosition.InitOut();
				}
			}
			if(this.battleMenuPosition.outDone && this.battleEndMessages[this.currentBattleEndPos].Length == 0)
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
		}
		
		if(this.showBattleMessage)
		{
			GUI.depth = -2000;
			this.battleMessagePosition.multiLabel.ShowOneLine(this.battleMessagePosition, 
					this.battleMessageText, this.battleMessageColor, this.battleMessageSColor);
		}
		else if(GameHandler.IsControlBattle() && 
			DataHolder.BattleSystem().battleRunning && !this.showBattleGains)
		{
			if(!this.showBattleMessage && this.showBattleInfo)
			{
				this.dialoguePosition.multiLabel.ShowOneLine(this.dialoguePosition, this.dialogueMessage);
			}
			
			if(this.bmUserIndex < 0) this.bmUserIndex = this.currentBMUser.Length-1;
			else if(this.bmUserIndex >= this.currentBMUser.Length) this.bmUserIndex = 0;
			if(this.currentBMUser.Length > 0 && this.currentBMUser[this.bmUserIndex].IsChoosingAction())
			{
				if(this.currentBMUser[this.bmUserIndex].isDead)
				{
					this.showBattleMenu = false;
					this.currentBMUser[this.bmUserIndex].EndBattleMenu(false);
					DataHolder.BattleSystem().AddAction(null);
				}
				else
				{
					if(!this.showBattleMenu)
					{
						this.battleMenuPosition = DataHolder.DialoguePositions().GetCopy(
								this.currentBMUser[this.bmUserIndex].battleMenu.bmPosition);
						this.battleMenuPosition.InitIn();
						this.battleMenuPosition.multiLabel.ResetScroll();
						if(this.getLastBMIndex)
						{
							this.battleMenuPosition.multiLabel.selection = (int)this.lastBMSelections[0];
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
							this.currentBMUser[this.bmUserIndex].battleMenu.SetSelectedTarget(
									this.battleMenuPosition.multiLabel.GetSelection());
						}
						this.currentBMUser[this.bmUserIndex].battleMenu.listUpdated = false;
					}
					
					if(!this.currentBMUser[this.bmUserIndex].battleMenu.hide && 
						(!this.currentBMUser[this.bmUserIndex].battleMenu.IsTargetMenu() || 
						DataHolder.BattleMenu().useTargetMenu))
					{
						if(this.currentBMUser[this.bmUserIndex].skillBlockChanged)
						{
							this.currentBMUser[this.bmUserIndex].battleMenu.ReloadSkillMenu();
							this.battleMenuPosition.multiLabel.newContent = true;
						}
						else if(this.currentBMUser[this.bmUserIndex].battleMenu.listUpdated)
						{
							this.currentBMUser[this.bmUserIndex].battleMenu.listUpdated = false;
							this.battleMenuPosition.multiLabel.newContent = true;
						}
						this.battleMenuPosition.multiLabel.ShowChoice(this.battleMenuPosition, "",
								this.currentBMUser[this.bmUserIndex].battleMenu.bmTitle, 
								this.currentBMUser[this.bmUserIndex].battleMenu.choices);
					}
					else
					{
						this.battleMenuPosition.multiLabel.maxSelection = this.currentBMUser[this.bmUserIndex].battleMenu.choices.Length;
					}
					if(eventOkPressed && !this.battleMenuPosition.outDone && !this.battleMenuPosition.IsFadingOut())
					{
						if(this.currentBMUser[this.bmUserIndex].battleMenu.choices.Length > 0 &&
							this.currentBMUser[this.bmUserIndex].battleMenu.choices[this.battleMenuPosition.multiLabel.GetSelection()].active)
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
					if(this.showBattleMenu && this.battleMenuPosition.outDone)
					{
						this.showBattleMenu = false;
						this.battleMenuPosition.outDone = false;
						if(this.markBackBM)
						{
							this.markBackBM = false;
							this.BattleMenuBack(false);
						}
						else if(this.markSwitchBM)
						{
							this.markSwitchBM = false;
							this.currentBMUser[this.bmUserIndex].battleMenu.CallMenu(this.switchModeBM, null);
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
		else if(GameHandler.IsControlMenu() && this.showMainMenu)
		{
			string hlpText = "";
			if(this.showAboutInfo)
			{
				hlpText = this.dialoguePosition.multiLabel.ShowText(this.dialoguePosition, this.dialogueMessage, this.dialogueName);
			}
			else
			{
				this.dialoguePosition.multiLabel.ShowChoice(this.dialoguePosition, this.dialogueMessage, this.dialogueName, this.contentChoice);
			}
			if(eventOkPressed && !this.dialoguePosition.outDone && !this.dialoguePosition.IsFadingOut())
			{
				if(this.showAboutInfo)
				{
					DataHolder.GameSettings().PlayAcceptAudio(this.audio);
					this.dialogueMessage = hlpText;
					this.dialoguePosition.multiLabel.newContent = true;
					if(this.dialogueMessage.Length == 0)
					{
						this.dialoguePosition.InitOut();
					}
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
			if(this.dialoguePosition.outDone)
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
		}
		// save handling
		else if(GameHandler.IsControlSave() &&
			(this.showSavePoint || this.showSaveMenu || this.showLoadMenu ||
			this.showSaveQuestion || this.showLoadQuestion))
		{
			this.dialoguePosition.multiLabel.ShowChoice(this.dialoguePosition, this.dialogueMessage, this.dialogueName, this.contentChoice);
			if(eventOkPressed && !this.dialoguePosition.outDone && !this.dialoguePosition.IsFadingOut())
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
			if(this.dialoguePosition.outDone)
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
		}
		// dialogue
		else if(this.showDialogue)
		{
			string hlpText = "";
			if(this.dialoguePosition.oneline)
			{
				hlpText = this.dialoguePosition.multiLabel.ShowOneLine(this.dialoguePosition, this.dialogueMessage);
			}
			else
			{
				hlpText = this.dialoguePosition.multiLabel.ShowText(this.dialoguePosition, this.dialogueMessage, this.dialogueName, this.speakerPortrait);
			}
			
			if(eventOkPressed && !this.dialoguePosition.outDone && !this.dialoguePosition.IsFadingOut())
			{
				if(!this.dialogueWait || this.dialogueTime2 > 0)
				{
					DataHolder.GameSettings().PlayAcceptAudio(this.audio);
				}
				this.dialogueTime2 = this.dialogueTime;
				this.dialogueMessage = hlpText;
				this.dialoguePosition.multiLabel.newContent = true;
				if(this.dialogueMessage.Length == 0)
				{
					this.dialogueWait = false;
					this.dialoguePosition.InitOut();
				}
			}
			if(this.dialoguePosition.outDone && this.dialogueMessage.Length == 0)
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
		}
		else if(this.showChoice)
		{
			this.dialoguePosition.multiLabel.ShowChoice(this.dialoguePosition, this.dialogueMessage, 
					this.dialogueName, this.contentChoice, this.speakerPortrait, null);
			if(eventOkPressed && !this.dialoguePosition.outDone && !this.dialoguePosition.IsFadingOut())
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
			if(this.dialoguePosition.outDone)
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
		eventOkPressed = false;
		GameHandler.DragHandler().DrawDrag(this.guiMatrix);
	}
}
