
using System.Collections;
using UnityEngine;

public class FontData : BaseData
{
	public GUIFont[] font;
	
	// XML data
	private string filename = "fonts";
	
	private static string FONTS = "fonts";
	private static string FONT = "font";
	
	public FontData()
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
				if(entry[XMLHandler.NODE_NAME] as string == FontData.FONTS)
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList subs = entry[XMLHandler.NODES] as ArrayList;
						int count = int.Parse((string)entry["count"]);
						name = new string[count];
						font = new GUIFont[count];
						foreach(Hashtable ht in subs)
						{
							if(ht[XMLHandler.NODE_NAME] as string == FontData.NAME)
							{
								name[int.Parse((string)ht["id"])] = ht[XMLHandler.CONTENT] as string;
							}
							else if(ht[XMLHandler.NODE_NAME] as string == FontData.FONT)
							{
								font[int.Parse((string)ht["id"])] = new GUIFont(ht);
							}
						}
					}
				}
			}
		}
	}
	
	public void ClearData()
	{
		this.name = null;
		this.font = null;
	}
	
	public void SaveData()
	{
		ArrayList data = new ArrayList();
		ArrayList subs = new ArrayList();
		Hashtable ht = new Hashtable();
		
		ht.Add(XMLHandler.NODE_NAME, FontData.FONTS);
		ht.Add("count", font.Length.ToString());
		
		for(int i=0; i<name.Length; i++)
		{
			subs.Add(HashtableHelper.GetContentHashtable(FontData.NAME, name[i], i));
			subs.Add(font[i].GetData(HashtableHelper.GetTitleHashtable(FontData.FONT, i)));
		}
		ht.Add(XMLHandler.NODES, subs);
		
		data.Add(ht);
		XMLHandler.SaveXML(dir, filename, data);
	}
	
	public void AddFont(string n, GUIFont f)
	{
		if(name == null)
		{
			name = new string[] {n};
			font = new GUIFont[] {f};
		}
		else
		{
			name = ArrayHelper.Add(n, name);
			font = ArrayHelper.Add(f, font);
		}
	}
	
	public GUIFont GetFont(Font f)
	{
		GUIFont gf = null;
		if(this.name != null)
		{
			for(int i=0; i<this.name.Length; i++)
			{
				if(f.ToString() == this.name[i])
				{
					gf = this.font[i];
					gf.SetFont(f);
					break;
				}
			}
		}
		return gf;
	}
	
	private bool CheckSkinFont(GUISkin skin)
	{
		return skin != null && skin.font != null;
	}
	
	public void PreloadFonts()
	{
		GUIFont f;
		// dialogue skins
		for(int i=0; i<DataHolder.DialoguePositions().GetDataCount(); i++)
		{
			DataHolder.DialoguePosition(i).LoadSkins();
			// default skin
			if(this.CheckSkinFont(DataHolder.DialoguePosition(i).skin))
			{
				f = this.GetFont(DataHolder.DialoguePosition(i).skin.font);
				if(f != null && !f.preloaded) f.Preload();
			}
			// select skin
			if(this.CheckSkinFont(DataHolder.DialoguePosition(i).selectSkin))
			{
				f = this.GetFont(DataHolder.DialoguePosition(i).selectSkin.font);
				if(f != null && !f.preloaded) f.Preload();
			}
			// ok button skin
			if(this.CheckSkinFont(DataHolder.DialoguePosition(i).okSkin))
			{
				f = this.GetFont(DataHolder.DialoguePosition(i).okSkin.font);
				if(f != null && !f.preloaded) f.Preload();
			}
			// name box skin
			if(this.CheckSkinFont(DataHolder.DialoguePosition(i).nameSkin))
			{
				f = this.GetFont(DataHolder.DialoguePosition(i).nameSkin.font);
				if(f != null && !f.preloaded) f.Preload();
			}
		}
		// battle skins
		DataHolder.BattleSystemData().LoadResources();
		if(this.CheckSkinFont(DataHolder.BattleSystemData().textSkin))
		{
			f = this.GetFont(DataHolder.BattleSystemData().textSkin.font);
			if(f != null && !f.preloaded) f.Preload();
		}
		// hud skins
		for(int i=0; i<DataHolder.HUDs().GetDataCount(); i++)
		{
			GUISkin skin = DataHolder.HUD(i).GetSkin();
			if(this.CheckSkinFont(skin))
			{
				f = this.GetFont(skin.font);
				if(f != null && !f.preloaded) f.Preload();
			}
		}
	}
}