
using UnityEditor;
using UnityEngine;

public class CharacterTab : BaseTab
{
	private GameObject tmpPrefab;
	
	public CharacterTab(ProjectWindow pw) : base(pw)
	{
		this.Reload();
	}
	
	new public void Reload()
	{
		base.Reload();
		this.tmpPrefab = null;
	}
	
	public void ShowTab()
	{
		int tmpSelection = selection;
		EditorGUILayout.BeginVertical();
		
		// buttons
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Add Character", GUILayout.Width(pw.mWidth2)))
		{
			DataHolder.Characters().filter.Deactivate();
			DataHolder.Characters().AddCharacter("New Character", "New Description",
						pw.GetLangCount(), pw.GetStatusValuesData());
			selection = DataHolder.Characters().GetDataCount()-1;
			GUI.FocusControl ("ID");
		}
		if(selection >= 0)
		{
			if(this.ShowCopyButton(DataHolder.Characters()))
			{
				DataHolder.Characters().filter.Deactivate();
			}
		}
		if(DataHolder.Characters().GetDataCount() > 1 && selection >= 0)
		{
			if(this.ShowRemButton("Remove Character", DataHolder.Characters()))
			{
				DataHolder.Characters().filter.Deactivate();
			}
		}
		this.CheckSelection(DataHolder.Characters());
		EditorGUILayout.EndHorizontal();
		
		// status value list
		this.AddItemListFilter(DataHolder.Characters(), 
				"race", DataHolder.Races().GetNameList(true), 
				"size", DataHolder.Sizes().GetNameList(true));
		
		// value settings
		EditorGUILayout.BeginVertical();
		SP2 = EditorGUILayout.BeginScrollView(SP2);
		if(DataHolder.Characters().GetDataCount() > 0 && selection >= 0)
		{
			this.AddID("Character ID");
			this.AddMultiLangIcon("Character Name", DataHolder.Characters());
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical("box");
			fold4 = EditorGUILayout.Foldout(fold4, "Base Settings");
			if(fold4)
			{
				if(selection != tmpSelection) this.tmpPrefab = null;
				EditorHelper.PrefabSettings("Prefab", ref DataHolder.Character(selection).prefabName, 
						ref this.tmpPrefab, CharacterData.PREFAB_PATH);
				DataHolder.Character(selection).prefabRoot = EditorGUILayout.TextField("Child as root", 
						DataHolder.Character(selection).prefabRoot, GUILayout.Width(pw.mWidth*2));
				EditorGUILayout.Separator();
				
				DataHolder.Character(selection).raceID = EditorGUILayout.Popup("Race", 
						DataHolder.Character(selection).raceID, DataHolder.Races().GetNameList(true), 
						GUILayout.Width(pw.mWidth*1.5f));
				DataHolder.Character(selection).sizeID = EditorGUILayout.Popup("Size", 
						DataHolder.Character(selection).sizeID, DataHolder.Sizes().GetNameList(true), 
						GUILayout.Width(pw.mWidth*1.5f));
				// update filter on change
				if((DataHolder.Characters().filter.useFilter[0] && 
					DataHolder.Character(selection).raceID != DataHolder.Characters().filter.filterID[0]) ||
					(DataHolder.Characters().filter.useFilter[1] && 
					DataHolder.Character(selection).sizeID != DataHolder.Characters().filter.filterID[1]))
				{
					DataHolder.Characters().filter.filterID[0] = DataHolder.Character(selection).raceID;
					DataHolder.Characters().filter.filterID[1] = DataHolder.Character(selection).sizeID;
					DataHolder.Characters().CreateFilterList(true);
				}
				
				DataHolder.Character(selection).currentClass = EditorGUILayout.Popup("Class", 
						DataHolder.Character(selection).currentClass, DataHolder.Classes().GetNameList(true), 
						GUILayout.Width(pw.mWidth*1.5f));
				
				EditorGUILayout.Separator();
				DataHolder.Character(selection).development.startLevel = EditorGUILayout.IntField("Start Level", 
						DataHolder.Character(selection).development.startLevel, GUILayout.Width(pw.mWidth));
				if(DataHolder.Character(selection).development.startLevel > DataHolder.Character(selection).development.maxLevel)
				{
					DataHolder.Character(selection).development.startLevel = DataHolder.Character(selection).development.maxLevel;
				}
				else if(DataHolder.Character(selection).development.startLevel < 1)
				{
					DataHolder.Character(selection).development.startLevel = 1;
				}
				DataHolder.Character(selection).development.maxLevel = EditorGUILayout.IntField("Max. Level", 
						DataHolder.Character(selection).development.maxLevel, GUILayout.Width(pw.mWidth));
				if(DataHolder.Character(selection).development.maxLevel < 1)
				{
					DataHolder.Character(selection).development.maxLevel = 1;
				}
				if(GUILayout.Button("Apply Changes", GUILayout.Width(pw.mWidth2)))
				{
					DataHolder.Character(selection).development.MaxLevelChanged();
				}
				EditorGUILayout.Separator();
				DataHolder.Character(selection).baseCounter = EditorGUILayout.Popup("Counter chance", 
						DataHolder.Character(selection).baseCounter, pw.GetFormulas(), GUILayout.Width(pw.mWidth*1.5f));
				DataHolder.Character(selection).baseCritical = EditorGUILayout.Popup("Critical chance", 
						DataHolder.Character(selection).baseCritical, pw.GetFormulas(), GUILayout.Width(pw.mWidth*1.5f));
				DataHolder.Character(selection).baseBlock = EditorGUILayout.Popup("Block chance", 
						DataHolder.Character(selection).baseBlock, pw.GetFormulas(), GUILayout.Width(pw.mWidth*1.5f));
				EditorGUILayout.Separator();
				DataHolder.Character(selection).noRevive = EditorGUILayout.Toggle("No revive", 
						DataHolder.Character(selection).noRevive, GUILayout.Width(pw.mWidth));
				DataHolder.Character(selection).leaveOnDeath = EditorGUILayout.Toggle("Leave on death", 
						DataHolder.Character(selection).leaveOnDeath, GUILayout.Width(pw.mWidth));
				
				EditorGUILayout.Separator();
				DataHolder.Character(selection).useMoveSpeedFormula = EditorGUILayout.Toggle("Move speed formula", 
						DataHolder.Character(selection).useMoveSpeedFormula, GUILayout.Width(pw.mWidth));
				if(DataHolder.Character(selection).useMoveSpeedFormula)
				{
					DataHolder.Character(selection).moveSpeedFormula = EditorGUILayout.Popup("Formula",
							DataHolder.Character(selection).moveSpeedFormula, 
							DataHolder.Formulas().GetNameList(true), GUILayout.Width(pw.mWidth));
				}
				else
				{
					DataHolder.Character(selection).moveSpeed = EditorGUILayout.FloatField("Move speed",
							DataHolder.Character(selection).moveSpeed, GUILayout.Width(pw.mWidth));
					if(DataHolder.Character(selection).moveSpeed <= 0)
					{
						DataHolder.Character(selection).moveSpeed = 0.1f;
					}
				}
				DataHolder.Character(selection).minMoveSpeed = EditorGUILayout.FloatField("Minimum speed",
						DataHolder.Character(selection).minMoveSpeed, GUILayout.Width(pw.mWidth));
				if(DataHolder.Character(selection).minMoveSpeed <= 0)
				{
					DataHolder.Character(selection).minMoveSpeed = 0.1f;
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold9 = EditorGUILayout.Foldout(fold9, "AI Mover Settings");
			if(fold9)
			{
				DataHolder.Character(selection).aiMoverSettings = EditorHelper.AIMoverSettings(
						DataHolder.Character(selection).aiMoverSettings, 
						DataHolder.GameSettings().partyFollow, false);
			}
			EditorGUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			
			EditorGUILayout.BeginVertical("box");
			fold16 = EditorGUILayout.Foldout(fold16, "Bonus/difficulty settings");
			if(fold16)
			{
				EditorHelper.BonusSettings(ref DataHolder.Character(selection).bonus, false);
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			EditorGUILayout.BeginVertical("box", GUILayout.MinWidth(pw.mWidth));
			fold6 = EditorGUILayout.Foldout(fold6, "Attack Settings");
			if(fold6)
			{
				DataHolder.Character(selection).baseElement = EditorGUILayout.Popup(
						"Attack element", DataHolder.Character(selection).baseElement, 
						DataHolder.Elements().GetNameList(true), GUILayout.Width(pw.mWidth));
				EditorGUILayout.Separator();
				
				if(GUILayout.Button("Add", GUILayout.Width(pw.mWidth3)))
				{
					DataHolder.Character(selection).AddBaseAttack();
				}
				for(int i=0; i<DataHolder.Character(selection).baseAttack.Length; i++)
				{
					EditorGUILayout.BeginHorizontal();
					DataHolder.Character(selection).baseAttack[i] = EditorGUILayout.Popup("Attack "+(i+1), 
							DataHolder.Character(selection).baseAttack[i], DataHolder.BaseAttacks().GetNameList(true),
							GUILayout.Width(pw.mWidth));
					
					if(DataHolder.Character(selection).baseAttack.Length > 1)
					{
						if(GUILayout.Button("Remove", GUILayout.Width(pw.mWidth3)))
						{
							DataHolder.Character(selection).RemoveBaseAttack(i);
							break;
						}
					}
					GUILayout.FlexibleSpace();
					EditorGUILayout.EndHorizontal();
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			if(DataHolder.BattleSystem().dynamicCombat ||
				(DataHolder.BattleSystem().IsRealTime() &&
				DataHolder.BattleControl().usePartyTarget &&
				DataHolder.BattleControl().autoAttackTarget))
			{
				EditorGUILayout.BeginVertical("box");
				fold8 = EditorGUILayout.Foldout(fold8, "Auto attack Settings");
				if(fold8)
				{
					DataHolder.Character(selection).autoAttack = EditorHelper.AutoAttackSettings(
							DataHolder.Character(selection).autoAttack);
					EditorGUILayout.Separator();
				}
				EditorGUILayout.EndVertical();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box", GUILayout.MinWidth(pw.mWidth));
			fold7 = EditorGUILayout.Foldout(fold7, "Battle Audio Settings");
			if(fold7)
			{
				for(int i=0; i<DataHolder.Character(selection).audioClipName.Length; i++)
				{
					if(DataHolder.Character(selection).audioClip[i] == null && 
						DataHolder.Character(selection).audioClipName[i] != null && 
						"" != DataHolder.Character(selection).audioClipName[i])
					{
						DataHolder.Character(selection).audioClip[i] = (AudioClip)Resources.Load(
								BattleSystemData.AUDIO_PATH+
								DataHolder.Character(selection).audioClipName[i], typeof(AudioClip));
					}
					DataHolder.Character(selection).audioClip[i] = (AudioClip)EditorGUILayout.ObjectField("Clip "+i, 
							DataHolder.Character(selection).audioClip[i], typeof(AudioClip), false, GUILayout.Width(pw.mWidth*1.2f));
					if(DataHolder.Character(selection).audioClip[i])
					{
						DataHolder.Character(selection).audioClipName[i] = DataHolder.Character(selection).audioClip[i].name;
					}
					else DataHolder.Character(selection).audioClipName[i] = "";
				}
				EditorGUILayout.Separator();
			}
			EditorGUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginVertical("box");
			fold12 = EditorGUILayout.Foldout(fold12, "Field animation settings");
			if(fold12)
			{
				DataHolder.Character(selection).fieldAnimations = EditorHelper.FieldAnimationsSettings(
						DataHolder.Character(selection).fieldAnimations);
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold5 = EditorGUILayout.Foldout(fold5, "Battle animation settings");
			if(fold5)
			{
				DataHolder.Character(selection).battleAnimations = EditorHelper.BaseAnimationsSettings(
						DataHolder.Character(selection).battleAnimations, true, true);
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold12 = EditorGUILayout.Foldout(fold12, "Custom animation settings");
			if(fold12)
			{
				DataHolder.Character(selection).customAnimations = EditorHelper.CustomAnimationSettings(
						DataHolder.Character(selection).customAnimations);
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical("box");
			fold2 = EditorGUILayout.Foldout(fold2, "Status Value Development");
			if(fold2)
			{
				StatusValue[] sv = pw.GetStatusValuesData();
				for(int i=0; i<pw.GetStatusValueCount(); i++)
				{
					if(!sv[i].IsConsumable())
					{
						GUILayout.Label (pw.GetStatusValue(i), EditorStyles.boldLabel);
						if(GUILayout.Button("Edit", GUILayout.Width(pw.mWidth2)))
						{
							StatusCurveWindow.Init(pw.GetStatusValue(i)+" Curve",
									ref DataHolder.Character(selection).development.statusValue[i].levelValue, sv[i]);
						}
					}
				}
			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.Separator();
			EditorGUILayout.BeginVertical("box");
			fold3 = EditorGUILayout.Foldout(fold3, "Skill Learning");
			if(fold3)
			{
				if(GUILayout.Button("Add", GUILayout.Width(pw.mWidth3)))
				{
					DataHolder.Characters().AddLearnSkill(selection);
				}
				for(int i=0; i<DataHolder.Character(selection).development.skill.Length; i++)
				{
					EditorGUILayout.Separator();
					EditorGUILayout.BeginHorizontal();
					DataHolder.Character(selection).development.skill[i].atLevel = EditorGUILayout.IntField("At level", 
							DataHolder.Character(selection).development.skill[i].atLevel, GUILayout.Width(pw.mWidth*0.7f));
					DataHolder.Character(selection).development.skill[i].atLevel = this.MinMaxCheck(
							DataHolder.Character(selection).development.skill[i].atLevel, 1, DataHolder.Character(selection).development.maxLevel);
					DataHolder.Character(selection).development.skill[i].skillID = EditorGUILayout.Popup(
							DataHolder.Character(selection).development.skill[i].skillID, pw.GetSkills(), GUILayout.Width(pw.mWidth*0.5f));
					
					DataHolder.Character(selection).development.skill[i].skillLevel = EditorGUILayout.IntField("Skill level", 
							DataHolder.Character(selection).development.skill[i].skillLevel, GUILayout.Width(pw.mWidth*0.7f));
					DataHolder.Character(selection).development.skill[i].skillLevel = this.MinMaxCheck(
							DataHolder.Character(selection).development.skill[i].skillLevel, 1, 
							DataHolder.Skill(DataHolder.Character(selection).development.skill[i].skillID).level.Length);
					
					if(GUILayout.Button("Remove", GUILayout.Width(pw.mWidth3)))
					{
						DataHolder.Characters().RemoveLearnSkill(selection, i);
					}
					EditorGUILayout.EndHorizontal();
				}
			}
			EditorGUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			this.Separate();
			
			
			EditorGUILayout.BeginVertical("box");
			fold17 = EditorGUILayout.Foldout(fold17, "Status value time changes");
			if(fold17)
			{
				GUILayout.Label("Battle", EditorStyles.boldLabel);
				if(GUILayout.Button("Add", GUILayout.Width(pw.mWidth3)))
				{
					DataHolder.Character(selection).battleStatusChange = ArrayHelper.Add(new StatusTimeChange(), 
							DataHolder.Character(selection).battleStatusChange);
				}
				for(int i=0; i<DataHolder.Character(selection).battleStatusChange.Length; i++)
				{
					EditorGUILayout.BeginVertical("box");
					if(GUILayout.Button("Remove", GUILayout.Width(pw.mWidth3)))
					{
						DataHolder.Character(selection).battleStatusChange = ArrayHelper.Remove(i, 
								DataHolder.Character(selection).battleStatusChange);
						break;
					}
					EditorHelper.StatusTimeChange(ref DataHolder.Character(selection).battleStatusChange[i]);
					EditorGUILayout.EndVertical();
				}
				EditorGUILayout.Separator();
				
				GUILayout.Label("Field", EditorStyles.boldLabel);
				if(GUILayout.Button("Add", GUILayout.Width(pw.mWidth3)))
				{
					DataHolder.Character(selection).fieldStatusChange = ArrayHelper.Add(new StatusTimeChange(), 
							DataHolder.Character(selection).fieldStatusChange);
				}
				for(int i=0; i<DataHolder.Character(selection).fieldStatusChange.Length; i++)
				{
					EditorGUILayout.BeginVertical("box");
					if(GUILayout.Button("Remove", GUILayout.Width(pw.mWidth3)))
					{
						DataHolder.Character(selection).fieldStatusChange = ArrayHelper.Remove(i, 
								DataHolder.Character(selection).fieldStatusChange);
						break;
					}
					EditorHelper.StatusTimeChange(ref DataHolder.Character(selection).fieldStatusChange[i]);
					EditorGUILayout.EndVertical();
				}
				
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			
			fold10 = EditorGUILayout.Foldout(fold10, "AI Settings");
			if(fold10)
			{
				DataHolder.Character(selection).aiControlled = EditorGUILayout.Toggle("AI controlled",
						DataHolder.Character(selection).aiControlled, GUILayout.Width(pw.mWidth));
				if(DataHolder.Character(selection).aiControlled)
				{
					if(DataHolder.BattleSystem().IsRealTime())
					{
						DataHolder.Character(selection).attackPartyTarget = EditorGUILayout.Toggle("Atk party target",
								DataHolder.Character(selection).attackPartyTarget, GUILayout.Width(pw.mWidth));
					}
					DataHolder.Character(selection).attackLastTarget = EditorGUILayout.Toggle("Atk last target",
							DataHolder.Character(selection).attackLastTarget, GUILayout.Width(pw.mWidth));
					DataHolder.Character(selection).aiNearestTarget = EditorGUILayout.Toggle("Nearest target",
							DataHolder.Character(selection).aiNearestTarget, GUILayout.Width(pw.mWidth));
					if(DataHolder.BattleSystem().IsRealTime())
					{
						DataHolder.Character(selection).aiTimeout = EditorGUILayout.FloatField("Timeout",
								DataHolder.Character(selection).aiTimeout, GUILayout.Width(pw.mWidth));
					}
					
					if(GUILayout.Button("Add Behaviour", GUILayout.Width(pw.mWidth2)))
					{
						DataHolder.Character(selection).AddAIBehaviour();
					}
					for(int i=0; i<DataHolder.Character(selection).aiBehaviour.Length; i++)
					{
						EditorGUILayout.BeginVertical("box");
						EditorGUILayout.BeginHorizontal();
						if(GUILayout.Button("Remove", GUILayout.Width(pw.mWidth3)))
						{
							DataHolder.Character(selection).RemoveAIBehaviour(i);
							break;
						}
						if(i > 0)
						{
							if(GUILayout.Button("Move up", GUILayout.Width(pw.mWidth3)))
							{
								DataHolder.Character(selection).MoveAIBehaviour(i, -1);
							}
						}
						if(i < DataHolder.Character(selection).aiBehaviour.Length-1)
						{
							if(GUILayout.Button("Move down", GUILayout.Width(pw.mWidth3)))
							{
								DataHolder.Character(selection).MoveAIBehaviour(i, 1);
							}
						}
						EditorGUILayout.EndHorizontal();
						DataHolder.Character(selection).aiBehaviour[i] = EditorHelper.AIBehaviourSettings(
								DataHolder.Character(selection).aiBehaviour[i], i);
						EditorGUILayout.EndVertical();
					}
				}
				this.Separate();
			}
			
			if(DataHolder.BattleSystem().IsRealTime())
			{
				EditorGUILayout.BeginVertical("box");
				fold11 = EditorGUILayout.Foldout(fold11, "Control map settings");
				if(fold11)
				{
					DataHolder.Character(selection).controlMap = EditorHelper.CharacterControlMapSettings(
							DataHolder.Character(selection).controlMap);
				}
				EditorGUILayout.EndVertical();
			}
			
			EditorGUILayout.EndVertical();
		}
		this.EndTab();
	}
}