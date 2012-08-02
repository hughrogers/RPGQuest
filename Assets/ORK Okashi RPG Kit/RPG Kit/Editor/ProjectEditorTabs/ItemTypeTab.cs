
using UnityEditor;
using UnityEngine;

public class ItemTypeTab : BaseTab
{
	public ItemTypeTab(ProjectWindow pw) : base(pw)
	{
		this.Reload();
	}
	
	public void ShowTab()
	{
		EditorGUILayout.BeginVertical();
		
		// buttons
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Add Item Type", GUILayout.Width(pw.mWidth2)))
		{
			DataHolder.ItemTypes().AddBaseData("New Item Type", "New Description", pw.GetLangCount());
			selection = DataHolder.ItemTypes().GetDataCount()-1;
			GUI.FocusControl ("ID");
		}
		this.ShowCopyButton(DataHolder.ItemTypes());
		if(DataHolder.ItemTypes().GetDataCount() > 1)
		{
			if(this.ShowRemButton("Remove Item Type", DataHolder.ItemTypes()))
			{
				pw.RemoveItemType(selection);
			}
		}
		this.CheckSelection(DataHolder.ItemTypes());
		EditorGUILayout.EndHorizontal();
		
		// elements list
		this.AddItemList(DataHolder.ItemTypes());
		
		// element settings
		EditorGUILayout.BeginVertical();
		SP2 = EditorGUILayout.BeginScrollView(SP2);
		if(DataHolder.ItemTypes().GetDataCount() > 0)
		{
			this.AddID("Item Type ID");
			this.AddMultiLangIcon("Type Name", DataHolder.ItemTypes());
			EditorGUILayout.EndVertical();
		}
		this.EndTab();
	}
}