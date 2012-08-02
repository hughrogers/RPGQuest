
using UnityEngine;
using System.Collections;

[AddComponentMenu("RPG Kit/Real Time Battles/Damage Dealer")]
public class DamageDealer : DamageBase
{
	public DamageDealerType type = DamageDealerType.TRIGGER_ENTER;
	public float destroyAfter = 0;
	public bool destroyOnDamage = false;
	public bool destroyOnCollision = false;
	
	public bool changeCollider = false;
	public float expand = 0;
	
	public bool baseAttack = false;
	public int[] skillID = new int[0];
	public int[] itemID = new int[0];
	
	public bool singleDamage = false;
	public bool singleEnemy = false;
	
	public float dmgEvery = 0;
	
	// ingame
	private BattleAction action = null;
	private bool damageActive = false;
	
	private Hashtable blocked = new Hashtable();
	
	private AudioClip audioClip = null;
	private float aVolume = 1;
	private float aMinDistance = 0;
	private float aMaxDistance = 100;
	private float aPitch = 1;
	private AudioRolloffMode audioRolloffMode = AudioRolloffMode.Linear;
	
	private GameObject prefab = null;
	private float destroyPrefabAfter = 0;
	
	private bool doExpand = false;
	
	void Awake()
	{
		if(this.destroyAfter > 0)
		{
			GameObject.Destroy(this.gameObject, this.destroyAfter);
		}
	}
	
	/*
	============================================================================
	Inspector functions
	============================================================================
	*/
	public void AddSkill()
	{
		this.skillID = ArrayHelper.Add(0, this.skillID);
	}
	
	public void RemoveSkill(int index)
	{
		this.skillID = ArrayHelper.Remove(index, this.skillID);
	}
	
	public void AddItem()
	{
		this.itemID = ArrayHelper.Add(0, this.itemID);
	}
	
	public void RemoveItem(int index)
	{
		this.itemID = ArrayHelper.Remove(index, this.itemID);
	}
	
	/*
	============================================================================
	Action functions
	============================================================================
	*/
	public void SetAudioClip(AudioClip a, float v, float min, float max, float p, AudioRolloffMode arm)
	{
		this.audioClip = a;
		this.aVolume = v;
		this.aMinDistance = min;
		this.aMaxDistance = max;
		this.aPitch = p;
		this.audioRolloffMode = arm;
	}
	
	public void SetPrefab(GameObject p, float t)
	{
		this.prefab = p;
		this.destroyPrefabAfter = t;
	}
	
	public void SetAction(BattleAction a)
	{
		this.action = a;
		if(this.action != null && this.action.user != null && 
			this.combatant == null)
		{
			this.combatant = this.action.user;
		}
	}
	
	public void SetDamageActive(bool dmg)
	{
		this.damageActive = dmg;
		if(this.damageActive)
		{
			this.doExpand = true;
			if(this.action == null || 
				!this.action.CheckDamageDealer(this))
			{
				this.damageActive = false;
			}
		}
		if(!this.damageActive)
		{
			this.doExpand = false;
			this.blocked = new Hashtable();
			this.audioClip = null;
			this.prefab = null;
		}
	}
	
	private void DoDamage(GameObject obj, Vector3 position, Quaternion rotation)
	{
		if(this.action != null)
		{
			DamageZone zone = (DamageZone)obj.GetComponent(typeof(DamageZone));
			if(zone != null && this.action.CanDamage(zone.GetCombatant()) &&
				(!this.singleDamage || !this.blocked.ContainsKey(zone.GetBattleID())) &&
				(!this.singleEnemy || this.blocked.Count == 0 || 
				(this.blocked.Count == 1 && this.blocked.ContainsKey(zone.GetBattleID()))) &&
				(this.dmgEvery == 0 || !this.blocked.ContainsKey(zone.GetBattleID())))
			{
				int id = zone.Damage(this.action);
				if(this.prefab != null)
				{
					GameObject pref = (GameObject)GameObject.Instantiate(this.prefab, position, rotation);
					if(this.destroyPrefabAfter > 0) GameObject.Destroy(pref, this.destroyPrefabAfter);
				}
				if(this.audioClip != null)
				{
					if(obj.audio == null) obj.AddComponent<AudioSource>();
					obj.audio.pitch = this.aPitch;
					obj.audio.volume = this.aVolume;
					obj.audio.rolloffMode = this.audioRolloffMode;
					obj.audio.minDistance = this.aMinDistance;
					obj.audio.maxDistance = this.aMaxDistance;
					obj.audio.PlayOneShot(this.audioClip);
				}
				
				if(!this.blocked.ContainsKey(id)) this.blocked.Add(id, this.dmgEvery);
				if(this.destroyOnDamage)
				{
					GameObject.Destroy(this.gameObject);
				}
			}
		}
	}
	
	public bool CheckOrigin(Transform t)
	{
		bool ok = this.transform.root != t;
		if(ok && this.combatant != null && 
			this.combatant.prefabInstance != null)
		{
			ok = this.combatant.prefabInstance.transform.root != t;
		}
		return ok;
	}
	
	void Update()
	{
		if(this.dmgEvery > 0)
		{
			int[] keys = new int[this.blocked.Count];
			this.blocked.Keys.CopyTo(keys, 0);
			for(int i=0; i<keys.Length; i++)
			{
				float t = (float)this.blocked[keys[i]];
				t -= GameHandler.DeltaBattleTime;
				if(t > 0) this.blocked[keys[i]] = t;
				else this.blocked.Remove(keys[i]);
			}
		}
		if(this.collider != null && this.changeCollider && this.doExpand)
		{
			float change = expand*GameHandler.DeltaBattleTime;
			if(this.collider is BoxCollider)
			{
				BoxCollider bc = this.collider as BoxCollider;
				bc.size += new Vector3(change, change, change);
			}
			else if(this.collider is SphereCollider)
			{
				SphereCollider sc = this.collider as SphereCollider;
				sc.radius += change;
			}
			else if(this.collider is CapsuleCollider)
			{
				CapsuleCollider cc = this.collider as CapsuleCollider;
				cc.radius += change;
				cc.height += change;
			}
		}
	}
	
	/*
	============================================================================
	Trigger functions
	============================================================================
	*/
	void OnTriggerEnter(Collider collider)
	{
		if(this.damageActive && DamageDealerType.TRIGGER_ENTER.Equals(this.type) &&
			this.CheckOrigin(collider.transform.root))
		{
			this.DoDamage(collider.gameObject, collider.transform.position, collider.transform.rotation);
			if(this.destroyOnCollision) GameObject.Destroy(this.gameObject);
		}
	}
	
	void OnTriggerExit(Collider collider)
	{
		if(this.damageActive && DamageDealerType.TRIGGER_EXIT.Equals(this.type) &&
			this.CheckOrigin(collider.transform.root))
		{
			this.DoDamage(collider.gameObject, collider.transform.position, collider.transform.rotation);
			if(this.destroyOnCollision) GameObject.Destroy(this.gameObject);
		}
	}
	
	void OnTriggerStay(Collider collider)
	{
		if(this.damageActive && DamageDealerType.TRIGGER_STAY.Equals(this.type) &&
			this.CheckOrigin(collider.transform.root))
		{
			this.DoDamage(collider.gameObject, collider.transform.position, collider.transform.rotation);
			if(this.destroyOnCollision) GameObject.Destroy(this.gameObject);
		}
	}
	
	/*
	============================================================================
	Collision functions
	============================================================================
	*/
	void OnCollisionEnter(Collision collision)
	{
		if(this.damageActive && DamageDealerType.COLLISION_ENTER.Equals(this.type) &&
			this.CheckOrigin(collision.transform.root))
		{
			Vector3 position = collision.transform.position;
			if(collision.contacts.Length > 0) position = collision.contacts[0].point;
			this.DoDamage(collision.gameObject, position, collision.transform.rotation);
			if(this.destroyOnCollision) GameObject.Destroy(this.gameObject);
		}
	}
	
	void OnCollisionExit(Collision collision)
	{
		if(this.damageActive && DamageDealerType.COLLISION_EXIT.Equals(this.type) &&
			this.CheckOrigin(collision.transform.root))
		{
			Vector3 position = collision.transform.position;
			if(collision.contacts.Length > 0) position = collision.contacts[0].point;
			this.DoDamage(collision.gameObject, position, collision.transform.rotation);
			if(this.destroyOnCollision) GameObject.Destroy(this.gameObject);
		}
	}
	
	void OnCollisionStay(Collision collision)
	{
		if(this.damageActive && DamageDealerType.COLLISION_STAY.Equals(this.type) &&
			this.CheckOrigin(collision.transform.root))
		{
			Vector3 position = collision.transform.position;
			if(collision.contacts.Length > 0) position = collision.contacts[0].point;
			this.DoDamage(collision.gameObject, position, collision.transform.rotation);
			if(this.destroyOnCollision) GameObject.Destroy(this.gameObject);
		}
	}
}
