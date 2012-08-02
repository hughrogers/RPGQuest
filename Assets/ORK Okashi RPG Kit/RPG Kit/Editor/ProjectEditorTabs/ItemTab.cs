
using UnityEditor;
using UnityEngine;

public class ItemTab : BaseTab
{
	private GameObject tmpPrefab;
	private AudioClip tmpAudio;
	
	public ItemTab(ProjectWindow pw) : base(pw)
	{
		this.Reload();
	}
	
	new public void Reload()
	{
		base.Reload();
		this.tmpPrefab = null;
		this.tmpAudio = null;
	}
	
	public void ShowTab()
	{
		int tmpSelection = selection;
		EditorGUILayout.BeginVertical();
		
		// buttons
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Add Item", GUILayout.Width(pw.mWidth2)))
		{
			DataHolder.Items().filter.Deactivate();
			DataHolder.Items().AddItem("New Item", "New Description", 
						pw.GetLangCount(), pw.GetStatusEffectCount(),
						pw.GetStatusValueCount());
			selection = DataHolder.Items().GetDataCount()-1;
			GUI.FocusControl ("ID");
		}
		if(selection >= 0)
		{
			if(this.ShowCopyButton(DataHolder.Items()))
			{
				DataHolder.Items().filter.Deactivate();
			}
		}
		if(DataHolder.Items().GetDataCount() > 1 && selection >= 0)
		{
			if(this.ShowRemButton("Remove Item", DataHolder.Items()))
			{
				DataHolder.Items().filter.Deactivate();
				pw.RemoveItem(selection);
			}
		}
		this.CheckSelection(DataHolder.Items());
		EditorGUILayout.EndHorizontal();
		
		// elements list
		this.AddItemListFilter(DataHolder.Items(), "type", DataHolder.ItemTypes().GetNameList(true));
		
		// element settings
		EditorGUILayout.BeginVertical();
		SP2 = EditorGUILayout.BeginScrollView(SP2);
		if(DataHolder.Items().GetDataCount() > 0 && selection >= 0)
		{
			EditorGUILayout.BeginHorizontal();
			this.AddID("Item ID");
			this.AddMultiLangIcon("Item Name", DataHolder.Items());
			
			EditorGUILayout.BeginVertical("box");
			fold2 = EditorGUILayout.Foldout(fold2, "Item Settings");
			if(fold2)
			{
				if(selection != tmpSelection) this.tmpPrefab = null;
				if(this.tmpPrefab == null && "" != DataHolder.Item(selection).prefabName)
				{
					this.tmpPrefab = (GameObject)Resources.Load(DataHolder.Items().GetPrefabPath()+DataHolder.Item(selection).prefabName, typeof(GameObject));
				}
				this.tmpPrefab = (GameObject)EditorGUILayout.ObjectField("Prefab", this.tmpPrefab, typeof(GameObject), false, GUILayout.Width(pw.mWidth*2));
				if(this.tmpPrefab) DataHolder.Item(selection).prefabName = this.tmpPrefab.name;
				else DataHolder.Item(selection).prefabName = "";
				
				EditorGUILayout.Separator();
				DataHolder.Item(selection).itemType = EditorGUILayout.Popup("Item type", 
						DataHolder.Item(selection).itemType, pw.GetItemTypes(), GUILayout.Width(pw.mWidth));
				
				// update filter on change
				if(DataHolder.Items().filter.useFilter[0] && 
					DataHolder.Item(selection).itemType != DataHolder.Items().filter.filterID[0])
				{
					DataHolder.Items().filter.filterID[0] = DataHolder.Item(selection).itemType;
					DataHolder.Items().CreateFilterList(true);
				}
				
				EditorGUILayout.Separator();
				DataHolder.Item(selection).dropable = EditorGUILayout.Toggle("Dropable", DataHolder.Item(selection).dropable, GUILayout.Width(pw.mWidth));
				DataHolder.Item(selection).stealable = EditorGUILayout.Toggle("Stealable", DataHolder.Item(selection).stealable, GUILayout.Width(pw.mWidth));
				EditorGUILayout.Separator();
				DataHolder.Item(selection).buyPrice = EditorGUILayout.IntField("Buy price", DataHolder.Item(selection).buyPrice, GUILayout.Width(pw.mWidth));
				DataHolder.Item(selection).sellable = EditorGUILayout.BeginToggleGroup("Sellable", DataHolder.Item(selection).sellable);
				DataHolder.Item(selection).sellPrice = EditorGUILayout.IntField("Sell price", DataHolder.Item(selection).sellPrice, GUILayout.Width(pw.mWidth));
				DataHolder.Item(selection).sellSetter = (ValueSetter)this.EnumToolbar("Sell in", (int)DataHolder.Item(selection).sellSetter, typeof(ValueSetter));
				EditorGUILayout.EndToggleGroup();
				
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold3 = EditorGUILayout.Foldout(fold3, "Usage Settings");
			if(fold3)
			{
				DataHolder.Item(selection).useable = EditorGUILayout.Toggle("Useable", 
						DataHolder.Item(selection).useable, GUILayout.Width(pw.mWidth));
				
				if(DataHolder.Item(selection).useable)
				{
					if(selection != tmpSelection) this.tmpAudio = null;
					if(this.tmpAudio == null && "" != DataHolder.Item(selection).audioName)
					{
						this.tmpAudio = (AudioClip)Resources.Load(ItemData.AUDIO_PATH+
								DataHolder.Item(selection).audioName, typeof(AudioClip));
					}
					this.tmpAudio = (AudioClip)EditorGUILayout.ObjectField("Use audio", this.tmpAudio, 
							typeof(AudioClip), false, GUILayout.Width(pw.mWidth*2));
					if(this.tmpAudio) DataHolder.Item(selection).audioName = this.tmpAudio.name;
					else DataHolder.Item(selection).audioName = "";
					EditorGUILayout.Separator();
					
					DataHolder.Item(selection).useInBattle = EditorGUILayout.Toggle("Usable in battle", 
							DataHolder.Item(selection).useInBattle, GUILayout.Width(pw.mWidth));
					DataHolder.Item(selection).useInField = EditorGUILayout.Toggle("Usable in field", 
							DataHolder.Item(selection).useInField, GUILayout.Width(pw.mWidth));
					EditorGUILayout.Separator();
					
					if(DataHolder.Item(selection).useInField &&
						!DataHolder.Item(selection).useInBattle)
					{
						DataHolder.Item(selection).callGlobalEvent = EditorGUILayout.Toggle("Call global event", 
								DataHolder.Item(selection).callGlobalEvent, GUILayout.Width(pw.mWidth));
						if(DataHolder.Item(selection).callGlobalEvent)
						{
							DataHolder.Item(selection).globalEventID = EditorGUILayout.Popup("Global event", 
									DataHolder.Item(selection).globalEventID, DataHolder.GlobalEvents().GetNameList(true), 
									GUILayout.Width(pw.mWidth*2));
						}
						EditorGUILayout.Separator();
					}
					else DataHolder.Item(selection).callGlobalEvent = false;
					
					if(!DataHolder.Item(selection).callGlobalEvent)
					{
						DataHolder.Item(selection).revive = EditorGUILayout.Toggle("Revive target", 
								DataHolder.Item(selection).revive, GUILayout.Width(pw.mWidth));
						DataHolder.Item(selection).consume = EditorGUILayout.Toggle("Consume", 
								DataHolder.Item(selection).consume, GUILayout.Width(pw.mWidth));
						EditorGUILayout.Separator();
						
						DataHolder.Item(selection).itemSkill = (ItemSkillType)this.EnumToolbar("Item skill", 
								(int)DataHolder.Item(selection).itemSkill, typeof(ItemSkillType));
						if(!ItemSkillType.NONE.Equals(DataHolder.Item(selection).itemSkill))
						{
							DataHolder.Item(selection).skillID = EditorGUILayout.Popup("Skill", 
									DataHolder.Item(selection).skillID, pw.GetSkills(), GUILayout.Width(pw.mWidth));
							DataHolder.Item(selection).skillLevel = EditorGUILayout.IntField("Skill level", 
									DataHolder.Item(selection).skillLevel, GUILayout.Width(pw.mWidth));
							
							DataHolder.Item(selection).skillLevel = this.MinMaxCheck(
									DataHolder.Item(selection).skillLevel, 1, 
									DataHolder.Skill(DataHolder.Item(selection).skillID).level.Length);
						}
						
						EditorGUILayout.Separator();
						DataHolder.Item(selection).itemVariable = (ItemVariableType)this.EnumToolbar("Item variable", 
								(int)DataHolder.Item(selection).itemVariable, typeof(ItemVariableType));
						if(!ItemVariableType.NONE.Equals(DataHolder.Item(selection).itemVariable))
						{
							DataHolder.Item(selection).variableKey = EditorGUILayout.TextField("Variable key", 
									DataHolder.Item(selection).variableKey, GUILayout.Width((int)(pw.mWidth*1.5)));
						}
						if(ItemVariableType.SET.Equals(DataHolder.Item(selection).itemVariable))
						{
							DataHolder.Item(selection).variableValue = EditorGUILayout.TextField("Variable value", 
									DataHolder.Item(selection).variableValue, GUILayout.Width((int)(pw.mWidth*1.5)));
						}
						
						EditorGUILayout.Separator();
						EditorGUILayout.BeginHorizontal();
						DataHolder.Item(selection).learnRecipe = EditorGUILayout.Toggle("Learn recipe", 
								DataHolder.Item(selection).learnRecipe, GUILayout.Width(pw.mWidth));
						if(DataHolder.Item(selection).learnRecipe)
						{
							DataHolder.Item(selection).recipeID = EditorGUILayout.Popup(DataHolder.Item(selection).recipeID, 
									DataHolder.ItemRecipes().GetNameList(true));
						}
						GUILayout.FlexibleSpace();
						EditorGUILayout.EndHorizontal();
						
						EditorGUILayout.Separator();
						EditorGUILayout.BeginHorizontal(GUILayout.Width(pw.mWidth*1.5f));
						DataHolder.Item(selection).battleAnimation = EditorGUILayout.BeginToggleGroup("Use battle animation", DataHolder.Item(selection).battleAnimation);
						DataHolder.Item(selection).animationID = EditorGUILayout.Popup(DataHolder.Item(selection).animationID, DataHolder.BattleAnimations().GetNameList(true));
						EditorGUILayout.EndToggleGroup();
						EditorGUILayout.EndHorizontal();
						
						if(!DataHolder.Item(selection).TargetNone() && 
							!DataHolder.Item(selection).TargetSelf())
						{
							DataHolder.Item(selection).useRange = EditorHelper.UseRangeSettings(DataHolder.Item(selection).useRange);
						}
						else DataHolder.Item(selection).useRange.active = false;
					}
					else DataHolder.Item(selection).useRange.active = false;
				}
				else DataHolder.Item(selection).useRange.active = false;
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			if(DataHolder.Item(selection).useable && 
				!DataHolder.Item(selection).callGlobalEvent)
			{
				EditorGUILayout.BeginVertical("box");
				fold6 = EditorGUILayout.Foldout(fold6, "Target Settings");
				if(fold6)
				{
					DataHolder.Item(selection).targetType = (TargetType)this.EnumToolbar("Target type", 
							(int)DataHolder.Item(selection).targetType, typeof(TargetType));
					if(!DataHolder.Item(selection).TargetSelf())
					{
						DataHolder.Item(selection).skillTarget = (SkillTarget)this.EnumToolbar("Item target", 
								(int)DataHolder.Item(selection).skillTarget, typeof(SkillTarget));
					}
					else DataHolder.Item(selection).skillTarget = SkillTarget.SINGLE;
					if(DataHolder.Item(selection).TargetNone())
					{
						DataHolder.Item(selection).targetRaycast = EditorHelper.TargetRaycastSettings(
								DataHolder.Item(selection).targetRaycast);
					}
					else DataHolder.Item(selection).targetRaycast.active = false;
					
					this.Separate();
				}
				EditorGUILayout.EndVertical();
			
				EditorGUILayout.BeginVertical("box");
				fold4 = EditorGUILayout.Foldout(fold4, "Status Value Changes");
				if(fold4)
				{
					for(int i=0; i<pw.GetStatusValueCount(); i++)
					{
						DataHolder.Item(selection).valueChange[i] = EditorHelper.ValueChangeSettings(i, 
								DataHolder.Item(selection).valueChange[i]);
					}
					this.Separate();
				}
				EditorGUILayout.EndVertical();
			
				EditorGUILayout.EndVertical();
				EditorGUILayout.BeginVertical("box");
				fold5 = EditorGUILayout.Foldout(fold5, "Status Effects");
				if(fold5)
				{
					for(int i=0; i<pw.GetStatusEffectCount(); i++)
					{
						DataHolder.Item(selection).skillEffect[i] = (SkillEffect)this.EnumToolbar(pw.GetStatusEffect(i), (int)DataHolder.Item(selection).skillEffect[i], typeof(SkillEffect));
					}
					this.Separate();
				}
			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
		}
		this.EndTab();
	}
}