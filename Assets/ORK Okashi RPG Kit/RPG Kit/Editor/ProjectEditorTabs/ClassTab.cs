
using UnityEditor;
using UnityEngine;

public class ClassTab : BaseTab
{
	public ClassTab(ProjectWindow pw) : base(pw)
	{
		this.Reload();
	}
	
	public void SetWeaponEPChanged(int index)
	{
		for(int i=0; i<DataHolder.Classes().GetDataCount(); i++)
		{
			if(!pw.IsWeaponEquipable(index, DataHolder.Classes().classes[i].equipPart))
			{
				DataHolder.Classes().classes[i].weapon[index] = false;
			}
		}
	}
	
	public void SetArmorEPChanged(int index)
	{
		for(int i=0; i<DataHolder.Classes().GetDataCount(); i++)
		{
			if(!pw.IsArmorEquipable(index, DataHolder.Classes().classes[i].equipPart))
			{
				DataHolder.Classes().classes[i].armor[index] = false;
			}
		}
	}
	
	public void ShowTab()
	{
		EditorGUILayout.BeginVertical();
		
		// buttons
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Add Class", GUILayout.Width(pw.mWidth2)))
		{
			DataHolder.Classes().AddClass("New Class", "New Description", 
					pw.GetLangCount(), pw.GetEquipmentPartCount(),
					pw.GetWeaponCount(), pw.GetArmorCount(),
					pw.GetElementCount());
			selection = DataHolder.Classes().GetDataCount()-1;
			pw.AddClass(selection);
			GUI.FocusControl ("ID");
		}
		if(this.ShowCopyButton(DataHolder.Classes()))
		{
			pw.AddClass(selection);
		}
		if(DataHolder.Classes().GetDataCount() > 1)
		{
			if(this.ShowRemButton("Remove Class", DataHolder.Classes()))
			{
				pw.RemoveClass(selection);
			}
		}
		this.CheckSelection(DataHolder.Classes());
		EditorGUILayout.EndHorizontal();
		
		// status value list
		this.AddItemList(DataHolder.Classes());
		
		// value settings
		EditorGUILayout.BeginVertical();
		SP2 = EditorGUILayout.BeginScrollView(SP2);
		if(DataHolder.Classes().GetDataCount() > 0)
		{
			this.AddID("Class ID");
			this.AddMultiLangIcon("Class Name", DataHolder.Classes());
			
			EditorGUILayout.BeginVertical("box");
			fold5 = EditorGUILayout.Foldout(fold5, "Skill Learning");
			if(fold5)
			{
				if(GUILayout.Button("Add", GUILayout.Width(pw.mWidth3)))
				{
					DataHolder.Classes().AddLearnSkill(selection);
				}
				for(int i=0; i<DataHolder.Class(selection).development.skill.Length; i++)
				{
					EditorGUILayout.Separator();
					EditorGUILayout.BeginHorizontal();
					DataHolder.Class(selection).development.skill[i].atLevel = EditorGUILayout.IntField(
							DataHolder.Class(selection).useClassLevel ? "Class level" : "Level", 
							DataHolder.Class(selection).development.skill[i].atLevel, GUILayout.Width(pw.mWidth*0.7f));
					DataHolder.Class(selection).development.skill[i].skillID = EditorGUILayout.Popup(
							DataHolder.Class(selection).development.skill[i].skillID, pw.GetSkills(), GUILayout.Width(pw.mWidth*0.5f));
					
					DataHolder.Class(selection).development.skill[i].skillLevel = EditorGUILayout.IntField("Skill level", 
							DataHolder.Class(selection).development.skill[i].skillLevel, GUILayout.Width(pw.mWidth*0.7f));
					DataHolder.Class(selection).development.skill[i].skillLevel = this.MinMaxCheck(
							DataHolder.Class(selection).development.skill[i].skillLevel, 1, 
							DataHolder.Skill(DataHolder.Class(selection).development.skill[i].skillID).level.Length);
					
					if(GUILayout.Button("Remove", GUILayout.Width(pw.mWidth3)))
					{
						DataHolder.Classes().RemoveLearnSkill(selection, i);
					}
					EditorGUILayout.EndHorizontal();
				}
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical("box");
			fold6 = EditorGUILayout.Foldout(fold6, "Equipment Parts");
			if(fold6)
			{
				for(int i=0; i<pw.GetEquipmentPartCount(); i++)
				{
					DataHolder.Class(selection).equipPart[i] = EditorGUILayout.Toggle(pw.GetEquipmentPart(i), DataHolder.Class(selection).equipPart[i], GUILayout.Width(pw.mWidth));
				}
			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginVertical("box");
			fold2 = EditorGUILayout.Foldout(fold2, "Weapons");
			if(fold2)
			{
				for(int i=0; i<pw.GetWeaponCount(); i++)
				{
					if(pw.IsWeaponEquipable(i, DataHolder.Class(selection).equipPart))
					{
						DataHolder.Class(selection).weapon[i] = EditorGUILayout.Toggle(pw.GetWeapon(i), DataHolder.Class(selection).weapon[i], GUILayout.Width(pw.mWidth));
					}
					else
					{
						DataHolder.Class(selection).weapon[i] = false;
					}
				}
			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginVertical("box");
			fold3 = EditorGUILayout.Foldout(fold3, "Armors");
			if(fold3)
			{
				for(int i=0; i<pw.GetArmorCount(); i++)
				{
					if(pw.IsArmorEquipable(i, DataHolder.Class(selection).equipPart))
					{
						DataHolder.Class(selection).armor[i] = EditorGUILayout.Toggle(pw.GetArmor(i), DataHolder.Class(selection).armor[i], GUILayout.Width(pw.mWidth));
					}
					else
					{
						DataHolder.Class(selection).armor[i] = false;
					}
				}
			}
			EditorGUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical("box");
			fold4 = EditorGUILayout.Foldout(fold4, "Element effectiveness");
			if(fold4)
			{
				for(int i=0; i<pw.GetElementCount(); i++)
				{
					DataHolder.Class(selection).elementValue[i] = EditorGUILayout.IntField(pw.GetElement(i), 
							DataHolder.Class(selection).elementValue[i], GUILayout.Width(pw.mWidth));
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold7 = EditorGUILayout.Foldout(fold7, "Race damage factor");
			if(fold7)
			{
				for(int i=0; i<DataHolder.Class(selection).raceValue.Length; i++)
				{
					DataHolder.Class(selection).raceValue[i] = EditorGUILayout.IntField(DataHolder.Race(i), 
							DataHolder.Class(selection).raceValue[i], GUILayout.Width(pw.mWidth));
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold8 = EditorGUILayout.Foldout(fold8, "Size damage factor");
			if(fold8)
			{
				for(int i=0; i<DataHolder.Class(selection).sizeValue.Length; i++)
				{
					DataHolder.Class(selection).sizeValue[i] = EditorGUILayout.IntField(DataHolder.Size(i), 
							DataHolder.Class(selection).sizeValue[i], GUILayout.Width(pw.mWidth));
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginVertical("box");
			fold9 = EditorGUILayout.Foldout(fold9, "Status Value Development");
			if(fold9)
			{
				bool tmp = DataHolder.Class(selection).useClassLevel;
				DataHolder.Class(selection).useClassLevel = EditorGUILayout.Toggle("Use class level",
						DataHolder.Class(selection).useClassLevel, GUILayout.Width(pw.mWidth));
				
				if(DataHolder.Class(selection).useClassLevel)
				{
					StatusValue[] sv = pw.GetStatusValuesData();
					if(tmp != DataHolder.Class(selection).useClassLevel)
					{
						DataHolder.Class(selection).development.Init(sv.Length);
					}
					
					DataHolder.Class(selection).development.startLevel = EditorGUILayout.IntField("Start Level", 
							DataHolder.Class(selection).development.startLevel, GUILayout.Width(pw.mWidth));
					if(DataHolder.Class(selection).development.startLevel > DataHolder.Class(selection).development.maxLevel)
					{
						DataHolder.Class(selection).development.startLevel = DataHolder.Class(selection).development.maxLevel;
					}
					else if(DataHolder.Class(selection).development.startLevel < 1)
					{
						DataHolder.Class(selection).development.startLevel = 1;
					}
					DataHolder.Class(selection).development.maxLevel = EditorGUILayout.IntField("Max. Level", 
							DataHolder.Class(selection).development.maxLevel, GUILayout.Width(pw.mWidth));
					if(DataHolder.Class(selection).development.maxLevel < 1)
					{
						DataHolder.Class(selection).development.maxLevel = 1;
					}
					if(GUILayout.Button("Apply Changes", GUILayout.Width(pw.mWidth2)))
					{
						DataHolder.Class(selection).development.MaxLevelChanged();
					}
					EditorGUILayout.Separator();
					
					for(int i=0; i<sv.Length; i++)
					{
						if(!sv[i].IsConsumable())
						{
							GUILayout.Label(pw.GetStatusValue(i), EditorStyles.boldLabel);
							if(GUILayout.Button("Edit", GUILayout.Width(pw.mWidth2)))
							{
								StatusCurveWindow.Init(pw.GetStatusValue(i)+" Curve",
										ref DataHolder.Class(selection).development.statusValue[i].levelValue, sv[i]);
							}
						}
					}
				}
			}
			EditorGUILayout.EndVertical();
			
			
			EditorGUILayout.BeginVertical("box");
			fold16 = EditorGUILayout.Foldout(fold16, "Bonus/difficulty settings");
			if(fold16)
			{
				EditorHelper.BonusSettings(ref DataHolder.Class(selection).bonus, false);
				this.Separate();
			}
			EditorGUILayout.EndVertical();
		}
		this.EndTab();
	}
}