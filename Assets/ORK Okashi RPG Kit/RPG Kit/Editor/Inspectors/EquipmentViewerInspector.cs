
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EquipmentViewer))]
public class EquipmentViewerInspector : Editor
{
	public override void OnInspectorGUI()
	{
		((EquipmentViewer)target).partID = EditorGUILayout.Popup("Equipment part", 
				((EquipmentViewer)target).partID, DataHolder.EquipmentParts().GetNameList(true));
		EditorGUILayout.Separator();
		
		GUILayout.Label("Control type", EditorStyles.boldLabel);
		string[] types = System.Enum.GetNames(typeof(ControlType));
		if(types.Length != ((EquipmentViewer)target).controlType.Length)
		{
			bool[] tmp = ((EquipmentViewer)target).controlType;
			((EquipmentViewer)target).controlType = new bool[types.Length];
			for(int i=0; i<((EquipmentViewer)target).controlType.Length; i++)
			{
				if(i<tmp.Length) ((EquipmentViewer)target).controlType[i] = tmp[i];
			}
		}
		for(int i=0; i<types.Length; i++)
		{
			((EquipmentViewer)target).controlType[i] = EditorGUILayout.Toggle(types[i], ((EquipmentViewer)target).controlType[i]);
		}
		EditorGUILayout.Separator();
		
		if(GUILayout.Button("Add link", GUILayout.Width(200)))
		{
			((EquipmentViewer)target).AddLink();
		}
		for(int i=0; i<((EquipmentViewer)target).childName.Length; i++)
		{
			EditorGUILayout.BeginVertical("box");
			if(GUILayout.Button("Remove", GUILayout.Width(75)))
			{
				((EquipmentViewer)target).RemoveLink(i);
				break;
			}
			((EquipmentViewer)target).childName[i] = EditorGUILayout.TextField("Child (equip)", ((EquipmentViewer)target).childName[i]);
			((EquipmentViewer)target).linkTo[i] = EditorGUILayout.TextField("Link to (root)", ((EquipmentViewer)target).linkTo[i]);
			EditorGUILayout.Separator();
			EditorGUILayout.EndVertical();
		}
		EditorGUILayout.Separator();
		
		if(GUI.changed)
            EditorUtility.SetDirty(target);
	}
}