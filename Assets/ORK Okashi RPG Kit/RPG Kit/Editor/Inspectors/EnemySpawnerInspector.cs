
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerInspector : Editor
{
	public override void OnInspectorGUI()
	{
		((EnemySpawner)target).enemyID = EditorGUILayout.Popup("Enemy", 
				((EnemySpawner)target).enemyID, DataHolder.Enemies().GetNameList(true));
		((EnemySpawner)target).destroyAfterSpawn = EditorGUILayout.Toggle("Destroy after spawn",
				((EnemySpawner)target).destroyAfterSpawn);
		
		if(!((EnemySpawner)target).destroyAfterSpawn && 
			DataHolder.BattleEnd().getImmediately)
		{
			((EnemySpawner)target).respawn = EditorGUILayout.Toggle("Respawn",
					((EnemySpawner)target).respawn);
			if(((EnemySpawner)target).respawn)
			{
				((EnemySpawner)target).respawnTime = EditorGUILayout.FloatField("Respawn after(s)",
						((EnemySpawner)target).respawnTime);
			}
		}
		EditorGUILayout.Separator();
		
		if(GUILayout.Button("Add waypoint", GUILayout.Width(150)))
		{
			((EnemySpawner)target).AddWaypoint();
		}
		if(((EnemySpawner)target).waypoint.Length > 0)
		{
			((EnemySpawner)target).waypointTime = EditorGUILayout.FloatField("Time at waypoint", 
					((EnemySpawner)target).waypointTime);
			for(int i=0; i<((EnemySpawner)target).waypoint.Length; i++)
			{
				EditorGUILayout.BeginHorizontal();
				if(GUILayout.Button("Remove", GUILayout.Width(75)))
				{
					((EnemySpawner)target).RemoveWaypoint(i);
					break;
				}
				((EnemySpawner)target).waypoint[i] = (GameObject)EditorGUILayout.ObjectField(
						((EnemySpawner)target).waypoint[i], typeof(GameObject), true);
				EditorGUILayout.EndHorizontal();
			}
		}
		EditorGUILayout.Separator();
		
		if(GUI.changed)
            EditorUtility.SetDirty(target);
	}
}