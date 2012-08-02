
using UnityEditor;
using UnityEngine;

public class SkillTypeTab : BaseTab
{
	public SkillTypeTab(ProjectWindow pw) : base(pw)
	{
		this.Reload();
	}
	
	public void ShowTab()
	{
		EditorGUILayout.BeginVertical();
		
		// buttons
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Add Skill Type", GUILayout.Width(pw.mWidth2)))
		{
			DataHolder.SkillTypes().AddBaseData("New Skill Type", "New Description", pw.GetLangCount());
			selection = DataHolder.SkillTypes().GetDataCount()-1;
			GUI.FocusControl ("ID");
			pw.AddSkillType();
		}
		if(this.ShowCopyButton(DataHolder.SkillTypes()))
		{
			pw.AddSkillType();
		}
		if(DataHolder.SkillTypes().GetDataCount() > 1)
		{
			if(this.ShowRemButton("Remove Skill Type", DataHolder.SkillTypes()))
			{
				pw.RemoveSkillType(selection);
			}
		}
		this.CheckSelection(DataHolder.SkillTypes());
		EditorGUILayout.EndHorizontal();
		
		// elements list
		this.AddItemList(DataHolder.SkillTypes());
		
		// element settings
		EditorGUILayout.BeginVertical();
		SP2 = EditorGUILayout.BeginScrollView(SP2);
		if(DataHolder.SkillTypes().GetDataCount() > 0)
		{
			this.AddID("Skill Type ID");
			this.AddMultiLangIcon("Type Name", DataHolder.SkillTypes());
			EditorGUILayout.EndVertical();
		}
		this.EndTab();
	}
}