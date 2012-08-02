
using UnityEditor;
using UnityEngine;

public class StatusValueTab : BaseTab
{
	public StatusValueTab(ProjectWindow pw) : base(pw)
	{
		this.Reload();
	}
	
	public void ShowTab()
	{
		EditorGUILayout.BeginVertical();
		
		// buttons
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Add Value", GUILayout.Width(pw.mWidth2)))
		{
			DataHolder.StatusValues().AddValue("New Value", "New Description", pw.GetLangCount());
			selection = DataHolder.StatusValues().GetDataCount()-1;
			pw.AddStatusValue(selection);
			GUI.FocusControl ("ID");
		}
		if(this.ShowCopyButton(DataHolder.StatusValues()))
		{
			pw.AddStatusValue(selection);
			DataHolder.StatusValues().ClearCopiedLevelUp();
		}
		if(DataHolder.StatusValues().GetDataCount() > 1)
		{
			if(this.ShowRemButton("Remove Value", DataHolder.StatusValues()))
			{
				DataHolder.StatusValues().RemoveStatusValue(selection);
				pw.RemoveStatusValue(selection);
			}
		}
		this.CheckSelection(DataHolder.StatusValues());
		EditorGUILayout.EndHorizontal();
		
		// status value list
		this.AddItemList(DataHolder.StatusValues());
		
		// value settings
		EditorGUILayout.BeginVertical();
		SP2 = EditorGUILayout.BeginScrollView(SP2);
		if(DataHolder.StatusValues().GetDataCount() > 0)
		{
			this.AddID("Value ID");
			this.AddMultiLangIcon("Status Name", DataHolder.StatusValues());
			
			EditorGUILayout.BeginVertical("box");
			fold2 = EditorGUILayout.Foldout(fold2, "Base settings");
			if(fold2)
			{
				DataHolder.StatusValue(selection).minValue = EditorGUILayout.IntField("Minimum Value", DataHolder.StatusValue(selection).minValue, GUILayout.Width(pw.mWidth));
				DataHolder.StatusValue(selection).maxValue = EditorGUILayout.IntField("Maximum Value", DataHolder.StatusValue(selection).maxValue, GUILayout.Width(pw.mWidth));
				if(GUILayout.Button("Apply Changes", GUILayout.Width(pw.mWidth2)))
				{
					pw.StatusValueMinMaxChanged(selection, DataHolder.StatusValue(selection).minValue, DataHolder.StatusValue(selection).maxValue);
				}
				
				EditorGUILayout.Separator();
				var prev = DataHolder.StatusValue(selection).type;
				DataHolder.StatusValue(selection).type = (StatusValueType)this.EnumToolbar("Type", (int)DataHolder.StatusValue(selection).type, typeof(StatusValueType), pw.mWidth*2);
				EditorGUILayout.Separator();
				if(prev != DataHolder.StatusValue(selection).type)
				{
					pw.SetStatusValueType(selection, DataHolder.StatusValue(selection).type);
				}
				
				if(StatusValueType.CONSUMABLE.Equals(DataHolder.StatusValue(selection).type))
				{
					DataHolder.StatusValue(selection).maxStatus = EditorGUILayout.Popup("Max Status Value", DataHolder.StatusValue(selection).maxStatus, DataHolder.StatusValues().GetNameList(true), GUILayout.Width(pw.mWidth));
					DataHolder.StatusValue(selection).killChar = EditorGUILayout.Toggle("Death on min.", DataHolder.StatusValue(selection).killChar, GUILayout.Width(pw.mWidth));
				}
				else if(StatusValueType.EXPERIENCE.Equals(DataHolder.StatusValue(selection).type))
				{
					bool prev2 = DataHolder.StatusValue(selection).levelUp;
					bool prev3 = DataHolder.StatusValue(selection).levelUpClass;
					DataHolder.StatusValue(selection).levelUp = EditorGUILayout.Toggle("Causes level up", 
							DataHolder.StatusValue(selection).levelUp, GUILayout.Width(pw.mWidth));
					if(DataHolder.StatusValue(selection).levelUp) DataHolder.StatusValue(selection).levelUpClass = false;
					
					DataHolder.StatusValue(selection).levelUpClass = EditorGUILayout.Toggle("Class level up", 
							DataHolder.StatusValue(selection).levelUpClass, GUILayout.Width(pw.mWidth));
					if(DataHolder.StatusValue(selection).levelUpClass) DataHolder.StatusValue(selection).levelUp = false;
					
					if(prev2 != DataHolder.StatusValue(selection).levelUp && DataHolder.StatusValue(selection).levelUp)
					{
						for(int i=0; i<DataHolder.StatusValues().GetDataCount(); i++)
						{
							if(selection != i)
							{
								DataHolder.StatusValues().value[i].levelUp = false;
							}
						}
					}
					if(prev3 != DataHolder.StatusValue(selection).levelUpClass && DataHolder.StatusValue(selection).levelUpClass)
					{
						for(int i=0; i<DataHolder.StatusValues().GetDataCount(); i++)
						{
							if(selection != i)
							{
								DataHolder.StatusValues().value[i].levelUpClass = false;
							}
						}
					}
				}
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold3 = EditorGUILayout.Foldout(fold3, "Refresh (add to value) battle text settings");
			if(fold3)
			{
				EditorHelper.BattleTextSettings(ref DataHolder.StatusValue(selection).addText, "% for refresh value", true);
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold4 = EditorGUILayout.Foldout(fold4, "Damage (sub from value) battle text settings");
			if(fold4)
			{
				EditorHelper.BattleTextSettings(ref DataHolder.StatusValue(selection).subText, "% for damage value", true);
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.EndVertical();
		}
		this.EndTab();
	}
}