
using System.Collections;

public class GlobalEventData : BaseData
{
	public GlobalEvent[] globalEvent = new GlobalEvent[0];
	
	// XML data
	private string filename = "globalEvents";
	
	public static string PREFAB_PATH = "Prefabs/GlobalEvents/";
	public static string AUDIO_PATH = "Audio/GlobalEvents/";
	
	private static string GLOBALEVENTS = "globalevents";
	private static string GLOBALEVENT = "globalevent";

	public GlobalEventData()
	{
		LoadData();
	}
	
	public void LoadData()
	{
		ArrayList data = XMLHandler.LoadXML(dir+filename);
		
		if(data.Count > 0)
		{
			foreach(Hashtable entry in data)
			{
				if(entry[XMLHandler.NODE_NAME] as string == GlobalEventData.GLOBALEVENTS)
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList subs = entry[XMLHandler.NODES] as ArrayList;
						name = new string[subs.Count/2];
						globalEvent = new GlobalEvent[subs.Count/2];
						foreach(Hashtable ht in subs)
						{
							if(ht[XMLHandler.NODE_NAME] as string == GlobalEventData.NAME)
							{
								int id = int.Parse((string)ht["id"]);
								if(id < name.Length)
								{
									name[id] = ht[XMLHandler.CONTENT] as string;
								}
							}
							else if(ht[XMLHandler.NODE_NAME] as string == GlobalEventData.GLOBALEVENT)
							{
								int id = int.Parse((string)ht["id"]);
								if(id < globalEvent.Length)
								{
									globalEvent[id] = new GlobalEvent();
									globalEvent[id].SetData(ht);
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
		ArrayList data = new ArrayList();
		ArrayList subs = new ArrayList();
		Hashtable sv = new Hashtable();
		
		sv.Add(XMLHandler.NODE_NAME, GlobalEventData.GLOBALEVENTS);
		
		if(name != null)
		{
			for(int i=0; i<name.Length; i++)
			{
				subs.Add(HashtableHelper.GetContentHashtable(GlobalEventData.NAME, name[i], i));
			}
			for(int i=0; i<globalEvent.Length; i++)
			{
				subs.Add(globalEvent[i].GetData(HashtableHelper.GetTitleHashtable(GlobalEventData.GLOBALEVENT, i)));
			}
			if(subs.Count > 0) sv.Add(XMLHandler.NODES, subs);
		}
		
		data.Add(sv);
		XMLHandler.SaveXML(dir, filename, data);
	}
	
	public void AddEvent(string n)
	{
		if(name == null)
		{
			name = new string[] {n};
		}
		else
		{
			name = ArrayHelper.Add(n, name);
		}
		globalEvent = ArrayHelper.Add(new GlobalEvent(), globalEvent);
	}
	
	public override void RemoveData(int index)
	{
		name = ArrayHelper.Remove(index, name);
		if(name.Length == 0) name = null;
		globalEvent = ArrayHelper.Remove(index, globalEvent);
	}
	
	public override void Copy(int index)
	{
		name = ArrayHelper.Add(name[index], name);
		globalEvent = ArrayHelper.Add(globalEvent[index].GetCopy(), globalEvent);
	}
}