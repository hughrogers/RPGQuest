
using UnityEditor;
using UnityEngine;

public class TextColorTab : BaseTab
{
	public TextColorTab(ProjectWindow pw) : base(pw)
	{
		this.Reload();
	}
	
	public void ShowTab()
	{
		EditorGUILayout.BeginVertical();
		
		// buttons
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Add Color", GUILayout.Width(pw.mWidth2)))
		{
			DataHolder.Colors().AddColor("Color name", new Color(1, 1, 1, 1));
			selection = DataHolder.Colors().GetDataCount()-1;
			GUI.FocusControl ("ID");
		}
		this.ShowCopyButton(DataHolder.Colors());
		if(selection > 1)
		{
			this.ShowRemButton("Remove Color", DataHolder.Colors());
		}
		this.CheckSelection(DataHolder.Colors());
		EditorGUILayout.EndHorizontal();
		
		// color list
		this.AddItemList(DataHolder.Colors());
		
		// color settings
		EditorGUILayout.BeginVertical();
		SP2 = EditorGUILayout.BeginScrollView(SP2);
		if(DataHolder.Colors().GetDataCount() > 0)
		{
			this.AddID("Color ID");
			DataHolder.Colors().name[selection] = EditorGUILayout.TextField("Color name", DataHolder.Colors().name[selection], GUILayout.Width(pw.mWidth*2));
			DataHolder.Colors().color[selection] = EditorGUILayout.ColorField("Select color", DataHolder.Colors().color[selection], GUILayout.Width(pw.mWidth));
			EditorGUILayout.EndVertical();
			EditorGUILayout.Separator();
			
			EditorGUILayout.BeginVertical();
			GUILayout.Label ("Default Colors", EditorStyles.boldLabel);
			EditorGUILayout.Toggle("Default Text", DataHolder.Colors().IsDefaultTextColor(selection), GUILayout.Width(pw.mWidth));
			EditorGUILayout.Toggle("Default Shadow", DataHolder.Colors().IsDefaultShadowColor(selection), GUILayout.Width(pw.mWidth));
			EditorGUILayout.EndVertical();
		}
		this.EndTab();
	}
}