
using UnityEditor;
using UnityEngine;

public class EventWindow : EditorWindow
{
	public int mWidth = 300;
	public int mWidth2 = 200;
	public int mWidth3 = 100;
	private Vector2 SP1 = new Vector2(0, 0);
	private Vector2 SP2 = new Vector2(0, 0);
	private Vector2 SP3 = new Vector2(0, 0);
	
	private int currentSection = 0;
	private string[] sections = new string[] {"Event Settings", "Event Steps"};
	
	private int SETTINGS = 0;
	private int STEPS = 1;
	
	private GameEvent gameEvent = new GameEvent();
	
	private bool showAdd = false;
	
	private string lastFile = "";
	private int addAt = 0;
	
	[MenuItem("RPG Kit/Event Editor")]
	public static void Init()
	{
		// Get existing open window or if none, make a new one:
		EventWindow window = (EventWindow)EditorWindow.GetWindow(typeof(EventWindow), false, "Event Editor");
		window.NewEvent();
		window.Show();
	}
	
	public static void Init(string openPath)
	{
		// Get existing open window or if none, make a new one:
		EventWindow window = (EventWindow)EditorWindow.GetWindow(typeof(EventWindow), false, "Event Editor");
		window.NewEvent();
		window.Show();
		window.LoadData(openPath);
	}
	
	void NewEvent()
	{
		this.lastFile = "";
		this.gameEvent = new GameEvent();
	}
	
	void LoadData(string openPath)
	{
		this.lastFile = openPath;
		this.gameEvent.LoadEventData(openPath);
	}
	
	void OnGUI()
	{
		EditorGUILayout.BeginVertical();
		GUI.SetNextControlName("Toolbar");
		var prevSection = currentSection;
		currentSection = GUILayout.SelectionGrid(currentSection, sections, 2);
		if(prevSection != currentSection)
		{
			GUI.FocusControl("Toolbar");
		}
		GUILayout.Box(" ", GUILayout.ExpandWidth(true));
		if(currentSection == this.SETTINGS)
		{
			EditorGUILayout.LabelField("File name", this.lastFile);
			gameEvent.blockControls = EditorGUILayout.Toggle("Block controls", gameEvent.blockControls, GUILayout.Width(this.mWidth));
			gameEvent.mainCamera = EditorGUILayout.Toggle("Use main camera", gameEvent.mainCamera, GUILayout.Width(this.mWidth));
			gameEvent.waypoints = EditorGUILayout.IntField("Waypoint number", gameEvent.waypoints, GUILayout.Width(this.mWidth));
			gameEvent.prefabs = EditorGUILayout.IntField("Prefab objects", gameEvent.prefabs, GUILayout.Width(this.mWidth));
			gameEvent.audioClips = EditorGUILayout.IntField("Audio clips", gameEvent.audioClips, GUILayout.Width(this.mWidth));
			EditorGUILayout.Separator();
			if(GUILayout.Button("Add Actor", GUILayout.Width(this.mWidth2)))
			{
				this.gameEvent.AddActor();
			}
			EditorGUILayout.Separator();
			SP1 = EditorGUILayout.BeginScrollView(SP1);
			for(int i=0; i<this.gameEvent.actor.Length; i++)
			{
				EditorGUILayout.BeginVertical("box");
				GameEventTabs.EventActor(i, this.gameEvent, this.gameEvent.actor[i]);
				EditorGUILayout.EndVertical();
			}
			EditorGUILayout.EndScrollView();
		}
		else if(currentSection == this.STEPS)
		{
			if(this.showAdd)
			{
				EditorGUILayout.BeginHorizontal();
				if(GUILayout.Button("Cancel", GUILayout.Width(this.mWidth2)))
				{
					this.showAdd = false;
					return;
				}
				
				this.addAt = EditorGUILayout.IntField("Add at", this.addAt, GUILayout.Width(this.mWidth));
				if(this.addAt < 0) this.addAt = 0;
				else if(this.addAt > this.gameEvent.step.Length) this.addAt = this.gameEvent.step.Length;
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				
				string[] types = System.Enum.GetNames(typeof(GameEventType));
				SP2 = EditorGUILayout.BeginScrollView(SP2);
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical();
				for(int i=0; i<types.Length; i++)
				{
					if(types[i] == "WAIT") GUILayout.Label("Base steps", EditorStyles.boldLabel);
					else if(types[i] == "SPAWN_PLAYER") GUILayout.Label("Spawn steps", EditorStyles.boldLabel);
					else if(types[i] == "SHOW_DIALOGUE") GUILayout.Label("Dialogue steps", EditorStyles.boldLabel);
					else if(types[i] == "SEND_MESSAGE") GUILayout.Label("Function steps", EditorStyles.boldLabel);
					else if(types[i] == "CUSTOM_STATISTIC") GUILayout.Label("Statistic steps", EditorStyles.boldLabel);
					else if(types[i] == "SET_CAMERA_POSITION")
					{
						EditorGUILayout.EndVertical();
						EditorGUILayout.Separator();
						EditorGUILayout.BeginVertical();
						GUILayout.Label("Camera steps", EditorStyles.boldLabel);
					}
					else if(types[i] == "SET_TO_POSITION") GUILayout.Label("Move steps", EditorStyles.boldLabel);
					else if(types[i] == "ROTATE_TO_WAYPOINT") GUILayout.Label("Rotate steps", EditorStyles.boldLabel);
					else if(types[i] == "PLAY_ANIMATION") GUILayout.Label("Animation steps", EditorStyles.boldLabel);
					else if(types[i] == "PLAY_SOUND") GUILayout.Label("Audio steps", EditorStyles.boldLabel);
					else if(types[i] == "FADE_OBJECT") GUILayout.Label("Fade steps", EditorStyles.boldLabel);
					else if(types[i] == "JOIN_PARTY")
					{
						EditorGUILayout.EndVertical();
						EditorGUILayout.Separator();
						EditorGUILayout.BeginVertical();
						GUILayout.Label("Party steps", EditorStyles.boldLabel);
					}
					else if(types[i] == "JOIN_BATTLE_PARTY") GUILayout.Label("Battle party steps", EditorStyles.boldLabel);
					else if(types[i] == "REGENERATE") GUILayout.Label("Status steps", EditorStyles.boldLabel);
					else if(types[i] == "CHECK_CLASS") GUILayout.Label("Class steps", EditorStyles.boldLabel);
					else if(types[i] == "LEARN_SKILL") GUILayout.Label("Skill steps", EditorStyles.boldLabel);
					else if(types[i] == "EQUIP_WEAPON") GUILayout.Label("Equip steps", EditorStyles.boldLabel);
					else if(types[i] == "ADD_ITEM")
					{
						EditorGUILayout.EndVertical();
						EditorGUILayout.Separator();
						EditorGUILayout.BeginVertical();
						GUILayout.Label("Item steps", EditorStyles.boldLabel);
					}
					else if(types[i] == "ADD_WEAPON") GUILayout.Label("Weapon steps", EditorStyles.boldLabel);
					else if(types[i] == "ADD_ARMOR") GUILayout.Label("Armor steps", EditorStyles.boldLabel);
					else if(types[i] == "ADD_MONEY") GUILayout.Label("Money steps", EditorStyles.boldLabel);
					else if(types[i] == "SET_VARIABLE") GUILayout.Label("Variable steps", EditorStyles.boldLabel);
					
					if(GUILayout.Button(types[i], GUILayout.Width(this.mWidth2)))
					{
						this.gameEvent.AddStep((GameEventType)System.Enum.ToObject(typeof(GameEventType), i), this.addAt);
						
						this.showAdd = false;
						break;
					}
				}
				EditorGUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndScrollView();
			}
			else
			{
				EditorGUILayout.BeginHorizontal();
				if(GUILayout.Button("Add Step", GUILayout.Width(this.mWidth2)))
				{
					this.showAdd = true;
					this.addAt = this.gameEvent.step.Length;
				}
				this.gameEvent.hideButtons = EditorGUILayout.Toggle("Hide buttons", this.gameEvent.hideButtons, GUILayout.Width(this.mWidth));
				GUILayout.FlexibleSpace();
				if(GUILayout.Button("Collapse all", GUILayout.Width(this.mWidth3)))
				{
					for(int i=0; i<this.gameEvent.step.Length; i++) this.gameEvent.step[i].fold = false;
				}
				if(GUILayout.Button("Expand all", GUILayout.Width(this.mWidth3)))
				{
					for(int i=0; i<this.gameEvent.step.Length; i++) this.gameEvent.step[i].fold = true;
				}
				EditorGUILayout.EndHorizontal();
				if(!this.showAdd)
				{
					EditorGUILayout.Separator();
					SP3= EditorGUILayout.BeginScrollView(SP3);
					for(int i=0; i<this.gameEvent.step.Length; i++)
					{
						EditorGUILayout.BeginVertical("box");
						GameEventTabs.ShowTab(i, this.gameEvent, this.gameEvent.step[i]);
						EditorGUILayout.EndVertical();
					}
					EditorGUILayout.EndScrollView();
				}
			}
		}
		
		// save handling
		EditorGUILayout.BeginHorizontal();
		GUI.SetNextControlName("new");
		if(GUILayout.Button("New"))
		{
			GUI.FocusControl("new");
			this.NewEvent();
		}
		GUI.SetNextControlName("load");
		if(GUILayout.Button("Load"))
		{
			GUI.FocusControl("load");
			string openPath = EditorUtility.OpenFilePanel("Open gameEvent file", Application.dataPath+"/Resources/Events", "xml");
			if(openPath.Length != 0)
			{
				this.LoadData(openPath);
			}
		}
		GUI.SetNextControlName("save");
		if(GUILayout.Button("Save"))
		{
			GUI.FocusControl("save");
			if(this.lastFile == "")
			{
				string savePath = EditorUtility.SaveFilePanel("Save gameEvent file", Application.dataPath+"/Resources/Events", this.lastFile, "xml");
				if(savePath.Length != 0)
				{
					this.gameEvent.SaveEventData(savePath);
					this.lastFile = savePath;
				}
			}
			else
			{
				this.gameEvent.SaveEventData(this.lastFile);
			}
		}
		
		if(this.lastFile != "")
		{
			GUI.SetNextControlName("saveas");
			if(GUILayout.Button("Save as ..."))
			{
				GUI.FocusControl("saveas");
				string savePath = EditorUtility.SaveFilePanel("Save gameEvent file as", Application.dataPath+"/Resources/Events", "", "xml");
				if(savePath.Length != 0)
				{
					this.gameEvent.SaveEventData(savePath);
					this.lastFile = savePath;
				}
			}
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();
	}
}