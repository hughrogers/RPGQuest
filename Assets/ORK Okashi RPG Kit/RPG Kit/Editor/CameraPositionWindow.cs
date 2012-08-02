
using UnityEditor;
using UnityEngine;

public class CameraPositionWindow : EditorWindow
{
	public int mWidth = 300;
	public int mWidth2 = 200;
	private int selection = 0;
	private Vector2 SP1 = new Vector2(0, 0);
	private Vector2 SP2 = new Vector2(0, 0);
	
	private bool fold1 = true;
	
	[MenuItem("RPG Kit/Position Editors/Camera Position Editor")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		CameraPositionWindow window = (CameraPositionWindow)EditorWindow.GetWindow(
			typeof(CameraPositionWindow), false, "Camera Position Editor");
		window.Show();
	}
	
	void OnGUI()
	{
		EditorGUILayout.BeginVertical();
		// buttons
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Add", GUILayout.Width(this.mWidth2)))
		{
			DataHolder.CameraPositions().AddCamPos("New Position");
			selection = DataHolder.CameraPositions().GetDataCount()-1;
			GUI.FocusControl ("ID");
		}
		GUI.SetNextControlName("Copy");
		if(GUILayout.Button("Copy", GUILayout.Width(this.mWidth2)))
		{
			GUI.FocusControl("Copy");
			DataHolder.CameraPositions().Copy(selection);
			selection = DataHolder.CameraPositions().GetDataCount()-1;
		}
		if(DataHolder.CameraPositions().GetDataCount() > 1)
		{
			GUI.SetNextControlName("Rem");
			if(GUILayout.Button("Remove", GUILayout.Width(this.mWidth2)))
			{
				GUI.FocusControl("Rem");
				DataHolder.CameraPositions().RemoveData(selection);
			}
		}
		if(selection > DataHolder.CameraPositions().GetDataCount()-1)
		{
			selection = DataHolder.CameraPositions().GetDataCount()-1;
		}
		EditorGUILayout.EndHorizontal();
		
		// elements list
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical(GUILayout.Width(this.mWidth2));
		EditorGUILayout.Separator();
		EditorGUILayout.BeginVertical("box");
		SP1 = EditorGUILayout.BeginScrollView(SP1);
		if(DataHolder.CameraPositions().GetDataCount() > 0)
		{
			var prev = selection;
			selection = GUILayout.SelectionGrid(selection, DataHolder.CameraPositions().GetNameList(true), 1);
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
		if(DataHolder.CameraPositions().GetDataCount() > 0)
		{
			EditorGUILayout.Separator();
			GUI.SetNextControlName("ID");
			EditorGUILayout.LabelField("Position ID", selection.ToString(), GUILayout.Width(this.mWidth));
			DataHolder.CameraPositions().name[selection] = EditorGUILayout.TextField("Name", DataHolder.CameraPositions().name[selection]);
			EditorGUILayout.Separator();
			
			EditorGUILayout.BeginVertical("box");
			fold1 = EditorGUILayout.Foldout(fold1, "Base settings");
			if(fold1)
			{
				EditorGUILayout.BeginHorizontal();
				DataHolder.CameraPosition(selection).targetChild = EditorGUILayout.BeginToggleGroup("Target child", DataHolder.CameraPosition(selection).targetChild);
				DataHolder.CameraPosition(selection).childName = EditorGUILayout.TextField(DataHolder.CameraPosition(selection).childName);
				EditorGUILayout.EndToggleGroup();
				EditorGUILayout.EndHorizontal();
				DataHolder.CameraPosition(selection).localPoint = EditorGUILayout.Toggle("Local point", DataHolder.CameraPosition(selection).localPoint, GUILayout.Width(this.mWidth));
				EditorGUILayout.Separator();
				DataHolder.CameraPosition(selection).position = EditorGUILayout.Vector3Field("Position offset", DataHolder.CameraPosition(selection).position);
				EditorGUILayout.Separator();
				DataHolder.CameraPosition(selection).rotation = EditorGUILayout.Vector3Field("Rotation", DataHolder.CameraPosition(selection).rotation);
				DataHolder.CameraPosition(selection).lookAt = EditorGUILayout.Toggle("Look at", DataHolder.CameraPosition(selection).lookAt, GUILayout.Width(this.mWidth));
				DataHolder.CameraPosition(selection).ignoreXRotation = EditorGUILayout.Toggle("Ignore X Rotation", DataHolder.CameraPosition(selection).ignoreXRotation, GUILayout.Width(this.mWidth));
				DataHolder.CameraPosition(selection).ignoreYRotation = EditorGUILayout.Toggle("Ignore Y Rotation", DataHolder.CameraPosition(selection).ignoreYRotation, GUILayout.Width(this.mWidth));
				DataHolder.CameraPosition(selection).ignoreZRotation = EditorGUILayout.Toggle("Ignore Z Rotation", DataHolder.CameraPosition(selection).ignoreZRotation, GUILayout.Width(this.mWidth));
				EditorGUILayout.Separator();
				DataHolder.CameraPosition(selection).setFoV = EditorGUILayout.BeginToggleGroup("Set Field of View", DataHolder.CameraPosition(selection).setFoV);
				DataHolder.CameraPosition(selection).fieldOfView = EditorGUILayout.FloatField("Field of View", DataHolder.CameraPosition(selection).fieldOfView, GUILayout.Width(this.mWidth));
				EditorGUILayout.EndToggleGroup();
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
			DataHolder.CameraPositions().SaveData();
		}
		EditorGUILayout.EndHorizontal();
	}
}