
using UnityEditor;
using UnityEngine;

public class WeaponTab : BaseTab
{
	private GameObject tmpPrefab;
	
	public WeaponTab(ProjectWindow pw) : base(pw)
	{
		this.Reload();
	}
	
	public void ShowTab()
	{
		int tmpSelection = selection;
		EditorGUILayout.BeginVertical();
		
		// buttons
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Add Weapon", GUILayout.Width(pw.mWidth2)))
		{
			DataHolder.Weapons().filter.Deactivate();
			DataHolder.Weapons().AddWeapon("New Weapon", "New Description", 
					pw.GetLangCount(), pw.GetEquipmentPartCount(),
					pw.GetStatusEffectCount());
			selection = DataHolder.Weapons().GetDataCount()-1;
			pw.AddWeapon(selection);
			GUI.FocusControl ("ID");
		}
		if(selection >= 0)
		{
			if(this.ShowCopyButton(DataHolder.Weapons()))
			{
				DataHolder.Weapons().filter.Deactivate();
				pw.AddWeapon(selection);
			}
		}
		if(DataHolder.Weapons().GetDataCount() > 1 && selection >= 0)
		{
			if(this.ShowRemButton("Remove Weapon", DataHolder.Weapons()))
			{
				DataHolder.Weapons().filter.Deactivate();
				pw.RemoveWeapon(selection);
			}
		}
		this.CheckSelection(DataHolder.Weapons());
		EditorGUILayout.EndHorizontal();
		
		// status value list
		this.AddItemListFilter(DataHolder.Weapons(), "part", DataHolder.EquipmentParts().GetNameList(true));
		
		// value settings
		EditorGUILayout.BeginVertical();
		SP2 = EditorGUILayout.BeginScrollView(SP2);
		if(DataHolder.Weapons().GetDataCount() > 0 && selection >= 0)
		{
			EditorGUILayout.BeginVertical();
			EditorGUILayout.BeginHorizontal();
			this.AddID("Weapon ID");
			this.AddMultiLangIcon("Weapon Name", DataHolder.Weapons());
			
			EditorGUILayout.BeginVertical("box");
			fold2 = EditorGUILayout.Foldout(fold2, "Equipment Settings");
			if(fold2)
			{
				if(selection != tmpSelection) this.tmpPrefab = null;
				if(this.tmpPrefab == null && "" != DataHolder.Weapon(selection).prefabName)
				{
					this.tmpPrefab = (GameObject)Resources.Load(DataHolder.Weapon(selection).GetPrefabPath()+DataHolder.Weapon(selection).prefabName, typeof(GameObject));
				}
				this.tmpPrefab = (GameObject)EditorGUILayout.ObjectField("Prefab", this.tmpPrefab, typeof(GameObject), false, GUILayout.Width(pw.mWidth*2));
				if(this.tmpPrefab) DataHolder.Weapon(selection).prefabName = this.tmpPrefab.name;
				else DataHolder.Weapon(selection).prefabName = "";
				
				EditorGUILayout.Separator();
				DataHolder.Weapon(selection).minimumLevel = EditorGUILayout.IntField("Minimum level", 
						DataHolder.Weapon(selection).minimumLevel, GUILayout.Width(pw.mWidth));
				DataHolder.Weapon(selection).minimumClassLevel = EditorGUILayout.IntField("Min. class level", 
						DataHolder.Weapon(selection).minimumClassLevel, GUILayout.Width(pw.mWidth));
				DataHolder.Weapon(selection).equipType = (EquipType)this.EnumToolbar("Equip type", 
						(int)DataHolder.Weapon(selection).equipType, typeof(EquipType));
				
				int count = pw.GetEquipmentPartCount();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical();
				GUILayout.Label ("Equipable on", EditorStyles.boldLabel);
				for(int i=0; i<count; i++)
				{
					bool prev = DataHolder.Weapon(selection).equipPart[i];
					DataHolder.Weapon(selection).equipPart[i] = EditorGUILayout.Toggle(pw.GetEquipmentPart(i), 
							DataHolder.Weapon(selection).equipPart[i], GUILayout.Width(pw.mWidth));
					if(prev != DataHolder.Weapon(selection).equipPart[i])
					{
						pw.SetWeaponEPChanged(selection);
					}
				}
				EditorGUILayout.EndVertical();
				
				EditorGUILayout.BeginVertical();
				GUILayout.Label ("Block part", EditorStyles.boldLabel);
				for(int i=0; i<count; i++)
				{
					DataHolder.Weapon(selection).blockPart[i] = EditorGUILayout.Toggle(pw.GetEquipmentPart(i), 
							DataHolder.Weapon(selection).blockPart[i], GUILayout.Width(pw.mWidth));
					if(DataHolder.Weapon(selection).equipPart[i]) DataHolder.Weapon(selection).blockPart[i] = false;
				}
				EditorGUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.Separator();
				DataHolder.Weapon(selection).dropable = EditorGUILayout.Toggle("Dropable", DataHolder.Weapon(selection).dropable, GUILayout.Width(pw.mWidth));
				EditorGUILayout.Separator();
				DataHolder.Weapon(selection).buyPrice = EditorGUILayout.IntField("Buy price", DataHolder.Weapon(selection).buyPrice, GUILayout.Width(pw.mWidth));
				DataHolder.Weapon(selection).sellable = EditorGUILayout.BeginToggleGroup("Sellable", DataHolder.Weapon(selection).sellable);
				DataHolder.Weapon(selection).sellPrice = EditorGUILayout.IntField("Sell price", DataHolder.Weapon(selection).sellPrice, GUILayout.Width(pw.mWidth));
				DataHolder.Weapon(selection).sellSetter = (ValueSetter)this.EnumToolbar("Sell in", (int)DataHolder.Weapon(selection).sellSetter, typeof(ValueSetter));
				EditorGUILayout.EndToggleGroup();
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical("box");
			fold6 = EditorGUILayout.Foldout(fold6, "Attack Settings");
			if(fold6)
			{
				EditorGUILayout.Separator();
				DataHolder.Weapon(selection).ownAttack = EditorGUILayout.Toggle("Use own attack", 
						DataHolder.Weapon(selection).ownAttack, GUILayout.Width(pw.mWidth));
				if(DataHolder.Weapon(selection).ownAttack)
				{
					DataHolder.Weapon(selection).element = EditorGUILayout.Popup("Attack element", 
							DataHolder.Weapon(selection).element, pw.GetElements(), GUILayout.Width(pw.mWidth));
					EditorGUILayout.Separator();
					
					if(GUILayout.Button("Add Attack", GUILayout.Width(pw.mWidth3)))
					{
						DataHolder.Weapon(selection).AddBaseAttack();
					}
					
					for(int i=0; i<DataHolder.Weapon(selection).baseAttack.Length; i++)
					{
						EditorGUILayout.BeginHorizontal();
						DataHolder.Weapon(selection).baseAttack[i] = EditorGUILayout.Popup("Attack "+(i+1), 
								DataHolder.Weapon(selection).baseAttack[i], DataHolder.BaseAttacks().GetNameList(true),
								GUILayout.Width(pw.mWidth));
						
						if(DataHolder.Weapon(selection).baseAttack.Length > 1)
						{
							if(GUILayout.Button("Remove", GUILayout.Width(pw.mWidth3)))
							{
								DataHolder.Weapon(selection).RemoveBaseAttack(i);
								break;
							}
						}
						GUILayout.FlexibleSpace();
						EditorGUILayout.EndHorizontal();
					}
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box", GUILayout.MinWidth(pw.mWidth));
			fold7 = EditorGUILayout.Foldout(fold7, "Battle Animation Settings");
			if(fold7)
			{
				DataHolder.Weapon(selection).ownBaseAnimations = EditorGUILayout.Toggle("Own animations", 
						DataHolder.Weapon(selection).ownBaseAnimations, GUILayout.Width(pw.mWidth));
				
				DataHolder.Weapon(selection).battleAnimations = EditorHelper.BaseAnimationsSettings(
						DataHolder.Weapon(selection).battleAnimations, 
						DataHolder.Weapon(selection).ownBaseAnimations, true);
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			
			EditorGUILayout.BeginVertical("box");
			fold16 = EditorGUILayout.Foldout(fold16, "Bonus/difficulty settings");
			if(fold16)
			{
				EditorHelper.BonusSettings(ref DataHolder.Weapon(selection).bonus, false);
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
					DataHolder.Weapon(selection).elementValue[i] = EditorGUILayout.IntField(pw.GetElement(i), 
							DataHolder.Weapon(selection).elementValue[i], GUILayout.Width(pw.mWidth));
					DataHolder.Weapon(selection).elementOperator[i] = (SimpleOperator)this.EnumToolbar("", 
							(int)DataHolder.Weapon(selection).elementOperator[i], typeof(SimpleOperator));
					EditorGUILayout.EndHorizontal();
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold8 = EditorGUILayout.Foldout(fold8, "Race damage factor changes");
			if(fold8)
			{
				for(int i=0; i<DataHolder.Weapon(selection).raceValue.Length; i++)
				{
					DataHolder.Weapon(selection).raceValue[i] = EditorGUILayout.IntField(DataHolder.Race(i), 
							DataHolder.Weapon(selection).raceValue[i], GUILayout.Width(pw.mWidth));
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold9 = EditorGUILayout.Foldout(fold9, "Size damage factor changes");
			if(fold9)
			{
				for(int i=0; i<DataHolder.Weapon(selection).sizeValue.Length; i++)
				{
					DataHolder.Weapon(selection).sizeValue[i] = EditorGUILayout.IntField(DataHolder.Size(i), 
							DataHolder.Weapon(selection).sizeValue[i], GUILayout.Width(pw.mWidth));
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical();
			EditorGUILayout.BeginVertical("box");
			fold5 = EditorGUILayout.Foldout(fold5, "Status Effects");
			if(fold5)
			{
				for(int i=0; i<pw.GetStatusEffectCount(); i++)
				{
					DataHolder.Weapon(selection).skillEffect[i] = (SkillEffect)this.EnumToolbar(pw.GetStatusEffect(i), (int)DataHolder.Weapon(selection).skillEffect[i], typeof(SkillEffect));
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
			
			fold4 = EditorGUILayout.Foldout(fold4, "Enable Skills");
			if(fold4)
			{
				if(GUILayout.Button("Add Skill", GUILayout.Width(pw.mWidth2)))
				{
					DataHolder.Weapon(selection).AddEquipmentSkill();
				}
				EditorGUILayout.BeginHorizontal();
				for(int i=0; i<DataHolder.Weapon(selection).equipmentSkill.Length; i++)
				{
					EditorGUILayout.BeginVertical("box", GUILayout.Width(pw.mWidth));
					EditorGUILayout.Separator();
					GUILayout.Label ("Skill "+(i+1).ToString(), EditorStyles.boldLabel);
					if(GUILayout.Button("Remove Skill", GUILayout.Width(pw.mWidth2)))
					{
						DataHolder.Weapon(selection).RemoveEquipmentSkill(i);
						break;
					}
					EditorGUILayout.Separator();
					
					DataHolder.Weapon(selection).equipmentSkill[i].skill = EditorGUILayout.Popup("Skill", 
							DataHolder.Weapon(selection).equipmentSkill[i].skill, pw.GetSkills());
					DataHolder.Weapon(selection).equipmentSkill[i].skillLevel = EditorGUILayout.IntField("Skill level", 
							DataHolder.Weapon(selection).equipmentSkill[i].skillLevel);
					DataHolder.Weapon(selection).equipmentSkill[i].skillLevel = this.MinMaxCheck(
							DataHolder.Weapon(selection).equipmentSkill[i].skillLevel, 1, 
							DataHolder.Skill(DataHolder.Weapon(selection).equipmentSkill[i].skill).level.Length);
					
					EditorGUILayout.Separator();
					GUILayout.Label ("Requirements", EditorStyles.boldLabel);
					EditorGUILayout.BeginHorizontal();
					DataHolder.Weapon(selection).equipmentSkill[i].requireLevel = EditorGUILayout.BeginToggleGroup("Level", 
							DataHolder.Weapon(selection).equipmentSkill[i].requireLevel);
					DataHolder.Weapon(selection).equipmentSkill[i].level = EditorGUILayout.IntField(
							DataHolder.Weapon(selection).equipmentSkill[i].level);
					EditorGUILayout.EndToggleGroup();
					EditorGUILayout.EndHorizontal();
					
					EditorGUILayout.BeginHorizontal();
					DataHolder.Weapon(selection).equipmentSkill[i].requireClass = EditorGUILayout.BeginToggleGroup("Class", 
							DataHolder.Weapon(selection).equipmentSkill[i].requireClass);
					DataHolder.Weapon(selection).equipmentSkill[i].classNumber = EditorGUILayout.Popup(
							DataHolder.Weapon(selection).equipmentSkill[i].classNumber, pw.GetClasses());
					EditorGUILayout.EndToggleGroup();
					EditorGUILayout.EndHorizontal();
					
					EditorGUILayout.BeginHorizontal();
					DataHolder.Weapon(selection).equipmentSkill[i].requireClassLevel = EditorGUILayout.BeginToggleGroup("Class level", 
							DataHolder.Weapon(selection).equipmentSkill[i].requireClassLevel);
					DataHolder.Weapon(selection).equipmentSkill[i].classLevel = EditorGUILayout.IntField(
							DataHolder.Weapon(selection).equipmentSkill[i].classLevel);
					EditorGUILayout.EndToggleGroup();
					EditorGUILayout.EndHorizontal();
					
					EditorGUILayout.Separator();
					for(int j=0; j<pw.GetStatusValueCount(); j++)
					{
						DataHolder.Weapon(selection).equipmentSkill[i].requireStatus[j] = EditorGUILayout.BeginToggleGroup(
								pw.GetStatusValue(j), DataHolder.Weapon(selection).equipmentSkill[i].requireStatus[j]);
						DataHolder.Weapon(selection).equipmentSkill[i].statusRequirement[j] = (ValueCheck)this.EnumToolbar("", 
								(int)DataHolder.Weapon(selection).equipmentSkill[i].statusRequirement[j], typeof(ValueCheck));
						DataHolder.Weapon(selection).equipmentSkill[i].statusValue[j] = EditorGUILayout.IntField(
								DataHolder.Weapon(selection).equipmentSkill[i].statusValue[j]);
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