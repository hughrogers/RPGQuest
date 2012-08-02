
using UnityEngine;
using System.Collections;

[AddComponentMenu("RPG Kit/Battles/Random Battles")]
public class RandomBattles : MonoBehaviour
{
	public GameObject arenaObject;
	
	public float battleChance = 10.0f;
	public float checkInterval = 0.5f;
	
	// distance settings
	public bool ignoreYDistance = true;
	public float minMoveCheckDistance = 0.1f;
	public float minBattleDistance = 5.0f;
	public float maxBattleDistance = 15.0f;
	
	// enemy settings
	public EnemyTeam[] enemyTeam = new EnemyTeam[1];
	
	// ingame
	private BattleArena battleArena;
	private Vector3 lastPosition = Vector3.zero;
	private Vector3 lastBattlePosition = Vector3.zero;
	private float timeout = 0;
	
	/*
	============================================================================
	Inspector functions
	============================================================================
	*/
	public void AddEnemyTeam()
	{
		this.enemyTeam = ArrayHelper.Add(new EnemyTeam(), this.enemyTeam);
	}
	
	public void RemoveEnemyTeam(int index)
	{
		this.enemyTeam = ArrayHelper.Remove(index, this.enemyTeam);
	}
	
	/*
	============================================================================
	Initialize functions
	============================================================================
	*/
	void Start()
	{
		bool destroy = DataHolder.BattleSystem().IsRealTime();
		
		if(this.arenaObject == null || this.collider == null) destroy = true;
		else this.battleArena = this.arenaObject.GetComponent<BattleArena>();
		if(this.battleArena == null) destroy = true;
		
		if(destroy) GameObject.Destroy(this.gameObject);
		else
		{
			this.gameObject.layer = 2;
			this.battleArena.deactivateAfter = false;
		}
	}
	
	/*
	============================================================================
	Battle check functions
	============================================================================
	*/
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject == GameHandler.Party().GetPlayer())
		{
			this.timeout = 0;
			this.lastPosition = other.transform.position;
			this.lastBattlePosition = other.transform.position;
		}
	}
	
	void OnTriggerStay(Collider other)
	{
		if(GameHandler.IsControlField() && 
			other.gameObject == GameHandler.Party().GetPlayer())
		{
			this.timeout += GameHandler.DeltaMovementTime;
			if(VectorHelper.Distance(this.lastPosition, other.transform.position, this.ignoreYDistance) >= this.minMoveCheckDistance &&
				this.timeout >= this.checkInterval)
			{
				this.timeout -= this.checkInterval;
				this.lastPosition = other.transform.position;
				float distance = VectorHelper.Distance(this.lastBattlePosition, other.transform.position, this.ignoreYDistance);
				
				if(distance >= this.maxBattleDistance || 
					(distance >= this.minBattleDistance && 
					DataHolder.GameSettings().GetRandom() <= this.battleChance))
				{
					this.lastBattlePosition = other.transform.position;
					
					int team = 0;
					float rnd = DataHolder.GameSettings().GetRandom();
					float ch = 0;
					
					for(int i=0; i<this.enemyTeam.Length; i++)
					{
						if(rnd >= ch && rnd <= ch+this.enemyTeam[i].chance)
						{
							team = i;
							break;
						}
						else ch += this.enemyTeam[i].chance;
					}
					
					this.battleArena.SetEnemyTeam(this.enemyTeam[team].enemyID);
					this.battleArena.CallStart();
				}
			}
		}
	}
	
	/*
	============================================================================
	Gizmo functions
	============================================================================
	*/
	void OnDrawGizmos()
	{
		Gizmos.DrawIcon(transform.position, "BattleArena.psd");
	}
}
