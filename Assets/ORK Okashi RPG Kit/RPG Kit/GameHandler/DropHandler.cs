
using UnityEngine;
using System.Collections;

public class DropHandler : MonoBehaviour
{
	public Hashtable drops;
	
	private static string SCENE = "scene";
	private static string SCENENAME = "scenename";
	private static string DROP = "DROP";
	
	public void ClearData()
	{
		this.drops = new Hashtable();
	}
	
	void Awake()
	{
		DontDestroyOnLoad(transform);
	}
	
	void OnLevelWasLoaded(int level)
	{
		this.SceneDrops();
	}
	
	public void SceneDrops()
	{
		if(DataHolder.GameSettings().saveDrops && this.drops.ContainsKey(Application.loadedLevelName))
		{
			ArrayList scene = (ArrayList)this.drops[Application.loadedLevelName];
			foreach(DropInfo info in scene)
			{
				info.Drop();
			}
		}
	}
	
	public void Collected(DropInfo info)
	{
		if(DataHolder.GameSettings().saveDrops && this.drops.ContainsKey(Application.loadedLevelName))
		{
			ArrayList scene = (ArrayList)this.drops[Application.loadedLevelName];
			this.drops.Remove(Application.loadedLevelName);
			scene.Remove(info);
			this.drops.Add(Application.loadedLevelName, scene);
		}
	}
	
	public void Drop(ItemDropType type, int itemID, int quantity)
	{
		bool drop = false;
		if(ItemDropType.ITEM.Equals(type))
		{
			drop = DataHolder.Item(itemID).dropable;
		}
		else if(ItemDropType.WEAPON.Equals(type))
		{
			drop = DataHolder.Weapon(itemID).dropable;
		}
		else if(ItemDropType.ARMOR.Equals(type))
		{
			drop = DataHolder.Armor(itemID).dropable;
		}
		
		if(drop)
		{
			this.Drop(GameHandler.GetPlayer().transform.position, type, itemID, quantity);
		}
	}
	
	public void Drop(Vector3 position, ItemDropType type, int itemID, int quantity)
	{
		// drop to world
		DropInfo info = new DropInfo(position, type, itemID, quantity);
		info.Drop();
		
		// save position
		if(DataHolder.GameSettings().saveDrops)
		{
			ArrayList scene = null;
			if(this.drops.ContainsKey(Application.loadedLevelName))
			{
				scene = (ArrayList)this.drops[Application.loadedLevelName];
				this.drops.Remove(Application.loadedLevelName);
			}
			else
			{
				scene = new ArrayList();
			}
			scene.Add(info);
			this.drops.Add(Application.loadedLevelName, scene);
		}
	}
	
	public void Drop(Vector3 position, int moneyAmount)
	{
		// drop to world
		DropInfo info = new DropInfo(position, moneyAmount);
		info.Drop();
		
		// save position
		if(DataHolder.GameSettings().saveDrops)
		{
			ArrayList scene = null;
			if(this.drops.ContainsKey(Application.loadedLevelName))
			{
				scene = (ArrayList)this.drops[Application.loadedLevelName];
				this.drops.Remove(Application.loadedLevelName);
			}
			else
			{
				scene = new ArrayList();
			}
			scene.Add(info);
			this.drops.Add(Application.loadedLevelName, scene);
		}
	}
	
	// data handling
	public Hashtable GetData(string title)
	{
		Hashtable ht = new Hashtable();
		ht.Add(XMLHandler.NODE_NAME, title);
		ArrayList s = new ArrayList();
		foreach(DictionaryEntry entry in this.drops)
		{
			Hashtable scene = new Hashtable();
			scene.Add(XMLHandler.NODE_NAME, DropHandler.SCENE);
			ArrayList ss = new ArrayList();
			
			Hashtable sn = new Hashtable();
			sn.Add(XMLHandler.NODE_NAME, DropHandler.SCENENAME);
			sn.Add(XMLHandler.CONTENT, (string)entry.Key);
			ss.Add(sn);
			
			foreach(DropInfo info in (ArrayList)entry.Value)
			{
				ss.Add(info.GetData(DropHandler.DROP));
			}
			if(ss.Count > 0) scene.Add(XMLHandler.NODES, ss);
			s.Add(scene);
		}
		if(s.Count > 0) ht.Add(XMLHandler.NODES, s);
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		this.ClearData();
		ArrayList s = ht[XMLHandler.NODES] as ArrayList;
		if(s != null)
		{
			foreach(Hashtable ht2 in s)
			{
				if(ht2[XMLHandler.NODE_NAME] as string == DropHandler.SCENE)
				{
					ArrayList scene = new ArrayList();
					string sn = "";
					ArrayList ss = ht2[XMLHandler.NODES] as ArrayList;
					foreach(Hashtable ht3 in ss)
					{
						if(ht3[XMLHandler.NODE_NAME] as string == DropHandler.SCENENAME)
						{
							sn = ht3[XMLHandler.CONTENT] as string;
						}
						else if(ht3[XMLHandler.NODE_NAME] as string == DropHandler.DROP)
						{
							scene.Add(new DropInfo(ht3));
						}
					}
					this.drops.Add(sn, scene);
				}
			}
		}
	}
}