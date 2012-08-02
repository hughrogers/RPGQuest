
using System.Collections;
using UnityEditor;
using UnityEngine;

public class EditorHelper
{
	public static float mWidth = 300;
	
	/*
	============================================================================
	Base functions
	============================================================================
	*/
	public static string[] CheckLanguageCount(string[] text)
	{
		return EditorHelper.CheckLanguageCount(text, DataHolder.Languages().GetDataCount());
	}
	
	public static string[] CheckLanguageCount(string[] text, int count)
	{
		if(count != text.Length)
		{
			string[] dmy = text;
			text = new string[count];
			for(int i=0; i<count; i++)
			{
				if(i<dmy.Length)
				{
					text[i] = dmy[i];
				}
				if(text[i] == null)
				{
					text[i] = "";
				}
			}
		}
		return text;
	}
	
	public static void TextIconSettings(ref string[] data, string title, ref string iconName, ref Texture2D icon, string iconPath)
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical();
		EditorHelper.TextSettings(ref data);
		EditorGUILayout.EndVertical();
		EditorHelper.IconSettings(title, ref iconName, ref icon, iconPath);
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
	}
	
	public static void TextSettings(ref string[] data)
	{
		for(int i=0; i<data.Length; i++)
		{
			if(data[i] == null) data[i] = "";
			data[i] = EditorGUILayout.TextField(DataHolder.Languages().GetName(i), 
					data[i], GUILayout.Width(mWidth*2));
		}
	}
	
	public static void IconSettings(string title, ref string iconName, ref Texture2D icon, string iconPath)
	{
		if(icon == null && iconName != null && "" != iconName)
		{
			icon = (Texture2D)Resources.Load(iconPath+iconName, typeof(Texture2D));
		}
		icon = (Texture2D)EditorGUILayout.ObjectField(title, icon, typeof(Texture2D), false);
		if(icon)
		{
			iconName = icon.name;
			icon = (Texture2D)Resources.Load(iconPath+iconName, typeof(Texture2D));
		}
		else iconName = "";
	}
	
	public static void AudioSettings(string title, ref string clipName, ref AudioClip clip, string audioPath)
	{
		if(clip == null && clipName != null && "" != clipName)
		{
			clip = (AudioClip)Resources.Load(audioPath+clipName, typeof(AudioClip));
		}
		clip = (AudioClip)EditorGUILayout.ObjectField(title, clip, typeof(AudioClip), false);
		if(clip)
		{
			clipName = clip.name;
			clip = (AudioClip)Resources.Load(audioPath+clipName, typeof(AudioClip));
		}
		else clipName = "";
	}
	
	public static void PrefabSettings(string title, ref string prefabName, ref GameObject obj, string prefabPath)
	{
		if(obj == null && prefabName != null && "" != prefabName)
		{
			obj = (GameObject)Resources.Load(prefabPath+prefabName, typeof(GameObject));
		}
		obj = (GameObject)EditorGUILayout.ObjectField(title, obj, typeof(GameObject), false);
		if(obj)
		{
			prefabName = obj.name;
			obj = (GameObject)Resources.Load(prefabPath+prefabName, typeof(GameObject));
		}
		else prefabName = "";
	}
	
	/*
	============================================================================
	Settings functions
	============================================================================
	*/
	public static ValueChange ValueChangeSettings(int index, ValueChange vc)
	{
		vc.active = EditorGUILayout.BeginToggleGroup(DataHolder.StatusValues().GetName(index), vc.active);
		if(vc.active)
		{
			vc.simpleOperator = (SimpleOperator)EditorTab.EnumToolbar("Operator", (int)vc.simpleOperator, typeof(SimpleOperator));
			vc.formulaChooser = (FormulaChooser)EditorTab.EnumToolbar("", (int)vc.formulaChooser, 
					typeof(FormulaChooser), (int)(EditorHelper.mWidth*1.5f));
			if(FormulaChooser.VALUE.Equals(vc.formulaChooser))
			{
				vc.value = EditorGUILayout.IntField("Value", vc.value, GUILayout.Width(EditorHelper.mWidth));
			}
			else if(FormulaChooser.STATUS.Equals(vc.formulaChooser))
			{
				vc.status = EditorGUILayout.Popup("Status Value", vc.status, 
						DataHolder.StatusValues().GetNameList(true), GUILayout.Width(EditorHelper.mWidth));
			}
			else if(FormulaChooser.FORMULA.Equals(vc.formulaChooser))
			{
				vc.formula = EditorGUILayout.Popup("Formula", vc.formula, 
						DataHolder.Formulas().GetNameList(true), GUILayout.Width(EditorHelper.mWidth));
			}
			else if(FormulaChooser.RANDOM.Equals(vc.formulaChooser))
			{
				vc.randomMin = EditorGUILayout.IntField("Minimum", vc.randomMin, GUILayout.Width(EditorHelper.mWidth));
				vc.randomMax= EditorGUILayout.IntField("Maximum", vc.randomMax, GUILayout.Width(EditorHelper.mWidth));
			}
			vc.efficiency = EditorGUILayout.FloatField("Efficiency", vc.efficiency, GUILayout.Width(EditorHelper.mWidth));
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			vc.cancelSkills = EditorGUILayout.Toggle("Cancel skills", vc.cancelSkills, GUILayout.Width(EditorHelper.mWidth));
			vc.blockable = EditorGUILayout.Toggle("Blockable", vc.blockable, GUILayout.Width(EditorHelper.mWidth));
			vc.ignoreDefend = EditorGUILayout.Toggle("Ignore defend", vc.ignoreDefend, GUILayout.Width(EditorHelper.mWidth));
			EditorGUILayout.EndVertical();
			EditorGUILayout.Separator();
			EditorGUILayout.BeginVertical();
			vc.ignoreElement = EditorGUILayout.Toggle("Ignore element", vc.ignoreElement, GUILayout.Width(EditorHelper.mWidth));
			vc.ignoreRace = EditorGUILayout.Toggle("Ignore race", vc.ignoreRace, GUILayout.Width(EditorHelper.mWidth));
			vc.ignoreSize = EditorGUILayout.Toggle("Ignore size", vc.ignoreSize, GUILayout.Width(EditorHelper.mWidth));
			EditorGUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.EndToggleGroup();
		EditorGUILayout.Separator();
		return vc;
	}
	
	public static StatusBar StatusBarSettings(StatusBar bar)
	{
		EditorGUILayout.BeginVertical();
		
		bar.bounds = EditorGUILayout.RectField("Bounds", 
							bar.bounds, GUILayout.Width(EditorHelper.mWidth));
		
		bar.statusID = EditorGUILayout.Popup("Status value", bar.statusID,
				DataHolder.StatusValues().GetNameList(true), GUILayout.Width(EditorHelper.mWidth));
		
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		bar.useImage = EditorGUILayout.Toggle("Use image", bar.useImage, GUILayout.Width(EditorHelper.mWidth));
		if(bar.useImage)
		{
			bar.showEmpty = EditorGUILayout.Toggle("Empty image", bar.showEmpty, GUILayout.Width(EditorHelper.mWidth));
			EditorGUILayout.EndHorizontal();
			
			if(bar.texture == null &&
				bar.imageName != null &&
				"" != bar.imageName)
			{
				bar.texture = (Texture2D)Resources.Load(DataHolder.HUDs().resourcePath+
						bar.imageName, typeof(Texture2D));
			}
			EditorGUILayout.BeginHorizontal();
			bar.texture = (Texture2D)EditorGUILayout.ObjectField("Image", bar.texture, typeof(Texture2D), false);
			if(bar.texture)
			{
				bar.imageName = bar.texture.name;
			}
			else bar.imageName = "";
			
			if(bar.showEmpty)
			{
				if(bar.emptyTexture == null &&
					bar.emptyImageName != null &&
					"" != bar.emptyImageName)
				{
					bar.emptyTexture = (Texture2D)Resources.Load(DataHolder.HUDs().resourcePath+
							bar.emptyImageName, typeof(Texture2D));
				}
				bar.emptyTexture = (Texture2D)EditorGUILayout.ObjectField("Empty", bar.emptyTexture, typeof(Texture2D), false);
				if(bar.emptyTexture)
				{
					bar.emptyImageName = bar.emptyTexture.name;
				}
				else bar.emptyImageName = "";
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			bar.scaleMode = (ScaleMode)EditorGUILayout.EnumPopup("Scale mode", 
					bar.scaleMode, GUILayout.Width(EditorHelper.mWidth));
			bar.alphaBlend = EditorGUILayout.Toggle("Alpha blend", 
					bar.alphaBlend, GUILayout.Width(EditorHelper.mWidth));
			bar.imageAspect = EditorGUILayout.FloatField("Image aspect", 
					bar.imageAspect, GUILayout.Width(EditorHelper.mWidth));
		}
		else
		{
			bar.showEmpty = EditorGUILayout.Toggle("Empty color", bar.showEmpty, GUILayout.Width(EditorHelper.mWidth));
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			bar.barColor = EditorGUILayout.Popup("Bar color", bar.barColor, 
					DataHolder.Colors().GetNameList(true), GUILayout.Width(EditorHelper.mWidth));
			if(bar.showEmpty)
			{
				bar.emptyColor = EditorGUILayout.Popup("Empty", bar.emptyColor, 
						DataHolder.Colors().GetNameList(true), GUILayout.Width(EditorHelper.mWidth));
			}
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.Separator();
		EditorGUILayout.EndVertical();
		
		return bar;
	}
	
	public static VariableCondition VariableConditionSettings(VariableCondition vars)
	{
		vars.needed = (AIConditionNeeded)EditorGUILayout.EnumPopup("Needed", 
				vars.needed, GUILayout.Width(mWidth));
		
		if(GUILayout.Button("Add Variable", GUILayout.Width(mWidth)))
		{
			vars.AddVariable();
		}
		for(int i=0; i<vars.variableKey.Length; i++)
		{
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Remove", GUILayout.Width(mWidth*0.5f)))
			{
				vars.RemoveVariable(i);
				break;
			}
			vars.checkType[i] = EditorGUILayout.Toggle(
					vars.checkType[i], GUILayout.Width(20));
			vars.variableKey[i] = EditorGUILayout.TextField(
					vars.variableKey[i], GUILayout.Width(mWidth*0.5f));
			
			if(vars.checkType[i]) GUILayout.Label("== ");
			else GUILayout.Label(" != ");
			
			vars.variableValue[i] = EditorGUILayout.TextField(
					vars.variableValue[i], GUILayout.Width(mWidth*0.5f));
			
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.Separator();
		
		if(GUILayout.Button("Add Number Variable", GUILayout.Width(mWidth)))
		{
			vars.AddNumberVariable();
		}
		for(int i=0; i<vars.numberVarKey.Length; i++)
		{
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Remove", GUILayout.Width(mWidth*0.5f)))
			{
				vars.RemoveNumberVariable(i);
				break;
			}
			
			vars.numberCheckType[i] = EditorGUILayout.Toggle(
					vars.numberCheckType[i], GUILayout.Width(20));
			vars.numberVarKey[i] = EditorGUILayout.TextField(
					vars.numberVarKey[i], GUILayout.Width(mWidth*0.5f));
			if(!vars.numberCheckType[i]) GUILayout.Label("not");
			vars.numberValueCheck[i] = (ValueCheck)EditorGUILayout.EnumPopup(
					vars.numberValueCheck[i], GUILayout.Width(mWidth*0.4f));
			vars.numberVarValue[i] = EditorGUILayout.FloatField(
					vars.numberVarValue[i], GUILayout.Width(mWidth*0.5f));
			
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.Separator();
		return vars;
	}
	
	public static StatusRequirement StatusRequirementSettings(StatusRequirement req)
	{
		req.statusNeeded = (StatusNeeded)EditorTab.EnumToolbar("Needed", (int)req.statusNeeded, typeof(StatusNeeded), 600);
		if(StatusNeeded.STATUS_VALUE.Equals(req.statusNeeded))
		{
			req.statID = EditorGUILayout.Popup("Status value", req.statID, DataHolder.StatusValues().GetNameList(true), GUILayout.Width(mWidth));
			EditorGUILayout.BeginHorizontal();
			req.comparison = (ValueCheck)EditorTab.EnumToolbar("Comparison", (int)req.comparison, typeof(ValueCheck), 300);
			req.value = EditorGUILayout.IntField(req.value, GUILayout.Width(mWidth*0.5f));
			req.setter = (ValueSetter)EditorTab.EnumToolbar("", (int)req.setter, typeof(ValueSetter));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
		}
		else if(StatusNeeded.ELEMENT.Equals(req.statusNeeded))
		{
			req.statID = EditorGUILayout.Popup("Element", req.statID, DataHolder.Elements().GetNameList(true), GUILayout.Width(mWidth));
			EditorGUILayout.BeginHorizontal();
			req.comparison = (ValueCheck)EditorTab.EnumToolbar("Comparison", (int)req.comparison, typeof(ValueCheck), 300);
			req.value = EditorGUILayout.IntField(req.value, GUILayout.Width(mWidth*0.5f));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
		}
		else if(StatusNeeded.SKILL.Equals(req.statusNeeded))
		{
			req.statID = EditorGUILayout.Popup("Skill", req.statID, DataHolder.Skills().GetNameList(true), GUILayout.Width(mWidth));
		}
		else if(StatusNeeded.RACE.Equals(req.statusNeeded))
		{
			req.statID = EditorGUILayout.Popup("Race", req.statID, DataHolder.Races().GetNameList(true), GUILayout.Width(mWidth));
		}
		else if(StatusNeeded.SIZE.Equals(req.statusNeeded))
		{
			req.statID = EditorGUILayout.Popup("Size", req.statID, DataHolder.Sizes().GetNameList(true), GUILayout.Width(mWidth));
		}
		else if(StatusNeeded.LEVEL.Equals(req.statusNeeded))
		{
			req.classLevel = EditorGUILayout.Toggle("Class level", req.classLevel, GUILayout.Width(mWidth));
			req.value = EditorGUILayout.IntField("Level", req.value, GUILayout.Width(mWidth));
			req.comparison = (ValueCheck)EditorTab.EnumToolbar("Comparison", (int)req.comparison, typeof(ValueCheck), 300);
		}
		EditorGUILayout.Separator();
		return req;
	}
	
	public static EffectPrefab EffectPrefabSettings(EffectPrefab pref)
	{
		if(pref.prefabInstance == null && "" != pref.prefabName)
		{
			pref.prefabInstance = (GameObject)Resources.Load(pref.prefabPath+pref.prefabName, typeof(GameObject));
		}
		pref.prefabInstance = (GameObject)EditorGUILayout.ObjectField("Prefab", 
				pref.prefabInstance, typeof(GameObject), false, GUILayout.Width(mWidth*2));
		if(pref.prefabInstance) pref.prefabName = pref.prefabInstance.name;
		else pref.prefabName = "";
		GUILayout.Label("Path/to/child", EditorStyles.boldLabel);
		pref.childName = EditorGUILayout.TextField("Target child", pref.childName, GUILayout.Width(mWidth*2));
		pref.localSpace = EditorGUILayout.Toggle("Local space", pref.localSpace, GUILayout.Width(mWidth));
		pref.offset = EditorGUILayout.Vector3Field("Offset", pref.offset, GUILayout.Width(mWidth));
		pref.targetRotation = EditorGUILayout.Toggle("Use target rotation", pref.targetRotation, GUILayout.Width(mWidth));
		if(pref.targetRotation)
		{
			pref.rotationOffset = EditorGUILayout.Vector3Field("Rotation offset", pref.rotationOffset, GUILayout.Width(mWidth));
		}
		EditorGUILayout.Separator();
		return pref;
	}
	
	public static AutoAttack AutoAttackSettings(AutoAttack aa)
	{
		aa.active = EditorGUILayout.Toggle("Active", aa.active, GUILayout.Width(mWidth));
		if(aa.active)
		{
			aa.interval = EditorGUILayout.FloatField("Time interval", aa.interval, GUILayout.Width(mWidth));
			if(aa.interval < 0) aa.interval = 0;
			aa.useSkill = EditorGUILayout.Toggle("Use skills", aa.useSkill, GUILayout.Width(mWidth));
			if(aa.useSkill)
			{
				aa.skillID = EditorGUILayout.Popup("Skill", aa.skillID, DataHolder.Skills().GetNameList(true), GUILayout.Width(mWidth));
				aa.skillLevel = EditorGUILayout.IntField("Skill level", aa.skillLevel, GUILayout.Width(mWidth));
				aa.skillLevel = BaseTab.MinMaxCheck(aa.skillLevel, 1, DataHolder.Skill(aa.skillID).level.Length);
			}
		}
		return aa;
	}
	
	public static UseRange UseRangeSettings(UseRange ur)
	{
		if(DataHolder.BattleSystem().IsRealTime())
		{
			EditorGUILayout.Separator();
			
			ur.active = EditorGUILayout.Toggle("Use range", ur.active, GUILayout.Width(mWidth));
			if(ur.active)
			{
				ur.range = EditorGUILayout.FloatField(ur.range, GUILayout.Width(mWidth*0.5f));
				ur.ignoreYDistance = EditorGUILayout.Toggle("Ignore Y distance", ur.ignoreYDistance, GUILayout.Width(mWidth));
			}
		}
		else ur.active = false;
		return ur;
	}
	
	public static PlayerControlSettings PlayerControlSettings(PlayerControlSettings pc)
	{
		pc.type = (PlayerControlType)EditorTab.EnumToolbar("Type", (int)pc.type, typeof(PlayerControlType), 300);
		if(pc.IsDefaultControl())
		{
			pc.moveDead = EditorGUILayout.Toggle("Move dead", pc.moveDead, GUILayout.Width(mWidth));
			pc.useCharacterSpeed = EditorGUILayout.Toggle("Use char. speed", pc.useCharacterSpeed, GUILayout.Width(mWidth));
			if(!pc.useCharacterSpeed)
			{
				pc.runSpeed = EditorGUILayout.FloatField("Run speed", pc.runSpeed, GUILayout.Width(mWidth));
			}
			pc.gravity = EditorGUILayout.FloatField("Gravity", pc.gravity, GUILayout.Width(mWidth));
			pc.speedSmoothing = EditorGUILayout.FloatField("Speed smoothing", pc.speedSmoothing, GUILayout.Width(mWidth));
			pc.rotateSpeed = EditorGUILayout.FloatField("Rotate speed", pc.rotateSpeed, GUILayout.Width(mWidth));
			
			pc.firstPerson = EditorGUILayout.Toggle("First person", pc.firstPerson, GUILayout.Width(mWidth));
			if(!pc.firstPerson)
			{
				pc.useCamDirection = EditorGUILayout.Toggle("Use cam direction", pc.useCamDirection, GUILayout.Width(mWidth));
			}
			pc.verticalAxis = EditorGUILayout.TextField("Vertical axis key", pc.verticalAxis, GUILayout.Width(mWidth*1.2f));
			pc.horizontalAxis = EditorGUILayout.TextField("Horizontal axis key", pc.horizontalAxis, GUILayout.Width(mWidth*1.2f));
			EditorGUILayout.Separator();
			
			GUILayout.Label("Jump settings", EditorStyles.boldLabel);
			pc.useJump = EditorGUILayout.Toggle("Use jump", pc.useJump, GUILayout.Width(mWidth));
			if(pc.useJump)
			{
				pc.jumpKey = EditorGUILayout.TextField("Jump key", pc.jumpKey, GUILayout.Width(mWidth*1.2f));
				pc.jumpDuration = EditorGUILayout.FloatField("Jump duration", pc.jumpDuration, GUILayout.Width(mWidth));
				pc.jumpSpeed = EditorGUILayout.FloatField("Jump speed", pc.jumpSpeed, GUILayout.Width(mWidth));
				pc.jumpInterpolation = (EaseType)EditorGUILayout.EnumPopup("Jump interpolation", 
						pc.jumpInterpolation, GUILayout.Width(mWidth));
				pc.inAirModifier = EditorGUILayout.FloatField("In air modifier", pc.inAirModifier, GUILayout.Width(mWidth));
				pc.jumpMaxGroundAngle = EditorGUILayout.FloatField("Max ground angle", pc.jumpMaxGroundAngle, GUILayout.Width(mWidth));
				FloatHelper.Limit(ref pc.jumpMaxGroundAngle, 0, 90);
			}
			EditorGUILayout.Separator();
			
			GUILayout.Label("Sprint settings", EditorStyles.boldLabel);
			pc.useSprint = EditorGUILayout.Toggle("Use sprint",
					pc.useSprint, GUILayout.Width(mWidth));
			if(pc.useSprint)
			{
				pc.sprintKey = EditorGUILayout.TextField("Sprint key", pc.sprintKey, GUILayout.Width(mWidth*1.2f));
				pc.sprintFactor = EditorGUILayout.FloatField("Sprint factor", pc.sprintFactor, GUILayout.Width(mWidth));
				EditorGUILayout.Separator();
				
				pc.useEnergy = EditorGUILayout.Toggle("Use energy", pc.useEnergy, GUILayout.Width(mWidth));
				if(pc.useEnergy)
				{
					pc.maxEFormula = EditorGUILayout.Toggle("Max formula", pc.maxEFormula, GUILayout.Width(mWidth));
					if(pc.maxEFormula)
					{
						pc.meFormula = EditorGUILayout.Popup("Max energy", pc.meFormula, 
								DataHolder.Formulas().GetNameList(true), GUILayout.Width(mWidth));
					}
					else
					{
						pc.maxEnergy = EditorGUILayout.FloatField("Max energy", pc.maxEnergy, GUILayout.Width(mWidth));
					}
					pc.energyCFormula = EditorGUILayout.Toggle("Consume formula", pc.energyCFormula, GUILayout.Width(mWidth));
					if(pc.energyCFormula)
					{
						pc.ecFormula = EditorGUILayout.Popup("Energy consume (s)", pc.ecFormula, 
								DataHolder.Formulas().GetNameList(true), GUILayout.Width(mWidth));
					}
					else
					{
						pc.energyConsume = EditorGUILayout.FloatField("Energy consume (s)", pc.energyConsume, GUILayout.Width(mWidth));
					}
					pc.energyRFormula = EditorGUILayout.Toggle("Regeneration formula", pc.energyRFormula, GUILayout.Width(mWidth));
					if(pc.energyRFormula)
					{
						pc.erFormula = EditorGUILayout.Popup("Energy regen. (s)", pc.erFormula, 
								DataHolder.Formulas().GetNameList(true), GUILayout.Width(mWidth));
					}
					else
					{
						pc.energyRegeneration = EditorGUILayout.FloatField("Energy regen. (s)", pc.energyRegeneration, GUILayout.Width(mWidth));
					}
				}
			}
		}
		else if(pc.IsMobileControl())
		{
			pc.mouseTouch = EditorHelper.MouseTouchControlSettings(pc.mouseTouch, true);
			pc.moveDead = EditorGUILayout.Toggle("Move dead", pc.moveDead, GUILayout.Width(mWidth));
			pc.useCharacterSpeed = EditorGUILayout.Toggle("Use char. speed",
					pc.useCharacterSpeed, GUILayout.Width(mWidth));
			if(!pc.useCharacterSpeed)
			{
				pc.runSpeed = EditorGUILayout.FloatField("Run speed", pc.runSpeed, GUILayout.Width(mWidth));
			}
			pc.gravity = EditorGUILayout.FloatField("Gravity", pc.gravity, GUILayout.Width(mWidth));
			pc.speedSmoothing = EditorGUILayout.FloatField("Speed smoothing", pc.speedSmoothing, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
			
			pc.raycastDistance = EditorGUILayout.FloatField("Raycast distance", pc.raycastDistance, GUILayout.Width(mWidth));
			pc.layerMask = EditorGUILayout.LayerField("Layer", pc.layerMask, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
			
			if(pc.cursorObject == null && "" != pc.cursorObjectName)
			{
				pc.cursorObject = (GameObject)Resources.Load(
						BattleSystemData.PREFAB_PATH+
						pc.cursorObjectName, typeof(GameObject));
			}
			pc.cursorObject = (GameObject)EditorGUILayout.ObjectField("Cursor prefab", 
					pc.cursorObject, typeof(GameObject), false, GUILayout.Width(mWidth*1.2f));
			if(pc.cursorObject) pc.cursorObjectName = pc.cursorObject.name;
			else pc.cursorObjectName = "";
			pc.cursorOffset = EditorGUILayout.Vector3Field("Cursor offset", pc.cursorOffset, GUILayout.Width(mWidth));
			pc.autoRemoveCursor = EditorGUILayout.Toggle("Autoremove cursor", pc.autoRemoveCursor, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
			
			pc.minimumMoveDistance = EditorGUILayout.FloatField("Min.move distance", pc.minimumMoveDistance, GUILayout.Width(mWidth));
			pc.ignoreYDistance = EditorGUILayout.Toggle("Ignore Y distance", pc.ignoreYDistance, GUILayout.Width(mWidth));
			pc.useEventMover = EditorGUILayout.Toggle("Use event mover", pc.useEventMover, GUILayout.Width(mWidth));
			pc.autoStopMove = EditorGUILayout.Toggle("Autostop move", pc.autoStopMove, GUILayout.Width(mWidth));
		}
		return pc;
	}
	
	public static CameraControlSettings CameraControlSettings(CameraControlSettings cc)
	{
		cc.type = (CameraControlType)EditorGUILayout.EnumPopup("Type", cc.type, GUILayout.Width(mWidth));
		if(cc.IsFollowControl())
		{
			cc.distance = EditorGUILayout.FloatField("Distance", cc.distance, GUILayout.Width(mWidth));
			cc.height = EditorGUILayout.FloatField("Height", cc.height, GUILayout.Width(mWidth));
			cc.heightDamping = EditorGUILayout.FloatField("Height damping", cc.heightDamping, GUILayout.Width(mWidth));
			cc.rotationDamping = EditorGUILayout.FloatField("Rotation damping", cc.rotationDamping, GUILayout.Width(mWidth));
		}
		else if(cc.IsLookControl())
		{
			cc.damping = EditorGUILayout.FloatField("Damping", cc.damping, GUILayout.Width(mWidth));
			cc.smooth = EditorGUILayout.Toggle("Smooth", cc.smooth, GUILayout.Width(mWidth));
		}
		else if(cc.IsMobileControl())
		{
			cc.distance = EditorGUILayout.FloatField("Distance", cc.distance, GUILayout.Width(mWidth));
			cc.height = EditorGUILayout.FloatField("Height", cc.height, GUILayout.Width(mWidth));
			cc.minHeight = EditorGUILayout.FloatField("Min. height", cc.minHeight, GUILayout.Width(mWidth));
			cc.maxHeight = EditorGUILayout.FloatField("Max. height", cc.maxHeight, GUILayout.Width(mWidth));
			cc.heightDamping = EditorGUILayout.FloatField("Height damping", cc.heightDamping, GUILayout.Width(mWidth));
			cc.allowRotation = EditorGUILayout.Toggle("Allow rotation", cc.allowRotation, GUILayout.Width(mWidth));
			cc.allowZoom = EditorGUILayout.Toggle("Allow Zoom", cc.allowZoom, GUILayout.Width(mWidth));
			cc.rotation = EditorGUILayout.FloatField("Rotation", cc.rotation, GUILayout.Width(mWidth));
			cc.rotationDamping = EditorGUILayout.FloatField("Rotation damping", cc.rotationDamping, GUILayout.Width(mWidth));
			cc.rotationFactor = EditorGUILayout.FloatField("Rotation factor", cc.rotationFactor, GUILayout.Width(mWidth));
			cc.zoomFactor = EditorGUILayout.FloatField("Zoom factor", cc.zoomFactor, GUILayout.Width(mWidth));
			cc.mouseTouch = EditorHelper.MouseTouchControlSettings(cc.mouseTouch, false);
			cc.mouseTouch.mode = MouseTouch.MOVE;
			cc.zoomPlusKey = EditorGUILayout.TextField("Zoom+ key", cc.zoomPlusKey, GUILayout.Width(mWidth));
			cc.zoomMinusKey = EditorGUILayout.TextField("Zoom- key", cc.zoomMinusKey, GUILayout.Width(mWidth));
			cc.zoomKeyChange = EditorGUILayout.FloatField("Zoom key change", cc.zoomKeyChange, GUILayout.Width(mWidth));
		}
		else if(cc.IsFirstPersonControl())
		{
			cc.onChild = EditorGUILayout.TextField("Place on child", cc.onChild, GUILayout.Width(mWidth));
			cc.offset = EditorGUILayout.Vector3Field("Offset", cc.offset, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
			
			cc.horizontalAxis = EditorGUILayout.TextField("Horizontal axis", cc.horizontalAxis, GUILayout.Width(mWidth));
			cc.verticalAxis = EditorGUILayout.TextField("Vertical axis", cc.verticalAxis, GUILayout.Width(mWidth));
			cc.sensitivity = EditorGUILayout.Vector2Field("Sensitivity", cc.sensitivity, GUILayout.Width(mWidth));
			cc.lockCursor = EditorGUILayout.Toggle("Lock cursor", cc.lockCursor, GUILayout.Width(mWidth));
		}
		return cc;
	}
	
	public static AIMoverSettings AIMoverSettings(AIMoverSettings mover, bool partyFollow, bool patrol)
	{
		mover.useAIMover = EditorGUILayout.Toggle("Use AI mover", mover.useAIMover, GUILayout.Width(mWidth));
		if(mover.useAIMover)
		{
			EditorGUILayout.Separator();
			GUILayout.Label("General move settings", EditorStyles.boldLabel);
			mover.useCombatantSpeed = EditorGUILayout.Toggle("Use comb. speed",
					mover.useCombatantSpeed, GUILayout.Width(mWidth));
			if(!mover.useCombatantSpeed)
			{
				mover.runSpeed = EditorGUILayout.FloatField("Run speed", 
						mover.runSpeed, GUILayout.Width(mWidth));
			}
			mover.gravity = EditorGUILayout.FloatField("Gravity", mover.gravity, GUILayout.Width(mWidth));
			mover.speedSmoothing = EditorGUILayout.FloatField("Speed smoothing", 
					mover.speedSmoothing, GUILayout.Width(mWidth));
			mover.ignoreYDistance = EditorGUILayout.Toggle("Ignore Y distance", mover.ignoreYDistance, GUILayout.Width(mWidth));
			mover.changeTimeout = EditorGUILayout.FloatField("Change timeout", 
					mover.changeTimeout, GUILayout.Width(mWidth));
			mover.playerDistance = EditorGUILayout.FloatField("Player distance", 
					mover.playerDistance, GUILayout.Width(mWidth));
			
			if(partyFollow)
			{
				EditorGUILayout.Separator();
				GUILayout.Label("Party follow settings", EditorStyles.boldLabel);
				mover.minFollowDistance = EditorGUILayout.FloatField("Min. follow distance", 
						mover.minFollowDistance, GUILayout.Width(mWidth));
				mover.giveWay = EditorGUILayout.Toggle("Give way", mover.giveWay, GUILayout.Width(mWidth));
				if(mover.giveWay)
				{
					mover.giveWayDistance = EditorGUILayout.FloatField("Give way distance", 
							mover.giveWayDistance, GUILayout.Width(mWidth));
				}
				mover.autoRespawn = EditorGUILayout.Toggle("Auto respawn", mover.autoRespawn, GUILayout.Width(mWidth));
				if(mover.autoRespawn)
				{
					mover.respawnDistance = EditorGUILayout.FloatField("Respawn distance", 
							mover.respawnDistance, GUILayout.Width(mWidth));
				}
			}
			
			if(patrol)
			{
				EditorGUILayout.Separator();
				GUILayout.Label("Enemy patrol settings", EditorStyles.boldLabel);
				mover.randomPatrol = EditorGUILayout.Toggle("Use random patrol",
						mover.randomPatrol, GUILayout.Width(mWidth));
				if(mover.randomPatrol)
				{
					mover.patrolRadius = EditorGUILayout.FloatField("Patrol radius", 
							mover.patrolRadius, GUILayout.Width(mWidth));
					mover.waypointTime = EditorGUILayout.FloatField("Waypoint time", 
							mover.waypointTime, GUILayout.Width(mWidth));
				}
			}
			else mover.randomPatrol = false;
		}
		EditorGUILayout.Separator();
		return mover;
	}
	
	public static AIBehaviour AIBehaviourSettings(AIBehaviour ai, int index)
	{
		GUILayout.Label("Priority "+(index+1).ToString(), EditorStyles.boldLabel);
		ai.battleAI = EditorGUILayout.Popup("Battle AI", ai.battleAI, 
				DataHolder.BattleAIs().GetNameList(true), GUILayout.Width(mWidth));
		ai.difficultyID = EditorGUILayout.Popup("Difficulty", ai.difficultyID,
				DataHolder.Difficulties().GetNameList(true), GUILayout.Width(mWidth));
		EditorGUILayout.Separator();
		
		if(GUILayout.Button("Add action", GUILayout.Width(mWidth*0.5f)))
		{
			ai.AddAction();
		}
		for(int i=0; i<ai.attackSelection.Length; i++)
		{
			EditorGUILayout.BeginVertical("box");
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Action "+(i+1).ToString(), EditorStyles.boldLabel);
			EditorGUILayout.Separator();
			if(GUILayout.Button("Remove", GUILayout.Width(mWidth*0.3f)))
			{
				ai.RemoveAction(i);
				break;
			}
			if(GUILayout.Button("Copy", GUILayout.Width(mWidth*0.3f)))
			{
				ai.CopyAction(i);
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
			
			ai.actionDifficultyID[i] = EditorGUILayout.Popup("Difficulty", ai.actionDifficultyID[i],
					DataHolder.Difficulties().GetNameList(true), GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
			
			EditorGUILayout.BeginHorizontal();
			ai.attackSelection[i] = (AttackSelection)EditorGUILayout.EnumPopup(
					ai.attackSelection[i], GUILayout.Width(mWidth*0.5f));
			if(AttackSelection.SKILL.Equals(ai.attackSelection[i]))
			{
				ai.useID[i] = EditorGUILayout.Popup(ai.useID[i], 
						DataHolder.Skills().GetNameList(true), GUILayout.Width(mWidth*0.7f));
				ai.useLevel[i] = EditorGUILayout.IntField("Skill level", ai.useLevel[i], GUILayout.Width(mWidth*0.7f));
				ai.useLevel[i] = EditorTab.MinMaxCheck(ai.useLevel[i], 1, DataHolder.Skill(ai.useID[i]).level.Length);
			}
			else if(AttackSelection.ITEM.Equals(ai.attackSelection[i]))
			{
				ai.useID[i] = EditorGUILayout.Popup(ai.useID[i], 
						DataHolder.Items().GetNameList(true), GUILayout.Width(mWidth*0.7f));
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
			EditorGUILayout.EndVertical();
		}
		EditorGUILayout.Separator();
		return ai;
	}
	
	public static CharacterControlMap CharacterControlMapSettings(CharacterControlMap map)
	{
		map.inputHandling = (InputHandling)EditorGUILayout.EnumPopup("Input handling", 
				map.inputHandling, GUILayout.Width(mWidth));
		map.axisTimeout = EditorGUILayout.FloatField("Axis timeout",
				map.axisTimeout, GUILayout.Width(mWidth*0.7f));
		EditorGUILayout.Separator();
		
		// attack keys
		if(GUILayout.Button("Add attack key", GUILayout.Width(mWidth*0.5f)))
		{
			map.AddAttack();
		}
		EditorGUILayout.Separator();
		for(int i=0; i<map.attackKey.Length; i++)
		{
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Remove", GUILayout.Width(mWidth*0.3f)))
			{
				map.RemoveAttack(i);
				break;
			}
			GUILayout.Label("Attack "+(i+1).ToString(), EditorStyles.boldLabel);
			map.attackKey[i] = EditorGUILayout.TextField(map.attackKey[i], GUILayout.Width(mWidth*0.5f));
			map.attackAxis[i] = EditorGUILayout.Toggle("Is axis", map.attackAxis[i], GUILayout.Width(mWidth));
			if(map.attackAxis[i])
			{
				map.attackTrigger[i] = EditorGUILayout.FloatField("Trigger at",
						map.attackTrigger[i], GUILayout.Width(mWidth*0.7f));
				if(map.attackTrigger[i] < -1) map.attackTrigger[i] = -1;
				else if(map.attackTrigger[i] > 1) map.attackTrigger[i] = 1;
			}
			map.attackAir[i] = EditorGUILayout.Toggle("In air", map.attackAir[i], GUILayout.Width(mWidth));
			if(!map.attackAir[i])
			{
				map.attackSpeed[i] = EditorGUILayout.FloatField("Min. speed",
						map.attackSpeed[i], GUILayout.Width(mWidth*0.7f));
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.Separator();
		
		// skill keys
		if(GUILayout.Button("Add skill key", GUILayout.Width(mWidth*0.5f)))
		{
			map.AddSkill();
		}
		EditorGUILayout.Separator();
		for(int i=0; i<map.skillID.Length; i++)
		{
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Remove", GUILayout.Width(mWidth*0.3f)))
			{
				map.RemoveSkill(i);
				break;
			}
			map.skillID[i] = EditorGUILayout.Popup(map.skillID[i], 
					DataHolder.Skills().GetNameList(true), GUILayout.Width(mWidth*0.5f));
			map.skillKey[i] = EditorGUILayout.TextField(map.skillKey[i], GUILayout.Width(mWidth*0.5f));
			map.skillAxis[i] = EditorGUILayout.Toggle("Is axis", map.skillAxis[i], GUILayout.Width(mWidth));
			if(map.skillAxis[i])
			{
				map.skillTrigger[i] = EditorGUILayout.FloatField("Trigger at",
						map.skillTrigger[i], GUILayout.Width(mWidth*0.7f));
				if(map.skillTrigger[i] < -1) map.attackTrigger[i] = -1;
				else if(map.skillTrigger[i] > 1) map.attackTrigger[i] = 1;
			}
			map.skillAir[i] = EditorGUILayout.Toggle("In air", map.skillAir[i], GUILayout.Width(mWidth));
			if(!map.skillAir[i])
			{
				map.skillSpeed[i] = EditorGUILayout.FloatField("Min. speed",
						map.skillSpeed[i], GUILayout.Width(mWidth*0.7f));
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.Separator();
		
		// item keys
		if(GUILayout.Button("Add item key", GUILayout.Width(mWidth*0.5f)))
		{
			map.AddItem();
		}
		EditorGUILayout.Separator();
		for(int i=0; i<map.itemID.Length; i++)
		{
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Remove", GUILayout.Width(mWidth*0.3f)))
			{
				map.RemoveItem(i);
				break;
			}
			map.itemID[i] = EditorGUILayout.Popup(map.itemID[i], 
					DataHolder.Items().GetNameList(true), GUILayout.Width(mWidth*0.5f));
			map.itemKey[i] = EditorGUILayout.TextField(map.itemKey[i], GUILayout.Width(mWidth*0.5f));
			map.itemAxis[i] = EditorGUILayout.Toggle("Is axis", map.itemAxis[i], GUILayout.Width(mWidth));
			if(map.itemAxis[i])
			{
				map.itemTrigger[i] = EditorGUILayout.FloatField("Trigger at",
						map.itemTrigger[i], GUILayout.Width(mWidth*0.7f));
				if(map.itemTrigger[i] < -1) map.attackTrigger[i] = -1;
				else if(map.itemTrigger[i] > 1) map.attackTrigger[i] = 1;
			}
			map.itemAir[i] = EditorGUILayout.Toggle("In air", map.itemAir[i], GUILayout.Width(mWidth));
			if(!map.itemAir[i])
			{
				map.itemSpeed[i] = EditorGUILayout.FloatField("Min. speed",
						map.itemSpeed[i], GUILayout.Width(mWidth*0.7f));
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.Separator();
		
		return map;
	}
	
	public static void AnimationData(string title, ref AnimationData data)
	{
		EditorGUILayout.BeginHorizontal("box");
		data.name = EditorGUILayout.TextField(title, data.name, GUILayout.Width(mWidth));
		if(data.name != "")
		{
			data.layer = EditorGUILayout.IntField("Layer", data.layer, GUILayout.Width(mWidth*0.6f));
			data.setSpeed = EditorGUILayout.Toggle("Set speed", data.setSpeed, GUILayout.Width(mWidth));
			if(data.setSpeed)
			{
				data.speedFormula = EditorGUILayout.Popup("Formula", 
						data.speedFormula, DataHolder.Formulas().GetNameList(true), GUILayout.Width(mWidth));
			}
		}
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
	}
	
	public static FieldAnimationSettings FieldAnimationsSettings(FieldAnimationSettings anim)
	{
		anim.baseAnimator = EditorGUILayout.Toggle("Use animator", anim.baseAnimator, GUILayout.Width(mWidth));
		if(anim.baseAnimator)
		{
			anim.walkSpeed = EditorGUILayout.FloatField("Walk speed", anim.walkSpeed, GUILayout.Width(mWidth*0.7f));
			anim.runSpeed = EditorGUILayout.FloatField("Run speed", anim.runSpeed, GUILayout.Width(mWidth*0.7f));
			anim.fadeLength = EditorGUILayout.FloatField("Fade length", anim.fadeLength, GUILayout.Width(mWidth*0.7f));
			anim.minFallTime = EditorGUILayout.FloatField("Min fall time", anim.minFallTime, GUILayout.Width(mWidth*0.7f));
			EditorGUILayout.Separator();
			
			EditorHelper.AnimationData("Idle", ref anim.idle);
			EditorHelper.AnimationData("Walk", ref anim.walk);
			EditorHelper.AnimationData("Run", ref anim.run);
			EditorHelper.AnimationData("Sprint", ref anim.sprint);
			EditorHelper.AnimationData("Jump", ref anim.jump);
			EditorHelper.AnimationData("Fall", ref anim.fall);
			EditorHelper.AnimationData("Land", ref anim.land);
		}
		return anim;
	}
	
	public static BattleAnimationSettings BaseAnimationsSettings(BattleAnimationSettings anim, bool baseAnims, bool character)
	{
		if(baseAnims)
		{
			GUILayout.Label("Base animations", EditorStyles.boldLabel);
			EditorHelper.AnimationData("Idle", ref anim.idle);
			EditorHelper.AnimationData("Walk", ref anim.walk);
			EditorHelper.AnimationData("Run", ref anim.run);
			EditorHelper.AnimationData("Sprint", ref anim.sprint);
			EditorHelper.AnimationData("Jump", ref anim.jump);
			EditorHelper.AnimationData("Fall", ref anim.fall);
			EditorHelper.AnimationData("Land", ref anim.land);
			EditorHelper.AnimationData("Attack", ref anim.attack);
			EditorHelper.AnimationData("Defend", ref anim.defend);
			EditorHelper.AnimationData("Item", ref anim.item);
			EditorHelper.AnimationData("Skill", ref anim.skill);
			EditorHelper.AnimationData("Damage", ref anim.damage);
			EditorHelper.AnimationData("Evade", ref anim.evade);
			EditorHelper.AnimationData("Death", ref anim.death);
			EditorHelper.AnimationData("Revive", ref anim.revive);
			
			if(character)
			{
				EditorHelper.AnimationData("Victory", ref anim.victory);
				EditorHelper.AnimationData("Idle after battle", ref anim.idleAfter);
			}
			EditorGUILayout.Separator();
		}
		
		GUILayout.Label("Battle animations", EditorStyles.boldLabel);
		EditorGUILayout.BeginHorizontal(GUILayout.Width(mWidth*1.2f));
		anim.animateBaseAttack = EditorGUILayout.BeginToggleGroup("Attack animation", 
				anim.animateBaseAttack);
		anim.baseAttackAnimationID = EditorGUILayout.Popup(
				anim.baseAttackAnimationID, DataHolder.BattleAnimations().GetNameList(true));
		EditorGUILayout.EndToggleGroup();
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal(GUILayout.Width(mWidth*1.2f));
		anim.animateDefend = EditorGUILayout.BeginToggleGroup("Defend animation", 
				anim.animateDefend);
		anim.defendAnimationID = EditorGUILayout.Popup(
				anim.defendAnimationID, DataHolder.BattleAnimations().GetNameList(true));
		EditorGUILayout.EndToggleGroup();
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal(GUILayout.Width(mWidth*1.2f));
		anim.animateEscape = EditorGUILayout.BeginToggleGroup("Escape animation", 
				anim.animateEscape);
		anim.escapeAnimationID = EditorGUILayout.Popup(
				anim.escapeAnimationID, DataHolder.BattleAnimations().GetNameList(true));
		EditorGUILayout.EndToggleGroup();
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal(GUILayout.Width(mWidth*1.2f));
		anim.animateDeath = EditorGUILayout.BeginToggleGroup("Death animation", 
				anim.animateDeath);
		anim.deathAnimationID = EditorGUILayout.Popup(
				anim.deathAnimationID, DataHolder.BattleAnimations().GetNameList(true));
		EditorGUILayout.EndToggleGroup();
		EditorGUILayout.EndHorizontal();
		
		return anim;
	}
	
	public static CustomAnimationSettings CustomAnimationSettings(CustomAnimationSettings anim)
	{
		if(GUILayout.Button("Add animation", GUILayout.Width(mWidth)))
		{
			anim.AddAnimation();
		}
		EditorGUILayout.Separator();
		
		for(int i=0; i<anim.animation.Length; i++)
		{
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Remove", GUILayout.Width(mWidth*0.5f)))
			{
				anim.RemoveAnimation(i);
				break;
			}
			EditorHelper.AnimationData("Animation", ref anim.animation[i]);
			EditorGUILayout.EndHorizontal();
		}
		
		return anim;
	}
	
	public static TargetRaycast TargetRaycastSettings(TargetRaycast ray)
	{
		ray.active = EditorGUILayout.Toggle("Target raycast", ray.active, GUILayout.Width(mWidth));
		if(ray.active)
		{
			EditorGUILayout.BeginVertical("box");
			GUILayout.Label("Raycast settings", EditorStyles.boldLabel);
			ray.distance = EditorGUILayout.FloatField("Distance", ray.distance, GUILayout.Width(mWidth));
			ray.layerMask = EditorGUILayout.LayerField("Layer", ray.layerMask, GUILayout.Width(mWidth));
			ray.ignoreUser = EditorGUILayout.Toggle("Ignore user", ray.ignoreUser, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
			
			ray.mouseTouch = EditorHelper.MouseTouchControlSettings(ray.mouseTouch, true);
			
			ray.rayOrigin = (TargetRayOrigin)EditorTab.EnumToolbar("Ray origin", (int)ray.rayOrigin, typeof(TargetRayOrigin));
			if(TargetRayOrigin.USER.Equals(ray.rayOrigin))
			{
				ray.pathToChild = EditorGUILayout.TextField("Path to child", ray.pathToChild, GUILayout.Width(mWidth*1.2f));
				if(!ray.mouseTouch.Active())
				{
					ray.rayDirection = EditorGUILayout.Vector3Field("Direction (local space)", ray.rayDirection, GUILayout.Width(mWidth));
				}
			}
			ray.offset = EditorGUILayout.Vector3Field("Offset", ray.offset, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
			
			GUILayout.Label("AI target", EditorStyles.boldLabel);
			ray.pathToTarget = EditorGUILayout.TextField("Path to child", ray.pathToTarget, GUILayout.Width(mWidth*1.2f));
			ray.targetOffset = EditorGUILayout.Vector3Field("Offset", ray.targetOffset, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
			EditorGUILayout.EndVertical();
		}
		return ray;
	}
	
	public static MouseTouchControl MouseTouchControlSettings(MouseTouchControl mouseTouch, bool setMode)
	{
		EditorGUILayout.BeginVertical("box");
		GUILayout.Label("Mouse/touch control settings", EditorStyles.boldLabel);
		mouseTouch.useClick = EditorGUILayout.Toggle("Use click", mouseTouch.useClick, GUILayout.Width(mWidth));
		if(mouseTouch.useClick)
		{
			mouseTouch.mouseClick = EditorGUILayout.IntField("Mouse button", mouseTouch.mouseClick, GUILayout.Width(mWidth));
		}
		
		mouseTouch.useTouch = EditorGUILayout.Toggle("Use touch", mouseTouch.useTouch, GUILayout.Width(mWidth));
		if(mouseTouch.useTouch)
		{
			mouseTouch.touchCount = EditorGUILayout.IntField("Touch count", mouseTouch.touchCount, GUILayout.Width(mWidth));
		}
		if(mouseTouch.useClick || mouseTouch.useTouch)
		{
			mouseTouch.clickCount = EditorGUILayout.IntField("Click count", mouseTouch.clickCount, GUILayout.Width(mWidth));
			if(setMode)
			{
				mouseTouch.mode = (MouseTouch)EditorGUILayout.EnumPopup("Mode", 
						mouseTouch.mode, GUILayout.Width(EditorHelper.mWidth));
			}
		}
		EditorGUILayout.EndVertical();
		return mouseTouch;
	}
	
	public static GameVariable GameVariableSettings(GameVariable var)
	{
		if(GUILayout.Button("Add variable", GUILayout.Width(mWidth)))
		{
			var.AddVariable();
		}
		for(int i=0; i<var.variableKey.Length; i++)
		{
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Remove", GUILayout.Width(mWidth*0.3f)))
			{
				var.RemoveVariable(i);
				break;
			}
			var.remove[i] = EditorGUILayout.Toggle("Remove", var.remove[i], GUILayout.Width(mWidth));
			var.variableKey[i] = EditorGUILayout.TextField("Key", var.variableKey[i], GUILayout.Width(mWidth));
			if(!var.remove[i])
			{
				var.variableValue[i] = EditorGUILayout.TextField("Value", var.variableValue[i], GUILayout.Width(mWidth));
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndVertical();
		}
		EditorGUILayout.Separator();
		
		if(GUILayout.Button("Add number variable", GUILayout.Width(mWidth)))
		{
			var.AddNumberVariable();
		}
		for(int i=0; i<var.numberVarKey.Length; i++)
		{
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Remove", GUILayout.Width(mWidth*0.3f)))
			{
				var.RemoveNumberVariable(i);
				break;
			}
			var.removeNumber[i] = EditorGUILayout.Toggle("Remove", var.removeNumber[i], GUILayout.Width(mWidth));
			var.numberVarKey[i] = EditorGUILayout.TextField("Key", var.numberVarKey[i], GUILayout.Width(mWidth));
			if(!var.removeNumber[i])
			{
				var.changeType[i] = (SimpleOperator)EditorGUILayout.EnumPopup(
						var.changeType[i], GUILayout.Width(EditorHelper.mWidth*0.5f));
				var.numberVarValue[i] = EditorGUILayout.FloatField(var.numberVarValue[i], GUILayout.Width(mWidth*0.5f));
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndVertical();
		}
		return var;
	}
	
	public static void BattleTextSettings(ref BattleTextSettings ts, string txt, bool showCount)
	{
		ts.active = EditorGUILayout.Toggle("Show text", ts.active, GUILayout.Width(mWidth));
		if(ts.active)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			GUILayout.Label("Text settings", EditorStyles.boldLabel);
			GUILayout.Label(txt);
			for(int i=0; i<ts.text.Length; i++)
			{
				ts.text[i] = EditorGUILayout.TextField(DataHolder.Languages().GetName(i), 
						ts.text[i], GUILayout.Width(mWidth));
			}
			
			ts.color = EditorGUILayout.Popup("Color", ts.color, DataHolder.Colors().GetNameList(true), GUILayout.Width(mWidth));
			ts.showShadow = EditorGUILayout.Toggle("Show shadow", ts.showShadow, GUILayout.Width(mWidth));
			if(ts.showShadow)
			{
				ts.shadowColor = EditorGUILayout.Popup("Shadow color", ts.shadowColor, DataHolder.Colors().GetNameList(true), GUILayout.Width(mWidth));
				ts.shadowOffset = EditorGUILayout.Vector2Field("Shadow offset", ts.shadowOffset, GUILayout.Width(mWidth));
			}
			EditorGUILayout.Separator();
			
			ts.visibleTime = EditorGUILayout.FloatField("Visible time", ts.visibleTime, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
			
			if(showCount && GUISystemType.UNITY.Equals(DataHolder.GameSettings().guiSystemType))
			{
				ts.countToValue = EditorGUILayout.Toggle("Count to value", ts.countToValue, GUILayout.Width(mWidth));
				if(ts.countToValue)
				{
					ts.startCountFrom = EditorGUILayout.FloatField("Start from (%)", ts.startCountFrom, GUILayout.Width(mWidth));
					ts.countInterpolate = (EaseType)EditorGUILayout.EnumPopup("Interpolation", ts.countInterpolate, GUILayout.Width(mWidth));
					ts.countTime = EditorGUILayout.FloatField("Time", ts.countTime, GUILayout.Width(mWidth));
				}
				EditorGUILayout.Separator();
			}
			else ts.countToValue = false;
			
			GUILayout.Label("Position settings", EditorStyles.boldLabel);
			ts.posChild = EditorGUILayout.TextField("Use child", ts.posChild, GUILayout.Width(mWidth));
			ts.localSpace = EditorGUILayout.Toggle("Local space", ts.localSpace, GUILayout.Width(mWidth));
			ts.baseOffset = EditorGUILayout.Vector3Field("Base offset", ts.baseOffset, GUILayout.Width(mWidth));
			ts.randomOffsetFrom = EditorGUILayout.Vector3Field("Random offset from", ts.randomOffsetFrom, GUILayout.Width(mWidth));
			ts.randomOffsetTo = EditorGUILayout.Vector3Field("Random offset to", ts.randomOffsetTo, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			
			EditorGUILayout.BeginVertical();
			GUILayout.Label("Fade settings", EditorStyles.boldLabel);
			ts.fadeIn = EditorGUILayout.Toggle("Fade in", ts.fadeIn, GUILayout.Width(mWidth));
			if(ts.fadeIn)
			{
				ts.fadeInTime = EditorGUILayout.FloatField("Fade in time", ts.fadeInTime, GUILayout.Width(mWidth));
				ts.fadeInStart = EditorGUILayout.FloatField("Start alpha", ts.fadeInStart, GUILayout.Width(mWidth));
				if(ts.fadeInStart < 0) ts.fadeInStart = 0; else if(ts.fadeInStart > 1) ts.fadeInStart = 1;
				ts.fadeInEnd = EditorGUILayout.FloatField("End alpha", ts.fadeInEnd, GUILayout.Width(mWidth));
				if(ts.fadeInEnd < 0) ts.fadeInEnd = 0; else if(ts.fadeInEnd > 1) ts.fadeInEnd = 1;
				ts.fadeInInterpolate = (EaseType)EditorGUILayout.EnumPopup("Interpolation", ts.fadeInInterpolate, GUILayout.Width(mWidth));
			}
			EditorGUILayout.Separator();
			ts.fadeOut = EditorGUILayout.Toggle("Fade out", ts.fadeOut, GUILayout.Width(mWidth));
			if(ts.fadeOut)
			{
				ts.fadeOutTime = EditorGUILayout.FloatField("Fade out time", ts.fadeOutTime, GUILayout.Width(mWidth));
				ts.fadeOutStart = EditorGUILayout.FloatField("Start alpha", ts.fadeOutStart, GUILayout.Width(mWidth));
				if(ts.fadeOutStart < 0) ts.fadeOutStart = 0; else if(ts.fadeOutStart > 1) ts.fadeOutStart = 1;
				ts.fadeOutEnd = EditorGUILayout.FloatField("End alpha", ts.fadeOutEnd, GUILayout.Width(mWidth));
				if(ts.fadeOutEnd < 0) ts.fadeOutEnd = 0; else if(ts.fadeOutEnd > 1) ts.fadeOutEnd = 1;
				ts.fadeOutInterpolate = (EaseType)EditorGUILayout.EnumPopup("Interpolation", ts.fadeOutInterpolate, GUILayout.Width(mWidth));
			}
			EditorGUILayout.Separator();
			
			GUILayout.Label("Prefab settings", EditorStyles.boldLabel);
			ts.spawnPrefab = EditorGUILayout.Toggle("Spawn prefab", ts.spawnPrefab, GUILayout.Width(mWidth));
			if(ts.spawnPrefab)
			{
				if(ts.prefab == null && "" != ts.prefabName)
				{
					ts.prefab = (GameObject)Resources.Load(BattleSystemData.PREFAB_PATH+
							ts.prefabName, typeof(GameObject));
				}
				ts.prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", ts.prefab, 
						typeof(GameObject), false, GUILayout.Width(mWidth));
				if(ts.prefab) ts.prefabName = ts.prefab.name;
				else ts.prefabName = "";
				
				ts.prefabTime = EditorGUILayout.FloatField("Destroy after", ts.prefabTime, GUILayout.Width(mWidth));
				ts.prefabChild = EditorGUILayout.TextField("Spawn on child", ts.prefabChild, GUILayout.Width(mWidth));
				ts.prefabOffset = EditorGUILayout.Vector3Field("Spawn offset", ts.prefabOffset, GUILayout.Width(mWidth));
			}
			EditorGUILayout.Separator();
			
			GUILayout.Label("Audio settings", EditorStyles.boldLabel);
			ts.playAudio = EditorGUILayout.Toggle("Play audio", ts.playAudio, GUILayout.Width(mWidth));
			if(ts.playAudio)
			{
				if(ts.audioClip == null && "" != ts.audioName)
				{
					ts.audioClip = (AudioClip)Resources.Load(BattleSystemData.AUDIO_PATH+
							ts.audioName, typeof(AudioClip));
				}
				ts.audioClip = (AudioClip)EditorGUILayout.ObjectField("Audio clip", ts.audioClip, 
						typeof(AudioClip), false, GUILayout.Width(mWidth));
				if(ts.audioClip) ts.audioName = ts.audioClip.name;
				else ts.audioName = "";
			}
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			
			EditorGUILayout.BeginVertical();
			GUILayout.Label("Animation", EditorStyles.boldLabel);
			ts.animate = EditorGUILayout.Toggle("Animate", ts.animate, GUILayout.Width(mWidth));
			if(ts.animate)
			{
				ts.xRandom = EditorGUILayout.Toggle("X random", ts.xRandom, GUILayout.Width(mWidth));
				if(ts.xRandom)
				{
					ts.xMin = EditorGUILayout.FloatField("X min", ts.xMin, GUILayout.Width(mWidth));
					ts.xMax = EditorGUILayout.FloatField("X max", ts.xMax, GUILayout.Width(mWidth));
				}
				else
				{
					ts.xSpeed = EditorGUILayout.FloatField("X speed", ts.xSpeed, GUILayout.Width(mWidth));
				}
				EditorGUILayout.Separator();
				ts.yRandom = EditorGUILayout.Toggle("Y random", ts.yRandom, GUILayout.Width(mWidth));
				if(ts.yRandom)
				{
					ts.yMin = EditorGUILayout.FloatField("Y min", ts.yMin, GUILayout.Width(mWidth));
					ts.yMax = EditorGUILayout.FloatField("Y max", ts.yMax, GUILayout.Width(mWidth));
				}
				else
				{
					ts.ySpeed = EditorGUILayout.FloatField("Y speed", ts.ySpeed, GUILayout.Width(mWidth));
				}
				EditorGUILayout.Separator();
				ts.zRandom = EditorGUILayout.Toggle("Z random", ts.zRandom, GUILayout.Width(mWidth));
				if(ts.zRandom)
				{
					ts.zMin = EditorGUILayout.FloatField("Z min", ts.zMin, GUILayout.Width(mWidth));
					ts.zMax = EditorGUILayout.FloatField("Z max", ts.zMax, GUILayout.Width(mWidth));
				}
				else
				{
					ts.zSpeed = EditorGUILayout.FloatField("Z speed", ts.zSpeed, GUILayout.Width(mWidth));
				}
				EditorGUILayout.Separator();
			}
			
			ts.flash = EditorGUILayout.Toggle("Flash object", ts.flash, GUILayout.Width(mWidth));
			if(ts.flash)
			{
				ts.fromCurrent = EditorGUILayout.Toggle("From current", ts.fromCurrent, GUILayout.Width(mWidth));
				ts.flashTime = EditorGUILayout.FloatField("Time", ts.flashTime, GUILayout.Width(mWidth));
				ts.flashChildren = EditorGUILayout.Toggle("Flash children", ts.flashChildren, GUILayout.Width(mWidth));
				ts.flashInterpolate = (EaseType)EditorGUILayout.EnumPopup("Interpolation", ts.flashInterpolate, GUILayout.Width(mWidth));
				EditorGUILayout.Separator();
				ts.flashAlpha = EditorGUILayout.Toggle("Alpha", ts.flashAlpha, GUILayout.Width(mWidth));
				if(ts.flashAlpha)
				{
					if(!ts.fromCurrent) ts.alphaStart = EditorGUILayout.FloatField("From", ts.alphaStart, GUILayout.Width(mWidth));
					ts.alphaEnd = EditorGUILayout.FloatField("To", ts.alphaEnd, GUILayout.Width(mWidth));
					EditorGUILayout.Separator();
				}
				ts.flashRed	= EditorGUILayout.Toggle("Red", ts.flashRed, GUILayout.Width(mWidth));
				if(ts.flashRed)
				{
					if(!ts.fromCurrent) ts.redStart = EditorGUILayout.FloatField("From", ts.redStart, GUILayout.Width(mWidth));
					ts.redEnd = EditorGUILayout.FloatField("To", ts.redEnd, GUILayout.Width(mWidth));
					EditorGUILayout.Separator();
				}
				ts.flashGreen = EditorGUILayout.Toggle("Green", ts.flashGreen, GUILayout.Width(mWidth));
				if(ts.flashGreen)
				{
					if(!ts.fromCurrent) ts.greenStart = EditorGUILayout.FloatField("From", ts.greenStart, GUILayout.Width(mWidth));
					ts.greenEnd = EditorGUILayout.FloatField("To", ts.greenEnd, GUILayout.Width(mWidth));
					EditorGUILayout.Separator();
				}
				ts.flashBlue = EditorGUILayout.Toggle("Blue", ts.flashBlue, GUILayout.Width(mWidth));
				if(ts.flashBlue)
				{
					if(!ts.fromCurrent) ts.blueStart = EditorGUILayout.FloatField("From", ts.blueStart, GUILayout.Width(mWidth));
					ts.blueEnd = EditorGUILayout.FloatField("To", ts.blueEnd, GUILayout.Width(mWidth));
				}
			}
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
		}
	}
	
	public static void StealChanceSettings(ref StealChance chance)
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical();
		chance.stealItem = EditorGUILayout.Toggle("Steal item", chance.stealItem, GUILayout.Width(mWidth));
		if(chance.stealItem)
		{
			chance.itemChance = EditorGUILayout.Popup("Steal chance", chance.itemChance, 
					DataHolder.Formulas().GetNameList(true), GUILayout.Width(mWidth));
			chance.itemBonus = EditorGUILayout.FloatField("Chance bonus", chance.itemBonus, GUILayout.Width(mWidth));
			chance.fixItem = EditorGUILayout.Toggle("Fix item", chance.fixItem, GUILayout.Width(mWidth));
			if(chance.fixItem)
			{
				chance.itemType = (ItemDropType)EditorGUILayout.EnumPopup("Type", 
						chance.itemType, GUILayout.Width(mWidth));
				if(ItemDropType.ITEM.Equals(chance.itemType))
				{
					chance.itemID = EditorGUILayout.Popup("Item", chance.itemID, 
							DataHolder.Items().GetNameList(true), GUILayout.Width(mWidth));
				}
				else if(ItemDropType.WEAPON.Equals(chance.itemType))
				{
					chance.itemID = EditorGUILayout.Popup("Weapon", chance.itemID, 
							DataHolder.Weapons().GetNameList(true), GUILayout.Width(mWidth));
				}
				else if(ItemDropType.ARMOR.Equals(chance.itemType))
				{
					chance.itemID = EditorGUILayout.Popup("Armor", chance.itemID, 
							DataHolder.Armors().GetNameList(true), GUILayout.Width(mWidth));
				}
			}
		}
		EditorGUILayout.EndVertical();
		EditorGUILayout.Separator();
		
		EditorGUILayout.BeginVertical();
		chance.stealMoney = EditorGUILayout.Toggle("Steal money", chance.stealMoney, GUILayout.Width(mWidth));
		if(chance.stealMoney)
		{
			chance.moneyChance = EditorGUILayout.Popup("Steal chance", chance.moneyChance, 
					DataHolder.Formulas().GetNameList(true), GUILayout.Width(mWidth));
			chance.moneyBonus = EditorGUILayout.FloatField("Chance bonus", chance.moneyBonus, GUILayout.Width(mWidth));
			chance.fixMoney = EditorGUILayout.Toggle("Fix money", chance.fixMoney, GUILayout.Width(mWidth));
			if(chance.fixMoney)
			{
				chance.money = EditorGUILayout.IntField("Money", chance.money, GUILayout.Width(mWidth));
			}
		}
		EditorGUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
	}
	
	public static void BonusSettings(ref BonusSettings bonus, bool showExp)
	{
		EditorGUILayout.BeginHorizontal();
		// status value bonuses
		EditorGUILayout.BeginVertical();
		GUILayout.Label("Status value bonuses", EditorStyles.boldLabel);
		for(int i=0; i<bonus.statusBonus.Length; i++)
		{
			if(DataHolder.StatusValue(i).IsConsumable() || 
				(!showExp && DataHolder.StatusValue(i).IsExperience()))
			{
				bonus.statusBonus[i] = 0;
			}
			else
			{
				bonus.statusBonus[i] = EditorGUILayout.IntField(DataHolder.StatusValues().GetName(i), 
						bonus.statusBonus[i], GUILayout.Width(mWidth));
			}
		}
		EditorGUILayout.EndVertical();
		
		// chance bonuses
		EditorGUILayout.BeginVertical();
		GUILayout.Label("Chance bonuses", EditorStyles.boldLabel);
		bonus.hitBonus = EditorGUILayout.FloatField("Hit bonus", bonus.hitBonus, GUILayout.Width(mWidth));
		bonus.counterBonus = EditorGUILayout.FloatField("Counter bonus", bonus.counterBonus, GUILayout.Width(mWidth));
		bonus.criticalBonus = EditorGUILayout.FloatField("Critical bonus", bonus.criticalBonus, GUILayout.Width(mWidth));
		bonus.blockBonus = EditorGUILayout.FloatField("Block bonus", bonus.blockBonus, GUILayout.Width(mWidth));
		bonus.escapeBonus = EditorGUILayout.FloatField("Escape bonus", bonus.escapeBonus, GUILayout.Width(mWidth));
		bonus.speedBonus = EditorGUILayout.FloatField("Speed bonus", bonus.speedBonus, GUILayout.Width(mWidth));
		bonus.itemStealBonus = EditorGUILayout.FloatField("Item steal bonus", bonus.itemStealBonus, GUILayout.Width(mWidth));
		bonus.moneyStealBonus = EditorGUILayout.FloatField("Money steal bonus", bonus.moneyStealBonus, GUILayout.Width(mWidth));
		EditorGUILayout.EndVertical();
		
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Separator();
		
		if(GUILayout.Button("Add difficulty bonuses", GUILayout.Width(mWidth)))
		{
			bonus.AddDifficultyBonus();
		}
		
		for(int i=0; i<bonus.difficultyBonus.Length; i++)
		{
			GUILayout.BeginVertical("box");
			if(GUILayout.Button("Remove", GUILayout.Width(mWidth*0.5f)))
			{
				bonus.RemoveDifficultyBonus(i);
				break;
			}
			EditorHelper.DifficultyBonusSettings(ref bonus.difficultyBonus[i], showExp);
			EditorGUILayout.Separator();
			EditorGUILayout.EndVertical();
		}
	}
	
	public static void DifficultyBonusSettings(ref DifficultyBonus bonus, bool showExp)
	{
		bonus.difficultyID = EditorGUILayout.Popup("Difficulty", bonus.difficultyID, 
				DataHolder.Difficulties().GetNameList(true), GUILayout.Width(mWidth));
		EditorGUILayout.Separator();
		
		EditorGUILayout.BeginHorizontal();
		// status value bonuses
		EditorGUILayout.BeginVertical();
		GUILayout.Label("Status value bonuses", EditorStyles.boldLabel);
		for(int i=0; i<bonus.statusBonus.Length; i++)
		{
			if(DataHolder.StatusValue(i).IsConsumable() || 
				(!showExp && DataHolder.StatusValue(i).IsExperience()))
			{
				bonus.statusBonus[i] = 0;
			}
			else
			{
				bonus.statusBonus[i] = EditorGUILayout.IntField(DataHolder.StatusValues().GetName(i), 
						bonus.statusBonus[i], GUILayout.Width(mWidth));
			}
		}
		EditorGUILayout.EndVertical();
		
		// chance bonuses
		EditorGUILayout.BeginVertical();
		GUILayout.Label("Chance bonuses", EditorStyles.boldLabel);
		bonus.hitBonus = EditorGUILayout.FloatField("Hit bonus", bonus.hitBonus, GUILayout.Width(mWidth));
		bonus.counterBonus = EditorGUILayout.FloatField("Counter bonus", bonus.counterBonus, GUILayout.Width(mWidth));
		bonus.criticalBonus = EditorGUILayout.FloatField("Critical bonus", bonus.criticalBonus, GUILayout.Width(mWidth));
		bonus.blockBonus = EditorGUILayout.FloatField("Block bonus", bonus.blockBonus, GUILayout.Width(mWidth));
		bonus.escapeBonus = EditorGUILayout.FloatField("Escape bonus", bonus.escapeBonus, GUILayout.Width(mWidth));
		bonus.speedBonus = EditorGUILayout.FloatField("Speed bonus", bonus.speedBonus, GUILayout.Width(mWidth));
		bonus.itemStealBonus = EditorGUILayout.FloatField("Item steal bonus", bonus.itemStealBonus, GUILayout.Width(mWidth));
		bonus.moneyStealBonus = EditorGUILayout.FloatField("Money steal bonus", bonus.moneyStealBonus, GUILayout.Width(mWidth));
		EditorGUILayout.EndVertical();
		
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Separator();
		
		EditorGUILayout.BeginHorizontal();
		// element bonuses
		EditorGUILayout.BeginVertical();
		GUILayout.Label("Element bonuses", EditorStyles.boldLabel);
		for(int i=0; i<bonus.elementBonus.Length; i++)
		{
			bonus.elementBonus[i] = EditorGUILayout.IntField(DataHolder.Elements().GetName(i), 
					bonus.elementBonus[i], GUILayout.Width(mWidth));
		}
		EditorGUILayout.EndVertical();
		
		// race bonuses
		EditorGUILayout.BeginVertical();
		GUILayout.Label("Race bonuses", EditorStyles.boldLabel);
		for(int i=0; i<bonus.raceBonus.Length; i++)
		{
			bonus.raceBonus[i] = EditorGUILayout.IntField(DataHolder.Races().GetName(i), 
					bonus.raceBonus[i], GUILayout.Width(mWidth));
		}
		EditorGUILayout.EndVertical();
		
		// size bonuses
		EditorGUILayout.BeginVertical();
		GUILayout.Label("Size bonuses", EditorStyles.boldLabel);
		for(int i=0; i<bonus.sizeBonus.Length; i++)
		{
			bonus.sizeBonus[i] = EditorGUILayout.IntField(DataHolder.Sizes().GetName(i), 
					bonus.sizeBonus[i], GUILayout.Width(mWidth));
		}
		EditorGUILayout.EndVertical();
		
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
	}
	
	public static void ItemTypeQuantitySelection(ref ItemDropType type, ref int id, ref int quantity)
	{
		ItemDropType tmp = type;
		type = (ItemDropType)EditorGUILayout.EnumPopup("Type", type, GUILayout.Width(mWidth));
		if(!type.Equals(tmp)) id = 0;
		if(ItemDropType.ITEM.Equals(type))
		{
			id = EditorGUILayout.Popup("Item", id, DataHolder.Items().GetNameList(true), GUILayout.Width(mWidth));
		}
		else if(ItemDropType.WEAPON.Equals(type))
		{
			id = EditorGUILayout.Popup("Weapon", id, DataHolder.Weapons().GetNameList(true), GUILayout.Width(mWidth));
		}
		else if(ItemDropType.ARMOR.Equals(type))
		{
			id = EditorGUILayout.Popup("Armor", id, DataHolder.Armors().GetNameList(true), GUILayout.Width(mWidth));
		}
		quantity = EditorGUILayout.IntField("Quantity", quantity, GUILayout.Width(mWidth));
	}
	
	public static void StatusTimeChange(ref StatusTimeChange stc)
	{
		stc.statusID = EditorGUILayout.Popup("Status value", stc.statusID, 
				DataHolder.StatusValues().GetNameList(true), GUILayout.Width(mWidth));
		if(!DataHolder.StatusValue(stc.statusID).IsConsumable())
		{
			stc.statusID = DataHolder.StatusValues().GetFirstConsumable();
		}
		stc.showText = EditorGUILayout.Toggle("Show text", stc.showText, GUILayout.Width(mWidth));
		stc.forcePerSecond = EditorGUILayout.Toggle("Force per second", stc.forcePerSecond, GUILayout.Width(mWidth));
		EditorGUILayout.Separator();
		
		stc.useFormula = EditorGUILayout.Toggle("Use formula", stc.useFormula, GUILayout.Width(mWidth));
		if(stc.useFormula)
		{
			stc.formulaID = EditorGUILayout.Popup("Formula", stc.formulaID, 
					DataHolder.Formulas().GetNameList(true), GUILayout.Width(mWidth));
		}
		else
		{
			stc.value = EditorGUILayout.FloatField("Value", stc.value, GUILayout.Width(mWidth));
		}
		EditorGUILayout.Separator();
		
		EditorGUILayout.BeginHorizontal();
		stc.minCheck = EditorGUILayout.Toggle("Minimum value", stc.minCheck, GUILayout.Width(mWidth));
		if(stc.minCheck)
		{
			stc.minValue = EditorGUILayout.IntField(stc.minValue, GUILayout.Width(mWidth*0.5f));
		}
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		stc.maxCheck = EditorGUILayout.Toggle("Maximum value", stc.maxCheck, GUILayout.Width(mWidth));
		if(stc.maxCheck)
		{
			stc.maxValue = EditorGUILayout.IntField(stc.maxValue, GUILayout.Width(mWidth*0.5f));
		}
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
	}
	
	public static void BattleAdvantage(ref BattleAdvantage ba)
	{
		ba.enabled = EditorGUILayout.Toggle("Enabled", ba.enabled, GUILayout.Width(mWidth));
		if(ba.enabled)
		{
			ba.chance = EditorGUILayout.FloatField("Chance", ba.chance, GUILayout.Width(mWidth));
			FloatHelper.ChanceLimit(ref ba.chance);
			
			ba.blockEscape = EditorGUILayout.Toggle("Block escape", ba.blockEscape, GUILayout.Width(mWidth));
			EditorGUILayout.Separator();
			
			ba.overrideText = EditorGUILayout.Toggle("Override text", ba.overrideText, GUILayout.Width(mWidth));
			if(ba.overrideText)
			{
				ba.textColor = EditorGUILayout.Popup("Text color", ba.textColor, 
						DataHolder.Colors().GetNameList(true), GUILayout.Width(mWidth));
				ba.shadowColor = EditorGUILayout.Popup("Shadow color", ba.shadowColor, 
						DataHolder.Colors().GetNameList(true), GUILayout.Width(mWidth));
				EditorGUILayout.Separator();
				
				EditorHelper.TextSettings(ref ba.text);
			}
			EditorGUILayout.Separator();
			
			EditorGUILayout.BeginVertical("box");
			GUILayout.Label("Party condition", EditorStyles.boldLabel);
			EditorHelper.GroupCondition(ref ba.partyCondition);
			EditorGUILayout.Separator();
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			GUILayout.Label("Enemies condition", EditorStyles.boldLabel);
			EditorHelper.GroupCondition(ref ba.enemiesCondition);
			EditorGUILayout.Separator();
			EditorGUILayout.EndVertical();
		}
	}
	
	public static void GroupCondition(ref GroupCondition gc)
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical();
		 if(DataHolder.BattleSystem().IsTurnBased())
		{
			gc.firstMove = EditorGUILayout.Toggle("First move", gc.firstMove, GUILayout.Width(mWidth));
			if(gc.firstMove)
			{
				gc.firstMoveRounds = EditorGUILayout.IntField("Rounds", gc.firstMoveRounds, GUILayout.Width(mWidth));
				if(gc.firstMoveRounds < 1) gc.firstMoveRounds = 1;
			}
		}
		else if(DataHolder.BattleSystem().IsActiveTime())
		{
			gc.timebar = EditorGUILayout.FloatField("Start timebar", gc.timebar, GUILayout.Width(mWidth));
		}
		for(int i=0; i<gc.setStatus.Length; i++)
		{
			if(DataHolder.StatusValue(i).IsConsumable())
			{
				EditorGUILayout.BeginHorizontal();
				gc.setStatus[i] = EditorGUILayout.Toggle("Set "+DataHolder.StatusValues().GetName(i), 
						gc.setStatus[i], GUILayout.Width(mWidth));
				
				if(gc.setStatus[i])
				{
					gc.status[i] = EditorGUILayout.IntField(gc.status[i], GUILayout.Width(mWidth*0.5f));
				}
				EditorGUILayout.EndHorizontal();
			}
			else gc.setStatus[i] = false;
		}
		EditorGUILayout.EndVertical();
		EditorGUILayout.Separator();
		
		EditorGUILayout.BeginVertical();
		for(int i=0; i<gc.effect.Length; i++)
		{
			gc.effect[i] = (SkillEffect)EditorTab.EnumToolbar(DataHolder.Effects().GetName(i), 
					(int)gc.effect[i], typeof(SkillEffect));
		}
		EditorGUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
	}
	
	public static void StatusCondition(ref StatusCondition condition)
	{
		condition.apply = EditorGUILayout.Toggle("Apply", condition.apply, GUILayout.Width(mWidth));
		if(condition.apply)
		{
			condition.stopChange = EditorGUILayout.Toggle("Stop changes", condition.stopChange, GUILayout.Width(mWidth));
			if(!condition.stopChange)
			{
				condition.value = EditorGUILayout.IntField("Value", condition.value, GUILayout.Width(mWidth));
				condition.simpleOperator = (SimpleOperator)EditorTab.EnumToolbar("Operator", (int)condition.simpleOperator, typeof(SimpleOperator));
				condition.setter = (ValueSetter)EditorTab.EnumToolbar("Set in", (int)condition.setter, typeof(ValueSetter));
				condition.execution = (StatusConditionExecution)EditorTab.EnumToolbar("Set on", (int)condition.execution, typeof(StatusConditionExecution));
				if(StatusConditionExecution.TIME.Equals(condition.execution))
				{
					condition.time = EditorGUILayout.IntField("Every (sec)", condition.time, GUILayout.Width(mWidth));
				}
			}
		}
	}
}
