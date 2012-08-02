
using System.Collections;

public class MusicData : BaseData
{
	public MusicClip[] music = new MusicClip[0];
	
	public string clipPath = "Audio/Music/";
	
	// XML data
	private string filename = "music";
	
	private static string MUSIC = "music";
	private static string CLIP = "clip";
	private static string RESOURCE = "resource";
	private static string LOOP = "loop";

	public MusicData()
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
				if(entry[XMLHandler.NODE_NAME] as string == MusicData.MUSIC)
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList subs = entry[XMLHandler.NODES] as ArrayList;
						name = new string[subs.Count];
						music = new MusicClip[subs.Count];
						foreach(Hashtable val in subs)
						{
							if(val[XMLHandler.NODE_NAME] as string == MusicData.CLIP)
							{
								int i = int.Parse((string)val["id"]);
								music[i] = new MusicClip();
								music[i].maxVolume = float.Parse((string)val["maxvol"]);
								if(val.ContainsKey("loop")) music[i].loop = true;
								
								int count = int.Parse((string)val["loops"]);
								music[i].checkTime = new float[count];
								music[i].setTime = new float[count];
								
								ArrayList s = val[XMLHandler.NODES] as ArrayList;
								foreach(Hashtable ht in s)
								{
									if(ht[XMLHandler.NODE_NAME] as string == MusicData.RESOURCE)
									{
										music[i].clipName = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == MusicData.NAME)
									{
										name[i] = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == MusicData.LOOP)
									{
										int j = int.Parse((string)ht["id"]);
										music[i].checkTime[j] = float.Parse((string)ht["check"]);
										music[i].setTime[j] = float.Parse((string)ht["set"]);
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
		ArrayList data = new ArrayList();
		ArrayList subs = new ArrayList();
		Hashtable sv = new Hashtable();
		
		sv.Add(XMLHandler.NODE_NAME, MusicData.MUSIC);
		
		if(name != null)
		{
			for(int i=0; i<name.Length; i++)
			{
				Hashtable ht = new Hashtable();
				ArrayList s = new ArrayList();
				
				ht.Add(XMLHandler.NODE_NAME, MusicData.CLIP);
				ht.Add("id", i.ToString());
				ht.Add("loops", music[i].checkTime.Length.ToString());
				ht.Add("maxvol", music[i].maxVolume.ToString());
				if(music[i].loop) ht.Add("loop", "true");
				
				Hashtable n = new Hashtable();
				n.Add(XMLHandler.NODE_NAME, MusicData.RESOURCE);
				n.Add(XMLHandler.CONTENT, music[i].clipName);
				s.Add(n);
				
				n = new Hashtable();
				n.Add(XMLHandler.NODE_NAME, MusicData.NAME);
				n.Add(XMLHandler.CONTENT, name[i]);
				s.Add(n);
				
				for(int j=0; j<music[i].checkTime.Length; j++)
				{
					n = new Hashtable();
					n.Add(XMLHandler.NODE_NAME, MusicData.LOOP);
					n.Add("id", j.ToString());
					n.Add("check", music[i].checkTime[j].ToString());
					n.Add("set", music[i].setTime[j].ToString());
					s.Add(n);
				}
				ht.Add(XMLHandler.NODES, s);
				subs.Add(ht);
			}
			sv.Add(XMLHandler.NODES, subs);
		}
		
		data.Add(sv);
		XMLHandler.SaveXML(dir, filename, data);
	}
	
	public void AddMusic(string n)
	{
		if(name == null)
		{
			name = new string[] {n};
			music = new MusicClip[] {new MusicClip()};
		}
		else
		{
			name = ArrayHelper.Add(n, name);
			music = ArrayHelper.Add(new MusicClip(), music);
		}
	}
	
	public override void RemoveData(int index)
	{
		name = ArrayHelper.Remove(index, name);
		if (name.Length == 0) name = null;
		music = ArrayHelper.Remove(index, music);
	}
	
	public MusicClip GetCopy(int index)
	{
		MusicClip m = new MusicClip();
		m.realID = index;
		m.clipName = music[index].clipName;
		m.maxVolume = music[index].maxVolume;
		m.loop = music[index].loop;
		m.checkTime = new float[music[index].checkTime.Length];
		m.setTime = new float[music[index].setTime.Length];
		for(int i=0; i<m.checkTime.Length; i++)
		{
			m.checkTime[i] = music[index].checkTime[i];
			m.setTime[i] = music[index].setTime[i];
		}
		return m;
	}
	
	public override void Copy(int index)
	{
		name = ArrayHelper.Add(name[index], name);
		music = ArrayHelper.Add(this.GetCopy(index), music);
	}
}