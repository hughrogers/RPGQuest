
using UnityEditor;
using UnityEngine;

public class GlobalEventTab : BaseTab
{
	private string[] types = new string[0];
	private string tmpCheck = "";
	private bool firstLoad = false;
	
	public GlobalEventTab(ProjectWindow pw) : base(pw)
	{
		this.Reload();
	}
	
	new public void Reload()
	{
		base.Reload();
		types = System.Enum.GetNames(typeof(ControlType));
	}
	
	public void ShowTab()
	{
		EditorGUILayout.BeginVertical();
		
		// buttons
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Add Event", GUILayout.Width(pw.mWidth2)))
		{
			DataHolder.GlobalEvents().AddEvent("New Event");
			selection = DataHolder.GlobalEvents().GetDataCount()-1;
			GUI.FocusControl ("ID");
		}
		this.ShowCopyButton(DataHolder.GlobalEvents());
		
		this.ShowRemButton("Remove Event", DataHolder.GlobalEvents());
		
		this.CheckSelection(DataHolder.GlobalEvents());
		
		EditorGUILayout.EndHorizontal();
		
		this.AddItemList(DataHolder.GlobalEvents());
		
		EditorGUILayout.BeginVertical();
		SP2 = EditorGUILayout.BeginScrollView(SP2);
		if(DataHolder.GlobalEvents().GetDataCount() > 0)
		{
			this.AddID("Event ID");
			DataHolder.GlobalEvents().name[selection] = EditorGUILayout.TextField("Name", DataHolder.GlobalEvents().name[selection]);
			this.Separate();
			
			EditorGUILayout.BeginVertical("box");
			fold1 = EditorGUILayout.Foldout(fold1, "Event execution conditions");
			if(fold1)
			{
				DataHolder.GlobalEvent(selection).eventType = (GlobalEventType)EditorTab.EnumToolbar("Event type", 
						(int)DataHolder.GlobalEvent(selection).eventType, typeof(GlobalEventType));
				
				if(DataHolder.GlobalEvent(selection).IsAutoType())
				{
					EditorGUILayout.Separator();
					DataHolder.GlobalEvent(selection).timeout = EditorGUILayout.FloatField("Check timeout (s)", 
							DataHolder.GlobalEvent(selection).timeout, GUILayout.Width(pw.mWidth));
					if(DataHolder.GlobalEvent(selection).timeout < 0)
					{
						DataHolder.GlobalEvent(selection).timeout = 0;
					}
					EditorGUILayout.Separator();
					
					GUILayout.Label("Control type", EditorStyles.boldLabel);
					for(int i=0; i<types.Length; i++)
					{
						DataHolder.GlobalEvent(selection).controlType[i] = EditorGUILayout.Toggle(types[i], 
								DataHolder.GlobalEvent(selection).controlType[i], GUILayout.Width(pw.mWidth));
					}
					EditorGUILayout.Separator();
					
					GUILayout.Label("Variable conditions", EditorStyles.boldLabel);
					DataHolder.GlobalEvent(selection).variables = EditorHelper.VariableConditionSettings(DataHolder.GlobalEvent(selection).variables);
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold2 = EditorGUILayout.Foldout(fold2, "Event settings");
			if(fold2)
			{
				tmpCheck = DataHolder.GlobalEvent(selection).eventFile;
				DataHolder.GlobalEvent(selection).eventFile = EditorGUILayout.TextField("Event file", DataHolder.GlobalEvent(selection).eventFile);
				if(tmpCheck != DataHolder.GlobalEvent(selection).eventFile)
				{
					DataHolder.GlobalEvent(selection).fileOk = false;
					this.firstLoad = false;
				}
				if(DataHolder.GlobalEvent(selection).fileOk && !this.firstLoad)
				{
					DataHolder.GlobalEvent(selection).LoadEvent();
				}
				
				EditorGUILayout.BeginHorizontal();
				if(GUILayout.Button("Check Event", GUILayout.Width(pw.mWidth2)))
				{
					DataHolder.GlobalEvent(selection).LoadEvent();
					if(DataHolder.GlobalEvent(selection).fileOk)
					{
						this.firstLoad = true;
						
						string[] tmp = DataHolder.GlobalEvent(selection).prefabName;
						DataHolder.GlobalEvent(selection).prefabName = new string[DataHolder.GlobalEvent(selection).gameEvent.prefabs];
						DataHolder.GlobalEvent(selection).prefab = new GameObject[DataHolder.GlobalEvent(selection).gameEvent.prefabs];
						for(int i=0; i<DataHolder.GlobalEvent(selection).prefabName.Length; i++)
						{
							if(i < tmp.Length) DataHolder.GlobalEvent(selection).prefabName[i] = tmp[i];
						}
						
						tmp = DataHolder.GlobalEvent(selection).audioName;
						DataHolder.GlobalEvent(selection).audioName = new string[DataHolder.GlobalEvent(selection).gameEvent.audioClips];
						DataHolder.GlobalEvent(selection).audioClip = new AudioClip[DataHolder.GlobalEvent(selection).gameEvent.audioClips];
						for(int i=0; i<DataHolder.GlobalEvent(selection).audioName.Length; i++)
						{
							if(i < tmp.Length) DataHolder.GlobalEvent(selection).audioName[i] = tmp[i];
						}
					}
				}
				
				if(DataHolder.GlobalEvent(selection).fileOk)
				{
					if(GUILayout.Button("Edit Event", GUILayout.Width(pw.mWidth2)))
					{
						EventWindow.Init(DataHolder.GlobalEvent(selection).GetEventFile());
						GUIUtility.ExitGUI();
					}
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.Separator();
					
					if(DataHolder.GlobalEvent(selection).gameEvent.actor.Length > 0)
					{
						GUILayout.Label("This event contains actors. Please note that global events don't support actor game objects.");
					}
					if(DataHolder.GlobalEvent(selection).gameEvent.waypoints > 0)
					{
						GUILayout.Label("This event contains waypoints. Please note that global events don't support waypoints.");
					}
					
					if(DataHolder.GlobalEvent(selection).prefabName.Length > 0)
					{
						GUILayout.Label("Prefabs (Assets/Resources/Prefabs/GlobalEvents/)", EditorStyles.boldLabel);
						for(int i=0; i<DataHolder.GlobalEvent(selection).prefabName.Length; i++)
						{
							if(DataHolder.GlobalEvent(selection).prefab[i] == null && 
								DataHolder.GlobalEvent(selection).prefabName[i] != null &&
								"" != DataHolder.GlobalEvent(selection).prefabName[i])
							{
								DataHolder.GlobalEvent(selection).prefab[i] = (GameObject)Resources.Load(GlobalEventData.PREFAB_PATH+
										DataHolder.GlobalEvent(selection).prefabName[i], typeof(GameObject));
							}
							
							DataHolder.GlobalEvent(selection).prefab[i] = (GameObject)EditorGUILayout.ObjectField("Prefab "+i, 
									DataHolder.GlobalEvent(selection).prefab[i], typeof(GameObject), false, GUILayout.Width(pw.mWidth*2));
							if(DataHolder.GlobalEvent(selection).prefab[i])
							{
								DataHolder.GlobalEvent(selection).prefabName[i] = DataHolder.GlobalEvent(selection).prefab[i].name;
							}
							else DataHolder.GlobalEvent(selection).prefabName[i] = "";
						}
						EditorGUILayout.Separator();
					}
					
					if(DataHolder.GlobalEvent(selection).audioName.Length > 0)
					{
						GUILayout.Label("Audio clips (Assets/Resources/Audio/GlobalEvents/)", EditorStyles.boldLabel);
						for(int i=0; i<DataHolder.GlobalEvent(selection).audioName.Length; i++)
						{
							if(DataHolder.GlobalEvent(selection).audioClip[i] == null && 
								DataHolder.GlobalEvent(selection).audioName[i] != null &&
								"" != DataHolder.GlobalEvent(selection).audioName[i])
							{
								DataHolder.GlobalEvent(selection).audioClip[i] = (AudioClip)Resources.Load(GlobalEventData.PREFAB_PATH+
										DataHolder.GlobalEvent(selection).audioName[i], typeof(GameObject));
							}
							
							DataHolder.GlobalEvent(selection).audioClip[i] = (AudioClip)EditorGUILayout.ObjectField("Audio clip "+i, 
									DataHolder.GlobalEvent(selection).audioClip[i], typeof(AudioClip), false, GUILayout.Width(pw.mWidth*2));
							if(DataHolder.GlobalEvent(selection).audioClip[i])
							{
								DataHolder.GlobalEvent(selection).audioName[i] = DataHolder.GlobalEvent(selection).audioClip[i].name;
							}
							else DataHolder.GlobalEvent(selection).audioName[i] = "";
						}
					}
				}
				else
				{
					if(GUILayout.Button("New Event", GUILayout.Width(pw.mWidth2)))
					{
						EventWindow.Init();
						GUIUtility.ExitGUI();
					}
					EditorGUILayout.EndHorizontal();
				}
				
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.EndVertical();
		}
		this.EndTab();
	}
}