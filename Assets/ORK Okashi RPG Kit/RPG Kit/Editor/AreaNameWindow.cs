
using UnityEditor;
using UnityEngine;

public class AreaNameWindow : EditorWindow
{
	public int mWidth = 300;
	public int mWidth2 = 200;
	private int selection = 0;
	private Vector2 SP1 = new Vector2(0, 0);
	private Vector2 SP2 = new Vector2(0, 0);
	
	private Texture2D tmpIcon;
	
	
	[MenuItem("RPG Kit/Area Name Editor")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		AreaNameWindow window = (AreaNameWindow)EditorWindow.GetWindow(
				typeof(AreaNameWindow), false, "Area Name Editor");
		window.Show();
	}
	
	void OnGUI()
	{
		EditorGUILayout.BeginVertical();
		// buttons
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Add", GUILayout.Width(this.mWidth2)))
		{
			DataHolder.AreaNames().AddBaseData("New Area", "New Description", DataHolder.Languages().GetDataCount());
			selection = DataHolder.AreaNames().GetDataCount()-1;
			GUI.FocusControl ("ID");
		}
		GUI.SetNextControlName("Copy");
		if(GUILayout.Button("Copy", GUILayout.Width(this.mWidth2)))
		{
			GUI.FocusControl("Copy");
			DataHolder.AreaNames().Copy(selection);
			selection = DataHolder.AreaNames().GetDataCount()-1;
		}
		if(DataHolder.AreaNames().GetDataCount() > 1)
		{
			GUI.SetNextControlName("Rem");
			if(GUILayout.Button("Remove", GUILayout.Width(this.mWidth2)))
			{
				GUI.FocusControl("Rem");
				DataHolder.AreaNames().RemoveData(selection);
			}
		}
		if(selection > DataHolder.AreaNames().GetDataCount()-1)
		{
			selection = DataHolder.AreaNames().GetDataCount()-1;
		}
		EditorGUILayout.EndHorizontal();
		
		// elements list
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical(GUILayout.Width(this.mWidth2));
		EditorGUILayout.Separator();
		SP1 = EditorGUILayout.BeginScrollView(SP1);
		
		if(DataHolder.AreaNames().GetDataCount() > 0)
		{
			int prev = selection;
			selection = GUILayout.SelectionGrid(selection, DataHolder.AreaNames().GetNameList(true), 1);
			if(prev != selection)
			{
				this.tmpIcon = null;
				GUI.FocusControl("ID");
			}
		}
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
		
		// element settings
		EditorGUILayout.BeginVertical();
		SP2 = EditorGUILayout.BeginScrollView(SP2);
		if(DataHolder.AreaNames().GetDataCount() > 0)
		{
			EditorGUILayout.BeginVertical();
			EditorGUILayout.Separator();
			GUI.SetNextControlName("ID");
			EditorGUILayout.LabelField("Area Name ID", selection.ToString(), GUILayout.Width(this.mWidth));
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			for(int i=0; i<DataHolder.Languages().GetDataCount(); i++)
			{
				GUILayout.Label (DataHolder.Language(i), EditorStyles.boldLabel);
				DataHolder.AreaNames().name[i].text[selection] = 
					EditorGUILayout.TextField("Area Name", DataHolder.AreaNames().name[i].text[selection]);
				DataHolder.AreaNames().description[i].text[selection] = 
					EditorGUILayout.TextField("Description", DataHolder.AreaNames().description[i].text[selection]);
			}
			EditorGUILayout.EndVertical();
			
			if(this.tmpIcon == null && DataHolder.AreaNames().icon[selection] != null && 
				"" != DataHolder.AreaNames().icon[selection])
			{
				this.tmpIcon = (Texture2D)Resources.Load(DataHolder.AreaNames().GetIconPath()+
					DataHolder.AreaNames().icon[selection], typeof(Texture2D));
			}
			this.tmpIcon = (Texture2D)EditorGUILayout.ObjectField("Icon", this.tmpIcon, typeof(Texture2D), false);
			if(this.tmpIcon) DataHolder.AreaNames().icon[selection] = this.tmpIcon.name;
			else DataHolder.AreaNames().icon[selection] = "";
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
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
			this.tmpIcon = null;
		}
		GUI.SetNextControlName("Save");
		if(GUILayout.Button("Save Settings"))
		{
			GUI.FocusControl("Save");
			DataHolder.AreaNames().SaveData();
		}
		EditorGUILayout.EndHorizontal();
	}
}