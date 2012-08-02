
using UnityEditor;
using UnityEngine;

public class EnemyTab : BaseTab
{
	private GameObject tmpPrefab;
	
	public EnemyTab(ProjectWindow pw) : base(pw)
	{
		this.Reload();
	}
	
	new public void Reload()
	{
		base.Reload();
		this.tmpPrefab = null;
	}
	
	public void ShowTab()
	{
		int tmpSelection = selection;
		EditorGUILayout.BeginVertical();
		
		// buttons
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Add Enemy", GUILayout.Width(pw.mWidth2)))
		{
			DataHolder.Enemies().filter.Deactivate();
			DataHolder.Enemies().AddEnemy("New Enemy", "New Description", 
						pw.GetLangCount(), pw.GetStatusEffectCount(),
						pw.GetStatusValueCount(), pw.GetElementCount());
			selection = DataHolder.Enemies().GetDataCount()-1;
			GUI.FocusControl ("ID");
		}
		if(selection >= 0)
		{
			if(this.ShowCopyButton(DataHolder.Enemies()))
			{
				DataHolder.Enemies().filter.Deactivate();
			}
		}
		if(DataHolder.Enemies().GetDataCount() > 1 && selection >= 0)
		{
			if(this.ShowRemButton("Remove Enemy", DataHolder.Enemies()))
			{
				DataHolder.Enemies().filter.Deactivate();
			}
		}
		this.CheckSelection(DataHolder.Enemies());
		EditorGUILayout.EndHorizontal();
		
		// elements list
		this.AddItemListFilter(DataHolder.Enemies(), 
				"race", DataHolder.Races().GetNameList(true), 
				"size", DataHolder.Sizes().GetNameList(true));
		
		// element settings
		EditorGUILayout.BeginVertical();
		SP2 = EditorGUILayout.BeginScrollView(SP2);
		if(DataHolder.Enemies().GetDataCount() > 0 && selection >= 0)
		{
			this.AddID("Enemy ID");
			this.AddMultiLangIcon("Enemy Name", DataHolder.Enemies());
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical("box", GUILayout.Width(pw.mWidth));
			fold2 = EditorGUILayout.Foldout(fold2, "Enemy settings");
			if(fold2)
			{
				if(selection != tmpSelection) this.tmpPrefab = null;
				EditorHelper.PrefabSettings("Prefab", ref DataHolder.Enemy(selection).prefabName, 
						ref this.tmpPrefab, EnemyData.PREFAB_PATH);
				DataHolder.Enemy(selection).prefabRoot = EditorGUILayout.TextField("Child as root", 
						DataHolder.Enemy(selection).prefabRoot, GUILayout.Width(pw.mWidth*2));
				
				EditorGUILayout.Separator();
				
				DataHolder.Enemy(selection).raceID = EditorGUILayout.Popup("Race", 
						DataHolder.Enemy(selection).raceID, DataHolder.Races().GetNameList(true), 
						GUILayout.Width(pw.mWidth*1.5f));
				DataHolder.Enemy(selection).sizeID = EditorGUILayout.Popup("Size", 
						DataHolder.Enemy(selection).sizeID, DataHolder.Sizes().GetNameList(true), 
						GUILayout.Width(pw.mWidth*1.5f));
				// update filter on change
				if((DataHolder.Enemies().filter.useFilter[0] && 
					DataHolder.Enemy(selection).raceID != DataHolder.Enemies().filter.filterID[0]) ||
					(DataHolder.Enemies().filter.useFilter[1] && 
					DataHolder.Enemy(selection).sizeID != DataHolder.Enemies().filter.filterID[1]))
				{
					DataHolder.Enemies().filter.filterID[0] = DataHolder.Enemy(selection).raceID;
					DataHolder.Enemies().filter.filterID[1] = DataHolder.Enemy(selection).sizeID;
					DataHolder.Enemies().CreateFilterList(true);
				}
				
				DataHolder.Enemy(selection).level = EditorGUILayout.IntField("Level", 
						DataHolder.Enemy(selection).level, GUILayout.Width(pw.mWidth));
				if(DataHolder.Enemy(selection).level < 1) DataHolder.Enemy(selection).level = 1;
				DataHolder.Enemy(selection).classLevel = EditorGUILayout.IntField("Class level", 
						DataHolder.Enemy(selection).classLevel, GUILayout.Width(pw.mWidth));
				if(DataHolder.Enemy(selection).classLevel < 1) DataHolder.Enemy(selection).classLevel = 1;
				EditorGUILayout.Separator();
				GUILayout.Label("Status values", EditorStyles.boldLabel);
				StatusValue[] sv = pw.GetStatusValuesData();
				for(int i=0; i<sv.Length; i++)
				{
					if(sv[i].IsNormal())
					{
						DataHolder.Enemy(selection).value[i] = EditorGUILayout.IntField(pw.GetStatusValue(i), 
								DataHolder.Enemy(selection).value[i], GUILayout.Width(pw.mWidth));
						
						if(DataHolder.Enemy(selection).value[i] < sv[i].minValue)
						{
							DataHolder.Enemy(selection).value[i] = sv[i].minValue;
						}
						else if(DataHolder.Enemy(selection).value[i] > sv[i].maxValue)
						{
							DataHolder.Enemy(selection).value[i] = sv[i].maxValue;
						}
					}
				}
				
				EditorGUILayout.Separator();
				GUILayout.Label("Chance settings", EditorStyles.boldLabel);
				DataHolder.Enemy(selection).baseCounter = EditorGUILayout.Popup("Counter chance", 
						DataHolder.Enemy(selection).baseCounter, pw.GetFormulas(), GUILayout.Width(pw.mWidth*1.5f));
				DataHolder.Enemy(selection).baseCritical = EditorGUILayout.Popup("Critical chance", 
						DataHolder.Enemy(selection).baseCritical, pw.GetFormulas(), GUILayout.Width(pw.mWidth*1.5f));
				DataHolder.Enemy(selection).baseBlock = EditorGUILayout.Popup("Block chance", 
						DataHolder.Enemy(selection).baseBlock, pw.GetFormulas(), GUILayout.Width(pw.mWidth*1.5f));
				
				EditorGUILayout.Separator();
				GUILayout.Label("Victory gains", EditorStyles.boldLabel);
				DataHolder.Enemy(selection).money = EditorGUILayout.IntField("Money", 
						DataHolder.Enemy(selection).money, GUILayout.Width(pw.mWidth));
				for(int i=0; i<sv.Length; i++)
				{
					if(sv[i].IsExperience())
					{
						DataHolder.Enemy(selection).value[i] = EditorGUILayout.IntField(pw.GetStatusValue(i), 
								DataHolder.Enemy(selection).value[i], GUILayout.Width(pw.mWidth));
					}
				}
				
				EditorGUILayout.Separator();
				GUILayout.Label("Steal settings", EditorStyles.boldLabel);
				DataHolder.Enemy(selection).stealItem = EditorGUILayout.Toggle("Steal item", 
						DataHolder.Enemy(selection).stealItem, GUILayout.Width(pw.mWidth));
				if(DataHolder.Enemy(selection).stealItem)
				{
					DataHolder.Enemy(selection).stealItemFactor = EditorGUILayout.FloatField("Steal factor", 
							DataHolder.Enemy(selection).stealItemFactor, GUILayout.Width(pw.mWidth));
					DataHolder.Enemy(selection).stealItemType = (ItemDropType)EditorGUILayout.EnumPopup("Type", 
							DataHolder.Enemy(selection).stealItemType, GUILayout.Width(pw.mWidth*1.5f));
					if(ItemDropType.ITEM.Equals(DataHolder.Enemy(selection).stealItemType))
					{
						DataHolder.Enemy(selection).stealItemID = EditorGUILayout.Popup("Item", 
								DataHolder.Enemy(selection).stealItemID, DataHolder.Items().GetNameList(true), 
								GUILayout.Width(pw.mWidth*1.5f));
					}
					else if(ItemDropType.WEAPON.Equals(DataHolder.Enemy(selection).stealItemType))
					{
						DataHolder.Enemy(selection).stealItemID = EditorGUILayout.Popup("Weapon", 
								DataHolder.Enemy(selection).stealItemID, DataHolder.Weapons().GetNameList(true), 
								GUILayout.Width(pw.mWidth*1.5f));
					}
					else if(ItemDropType.ARMOR.Equals(DataHolder.Enemy(selection).stealItemType))
					{
						DataHolder.Enemy(selection).stealItemID = EditorGUILayout.Popup("Armor", 
								DataHolder.Enemy(selection).stealItemID, DataHolder.Armors().GetNameList(true), 
								GUILayout.Width(pw.mWidth*1.5f));
					}
					DataHolder.Enemy(selection).stealItemOnce = EditorGUILayout.Toggle("Steal once", 
							DataHolder.Enemy(selection).stealItemOnce, GUILayout.Width(pw.mWidth));
				}
				EditorGUILayout.Separator();
				DataHolder.Enemy(selection).stealMoney = EditorGUILayout.Toggle("Steal money", 
						DataHolder.Enemy(selection).stealMoney, GUILayout.Width(pw.mWidth));
				if(DataHolder.Enemy(selection).stealMoney)
				{
					DataHolder.Enemy(selection).stealMoneyFactor = EditorGUILayout.FloatField("Steal factor", 
							DataHolder.Enemy(selection).stealMoneyFactor, GUILayout.Width(pw.mWidth));
					DataHolder.Enemy(selection).stealMoneyAmount = EditorGUILayout.IntField("Money amount", 
							DataHolder.Enemy(selection).stealMoneyAmount, GUILayout.Width(pw.mWidth));
					DataHolder.Enemy(selection).stealMoneyOnce = EditorGUILayout.Toggle("Steal once", 
							DataHolder.Enemy(selection).stealMoneyOnce, GUILayout.Width(pw.mWidth));
				}
				
				EditorGUILayout.Separator();
				GUILayout.Label("Move speed settings", EditorStyles.boldLabel);
				DataHolder.Enemy(selection).useMoveSpeedFormula = EditorGUILayout.Toggle("Move speed formula", 
						DataHolder.Enemy(selection).useMoveSpeedFormula, GUILayout.Width(pw.mWidth));
				if(DataHolder.Enemy(selection).useMoveSpeedFormula)
				{
					DataHolder.Enemy(selection).moveSpeedFormula = EditorGUILayout.Popup("Formula",
							DataHolder.Enemy(selection).moveSpeedFormula, 
							DataHolder.Formulas().GetNameList(true), GUILayout.Width(pw.mWidth));
				}
				else
				{
					DataHolder.Enemy(selection).moveSpeed = EditorGUILayout.FloatField("Move speed",
							DataHolder.Enemy(selection).moveSpeed, GUILayout.Width(pw.mWidth));
					if(DataHolder.Enemy(selection).moveSpeed <= 0)
					{
						DataHolder.Enemy(selection).moveSpeed = 0.1f;
					}
				}
				DataHolder.Enemy(selection).minMoveSpeed = EditorGUILayout.FloatField("Minimum speed",
						DataHolder.Enemy(selection).minMoveSpeed, GUILayout.Width(pw.mWidth));
				if(DataHolder.Enemy(selection).minMoveSpeed <= 0)
				{
					DataHolder.Enemy(selection).minMoveSpeed = 0.1f;
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold10 = EditorGUILayout.Foldout(fold10, "AI Mover Settings");
			if(fold10)
			{
				DataHolder.Enemy(selection).aiMoverSettings = EditorHelper.AIMoverSettings(
						DataHolder.Enemy(selection).aiMoverSettings, false, true);
			}
			EditorGUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			
			EditorGUILayout.BeginVertical("box");
			fold17 = EditorGUILayout.Foldout(fold17, "Status value time changes");
			if(fold17)
			{
				GUILayout.Label("Battle", EditorStyles.boldLabel);
				if(GUILayout.Button("Add", GUILayout.Width(pw.mWidth3)))
				{
					DataHolder.Enemy(selection).fieldStatusChange = ArrayHelper.Add(new StatusTimeChange(), 
							DataHolder.Enemy(selection).fieldStatusChange);
				}
				for(int i=0; i<DataHolder.Enemy(selection).fieldStatusChange.Length; i++)
				{
					EditorGUILayout.BeginVertical("box");
					if(GUILayout.Button("Remove", GUILayout.Width(pw.mWidth3)))
					{
						DataHolder.Enemy(selection).fieldStatusChange = ArrayHelper.Remove(i, 
								DataHolder.Enemy(selection).fieldStatusChange);
						break;
					}
					EditorHelper.StatusTimeChange(ref DataHolder.Enemy(selection).fieldStatusChange[i]);
					EditorGUILayout.EndVertical();
				}
				
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical("box", GUILayout.Width(pw.mWidth));
			fold3 = EditorGUILayout.Foldout(fold3, "Element effectiveness");
			if(fold3)
			{
				for(int i=0; i<pw.GetElementCount(); i++)
				{
					DataHolder.Enemy(selection).elementValue[i] = EditorGUILayout.IntField(pw.GetElement(i), DataHolder.Enemy(selection).elementValue[i]);
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box", GUILayout.Width(pw.mWidth));
			fold13 = EditorGUILayout.Foldout(fold13, "Race damage factor");
			if(fold13)
			{
				for(int i=0; i<DataHolder.Enemy(selection).raceValue.Length; i++)
				{
					DataHolder.Enemy(selection).raceValue[i] = EditorGUILayout.IntField(DataHolder.Race(i), 
							DataHolder.Enemy(selection).raceValue[i], GUILayout.Width(pw.mWidth));
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box", GUILayout.Width(pw.mWidth));
			fold14 = EditorGUILayout.Foldout(fold14, "Size damage factor");
			if(fold14)
			{
				for(int i=0; i<DataHolder.Enemy(selection).sizeValue.Length; i++)
				{
					DataHolder.Enemy(selection).sizeValue[i] = EditorGUILayout.IntField(DataHolder.Size(i), 
							DataHolder.Enemy(selection).sizeValue[i], GUILayout.Width(pw.mWidth));
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold7 = EditorGUILayout.Foldout(fold7, "Status Effects");
			if(fold7)
			{
				for(int i=0; i<pw.GetStatusEffectCount(); i++)
				{
					DataHolder.Enemy(selection).skillEffect[i] = (SkillEffect)this.EnumToolbar(pw.GetStatusEffect(i), 
							(int)DataHolder.Enemy(selection).skillEffect[i], typeof(SkillEffect));
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			
			EditorGUILayout.BeginVertical("box");
			fold16 = EditorGUILayout.Foldout(fold16, "Bonus/difficulty settings");
			if(fold16)
			{
				EditorHelper.BonusSettings(ref DataHolder.Enemy(selection).bonus, true);
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			
			fold4 = EditorGUILayout.Foldout(fold4, "Item Drops");
			if(fold4)
			{
				if(GUILayout.Button("Add Item Drop", GUILayout.Width(pw.mWidth2)))
				{
					DataHolder.Enemies().AddItemDrop(selection);
				}
				for(int i=0; i<DataHolder.Enemy(selection).itemDrop.Length; i++)
				{
					EditorGUILayout.BeginVertical("box");
					
					EditorGUILayout.BeginHorizontal();
					if(GUILayout.Button("Remove", GUILayout.Width(pw.mWidth3)))
					{
						DataHolder.Enemies().RemoveItemDrop(selection, i);
						break;
					}
					
					ItemDropType prev = DataHolder.Enemy(selection).itemDrop[i].type;
					DataHolder.Enemy(selection).itemDrop[i].type = (ItemDropType)this.EnumToolbar("", 
							(int)DataHolder.Enemy(selection).itemDrop[i].type, typeof(ItemDropType));
					if(prev != DataHolder.Enemy(selection).itemDrop[i].type)
					{
						DataHolder.Enemy(selection).itemDrop[i].itemID = 0;
					}
					
					if(ItemDropType.ITEM.Equals(DataHolder.Enemy(selection).itemDrop[i].type))
					{
						DataHolder.Enemy(selection).itemDrop[i].itemID = EditorGUILayout.Popup(
								DataHolder.Enemy(selection).itemDrop[i].itemID, pw.GetItems(), GUILayout.Width(pw.mWidth*0.7f));
					}
					else if(ItemDropType.WEAPON.Equals(DataHolder.Enemy(selection).itemDrop[i].type))
					{
						DataHolder.Enemy(selection).itemDrop[i].itemID = EditorGUILayout.Popup(
								DataHolder.Enemy(selection).itemDrop[i].itemID, pw.GetWeapons(), GUILayout.Width(pw.mWidth*0.7f));
					}
					else if(ItemDropType.ARMOR.Equals(DataHolder.Enemy(selection).itemDrop[i].type))
					{
						DataHolder.Enemy(selection).itemDrop[i].itemID = EditorGUILayout.Popup(
								DataHolder.Enemy(selection).itemDrop[i].itemID, pw.GetArmors(), GUILayout.Width(pw.mWidth*0.7f));
					}
					
					DataHolder.Enemy(selection).itemDrop[i].quantity = EditorGUILayout.IntField("Quantity", 
							DataHolder.Enemy(selection).itemDrop[i].quantity, GUILayout.Width(pw.mWidth));
					if(DataHolder.Enemy(selection).itemDrop[i].quantity < 1)
					{
						DataHolder.Enemy(selection).itemDrop[i].quantity = 1;
					}
					DataHolder.Enemy(selection).itemDrop[i].chance = EditorGUILayout.FloatField("Chance (%)", 
							DataHolder.Enemy(selection).itemDrop[i].chance, GUILayout.Width(pw.mWidth));
					FloatHelper.ChanceLimit(ref DataHolder.Enemy(selection).itemDrop[i].chance);
					
					GUILayout.FlexibleSpace();
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.EndVertical();
				}
				this.Separate();
			}
			
			EditorGUILayout.BeginVertical("box");
			fold12 = EditorGUILayout.Foldout(fold12, "Variable changes");
			if(fold12)
			{
				DataHolder.Enemy(selection).variables = EditorHelper.GameVariableSettings(DataHolder.Enemy(selection).variables);
				EditorGUILayout.Separator();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical("box", GUILayout.MinWidth(pw.mWidth));
			fold6 = EditorGUILayout.Foldout(fold6, "Attack Settings");
			if(fold6)
			{
				DataHolder.Enemy(selection).baseElement = EditorGUILayout.Popup(
						"Attack element", DataHolder.Enemy(selection).baseElement, 
						DataHolder.Elements().GetNameList(true), GUILayout.Width(pw.mWidth));
				EditorGUILayout.Separator();
				
				if(GUILayout.Button("Add", GUILayout.Width(pw.mWidth3)))
				{
					DataHolder.Enemy(selection).AddBaseAttack();
				}
				for(int i=0; i<DataHolder.Enemy(selection).baseAttack.Length; i++)
				{
					EditorGUILayout.BeginHorizontal();
					DataHolder.Enemy(selection).baseAttack[i] = EditorGUILayout.Popup("Attack "+(i+1), 
							DataHolder.Enemy(selection).baseAttack[i], DataHolder.BaseAttacks().GetNameList(true),
							GUILayout.Width(pw.mWidth));
					
					if(DataHolder.Enemy(selection).baseAttack.Length > 1)
					{
						if(GUILayout.Button("Remove", GUILayout.Width(pw.mWidth3)))
						{
							DataHolder.Enemy(selection).RemoveBaseAttack(i);
							break;
						}
					}
					GUILayout.FlexibleSpace();
					EditorGUILayout.EndHorizontal();
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box", GUILayout.MinWidth(pw.mWidth));
			fold9 = EditorGUILayout.Foldout(fold9, "Battle Audio Settings");
			if(fold9)
			{
				for(int i=0; i<DataHolder.Enemy(selection).audioClipName.Length; i++)
				{
					if(DataHolder.Enemy(selection).audioClip[i] == null && 
						DataHolder.Enemy(selection).audioClipName[i] != null && 
						"" != DataHolder.Enemy(selection).audioClipName[i])
					{
						DataHolder.Enemy(selection).audioClip[i] = (AudioClip)Resources.Load(
								BattleSystemData.AUDIO_PATH+
								DataHolder.Enemy(selection).audioClipName[i], typeof(AudioClip));
					}
					DataHolder.Enemy(selection).audioClip[i] = (AudioClip)EditorGUILayout.ObjectField("Clip "+i, 
							DataHolder.Enemy(selection).audioClip[i], typeof(AudioClip), false, GUILayout.Width(pw.mWidth*1.2f));
					if(DataHolder.Enemy(selection).audioClip[i])
					{
						DataHolder.Enemy(selection).audioClipName[i] = DataHolder.Enemy(selection).audioClip[i].name;
					}
					else DataHolder.Enemy(selection).audioClipName[i] = "";
				}
				EditorGUILayout.Separator();
			}
			EditorGUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginVertical("box");
			fold11 = EditorGUILayout.Foldout(fold11, "Field animation settings");
			if(fold11)
			{
				DataHolder.Enemy(selection).fieldAnimations = EditorHelper.FieldAnimationsSettings(
						DataHolder.Enemy(selection).fieldAnimations);
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold5 = EditorGUILayout.Foldout(fold5, "Battle animation settings");
			if(fold5)
			{
				DataHolder.Enemy(selection).battleAnimations = EditorHelper.BaseAnimationsSettings(
						DataHolder.Enemy(selection).battleAnimations, true, false);
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold15 = EditorGUILayout.Foldout(fold15, "Custom animation settings");
			if(fold15)
			{
				DataHolder.Enemy(selection).customAnimations = EditorHelper.CustomAnimationSettings(
						DataHolder.Enemy(selection).customAnimations);
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			fold8 = EditorGUILayout.Foldout(fold8, "AI Settings");
			if(fold8)
			{
				DataHolder.Enemy(selection).attackPartyTarget = false;
				if(DataHolder.BattleSystem().IsRealTime())
				{
					DataHolder.Enemy(selection).aggressiveType = (AggressiveType)EditorGUILayout.EnumPopup("Aggressive", 
							DataHolder.Enemy(selection).aggressiveType, GUILayout.Width(pw.mWidth));
				}
				else
				{
					DataHolder.Enemy(selection).aggressiveType = AggressiveType.ALWAYS;
				}
				
				DataHolder.Enemy(selection).attackLastTarget = EditorGUILayout.Toggle("Atk last target",
						DataHolder.Enemy(selection).attackLastTarget, GUILayout.Width(pw.mWidth));
				DataHolder.Enemy(selection).aiNearestTarget = EditorGUILayout.Toggle("Nearest target",
							DataHolder.Enemy(selection).aiNearestTarget, GUILayout.Width(pw.mWidth));
				if(DataHolder.BattleSystem().IsRealTime())
				{
					DataHolder.Enemy(selection).aiTimeout = EditorGUILayout.FloatField("Timeout",
							DataHolder.Enemy(selection).aiTimeout, GUILayout.Width(pw.mWidth));
				}
				if(GUILayout.Button("Add Behaviour", GUILayout.Width(pw.mWidth2)))
				{
					DataHolder.Enemy(selection).AddAIBehaviour();
				}
				for(int i=0; i<DataHolder.Enemy(selection).aiBehaviour.Length; i++)
				{
					EditorGUILayout.BeginVertical("box");
					EditorGUILayout.BeginHorizontal();
					if(GUILayout.Button("Remove", GUILayout.Width(pw.mWidth3)))
					{
						DataHolder.Enemy(selection).RemoveAIBehaviour(i);
						break;
					}
					if(i > 0)
					{
						if(GUILayout.Button("Move up", GUILayout.Width(pw.mWidth3)))
						{
							DataHolder.Enemy(selection).MoveAIBehaviour(i, -1);
						}
					}
					if(i < DataHolder.Enemy(selection).aiBehaviour.Length-1)
					{
						if(GUILayout.Button("Move down", GUILayout.Width(pw.mWidth3)))
						{
							DataHolder.Enemy(selection).MoveAIBehaviour(i, 1);
						}
					}
					EditorGUILayout.EndHorizontal();
					DataHolder.Enemy(selection).aiBehaviour[i] = EditorHelper.AIBehaviourSettings(
							DataHolder.Enemy(selection).aiBehaviour[i], i);
					EditorGUILayout.EndVertical();
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
		}
		this.EndTab();
	}
}