
using UnityEditor;
using UnityEngine;

public class BattleAnimationTabs
{
	public static int mWidth = 300;
	public static int mWidth2 = 200;
	
	public static void ShowTab(int index, BattleAnimation battleAnimation, AnimationStep baseStep)
	{
		if(baseStep is WaitAStep) BattleAnimationTabs.WaitAStep(index, battleAnimation, (WaitAStep)baseStep);
		else if(baseStep is GoToAStep) BattleAnimationTabs.GoToAStep(index, battleAnimation, (GoToAStep)baseStep);
		else if(baseStep is RandomAStep) BattleAnimationTabs.RandomAStep(index, battleAnimation, (RandomAStep)baseStep);
		else if(baseStep is CheckRandomAStep) BattleAnimationTabs.CheckRandomAStep(index, battleAnimation, (CheckRandomAStep)baseStep);
		else if(baseStep is CheckFormulaAStep) BattleAnimationTabs.CheckFormulaAStep(index, battleAnimation, (CheckFormulaAStep)baseStep);
		else if(baseStep is WaitForButtonAStep) BattleAnimationTabs.WaitForButtonAStep(index, battleAnimation, (WaitForButtonAStep)baseStep);
		else if(baseStep is CheckDifficultyAStep) BattleAnimationTabs.CheckDifficultyAStep(index, battleAnimation, (CheckDifficultyAStep)baseStep);
		else if(baseStep is CheckUserAStep) BattleAnimationTabs.CheckUserAStep(index, battleAnimation, (CheckUserAStep)baseStep);
		else if(baseStep is PlayAnimationAStep) BattleAnimationTabs.PlayAnimationAStep(index, battleAnimation, (PlayAnimationAStep)baseStep);
		else if(baseStep is StopAnimationAStep) BattleAnimationTabs.StopAnimationAStep(index, battleAnimation, (StopAnimationAStep)baseStep);
		else if(baseStep is CallAnimationAStep) BattleAnimationTabs.CallAnimationAStep(index, battleAnimation, (CallAnimationAStep)baseStep);
		else if(baseStep is PlaySoundAStep) BattleAnimationTabs.PlaySoundAStep(index, battleAnimation, (PlaySoundAStep)baseStep);
		else if(baseStep is StopSoundAStep) BattleAnimationTabs.StopSoundAStep(index, battleAnimation, (StopSoundAStep)baseStep);
		else if(baseStep is CalculateAStep) BattleAnimationTabs.CalculateAStep(index, battleAnimation, (CalculateAStep)baseStep);
		else if(baseStep is DamageMultiplierAStep) BattleAnimationTabs.DamageMultiplierAStep(index, battleAnimation, (DamageMultiplierAStep)baseStep);
		else if(baseStep is ActivateDamageAStep) BattleAnimationTabs.ActivateDamageAStep(index, battleAnimation, (ActivateDamageAStep)baseStep);
		else if(baseStep is RestoreControlAStep) BattleAnimationTabs.RestoreControlAStep(index, battleAnimation, (RestoreControlAStep)baseStep);
		else if(baseStep is SetCamPosAStep) BattleAnimationTabs.SetCamPosAStep(index, battleAnimation, (SetCamPosAStep)baseStep);
		else if(baseStep is FadeCamPosAStep) BattleAnimationTabs.FadeCamPosAStep(index, battleAnimation, (FadeCamPosAStep)baseStep);
		else if(baseStep is SetInitialCamPosAStep) BattleAnimationTabs.SetInitialCamPosAStep(index, battleAnimation, (SetInitialCamPosAStep)baseStep);
		else if(baseStep is FadeToInitialCamPosAStep) BattleAnimationTabs.FadeToInitialCamPosAStep(index, battleAnimation, (FadeToInitialCamPosAStep)baseStep);
		else if(baseStep is ShakeCameraAStep) BattleAnimationTabs.ShakeCameraAStep(index, battleAnimation, (ShakeCameraAStep)baseStep);
		else if(baseStep is RotateCamAroundAStep) BattleAnimationTabs.RotateCamAroundAStep(index, battleAnimation, (RotateCamAroundAStep)baseStep);
		else if(baseStep is MountCameraAStep) BattleAnimationTabs.MountCameraAStep(index, battleAnimation, (MountCameraAStep)baseStep);
		else if(baseStep is FadeObjectAStep) BattleAnimationTabs.FadeObjectAStep(index, battleAnimation, (FadeObjectAStep)baseStep);
		else if(baseStep is FadeCameraAStep) BattleAnimationTabs.FadeCameraAStep(index, battleAnimation, (FadeCameraAStep)baseStep);
		else if(baseStep is SetToPositionAStep) BattleAnimationTabs.SetToPositionAStep(index, battleAnimation, (SetToPositionAStep)baseStep);
		else if(baseStep is MoveToAStep) BattleAnimationTabs.MoveToAStep(index, battleAnimation, (MoveToAStep)baseStep);
		else if(baseStep is MoveToDirectionAStep) BattleAnimationTabs.MoveToDirectionAStep(index, battleAnimation, (MoveToDirectionAStep)baseStep);
		else if(baseStep is RotateToAStep) BattleAnimationTabs.RotateToAStep(index, battleAnimation, (RotateToAStep)baseStep);
		else if(baseStep is RotationAStep) BattleAnimationTabs.RotationAStep(index, battleAnimation, (RotationAStep)baseStep);
		else if(baseStep is LookAtAStep) BattleAnimationTabs.LookAtAStep(index, battleAnimation, (LookAtAStep)baseStep);
		else if(baseStep is SpawnPrefabAStep) BattleAnimationTabs.SpawnPrefabAStep(index, battleAnimation, (SpawnPrefabAStep)baseStep);
		else if(baseStep is DestroyPrefabAStep) BattleAnimationTabs.DestroyPrefabAStep(index, battleAnimation, (DestroyPrefabAStep)baseStep);
		else if(baseStep is SetVariableAStep) BattleAnimationTabs.SetVariableAStep(index, battleAnimation, (SetVariableAStep)baseStep);
		else if(baseStep is RemoveVariableAStep) BattleAnimationTabs.RemoveVariableAStep(index, battleAnimation, (RemoveVariableAStep)baseStep);
		else if(baseStep is CheckVariableAStep) BattleAnimationTabs.CheckVariableAStep(index, battleAnimation, (CheckVariableAStep)baseStep);
		else if(baseStep is SetNumberVariableAStep) BattleAnimationTabs.SetNumberVariableAStep(index, battleAnimation, (SetNumberVariableAStep)baseStep);
		else if(baseStep is RemoveNumberVariableAStep) BattleAnimationTabs.RemoveNumberVariableAStep(index, battleAnimation, (RemoveNumberVariableAStep)baseStep);
		else if(baseStep is CheckNumberVariableAStep) BattleAnimationTabs.CheckNumberVariableAStep(index, battleAnimation, (CheckNumberVariableAStep)baseStep);
		else if(baseStep is SendMessageAStep) BattleAnimationTabs.SendMessageAStep(index, battleAnimation, (SendMessageAStep)baseStep);
		else if(baseStep is BroadcastMessageAStep) BattleAnimationTabs.BroadcastMessageAStep(index, battleAnimation, (BroadcastMessageAStep)baseStep);
		else if(baseStep is AddComponentAStep) BattleAnimationTabs.AddComponentAStep(index, battleAnimation, (AddComponentAStep)baseStep);
		else if(baseStep is RemoveComponentAStep) BattleAnimationTabs.RemoveComponentAStep(index, battleAnimation, (RemoveComponentAStep)baseStep);
		else if(baseStep is AddItemAStep) BattleAnimationTabs.AddItemAStep(index, battleAnimation, (AddItemAStep)baseStep);
		else if(baseStep is RemoveItemAStep) BattleAnimationTabs.RemoveItemAStep(index, battleAnimation, (RemoveItemAStep)baseStep);
		else if(baseStep is CheckItemAStep) BattleAnimationTabs.CheckItemAStep(index, battleAnimation, (CheckItemAStep)baseStep);
		else if(baseStep is SetPlayerPrefsAStep) BattleAnimationTabs.SetPlayerPrefsAStep(index, battleAnimation, (SetPlayerPrefsAStep)baseStep);
		else if(baseStep is GetPlayerPrefsAStep) BattleAnimationTabs.GetPlayerPrefsAStep(index, battleAnimation, (GetPlayerPrefsAStep)baseStep);
		else if(baseStep is HasPlayerPrefsAStep) BattleAnimationTabs.HasPlayerPrefsAStep(index, battleAnimation, (HasPlayerPrefsAStep)baseStep);
		else if(baseStep is CustomStatisticAStep) BattleAnimationTabs.CustomStatisticAStep(index, battleAnimation, (CustomStatisticAStep)baseStep);
		else if(baseStep is ClearStatisticAStep) BattleAnimationTabs.ClearStatisticAStep(index, battleAnimation, (ClearStatisticAStep)baseStep);
		// last
		else if(baseStep is AnimationStep) BattleAnimationTabs.AnimationStep(index, battleAnimation, (AnimationStep)baseStep);
	}

	// baseStep steps
	public static void AnimationStep(int index, BattleAnimation battleAnimation, AnimationStep baseStep)
	{
		if(!battleAnimation.hideButtons)
		{
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Remove", GUILayout.Width(100)))
			{
				battleAnimation.RemoveStep(index);
				return;
			}
			if(index > 0)
			{
				if(GUILayout.Button("Move Up", GUILayout.Width(100)))
				{
					battleAnimation.MoveStepUp(index);
					return;
				}
			}
			if(index < battleAnimation.step.Length-1)
			{
				if(GUILayout.Button("Move Down", GUILayout.Width(100)))
				{
					battleAnimation.MoveStepDown(index);
					return;
				}
			}
			baseStep.stepEnabled = EditorGUILayout.Toggle("Step enabled", baseStep.stepEnabled, GUILayout.Width(200));
			GUILayout.FlexibleSpace();
			if(GUILayout.Button("Copy", GUILayout.Width(100)))
			{
				battleAnimation.InsertStep(baseStep.GetCopy(battleAnimation), index+1);
				return;
			}
			if(GUILayout.Button("Move To", GUILayout.Width(100)))
			{
				battleAnimation.MoveStepTo(baseStep.moveTo, index);
			}
			baseStep.moveTo = EditorGUILayout.IntField(baseStep.moveTo, GUILayout.Width(50));
			if(baseStep.moveTo < 0) baseStep.moveTo = 0;
			else if(baseStep.moveTo >= battleAnimation.step.Length) baseStep.moveTo = battleAnimation.step.Length-1;
			EditorGUILayout.EndHorizontal();
		}
		if(baseStep is RandomAStep) {}
		else
		{
			baseStep.next = EditorGUILayout.IntField("Next step", baseStep.next, GUILayout.Width(200));
		}
		EditorGUILayout.Separator();
	}
	
	public static void WaitAStep(int index, BattleAnimation battleAnimation, WaitAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Wait");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.time = EditorGUILayout.FloatField("Time (s)", baseStep.time, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void GoToAStep(int index, BattleAnimation battleAnimation, GoToAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Go To Step");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			EditorGUILayout.Separator();
		}
	}
	
	public static void RandomAStep(int index, BattleAnimation battleAnimation, RandomAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Random Step");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.min = EditorGUILayout.IntField("Minimum", baseStep.min, GUILayout.Width(mWidth));
			baseStep.max = EditorGUILayout.IntField("Maximum", baseStep.max, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void CheckRandomAStep(int index, BattleAnimation battleAnimation, CheckRandomAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Check Random");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.float1 = EditorGUILayout.FloatField("Value (0-100)", baseStep.float1, GUILayout.Width(mWidth));
			if(baseStep.float1 < 0) baseStep.float1 = 0;
			else if(baseStep.float1 > 100) baseStep.float1 = 100;
			baseStep.nextFail = EditorGUILayout.IntField("Next step fail", baseStep.nextFail, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void CheckFormulaAStep(int index, BattleAnimation battleAnimation, CheckFormulaAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Check Formula");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.formulaID = EditorGUILayout.Popup("Formula", baseStep.formulaID, DataHolder.Formulas().GetNameList(true),GUILayout.Width(mWidth));
			baseStep.characterID = EditorGUILayout.Popup("Character", baseStep.characterID, DataHolder.Characters().GetNameList(true),GUILayout.Width(mWidth));
			baseStep.nextFail = EditorGUILayout.IntField("Next step fail", baseStep.nextFail, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void WaitForButtonAStep(int index, BattleAnimation battleAnimation, WaitForButtonAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Wait for Button");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.time = EditorGUILayout.FloatField("Time (s)", baseStep.time, GUILayout.Width(mWidth));
			baseStep.key = EditorGUILayout.TextField("Button name", baseStep.key, GUILayout.Width(mWidth));
			baseStep.nextFail = EditorGUILayout.IntField("Next step fail", baseStep.nextFail, GUILayout.Width(mWidth));
			baseStep.show = EditorGUILayout.Toggle("Enemy auto fail", baseStep.show, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void CheckDifficultyAStep(int index, BattleAnimation battleAnimation, CheckDifficultyAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Check Difficulty");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.number = EditorGUILayout.Popup("Difficulty", baseStep.number, 
					DataHolder.Difficulties().GetNameList(true), GUILayout.Width(mWidth));
			baseStep.valueCheck = (ValueCheck)EditorGUILayout.EnumPopup("Check", 
					baseStep.valueCheck, GUILayout.Width(mWidth));
			baseStep.nextFail = EditorGUILayout.IntField("Next step fail", 
					baseStep.nextFail, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void CheckUserAStep(int index, BattleAnimation battleAnimation, CheckUserAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Check User");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.show = EditorGUILayout.Toggle("Character/Enemy", baseStep.show, GUILayout.Width(mWidth));
			baseStep.nextFail = EditorGUILayout.IntField("Next step fail", 
					baseStep.nextFail, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	// animation steps
	public static void PlayAnimationAStep(int index, BattleAnimation battleAnimation, PlayAnimationAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Play Animation");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.statusOrigin = (StatusOrigin)EditorGUILayout.EnumPopup("Animate", baseStep.statusOrigin, GUILayout.Width(mWidth));
			
			baseStep.show2 = EditorGUILayout.Toggle("Default animation", baseStep.show2, GUILayout.Width(mWidth));
			if(baseStep.show2)
			{
				baseStep.combatantAnimation = (CombatantAnimation)EditorGUILayout.EnumPopup("Animation", baseStep.combatantAnimation, GUILayout.Width(mWidth));
			}
			else
			{
				baseStep.value = EditorGUILayout.TextField("Animation name", baseStep.value, GUILayout.Width(mWidth));
				EditorGUILayout.BeginHorizontal();
				baseStep.show = EditorGUILayout.Toggle("Set layer", baseStep.show, GUILayout.Width(mWidth));
				if(baseStep.show)
				{
					baseStep.min = EditorGUILayout.IntField(baseStep.min, GUILayout.Width(mWidth2));
				}
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
			}
			baseStep.wait = EditorGUILayout.Toggle("Wait", baseStep.wait, GUILayout.Width(mWidth));
			
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
	
	public static void StopAnimationAStep(int index, BattleAnimation battleAnimation, StopAnimationAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Stop Animation");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.statusOrigin = (StatusOrigin)EditorGUILayout.EnumPopup("Animate", baseStep.statusOrigin, GUILayout.Width(mWidth));
			baseStep.show = EditorGUILayout.Toggle("Stop all", baseStep.show, GUILayout.Width(mWidth));
			if(!baseStep.show)
			{
				baseStep.show2 = EditorGUILayout.Toggle("Default animation", baseStep.show2, GUILayout.Width(mWidth));
				if(baseStep.show2)
				{
					baseStep.combatantAnimation = (CombatantAnimation)EditorGUILayout.EnumPopup("Animation", baseStep.combatantAnimation, GUILayout.Width(mWidth));
				}
				else
				{
					baseStep.value = EditorGUILayout.TextField("Animation name", baseStep.value, GUILayout.Width(mWidth));
				}
			}
			EditorGUILayout.Separator();
		}
	}
	
	public static void CallAnimationAStep(int index, BattleAnimation battleAnimation, CallAnimationAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Call Animation");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.number = EditorGUILayout.Popup("Animation", baseStep.number, 
					DataHolder.BattleAnimations().GetNameList(true), GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	// audio steps
	public static void PlaySoundAStep(int index, BattleAnimation battleAnimation, PlaySoundAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Play Sound");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.wait = EditorGUILayout.Toggle("Wait", baseStep.wait, GUILayout.Width(mWidth));
			baseStep.show4 = EditorGUILayout.Toggle("Use index clip", baseStep.show4, GUILayout.Width(mWidth));
			
			EditorGUILayout.BeginHorizontal();
			if(baseStep.show4)
			{
				baseStep.audioID = EditorGUILayout.IntField("Clip index", baseStep.audioID, GUILayout.Width(mWidth2));
				if(baseStep.audioID < 0) baseStep.audioID = 0;
				else if(baseStep.audioID >= 20) baseStep.audioID = 19;
				baseStep.statusOrigin2 = (StatusOrigin)EditorGUILayout.EnumPopup(baseStep.statusOrigin2, GUILayout.Width(mWidth2));
			}
			else baseStep.audioID = EditorGUILayout.Popup("Audio Clip", baseStep.audioID, 
							battleAnimation.GetAudioClipList(), GUILayout.Width(mWidth));
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
			if(baseStep.speed < 1) baseStep.speed = 1;
			
			baseStep.show3 = EditorGUILayout.Toggle("Loop", baseStep.show3, GUILayout.Width(mWidth));
			
			EditorGUILayout.Separator();
			baseStep.animationObject = (BattleAnimationObject)EditorGUILayout.EnumPopup("Play on", 
					baseStep.animationObject, GUILayout.Width(mWidth));
			if(BattleAnimationObject.PREFAB.Equals(baseStep.animationObject))
			{
				baseStep.prefabID2 = EditorGUILayout.IntField("Prefab ID", baseStep.prefabID2, GUILayout.Width(mWidth));
			}
			baseStep.pathToChild = EditorGUILayout.TextField("Path to child", baseStep.pathToChild, GUILayout.Width(mWidth*1.2f));
			EditorGUILayout.Separator();
		}
	}
	
	public static void StopSoundAStep(int index, BattleAnimation battleAnimation, StopSoundAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Stop Sound");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.show = EditorGUILayout.Toggle("Pause", baseStep.show, GUILayout.Width(mWidth));
			baseStep.animationObject = (BattleAnimationObject)EditorGUILayout.EnumPopup("Stop on", 
					baseStep.animationObject, GUILayout.Width(mWidth));
			if(BattleAnimationObject.PREFAB.Equals(baseStep.animationObject))
			{
				baseStep.prefabID2 = EditorGUILayout.IntField("Prefab ID", baseStep.prefabID2, GUILayout.Width(mWidth));
			}
			baseStep.pathToChild = EditorGUILayout.TextField("Path to child", baseStep.pathToChild, GUILayout.Width(mWidth*1.2f));
			EditorGUILayout.Separator();
		}
	}
	
	// battle system steps
	public static void CalculateAStep(int index, BattleAnimation battleAnimation, CalculateAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Calculate");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			
			EditorGUILayout.BeginHorizontal();
			baseStep.show2 = EditorGUILayout.Toggle("Damage factor", baseStep.show2, GUILayout.Width(mWidth));
			if(baseStep.show2)
			{
				baseStep.float1 = EditorGUILayout.FloatField(baseStep.float1, GUILayout.Width(mWidth2));
			}
			else baseStep.float1 = 1;
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
			
			baseStep.show = EditorGUILayout.Toggle("Auto animation", baseStep.show, GUILayout.Width(mWidth));
			if(baseStep.show)
			{
				baseStep.wait = EditorGUILayout.Toggle("Wait", baseStep.wait, GUILayout.Width(mWidth));
				
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
					baseStep.time = EditorGUILayout.FloatField("Fade length", baseStep.time, GUILayout.Width(mWidth));
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
			}
			EditorGUILayout.Separator();
		}
	}
	
	public static void DamageMultiplierAStep(int index, BattleAnimation battleAnimation, DamageMultiplierAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Damage Multiplier");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.float1 = EditorGUILayout.FloatField("Multiply by", baseStep.float1, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void ActivateDamageAStep(int index, BattleAnimation battleAnimation, ActivateDamageAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Activate Damage");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.show3 = EditorGUILayout.Toggle("On prefab", baseStep.show3, GUILayout.Width(mWidth));
			if(baseStep.show3)
			{
				baseStep.prefabID = EditorGUILayout.IntField("Prefab ID", baseStep.prefabID, GUILayout.Width(mWidth));
			}
			EditorGUILayout.Separator();
			
			baseStep.show = EditorGUILayout.Toggle("Active/Inactive", baseStep.show, GUILayout.Width(mWidth));
			if(baseStep.show)
			{
				EditorGUILayout.Separator();
				EditorGUILayout.BeginHorizontal();
				baseStep.show2 = EditorGUILayout.Toggle("Add audio", baseStep.show2, GUILayout.Width(mWidth));
				if(baseStep.show2)
				{
					baseStep.show4 = EditorGUILayout.Toggle("Use index clip", baseStep.show4, GUILayout.Width(mWidth));
					if(baseStep.show4)
					{
						baseStep.audioID = EditorGUILayout.IntField(baseStep.audioID, GUILayout.Width(mWidth2));
						if(baseStep.audioID < 0) baseStep.audioID = 0;
						else if(baseStep.audioID >= 20) baseStep.audioID = 19;
					}
					else baseStep.audioID = EditorGUILayout.Popup(baseStep.audioID, 
							battleAnimation.GetAudioClipList(), GUILayout.Width(mWidth2));
				}
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				if(baseStep.show2)
				{
					baseStep.volume = EditorGUILayout.FloatField("Volume", baseStep.volume, GUILayout.Width(mWidth));
				
					EditorGUILayout.BeginHorizontal();
					baseStep.float1 = EditorGUILayout.FloatField("Min. distance", baseStep.float1, GUILayout.Width(mWidth2));
					baseStep.float2 = EditorGUILayout.FloatField("Max. distance", baseStep.float2, GUILayout.Width(mWidth2));
					GUILayout.FlexibleSpace();
					EditorGUILayout.EndHorizontal();
					
					baseStep.audioRolloffMode = (AudioRolloffMode)EditorGUILayout.EnumPopup("Rolloff mode", baseStep.audioRolloffMode, GUILayout.Width(mWidth));
					baseStep.speed = EditorGUILayout.FloatField("Pitch", baseStep.speed, GUILayout.Width(mWidth));
					if(baseStep.speed < 1) baseStep.speed = 1;
				}
				EditorGUILayout.Separator();
				
				EditorGUILayout.BeginHorizontal();
				baseStep.show5 = EditorGUILayout.Toggle("Add prefab", baseStep.show5, GUILayout.Width(mWidth));
				if(baseStep.show5)
				{
					baseStep.prefabID2 = EditorGUILayout.Popup(baseStep.prefabID2, 
							battleAnimation.GetPrefabList(), GUILayout.Width(mWidth2));
					baseStep.float3 = EditorGUILayout.FloatField("Destroy after", baseStep.float3,
							GUILayout.Width(mWidth));
				}
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.Separator();
		}
	}
	
	public static void RestoreControlAStep(int index, BattleAnimation battleAnimation, RestoreControlAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Restore Control");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			EditorGUILayout.Separator();
		}
	}
	
	// camera steps
	public static void SetCamPosAStep(int index, BattleAnimation battleAnimation, SetCamPosAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Set Camera Position");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.animationObject = (BattleAnimationObject)EditorGUILayout.EnumPopup("Set on", 
					baseStep.animationObject, GUILayout.Width(mWidth));
			if(BattleAnimationObject.PREFAB.Equals(baseStep.animationObject))
			{
				baseStep.prefabID2 = EditorGUILayout.IntField("Prefab ID", baseStep.prefabID2, GUILayout.Width(mWidth));
			}
			baseStep.posID = EditorGUILayout.Popup("Camera position", baseStep.posID, DataHolder.CameraPositions().GetNameList(true), GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void FadeCamPosAStep(int index, BattleAnimation battleAnimation, FadeCamPosAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Fade Camera Position");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			EditorGUILayout.BeginHorizontal();
			baseStep.time = EditorGUILayout.FloatField("Time (s)", baseStep.time, GUILayout.Width(mWidth));
			if(baseStep.time <= 0) baseStep.time = 0.1f;
			baseStep.wait = EditorGUILayout.Toggle("Wait", baseStep.wait, GUILayout.Width(mWidth));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			baseStep.animationObject = (BattleAnimationObject)EditorGUILayout.EnumPopup("Fade to", 
					baseStep.animationObject, GUILayout.Width(mWidth));
			if(BattleAnimationObject.PREFAB.Equals(baseStep.animationObject))
			{
				baseStep.prefabID2 = EditorGUILayout.IntField("Prefab ID", baseStep.prefabID2, GUILayout.Width(mWidth));
			}
			baseStep.posID = EditorGUILayout.Popup("Camera position", baseStep.posID, DataHolder.CameraPositions().GetNameList(true), GUILayout.Width(mWidth));
			baseStep.interpolate = (EaseType)EditorGUILayout.EnumPopup("Interpolation", baseStep.interpolate, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void SetInitialCamPosAStep(int index, BattleAnimation battleAnimation, SetInitialCamPosAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Set Initial Camera Position");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			EditorGUILayout.Separator();
		}
	}
	
	public static void FadeToInitialCamPosAStep(int index, BattleAnimation battleAnimation, FadeToInitialCamPosAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Fade To Initial Camera Position");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
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
	
	public static void ShakeCameraAStep(int index, BattleAnimation battleAnimation, ShakeCameraAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Shake Camera");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
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
	
	public static void RotateCamAroundAStep(int index, BattleAnimation battleAnimation, RotateCamAroundAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Rotate Camera Around");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.animationObject = (BattleAnimationObject)EditorGUILayout.EnumPopup("Rotate around", 
					baseStep.animationObject, GUILayout.Width(mWidth));
			if(BattleAnimationObject.PREFAB.Equals(baseStep.animationObject))
			{
				baseStep.prefabID2 = EditorGUILayout.IntField("Prefab ID", baseStep.prefabID2, GUILayout.Width(mWidth));
			}
			baseStep.pathToChild = EditorGUILayout.TextField("Path to child", baseStep.pathToChild, GUILayout.Width(mWidth*1.2f));
			EditorGUILayout.Separator();
			
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
	
	public static void MountCameraAStep(int index, BattleAnimation battleAnimation, MountCameraAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Mount Camera");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.show = EditorGUILayout.Toggle("Mount/unmount", baseStep.show, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
			
			if(baseStep.show)
			{
				baseStep.animationObject = (BattleAnimationObject)EditorGUILayout.EnumPopup("Mount on", 
						baseStep.animationObject, GUILayout.Width(mWidth));
				if(BattleAnimationObject.PREFAB.Equals(baseStep.animationObject))
				{
					baseStep.prefabID2 = EditorGUILayout.IntField("Prefab ID", baseStep.prefabID2, GUILayout.Width(mWidth));
				}
				baseStep.pathToChild = EditorGUILayout.TextField("Path to child", baseStep.pathToChild, GUILayout.Width(mWidth*1.2f));
				EditorGUILayout.Separator();
				
				baseStep.show2 = EditorGUILayout.Toggle("Set to position", baseStep.show2, GUILayout.Width(mWidth));
				if(baseStep.show2)
				{
					baseStep.v3 = EditorGUILayout.Vector3Field("Offset", baseStep.v3, GUILayout.Width(mWidth));
					baseStep.show4 = EditorGUILayout.Toggle("Local space", baseStep.show4, GUILayout.Width(mWidth));
				}
				EditorGUILayout.Separator();
				
				baseStep.show3 = EditorGUILayout.Toggle("Use rotation", baseStep.show3, GUILayout.Width(mWidth));
				if(baseStep.show3)
				{
					baseStep.v3_2 = EditorGUILayout.Vector3Field("Rotation offset", baseStep.v3_2, GUILayout.Width(mWidth));
				}
				EditorGUILayout.Separator();
			}
		}
	}
	
	// fade steps
	public static void FadeObjectAStep(int index, BattleAnimation battleAnimation, FadeObjectAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Fade Object");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			EditorGUILayout.BeginHorizontal();
			baseStep.time = EditorGUILayout.FloatField("Time (s)", baseStep.time, GUILayout.Width(mWidth));
			if(baseStep.time <= 0) baseStep.time = 0.1f;
			baseStep.wait = EditorGUILayout.Toggle("Wait", baseStep.wait, GUILayout.Width(mWidth));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			baseStep.animationObject = (BattleAnimationObject)EditorGUILayout.EnumPopup("Object", 
					baseStep.animationObject, GUILayout.Width(mWidth));
			if(BattleAnimationObject.PREFAB.Equals(baseStep.animationObject))
			{
				baseStep.prefabID2 = EditorGUILayout.IntField("Prefab ID", baseStep.prefabID2, GUILayout.Width(mWidth));
			}
			baseStep.pathToChild = EditorGUILayout.TextField("Path to child", baseStep.pathToChild, GUILayout.Width(mWidth*1.2f));
			baseStep.show6 = EditorGUILayout.Toggle("Fade children", baseStep.show6, GUILayout.Width(mWidth));
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
	
	public static void FadeCameraAStep(int index, BattleAnimation battleAnimation, FadeCameraAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Fade Camera");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
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
	
	// move steps
	public static void SetToPositionAStep(int index, BattleAnimation battleAnimation, SetToPositionAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Set To Position");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.animationObject = (BattleAnimationObject)EditorGUILayout.EnumPopup("Object", 
					baseStep.animationObject, GUILayout.Width(mWidth));
			if(BattleAnimationObject.ARENA.Equals(baseStep.animationObject)) baseStep.animationObject = BattleAnimationObject.USER;
			if(BattleAnimationObject.PREFAB.Equals(baseStep.animationObject))
			{
				baseStep.prefabID2 = EditorGUILayout.IntField("Prefab ID", baseStep.prefabID2, GUILayout.Width(mWidth));
			}
			baseStep.moveToTarget = (BattleMoveToTarget)EditorGUILayout.EnumPopup("Set to", baseStep.moveToTarget, GUILayout.Width(mWidth));
			baseStep.pathToChild = EditorGUILayout.TextField("Path to child", baseStep.pathToChild, GUILayout.Width(mWidth*1.2f));
			baseStep.v3 = EditorGUILayout.Vector3Field("Offset", baseStep.v3, GUILayout.Width(mWidth));
			
			EditorGUILayout.Separator();
		}
	}
	
	public static void MoveToAStep(int index, BattleAnimation battleAnimation, MoveToAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Move To");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			
			baseStep.show3 = EditorGUILayout.Toggle("Move prefab", baseStep.show3, GUILayout.Width(mWidth));
			if(baseStep.show3)
			{
				baseStep.prefabID = EditorGUILayout.IntField("Prefab ID", baseStep.prefabID, GUILayout.Width(mWidth));
			}
			EditorGUILayout.Separator();
			
			baseStep.show5 = EditorGUILayout.Toggle("Move by speed", baseStep.show5, GUILayout.Width(mWidth));;
			if(baseStep.show5)
			{
				if(!baseStep.show3)
				{
					baseStep.show6 = EditorGUILayout.Toggle("Use battle speed", baseStep.show6, GUILayout.Width(mWidth));
				}
				else baseStep.show6 = false;
				if(!baseStep.show6)
				{
					baseStep.speed = EditorGUILayout.FloatField("Speed", baseStep.speed, GUILayout.Width(mWidth));
					if(baseStep.speed < 0) baseStep.speed = 0.1f;
				}
			}
			else
			{
				baseStep.time = EditorGUILayout.FloatField("Time (s)", baseStep.time, GUILayout.Width(mWidth));
				if(baseStep.time <= 0) baseStep.time = 0.1f;
			}
			baseStep.wait = EditorGUILayout.Toggle("Wait", baseStep.wait, GUILayout.Width(mWidth));
			
			EditorGUILayout.Separator();
			baseStep.show7 = EditorGUILayout.Toggle("Move to prefab", baseStep.show7, GUILayout.Width(mWidth));
			if(baseStep.show7)
			{
				baseStep.number = EditorGUILayout.IntField("Prefab ID", baseStep.number, GUILayout.Width(mWidth));
			}
			else
			{
				baseStep.moveToTarget = (BattleMoveToTarget)EditorGUILayout.EnumPopup("Move to", baseStep.moveToTarget, GUILayout.Width(mWidth));
			}
			
			baseStep.pathToChild = EditorGUILayout.TextField("Path to child", baseStep.pathToChild, GUILayout.Width(mWidth*1.2f));
			if(!baseStep.show5) baseStep.show = EditorGUILayout.Toggle("Face direction", baseStep.show, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
			
			EditorGUILayout.BeginHorizontal();
			baseStep.controller = EditorGUILayout.Toggle("Controller move", baseStep.controller, GUILayout.Width(mWidth));;
			if(baseStep.controller) baseStep.show2 = EditorGUILayout.Toggle("Apply gravity", baseStep.show2, GUILayout.Width(mWidth));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Separator();
			
			if(!baseStep.show5)
			{
				baseStep.interpolate = (EaseType)EditorGUILayout.EnumPopup("Interpolation", 
						baseStep.interpolate, GUILayout.Width(mWidth));
			}
			
			EditorGUILayout.BeginHorizontal();
			baseStep.show4 = EditorGUILayout.Toggle("Stop before position", baseStep.show4, GUILayout.Width(mWidth));
			if(baseStep.show4) baseStep.float1 = EditorGUILayout.FloatField(baseStep.float1, GUILayout.Width(mWidth2));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
		}
	}
	
	public static void MoveToDirectionAStep(int index, BattleAnimation battleAnimation, MoveToDirectionAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Move To Direction");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			
			baseStep.show3 = EditorGUILayout.Toggle("Move prefab", baseStep.show3, GUILayout.Width(mWidth));
			if(baseStep.show3)
			{
				baseStep.prefabID = EditorGUILayout.IntField("Prefab ID", baseStep.prefabID, GUILayout.Width(mWidth));
			}
			EditorGUILayout.Separator();
			
			EditorGUILayout.BeginHorizontal();
			baseStep.time = EditorGUILayout.FloatField("Time (s)", baseStep.time, GUILayout.Width(mWidth));
			if(baseStep.time <= 0) baseStep.time = 0.1f;
			baseStep.wait = EditorGUILayout.Toggle("Wait", baseStep.wait, GUILayout.Width(mWidth));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
			
			baseStep.show2 = EditorGUILayout.Toggle("Local space", baseStep.show2, GUILayout.Width(mWidth));
			baseStep.v3 = EditorGUILayout.Vector3Field("Direction", baseStep.v3, GUILayout.Width(mWidth));
			baseStep.show = EditorGUILayout.Toggle("Face direction", baseStep.show, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
			
			baseStep.speed = EditorGUILayout.FloatField("Speed", baseStep.speed, GUILayout.Width(mWidth));
			baseStep.controller = EditorGUILayout.Toggle("Controller move", baseStep.controller, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void RotateToAStep(int index, BattleAnimation battleAnimation, RotateToAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Rotate To");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			
			baseStep.show3 = EditorGUILayout.Toggle("Rotate prefab", baseStep.show3, GUILayout.Width(mWidth));
			if(baseStep.show3)
			{
				baseStep.prefabID = EditorGUILayout.IntField("Prefab ID", baseStep.prefabID, GUILayout.Width(mWidth));
			}
			EditorGUILayout.Separator();
			
			EditorGUILayout.BeginHorizontal();
			baseStep.time = EditorGUILayout.FloatField("Time (s)", baseStep.time, GUILayout.Width(mWidth));
			if(baseStep.time <= 0) baseStep.time = 0.1f;
			baseStep.wait = EditorGUILayout.Toggle("Wait", baseStep.wait, GUILayout.Width(mWidth));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			baseStep.moveToTarget = (BattleMoveToTarget)EditorGUILayout.EnumPopup("Rotate to", baseStep.moveToTarget, GUILayout.Width(mWidth));
			baseStep.pathToChild = EditorGUILayout.TextField("Path to child", baseStep.pathToChild, GUILayout.Width(mWidth*1.2f));
			EditorGUILayout.Separator();
			
			baseStep.interpolate = (EaseType)EditorGUILayout.EnumPopup("Interpolation", baseStep.interpolate, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void RotationAStep(int index, BattleAnimation battleAnimation, RotationAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Rotation");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			
			baseStep.show3 = EditorGUILayout.Toggle("Rotate prefab", baseStep.show3, GUILayout.Width(mWidth));
			if(baseStep.show3)
			{
				baseStep.prefabID = EditorGUILayout.IntField("Prefab ID", baseStep.prefabID, GUILayout.Width(mWidth));
			}
			EditorGUILayout.Separator();
			
			EditorGUILayout.BeginHorizontal();
			baseStep.time = EditorGUILayout.FloatField("Time (s)", baseStep.time, GUILayout.Width(mWidth));
			if(baseStep.time <= 0) baseStep.time = 0.1f;
			baseStep.wait = EditorGUILayout.Toggle("Wait", baseStep.wait, GUILayout.Width(mWidth));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
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
			EditorGUILayout.Separator();
			
			baseStep.show = EditorGUILayout.Toggle("Set axis", baseStep.show, GUILayout.Width(mWidth));
			if(baseStep.show)
			{
				baseStep.v3 = EditorGUILayout.Vector3Field("Rotation axis", baseStep.v3, GUILayout.Width(mWidth));
			}
			EditorGUILayout.Separator();
		}
	}
	
	public static void LookAtAStep(int index, BattleAnimation battleAnimation, LookAtAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Look At");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			
			baseStep.show3 = EditorGUILayout.Toggle("Use prefab", baseStep.show3, GUILayout.Width(mWidth));
			if(baseStep.show3)
			{
				baseStep.prefabID = EditorGUILayout.IntField("Prefab ID", baseStep.prefabID, GUILayout.Width(mWidth));
			}
			EditorGUILayout.Separator();
			
			baseStep.moveToTarget = (BattleMoveToTarget)EditorGUILayout.EnumPopup("Look at", baseStep.moveToTarget, GUILayout.Width(mWidth));
			baseStep.pathToChild = EditorGUILayout.TextField("Path to child", baseStep.pathToChild, GUILayout.Width(mWidth*1.2f));
			baseStep.show = EditorGUILayout.Toggle("Ignore y", baseStep.show, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	// spawn steps
	public static void SpawnPrefabAStep(int index, BattleAnimation battleAnimation, SpawnPrefabAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Spawn Prefab");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.prefabID = EditorGUILayout.Popup("Prefab", baseStep.prefabID, battleAnimation.GetPrefabList(), GUILayout.Width(mWidth));
			baseStep.number = EditorGUILayout.IntField("Prefab ID", baseStep.number, GUILayout.Width(mWidth));
			baseStep.animationObject = (BattleAnimationObject)EditorGUILayout.EnumPopup("On object", baseStep.animationObject, GUILayout.Width(mWidth));
			if(BattleAnimationObject.PREFAB.Equals(baseStep.animationObject))
			{
				baseStep.prefabID2 = EditorGUILayout.IntField("Prefab ID", baseStep.prefabID2, GUILayout.Width(mWidth));
			}
			baseStep.pathToChild = EditorGUILayout.TextField("Path to child", baseStep.pathToChild, GUILayout.Width(mWidth*1.2f));
			EditorGUILayout.Separator();
			
			baseStep.v3 = EditorGUILayout.Vector3Field("Spawn offset", baseStep.v3, GUILayout.Width(mWidth));
			baseStep.show3 = EditorGUILayout.Toggle("Align rotation", baseStep.show3, GUILayout.Width(mWidth));
			baseStep.show2 = EditorGUILayout.Toggle("Mount prefab", baseStep.show2, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void DestroyPrefabAStep(int index, BattleAnimation battleAnimation, DestroyPrefabAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Destroy Prefab");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.number = EditorGUILayout.IntField("Prefab ID", baseStep.number, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	// variable steps
	public static void SetVariableAStep(int index, BattleAnimation battleAnimation, SetVariableAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Set Variable");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.key = EditorGUILayout.TextField("Variable key", baseStep.key, GUILayout.Width(mWidth));
			baseStep.value = EditorGUILayout.TextField("Variable value", baseStep.value, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void RemoveVariableAStep(int index, BattleAnimation battleAnimation, RemoveVariableAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Remove Variable");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.key = EditorGUILayout.TextField("Variable key", baseStep.key, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void CheckVariableAStep(int index, BattleAnimation battleAnimation, CheckVariableAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Check Variable");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.key = EditorGUILayout.TextField("Variable key", baseStep.key, GUILayout.Width(mWidth));
			baseStep.value = EditorGUILayout.TextField("Variable value", baseStep.value, GUILayout.Width(mWidth));
			baseStep.nextFail = EditorGUILayout.IntField("Next step fail", baseStep.nextFail, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void SetNumberVariableAStep(int index, BattleAnimation battleAnimation, SetNumberVariableAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Set Number Variable");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.key = EditorGUILayout.TextField("Variable key", baseStep.key, GUILayout.Width(mWidth));
			baseStep.simpleOperator = (SimpleOperator)EditorGUILayout.EnumPopup("Operation", baseStep.simpleOperator, GUILayout.Width(mWidth));
			baseStep.float1 = EditorGUILayout.FloatField("Variable value", baseStep.float1, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void RemoveNumberVariableAStep(int index, BattleAnimation battleAnimation, RemoveNumberVariableAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Remove Number Variable");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.key = EditorGUILayout.TextField("Variable key", baseStep.key, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void CheckNumberVariableAStep(int index, BattleAnimation battleAnimation, CheckNumberVariableAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Check Number Variable");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.key = EditorGUILayout.TextField("Variable key", baseStep.key, GUILayout.Width(mWidth));
			baseStep.valueCheck = (ValueCheck)EditorGUILayout.EnumPopup("Check", baseStep.valueCheck, GUILayout.Width(mWidth));
			baseStep.float1 = EditorGUILayout.FloatField("Variable value", baseStep.float1, GUILayout.Width(mWidth));
			baseStep.nextFail = EditorGUILayout.IntField("Next step fail", baseStep.nextFail, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void SetPlayerPrefsAStep(int index, BattleAnimation battleAnimation, SetPlayerPrefsAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Set PlayerPrefs");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.show = EditorGUILayout.Toggle("Number variable", baseStep.show, GUILayout.Width(mWidth));
			baseStep.key = EditorGUILayout.TextField("Variable key", baseStep.key, GUILayout.Width(mWidth));
			baseStep.value = EditorGUILayout.TextField("PlayerPrefs key", baseStep.value, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void GetPlayerPrefsAStep(int index, BattleAnimation battleAnimation, GetPlayerPrefsAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Get PlayerPrefs");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.value = EditorGUILayout.TextField("PlayerPrefs key", baseStep.value, GUILayout.Width(mWidth));
			baseStep.show = EditorGUILayout.Toggle("Number variable", baseStep.show, GUILayout.Width(mWidth));
			baseStep.key = EditorGUILayout.TextField("Variable key", baseStep.key, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void HasPlayerPrefsAStep(int index, BattleAnimation battleAnimation, HasPlayerPrefsAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Has PlayerPrefs");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.key = EditorGUILayout.TextField("PlayerPrefs key", baseStep.key, GUILayout.Width(mWidth));
			baseStep.nextFail = EditorGUILayout.IntField("Next step fail", baseStep.nextFail, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	// function steps
	public static void SendMessageAStep(int index, BattleAnimation battleAnimation, SendMessageAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Send Message");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.animationObject = (BattleAnimationObject)EditorGUILayout.EnumPopup("Object", 
					baseStep.animationObject, GUILayout.Width(mWidth));
			if(BattleAnimationObject.PREFAB.Equals(baseStep.animationObject))
			{
				baseStep.prefabID2 = EditorGUILayout.IntField("Prefab ID", baseStep.prefabID2, GUILayout.Width(mWidth));
			}
			baseStep.pathToChild = EditorGUILayout.TextField("Path to child", baseStep.pathToChild, GUILayout.Width(mWidth*1.2f));
			EditorGUILayout.Separator();
			
			baseStep.key = EditorGUILayout.TextField("Function", baseStep.key, GUILayout.Width(mWidth));
			baseStep.value = EditorGUILayout.TextField("Value", baseStep.value, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void BroadcastMessageAStep(int index, BattleAnimation battleAnimation, BroadcastMessageAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Broadcast Message");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.animationObject = (BattleAnimationObject)EditorGUILayout.EnumPopup("Object", 
					baseStep.animationObject, GUILayout.Width(mWidth));
			if(BattleAnimationObject.PREFAB.Equals(baseStep.animationObject))
			{
				baseStep.prefabID2 = EditorGUILayout.IntField("Prefab ID", baseStep.prefabID2, GUILayout.Width(mWidth));
			}
			baseStep.key = EditorGUILayout.TextField("Function", baseStep.key, GUILayout.Width(mWidth));
			baseStep.value = EditorGUILayout.TextField("Value", baseStep.value, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void AddComponentAStep(int index, BattleAnimation battleAnimation, AddComponentAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Add Component");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.animationObject = (BattleAnimationObject)EditorGUILayout.EnumPopup("Object", 
					baseStep.animationObject, GUILayout.Width(mWidth));
			if(BattleAnimationObject.PREFAB.Equals(baseStep.animationObject))
			{
				baseStep.prefabID2 = EditorGUILayout.IntField("Prefab ID", baseStep.prefabID2, GUILayout.Width(mWidth));
			}
			baseStep.pathToChild = EditorGUILayout.TextField("Path to child", baseStep.pathToChild, GUILayout.Width(mWidth*1.2f));
			EditorGUILayout.Separator();
			
			baseStep.key = EditorGUILayout.TextField("Component", baseStep.key, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void RemoveComponentAStep(int index, BattleAnimation battleAnimation, RemoveComponentAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Remove Component");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.animationObject = (BattleAnimationObject)EditorGUILayout.EnumPopup("Object", 
					baseStep.animationObject, GUILayout.Width(mWidth));
			if(BattleAnimationObject.PREFAB.Equals(baseStep.animationObject))
			{
				baseStep.prefabID2 = EditorGUILayout.IntField("Prefab ID", baseStep.prefabID2, GUILayout.Width(mWidth));
			}
			baseStep.pathToChild = EditorGUILayout.TextField("Path to child", baseStep.pathToChild, GUILayout.Width(mWidth*1.2f));
			EditorGUILayout.Separator();
			
			baseStep.key = EditorGUILayout.TextField("Component", baseStep.key, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	// item steps
	public static void AddItemAStep(int index, BattleAnimation battleAnimation, AddItemAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Add Item");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.itemID = EditorGUILayout.Popup("Item", baseStep.itemID, DataHolder.Items().GetNameList(true), GUILayout.Width(mWidth));
			baseStep.number = EditorGUILayout.IntField("Number", baseStep.number, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void RemoveItemAStep(int index, BattleAnimation battleAnimation, RemoveItemAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Remove Item");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.itemID = EditorGUILayout.Popup("Item", baseStep.itemID, DataHolder.Items().GetNameList(true), GUILayout.Width(mWidth));
			baseStep.number = EditorGUILayout.IntField("Number", baseStep.number, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void CheckItemAStep(int index, BattleAnimation battleAnimation, CheckItemAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Check Item");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.itemID = EditorGUILayout.Popup("Item", baseStep.itemID, DataHolder.Items().GetNameList(true), GUILayout.Width(mWidth));
			baseStep.number = EditorGUILayout.IntField("Number", baseStep.number, GUILayout.Width(mWidth));
			baseStep.nextFail = EditorGUILayout.IntField("Next step fail", baseStep.nextFail, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	// statistic steps
	public static void CustomStatisticAStep(int index, BattleAnimation battleAnimation, CustomStatisticAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Custom Statistic");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			baseStep.number = EditorGUILayout.IntField("Index", 
					baseStep.number, GUILayout.Width(mWidth));
			baseStep.itemID = EditorGUILayout.IntField("Add", 
					baseStep.itemID, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
		}
	}
	
	public static void ClearStatisticAStep(int index, BattleAnimation battleAnimation, ClearStatisticAStep baseStep)
	{
		baseStep.fold = EditorGUILayout.Foldout(baseStep.fold, "Step "+index+": Clear Statistic");
		if(baseStep.fold)
		{
			BattleAnimationTabs.AnimationStep(index, battleAnimation, baseStep);
			EditorGUILayout.Separator();
		}
	}
}