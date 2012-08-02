
using System.Collections;
using UnityEngine;

public class GameSettingsData
{
	public bool encryptData = false;
	
	// general settings
	public Vector2 defaultScreen = new Vector2(1280, 800);
	public GUIScaleMode guiScaleMode = GUIScaleMode.STRETCH_TO_FILL;
	
	public float cursorTimeout = 0.25f;
	public float clickTimeout = 0.2f;
	public GameControl gameControl = GameControl.BOTH;
	
	public string pauseKey = "";
	public bool pauseTime = false;
	public bool freezePause = false;
	// yet unsaved
	public bool saveWindowDrag = true;
	
	// gui system settings
	public GUISystemType guiSystemType = GUISystemType.UNITY;
	public FilterMode guiFilterMode = FilterMode.Bilinear;
	public GUIImageStretch guiImageStretch = GUIImageStretch.BILINEAR;
	public TextureDelete battleTextureDelete = TextureDelete.NONE;
	public TextureDelete menuTextureDelete = TextureDelete.NONE;
	public float mipMapBias = 0.5f;
	public bool preloadFonts = false;
	public bool preloadBoxes = false;
	public bool preloadAreaNames = false;
	public bool noScrollbar = false;
	public bool noScrollbarThumb = false;
	public bool noAutoCollapse = false;
	public int maxClickDistance = 2;
	public float hudRefreshRate = 0.5f;
	public bool hudRefreshFrame = false;
	public bool autoRemoveLayer = false;
	
	// control handling
	public InputHandling inputHandling = InputHandling.BUTTON_DOWN;
	
	// keyboard controls
	public string acceptKey = "Accept";
	public string cancelKey = "Cancel";
	public string verticalKey = "Vertical";
	public string horizontalKey = "Horizontal";
	
	// character/player settings
	public string charPlusKey = "";
	public string charMinusKey = "";
	public bool switchPlayer = false;
	public bool switchOnlyBP = false;
	
	// party spawn
	public int maxBattleParty = 3;
	public bool spawnParty = false;
	public bool onlyInBattleArea = false;
	public bool spawnOnlyBP = false;
	public float spawnDistance = 3;
	
	// party follow
	public bool partyFollow = false;
	
	// player/camera controls
	public string[] playerComponent = new string[0];
	public bool addInteractionController = false;
	public string interactionControllerName = "";
	public string interactionControllerChildName = "";
	public Vector3 interactionControllerOffset = Vector3.zero;
	
	public PlayerControlSettings playerControlSettings = new PlayerControlSettings();
	public CameraControlSettings cameraControlSettings = new CameraControlSettings();
	
	// skill levels
	public string skillPlusKey = "";
	public string skillMinusKey = "";
	public bool loopSkillLevels = false;
	public bool addSkillLevel = false;
	public string[] skillLevelName = new string[0];
	
	// div settings
	public bool showAreaNames = false;
	public int areaNamePosition = 0;
	public float areaNameVisibleTime = 3;
	public float minRandomRange = 0;
	public float maxRandomRange = 100;
	
	public GameOverSettings gameOver = new GameOverSettings();
	
	// audio settings
	public string cursorMoveAudio = "";
	public AudioClip cursorMoveClip;
	public float cursorMoveVolume = 1.0f;
	
	public string acceptAudio = "";
	public AudioClip acceptClip;
	public float acceptVolume = 1.0f;
	
	public string cancelAudio = "";
	public AudioClip cancelClip;
	public float cancelVolume = 1.0f;
	
	public string failAudio = "";
	public AudioClip failClip;
	public float failVolume = 1.0f;
	
	public string skillLevelAudio = "";
	public AudioClip skillLevelClip;
	public float skillLevelVolume = 1.0f;
	
	// dialogue settings
	public Vector2[] dialogueOkSize = new Vector2[0];
	public string[] dialogueOkText = new string[0];
	public Vector2[] dialogueOkOffset = new Vector2[0];
	public ButtonPosition dialogueOkPosition = ButtonPosition.BOTTOM_RIGHT;
	public float scrollSpeed = 3;
	public string dialogueOkIconName = "";
	public Texture2D dialogueOkIcon;
	
	// item collector settings
	public int itemCollectionPosition = 0;
	public string itemCollectionAnimation = "";
	public string[] itemCollectionText = new string[0];
	public bool itemCollectionChoice = false;
	public string[] itemCollectionYesText = new string[0];
	public string[] itemCollectionNoText = new string[0];
	
	// item drop settings
	public bool saveDrops = false;
	public bool dropOnGround = false;
	public int dropMask = 1;
	public Vector2 dropOffsetX = new Vector2(1, -1);
	public Vector2 dropOffsetY = new Vector2(1, -1);
	public Vector2 dropOffsetZ = new Vector2(1, 1);
	
	// money
	public string[] moneyText = new string[0];
	public string moneyIconName = "";
	public Texture2D moneyIcon;
	public string moneyPrefabName = "";
	public GameObject moneyPrefab = null;
	public string[] moneyCollectionText = new string[0];
	public bool moneyCollectionChoice = false;
	public string[] moneyCollectionYesText = new string[0];
	public string[] moneyCollectionNoText = new string[0];
	
	// time
	public string[] timeText = new string[0];
	public string timeIconName = "";
	public Texture2D timeIcon;
	
	// statistics
	public GameStatistic statistic = new GameStatistic();
	
	public static string ICON_PATH = "Icons/GameSettings/";
	public static string AUDIO_PATH = "Audio/GameSettings/";
	
	// XML data
	private string dir = "ProjectSettings/";
	private string filename = "gameSettings";
	
	private static string GAMESETTINGS = "gamesettings";
	private static string GAMESETTING = "gamesetting";
	private static string DIALOGUEOK = "dialogueoktext";
	private static string DIALOGUEOKICON = "dialogueokicon";
	private static string DEFAULTSCREEN = "defaultscreen";
	private static string CURSORAUDIO = "cursoraudio";
	private static string ACCEPTAUDIO = "acceptaudio";
	private static string CANCELAUDIO = "cancelaudio";
	private static string FAILAUDIO = "failaudio";
	private static string SKILLLEVELAUDIO = "skilllevelaudio";
	private static string GAMEOVER = "gameover";
	private static string ITEMCOLLECTIONTEXT = "itemcollectiontext";
	private static string ITEMCOLLECTIONYESTEXT = "itemcollectionyestext";
	private static string ITEMCOLLECTIONNOTEXT = "itemcollectionnotext";
	private static string DROPSETTINGS = "dropsettings";
	private static string ACCEPTKEY = "acceptkey";
	private static string CANCELKEY = "cancelkey";
	private static string VERTICALKEY = "verticalkey";
	private static string HORIZONTALKEY = "horizontalkey";
	private static string MONEYTEXT = "moneytext";
	private static string MONEYICON = "moneyicon";
	private static string TIMETEXT = "timetext";
	private static string TIMEICON = "timeicon";
	private static string SKILLPLUSKEY = "skillpluskey";
	private static string SKILLMINUSKEY = "skillminuskey";
	private static string SKILLLEVELNAME = "skilllevelname";
	private static string CHARPLUSKEY = "charpluskey";
	private static string CHARMINUSKEY = "charminuskey";
	private static string PLAYERCOMPONENT = "playercomponent";
	private static string PLAYERCONTROLSETTINGS = "playercontrolsettings";
	private static string CAMERACONTROLSETTINGS = "cameracontrolsettings";
	private static string MONEYPREFAB = "moneyprefab";
	private static string MONEYCOLLECTIONTEXT = "moneycollectiontext";
	private static string MONEYCOLLECTIONYES = "moneycollectionyes";
	private static string MONEYCOLLECTIONNO = "moneycollectionno";
	private static string ITEMCOLLECTIONANIMATION = "itemcollectionanimation";
	private static string PAUSEKEY = "pausekey";
	private static string INTERACTIONCONTROLLER = "interactioncontroller";
	private static string INTERACTIONCONTROLLERCHILD = "interactioncontrollerchild";
	private static string STATISTIC = "statistic";
	private static string GAMEOVERSETTINGS = "gameoversettings";

	public GameSettingsData()
	{
		LoadData();
	}
	
	public void LoadData()
	{
		ArrayList data = XMLHandler.LoadXML(dir+filename);
		
		int langs = DataHolder.Languages().GetDataCount();
		this.moneyText = new string[langs];
		this.timeText = new string[langs];
		this.itemCollectionText = new string[langs];
		this.itemCollectionYesText = new string[langs];
		this.itemCollectionNoText = new string[langs];
		this.dialogueOkText = new string[langs];
		this.dialogueOkSize = new Vector2[langs];
		this.dialogueOkOffset = new Vector2[langs];
		this.skillLevelName = new string[langs];
		this.moneyCollectionText = new string[langs];
		this.moneyCollectionYesText = new string[langs];
		this.moneyCollectionNoText = new string[langs];
		for(int i=0; i<langs; i++)
		{
			this.moneyText[i] = "% Money";
			this.timeText[i] = "Time: %";
			this.itemCollectionText[i] = "You found % %n!";
			this.itemCollectionYesText[i] = "Take";
			this.itemCollectionNoText[i] = "Leave";
			this.dialogueOkText[i] = "Ok";
			this.dialogueOkSize[i] = new Vector2(100, 30);
			this.dialogueOkOffset[i] = new Vector2(0, 0);
			this.skillLevelName[i] = "%n Lvl %";
			this.moneyCollectionText[i] = "You found % money!";
			this.moneyCollectionYesText[i] = "Take";
			this.moneyCollectionNoText[i] = "Leave";
		}
		
		if(data.Count > 0)
		{
			foreach(Hashtable entry in data)
			{
				if(entry[XMLHandler.NODE_NAME] as string == GameSettingsData.GAMESETTINGS)
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList subs = entry[XMLHandler.NODES] as ArrayList;
						foreach(Hashtable val in subs)
						{
							if(val[XMLHandler.NODE_NAME] as string == GameSettingsData.GAMESETTING)
							{
								if(val.ContainsKey("encryptdata")) encryptData = true;
								
								if(val.ContainsKey("acceptkey")) acceptKey = val["acceptkey"] as string;
								if(val.ContainsKey("cancelkey")) cancelKey = val["cancelkey"] as string;
								if(val.ContainsKey("verticalkey")) verticalKey = val["verticalkey"] as string;
								if(val.ContainsKey("horizontalkey")) horizontalKey = val["horizontalkey"] as string;
								if(val.ContainsKey("scrollspeed")) scrollSpeed = float.Parse((string)val["scrollspeed"]);
								if(val.ContainsKey("icon")) dialogueOkIconName = val["icon"] as string;
								if(val.ContainsKey("itemcollectionpos")) itemCollectionPosition = int.Parse((string)val["itemcollectionpos"]);
								if(val.ContainsKey("pausetime")) pauseTime = true;
								if(val.ContainsKey("switchplayer")) switchPlayer = true;
								if(val.ContainsKey("switchonlybp")) switchOnlyBP = true;
								if(val.ContainsKey("freezepause")) freezePause = true;
								
								if(val.ContainsKey("minrandomrange")) minRandomRange = float.Parse((string)val["minrandomrange"]);
								if(val.ContainsKey("maxrandomrange")) maxRandomRange = float.Parse((string)val["maxrandomrange"]);
								
								maxBattleParty = int.Parse((string)val["maxactiveparty"]);
								cursorTimeout = float.Parse((string)val["cursortimeout"]);
								if(val.ContainsKey("clicktimeout"))
								{
									clickTimeout = float.Parse((string)val["clicktimeout"]);
								}
								gameControl = (GameControl)System.Enum.Parse(
										typeof(GameControl), (string)val["gamecontrol"]);
								dialogueOkPosition = (ButtonPosition)System.Enum.Parse(
										typeof(ButtonPosition), (string)val["dialogueokposition"]);
								
								if(val.ContainsKey("guisystemtype"))
								{
									guiSystemType = (GUISystemType)System.Enum.Parse(
											typeof(GUISystemType), (string)val["guisystemtype"]);
								}
								if(val.ContainsKey("guifiltermode"))
								{
									guiFilterMode = (FilterMode)System.Enum.Parse(
											typeof(FilterMode), (string)val["guifiltermode"]);
								}
								if(val.ContainsKey("guiimagestretch"))
								{
									guiImageStretch = (GUIImageStretch)System.Enum.Parse(
											typeof(GUIImageStretch), (string)val["guiimagestretch"]);
								}
								if(val.ContainsKey("battletexturedelete"))
								{
									battleTextureDelete = (TextureDelete)System.Enum.Parse(
											typeof(TextureDelete), (string)val["battletexturedelete"]);
								}
								if(val.ContainsKey("menutexturedelete"))
								{
									menuTextureDelete = (TextureDelete)System.Enum.Parse(
											typeof(TextureDelete), (string)val["menutexturedelete"]);
								}
								if(val.ContainsKey("mipmapbias")) mipMapBias = float.Parse((string)val["mipmapbias"]);
								if(val.ContainsKey("preloadfonts")) preloadFonts = true;
								if(val.ContainsKey("preloadboxes")) preloadBoxes = true;
								if(val.ContainsKey("preloadareanames")) preloadAreaNames = true;
								if(val.ContainsKey("noscrollbar")) noScrollbar = true;
								if(val.ContainsKey("noscrollbarthumb")) noScrollbarThumb = true;
								if(val.ContainsKey("noautocollapse")) noAutoCollapse = true;
								if(val.ContainsKey("hudrefreshframe")) hudRefreshFrame = true;
								if(val.ContainsKey("autoremovelayer")) autoRemoveLayer = true;
								if(val.ContainsKey("loopskilllevels")) loopSkillLevels = true;
								if(val.ContainsKey("addskilllevel")) addSkillLevel = true;
								if(val.ContainsKey("itemcollectionchoice")) itemCollectionChoice = true;
								if(val.ContainsKey("moneycollectionchoice")) moneyCollectionChoice = true;
								if(val.ContainsKey("hudrefreshrate"))
								{
									hudRefreshRate = float.Parse((string)val["hudrefreshrate"]);
								}
								if(val.ContainsKey("maxclickdistance"))
								{
									maxClickDistance = int.Parse((string)val["maxclickdistance"]);
								}
								
								if(val.ContainsKey("areanamepos"))
								{
									this.showAreaNames = true;
									this.areaNamePosition = int.Parse((string)val["areanamepos"]);
									this.areaNameVisibleTime = float.Parse((string)val["areanametime"]);
								}
								
								if(val.ContainsKey("inputhandling"))
								{
									this.inputHandling = (InputHandling)System.Enum.Parse(
											typeof(InputHandling), (string)val["inputhandling"]);
								}
								
								if(val.ContainsKey("playercomponents"))
								{
									this.playerComponent = new string[int.Parse((string)val["playercomponents"])];
								}
								
								if(val.ContainsKey("spawndistance"))
								{
									this.spawnParty = true;
									this.spawnDistance = float.Parse((string)val["spawndistance"]);
									BoolHelper.FromHashtable(val, "onlyinbattlearea", ref this.onlyInBattleArea);
									BoolHelper.FromHashtable(val, "spawnonlybp", ref this.spawnOnlyBP);
								}
								
								if(val.ContainsKey("partyfollow"))
								{
									this.partyFollow = true;
								}
								
								if(val.ContainsKey("guiscalemode"))
								{
									guiScaleMode = (GUIScaleMode)System.Enum.Parse(
											typeof(GUIScaleMode), (string)val["guiscalemode"]);
								}
								
								ArrayList s = val[XMLHandler.NODES] as ArrayList;
								foreach(Hashtable ht in s)
								{
									if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.DIALOGUEOK)
									{
										int i = int.Parse((string)ht["id"]);
										if(i < langs)
										{
											dialogueOkSize[i] = new Vector2(float.Parse((string)ht["width"]), 
													float.Parse((string)ht["height"]));
											dialogueOkOffset[i] = new Vector2(float.Parse((string)ht["x"]), 
													float.Parse((string)ht["y"]));
											dialogueOkText[i] = ht[XMLHandler.CONTENT] as string;
										}
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.DIALOGUEOKICON)
									{
										dialogueOkIconName = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.DEFAULTSCREEN)
									{
										defaultScreen.x = float.Parse((string)ht["width"]);
										defaultScreen.y = float.Parse((string)ht["height"]);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.CURSORAUDIO)
									{
										this.cursorMoveAudio = ht[XMLHandler.CONTENT] as string;
										this.cursorMoveVolume = float.Parse((string)ht["volume"]);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.ACCEPTAUDIO)
									{
										this.acceptAudio = ht[XMLHandler.CONTENT] as string;
										this.acceptVolume = float.Parse((string)ht["volume"]);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.CANCELAUDIO)
									{
										this.cancelAudio = ht[XMLHandler.CONTENT] as string;
										this.cancelVolume = float.Parse((string)ht["volume"]);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.FAILAUDIO)
									{
										this.failAudio = ht[XMLHandler.CONTENT] as string;
										this.failVolume = float.Parse((string)ht["volume"]);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.SKILLLEVELAUDIO)
									{
										this.skillLevelAudio = ht[XMLHandler.CONTENT] as string;
										this.skillLevelVolume = float.Parse((string)ht["volume"]);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.GAMEOVER)
									{
										this.gameOver.scene = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.ITEMCOLLECTIONTEXT)
									{
										int i = int.Parse((string)ht["id"]);
										if(i < langs) this.itemCollectionText[i] = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.ITEMCOLLECTIONYESTEXT)
									{
										int i = int.Parse((string)ht["id"]);
										if(i < langs) this.itemCollectionYesText[i] = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.ITEMCOLLECTIONNOTEXT)
									{
										int i = int.Parse((string)ht["id"]);
										if(i < langs) this.itemCollectionNoText[i] = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.DROPSETTINGS)
									{
										this.saveDrops = bool.Parse((string)ht["save"]);
										if(ht.ContainsKey("dropmask"))
										{
											this.dropOnGround = true;
											this.dropMask = int.Parse((string)ht["dropmask"]);
										}
										this.dropOffsetX = new Vector2(float.Parse((string)ht["x1"]), float.Parse((string)ht["x2"]));
										this.dropOffsetY = new Vector2(float.Parse((string)ht["y1"]), float.Parse((string)ht["y2"]));
										this.dropOffsetZ = new Vector2(float.Parse((string)ht["z1"]), float.Parse((string)ht["z2"]));
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.ACCEPTKEY)
									{
										this.acceptKey = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.CANCELKEY)
									{
										this.cancelKey = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.VERTICALKEY)
									{
										this.verticalKey = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.HORIZONTALKEY)
									{
										this.horizontalKey = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.MONEYTEXT)
									{
										int i = int.Parse((string)ht["id"]);
										if(i < langs) this.moneyText[i] = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.MONEYICON)
									{
										this.moneyIconName = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.TIMETEXT)
									{
										int i = int.Parse((string)ht["id"]);
										if(i < langs) this.timeText[i] = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.TIMEICON)
									{
										this.timeIconName = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.SKILLPLUSKEY)
									{
										this.skillPlusKey = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.SKILLMINUSKEY)
									{
										this.skillMinusKey = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.SKILLLEVELNAME)
									{
										int i = int.Parse((string)ht["id"]);
										if(i < langs) this.skillLevelName[i] = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.CHARPLUSKEY)
									{
										this.charPlusKey = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.CHARMINUSKEY)
									{
										this.charMinusKey = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.PLAYERCOMPONENT)
									{
										int i = int.Parse((string)ht["id"]);
										if(i < this.playerComponent.Length) this.playerComponent[i] = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.PLAYERCONTROLSETTINGS)
									{
										this.playerControlSettings.SetData(ht);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.CAMERACONTROLSETTINGS)
									{
										this.cameraControlSettings.SetData(ht);
										this.cameraControlSettings.mouseTouch.mode = MouseTouch.MOVE;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.MONEYPREFAB)
									{
										this.moneyPrefabName = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.MONEYCOLLECTIONTEXT)
									{
										int i = int.Parse((string)ht["id"]);
										if(i < langs) this.moneyCollectionText[i] = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.MONEYCOLLECTIONYES)
									{
										int i = int.Parse((string)ht["id"]);
										if(i < langs) this.moneyCollectionYesText[i] = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.MONEYCOLLECTIONNO)
									{
										int i = int.Parse((string)ht["id"]);
										if(i < langs) this.moneyCollectionNoText[i] = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.ITEMCOLLECTIONANIMATION)
									{
										this.itemCollectionAnimation = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.PAUSEKEY)
									{
										this.pauseKey = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.INTERACTIONCONTROLLER)
									{
										this.addInteractionController = true;
										this.interactionControllerName = ht[XMLHandler.CONTENT] as string;
										this.interactionControllerOffset = VectorHelper.FromHashtable(ht);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.INTERACTIONCONTROLLERCHILD)
									{
										this.interactionControllerChildName = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.STATISTIC)
									{
										this.statistic.SetData(ht);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == GameSettingsData.GAMEOVERSETTINGS)
									{
										this.gameOver.SetData(ht);
									}
								}
							}
						}
					}
				}
			}
		}
	}
	
	public void SaveData()
	{
		ArrayList data = new ArrayList();
		ArrayList subs  = new ArrayList();
		Hashtable sv = new Hashtable();
		
		sv.Add(XMLHandler.NODE_NAME, GameSettingsData.GAMESETTINGS);
		
		Hashtable ht = new Hashtable();
		ArrayList s = new ArrayList();
		
		ht.Add(XMLHandler.NODE_NAME, GameSettingsData.GAMESETTING);
		if(encryptData) ht.Add("encryptdata", "true");
		ht.Add("langs", dialogueOkText.Length.ToString());
		ht.Add("cursortimeout", cursorTimeout.ToString());
		ht.Add("clicktimeout", clickTimeout.ToString());
		ht.Add("gamecontrol", gameControl.ToString());
		ht.Add("dialogueokposition", dialogueOkPosition.ToString());
		ht.Add("maxactiveparty", maxBattleParty.ToString());
		ht.Add("scrollspeed", scrollSpeed.ToString());
		ht.Add("minrandomrange", minRandomRange.ToString());
		ht.Add("maxrandomrange", maxRandomRange.ToString());
		ht.Add("guisystemtype", guiSystemType.ToString());
		ht.Add("guifiltermode", guiFilterMode.ToString());
		ht.Add("guiimagestretch", guiImageStretch.ToString());
		ht.Add("maxclickdistance", maxClickDistance.ToString());
		ht.Add("hudrefreshrate", hudRefreshRate.ToString());
		ht.Add("mipmapbias", mipMapBias.ToString());
		ht.Add("battletexturedelete", battleTextureDelete.ToString());
		ht.Add("menutexturedelete", menuTextureDelete.ToString());
		ht.Add("inputhandling", inputHandling.ToString());
		ht.Add("guiscalemode", guiScaleMode.ToString());
		if(preloadFonts) ht.Add("preloadfonts", "true");
		if(preloadBoxes) ht.Add("preloadboxes", "true");
		if(preloadAreaNames) ht.Add("preloadareanames", "true");
		if(noScrollbar) ht.Add("noscrollbar", "true");
		if(noScrollbarThumb) ht.Add("noscrollbarthumb", "true");
		if(noAutoCollapse) ht.Add("noautocollapse", "true");
		if(hudRefreshFrame) ht.Add("hudrefreshframe", "true");
		if(autoRemoveLayer) ht.Add("autoremovelayer", "true");
		if(loopSkillLevels) ht.Add("loopskilllevels", "true");
		if(addSkillLevel) ht.Add("addskilllevel", "true");
		if(itemCollectionChoice) ht.Add("itemcollectionchoice", "true");
		if(moneyCollectionChoice) ht.Add("moneycollectionchoice", "true");
		if(switchPlayer) ht.Add("switchplayer", "true");
		if(switchOnlyBP) ht.Add("switchonlybp", "true");
		if(pauseTime) ht.Add("pausetime", "true");
		if(freezePause) ht.Add("freezepause", "true");
		
		if(this.spawnParty)
		{
			ht.Add("spawndistance", this.spawnDistance.ToString());
			if(this.spawnOnlyBP) ht.Add("spawnonlybp", "true");
			if(this.onlyInBattleArea) ht.Add("onlyinbattlearea", "true");
		}
		
		if("" != dialogueOkIconName)
		{
			Hashtable ht2 = new Hashtable();
			ht2.Add(XMLHandler.NODE_NAME, GameSettingsData.DIALOGUEOKICON);
			ht2.Add(XMLHandler.CONTENT, dialogueOkIconName);
			s.Add(ht2);
		}
		if(this.showAreaNames)
		{
			ht.Add("areanamepos", this.areaNamePosition.ToString());
			ht.Add("areanametime", this.areaNameVisibleTime.ToString());
		}
		
		ht.Add("itemcollectionpos", itemCollectionPosition.ToString());
		if(itemCollectionAnimation != "")
		{
			s.Add(HashtableHelper.GetContentHashtable(
					GameSettingsData.ITEMCOLLECTIONANIMATION, itemCollectionAnimation));
		}
		for(int i=0; i<itemCollectionText.Length; i++)
		{
			s.Add(HashtableHelper.GetContentHashtable(
					GameSettingsData.ITEMCOLLECTIONTEXT, itemCollectionText[i], i));
			s.Add(HashtableHelper.GetContentHashtable(
					GameSettingsData.ITEMCOLLECTIONYESTEXT, itemCollectionYesText[i], i));
			s.Add(HashtableHelper.GetContentHashtable(
					GameSettingsData.ITEMCOLLECTIONNOTEXT, itemCollectionNoText[i], i));
		}
		for(int i=0; i<dialogueOkText.Length; i++)
		{
			Hashtable ht2 = new Hashtable();
			ht2.Add(XMLHandler.NODE_NAME, GameSettingsData.DIALOGUEOK);
			ht2.Add("id", i.ToString());
			ht2.Add(XMLHandler.CONTENT, dialogueOkText[i]);
			ht2.Add("width", dialogueOkSize[i].x.ToString());
			ht2.Add("height", dialogueOkSize[i].y.ToString());
			ht2.Add("x", dialogueOkOffset[i].x.ToString());
			ht2.Add("y", dialogueOkOffset[i].y.ToString());
			s.Add(ht2);
		}
		
		Hashtable n = new Hashtable();
		n.Add(XMLHandler.NODE_NAME, GameSettingsData.DEFAULTSCREEN);
		n.Add("width", defaultScreen.x.ToString());
		n.Add("height", defaultScreen.y.ToString());
		s.Add(n);
		
		if("" != this.cursorMoveAudio)
		{
			n = HashtableHelper.GetContentHashtable(GameSettingsData.CURSORAUDIO, this.cursorMoveAudio);
			n.Add("volume", this.cursorMoveVolume.ToString());
			s.Add(n);
		}
		if("" != this.acceptAudio)
		{
			n = HashtableHelper.GetContentHashtable(GameSettingsData.ACCEPTAUDIO, this.acceptAudio);
			n.Add("volume", this.acceptVolume.ToString());
			s.Add(n);
		}
		if("" != this.cancelAudio)
		{
			n = HashtableHelper.GetContentHashtable(GameSettingsData.CANCELAUDIO, this.cancelAudio);
			n.Add("volume", this.cancelVolume.ToString());
			s.Add(n);
		}
		if("" != this.failAudio)
		{
			n = HashtableHelper.GetContentHashtable(GameSettingsData.FAILAUDIO, this.failAudio);
			n.Add("volume", this.failVolume.ToString());
			s.Add(n);
		}
		if("" != this.skillLevelAudio)
		{
			n = HashtableHelper.GetContentHashtable(GameSettingsData.SKILLLEVELAUDIO, this.skillLevelAudio);
			n.Add("volume", this.skillLevelVolume.ToString());
			s.Add(n);
		}
		
		n = new Hashtable();
		n.Add(XMLHandler.NODE_NAME, GameSettingsData.DROPSETTINGS);
		n.Add("save", this.saveDrops.ToString());
		if(this.dropOnGround) n.Add("dropmask", this.dropMask.ToString());
		n.Add("x1", this.dropOffsetX.x.ToString());
		n.Add("x2", this.dropOffsetX.y.ToString());
		n.Add("y1", this.dropOffsetY.x.ToString());
		n.Add("y2", this.dropOffsetY.y.ToString());
		n.Add("z1", this.dropOffsetZ.x.ToString());
		n.Add("z2", this.dropOffsetZ.y.ToString());
		s.Add(n);
		
		s.Add(HashtableHelper.GetContentHashtable(GameSettingsData.ACCEPTKEY, this.acceptKey));
		s.Add(HashtableHelper.GetContentHashtable(GameSettingsData.CANCELKEY, this.cancelKey));
		s.Add(HashtableHelper.GetContentHashtable(GameSettingsData.VERTICALKEY, this.verticalKey));
		s.Add(HashtableHelper.GetContentHashtable(GameSettingsData.HORIZONTALKEY, this.horizontalKey));
		s.Add(HashtableHelper.GetContentHashtable(GameSettingsData.SKILLPLUSKEY, this.skillPlusKey));
		s.Add(HashtableHelper.GetContentHashtable(GameSettingsData.SKILLMINUSKEY, this.skillMinusKey));
		s.Add(HashtableHelper.GetContentHashtable(GameSettingsData.CHARPLUSKEY, this.charPlusKey));
		s.Add(HashtableHelper.GetContentHashtable(GameSettingsData.CHARMINUSKEY, this.charMinusKey));
		s.Add(HashtableHelper.GetContentHashtable(GameSettingsData.PAUSEKEY, this.pauseKey));
		
		if("" != this.moneyIconName)
		{
			s.Add(HashtableHelper.GetContentHashtable(GameSettingsData.MONEYICON, this.moneyIconName));
		}
		for(int i=0; i<this.moneyText.Length; i++)
		{
			s.Add(HashtableHelper.GetContentHashtable(GameSettingsData.MONEYTEXT, this.moneyText[i], i));
		}
		if("" != this.timeIconName)
		{
			s.Add(HashtableHelper.GetContentHashtable(GameSettingsData.TIMEICON, this.timeIconName));
		}
		for(int i=0; i<this.timeText.Length; i++)
		{
			s.Add(HashtableHelper.GetContentHashtable(GameSettingsData.TIMETEXT, this.timeText[i], i));
		}
		for(int i=0; i<this.skillLevelName.Length; i++)
		{
			s.Add(HashtableHelper.GetContentHashtable(GameSettingsData.SKILLLEVELNAME, this.skillLevelName[i], i));
		}
		
		if(this.playerComponent.Length > 0)
		{
			ht.Add("playercomponents", this.playerComponent.Length.ToString());
			for(int i=0; i<this.playerComponent.Length; i++)
			{
				s.Add(HashtableHelper.GetContentHashtable(GameSettingsData.PLAYERCOMPONENT, this.playerComponent[i], i));
			}
		}
		
		s.Add(this.playerControlSettings.GetData(HashtableHelper.GetTitleHashtable(GameSettingsData.PLAYERCONTROLSETTINGS)));
		s.Add(this.cameraControlSettings.GetData(HashtableHelper.GetTitleHashtable(GameSettingsData.CAMERACONTROLSETTINGS)));
		if(this.partyFollow) ht.Add("partyfollow", "true");
		
		s.Add(HashtableHelper.GetContentHashtable(GameSettingsData.MONEYPREFAB, this.moneyPrefabName));
		for(int i=0; i<this.moneyCollectionText.Length; i++)
		{
			s.Add(HashtableHelper.GetContentHashtable(
					GameSettingsData.MONEYCOLLECTIONTEXT, this.moneyCollectionText[i], i));
			s.Add(HashtableHelper.GetContentHashtable(
					GameSettingsData.MONEYCOLLECTIONYES, this.moneyCollectionYesText[i], i));
			s.Add(HashtableHelper.GetContentHashtable(
					GameSettingsData.MONEYCOLLECTIONNO, this.moneyCollectionNoText[i], i));
		}
		
		if(this.addInteractionController)
		{
			Hashtable ht2 = HashtableHelper.GetContentHashtable(
					GameSettingsData.INTERACTIONCONTROLLER, this.interactionControllerName);
			VectorHelper.ToHashtable(ref ht2, this.interactionControllerOffset);
			s.Add(ht2);
			if(this.interactionControllerChildName != "")
			{
				s.Add(HashtableHelper.GetContentHashtable(
					GameSettingsData.INTERACTIONCONTROLLERCHILD, this.interactionControllerChildName));
			}
		}
		
		s.Add(this.statistic.GetData(HashtableHelper.GetTitleHashtable(
					GameSettingsData.STATISTIC)));
		
		s.Add(this.gameOver.GetData(HashtableHelper.GetTitleHashtable(
					GameSettingsData.GAMEOVERSETTINGS)));
		
		ht.Add(XMLHandler.NODES, s);
		subs.Add(ht);
		sv.Add(XMLHandler.NODES, subs);
		data.Add(sv);
		XMLHandler.SaveXML(dir, filename, data);
	}
	
	public void AddLanguage(int lang)
	{
		dialogueOkText = ArrayHelper.Add("Ok", dialogueOkText);
		itemCollectionText = ArrayHelper.Add("You found % %n!", itemCollectionText);
		itemCollectionYesText = ArrayHelper.Add("Take", itemCollectionYesText);
		itemCollectionNoText = ArrayHelper.Add("Leave", itemCollectionNoText);
		dialogueOkSize = ArrayHelper.Add(new Vector2(100, 30), dialogueOkSize);
		dialogueOkOffset = ArrayHelper.Add(new Vector2(0, 0), dialogueOkOffset);
		moneyText = ArrayHelper.Add("% Money", moneyText);
		timeText = ArrayHelper.Add("Time: %", timeText);
		skillLevelName = ArrayHelper.Add("%n Lvl %", skillLevelName);
		moneyCollectionText = ArrayHelper.Add("You found % money!", moneyCollectionText);
		moneyCollectionYesText = ArrayHelper.Add("Take", moneyCollectionYesText);
		moneyCollectionNoText = ArrayHelper.Add("Leave", moneyCollectionNoText);
	}
	
	public void RemoveLanguage(int lang)
	{
		dialogueOkText = ArrayHelper.Remove(lang, dialogueOkText);
		itemCollectionText = ArrayHelper.Remove(lang, itemCollectionText);
		itemCollectionYesText = ArrayHelper.Remove(lang, itemCollectionYesText);
		itemCollectionNoText = ArrayHelper.Remove(lang, itemCollectionNoText);
		dialogueOkSize = ArrayHelper.Remove(lang, dialogueOkSize);
		dialogueOkOffset = ArrayHelper.Remove(lang, dialogueOkOffset);
		moneyText = ArrayHelper.Remove(lang, moneyText);
		timeText = ArrayHelper.Remove(lang, timeText);
		skillLevelName = ArrayHelper.Remove(lang, skillLevelName);
		moneyCollectionText = ArrayHelper.Remove(lang, moneyCollectionText);
		moneyCollectionYesText = ArrayHelper.Remove(lang, moneyCollectionYesText);
		moneyCollectionNoText = ArrayHelper.Remove(lang, moneyCollectionNoText);
	}
	
	public bool IsMouseAllowed()
	{
		bool allowed = false;
		if(GameControl.BOTH.Equals(this.gameControl) ||
			GameControl.MOUSE.Equals(this.gameControl))
		{
			allowed = true;
		}
		return allowed;
	}
	
	public bool IsKeyboardAllowed()
	{
		bool allowed = false;
		if(GameControl.BOTH.Equals(this.gameControl) ||
			GameControl.KEYBOARD.Equals(this.gameControl))
		{
			allowed = true;
		}
		return allowed;
	}
	
	public GUIContent GetOkButtonContent()
	{
		return new GUIContent(this.dialogueOkText[GameHandler.GetLanguage()], this.dialogueOkIcon);
	}
	
	private string GetItemCollectionReplace(string txt, int id, int n, ItemDropType t)
	{
		if(ItemDropType.ITEM.Equals(t)) txt = txt.Replace("%n", DataHolder.Items().GetName(id));
		else if(ItemDropType.WEAPON.Equals(t)) txt = txt.Replace("%n", DataHolder.Weapons().GetName(id));
		else if(ItemDropType.ARMOR.Equals(t)) txt = txt.Replace("%n", DataHolder.Armors().GetName(id));
		txt = txt.Replace("%", n.ToString());
		return txt;
	}
	
	public string GetItemCollectionString(int id, int n, ItemDropType t)
	{
		return this.GetItemCollectionReplace(this.itemCollectionText[GameHandler.GetLanguage()], id, n, t);
	}
	
	public string[] GetItemCollectionChoice(int id, int n, ItemDropType t)
	{
		return new string[] {
				this.GetItemCollectionReplace(this.itemCollectionYesText[GameHandler.GetLanguage()], id, n, t),
				this.GetItemCollectionReplace(this.itemCollectionNoText[GameHandler.GetLanguage()], id, n, t)};
	}
	
	public string GetMoneyCollectionString(int n)
	{
		return this.moneyCollectionText[GameHandler.GetLanguage()].Replace("%", n.ToString());
	}
	
	public string[] GetMoneyCollectionChoice(int n)
	{
		return new string[] {
				this.moneyCollectionYesText[GameHandler.GetLanguage()].Replace("%", n.ToString()),
				this.moneyCollectionNoText[GameHandler.GetLanguage()].Replace("%", n.ToString())};
	}
	
	public void LoadResources()
	{
		if("" != this.cursorMoveAudio)
		{
			this.cursorMoveClip = (AudioClip)Resources.Load(GameSettingsData.AUDIO_PATH+this.cursorMoveAudio, typeof(AudioClip));
		}
		if("" != this.acceptAudio)
		{
			this.acceptClip = (AudioClip)Resources.Load(GameSettingsData.AUDIO_PATH+this.acceptAudio, typeof(AudioClip));
		}
		if("" != this.cancelAudio)
		{
			this.cancelClip = (AudioClip)Resources.Load(GameSettingsData.AUDIO_PATH+this.cancelAudio, typeof(AudioClip));
		}
		if("" != this.failAudio)
		{
			this.failClip = (AudioClip)Resources.Load(GameSettingsData.AUDIO_PATH+this.failAudio, typeof(AudioClip));
		}
		if("" != this.skillLevelAudio)
		{
			this.skillLevelClip = (AudioClip)Resources.Load(GameSettingsData.AUDIO_PATH+this.skillLevelAudio, typeof(AudioClip));
		}
		if("" != this.dialogueOkIconName)
		{
			this.dialogueOkIcon = (Texture2D)Resources.Load(GameSettingsData.ICON_PATH+this.dialogueOkIconName, typeof(Texture2D));
		}
		if("" != this.moneyIconName)
		{
			this.moneyIcon = (Texture2D)Resources.Load(GameSettingsData.ICON_PATH+this.moneyIconName, typeof(Texture2D));
		}
		if("" != this.timeIconName)
		{
			this.timeIcon = (Texture2D)Resources.Load(GameSettingsData.ICON_PATH+this.timeIconName, typeof(Texture2D));
		}
		if("" != this.moneyPrefabName)
		{
			this.moneyPrefab = (GameObject)Resources.Load(DataHolder.Items().GetPrefabPath()+this.moneyPrefabName, typeof(GameObject));
		}
	}
	
	public void PlayCursorMoveAudio(AudioSource audio)
	{
		if(audio && this.cursorMoveClip) audio.PlayOneShot(this.cursorMoveClip, this.cursorMoveVolume);
	}
	
	public void PlayAcceptAudio(AudioSource audio)
	{
		if(audio && this.acceptClip) audio.PlayOneShot(this.acceptClip, this.acceptVolume);
	}
	
	public void PlayCancelAudio(AudioSource audio)
	{
		if(audio && this.cancelClip) audio.PlayOneShot(this.cancelClip, this.cancelVolume);
	}
	
	public void PlayFailAudio(AudioSource audio)
	{
		if(audio && this.failClip)
		{
			audio.PlayOneShot(this.failClip, this.failVolume);
		}
	}
	
	public void PlaySkillLevelAudio(AudioSource audio)
	{
		if(audio && this.skillLevelClip)
		{
			audio.PlayOneShot(this.skillLevelClip, this.skillLevelVolume);
		}
	}
	
	public float GetRandom()
	{
		return Random.Range(this.minRandomRange, this.maxRandomRange);
	}
	
	// money content
	public string GetMoneyText()
	{
		string txt = this.moneyText[GameHandler.GetLanguage()];
		txt = txt.Replace("%", GameHandler.GetMoney().ToString());
		return txt;
	}
	
	public ChoiceContent GetMoneyContent(HUDContentType type)
	{
		ChoiceContent cc = null;
		if(HUDContentType.TEXT.Equals(type))
		{
			cc = new ChoiceContent(new GUIContent(this.GetMoneyText()));
		}
		else if(HUDContentType.ICON.Equals(type))
		{
			cc = new ChoiceContent(new GUIContent(this.moneyIcon));
		}
		else if(HUDContentType.BOTH.Equals(type))
		{
			cc = new ChoiceContent(new GUIContent(this.GetMoneyText(), this.moneyIcon));
		}
		return cc;
	}
	
	public GameObject GetMoneyPrefabInstance()
	{
		GameObject go = null;
		if(this.moneyPrefab != null)
		{
			go = (GameObject)GameObject.Instantiate(this.moneyPrefab);
		}
		return go;
	}
	
	// time content
	public string GetTimeText()
	{
		string txt = this.timeText[GameHandler.GetLanguage()];
		txt = txt.Replace("%", GameHandler.GetTimeString());
		return txt;
	}
	
	public ChoiceContent GetTimeContent(HUDContentType type)
	{
		ChoiceContent cc = null;
		if(HUDContentType.TEXT.Equals(type))
		{
			cc = new ChoiceContent(new GUIContent(this.GetTimeText()));
		}
		else if(HUDContentType.ICON.Equals(type))
		{
			cc = new ChoiceContent(new GUIContent(this.timeIcon));
		}
		else if(HUDContentType.BOTH.Equals(type))
		{
			cc = new ChoiceContent(new GUIContent(this.GetTimeText(), this.timeIcon));
		}
		return cc;
	}
	
	public string GetSkillLevelName(string name, int lvl)
	{
		string txt = name;
		if(this.addSkillLevel)
		{
			txt = this.skillLevelName[GameHandler.GetLanguage()];
			txt = txt.Replace("%n", name);
			txt = txt.Replace("%", lvl.ToString());
		}
		return txt;
	}
	
	// player components
	public void AddPlayerComponent()
	{
		this.playerComponent = ArrayHelper.Add("", this.playerComponent);
	}
	
	public void RemovePlayerComponent(int index)
	{
		this.playerComponent = ArrayHelper.Remove(index, this.playerComponent);
	}
	
	public void AddInteractionController(GameObject player)
	{
		if(this.addInteractionController && player != null && 
			player.GetComponentInChildren<InteractionController>() == null)
		{
			Transform t = TransformHelper.GetChild(this.interactionControllerChildName, player.transform);
			GameObject tmp = (GameObject)Resources.Load(CharacterData.PREFAB_PATH+
					this.interactionControllerName, typeof(GameObject));
			if(tmp != null)
			{
				GameObject ic = (GameObject)GameObject.Instantiate(tmp);
				if(ic != null)
				{
					TransformHelper.Mount(t, ic.transform, true, true, 
							this.interactionControllerOffset, true, Vector3.zero);
				}
			}
		}
	}
}