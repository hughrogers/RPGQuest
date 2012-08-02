
using UnityEditor;
using UnityEngine;

public class MusicWindow : EditorWindow
{
	public int mWidth = 300;
	public int mWidth2 = 200;
	private int selection = 0;
	private Vector2 SP1 = new Vector2(0, 0);
	private Vector2 SP2 = new Vector2(0, 0);
	
	private AudioClip musicSource;
	
	[MenuItem("RPG Kit/Music Editor")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		MusicWindow window = (MusicWindow)EditorWindow.GetWindow(typeof(MusicWindow), false, "Music Editor");
		window.Show();
	}
	
	void OnGUI()
	{
		EditorGUILayout.BeginVertical();
		// buttons
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Add", GUILayout.Width(this.mWidth2)))
		{
			DataHolder.Music().AddMusic("New Music");
			selection = DataHolder.Music().GetDataCount()-1;
			this.musicSource = null;
			GUI.FocusControl ("ID");
		}
		GUI.SetNextControlName("Copy");
		if(GUILayout.Button("Copy", GUILayout.Width(this.mWidth2)))
		{
			GUI.FocusControl("Copy");
			DataHolder.Music().Copy(selection);
			this.musicSource = null;
			selection = DataHolder.Music().GetDataCount()-1;
		}
		if(DataHolder.CameraPositions().GetDataCount() > 1)
		{
			GUI.SetNextControlName("Rem");
			if(GUILayout.Button("Remove", GUILayout.Width(this.mWidth2)))
			{
				GUI.FocusControl("Rem");
				this.musicSource = null;
				DataHolder.Music().RemoveData(selection);
			}
		}
		if(selection > DataHolder.Music().GetDataCount()-1)
		{
			selection = DataHolder.Music().GetDataCount()-1;
		}
		EditorGUILayout.EndHorizontal();
		
		// elements list
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical(GUILayout.Width(this.mWidth2));
		EditorGUILayout.Separator();
		EditorGUILayout.BeginVertical("box");
		SP1 = EditorGUILayout.BeginScrollView(SP1);
		if(DataHolder.Music().GetDataCount() > 0)
		{
			var prev = selection;
			selection = GUILayout.SelectionGrid(selection, DataHolder.Music().GetNameList(true), 1);
			if(prev != selection)
			{
				this.musicSource = null;
				GUI.FocusControl("ID");
			}
		}
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndVertical();
		
		// element settings
		EditorGUILayout.BeginVertical();
		SP2 = EditorGUILayout.BeginScrollView(SP2);
		if(DataHolder.Music().GetDataCount() > 0)
		{
			EditorGUILayout.BeginVertical();
			EditorGUILayout.Separator();
			GUI.SetNextControlName("ID");
			EditorGUILayout.LabelField("Position ID", selection.ToString(), GUILayout.Width(this.mWidth));
			DataHolder.Music().name[selection] = EditorGUILayout.TextField("Name", DataHolder.Music().name[selection]);
			DataHolder.MusicClip(selection).maxVolume = EditorGUILayout.FloatField("Max volume", DataHolder.MusicClip(selection).maxVolume, GUILayout.Width(this.mWidth));
			DataHolder.MusicClip(selection).loop = EditorGUILayout.Toggle("Loop clip", DataHolder.MusicClip(selection).loop, GUILayout.Width(this.mWidth));
			EditorGUILayout.Separator();
			if(this.musicSource == null && DataHolder.MusicClip(selection).clipName != null && "" != DataHolder.MusicClip(selection).clipName)
			{
				this.musicSource = (AudioClip)Resources.Load(DataHolder.Music().clipPath+DataHolder.MusicClip(selection).clipName, typeof(AudioClip));
			}
			this.musicSource = (AudioClip)EditorGUILayout.ObjectField("Audio clip", this.musicSource, typeof(AudioClip), false);
			if(this.musicSource) DataHolder.MusicClip(selection).clipName = this.musicSource.name;
			else DataHolder.MusicClip(selection).clipName = "";
			
			EditorGUILayout.Separator();
			if(GUILayout.Button("Add Loop", GUILayout.Width(this.mWidth)))
			{
				DataHolder.MusicClip(selection).AddLoop();
			}
			for(int i=0; i<DataHolder.MusicClip(selection).checkTime.Length; i++)
			{
				EditorGUILayout.BeginVertical("box");
				GUILayout.Label("Loop step "+i, EditorStyles.boldLabel);
				if(GUILayout.Button("Remove", GUILayout.Width(this.mWidth*0.5f)))
				{
					DataHolder.MusicClip(selection).RemoveLoop(i);
					return;
				}
				DataHolder.MusicClip(selection).checkTime[i] = EditorGUILayout.FloatField("Check time", 
						DataHolder.MusicClip(selection).checkTime[i], GUILayout.Width(this.mWidth));
				DataHolder.MusicClip(selection).setTime[i] = EditorGUILayout.FloatField("Set time", 
						DataHolder.MusicClip(selection).setTime[i], GUILayout.Width(this.mWidth));
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
			this.musicSource = null;
		}
		GUI.SetNextControlName("Save");
		if(GUILayout.Button("Save Settings"))
		{
			GUI.FocusControl("Save");
			DataHolder.Music().SaveData();
		}
		EditorGUILayout.EndHorizontal();
	}
}