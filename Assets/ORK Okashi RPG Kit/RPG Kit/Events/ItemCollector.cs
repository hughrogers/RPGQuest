
using UnityEngine;
using System.Collections;

[AddComponentMenu("RPG Kit/Events/Item Collector")]
public class ItemCollector : BaseInteraction
{
	public bool isMoney = false;
	public ItemDropType itemDropType = ItemDropType.ITEM;
	public int itemID = 0;
	public int itemNumber = 1;
	public bool spawnPrefab = true;
	public Vector3 offset = Vector3.zero;
	public bool localSpace = false;
	public Vector3 rotationOffset = Vector3.zero;
	
	// place on ground
	public bool onGround = true;
	public float distance = 100.0f;
	public LayerMask layerMask = 0;
	
	private GameObject pref;
	private DropInfo dropInfo;
	
	public void SetDropInfo(DropInfo info)
	{
		this.dropInfo = info;
	}
	
	void Start()
	{
		if(this.autoDestroyOnVariables && !this.CheckVariables())
		{
			GameObject.Destroy(this.gameObject);
		}
		else if(spawnPrefab)
		{
			if(this.isMoney)
			{
				this.pref = DataHolder.GameSettings().GetMoneyPrefabInstance();
			}
			else if(ItemDropType.ITEM.Equals(this.itemDropType))
			{
				this.pref = DataHolder.Item(itemID).GetPrefabInstance();
			}
			else if(ItemDropType.WEAPON.Equals(this.itemDropType))
			{
				this.pref = DataHolder.Weapon(itemID).GetPrefabInstance();
			}
			else if(ItemDropType.ARMOR.Equals(this.itemDropType))
			{
				this.pref = DataHolder.Armor(itemID).GetPrefabInstance();
			}
			if(this.pref != null)
			{
				TransformHelper.Mount(this.transform, this.pref.transform, 
						true, this.localSpace, this.offset, true, this.rotationOffset);
				this.pref.AddComponent<ItemCollectorForward>().startType = this.startType;
			}
		}
	}
	
	public void SetOnGround()
	{
		if(this.onGround)
		{
			RaycastHit hit = new RaycastHit();
			if(Physics.Raycast(transform.position, -Vector3.up, out hit, this.distance, 1 << this.layerMask.value))
			{
				transform.position = hit.point;
			}
		}
	}
	
	void Awake()
	{
		this.SetOnGround();
		if(EventStartType.AUTOSTART.Equals(this.startType) && this.CheckVariables())
		{
			this.CollectItem();
		}
	}
	
	void Update()
	{
		if(this.KeyPress()) this.CollectItem();
	}
	
	public void OnTriggerEnter(Collider other)
	{
		if(this.CheckTriggerEnter(other) && GameHandler.IsControlField())
		{
			this.CollectItem();
		}
	}
	
	public void OnTriggerExit(Collider other)
	{
		if(this.CheckTriggerExit(other) && GameHandler.IsControlField())
		{
			this.CollectItem();
		}
	}
	
	public override void TouchInteract()
	{
		this.OnMouseUp();
	}
	
	public void OnMouseUp()
	{
		if(EventStartType.INTERACT.Equals(this.startType) && GameHandler.IsControlField() &&
				this.CheckVariables() && this.gameObject.active && DataHolder.GameSettings().IsMouseAllowed())
		{
			GameObject p = GameHandler.GetPlayer();
			if(p && Vector3.Distance(p.transform.position, this.transform.position) < this.maxMouseDistance)
			{
				this.CollectItem();
			}
		}
	}
	
	public override bool Interact()
	{
		bool val = false;
		// start event on interaction here
		if(EventStartType.INTERACT.Equals(this.startType) && GameHandler.IsControlField() &&
			this.CheckVariables() && this.gameObject.active)
		{
			this.CollectItem();
			val = true;
		}
		return val;
	}
	
	public override bool DropInteract(ChoiceContent drop)
	{
		bool val = false;
		if(EventStartType.DROP.Equals(this.startType) && GameHandler.IsControlField() &&
			this.CheckVariables() && this.gameObject.active && this.CheckDrop(drop))
		{
			this.CollectItem();
			val = true;
		}
		return val;
	}
	
	public void CollectItem()
	{
		GameHandler.SetControlType(ControlType.EVENT);
		if(this.isMoney)
		{
			GameHandler.GetLevelHandler().ShowItemCollection(
					DataHolder.GameSettings().GetMoneyCollectionString(this.itemNumber),
					DataHolder.GameSettings().GetMoneyCollectionChoice(this.itemNumber), this);
		}
		else
		{
			GameHandler.GetLevelHandler().ShowItemCollection(
					DataHolder.GameSettings().GetItemCollectionString(this.itemID, this.itemNumber, this.itemDropType),
					DataHolder.GameSettings().GetItemCollectionChoice(this.itemID, this.itemNumber, this.itemDropType), this);
		}
	}
	
	public void CollectionFinished(bool ok)
	{
		this.StartCoroutine(this.CollectionFinished2(ok));
	}
	
	private IEnumerator CollectionFinished2(bool ok)
	{
		if(ok)
		{
			if(DataHolder.GameSettings().itemCollectionAnimation != "")
			{
				Character p = GameHandler.Party().GetPlayerCharacter();
				if(p != null)
				{
					Animation anim = p.GetAnimationComponent();
					if(anim != null && anim[DataHolder.GameSettings().itemCollectionAnimation] != null)
					{
						anim.Play(DataHolder.GameSettings().itemCollectionAnimation);
						yield return new WaitForSeconds(AnimationHelper.GetLength(anim, DataHolder.GameSettings().itemCollectionAnimation));
					}
				}
			}
			
			this.SetVariables();
			if(this.isMoney)
			{
				GameHandler.AddMoney(this.itemNumber);
			}
			else if(ItemDropType.ITEM.Equals(this.itemDropType))
			{
				GameHandler.AddItem(this.itemID, this.itemNumber);
			}
			else if(ItemDropType.WEAPON.Equals(this.itemDropType))
			{
				GameHandler.AddWeapon(this.itemID, this.itemNumber);
			}
			else if(ItemDropType.ARMOR.Equals(this.itemDropType))
			{
				GameHandler.AddArmor(this.itemID, this.itemNumber);
			}
			if(this.dropInfo != null) GameHandler.DropHandler().Collected(this.dropInfo);
			GameHandler.GetLevelHandler().interactionList.Remove(this.gameObject);
			if(this.pref != null) GameHandler.GetLevelHandler().interactionList.Remove(this.pref);
			GameObject.Destroy(this.gameObject);
		}
		GameHandler.SetControlType(ControlType.FIELD);
	}
	
	void OnDrawGizmos()
	{
		Gizmos.DrawIcon(transform.position, "ItemCollector.psd");
	}
}
