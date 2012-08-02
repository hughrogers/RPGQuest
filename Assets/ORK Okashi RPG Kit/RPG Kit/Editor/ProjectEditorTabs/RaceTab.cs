
using UnityEditor;
using UnityEngine;

public class RaceTab : BaseTab
{
	public RaceTab(ProjectWindow pw) : base(pw)
	{
		this.Reload();
	}
	
	public void ShowTab()
	{
		EditorGUILayout.BeginVertical();
		
		// buttons
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Add Race", GUILayout.Width(pw.mWidth2)))
		{
			DataHolder.Races().AddBaseData("New Race", "New Description", pw.GetLangCount());
			selection = DataHolder.Races().GetDataCount()-1;
			pw.AddRace();
			GUI.FocusControl ("ID");
		}
		if(this.ShowCopyButton(DataHolder.Races()))
		{
			pw.AddRace();
		}
		if(DataHolder.Races().GetDataCount() > 1)
		{
			if(this.ShowRemButton("Remove Race", DataHolder.Races()))
			{
				pw.RemoveRace(selection);
			}
		}
		this.CheckSelection(DataHolder.Races());
		EditorGUILayout.EndHorizontal();
		
		this.AddItemList(DataHolder.Races());
		
		EditorGUILayout.BeginVertical();
		SP2 = EditorGUILayout.BeginScrollView(SP2);
		if(DataHolder.Races().GetDataCount() > 0)
		{
			this.AddID("Race ID");
			this.AddMultiLangIcon("Race Name", DataHolder.Races());
			EditorGUILayout.EndVertical();
		}
		this.EndTab();
	}
}