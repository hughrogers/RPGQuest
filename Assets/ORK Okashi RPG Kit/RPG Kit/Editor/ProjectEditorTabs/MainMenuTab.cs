
using UnityEditor;
using UnityEngine;

public class MainMenuTab : BaseTab
{
	public MainMenuTab(ProjectWindow pw) : base(pw)
	{
		this.Reload();
	}
	
	public void ShowTab()
	{
		EditorGUILayout.BeginVertical();
		SP1 = EditorGUILayout.BeginScrollView(SP1);
		EditorGUILayout.Separator();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical("box");
		fold1 = EditorGUILayout.Foldout(fold1, "General settings");
		if(fold1)
		{
			DataHolder.MainMenu().menuPosition = EditorGUILayout.Popup("Position", DataHolder.MainMenu().menuPosition, 
					DataHolder.DialoguePositions().GetNameList(true), GUILayout.Width(pw.mWidth));
			
			EditorGUILayout.Separator();
			GUILayout.Label("Cancel text", EditorStyles.boldLabel);
			for(int i=0; i<DataHolder.MainMenu().cancelText.Length; i++)
			{
				DataHolder.MainMenu().cancelText[i] = EditorGUILayout.TextField(DataHolder.Languages().GetName(i), 
						DataHolder.MainMenu().cancelText[i], GUILayout.Width(pw.mWidth*2));
				EditorGUILayout.Separator();
			}
			
			EditorGUILayout.Separator();
			GUILayout.Label("New game fading", EditorStyles.boldLabel);
			DataHolder.MainMenu().fadeOut = EditorGUILayout.Toggle("Fade out", DataHolder.MainMenu().fadeOut, GUILayout.Width(pw.mWidth));
			if(DataHolder.MainMenu().fadeOut)
			{
				DataHolder.MainMenu().fadeOutTime = EditorGUILayout.FloatField("Time", 
						DataHolder.MainMenu().fadeOutTime, GUILayout.Width(pw.mWidth));
				DataHolder.MainMenu().fadeOutInterpolate = (EaseType)EditorGUILayout.EnumPopup("Interpolation", 
						DataHolder.MainMenu().fadeOutInterpolate, GUILayout.Width(pw.mWidth));
				EditorGUILayout.Separator();
			}
			DataHolder.MainMenu().fadeIn = EditorGUILayout.Toggle("Fade in", DataHolder.MainMenu().fadeIn, GUILayout.Width(pw.mWidth));
			if(DataHolder.MainMenu().fadeIn)
			{
				DataHolder.MainMenu().fadeInTime = EditorGUILayout.FloatField("Time", 
						DataHolder.MainMenu().fadeInTime, GUILayout.Width(pw.mWidth));
				DataHolder.MainMenu().fadeInInterpolate = (EaseType)EditorGUILayout.EnumPopup("Interpolation", 
						DataHolder.MainMenu().fadeInInterpolate, GUILayout.Width(pw.mWidth));
				EditorGUILayout.Separator();
			}
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical("box");
		fold7 = EditorGUILayout.Foldout(fold7, "Exit to main menu");
		if(fold7)
		{
			int langs = DataHolder.Languages().GetDataCount();
			DataHolder.MainMenu().questionText = EditorHelper.CheckLanguageCount(DataHolder.MainMenu().questionText, langs);
			DataHolder.MainMenu().questionYes = EditorHelper.CheckLanguageCount(DataHolder.MainMenu().questionYes, langs);
			DataHolder.MainMenu().questionNo = EditorHelper.CheckLanguageCount(DataHolder.MainMenu().questionNo, langs);
			
			DataHolder.MainMenu().mainMenuScene = EditorGUILayout.TextField("Menu scene", 
					DataHolder.MainMenu().mainMenuScene, GUILayout.Width(pw.mWidth*2));
			DataHolder.MainMenu().autoCall = EditorGUILayout.Toggle("Auto call menu", DataHolder.MainMenu().autoCall, GUILayout.Width(pw.mWidth));
			if(DataHolder.MainMenu().autoCall)
			{
				DataHolder.MainMenu().waitTime = EditorGUILayout.FloatField("Wait (s)", 
						DataHolder.MainMenu().waitTime, GUILayout.Width(pw.mWidth));
			}
			
			EditorGUILayout.Separator();
			DataHolder.MainMenu().questionPosition = EditorGUILayout.Popup("Question position", DataHolder.MainMenu().questionPosition, 
					DataHolder.DialoguePositions().GetNameList(true), GUILayout.Width(pw.mWidth));
			
			EditorGUILayout.Separator();
			for(int i=0; i<langs; i++)
			{
				GUILayout.Label(DataHolder.Languages().GetName(i), EditorStyles.boldLabel);
				DataHolder.MainMenu().questionText[i] = EditorGUILayout.TextField("Question text", 
						DataHolder.MainMenu().questionText[i], GUILayout.Width(pw.mWidth*2));
				DataHolder.MainMenu().questionYes[i] = EditorGUILayout.TextField("Yes text", 
						DataHolder.MainMenu().questionYes[i], GUILayout.Width(pw.mWidth*2));
				DataHolder.MainMenu().questionNo[i] = EditorGUILayout.TextField("No text", 
						DataHolder.MainMenu().questionNo[i], GUILayout.Width(pw.mWidth*2));
			}
			
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical("box");
		fold2 = EditorGUILayout.Foldout(fold2, "New game");
		if(fold2)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			GUILayout.Label("Choice text", EditorStyles.boldLabel);
			for(int i=0; i<DataHolder.MainMenu().newGameText.Length; i++)
			{
				DataHolder.MainMenu().newGameText[i] = EditorGUILayout.TextField(DataHolder.Languages().GetName(i), 
						DataHolder.MainMenu().newGameText[i], GUILayout.Width(pw.mWidth*2));
				EditorGUILayout.Separator();
			}
			EditorGUILayout.EndVertical();
			if(DataHolder.MainMenu().newIcon == null && 
				DataHolder.MainMenu().newIconName != null &&
				"" != DataHolder.MainMenu().newIconName)
			{
				DataHolder.MainMenu().newIcon = (Texture2D)Resources.Load(DataHolder.MainMenu().iconPath+
						DataHolder.MainMenu().newIconName, typeof(Texture2D));
			}
			DataHolder.MainMenu().newIcon = (Texture2D)EditorGUILayout.ObjectField("Icon", DataHolder.MainMenu().newIcon, typeof(Texture2D), false);
			if(DataHolder.MainMenu().newIcon) DataHolder.MainMenu().newIconName = DataHolder.MainMenu().newIcon.name;
			else DataHolder.MainMenu().newIconName = "";
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Separator();
			DataHolder.MainMenu().showDifficulty = EditorGUILayout.Toggle("Difficulty selection", 
					DataHolder.MainMenu().showDifficulty, GUILayout.Width(pw.mWidth));
			if(DataHolder.MainMenu().showDifficulty)
			{
				GUILayout.Label("Difficulty question text", EditorStyles.boldLabel);
				EditorHelper.TextSettings(ref DataHolder.MainMenu().difficultyQuestion);
			}
			
			EditorGUILayout.Separator();
			DataHolder.MainMenu().newGameScene = EditorGUILayout.TextField("Load scene", 
					DataHolder.MainMenu().newGameScene, GUILayout.Width(pw.mWidth*2));
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical("box");
		fold3 = EditorGUILayout.Foldout(fold3, "Load game");
		if(fold3)
		{
			DataHolder.MainMenu().showLoad = EditorGUILayout.Toggle("Show", DataHolder.MainMenu().showLoad, GUILayout.Width(pw.mWidth));
			if(DataHolder.MainMenu().showLoad)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical();
				GUILayout.Label("Choice text", EditorStyles.boldLabel);
				for(int i=0; i<DataHolder.MainMenu().loadGameText.Length; i++)
				{
					DataHolder.MainMenu().loadGameText[i] = EditorGUILayout.TextField(DataHolder.Languages().GetName(i), 
							DataHolder.MainMenu().loadGameText[i], GUILayout.Width(pw.mWidth*2));
					EditorGUILayout.Separator();
				}
				EditorGUILayout.EndVertical();
				if(DataHolder.MainMenu().loadIcon == null && 
					DataHolder.MainMenu().loadIconName != null &&
					"" != DataHolder.MainMenu().loadIconName)
				{
					DataHolder.MainMenu().loadIcon = (Texture2D)Resources.Load(DataHolder.MainMenu().iconPath+
							DataHolder.MainMenu().loadIconName, typeof(Texture2D));
				}
				DataHolder.MainMenu().loadIcon = (Texture2D)EditorGUILayout.ObjectField("Icon", DataHolder.MainMenu().loadIcon, typeof(Texture2D), false);
				if(DataHolder.MainMenu().loadIcon) DataHolder.MainMenu().loadIconName = DataHolder.MainMenu().loadIcon.name;
				else DataHolder.MainMenu().loadIconName = "";
				EditorGUILayout.EndHorizontal();
			}
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical("box");
		fold4 = EditorGUILayout.Foldout(fold4, "Language");
		if(fold4)
		{
			DataHolder.MainMenu().showLanguage = EditorGUILayout.Toggle("Show", DataHolder.MainMenu().showLanguage, GUILayout.Width(pw.mWidth));
			if(DataHolder.MainMenu().showLanguage)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical();
				GUILayout.Label("Choice text", EditorStyles.boldLabel);
				for(int i=0; i<DataHolder.MainMenu().languageText.Length; i++)
				{
					DataHolder.MainMenu().languageText[i] = EditorGUILayout.TextField(DataHolder.Languages().GetName(i), 
							DataHolder.MainMenu().languageText[i], GUILayout.Width(pw.mWidth*2));
					EditorGUILayout.Separator();
				}
				EditorGUILayout.EndVertical();
				if(DataHolder.MainMenu().languageIcon == null && 
					DataHolder.MainMenu().languageIconName != null &&
					"" != DataHolder.MainMenu().languageIconName)
				{
					DataHolder.MainMenu().languageIcon = (Texture2D)Resources.Load(DataHolder.MainMenu().iconPath+
							DataHolder.MainMenu().languageIconName, typeof(Texture2D));
				}
				DataHolder.MainMenu().languageIcon = (Texture2D)EditorGUILayout.ObjectField("Icon", DataHolder.MainMenu().languageIcon, typeof(Texture2D), false);
				if(DataHolder.MainMenu().languageIcon) DataHolder.MainMenu().languageIconName = DataHolder.MainMenu().languageIcon.name;
				else DataHolder.MainMenu().languageIconName = "";
				EditorGUILayout.EndHorizontal();
			}
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical("box");
		fold5 = EditorGUILayout.Foldout(fold5, "About");
		if(fold5)
		{
			DataHolder.MainMenu().showAbout = EditorGUILayout.Toggle("Show", DataHolder.MainMenu().showAbout, GUILayout.Width(pw.mWidth));
			if(DataHolder.MainMenu().showAbout)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical();
				GUILayout.Label("Choice text", EditorStyles.boldLabel);
				for(int i=0; i<DataHolder.MainMenu().aboutText.Length; i++)
				{
					DataHolder.MainMenu().aboutText[i] = EditorGUILayout.TextField(DataHolder.Languages().GetName(i), 
							DataHolder.MainMenu().aboutText[i], GUILayout.Width(pw.mWidth*2));
					EditorGUILayout.Separator();
				}
				EditorGUILayout.EndVertical();
				if(DataHolder.MainMenu().aboutIcon == null && 
					DataHolder.MainMenu().aboutIconName != null &&
					"" != DataHolder.MainMenu().aboutIconName)
				{
					DataHolder.MainMenu().aboutIcon = (Texture2D)Resources.Load(DataHolder.MainMenu().iconPath+
							DataHolder.MainMenu().aboutIconName, typeof(Texture2D));
				}
				DataHolder.MainMenu().aboutIcon = (Texture2D)EditorGUILayout.ObjectField("Icon", DataHolder.MainMenu().aboutIcon, typeof(Texture2D), false);
				if(DataHolder.MainMenu().aboutIcon) DataHolder.MainMenu().aboutIconName = DataHolder.MainMenu().aboutIcon.name;
				else DataHolder.MainMenu().aboutIconName = "";
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.Separator();
				DataHolder.MainMenu().aboutPosition = EditorGUILayout.Popup("Info position", DataHolder.MainMenu().aboutPosition, 
						DataHolder.DialoguePositions().GetNameList(true), GUILayout.Width(pw.mWidth));
				GUILayout.Label("About info", EditorStyles.boldLabel);
				for(int i=0; i<DataHolder.MainMenu().aboutInfo.Length; i++)
				{
					GUILayout.Label(DataHolder.Language(i));
					DataHolder.MainMenu().aboutInfo[i] = EditorGUILayout.TextArea(DataHolder.MainMenu().aboutInfo[i], 
							GUILayout.Width(pw.mWidth*2), GUILayout.Height(50));
					EditorGUILayout.Separator();
				}
			}
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical("box");
		fold6 = EditorGUILayout.Foldout(fold6, "Exit");
		if(fold6)
		{
			DataHolder.MainMenu().showExit = EditorGUILayout.Toggle("Show", DataHolder.MainMenu().showExit, GUILayout.Width(pw.mWidth));
			if(DataHolder.MainMenu().showExit)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical();
				GUILayout.Label("Choice text", EditorStyles.boldLabel);
				for(int i=0; i<DataHolder.MainMenu().exitText.Length; i++)
				{
					DataHolder.MainMenu().exitText[i] = EditorGUILayout.TextField(DataHolder.Languages().GetName(i), 
							DataHolder.MainMenu().exitText[i], GUILayout.Width(pw.mWidth*2));
					EditorGUILayout.Separator();
				}
				EditorGUILayout.EndVertical();
				if(DataHolder.MainMenu().exitIcon == null && 
					DataHolder.MainMenu().exitIconName != null &&
					"" != DataHolder.MainMenu().exitIconName)
				{
					DataHolder.MainMenu().aboutIcon = (Texture2D)Resources.Load(DataHolder.MainMenu().iconPath+
							DataHolder.MainMenu().exitIconName, typeof(Texture2D));
				}
				DataHolder.MainMenu().exitIcon = (Texture2D)EditorGUILayout.ObjectField("Icon", DataHolder.MainMenu().exitIcon, typeof(Texture2D), false);
				if(DataHolder.MainMenu().exitIcon) DataHolder.MainMenu().exitIconName = DataHolder.MainMenu().exitIcon.name;
				else DataHolder.MainMenu().exitIconName = "";
				EditorGUILayout.EndHorizontal();
			}
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		
		this.Separate();
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}
}