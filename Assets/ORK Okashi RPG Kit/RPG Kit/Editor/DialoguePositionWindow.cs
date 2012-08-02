
using UnityEditor;
using UnityEngine;
using System.Collections;

public class DialoguePositionWindow : EditorWindow
{
	public int mWidth = 300;
	public int mWidth2 = 200;
	private int selection = 0;
	private Vector2 SP1 = new Vector2(0, 0);
	private Vector2 SP2 = new Vector2(0, 0);
	
	private bool fold1 = true;
	private bool fold2 = true;
	private bool fold3 = true;
	private bool fold4 = true;
	private bool fold5 = true;
	private bool fold6 = true;
	private bool fold7 = true;
	
	private bool recalculate = false;
	private bool recalcPaddings = true;
	private bool recalcSpacings = true;
	private Vector2 oldSize = new Vector2(1280, 800);
	private Vector2 newSize = new Vector2(1280, 800);
	
	[MenuItem("RPG Kit/Position Editors/Dialogue Position Editor")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		DialoguePositionWindow window = (DialoguePositionWindow)EditorWindow.GetWindow(typeof(DialoguePositionWindow), false, "Dialogue Position Editor");
		window.Show();
	}
	
	static void Init(int index)
	{
		// Get existing open window or if none, make a new one:
		DialoguePositionWindow window = (DialoguePositionWindow)EditorWindow.GetWindow(typeof(DialoguePositionWindow), false, "Dialogue Position Editor");
		window.selection = index;
		window.Show();
	}
	
	void OnGUI()
	{
		if(this.recalculate)
		{
			EditorGUILayout.BeginVertical();
			if(GUILayout.Button("Cancel", GUILayout.Width(this.mWidth2)))
			{
				this.recalculate = false;
				return;
			}
			EditorGUILayout.Separator();
			GUILayout.Label("Recalculate screen size", EditorStyles.boldLabel);
			this.oldSize = EditorGUILayout.Vector2Field("Old screen size", this.oldSize, GUILayout.Width(this.mWidth));
			this.newSize = EditorGUILayout.Vector2Field("Sew screen size", this.newSize, GUILayout.Width(this.mWidth));
			this.recalcPaddings = EditorGUILayout.Toggle("Paddings", this.recalcPaddings, GUILayout.Width(this.mWidth));
			this.recalcSpacings = EditorGUILayout.Toggle("Spacings", this.recalcSpacings, GUILayout.Width(this.mWidth));
			
			if(GUILayout.Button("Perform resize", GUILayout.Width(this.mWidth)))
			{
				float factorX = this.newSize.x / this.oldSize.x;
				float factorY = this.newSize.y / this.oldSize.y;
				for(int i=0; i<DataHolder.DialoguePositions().GetDataCount(); i++)
				{
					DataHolder.DialoguePosition(i).boxBounds = RecalculationHelper.RecalculateRect(DataHolder.DialoguePosition(i).boxBounds, factorX, factorY);
					DataHolder.DialoguePosition(i).nameBounds = RecalculationHelper.RecalculateRect(DataHolder.DialoguePosition(i).nameBounds, factorX, factorY);
					DataHolder.DialoguePosition(i).dragBounds = RecalculationHelper.RecalculateRect(DataHolder.DialoguePosition(i).dragBounds, factorX, factorY);
					DataHolder.DialoguePosition(i).moveInStart = RecalculationHelper.RecalculateVector2(DataHolder.DialoguePosition(i).moveInStart, factorX, factorY);
					DataHolder.DialoguePosition(i).moveOutStart = RecalculationHelper.RecalculateVector2(DataHolder.DialoguePosition(i).moveOutStart, factorX, factorY);
					DataHolder.DialoguePosition(i).choiceWidth *= factorX;
					DataHolder.DialoguePosition(i).choiceOffsetX *= factorX;
					if(this.recalcPaddings)
					{
						DataHolder.DialoguePosition(i).boxPadding = RecalculationHelper.RecalculateVector4(DataHolder.DialoguePosition(i).boxPadding, factorX, factorY);
						DataHolder.DialoguePosition(i).namePadding = RecalculationHelper.RecalculateVector4(DataHolder.DialoguePosition(i).namePadding, factorX, factorY);
						DataHolder.DialoguePosition(i).choicePadding = RecalculationHelper.RecalculateVector2(DataHolder.DialoguePosition(i).choicePadding, factorX, factorY);
					}
					if(this.recalcSpacings)
					{
						DataHolder.DialoguePosition(i).lineSpacing *= factorY;
						DataHolder.DialoguePosition(i).columnSpacing *= ((factorX+factorY)/2);;
					}
				}
				this.recalculate = false;
				return;
			}
			EditorGUILayout.EndVertical();
		}
		else
		{
			EditorGUILayout.BeginVertical();
			// buttons
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Add", GUILayout.Width(this.mWidth2)))
			{
				DataHolder.DialoguePositions().AddDialoguePos("New Position");
				selection = DataHolder.DialoguePositions().GetDataCount()-1;
				GUI.FocusControl ("ID");
			}
			GUI.SetNextControlName("Copy");
			if(GUILayout.Button("Copy", GUILayout.Width(this.mWidth2)))
			{
				GUI.FocusControl("Copy");
				DataHolder.DialoguePositions().Copy(selection);
				selection = DataHolder.DialoguePositions().GetDataCount()-1;
			}
			if(DataHolder.DialoguePositions().GetDataCount() > 1)
			{
				GUI.SetNextControlName("Rem");
				if(GUILayout.Button("Remove", GUILayout.Width(this.mWidth2)))
				{
					GUI.FocusControl("Rem");
					DataHolder.DialoguePositions().RemoveData(selection);
				}
			}
			GUILayout.FlexibleSpace();
			if(GUILayout.Button("Recalculate", GUILayout.Width(this.mWidth2)))
			{
				this.recalculate = true;
				return;
			}
			if(selection > DataHolder.DialoguePositions().GetDataCount()-1)
			{
				selection = DataHolder.DialoguePositions().GetDataCount()-1;
			}
			EditorGUILayout.EndHorizontal();
			
			// elements list
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical(GUILayout.Width(this.mWidth2));
			EditorGUILayout.Separator();
			EditorGUILayout.BeginVertical("box");
			SP1 = EditorGUILayout.BeginScrollView(SP1);
			if(DataHolder.DialoguePositions().GetDataCount() > 0)
			{
				int prev = selection;
				selection = GUILayout.SelectionGrid(selection, DataHolder.DialoguePositions().GetNameList(true), 1);
				if(prev != selection)
				{
					GUI.FocusControl("ID");
				}
			}
			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndVertical();
			
			// element settings
			EditorGUILayout.BeginVertical();
			SP2 = EditorGUILayout.BeginScrollView(SP2);
			if(DataHolder.DialoguePositions().GetDataCount() > 0)
			{
				EditorGUILayout.Separator();
				GUI.SetNextControlName("ID");
				EditorGUILayout.LabelField("Position ID", selection.ToString(), GUILayout.Width(this.mWidth));
				DataHolder.DialoguePositions().name[selection] = EditorGUILayout.TextField("Name", DataHolder.DialoguePositions().name[selection]);
				EditorGUILayout.Separator();
				
				EditorGUILayout.BeginVertical("box");
				fold7 = EditorGUILayout.Foldout(fold7, "Base settings");
				if(fold7)
				{
					if(DataHolder.DialoguePosition(selection).skin == null &&
						DataHolder.DialoguePosition(selection).skinName != null &&
						"" != DataHolder.DialoguePosition(selection).skinName)
					{
						DataHolder.DialoguePosition(selection).skin = (GUISkin)Resources.Load(DataHolder.DialoguePositions().skinPath+
								DataHolder.DialoguePosition(selection).skinName, typeof(GUISkin));
					}
					DataHolder.DialoguePosition(selection).skin = (GUISkin)EditorGUILayout.ObjectField("GUISkin", 
							DataHolder.DialoguePosition(selection).skin, typeof(GUISkin), false, GUILayout.Width(this.mWidth*1.5f));
					if(DataHolder.DialoguePosition(selection).skin)
					{
						DataHolder.DialoguePosition(selection).skinName = DataHolder.DialoguePosition(selection).skin.name;
					}
					else DataHolder.DialoguePosition(selection).skinName = "";
					
					if(DataHolder.DialoguePosition(selection).selectSkin == null &&
						DataHolder.DialoguePosition(selection).selectSkinName != null &&
						"" != DataHolder.DialoguePosition(selection).selectSkinName)
					{
						DataHolder.DialoguePosition(selection).selectSkin = (GUISkin)Resources.Load(DataHolder.DialoguePositions().skinPath+
								DataHolder.DialoguePosition(selection).selectSkinName, typeof(GUISkin));
					}
					DataHolder.DialoguePosition(selection).selectSkin = (GUISkin)EditorGUILayout.ObjectField("Selected choice skin", 
							DataHolder.DialoguePosition(selection).selectSkin, typeof(GUISkin), false, GUILayout.Width(this.mWidth*1.5f));
					if(DataHolder.DialoguePosition(selection).selectSkin)
					{
						DataHolder.DialoguePosition(selection).selectSkinName = DataHolder.DialoguePosition(selection).selectSkin.name;
					}
					else DataHolder.DialoguePosition(selection).selectSkinName = "";
					
					if(DataHolder.DialoguePosition(selection).okSkin == null &&
						DataHolder.DialoguePosition(selection).okSkinName != null &&
						"" != DataHolder.DialoguePosition(selection).okSkinName)
					{
						DataHolder.DialoguePosition(selection).okSkin = (GUISkin)Resources.Load(DataHolder.DialoguePositions().skinPath+
								DataHolder.DialoguePosition(selection).okSkinName, typeof(GUISkin));
					}
					DataHolder.DialoguePosition(selection).okSkin = (GUISkin)EditorGUILayout.ObjectField("OK button skin", 
							DataHolder.DialoguePosition(selection).okSkin, typeof(GUISkin), false, GUILayout.Width(this.mWidth*1.5f));
					if(DataHolder.DialoguePosition(selection).okSkin)
					{
						DataHolder.DialoguePosition(selection).okSkinName = DataHolder.DialoguePosition(selection).okSkin.name;
					}
					else DataHolder.DialoguePosition(selection).okSkinName = "";
					
					if(DataHolder.DialoguePosition(selection).nameSkin == null &&
						DataHolder.DialoguePosition(selection).nameSkinName != null &&
						"" != DataHolder.DialoguePosition(selection).nameSkinName)
					{
						DataHolder.DialoguePosition(selection).nameSkin = (GUISkin)Resources.Load(DataHolder.DialoguePositions().skinPath+
								DataHolder.DialoguePosition(selection).nameSkinName, typeof(GUISkin));
					}
					DataHolder.DialoguePosition(selection).nameSkin = (GUISkin)EditorGUILayout.ObjectField("Name box skin", 
							DataHolder.DialoguePosition(selection).nameSkin, typeof(GUISkin), false, GUILayout.Width(this.mWidth*1.5f));
					if(DataHolder.DialoguePosition(selection).nameSkin)
					{
						DataHolder.DialoguePosition(selection).nameSkinName = DataHolder.DialoguePosition(selection).nameSkin.name;
					}
					else DataHolder.DialoguePosition(selection).nameSkinName = "";
					
					EditorGUILayout.Separator();
					
					DataHolder.DialoguePosition(selection).isDragWindow = EditorGUILayout.Toggle("Drag window", 
							DataHolder.DialoguePosition(selection).isDragWindow, GUILayout.Width(this.mWidth));
					if(DataHolder.DialoguePosition(selection).isDragWindow)
					{
						DataHolder.DialoguePosition(selection).dragBounds = EditorGUILayout.RectField("Drag bounds", 
								DataHolder.DialoguePosition(selection).dragBounds, GUILayout.Width(this.mWidth));
					}
					
					EditorGUILayout.Separator();
					DataHolder.DialoguePosition(selection).hideButton = EditorGUILayout.Toggle("Hide OK button", 
							DataHolder.DialoguePosition(selection).hideButton, GUILayout.Width(this.mWidth));
					DataHolder.DialoguePosition(selection).autoCollapse = EditorGUILayout.Toggle("Auto collapse", 
							DataHolder.DialoguePosition(selection).autoCollapse, GUILayout.Width(this.mWidth));
					
					if(!DataHolder.DialoguePosition(selection).isDragWindow)
					{
						DataHolder.DialoguePosition(selection).oneline = EditorGUILayout.Toggle("Is one line", 
								DataHolder.DialoguePosition(selection).oneline, GUILayout.Width(this.mWidth));
					}
					if(DataHolder.DialoguePosition(selection).oneline && !DataHolder.DialoguePosition(selection).isDragWindow)
					{
						DataHolder.DialoguePosition(selection).alignCenter = EditorGUILayout.Toggle("Center align", 
								DataHolder.DialoguePosition(selection).alignCenter, GUILayout.Width(this.mWidth));
					}
					else
					{
						DataHolder.DialoguePosition(selection).scrollable = EditorGUILayout.Toggle("Scrollable", 
								DataHolder.DialoguePosition(selection).scrollable, GUILayout.Width(this.mWidth));
					}
					EditorGUILayout.Separator();
				}
				EditorGUILayout.EndVertical();
				
				EditorGUILayout.BeginVertical("box");
				fold1 = EditorGUILayout.Foldout(fold1, "Text box");
				if(fold1)
				{
					DataHolder.DialoguePosition(selection).showBox = EditorGUILayout.Toggle("Show box", DataHolder.DialoguePosition(selection).showBox, GUILayout.Width(this.mWidth));
					
					DataHolder.DialoguePosition(selection).boxBounds = EditorGUILayout.RectField("Bounds", DataHolder.DialoguePosition(selection).boxBounds, GUILayout.Width(this.mWidth));
					if(!DataHolder.DialoguePosition(selection).alignCenter)
					{
						DataHolder.DialoguePosition(selection).boxPadding = EditorGUILayout.Vector4Field("Padding", DataHolder.DialoguePosition(selection).boxPadding, GUILayout.Width(this.mWidth));
						DataHolder.DialoguePosition(selection).lineSpacing = EditorGUILayout.FloatField("Line spacing", DataHolder.DialoguePosition(selection).lineSpacing, GUILayout.Width(this.mWidth));
					}
					EditorGUILayout.Separator();
				}
				EditorGUILayout.EndVertical();
				
				if(!DataHolder.DialoguePosition(selection).oneline && !DataHolder.DialoguePosition(selection).isDragWindow)
				{
					EditorGUILayout.BeginVertical("box");
					fold2 = EditorGUILayout.Foldout(fold2, "Name box");
					if(fold2)
					{
						DataHolder.DialoguePosition(selection).showNameBox = EditorGUILayout.Toggle("Show box", DataHolder.DialoguePosition(selection).showNameBox, GUILayout.Width(this.mWidth));
						DataHolder.DialoguePosition(selection).nameBounds = EditorGUILayout.RectField("Bounds", DataHolder.DialoguePosition(selection).nameBounds, GUILayout.Width(this.mWidth));
						DataHolder.DialoguePosition(selection).namePadding = EditorGUILayout.Vector4Field("Padding", DataHolder.DialoguePosition(selection).namePadding, GUILayout.Width(this.mWidth));
						EditorGUILayout.Separator();
					}
					EditorGUILayout.EndVertical();
				}
				
				EditorGUILayout.BeginVertical("box");
				fold3 = EditorGUILayout.Foldout(fold3, "Shadow");
				if(fold3)
				{
					DataHolder.DialoguePosition(selection).showShadow = EditorGUILayout.Toggle("Show shadow", DataHolder.DialoguePosition(selection).showShadow, GUILayout.Width(this.mWidth));
					if(DataHolder.DialoguePosition(selection).showShadow)
					{
						DataHolder.DialoguePosition(selection).shadowOffset = EditorGUILayout.Vector2Field("Offset", DataHolder.DialoguePosition(selection).shadowOffset, GUILayout.Width(this.mWidth));
					}
					EditorGUILayout.Separator();
				}
				EditorGUILayout.EndVertical();
				
				EditorGUILayout.BeginVertical("box");
				fold4 = EditorGUILayout.Foldout(fold4, "Choice");
				if(fold4)
				{
					DataHolder.DialoguePosition(selection).choicePadding = EditorGUILayout.Vector2Field("Choice padding", DataHolder.DialoguePosition(selection).choicePadding, GUILayout.Width(this.mWidth));
					DataHolder.DialoguePosition(selection).choiceInactiveAlpha = EditorGUILayout.FloatField("Inactive alpha", DataHolder.DialoguePosition(selection).choiceInactiveAlpha, GUILayout.Width(this.mWidth));
					DataHolder.DialoguePosition(selection).choiceDefineWidth = EditorGUILayout.Toggle("Define width", DataHolder.DialoguePosition(selection).choiceDefineWidth, GUILayout.Width(this.mWidth));
					if(DataHolder.DialoguePosition(selection).choiceDefineWidth)
					{
						DataHolder.DialoguePosition(selection).choiceWidth = EditorGUILayout.FloatField("Choice width", DataHolder.DialoguePosition(selection).choiceWidth, GUILayout.Width(this.mWidth));
						DataHolder.DialoguePosition(selection).choiceOffsetX = EditorGUILayout.FloatField("Choice offset x", DataHolder.DialoguePosition(selection).choiceOffsetX, GUILayout.Width(this.mWidth));
					}
					EditorGUILayout.Separator();
					DataHolder.DialoguePosition(selection).choiceColumns = EditorGUILayout.IntField("Choice columns", DataHolder.DialoguePosition(selection).choiceColumns, GUILayout.Width(this.mWidth));
					DataHolder.DialoguePosition(selection).columnSpacing = EditorGUILayout.FloatField("Column spacing", DataHolder.DialoguePosition(selection).columnSpacing, GUILayout.Width(this.mWidth));
					DataHolder.DialoguePosition(selection).columnFill = (ColumnFill)EditorGUILayout.EnumPopup("Column fill", DataHolder.DialoguePosition(selection).columnFill, GUILayout.Width(this.mWidth));
					EditorGUILayout.Separator();
					
					DataHolder.DialoguePosition(selection).selectFirst = EditorGUILayout.Toggle("Select first", 
							DataHolder.DialoguePosition(selection).selectFirst, GUILayout.Width(this.mWidth));
					EditorGUILayout.Separator();
				}
				EditorGUILayout.EndVertical();
				
				EditorGUILayout.BeginVertical("box");
				fold5 = EditorGUILayout.Foldout(fold5, "Fading");
				if(fold5)
				{
					DataHolder.DialoguePosition(selection).fadeIn = EditorGUILayout.Toggle("Fade in", DataHolder.DialoguePosition(selection).fadeIn, GUILayout.Width(this.mWidth));
					if(DataHolder.DialoguePosition(selection).fadeIn)
					{
						DataHolder.DialoguePosition(selection).fadeInTime = EditorGUILayout.FloatField("Time", DataHolder.DialoguePosition(selection).fadeInTime, GUILayout.Width(this.mWidth));
						DataHolder.DialoguePosition(selection).fadeInInterpolation = (EaseType)EditorGUILayout.EnumPopup("Interpolation", DataHolder.DialoguePosition(selection).fadeInInterpolation, GUILayout.Width(this.mWidth));
					}
					EditorGUILayout.Separator();
					DataHolder.DialoguePosition(selection).fadeOut = EditorGUILayout.Toggle("Fade out", DataHolder.DialoguePosition(selection).fadeOut, GUILayout.Width(this.mWidth));
					if(DataHolder.DialoguePosition(selection).fadeOut)
					{
						DataHolder.DialoguePosition(selection).fadeOutTime = EditorGUILayout.FloatField("Time", DataHolder.DialoguePosition(selection).fadeOutTime, GUILayout.Width(this.mWidth));
						DataHolder.DialoguePosition(selection).fadeOutInterpolation = (EaseType)EditorGUILayout.EnumPopup("Interpolation", DataHolder.DialoguePosition(selection).fadeOutInterpolation, GUILayout.Width(this.mWidth));
					}
					EditorGUILayout.Separator();
				}
				EditorGUILayout.EndVertical();
				
				EditorGUILayout.BeginVertical("box");
				fold6 = EditorGUILayout.Foldout(fold6, "Moving");
				if(fold6)
				{
					DataHolder.DialoguePosition(selection).moveIn = EditorGUILayout.Toggle("Move in", DataHolder.DialoguePosition(selection).moveIn, GUILayout.Width(this.mWidth));
					if(DataHolder.DialoguePosition(selection).moveIn)
					{
						DataHolder.DialoguePosition(selection).moveInStart = EditorGUILayout.Vector2Field("Start position", DataHolder.DialoguePosition(selection).moveInStart, GUILayout.Width(this.mWidth));
						DataHolder.DialoguePosition(selection).moveInTime = EditorGUILayout.FloatField("Time", DataHolder.DialoguePosition(selection).moveInTime, GUILayout.Width(this.mWidth));
						DataHolder.DialoguePosition(selection).moveInInterpolation = (EaseType)EditorGUILayout.EnumPopup("Interpolation", DataHolder.DialoguePosition(selection).moveInInterpolation, GUILayout.Width(this.mWidth));
					}
					EditorGUILayout.Separator();
					DataHolder.DialoguePosition(selection).moveOut = EditorGUILayout.Toggle("Move out", DataHolder.DialoguePosition(selection).moveOut, GUILayout.Width(this.mWidth));
					if(DataHolder.DialoguePosition(selection).moveOut)
					{
						DataHolder.DialoguePosition(selection).moveOutStart = EditorGUILayout.Vector2Field("End position", DataHolder.DialoguePosition(selection).moveOutStart, GUILayout.Width(this.mWidth));
						DataHolder.DialoguePosition(selection).moveOutTime = EditorGUILayout.FloatField("Time", DataHolder.DialoguePosition(selection).moveOutTime, GUILayout.Width(this.mWidth));
						DataHolder.DialoguePosition(selection).moveOutInterpolation = (EaseType)EditorGUILayout.EnumPopup("Interpolation", DataHolder.DialoguePosition(selection).moveOutInterpolation, GUILayout.Width(this.mWidth));
					}
					EditorGUILayout.Separator();
				}
				EditorGUILayout.EndVertical();
			}
			EditorGUILayout.Separator();
			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();
			GUI.SetNextControlName("Reload");
			if(GUILayout.Button("Reload Settings"))
			{
				GUI.FocusControl("Reload");
				DataHolder.Instance().Init();
				selection = 0;
			}
			GUI.SetNextControlName("Save");
			if(GUILayout.Button("Save Settings"))
			{
				GUI.FocusControl("Save");
				DataHolder.DialoguePositions().SaveData();
			}
			EditorGUILayout.EndHorizontal();
		}
	}
}