
using UnityEditor;
using UnityEngine;

public class ArmorTab : BaseTab
{
	private GameObject tmpPrefab;
	
	public ArmorTab(ProjectWindow pw) : base(pw)
	{
		this.Reload();
		this.tmpPrefab = null;
	}
	
	public void ShowTab()
	{
		int tmpSelection = selection;
		EditorGUILayout.BeginVertical();
		
		// buttons
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Add Armor", GUILayout.Width(pw.mWidth2)))
		{
			DataHolder.Armors().filter.Deactivate();
			DataHolder.Armors().AddArmor("New Armor", "New Description", 
					pw.GetLangCount(), pw.GetEquipmentPartCount(),
					pw.GetStatusEffectCount(), pw.GetElementCount());
			selection = DataHolder.Armors().GetDataCount()-1;
			pw.AddArmor(selection);
			GUI.FocusControl ("ID");
		}
		if(selection >= 0)
		{
			if(this.ShowCopyButton(DataHolder.Armors()))
			{
				DataHolder.Armors().filter.Deactivate();
				pw.AddArmor(selection);
			}
		}
		if(DataHolder.Armors().GetDataCount() > 1 && selection >= 0)
		{
			if(this.ShowRemButton("Remove Armor", DataHolder.Armors()))
			{
				DataHolder.Armors().filter.Deactivate();
				pw.RemoveArmor(selection);
			}
		}
		this.CheckSelection(DataHolder.Armors());
		EditorGUILayout.EndHorizontal();
		
		// status value list
		this.AddItemListFilter(DataHolder.Armors(), "part", DataHolder.EquipmentParts().GetNameList(true));
		
		// value settings
		EditorGUILayout.BeginVertical();
		SP2 = EditorGUILayout.BeginScrollView(SP2);
		if(DataHolder.Armors().GetDataCount() > 0 && selection >= 0)
		{
			EditorGUILayout.BeginVertical();
			EditorGUILayout.BeginHorizontal();
			this.AddID("Armor ID");
			this.AddMultiLangIcon("Armor Name", DataHolder.Armors());
			
			EditorGUILayout.BeginVertical("box");
			fold2 = EditorGUILayout.Foldout(fold2, "Equipment Settings");
			if(fold2)
			{
				if(selection != tmpSelection) this.tmpPrefab = null;
				if(this.tmpPrefab == null && "" != DataHolder.Armor(selection).prefabName)
				{
					this.tmpPrefab = (GameObject)Resources.Load(DataHolder.Armor(selection).GetPrefabPath()+DataHolder.Armor(selection).prefabName, typeof(GameObject));
				}
				this.tmpPrefab = (GameObject)EditorGUILayout.ObjectField("Prefab", this.tmpPrefab, typeof(GameObject), false, GUILayout.Width(pw.mWidth*2));
				if(this.tmpPrefab) DataHolder.Armor(selection).prefabName = this.tmpPrefab.name;
				else DataHolder.Armor(selection).prefabName = "";
				
				EditorGUILayout.Separator();
				DataHolder.Armor(selection).minimumLevel = EditorGUILayout.IntField("Minimum level", 
						DataHolder.Armor(selection).minimumLevel, GUILayout.Width(pw.mWidth));
				DataHolder.Armor(selection).minimumClassLevel = EditorGUILayout.IntField("Min. class level", 
						DataHolder.Armor(selection).minimumClassLevel, GUILayout.Width(pw.mWidth));
				DataHolder.Armor(selection).equipType = (EquipType)this.EnumToolbar("Equip type", 
						(int)DataHolder.Armor(selection).equipType, typeof(EquipType));
				
				int count = pw.GetEquipmentPartCount();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical();
				GUILayout.Label ("Equipable on", EditorStyles.boldLabel);
				for(int i=0; i<count; i++)
				{
					bool prev = DataHolder.Armor(selection).equipPart[i];
					DataHolder.Armor(selection).equipPart[i] = EditorGUILayout.Toggle(pw.GetEquipmentPart(i), DataHolder.Armor(selection).equipPart[i], GUILayout.Width(pw.mWidth));
					if(prev != DataHolder.Armor(selection).equipPart[i])
					{
						pw.SetArmorEPChanged(selection);
					}
				}
				EditorGUILayout.EndVertical();
				
				EditorGUILayout.BeginVertical();
				GUILayout.Label ("Block part", EditorStyles.boldLabel);
				for(int i=0; i<count; i++)
				{
					DataHolder.Armor(selection).blockPart[i] = EditorGUILayout.Toggle(pw.GetEquipmentPart(i), 
							DataHolder.Armor(selection).blockPart[i], GUILayout.Width(pw.mWidth));
					if(DataHolder.Armor(selection).equipPart[i]) DataHolder.Armor(selection).blockPart[i] = false;
				}
				EditorGUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.Separator();
				DataHolder.Armor(selection).dropable = EditorGUILayout.Toggle("Dropable", DataHolder.Armor(selection).dropable, GUILayout.Width(pw.mWidth));
				EditorGUILayout.Separator();
				DataHolder.Armor(selection).buyPrice = EditorGUILayout.IntField("Buy price", DataHolder.Armor(selection).buyPrice, GUILayout.Width(pw.mWidth));
				DataHolder.Armor(selection).sellable = EditorGUILayout.BeginToggleGroup("Sellable", DataHolder.Armor(selection).sellable);
				DataHolder.Armor(selection).sellPrice = EditorGUILayout.IntField("Sell price", DataHolder.Armor(selection).sellPrice, GUILayout.Width(pw.mWidth));
				DataHolder.Armor(selection).sellSetter = (ValueSetter)this.EnumToolbar("Sell in", (int)DataHolder.Armor(selection).sellSetter, typeof(ValueSetter));
				EditorGUILayout.EndToggleGroup();
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			
			EditorGUILayout.BeginVertical("box");
			fold16 = EditorGUILayout.Foldout(fold16, "Bonus/difficulty settings");
			if(fold16)
			{
				EditorHelper.BonusSettings(ref DataHolder.Armor(selection).bonus, false);
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical("box");
			fold7 = EditorGUILayout.Foldout(fold7, "Element effectiveness");
			if(fold7)
			{
				for(int i=0; i<pw.GetElementCount(); i++)
				{
					EditorGUILayout.BeginHorizontal();
					DataHolder.Armor(selection).elementValue[i] = EditorGUILayout.IntField(pw.GetElement(i), 
							DataHolder.Armor(selection).elementValue[i], GUILayout.Width(pw.mWidth));
					DataHolder.Armor(selection).elementOperator[i] = (SimpleOperator)this.EnumToolbar("", 
							(int)DataHolder.Armor(selection).elementOperator[i], typeof(SimpleOperator));
					EditorGUILayout.EndHorizontal();
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold8 = EditorGUILayout.Foldout(fold8, "Race damage factor changes");
			if(fold8)
			{
				for(int i=0; i<DataHolder.Armor(selection).raceValue.Length; i++)
				{
					DataHolder.Armor(selection).raceValue[i] = EditorGUILayout.IntField(DataHolder.Race(i), 
							DataHolder.Armor(selection).raceValue[i], GUILayout.Width(pw.mWidth));
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold9 = EditorGUILayout.Foldout(fold9, "Size damage factor changes");
			if(fold9)
			{
				for(int i=0; i<DataHolder.Armor(selection).sizeValue.Length; i++)
				{
					DataHolder.Armor(selection).sizeValue[i] = EditorGUILayout.IntField(DataHolder.Size(i), 
							DataHolder.Armor(selection).sizeValue[i], GUILayout.Width(pw.mWidth));
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold5 = EditorGUILayout.Foldout(fold5, "Status Effects");
			if(fold5)
			{
				for(int i=0; i<pw.GetStatusEffectCount(); i++)
				{
					DataHolder.Armor(selection).skillEffect[i] = (SkillEffect)this.EnumToolbar(pw.GetStatusEffect(i), (int)DataHolder.Armor(selection).skillEffect[i], typeof(SkillEffect));
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.EndHorizontal();
			
			fold4 = EditorGUILayout.Foldout(fold4, "Enable Skills");
			if(fold4)
			{
				if(GUILayout.Button("Add Skill", GUILayout.Width(pw.mWidth2)))
				{
					DataHolder.Armor(selection).AddEquipmentSkill();
				}
				EditorGUILayout.BeginHorizontal();
				for(int i=0; i<DataHolder.Armor(selection).equipmentSkill.Length; i++)
				{
					EditorGUILayout.BeginVertical("box", GUILayout.Width(pw.mWidth));
					EditorGUILayout.Separator();
					GUILayout.Label("Skill "+(i+1).ToString(), EditorStyles.boldLabel);
					if(GUILayout.Button("Remove Skill", GUILayout.Width(pw.mWidth2)))
					{
						DataHolder.Armor(selection).RemoveEquipmentSkill(i);
						break;
					}
					EditorGUILayout.Separator();
					
					DataHolder.Armor(selection).equipmentSkill[i].skill = EditorGUILayout.Popup("Skill", 
							DataHolder.Armor(selection).equipmentSkill[i].skill, pw.GetSkills());
					DataHolder.Armor(selection).equipmentSkill[i].skillLevel = EditorGUILayout.IntField("Skill level", 
							DataHolder.Armor(selection).equipmentSkill[i].skillLevel);
					DataHolder.Armor(selection).equipmentSkill[i].skillLevel = this.MinMaxCheck(
							DataHolder.Armor(selection).equipmentSkill[i].skillLevel, 1, 
							DataHolder.Skill(DataHolder.Armor(selection).equipmentSkill[i].skill).level.Length);
					
					EditorGUILayout.Separator();
					GUILayout.Label ("Requirements", EditorStyles.boldLabel);
					EditorGUILayout.BeginHorizontal();
					DataHolder.Armor(selection).equipmentSkill[i].requireLevel = EditorGUILayout.BeginToggleGroup("Level", 
							DataHolder.Armor(selection).equipmentSkill[i].requireLevel);
					DataHolder.Armor(selection).equipmentSkill[i].level = EditorGUILayout.IntField(
							DataHolder.Armor(selection).equipmentSkill[i].level);
					EditorGUILayout.EndToggleGroup();
					EditorGUILayout.EndHorizontal();
					
					EditorGUILayout.BeginHorizontal();
					DataHolder.Armor(selection).equipmentSkill[i].requireClass = EditorGUILayout.BeginToggleGroup("Class", 
							DataHolder.Armor(selection).equipmentSkill[i].requireClass);
					DataHolder.Armor(selection).equipmentSkill[i].classNumber = EditorGUILayout.Popup(
							DataHolder.Armor(selection).equipmentSkill[i].classNumber, pw.GetClasses());
					EditorGUILayout.EndToggleGroup();
					EditorGUILayout.EndHorizontal();
					
					EditorGUILayout.BeginHorizontal();
					DataHolder.Armor(selection).equipmentSkill[i].requireClassLevel = EditorGUILayout.BeginToggleGroup("Class level", 
							DataHolder.Armor(selection).equipmentSkill[i].requireClassLevel);
					DataHolder.Armor(selection).equipmentSkill[i].classLevel = EditorGUILayout.IntField(
							DataHolder.Armor(selection).equipmentSkill[i].classLevel);
					EditorGUILayout.EndToggleGroup();
					EditorGUILayout.EndHorizontal();
					
					EditorGUILayout.Separator();
					for(int j=0; j<pw.GetStatusValueCount(); j++)
					{
						DataHolder.Armor(selection).equipmentSkill[i].requireStatus[j] = EditorGUILayout.BeginToggleGroup(
								pw.GetStatusValue(j), DataHolder.Armor(selection).equipmentSkill[i].requireStatus[j]);
						DataHolder.Armor(selection).equipmentSkill[i].statusRequirement[j] = (ValueCheck)this.EnumToolbar("", 
								(int)DataHolder.Armor(selection).equipmentSkill[i].statusRequirement[j], typeof(ValueCheck));
						DataHolder.Armor(selection).equipmentSkill[i].statusValue[j] = EditorGUILayout.IntField(
								DataHolder.Armor(selection).equipmentSkill[i].statusValue[j]);
						EditorGUILayout.EndToggleGroup();
					}
					EditorGUILayout.Separator();
					EditorGUILayout.EndVertical();
				}
				EditorGUILayout.EndHorizontal();
				this.Separate();
			}
			EditorGUILayout.EndVertical();
		}
		this.EndTab();
	}
}