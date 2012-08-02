
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SceneChanger))]
public class SceneChangerInspector : Editor
{
	public override void OnInspectorGUI()
	{
		((SceneChanger)target).sceneName = EditorGUILayout.TextField("Scene name", ((SceneChanger)target).sceneName);
		((SceneChanger)target).spawnID = EditorGUILayout.IntField("Spawn ID", ((SceneChanger)target).spawnID);
		
		EditorGUILayout.Separator();
		((SceneChanger)target).fadeOut = EditorGUILayout.Toggle("Fade out", ((SceneChanger)target).fadeOut);
		if(((SceneChanger)target).fadeOut)
		{
			((SceneChanger)target).fadeOutTime = EditorGUILayout.FloatField("Time", ((SceneChanger)target).fadeOutTime);
			((SceneChanger)target).fadeOutInterpolate = (EaseType)EditorGUILayout.EnumPopup("Interpolation", ((SceneChanger)target).fadeOutInterpolate);
			EditorGUILayout.Separator();
		}
		((SceneChanger)target).fadeIn = EditorGUILayout.Toggle("Fade in", ((SceneChanger)target).fadeIn);
		if(((SceneChanger)target).fadeIn)
		{
			((SceneChanger)target).fadeInTime = EditorGUILayout.FloatField("Time", ((SceneChanger)target).fadeInTime);
			((SceneChanger)target).fadeInInterpolate = (EaseType)EditorGUILayout.EnumPopup("Interpolation", ((SceneChanger)target).fadeInInterpolate);
			EditorGUILayout.Separator();
		}
		
		EditorGUILayout.Separator();
		GUILayout.Label("Variable conditions", EditorStyles.boldLabel);
		if(((SceneChanger)target).variableKey.Length > 0 || ((SceneChanger)target).numberVarKey.Length > 0)
		{
			((SceneChanger)target).needed = (AIConditionNeeded)EditorGUILayout.EnumPopup("Needed", ((SceneChanger)target).needed);
		}
		if(GUILayout.Button("Add Variable", GUILayout.Width(150)))
		{
			((SceneChanger)target).AddVariableCondition();
		}
		if(((SceneChanger)target).variableKey.Length > 0)
		{
			for(int i=0; i<((SceneChanger)target).variableKey.Length; i++)
			{
				EditorGUILayout.BeginHorizontal();
				if(GUILayout.Button("Remove", GUILayout.Width(75)))
				{
					((SceneChanger)target).RemoveVariableCondition(i);
					return;
				}
				((SceneChanger)target).checkType[i] = EditorGUILayout.Toggle(((SceneChanger)target).checkType[i], GUILayout.Width(20));
				((SceneChanger)target).variableKey[i] = EditorGUILayout.TextField(((SceneChanger)target).variableKey[i]);
				if(((SceneChanger)target).checkType[i]) GUILayout.Label(" = ");
				else GUILayout.Label(" != ");
				((SceneChanger)target).variableValue[i] = EditorGUILayout.TextField(((SceneChanger)target).variableValue[i]);
				EditorGUILayout.EndHorizontal();
			}
		}
		
		if(GUILayout.Button("Add Number Variable", GUILayout.Width(150)))
		{
			((SceneChanger)target).AddNumberVariableCondition();
		}
		if(((SceneChanger)target).numberVarKey.Length > 0)
		{
			for(int i=0; i<((SceneChanger)target).numberVarKey.Length; i++)
			{
				EditorGUILayout.BeginHorizontal();
				if(GUILayout.Button("Remove", GUILayout.Width(75)))
				{
					((SceneChanger)target).RemoveNumberVariableCondition(i);
					return;
				}
				((SceneChanger)target).numberCheckType[i] = EditorGUILayout.Toggle(((SceneChanger)target).numberCheckType[i], GUILayout.Width(20));
				((SceneChanger)target).numberVarKey[i] = EditorGUILayout.TextField(((SceneChanger)target).numberVarKey[i]);
				if(!((SceneChanger)target).numberCheckType[i]) GUILayout.Label("not");
				((SceneChanger)target).numberValueCheck[i] = (ValueCheck)EditorGUILayout.EnumPopup(((SceneChanger)target).numberValueCheck[i]);
				((SceneChanger)target).numberVarValue[i] = EditorGUILayout.FloatField(((SceneChanger)target).numberVarValue[i]);
				EditorGUILayout.EndHorizontal();
			}
		}
		EditorGUILayout.Separator();
		
		if(GUI.changed)
            EditorUtility.SetDirty(target);
	}
}