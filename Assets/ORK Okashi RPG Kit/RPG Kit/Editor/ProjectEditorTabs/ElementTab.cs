
using UnityEditor;
using UnityEngine;

public class ElementTab : BaseTab
{
	public ElementTab(ProjectWindow pw) : base(pw)
	{
		this.Reload();
	}
	
	public void ShowTab()
	{
		EditorGUILayout.BeginVertical();
		
		// buttons
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Add Element", GUILayout.Width(pw.mWidth2)))
		{
			DataHolder.Elements().AddBaseData("New Element", "New Description", pw.GetLangCount());
			selection = DataHolder.Elements().GetDataCount()-1;
			pw.AddElement(selection);
			GUI.FocusControl ("ID");
		}
		if(this.ShowCopyButton(DataHolder.Elements()))
		{
			pw.AddElement(selection);
		}
		if(DataHolder.Elements().GetDataCount() > 1)
		{
			if(this.ShowRemButton("Remove Element", DataHolder.Elements()))
			{
				pw.RemoveElement(selection);
			}
		}
		this.CheckSelection(DataHolder.Elements());
		EditorGUILayout.EndHorizontal();
		
		// elements list
		this.AddItemList(DataHolder.Elements());
		
		// element settings
		EditorGUILayout.BeginVertical();
		SP2 = EditorGUILayout.BeginScrollView(SP2);
		if(DataHolder.Elements().GetDataCount() > 0)
		{
			this.AddID("Element ID");
			this.AddMultiLangIcon("Element Name", DataHolder.Elements());
			EditorGUILayout.EndVertical();
		}
		this.EndTab();
	}
}