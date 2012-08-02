
using UnityEditor;
using UnityEngine;

public class BaseAttackTab : BaseTab
{
	private AudioClip tmpAudio = null;
	private AudioClip tmpAudio2 = null;
	private int tmpSelection = 0;
	
	public BaseAttackTab(ProjectWindow pw) : base(pw)
	{
		this.Reload();
	}
	
	new public void Reload()
	{
		base.Reload();
		this.tmpAudio = null;
		this.tmpAudio2 = null;
	}
	
	public void ShowTab()
	{
		this.tmpSelection = selection;
		EditorGUILayout.BeginVertical();
		
		// buttons
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Add Attack", GUILayout.Width(pw.mWidth2)))
		{
			DataHolder.BaseAttacks().AddAttack("New Attack");
			selection = DataHolder.BaseAttacks().GetDataCount()-1;
			GUI.FocusControl ("ID");
		}
		this.ShowCopyButton(DataHolder.BaseAttacks());
		if(DataHolder.BaseAttacks().GetDataCount() > 1)
		{
			if(this.ShowRemButton("Remove Attack", DataHolder.BaseAttacks()))
			{
				pw.RemoveBaseAttack(selection);
			}
		}
		
		this.CheckSelection(DataHolder.BaseAttacks());
		
		EditorGUILayout.EndHorizontal();
		
		this.AddItemList(DataHolder.BaseAttacks());
		
		if(selection != tmpSelection)
		{
			this.tmpAudio = null;
			this.tmpAudio2 = null;
		}
		
		EditorGUILayout.BeginVertical();
		SP2 = EditorGUILayout.BeginScrollView(SP2);
		if(DataHolder.BaseAttacks().GetDataCount() > 0)
		{
			this.AddID("Attack ID");
			DataHolder.BaseAttacks().name[selection] = EditorGUILayout.TextField("Name", DataHolder.BaseAttacks().name[selection]);
			this.Separate();
			
			EditorGUILayout.BeginVertical("box");
			fold1 = EditorGUILayout.Foldout(fold1, "Base settings");
			if(fold1)
			{
				DataHolder.BaseAttack(selection).availableTime = EditorGUILayout.FloatField("Available time",
						DataHolder.BaseAttack(selection).availableTime, GUILayout.Width(pw.mWidth));
				EditorGUILayout.Separator();
				
				DataHolder.BaseAttack(selection).hasCritical = EditorGUILayout.Toggle("Has critical hit", 
						DataHolder.BaseAttack(selection).hasCritical, GUILayout.Width(pw.mWidth));
				EditorGUILayout.Separator();
				
				DataHolder.BaseAttack(selection).absorb = EditorGUILayout.Toggle("Absorb damage", 
						DataHolder.BaseAttack(selection).absorb, GUILayout.Width(pw.mWidth));
				if(DataHolder.BaseAttack(selection).absorb)
				{
					DataHolder.BaseAttack(selection).absorbValue = EditorGUILayout.IntField("Percent", 
							DataHolder.BaseAttack(selection).absorbValue, GUILayout.Width(pw.mWidth));
				}
				EditorGUILayout.Separator();
				
				DataHolder.BaseAttack(selection).hitChance = EditorGUILayout.Toggle("Calc. hit chance", 
						DataHolder.BaseAttack(selection).hitChance, GUILayout.Width(pw.mWidth));
				if(DataHolder.BaseAttack(selection).hitChance)
				{
					DataHolder.BaseAttack(selection).hitFormula = EditorGUILayout.Popup("Hit chance", DataHolder.BaseAttack(selection).hitFormula, 
							DataHolder.Formulas().GetNameList(true), GUILayout.Width(pw.mWidth));
				}
				
				DataHolder.BaseAttack(selection).useRange = EditorHelper.UseRangeSettings(DataHolder.BaseAttack(selection).useRange);
				EditorGUILayout.Separator();
				
				DataHolder.BaseAttack(selection).overrideAnimation = EditorGUILayout.Toggle("Override animation", 
						DataHolder.BaseAttack(selection).overrideAnimation, GUILayout.Width(pw.mWidth));
				if(DataHolder.BaseAttack(selection).overrideAnimation)
				{
					DataHolder.BaseAttack(selection).animationID = EditorGUILayout.Popup("Animation", DataHolder.BaseAttack(selection).animationID, 
							DataHolder.BattleAnimations().GetNameList(true), GUILayout.Width(pw.mWidth));
				}
				
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold2 = EditorGUILayout.Foldout(fold2, "Audio settings");
			if(fold2)
			{
				if(this.tmpAudio == null && 
					"" != DataHolder.BaseAttack(selection).audioName)
				{
					this.tmpAudio = (AudioClip)Resources.Load(BaseAttackData.AUDIO_PATH+
							DataHolder.BaseAttack(selection).audioName, typeof(AudioClip));
				}
				this.tmpAudio = (AudioClip)EditorGUILayout.ObjectField("Attack audio", 
						this.tmpAudio, typeof(AudioClip), false, GUILayout.Width(pw.mWidth*2));
				if(this.tmpAudio)
				{
					DataHolder.BaseAttack(selection).audioName = this.tmpAudio.name;
				}
				else DataHolder.BaseAttack(selection).audioName = "";
				
				if(DataHolder.BaseAttack(selection).hasCritical)
				{
					EditorGUILayout.Separator();
					
					if(this.tmpAudio2 == null && "" != DataHolder.BaseAttack(selection).criticalAudioName)
					{
						this.tmpAudio2 = (AudioClip)Resources.Load(BaseAttackData.AUDIO_PATH+
								DataHolder.BaseAttack(selection).criticalAudioName, typeof(AudioClip));
					}
					this.tmpAudio2 = (AudioClip)EditorGUILayout.ObjectField("Critical audio", this.tmpAudio2, 
							typeof(AudioClip), false, GUILayout.Width(pw.mWidth*2));
					if(this.tmpAudio2) DataHolder.BaseAttack(selection).criticalAudioName = this.tmpAudio2.name;
					else DataHolder.BaseAttack(selection).criticalAudioName = "";
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold5 = EditorGUILayout.Foldout(fold5, "Consume item (character/weapon only)");
			if(fold5)
			{
				if(GUILayout.Button("Add item consume", GUILayout.Width(pw.mWidth2)))
				{
					DataHolder.BaseAttack(selection).AddItemConsume();
				}
				for(int i=0; i<DataHolder.BaseAttack(selection).consumeItemID.Length; i++)
				{
					EditorGUILayout.BeginHorizontal();
					if(GUILayout.Button("Remove", GUILayout.Width(pw.mWidth3)))
					{
						DataHolder.BaseAttack(selection).RemoveItemConsume(i);
						break;
					}
					DataHolder.BaseAttack(selection).consumeItemID[i] = EditorGUILayout.Popup("Item", 
							DataHolder.BaseAttack(selection).consumeItemID[i], DataHolder.Items().GetNameList(true), 
							GUILayout.Width(pw.mWidth));
					DataHolder.BaseAttack(selection).consumeItemQuantity[i] = EditorGUILayout.IntField("Quantity", 
							DataHolder.BaseAttack(selection).consumeItemQuantity[i], GUILayout.Width(pw.mWidth));
					GUILayout.FlexibleSpace();
					EditorGUILayout.EndHorizontal();
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold6 = EditorGUILayout.Foldout(fold6, "Steal chance");
			if(fold6)
			{
				EditorHelper.StealChanceSettings(ref DataHolder.BaseAttack(selection).stealChance);
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold3 = EditorGUILayout.Foldout(fold3, "Consume status value settings");
			if(fold3)
			{
				for(int i=0; i<DataHolder.BaseAttack(selection).consume.Length; i++)
				{
					if(DataHolder.StatusValue(i).IsConsumable())
					{
						DataHolder.BaseAttack(selection).consume[i] = EditorHelper.ValueChangeSettings(i, 
								DataHolder.BaseAttack(selection).consume[i]);
					}
					else
					{
						DataHolder.BaseAttack(selection).consume[i] = new ValueChange();
					}
				}
			}
			EditorGUILayout.EndVertical();
			
			if(DataHolder.BaseAttack(selection).hasCritical)
			{
				EditorGUILayout.BeginVertical("box");
				fold3 = EditorGUILayout.Foldout(fold3, "Critical consume status value settings");
				if(fold3)
				{
					for(int i=0; i<DataHolder.BaseAttack(selection).criticalConsume.Length; i++)
					{
						if(DataHolder.StatusValue(i).IsConsumable())
						{
							DataHolder.BaseAttack(selection).criticalConsume[i] = EditorHelper.ValueChangeSettings(i, 
									DataHolder.BaseAttack(selection).criticalConsume[i]);
						}
						else
						{
							DataHolder.BaseAttack(selection).criticalConsume[i] = new ValueChange();
						}
					}
				}
				EditorGUILayout.EndVertical();
			}
			
			EditorGUILayout.EndVertical();
		}
		this.EndTab();
	}
}