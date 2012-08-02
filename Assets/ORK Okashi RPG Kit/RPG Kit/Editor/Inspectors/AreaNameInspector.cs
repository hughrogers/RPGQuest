
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AreaName))]
public class AreaNameInspector : Editor
{
	public override void OnInspectorGUI()
	{
		((AreaName)target).areaName = EditorGUILayout.Popup("Area Name", ((AreaName)target).areaName, DataHolder.AreaNames().GetNameList(true));
		
		if(GUI.changed)
            EditorUtility.SetDirty(target);
	}
}