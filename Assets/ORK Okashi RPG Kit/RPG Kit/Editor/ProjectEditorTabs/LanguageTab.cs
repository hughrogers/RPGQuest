
using UnityEditor;
using UnityEngine;

public class LanguageTab : BaseTab
{
	public LanguageTab(ProjectWindow pw) : base(pw)
	{
		this.Reload();
	}
	
	public void ShowTab()
	{
		EditorGUILayout.BeginVertical();
		
		// buttons
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Add Language", GUILayout.Width(pw.mWidth2)))
		{
			DataHolder.Languages().AddLanguage("New Language");
			selection = DataHolder.Languages().GetDataCount()-1;
			pw.AddLanguage(selection);
			GUI.FocusControl ("ID");
		}
		if(this.ShowCopyButton(DataHolder.Languages()))
		{
			pw.AddLanguage(selection);
		}
		if(DataHolder.Languages().GetDataCount() > 1)
		{
			if(this.ShowRemButton("Remove Language", DataHolder.Languages()))
			{
				pw.RemoveLanguage(selection);
			}
		}
		this.CheckSelection(DataHolder.Languages());
		EditorGUILayout.EndHorizontal();
		
		// status value list
		this.AddItemList(DataHolder.Languages());
		
		// value settings
		EditorGUILayout.BeginVertical();
		SP2 = EditorGUILayout.BeginScrollView(SP2);
		if(DataHolder.Languages().GetDataCount() > 0)
		{
			this.AddID("Language ID");
			DataHolder.Languages().name[selection] = EditorGUILayout.TextField("Language name", DataHolder.Languages().name[selection]);
			EditorGUILayout.EndVertical();
		}
		this.EndTab();
	}
}