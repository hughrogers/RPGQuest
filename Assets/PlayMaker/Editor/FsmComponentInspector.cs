// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

// comment out to compile out iTween editing functions
// E.g., if you've removed iTween actions.
// Can't put this in iTween actions since this is compiled first...
#define iTweenPathEditing

using UnityEditor;
using UnityEngine;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;
using System.Collections.Generic;

/// <summary>
/// Custom inspector for PlayMakerFSM
/// </summary>
[CustomEditor(typeof(PlayMakerFSM))]
public class FsmComponentInspector : Editor
{
	const float indent = 20;
	
	/// <summary>
	/// Inspector target
	/// </summary>
	PlayMakerFSM fsmComponent;

	// Label for watermark button
	[System.NonSerialized]
	private GUIContent watermarkLabel;

	public GUIContent WatermarkLabel
	{
		get
		{
			return watermarkLabel ?? (watermarkLabel = new GUIContent
			                                           	{
			                                           		text = FsmEditorUtility.GetWatermarkLabel(fsmComponent, "Choose a watermark image..."),
			                                           		tooltip = "Choose a watermark image to help visually identify the FSM.\nNOTE: Watermarks can be turned off in Preferences."
			                                           	});
		}
	}

	// Foldout states

	private bool showControls = true;
	private bool showInfo;
	private bool showStates;
	private bool showEvents;
	private bool showVariables;
	
	// Fsm Variables

	List<FsmVariable> fsmVariables = new List<FsmVariable>();
	
	void OnEnable()
	{
		fsmComponent = target as PlayMakerFSM;
		
		// can happen when playmaker is updated
		if (fsmComponent != null)
		{
			BuildFsmVariableList();
		}
	}

	public override void OnInspectorGUI()
	{
		//EditorGUIUtility.LookLikeControls();

		// can happen when playmaker is updated...?

		if (fsmComponent == null)
		{
			return;
		}

		var fsm = fsmComponent.Fsm;

		// Make sure common PlayMaker styles are initialized
		// TODO: Remove this dependency so Inspector is lighter weight?

		if (!FsmEditorStyles.IsInitialized())
		{
			FsmEditorStyles.Init();
		}
		
		// Begin GUI

		EditorGUILayout.BeginHorizontal();

		// Edit FSM name

		fsm.Name = EditorGUILayout.TextField(fsm.Name);

		// Open PlayMaker editor button

		if (GUILayout.Button(new GUIContent("Edit","Edit in the PlayMaker FSM Editor"), GUILayout.MaxWidth(45)))
		{
			OpenInEditor(fsmComponent);
			GUIUtility.ExitGUI();
		}

		EditorGUILayout.EndHorizontal();
		
		// Description

		fsm.Description = FsmEditorGUILayout.TextAreaWithHint(fsm.Description, "Description...", GUILayout.MinHeight(60));

		// Help Url

		EditorGUILayout.BeginHorizontal();

		fsm.DocUrl = FsmEditorGUILayout.TextFieldWithHint(fsm.DocUrl, "Documentation Url...");

		var guiEnabled = GUI.enabled;

		if (string.IsNullOrEmpty(fsm.DocUrl))
		{
			GUI.enabled = false;
		}

		if (FsmEditorGUILayout.HelpButton())
		{
			Application.OpenURL(fsm.DocUrl);
		}

		EditorGUILayout.EndHorizontal();

		GUI.enabled = guiEnabled;

		// Basic Settings

/*		if (GUILayout.Button(WatermarkLabel, EditorStyles.popup))
		{
			GenerateWatermarkMenu().ShowAsContext();
		}
*/
		fsm.RestartOnEnable = GUILayout.Toggle(fsm.RestartOnEnable,
												new GUIContent("Reset On Disable",
															   "Should the FSM reset or keep its current state on Enable/Disable. Uncheck this if you want to pause an FSM by enabling/disabling the PlayMakerFSM component."));

		fsm.ShowStateLabel = GUILayout.Toggle(fsm.ShowStateLabel, new GUIContent("Show State Label","Show active state label in game view.\nNOTE: Requires PlayMakerGUI in scene"));

		fsm.EnableDebugFlow = GUILayout.Toggle(fsm.EnableDebugFlow, new GUIContent("Enable Debug Flow", "Enable caching of variables and other state info while the game is running. NOTE: Disabling this can improve performance in the editor (it is always disabled in standalone builds)."));


		// VARIABLES

		FsmEditorGUILayout.LightDivider();
		showControls = EditorGUILayout.Foldout(showControls, new GUIContent("Controls", "FSM Variables and Events exposed in the Inspector."), FsmEditorStyles.CategoryFoldout);

		if (showControls)
		{
			//EditorGUIUtility.LookLikeInspector();

			BuildFsmVariableList();

			foreach (var fsmVar in fsmVariables)
			{
				if (fsmVar.ShowInInspector)
				{
					fsmVar.DoValueGUI(new GUIContent(fsmVar.Name, fsmVar.Name + (!string.IsNullOrEmpty(fsmVar.Tooltip) ? ":\n" + fsmVar.Tooltip : "")));
				}
			}

			if (GUI.changed)
			{
				FsmEditor.RepaintAll();
			}
		}

		// EVENTS

		//FsmEditorGUILayout.LightDivider();
		//showExposedEvents = EditorGUILayout.Foldout(showExposedEvents, new GUIContent("Events", "To expose events here:\nIn PlayMaker Editor, Events tab, select an event and check Inspector."), FsmEditorStyles.CategoryFoldout);

		if (showControls)
		{
			EditorGUI.indentLevel = 1;

			//GUI.enabled = Application.isPlaying;

			foreach (var fsmEvent in fsm.ExposedEvents)
			{
				if (GUILayout.Button(fsmEvent.Name))
				{
					fsm.Event(fsmEvent);
				}
			}

			if (GUI.changed)
			{
				FsmEditor.RepaintAll();
			}
		}

		//GUI.enabled = true;

		//INFO

		EditorGUI.indentLevel = 0;

		FsmEditorGUILayout.LightDivider();
		showInfo = EditorGUILayout.Foldout(showInfo, "Info", FsmEditorStyles.CategoryFoldout);

		if (showInfo)
		{
			EditorGUI.indentLevel = 1;

			//FsmEditorGUILayout.LightDivider();
			//GUILayout.Label("Summary", EditorStyles.boldLabel);
		
			showStates = EditorGUILayout.Foldout(showStates, "States [" + fsmComponent.FsmStates.Length + "]");
			if (showStates)
			{
				string states = "";
				
				if (fsmComponent.FsmStates.Length > 0)
				{
					foreach (var state in fsmComponent.FsmStates)
					{
						states += "\t\t" + state.Name + "\n";
					}
					states = states.Substring(0,states.Length-1);
				}
				else
				{
					states = "\t\t[none]";
				}
				
				GUILayout.Label(states);
			}
			
			showEvents = EditorGUILayout.Foldout(showEvents, "Events [" + fsmComponent.FsmEvents.Length + "]");
			if (showEvents) 
			{
				string events = "";
				
				if (fsmComponent.FsmEvents.Length > 0)
				{
					foreach (var fsmEvent in fsmComponent.FsmEvents)
					{
						events += "\t\t" + fsmEvent.Name + "\n";
					}
					events = events.Substring(0,events.Length-1);
				}
				else
				{
					events = "\t\t[none]";
				}
				
				GUILayout.Label(events);
			}
			
			showVariables = EditorGUILayout.Foldout(showVariables, "Variables [" + fsmVariables.Count + "]");
			if (showVariables)
			{
				string variables = "";
				
				if (fsmVariables.Count > 0)
				{
					foreach (var fsmVar in fsmVariables)
					{
						variables += "\t\t" + fsmVar.Name + "\n";
					}
					variables = variables.Substring(0,variables.Length-1);
				}
				else
				{
					variables = "\t\t[none]";
				}
				
				GUILayout.Label(variables);
			}
		}
	}

	public static void OpenInEditor(PlayMakerFSM fsmComponent)
	{
		if (FsmEditor.Instance == null)
		{
			FsmEditorWindow.OpenWindow(fsmComponent);
		}
		else
		{
			FsmEditor.SelectFsm(fsmComponent.Fsm);
		}
	}

	public static void OpenInEditor(Fsm fsm)
	{
		if (fsm.Owner != null)
		{
			OpenInEditor(fsm.Owner as PlayMakerFSM);
		}
	}

	public static void OpenInEditor(GameObject go)
	{
		if (go != null)
		{
			OpenInEditor(FsmEditorUtility.FindFsmOnGameObject(go));
		}
	}


/*	GenericMenu GenerateWatermarkMenu()
	{
		var menu = new GenericMenu();

		var watermarkNames = FsmEditorUtility.GetWatermarkNames();

		menu.AddItem(new GUIContent("No Watermark"), false, SetWatermark, "" );

		foreach (var watermarkName in watermarkNames)
		{
			menu.AddItem(new GUIContent(watermarkName), false, SetWatermark, watermarkName);
		}

		return menu;
	}

	/// <summary>
	/// Set the watermark. Called by the context menu.
	/// </summary>
	void SetWatermark(object watermarkName)
	{
		FsmEditorUtility.SetWatermarkTexture(fsmComponent, watermarkName as string);

		watermarkLabel.text = FsmEditorUtility.GetWatermarkLabel(fsmComponent);
	
		EditorUtility.SetDirty(fsmComponent);

		FsmEditor.Repaint(true);
	}
*/
	void BuildFsmVariableList()
	{
		fsmVariables = FsmVariable.GetFsmVariableList(fsmComponent.Fsm.Variables, target);

		fsmVariables.Sort();
	}
	
	#if iTweenPathEditing

	// Live iTween path editing
	
	iTweenMoveTo temp;
	Vector3[] tempVct3;
	FsmState lastSelectedState;
	
	public void OnSceneGUI()
	{
		if (fsmComponent == null)
		{
			return;
		}

		var fsm = fsmComponent.Fsm;

		if (fsm == null)
		{
			return; 
		}

		if (fsm.EditState != null)
		{
			fsm.EditState.Fsm = fsm;

			for (var k = 0; k < fsm.EditState.Actions.Length; k++)
			{
				var iTweenMoveTo = fsm.EditState.Actions[k] as iTweenMoveTo;
				if (iTweenMoveTo != null)
				{
					temp = iTweenMoveTo;
					if (temp.transforms.Length >= 2)
					{
						Undo.SetSnapshotTarget(fsmComponent.gameObject, "Adjust iTween Path");
						tempVct3 = new Vector3[temp.transforms.Length];
						for (var i = 0; i < temp.transforms.Length; i++)
						{
							if (temp.transforms[i].IsNone) tempVct3[i] = temp.vectors[i].IsNone ? Vector3.zero : temp.vectors[i].Value;
							else
							{
								if (temp.transforms[i].Value == null)
								{
									tempVct3[i] = temp.vectors[i].IsNone ? Vector3.zero : temp.vectors[i].Value;
								}
								else
								{
									tempVct3[i] = temp.transforms[i].Value.transform.position +
									              (temp.vectors[i].IsNone ? Vector3.zero : temp.vectors[i].Value);
								}
							}
							tempVct3[i] = Handles.PositionHandle(tempVct3[i], Quaternion.identity);
							if (temp.transforms[i].IsNone)
							{
								if (!temp.vectors[i].IsNone)
								{
									temp.vectors[i].Value = tempVct3[i];
								}
							}
							else
							{
								if (temp.transforms[i].Value == null)
								{
									if (!temp.vectors[i].IsNone)
									{
										temp.vectors[i].Value = tempVct3[i];
									}
								}
								else
								{
									if (!temp.vectors[i].IsNone)
									{
										temp.vectors[i] = tempVct3[i] - temp.transforms[i].Value.transform.position;
									}
								}
							}
						}
						Handles.Label(tempVct3[0], "'" + fsmComponent.name + "' Begin");
						Handles.Label(tempVct3[tempVct3.Length - 1], "'" + fsmComponent.name + "' End");
						if (GUI.changed) FsmEditor.EditingActions();
					}
				}
			}
		}
	}
	
	#endif
}


