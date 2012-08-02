
using UnityEditor;
using UnityEngine;

public class SizeTab : BaseTab
{
	public SizeTab(ProjectWindow pw) : base(pw)
	{
		this.Reload();
	}
	
	public void ShowTab()
	{
		EditorGUILayout.BeginVertical();
		
		// buttons
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Add Size", GUILayout.Width(pw.mWidth2)))
		{
			DataHolder.Sizes().AddBaseData("New Size", "New Description", pw.GetLangCount());
			selection = DataHolder.Sizes().GetDataCount()-1;
			pw.AddSize();
			GUI.FocusControl ("ID");
		}
		if(this.ShowCopyButton(DataHolder.Sizes()))
		{
			pw.AddSize();
		}
		if(DataHolder.Sizes().GetDataCount() > 1)
		{
			if(this.ShowRemButton("Remove Size", DataHolder.Sizes()))
			{
				pw.RemoveSize(selection);
			}
		}
		this.CheckSelection(DataHolder.Sizes());
		EditorGUILayout.EndHorizontal();
		
		this.AddItemList(DataHolder.Sizes());
		
		EditorGUILayout.BeginVertical();
		SP2 = EditorGUILayout.BeginScrollView(SP2);
		if(DataHolder.Sizes().GetDataCount() > 0)
		{
			this.AddID("Size ID");
			this.AddMultiLangIcon("Size Name", DataHolder.Sizes());
			EditorGUILayout.EndVertical();
		}
		this.EndTab();
	}
}