
using System.Collections;
using UnityEngine;

public class TeleportData : BaseLangData
{
	public TeleportTarget[] teleport = new TeleportTarget[0];
	
	// XML data
	private string filename = "teleports";
	
	private static string TELEPORTS = "teleports";
	private static string TELEPORT = "teleport";
	private static string DATA = "data";

	public TeleportData()
	{
		LoadData();
	}
	
	public override string GetIconPath() { return "Icons/Teleports/"; }
	
	public void LoadData()
	{
		ArrayList data = XMLHandler.LoadXML(dir+filename);
		
		if(data.Count > 0)
		{
			foreach(Hashtable entry in data)
			{
				if(entry[XMLHandler.NODE_NAME] as string == TeleportData.TELEPORTS)
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList subs = entry[XMLHandler.NODES] as ArrayList;
						this.icon = new string[subs.Count];
						this.teleport = new TeleportTarget[subs.Count];
						
						foreach(Hashtable val in subs)
						{
							if(val[XMLHandler.NODE_NAME] as string == TeleportData.TELEPORT)
							{
								int i = int.Parse((string)val["id"]);
								this.icon[i] = "";
								
								this.teleport[i] = new TeleportTarget();
								
								ArrayList s = val[XMLHandler.NODES] as ArrayList;
								foreach(Hashtable ht in s)
								{
									this.LoadLanguages(ht, i, subs.Count);
									if(ht[XMLHandler.NODE_NAME] as string == TeleportData.DATA)
									{
										this.teleport[i].SetData(ht);
									}
								}
							}
						}
					}
				}
			}
		}
	}
	
	public void SaveData()
	{
		if(name.Length > 0)
		{
			ArrayList data = new ArrayList();
			ArrayList subs = new ArrayList();
			
			Hashtable sv = new Hashtable();
			sv.Add(XMLHandler.NODE_NAME, TeleportData.TELEPORTS);
			
			for(int i=0; i<name[0].Count(); i++)
			{
				Hashtable val = new Hashtable();
				ArrayList s = new ArrayList();
				
				val.Add(XMLHandler.NODE_NAME, TeleportData.TELEPORT);
				val.Add("id", i.ToString());
				s.Add(this.teleport[i].GetData(HashtableHelper.GetTitleHashtable(TeleportData.DATA)));
				
				s = this.SaveLanguages(s, i);
				val.Add(XMLHandler.NODES, s);
				subs.Add(val);
			}
			sv.Add(XMLHandler.NODES, subs);
			data.Add(sv);
			
			XMLHandler.SaveXML(dir, filename, data);
		}
	}
	
	public void AddTeleport(string n, string d, int count)
	{
		base.AddBaseData(n, d, count);
		this.teleport = ArrayHelper.Add(new TeleportTarget(), this.teleport);
	}
	
	public override void RemoveData(int index)
	{
		base.RemoveData(index);
		this.teleport = ArrayHelper.Remove(index, this.teleport);
	}
	
	public override void Copy(int index)
	{
		base.Copy(index);
		this.teleport = ArrayHelper.Add(this.teleport[index].GetCopy(), this.teleport);
	}
	
	public int[] GetTeleportIDs(bool ignoreConditions)
	{
		int[] ids = new int[0];
		for(int i=0; i<this.teleport.Length; i++)
		{
			if(ignoreConditions || this.teleport[i].Available())
			{
				ids = ArrayHelper.Add(i, ids);
			}
		}
		return ids;
	}
	
	public string[] GetNamesForIDs(int[] ids)
	{
		string[] names = new string[ids.Length];
		for(int i=0; i<names.Length; i++)
		{
			names[i] = this.GetName(ids[i]);
		}
		return names;
	}
}