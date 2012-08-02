
using UnityEngine;
using System.Collections;

[AddComponentMenu("RPG Kit/Real Time Battles/Battle Area")]
public class BattleArea : BaseSpawner
{
	// enemy spawn
	public bool spawnOnEnter = false;
	public LayerMask layerMask = 1;
	
	public int[] enemyID = new int[0];
	public int[] enemyQuantity = new int[0];
	public Vector3[] spawnOffset = new Vector3[0];
	public bool[] respawnEnemy = new bool[0];
	public float[] respawnTime = new float[0];
	
	// battle end settings
	public bool showStartMessage = false;
	public bool noGameover = false;
	
	// ingame
	private bool clearBattle = false;
	private float timeout = 0;
	
	private int[] spawnedQuantity = new int[0];
	
	void Start()
	{
		this.gameObject.layer = 2;
		if(this.collider == null) GameObject.Destroy(this.gameObject);
		else
		{
			this.spawnedQuantity = new int[this.enemyID.Length];
			if(!this.spawnOnEnter) this.StartCoroutine(this.SpawnAllEnemies());
		}
	}
	
	/*
	============================================================================
	Inspector functions
	============================================================================
	*/
	public void AddEnemy()
	{
		this.enemyID = ArrayHelper.Add(0, this.enemyID);
		this.enemyQuantity = ArrayHelper.Add(1, this.enemyQuantity);
		this.spawnOffset = ArrayHelper.Add(Vector3.zero, this.spawnOffset);
		this.respawnEnemy = ArrayHelper.Add(false, this.respawnEnemy);
		this.respawnTime = ArrayHelper.Add(2, this.respawnTime);
	}
	
	public void RemoveEnemy(int index)
	{
		this.enemyID = ArrayHelper.Remove(index, this.enemyID);
		this.enemyQuantity = ArrayHelper.Remove(index, this.enemyQuantity);
		this.spawnOffset = ArrayHelper.Remove(index, this.spawnOffset);
		this.respawnEnemy = ArrayHelper.Remove(index, this.respawnEnemy);
		this.respawnTime = ArrayHelper.Remove(index, this.respawnTime);
	}
	
	void OnDrawGizmos()
	{
		Gizmos.DrawIcon(transform.position, "BattleArena.psd");
	}
	
	/*
	============================================================================
	Enter/exit battle functions
	============================================================================
	*/
	void OnTriggerEnter(Collider collider)
	{
		if(collider.gameObject == GameHandler.Party().GetPlayer())
		{
			if(this.clearBattle) this.clearBattle = false;
			else
			{
				if(this.spawnOnEnter)
				{
					this.StartCoroutine(this.SpawnAllEnemies());
				}
				GameHandler.SetInBattleArea(1);
				GameHandler.SetBattleArea(this);
				if(DataHolder.BattleEnd().getImmediately || 
					DataHolder.BattleSystem().enemies.Length > 0)
				{
					if(this.showStartMessage) DataHolder.BattleSystem().ShowBattleStartMessage();
					DataHolder.BattleSystem().SetupBattle(GameHandler.Party().GetBattleParty(), null, false);
					DataHolder.BattleSystem().StartBattle();
				}
			}
		}
	}
	
	void OnTriggerExit(Collider collider)
	{
		if(collider.gameObject == GameHandler.Party().GetPlayer())
		{
			this.timeout = 1;
			this.clearBattle = true;
		}
	}
	
	void Update()
	{
		if(this.clearBattle)
		{
			this.timeout -= GameHandler.DeltaTime;
			if(this.timeout <= 0)
			{
				GameHandler.SetInBattleArea(-1);
				this.clearBattle = false;
			}
		}
	}
	
	public IEnumerator BattleLost()
	{
		if(!this.noGameover) GameHandler.GetMusicHandler().Stop();
		yield return new WaitForSeconds(DataHolder.BattleSystemData().battleMessageShowTime);
		if(this.noGameover)
		{
			DataHolder.BattleSystem().ClearBattle(false);
			GameHandler.ClearInBattleArea();
		}
		else
		{
			GameHandler.GameOver();
		}
	}
	
	/*
	============================================================================
	Spawn functions
	============================================================================
	*/
	private Vector3 GetSpawnCenter()
	{
		Vector3 castOrigin = this.collider.bounds.center;
		castOrigin.y += this.collider.bounds.extents.y-0.1f;
		return castOrigin;
	}
	
	private Vector3 GetRandomAdd()
	{
		Vector3 add = Vector3.zero;
		add.x += Random.Range(-this.collider.bounds.extents.x, this.collider.bounds.extents.x);
		add.z += Random.Range(-this.collider.bounds.extents.z, this.collider.bounds.extents.z);
		return add;
	}
	
	private IEnumerator SpawnAllEnemies()
	{
		Vector3 castOrigin = this.GetSpawnCenter();
		int count = 0;
		for(int i=0; i<this.enemyID.Length; i++)
		{
			while(this.spawnedQuantity[i] < this.enemyQuantity[i])
			{
				if(!this.SpawnEnemy(i, castOrigin+this.GetRandomAdd()))
				{
					count++;
					if(count > 10)
					{
						count = 0;
						yield return null;
					}
				}
			}
		}
	}
	
	private bool SpawnEnemy(int index, Vector3 castOrigin)
	{
		bool spawned = false;
		RaycastHit hit;
		if(Physics.Raycast(castOrigin, -Vector3.up, out hit, this.collider.bounds.extents.z, 1 << this.layerMask.value))
		{
			// check if not a spawn zone
			if(!GameHandler.WithinNoSpawn(hit.point))
			{
				Enemy enemy = DataHolder.Enemies().GetCopy(this.enemyID[index]);
				enemy.Init();
				enemy.SetBaseSpawner(this);
				GameObject obj = enemy.CreatePrefabInstance();
				if(obj != null)
				{
					obj.transform.position = hit.point+this.spawnOffset[index];
					DataHolder.BattleSystem().AddEnemy(enemy);
					this.spawnedQuantity[index]++;
				}
				spawned = true;
			}
		}
		return spawned;
	}
	
	/*
	============================================================================
	Respawn functions
	============================================================================
	*/
	public override void CheckRespawn(Enemy e)
	{
		if(DataHolder.BattleEnd().getImmediately)
		{
			for(int i=0; i<this.enemyID.Length; i++)
			{
				if(this.enemyID[i] == e.realID)
				{
					this.spawnedQuantity[i]--;
					if(this.respawnEnemy[i])
					{
						this.StartCoroutine(this.RespawnAfter(i, this.respawnTime[i]));
					}
					break;
				}
			}
		}
	}
	
	private IEnumerator RespawnAfter(int index, float time)
	{
		if(time > 0) yield return new WaitForSeconds(time);
		Vector3 castOrigin = this.GetSpawnCenter();
		while(!this.SpawnEnemy(index, castOrigin+this.GetRandomAdd()))
		{
			yield return null;
		}
	}
}
