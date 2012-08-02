
using UnityEditor;
using UnityEngine;

public class LoadSaveHUDTab : BaseTab
{
	public LoadSaveHUDTab(ProjectWindow pw) : base(pw)
	{
		this.Reload();
	}
	
	new public void Reload()
	{
		base.Reload();
	}
	
	public void ShowTab()
	{
		EditorGUILayout.BeginVertical();
		SP1 = EditorGUILayout.BeginScrollView(SP1);
		EditorGUILayout.Separator();
		
		EditorGUILayout.BeginVertical("box");
		fold1 = EditorGUILayout.Foldout(fold1, "General settings");
		if(fold1)
		{
			DataHolder.LoadSaveHUD().maxSaveGames = EditorGUILayout.IntField("Max save games", 
					DataHolder.LoadSaveHUD().maxSaveGames, GUILayout.Width(pw.mWidth));
			DataHolder.LoadSaveHUD().saveGameType = (SaveGameType)EditorGUILayout.EnumPopup("Save to", 
					DataHolder.LoadSaveHUD().saveGameType, GUILayout.Width(pw.mWidth));
			DataHolder.LoadSaveHUD().encryptData = EditorGUILayout.Toggle("Encrypt data", 
					DataHolder.LoadSaveHUD().encryptData, GUILayout.Width(pw.mWidth));
			
			EditorGUILayout.Separator();
			DataHolder.LoadSaveHUD().savePosition = EditorGUILayout.Popup("Position", DataHolder.LoadSaveHUD().savePosition, 
					DataHolder.DialoguePositions().GetNameList(true), GUILayout.Width(pw.mWidth*1.5f));
			DataHolder.LoadSaveHUD().saveQuestionPosition = EditorGUILayout.Popup("Question pos", DataHolder.LoadSaveHUD().saveQuestionPosition, 
					DataHolder.DialoguePositions().GetNameList(true), GUILayout.Width(pw.mWidth*1.5f));
			
			EditorGUILayout.Separator();
			GUILayout.Label("Load game fading", EditorStyles.boldLabel);
			DataHolder.LoadSaveHUD().fadeOut = EditorGUILayout.Toggle("Fade out", DataHolder.LoadSaveHUD().fadeOut, GUILayout.Width(pw.mWidth));
			if(DataHolder.LoadSaveHUD().fadeOut)
			{
				DataHolder.LoadSaveHUD().fadeOutTime = EditorGUILayout.FloatField("Time", 
						DataHolder.LoadSaveHUD().fadeOutTime, GUILayout.Width(pw.mWidth));
				DataHolder.LoadSaveHUD().fadeOutInterpolate = (EaseType)EditorGUILayout.EnumPopup("Interpolation", 
						DataHolder.LoadSaveHUD().fadeOutInterpolate, GUILayout.Width(pw.mWidth));
				EditorGUILayout.Separator();
			}
			DataHolder.LoadSaveHUD().fadeIn = EditorGUILayout.Toggle("Fade in", 
					DataHolder.LoadSaveHUD().fadeIn, GUILayout.Width(pw.mWidth));
			if(DataHolder.LoadSaveHUD().fadeIn)
			{
				DataHolder.LoadSaveHUD().fadeInTime = EditorGUILayout.FloatField("Time", 
						DataHolder.LoadSaveHUD().fadeInTime, GUILayout.Width(pw.mWidth));
				DataHolder.LoadSaveHUD().fadeInInterpolate = (EaseType)EditorGUILayout.EnumPopup("Interpolation", 
						DataHolder.LoadSaveHUD().fadeInInterpolate, GUILayout.Width(pw.mWidth));
				EditorGUILayout.Separator();
			}
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical("box");
		fold6 = EditorGUILayout.Foldout(fold6, "Auto save settings");
		if(fold6)
		{
			GUILayout.Label("File index text", EditorStyles.boldLabel);
			for(int i=0; i<DataHolder.LoadSaveHUD().autoFileName.Length; i++)
			{
				DataHolder.LoadSaveHUD().autoFileName[i] = EditorGUILayout.TextField(DataHolder.Languages().GetName(i), 
						DataHolder.LoadSaveHUD().autoFileName[i], GUILayout.Width(pw.mWidth*2));
			}
			
			EditorGUILayout.Separator();
			DataHolder.LoadSaveHUD().showAutoSaveMessage = EditorGUILayout.Toggle("Show message", 
					DataHolder.LoadSaveHUD().showAutoSaveMessage, GUILayout.Width(pw.mWidth));
			
			if(DataHolder.LoadSaveHUD().showAutoSaveMessage)
			{
				DataHolder.LoadSaveHUD().autoMessagePosition = EditorGUILayout.Popup("Position", 
						DataHolder.LoadSaveHUD().autoMessagePosition, 
						DataHolder.DialoguePositions().GetNameList(true), GUILayout.Width(pw.mWidth*1.5f));
				DataHolder.LoadSaveHUD().autoVisibilityTime = EditorGUILayout.FloatField("Visibility time", 
						DataHolder.LoadSaveHUD().autoVisibilityTime, GUILayout.Width(pw.mWidth));
				
				GUILayout.Label("Auto save message", EditorStyles.boldLabel);
				for(int i=0; i<DataHolder.LoadSaveHUD().autoSaveMessage.Length; i++)
				{
					DataHolder.LoadSaveHUD().autoSaveMessage[i] = EditorGUILayout.TextField(DataHolder.Languages().GetName(i), 
							DataHolder.LoadSaveHUD().autoSaveMessage[i], GUILayout.Width(pw.mWidth*2));
				}
			}
			
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical("box");
		fold5 = EditorGUILayout.Foldout(fold5, "Save settings");
		if(fold5)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			GUILayout.Label("Game", EditorStyles.boldLabel);
			DataHolder.LoadSaveHUD().saveStatistics = EditorGUILayout.Toggle("Statistics", 
					DataHolder.LoadSaveHUD().saveStatistics, GUILayout.Width(pw.mWidth));
			DataHolder.LoadSaveHUD().saveTime = EditorGUILayout.Toggle("Game time", 
					DataHolder.LoadSaveHUD().saveTime, GUILayout.Width(pw.mWidth));
			DataHolder.LoadSaveHUD().saveParty = EditorGUILayout.Toggle("Party", 
					DataHolder.LoadSaveHUD().saveParty, GUILayout.Width(pw.mWidth));
			if(DataHolder.LoadSaveHUD().saveParty)
			{
				DataHolder.LoadSaveHUD().savePartyPosition = EditorGUILayout.Toggle("Position", 
						DataHolder.LoadSaveHUD().savePartyPosition, GUILayout.Width(pw.mWidth));
			}
			else DataHolder.LoadSaveHUD().savePartyPosition = false;
			if(!DataHolder.LoadSaveHUD().savePartyPosition)
			{
				DataHolder.LoadSaveHUD().saveSceneName = EditorGUILayout.TextField("Load scene", 
						DataHolder.LoadSaveHUD().saveSceneName, GUILayout.Width(pw.mWidth));
				DataHolder.LoadSaveHUD().saveSpawnID = EditorGUILayout.IntField("Spawn ID", 
						DataHolder.LoadSaveHUD().saveSpawnID, GUILayout.Width(pw.mWidth));
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical();
			GUILayout.Label("Inventory", EditorStyles.boldLabel);
			DataHolder.LoadSaveHUD().saveItems = EditorGUILayout.Toggle("Items", 
					DataHolder.LoadSaveHUD().saveItems, GUILayout.Width(pw.mWidth));
			DataHolder.LoadSaveHUD().saveWeapons = EditorGUILayout.Toggle("Weapons", 
					DataHolder.LoadSaveHUD().saveWeapons, GUILayout.Width(pw.mWidth));
			DataHolder.LoadSaveHUD().saveArmors = EditorGUILayout.Toggle("Armors", 
					DataHolder.LoadSaveHUD().saveArmors, GUILayout.Width(pw.mWidth));
			DataHolder.LoadSaveHUD().saveRecipes = EditorGUILayout.Toggle("Recipes", 
					DataHolder.LoadSaveHUD().saveRecipes, GUILayout.Width(pw.mWidth));
			DataHolder.LoadSaveHUD().saveMoney = EditorGUILayout.Toggle("Money", 
					DataHolder.LoadSaveHUD().saveMoney, GUILayout.Width(pw.mWidth));
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical();
			GUILayout.Label("Game variables", EditorStyles.boldLabel);
			DataHolder.LoadSaveHUD().gameVariableSelector = (Selector)EditorGUILayout.EnumPopup("Game variables", 
					DataHolder.LoadSaveHUD().gameVariableSelector, GUILayout.Width(pw.mWidth));
			if(!Selector.NONE.Equals(DataHolder.LoadSaveHUD().gameVariableSelector))
			{
				if(Selector.SELECT.Equals(DataHolder.LoadSaveHUD().gameVariableSelector))
				{
					GUILayout.Label("Include game variables", EditorStyles.boldLabel);
				}
				else if(Selector.ALL.Equals(DataHolder.LoadSaveHUD().gameVariableSelector))
				{
					GUILayout.Label("Exclude game variables", EditorStyles.boldLabel);
				}
				
				if(GUILayout.Button("Add variable", GUILayout.Width(pw.mWidth3)))
				{
					DataHolder.LoadSaveHUD().gameVariableList = ArrayHelper.Add("", 
							DataHolder.LoadSaveHUD().gameVariableList);
				}
				for(int i=0; i<DataHolder.LoadSaveHUD().gameVariableList.Length; i++)
				{
					EditorGUILayout.BeginHorizontal();
					if(GUILayout.Button("Remove", GUILayout.Width(pw.mWidth3)))
					{
						DataHolder.LoadSaveHUD().gameVariableList = ArrayHelper.Remove(i, 
								DataHolder.LoadSaveHUD().gameVariableList);
						break;
					}
					DataHolder.LoadSaveHUD().gameVariableList[i] = EditorGUILayout.TextField(
							DataHolder.LoadSaveHUD().gameVariableList[i], GUILayout.Width(pw.mWidth));
					EditorGUILayout.EndHorizontal();
				}
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical();
			GUILayout.Label("Number variables", EditorStyles.boldLabel);
			DataHolder.LoadSaveHUD().numberVariableSelector = (Selector)EditorGUILayout.EnumPopup("Number variables", 
					DataHolder.LoadSaveHUD().numberVariableSelector, GUILayout.Width(pw.mWidth));
			if(!Selector.NONE.Equals(DataHolder.LoadSaveHUD().numberVariableSelector))
			{
				if(Selector.SELECT.Equals(DataHolder.LoadSaveHUD().numberVariableSelector))
				{
					GUILayout.Label("Include number variables", EditorStyles.boldLabel);
				}
				else if(Selector.ALL.Equals(DataHolder.LoadSaveHUD().numberVariableSelector))
				{
					GUILayout.Label("Exclude number variables", EditorStyles.boldLabel);
				}
				
				if(GUILayout.Button("Add variable", GUILayout.Width(pw.mWidth3)))
				{
					DataHolder.LoadSaveHUD().numberVariableList = ArrayHelper.Add("", 
							DataHolder.LoadSaveHUD().numberVariableList);
				}
				for(int i=0; i<DataHolder.LoadSaveHUD().numberVariableList.Length; i++)
				{
					EditorGUILayout.BeginHorizontal();
					if(GUILayout.Button("Remove", GUILayout.Width(pw.mWidth3)))
					{
						DataHolder.LoadSaveHUD().numberVariableList = ArrayHelper.Remove(i, 
								DataHolder.LoadSaveHUD().numberVariableList);
						break;
					}
					DataHolder.LoadSaveHUD().numberVariableList[i] = EditorGUILayout.TextField(
							DataHolder.LoadSaveHUD().numberVariableList[i], GUILayout.Width(pw.mWidth));
					EditorGUILayout.EndHorizontal();
				}
			}
			EditorGUILayout.EndVertical();
			
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical("box");
		fold2 = EditorGUILayout.Foldout(fold2, "Text/icon settings");
		if(fold2)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			for(int i=0; i<DataHolder.LoadSaveHUD().saveText.Length; i++)
			{
				GUILayout.Label(DataHolder.Languages().GetName(i), EditorStyles.boldLabel);
				DataHolder.LoadSaveHUD().saveText[i] = EditorGUILayout.TextField("Save text", 
						DataHolder.LoadSaveHUD().saveText[i], GUILayout.Width(pw.mWidth*2));
				DataHolder.LoadSaveHUD().loadText[i] = EditorGUILayout.TextField("Load text", 
						DataHolder.LoadSaveHUD().loadText[i], GUILayout.Width(pw.mWidth*2));
				DataHolder.LoadSaveHUD().saveYesText[i] = EditorGUILayout.TextField("Yes text", 
						DataHolder.LoadSaveHUD().saveYesText[i], GUILayout.Width(pw.mWidth*2));
				DataHolder.LoadSaveHUD().saveNoText[i] = EditorGUILayout.TextField("No text", 
						DataHolder.LoadSaveHUD().saveNoText[i], GUILayout.Width(pw.mWidth*2));
				DataHolder.LoadSaveHUD().saveQuestionText[i] = EditorGUILayout.TextField("Save question", 
						DataHolder.LoadSaveHUD().saveQuestionText[i], GUILayout.Width(pw.mWidth*2));
				DataHolder.LoadSaveHUD().loadQuestionText[i] = EditorGUILayout.TextField("Load question", 
						DataHolder.LoadSaveHUD().loadQuestionText[i], GUILayout.Width(pw.mWidth*2));
				EditorGUILayout.Separator();
			}
			EditorGUILayout.EndVertical();
			if(DataHolder.LoadSaveHUD().fileIcon == null && 
				DataHolder.LoadSaveHUD().fileIconName != null &&
				"" != DataHolder.LoadSaveHUD().fileIconName)
			{
				DataHolder.LoadSaveHUD().fileIcon = (Texture2D)Resources.Load(DataHolder.LoadSaveHUD().iconPath+
						DataHolder.LoadSaveHUD().fileIconName, typeof(Texture2D));
			}
			DataHolder.LoadSaveHUD().fileIcon =(Texture2D)EditorGUILayout.ObjectField("File icon", 
					DataHolder.LoadSaveHUD().fileIcon, typeof(Texture2D), false);
			if(DataHolder.LoadSaveHUD().fileIcon)
			{
				DataHolder.LoadSaveHUD().fileIconName = DataHolder.LoadSaveHUD().fileIcon.name;
			}
			else DataHolder.LoadSaveHUD().fileIconName = "";
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical("box");
		fold3 = EditorGUILayout.Foldout(fold3, "Layout settings");
		if(fold3)
		{
			GUILayout.Label("%i = file number");
			GUILayout.Label("%n = player name, %l = player level");
			GUILayout.Label("%a = area name, %t = game time");
			EditorGUILayout.Separator();
			GUILayout.Label("File info", EditorStyles.boldLabel);
			for(int i=0; i<DataHolder.LoadSaveHUD().fileInfoLayout.Length; i++)
			{
				DataHolder.LoadSaveHUD().fileInfoLayout[i] = EditorGUILayout.TextField(DataHolder.Languages().GetName(i), DataHolder.LoadSaveHUD().fileInfoLayout[i], GUILayout.Width(pw.mWidth*2));
			}
			EditorGUILayout.Separator();
			GUILayout.Label("Empty file info", EditorStyles.boldLabel);
			for(int i=0; i<DataHolder.LoadSaveHUD().emptyInfoLayout.Length; i++)
			{
				DataHolder.LoadSaveHUD().emptyInfoLayout[i] = EditorGUILayout.TextField(DataHolder.Languages().GetName(i), DataHolder.LoadSaveHUD().emptyInfoLayout[i], GUILayout.Width(pw.mWidth*2));
			}
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical("box");
		fold4 = EditorGUILayout.Foldout(fold4, "Save point settings");
		if(fold4)
		{
			DataHolder.LoadSaveHUD().showChoice = EditorGUILayout.Toggle("Show choice", DataHolder.LoadSaveHUD().showChoice, GUILayout.Width(pw.mWidth));
			DataHolder.LoadSaveHUD().spPosition = EditorGUILayout.Popup("Position", DataHolder.LoadSaveHUD().spPosition, 
					DataHolder.DialoguePositions().GetNameList(true), GUILayout.Width(pw.mWidth));
			DataHolder.LoadSaveHUD().showLoad = EditorGUILayout.Toggle("Show load", DataHolder.LoadSaveHUD().showLoad, GUILayout.Width(pw.mWidth));
			
			EditorGUILayout.Separator();
			for(int i=0; i<DataHolder.LoadSaveHUD().spSaveText.Length; i++)
			{
				GUILayout.Label(DataHolder.Languages().GetName(i), EditorStyles.boldLabel);
				DataHolder.LoadSaveHUD().spSaveText[i] = EditorGUILayout.TextField("Save choice", DataHolder.LoadSaveHUD().spSaveText[i], GUILayout.Width(pw.mWidth*2));
				DataHolder.LoadSaveHUD().spLoadText[i] = EditorGUILayout.TextField("Load choice", DataHolder.LoadSaveHUD().spLoadText[i], GUILayout.Width(pw.mWidth*2));
				DataHolder.LoadSaveHUD().spCancelText[i] = EditorGUILayout.TextField("Cancel choice", DataHolder.LoadSaveHUD().spCancelText[i], GUILayout.Width(pw.mWidth*2));
				EditorGUILayout.Separator();
			}
		}
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.Separator();
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}
}