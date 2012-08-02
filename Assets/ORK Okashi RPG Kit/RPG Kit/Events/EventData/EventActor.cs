
using System.Collections;
using UnityEngine;

public class EventActor
{
	public bool fold = true;
	
	public bool isPlayer = false;
	public bool overrideName = false;
	public bool showName = false;
	public string[] dialogName = new string[0];
	
	// ingame
	public GameObject actor;
	
	public EventActor()
	{
		
	}
	
	public GameObject GetActor()
	{
		if(this.actor == null && this.isPlayer)
		{
			this.actor = GameHandler.GetPlayer();
		}
		return this.actor;
	}
	
	public string GetName()
	{
		string name = "";
		if(this.showName && this.isPlayer && !this.overrideName)
		{
			int id = GameHandler.Party().GetPlayerID();
			Character c = GameHandler.Party().GetPartyMember(id);
			if(c != null)
			{
				name = c.GetName();
			}
		}
		else if(this.showName)
		{
			name = this.dialogName[GameHandler.GetLanguage()];
		}
		return name;
	}
	
	public Hashtable GetData()
	{
		Hashtable ht = new Hashtable();
		if(this.isPlayer) ht.Add("player", "true");
		if(this.showName) ht.Add("showname", "true");
		
		if(this.showName && (!this.isPlayer || (this.isPlayer && this.overrideName)))
		{
			ArrayList subs = new ArrayList();
			for(int i=0; i<this.dialogName.Length; i++)
			{
				Hashtable s = new Hashtable();
				s.Add(XMLHandler.NODE_NAME, "name");
				s.Add("id", i.ToString());
				s.Add(XMLHandler.CONTENT, this.dialogName[i]);
				subs.Add(s);
			}
			ht.Add(XMLHandler.NODES, subs);
		}
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		if(ht.ContainsKey("player")) this.isPlayer = true;
		if(ht.ContainsKey("showname")) this.showName = true;
		
		this.dialogName = new string[DataHolder.Languages().GetDataCount()];
		for(int i=0; i<this.dialogName.Length; i++)
		{
			this.dialogName[i] = "";
		}
		
		if(ht.ContainsKey(XMLHandler.NODES))
		{
			if(this.isPlayer) this.overrideName = true;
			ArrayList subs2 = ht[XMLHandler.NODES] as ArrayList;
			foreach(Hashtable ht2 in subs2)
			{
				if(ht2[XMLHandler.NODE_NAME] as string == "name")
				{
					int id = int.Parse((string)ht2["id"]);
					if(id < this.dialogName.Length) this.dialogName[id] = ht2[XMLHandler.CONTENT] as string;
				}
			}
		}
	}
}