
using UnityEditor;
using UnityEngine;
using System.Collections;

public class GameSettingsTab : BaseTab
{
	private Texture2D tmpIcon2;
	private Texture2D tmpIcon3;
	private GameObject tmpObject;
	
	private int viewMode = 0;
	private FontSaver[] fonts = new FontSaver[0];
	private bool changeMipMaps = false;
	private bool mipmapEnabled = false;
	private bool compressed = false;
	
	public GameSettingsTab(ProjectWindow pw) : base(pw)
	{
		this.Reload();
	}
	
	new public void Reload()
	{
		base.Reload();
		this.tmpIcon2 = null;
		this.tmpIcon3 = null;
		this.tmpObject = null;
		
		int count = pw.GetLangCount();
		bool reset = false;
		if(DataHolder.GameSettings().dialogueOkText.Length != count)
		{
			DataHolder.GameSettings().dialogueOkText = new string[count];
			DataHolder.GameSettings().itemCollectionText = new string[count];
			DataHolder.GameSettings().dialogueOkSize = new Vector2[count];
			DataHolder.GameSettings().dialogueOkOffset = new Vector2[count];
			reset = true;
		}
		for(int i=0; i<count; i++)
		{
			if(DataHolder.GameSettings().dialogueOkText[i] == null) DataHolder.GameSettings().dialogueOkText[i] = "Ok";
			if(DataHolder.GameSettings().itemCollectionText[i] == null) DataHolder.GameSettings().itemCollectionText[i] = "You found % %n!";
			if(reset)
			{
				DataHolder.GameSettings().dialogueOkSize[i] = new Vector2(100, 30);
				DataHolder.GameSettings().dialogueOkOffset[i] = new Vector2(0, 0);
			}
		}
	}
	
	public void ShowTab()
	{
		if(this.viewMode == 1)
		{
			EditorGUILayout.BeginVertical();
			SP1 = EditorGUILayout.BeginScrollView(SP1);
			EditorGUILayout.BeginHorizontal();
			GUI.SetNextControlName("SaveFont");
			if(GUILayout.Button("Save Fonts", GUILayout.Width(pw.mWidth2)))
			{
				GUI.FocusControl("SaveFont");
				this.SaveFontData();
				return;
			}
			if(GUILayout.Button("Cancel", GUILayout.Width(pw.mWidth2)))
			{
				this.fonts = new FontSaver[0];
				this.viewMode = 0;
				return;
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
			GUILayout.Label("Warning: Unicode fonts can take some time to save!", EditorStyles.boldLabel);
			GUILayout.Label("Warning: Dynamic fonts are not supported!", EditorStyles.boldLabel);
			EditorGUILayout.Separator();
			for(int i=0; i<this.fonts.Length; i++)
			{
				this.fonts[i].ShowSettings();
			}
			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
		}
		else if(this.viewMode == 2)
		{
			EditorGUILayout.BeginVertical();
			SP1 = EditorGUILayout.BeginScrollView(SP1);
			EditorGUILayout.BeginHorizontal();
			GUI.SetNextControlName("ChangeTextures");
			if(GUILayout.Button("Change textures", GUILayout.Width(pw.mWidth2)))
			{
				GUI.FocusControl("ChangeTextures");
				Object[] obj = Resources.LoadAll("");
				for(int i=0; i<obj.Length; i++)
				{
					if(obj[i] is Texture)
					{
						string path = AssetDatabase.GetAssetPath(obj[i]);
						TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
						if(importer != null)
						{
							importer.textureType = TextureImporterType.Advanced;
							importer.isReadable = true;
							if(this.changeMipMaps) importer.mipmapEnabled = this.mipmapEnabled;
							if(this.compressed) importer.textureFormat = TextureImporterFormat.DXT5;
							else importer.textureFormat = TextureImporterFormat.ARGB32;
							AssetDatabase.ImportAsset(path);
						}
					}
				}
				this.viewMode = 0;
				return;
			}
			if(GUILayout.Button("Cancel", GUILayout.Width(pw.mWidth2)))
			{
				this.viewMode = 0;
				return;
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
			GUILayout.Label("Warning: All textures in a 'Resources' folder will be reimported with the following settings:", EditorStyles.boldLabel);
			GUILayout.Label("Texture Type: Advanced", EditorStyles.boldLabel);
			GUILayout.Label("Read/Write Enabled", EditorStyles.boldLabel);
			GUILayout.Label("Texture Format: ARGB 32 bit", EditorStyles.boldLabel);
			EditorGUILayout.Separator();
			this.compressed = EditorGUILayout.Toggle("Compress (DXT5)", this.compressed, GUILayout.Width(pw.mWidth));
			this.changeMipMaps = EditorGUILayout.Toggle("Change Mip Maps", this.changeMipMaps, GUILayout.Width(pw.mWidth));
			if(this.changeMipMaps)
			{
				this.mipmapEnabled = EditorGUILayout.Toggle("Generate Mip Maps", this.mipmapEnabled, GUILayout.Width(pw.mWidth));
			}
			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
		}
		else
		{
			int langs = DataHolder.Languages().GetDataCount();
			EditorGUILayout.BeginVertical();
			SP1 = EditorGUILayout.BeginScrollView(SP1);
			EditorGUILayout.Separator();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical("box");
			fold1 = EditorGUILayout.Foldout(fold1, "Base settings");
			if(fold1)
			{
				GUILayout.Label("Data settings", EditorStyles.boldLabel);
				DataHolder.GameSettings().encryptData = EditorGUILayout.Toggle("Encrypt data", 
						DataHolder.GameSettings().encryptData, GUILayout.Width(pw.mWidth));
				EditorGUILayout.Separator();
				
				GUILayout.Label("Default screen size", EditorStyles.boldLabel);
				EditorGUILayout.BeginHorizontal();
				DataHolder.GameSettings().defaultScreen.x = EditorGUILayout.FloatField("Width", 
						DataHolder.GameSettings().defaultScreen.x, GUILayout.Width(pw.mWidth));
				DataHolder.GameSettings().defaultScreen.y = EditorGUILayout.FloatField("Height", 
						DataHolder.GameSettings().defaultScreen.y, GUILayout.Width(pw.mWidth));
				EditorGUILayout.EndHorizontal();
				DataHolder.GameSettings().guiScaleMode = (GUIScaleMode)EditorGUILayout.EnumPopup("GUI scale mode", 
						DataHolder.GameSettings().guiScaleMode, GUILayout.Width(pw.mWidth*1.2f));
				EditorGUILayout.Separator();
				
				GUILayout.Label("Key settings", EditorStyles.boldLabel);
				DataHolder.GameSettings().inputHandling = (InputHandling)EditorGUILayout.EnumPopup("Input handling",
						DataHolder.GameSettings().inputHandling, GUILayout.Width(pw.mWidth*1.2f));
				if(InputHandling.BUTTON_DOWN.Equals(DataHolder.GameSettings().inputHandling) ||
					InputHandling.BUTTON_UP.Equals(DataHolder.GameSettings().inputHandling))
				{
					GUILayout.Label("Input axes (name) set up in the Input Manager are used as keys.");
				}
				else
				{
					GUILayout.Label("Button names are used as keys. E.g. 'a'");
				}
				
				DataHolder.GameSettings().acceptKey = EditorGUILayout.TextField("Accept key", 
						DataHolder.GameSettings().acceptKey, GUILayout.Width(pw.mWidth));
				DataHolder.GameSettings().cancelKey = EditorGUILayout.TextField("Cancel key", 
						DataHolder.GameSettings().cancelKey, GUILayout.Width(pw.mWidth));
				DataHolder.GameSettings().verticalKey = EditorGUILayout.TextField("Vertical key", 
						DataHolder.GameSettings().verticalKey, GUILayout.Width(pw.mWidth));
				DataHolder.GameSettings().horizontalKey = EditorGUILayout.TextField("Horizontal key", 
						DataHolder.GameSettings().horizontalKey, GUILayout.Width(pw.mWidth));
				EditorGUILayout.Separator();
				
				GUILayout.Label("Skill level settings", EditorStyles.boldLabel);
				DataHolder.GameSettings().skillPlusKey = EditorGUILayout.TextField("Level plus key", 
						DataHolder.GameSettings().skillPlusKey, GUILayout.Width(pw.mWidth));
				DataHolder.GameSettings().skillMinusKey = EditorGUILayout.TextField("Level minus key", 
						DataHolder.GameSettings().skillMinusKey, GUILayout.Width(pw.mWidth));
				DataHolder.GameSettings().loopSkillLevels = EditorGUILayout.Toggle("Loop levels",
						DataHolder.GameSettings().loopSkillLevels, GUILayout.Width(pw.mWidth));
				DataHolder.GameSettings().addSkillLevel = EditorGUILayout.Toggle("Add levels",
						DataHolder.GameSettings().addSkillLevel, GUILayout.Width(pw.mWidth));
				
				if(DataHolder.GameSettings().addSkillLevel)
				{
					DataHolder.GameSettings().skillLevelName = EditorHelper.CheckLanguageCount(
							DataHolder.GameSettings().skillLevelName, langs);
					
					GUILayout.Label("%n for skill name, % for skill level");
					for(int i=0; i<langs; i++)
					{
						DataHolder.GameSettings().skillLevelName[i] = EditorGUILayout.TextField(DataHolder.Languages().GetName(i), 
								DataHolder.GameSettings().skillLevelName[i], GUILayout.Width(pw.mWidth*1.2f));
					}
				}
				EditorGUILayout.Separator();
				
				GUILayout.Label("Control settings", EditorStyles.boldLabel);
				DataHolder.GameSettings().gameControl = (GameControl)this.EnumToolbar("Game control", 
						(int)DataHolder.GameSettings().gameControl, typeof(GameControl), (int)(pw.mWidth*1.5f));
				DataHolder.GameSettings().cursorTimeout = EditorGUILayout.FloatField("Cursor timeout", 
						DataHolder.GameSettings().cursorTimeout, GUILayout.Width(pw.mWidth));
				DataHolder.GameSettings().clickTimeout = EditorGUILayout.FloatField("Click timeout", 
						DataHolder.GameSettings().clickTimeout, GUILayout.Width(pw.mWidth));
				EditorGUILayout.Separator();
				
				GUILayout.Label("Pause settings", EditorStyles.boldLabel);
				DataHolder.GameSettings().pauseKey = EditorGUILayout.TextField("Pause key", 
						DataHolder.GameSettings().pauseKey, GUILayout.Width(pw.mWidth));
				DataHolder.GameSettings().pauseTime = EditorGUILayout.Toggle("Pause time",
						DataHolder.GameSettings().pauseTime, GUILayout.Width(pw.mWidth));
				DataHolder.GameSettings().freezePause = EditorGUILayout.Toggle("Freeze on pause",
						DataHolder.GameSettings().freezePause, GUILayout.Width(pw.mWidth));
				EditorGUILayout.Separator();
				
				GUILayout.Label("GUI System", EditorStyles.boldLabel);
				DataHolder.GameSettings().guiSystemType = (GUISystemType)this.EnumToolbar("System type", 
						(int)DataHolder.GameSettings().guiSystemType, typeof(GUISystemType), (int)(pw.mWidth*1.5f));
				if(GUISystemType.ORK.Equals(DataHolder.GameSettings().guiSystemType))
				{
					GUILayout.Label("Warning: Only textures with enabled pixel read/write\n"+
							"are supported! This also applies for GUISkins!", EditorStyles.boldLabel);
					DataHolder.GameSettings().guiFilterMode = (FilterMode)this.EnumToolbar("Filter mode", 
							(int)DataHolder.GameSettings().guiFilterMode, typeof(FilterMode), (int)(pw.mWidth*1.5f));
					DataHolder.GameSettings().guiImageStretch = (GUIImageStretch)this.EnumToolbar("Image stretching", 
							(int)DataHolder.GameSettings().guiImageStretch, typeof(GUIImageStretch), (int)(pw.mWidth*1.5f));
					
					DataHolder.GameSettings().battleTextureDelete = (TextureDelete)this.EnumToolbar("Battle tex delete", 
							(int)DataHolder.GameSettings().battleTextureDelete, typeof(TextureDelete), (int)(pw.mWidth*1.5f));
					DataHolder.GameSettings().menuTextureDelete = (TextureDelete)this.EnumToolbar("Menu tex delete", 
							(int)DataHolder.GameSettings().menuTextureDelete, typeof(TextureDelete), (int)(pw.mWidth*1.5f));
					
					DataHolder.GameSettings().mipMapBias = EditorGUILayout.FloatField("Mip map bias",
							DataHolder.GameSettings().mipMapBias, GUILayout.Width(pw.mWidth));
					DataHolder.GameSettings().maxClickDistance = EditorGUILayout.IntField("Click offset",
							DataHolder.GameSettings().maxClickDistance, GUILayout.Width(pw.mWidth));
					if(DataHolder.GameSettings().maxClickDistance < 0) DataHolder.GameSettings().maxClickDistance = 0;
					
					EditorGUILayout.BeginHorizontal();
					DataHolder.GameSettings().hudRefreshRate = EditorGUILayout.FloatField("HUD refresh rate",
							DataHolder.GameSettings().hudRefreshRate, GUILayout.Width(pw.mWidth));
					DataHolder.GameSettings().hudRefreshFrame = EditorGUILayout.Toggle("Frames",
							DataHolder.GameSettings().hudRefreshFrame, GUILayout.Width(pw.mWidth));
					EditorGUILayout.EndHorizontal();
					DataHolder.GameSettings().autoRemoveLayer = EditorGUILayout.Toggle("Auto rem. layer",
							DataHolder.GameSettings().autoRemoveLayer, GUILayout.Width(pw.mWidth));
					
					EditorGUILayout.Separator();
					DataHolder.GameSettings().preloadFonts = EditorGUILayout.Toggle("Preload fonts",
							DataHolder.GameSettings().preloadFonts, GUILayout.Width(pw.mWidth));
					DataHolder.GameSettings().preloadBoxes = EditorGUILayout.Toggle("Preload boxes",
							DataHolder.GameSettings().preloadBoxes, GUILayout.Width(pw.mWidth));
					DataHolder.GameSettings().preloadAreaNames = EditorGUILayout.Toggle("Preload area names",
							DataHolder.GameSettings().preloadAreaNames, GUILayout.Width(pw.mWidth));
					DataHolder.GameSettings().noScrollbar = EditorGUILayout.Toggle("No scrollbar",
							DataHolder.GameSettings().noScrollbar, GUILayout.Width(pw.mWidth));
					DataHolder.GameSettings().noScrollbarThumb = EditorGUILayout.Toggle("No scroll thumb",
							DataHolder.GameSettings().noScrollbarThumb, GUILayout.Width(pw.mWidth));
					DataHolder.GameSettings().noAutoCollapse = EditorGUILayout.Toggle("No auto collapse",
							DataHolder.GameSettings().noAutoCollapse, GUILayout.Width(pw.mWidth));
					
					EditorGUILayout.Separator();
					EditorGUILayout.BeginHorizontal();
					GUI.SetNextControlName("SaveFont");
					if(GUILayout.Button("Save fonts", GUILayout.Width(pw.mWidth2)))
					{
						GUI.FocusControl("SaveFont");
						this.InitFontSave();
					}
					GUI.SetNextControlName("ChangeTextures");
					if(GUILayout.Button("Change textures", GUILayout.Width(pw.mWidth2)))
					{
						GUI.FocusControl("ChangeTextures");
						this.viewMode = 2;
					}
					EditorGUILayout.EndHorizontal();
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box", GUILayout.Width(pw.mWidth*2));
			fold9 = EditorGUILayout.Foldout(fold9, "Party settings");
			if(fold9)
			{
				DataHolder.GameSettings().maxBattleParty = EditorGUILayout.IntField("Battle party size", 
						DataHolder.GameSettings().maxBattleParty, GUILayout.Width(pw.mWidth));
				EditorGUILayout.Separator();
				
				GUILayout.Label("Key settings", EditorStyles.boldLabel);
				DataHolder.GameSettings().charPlusKey = EditorGUILayout.TextField("Char. plus key", 
						DataHolder.GameSettings().charPlusKey, GUILayout.Width(pw.mWidth));
				DataHolder.GameSettings().charMinusKey = EditorGUILayout.TextField("Char. minus key", 
						DataHolder.GameSettings().charMinusKey, GUILayout.Width(pw.mWidth));
				DataHolder.GameSettings().switchPlayer = EditorGUILayout.Toggle("Switch player",
						DataHolder.GameSettings().switchPlayer, GUILayout.Width(pw.mWidth));
				if(DataHolder.GameSettings().switchPlayer)
				{
					DataHolder.GameSettings().switchOnlyBP = EditorGUILayout.Toggle("Only battle party",
							DataHolder.GameSettings().switchOnlyBP, GUILayout.Width(pw.mWidth));
				}
				EditorGUILayout.Separator();
				
				GUILayout.Label("Party spawn settings", EditorStyles.boldLabel);
				DataHolder.GameSettings().spawnParty = EditorGUILayout.Toggle("Spawn party",
						DataHolder.GameSettings().spawnParty, GUILayout.Width(pw.mWidth));
				if(DataHolder.GameSettings().spawnParty)
				{
					if(DataHolder.BattleSystem().IsRealTime())
					{
						DataHolder.GameSettings().onlyInBattleArea = EditorGUILayout.Toggle("Only in battles", 
								DataHolder.GameSettings().onlyInBattleArea, GUILayout.Width(pw.mWidth));
					}
					else DataHolder.GameSettings().onlyInBattleArea = false;
					DataHolder.GameSettings().spawnOnlyBP = EditorGUILayout.Toggle("Only battle party",
							DataHolder.GameSettings().spawnOnlyBP, GUILayout.Width(pw.mWidth));
					DataHolder.GameSettings().spawnDistance = EditorGUILayout.FloatField("Distance",
							DataHolder.GameSettings().spawnDistance, GUILayout.Width(pw.mWidth));
					DataHolder.GameSettings().partyFollow = EditorGUILayout.Toggle("Party follow",
							DataHolder.GameSettings().partyFollow, GUILayout.Width(pw.mWidth));
				}
				else DataHolder.GameSettings().partyFollow = false;
				EditorGUILayout.Separator();
				
				GUILayout.Label("Auto add components to player", EditorStyles.boldLabel);
				if(GUILayout.Button("Add component", GUILayout.Width(pw.mWidth2)))
				{
					DataHolder.GameSettings().AddPlayerComponent();
				}
				for(int i=0; i<DataHolder.GameSettings().playerComponent.Length; i++)
				{
					EditorGUILayout.BeginHorizontal();
					if(GUILayout.Button("Remove", GUILayout.Width(pw.mWidth3)))
					{
						DataHolder.GameSettings().RemovePlayerComponent(i);
						break;
					}
					DataHolder.GameSettings().playerComponent[i] = EditorGUILayout.TextField("Component",
							DataHolder.GameSettings().playerComponent[i]);
					EditorGUILayout.EndHorizontal();
				}
				EditorGUILayout.Separator();
				
				DataHolder.GameSettings().addInteractionController = EditorGUILayout.Toggle("Add interaction controller",
						DataHolder.GameSettings().addInteractionController, GUILayout.Width(pw.mWidth));
				if(DataHolder.GameSettings().addInteractionController)
				{
					if(this.tmpObject == null && 
						"" != DataHolder.GameSettings().interactionControllerName)
					{
						this.tmpObject = (GameObject)Resources.Load(
								CharacterData.PREFAB_PATH+
								DataHolder.GameSettings().interactionControllerName, typeof(GameObject));
					}
					this.tmpObject = (GameObject)EditorGUILayout.ObjectField(
							"Controller prefab", this.tmpObject, 
							typeof(GameObject), false, GUILayout.Width(pw.mWidth*2));
					if(this.tmpObject != null)
					{
						DataHolder.GameSettings().interactionControllerName = this.tmpObject.name;
					}
					else DataHolder.GameSettings().interactionControllerName = "";
						
					DataHolder.GameSettings().interactionControllerChildName = EditorGUILayout.TextField("Path to child", 
							DataHolder.GameSettings().interactionControllerChildName, GUILayout.Width(pw.mWidth*2));
					DataHolder.GameSettings().interactionControllerOffset = EditorGUILayout.Vector3Field("Offset", 
							DataHolder.GameSettings().interactionControllerOffset, GUILayout.Width(pw.mWidth));
				}
				EditorGUILayout.Separator();
			}
			EditorGUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical("box", GUILayout.Width(pw.mWidth*2));
			fold10 = EditorGUILayout.Foldout(fold10, "Player control settings");
			if(fold10)
			{
				GUILayout.Label("Player control settings", EditorStyles.boldLabel);
				DataHolder.GameSettings().playerControlSettings = EditorHelper.PlayerControlSettings(
						DataHolder.GameSettings().playerControlSettings);
				EditorGUILayout.Separator();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box", GUILayout.Width(pw.mWidth*2));
			fold11 = EditorGUILayout.Foldout(fold11, "Camera control settings");
			if(fold11)
			{
				GUILayout.Label("Camera control settings", EditorStyles.boldLabel);
				DataHolder.GameSettings().cameraControlSettings = EditorHelper.CameraControlSettings(
						DataHolder.GameSettings().cameraControlSettings);
				EditorGUILayout.Separator();
			}
			EditorGUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginVertical("box");
			fold3 = EditorGUILayout.Foldout(fold3, "Dialogue settings");
			if(fold3)
			{
				DataHolder.GameSettings().scrollSpeed = EditorGUILayout.FloatField("Key scrollspeed", DataHolder.GameSettings().scrollSpeed, GUILayout.Width(pw.mWidth));
				EditorGUILayout.Separator();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical();
				for(int i=0; i<DataHolder.GameSettings().dialogueOkText.Length; i++)
				{
					GUILayout.Label("OK button "+DataHolder.Languages().GetName(i), EditorStyles.boldLabel);
					DataHolder.GameSettings().dialogueOkText[i] = EditorGUILayout.TextField("Text", DataHolder.GameSettings().dialogueOkText[i], GUILayout.Width(pw.mWidth*1.2f));
					EditorGUILayout.BeginHorizontal();
					DataHolder.GameSettings().dialogueOkSize[i].x = EditorGUILayout.FloatField("Width", DataHolder.GameSettings().dialogueOkSize[i].x, GUILayout.Width(pw.mWidth));
					DataHolder.GameSettings().dialogueOkSize[i].y = EditorGUILayout.FloatField("Height", DataHolder.GameSettings().dialogueOkSize[i].y, GUILayout.Width(pw.mWidth));
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					DataHolder.GameSettings().dialogueOkOffset[i].x = EditorGUILayout.FloatField("Offset X", DataHolder.GameSettings().dialogueOkOffset[i].x, GUILayout.Width(pw.mWidth));
					DataHolder.GameSettings().dialogueOkOffset[i].y = EditorGUILayout.FloatField("Offset Y", DataHolder.GameSettings().dialogueOkOffset[i].y, GUILayout.Width(pw.mWidth));
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.Separator();
				}
				EditorGUILayout.EndVertical();
				if(this.tmpIcon == null && 
					DataHolder.GameSettings().dialogueOkIconName != null &&
					"" != DataHolder.GameSettings().dialogueOkIconName)
				{
					this.tmpIcon = (Texture2D)Resources.Load(GameSettingsData.ICON_PATH+
							DataHolder.GameSettings().dialogueOkIconName, typeof(Texture2D));
				}
				this.tmpIcon = (Texture2D)EditorGUILayout.ObjectField("OK icon", this.tmpIcon, typeof(Texture2D), false);
				if(this.tmpIcon) DataHolder.GameSettings().dialogueOkIconName = this.tmpIcon.name;
				else DataHolder.GameSettings().dialogueOkIconName = "";
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				DataHolder.GameSettings().dialogueOkPosition = (ButtonPosition)EditorGUILayout.EnumPopup("Position", 
						DataHolder.GameSettings().dialogueOkPosition, GUILayout.Width(pw.mWidth));
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold12 = EditorGUILayout.Foldout(fold12, "Statistic settings");
			if(fold12)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical();
				GUILayout.Label("Enemy statistics", EditorStyles.boldLabel);
				DataHolder.Statistic.logKilledEnemies = EditorGUILayout.Toggle("Total killed", 
						DataHolder.Statistic.logKilledEnemies, GUILayout.Width(pw.mWidth));
				DataHolder.Statistic.logSingleEnemies = EditorGUILayout.Toggle("Single killed", 
						DataHolder.Statistic.logSingleEnemies, GUILayout.Width(pw.mWidth));
				EditorGUILayout.EndVertical();
				
				EditorGUILayout.BeginVertical();
				GUILayout.Label("Item statistics", EditorStyles.boldLabel);
				DataHolder.Statistic.logUsedItems = EditorGUILayout.Toggle("Total used", 
						DataHolder.Statistic.logUsedItems, GUILayout.Width(pw.mWidth));
				DataHolder.Statistic.logSingleItems = EditorGUILayout.Toggle("Single used", 
						DataHolder.Statistic.logSingleItems, GUILayout.Width(pw.mWidth));
				DataHolder.Statistic.logCreatedItems = EditorGUILayout.Toggle("Total created", 
						DataHolder.Statistic.logCreatedItems, GUILayout.Width(pw.mWidth));
				DataHolder.Statistic.logSingleCreated = EditorGUILayout.Toggle("Single created", 
						DataHolder.Statistic.logSingleCreated, GUILayout.Width(pw.mWidth));
				EditorGUILayout.EndVertical();
				
				if(!DataHolder.BattleSystem().IsRealTime())
				{
					EditorGUILayout.BeginVertical();
					GUILayout.Label("Battle statistics", EditorStyles.boldLabel);
					DataHolder.Statistic.logBattles = EditorGUILayout.Toggle("Total battles", 
							DataHolder.Statistic.logBattles, GUILayout.Width(pw.mWidth));
					DataHolder.Statistic.logWonBattles = EditorGUILayout.Toggle("Won battles", 
							DataHolder.Statistic.logWonBattles, GUILayout.Width(pw.mWidth));
					DataHolder.Statistic.logLostBattles = EditorGUILayout.Toggle("Lost battles", 
							DataHolder.Statistic.logLostBattles, GUILayout.Width(pw.mWidth));
					DataHolder.Statistic.logEscapedBattles = EditorGUILayout.Toggle("Escaped battles", 
							DataHolder.Statistic.logEscapedBattles, GUILayout.Width(pw.mWidth));
					EditorGUILayout.EndVertical();
				}
				
				EditorGUILayout.BeginVertical();
				GUILayout.Label("General statistics", EditorStyles.boldLabel);
				DataHolder.Statistic.logCustom = EditorGUILayout.Toggle("Custom", 
						DataHolder.Statistic.logCustom, GUILayout.Width(pw.mWidth));
				EditorGUILayout.EndVertical();
				
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			
			EditorGUILayout.BeginVertical("box");
			fold13 = EditorGUILayout.Foldout(fold13, "Game over settings");
			if(fold13)
			{
				DataHolder.GameSettings().gameOver.scene = EditorGUILayout.TextField("Load scene", 
						DataHolder.GameSettings().gameOver.scene, GUILayout.Width(pw.mWidth*2));
				EditorGUILayout.Separator();
				
				DataHolder.GameSettings().gameOver.showChoice = EditorGUILayout.Toggle("Show choice", 
						DataHolder.GameSettings().gameOver.showChoice, GUILayout.Width(pw.mWidth));
				if(DataHolder.GameSettings().gameOver.showChoice)
				{
					DataHolder.GameSettings().gameOver.dialoguePosition = EditorGUILayout.Popup("Position", 
							DataHolder.GameSettings().gameOver.dialoguePosition, DataHolder.DialoguePositions().GetNameList(true), 
							GUILayout.Width(pw.mWidth*1.5f));
					DataHolder.GameSettings().gameOver.waitTime = EditorGUILayout.FloatField("Show after (s)", 
							DataHolder.GameSettings().gameOver.waitTime, GUILayout.Width(pw.mWidth));
					EditorGUILayout.Separator();
					
					EditorGUILayout.BeginVertical("box");
					DataHolder.GameSettings().gameOver.showRetry = EditorGUILayout.Toggle("Show retry", 
							DataHolder.GameSettings().gameOver.showRetry, GUILayout.Width(pw.mWidth));
					if(DataHolder.GameSettings().gameOver.showRetry)
					{
						EditorHelper.TextIconSettings(ref DataHolder.GameSettings().gameOver.retryText, 
								"Retry icon", ref DataHolder.GameSettings().gameOver.retryIconName, 
								ref DataHolder.GameSettings().gameOver.retryIcon, GameSettingsData.ICON_PATH);
					}
					EditorGUILayout.EndVertical();
					
					EditorGUILayout.BeginVertical("box");
					DataHolder.GameSettings().gameOver.showLoad = EditorGUILayout.Toggle("Show load", 
							DataHolder.GameSettings().gameOver.showLoad, GUILayout.Width(pw.mWidth));
					if(DataHolder.GameSettings().gameOver.showLoad)
					{
						EditorHelper.TextIconSettings(ref DataHolder.GameSettings().gameOver.loadText, 
								"Load icon", ref DataHolder.GameSettings().gameOver.loadIconName, 
								ref DataHolder.GameSettings().gameOver.loadIcon, GameSettingsData.ICON_PATH);
					}
					EditorGUILayout.EndVertical();
					
					EditorGUILayout.BeginVertical("box");
					DataHolder.GameSettings().gameOver.showExit = EditorGUILayout.Toggle("Show exit", 
							DataHolder.GameSettings().gameOver.showExit, GUILayout.Width(pw.mWidth));
					if(DataHolder.GameSettings().gameOver.showExit)
					{
						EditorHelper.TextIconSettings(ref DataHolder.GameSettings().gameOver.exitText, 
								"Exit icon", ref DataHolder.GameSettings().gameOver.exitIconName, 
								ref DataHolder.GameSettings().gameOver.exitIcon, GameSettingsData.ICON_PATH);
					}
					EditorGUILayout.EndVertical();
				}
				
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical("box");
			fold2 = EditorGUILayout.Foldout(fold2, "Div. settings");
			if(fold2)
			{
				DataHolder.GameSettings().showAreaNames = EditorGUILayout.Toggle("Show area names", 
						DataHolder.GameSettings().showAreaNames, GUILayout.Width(pw.mWidth));
				if(DataHolder.GameSettings().showAreaNames)
				{
					DataHolder.GameSettings().areaNamePosition = EditorGUILayout.Popup("Area name position", 
							DataHolder.GameSettings().areaNamePosition, 
							DataHolder.DialoguePositions().GetNameList(true), GUILayout.Width(pw.mWidth));
					DataHolder.GameSettings().areaNameVisibleTime = EditorGUILayout.FloatField("Visibility time", 
							DataHolder.GameSettings().areaNameVisibleTime, GUILayout.Width(pw.mWidth));
				}
				EditorGUILayout.Separator();
				
				GUILayout.Label("Chance settings", EditorStyles.boldLabel);
				DataHolder.GameSettings().minRandomRange = EditorGUILayout.FloatField("Min. random range", 
						DataHolder.GameSettings().minRandomRange, GUILayout.Width(pw.mWidth));
				DataHolder.GameSettings().maxRandomRange = EditorGUILayout.FloatField("Max. random range", 
						DataHolder.GameSettings().maxRandomRange, GUILayout.Width(pw.mWidth));
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.Separator();
			EditorGUILayout.BeginVertical("box", GUILayout.MinWidth(600));
			fold4 = EditorGUILayout.Foldout(fold4, "Audio settings");
			if(fold4)
			{
				EditorGUILayout.BeginHorizontal();
				EditorHelper.AudioSettings("Cursor move", ref DataHolder.GameSettings().cursorMoveAudio, 
						ref DataHolder.GameSettings().cursorMoveClip, GameSettingsData.AUDIO_PATH);
				DataHolder.GameSettings().cursorMoveVolume = EditorGUILayout.FloatField("Volume", 
						DataHolder.GameSettings().cursorMoveVolume, GUILayout.Width(pw.mWidth*0.7f));
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				EditorHelper.AudioSettings("Accept", ref DataHolder.GameSettings().acceptAudio, 
						ref DataHolder.GameSettings().acceptClip, GameSettingsData.AUDIO_PATH);
				DataHolder.GameSettings().acceptVolume = EditorGUILayout.FloatField("Volume", 
						DataHolder.GameSettings().acceptVolume, GUILayout.Width(pw.mWidth*0.7f));
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				EditorHelper.AudioSettings("Cancel", ref DataHolder.GameSettings().cancelAudio, 
						ref DataHolder.GameSettings().cancelClip, GameSettingsData.AUDIO_PATH);
				DataHolder.GameSettings().cancelVolume = EditorGUILayout.FloatField("Volume", 
						DataHolder.GameSettings().cancelVolume, GUILayout.Width(pw.mWidth*0.7f));
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				EditorHelper.AudioSettings("Fail", ref DataHolder.GameSettings().failAudio, 
						ref DataHolder.GameSettings().failClip, GameSettingsData.AUDIO_PATH);
				DataHolder.GameSettings().failVolume = EditorGUILayout.FloatField("Volume", 
						DataHolder.GameSettings().failVolume, GUILayout.Width(pw.mWidth*0.7f));
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				EditorHelper.AudioSettings("Skill level change", ref DataHolder.GameSettings().skillLevelAudio, 
						ref DataHolder.GameSettings().skillLevelClip, GameSettingsData.AUDIO_PATH);
				DataHolder.GameSettings().skillLevelVolume = EditorGUILayout.FloatField("Volume", 
						DataHolder.GameSettings().skillLevelVolume, GUILayout.Width(pw.mWidth*0.7f));
				EditorGUILayout.EndHorizontal();
				
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical("box");
			fold5 = EditorGUILayout.Foldout(fold5, "Item collection settings");
			if(fold5)
			{
				DataHolder.GameSettings().itemCollectionPosition = EditorGUILayout.Popup("Position", DataHolder.GameSettings().itemCollectionPosition, 
						DataHolder.DialoguePositions().GetNameList(true), GUILayout.Width(pw.mWidth));
				DataHolder.GameSettings().itemCollectionChoice = EditorGUILayout.Toggle("Choice",
						DataHolder.GameSettings().itemCollectionChoice, GUILayout.Width(pw.mWidth));
				DataHolder.GameSettings().itemCollectionAnimation = EditorGUILayout.TextField("Play animation", 
						DataHolder.GameSettings().itemCollectionAnimation, GUILayout.Width(pw.mWidth*2));
				
				GUILayout.Label("% = item number, %n = item name");
				for(int i=0; i<DataHolder.GameSettings().itemCollectionText.Length; i++)
				{
					GUILayout.Label(DataHolder.Languages().GetName(i), EditorStyles.boldLabel);
					DataHolder.GameSettings().itemCollectionText[i] = EditorGUILayout.TextField("Info text", 
							DataHolder.GameSettings().itemCollectionText[i], GUILayout.Width(pw.mWidth*2));
					if(DataHolder.GameSettings().itemCollectionChoice)
					{
						DataHolder.GameSettings().itemCollectionYesText[i] = EditorGUILayout.TextField("Ok", 
								DataHolder.GameSettings().itemCollectionYesText[i], GUILayout.Width(pw.mWidth*2));
						DataHolder.GameSettings().itemCollectionNoText[i] = EditorGUILayout.TextField("Cancel", 
								DataHolder.GameSettings().itemCollectionNoText[i], GUILayout.Width(pw.mWidth*2));
					}
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold6 = EditorGUILayout.Foldout(fold6, "Drop settings");
			if(fold6)
			{
				DataHolder.GameSettings().saveDrops = EditorGUILayout.Toggle("Save position", 
						DataHolder.GameSettings().saveDrops, GUILayout.Width(pw.mWidth));
				DataHolder.GameSettings().dropOnGround = EditorGUILayout.Toggle("Drop on ground", 
						DataHolder.GameSettings().dropOnGround, GUILayout.Width(pw.mWidth));
				if(DataHolder.GameSettings().dropOnGround)
				{
					DataHolder.GameSettings().dropMask = EditorGUILayout.LayerField("Layer mask", 
							DataHolder.GameSettings().dropMask, GUILayout.Width(pw.mWidth));
				}
				DataHolder.GameSettings().dropOffsetX = EditorGUILayout.Vector2Field("Offset X", 
						DataHolder.GameSettings().dropOffsetX, GUILayout.Width(pw.mWidth));
				DataHolder.GameSettings().dropOffsetY = EditorGUILayout.Vector2Field("Offset Y", 
						DataHolder.GameSettings().dropOffsetY, GUILayout.Width(pw.mWidth));
				DataHolder.GameSettings().dropOffsetZ = EditorGUILayout.Vector2Field("Offset Z", 
						DataHolder.GameSettings().dropOffsetZ, GUILayout.Width(pw.mWidth));
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical("box");
			fold7 = EditorGUILayout.Foldout(fold7, "Money settings");
			if(fold7)
			{
				GUILayout.Label("Text settings", EditorStyles.boldLabel);
				DataHolder.GameSettings().moneyText = EditorHelper.CheckLanguageCount(
						DataHolder.GameSettings().moneyText, langs);
				
				GUILayout.Label("Use % for current money from inventory");
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical();
				for(int i=0; i<langs; i++)
				{
					DataHolder.GameSettings().moneyText[i] = EditorGUILayout.TextField(DataHolder.Languages().GetName(i), 
							DataHolder.GameSettings().moneyText[i], GUILayout.Width(pw.mWidth*1.2f));
				}
				EditorGUILayout.EndVertical();
				if(this.tmpIcon2 == null && 
					DataHolder.GameSettings().moneyIconName != null &&
					"" != DataHolder.GameSettings().moneyIconName)
				{
					this.tmpIcon2 = (Texture2D)Resources.Load(GameSettingsData.ICON_PATH+
							DataHolder.GameSettings().moneyIconName, typeof(Texture2D));
				}
				this.tmpIcon2 = (Texture2D)EditorGUILayout.ObjectField("Icon", this.tmpIcon2, typeof(Texture2D), false);
				if(this.tmpIcon2) DataHolder.GameSettings().moneyIconName = this.tmpIcon2.name;
				else DataHolder.GameSettings().moneyIconName = "";
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Separator();
				
				GUILayout.Label("Money collection settings", EditorStyles.boldLabel);
				if(DataHolder.GameSettings().moneyPrefab == null && 
					"" != DataHolder.GameSettings().moneyPrefabName)
				{
					DataHolder.GameSettings().moneyPrefab = (GameObject)Resources.Load(DataHolder.Items().GetPrefabPath()+
							DataHolder.GameSettings().moneyPrefabName, typeof(GameObject));
				}
				DataHolder.GameSettings().moneyPrefab = (GameObject)EditorGUILayout.ObjectField("Prefab", 
						DataHolder.GameSettings().moneyPrefab, typeof(GameObject), false, GUILayout.Width(pw.mWidth*2));
				if(DataHolder.GameSettings().moneyPrefab) 
				{
					DataHolder.GameSettings().moneyPrefabName = DataHolder.GameSettings().moneyPrefab.name;
				}
				else DataHolder.GameSettings().moneyPrefabName = "";
				EditorGUILayout.Separator();
				
				DataHolder.GameSettings().moneyCollectionChoice = EditorGUILayout.Toggle("Choice",
						DataHolder.GameSettings().moneyCollectionChoice, GUILayout.Width(pw.mWidth));
				
				GUILayout.Label("% = money amount");
				for(int i=0; i<DataHolder.GameSettings().moneyCollectionText.Length; i++)
				{
					GUILayout.Label(DataHolder.Languages().GetName(i), EditorStyles.boldLabel);
					DataHolder.GameSettings().moneyCollectionText[i] = EditorGUILayout.TextField("Info text", 
							DataHolder.GameSettings().moneyCollectionText[i], GUILayout.Width(pw.mWidth*2));
					if(DataHolder.GameSettings().moneyCollectionChoice)
					{
						DataHolder.GameSettings().moneyCollectionYesText[i] = EditorGUILayout.TextField("Ok", 
								DataHolder.GameSettings().moneyCollectionYesText[i], GUILayout.Width(pw.mWidth*2));
						DataHolder.GameSettings().moneyCollectionNoText[i] = EditorGUILayout.TextField("Cancel", 
								DataHolder.GameSettings().moneyCollectionNoText[i], GUILayout.Width(pw.mWidth*2));
					}
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold8 = EditorGUILayout.Foldout(fold8, "Time text settings");
			if(fold8)
			{
				DataHolder.GameSettings().timeText = EditorHelper.CheckLanguageCount(
					DataHolder.GameSettings().timeText, langs);
				
				GUILayout.Label("Use % for current game time");
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical();
				for(int i=0; i<langs; i++)
				{
					DataHolder.GameSettings().timeText[i] = EditorGUILayout.TextField(DataHolder.Languages().GetName(i), 
							DataHolder.GameSettings().timeText[i], GUILayout.Width(pw.mWidth*1.2f));
				}
				EditorGUILayout.EndVertical();
				if(this.tmpIcon3 == null && 
					DataHolder.GameSettings().timeIconName != null &&
					"" != DataHolder.GameSettings().timeIconName)
				{
					this.tmpIcon3 = (Texture2D)Resources.Load(GameSettingsData.ICON_PATH+
							DataHolder.GameSettings().timeIconName, typeof(Texture2D));
				}
				this.tmpIcon3 = (Texture2D)EditorGUILayout.ObjectField("Icon", this.tmpIcon3, typeof(Texture2D), false);
				if(this.tmpIcon3) DataHolder.GameSettings().timeIconName = this.tmpIcon3.name;
				else DataHolder.GameSettings().timeIconName = "";
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Separator();
			}
			EditorGUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			this.Separate();
			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
		}
	}
	
	/*
	============================================================================
	Font functions
	============================================================================
	*/
	private void InitFontSave()
	{
		DataHolder.Fonts().ClearData();
		this.fonts = new FontSaver[0];
		// dialogue skins
		for(int i=0; i<DataHolder.DialoguePositions().GetDataCount(); i++)
		{
			DataHolder.DialoguePosition(i).LoadSkins();
			// default skin
			if(this.CheckSkinFont(DataHolder.DialoguePosition(i).skin))
			{
				this.fonts = this.AddFontSaver(new FontSaver(DataHolder.DialoguePosition(i).skin.font), this.fonts);
			}
			// select skin
			if(this.CheckSkinFont(DataHolder.DialoguePosition(i).selectSkin))
			{
				this.fonts = this.AddFontSaver(new FontSaver(DataHolder.DialoguePosition(i).selectSkin.font), this.fonts);
			}
			// ok button skin
			if(this.CheckSkinFont(DataHolder.DialoguePosition(i).okSkin))
			{
				this.fonts = this.AddFontSaver(new FontSaver(DataHolder.DialoguePosition(i).okSkin.font), this.fonts);
			}
			// name box skin
			if(this.CheckSkinFont(DataHolder.DialoguePosition(i).nameSkin))
			{
				this.fonts = this.AddFontSaver(new FontSaver(DataHolder.DialoguePosition(i).nameSkin.font), this.fonts);
			}
		}
		// battle skins
		DataHolder.BattleSystemData().LoadResources();
		if(this.CheckSkinFont(DataHolder.BattleSystemData().textSkin))
		{
			this.fonts = this.AddFontSaver(new FontSaver(DataHolder.BattleSystemData().textSkin.font), this.fonts);
		}
		// hud skins
		for(int i=0; i<DataHolder.HUDs().GetDataCount(); i++)
		{
			GUISkin skin = DataHolder.HUD(i).GetSkin();
			if(this.CheckSkinFont(skin))
			{
				this.fonts = this.AddFontSaver(new FontSaver(skin.font), this.fonts);
			}
		}
		this.viewMode = 1;
	}
	
	private void SaveFontData()
	{
		for(int i=0; i<this.fonts.Length; i++)
		{
			DataHolder.Fonts().AddFont(this.fonts[i].realFontName, this.fonts[i].GetFontData());
		}
		DataHolder.Fonts().SaveData();
		this.fonts = new FontSaver[0];
		this.viewMode = 0;
	}
	
	private bool CheckSkinFont(GUISkin skin)
	{
		return skin != null && skin.font != null && !this.FontAdded(skin.font.ToString());
	}
	
	private bool FontAdded(string name)
	{
		bool found = false;
		for(int i=0; i<this.fonts.Length; i++)
		{
			if(name == this.fonts[i].realFontName)
			{
				found = true;
				break;
			}
		}
		return found;
	}
	
	private FontSaver[] AddFontSaver(FontSaver n, FontSaver[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(FontSaver str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(FontSaver)) as FontSaver[];
	}
}