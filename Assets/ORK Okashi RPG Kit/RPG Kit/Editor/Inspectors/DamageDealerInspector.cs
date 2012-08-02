
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DamageDealer))]
public class DamageDealerInspector : Editor
{
	public override void OnInspectorGUI()
	{
		((DamageDealer)target).type = (DamageDealerType)EditorGUILayout.EnumPopup("Type", 
				((DamageDealer)target).type);
		((DamageDealer)target).destroyAfter = EditorGUILayout.FloatField("Destroy after",
				((DamageDealer)target).destroyAfter);
		((DamageDealer)target).destroyOnDamage = EditorGUILayout.Toggle("Destroy on damage",
				((DamageDealer)target).destroyOnDamage);
		((DamageDealer)target).destroyOnCollision = EditorGUILayout.Toggle("Destroy on collision",
				((DamageDealer)target).destroyOnCollision);
		EditorGUILayout.Separator();
		
		if(((DamageDealer)target).collider is BoxCollider ||
			((DamageDealer)target).collider is SphereCollider ||
			((DamageDealer)target).collider is CapsuleCollider)
		{
			((DamageDealer)target).changeCollider = EditorGUILayout.Toggle("Change collider",
					((DamageDealer)target).changeCollider);
			if(((DamageDealer)target).changeCollider)
			{
				((DamageDealer)target).expand = EditorGUILayout.FloatField("Change (s)",
						((DamageDealer)target).expand);
			}
		}
		else ((DamageDealer)target).changeCollider = false;
		
		GUILayout.Label("Damage settings", EditorStyles.boldLabel);
		((DamageDealer)target).singleDamage = EditorGUILayout.Toggle("Single damage",
				((DamageDealer)target).singleDamage);
		((DamageDealer)target).singleEnemy = EditorGUILayout.Toggle("Single enemy",
				((DamageDealer)target).singleEnemy);
		if(!((DamageDealer)target).singleDamage &&
			(DamageDealerType.TRIGGER_STAY.Equals(((DamageDealer)target).type) ||
			DamageDealerType.COLLISION_STAY.Equals(((DamageDealer)target).type)))
		{
			((DamageDealer)target).dmgEvery = EditorGUILayout.FloatField("Damage every (s)",
					((DamageDealer)target).dmgEvery);
		}
		else ((DamageDealer)target).dmgEvery = 0;
		
		EditorGUILayout.Separator();
		((DamageDealer)target).baseAttack = EditorGUILayout.Toggle("Base attack",
				((DamageDealer)target).baseAttack);
		EditorGUILayout.Separator();
		
		if(GUILayout.Button("Add skill"))
		{
			((DamageDealer)target).AddSkill();
		}
		for(int i=0; i<((DamageDealer)target).skillID.Length; i++)
		{
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Remove", GUILayout.Width(100)))
			{
				((DamageDealer)target).RemoveSkill(i);
				break;
			}
			((DamageDealer)target).skillID[i] = EditorGUILayout.Popup(
					((DamageDealer)target).skillID[i], DataHolder.Skills().GetNameList(true));
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.Separator();
		
		if(GUILayout.Button("Add Item"))
		{
			((DamageDealer)target).AddItem();
		}
		for(int i=0; i<((DamageDealer)target).itemID.Length; i++)
		{
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Remove", GUILayout.Width(100)))
			{
				((DamageDealer)target).RemoveItem(i);
				break;
			}
			((DamageDealer)target).itemID[i] = EditorGUILayout.Popup(
					((DamageDealer)target).itemID[i], DataHolder.Items().GetNameList(true));
			EditorGUILayout.EndHorizontal();
		}
		
		EditorGUILayout.Separator();
		
		if(GUI.changed)
            EditorUtility.SetDirty(target);
	}
}