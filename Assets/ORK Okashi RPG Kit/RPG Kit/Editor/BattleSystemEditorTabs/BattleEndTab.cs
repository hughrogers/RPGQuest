
using UnityEditor;
using UnityEngine;
using System.Collections;

public class BattleEndTab : EditorTab
{
	private BattleSystemWindow pw;
	
	public BattleEndTab(BattleSystemWindow pw) : base()
	{
		this.pw = pw;
	}
	
	public void ShowTab()
	{
		EditorGUILayout.BeginVertical();
		SP1 = EditorGUILayout.BeginScrollView(SP1);
		
		this.Separate();
		EditorGUILayout.BeginVertical();
		
		EditorGUILayout.BeginVertical("box");
		fold3 = EditorGUILayout.Foldout(fold3, "General settings");
		if(fold3)
		{
			DataHolder.BattleEnd().dialoguePosition = EditorGUILayout.Popup("Position", 
					DataHolder.BattleEnd().dialoguePosition, 
					DataHolder.DialoguePositions().GetNameList(true), GUILayout.Width(pw.mWidth));
			EditorGUILayout.Separator();
			DataHolder.BattleEnd().splitExp = EditorGUILayout.Toggle("Split experience", 
					DataHolder.BattleEnd().splitExp, GUILayout.Width(pw.mWidth));
			
			if(DataHolder.BattleSystem().IsRealTime())
			{
				DataHolder.BattleEnd().getImmediately = EditorGUILayout.Toggle("Get immediately", 
						DataHolder.BattleEnd().getImmediately, GUILayout.Width(pw.mWidth));
				if(DataHolder.BattleEnd().getImmediately)
				{
					DataHolder.BattleEnd().dropItems = EditorGUILayout.Toggle("Drop items", 
							DataHolder.BattleEnd().dropItems, GUILayout.Width(pw.mWidth));
					DataHolder.BattleEnd().dropMoney = EditorGUILayout.Toggle("Drop money", 
							DataHolder.BattleEnd().dropMoney, GUILayout.Width(pw.mWidth));
				}
				else DataHolder.BattleEnd().dropItems = false;
			}
			else
			{
				DataHolder.BattleEnd().getImmediately = false;
				DataHolder.BattleEnd().dropItems = false;
			}
			EditorGUILayout.Separator();
		}
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical("box");
		fold1 = EditorGUILayout.Foldout(fold1, "Victory gain notification");
		if(fold1)
		{
			DataHolder.BattleEnd().showGains = EditorGUILayout.Toggle("Show gains", 
					DataHolder.BattleEnd().showGains, GUILayout.Width(pw.mWidth));
			if(DataHolder.BattleEnd().showGains)
			{
				for(int i=0; i<DataHolder.BattleEnd().gainOrder.Length; i++)
				{
					if(DataHolder.BattleEnd().gainOrder[i] == BattleEnd.MONEY)
					{
						this.ShowMoneyText(i);
					}
					else if(DataHolder.BattleEnd().gainOrder[i] == BattleEnd.ITEM)
					{
						this.ShowItemText(i);
					}
					else if(DataHolder.BattleEnd().gainOrder[i] == BattleEnd.EXPERIENCE)
					{
						this.ShowExperienceText(i);
					}
					EditorGUILayout.Separator();
				}
			}
			EditorGUILayout.Separator();
		}
		EditorGUILayout.EndVertical();
		EditorGUILayout.Separator();
		
		EditorGUILayout.BeginVertical("box");
		fold2 = EditorGUILayout.Foldout(fold2, "Level up notification");
		if(fold2)
		{
			DataHolder.BattleEnd().showLevelUp = EditorGUILayout.Toggle("Show level up", 
					DataHolder.BattleEnd().showLevelUp, GUILayout.Width(pw.mWidth));
			if(DataHolder.BattleEnd().showLevelUp)
			{
				for(int i=0; i<DataHolder.BattleEnd().levelUpOrder.Length; i++)
				{
					if(DataHolder.BattleEnd().levelUpOrder[i] == BattleEnd.LEVELUP)
					{
						this.ShowLevelUpText(i);
					}
					else if(DataHolder.BattleEnd().levelUpOrder[i] == BattleEnd.STATUS)
					{
						this.ShowStatusText(i);
					}
					else if(DataHolder.BattleEnd().levelUpOrder[i] == BattleEnd.SKILL)
					{
						this.ShowSkillText(i);
					}
					EditorGUILayout.Separator();
				}

			}
			EditorGUILayout.Separator();
		}
		EditorGUILayout.EndVertical();
		
		this.Separate();
		EditorGUILayout.EndVertical();
		EditorGUILayout.Separator();
		
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}
	
	private void UpDownGain(int index)
	{
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		if(index > 0)
		{
			if(GUILayout.Button("Move up", GUILayout.Width(100)))
			{
				DataHolder.BattleEnd().GainMoveUp(index);
			}
		}
		if(index < DataHolder.BattleEnd().gainOrder.Length-1)
		{
			if(GUILayout.Button("Move down", GUILayout.Width(100)))
			{
				DataHolder.BattleEnd().GainMoveDown(index);
			}
		}
		EditorGUILayout.EndHorizontal();
	}
	
	private void UpDownLevel(int index)
	{
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		if(index > 0)
		{
			if(GUILayout.Button("Move up", GUILayout.Width(100)))
			{
				DataHolder.BattleEnd().LevelMoveUp(index);
			}
		}
		if(index < DataHolder.BattleEnd().levelUpOrder.Length-1)
		{
			if(GUILayout.Button("Move down", GUILayout.Width(100)))
			{
				DataHolder.BattleEnd().LevelMoveDown(index);
			}
		}
		EditorGUILayout.EndHorizontal();
	}
	
	private void SetNames(ArrayList names)
	{
		for(int i=0; i<names.Count; i++)
		{
			GUILayout.Label(DataHolder.Language(i));
			names[i] = EditorGUILayout.TextArea(names[i] as string, GUILayout.Height(pw.mWidth*0.3f));
		}
	}
	
	private void ShowMoneyText(int index)
	{
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Money text", EditorStyles.boldLabel);
		this.Separate();
		GUILayout.Label("Use % for money value");
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		this.UpDownGain(index);
		this.SetNames(DataHolder.BattleEnd().moneyText);
	}
	
	private void ShowItemText(int index)
	{
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Item text", EditorStyles.boldLabel);
		this.Separate();
		GUILayout.Label("Use % for item count, %n for item name");
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		this.UpDownGain(index);
		this.SetNames(DataHolder.BattleEnd().itemText);
	}
	
	private void ShowExperienceText(int index)
	{
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Experience text", EditorStyles.boldLabel);
		this.Separate();
		GUILayout.Label("% for experience value, %n for experience name");
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		this.UpDownGain(index);
		this.SetNames(DataHolder.BattleEnd().experienceText);
	}
	
	private void ShowLevelUpText(int index)
	{
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Level up text", EditorStyles.boldLabel);
		this.Separate();
		GUILayout.Label("Use % for level, %n for character name");
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		this.UpDownLevel(index);
		this.SetNames(DataHolder.BattleEnd().levelUpText);
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Class level up text", EditorStyles.boldLabel);
		this.Separate();
		GUILayout.Label("Use % for level, %n for character name, %c for class name");
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		this.SetNames(DataHolder.BattleEnd().classLevelUpText);
	}
	
	private void ShowStatusText(int index)
	{
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Status value text", EditorStyles.boldLabel);
		this.Separate();
		GUILayout.Label("Use % for value change, %n for status value name");
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		this.UpDownLevel(index);
		this.SetNames(DataHolder.BattleEnd().statusValueText);
	}
	
	private void ShowSkillText(int index)
	{
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Skill learn text", EditorStyles.boldLabel);
		this.Separate();
		GUILayout.Label("Use %n for skill name");
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		this.UpDownLevel(index);
		this.SetNames(DataHolder.BattleEnd().skillText);
	}
}