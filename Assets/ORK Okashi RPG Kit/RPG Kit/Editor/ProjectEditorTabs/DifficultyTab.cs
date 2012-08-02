
using UnityEditor;
using UnityEngine;

public class DifficultyTab : BaseTab
{
	public DifficultyTab(ProjectWindow pw) : base(pw)
	{
		this.Reload();
	}
	
	public void ShowTab()
	{
		EditorGUILayout.BeginVertical();
		
		// buttons
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Add Difficulty", GUILayout.Width(pw.mWidth2)))
		{
			DataHolder.Difficulties().AddDifficulty("New Difficulty", "New Description", pw.GetLangCount());
			selection = DataHolder.Difficulties().GetDataCount()-1;
			GUI.FocusControl ("ID");
		}
		this.ShowCopyButton(DataHolder.Difficulties());
		if(DataHolder.Difficulties().GetDataCount() > 1)
		{
			if(this.ShowRemButton("Remove Difficulty", DataHolder.Difficulties()))
			{
				pw.RemoveDifficulty(selection);
			}
		}
		this.CheckSelection(DataHolder.Difficulties());
		EditorGUILayout.EndHorizontal();
		
		this.AddItemList(DataHolder.Difficulties());
		
		EditorGUILayout.BeginVertical();
		SP2 = EditorGUILayout.BeginScrollView(SP2);
		if(DataHolder.Difficulties().GetDataCount() > 0)
		{
			this.AddID("Difficulty ID");
			this.AddMultiLangIcon("Name", DataHolder.Difficulties());
			
			EditorGUILayout.BeginVertical("box");
			fold2 = EditorGUILayout.Foldout(fold2, "Time factor settings");
			if(fold2)
			{
				DataHolder.Difficulty(selection).timeFactor = EditorGUILayout.FloatField("Time factor", 
						DataHolder.Difficulty(selection).timeFactor, GUILayout.Width(pw.mWidth));
				DataHolder.Difficulty(selection).movementFactor = EditorGUILayout.FloatField("Movement factor", 
						DataHolder.Difficulty(selection).movementFactor, GUILayout.Width(pw.mWidth));
				DataHolder.Difficulty(selection).battleFactor = EditorGUILayout.FloatField("Battle factor", 
						DataHolder.Difficulty(selection).battleFactor, GUILayout.Width(pw.mWidth));
				DataHolder.Difficulty(selection).animationFactor = EditorGUILayout.FloatField("Animation factor", 
						DataHolder.Difficulty(selection).animationFactor, GUILayout.Width(pw.mWidth));
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold3 = EditorGUILayout.Foldout(fold3, "Enemy settings");
			if(fold3)
			{
				GUILayout.Label("Enemy status multipliers", EditorStyles.boldLabel);
				for(int i=0; i<DataHolder.Difficulty(selection).statusMultiplier.Length; i++)
				{
					if(!DataHolder.StatusValue(i).IsConsumable())
					{
						DataHolder.Difficulty(selection).statusMultiplier[i] = EditorGUILayout.FloatField(DataHolder.StatusValues().GetName(i), 
								DataHolder.Difficulty(selection).statusMultiplier[i], GUILayout.Width(pw.mWidth));
						if(DataHolder.Difficulty(selection).statusMultiplier[i] <= 0)
						{
							DataHolder.Difficulty(selection).statusMultiplier[i] = 0.1f;
						}
					}
				}
				EditorGUILayout.Separator();
				
				GUILayout.Label("Enemy element multipliers", EditorStyles.boldLabel);
				for(int i=0; i<DataHolder.Difficulty(selection).elementMultiplier.Length; i++)
				{
					DataHolder.Difficulty(selection).elementMultiplier[i] = EditorGUILayout.FloatField(DataHolder.Elements().GetName(i), 
							DataHolder.Difficulty(selection).elementMultiplier[i], GUILayout.Width(pw.mWidth));
					if(DataHolder.Difficulty(selection).elementMultiplier[i] <= 0)
					{
						DataHolder.Difficulty(selection).elementMultiplier[i] = 0.1f;
					}
				}
				EditorGUILayout.Separator();
				
				GUILayout.Label("Enemy race multipliers", EditorStyles.boldLabel);
				for(int i=0; i<DataHolder.Difficulty(selection).raceMultiplier.Length; i++)
				{
					DataHolder.Difficulty(selection).raceMultiplier[i] = EditorGUILayout.FloatField(DataHolder.Races().GetName(i), 
							DataHolder.Difficulty(selection).raceMultiplier[i], GUILayout.Width(pw.mWidth));
					if(DataHolder.Difficulty(selection).raceMultiplier[i] <= 0)
					{
						DataHolder.Difficulty(selection).raceMultiplier[i] = 0.1f;
					}
				}
				EditorGUILayout.Separator();
				
				GUILayout.Label("Enemy size multipliers", EditorStyles.boldLabel);
				for(int i=0; i<DataHolder.Difficulty(selection).sizeMultiplier.Length; i++)
				{
					DataHolder.Difficulty(selection).sizeMultiplier[i] = EditorGUILayout.FloatField(DataHolder.Sizes().GetName(i), 
							DataHolder.Difficulty(selection).sizeMultiplier[i], GUILayout.Width(pw.mWidth));
					if(DataHolder.Difficulty(selection).sizeMultiplier[i] <= 0)
					{
						DataHolder.Difficulty(selection).sizeMultiplier[i] = 0.1f;
					}
				}
				EditorGUILayout.Separator();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.EndVertical();
		}
		this.EndTab();
	}
}