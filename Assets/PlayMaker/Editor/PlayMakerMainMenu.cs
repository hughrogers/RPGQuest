// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using HutongGames.PlayMakerEditor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

static class PlayMakerMainMenu
{
	[MenuItem("PlayMaker/PlayMaker Editor")]
	public static void OpenFsmEditor()
	{
		FsmEditorWindow.OpenWindow();
	}

	#region EDITOR WINDOWS

	[MenuItem("PlayMaker/Editor Windows/FSM Browser", true)]
	public static bool ValidateOpenFsmSelectorWindow()
	{
		return FsmEditorWindow.IsOpen();
	}

	[MenuItem("PlayMaker/Editor Windows/FSM Browser")]
	public static void OpenFsmSelectorWindow()
	{
		FsmEditor.OpenFsmSelectorWindow();
	}

	[MenuItem("PlayMaker/Editor Windows/State Browser", true)]
	public static bool ValidateOpenStateSelectorWindow()
	{
		return FsmEditorWindow.IsOpen();
	}

	[MenuItem("PlayMaker/Editor Windows/State Browser")]
	public static void OpenStateSelectorWindow()
	{
		FsmEditor.OpenStateSelectorWindow();
	}

	[MenuItem("PlayMaker/Editor Windows/Templates Browser", true)]
	public static bool ValidateOpenFsmTemplateWindow()
	{
		return FsmEditorWindow.IsOpen();
	}

	[MenuItem("PlayMaker/Editor Windows/Templates Browser")]
	public static void OpenFsmTemplateWindow()
	{
		FsmEditor.OpenFsmTemplateWindow();
	}

	[MenuItem("PlayMaker/Editor Windows/Edit Tool Window", true)]
	public static bool ValidateOpenToolWindow()
	{
		return FsmEditorWindow.IsOpen();
	}

	[MenuItem("PlayMaker/Editor Windows/Edit Tool Window")]
	public static void OpenToolWindow()
	{
		FsmEditor.OpenToolWindow();
	}

	[MenuItem("PlayMaker/Editor Windows/Action Browser", true)]
	public static bool ValidateOpenActionWindow()
	{
		return FsmEditorWindow.IsOpen();
	}

	[MenuItem("PlayMaker/Editor Windows/Action Browser")]
	public static void OpenActionWindow()
	{
		FsmEditor.OpenActionWindow();
	}

	[MenuItem("PlayMaker/Editor Windows/Global Variables", true)]
	public static bool ValidateOpenGlobalVariablesWindow()
	{
		return FsmEditorWindow.IsOpen();
	}

	[MenuItem("PlayMaker/Editor Windows/Global Variables")]
	public static void OpenGlobalVariablesWindow()
	{
		FsmEditor.OpenGlobalVariablesWindow();
	}

	[MenuItem("PlayMaker/Editor Windows/Event Browser", true)]
	public static bool ValidateOpenGlobalEventsWindow()
	{
		return FsmEditorWindow.IsOpen();
	}

	[MenuItem("PlayMaker/Editor Windows/Event Browser")]
	public static void OpenGlobalEventsWindow()
	{
		FsmEditor.OpenGlobalEventsWindow();
	}

	[MenuItem("PlayMaker/Editor Windows/Log Window", true)]
	public static bool ValidateOpenFsmLogWindow()
	{
		return FsmEditorWindow.IsOpen();
	}

	[MenuItem("PlayMaker/Editor Windows/Log Window")]
	public static void OpenFsmLogWindow()
	{
		FsmEditor.OpenFsmLogWindow();
	}

	[MenuItem("PlayMaker/Editor Windows/Console", true)]
	public static bool ValidateOpenReportWindow()
	{
		return FsmEditorWindow.IsOpen();
	}

	[MenuItem("PlayMaker/Editor Windows/Console")]
	public static void OpenReportWindow()
	{
		FsmEditor.OpenReportWindow();
	}

	#endregion

	//[MenuItem("PlayMaker/")]

	#region COMPONENTS

	//[MenuItem("Assets/Add PlayMakerFSM", true)]
	//[MenuItem("GameObject/Add PlayMakerFSM", true)]
	[MenuItem("PlayMaker/Components/Add FSM To Selected Objects", true)]
	public static bool ValidateAddFsmToSelected()
	{
		return Selection.activeGameObject != null;
	}

	//[MenuItem("Assets/Add PlayMakerFSM", false, 100)]
	//[MenuItem("GameObject/Add PlayMakerFSM", false, 10000)]
	[MenuItem("PlayMaker/Components/Add FSM To Selected Objects")]
	public static void AddFsmToSelected()
	{
		FsmBuilder.AddFsmToSelected();
		//PlayMakerFSM playmakerFSM = Selection.activeGameObject.AddComponent<PlayMakerFSM>();
		//FsmEditor.SelectFsm(playmakerFSM.Fsm);
	}

	[MenuItem("PlayMaker/Components/Add PlayMakerGUI to Scene", true)]
	public static bool ValidateAddPlayMakerGUI()
	{
		return (Object.FindObjectOfType(typeof(PlayMakerGUI)) as PlayMakerGUI) == null;
	}

	[MenuItem("PlayMaker/Components/Add PlayMakerGUI to Scene")]
	public static void AddPlayMakerGUI()
	{
		PlayMakerGUI.Instance.enabled = true;
	}

	#endregion

	#region TOOLS

	[MenuItem("PlayMaker/Tools/Load All PlayMaker Prefabs In Project")]
	public static void LoadAllPrefabsInProject()
	{
		var paths = FsmEditorUtility.LoadAllPrefabsInProject();
		var output = "";

		foreach (var path in paths)
		{
			output += path + "\n";
		}

		if (output == "")
		{
			EditorUtility.DisplayDialog("Loading PlayMaker Prefabs", "No PlayMaker Prefabs Found!", "OK");
		}
		else
		{
			EditorUtility.DisplayDialog("Loaded PlayMaker Prefabs", output, "OK");
		}
	}

	[MenuItem("PlayMaker/Tools/Custom Action Wizard")]
	private static void CreateWizard()
	{
		EditorWindow.GetWindow<PlayMakerCustomActionWizard>(true);
	}

	[MenuItem("PlayMaker/Tools/Export Globals")]
	private static void ExportGlobals()
	{
		FsmEditorUtility.ExportGlobals();
	}

	
	[MenuItem("PlayMaker/Tools/Import Globals")]
	private static void ImportGlobals()
	{
		FsmEditorUtility.ImportGlobals();
	}

	[MenuItem("PlayMaker/Tools/Documentation Helpers")]
	private static void DocHelpers()
	{
		EditorWindow.GetWindow<PlayMakerDocHelpers>(true);
	}


	#endregion

	#region DOCUMENTATION

	[MenuItem("PlayMaker/Online Resources/HutongGames")]
	public static void HutongGames()
	{
		Application.OpenURL("http://www.hutonggames.com/");
	}

	[MenuItem("PlayMaker/Online Resources/Online Manual")]
	public static void OnlineManual()
	{
		EditorCommands.OpenWikiHelp();
		//Application.OpenURL("https://hutonggames.fogbugz.com/default.asp?W1");
	}

	[MenuItem("PlayMaker/Online Resources/Video Tutorials")]
	public static void VideoTutorials()
	{
		Application.OpenURL("http://www.screencast.com/users/HutongGames/folders/PlayMaker");
	}

	[MenuItem("PlayMaker/Online Resources/YouTube Channel")]
	public static void YouTubeChannel()
	{
		Application.OpenURL("http://www.youtube.com/user/HutongGamesLLC");
	}

	[MenuItem("PlayMaker/Online Resources/PlayMaker Forums")]
	public static void PlayMakerForum()
	{
		Application.OpenURL("http://hutonggames.com/playmakerforum/");
	}

	//[MenuItem("PlayMaker/Documentation/")]
	[MenuItem("PlayMaker/Online Resources/Release Notes")]
	public static void ReleaseNotes()
	{
		EditorCommands.OpenWikiPage(WikiPages.ReleaseNotes);
		//Application.OpenURL("https://hutonggames.fogbugz.com/default.asp?W311");
	}

	#endregion

	[MenuItem("PlayMaker/Submit Bug Report")]
	private static void SubmitBug()
	{
		EditorWindow.GetWindow<PlayMakerBugReportWindow>(true);
	}

	[MenuItem("PlayMaker/Welcome Screen")]
	public static void OpenWelcomeWindow()
	{
		EditorWindow.GetWindow<PlayMakerWelcomeWindow>(true);
	}

	//http://u3d.as/content/hutong-games-llc/playmaker/1Az

/*	[MenuItem("PlayMaker/Check For Updates")]
	public static void CheckForUpdates()
	{
		AssetStore.Open("1z");
	}*/

	[MenuItem("PlayMaker/About PlayMaker...")]
	public static void OpenAboutWindow()
	{
		EditorWindow.GetWindow<AboutWindow>(true);
	}
}
