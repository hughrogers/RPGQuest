
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemCollector))]
public class ItemCollectorInspector : BaseInspector
{
	public override void OnInspectorGUI()
	{
		this.EventStartSettings((ItemCollector)target);
		
		EditorGUILayout.Separator();
		((ItemCollector)target).isMoney = EditorGUILayout.Toggle("Money", ((ItemCollector)target).isMoney);
		if(((ItemCollector)target).isMoney)
		{
			((ItemCollector)target).itemNumber = EditorGUILayout.IntField("Amount", ((ItemCollector)target).itemNumber);
		}
		else
		{
			((ItemCollector)target).itemDropType = (ItemDropType)EditorGUILayout.EnumPopup("Item type", ((ItemCollector)target).itemDropType);
			
			if(ItemDropType.ITEM.Equals(((ItemCollector)target).itemDropType))
			{
				((ItemCollector)target).itemID = EditorGUILayout.Popup("Item", ((ItemCollector)target).itemID, DataHolder.Items().GetNameList(true));
			}
			else if(ItemDropType.WEAPON.Equals(((ItemCollector)target).itemDropType))
			{
				((ItemCollector)target).itemID = EditorGUILayout.Popup("Weapon", ((ItemCollector)target).itemID, DataHolder.Weapons().GetNameList(true));
			}
			else if(ItemDropType.ARMOR.Equals(((ItemCollector)target).itemDropType))
			{
				((ItemCollector)target).itemID = EditorGUILayout.Popup("Armor", ((ItemCollector)target).itemID, DataHolder.Armors().GetNameList(true));
			}
			((ItemCollector)target).itemNumber = EditorGUILayout.IntField("Number", ((ItemCollector)target).itemNumber);
		}
		((ItemCollector)target).spawnPrefab = EditorGUILayout.Toggle("Spawn prefab", ((ItemCollector)target).spawnPrefab);
		if(((ItemCollector)target).spawnPrefab)
		{
			((ItemCollector)target).offset = EditorGUILayout.Vector3Field("Offset", ((ItemCollector)target).offset);
			((ItemCollector)target).localSpace = EditorGUILayout.Toggle("Local space", ((ItemCollector)target).localSpace);
			((ItemCollector)target).rotationOffset = EditorGUILayout.Vector3Field("Rotation offset", ((ItemCollector)target).rotationOffset);
		}
		
		((ItemCollector)target).onGround = EditorGUILayout.Toggle("Place on ground", ((ItemCollector)target).onGround);
		if(((ItemCollector)target).onGround)
		{
			((ItemCollector)target).distance = EditorGUILayout.FloatField("Distance", ((ItemCollector)target).distance);
			((ItemCollector)target).layerMask = EditorGUILayout.LayerField("Layer mask", ((ItemCollector)target).layerMask);
		}
		
		EditorGUILayout.Separator();
		fold1 = EditorGUILayout.Foldout(fold1, "Variable setup");
		if(fold1)
		{
			this.VariableSettings((ItemCollector)target);
			EditorGUILayout.Separator();
			
			GUILayout.Label("Set variable after collection", EditorStyles.boldLabel);
			if(GUILayout.Button("Add", GUILayout.Width(150)))
			{
				((ItemCollector)target).AddVariableSet();
			}
			for(int i=0; i<((ItemCollector)target).setVariableKey.Length; i++)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Separator();
				GUILayout.Label(i.ToString()+":");
				((ItemCollector)target).setVariableKey[i] = EditorGUILayout.TextField(((ItemCollector)target).setVariableKey[i]);
				GUILayout.Label(" = ");
				((ItemCollector)target).setVariableValue[i] = EditorGUILayout.TextField(((ItemCollector)target).setVariableValue[i]);
				EditorGUILayout.Separator();
				if(GUILayout.Button("Remove", GUILayout.Width(75)))
				{
					((ItemCollector)target).RemoveVariableSet(i);
					return;
				}
				EditorGUILayout.EndHorizontal();
			}
			
			if(GUILayout.Button("Add Number", GUILayout.Width(150)))
			{
				((ItemCollector)target).AddNumberVariableSet();
			}
			for(int i=0; i<((ItemCollector)target).setNumberVarKey.Length; i++)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Separator();
				GUILayout.Label(i.ToString()+":");
				((ItemCollector)target).setNumberVarKey[i] = EditorGUILayout.TextField(((ItemCollector)target).setNumberVarKey[i]);
				((ItemCollector)target).setNumberOperator[i] = (SimpleOperator)EditorGUILayout.EnumPopup(((ItemCollector)target).setNumberOperator[i]);
				((ItemCollector)target).setNumberVarValue[i] = EditorGUILayout.FloatField(((ItemCollector)target).setNumberVarValue[i]);
				EditorGUILayout.Separator();
				if(GUILayout.Button("Remove", GUILayout.Width(75)))
				{
					((ItemCollector)target).RemoveNumberVariableSet(i);
					return;
				}
				EditorGUILayout.EndHorizontal();
			}
		}
		
		EditorGUILayout.Separator();
		
		if(GUI.changed)
            EditorUtility.SetDirty(target);
	}
}