// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using HutongGames.PlayMakerEditor;
using UnityEditor;
using UnityEngine;

// You can't open an EditorWindow class in a C# Dll (as far as I can tell)
// So we use a wrapper script to create the window and hook the editor up
// TODO: move this to dll when Unity supports it...

[System.Serializable]
class FsmEditorWindow : EditorWindow
{
	static FsmEditorWindow instance;

	[SerializeField]
	FsmEditor fsmEditor;
	
	// tool windows (can't open them inside dll)

	//[SerializeField] PlayMakerWelcomeWindow welcomeWindow;
	[SerializeField] FsmSelectorWindow fsmSelectorWindow;
	[SerializeField] FsmTemplateWindow fsmTemplateWindow;
	[SerializeField] FsmStateWindow stateSelectorWindow;
	[SerializeField] FsmActionWindow actionWindow;
	[SerializeField] FsmErrorWindow errorWindow;
	[SerializeField] FsmLogWindow logWindow;
	[SerializeField] ContextToolWindow toolWindow;
	[SerializeField] GlobalEventsWindow globalEventsWindow;
	[SerializeField] GlobalVariablesWindow globalVariablesWindow;
	[SerializeField] ReportWindow reportWindow;
	[SerializeField] AboutWindow aboutWindow;

	/// <summary>
	/// Open the Fsm Editor and optionally show the Welcome Screen
	/// </summary>
	public static void OpenWindow()
	{
		GetWindow<FsmEditorWindow>();

		if (EditorPrefs.GetBool("PlayMaker.ShowWelcomeScreen", true))
		{
			GetWindow<PlayMakerWelcomeWindow>(true);
		}
	}

	/// <summary>
	/// Open the Fsm Editor and select an Fsm Component
	/// </summary>
	public static void OpenWindow(PlayMakerFSM fsmComponent)
	{
		OpenWindow();

		FsmEditor.SelectFsm(fsmComponent.Fsm);
	}

	/// <summary>
	/// Is the Fsm Editor open?
	/// </summary>
	public static bool IsOpen()
	{
		return instance != null;
	}

	// ReSharper disable UnusedMember.Local

	/// <summary>
	/// Called when the Fsm Editor window is created
	/// NOTE: happens on playmode change and recompile!
	/// </summary>

	void OnEnable()
	{
		instance = this;

		if (fsmEditor == null)
		{
			fsmEditor = new FsmEditor();
		}
		
		fsmEditor.InitWindow(this);
		fsmEditor.OnEnable();
	}
	
	/// <summary>
	/// Do the GUI
	/// </summary>
	void OnGUI()
	{
		fsmEditor.OnGUI();
		
/*		BeginWindows();
		
		fsmEditor.DoPopupWindows();
		
		EndWindows();*/
		
		if (Event.current.type == EventType.ValidateCommand)
    	{
			//Debug.Log(Event.current.commandName);

			switch (Event.current.commandName)
			{
				case "UndoRedoPerformed":
					Event.current.Use();
					break;
				
				case "Copy":
					Event.current.Use();
					break;
					
				case "Paste":
					Event.current.Use();
					break;
				
				case "SelectAll":
					Event.current.Use();
					break;
			}
    	}
		
		if (Event.current.type == EventType.ExecuteCommand)
		{
			
			//Debug.Log(Event.current.commandName);
			
			switch (Event.current.commandName)
			{
				case "UndoRedoPerformed":
					FsmEditor.UndoRedoPerformed();
					break;
				
				case "Copy":
					FsmEditor.Copy();
					break;
					
				case "Paste":
					FsmEditor.Paste();
					break;
				
				case "SelectAll":
					FsmEditor.SelectAll();
					break;

				case "OpenWelcomeWindow":
					GetWindow<PlayMakerWelcomeWindow>();
					break;

				case "OpenToolWindow":
					toolWindow = GetWindow<ContextToolWindow>();
					break;
				
				case "OpenFsmSelectorWindow":
					fsmSelectorWindow = GetWindow<FsmSelectorWindow>();
					fsmSelectorWindow.ShowUtility();
					break;
				
				case "OpenFsmTemplateWindow":
					fsmTemplateWindow = GetWindow<FsmTemplateWindow>();
					break;
				
				case "OpenStateSelectorWindow":
					stateSelectorWindow = GetWindow<FsmStateWindow>();
					break;
				
				case "OpenActionWindow":
					actionWindow = GetWindow<FsmActionWindow>();
					break;
				
				case "OpenGlobalEventsWindow":
					globalEventsWindow = GetWindow<FsmEventsWindow>();
					break;

				case "OpenGlobalVariablesWindow":
					globalVariablesWindow = GetWindow<FsmGlobalsWindow>();
					break;

				case "OpenErrorWindow":
					errorWindow = GetWindow<FsmErrorWindow>();
					break;
				
				case "OpenFsmLogWindow":
					logWindow = GetWindow<FsmLogWindow>();
					break;
				
				case "OpenAboutWindow":
					aboutWindow = GetWindow<AboutWindow>();
					break;
				
				case "OpenReportWindow":
					reportWindow = GetWindow<ReportWindow>();
					break;
				
				case "AddFsmComponent":
					PlayMakerMainMenu.AddFsmToSelected();
					break;
				
				case "RepaintAll":
					RepaintAllWindows();
					break;
			}
	
			GUIUtility.ExitGUI();
		}
	
	}

	public void RepaintAllWindows()
	{
		if (toolWindow != null)
		{
			toolWindow.Repaint();
		}
		
		if (fsmSelectorWindow != null)
		{
			fsmSelectorWindow.Repaint();
		}
		
		if (stateSelectorWindow != null)
		{
			stateSelectorWindow.Repaint();
		}

		if (actionWindow != null)
		{
			actionWindow.Repaint();
		}

		if (globalEventsWindow != null)
		{
			globalEventsWindow.Repaint();
		}

		if (globalVariablesWindow != null)
		{
			globalVariablesWindow.Repaint();
		}

		if (errorWindow != null)
		{
			errorWindow.Repaint();
		}
	
		if (logWindow != null)
		{
			logWindow.Repaint();
		}

		if (reportWindow != null)
		{
			reportWindow.Repaint();
		}

		Repaint();
	}

	void Update()
	{
		fsmEditor.Update();
	}

	void OnInspectorUpdate()
	{
		fsmEditor.OnInspectorUpdate();
	}

	void OnFocus()
	{
		fsmEditor.OnFocus();
	}

	void OnSelectionChange()
	{
		fsmEditor.OnSelectionChange();
	}

	void OnHierarchyChange()
	{
		fsmEditor.OnHierarchyChange();
	}

	void OnProjectChange()
	{
		if (fsmEditor != null)
		{
			fsmEditor.OnProjectChange();
		}
	}

	void OnDisable()
	{
		if (fsmEditor != null)
		{
			fsmEditor.OnDisable();
		}

		instance = null;
	}

	void OnDestroy()
	{
		if (toolWindow != null)
		{
			toolWindow.Close();
		}
		
		if (fsmSelectorWindow != null)
		{
			fsmSelectorWindow.Close();
		}
		
		if (fsmTemplateWindow != null)
		{
			fsmTemplateWindow.Close();
		}

		if (stateSelectorWindow != null)
		{
			stateSelectorWindow.Close();
		}

		if (actionWindow != null)
		{
			actionWindow.Close();
		}

		if (globalVariablesWindow != null)
		{
			globalVariablesWindow.Close();
		}

		if (globalEventsWindow != null)
		{
			globalEventsWindow.Close();
		}
		
		if (errorWindow != null)
		{
			errorWindow.Close();
		}

		if (logWindow != null)
		{
			logWindow.Close();
		}

		if (reportWindow != null)
		{
			reportWindow.Close();
		}

		if (aboutWindow != null)
		{
			aboutWindow.Close();
		}
	}

	// ReSharper restore UnusedMember.Local
}

