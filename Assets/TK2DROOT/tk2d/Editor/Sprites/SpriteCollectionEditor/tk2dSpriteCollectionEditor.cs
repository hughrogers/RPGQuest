using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(tk2dSpriteCollection))]
public class tk2dSpriteCollectionEditor : Editor
{
	const string defaultSpriteCollectionName = "SpriteCollection";
	
    public override void OnInspectorGUI()
    {
        tk2dSpriteCollection gen = (tk2dSpriteCollection)target;
		GUILayout.BeginVertical();
		GUILayout.Space(8);
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Open Editor...", EditorStyles.miniButton, GUILayout.MinWidth(120)))
		{
			if (gen.name == defaultSpriteCollectionName)
			{
				EditorUtility.DisplayDialog("Invalid Sprite Collection name", "Please rename sprite collection before proceeding", "Ok");
			}
			else
			{
				tk2dSpriteCollectionEditorPopup v = EditorWindow.GetWindow( typeof(tk2dSpriteCollectionEditorPopup), false, "Sprite Collection Editor" ) as tk2dSpriteCollectionEditorPopup;
				v.SetGenerator(gen);
			}
		}
		GUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }

	// Menu entries
	[MenuItem("Assets/Create/tk2d/Sprite Collection", false, 10000)]
    static void DoCollectionCreate()
    {
		string path = tk2dEditorUtility.CreateNewPrefab(defaultSpriteCollectionName);
        if (path != null)
        {
            GameObject go = new GameObject();
            go.AddComponent<tk2dSpriteCollection>();
            go.active = false;

#if (UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4)
			Object p = EditorUtility.CreateEmptyPrefab(path);
            EditorUtility.ReplacePrefab(go, p, ReplacePrefabOptions.ConnectToPrefab);
#else
			Object p = PrefabUtility.CreateEmptyPrefab(path);
            PrefabUtility.ReplacePrefab(go, p, ReplacePrefabOptions.ConnectToPrefab);
#endif
			
            GameObject.DestroyImmediate(go);
        }
    }	
}
