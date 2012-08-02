
using System.Collections;
using UnityEditor;
using UnityEngine;

public class GameEventTabs
{
	public static int mWidth = 300;
	public static int mWidth2 = 200;
	
	public static void ShowTab(int index, GameEvent gameEvent, EventStep baseStep)
	{
		if(baseStep is WaitStep) GameEventTabs.WaitStep(index, gameEvent, (WaitStep)baseStep);
		else if(baseStep is GoToStep) GameEventTabs.GoToStep(index, gameEvent, (GoToStep)baseStep);
		else if(baseStep is RandomStep) GameEventTabs.RandomStep(index, gameEvent, (RandomStep)baseStep);
		else if(baseStep is EndEventStep) GameEventTabs.EndEventStep(index, gameEvent, (EndEventStep)baseStep);
		else if(baseStep is LoadSceneStep) GameEventTabs.LoadSceneStep(index, gameEvent, (LoadSceneStep)baseStep);
		else if(baseStep is CheckRandomStep) GameEventTabs.CheckRandomStep(index, gameEvent, (CheckRandomStep)baseStep);
		else if(baseStep is CheckFormulaStep) GameEventTabs.CheckFormulaStep(index, gameEvent, (CheckFormulaStep)baseStep);
		else if(baseStep is WaitForButtonStep) GameEventTabs.WaitForButtonStep(index, gameEvent, (WaitForButtonStep)baseStep);
		else if(baseStep is CheckDifficultyStep) GameEventTabs.CheckDifficultyStep(index, gameEvent, (CheckDifficultyStep)baseStep);
		else if(baseStep is PlayAnimationStep) GameEventTabs.PlayAnimationStep(index, gameEvent, (PlayAnimationStep)baseStep);
		else if(baseStep is StopAnimationStep) GameEventTabs.StopAnimationStep(index, gameEvent, (StopAnimationStep)baseStep);
		else if(baseStep is AddArmorStep) GameEventTabs.AddArmorStep(index, gameEvent, (AddArmorStep)baseStep);
		else if(baseStep is RemoveArmorStep) GameEventTabs.RemoveArmorStep(index, gameEvent, (RemoveArmorStep)baseStep);
		else if(baseStep is CheckArmorStep) GameEventTabs.CheckArmorStep(index, gameEvent, (CheckArmorStep)baseStep);
		else if(baseStep is PlaySoundStep) GameEventTabs.PlaySoundStep(index, gameEvent, (PlaySoundStep)baseStep);
		else if(baseStep is StopSoundStep) GameEventTabs.StopSoundStep(index, gameEvent, (StopSoundStep)baseStep);
		else if(baseStep is PlayMusicStep) GameEventTabs.PlayMusicStep(index, gameEvent, (PlayMusicStep)baseStep);
		else if(baseStep is StoreMusicStep) GameEventTabs.StoreMusicStep(index, gameEvent, (StoreMusicStep)baseStep);
		else if(baseStep is JoinBattlePartyStep) GameEventTabs.JoinBattlePartyStep(index, gameEvent, (JoinBattlePartyStep)baseStep);
		else if(baseStep is LeaveBattlePartyStep) GameEventTabs.LeaveBattlePartyStep(index, gameEvent, (LeaveBattlePartyStep)baseStep);
		else if(baseStep is IsInBattlePartyStep) GameEventTabs.IsInBattlePartyStep(index, gameEvent, (IsInBattlePartyStep)baseStep);
		else if(baseStep is LockBattlePartyMemberStep) GameEventTabs.LockBattlePartyMemberStep(index, gameEvent, (LockBattlePartyMemberStep)baseStep);
		else if(baseStep is UnlockBattlePartyMemberStep) GameEventTabs.UnlockBattlePartyMemberStep(index, gameEvent, (UnlockBattlePartyMemberStep)baseStep);
		else if(baseStep is IsLockedBattlePartyMemberStep) GameEventTabs.IsLockedBattlePartyMemberStep(index, gameEvent, (IsLockedBattlePartyMemberStep)baseStep);
		else if(baseStep is SetCamPosStep) GameEventTabs.SetCamPosStep(index, gameEvent, (SetCamPosStep)baseStep);
		else if(baseStep is FadeCamPosStep) GameEventTabs.FadeCamPosStep(index, gameEvent, (FadeCamPosStep)baseStep);
		else if(baseStep is SetInitialCamPosStep) GameEventTabs.SetInitialCamPosStep(index, gameEvent, (SetInitialCamPosStep)baseStep);
		else if(baseStep is FadeToInitialCamPosStep) GameEventTabs.FadeToInitialCamPosStep(index, gameEvent, (FadeToInitialCamPosStep)baseStep);
		else if(baseStep is ShakeCameraStep) GameEventTabs.ShakeCameraStep(index, gameEvent, (ShakeCameraStep)baseStep);
		else if(baseStep is RotateCamAroundStep) GameEventTabs.RotateCamAroundStep(index, gameEvent, (RotateCamAroundStep)baseStep);
		else if(baseStep is ShowDialogueStep) GameEventTabs.ShowDialogueStep(index, gameEvent, (ShowDialogueStep)baseStep);
		else if(baseStep is ShowChoiceStep) GameEventTabs.ShowChoiceStep(index, gameEvent, (ShowChoiceStep)baseStep);
		else if(baseStep is EquipWeaponStep) GameEventTabs.EquipWeaponStep(index, gameEvent, (EquipWeaponStep)baseStep);
		else if(baseStep is EquipArmorStep) GameEventTabs.EquipArmorStep(index, gameEvent, (EquipArmorStep)baseStep);
		else if(baseStep is UnequipStep) GameEventTabs.UnequipStep(index, gameEvent, (UnequipStep)baseStep);
		else if(baseStep is FadeObjectStep) GameEventTabs.FadeObjectStep(index, gameEvent, (FadeObjectStep)baseStep);
		else if(baseStep is FadeCameraStep) GameEventTabs.FadeCameraStep(index, gameEvent, (FadeCameraStep)baseStep);
		else if(baseStep is SendMessageStep) GameEventTabs.SendMessageStep(index, gameEvent, (SendMessageStep)baseStep);
		else if(baseStep is BroadcastMessageStep) GameEventTabs.BroadcastMessageStep(index, gameEvent, (BroadcastMessageStep)baseStep);
		else if(baseStep is AddComponentStep) GameEventTabs.AddComponentStep(index, gameEvent, (AddComponentStep)baseStep);
		else if(baseStep is RemoveComponentStep) GameEventTabs.RemoveComponentStep(index, gameEvent, (RemoveComponentStep)baseStep);
		else if(baseStep is ActivateObjectStep) GameEventTabs.ActivateObjectStep(index, gameEvent, (ActivateObjectStep)baseStep);
		else if(baseStep is ObjectVisibleStep) GameEventTabs.ObjectVisibleStep(index, gameEvent, (ObjectVisibleStep)baseStep);
		else if(baseStep is ParentObjectStep) GameEventTabs.ParentObjectStep(index, gameEvent, (ParentObjectStep)baseStep);
		else if(baseStep is AddItemStep) GameEventTabs.AddItemStep(index, gameEvent, (AddItemStep)baseStep);
		else if(baseStep is RemoveItemStep) GameEventTabs.RemoveItemStep(index, gameEvent, (RemoveItemStep)baseStep);
		else if(baseStep is CheckItemStep) GameEventTabs.CheckItemStep(index, gameEvent, (CheckItemStep)baseStep);
		else if(baseStep is LearnItemRecipeStep) GameEventTabs.LearnItemRecipeStep(index, gameEvent, (LearnItemRecipeStep)baseStep);
		else if(baseStep is ItemRecipeKnownStep) GameEventTabs.ItemRecipeKnownStep(index, gameEvent, (ItemRecipeKnownStep)baseStep);
		else if(baseStep is LevelUpStep) GameEventTabs.LevelUpStep(index, gameEvent, (LevelUpStep)baseStep);
		else if(baseStep is CheckLevelStep) GameEventTabs.CheckLevelStep(index, gameEvent, (CheckLevelStep)baseStep);
		else if(baseStep is AddMoneyStep) GameEventTabs.AddMoneyStep(index, gameEvent, (AddMoneyStep)baseStep);
		else if(baseStep is RemoveMoneyStep) GameEventTabs.RemoveMoneyStep(index, gameEvent, (RemoveMoneyStep)baseStep);
		else if(baseStep is SetMoneyStep) GameEventTabs.SetMoneyStep(index, gameEvent, (SetMoneyStep)baseStep);
		else if(baseStep is CheckMoneyStep) GameEventTabs.CheckMoneyStep(index, gameEvent, (CheckMoneyStep)baseStep);
		else if(baseStep is SetToPositionStep) GameEventTabs.SetToPositionStep(index, gameEvent, (SetToPositionStep)baseStep);
		else if(baseStep is MoveToWaypointStep) GameEventTabs.MoveToWaypointStep(index, gameEvent, (MoveToWaypointStep)baseStep);
		else if(baseStep is MoveToDirectionStep) GameEventTabs.MoveToDirectionStep(index, gameEvent, (MoveToDirectionStep)baseStep);
		else if(baseStep is MoveToPrefabStep) GameEventTabs.MoveToPrefabStep(index, gameEvent, (MoveToPrefabStep)baseStep);
		else if(baseStep is RotateToWaypointStep) GameEventTabs.RotateToWaypointStep(index, gameEvent, (RotateToWaypointStep)baseStep);
		else if(baseStep is RotationStep) GameEventTabs.RotationStep(index, gameEvent, (RotationStep)baseStep);
		else if(baseStep is JoinPartyStep) GameEventTabs.JoinPartyStep(index, gameEvent, (JoinPartyStep)baseStep);
		else if(baseStep is LeavePartyStep) GameEventTabs.LeavePartyStep(index, gameEvent, (LeavePartyStep)baseStep);
		else if(baseStep is IsInPartyStep) GameEventTabs.IsInPartyStep(index, gameEvent, (IsInPartyStep)baseStep);
		else if(baseStep is HasLeftPartyStep) GameEventTabs.HasLeftPartyStep(index, gameEvent, (HasLeftPartyStep)baseStep);
		else if(baseStep is CheckPlayerStep) GameEventTabs.CheckPlayerStep(index, gameEvent, (CheckPlayerStep)baseStep);
		else if(baseStep is SetPlayerStep) GameEventTabs.SetPlayerStep(index, gameEvent, (SetPlayerStep)baseStep);
		else if(baseStep is LearnSkillStep) GameEventTabs.LearnSkillStep(index, gameEvent, (LearnSkillStep)baseStep);
		else if(baseStep is ForgetSkillStep) GameEventTabs.ForgetSkillStep(index, gameEvent, (ForgetSkillStep)baseStep);
		else if(baseStep is HasSkillStep) GameEventTabs.HasSkillStep(index, gameEvent, (HasSkillStep)baseStep);
		else if(baseStep is SpawnPlayerStep) GameEventTabs.SpawnPlayerStep(index, gameEvent, (SpawnPlayerStep)baseStep);
		else if(baseStep is DestroyPlayerStep) GameEventTabs.DestroyPlayerStep(index, gameEvent, (DestroyPlayerStep)baseStep);
		else if(baseStep is SpawnPrefabStep) GameEventTabs.SpawnPrefabStep(index, gameEvent, (SpawnPrefabStep)baseStep);
		else if(baseStep is DestroyPrefabStep) GameEventTabs.DestroyPrefabStep(index, gameEvent, (DestroyPrefabStep)baseStep);
		else if(baseStep is ChangeStatusEffectStep) GameEventTabs.ChangeStatusEffectStep(index, gameEvent, (ChangeStatusEffectStep)baseStep);
		else if(baseStep is RegenerateStep) GameEventTabs.RegenerateStep(index, gameEvent, (RegenerateStep)baseStep);
		else if(baseStep is CheckStatusValueStep) GameEventTabs.CheckStatusValueStep(index, gameEvent, (CheckStatusValueStep)baseStep);
		else if(baseStep is SetStatusValueStep) GameEventTabs.SetStatusValueStep(index, gameEvent, (SetStatusValueStep)baseStep);
		else if(baseStep is SetVariableStep) GameEventTabs.SetVariableStep(index, gameEvent, (SetVariableStep)baseStep);
		else if(baseStep is RemoveVariableStep) GameEventTabs.RemoveVariableStep(index, gameEvent, (RemoveVariableStep)baseStep);
		else if(baseStep is CheckVariableStep) GameEventTabs.CheckVariableStep(index, gameEvent, (CheckVariableStep)baseStep);
		else if(baseStep is AddWeaponStep) GameEventTabs.AddWeaponStep(index, gameEvent, (AddWeaponStep)baseStep);
		else if(baseStep is RemoveWeaponStep) GameEventTabs.RemoveWeaponStep(index, gameEvent, (RemoveWeaponStep)baseStep);
		else if(baseStep is CheckWeaponStep) GameEventTabs.CheckWeaponStep(index, gameEvent, (CheckWeaponStep)baseStep);
		else if(baseStep is SetNumberVariableStep) GameEventTabs.SetNumberVariableStep(index, gameEvent, (SetNumberVariableStep)baseStep);
		else if(baseStep is RemoveNumberVariableStep) GameEventTabs.RemoveNumberVariableStep(index, gameEvent, (RemoveNumberVariableStep)baseStep);
		else if(baseStep is CheckNumberVariableStep) GameEventTabs.CheckNumberVariableStep(index, gameEvent, (CheckNumberVariableStep)baseStep);
		else if(baseStep is CheckClassStep) GameEventTabs.CheckClassStep(index, gameEvent, (CheckClassStep)baseStep);
		else if(baseStep is ChangeClassStep) GameEventTabs.ChangeClassStep(index, gameEvent, (ChangeClassStep)baseStep);
		else if(baseStep is SetPlayerPrefsStep) GameEventTabs.SetPlayerPrefsStep(index, gameEvent, (SetPlayerPrefsStep)baseStep);
		else if(baseStep is GetPlayerPrefsStep) GameEventTabs.GetPlayerPrefsStep(index, gameEvent, (GetPlayerPrefsStep)baseStep);
		else if(baseStep is HasPlayerPrefsStep) GameEventTabs.HasPlayerPrefsStep(index, gameEvent, (HasPlayerPrefsStep)baseStep);
		else if(baseStep is CallGlobalEventStep) GameEventTabs.CallGlobalEventStep(index, gameEvent, (CallGlobalEventStep)baseStep);
		else if(baseStep is TeleportStep) GameEventTabs.TeleportStep(index, gameEvent, (TeleportStep)baseStep);
		else if(baseStep is TeleportChoiceStep) GameEventTabs.TeleportChoiceStep(index, gameEvent, (TeleportChoiceStep)baseStep);
		else if(baseStep is SetCharacterNameStep) GameEventTabs.SetCharacterNameStep(index, gameEvent, (SetCharacterNameStep)baseStep);
		else if(baseStep is CustomStatisticStep) GameEventTabs.CustomStatisticStep(index, gameEvent, (CustomStatisticStep)baseStep);
		else if(baseStep is ClearStatisticStep) GameEventTabs.ClearStatisticStep(index, gameEvent, (ClearStatisticStep)baseStep);
		// always last
		else if(baseStep is EventStep) GameEventTabs.EventStep(index, gameEvent, (EventStep)baseStep);
	}
	
	// gameEvent actor
	public static void EventActor(int index, GameEvent gameEvent, EventActor actor)
	{
		actor.fold = EditorGUILayout.Foldout(actor.fold, "Actor "+index);
		if(actor.fold)
		{
			if(GUILayout.Button("Remove", GUILayout.Width(100)))
			{
				gameEvent.RemoveActor(index);
				return;
			}
			EditorGUILayout.Separator();
			actor.isPlayer = EditorGUILayout.Toggle("Player", actor.isPlayer, GUILayout.Width(200));
			if(!actor.isPlayer)
			{
				
			}
			actor.showName = EditorGUILayout.BeginToggleGroup("Show name in dialog", actor.showName);
			if(actor.isPlayer) actor.overrideName = EditorGUILayout.Toggle("Override name", actor.overrideName, GUILayout.Width(200));
			if(!actor.isPlayer || (actor.isPlayer && actor.overrideName))
			{
				if(actor.dialogName.Length != DataHolder.Languages().GetDataCount())
				{
					var tmp = actor.dialogName;
					actor.dialogName = new string[DataHolder.Languages().GetDataCount()];
					for(int i=0; i<actor.dialogName.Length; i++)
					{
						if(i < tmp.Length && tmp[i] != null)
						{
							actor.dialogName[i] = tmp[i];
						}
						else
						{
							actor.dialogName[i] = "";
						}
					}
				}
				
				for(int i=0; i<actor.dialogName.Length; i++)
				{
					actor.dialogName[i] = EditorGUILayout.TextField(DataHolder.Language(i), actor.dialogName[i], GUILayout.Width(400));
				}
			}
			EditorGUILayout.EndToggleGroup();
			EditorGUILayout.Separator();
		}
	}

	// baseStep steps
	public static void EventStep(int index, GameEvent gameEvent, EventStep baseStep)
	{
		if(!gameEvent.hideButtons)
		{
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Remove", GUILayout.Width(100)))
			{
				gameEvent.RemoveStep(index);
				return;
			}
			if(index > 0)
			{
				if(GUILayout.Button("Move Up", GUILayout.Width(100)))
				{
					gameEvent.MoveStepUp(index);
					return;
				}
			}
			if(index < gameEvent.step.Length-1)
			{
				if(GUILayout.Button("Move Down", GUILayout.Width(100)))
				{
					gameEvent.MoveStepDown(index);
					return;
				}
			}
			baseStep.stepEnabled = EditorGUILayout.Toggle("Step enabled", baseStep.stepEnabled, GUILayout.Width(200));
			GUILayout.FlexibleSpace();
			if(GUILayout.Button("Copy", GUILayout.Width(100)))
			{
				gameEvent.InsertStep(baseStep.GetCopy(gameEvent), index+1);
				return;
			}
			if(GUILayout.Button("Move To", GUILayout.Width(100)))
			{
				gameEvent.MoveStepTo(baseStep.moveTo, index);
			}
			baseStep.moveTo = EditorGUILayout.IntField(baseStep.moveTo, GUILayout.Width(50));
			if(baseStep.moveTo < 0) baseStep.moveTo = 0;
			else if(baseStep.moveTo >= gameEvent.step.Length) baseStep.moveTo = gameEvent.step.Length-1;
			EditorGUILayout.EndHorizontal();
		}
		if(baseStep is RandomStep || baseStep is EndEventStep || 
			baseStep is ShowChoiceStep || baseStep is TeleportStep ||
			baseStep is TeleportChoiceStep) {}
		else
		{
			baseStep.next = EditorGUILayout.IntField("Next step", baseStep.next, GUILayout.Width(200));
		}
		EditorGUILayout.Separator();
	}
	
	public static void WaitStep(int index, GameEvent gameEvent, WaitStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Wait");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.time = EditorGUILayout.FloatField("Time (s)", baseStep.time, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void GoToStep(int index, GameEvent gameEvent, GoToStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Go To Step");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			EditorGUILayout.Separator();
		}
	}
	
	public static void RandomStep(int index, GameEvent gameEvent, RandomStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Random Step");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.min = EditorGUILayout.IntField("Minimum", baseStep.min, GUILayout.Width(mWidth));
			baseStep.max = EditorGUILayout.IntField("Maximum", baseStep.max, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void EndEventStep(int index, GameEvent gameEvent, EndEventStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": End Event");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			EditorGUILayout.Separator();
		}
	}
	
	public static void LoadSceneStep(int index, GameEvent gameEvent, LoadSceneStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Load Scene");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.scene = EditorGUILayout.TextField("Scene", baseStep.scene, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void CheckRandomStep(int index, GameEvent gameEvent, CheckRandomStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Check Random");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.float1 = EditorGUILayout.FloatField("Value (0-100)", baseStep.float1, GUILayout.Width(mWidth));
			if(baseStep.float1 < 0) baseStep.float1 = 0;
			else if(baseStep.float1 > 100) baseStep.float1 = 100;
			baseStep.nextFail = EditorGUILayout.IntField("Next step fail", baseStep.nextFail, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void CheckFormulaStep(int index, GameEvent gameEvent, CheckFormulaStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Check Formula");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.formulaID = EditorGUILayout.Popup("Formula", baseStep.formulaID, DataHolder.Formulas().GetNameList(true),GUILayout.Width(mWidth));
			baseStep.characterID = EditorGUILayout.Popup("Character", baseStep.characterID, DataHolder.Characters().GetNameList(true),GUILayout.Width(mWidth));
			baseStep.nextFail = EditorGUILayout.IntField("Next step fail", baseStep.nextFail, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void WaitForButtonStep(int index, GameEvent gameEvent, WaitForButtonStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Wait for Button");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.time = EditorGUILayout.FloatField("Time (s)", baseStep.time, GUILayout.Width(mWidth));
			baseStep.key = EditorGUILayout.TextField("Button name", baseStep.key, GUILayout.Width(mWidth));
			baseStep.nextFail = EditorGUILayout.IntField("Next step fail", baseStep.nextFail, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void CheckDifficultyStep(int index, GameEvent gameEvent, CheckDifficultyStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Check Difficulty");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.number = EditorGUILayout.Popup("Difficulty", baseStep.number, 
					DataHolder.Difficulties().GetNameList(true), GUILayout.Width(mWidth));
			baseStep.valueCheck = (ValueCheck)EditorGUILayout.EnumPopup("Check", 
					baseStep.valueCheck, GUILayout.Width(mWidth));
			baseStep.nextFail = EditorGUILayout.IntField("Next step fail", 
					baseStep.nextFail, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	// animation steps
	public static void PlayAnimationStep(int index, GameEvent gameEvent, PlayAnimationStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Play Animation");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.show3 = EditorGUILayout.Toggle("Animate prefab", baseStep.show3, GUILayout.Width(mWidth));
			if(baseStep.show3)
			{
				baseStep.actorID = EditorGUILayout.IntField("Prefab ID", baseStep.actorID, GUILayout.Width(mWidth));
			}
			else
			{
				baseStep.actorID = EditorGUILayout.Popup("Actor", baseStep.actorID, gameEvent.GetActorList(), GUILayout.Width(mWidth));
			}
			
			EditorGUILayout.BeginHorizontal();
			baseStep.value = EditorGUILayout.TextField("Animation name", baseStep.value, GUILayout.Width(mWidth));
			baseStep.wait = EditorGUILayout.Toggle("Wait", baseStep.wait, GUILayout.Width(mWidth));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			baseStep.number = EditorGUILayout.Popup("Play type", baseStep.number, baseStep.playOptions, GUILayout.Width(mWidth));
			// play mode
			if(baseStep.playOptions[baseStep.number] == "Play" || baseStep.playOptions[baseStep.number] == "CrossFade" ||
					baseStep.playOptions[baseStep.number] == "PlayQueued" || baseStep.playOptions[baseStep.number] == "CrossFadeQueued")
			{
				baseStep.playMode = (PlayMode)EditorGUILayout.EnumPopup("Play mode", baseStep.playMode, GUILayout.Width(mWidth));
			}
			// fade Length
			if(baseStep.playOptions[baseStep.number] == "CrossFade" || baseStep.playOptions[baseStep.number] == "Blend" ||
					baseStep.playOptions[baseStep.number] == "CrossFadeQueued")
			{
				baseStep.time = EditorGUILayout.FloatField("Fade Length", baseStep.time, GUILayout.Width(mWidth));
			}
			// target weight
			if(baseStep.playOptions[baseStep.number] == "Blend")
			{
				baseStep.speed = EditorGUILayout.FloatField("Target weight", baseStep.speed, GUILayout.Width(mWidth));
			}
			// queue mode
			if(baseStep.playOptions[baseStep.number] == "PlayQueued" || baseStep.playOptions[baseStep.number] == "CrossFadeQueued")
			{
				baseStep.queueMode = (QueueMode)EditorGUILayout.EnumPopup("Queue mode", baseStep.queueMode, GUILayout.Width(mWidth));
			}
			EditorGUILayout.Separator();
		}
	}
	
	public static void StopAnimationStep(int index, GameEvent gameEvent, StopAnimationStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Stop Animation");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.show3 = EditorGUILayout.Toggle("Animate prefab", baseStep.show3, GUILayout.Width(mWidth));
			if(baseStep.show3)
			{
				baseStep.actorID = EditorGUILayout.IntField("Prefab ID", baseStep.actorID, GUILayout.Width(mWidth));
			}
			else
			{
				baseStep.actorID = EditorGUILayout.Popup("Actor", baseStep.actorID, gameEvent.GetActorList(), GUILayout.Width(mWidth));
			}
			baseStep.show = EditorGUILayout.Toggle("Stop all", baseStep.show, GUILayout.Width(mWidth));
			if(!baseStep.show)
			{
				baseStep.value = EditorGUILayout.TextField("Animation name", baseStep.value, GUILayout.Width(mWidth));
			}
			EditorGUILayout.Separator();
		}
	}
	
	// armor steps
	public static void AddArmorStep(int index, GameEvent gameEvent, AddArmorStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Add Armor");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.armorID = EditorGUILayout.Popup("Armor", baseStep.armorID, DataHolder.Armors().GetNameList(true), GUILayout.Width(mWidth));
			baseStep.number = EditorGUILayout.IntField("Number", baseStep.number, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void RemoveArmorStep(int index, GameEvent gameEvent, RemoveArmorStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Remove Armor");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.armorID = EditorGUILayout.Popup("Armor", baseStep.armorID, DataHolder.Armors().GetNameList(true), GUILayout.Width(mWidth));
			baseStep.number = EditorGUILayout.IntField("Number", baseStep.number, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void CheckArmorStep(int index, GameEvent gameEvent, CheckArmorStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Check Armor");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.armorID = EditorGUILayout.Popup("Armor", baseStep.armorID, DataHolder.Armors().GetNameList(true), GUILayout.Width(mWidth));
			baseStep.number = EditorGUILayout.IntField("Number", baseStep.number, GUILayout.Width(mWidth));
			baseStep.nextFail = EditorGUILayout.IntField("Next step fail", baseStep.nextFail, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	// audio steps
	public static void PlaySoundStep(int index, GameEvent gameEvent, PlaySoundStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Play Sound");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.wait = EditorGUILayout.Toggle("Wait", baseStep.wait, GUILayout.Width(mWidth));
			
			EditorGUILayout.BeginHorizontal();
			baseStep.audioID = EditorGUILayout.Popup("Audio Clip", baseStep.audioID, gameEvent.GetAudioClipList(), GUILayout.Width(mWidth));
			baseStep.show = EditorGUILayout.Toggle("PlayOneShot", baseStep.show, GUILayout.Width(mWidth));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			baseStep.volume = EditorGUILayout.FloatField("Volume", baseStep.volume, GUILayout.Width(mWidth));
			
			EditorGUILayout.BeginHorizontal();
			baseStep.float1 = EditorGUILayout.FloatField("Min. distance", baseStep.float1, GUILayout.Width(mWidth2));
			baseStep.float2 = EditorGUILayout.FloatField("Max. distance", baseStep.float2, GUILayout.Width(mWidth2));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			baseStep.audioRolloffMode = (AudioRolloffMode)EditorGUILayout.EnumPopup("Rolloff mode", baseStep.audioRolloffMode, GUILayout.Width(mWidth));
			baseStep.speed = EditorGUILayout.FloatField("Pitch", baseStep.speed, GUILayout.Width(mWidth));
			
			baseStep.show3 = EditorGUILayout.Toggle("Loop", baseStep.show3, GUILayout.Width(mWidth));
			
			EditorGUILayout.Separator();
			baseStep.show2 = EditorGUILayout.Toggle("On waypoint", baseStep.show2, GUILayout.Width(mWidth));
			if(baseStep.show2)
			{
				baseStep.actorID = EditorGUILayout.Popup("Waypoint", baseStep.actorID, gameEvent.GetWaypointList(), GUILayout.Width(mWidth));
			}
			else
			{
				baseStep.actorID = EditorGUILayout.Popup("Actor", baseStep.actorID, gameEvent.GetActorList(), GUILayout.Width(mWidth));
			}
			EditorGUILayout.Separator();
		}
	}
	
	public static void StopSoundStep(int index, GameEvent gameEvent, StopSoundStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Stop Sound");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.show = EditorGUILayout.Toggle("Pause", baseStep.show, GUILayout.Width(mWidth));
			baseStep.show2 = EditorGUILayout.Toggle("On waypoint", baseStep.show2, GUILayout.Width(mWidth));
			if(baseStep.show2)
			{
				baseStep.actorID = EditorGUILayout.Popup("Waypoint", baseStep.actorID, gameEvent.GetWaypointList(), GUILayout.Width(mWidth));
			}
			else
			{
				baseStep.actorID = EditorGUILayout.Popup("Actor", baseStep.actorID, gameEvent.GetActorList(), GUILayout.Width(mWidth));
			}
			EditorGUILayout.Separator();
		}
	}
	
	public static void PlayMusicStep(int index, GameEvent gameEvent, PlayMusicStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Play Music");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.show = EditorGUILayout.Toggle("Play stored", baseStep.show, GUILayout.Width(mWidth));
			
			baseStep.playType = (MusicPlayType)EditorGUILayout.EnumPopup("Play type", baseStep.playType, GUILayout.Width(mWidth));
			if(baseStep.show && (MusicPlayType.STOP.Equals(baseStep.playType) || MusicPlayType.FADE_OUT.Equals(baseStep.playType)))
			{
				baseStep.playType = MusicPlayType.PLAY;
			}
			if(!baseStep.playType.Equals(MusicPlayType.FADE_OUT) && !baseStep.playType.Equals(MusicPlayType.STOP) && !baseStep.show)
			{
				baseStep.musicID = EditorGUILayout.Popup("Music", baseStep.musicID, DataHolder.Music().GetNameList(true), GUILayout.Width(mWidth));
			}
			if(!baseStep.playType.Equals(MusicPlayType.PLAY) && !baseStep.playType.Equals(MusicPlayType.STOP))
			{
				baseStep.float1 = EditorGUILayout.FloatField("Fade time (s)", baseStep.float1, GUILayout.Width(mWidth));
				baseStep.interpolate = (EaseType)EditorGUILayout.EnumPopup("Interpolation", baseStep.interpolate, GUILayout.Width(mWidth));
			}
			EditorGUILayout.Separator();
		}
	}
	
	public static void StoreMusicStep(int index, GameEvent gameEvent, StoreMusicStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Store Current Music");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			EditorGUILayout.Separator();
		}
	}
	
	// battle party steps
	public static void JoinBattlePartyStep(int index, GameEvent gameEvent, JoinBattlePartyStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Join Battle Party");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.characterID = EditorGUILayout.Popup("Character", baseStep.characterID, DataHolder.Characters().GetNameList(true), GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void LeaveBattlePartyStep(int index, GameEvent gameEvent, LeaveBattlePartyStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Leave Battle Party");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.characterID = EditorGUILayout.Popup("Character", baseStep.characterID, DataHolder.Characters().GetNameList(true), GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void IsInBattlePartyStep(int index, GameEvent gameEvent, IsInBattlePartyStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Is In Battle Party");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.characterID = EditorGUILayout.Popup("Character", baseStep.characterID, DataHolder.Characters().GetNameList(true), GUILayout.Width(mWidth));
			baseStep.nextFail = EditorGUILayout.IntField("Next step fail", baseStep.nextFail, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void LockBattlePartyMemberStep(int index, GameEvent gameEvent, LockBattlePartyMemberStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Lock Battle Party Member");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.characterID = EditorGUILayout.Popup("Character", baseStep.characterID, DataHolder.Characters().GetNameList(true), GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void UnlockBattlePartyMemberStep(int index, GameEvent gameEvent, UnlockBattlePartyMemberStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Unlock Battle Party Member");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.characterID = EditorGUILayout.Popup("Character", baseStep.characterID, DataHolder.Characters().GetNameList(true), GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void IsLockedBattlePartyMemberStep(int index, GameEvent gameEvent, IsLockedBattlePartyMemberStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Is Locked Battle Party Member");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.characterID = EditorGUILayout.Popup("Character", baseStep.characterID, DataHolder.Characters().GetNameList(true), GUILayout.Width(mWidth));
			baseStep.nextFail = EditorGUILayout.IntField("Next step fail", baseStep.nextFail, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	// camera steps
	public static void SetCamPosStep(int index, GameEvent gameEvent, SetCamPosStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Set Camera Position");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.show3 = EditorGUILayout.Toggle("On prefab", baseStep.show3, GUILayout.Width(mWidth));
			if(baseStep.show3)
			{
				baseStep.actorID = EditorGUILayout.IntField("Prefab ID", baseStep.actorID, GUILayout.Width(mWidth));
			}
			else
			{
				baseStep.actorID = EditorGUILayout.Popup("Actor", baseStep.actorID, gameEvent.GetActorList(), GUILayout.Width(mWidth));
			}
			baseStep.show4 = EditorGUILayout.Toggle("Use actor", baseStep.show4, GUILayout.Width(mWidth));
			if(baseStep.show4)
			{
				baseStep.show5 = EditorGUILayout.Toggle("Position", baseStep.show5, GUILayout.Width(mWidth));
				baseStep.show6 = EditorGUILayout.Toggle("Rotation", baseStep.show6, GUILayout.Width(mWidth));
				EditorGUILayout.BeginHorizontal();
				baseStep.show7 = EditorGUILayout.Toggle("Field of view", baseStep.show7, GUILayout.Width(mWidth));
				if(baseStep.show7)
				{
					baseStep.float1 = EditorGUILayout.FloatField(baseStep.float1, GUILayout.Width(mWidth));
				}
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
			}
			else
			{
				baseStep.posID = EditorGUILayout.Popup("Camera position", baseStep.posID, DataHolder.CameraPositions().GetNameList(true), GUILayout.Width(mWidth));
			}
			EditorGUILayout.Separator();
		}
	}
	
	public static void FadeCamPosStep(int index, GameEvent gameEvent, FadeCamPosStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Fade Camera Position");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			EditorGUILayout.BeginHorizontal();
			baseStep.time = EditorGUILayout.FloatField("Time (s)", baseStep.time, GUILayout.Width(mWidth));
			if(baseStep.time <= 0) baseStep.time = 0.1f;
			baseStep.wait = EditorGUILayout.Toggle("Wait", baseStep.wait, GUILayout.Width(mWidth));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			baseStep.show3 = EditorGUILayout.Toggle("On prefab", baseStep.show3, GUILayout.Width(mWidth));
			if(baseStep.show3)
			{
				baseStep.actorID = EditorGUILayout.IntField("Prefab ID", baseStep.actorID, GUILayout.Width(mWidth));
			}
			else
			{
				baseStep.actorID = EditorGUILayout.Popup("Actor", baseStep.actorID, gameEvent.GetActorList(), GUILayout.Width(mWidth));
			}
			baseStep.show4 = EditorGUILayout.Toggle("Use actor", baseStep.show4, GUILayout.Width(mWidth));
			if(baseStep.show4)
			{
				baseStep.show5 = EditorGUILayout.Toggle("Position", baseStep.show5, GUILayout.Width(mWidth));
				baseStep.show6 = EditorGUILayout.Toggle("Rotation", baseStep.show6, GUILayout.Width(mWidth));
				EditorGUILayout.BeginHorizontal();
				baseStep.show7 = EditorGUILayout.Toggle("Field of view", baseStep.show7, GUILayout.Width(mWidth));
				if(baseStep.show7)
				{
					baseStep.float1 = EditorGUILayout.FloatField(baseStep.float1, GUILayout.Width(mWidth));
				}
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
			}
			else
			{
				baseStep.posID = EditorGUILayout.Popup("Camera position", baseStep.posID, DataHolder.CameraPositions().GetNameList(true), GUILayout.Width(mWidth));
			}
			baseStep.interpolate = (EaseType)EditorGUILayout.EnumPopup("Interpolation", baseStep.interpolate, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void SetInitialCamPosStep(int index, GameEvent gameEvent, SetInitialCamPosStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Set Initial Camera Position");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			EditorGUILayout.Separator();
		}
	}
	
	public static void FadeToInitialCamPosStep(int index, GameEvent gameEvent, FadeToInitialCamPosStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Fade To Initial Camera Position");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			EditorGUILayout.BeginHorizontal();
			baseStep.time = EditorGUILayout.FloatField("Time (s)", baseStep.time, GUILayout.Width(mWidth));
			if(baseStep.time <= 0) baseStep.time = 0.1f;
			baseStep.wait = EditorGUILayout.Toggle("Wait", baseStep.wait, GUILayout.Width(mWidth));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			baseStep.interpolate = (EaseType)EditorGUILayout.EnumPopup("Interpolation", baseStep.interpolate, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void ShakeCameraStep(int index, GameEvent gameEvent, ShakeCameraStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Shake Camera");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			EditorGUILayout.BeginHorizontal();
			baseStep.time = EditorGUILayout.FloatField("Time (s)", baseStep.time, GUILayout.Width(mWidth));
			if(baseStep.time <= 0) baseStep.time = 0.1f;
			baseStep.wait = EditorGUILayout.Toggle("Wait", baseStep.wait, GUILayout.Width(mWidth));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			baseStep.intensity = EditorGUILayout.FloatField("Intensity (0-1)", baseStep.intensity, GUILayout.Width(mWidth));
			if(baseStep.intensity < 0) baseStep.intensity = 0;
			else if(baseStep.intensity > 1) baseStep.intensity = 1;
			baseStep.speed = EditorGUILayout.FloatField("Shake speed", baseStep.speed, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void RotateCamAroundStep(int index, GameEvent gameEvent, RotateCamAroundStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Rotate Camera Around");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.show3 = EditorGUILayout.Toggle("On prefab", baseStep.show3, GUILayout.Width(mWidth));
			if(baseStep.show3)
			{
				baseStep.actorID = EditorGUILayout.IntField("Prefab ID", baseStep.actorID, GUILayout.Width(mWidth));
			}
			else
			{
				baseStep.actorID = EditorGUILayout.Popup("Actor", baseStep.actorID, gameEvent.GetActorList(), GUILayout.Width(mWidth));
			}
			EditorGUILayout.BeginHorizontal();
			baseStep.time = EditorGUILayout.FloatField("Time (s)", baseStep.time, GUILayout.Width(mWidth));
			if(baseStep.time <= 0) baseStep.time = 0.1f;
			baseStep.wait = EditorGUILayout.Toggle("Wait", baseStep.wait, GUILayout.Width(mWidth));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			baseStep.speed = EditorGUILayout.FloatField("Speed", baseStep.speed, GUILayout.Width(mWidth));
			baseStep.v3 = EditorGUILayout.Vector3Field("Angles (0-1)", baseStep.v3, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	// dialogue steps
	public static void ShowDialogueStep(int index, GameEvent gameEvent, ShowDialogueStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Show Dialogue");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			baseStep.number = EditorGUILayout.Popup("Dialogue position", baseStep.number, DataHolder.DialoguePositions().GetNameList(true), GUILayout.Width(mWidth));
			EditorGUILayout.BeginHorizontal();
			baseStep.show3 = EditorGUILayout.Toggle("Show speaker", baseStep.show3, GUILayout.Width(mWidth2));
			if(baseStep.show3)
			{
				baseStep.actorID = EditorGUILayout.Popup(baseStep.actorID, gameEvent.GetActorList(), GUILayout.Width(mWidth2));
			}
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			baseStep.show = EditorGUILayout.Toggle("Close after (s)", baseStep.show, GUILayout.Width(mWidth2));
			if(baseStep.show)
			{
				baseStep.time = EditorGUILayout.FloatField(baseStep.time, GUILayout.Width(mWidth2));
				if(baseStep.time <= 0) baseStep.time = 1;
				EditorGUILayout.EndHorizontal();
				baseStep.show2 = EditorGUILayout.Toggle("Block accept", baseStep.show2, GUILayout.Width(mWidth));
			}
			else EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical();
			baseStep.show4 = EditorGUILayout.Toggle("Show portrait", baseStep.show4, GUILayout.Width(mWidth));
			if(baseStep.show4)
			{
				baseStep.show5 = EditorGUILayout.Toggle("In box", baseStep.show5, GUILayout.Width(mWidth));
				baseStep.v2 = EditorGUILayout.Vector2Field("Position", baseStep.v2);
				EditorGUILayout.EndVertical();
				
				EditorGUILayout.BeginVertical();
				if(baseStep.image == null && baseStep.scene != null && "" != baseStep.scene)
				{
					baseStep.image = (Texture2D)Resources.Load(SpeakerPortrait.GetIconPath()+baseStep.scene, typeof(Texture2D));
				}
				baseStep.image = (Texture2D)EditorGUILayout.ObjectField("Image", baseStep.image, typeof(Texture2D), false);
				if(baseStep.image) baseStep.scene = baseStep.image.name;
				else baseStep.scene = "";
			}
			EditorGUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			for(int i=0; i<baseStep.message.Length; i++)
			{
				EditorGUILayout.Separator();
				GUILayout.Label(DataHolder.Language(i), EditorStyles.boldLabel);
				baseStep.message[i] = EditorGUILayout.TextArea(baseStep.message[i], GUILayout.Height(mWidth2));
			}
			EditorGUILayout.Separator();
		}
	}
	
	public static void ShowChoiceStep(int index, GameEvent gameEvent, ShowChoiceStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Show Choice");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			baseStep.number = EditorGUILayout.Popup("Dialogue position", baseStep.number, DataHolder.DialoguePositions().GetNameList(true), GUILayout.Width(mWidth));
			
			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();
			baseStep.show3 = EditorGUILayout.Toggle("Show speaker", baseStep.show3, GUILayout.Width(mWidth2));
			if(baseStep.show3)
			{
				baseStep.actorID = EditorGUILayout.Popup(baseStep.actorID, gameEvent.GetActorList(), GUILayout.Width(mWidth2));
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical();
			baseStep.show4 = EditorGUILayout.Toggle("Show portrait", baseStep.show4, GUILayout.Width(mWidth));
			if(baseStep.show4)
			{
				baseStep.show5 = EditorGUILayout.Toggle("In box", baseStep.show5, GUILayout.Width(mWidth));
				baseStep.v2 = EditorGUILayout.Vector2Field("Position", baseStep.v2);
				EditorGUILayout.EndVertical();
				
				EditorGUILayout.BeginVertical();
				if(baseStep.image == null && baseStep.scene != null && "" != baseStep.scene)
				{
					baseStep.image = (Texture2D)Resources.Load(SpeakerPortrait.GetIconPath()+baseStep.scene, typeof(Texture2D));
				}
				baseStep.image = (Texture2D)EditorGUILayout.ObjectField("Image", baseStep.image, typeof(Texture2D), false);
				if(baseStep.image) baseStep.scene = baseStep.image.name;
				else baseStep.scene = "";
			}
			EditorGUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Separator();
			GUILayout.Label("Message", EditorStyles.boldLabel);
			for(int i=0; i<baseStep.message.Length; i++)
			{
				baseStep.message[i] = EditorGUILayout.TextField(DataHolder.Language(i), baseStep.message[i]);
			}
			EditorGUILayout.Separator();
			
			// choice settings
			if(GUILayout.Button("Add choice", GUILayout.Width(mWidth2)))
			{
				baseStep.AddChoice();
				return;
			}
			for(int i=0; i<baseStep.choiceNext.Length; i++)
			{
				EditorGUILayout.BeginVertical("box");
				GUILayout.Label("Choice "+i, EditorStyles.boldLabel);
				if(baseStep.addItem[i])
				{
					GUILayout.Label("%n for item name, % for quantity");
				}
				for(int j=0; j<((string[])baseStep.choice[i]).Length; j++)
				{
					((string[])baseStep.choice[i])[j] = EditorGUILayout.TextField(DataHolder.Language(j), ((string[])baseStep.choice[i])[j]);
				}
				EditorGUILayout.BeginHorizontal();
				baseStep.choiceNext[i] = EditorGUILayout.IntField("Next step", baseStep.choiceNext[i], GUILayout.Width(mWidth2));
				if(baseStep.choiceNext.Length > 1)
				{
					if(GUILayout.Button("Remove", GUILayout.Width(mWidth2)))
					{
						baseStep.RemoveChoice(i);
						return;
					}
				}
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				
				baseStep.addItem[i] = EditorGUILayout.Toggle("Add item", 
						baseStep.addItem[i], GUILayout.Width(mWidth));
				if(baseStep.addItem[i])
				{
					EditorGUILayout.BeginHorizontal();
					EditorHelper.ItemTypeQuantitySelection(ref baseStep.itemChoiceType[i],
							ref baseStep.itemChoice[i], ref baseStep.itemChoiceQuantity[i]);
					GUILayout.FlexibleSpace();
					EditorGUILayout.EndHorizontal();
				}
				
				
				baseStep.addVariableCondition[i] = EditorGUILayout.Toggle("Add condition", 
						baseStep.addVariableCondition[i], GUILayout.Width(mWidth));
				if(baseStep.addVariableCondition[i])
				{
					baseStep.variableCondition[i] = EditorHelper.VariableConditionSettings(baseStep.variableCondition[i]);
				}
				EditorGUILayout.Separator();
				EditorGUILayout.EndVertical();
			}
			EditorGUILayout.Separator();
		}
	}
	
	public static void TeleportChoiceStep(int index, GameEvent gameEvent, TeleportChoiceStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Teleport Choice");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			baseStep.number = EditorGUILayout.Popup("Dialogue position", baseStep.number, 
					DataHolder.DialoguePositions().GetNameList(true), GUILayout.Width(mWidth));
			
			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();
			baseStep.show3 = EditorGUILayout.Toggle("Show speaker", baseStep.show3, GUILayout.Width(mWidth2));
			if(baseStep.show3)
			{
				baseStep.actorID = EditorGUILayout.Popup(baseStep.actorID, gameEvent.GetActorList(), GUILayout.Width(mWidth2));
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical();
			baseStep.show4 = EditorGUILayout.Toggle("Show portrait", baseStep.show4, GUILayout.Width(mWidth));
			if(baseStep.show4)
			{
				baseStep.show5 = EditorGUILayout.Toggle("In box", baseStep.show5, GUILayout.Width(mWidth));
				baseStep.v2 = EditorGUILayout.Vector2Field("Position", baseStep.v2);
				EditorGUILayout.EndVertical();
				
				EditorGUILayout.BeginVertical();
				if(baseStep.image == null && baseStep.scene != null && "" != baseStep.scene)
				{
					baseStep.image = (Texture2D)Resources.Load(SpeakerPortrait.GetIconPath()+baseStep.scene, typeof(Texture2D));
				}
				baseStep.image = (Texture2D)EditorGUILayout.ObjectField("Image", baseStep.image, typeof(Texture2D), false);
				if(baseStep.image) baseStep.scene = baseStep.image.name;
				else baseStep.scene = "";
			}
			EditorGUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Separator();
			baseStep.show7 = EditorGUILayout.Toggle("Ignore conditions", baseStep.show7, GUILayout.Width(mWidth));
			
			EditorGUILayout.Separator();
			GUILayout.Label("Message", EditorStyles.boldLabel);
			for(int i=0; i<baseStep.message.Length; i++)
			{
				baseStep.message[i] = EditorGUILayout.TextField(DataHolder.Language(i), baseStep.message[i]);
			}
			EditorGUILayout.Separator();
			
			
			baseStep.show6 = EditorGUILayout.Toggle("Add cancel", baseStep.show6, GUILayout.Width(mWidth));
			if(baseStep.show6)
			{
				GUILayout.Label("Cancel text", EditorStyles.boldLabel);
				for(int j=0; j<((string[])baseStep.choice[0]).Length; j++)
				{
					((string[])baseStep.choice[0])[j] = EditorGUILayout.TextField(DataHolder.Language(j), ((string[])baseStep.choice[0])[j]);
				}
				baseStep.choiceNext[0] = EditorGUILayout.IntField("Next step", baseStep.choiceNext[0], GUILayout.Width(mWidth));
			}
			EditorGUILayout.Separator();
			
			baseStep.nextFail = EditorGUILayout.IntField("Next step fail", baseStep.nextFail, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	// equip step
	public static void EquipWeaponStep(int index, GameEvent gameEvent, EquipWeaponStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Equip Weapon");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.characterID = EditorGUILayout.Popup("Character", baseStep.characterID, DataHolder.Characters().GetNameList(true), GUILayout.Width(mWidth));
			
			Class cl = DataHolder.Class(DataHolder.Character(baseStep.characterID).currentClass);
			
			int dummy = 0;
			ArrayList tmp = cl.GetEquipmentParts();
			string[] names = cl.GetEquipmentPartNames(true);
			for(int i=0; i<tmp.Count; i++)
			{
				if((int)tmp[i] == baseStep.number)
				{
					dummy = i;
					break;
				}
			}
			dummy = EditorGUILayout.Popup("Equip on", dummy, names, GUILayout.Width(mWidth));
			if(dummy < tmp.Count) baseStep.number = (int)tmp[dummy];
			
			dummy = 0;
			tmp = cl.GetEquipableWeapons(baseStep.number);
			names = cl.GetEquipableWeaponNames(baseStep.number, true);
			for(int i=0; i<tmp.Count; i++)
			{
				if((int)tmp[i] == baseStep.weaponID)
				{
					dummy = i;
					break;
				}
			}
			dummy = EditorGUILayout.Popup("Weapon", dummy, names, GUILayout.Width(mWidth));
			if(dummy < tmp.Count) baseStep.weaponID = (int)tmp[dummy];
			
			EditorGUILayout.Separator();
		}
	}
	
	public static void EquipArmorStep(int index, GameEvent gameEvent, EquipArmorStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Equip Armor");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.characterID = EditorGUILayout.Popup("Character", baseStep.characterID, DataHolder.Characters().GetNameList(true), GUILayout.Width(mWidth));
			
			Class cl = DataHolder.Class(DataHolder.Character(baseStep.characterID).currentClass);
			
			int dummy = 0;
			ArrayList tmp = cl.GetEquipmentParts();
			string[] names = cl.GetEquipmentPartNames(true);
			for(int i=0; i<tmp.Count; i++)
			{
				if((int)tmp[i] == baseStep.number)
				{
					dummy = i;
					break;
				}
			}
			dummy = EditorGUILayout.Popup("Equip on", dummy, names, GUILayout.Width(mWidth));
			if(dummy < tmp.Count) baseStep.number = (int)tmp[dummy];
			
			dummy = 0;
			tmp = cl.GetEquipableArmors(baseStep.number);
			names = cl.GetEquipableArmorNames(baseStep.number, true);
			for(int i=0; i<tmp.Count; i++)
			{
				if((int)tmp[i] == baseStep.armorID)
				{
					dummy = i;
					break;
				}
			}
			dummy = EditorGUILayout.Popup("Armor", dummy, names, GUILayout.Width(mWidth));
			if(dummy < tmp.Count) baseStep.armorID = (int)tmp[dummy];
			
			EditorGUILayout.Separator();
		}
	}
	
	public static void UnequipStep(int index, GameEvent gameEvent, UnequipStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Unequip");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.characterID = EditorGUILayout.Popup("Character", baseStep.characterID, DataHolder.Characters().GetNameList(true), GUILayout.Width(mWidth));
			
			baseStep.show = EditorGUILayout.Toggle("Unequip all", baseStep.show, GUILayout.Width(mWidth));
			
			if(!baseStep.show)
			{
				Class cl = DataHolder.Class(DataHolder.Character(baseStep.characterID).currentClass);
				
				int dummy = 0;
				ArrayList tmp = cl.GetEquipmentParts();
				string[] names = cl.GetEquipmentPartNames(true);
				for(int i=0; i<tmp.Count; i++)
				{
					if((int)tmp[i] == baseStep.number)
					{
						dummy = i;
						break;
					}
				}
				dummy = EditorGUILayout.Popup("Unequip", dummy, names, GUILayout.Width(mWidth));
				if(dummy < tmp.Count) baseStep.number = (int)tmp[dummy];
			}
			EditorGUILayout.Separator();
		}
	}
	
	// fade steps
	public static void FadeObjectStep(int index, GameEvent gameEvent, FadeObjectStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Fade Object");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			EditorGUILayout.BeginHorizontal();
			baseStep.time = EditorGUILayout.FloatField("Time (s)", baseStep.time, GUILayout.Width(mWidth));
			if(baseStep.time <= 0) baseStep.time = 0.1f;
			baseStep.wait = EditorGUILayout.Toggle("Wait", baseStep.wait, GUILayout.Width(mWidth));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			baseStep.show3 = EditorGUILayout.Toggle("Fade prefab", baseStep.show3, GUILayout.Width(mWidth));
			
			EditorGUILayout.BeginHorizontal();
			if(baseStep.show3)
			{
				baseStep.actorID = EditorGUILayout.IntField("Prefab ID", baseStep.actorID, GUILayout.Width(mWidth));
			}
			else
			{
				baseStep.actorID = EditorGUILayout.Popup("Actor", baseStep.actorID, gameEvent.GetActorList(), GUILayout.Width(mWidth));
			}
			baseStep.show6 = EditorGUILayout.Toggle("Fade children", baseStep.show6, GUILayout.Width(mWidth));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
			
			EditorGUILayout.BeginHorizontal();
			baseStep.show10 = EditorGUILayout.Toggle("Set property", baseStep.show10, GUILayout.Width(mWidth));
			if(baseStep.show10)
			{
				baseStep.materialProperty = EditorGUILayout.TextField(baseStep.materialProperty, GUILayout.Width(mWidth));
				if(baseStep.materialProperty.IndexOf(" ", 0) != -1)
				{
					baseStep.materialProperty = "_Color";
				}
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				
				baseStep.show11 = EditorGUILayout.Toggle("Is float", baseStep.show11, GUILayout.Width(mWidth));
			}
			else
			{
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				baseStep.materialProperty = "_Color";
				baseStep.show11 = false;
			}
			EditorGUILayout.Separator();
			
			baseStep.show7 = EditorGUILayout.Toggle("Flash", baseStep.show7, GUILayout.Width(mWidth));
			baseStep.show8 = EditorGUILayout.Toggle("From current", baseStep.show8, GUILayout.Width(mWidth));
			
			if(baseStep.show11)
			{
				baseStep.show = true;
				baseStep.show2 = false;
				baseStep.show4 = false;
				baseStep.show5 = false;
				
				if(!baseStep.show8) baseStep.float7 = EditorGUILayout.FloatField("From", baseStep.float7, GUILayout.Width(mWidth2));
				baseStep.float8 = EditorGUILayout.FloatField("To", baseStep.float8, GUILayout.Width(mWidth2));
			}
			else
			{
				EditorGUILayout.BeginHorizontal();
				baseStep.show = EditorGUILayout.Toggle("Alpha", baseStep.show, GUILayout.Width(mWidth2));
				if(baseStep.show)
				{
					if(!baseStep.show8) baseStep.float7 = EditorGUILayout.FloatField("From", baseStep.float7, GUILayout.Width(mWidth2));
					baseStep.float8 = EditorGUILayout.FloatField("To", baseStep.float8, GUILayout.Width(mWidth2));
				}
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				baseStep.show2 = EditorGUILayout.Toggle("Red", baseStep.show2, GUILayout.Width(mWidth2));
				if(baseStep.show2)
				{
					if(!baseStep.show8) baseStep.float1 = EditorGUILayout.FloatField("From", baseStep.float1, GUILayout.Width(mWidth2));
					baseStep.float2 = EditorGUILayout.FloatField("To", baseStep.float2, GUILayout.Width(mWidth2));
				}
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				baseStep.show4 = EditorGUILayout.Toggle("Green", baseStep.show4, GUILayout.Width(mWidth2));
				if(baseStep.show4)
				{
					if(!baseStep.show8) baseStep.float3 = EditorGUILayout.FloatField("From", baseStep.float3, GUILayout.Width(mWidth2));
					baseStep.float4 = EditorGUILayout.FloatField("To", baseStep.float4, GUILayout.Width(mWidth2));
				}
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				baseStep.show5 = EditorGUILayout.Toggle("Blue", baseStep.show5, GUILayout.Width(mWidth2));
				if(baseStep.show5)
				{
					if(!baseStep.show8) baseStep.float5 = EditorGUILayout.FloatField("From", baseStep.float5, GUILayout.Width(mWidth2));
					baseStep.float6 = EditorGUILayout.FloatField("To", baseStep.float6, GUILayout.Width(mWidth2));
				}
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
			}
			
			baseStep.interpolate = (EaseType)EditorGUILayout.EnumPopup("Interpolation", baseStep.interpolate, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
			
			baseStep.show9 = EditorGUILayout.Toggle("Shared material", baseStep.show9, GUILayout.Width(mWidth));
			if(baseStep.show9)
			{
				GUILayout.Label("Warning: Using shared materials will change this material in the project!", EditorStyles.boldLabel);
				GUILayout.Label("All objects using this material will be changed!", EditorStyles.boldLabel);
			}
			EditorGUILayout.Separator();
		}
	}
	
	public static void FadeCameraStep(int index, GameEvent gameEvent, FadeCameraStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Fade Camera");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			EditorGUILayout.BeginHorizontal();
			baseStep.time = EditorGUILayout.FloatField("Time (s)", baseStep.time, GUILayout.Width(mWidth));
			if(baseStep.time <= 0) baseStep.time = 0.1f;
			baseStep.wait = EditorGUILayout.Toggle("Wait", baseStep.wait, GUILayout.Width(mWidth));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
			
			baseStep.show7 = EditorGUILayout.Toggle("Flash", baseStep.show7, GUILayout.Width(mWidth));
			EditorGUILayout.BeginHorizontal();
			baseStep.show = EditorGUILayout.Toggle("Alpha", baseStep.show, GUILayout.Width(mWidth2));
			if(baseStep.show)
			{
				baseStep.float7 = EditorGUILayout.FloatField("From", baseStep.float7, GUILayout.Width(mWidth2));
				baseStep.float8 = EditorGUILayout.FloatField("To", baseStep.float8, GUILayout.Width(mWidth2));
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			baseStep.show2 = EditorGUILayout.Toggle("Red", baseStep.show2, GUILayout.Width(mWidth2));
			if(baseStep.show2)
			{
				baseStep.float1 = EditorGUILayout.FloatField("From", baseStep.float1, GUILayout.Width(mWidth2));
				baseStep.float2 = EditorGUILayout.FloatField("To", baseStep.float2, GUILayout.Width(mWidth2));
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			baseStep.show4 = EditorGUILayout.Toggle("Green", baseStep.show4, GUILayout.Width(mWidth2));
			if(baseStep.show4)
			{
				baseStep.float3 = EditorGUILayout.FloatField("From", baseStep.float3, GUILayout.Width(mWidth2));
				baseStep.float4 = EditorGUILayout.FloatField("To", baseStep.float4, GUILayout.Width(mWidth2));
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			baseStep.show5 = EditorGUILayout.Toggle("Blue", baseStep.show5, GUILayout.Width(mWidth2));
			if(baseStep.show5)
			{
				baseStep.float5 = EditorGUILayout.FloatField("From", baseStep.float5, GUILayout.Width(mWidth2));
				baseStep.float6 = EditorGUILayout.FloatField("To", baseStep.float6, GUILayout.Width(mWidth2));
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			baseStep.interpolate = (EaseType)EditorGUILayout.EnumPopup("Interpolation", baseStep.interpolate, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void SendMessageStep(int index, GameEvent gameEvent, SendMessageStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Send Message");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.show3 = EditorGUILayout.Toggle("On prefab", baseStep.show3, GUILayout.Width(mWidth));
			if(baseStep.show3)
			{
				baseStep.actorID = EditorGUILayout.IntField("Prefab ID", baseStep.actorID, GUILayout.Width(mWidth));
			}
			else
			{
				baseStep.actorID = EditorGUILayout.Popup("Actor", baseStep.actorID, gameEvent.GetActorList(), GUILayout.Width(mWidth));
			}
			baseStep.key = EditorGUILayout.TextField("Function", baseStep.key, GUILayout.Width(mWidth));
			baseStep.value = EditorGUILayout.TextField("Value", baseStep.value, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void BroadcastMessageStep(int index, GameEvent gameEvent, BroadcastMessageStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Broadcast Message");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.show3 = EditorGUILayout.Toggle("On prefab", baseStep.show3, GUILayout.Width(mWidth));
			if(baseStep.show3)
			{
				baseStep.actorID = EditorGUILayout.IntField("Prefab ID", baseStep.actorID, GUILayout.Width(mWidth));
			}
			else
			{
				baseStep.actorID = EditorGUILayout.Popup("Actor", baseStep.actorID, gameEvent.GetActorList(), GUILayout.Width(mWidth));
			}
			baseStep.key = EditorGUILayout.TextField("Function", baseStep.key, GUILayout.Width(mWidth));
			baseStep.value = EditorGUILayout.TextField("Value", baseStep.value, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void AddComponentStep(int index, GameEvent gameEvent, AddComponentStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Add Component");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.show3 = EditorGUILayout.Toggle("On prefab", baseStep.show3, GUILayout.Width(mWidth));
			if(baseStep.show3)
			{
				baseStep.actorID = EditorGUILayout.IntField("Prefab ID", baseStep.actorID, GUILayout.Width(mWidth));
			}
			else
			{
				baseStep.actorID = EditorGUILayout.Popup("Actor", baseStep.actorID, gameEvent.GetActorList(), GUILayout.Width(mWidth));
			}
			baseStep.key = EditorGUILayout.TextField("Component", baseStep.key, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void RemoveComponentStep(int index, GameEvent gameEvent, RemoveComponentStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Remove Component");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.show3 = EditorGUILayout.Toggle("On prefab", baseStep.show3, GUILayout.Width(mWidth));
			if(baseStep.show3)
			{
				baseStep.actorID = EditorGUILayout.IntField("Prefab ID", baseStep.actorID, GUILayout.Width(mWidth));
			}
			else
			{
				baseStep.actorID = EditorGUILayout.Popup("Actor", baseStep.actorID, gameEvent.GetActorList(), GUILayout.Width(mWidth));
			}
			baseStep.key = EditorGUILayout.TextField("Component", baseStep.key, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void ActivateObjectStep(int index, GameEvent gameEvent, ActivateObjectStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Activate Object");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.show3 = EditorGUILayout.Toggle("Use prefab", baseStep.show3, GUILayout.Width(mWidth));
			if(baseStep.show3)
			{
				baseStep.actorID = EditorGUILayout.IntField("Prefab ID", baseStep.actorID, GUILayout.Width(mWidth));
			}
			else
			{
				baseStep.actorID = EditorGUILayout.Popup("Actor", baseStep.actorID, gameEvent.GetActorList(), GUILayout.Width(mWidth));
			}
			baseStep.show = EditorGUILayout.Toggle("Active/Inactive", baseStep.show, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void ObjectVisibleStep(int index, GameEvent gameEvent, ObjectVisibleStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Object Visible");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.show3 = EditorGUILayout.Toggle("Use prefab", baseStep.show3, GUILayout.Width(mWidth));
			if(baseStep.show3)
			{
				baseStep.actorID = EditorGUILayout.IntField("Prefab ID", baseStep.actorID, GUILayout.Width(mWidth));
			}
			else
			{
				baseStep.actorID = EditorGUILayout.Popup("Actor", baseStep.actorID, gameEvent.GetActorList(), GUILayout.Width(mWidth));
			}
			baseStep.show = EditorGUILayout.Toggle("Visible/Invisible", baseStep.show, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void ParentObjectStep(int index, GameEvent gameEvent, ParentObjectStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Parent Object");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.show2 = EditorGUILayout.Toggle("Use prefab", baseStep.show2, GUILayout.Width(mWidth));
			if(baseStep.show2)
			{
				baseStep.actorID = EditorGUILayout.IntField("Prefab ID", baseStep.actorID, GUILayout.Width(mWidth));
			}
			else
			{
				baseStep.actorID = EditorGUILayout.Popup("Actor", baseStep.actorID, gameEvent.GetActorList(), GUILayout.Width(mWidth));
			}
			baseStep.show = EditorGUILayout.Toggle("Parent/Unparent", baseStep.show, GUILayout.Width(mWidth));
			
			if(baseStep.show)
			{
				baseStep.show3 = EditorGUILayout.Toggle("Parent to prefab", baseStep.show3, GUILayout.Width(mWidth));
				if(baseStep.show3)
				{
					baseStep.prefabID = EditorGUILayout.IntField("Prefab ID", baseStep.prefabID, GUILayout.Width(mWidth));
				}
				else
				{
					baseStep.prefabID = EditorGUILayout.Popup("Actor", baseStep.prefabID, gameEvent.GetActorList(), GUILayout.Width(mWidth));
				}
				baseStep.pathToChild = EditorGUILayout.TextField("Path to child", baseStep.pathToChild, GUILayout.Width(mWidth*1.2f));
				EditorGUILayout.Separator();
				
				baseStep.show4 = EditorGUILayout.Toggle("Set to position", baseStep.show4, GUILayout.Width(mWidth));
				if(baseStep.show4)
				{
					baseStep.v3 = EditorGUILayout.Vector3Field("Offset", baseStep.v3, GUILayout.Width(mWidth));
					baseStep.show6 = EditorGUILayout.Toggle("Local space", baseStep.show6, GUILayout.Width(mWidth));
				}
				EditorGUILayout.Separator();
				
				baseStep.show5 = EditorGUILayout.Toggle("Use rotation", baseStep.show5, GUILayout.Width(mWidth));
				if(baseStep.show5)
				{
					baseStep.v3_2 = EditorGUILayout.Vector3Field("Rotation offset", baseStep.v3_2, GUILayout.Width(mWidth));
				}
				EditorGUILayout.Separator();
				
			}
			EditorGUILayout.Separator();
		}
	}
	
	public static void CallGlobalEventStep(int index, GameEvent gameEvent, CallGlobalEventStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Call Global Event");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.number = EditorGUILayout.Popup("Global Event", baseStep.number, 
					DataHolder.GlobalEvents().GetNameList(true), GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	// item steps
	public static void AddItemStep(int index, GameEvent gameEvent, AddItemStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Add Item");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.itemID = EditorGUILayout.Popup("Item", baseStep.itemID, DataHolder.Items().GetNameList(true), GUILayout.Width(mWidth));
			baseStep.number = EditorGUILayout.IntField("Number", baseStep.number, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void RemoveItemStep(int index, GameEvent gameEvent, RemoveItemStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Remove Item");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.itemID = EditorGUILayout.Popup("Item", baseStep.itemID, DataHolder.Items().GetNameList(true), GUILayout.Width(mWidth));
			baseStep.number = EditorGUILayout.IntField("Number", baseStep.number, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void CheckItemStep(int index, GameEvent gameEvent, CheckItemStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Check Item");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.itemID = EditorGUILayout.Popup("Item", baseStep.itemID, DataHolder.Items().GetNameList(true), GUILayout.Width(mWidth));
			baseStep.number = EditorGUILayout.IntField("Number", baseStep.number, GUILayout.Width(mWidth));
			baseStep.nextFail = EditorGUILayout.IntField("Next step fail", baseStep.nextFail, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void LearnItemRecipeStep(int index, GameEvent gameEvent, LearnItemRecipeStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Learn Item Recipe");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.itemID = EditorGUILayout.Popup("Recipe", baseStep.itemID, DataHolder.ItemRecipes().GetNameList(true), GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void ItemRecipeKnownStep(int index, GameEvent gameEvent, ItemRecipeKnownStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Item Recipe Known");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.itemID = EditorGUILayout.Popup("Recipe", baseStep.itemID, DataHolder.ItemRecipes().GetNameList(true), GUILayout.Width(mWidth));
			baseStep.nextFail = EditorGUILayout.IntField("Next step fail", baseStep.nextFail, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	// level steps
	public static void LevelUpStep(int index, GameEvent gameEvent, LevelUpStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Level Up");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			EditorGUILayout.BeginHorizontal();
			baseStep.show = EditorGUILayout.Toggle("Whole party", baseStep.show, GUILayout.Width(mWidth));
			if(baseStep.show)
			{
				EditorGUILayout.EndHorizontal();
				baseStep.show2 = EditorGUILayout.Toggle("Only battle party", baseStep.show2, GUILayout.Width(mWidth));
			}
			else
			{
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
			}
			if(!baseStep.show)
			{
				baseStep.characterID = EditorGUILayout.Popup("Character", baseStep.characterID, DataHolder.Characters().GetNameList(true), GUILayout.Width(mWidth));
			}
			baseStep.show3 = EditorGUILayout.Toggle("Class level", baseStep.show3, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void CheckLevelStep(int index, GameEvent gameEvent, CheckLevelStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Check Level");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			EditorGUILayout.BeginHorizontal();
			baseStep.show = EditorGUILayout.Toggle("Whole party", baseStep.show, GUILayout.Width(mWidth));
			if(baseStep.show)
			{
				EditorGUILayout.EndHorizontal();
				baseStep.show2 = EditorGUILayout.Toggle("Only battle party", baseStep.show2, GUILayout.Width(mWidth));
			}
			else
			{
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
			}
			if(!baseStep.show)
			{
				baseStep.characterID = EditorGUILayout.Popup("Character", baseStep.characterID, DataHolder.Characters().GetNameList(true), GUILayout.Width(mWidth));
			}
			baseStep.show3 = EditorGUILayout.Toggle("Class level", baseStep.show3, GUILayout.Width(mWidth));
			baseStep.number = EditorGUILayout.IntField("Level", baseStep.number, GUILayout.Width(mWidth));
			baseStep.nextFail = EditorGUILayout.IntField("Next step fail", baseStep.nextFail, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	// money steps
	public static void AddMoneyStep(int index, GameEvent gameEvent, AddMoneyStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Add Money");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.number = EditorGUILayout.IntField("Number", baseStep.number, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void RemoveMoneyStep(int index, GameEvent gameEvent, RemoveMoneyStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Remove Money");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.number = EditorGUILayout.IntField("Number", baseStep.number, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void SetMoneyStep(int index, GameEvent gameEvent, SetMoneyStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Set Money");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.number = EditorGUILayout.IntField("Number", baseStep.number, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void CheckMoneyStep(int index, GameEvent gameEvent, CheckMoneyStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Check Money");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.number = EditorGUILayout.IntField("Number", baseStep.number, GUILayout.Width(mWidth));
			baseStep.nextFail = EditorGUILayout.IntField("Next step fail", baseStep.nextFail, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	// move steps
	public static void SetToPositionStep(int index, GameEvent gameEvent, SetToPositionStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Set To Position");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.show = EditorGUILayout.Toggle("On waypoint", baseStep.show, GUILayout.Width(mWidth));
			if(baseStep.show)
			{
				baseStep.waypointID = EditorGUILayout.Popup("Waypoint", baseStep.waypointID, gameEvent.GetWaypointList(), GUILayout.Width(mWidth));
				baseStep.v3 = EditorGUILayout.Vector3Field("Offset", baseStep.v3, GUILayout.Width(mWidth));
			}
			else
			{
				baseStep.v3 = EditorGUILayout.Vector3Field("Position", baseStep.v3, GUILayout.Width(mWidth));
			}
			
			baseStep.show3 = EditorGUILayout.Toggle("Set prefab", baseStep.show3, GUILayout.Width(mWidth));
			if(baseStep.show3)
			{
				baseStep.actorID = EditorGUILayout.IntField("Prefab ID", baseStep.actorID, GUILayout.Width(mWidth));
			}
			else
			{
				baseStep.actorID = EditorGUILayout.Popup("Actor", baseStep.actorID, gameEvent.GetActorList(), GUILayout.Width(mWidth));
			}
			EditorGUILayout.Separator();
		}
	}
	
	public static void MoveToWaypointStep(int index, GameEvent gameEvent, MoveToWaypointStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Move To Waypoint");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			EditorGUILayout.BeginHorizontal();
			baseStep.time = EditorGUILayout.FloatField("Time (s)", baseStep.time, GUILayout.Width(mWidth));
			if(baseStep.time <= 0) baseStep.time = 0.1f;
			baseStep.wait = EditorGUILayout.Toggle("Wait", baseStep.wait, GUILayout.Width(mWidth));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			baseStep.waypointID = EditorGUILayout.Popup("Waypoint", baseStep.waypointID, gameEvent.GetWaypointList(), GUILayout.Width(mWidth));
			baseStep.show = EditorGUILayout.Toggle("Face direction", baseStep.show, GUILayout.Width(mWidth));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			baseStep.show3 = EditorGUILayout.Toggle("Move prefab", baseStep.show3, GUILayout.Width(mWidth));
			EditorGUILayout.BeginHorizontal();
			if(baseStep.show3)
			{
				baseStep.actorID = EditorGUILayout.IntField("Prefab ID", baseStep.actorID, GUILayout.Width(mWidth));
			}
			else
			{
				baseStep.actorID = EditorGUILayout.Popup("Actor", baseStep.actorID, gameEvent.GetActorList(), GUILayout.Width(mWidth));
			}
			baseStep.controller = EditorGUILayout.Toggle("Controller move", baseStep.controller, GUILayout.Width(mWidth));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			baseStep.interpolate = (EaseType)EditorGUILayout.EnumPopup("Interpolation", baseStep.interpolate, GUILayout.Width(mWidth));
			if(baseStep.controller) baseStep.show2 = EditorGUILayout.Toggle("Apply gravity", baseStep.show2, GUILayout.Width(mWidth));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
		}
	}
	
	public static void MoveToDirectionStep(int index, GameEvent gameEvent, MoveToDirectionStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Move To Direction");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			EditorGUILayout.BeginHorizontal();
			baseStep.time = EditorGUILayout.FloatField("Time (s)", baseStep.time, GUILayout.Width(mWidth));
			if(baseStep.time <= 0) baseStep.time = 0.1f;
			baseStep.wait = EditorGUILayout.Toggle("Wait", baseStep.wait, GUILayout.Width(mWidth));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			baseStep.show2 = EditorGUILayout.Toggle("Local space", baseStep.show2, GUILayout.Width(mWidth));
			EditorGUILayout.BeginHorizontal();
			baseStep.v3 = EditorGUILayout.Vector3Field("Direction", baseStep.v3, GUILayout.Width(mWidth));
			baseStep.show = EditorGUILayout.Toggle("Face direction", baseStep.show, GUILayout.Width(mWidth));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			baseStep.speed = EditorGUILayout.FloatField("Speed", baseStep.speed, GUILayout.Width(mWidth));
			
			baseStep.show3 = EditorGUILayout.Toggle("Move prefab", baseStep.show3, GUILayout.Width(mWidth));
			EditorGUILayout.BeginHorizontal();
			if(baseStep.show3)
			{
				baseStep.actorID = EditorGUILayout.IntField("Prefab ID", baseStep.actorID, GUILayout.Width(mWidth));
			}
			else
			{
				baseStep.actorID = EditorGUILayout.Popup("Actor", baseStep.actorID, gameEvent.GetActorList(), GUILayout.Width(mWidth));
			}
			baseStep.controller = EditorGUILayout.Toggle("Controller move", baseStep.controller, GUILayout.Width(mWidth));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
		}
	}
	
	public static void MoveToPrefabStep(int index, GameEvent gameEvent, MoveToPrefabStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Move To Prefab");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			EditorGUILayout.BeginHorizontal();
			baseStep.time = EditorGUILayout.FloatField("Time (s)", baseStep.time, GUILayout.Width(mWidth));
			if(baseStep.time <= 0) baseStep.time = 0.1f;
			baseStep.wait = EditorGUILayout.Toggle("Wait", baseStep.wait, GUILayout.Width(mWidth));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			baseStep.prefabID = EditorGUILayout.IntField("Prefab ID", baseStep.prefabID, GUILayout.Width(mWidth));
			baseStep.show = EditorGUILayout.Toggle("Face direction", baseStep.show, GUILayout.Width(mWidth));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			baseStep.show3 = EditorGUILayout.Toggle("Move prefab", baseStep.show3, GUILayout.Width(mWidth));
			EditorGUILayout.BeginHorizontal();
			if(baseStep.show3)
			{
				baseStep.actorID = EditorGUILayout.IntField("Prefab ID", baseStep.actorID, GUILayout.Width(mWidth));
			}
			else
			{
				baseStep.actorID = EditorGUILayout.Popup("Actor", baseStep.actorID, gameEvent.GetActorList(), GUILayout.Width(mWidth));
			}
			baseStep.controller = EditorGUILayout.Toggle("Controller move", baseStep.controller, GUILayout.Width(mWidth));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			baseStep.interpolate = (EaseType)EditorGUILayout.EnumPopup("Interpolation", baseStep.interpolate, GUILayout.Width(mWidth));
			if(baseStep.controller) baseStep.show2 = EditorGUILayout.Toggle("Apply gravity", baseStep.show2, GUILayout.Width(mWidth));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
		}
	}
	
	public static void RotateToWaypointStep(int index, GameEvent gameEvent, RotateToWaypointStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Rotate To Waypoint");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			EditorGUILayout.BeginHorizontal();
			baseStep.time = EditorGUILayout.FloatField("Time (s)", baseStep.time, GUILayout.Width(mWidth));
			if(baseStep.time <= 0) baseStep.time = 0.1f;
			baseStep.wait = EditorGUILayout.Toggle("Wait", baseStep.wait, GUILayout.Width(mWidth));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			baseStep.waypointID = EditorGUILayout.Popup("Waypoint", baseStep.waypointID, gameEvent.GetWaypointList(), GUILayout.Width(mWidth));
			baseStep.show3 = EditorGUILayout.Toggle("Move prefab", baseStep.show3, GUILayout.Width(mWidth));
			if(baseStep.show3)
			{
				baseStep.actorID = EditorGUILayout.IntField("Prefab ID", baseStep.actorID, GUILayout.Width(mWidth));
			}
			else
			{
				baseStep.actorID = EditorGUILayout.Popup("Actor", baseStep.actorID, gameEvent.GetActorList(), GUILayout.Width(mWidth));
			}
			baseStep.interpolate = (EaseType)EditorGUILayout.EnumPopup("Interpolation", baseStep.interpolate, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void RotationStep(int index, GameEvent gameEvent, RotationStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Rotation");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			
			EditorGUILayout.BeginHorizontal();
			baseStep.time = EditorGUILayout.FloatField("Time (s)", baseStep.time, GUILayout.Width(mWidth));
			if(baseStep.time <= 0) baseStep.time = 0.1f;
			baseStep.wait = EditorGUILayout.Toggle("Wait", baseStep.wait, GUILayout.Width(mWidth));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			baseStep.show3 = EditorGUILayout.Toggle("Move prefab", baseStep.show3, GUILayout.Width(mWidth));
			if(baseStep.show3)
			{
				baseStep.actorID = EditorGUILayout.IntField("Prefab ID", baseStep.actorID, GUILayout.Width(mWidth));
			}
			else
			{
				baseStep.actorID = EditorGUILayout.Popup("Actor", baseStep.actorID, gameEvent.GetActorList(), GUILayout.Width(mWidth));
			}
			baseStep.show2 = EditorGUILayout.Toggle("Interpolate", baseStep.show2, GUILayout.Width(mWidth));
			if(baseStep.show2)
			{
				baseStep.interpolate = (EaseType)EditorGUILayout.EnumPopup("Interpolation", baseStep.interpolate, GUILayout.Width(mWidth));
				baseStep.speed = EditorGUILayout.FloatField("Degree", baseStep.speed, GUILayout.Width(mWidth));
			}
			else
			{
				baseStep.speed = EditorGUILayout.FloatField("Speed", baseStep.speed, GUILayout.Width(mWidth));
			}
			
			baseStep.show = EditorGUILayout.Toggle("Set axis", baseStep.show, GUILayout.Width(mWidth));
			if(baseStep.show)
			{
				baseStep.v3 = EditorGUILayout.Vector3Field("Rotation axis", baseStep.v3, GUILayout.Width(mWidth));
			}
			EditorGUILayout.Separator();
		}
	}
	
	// party steps
	public static void JoinPartyStep(int index, GameEvent gameEvent, JoinPartyStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Join Party");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.characterID = EditorGUILayout.Popup("Character", baseStep.characterID, DataHolder.Characters().GetNameList(true), GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void LeavePartyStep(int index, GameEvent gameEvent, LeavePartyStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Leave Party");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.characterID = EditorGUILayout.Popup("Character", baseStep.characterID, DataHolder.Characters().GetNameList(true), GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void IsInPartyStep(int index, GameEvent gameEvent, IsInPartyStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Is In Party");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.characterID = EditorGUILayout.Popup("Character", baseStep.characterID, DataHolder.Characters().GetNameList(true), GUILayout.Width(mWidth));
			baseStep.nextFail = EditorGUILayout.IntField("Next step fail", baseStep.nextFail, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void HasLeftPartyStep(int index, GameEvent gameEvent, HasLeftPartyStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Has Left Party");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.characterID = EditorGUILayout.Popup("Character", baseStep.characterID, DataHolder.Characters().GetNameList(true), GUILayout.Width(mWidth));
			baseStep.nextFail = EditorGUILayout.IntField("Next step fail", baseStep.nextFail, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void CheckPlayerStep(int index, GameEvent gameEvent, CheckPlayerStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Check Player");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.characterID = EditorGUILayout.Popup("Character", baseStep.characterID, DataHolder.Characters().GetNameList(true), GUILayout.Width(mWidth));
			baseStep.nextFail = EditorGUILayout.IntField("Next step fail", baseStep.nextFail, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void SetPlayerStep(int index, GameEvent gameEvent, SetPlayerStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Set Player");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.characterID = EditorGUILayout.Popup("Character", baseStep.characterID, DataHolder.Characters().GetNameList(true), GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void SetCharacterNameStep(int index, GameEvent gameEvent, SetCharacterNameStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Set Character Name");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.characterID = EditorGUILayout.Popup("Character", baseStep.characterID, 
					DataHolder.Characters().GetNameList(true), GUILayout.Width(mWidth));
			baseStep.show = EditorGUILayout.Toggle("From variable", baseStep.show, GUILayout.Width(mWidth));
			if(baseStep.show)
			{
				baseStep.key = EditorGUILayout.TextField("Variable key", baseStep.key, GUILayout.Width(mWidth));
			}
			else
			{
				baseStep.key = EditorGUILayout.TextField("Name", baseStep.key, GUILayout.Width(mWidth));
			}
			EditorGUILayout.Separator();
		}
	}
	
	// skill steps
	public static void LearnSkillStep(int index, GameEvent gameEvent, LearnSkillStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Learn Skill");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.characterID = EditorGUILayout.Popup("Character", baseStep.characterID, DataHolder.Characters().GetNameList(true), GUILayout.Width(mWidth));
			baseStep.skillID = EditorGUILayout.Popup("Skill", baseStep.skillID, DataHolder.Skills().GetNameList(true), GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void ForgetSkillStep(int index, GameEvent gameEvent, ForgetSkillStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Forget Skill");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.characterID = EditorGUILayout.Popup("Character", baseStep.characterID, DataHolder.Characters().GetNameList(true), GUILayout.Width(mWidth));
			baseStep.skillID = EditorGUILayout.Popup("Skill", baseStep.skillID, DataHolder.Skills().GetNameList(true), GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void HasSkillStep(int index, GameEvent gameEvent, HasSkillStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Has Skill");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			EditorGUILayout.BeginHorizontal();
			baseStep.show = EditorGUILayout.Toggle("Whole party", baseStep.show, GUILayout.Width(mWidth));
			if(baseStep.show)
			{
				EditorGUILayout.EndHorizontal();
				baseStep.show2 = EditorGUILayout.Toggle("Only battle party", baseStep.show2, GUILayout.Width(mWidth));
			}
			else
			{
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
			}
			if(!baseStep.show)
			{
				baseStep.characterID = EditorGUILayout.Popup("Character", baseStep.characterID, DataHolder.Characters().GetNameList(true), GUILayout.Width(mWidth));
			}
			EditorGUILayout.BeginHorizontal();
			baseStep.skillID = EditorGUILayout.Popup("Skill", baseStep.skillID, DataHolder.Skills().GetNameList(true), GUILayout.Width(mWidth));
			baseStep.show3 = EditorGUILayout.Toggle("Learned", baseStep.show3, GUILayout.Width(mWidth));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			baseStep.nextFail = EditorGUILayout.IntField("Next step fail", baseStep.nextFail, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	// spawn steps
	public static void SpawnPlayerStep(int index, GameEvent gameEvent, SpawnPlayerStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Spawn Player");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.spawnID = EditorGUILayout.IntField("SpawnPoint ID", baseStep.spawnID, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void DestroyPlayerStep(int index, GameEvent gameEvent, DestroyPlayerStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Destroy Player");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.show = EditorGUILayout.Toggle("Whole party", baseStep.show, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void SpawnPrefabStep(int index, GameEvent gameEvent, SpawnPrefabStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Spawn Prefab");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.prefabID = EditorGUILayout.Popup("Prefab", baseStep.prefabID, gameEvent.GetPrefabList(), GUILayout.Width(mWidth));
			baseStep.number = EditorGUILayout.IntField("Prefab ID", baseStep.number, GUILayout.Width(mWidth));
			baseStep.show = EditorGUILayout.Toggle("On actor", baseStep.show, GUILayout.Width(mWidth));
			if(baseStep.show)
			{
				baseStep.spawnID = EditorGUILayout.Popup("Actor", baseStep.spawnID, gameEvent.GetActorList(), GUILayout.Width(mWidth));
			}
			else
			{
				baseStep.spawnID = EditorGUILayout.Popup("Waypoint", baseStep.spawnID, gameEvent.GetWaypointList(), GUILayout.Width(mWidth));
			}
			baseStep.v3 = EditorGUILayout.Vector3Field("Spawn offset", baseStep.v3, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void DestroyPrefabStep(int index, GameEvent gameEvent, DestroyPrefabStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Destroy Prefab");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.number = EditorGUILayout.IntField("Prefab ID", baseStep.number, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void TeleportStep(int index, GameEvent gameEvent, TeleportStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Teleport (ends event)");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.spawnID = EditorGUILayout.Popup("Teleport to", baseStep.spawnID, 
					DataHolder.Teleports().GetNameList(true), GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	// status effect steps
	public static void ChangeStatusEffectStep(int index, GameEvent gameEvent, ChangeStatusEffectStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Change Status Effect");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			EditorGUILayout.BeginHorizontal();
			baseStep.show = EditorGUILayout.Toggle("Whole party", baseStep.show, GUILayout.Width(mWidth));
			if(baseStep.show)
			{
				EditorGUILayout.EndHorizontal();
				baseStep.show2 = EditorGUILayout.Toggle("Only battle party", baseStep.show2, GUILayout.Width(mWidth));
			}
			else
			{
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
			}
			if(!baseStep.show)
			{
				baseStep.characterID = EditorGUILayout.Popup("Character", baseStep.characterID, DataHolder.Characters().GetNameList(true), GUILayout.Width(mWidth));
			}
			EditorGUILayout.Separator();
			for(int i=0; i<baseStep.effect.Length; i++)
			{
				baseStep.effect[i] = (SkillEffect)EditorGUILayout.EnumPopup(DataHolder.Effects().GetName(i), baseStep.effect[i], GUILayout.Width(mWidth));
			}
			EditorGUILayout.Separator();
		}
	}
	
	// status value steps
	public static void RegenerateStep(int index, GameEvent gameEvent, RegenerateStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Regenerate");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			EditorGUILayout.BeginHorizontal();
			baseStep.show = EditorGUILayout.Toggle("Whole party", baseStep.show, GUILayout.Width(mWidth));
			if(baseStep.show)
			{
				EditorGUILayout.EndHorizontal();
				baseStep.show2 = EditorGUILayout.Toggle("Only battle party", baseStep.show2, GUILayout.Width(mWidth));
			}
			else
			{
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
			}
			if(!baseStep.show)
			{
				baseStep.characterID = EditorGUILayout.Popup("Character", baseStep.characterID, DataHolder.Characters().GetNameList(true), GUILayout.Width(mWidth));
			}
			EditorGUILayout.Separator();
		}
	}
	
	public static void CheckStatusValueStep(int index, GameEvent gameEvent, CheckStatusValueStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Check Status Value");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			EditorGUILayout.BeginHorizontal();
			baseStep.show = EditorGUILayout.Toggle("Whole party", baseStep.show, GUILayout.Width(mWidth));
			if(baseStep.show)
			{
				EditorGUILayout.EndHorizontal();
				baseStep.show2 = EditorGUILayout.Toggle("Only battle party", baseStep.show2, GUILayout.Width(mWidth));
			}
			else
			{
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
			}
			if(!baseStep.show)
			{
				baseStep.characterID = EditorGUILayout.Popup("Character", baseStep.characterID, DataHolder.Characters().GetNameList(true), GUILayout.Width(mWidth));
			}
			baseStep.itemID = EditorGUILayout.Popup("Status value", baseStep.itemID, DataHolder.StatusValues().GetNameList(true), GUILayout.Width(mWidth));
			EditorGUILayout.BeginHorizontal();
			baseStep.valueCheck = (ValueCheck)EditorGUILayout.EnumPopup("Check", baseStep.valueCheck, GUILayout.Width(mWidth));
			baseStep.number = EditorGUILayout.IntField(baseStep.number, GUILayout.Width(mWidth));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			baseStep.nextFail = EditorGUILayout.IntField("Next step fail", baseStep.nextFail, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void SetStatusValueStep(int index, GameEvent gameEvent, SetStatusValueStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Set Status Value");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			EditorGUILayout.BeginHorizontal();
			baseStep.show = EditorGUILayout.Toggle("Whole party", baseStep.show, GUILayout.Width(mWidth));
			if(baseStep.show)
			{
				EditorGUILayout.EndHorizontal();
				baseStep.show2 = EditorGUILayout.Toggle("Only battle party", baseStep.show2, GUILayout.Width(mWidth));
			}
			else
			{
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
			}
			if(!baseStep.show)
			{
				baseStep.characterID = EditorGUILayout.Popup("Character", baseStep.characterID, DataHolder.Characters().GetNameList(true), GUILayout.Width(mWidth));
			}
			baseStep.itemID = EditorGUILayout.Popup("Status value", baseStep.itemID, DataHolder.StatusValues().GetNameList(true), GUILayout.Width(mWidth));
			EditorGUILayout.BeginHorizontal();
			baseStep.simpleOperator = (SimpleOperator)EditorGUILayout.EnumPopup("Operation", baseStep.simpleOperator, GUILayout.Width(mWidth));
			baseStep.number = EditorGUILayout.IntField(baseStep.number, GUILayout.Width(mWidth));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
		}
	}
	
	// variable steps
	public static void SetVariableStep(int index, GameEvent gameEvent, SetVariableStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Set Variable");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.key = EditorGUILayout.TextField("Variable key", baseStep.key, GUILayout.Width(mWidth));
			baseStep.value = EditorGUILayout.TextField("Variable value", baseStep.value, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void RemoveVariableStep(int index, GameEvent gameEvent, RemoveVariableStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Remove Variable");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.key = EditorGUILayout.TextField("Variable key", baseStep.key, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void CheckVariableStep(int index, GameEvent gameEvent, CheckVariableStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Check Variable");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.key = EditorGUILayout.TextField("Variable key", baseStep.key, GUILayout.Width(mWidth));
			baseStep.value = EditorGUILayout.TextField("Variable value", baseStep.value, GUILayout.Width(mWidth));
			baseStep.nextFail = EditorGUILayout.IntField("Next step fail", baseStep.nextFail, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void SetNumberVariableStep(int index, GameEvent gameEvent, SetNumberVariableStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Set Number Variable");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.key = EditorGUILayout.TextField("Variable key", baseStep.key, GUILayout.Width(mWidth));
			baseStep.simpleOperator = (SimpleOperator)EditorGUILayout.EnumPopup("Operation", baseStep.simpleOperator, GUILayout.Width(mWidth));
			baseStep.float1 = EditorGUILayout.FloatField("Variable value", baseStep.float1, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void RemoveNumberVariableStep(int index, GameEvent gameEvent, RemoveNumberVariableStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Remove Number Variable");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.key = EditorGUILayout.TextField("Variable key", baseStep.key, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void CheckNumberVariableStep(int index, GameEvent gameEvent, CheckNumberVariableStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Check Number Variable");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.key = EditorGUILayout.TextField("Variable key", baseStep.key, GUILayout.Width(mWidth));
			baseStep.valueCheck = (ValueCheck)EditorGUILayout.EnumPopup("Check", baseStep.valueCheck, GUILayout.Width(mWidth));
			baseStep.float1 = EditorGUILayout.FloatField("Variable value", baseStep.float1, GUILayout.Width(mWidth));
			baseStep.nextFail = EditorGUILayout.IntField("Next step fail", baseStep.nextFail, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void SetPlayerPrefsStep(int index, GameEvent gameEvent, SetPlayerPrefsStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Set PlayerPrefs");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.show = EditorGUILayout.Toggle("Number variable", baseStep.show, GUILayout.Width(mWidth));
			baseStep.key = EditorGUILayout.TextField("Variable key", baseStep.key, GUILayout.Width(mWidth));
			baseStep.value = EditorGUILayout.TextField("PlayerPrefs key", baseStep.value, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void GetPlayerPrefsStep(int index, GameEvent gameEvent, GetPlayerPrefsStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Get PlayerPrefs");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.value = EditorGUILayout.TextField("PlayerPrefs key", baseStep.value, GUILayout.Width(mWidth));
			baseStep.show = EditorGUILayout.Toggle("Number variable", baseStep.show, GUILayout.Width(mWidth));
			baseStep.key = EditorGUILayout.TextField("Variable key", baseStep.key, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void HasPlayerPrefsStep(int index, GameEvent gameEvent, HasPlayerPrefsStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Has PlayerPrefs");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.key = EditorGUILayout.TextField("PlayerPrefs key", baseStep.key, GUILayout.Width(mWidth));
			baseStep.nextFail = EditorGUILayout.IntField("Next step fail", baseStep.nextFail, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	// weapon steps
	public static void AddWeaponStep(int index, GameEvent gameEvent, AddWeaponStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Add Weapon");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.weaponID = EditorGUILayout.Popup("Weapon", baseStep.weaponID, DataHolder.Weapons().GetNameList(true), GUILayout.Width(mWidth));
			baseStep.number = EditorGUILayout.IntField("Number", baseStep.number, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void RemoveWeaponStep(int index, GameEvent gameEvent, RemoveWeaponStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Remove Weapon");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.weaponID = EditorGUILayout.Popup("Weapon", baseStep.weaponID, DataHolder.Weapons().GetNameList(true), GUILayout.Width(mWidth));
			baseStep.number = EditorGUILayout.IntField("Number", baseStep.number, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void CheckWeaponStep(int index, GameEvent gameEvent, CheckWeaponStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Check Weapon");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.weaponID = EditorGUILayout.Popup("Weapon", baseStep.weaponID, DataHolder.Weapons().GetNameList(true), GUILayout.Width(mWidth));
			baseStep.number = EditorGUILayout.IntField("Number", baseStep.number, GUILayout.Width(mWidth));
			baseStep.nextFail = EditorGUILayout.IntField("Next step fail", baseStep.nextFail, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	// class steps
	public static void CheckClassStep(int index, GameEvent gameEvent, CheckClassStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Check Class");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.characterID = EditorGUILayout.Popup("Character", baseStep.characterID, DataHolder.Characters().GetNameList(true), GUILayout.Width(mWidth));
			baseStep.number = EditorGUILayout.Popup("Class", baseStep.number, DataHolder.Classes().GetNameList(true), GUILayout.Width(mWidth));
			baseStep.nextFail = EditorGUILayout.IntField("Next step fail", baseStep.nextFail, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void ChangeClassStep(int index, GameEvent gameEvent, ChangeClassStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Change Class");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.characterID = EditorGUILayout.Popup("Character", baseStep.characterID, 
					DataHolder.Characters().GetNameList(true), GUILayout.Width(mWidth));
			baseStep.number = EditorGUILayout.Popup("Class", baseStep.number, 
					DataHolder.Classes().GetNameList(true), GUILayout.Width(mWidth));
			
			baseStep.show = EditorGUILayout.Toggle("Forget old skills", 
					baseStep.show, GUILayout.Width(mWidth));
			baseStep.show2 = EditorGUILayout.Toggle("Learn skills", 
					baseStep.show2, GUILayout.Width(mWidth));
			baseStep.show3 = EditorGUILayout.Toggle("Set class level", 
					baseStep.show3, GUILayout.Width(mWidth));
			baseStep.show4 = EditorGUILayout.Toggle("Remove old bonus", 
					baseStep.show4, GUILayout.Width(mWidth));
			baseStep.show5 = EditorGUILayout.Toggle("Get status bonus", 
					baseStep.show5, GUILayout.Width(mWidth));
			
			EditorGUILayout.Separator();
		}
	}
	
	// statistic steps
	public static void CustomStatisticStep(int index, GameEvent gameEvent, CustomStatisticStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Custom Statistic");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			baseStep.number = EditorGUILayout.IntField("Index", 
					baseStep.number, GUILayout.Width(mWidth));
			baseStep.itemID = EditorGUILayout.IntField("Add", 
					baseStep.itemID, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void ClearStatisticStep(int index, GameEvent gameEvent, ClearStatisticStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Clear Statistic");
		if(baseStep.fold)
		{
			GameEventTabs.EventStep(index, gameEvent, baseStep);
			EditorGUILayout.Separator();
		}
	}
}