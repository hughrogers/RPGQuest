
using System.Collections;
using UnityEditor;
using UnityEngine;

public class BattleMenuTab : EditorTab
{
	private BattleSystemWindow pw;
	
	private GameObject tmpPrefab;
	
	public BattleMenuTab(BattleSystemWindow pw) : base()
	{
		this.pw = pw;
	}
	
	public void ShowTab()
	{
		EditorGUILayout.BeginVertical();
		SP1 = EditorGUILayout.BeginScrollView(SP1);
		this.Separate();
		
		EditorGUILayout.BeginVertical("box");
		fold7 = EditorGUILayout.Foldout(fold7, "General settings");
		if(fold7)
		{
			DataHolder.BattleMenu().dialoguePosition = EditorGUILayout.Popup("Menu position", 
					DataHolder.BattleMenu().dialoguePosition, DataHolder.DialoguePositions().GetNameList(true),
					GUILayout.Width(pw.mWidth));
			
			this.Separate();
			DataHolder.BattleMenu().enableDrag = EditorGUILayout.Toggle("Enable drag",
					DataHolder.BattleMenu().enableDrag, GUILayout.Width(pw.mWidth));
			DataHolder.BattleMenu().enableDoubleClick = EditorGUILayout.Toggle("Double click use",
					DataHolder.BattleMenu().enableDoubleClick, GUILayout.Width(pw.mWidth));
			
			this.Separate();
			DataHolder.BattleMenu().addBack = EditorGUILayout.Toggle("Add back button",
					DataHolder.BattleMenu().addBack, GUILayout.Width(pw.mWidth));
			if(DataHolder.BattleMenu().addBack)
			{
				DataHolder.BattleMenu().backFirst = EditorGUILayout.Toggle("First element",
						DataHolder.BattleMenu().backFirst, GUILayout.Width(pw.mWidth));
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical();
				GUILayout.Label("Name", EditorStyles.boldLabel);
				this.SetNames(DataHolder.BattleMenu().backName);
				EditorGUILayout.EndVertical();
				EditorGUILayout.BeginVertical();
				if(DataHolder.BattleMenu().backIcon == null &&
					DataHolder.BattleMenu().backIconName != null &&
					"" != DataHolder.BattleMenu().backIconName)
				{
					DataHolder.BattleMenu().backIcon = (Texture2D)Resources.Load(
							BattleSystemData.ICON_PATH+
							DataHolder.BattleMenu().backIconName, typeof(Texture2D));
				}
				DataHolder.BattleMenu().backIcon = (Texture2D)EditorGUILayout.ObjectField("Icon", DataHolder.BattleMenu().backIcon, typeof(Texture2D), false);
				if(DataHolder.BattleMenu().backIcon)
				{
					DataHolder.BattleMenu().backIconName = DataHolder.BattleMenu().backIcon.name;
				}
				else DataHolder.BattleMenu().backIconName = "";
				EditorGUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
			}
			
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		
		for(int i=0; i<DataHolder.BattleMenu().order.Length; i++)
		{
			EditorGUILayout.BeginVertical("box");
			if(DataHolder.BattleMenu().order[i] == BattleMenu.ATTACK)
			{
				this.ShowAttack(i);
			}
			else if(DataHolder.BattleMenu().order[i] == BattleMenu.SKILL)
			{
				this.ShowSkill(i);
			}
			else if(DataHolder.BattleMenu().order[i] == BattleMenu.ITEM)
			{
				this.ShowItem(i);
			}
			else if(DataHolder.BattleMenu().order[i] == BattleMenu.DEFEND)
			{
				this.ShowDefend(i);
			}
			else if(DataHolder.BattleMenu().order[i] == BattleMenu.ESCAPE)
			{
				this.ShowEscape(i);
			}
			else if(DataHolder.BattleMenu().order[i] == BattleMenu.ENDTURN)
			{
				this.ShowEndTurn(i);
			}
			EditorGUILayout.EndVertical();
		}
		
		EditorGUILayout.BeginVertical("box");
		this.ShowTarget();
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}
	
	private void CheckLanguages(ArrayList names, string name)
	{
		if(names.Count < DataHolder.Languages().GetDataCount())
		{
			int count = DataHolder.Languages().GetDataCount() - names.Count;
			for(int i=0; i<count; i++)
			{
				names.Add(name);
			}
		}
	}
	
	private void SetNames(ArrayList names)
	{
		for(int i=0; i<names.Count; i++)
		{
			names[i] = EditorGUILayout.TextField(DataHolder.Language(i), names[i] as string, GUILayout.Width(pw.mWidth));
		}
	}
	
	private void UpDown(int index)
	{
		EditorGUILayout.BeginHorizontal();
		if(index > 0)
		{
			if(GUILayout.Button("Move up", GUILayout.Width(100)))
			{
				DataHolder.BattleMenu().MoveUp(index);
			}
		}
		if(index < DataHolder.BattleMenu().order.Length-1)
		{
			if(GUILayout.Button("Move down", GUILayout.Width(100)))
			{
				DataHolder.BattleMenu().MoveDown(index);
			}
		}
		EditorGUILayout.EndHorizontal();
	}
	
	private void ShowAttack(int index)
	{
		this.CheckLanguages(DataHolder.BattleMenu().attackName, BattleMenu.ATTACK);
		
		fold1 = EditorGUILayout.Foldout(fold1, "Attack");
		if(fold1)
		{
			this.UpDown(index);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			DataHolder.BattleMenu().showAttack = EditorGUILayout.Toggle("Enable attack", DataHolder.BattleMenu().showAttack, GUILayout.Width(pw.mWidth));
			if(DataHolder.BattleMenu().showAttack)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical();
				GUILayout.Label("Name", EditorStyles.boldLabel);
				this.SetNames(DataHolder.BattleMenu().attackName);
				EditorGUILayout.EndVertical();
				EditorGUILayout.BeginVertical();
				if(DataHolder.BattleMenu().attackIcon == null &&
					DataHolder.BattleMenu().attackIconName != null &&
					"" != DataHolder.BattleMenu().attackIconName)
				{
					DataHolder.BattleMenu().attackIcon = (Texture2D)Resources.Load(BattleSystemData.ICON_PATH+
							DataHolder.BattleMenu().attackIconName, typeof(Texture2D));
				}
				DataHolder.BattleMenu().attackIcon = (Texture2D)EditorGUILayout.ObjectField("Icon", DataHolder.BattleMenu().attackIcon, typeof(Texture2D), false);
				if(DataHolder.BattleMenu().attackIcon)
				{
					DataHolder.BattleMenu().attackIconName = DataHolder.BattleMenu().attackIcon.name;
				}
				else DataHolder.BattleMenu().attackIconName = "";
				EditorGUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
		}
	}
	
	private void ShowSkill(int index)
	{
		this.CheckLanguages(DataHolder.BattleMenu().skillName, BattleMenu.SKILL);
		
		fold2 = EditorGUILayout.Foldout(fold2, "Skill");
		if(fold2)
		{
			this.UpDown(index);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			DataHolder.BattleMenu().showSkills = EditorGUILayout.Toggle("Enable skills", DataHolder.BattleMenu().showSkills, GUILayout.Width(pw.mWidth));
			if(DataHolder.BattleMenu().showSkills)
			{
				DataHolder.BattleMenu().skillPosition = EditorGUILayout.Popup("Menu position", DataHolder.BattleMenu().skillPosition, 
							DataHolder.DialoguePositions().GetNameList(true), GUILayout.Width(pw.mWidth));
				DataHolder.BattleMenu().combineSkills = EditorGUILayout.Toggle("Combine types", 
						DataHolder.BattleMenu().combineSkills, GUILayout.Width(pw.mWidth));
				if(DataHolder.BattleMenu().combineSkills)
				{
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.BeginVertical();
					GUILayout.Label("Name", EditorStyles.boldLabel);
					this.SetNames(DataHolder.BattleMenu().skillName);
					EditorGUILayout.EndVertical();
					EditorGUILayout.BeginVertical();
					if(DataHolder.BattleMenu().skillIcon == null &&
						DataHolder.BattleMenu().skillIconName != null &&
						"" != DataHolder.BattleMenu().skillIconName)
					{
						DataHolder.BattleMenu().skillIcon = (Texture2D)Resources.Load(BattleSystemData.ICON_PATH+
								DataHolder.BattleMenu().skillIconName, typeof(Texture2D));
					}
					DataHolder.BattleMenu().skillIcon = (Texture2D)EditorGUILayout.ObjectField("Icon", DataHolder.BattleMenu().skillIcon, typeof(Texture2D), false);
					if(DataHolder.BattleMenu().skillIcon)
					{
						DataHolder.BattleMenu().skillIconName = DataHolder.BattleMenu().skillIcon.name;
					}
					else DataHolder.BattleMenu().skillIconName = "";
					EditorGUILayout.EndVertical();
					GUILayout.FlexibleSpace();
					EditorGUILayout.EndHorizontal();
				}
			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
		}
	}
	
	private void ShowItem(int index)
	{
		this.CheckLanguages(DataHolder.BattleMenu().itemName, BattleMenu.ITEM);
		
		fold3 = EditorGUILayout.Foldout(fold3, "Item");
		if(fold3)
		{
			this.UpDown(index);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			DataHolder.BattleMenu().showItems = EditorGUILayout.Toggle("Enable items", DataHolder.BattleMenu().showItems, GUILayout.Width(pw.mWidth));
			if(DataHolder.BattleMenu().showItems)
			{
				DataHolder.BattleMenu().itemPosition = EditorGUILayout.Popup("Menu position", DataHolder.BattleMenu().itemPosition, 
							DataHolder.DialoguePositions().GetNameList(true), GUILayout.Width(pw.mWidth));
				DataHolder.BattleMenu().combineItems = EditorGUILayout.Toggle("Combine types", 
						DataHolder.BattleMenu().combineItems, GUILayout.Width(pw.mWidth));
				if(DataHolder.BattleMenu().combineItems)
				{
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.BeginVertical();
					GUILayout.Label("Name", EditorStyles.boldLabel);
					this.SetNames(DataHolder.BattleMenu().itemName);
					EditorGUILayout.EndVertical();
					EditorGUILayout.BeginVertical();
					if(DataHolder.BattleMenu().itemIcon == null &&
						DataHolder.BattleMenu().itemIconName != null &&
						"" != DataHolder.BattleMenu().itemIconName)
					{
						DataHolder.BattleMenu().itemIcon = (Texture2D)Resources.Load(BattleSystemData.ICON_PATH+
								DataHolder.BattleMenu().itemIconName, typeof(Texture2D));
					}
					DataHolder.BattleMenu().itemIcon = (Texture2D)EditorGUILayout.ObjectField("Icon", DataHolder.BattleMenu().itemIcon, typeof(Texture2D), false);
					if(DataHolder.BattleMenu().itemIcon)
					{
						DataHolder.BattleMenu().itemIconName = DataHolder.BattleMenu().itemIcon.name;
					}
					else DataHolder.BattleMenu().itemIconName = "";
					EditorGUILayout.EndVertical();
					GUILayout.FlexibleSpace();
					EditorGUILayout.EndHorizontal();
				}
			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
		}
	}
	
	private void ShowDefend(int index)
	{
		this.CheckLanguages(DataHolder.BattleMenu().defendName, BattleMenu.DEFEND);
		
		fold4 = EditorGUILayout.Foldout(fold4, "Defend");
		if(fold4)
		{
			this.UpDown(index);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			DataHolder.BattleMenu().showDefend = EditorGUILayout.Toggle("Enable defend", DataHolder.BattleMenu().showDefend, GUILayout.Width(pw.mWidth));
			if(DataHolder.BattleMenu().showDefend)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical();
				GUILayout.Label("Name", EditorStyles.boldLabel);
				this.SetNames(DataHolder.BattleMenu().defendName);
				EditorGUILayout.EndVertical();
				EditorGUILayout.BeginVertical();
				if(DataHolder.BattleMenu().defendIcon == null &&
					DataHolder.BattleMenu().defendIconName != null &&
					"" != DataHolder.BattleMenu().defendIconName)
				{
					DataHolder.BattleMenu().defendIcon = (Texture2D)Resources.Load(BattleSystemData.ICON_PATH+
							DataHolder.BattleMenu().defendIconName, typeof(Texture2D));
				}
				DataHolder.BattleMenu().defendIcon = (Texture2D)EditorGUILayout.ObjectField("Icon", DataHolder.BattleMenu().defendIcon, typeof(Texture2D), false);
				if(DataHolder.BattleMenu().defendIcon)
				{
					DataHolder.BattleMenu().defendIconName = DataHolder.BattleMenu().defendIcon.name;
				}
				else DataHolder.BattleMenu().defendIconName = "";
				EditorGUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
		}
	}
	
	private void ShowEscape(int index)
	{
		this.CheckLanguages(DataHolder.BattleMenu().escapeName, BattleMenu.ESCAPE);
		
		fold5 = EditorGUILayout.Foldout(fold5, "Escape");
		if(fold5)
		{
			this.UpDown(index);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			DataHolder.BattleMenu().showEscape = EditorGUILayout.Toggle("Enable escape", DataHolder.BattleMenu().showEscape, GUILayout.Width(pw.mWidth));
			if(DataHolder.BattleMenu().showEscape)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical();
				GUILayout.Label("Name", EditorStyles.boldLabel);
				this.SetNames(DataHolder.BattleMenu().escapeName);
				EditorGUILayout.EndVertical();
				EditorGUILayout.BeginVertical();
				if(DataHolder.BattleMenu().escapeIcon == null &&
					DataHolder.BattleMenu().escapeIconName != null &&
					"" != DataHolder.BattleMenu().escapeIconName)
				{
					DataHolder.BattleMenu().escapeIcon = (Texture2D)Resources.Load(BattleSystemData.ICON_PATH+
							DataHolder.BattleMenu().escapeIconName, typeof(Texture2D));
				}
				DataHolder.BattleMenu().escapeIcon = (Texture2D)EditorGUILayout.ObjectField("Icon", DataHolder.BattleMenu().escapeIcon, typeof(Texture2D), false);
				if(DataHolder.BattleMenu().escapeIcon)
				{
					DataHolder.BattleMenu().escapeIconName = DataHolder.BattleMenu().escapeIcon.name;
				}
				else DataHolder.BattleMenu().escapeIconName = "";
				EditorGUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
		}
	}
	
	private void ShowEndTurn(int index)
	{
		this.CheckLanguages(DataHolder.BattleMenu().endTurnName, BattleMenu.ENDTURN);
		
		fold5 = EditorGUILayout.Foldout(fold5, "End turn");
		if(fold5)
		{
			this.UpDown(index);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			DataHolder.BattleMenu().showEndTurn = EditorGUILayout.Toggle("Enable end turn", 
					DataHolder.BattleMenu().showEndTurn, GUILayout.Width(pw.mWidth));
			if(DataHolder.BattleMenu().showEndTurn)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical();
				GUILayout.Label("Name", EditorStyles.boldLabel);
				this.SetNames(DataHolder.BattleMenu().endTurnName);
				EditorGUILayout.EndVertical();
				EditorGUILayout.BeginVertical();
				if(DataHolder.BattleMenu().endTurnIcon == null &&
					DataHolder.BattleMenu().endTurnIconName != null &&
					"" != DataHolder.BattleMenu().endTurnIconName)
				{
					DataHolder.BattleMenu().endTurnIcon = (Texture2D)Resources.Load(BattleSystemData.ICON_PATH+
							DataHolder.BattleMenu().endTurnIconName, typeof(Texture2D));
				}
				DataHolder.BattleMenu().endTurnIcon = (Texture2D)EditorGUILayout.ObjectField("Icon", 
						DataHolder.BattleMenu().endTurnIcon, typeof(Texture2D), false);
				if(DataHolder.BattleMenu().endTurnIcon)
				{
					DataHolder.BattleMenu().endTurnIconName = DataHolder.BattleMenu().endTurnIcon.name;
				}
				else DataHolder.BattleMenu().endTurnIconName = "";
				EditorGUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
		}
	}
	
	private void ShowTarget()
	{
		fold6 = EditorGUILayout.Foldout(fold6, "Target selection");
		if(fold6)
		{
			DataHolder.BattleMenu().useTargetMenu = EditorGUILayout.Toggle("Use menu", 
					DataHolder.BattleMenu().useTargetMenu, GUILayout.Width(pw.mWidth));
			if(DataHolder.BattleMenu().useTargetMenu)
			{
				DataHolder.BattleMenu().targetPosition = EditorGUILayout.Popup("Menu position", 
						DataHolder.BattleMenu().targetPosition, 
						DataHolder.DialoguePositions().GetNameList(true), GUILayout.Width(pw.mWidth));
			}
			EditorGUILayout.Separator();
			DataHolder.BattleMenu().useTargetCursor = EditorGUILayout.Toggle("Use cursor", 
					DataHolder.BattleMenu().useTargetCursor, GUILayout.Width(pw.mWidth));
			if(DataHolder.BattleMenu().useTargetCursor)
			{
				if(this.tmpPrefab == null && "" != DataHolder.BattleMenu().cursorPrefabName)
				{
					this.tmpPrefab = (GameObject)Resources.Load(
							BattleSystemData.PREFAB_PATH+
							DataHolder.BattleMenu().cursorPrefabName, typeof(GameObject));
				}
				this.tmpPrefab = (GameObject)EditorGUILayout.ObjectField("Cursor prefab", this.tmpPrefab, 
						typeof(GameObject), false, GUILayout.Width(pw.mWidth*2));
				if(this.tmpPrefab) DataHolder.BattleMenu().cursorPrefabName = this.tmpPrefab.name;
				else DataHolder.BattleMenu().cursorPrefabName = "";
				
				
				DataHolder.BattleMenu().cursorChildName = EditorGUILayout.TextField("Path to child", 
						DataHolder.BattleMenu().cursorChildName, GUILayout.Width(pw.mWidth*2));
				
				DataHolder.BattleMenu().cursorOffset = EditorGUILayout.Vector3Field("Cursor offset", 
						DataHolder.BattleMenu().cursorOffset, GUILayout.Width(pw.mWidth));
			}
			
			EditorGUILayout.Separator();
			DataHolder.BattleMenu().useTargetBlink = EditorGUILayout.Toggle("Blink target", 
					DataHolder.BattleMenu().useTargetBlink, GUILayout.Width(pw.mWidth));
			
			if(DataHolder.BattleMenu().useTargetBlink)
			{
				DataHolder.BattleMenu().fromCurrent = EditorGUILayout.Toggle("From current", 
						DataHolder.BattleMenu().fromCurrent, GUILayout.Width(pw.mWidth));
				DataHolder.BattleMenu().blinkChildren = EditorGUILayout.Toggle("Blink children", 
						DataHolder.BattleMenu().blinkChildren, GUILayout.Width(pw.mWidth));
				DataHolder.BattleMenu().blinkInterpolation = (EaseType)EditorGUILayout.EnumPopup("Interpolation", 
						DataHolder.BattleMenu().blinkInterpolation, GUILayout.Width(pw.mWidth));
				DataHolder.BattleMenu().blinkTime = EditorGUILayout.FloatField("Blink Time", 
						DataHolder.BattleMenu().blinkTime, GUILayout.Width(pw.mWidth));
				
				EditorGUILayout.BeginHorizontal();
				DataHolder.BattleMenu().aBlink = EditorGUILayout.Toggle("Alpha", 
						DataHolder.BattleMenu().aBlink, GUILayout.Width(pw.mWidth));
				if(DataHolder.BattleMenu().aBlink)
				{
					if(!DataHolder.BattleMenu().fromCurrent) DataHolder.BattleMenu().aStart = EditorGUILayout.FloatField("From", 
							DataHolder.BattleMenu().aStart, GUILayout.Width(pw.mWidth));
					DataHolder.BattleMenu().aEnd = EditorGUILayout.FloatField("To", 
							DataHolder.BattleMenu().aEnd, GUILayout.Width(pw.mWidth));
				}
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				DataHolder.BattleMenu().rBlink = EditorGUILayout.Toggle("Red", 
						DataHolder.BattleMenu().rBlink, GUILayout.Width(pw.mWidth));
				if(DataHolder.BattleMenu().rBlink)
				{
					if(!DataHolder.BattleMenu().fromCurrent) DataHolder.BattleMenu().rStart = EditorGUILayout.FloatField("From", 
							DataHolder.BattleMenu().rStart, GUILayout.Width(pw.mWidth));
					DataHolder.BattleMenu().rEnd = EditorGUILayout.FloatField("To", 
							DataHolder.BattleMenu().rEnd, GUILayout.Width(pw.mWidth));
				}
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				DataHolder.BattleMenu().gBlink = EditorGUILayout.Toggle("Green", 
						DataHolder.BattleMenu().gBlink, GUILayout.Width(pw.mWidth));
				if(DataHolder.BattleMenu().gBlink)
				{
					if(!DataHolder.BattleMenu().fromCurrent) DataHolder.BattleMenu().gStart = EditorGUILayout.FloatField("From", 
							DataHolder.BattleMenu().gStart, GUILayout.Width(pw.mWidth));
					DataHolder.BattleMenu().gEnd = EditorGUILayout.FloatField("To", 
							DataHolder.BattleMenu().gEnd, GUILayout.Width(pw.mWidth));
				}
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				DataHolder.BattleMenu().bBlink = EditorGUILayout.Toggle("Blue", 
						DataHolder.BattleMenu().bBlink, GUILayout.Width(pw.mWidth));
				if(DataHolder.BattleMenu().bBlink)
				{
					if(!DataHolder.BattleMenu().fromCurrent) DataHolder.BattleMenu().bStart= EditorGUILayout.FloatField("From", 
							DataHolder.BattleMenu().bStart, GUILayout.Width(pw.mWidth));
					DataHolder.BattleMenu().bEnd = EditorGUILayout.FloatField("To", 
							DataHolder.BattleMenu().bEnd, GUILayout.Width(pw.mWidth));
				}
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Separator();
			}
			EditorGUILayout.Separator();
			DataHolder.BattleMenu().mouseTouch = EditorHelper.MouseTouchControlSettings(
					DataHolder.BattleMenu().mouseTouch, true);
		}
	}
}