
using UnityEditor;
using UnityEngine;

public class StatusEffectTab : BaseTab
{
	public StatusEffectTab(ProjectWindow pw) : base(pw)
	{
		this.Reload();
	}
	
	public void ShowTab()
	{
		EditorGUILayout.BeginVertical();
		
		// buttons
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Add Effect", GUILayout.Width(pw.mWidth2)))
		{
			DataHolder.Effects().AddEffect("New Effect", "New Description", 
						pw.GetLangCount(), pw.GetStatusValueCount(),
						pw.GetElementCount());
			selection = DataHolder.Effects().GetDataCount()-1;
			pw.AddStatusEffect(selection);
			GUI.FocusControl ("ID");
		}
		if(this.ShowCopyButton(DataHolder.Effects()))
		{
			pw.AddStatusEffect(selection);
		}
		if(DataHolder.Effects().GetDataCount() > 1)
		{
			if(this.ShowRemButton("Remove Effect", DataHolder.Effects()))
			{
				pw.RemoveStatusEffect(selection);
			}
		}
		this.CheckSelection(DataHolder.Effects());
		EditorGUILayout.EndHorizontal();
		
		// status effect list
		this.AddItemList(DataHolder.Effects());
		
		// effect settings
		EditorGUILayout.BeginVertical();
		SP2 = EditorGUILayout.BeginScrollView(SP2);
		if(DataHolder.Effects().GetDataCount() > 0)
		{
			this.AddID("Effect ID");
			this.AddMultiLangIcon("Effect Name", DataHolder.Effects());
			
			EditorGUILayout.BeginVertical("box");
			fold2 = EditorGUILayout.Foldout(fold2, "Effect Settings");
			if(fold2)
			{
				DataHolder.Effect(selection).stopMove = EditorGUILayout.Toggle("Stop move", 
						DataHolder.Effect(selection).stopMove, GUILayout.Width(pw.mWidth));
				if(DataHolder.Effect(selection).stopMove)
				{
					DataHolder.Effect(selection).stopMovement = EditorGUILayout.Toggle("Stop movement", 
							DataHolder.Effect(selection).stopMovement, GUILayout.Width(pw.mWidth));
				}
				else DataHolder.Effect(selection).stopMovement = false;
				
				DataHolder.Effect(selection).autoAttack = EditorGUILayout.Toggle("Auto attack", 
						DataHolder.Effect(selection).autoAttack, GUILayout.Width(pw.mWidth));
				DataHolder.Effect(selection).attackFriends = EditorGUILayout.Toggle("Attack friends", 
						DataHolder.Effect(selection).attackFriends, GUILayout.Width(pw.mWidth));
				DataHolder.Effect(selection).blockAttack = EditorGUILayout.Toggle("Block attack", 
						DataHolder.Effect(selection).blockAttack, GUILayout.Width(pw.mWidth));
				DataHolder.Effect(selection).blockSkills = EditorGUILayout.Toggle("Block skills", 
						DataHolder.Effect(selection).blockSkills, GUILayout.Width(pw.mWidth));
				DataHolder.Effect(selection).blockItems = EditorGUILayout.Toggle("Block items", 
						DataHolder.Effect(selection).blockItems, GUILayout.Width(pw.mWidth));
				DataHolder.Effect(selection).blockDefend = EditorGUILayout.Toggle("Block defend", 
						DataHolder.Effect(selection).blockDefend, GUILayout.Width(pw.mWidth));
				DataHolder.Effect(selection).blockEscape = EditorGUILayout.Toggle("Block escape", 
						DataHolder.Effect(selection).blockEscape, GUILayout.Width(pw.mWidth));
				DataHolder.Effect(selection).reflectSkills = EditorGUILayout.Toggle("Reflect skills", 
						DataHolder.Effect(selection).reflectSkills, GUILayout.Width(pw.mWidth));
				EditorGUILayout.Separator();
				
				EditorGUILayout.BeginHorizontal();
				DataHolder.Effect(selection).setElement = EditorGUILayout.BeginToggleGroup("Set attack element", DataHolder.Effect(selection).setElement);
				DataHolder.Effect(selection).attackElement = EditorGUILayout.Popup(DataHolder.Effect(selection).attackElement, pw.GetElements(), GUILayout.Width(pw.mWidth*0.7f));
				EditorGUILayout.EndToggleGroup();
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Separator();
				
				EditorGUILayout.BeginHorizontal();
				DataHolder.Effect(selection).hitChance = EditorGUILayout.BeginToggleGroup("Calculate hit chance", DataHolder.Effect(selection).hitChance);
				DataHolder.Effect(selection).hitFormula = EditorGUILayout.Popup(DataHolder.Effect(selection).hitFormula, pw.GetFormulas(), GUILayout.Width(pw.mWidth*0.7f));
				EditorGUILayout.EndToggleGroup();
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Separator();
				
				GUILayout.Label("Effect End", EditorStyles.boldLabel);
				DataHolder.Effect(selection).endWithBattle = EditorGUILayout.Toggle("End with battle", DataHolder.Effect(selection).endWithBattle, GUILayout.Width(pw.mWidth));
				DataHolder.Effect(selection).endOnAttack = EditorGUILayout.Toggle("End on attack", DataHolder.Effect(selection).endOnAttack, GUILayout.Width(pw.mWidth));
				DataHolder.Effect(selection).end = (StatusEffectEnd)this.EnumToolbar("End after", (int)DataHolder.Effect(selection).end, typeof(StatusEffectEnd));
				if(StatusEffectEnd.TURN.Equals(DataHolder.Effect(selection).end))
				{
					DataHolder.Effect(selection).endValue = EditorGUILayout.IntField("Turns", DataHolder.Effect(selection).endValue, GUILayout.Width(pw.mWidth));
					DataHolder.Effect(selection).endChance = EditorGUILayout.IntField("Chance (%)", DataHolder.Effect(selection).endChance, GUILayout.Width(pw.mWidth));
				}
				else if(StatusEffectEnd.TIME.Equals(DataHolder.Effect(selection).end))
				{
					DataHolder.Effect(selection).endValue = EditorGUILayout.IntField("Time (sec)", DataHolder.Effect(selection).endValue, GUILayout.Width(pw.mWidth));
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			
			EditorGUILayout.BeginVertical("box");
			fold16 = EditorGUILayout.Foldout(fold16, "Bonus/difficulty settings");
			if(fold16)
			{
				EditorHelper.BonusSettings(ref DataHolder.Effect(selection).bonus, false);
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			
			EditorGUILayout.BeginVertical("box");
			fold11 = EditorGUILayout.Foldout(fold11, "Block attack/skill type settings");
			if(fold11)
			{
				DataHolder.Effect(selection).blockBaseAttacks = EditorGUILayout.Toggle(
							"Block attacks", DataHolder.Effect(selection).blockBaseAttacks, 
							GUILayout.Width(pw.mWidth));
				EditorGUILayout.Separator();
				
				GUILayout.Label("Block skill types", EditorStyles.boldLabel);
				for(int i=0; i<DataHolder.SkillTypeCount; i++)
				{
					DataHolder.Effect(selection).skillTypeBlock[i] = EditorGUILayout.Toggle(
							DataHolder.SkillTypes().GetName(i), 
							DataHolder.Effect(selection).skillTypeBlock[i], 
							GUILayout.Width(pw.mWidth));
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			
			EditorGUILayout.BeginVertical("box");
			fold3 = EditorGUILayout.Foldout(fold3, "Status Conditions");
			if(fold3)
			{
				for(int i=0; i<DataHolder.StatusValueCount; i++)
				{
					EditorGUILayout.BeginVertical("box");
					GUILayout.Label(DataHolder.StatusValues().GetName(i), EditorStyles.boldLabel);
					EditorHelper.StatusCondition(ref DataHolder.Effect(selection).condition[i]);
					EditorGUILayout.Separator();
					EditorGUILayout.EndVertical();
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical("box");
			fold4= EditorGUILayout.Foldout(fold4, "Element effectiveness");
			if(fold4)
			{
				for(int i=0; i<pw.GetElementCount(); i++)
				{
					EditorGUILayout.BeginHorizontal();
					DataHolder.Effect(selection).elementValue[i] = EditorGUILayout.IntField(pw.GetElement(i), DataHolder.Effect(selection).elementValue[i], GUILayout.Width(pw.mWidth));
					DataHolder.Effect(selection).elementOperator[i] = (SimpleOperator)this.EnumToolbar("", (int)DataHolder.Effect(selection).elementOperator[i], typeof(SimpleOperator));
					EditorGUILayout.EndHorizontal();
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold9= EditorGUILayout.Foldout(fold9, "Race damage factor changes");
			if(fold9)
			{
				for(int i=0; i<DataHolder.Effect(selection).raceValue.Length; i++)
				{
					DataHolder.Effect(selection).raceValue[i] = EditorGUILayout.IntField(DataHolder.Race(i), 
							DataHolder.Effect(selection).raceValue[i], GUILayout.Width(pw.mWidth));
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold10= EditorGUILayout.Foldout(fold10, "Size damage factor changes");
			if(fold10)
			{
				for(int i=0; i<DataHolder.Effect(selection).sizeValue.Length; i++)
				{
					DataHolder.Effect(selection).sizeValue[i] = EditorGUILayout.IntField(DataHolder.Size(i), 
							DataHolder.Effect(selection).sizeValue[i], GUILayout.Width(pw.mWidth));
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginVertical("box");
			fold5= EditorGUILayout.Foldout(fold5, "Auto apply");
			if(fold5)
			{
				DataHolder.Effect(selection).autoApply = EditorGUILayout.Toggle("Use auto apply", DataHolder.Effect(selection).autoApply, GUILayout.Width(pw.mWidth));
				if(DataHolder.Effect(selection).autoApply)
				{
					DataHolder.Effect(selection).applyNeeded = (AIConditionNeeded)this.EnumToolbar("Needed", 
							(int)DataHolder.Effect(selection).applyNeeded, typeof(AIConditionNeeded));
					EditorGUILayout.Separator();
					if(GUILayout.Button("Add", GUILayout.Width(pw.mWidth3)))
					{
						DataHolder.Effect(selection).AddAutoApply();
					}
					for(int i=0; i<DataHolder.Effect(selection).applyRequirement.Length; i++)
					{
						EditorGUILayout.BeginVertical("box");
						if(GUILayout.Button("Remove", GUILayout.Width(pw.mWidth3)))
						{
							DataHolder.Effect(selection).RemoveAutoApply(i);
							return;
						}
						DataHolder.Effect(selection).applyRequirement[i] = EditorHelper.StatusRequirementSettings(
								DataHolder.Effect(selection).applyRequirement[i]);
						EditorGUILayout.EndVertical();
					}
				}
				EditorGUILayout.Separator();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold6= EditorGUILayout.Foldout(fold6, "Auto remove");
			if(fold6)
			{
				DataHolder.Effect(selection).autoRemove = EditorGUILayout.Toggle("Use auto remove", DataHolder.Effect(selection).autoRemove, GUILayout.Width(pw.mWidth));
				if(DataHolder.Effect(selection).autoRemove)
				{
					DataHolder.Effect(selection).removeNeeded = (AIConditionNeeded)this.EnumToolbar("Needed", 
							(int)DataHolder.Effect(selection).removeNeeded, typeof(AIConditionNeeded));
					EditorGUILayout.Separator();
					if(GUILayout.Button("Add", GUILayout.Width(pw.mWidth3)))
					{
						DataHolder.Effect(selection).AddAutoRemove();
					}
					for(int i=0; i<DataHolder.Effect(selection).removeRequirement.Length; i++)
					{
						EditorGUILayout.BeginVertical("box");
						if(GUILayout.Button("Remove", GUILayout.Width(pw.mWidth3)))
						{
							DataHolder.Effect(selection).RemoveAutoRemove(i);
							return;
						}
						DataHolder.Effect(selection).removeRequirement[i] = EditorHelper.StatusRequirementSettings(
								DataHolder.Effect(selection).removeRequirement[i]);
						EditorGUILayout.EndVertical();
					}
				}
				EditorGUILayout.Separator();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold7= EditorGUILayout.Foldout(fold7, "Spawn prefabs");
			if(fold7)
			{
				if(GUILayout.Button("Add", GUILayout.Width(pw.mWidth3)))
				{
					DataHolder.Effect(selection).AddPrefab();
				}
				for(int i=0; i<DataHolder.Effect(selection).prefab.Length; i++)
				{
					EditorGUILayout.BeginVertical("box");
					if(GUILayout.Button("Remove", GUILayout.Width(pw.mWidth3)))
					{
						DataHolder.Effect(selection).RemovePrefab(i);
						return;
					}
					DataHolder.Effect(selection).prefab[i] = EditorHelper.EffectPrefabSettings(
							DataHolder.Effect(selection).prefab[i]);
					EditorGUILayout.EndVertical();
				}
				EditorGUILayout.Separator();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold8 = EditorGUILayout.Foldout(fold8, "End effect casts");
			if(fold8)
			{
				if(GUILayout.Button("Add", GUILayout.Width(pw.mWidth3)))
				{
					DataHolder.Effect(selection).AddEndChange();
				}
				for(int i=0; i<DataHolder.Effect(selection).effectChangeID.Length; i++)
				{
					EditorGUILayout.BeginHorizontal("box");
					if(GUILayout.Button("Remove", GUILayout.Width(pw.mWidth3)))
					{
						DataHolder.Effect(selection).RemoveEndChange(i);
						return;
					}
					DataHolder.Effect(selection).effectChangeID[i] = EditorGUILayout.Popup(
							DataHolder.Effect(selection).effectChangeID[i], DataHolder.Effects().GetNameList(true), 
					        GUILayout.Width(pw.mWidth));
					DataHolder.Effect(selection).endEffectChanges[i] = (SkillEffect)BaseTab.EnumToolbar("", 
							(int)DataHolder.Effect(selection).endEffectChanges[i], typeof(SkillEffect));
					EditorGUILayout.EndHorizontal();
				}
				EditorGUILayout.Separator();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.EndVertical();
		}
		this.EndTab();
	}
}