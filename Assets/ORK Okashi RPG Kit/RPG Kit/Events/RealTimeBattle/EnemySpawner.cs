
using UnityEngine;
using System.Collections;

[AddComponentMenu("RPG Kit/Real Time Battles/Enemy Spawner")]
public class EnemySpawner : BaseSpawner
{
	public int enemyID = 0;
	public bool destroyAfterSpawn = true;
	
	public bool respawn = false;
	public float respawnTime = 2.0f;
	
	public float waypointTime = 5.0f;
	public GameObject[] waypoint = new GameObject[0];
	
	public void AddWaypoint()
	{
		this.waypoint = ArrayHelper.Add(null, this.waypoint);
	}
	
	public void RemoveWaypoint(int index)
	{
		this.waypoint = ArrayHelper.Remove(index, this.waypoint);
	}
	
	void Start()
	{
		this.SpawnEnemy();
		if(this.destroyAfterSpawn)
		{
			GameObject.Destroy(this.gameObject);
		}
	}
	
	public void SpawnEnemy()
	{
		Enemy enemy = DataHolder.Enemies().GetCopy(this.enemyID);
		enemy.Init();
		if(!this.destroyAfterSpawn) enemy.SetBaseSpawner(this);
		GameObject obj = enemy.CreatePrefabInstance();
		if(obj != null)
		{
			obj.transform.position = this.transform.position;
			obj.transform.rotation = this.transform.rotation;
			DataHolder.BattleSystem().AddEnemy(enemy);
			if(this.waypoint.Length > 0)
			{
				enemy.SetRoute(this.waypointTime, this.waypoint);
			}
		}
	}
	
	public override void CheckRespawn(Enemy e)
	{
		if(DataHolder.BattleEnd().getImmediately && this.respawn)
		{
			this.StartCoroutine(this.Respawn());
		}
	}
	
	private IEnumerator Respawn()
	{
		if(this.respawnTime > 0) yield return new WaitForSeconds(this.respawnTime);
		this.SpawnEnemy();
	}
	
	void OnDrawGizmos()
	{
		Gizmos.DrawIcon(transform.position, "BattleAgent.psd");
	}
}
