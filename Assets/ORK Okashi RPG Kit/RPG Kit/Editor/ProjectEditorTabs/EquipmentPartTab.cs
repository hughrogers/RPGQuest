
using UnityEditor;
using UnityEngine;

public class EquipmentPartTab : BaseTab
{
	public EquipmentPartTab(ProjectWindow pw) : base(pw)
	{
		this.Reload();
	}
	
	public void ShowTab()
	{
		EditorGUILayout.BeginVertical();
		
		// buttons
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Add Equipment Part", GUILayout.Width(pw.mWidth2)))
		{
			DataHolder.EquipmentParts().AddBaseData("New Equipment Part", "New Description", pw.GetLangCount());
			selection = DataHolder.EquipmentParts().GetDataCount()-1;
			pw.AddEquipmentPart(selection);
			GUI.FocusControl ("ID");
		}
		if(this.ShowCopyButton(DataHolder.EquipmentParts()))
		{
			pw.AddEquipmentPart(selection);
		}
		if(DataHolder.EquipmentParts().GetDataCount() > 1)
		{
			if(this.ShowRemButton("Remove Equipment Part", DataHolder.EquipmentParts()))
			{
				pw.RemoveEquipmentPart(selection);
			}
		}
		this.CheckSelection(DataHolder.EquipmentParts());
		EditorGUILayout.EndHorizontal();
		
		// elements list
		this.AddItemList(DataHolder.EquipmentParts());
		
		// element settings
		EditorGUILayout.BeginVertical();
		SP2 = EditorGUILayout.BeginScrollView(SP2);
		if(DataHolder.EquipmentParts().GetDataCount() > 0)
		{
			this.AddID("Equipment ID");
			this.AddMultiLangIcon("Equ. Part Name", DataHolder.EquipmentParts());
			EditorGUILayout.EndVertical();
		}
		this.EndTab();
	}
}