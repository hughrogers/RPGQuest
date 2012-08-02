
using UnityEditor;
using UnityEngine;

public class BaseInspector : Editor
{
	protected bool fold1 = true;
	protected bool fold2 = true;
	protected bool fold3 = true;
	protected bool fold4 = true;
	protected bool fold5 = true;
	protected bool fold6 = true;
	protected bool fold7 = true;
	
	public void EventStartSettings(BaseInteraction target)
	{
		target.startType = (EventStartType)EditorGUILayout.EnumPopup("Start type", 
					target.startType);
		
		if(EventStartType.INTERACT.Equals(target.startType))
		{
			target.maxMouseDistance = EditorGUILayout.FloatField("Max mouse distance", 
					target.maxMouseDistance);
		}
		else if(EventStartType.KEY_PRESS.Equals(target.startType))
		{
			target.keyToPress = EditorGUILayout.TextField("Key to press", 
					target.keyToPress);
			target.keyPressInTrigger = EditorGUILayout.Toggle("In trigger", 
					target.keyPressInTrigger);
		}
		else if(EventStartType.DROP.Equals(target.startType))
		{
			target.dropType = (ItemDropType)EditorGUILayout.EnumPopup("Item type", 
					target.dropType);
			if(ItemDropType.ITEM.Equals(target.dropType))
			{
				target.dropID = EditorGUILayout.Popup("Item", 
						target.dropID, DataHolder.Items().GetNameList(true));
			}
			else if(ItemDropType.WEAPON.Equals(target.dropType))
			{
				target.dropID = EditorGUILayout.Popup("Weapon", 
						target.dropID, DataHolder.Weapons().GetNameList(true));
			}
			else if(ItemDropType.ARMOR.Equals(target.dropType))
			{
				target.dropID = EditorGUILayout.Popup("Armor", 
						target.dropID, DataHolder.Armors().GetNameList(true));
			}
			target.consumeDrop = EditorGUILayout.Toggle("Consume item", 
					target.consumeDrop);
		}
		
		if(GUI.changed)
            EditorUtility.SetDirty(target);
	}
	
	public void VariableSettings(BaseInteraction target)
	{
		GUILayout.Label("Variable conditions", EditorStyles.boldLabel);
		if(target.variableKey.Length > 0 || target.numberVarKey.Length > 0)
		{
			target.needed = (AIConditionNeeded)EditorGUILayout.EnumPopup("Needed", target.needed);
			target.autoDestroyOnVariables = EditorGUILayout.Toggle("Auto destroy", 
					target.autoDestroyOnVariables);
		}
		
		if(GUILayout.Button("Add Variable", GUILayout.Width(150)))
		{
			target.AddVariableCondition();
		}
		if(target.variableKey.Length > 0)
		{
			for(int i=0; i<target.variableKey.Length; i++)
			{
				EditorGUILayout.BeginHorizontal();
				if(GUILayout.Button("Remove", GUILayout.Width(75)))
				{
					target.RemoveVariableCondition(i);
					return;
				}
				target.checkType[i] = EditorGUILayout.Toggle(target.checkType[i], GUILayout.Width(20));
				target.variableKey[i] = EditorGUILayout.TextField(target.variableKey[i]);
				if(target.checkType[i]) GUILayout.Label("== ");
				else GUILayout.Label(" != ");
				target.variableValue[i] = EditorGUILayout.TextField(target.variableValue[i]);
				EditorGUILayout.EndHorizontal();
			}
		}
		
		if(GUILayout.Button("Add Number Variable", GUILayout.Width(150)))
		{
			target.AddNumberVariableCondition();
		}
		if(target.numberVarKey.Length > 0)
		{
			for(int i=0; i<target.numberVarKey.Length; i++)
			{
				EditorGUILayout.BeginHorizontal();
				if(GUILayout.Button("Remove", GUILayout.Width(75)))
				{
					target.RemoveNumberVariableCondition(i);
					return;
				}
				target.numberCheckType[i] = EditorGUILayout.Toggle(target.numberCheckType[i], GUILayout.Width(20));
				target.numberVarKey[i] = EditorGUILayout.TextField(target.numberVarKey[i]);
				if(!target.numberCheckType[i]) GUILayout.Label("not");
				target.numberValueCheck[i] = (ValueCheck)EditorGUILayout.EnumPopup(target.numberValueCheck[i]);
				target.numberVarValue[i] = EditorGUILayout.FloatField(target.numberVarValue[i]);
				EditorGUILayout.EndHorizontal();
			}
		}
		
		if(GUI.changed)
            EditorUtility.SetDirty(target);
	}
}
