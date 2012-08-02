
using System.Collections;

public class SkillTypeData : BaseLangData
{
	// XML data
	private string filename = "skilltypes";
	
	private static string SKILLTYPES = "skilltypes";
	private static string SKILLTYPE = "skilltype";

	public SkillTypeData()
	{
		LoadData();
	}
	
	public override string GetIconPath() { return "Icons/SkillType/"; }
	
	public void LoadData()
	{
		ArrayList data = XMLHandler.LoadXML(dir+filename);
		
		if(data.Count > 0)
		{
			foreach(Hashtable entry in data)
			{
				if(entry[XMLHandler.NODE_NAME] as string == SkillTypeData.SKILLTYPES)
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList subs = entry[XMLHandler.NODES] as ArrayList;
						icon = new string[subs.Count];
						
						foreach(Hashtable val in subs)
						{
							if(val[XMLHandler.NODE_NAME] as string == SkillTypeData.SKILLTYPE)
							{
								int i = int.Parse((string)val["id"]);
								icon[i] = "";
								
								ArrayList s = val[XMLHandler.NODES] as ArrayList;
								foreach(Hashtable ht in s)
								{
									this.LoadLanguages(ht, i, subs.Count);
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
			sv.Add(XMLHandler.NODE_NAME, SkillTypeData.SKILLTYPES);
			
			for(int i=0; i<name[0].Count(); i++)
			{
				Hashtable val = new Hashtable();
				ArrayList s = new ArrayList();
				
				val.Add(XMLHandler.NODE_NAME, SkillTypeData.SKILLTYPE);
				val.Add("id", i.ToString());
				
				s = this.SaveLanguages(s, i);
				val.Add(XMLHandler.NODES, s);
				subs.Add(val);
			}
			sv.Add(XMLHandler.NODES, subs);
			data.Add(sv);
			
			XMLHandler.SaveXML(dir, filename, data);
		}
	}
}