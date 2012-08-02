
using UnityEditor;
using UnityEngine;

public class SkillTab : BaseTab
{
	private int tmpSel = 0;
	private int levelSelection = 0;
	private string[] sections = new string[] {"1"};
	private AudioClip tmpAudio = null;
	
	public SkillTab(ProjectWindow pw) : base(pw)
	{
		this.Reload();
	}
	
	new public void Reload()
	{
		base.Reload();
		this.levelSelection = 0;
		this.tmpAudio = null;
	}
	
	public void ShowTab()
	{
		tmpSel = selection;
		EditorGUILayout.BeginVertical();
		
		// buttons
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Add Skill", GUILayout.Width(pw.mWidth2)))
		{
			DataHolder.Skills().filter.Deactivate();
			DataHolder.Skills().AddSkill("New Skill", "New Description", pw.GetLangCount());
			selection = DataHolder.Skills().GetDataCount()-1;
			pw.AddSkill(selection);
			GUI.FocusControl ("ID");
		}
		if(selection >= 0)
		{
			if(this.ShowCopyButton(DataHolder.Skills()))
			{
				DataHolder.Skills().filter.Deactivate();
				pw.AddSkill(selection);
			}
		}
		if(DataHolder.Skills().GetDataCount() > 1 && selection >= 0)
		{
			if(this.ShowRemButton("Remove Skill", DataHolder.Skills()))
			{
				DataHolder.Skills().filter.Deactivate();
				pw.RemoveSkill(selection);
			}
		}
		this.CheckSelection(DataHolder.Skills());
		EditorGUILayout.EndHorizontal();
		
		// elements list
		this.AddItemListFilter(DataHolder.Skills(), "type", DataHolder.SkillTypes().GetNameList(true));
		
		if(tmpSel != selection) levelSelection = 0;
		
		// element settings
		EditorGUILayout.BeginVertical();
		SP2 = EditorGUILayout.BeginScrollView(SP2);
		if(DataHolder.Skills().GetDataCount() > 0 && selection >= 0)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			this.AddID("Skill ID");
			this.AddMultiLangIcon("Skill Name", DataHolder.Skills());
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold10 = EditorGUILayout.Foldout(fold10, "Base Settings");
			if(fold10)
			{
				DataHolder.Skill(selection).isPassive = EditorGUILayout.Toggle("Passive skill", 
						DataHolder.Skill(selection).isPassive, GUILayout.Width(pw.mWidth));
				
				if(DataHolder.Skill(selection).isPassive)
				{
					DataHolder.Skill(selection).useInBattle = false;
					DataHolder.Skill(selection).useInField = false;
					DataHolder.Skill(selection).skilltype = EditorGUILayout.Popup("Skill type", 
							DataHolder.Skill(selection).skilltype, pw.GetSkillTypes(), GUILayout.Width(pw.mWidth));
				}
				else
				{
					DataHolder.Skill(selection).useInBattle = EditorGUILayout.Toggle("Usable in battle", 
							DataHolder.Skill(selection).useInBattle, GUILayout.Width(pw.mWidth));
					DataHolder.Skill(selection).useInField = EditorGUILayout.Toggle("Usable in field", 
							DataHolder.Skill(selection).useInField, GUILayout.Width(pw.mWidth));
					DataHolder.Skill(selection).skilltype = EditorGUILayout.Popup("Skill type", 
							DataHolder.Skill(selection).skilltype, pw.GetSkillTypes(), GUILayout.Width(pw.mWidth));
				}
				
				// update filter on change
				if(DataHolder.Skills().filter.useFilter[0] && 
					DataHolder.Skill(selection).skilltype != DataHolder.Skills().filter.filterID[0])
				{
					DataHolder.Skills().filter.filterID[0] = DataHolder.Skill(selection).skilltype;
					DataHolder.Skills().CreateFilterList(true);
				}
				
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			if(!DataHolder.Skill(selection).isPassive)
			{
				EditorGUILayout.BeginVertical("box");
				fold14 = EditorGUILayout.Foldout(fold14, "Target Settings");
				if(fold14)
				{
					DataHolder.Skill(selection).targetType = (TargetType)this.EnumToolbar("Target type", 
							(int)DataHolder.Skill(selection).targetType, typeof(TargetType));
					if(!DataHolder.Skill(selection).TargetSelf())
					{
						DataHolder.Skill(selection).skillTarget = (SkillTarget)this.EnumToolbar("Skill target", 
								(int)DataHolder.Skill(selection).skillTarget, typeof(SkillTarget));
					}
					else DataHolder.Skill(selection).skillTarget = SkillTarget.SINGLE;
					if(DataHolder.Skill(selection).TargetNone())
					{
						DataHolder.Skill(selection).targetRaycast = EditorHelper.TargetRaycastSettings(
							DataHolder.Skill(selection).targetRaycast);
					}
					else DataHolder.Skill(selection).targetRaycast.active = false;
					
					this.Separate();
				}
				EditorGUILayout.EndVertical();
			}
			
			EditorGUILayout.BeginVertical("box");
			fold9 = EditorGUILayout.Foldout(fold9, "Level Settings");
			if(fold9)
			{
				EditorGUILayout.BeginHorizontal();
				if(GUILayout.Button("Add level", GUILayout.Width(pw.mWidth3)))
				{
					DataHolder.Skill(selection).AddLevel();
					levelSelection = DataHolder.Skills().skill[selection].level.Length-1;
				}
				if(GUILayout.Button("Copy level", GUILayout.Width(pw.mWidth3)))
				{
					DataHolder.Skill(selection).CopyLevel(levelSelection);
					levelSelection = DataHolder.Skills().skill[selection].level.Length-1;
				}
				if(DataHolder.Skill(selection).level.Length > 1)
				{
					if(GUILayout.Button("Remove level", GUILayout.Width(pw.mWidth3)))
					{
						DataHolder.Skill(selection).RemoveLevel(levelSelection);
					}
				}
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				if(sections.Length != DataHolder.Skill(selection).level.Length)
				{
					sections = new string[DataHolder.Skill(selection).level.Length];
					for(int i=0; i<sections.Length; i++) sections[i] = (i+1).ToString();
				}
				levelSelection = GUILayout.SelectionGrid(levelSelection, sections, 15);
			}
			EditorGUILayout.EndVertical();
			
			if(levelSelection >= DataHolder.Skills().skill[selection].level.Length)
			{
				levelSelection = DataHolder.Skills().skill[selection].level.Length-1;
			}
			
			DataHolder.Skill(selection).level[levelSelection] = this.ShowSkillLevel(
					DataHolder.Skill(selection).level[levelSelection], DataHolder.Skill(selection));
			
			EditorGUILayout.EndHorizontal();
		}
		this.EndTab();
	}
	
	private SkillLevel ShowSkillLevel(SkillLevel level, Skill skill)
	{
		if(skill.isPassive)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical("box");
			fold8 = EditorGUILayout.Foldout(fold8, "Element effectiveness");
			if(fold8)
			{
				for(int i=0; i<pw.GetElementCount(); i++)
				{
					EditorGUILayout.BeginHorizontal();
					level.elementValue[i] = EditorGUILayout.IntField(pw.GetElement(i), 
							level.elementValue[i], GUILayout.Width(pw.mWidth));
					level.elementOperator[i] = (SimpleOperator)this.EnumToolbar("", 
							(int)level.elementOperator[i], typeof(SimpleOperator));
					EditorGUILayout.EndHorizontal();
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold11 = EditorGUILayout.Foldout(fold11, "Race damage factor changes");
			if(fold11)
			{
				for(int i=0; i<level.raceValue.Length; i++)
				{
					level.raceValue[i] = EditorGUILayout.IntField(DataHolder.Race(i), 
							level.raceValue[i], GUILayout.Width(pw.mWidth));
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold13 = EditorGUILayout.Foldout(fold13, "Size damage factor changes");
			if(fold13)
			{
				for(int i=0; i<level.sizeValue.Length; i++)
				{
					level.sizeValue[i] = EditorGUILayout.IntField(DataHolder.Size(i), 
							level.sizeValue[i], GUILayout.Width(pw.mWidth));
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
				EditorHelper.BonusSettings(ref level.bonus, false);
				this.Separate();
			}
			EditorGUILayout.EndVertical();
		}
		else
		{
			EditorGUILayout.BeginVertical("box");
			fold2 = EditorGUILayout.Foldout(fold2, "Skill Settings");
			if(fold2)
			{
				EditorGUILayout.Separator();
				
				if(selection != tmpSel) this.tmpAudio = null;
				if(this.tmpAudio == null && "" != level.audioName)
				{
					this.tmpAudio = (AudioClip)Resources.Load(SkillData.AUDIO_PATH+
							level.audioName, typeof(AudioClip));
				}
				this.tmpAudio = (AudioClip)EditorGUILayout.ObjectField("Use audio", this.tmpAudio, 
						typeof(AudioClip), false, GUILayout.Width(pw.mWidth*2));
				if(this.tmpAudio) level.audioName = this.tmpAudio.name;
				else level.audioName = "";
				EditorGUILayout.Separator();
				
				level.skillElement = EditorGUILayout.Popup("Skill element", 
						level.skillElement, pw.GetElements(), GUILayout.Width(pw.mWidth));
				level.counterable = EditorGUILayout.Toggle("Counterable", 
						level.counterable, GUILayout.Width(pw.mWidth));
				level.reflectable = EditorGUILayout.Toggle("Reflectable", 
						level.reflectable, GUILayout.Width(pw.mWidth));
				level.revive = EditorGUILayout.Toggle("Revive target", 
						level.revive, GUILayout.Width(pw.mWidth));
				
				if(DataHolder.BattleSystem().CanUseSkillCasting())
				{
					EditorGUILayout.Separator();
					if(skill.useInBattle)
					{
						EditorGUILayout.BeginHorizontal();
						level.castTime = EditorGUILayout.FloatField("Cast time", 
								level.castTime, GUILayout.Width(pw.mWidth));
						if(level.castTime > 0)
						{
							level.cancelable = EditorGUILayout.Toggle("Cancelable", 
									level.cancelable, GUILayout.Width(pw.mWidth));
						}
						if(level.castTime < 0) level.castTime = 0;
						GUILayout.FlexibleSpace();
						EditorGUILayout.EndHorizontal();
					}
					else level.castTime = 0;
					
					if(DataHolder.BattleSystem().IsActiveTime())
					{
						level.endTurn = EditorGUILayout.Toggle("End turn", 
								level.endTurn, GUILayout.Width(pw.mWidth));
						if(!level.endTurn)
						{
							level.timebarUse = EditorGUILayout.FloatField("Timebar use", 
									level.timebarUse, GUILayout.Width(pw.mWidth));
							if(level.timebarUse <= 0) level.timebarUse = 1;
						}
					}
				}
				else level.castTime = 0;
				
				EditorGUILayout.Separator();
				EditorGUILayout.BeginHorizontal();
				level.skillReuse = (StatusEffectEnd)this.EnumToolbar("Reuse after", 
						(int)level.skillReuse, typeof(StatusEffectEnd));
				if(!StatusEffectEnd.NONE.Equals(level.skillReuse))
				{
					level.reuseTime = EditorGUILayout.FloatField(
							level.reuseTime, GUILayout.Width(pw.mWidth*0.5f));
				}
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.Separator();
				EditorGUILayout.BeginHorizontal(GUILayout.Width(pw.mWidth*1.5f));
				level.hitChance = EditorGUILayout.BeginToggleGroup("Calculate hit chance", level.hitChance);
				level.hitFormula = EditorGUILayout.Popup(level.hitFormula, pw.GetFormulas());
				EditorGUILayout.EndToggleGroup();
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.Separator();
				EditorGUILayout.BeginHorizontal(GUILayout.Width(pw.mWidth*1.5f));
				level.battleAnimation = EditorGUILayout.BeginToggleGroup("Use battle animation", level.battleAnimation);
				level.animationID = EditorGUILayout.Popup(level.animationID, DataHolder.BattleAnimations().GetNameList(true));
				EditorGUILayout.EndToggleGroup();
				EditorGUILayout.EndHorizontal();
				
				if(!DataHolder.Skill(selection).TargetNone() && 
					!DataHolder.Skill(selection).TargetSelf())
				{
					level.useRange = EditorHelper.UseRangeSettings(level.useRange);
				}
				else level.useRange.active = false;
				
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold12 = EditorGUILayout.Foldout(fold12, "Steal chance");
			if(fold12)
			{
				EditorHelper.StealChanceSettings(ref level.stealChance);
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold3 = EditorGUILayout.Foldout(fold3, "User Settings");
			if(fold3)
			{
				GUILayout.Label ("Consume Status Value", EditorStyles.boldLabel);
				for(int i=0; i<pw.GetStatusValueCount(); i++)
				{
					if(pw.IsStatusValueConsumable(i))
					{
						level.userConsume[i] = EditorHelper.ValueChangeSettings(i, 
								level.userConsume[i]);
					}
					else
					{
						level.userConsume[i] = new ValueChange();
					}
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold4 = EditorGUILayout.Foldout(fold4, "Target Settings");
			if(fold4)
			{
				if(DataHolder.BattleSystem().IsTurnBased() && 
					!DataHolder.BattleSystem().dynamicCombat)
				{
					level.orderChange = EditorGUILayout.IntField("Order change", 
							level.orderChange, GUILayout.Width(pw.mWidth));
				}
				EditorGUILayout.Separator();
				
				GUILayout.Label ("Consume Status Value", EditorStyles.boldLabel);
				for(int i=0; i<pw.GetStatusValueCount(); i++)
				{
					if(pw.IsStatusValueConsumable(i))
					{
						level.targetConsume[i] = EditorHelper.ValueChangeSettings(i, 
								level.targetConsume[i]);
					}
					else
					{
						level.targetConsume[i] = new ValueChange();
					}
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical();
		}
		
		EditorGUILayout.BeginVertical("box");
		fold5 = EditorGUILayout.Foldout(fold5, "Status Effects");
		if(fold5)
		{
			for(int i=0; i<pw.GetStatusEffectCount(); i++)
			{
				level.skillEffect[i] = (SkillEffect)this.EnumToolbar(pw.GetStatusEffect(i), 
						(int)level.skillEffect[i], typeof(SkillEffect));
			}
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		
		if(!skill.isPassive)
		{
			EditorGUILayout.BeginVertical("box");
			fold6 = EditorGUILayout.Foldout(fold6, "Useable after (skill combo)");
			if(fold6)
			{
				if(GUILayout.Button("Add", GUILayout.Width(pw.mWidth2)))
				{
					level.AddSkillCombo();
				}
				for(int i=0; i<level.skillCombo.Length; i++)
				{
					EditorGUILayout.BeginHorizontal();
					if(GUILayout.Button("Remove", GUILayout.Width(pw.mWidth3)))
					{
						level.RemoveSkillCombo(i);
						break;
					}
					level.skillCombo[i] = EditorGUILayout.Popup(level.skillCombo[i], 
							DataHolder.Skills().GetNameList(true), GUILayout.Width(pw.mWidth));
					EditorGUILayout.EndHorizontal();
				}
			}
			EditorGUILayout.EndVertical();
		}
		EditorGUILayout.EndVertical();
		return level;
	}
}