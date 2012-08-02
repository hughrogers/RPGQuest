
using UnityEditor;
using UnityEngine;

public class BattleAITab : BaseTab
{
	public BattleAITab(ProjectWindow pw) : base(pw)
	{
		this.Reload();
	}
	
	public void ShowTab()
	{
		EditorGUILayout.BeginVertical();
		
		// buttons
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Add AI", GUILayout.Width(pw.mWidth2)))
		{
			DataHolder.BattleAIs().AddAI("New AI");
			selection = DataHolder.BattleAIs().GetDataCount()-1;
			GUI.FocusControl ("ID");
		}
		this.ShowCopyButton(DataHolder.BattleAIs());
		if(DataHolder.BattleAIs().GetDataCount() > 1)
		{
			if(this.ShowRemButton("Remove AI", DataHolder.BattleAIs()))
			{
				pw.RemoveEnemyAI(selection);
			}
		}
		this.CheckSelection(DataHolder.BattleAIs());
		EditorGUILayout.EndHorizontal();
		
		// status value list
		this.AddItemList(DataHolder.BattleAIs());
		
		// value settings
		EditorGUILayout.BeginVertical();
		SP2 = EditorGUILayout.BeginScrollView(SP2);
		if(DataHolder.BattleAIs().GetDataCount() > 0)
		{
			this.AddID("AI ID");
			DataHolder.BattleAIs().name[selection] = EditorGUILayout.TextField("Name", DataHolder.BattleAIs().name[selection]);
			this.Separate();
			DataHolder.BattleAI(selection).needed = (AIConditionNeeded)this.EnumToolbar("Needed", 
					(int)DataHolder.BattleAI(selection).needed, typeof(AIConditionNeeded));
			EditorGUILayout.Separator();
			
			fold1 = EditorGUILayout.Foldout(fold1, "Conditions");
			if(fold1)
			{
				if(GUILayout.Button("Add Condition", GUILayout.Width(pw.mWidth2)))
				{
					DataHolder.BattleAIs().AddCondition(selection);
				}
				
				for(int i=0; i<DataHolder.BattleAI(selection).condition.Length; i++)
				{
					EditorGUILayout.Separator();
					EditorGUILayout.BeginVertical("box");
					GUILayout.Label ("Condition "+(i+1).ToString(), EditorStyles.boldLabel);
					if(GUILayout.Button("Remove Condition", GUILayout.Width(pw.mWidth2)))
					{
						DataHolder.BattleAIs().RemoveCondition(selection, i);
						break;
					}
					
					DataHolder.BattleAI(selection).condition[i].target = (AIConditionTarget)this.EnumToolbar("Target", 
							(int)DataHolder.BattleAI(selection).condition[i].target, typeof(AIConditionTarget));
					EditorGUILayout.Separator();
					DataHolder.BattleAI(selection).condition[i].type = (AIConditionType)this.EnumToolbar("", 
							(int)DataHolder.BattleAI(selection).condition[i].type, typeof(AIConditionType), (int)(pw.mWidth*2.5f));
					EditorGUILayout.Separator();
					if(AIConditionType.STATUS.Equals(DataHolder.BattleAI(selection).condition[i].type))
					{
						EditorGUILayout.BeginHorizontal();
						DataHolder.BattleAI(selection).condition[i].statusID = EditorGUILayout.Popup("Status Value", 
								DataHolder.BattleAI(selection).condition[i].statusID, pw.GetStatusValues(), GUILayout.Width(pw.mWidth));
						DataHolder.BattleAI(selection).condition[i].statusSetter = (ValueSetter)this.EnumToolbar("", 
								(int)DataHolder.BattleAI(selection).condition[i].statusSetter, typeof(ValueSetter));
						EditorGUILayout.EndHorizontal();
						if(ValueSetter.VALUE.Equals(DataHolder.BattleAI(selection).condition[i].statusSetter))
						{
							EditorGUILayout.BeginHorizontal();
							DataHolder.BattleAI(selection).condition[i].statusValue = EditorGUILayout.IntField("Value", 
									DataHolder.BattleAI(selection).condition[i].statusValue, GUILayout.Width(pw.mWidth));
							DataHolder.BattleAI(selection).condition[i].statusCheck = (ValueCheck)this.EnumToolbar("", 
									(int)DataHolder.BattleAI(selection).condition[i].statusCheck, typeof(ValueCheck));
							EditorGUILayout.EndHorizontal();
						}
						else if(ValueSetter.PERCENT.Equals(DataHolder.BattleAI(selection).condition[i].statusSetter))
						{
							DataHolder.BattleAI(selection).condition[i].checkStatusID = EditorGUILayout.Popup("Check Against", 
									DataHolder.BattleAI(selection).condition[i].checkStatusID, pw.GetStatusValues(), GUILayout.Width(pw.mWidth));
							EditorGUILayout.BeginHorizontal();
							DataHolder.BattleAI(selection).condition[i].statusValue = EditorGUILayout.IntField("Percent", 
									DataHolder.BattleAI(selection).condition[i].statusValue, GUILayout.Width(pw.mWidth));
							DataHolder.BattleAI(selection).condition[i].statusCheck = (ValueCheck)this.EnumToolbar("", 
									(int)DataHolder.BattleAI(selection).condition[i].statusCheck, typeof(ValueCheck));
							EditorGUILayout.EndHorizontal();
						}
					}
					else if(AIConditionType.EFFECT.Equals(DataHolder.BattleAI(selection).condition[i].type))
					{
						EditorGUILayout.BeginHorizontal();
						DataHolder.BattleAI(selection).condition[i].effectID = EditorGUILayout.Popup("Status Effect", 
								DataHolder.BattleAI(selection).condition[i].effectID, pw.GetStatusEffects(), GUILayout.Width(pw.mWidth));
						DataHolder.BattleAI(selection).condition[i].effectActive = (ActiveSelection)this.EnumToolbar("", 
								(int)DataHolder.BattleAI(selection).condition[i].effectActive, typeof(ActiveSelection));
						EditorGUILayout.EndHorizontal();
					}
					else if(AIConditionType.ELEMENT.Equals(DataHolder.BattleAI(selection).condition[i].type))
					{
						DataHolder.BattleAI(selection).condition[i].elementID = EditorGUILayout.Popup("Element", 
								DataHolder.BattleAI(selection).condition[i].elementID, pw.GetElements(), GUILayout.Width(pw.mWidth));
						EditorGUILayout.BeginHorizontal();
						DataHolder.BattleAI(selection).condition[i].elementValue = EditorGUILayout.IntField("Value", 
								DataHolder.BattleAI(selection).condition[i].elementValue, GUILayout.Width(pw.mWidth));
						DataHolder.BattleAI(selection).condition[i].elementCheck = (ValueCheck)this.EnumToolbar("", 
								(int)DataHolder.BattleAI(selection).condition[i].elementCheck, typeof(ValueCheck));
						EditorGUILayout.EndHorizontal();
					}
					else if(AIConditionType.TURN.Equals(DataHolder.BattleAI(selection).condition[i].type))
					{
						DataHolder.BattleAI(selection).condition[i].turn = EditorGUILayout.IntField("Turn", 
								DataHolder.BattleAI(selection).condition[i].turn, GUILayout.Width(pw.mWidth));
						DataHolder.BattleAI(selection).condition[i].everyTurn = EditorGUILayout.Toggle("Every n Turns", 
								DataHolder.BattleAI(selection).condition[i].everyTurn, GUILayout.Width(pw.mWidth));
					}
					else if(AIConditionType.CHANCE.Equals(DataHolder.BattleAI(selection).condition[i].type))
					{
						DataHolder.BattleAI(selection).condition[i].chance = EditorGUILayout.IntField("Chance (%)", 
								DataHolder.BattleAI(selection).condition[i].chance, GUILayout.Width(pw.mWidth));
						DataHolder.BattleAI(selection).condition[i].chance = this.ChanceCheck(DataHolder.BattleAI(selection).condition[i].chance);
					}
					else if(AIConditionType.DEATH.Equals(DataHolder.BattleAI(selection).condition[i].type))
					{
						DataHolder.BattleAI(selection).condition[i].isDead = EditorGUILayout.Toggle("Is dead", 
								DataHolder.BattleAI(selection).condition[i].isDead, GUILayout.Width(pw.mWidth));
					}
					else if(AIConditionType.RACE.Equals(DataHolder.BattleAI(selection).condition[i].type))
					{
						DataHolder.BattleAI(selection).condition[i].raceID = EditorGUILayout.Popup("Race", 
								DataHolder.BattleAI(selection).condition[i].raceID, DataHolder.Races().GetNameList(true), 
								GUILayout.Width(pw.mWidth*1.5f));
					}
					else if(AIConditionType.SIZE.Equals(DataHolder.BattleAI(selection).condition[i].type))
					{
						DataHolder.BattleAI(selection).condition[i].sizeID = EditorGUILayout.Popup("Size", 
								DataHolder.BattleAI(selection).condition[i].sizeID, DataHolder.Sizes().GetNameList(true), 
								GUILayout.Width(pw.mWidth*1.5f));
					}
					EditorGUILayout.Separator();
					EditorGUILayout.EndVertical();
				}
				this.Separate();
			}
			
			EditorGUILayout.EndVertical();
		}
		this.EndTab();
	}
}