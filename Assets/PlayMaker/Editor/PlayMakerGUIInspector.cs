// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayMakerGUI))]
class PlayMakerGUIInspector : Editor
{
	private PlayMakerGUI guiComponent;

	void OnEnable()
	{
		guiComponent = (PlayMakerGUI) target;

		guiComponent.drawStateLabels = EditorPrefs.GetBool("PlayMaker.ShowStateLabelsInGameView");

		CheckForDuplicateComponents();
	}

	public override void OnInspectorGUI()
	{
		EditorGUIUtility.LookLikeInspector();

		GUILayout.Label("NOTES", EditorStyles.boldLabel);
		
		GUILayout.Label("- A scene should only have one PlayMakerGUI component.\n- PlayMaker will auto-add this component.\n- Disable auto-add in Preferences.");
		
		GUILayout.Label("General", EditorStyles.boldLabel);

		EditorGUI.indentLevel = 1;

		guiComponent.enableGUILayout = EditorGUILayout.Toggle(new GUIContent("Enable GUILayout",
		                                                               "Disabling GUILayout can improve the performance of GUI actions, especially on mobile devices. NOTE: You cannot use GUILayout actions with GUILayout disabled."),
																	   guiComponent.enableGUILayout);
		guiComponent.controlMouseCursor = EditorGUILayout.Toggle(new GUIContent("Control Mouse Cursor",
		                                                                  "Disable this if you have scripts that need to control the mouse cursor."),
																		  guiComponent.controlMouseCursor);

		guiComponent.previewOnGUI = EditorGUILayout.Toggle(new GUIContent("Preview GUI Actions While Editing", "This lets you preview GUI actions as you edit them. NOTE: This is an experimental feature, so you might run into some bugs!"), guiComponent.previewOnGUI);

		EditorGUI.indentLevel = 0;
		GUILayout.Label("Debugging", EditorStyles.boldLabel);
		EditorGUI.indentLevel = 1;

		var drawStateLabels = EditorGUILayout.Toggle(new GUIContent("Draw Active State Labels", "Draw the currently active state over GameObjects in the Game View. You can enable/disable for each FSM in the PlayMakerFSM Inspector."), guiComponent.drawStateLabels);

		if (drawStateLabels != guiComponent.drawStateLabels)
		{
			guiComponent.drawStateLabels = drawStateLabels;
			EditorPrefs.SetBool("PlayMaker.ShowStateLabelsInGameView", drawStateLabels);
		}


		GUI.enabled = guiComponent.drawStateLabels;
		//EditorGUI.indentLevel = 2;

		guiComponent.GUITextureStateLabels = EditorGUILayout.Toggle(new GUIContent("GUITexture State Labels", "Draw active state labels on GUITextures."), guiComponent.GUITextureStateLabels);
		guiComponent.GUITextStateLabels = EditorGUILayout.Toggle(new GUIContent("GUIText State Labels", "Draw active state labels on GUITexts."), guiComponent.GUITextStateLabels);

		GUI.enabled = true;
		//EditorGUI.indentLevel = 1;

		guiComponent.filterLabelsWithDistance = EditorGUILayout.Toggle(new GUIContent("Filter State Labels With Distance", "This is useful if you only want to see nearby state labels as you move in the Game View."), guiComponent.filterLabelsWithDistance);

		GUI.enabled = guiComponent.filterLabelsWithDistance;

		guiComponent.maxLabelDistance = EditorGUILayout.FloatField(new GUIContent("Distance", "Distance is measured from the main camera"), guiComponent.maxLabelDistance);

		if (GUI.changed)
		{
			CheckForDuplicateComponents();
		}
	}

	void CheckForDuplicateComponents()
	{
		var components = FindObjectsOfType(typeof(PlayMakerGUI));

		if (components.Length > 1)
		{
			if (EditorUtility.DisplayDialog("Playmaker", "The scene has more than one PlayMakerGUI!\nRemove other instances?", "Yes", "No"))
			{
				foreach (Object component in components)
				{
					if (component != target)
					{
						var behavior = (PlayMakerGUI)component as Behaviour;
						
						// Delete the game object if it only has the PlayMakerGUI component?

						if (behavior.gameObject.GetComponents(typeof(Component)).Length == 2) // every game object has a transform component
						{
							if (EditorUtility.DisplayDialog("Playmaker", "Delete: " + behavior.gameObject.name + "?", "Yes", "No"))
							{
								DestroyImmediate(behavior.gameObject);
							}
						}
						else
						{
							DestroyImmediate(component);
						}
					}
				}
			}
		}
	}

}
