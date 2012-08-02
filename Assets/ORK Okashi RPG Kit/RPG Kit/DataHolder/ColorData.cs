
using System.Collections;
using UnityEngine;

public class ColorData : BaseData
{
	public Color[] color;
	
	// XML data
	private string filename = "textColors";
	
	private static string TEXTCOLORS = "textcolors";
	private static string COLOR = "color";
	
	public ColorData()
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
				if(entry[XMLHandler.NODE_NAME] as string == ColorData.TEXTCOLORS)
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList subs = entry[XMLHandler.NODES] as ArrayList;
						name = new string[subs.Count];
						color = new Color[subs.Count];
						foreach(Hashtable c in subs)
						{
							int i = int.Parse((string)c["id"]);
							name[i] = c[XMLHandler.CONTENT] as string;
							color[i] = new Color(float.Parse((string)c["r"]),
														float.Parse((string)c["g"]),
														float.Parse((string)c["b"]),
														float.Parse((string)c["a"]));
						}
					}
				}
			}
		}
		else
		{
			this.AddColor("White", new Color(1, 1, 1, 1));
			this.AddColor("Shadow", new Color(0, 0, 0, 1));
		}
	}
	
	public void SaveData()
	{
		ArrayList data = new ArrayList();
		ArrayList subs = new ArrayList();
		Hashtable tc = new Hashtable();
		
		tc.Add(XMLHandler.NODE_NAME, ColorData.TEXTCOLORS);
		
		for(int i=0; i<name.Length; i++)
		{
			Hashtable c = new Hashtable();
			c.Add(XMLHandler.NODE_NAME, ColorData.COLOR);
			c.Add("id", i.ToString());
			c.Add(XMLHandler.CONTENT, name[i]);
			c.Add("r", color[i].r.ToString());
			c.Add("g", color[i].g.ToString());
			c.Add("b", color[i].b.ToString());
			c.Add("a", color[i].a.ToString());
			subs.Add(c);
		}
		tc.Add(XMLHandler.NODES, subs);
		
		data.Add(tc);
		XMLHandler.SaveXML(dir, filename, data);
	}
	
	public void AddColor(string n, Color c)
	{
		if(name == null)
		{
			name = new string[] {n};
			color = new Color[] {c};
		}
		else
		{
			name = ArrayHelper.Add(n, name);
			color = ArrayHelper.Add(c, color);
		}
	}
	
	public override void RemoveData(int index)
	{
		name = ArrayHelper.Remove(index, name);
		color = ArrayHelper.Remove(index, color);
	}
	
	public override void Copy(int index)
	{
		this.AddColor(name[index], color[index]);
	}
	
	public bool IsDefaultTextColor(int i)
	{
		return (i == 0);
	}
	
	public bool IsDefaultShadowColor(int i)
	{
		return (i == 1);
	}
}