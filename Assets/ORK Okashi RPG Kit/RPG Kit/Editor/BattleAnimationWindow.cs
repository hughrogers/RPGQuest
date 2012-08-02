
using UnityEditor;
using UnityEngine;

public class BattleAnimationWindow : EditorWindow
{
	public int mWidth = 300;
	public int mWidth2 = 200;
	public int mWidth3 = 100;
	private int selection = 0;
	private Vector2 SP1 = new Vector2(0, 0);
	private Vector2 SP2 = new Vector2(0, 0);
	private Vector2 SP3 = new Vector2(0, 0);
	
	private bool fold1 = true;
	private bool fold2 = true;
	private bool fold3 = true;
	
	private bool showAdd = false;
	
	private int addAt = 0;
	
	[MenuItem("RPG Kit/Battle System/Battle Animation Editor")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		BattleAnimationWindow window = (BattleAnimationWindow)EditorWindow.GetWindow(
			typeof(BattleAnimationWindow), false, "Battle Animation Editor");
		DataHolder.Instance().Init();
		window.Show();
	}
	
	void OnGUI()
	{
		EditorGUILayout.BeginVertical();
		// buttons
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Add", GUILayout.Width(this.mWidth2)))
		{
			DataHolder.BattleAnimations().AddBattleAnimation("New Animation");
			selection = DataHolder.BattleAnimations().GetDataCount()-1;
			GUI.FocusControl ("ID");
		}
		GUI.SetNextControlName("Copy");
		if(GUILayout.Button("Copy", GUILayout.Width(this.mWidth2)))
		{
			GUI.FocusControl("Copy");
			DataHolder.BattleAnimations().Copy(selection);
			selection = DataHolder.BattleAnimations().GetDataCount()-1;
		}
		if(DataHolder.BattleAnimations().GetDataCount() > 1)
		{
			GUI.SetNextControlName("Rem");
			if(GUILayout.Button("Remove", GUILayout.Width(this.mWidth2)))
			{
				GUI.FocusControl("Rem");
				DataHolder.BattleAnimations().RemoveData(selection);
			}
		}
		if(selection > DataHolder.BattleAnimations().GetDataCount()-1)
		{
			selection = DataHolder.BattleAnimations().GetDataCount()-1;
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical(GUILayout.Width(this.mWidth2));
		EditorGUILayout.Separator();
		EditorGUILayout.BeginVertical("box");
		SP1 = EditorGUILayout.BeginScrollView(SP1);
		if(DataHolder.BattleAnimations().GetDataCount() > 0)
		{
			var prev = selection;
			selection = GUILayout.SelectionGrid(selection, DataHolder.BattleAnimations().GetNameList(true), 1);
			if(prev != selection)
			{
				GUI.FocusControl("ID");
			}
		}
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical();
		if(DataHolder.BattleAnimations().GetDataCount() > 0)
		{
			EditorGUILayout.Separator();
			GUI.SetNextControlName("ID");
			EditorGUILayout.LabelField("Position ID", selection.ToString(), GUILayout.Width(this.mWidth));
			DataHolder.BattleAnimations().name[selection] = EditorGUILayout.TextField("Name", DataHolder.BattleAnimations().name[selection]);
			EditorGUILayout.Separator();
			
			EditorGUILayout.BeginVertical();
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
				else if(this.addAt > DataHolder.BattleAnimation(selection).step.Length)
				{
					this.addAt = DataHolder.BattleAnimation(selection).step.Length;
				}
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				
				string[] types = System.Enum.GetNames(typeof(BattleAnimationType));
				SP2 = EditorGUILayout.BeginScrollView(SP2);
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical();
				for(int i=0; i<types.Length; i++)
				{
					if(types[i] == "WAIT") GUILayout.Label("Base steps", EditorStyles.boldLabel);
					if(types[i] == "CALCULATE") GUILayout.Label("Battle system steps", EditorStyles.boldLabel);
					if(types[i] == "SPAWN_PREFAB") GUILayout.Label("Spawn steps", EditorStyles.boldLabel);
					if(types[i] == "SEND_MESSAGE") GUILayout.Label("Function steps", EditorStyles.boldLabel);
					if(types[i] == "CUSTOM_STATISTIC") GUILayout.Label("Statistic steps", EditorStyles.boldLabel);
					else if(types[i] == "SET_CAMERA_POSITION")
					{
						EditorGUILayout.EndVertical();
						EditorGUILayout.Separator();
						EditorGUILayout.BeginVertical();
						GUILayout.Label("Camera steps", EditorStyles.boldLabel);
					}
					else if(types[i] == "SET_TO_POSITION") GUILayout.Label("Move steps", EditorStyles.boldLabel);
					else if(types[i] == "ROTATE_TO") GUILayout.Label("Rotate steps", EditorStyles.boldLabel);
					else if(types[i] == "PLAY_ANIMATION")
					{
						EditorGUILayout.EndVertical();
						EditorGUILayout.Separator();
						EditorGUILayout.BeginVertical();
						GUILayout.Label("Animation steps", EditorStyles.boldLabel);
					}
					else if(types[i] == "PLAY_SOUND") GUILayout.Label("Audio steps", EditorStyles.boldLabel);
					else if(types[i] == "FADE_OBJECT") GUILayout.Label("Fade steps", EditorStyles.boldLabel);
					else if(types[i] == "ADD_ITEM")
					{
						EditorGUILayout.EndVertical();
						EditorGUILayout.Separator();
						EditorGUILayout.BeginVertical();
						GUILayout.Label("Item steps", EditorStyles.boldLabel);
					}
					else if(types[i] == "SET_VARIABLE") GUILayout.Label("Variable steps", EditorStyles.boldLabel);
					
					if(GUILayout.Button(types[i], GUILayout.Width(this.mWidth2)))
					{
						DataHolder.BattleAnimation(selection).AddStep(
								(BattleAnimationType)System.Enum.ToObject(typeof(BattleAnimationType), i), this.addAt);
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
				if(!this.showAdd)
				{
					EditorGUILayout.BeginVertical("box", GUILayout.MinWidth(this.mWidth));
					this.fold3 = EditorGUILayout.Foldout(this.fold3, "Base settings");
					if(this.fold3)
					{
						DataHolder.BattleAnimation(selection).returnToBaseCamPos = EditorGUILayout.Toggle("Reset cam pos",
								DataHolder.BattleAnimation(selection).returnToBaseCamPos, GUILayout.Width(this.mWidth));
						DataHolder.BattleAnimation(selection).returnLooks = EditorGUILayout.Toggle("Reset look angles",
								DataHolder.BattleAnimation(selection).returnLooks, GUILayout.Width(this.mWidth));
						DataHolder.BattleAnimation(selection).calculationNeeded = EditorGUILayout.Toggle("Calculation needed",
								DataHolder.BattleAnimation(selection).calculationNeeded, GUILayout.Width(this.mWidth));
						EditorGUILayout.Separator();
					}
					EditorGUILayout.EndVertical();
					
					EditorGUILayout.BeginVertical("box", GUILayout.MinWidth(this.mWidth));
					this.fold1 = EditorGUILayout.Foldout(this.fold1, "Prefab settings");
					if(this.fold1)
					{
						EditorGUILayout.BeginHorizontal();
						if(GUILayout.Button("Add prefab", GUILayout.Width(this.mWidth2)))
						{
							DataHolder.BattleAnimation(selection).AddPrefab();
							return;
						}
						DataHolder.BattleAnimation(selection).autoDestroyPrefabs = EditorGUILayout.Toggle("Destroy prefabs", 
							DataHolder.BattleAnimation(selection).autoDestroyPrefabs, GUILayout.Width(this.mWidth));
						GUILayout.FlexibleSpace();
						EditorGUILayout.EndHorizontal();
						
						for(int i=0; i<DataHolder.BattleAnimation(selection).prefab.Length; i++)
						{
							if(DataHolder.BattleAnimation(selection).prefab[i] == null && 
								DataHolder.BattleAnimation(selection).prefabName[i] != null && 
								"" != DataHolder.BattleAnimation(selection).prefabName[i])
							{
								DataHolder.BattleAnimation(selection).prefab[i] = (GameObject)Resources.Load(
										BattleSystemData.PREFAB_PATH+
										DataHolder.BattleAnimation(selection).prefabName[i], typeof(GameObject));
							}
							
							EditorGUILayout.BeginHorizontal();
							DataHolder.BattleAnimation(selection).prefab[i] = (GameObject)EditorGUILayout.ObjectField("Prefab "+i, 
									DataHolder.BattleAnimation(selection).prefab[i], typeof(GameObject), false, GUILayout.Width(this.mWidth*2));
							if(DataHolder.BattleAnimation(selection).prefab[i])
							{
								DataHolder.BattleAnimation(selection).prefabName[i] = DataHolder.BattleAnimation(selection).prefab[i].name;
							}
							else DataHolder.BattleAnimation(selection).prefabName[i] = "";
							if(GUILayout.Button("Remove", GUILayout.Width(this.mWidth3)))
							{
								DataHolder.BattleAnimation(selection).RemovePrefab(i);
								return;
							}
							GUILayout.FlexibleSpace();
							EditorGUILayout.EndHorizontal();
						}
						EditorGUILayout.Separator();
					}
					EditorGUILayout.EndVertical();
					
					EditorGUILayout.BeginVertical("box", GUILayout.MinWidth(this.mWidth));
					this.fold2 = EditorGUILayout.Foldout(this.fold2, "Audio clip settings");
					if(this.fold2)
					{
						if(GUILayout.Button("Add audio clip", GUILayout.Width(this.mWidth2)))
						{
							DataHolder.BattleAnimation(selection).AddAudioClip();
							return;
						}
						for(int i=0; i<DataHolder.BattleAnimation(selection).audioClip.Length; i++)
						{
							if(DataHolder.BattleAnimation(selection).audioClip[i] == null && 
								DataHolder.BattleAnimation(selection).audioName[i] != null && 
								"" != DataHolder.BattleAnimation(selection).audioName[i])
							{
								DataHolder.BattleAnimation(selection).audioClip[i] = (AudioClip)Resources.Load(
										BattleSystemData.AUDIO_PATH+
										DataHolder.BattleAnimation(selection).audioName[i], typeof(AudioClip));
							}
							
							EditorGUILayout.BeginHorizontal();
							DataHolder.BattleAnimation(selection).audioClip[i] = (AudioClip)EditorGUILayout.ObjectField("Audio clip "+i, 
									DataHolder.BattleAnimation(selection).audioClip[i], typeof(AudioClip), false, GUILayout.Width(this.mWidth*2));
							if(DataHolder.BattleAnimation(selection).audioClip[i])
							{
								DataHolder.BattleAnimation(selection).audioName[i] = DataHolder.BattleAnimation(selection).audioClip[i].name;
							}
							else DataHolder.BattleAnimation(selection).audioName[i] = "";
							if(GUILayout.Button("Remove", GUILayout.Width(this.mWidth3)))
							{
								DataHolder.BattleAnimation(selection).RemoveAudioClip(i);
								return;
							}
							GUILayout.FlexibleSpace();
							EditorGUILayout.EndHorizontal();
						}
						EditorGUILayout.Separator();
					}
					EditorGUILayout.EndVertical();
					EditorGUILayout.Separator();
					EditorGUILayout.Separator();
					
					EditorGUILayout.BeginHorizontal();
					if(GUILayout.Button("Add Step", GUILayout.Width(this.mWidth2)))
					{
						this.showAdd = true;
						this.addAt = DataHolder.BattleAnimation(selection).step.Length;
					}
					DataHolder.BattleAnimation(selection).hideButtons = EditorGUILayout.Toggle("Hide buttons", DataHolder.BattleAnimation(selection).hideButtons, GUILayout.Width(this.mWidth));
					GUILayout.FlexibleSpace();
					if(GUILayout.Button("Collapse all", GUILayout.Width(this.mWidth3)))
					{
						for(int i=0; i<DataHolder.BattleAnimation(selection).step.Length; i++) DataHolder.BattleAnimation(selection).step[i].fold = false;
					}
					if(GUILayout.Button("Expand all", GUILayout.Width(this.mWidth3)))
					{
						for(int i=0; i<DataHolder.BattleAnimation(selection).step.Length; i++) DataHolder.BattleAnimation(selection).step[i].fold = true;
					}
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.Separator();
					
					SP3= EditorGUILayout.BeginScrollView(SP3);
					for(int i=0; i<DataHolder.BattleAnimation(selection).step.Length; i++)
					{
						EditorGUILayout.BeginVertical("box");
						BattleAnimationTabs.ShowTab(i, DataHolder.BattleAnimation(selection), DataHolder.BattleAnimation(selection).step[i]);
						EditorGUILayout.EndVertical();
					}
					EditorGUILayout.EndScrollView();
				}
			}
			EditorGUILayout.Separator();
			EditorGUILayout.EndVertical();
		}
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
		
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
			DataHolder.BattleAnimations().SaveData();
		}
		EditorGUILayout.EndHorizontal();
	}
}