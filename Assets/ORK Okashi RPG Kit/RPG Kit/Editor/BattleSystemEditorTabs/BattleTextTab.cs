
using System.Collections;
using UnityEditor;
using UnityEngine;

public class BattleTextTab : EditorTab
{
	private BattleSystemWindow pw;
	
	public BattleTextTab(BattleSystemWindow pw) : base()
	{
		this.pw = pw;
	}
	
	public void ShowTab()
	{
		EditorGUILayout.BeginVertical();
		SP1 = EditorGUILayout.BeginScrollView(SP1);
		
		this.Separate();
		
		int count = DataHolder.Languages().GetDataCount();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical("box");
		fold6 = EditorGUILayout.Foldout(fold6, "Info text");
		if(fold6)
		{
			DataHolder.BattleSystemData().showInfo = EditorGUILayout.Toggle("Show infos", 
					DataHolder.BattleSystemData().showInfo, GUILayout.Width(pw.mWidth));
			
			if(DataHolder.BattleSystemData().showInfo)
			{
				DataHolder.BattleSystemData().infoPosition = EditorGUILayout.Popup("Position", 
						DataHolder.BattleSystemData().infoPosition, DataHolder.DialoguePositions().GetNameList(true), 
						GUILayout.Width(pw.mWidth*1.5f));
				DataHolder.BattleSystemData().infoShowTime = EditorGUILayout.FloatField("Visible time", 
						DataHolder.BattleSystemData().infoShowTime, GUILayout.Width(pw.mWidth));
				EditorGUILayout.Separator();
				
				GUILayout.Label("Show", EditorStyles.boldLabel);
				DataHolder.BattleSystemData().showSkills = EditorGUILayout.Toggle("Skills", 
						DataHolder.BattleSystemData().showSkills, GUILayout.Width(pw.mWidth));
				DataHolder.BattleSystemData().showItems = EditorGUILayout.Toggle("Items", 
						DataHolder.BattleSystemData().showItems, GUILayout.Width(pw.mWidth));
				DataHolder.BattleSystemData().showDefend = EditorGUILayout.Toggle("Defend", 
						DataHolder.BattleSystemData().showDefend, GUILayout.Width(pw.mWidth));
				DataHolder.BattleSystemData().showEscape = EditorGUILayout.Toggle("Escape", 
						DataHolder.BattleSystemData().showEscape, GUILayout.Width(pw.mWidth));
				DataHolder.BattleSystemData().showCounter = EditorGUILayout.Toggle("Counter attack", 
						DataHolder.BattleSystemData().showCounter, GUILayout.Width(pw.mWidth));
				DataHolder.BattleSystemData().showStealItem = EditorGUILayout.Toggle("Steal item", 
						DataHolder.BattleSystemData().showStealItem, GUILayout.Width(pw.mWidth));
				DataHolder.BattleSystemData().showStealItemFail = EditorGUILayout.Toggle("Steal item fail", 
						DataHolder.BattleSystemData().showStealItemFail, GUILayout.Width(pw.mWidth));
				DataHolder.BattleSystemData().showStealMoney = EditorGUILayout.Toggle("Steal money", 
						DataHolder.BattleSystemData().showStealMoney, GUILayout.Width(pw.mWidth));
				DataHolder.BattleSystemData().showStealMoneyFail = EditorGUILayout.Toggle("Steal money fail", 
						DataHolder.BattleSystemData().showStealMoneyFail, GUILayout.Width(pw.mWidth));
			}
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical("box");
		fold14 = EditorGUILayout.Foldout(fold14, "Battle messages");
		if(fold14)
		{
			DataHolder.BattleSystemData().showBattleMessage = EditorGUILayout.Toggle("Show messges", 
					DataHolder.BattleSystemData().showBattleMessage, GUILayout.Width(pw.mWidth));
			if(DataHolder.BattleSystemData().showInfo)
			{
				DataHolder.BattleSystemData().battleMessagePosition = EditorGUILayout.Popup("Position", 
						DataHolder.BattleSystemData().battleMessagePosition, DataHolder.DialoguePositions().GetNameList(true), 
						GUILayout.Width(pw.mWidth*1.5f));
				DataHolder.BattleSystemData().battleMessageShowTime = EditorGUILayout.FloatField("Visible time", DataHolder.BattleSystemData().battleMessageShowTime, 
						GUILayout.Width(pw.mWidth));
			}
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginVertical("box");
		fold7 = EditorGUILayout.Foldout(fold7, "Counter attack text");
		if(fold7)
		{
			for(int i=0; i<count; i++)
			{
				DataHolder.BattleSystemData().counterText[i] = EditorGUILayout.TextField(DataHolder.Languages().GetName(i), 
						DataHolder.BattleSystemData().counterText[i] as string, GUILayout.Width(pw.mWidth*1.5f));
			}
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical("box");
		fold8 = EditorGUILayout.Foldout(fold8, "All allies text");
		if(fold8)
		{
			for(int i=0; i<count; i++)
			{
				DataHolder.BattleSystemData().allAlliesText[i] = EditorGUILayout.TextField(DataHolder.Languages().GetName(i), 
						DataHolder.BattleSystemData().allAlliesText[i] as string, GUILayout.Width(pw.mWidth*1.5f));
			}
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		EditorGUILayout.Separator();
		EditorGUILayout.BeginVertical("box");
		fold9 = EditorGUILayout.Foldout(fold9, "All enemies text");
		if(fold9)
		{
			for(int i=0; i<count; i++)
			{
				DataHolder.BattleSystemData().allEnemiesText[i] = EditorGUILayout.TextField(DataHolder.Languages().GetName(i), 
						DataHolder.BattleSystemData().allEnemiesText[i] as string, GUILayout.Width(pw.mWidth*1.5f));
			}
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical("box");
		fold15 = EditorGUILayout.Foldout(fold15, "Steal item text");
		if(fold15)
		{
			GUILayout.Label("%n for user name, % for item name");
			for(int i=0; i<count; i++)
			{
				DataHolder.BattleSystemData().stealItemText[i] = EditorGUILayout.TextField(DataHolder.Languages().GetName(i), 
						DataHolder.BattleSystemData().stealItemText[i] as string, GUILayout.Width(pw.mWidth*1.5f));
			}
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		EditorGUILayout.Separator();
		EditorGUILayout.BeginVertical("box");
		fold20 = EditorGUILayout.Foldout(fold20, "Steal item fail text");
		if(fold20)
		{
			GUILayout.Label("%n for user name");
			for(int i=0; i<count; i++)
			{
				DataHolder.BattleSystemData().stealItemFailText[i] = EditorGUILayout.TextField(DataHolder.Languages().GetName(i), 
						DataHolder.BattleSystemData().stealItemFailText[i] as string, GUILayout.Width(pw.mWidth*1.5f));
			}
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical("box");
		fold5 = EditorGUILayout.Foldout(fold5, "Steal money text");
		if(fold5)
		{
			GUILayout.Label("%n for user name, % for money amount");
			for(int i=0; i<count; i++)
			{
				DataHolder.BattleSystemData().stealMoneyText[i] = EditorGUILayout.TextField(DataHolder.Languages().GetName(i), 
						DataHolder.BattleSystemData().stealMoneyText[i] as string, GUILayout.Width(pw.mWidth*1.5f));
			}
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		EditorGUILayout.Separator();
		EditorGUILayout.BeginVertical("box");
		fold16 = EditorGUILayout.Foldout(fold16, "Steal money fail text");
		if(fold16)
		{
			GUILayout.Label("%n for user name");
			for(int i=0; i<count; i++)
			{
				DataHolder.BattleSystemData().stealMoneyFailText[i] = EditorGUILayout.TextField(DataHolder.Languages().GetName(i), 
						DataHolder.BattleSystemData().stealMoneyFailText[i] as string, GUILayout.Width(pw.mWidth*1.5f));
			}
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical("box");
		fold10 = EditorGUILayout.Foldout(fold10, "Battle start text");
		if(fold10)
		{
			DataHolder.BattleSystemData().battleStartColor = EditorGUILayout.Popup("Color", DataHolder.BattleSystemData().battleStartColor, 
					DataHolder.Colors().GetNameList(true), GUILayout.Width(pw.mWidth));
			DataHolder.BattleSystemData().battleStartSColor = EditorGUILayout.Popup("Shadow color", DataHolder.BattleSystemData().battleStartSColor, 
					DataHolder.Colors().GetNameList(true), GUILayout.Width(pw.mWidth));
			for(int i=0; i<count; i++)
			{
				DataHolder.BattleSystemData().battleStartText[i] = EditorGUILayout.TextField(DataHolder.Languages().GetName(i), 
						DataHolder.BattleSystemData().battleStartText[i] as string, GUILayout.Width(pw.mWidth*1.5f));
			}
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		EditorGUILayout.Separator();
		EditorGUILayout.BeginVertical("box");
		fold11 = EditorGUILayout.Foldout(fold11, "Battle victory text");
		if(fold11)
		{
			DataHolder.BattleSystemData().battleVictoryColor = EditorGUILayout.Popup("Color", DataHolder.BattleSystemData().battleVictoryColor, 
					DataHolder.Colors().GetNameList(true), GUILayout.Width(pw.mWidth));
			DataHolder.BattleSystemData().battleVictorySColor = EditorGUILayout.Popup("Shadow color", DataHolder.BattleSystemData().battleVictorySColor, 
					DataHolder.Colors().GetNameList(true), GUILayout.Width(pw.mWidth));
			for(int i=0; i<count; i++)
			{
				DataHolder.BattleSystemData().battleVictoryText[i] = EditorGUILayout.TextField(DataHolder.Languages().GetName(i), 
						DataHolder.BattleSystemData().battleVictoryText[i] as string, GUILayout.Width(pw.mWidth*1.5f));
			}
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		
		
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical("box");
		fold12 = EditorGUILayout.Foldout(fold12, "Battle defeat text");
		if(fold12)
		{
			DataHolder.BattleSystemData().battleDefeatColor = EditorGUILayout.Popup("Color", DataHolder.BattleSystemData().battleDefeatColor, 
					DataHolder.Colors().GetNameList(true), GUILayout.Width(pw.mWidth));
			DataHolder.BattleSystemData().battleDefeatSColor = EditorGUILayout.Popup("Shadow color", DataHolder.BattleSystemData().battleDefeatSColor, 
					DataHolder.Colors().GetNameList(true), GUILayout.Width(pw.mWidth));
			for(int i=0; i<count; i++)
			{
				DataHolder.BattleSystemData().battleDefeatText[i] = EditorGUILayout.TextField(DataHolder.Languages().GetName(i), 
						DataHolder.BattleSystemData().battleDefeatText[i] as string, GUILayout.Width(pw.mWidth*1.5f));
			}
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		EditorGUILayout.Separator();
		EditorGUILayout.BeginVertical("box");
		fold13 = EditorGUILayout.Foldout(fold13, "Battle escape text");
		if(fold13)
		{
			DataHolder.BattleSystemData().battleEscapeColor = EditorGUILayout.Popup("Color", DataHolder.BattleSystemData().battleEscapeColor, 
					DataHolder.Colors().GetNameList(true), GUILayout.Width(pw.mWidth));
			DataHolder.BattleSystemData().battleEscapeSColor = EditorGUILayout.Popup("Shadow color", DataHolder.BattleSystemData().battleEscapeSColor, 
					DataHolder.Colors().GetNameList(true), GUILayout.Width(pw.mWidth));
			for(int i=0; i<count; i++)
			{
				DataHolder.BattleSystemData().battleEscapeText[i] = EditorGUILayout.TextField(DataHolder.Languages().GetName(i), 
						DataHolder.BattleSystemData().battleEscapeText[i] as string, GUILayout.Width(pw.mWidth*1.5f));
			}
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		
		this.Separate();
		DataHolder.BattleSystemData().showUserDamage = EditorGUILayout.Toggle("Show user dmg", 
				DataHolder.BattleSystemData().showUserDamage, GUILayout.Width(pw.mWidth));
		
		if(DataHolder.BattleSystemData().textSkin == null &&
			DataHolder.BattleSystemData().textSkinName != null &&
			"" != DataHolder.BattleSystemData().textSkinName)
		{
			DataHolder.BattleSystemData().textSkin = (GUISkin)Resources.Load(DataHolder.BattleSystemData().skinPath+
					DataHolder.BattleSystemData().textSkinName, typeof(GUISkin));
		}
		DataHolder.BattleSystemData().textSkin = (GUISkin)EditorGUILayout.ObjectField("GUISkin", DataHolder.BattleSystemData().textSkin, typeof(GUISkin), false, GUILayout.Width(pw.mWidth*1.5f));
		if(DataHolder.BattleSystemData().textSkin)
		{
			DataHolder.BattleSystemData().textSkinName = DataHolder.BattleSystemData().textSkin.name;
		}
		else DataHolder.BattleSystemData().textSkinName = "";
		DataHolder.BattleSystemData().mountTexts = EditorGUILayout.Toggle("Mount texts",
				DataHolder.BattleSystemData().mountTexts, GUILayout.Width(pw.mWidth));
		
		EditorGUILayout.BeginVertical("box");
		fold3 = EditorGUILayout.Foldout(fold3, "Effect text");
		if(fold3)
		{
			EditorHelper.BattleTextSettings(ref DataHolder.BattleSystemData().effectTextSettings, "% for effect name", false);
		}
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical("box");
		fold4 = EditorGUILayout.Foldout(fold4, "Miss text");
		if(fold4)
		{
			EditorHelper.BattleTextSettings(ref DataHolder.BattleSystemData().missTextSettings, "", false);
		}
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical("box");
		fold19 = EditorGUILayout.Foldout(fold19, "Block text");
		if(fold19)
		{
			EditorHelper.BattleTextSettings(ref DataHolder.BattleSystemData().blockTextSettings, "", false);
		}
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical("box");
		fold17 = EditorGUILayout.Foldout(fold17, "Cast cancel text");
		if(fold17)
		{
			EditorHelper.BattleTextSettings(ref DataHolder.BattleSystemData().castCancelTextSettings, "% for skill name", false);
		}
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical("box");
		fold18 = EditorGUILayout.Foldout(fold18, "Level up text");
		if(fold18)
		{
			EditorHelper.BattleTextSettings(ref DataHolder.BattleSystemData().levelUpTextSettings, "% for level number", false);
		}
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical("box");
		fold19 = EditorGUILayout.Foldout(fold19, "Class level up text");
		if(fold19)
		{
			EditorHelper.BattleTextSettings(ref DataHolder.BattleSystemData().classLevelUpTextSettings, "% for level number", false);
		}
		EditorGUILayout.EndVertical();
		
		this.Separate();
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}
}