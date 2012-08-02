
using UnityEditor;
using UnityEngine;

public class AboutORKWindow : EditorWindow
{
	public int mWidth = 200;
	private Vector2 SP1 = new Vector2(0, 0);
	
	private Texture2D texture;
	
	
	[MenuItem("RPG Kit/About ORK")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		AboutORKWindow window = (AboutORKWindow)EditorWindow.GetWindowWithRect(
				typeof(AboutORKWindow), new Rect(100, 100, 310, 410), true, "About ORK");
		window.Show();
	}
	
	void OnGUI()
	{
		EditorGUILayout.BeginVertical();
		SP1 = EditorGUILayout.BeginScrollView(SP1);
		if(this.texture == null)
		{
			this.texture = (Texture2D)Resources.LoadAssetAtPath("Assets/RPG Kit/Editor/About/ORK_logo.png", typeof(Texture2D));
		}
		GUI.DrawTexture(new Rect(0, 0, 300, 200), this.texture);
		
		GUILayout.Space(210);
		EditorGUILayout.BeginVertical("box");
		GUILayout.Label("ORK Okashi RPG Kit, Version 1.2.0", EditorStyles.boldLabel);
		GUILayout.Label("(c) 2010 Okashi Itsumo e.U. All rights reserved.");
		
		if(GUILayout.Button("News"))
		{
			Application.OpenURL("http://www.rpg-kit.com/news.html");
		}
		if(GUILayout.Button("ORK Wiki/Documentation"))
		{
			Application.OpenURL("http://www.rpg-kit.com/wiki");
		}
		if(GUILayout.Button("Tutorials"))
		{
			Application.OpenURL("http://www.rpg-kit.com/tutorials.html");
		}
		if(GUILayout.Button("Changelog"))
		{
			Application.OpenURL("http://www.rpg-kit.com/changelog.html");
		}
		if(GUILayout.Button("Extensions"))
		{
			Application.OpenURL("http://www.rpg-kit.com/extensions.html");
		}
		if(GUILayout.Button("Contact (email)"))
		{
			Application.OpenURL("mailto:ork@rpg-kit.com");
		}
		
		EditorGUILayout.Separator();
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}
}