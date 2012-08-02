
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SavePoint))]
public class SavePointInspector : BaseInspector
{
	public override void OnInspectorGUI()
	{
		this.EventStartSettings((SavePoint)target);
		EditorGUILayout.Separator();
		((SavePoint)target).savePointType = (SavePointType)EditorGUILayout.EnumPopup("Type", ((SavePoint)target).savePointType);
		if(!SavePointType.SAVE_POINT.Equals(((SavePoint)target).savePointType))
		{
			((SavePoint)target).destroyAfter = EditorGUILayout.Toggle("Destroy after", ((SavePoint)target).destroyAfter);
		}
		else ((SavePoint)target).destroyAfter = false;
		EditorGUILayout.Separator();
		
		fold1 = EditorGUILayout.Foldout(fold1, "Variable setup");
		if(fold1)
		{
			this.VariableSettings((SavePoint)target);
			
			if(!SavePointType.SAVE_POINT.Equals(((SavePoint)target).savePointType))
			{
				EditorGUILayout.Separator();
				
				GUILayout.Label("Set variable when saving", EditorStyles.boldLabel);
				if(GUILayout.Button("Add", GUILayout.Width(150)))
				{
					((SavePoint)target).AddVariableSet();
				}
				for(int i=0; i<((SavePoint)target).setVariableKey.Length; i++)
				{
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.Separator();
					GUILayout.Label(i.ToString()+":");
					((SavePoint)target).setVariableKey[i] = EditorGUILayout.TextField(((SavePoint)target).setVariableKey[i]);
					GUILayout.Label(" = ");
					((SavePoint)target).setVariableValue[i] = EditorGUILayout.TextField(((SavePoint)target).setVariableValue[i]);
					EditorGUILayout.Separator();
					if(GUILayout.Button("Remove", GUILayout.Width(75)))
					{
						((SavePoint)target).RemoveVariableSet(i);
						return;
					}
					EditorGUILayout.EndHorizontal();
				}
				
				if(GUILayout.Button("Add Number", GUILayout.Width(150)))
				{
					((SavePoint)target).AddNumberVariableSet();
				}
				for(int i=0; i<((SavePoint)target).setNumberVarKey.Length; i++)
				{
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.Separator();
					GUILayout.Label(i.ToString()+":");
					((SavePoint)target).setNumberVarKey[i] = EditorGUILayout.TextField(((SavePoint)target).setNumberVarKey[i]);
					((SavePoint)target).setNumberOperator[i] = (SimpleOperator)EditorGUILayout.EnumPopup(((SavePoint)target).setNumberOperator[i]);
					((SavePoint)target).setNumberVarValue[i] = EditorGUILayout.FloatField(((SavePoint)target).setNumberVarValue[i]);
					EditorGUILayout.Separator();
					if(GUILayout.Button("Remove", GUILayout.Width(75)))
					{
						((SavePoint)target).RemoveNumberVariableSet(i);
						return;
					}
					EditorGUILayout.EndHorizontal();
				}
			}
		}
		EditorGUILayout.Separator();
		
		if(GUI.changed)
            EditorUtility.SetDirty(target);
	}
}