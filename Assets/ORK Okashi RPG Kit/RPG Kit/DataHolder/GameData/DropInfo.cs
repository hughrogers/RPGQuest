
using UnityEngine;
using System.Collections;

public class DropInfo
{
	public Vector3 position = Vector3.zero;
	public ItemShort item = new ItemShort();
	
	public DropInfo(Hashtable ht)
	{
		this.SetData(ht);
	}
	
	public DropInfo(Vector3 pos, ItemDropType t, int itemID, int q)
	{
		this.SetPosition(pos);
		this.item.type = t;
		this.item.id = itemID;
		this.item.quantity = q;
	}
	
	public DropInfo(Vector3 pos, int moneyAmount)
	{
		this.SetPosition(pos);
		this.item.isMoney = true;
		this.item.quantity = moneyAmount;
	}
	
	public void SetPosition(Vector3 pos)
	{
		this.position = pos;
		this.position.x += Random.Range(DataHolder.GameSettings().dropOffsetX.x, DataHolder.GameSettings().dropOffsetX.y);
		this.position.y += Random.Range(DataHolder.GameSettings().dropOffsetY.x, DataHolder.GameSettings().dropOffsetY.y);
		this.position.z += Random.Range(DataHolder.GameSettings().dropOffsetZ.x, DataHolder.GameSettings().dropOffsetZ.y);
	}
	
	public void Drop()
	{
		GameObject go = new GameObject();
		go.transform.position = position;
		ItemCollector ic = (ItemCollector)go.AddComponent("ItemCollector");
		ic.startType = EventStartType.INTERACT;
		ic.onGround = DataHolder.GameSettings().dropOnGround;
		ic.layerMask = DataHolder.GameSettings().dropMask;
		ic.itemDropType = this.item.type;
		ic.itemID = this.item.id;
		ic.itemNumber = this.item.quantity;
		ic.isMoney = this.item.isMoney;
		ic.SetDropInfo(this);
	}
	
	// data handling
	public Hashtable GetData(string title)
	{
		Hashtable ht = new Hashtable();
		ht.Add(XMLHandler.NODE_NAME, title);
		ht.Add("x", this.position.x.ToString());
		ht.Add("y", this.position.y.ToString());
		ht.Add("z", this.position.z.ToString());
		return item.GetData(ht);
	}
	
	public void SetData(Hashtable ht)
	{
		this.position = new Vector3(float.Parse((string)ht["x"]), 
				float.Parse((string)ht["y"]), float.Parse((string)ht["z"]));
		this.item.SetData(ht);
	}
}