
using UnityEditor;
using UnityEngine;

public class HUDEditor : EditorWindow
{
	public int mWidth = 300;
	public int mWidth2 = 200;
	private int selection = 0;
	private Vector2 SP1 = new Vector2(0, 0);
	private Vector2 SP2 = new Vector2(0, 0);
	
	protected bool fold1 = true;
	protected bool fold2 = true;
	protected bool fold3 = true;
	protected bool fold4 = true;
	
	private bool recalculate = false;
	private bool recalcSpacings = true;
	private Vector2 oldSize = new Vector2(1280, 800);
	private Vector2 newSize = new Vector2(1280, 800);
	
	[MenuItem("RPG Kit/HUD Editor")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		HUDEditor window = (HUDEditor)EditorWindow.GetWindow(typeof(HUDEditor), false, "HUD Editor");
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
			this.recalcSpacings = EditorGUILayout.Toggle("Spacings", this.recalcSpacings, GUILayout.Width(this.mWidth));
			
			if(GUILayout.Button("Perform resize", GUILayout.Width(this.mWidth)))
			{
				float factorX = this.newSize.x / this.oldSize.x;
				float factorY = this.newSize.y / this.oldSize.y;
				for(int i=0; i<DataHolder.HUDs().GetDataCount(); i++)
				{
					DataHolder.HUD(i).bounds = RecalculationHelper.RecalculateRect(DataHolder.HUD(i).bounds, factorX, factorY);
					DataHolder.HUD(i).offset = RecalculationHelper.RecalculateVector2(DataHolder.HUD(i).offset, factorX, factorY);
					DataHolder.HUD(i).moveInStart = RecalculationHelper.RecalculateVector2(DataHolder.HUD(i).moveInStart, factorX, factorY);
					DataHolder.HUD(i).moveOutStart = RecalculationHelper.RecalculateVector2(DataHolder.HUD(i).moveOutStart, factorX, factorY);
					for(int j=0; j<DataHolder.HUD(i).element.Length; j++)
					{
						DataHolder.HUD(i).element[j].bounds = RecalculationHelper.RecalculateRect(DataHolder.HUD(i).element[j].bounds, factorX, factorY);
						if(this.recalcSpacings)
						{
							DataHolder.HUD(i).element[j].spacing *= ((factorX+factorY)/2);
						}
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
				DataHolder.HUDs().AddHUD("New HUD");
				selection = DataHolder.HUDs().GetDataCount()-1;
				GUI.FocusControl ("ID");
			}
			GUI.SetNextControlName("Copy");
			if(GUILayout.Button("Copy", GUILayout.Width(this.mWidth2)))
			{
				GUI.FocusControl("Copy");
				DataHolder.HUDs().Copy(selection);
				selection = DataHolder.HUDs().GetDataCount()-1;
			}
			GUI.SetNextControlName("Rem");
			if(GUILayout.Button("Remove", GUILayout.Width(this.mWidth2)))
			{
				GUI.FocusControl("Rem");
				DataHolder.HUDs().RemoveData(selection);
			}
			GUILayout.FlexibleSpace();
			if(GUILayout.Button("Recalculate", GUILayout.Width(this.mWidth2)))
			{
				this.recalculate = true;
				return;
			}
			if(selection > DataHolder.HUDs().GetDataCount()-1)
			{
				selection = DataHolder.HUDs().GetDataCount()-1;
			}
			EditorGUILayout.EndHorizontal();
			
			// elements list
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical(GUILayout.Width(this.mWidth2));
			EditorGUILayout.Separator();
			EditorGUILayout.BeginVertical("box");
			SP1 = EditorGUILayout.BeginScrollView(SP1);
			if(DataHolder.HUDs().GetDataCount() > 0)
			{
				int prev = selection;
				selection = GUILayout.SelectionGrid(selection, DataHolder.HUDs().GetNameList(true), 1);
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
			if(DataHolder.HUDs().GetDataCount() > 0)
			{
				EditorGUILayout.BeginVertical();
				EditorGUILayout.Separator();
				GUI.SetNextControlName("ID");
				EditorGUILayout.LabelField("Position ID", selection.ToString(), GUILayout.Width(this.mWidth));
				DataHolder.HUDs().name[selection] = EditorGUILayout.TextField("Name", DataHolder.HUDs().name[selection]);
				EditorGUILayout.Separator();
				
				EditorGUILayout.BeginVertical("box");
				fold1 = EditorGUILayout.Foldout(fold1, "Visibility");
				if(fold1)
				{
					string[] types = System.Enum.GetNames(typeof(ControlType));
					if(types.Length != DataHolder.HUD(selection).controlType.Length)
					{
						bool[] tmp = DataHolder.HUD(selection).controlType;
						DataHolder.HUD(selection).controlType = new bool[types.Length];
						for(int i=0; i<DataHolder.HUD(selection).controlType.Length; i++)
						{
							if(i<tmp.Length) DataHolder.HUD(selection).controlType[i] = tmp[i];
						}
					}
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.BeginVertical();
					for(int i=0; i<types.Length; i++)
					{
						DataHolder.HUD(selection).controlType[i] = EditorGUILayout.Toggle(types[i], DataHolder.HUD(selection).controlType[i], GUILayout.Width(this.mWidth));
					}
					EditorGUILayout.EndVertical();
					DataHolder.HUD(selection).onInteraction = EditorGUILayout.Toggle("Interaction available", DataHolder.HUD(selection).onInteraction, GUILayout.Width(this.mWidth));
					GUILayout.FlexibleSpace();
					EditorGUILayout.EndHorizontal();
					
					EditorGUILayout.Separator();
				}
				EditorGUILayout.EndVertical();
				
				EditorGUILayout.BeginVertical("box");
				fold2 = EditorGUILayout.Foldout(fold2, "General settings");
				if(fold2)
				{
					if(DataHolder.HUD(selection).skin == null &&
						DataHolder.HUD(selection).skinName != null &&
						"" != DataHolder.HUD(selection).skinName)
					{
						DataHolder.HUD(selection).skin = (GUISkin)Resources.Load(DataHolder.HUDs().resourcePath+
								DataHolder.HUD(selection).skinName, typeof(GUISkin));
					}
					DataHolder.HUD(selection).skin = (GUISkin)EditorGUILayout.ObjectField("GUISkin", DataHolder.HUD(selection).skin, typeof(GUISkin), false);
					if(DataHolder.HUD(selection).skin)
					{
						DataHolder.HUD(selection).skinName = DataHolder.HUD(selection).skin.name;
					}
					else DataHolder.HUD(selection).skinName = "";
					
					EditorGUILayout.Separator();
					
					DataHolder.HUD(selection).showBox = EditorGUILayout.Toggle("Show box", DataHolder.HUD(selection).showBox, GUILayout.Width(this.mWidth));
					DataHolder.HUD(selection).bounds = EditorGUILayout.RectField("Bounds", DataHolder.HUD(selection).bounds, GUILayout.Width(this.mWidth));
					DataHolder.HUD(selection).onlyOne = EditorGUILayout.Toggle("Only one shown", DataHolder.HUD(selection).onlyOne, GUILayout.Width(this.mWidth));
					if(!DataHolder.HUD(selection).onlyOne)
					{
						DataHolder.HUD(selection).offset = EditorGUILayout.Vector2Field("Offset per char", DataHolder.HUD(selection).offset, GUILayout.Width(this.mWidth));
					}
					
					DataHolder.HUD(selection).hudClick = (HUDClick)EditorGUILayout.EnumPopup("Click function", DataHolder.HUD(selection).hudClick, GUILayout.Width(this.mWidth));
					if(HUDClick.MENUSCREEN.Equals(DataHolder.HUD(selection).hudClick))
					{
						DataHolder.HUD(selection).screenIndex = EditorGUILayout.IntField("Menu screen ID", 
								DataHolder.HUD(selection).screenIndex, GUILayout.Width(this.mWidth));
					}
					else if(HUDClick.BATTLEMENU.Equals(DataHolder.HUD(selection).hudClick))
					{
						GUILayout.Label("0=Base menu, 1=Item menu, 2=Skill menu");
						DataHolder.HUD(selection).screenIndex = EditorGUILayout.IntField("Battle menu", 
								DataHolder.HUD(selection).screenIndex, GUILayout.Width(this.mWidth));
					}
					
					EditorGUILayout.Separator();
					GUILayout.Label("Fading", EditorStyles.boldLabel);
					DataHolder.HUD(selection).fadeIn = EditorGUILayout.Toggle("Fade in", DataHolder.HUD(selection).fadeIn, GUILayout.Width(this.mWidth));
					if(DataHolder.HUD(selection).fadeIn)
					{
						DataHolder.HUD(selection).fadeInTime = EditorGUILayout.FloatField("Time", DataHolder.HUD(selection).fadeInTime, GUILayout.Width(this.mWidth));
						DataHolder.HUD(selection).fadeInInterpolation = (EaseType)EditorGUILayout.EnumPopup("Interpolation", DataHolder.HUD(selection).fadeInInterpolation, GUILayout.Width(this.mWidth));
					}
					EditorGUILayout.Separator();
					DataHolder.HUD(selection).fadeOut = EditorGUILayout.Toggle("Fade out", DataHolder.HUD(selection).fadeOut, GUILayout.Width(this.mWidth));
					if(DataHolder.HUD(selection).fadeOut)
					{
						DataHolder.HUD(selection).fadeOutTime = EditorGUILayout.FloatField("Time", DataHolder.HUD(selection).fadeOutTime, GUILayout.Width(this.mWidth));
						DataHolder.HUD(selection).fadeOutInterpolation = (EaseType)EditorGUILayout.EnumPopup("Interpolation", DataHolder.HUD(selection).fadeOutInterpolation, GUILayout.Width(this.mWidth));
					}
					
					EditorGUILayout.Separator();
					GUILayout.Label("Moving", EditorStyles.boldLabel);
					DataHolder.HUD(selection).moveIn = EditorGUILayout.Toggle("Move in", DataHolder.HUD(selection).moveIn, GUILayout.Width(this.mWidth));
					if(DataHolder.HUD(selection).moveIn)
					{
						DataHolder.HUD(selection).moveInStart = EditorGUILayout.Vector2Field("Start position", DataHolder.HUD(selection).moveInStart, GUILayout.Width(this.mWidth));
						DataHolder.HUD(selection).moveInTime = EditorGUILayout.FloatField("Time", DataHolder.HUD(selection).moveInTime, GUILayout.Width(this.mWidth));
						DataHolder.HUD(selection).moveInInterpolation = (EaseType)EditorGUILayout.EnumPopup("Interpolation", DataHolder.HUD(selection).moveInInterpolation, GUILayout.Width(this.mWidth));
					}
					EditorGUILayout.Separator();
					DataHolder.HUD(selection).moveOut = EditorGUILayout.Toggle("Move out", DataHolder.HUD(selection).moveOut, GUILayout.Width(this.mWidth));
					if(DataHolder.HUD(selection).moveOut)
					{
						DataHolder.HUD(selection).moveOutStart = EditorGUILayout.Vector2Field("End position", DataHolder.HUD(selection).moveOutStart, GUILayout.Width(this.mWidth));
						DataHolder.HUD(selection).moveOutTime = EditorGUILayout.FloatField("Time", DataHolder.HUD(selection).moveOutTime, GUILayout.Width(this.mWidth));
						DataHolder.HUD(selection).moveOutInterpolation = (EaseType)EditorGUILayout.EnumPopup("Interpolation", DataHolder.HUD(selection).moveOutInterpolation, GUILayout.Width(this.mWidth));
					}
					
					EditorGUILayout.Separator();
				}
				EditorGUILayout.EndVertical();
				
				EditorGUILayout.Separator();
				EditorGUILayout.BeginHorizontal();
				if(GUILayout.Button("Add HUD Element", GUILayout.Width(this.mWidth)))
				{
					DataHolder.HUD(selection).AddElement();
				}
				GUILayout.FlexibleSpace();
				if(GUILayout.Button("Collapse all", GUILayout.Width(this.mWidth*0.5f)))
				{
					for(int i=0; i<DataHolder.HUD(selection).element.Length; i++) DataHolder.HUD(selection).element[i].fold = false;
				}
				if(GUILayout.Button("Expand all", GUILayout.Width(this.mWidth*0.5f)))
				{
					for(int i=0; i<DataHolder.HUD(selection).element.Length; i++) DataHolder.HUD(selection).element[i].fold = true;
				}
				EditorGUILayout.EndHorizontal();
				
				for(int i=0; i<DataHolder.HUD(selection).element.Length; i++)
				{
					EditorGUILayout.BeginVertical("box");
					DataHolder.HUD(selection).element[i].fold = EditorGUILayout.Foldout(DataHolder.HUD(selection).element[i].fold, "HUD Element "+i);
					if(DataHolder.HUD(selection).element[i].fold)
					{
						EditorGUILayout.BeginHorizontal();
						if(GUILayout.Button("Remove", GUILayout.Width(this.mWidth*0.5f)))
						{
							DataHolder.HUD(selection).RemoveElement(i);
							return;
						}
						if(i > 0)
						{
							if(GUILayout.Button("Move Up", GUILayout.Width(this.mWidth*0.5f)))
							{
								DataHolder.HUD(selection).MoveUp(i);
								return;
							}
						}
						if(i < DataHolder.HUD(selection).element.Length-1)
						{
							if(GUILayout.Button("Move Down", GUILayout.Width(this.mWidth*0.5f)))
							{
								DataHolder.HUD(selection).MoveDown(i);
								return;
							}
						}
						GUILayout.FlexibleSpace();
						EditorGUILayout.EndHorizontal();
						DataHolder.HUD(selection).element[i].showBox = EditorGUILayout.Toggle("Show box", 
								DataHolder.HUD(selection).element[i].showBox, GUILayout.Width(this.mWidth));
						DataHolder.HUD(selection).element[i].bounds = EditorGUILayout.RectField("Bounds", 
								DataHolder.HUD(selection).element[i].bounds, GUILayout.Width(this.mWidth));
						DataHolder.HUD(selection).element[i].textAnchor = (TextAnchor)EditorGUILayout.EnumPopup("Anchor", 
								DataHolder.HUD(selection).element[i].textAnchor, GUILayout.Width(this.mWidth));
						
						EditorGUILayout.Separator();
						DataHolder.HUD(selection).element[i].type = (HUDElementType)EditorGUILayout.EnumPopup("Type", DataHolder.HUD(selection).element[i].type, GUILayout.Width(this.mWidth));
						
						if((HUDElementType.STATUS.Equals(DataHolder.HUD(selection).element[i].type) &&
							DataHolder.StatusValue(DataHolder.HUD(selection).element[i].statusID).IsConsumable()) ||
							HUDElementType.TIMEBAR.Equals(DataHolder.HUD(selection).element[i].type) ||
							HUDElementType.USED_TIMEBAR.Equals(DataHolder.HUD(selection).element[i].type) ||
							HUDElementType.CASTTIME.Equals(DataHolder.HUD(selection).element[i].type))
						{
							DataHolder.HUD(selection).element[i].displayType = (HUDDisplayType)EditorGUILayout.EnumPopup("Display type", 
									DataHolder.HUD(selection).element[i].displayType, GUILayout.Width(this.mWidth));
							if(HUDDisplayType.BAR.Equals(DataHolder.HUD(selection).element[i].displayType))
							{
								DataHolder.HUD(selection).element[i].useImage = EditorGUILayout.Toggle("Use bar image", 
										DataHolder.HUD(selection).element[i].useImage, GUILayout.Width(this.mWidth));
								if(!DataHolder.HUD(selection).element[i].useImage)
								{
									DataHolder.HUD(selection).element[i].barColor = EditorGUILayout.Popup("Bar color", 
											DataHolder.HUD(selection).element[i].barColor, DataHolder.Colors().GetNameList(true), GUILayout.Width(this.mWidth));
								}
							}
						}
						
						// text element
						if(HUDElementType.TEXT.Equals(DataHolder.HUD(selection).element[i].type))
						{
							EditorGUILayout.Separator();
							GUILayout.Label("Text", EditorStyles.boldLabel);
							int langs = DataHolder.Languages().GetDataCount();
							if(DataHolder.HUD(selection).element[i].text.Length != langs)
							{
								string[] dmy = DataHolder.HUD(selection).element[i].text;
								DataHolder.HUD(selection).element[i].text = new string[langs];
								for(int j=0; j<langs; j++)
								{
									if(j < dmy.Length) DataHolder.HUD(selection).element[i].text[j] = dmy[j];
									else DataHolder.HUD(selection).element[i].text[j] = "";
								}
							}
							for(int j=0; j<langs; j++)
							{
								if(DataHolder.HUD(selection).element[i].text[j] == null) DataHolder.HUD(selection).element[i].text[j] = "";
								DataHolder.HUD(selection).element[i].text[j] = EditorGUILayout.TextField(DataHolder.Languages().GetName(j), 
										DataHolder.HUD(selection).element[i].text[j]);
							}
						}
						// name element
						else if(HUDElementType.NAME.Equals(DataHolder.HUD(selection).element[i].type))
						{
							DataHolder.HUD(selection).element[i].nameType = (HUDNameType)EditorGUILayout.EnumPopup("Name type", 
									DataHolder.HUD(selection).element[i].nameType, GUILayout.Width(this.mWidth));
							if(HUDNameType.STATUS.Equals(DataHolder.HUD(selection).element[i].nameType))
							{
								DataHolder.HUD(selection).element[i].statusID = EditorGUILayout.Popup("Status value", DataHolder.HUD(selection).element[i].statusID,
										DataHolder.StatusValues().GetNameList(true), GUILayout.Width(this.mWidth));
							}
						}
						// status element
						else if(HUDElementType.STATUS.Equals(DataHolder.HUD(selection).element[i].type))
						{
							EditorGUILayout.Separator();
							DataHolder.HUD(selection).element[i].statusID = EditorGUILayout.Popup("Status value", DataHolder.HUD(selection).element[i].statusID,
									DataHolder.StatusValues().GetNameList(true), GUILayout.Width(this.mWidth));
							bool consumable = DataHolder.StatusValue(DataHolder.HUD(selection).element[i].statusID).IsConsumable();
							if(!consumable) DataHolder.HUD(selection).element[i].displayType = HUDDisplayType.TEXT;
							if(consumable &&
								HUDDisplayType.TEXT.Equals(DataHolder.HUD(selection).element[i].displayType))
							{
								DataHolder.HUD(selection).element[i].showMax = EditorGUILayout.Toggle("Show max SV", 
										DataHolder.HUD(selection).element[i].showMax, GUILayout.Width(this.mWidth));
								if(DataHolder.HUD(selection).element[i].showMax)
								{
									DataHolder.HUD(selection).element[i].divider = EditorGUILayout.TextField("Divider", DataHolder.HUD(selection).element[i].divider);
								}
							}
						}
						// effect element
						else if(HUDElementType.EFFECT.Equals(DataHolder.HUD(selection).element[i].type))
						{
							DataHolder.HUD(selection).element[i].rows = EditorGUILayout.IntField("Rows", 
									DataHolder.HUD(selection).element[i].rows, GUILayout.Width(this.mWidth));
							DataHolder.HUD(selection).element[i].columns = EditorGUILayout.IntField("Columns", 
									DataHolder.HUD(selection).element[i].columns, GUILayout.Width(this.mWidth));
							DataHolder.HUD(selection).element[i].spacing = EditorGUILayout.FloatField("Spacing", 
									DataHolder.HUD(selection).element[i].spacing, GUILayout.Width(this.mWidth));
						}
						// variable element
						else if(HUDElementType.VARIABLE.Equals(DataHolder.HUD(selection).element[i].type))
						{
							EditorGUILayout.Separator();
							GUILayout.Label("Text: Use #v where the variable should be displayed", EditorStyles.boldLabel);
							int langs = DataHolder.Languages().GetDataCount();
							if(DataHolder.HUD(selection).element[i].text.Length != langs)
							{
								string[] dmy = DataHolder.HUD(selection).element[i].text;
								DataHolder.HUD(selection).element[i].text = new string[langs];
								for(int j=0; j<langs; j++)
								{
									if(j < dmy.Length) DataHolder.HUD(selection).element[i].text[j] = dmy[j];
									else DataHolder.HUD(selection).element[i].text[j] = "";
								}
							}
							for(int j=0; j<langs; j++)
							{
								if(DataHolder.HUD(selection).element[i].text[j] == null) DataHolder.HUD(selection).element[i].text[j] = "";
								DataHolder.HUD(selection).element[i].text[j] = EditorGUILayout.TextField(DataHolder.Languages().GetName(j), 
										DataHolder.HUD(selection).element[i].text[j]);
							}
							
							EditorGUILayout.Separator();
							DataHolder.HUD(selection).element[i].variableName = EditorGUILayout.TextField("Variable name", 
									DataHolder.HUD(selection).element[i].variableName);
							DataHolder.HUD(selection).element[i].numberVariable = EditorGUILayout.Toggle("Number variable", 
									DataHolder.HUD(selection).element[i].numberVariable, GUILayout.Width(this.mWidth));
							if(DataHolder.HUD(selection).element[i].numberVariable)
							{
								DataHolder.HUD(selection).element[i].asInt = EditorGUILayout.Toggle("As integer", 
										DataHolder.HUD(selection).element[i].asInt, GUILayout.Width(this.mWidth));
							}
						}
						
						// image options
						if(HUDElementType.IMAGE.Equals(DataHolder.HUD(selection).element[i].type) ||
							(HUDElementType.STATUS.Equals(DataHolder.HUD(selection).element[i].type) && 
							HUDDisplayType.BAR.Equals(DataHolder.HUD(selection).element[i].displayType) && 
							DataHolder.HUD(selection).element[i].useImage && 
							DataHolder.StatusValue(DataHolder.HUD(selection).element[i].statusID).IsConsumable()) ||
							((HUDElementType.TIMEBAR.Equals(DataHolder.HUD(selection).element[i].type) ||
							HUDElementType.USED_TIMEBAR.Equals(DataHolder.HUD(selection).element[i].type) ||
							HUDElementType.CASTTIME.Equals(DataHolder.HUD(selection).element[i].type)) && 
							HUDDisplayType.BAR.Equals(DataHolder.HUD(selection).element[i].displayType) &&
							DataHolder.HUD(selection).element[i].useImage))
						{
							EditorGUILayout.Separator();
							if(DataHolder.HUD(selection).element[i].texture == null &&
								DataHolder.HUD(selection).element[i].imageName != null &&
								"" != DataHolder.HUD(selection).element[i].imageName)
							{
								DataHolder.HUD(selection).element[i].texture = (Texture2D)Resources.Load(DataHolder.HUDs().resourcePath+
										DataHolder.HUD(selection).element[i].imageName, typeof(Texture2D));
							}
							DataHolder.HUD(selection).element[i].texture = (Texture2D)EditorGUILayout.ObjectField("Image", DataHolder.HUD(selection).element[i].texture, typeof(Texture2D), false);
							if(DataHolder.HUD(selection).element[i].texture)
							{
								DataHolder.HUD(selection).element[i].imageName = DataHolder.HUD(selection).element[i].texture.name;
							}
							else DataHolder.HUD(selection).element[i].imageName = "";
							
							DataHolder.HUD(selection).element[i].scaleMode = (ScaleMode)EditorGUILayout.EnumPopup("Scale mode", 
									DataHolder.HUD(selection).element[i].scaleMode, GUILayout.Width(this.mWidth));
							DataHolder.HUD(selection).element[i].alphaBlend = EditorGUILayout.Toggle("Alpha blend", 
									DataHolder.HUD(selection).element[i].alphaBlend, GUILayout.Width(this.mWidth));
							DataHolder.HUD(selection).element[i].imageAspect = EditorGUILayout.FloatField("Image aspect", 
									DataHolder.HUD(selection).element[i].imageAspect, GUILayout.Width(this.mWidth));
						}
						
						
						// text options
						if(HUDElementType.TEXT.Equals(DataHolder.HUD(selection).element[i].type) ||
							HUDElementType.NAME.Equals(DataHolder.HUD(selection).element[i].type) ||
							(HUDElementType.STATUS.Equals(DataHolder.HUD(selection).element[i].type) && 
							HUDDisplayType.TEXT.Equals(DataHolder.HUD(selection).element[i].displayType)) ||
							((HUDElementType.TIMEBAR.Equals(DataHolder.HUD(selection).element[i].type) ||
							HUDElementType.USED_TIMEBAR.Equals(DataHolder.HUD(selection).element[i].type) ||
							HUDElementType.CASTTIME.Equals(DataHolder.HUD(selection).element[i].type)) && 
							HUDDisplayType.TEXT.Equals(DataHolder.HUD(selection).element[i].displayType)) ||
							(HUDElementType.EFFECT.Equals(DataHolder.HUD(selection).element[i].type) &&
							(HUDContentType.TEXT.Equals(DataHolder.HUD(selection).element[i].contentType) ||
							HUDContentType.BOTH.Equals(DataHolder.HUD(selection).element[i].contentType))) ||
							HUDElementType.VARIABLE.Equals(DataHolder.HUD(selection).element[i].type))
						{
							EditorGUILayout.Separator();
							DataHolder.HUD(selection).element[i].textColor = EditorGUILayout.Popup("Text color", 
									DataHolder.HUD(selection).element[i].textColor, DataHolder.Colors().GetNameList(true), GUILayout.Width(this.mWidth));
							DataHolder.HUD(selection).element[i].showShadow = EditorGUILayout.Toggle("Shadow", 
									DataHolder.HUD(selection).element[i].showShadow, GUILayout.Width(this.mWidth));
							if(DataHolder.HUD(selection).element[i].showShadow)
							{
								DataHolder.HUD(selection).element[i].shadowColor = EditorGUILayout.Popup("Shadow color", 
										DataHolder.HUD(selection).element[i].shadowColor, DataHolder.Colors().GetNameList(true), GUILayout.Width(this.mWidth));
								DataHolder.HUD(selection).element[i].shadowOffset = EditorGUILayout.Vector2Field("Shadow offset", 
										DataHolder.HUD(selection).element[i].shadowOffset, GUILayout.Width(this.mWidth));
							}
						}
						
						if(HUDElementType.NAME.Equals(DataHolder.HUD(selection).element[i].type) ||
							HUDElementType.EFFECT.Equals(DataHolder.HUD(selection).element[i].type))
						{
							DataHolder.HUD(selection).element[i].contentType = (HUDContentType)EditorGUILayout.EnumPopup("Content", 
									DataHolder.HUD(selection).element[i].contentType, GUILayout.Width(this.mWidth));
						}
						EditorGUILayout.Separator();
					}
					EditorGUILayout.EndVertical();
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
				DataHolder.HUDs().SaveData();
			}
			EditorGUILayout.EndHorizontal();
		}
	}
}