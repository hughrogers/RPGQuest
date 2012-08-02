
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BattleArea))]
public class BattleAreaInspector : Editor
{
	private bool fold1 = true;
	private bool fold2 = true;
	
	public override void OnInspectorGUI()
	{
		fold1 = EditorGUILayout.Foldout(fold1, "Battle settings");
		if(fold1)
		{
			((BattleArea)target).showStartMessage = EditorGUILayout.Toggle("Battle start message",
					((BattleArea)target).showStartMessage);
			((BattleArea)target).noGameover = EditorGUILayout.Toggle("No game over",
					((BattleArea)target).noGameover);
			EditorGUILayout.Separator();
		}
		
		fold2 = EditorGUILayout.Foldout(fold2, "Enemy spawn settings");
		if(fold2)
		{
			((BattleArea)target).spawnOnEnter = EditorGUILayout.Toggle("Spawn on enter",
					((BattleArea)target).spawnOnEnter);
			((BattleArea)target).layerMask = EditorGUILayout.LayerField("Layermask",
					((BattleArea)target).layerMask);
			if(GUILayout.Button("Add enemy", GUILayout.Width(150)))
			{
				((BattleArea)target).AddEnemy();
			}
			for(int i=0; i<((BattleArea)target).enemyID.Length; i++)
			{
				EditorGUILayout.BeginVertical("box");
				if(GUILayout.Button("Remove", GUILayout.Width(75)))
				{
					((BattleArea)target).RemoveEnemy(i);
					break;
				}
				((BattleArea)target).enemyID[i] = EditorGUILayout.Popup("Enemy",
						((BattleArea)target).enemyID[i], DataHolder.Enemies().GetNameList(true));
				((BattleArea)target).enemyQuantity[i] = EditorGUILayout.IntField("Quantity",
						((BattleArea)target).enemyQuantity[i]);
				if(((BattleArea)target).enemyQuantity[i] < 1) ((BattleArea)target).enemyQuantity[i] = 1;
				
				((BattleArea)target).spawnOffset[i] = EditorGUILayout.Vector3Field("Spawn offset",
						((BattleArea)target).spawnOffset[i]);
				
				if(DataHolder.BattleEnd().getImmediately)
				{
					((BattleArea)target).respawnEnemy[i] = EditorGUILayout.Toggle("Respawn",
							((BattleArea)target).respawnEnemy[i]);
					if(((BattleArea)target).respawnEnemy[i])
					{
						((BattleArea)target).respawnTime[i] = EditorGUILayout.FloatField("Respawn after(s)",
								((BattleArea)target).respawnTime[i]);
						if(((BattleArea)target).respawnTime[i] < 0) ((BattleArea)target).respawnTime[i] = 0;
					}
				}
				EditorGUILayout.Separator();
				EditorGUILayout.EndVertical();
			}
		}
		
		if(GUI.changed)
            EditorUtility.SetDirty(target);
	}
}
