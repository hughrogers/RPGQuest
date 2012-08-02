
using UnityEditor;
using UnityEngine;

public class TeleportTab : BaseTab
{
	public TeleportTab(ProjectWindow pw) : base(pw)
	{
		this.Reload();
	}
	
	public void ShowTab()
	{
		EditorGUILayout.BeginVertical();
		
		// buttons
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Add Teleport", GUILayout.Width(pw.mWidth2)))
		{
			DataHolder.Teleports().AddTeleport("New Teleport", "New Description", pw.GetLangCount());
			selection = DataHolder.Teleports().GetDataCount()-1;
			GUI.FocusControl ("ID");
		}
		this.ShowCopyButton(DataHolder.Teleports());
		if(DataHolder.Teleports().GetDataCount() > 1)
		{
			this.ShowRemButton("Remove Teleport", DataHolder.Teleports());
		}
		this.CheckSelection(DataHolder.Teleports());
		EditorGUILayout.EndHorizontal();
		
		this.AddItemList(DataHolder.Teleports());
		
		EditorGUILayout.BeginVertical();
		SP2 = EditorGUILayout.BeginScrollView(SP2);
		if(DataHolder.Teleports().GetDataCount() > 0)
		{
			this.AddID("Teleport ID");
			this.AddMultiLangIcon("Name", DataHolder.Teleports());
			
			EditorGUILayout.BeginVertical("box");
			fold1 = EditorGUILayout.Foldout(fold1, "Teleport settings");
			if(fold1)
			{
				GUILayout.Label("Teleport target", EditorStyles.boldLabel);
				DataHolder.Teleport(selection).sceneName = EditorGUILayout.TextField("Scene name", 
						DataHolder.Teleport(selection).sceneName, GUILayout.Width(pw.mWidth*2));
				DataHolder.Teleport(selection).spawnID = EditorGUILayout.IntField("Spawn ID", 
						DataHolder.Teleport(selection).spawnID, GUILayout.Width(pw.mWidth));
				
				EditorGUILayout.Separator();
				GUILayout.Label("Screen fading", EditorStyles.boldLabel);
				DataHolder.Teleport(selection).fadeOut = EditorGUILayout.Toggle("Fade out", 
						DataHolder.Teleport(selection).fadeOut, GUILayout.Width(pw.mWidth));
				if(DataHolder.Teleport(selection).fadeOut)
				{
					DataHolder.Teleport(selection).fadeOutTime = EditorGUILayout.FloatField("Time", 
							DataHolder.Teleport(selection).fadeOutTime, GUILayout.Width(pw.mWidth));
					DataHolder.Teleport(selection).fadeOutInterpolate = (EaseType)EditorGUILayout.EnumPopup("Interpolation", 
							DataHolder.Teleport(selection).fadeOutInterpolate, GUILayout.Width(pw.mWidth));
					EditorGUILayout.Separator();
				}
				
				EditorGUILayout.Separator();
				DataHolder.Teleport(selection).fadeIn = EditorGUILayout.Toggle("Fade in", 
						DataHolder.Teleport(selection).fadeIn, GUILayout.Width(pw.mWidth));
				if(DataHolder.Teleport(selection).fadeIn)
				{
					DataHolder.Teleport(selection).fadeInTime = EditorGUILayout.FloatField("Time", 
							DataHolder.Teleport(selection).fadeInTime, GUILayout.Width(pw.mWidth));
					DataHolder.Teleport(selection).fadeInInterpolate = (EaseType)EditorGUILayout.EnumPopup("Interpolation", 
							DataHolder.Teleport(selection).fadeInInterpolate, GUILayout.Width(pw.mWidth));
					EditorGUILayout.Separator();
				}
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold2 = EditorGUILayout.Foldout(fold2, "List conditions");
			if(fold2)
			{
				GUILayout.Label("Variable conditions", EditorStyles.boldLabel);
				DataHolder.Teleport(selection).variables = EditorHelper.VariableConditionSettings(DataHolder.Teleport(selection).variables);
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.EndVertical();
		}
		this.EndTab();
	}
}