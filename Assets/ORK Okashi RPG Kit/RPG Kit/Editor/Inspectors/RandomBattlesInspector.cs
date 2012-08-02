
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RandomBattles))]
public class RandomBattlesInspector : BaseInspector
{
	private GameObject tmpObject = null;
	
	public override void OnInspectorGUI()
	{
		EditorGUILayout.BeginVertical();
		EditorGUILayout.BeginVertical("box");
		fold1 = EditorGUILayout.Foldout(fold1, "General settings");
		if(fold1)
		{
			this.tmpObject = ((RandomBattles)target).arenaObject;
			((RandomBattles)target).arenaObject = (GameObject)EditorGUILayout.ObjectField("Battle arena", 
					((RandomBattles)target).arenaObject, typeof(GameObject), true);
			if(this.tmpObject != ((RandomBattles)target).arenaObject &&
				((RandomBattles)target).arenaObject != null && 
				((RandomBattles)target).arenaObject.GetComponent<BattleArena>() == null)
			{
				GUILayout.Label("Warning! No battle arena component detected", EditorStyles.boldLabel);
			}
			EditorGUILayout.Separator();
			
			((RandomBattles)target).battleChance = EditorGUILayout.FloatField("Chance (%)", 
					((RandomBattles)target).battleChance);
			FloatHelper.ChanceLimit(ref ((RandomBattles)target).battleChance);
			((RandomBattles)target).checkInterval = EditorGUILayout.FloatField("Check interval (s)", 
					((RandomBattles)target).checkInterval);
			EditorGUILayout.Separator();
			
			GUILayout.Label("Distance settings", EditorStyles.boldLabel);
			((RandomBattles)target).ignoreYDistance = EditorGUILayout.Toggle("Ignore y distance", 
					((RandomBattles)target).ignoreYDistance);
			((RandomBattles)target).minMoveCheckDistance = EditorGUILayout.FloatField("Min move distance", 
					((RandomBattles)target).minMoveCheckDistance);
			((RandomBattles)target).minBattleDistance = EditorGUILayout.FloatField("Min battle distance", 
					((RandomBattles)target).minBattleDistance);
			((RandomBattles)target).maxBattleDistance = EditorGUILayout.FloatField("Max battle distance", 
					((RandomBattles)target).maxBattleDistance);
			
			EditorGUILayout.Separator();
		}
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.Separator();
		if(GUILayout.Button("Add enemy team", GUILayout.Width(150)))
		{
			((RandomBattles)target).AddEnemyTeam();
		}
		
		float total = 0;
		for(int i=0; i<((RandomBattles)target).enemyTeam.Length; i++)
		{
			total += ((RandomBattles)target).enemyTeam[i].chance;
		}
		if(total > DataHolder.GameSettings().maxRandomRange)
		{
			GUILayout.Label("Warning! Total team chances are above maximum random value!", EditorStyles.boldLabel);
		}
		GUILayout.Label("Total team chance: "+total);
		
		for(int i=0; i<((RandomBattles)target).enemyTeam.Length; i++)
		{
			EditorGUILayout.BeginVertical("box");
			((RandomBattles)target).enemyTeam[i].fold = EditorGUILayout.Foldout(
					((RandomBattles)target).enemyTeam[i].fold, "Enemy team "+i);
			if(((RandomBattles)target).enemyTeam[i].fold)
			{
				if(((RandomBattles)target).enemyTeam.Length > 1)
				{
					if(GUILayout.Button("Remove", GUILayout.Width(75)))
					{
						((RandomBattles)target).RemoveEnemyTeam(i);
						break;
					}
				}
				((RandomBattles)target).enemyTeam[i].chance = EditorGUILayout.FloatField("Chance (%)", 
					((RandomBattles)target).enemyTeam[i].chance);
				FloatHelper.ChanceLimit(ref ((RandomBattles)target).enemyTeam[i].chance);
				EditorGUILayout.Separator();
				
				if(GUILayout.Button("Add enemy", GUILayout.Width(150)))
				{
					((RandomBattles)target).enemyTeam[i].AddEnemy();
				}
				
				for(int j=0; j<((RandomBattles)target).enemyTeam[i].enemyID.Length; j++)
				{
					EditorGUILayout.BeginHorizontal();
					((RandomBattles)target).enemyTeam[i].enemyID[j] = EditorGUILayout.Popup("Enemy "+j, 
							((RandomBattles)target).enemyTeam[i].enemyID[j], 
							DataHolder.Enemies().GetNameList(true));
					
					if(((RandomBattles)target).enemyTeam[i].enemyID.Length > 1)
					{
						if(GUILayout.Button("Remove", GUILayout.Width(75)))
						{
							((RandomBattles)target).enemyTeam[i].RemoveEnemy(j);
							break;
						}
					}
					EditorGUILayout.EndHorizontal();
				}
				
				EditorGUILayout.Separator();
			}
			EditorGUILayout.EndVertical();
		}
		
		EditorGUILayout.EndVertical();
		
		if(GUI.changed)
            EditorUtility.SetDirty(target);
	}
}
