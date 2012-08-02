
using System.Collections;
using UnityEngine;

public class DialoguePositionData : BaseData
{
	public DialoguePosition[] position = new DialoguePosition[0];
	
	public Texture2D[] box;
	public Texture2D[] focusBox;
	public int preloadIndex = 0;
	
	// XML data
	private string filename = "dialoguePositions";
	
	private static string DIALOGUEPOSITIONS = "dialoguepositions";
	private static string DIALOGUEPOSITION = "dialogueposition";
	private static string BOXBOUNDS = "boxbounds";
	private static string NAMEBOXBOUNDS = "nameboxbounds";
	private static string BOXPADDING = "boxpadding";
	private static string NAMEBOXPADDING = "nameboxpadding";
	private static string SHADOWOFFSET = "shadowoffset";
	private static string CHOICEPADDING = "choicepadding";
	private static string SKIN = "skin";
	private static string SELECTSKIN = "selectskin";
	private static string OKSKIN = "okskin";
	private static string NAMESKIN = "nameskin";
	private static string DRAGBOUNDS = "dragbounds";

	public DialoguePositionData()
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
				if(entry[XMLHandler.NODE_NAME] as string == DialoguePositionData.DIALOGUEPOSITIONS)
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList subs = entry[XMLHandler.NODES] as ArrayList;
						name = new string[subs.Count];
						position = new DialoguePosition[subs.Count];
						foreach(Hashtable val in subs)
						{
							if(val[XMLHandler.NODE_NAME] as string == DialoguePositionData.DIALOGUEPOSITION)
							{
								int i = int.Parse((string)val["id"]);
								position[i] = new DialoguePosition();
								
								position[i].lineSpacing = int.Parse((string)val["linespacing"]);
								position[i].showBox = bool.Parse((string)val["showbox"]);
								position[i].showNameBox = bool.Parse((string)val["shownamebox"]);
								position[i].showShadow = bool.Parse((string)val["showshadow"]);
								position[i].scrollable = bool.Parse((string)val["scrollable"]);
								if(val.ContainsKey("autocollapse")) position[i].autoCollapse = bool.Parse((string)val["autocollapse"]);
								if(val.ContainsKey("oneline")) position[i].oneline = bool.Parse((string)val["oneline"]);
								if(val.ContainsKey("aligncenter")) position[i].alignCenter = bool.Parse((string)val["aligncenter"]);
								if(val.ContainsKey("hidebutton")) position[i].hideButton = bool.Parse((string)val["hidebutton"]);
								if(val.ContainsKey("choiceinactivealpha")) position[i].choiceInactiveAlpha = float.Parse((string)val["choiceinactivealpha"]);
								if(val.ContainsKey("choicecolumns")) position[i].choiceColumns = int.Parse((string)val["choicecolumns"]);
								if(val.ContainsKey("columnspacing")) position[i].columnSpacing = int.Parse((string)val["columnspacing"]);
								if(val.ContainsKey("columnfill")) position[i].columnFill = (ColumnFill)System.Enum.Parse(typeof(ColumnFill), (string)val["columnfill"]);
								if(val.ContainsKey("selectfirst")) position[i].selectFirst = true;
								
								if(val.ContainsKey("choicewidth"))
								{
									position[i].choiceDefineWidth = true;
									position[i].choiceWidth = float.Parse((string)val["choicewidth"]);
									position[i].choiceOffsetX = float.Parse((string)val["choiceox"]);
								}
								
								// fading
								if(val.ContainsKey("fadeintime"))
								{
									position[i].fadeIn = true;
									position[i].fadeInTime = float.Parse((string)val["fadeintime"]);
									position[i].fadeInInterpolation = (EaseType)System.Enum.Parse(typeof(EaseType), (string)val["fadeininterpolation"]);
								}
								if(val.ContainsKey("fadeouttime"))
								{
									position[i].fadeOut = true;
									position[i].fadeOutTime = float.Parse((string)val["fadeouttime"]);
									position[i].fadeOutInterpolation = (EaseType)System.Enum.Parse(typeof(EaseType), (string)val["fadeoutinterpolation"]);
								}
								// moving
								if(val.ContainsKey("moveintime"))
								{
									position[i].moveIn = true;
									position[i].moveInStart.x = float.Parse((string)val["mix"]);
									position[i].moveInStart.y = float.Parse((string)val["miy"]);
									position[i].moveInTime = float.Parse((string)val["moveintime"]);
									position[i].moveInInterpolation = (EaseType)System.Enum.Parse(typeof(EaseType), (string)val["moveininterpolation"]);
								}
								if(val.ContainsKey("moveouttime"))
								{
									position[i].moveOut = true;
									position[i].moveOutStart.x = float.Parse((string)val["mox"]);
									position[i].moveOutStart.y = float.Parse((string)val["moy"]);
									position[i].moveOutTime = float.Parse((string)val["moveouttime"]);
									position[i].moveOutInterpolation = (EaseType)System.Enum.Parse(typeof(EaseType), (string)val["moveoutinterpolation"]);
								}
								
								ArrayList s = val[XMLHandler.NODES] as ArrayList;
								foreach(Hashtable ht in s)
								{
									if(ht[XMLHandler.NODE_NAME] as string == DialoguePositionData.NAME)
									{
										name[i] = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == DialoguePositionData.SKIN)
									{
										position[i].skinName = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == DialoguePositionData.SELECTSKIN)
									{
										position[i].selectSkinName = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == DialoguePositionData.OKSKIN)
									{
										position[i].okSkinName = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == DialoguePositionData.NAMESKIN)
									{
										position[i].nameSkinName = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == DialoguePositionData.BOXBOUNDS)
									{
										position[i].boxBounds.x = float.Parse((string)ht["x"]);
										position[i].boxBounds.y = float.Parse((string)ht["y"]);
										position[i].boxBounds.width = float.Parse((string)ht["w"]);
										position[i].boxBounds.height = float.Parse((string)ht["h"]);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == DialoguePositionData.NAMEBOXBOUNDS)
									{
										position[i].nameBounds.x = float.Parse((string)ht["x"]);
										position[i].nameBounds.y = float.Parse((string)ht["y"]);
										position[i].nameBounds.width = float.Parse((string)ht["w"]);
										position[i].nameBounds.height = float.Parse((string)ht["h"]);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == DialoguePositionData.BOXPADDING)
									{
										position[i].boxPadding.x = float.Parse((string)ht["x"]);
										position[i].boxPadding.y = float.Parse((string)ht["y"]);
										position[i].boxPadding.z = float.Parse((string)ht["z"]);
										position[i].boxPadding.w = float.Parse((string)ht["w"]);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == DialoguePositionData.NAMEBOXPADDING)
									{
										position[i].namePadding.x = float.Parse((string)ht["x"]);
										position[i].namePadding.y = float.Parse((string)ht["y"]);
										position[i].namePadding.z = float.Parse((string)ht["z"]);
										position[i].namePadding.w = float.Parse((string)ht["w"]);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == DialoguePositionData.SHADOWOFFSET)
									{
										position[i].shadowOffset.x = float.Parse((string)ht["x"]);
										position[i].shadowOffset.y = float.Parse((string)ht["y"]);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == DialoguePositionData.CHOICEPADDING)
									{
										position[i].choicePadding.x = float.Parse((string)ht["x"]);
										position[i].choicePadding.y = float.Parse((string)ht["y"]);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == DialoguePositionData.DRAGBOUNDS)
									{
										position[i].isDragWindow = true;
										position[i].dragBounds.x = float.Parse((string)ht["x"]);
										position[i].dragBounds.y = float.Parse((string)ht["y"]);
										position[i].dragBounds.width = float.Parse((string)ht["w"]);
										position[i].dragBounds.height = float.Parse((string)ht["h"]);
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
		
		sv.Add(XMLHandler.NODE_NAME, DialoguePositionData.DIALOGUEPOSITIONS);
		
		if(name != null)
		{
			for(int i=0; i<name.Length; i++)
			{
				Hashtable ht = new Hashtable();
				ArrayList s = new ArrayList();
				
				ht.Add(XMLHandler.NODE_NAME, DialoguePositionData.DIALOGUEPOSITION);
				ht.Add("id", i.ToString());
				ht.Add("linespacing", position[i].lineSpacing.ToString());
				ht.Add("showbox", position[i].showBox.ToString());
				ht.Add("shownamebox", position[i].showNameBox.ToString());
				ht.Add("showshadow", position[i].showShadow.ToString());
				ht.Add("scrollable", position[i].scrollable.ToString());
				ht.Add("autocollapse", position[i].autoCollapse.ToString());
				ht.Add("oneline", position[i].oneline.ToString());
				ht.Add("aligncenter", position[i].alignCenter.ToString());
				ht.Add("hidebutton", position[i].hideButton.ToString());
				ht.Add("choiceinactivealpha", position[i].choiceInactiveAlpha.ToString());
				ht.Add("choicecolumns", position[i].choiceColumns.ToString());
				ht.Add("columnspacing", position[i].columnSpacing.ToString());
				ht.Add("columnfill", position[i].columnFill.ToString());
				if(position[i].selectFirst) ht.Add("selectfirst", "true");
				
				if(position[i].choiceDefineWidth)
				{
					ht.Add("choicewidth", position[i].choiceWidth.ToString());
					ht.Add("choiceox", position[i].choiceOffsetX.ToString());
				}
				
				if(this.position[i].fadeIn)
				{
					ht.Add("fadeintime", this.position[i].fadeInTime.ToString());
					ht.Add("fadeininterpolation", this.position[i].fadeInInterpolation.ToString());
				}
				if(this.position[i].fadeOut)
				{
					ht.Add("fadeouttime", this.position[i].fadeOutTime.ToString());
					ht.Add("fadeoutinterpolation", this.position[i].fadeOutInterpolation.ToString());
				}
				if(this.position[i].moveIn)
				{
					ht.Add("mix", this.position[i].moveInStart.x.ToString());
					ht.Add("miy", this.position[i].moveInStart.y.ToString());
					ht.Add("moveintime", this.position[i].moveInTime.ToString());
					ht.Add("moveininterpolation", this.position[i].moveInInterpolation.ToString());
				}
				if(this.position[i].moveOut)
				{
					ht.Add("mox", this.position[i].moveOutStart.x.ToString());
					ht.Add("moy", this.position[i].moveOutStart.y.ToString());
					ht.Add("moveouttime", this.position[i].moveOutTime.ToString());
					ht.Add("moveoutinterpolation", this.position[i].moveOutInterpolation.ToString());
				}
				
				if("" != this.position[i].skinName)
				{
					Hashtable ht2 = new Hashtable();
					ht2.Add(XMLHandler.NODE_NAME, DialoguePositionData.SKIN);
					ht2.Add(XMLHandler.CONTENT, this.position[i].skinName);
					s.Add(ht2);
				}
				if("" != this.position[i].selectSkinName)
				{
					Hashtable ht2 = new Hashtable();
					ht2.Add(XMLHandler.NODE_NAME, DialoguePositionData.SELECTSKIN);
					ht2.Add(XMLHandler.CONTENT, this.position[i].selectSkinName);
					s.Add(ht2);
				}
				if("" != this.position[i].okSkinName)
				{
					Hashtable ht2 = new Hashtable();
					ht2.Add(XMLHandler.NODE_NAME, DialoguePositionData.OKSKIN);
					ht2.Add(XMLHandler.CONTENT, this.position[i].okSkinName);
					s.Add(ht2);
				}
				if("" != this.position[i].nameSkinName)
				{
					Hashtable ht2 = new Hashtable();
					ht2.Add(XMLHandler.NODE_NAME, DialoguePositionData.NAMESKIN);
					ht2.Add(XMLHandler.CONTENT, this.position[i].nameSkinName);
					s.Add(ht2);
				}
				
				Hashtable n = new Hashtable();
				n.Add(XMLHandler.NODE_NAME, DialoguePositionData.NAME);
				n.Add(XMLHandler.CONTENT, name[i]);
				s.Add(n);
				
				n = new Hashtable();
				n.Add(XMLHandler.NODE_NAME, DialoguePositionData.BOXBOUNDS);
				n.Add("x", position[i].boxBounds.x.ToString());
				n.Add("y", position[i].boxBounds.y.ToString());
				n.Add("w", position[i].boxBounds.width.ToString());
				n.Add("h", position[i].boxBounds.height.ToString());
				s.Add(n);
				
				n = new Hashtable();
				n.Add(XMLHandler.NODE_NAME, DialoguePositionData.NAMEBOXBOUNDS);
				n.Add("x", position[i].nameBounds.x.ToString());
				n.Add("y", position[i].nameBounds.y.ToString());
				n.Add("w", position[i].nameBounds.width.ToString());
				n.Add("h", position[i].nameBounds.height.ToString());
				s.Add(n);
				
				n = new Hashtable();
				n.Add(XMLHandler.NODE_NAME, DialoguePositionData.BOXPADDING);
				n.Add("x", position[i].boxPadding.x.ToString());
				n.Add("y", position[i].boxPadding.y.ToString());
				n.Add("z", position[i].boxPadding.z.ToString());
				n.Add("w", position[i].boxPadding.w.ToString());
				s.Add(n);
				
				n = new Hashtable();
				n.Add(XMLHandler.NODE_NAME, DialoguePositionData.NAMEBOXPADDING);
				n.Add("x", position[i].namePadding.x.ToString());
				n.Add("y", position[i].namePadding.y.ToString());
				n.Add("z", position[i].namePadding.z.ToString());
				n.Add("w", position[i].namePadding.w.ToString());
				s.Add(n);
				
				n = new Hashtable();
				n.Add(XMLHandler.NODE_NAME, DialoguePositionData.SHADOWOFFSET);
				n.Add("x", position[i].shadowOffset.x.ToString());
				n.Add("y", position[i].shadowOffset.y.ToString());
				s.Add(n);
				
				n = new Hashtable();
				n.Add(XMLHandler.NODE_NAME, DialoguePositionData.CHOICEPADDING);
				n.Add("x", position[i].choicePadding.x.ToString());
				n.Add("y", position[i].choicePadding.y.ToString());
				s.Add(n);
				
				if(position[i].isDragWindow)
				{
					n = new Hashtable();
					n.Add(XMLHandler.NODE_NAME, DialoguePositionData.DRAGBOUNDS);
					n.Add("x", position[i].dragBounds.x.ToString());
					n.Add("y", position[i].dragBounds.y.ToString());
					n.Add("w", position[i].dragBounds.width.ToString());
					n.Add("h", position[i].dragBounds.height.ToString());
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
	
	public void AddDialoguePos(string n)
	{
		if(name == null)
		{
			name = new string[] {n};
			position = new DialoguePosition[] {new DialoguePosition()};
		}
		else
		{
			name = ArrayHelper.Add(n, name);
			position = ArrayHelper.Add(new DialoguePosition(), position);
		}
	}
	
	public override void RemoveData(int index)
	{
		name = ArrayHelper.Remove(index, name);
		if (name.Length == 0) name = null;
		position = ArrayHelper.Remove(index, position);
	}
	
	public DialoguePosition GetCopy(int index)
	{
		if(index < 0 || index >= position.Length) index = 0;
		DialoguePosition p = new DialoguePosition();
		p.realID = index;
		p.boxBounds = new Rect(position[index].boxBounds.x, position[index].boxBounds.y, 
				position[index].boxBounds.width, position[index].boxBounds.height);
		p.boxPadding = new Vector4(position[index].boxPadding.x, position[index].boxPadding.y,
				position[index].boxPadding.z, position[index].boxPadding.w);
		p.lineSpacing = position[index].lineSpacing;
		p.showBox = position[index].showBox;
		p.nameBounds = new Rect(position[index].nameBounds.x, position[index].nameBounds.y, 
				position[index].nameBounds.width, position[index].nameBounds.height);
		p.namePadding = new Vector4(position[index].namePadding.x, position[index].namePadding.y,
				position[index].namePadding.z, position[index].namePadding.w);
		p.showNameBox = position[index].showNameBox;
		p.showShadow = position[index].showShadow;
		p.shadowOffset = new Vector2(position[index].shadowOffset.x, position[index].shadowOffset.y);
		p.scrollable = position[index].scrollable;
		p.autoCollapse = position[index].autoCollapse;
		p.oneline = position[index].oneline;
		p.alignCenter = position[index].alignCenter;
		p.hideButton = position[index].hideButton;
		p.fadeIn = position[index].fadeIn;
		p.fadeInTime = position[index].fadeInTime;
		p.fadeInInterpolation = position[index].fadeInInterpolation;
		p.fadeOut = position[index].fadeOut;
		p.fadeOutTime = position[index].fadeOutTime;
		p.fadeOutInterpolation = position[index].fadeOutInterpolation;
		p.choicePadding = new Vector2(position[index].choicePadding.x, position[index].choicePadding.y);
		p.moveIn = position[index].moveIn;
		p.moveInTime = position[index].moveInTime;
		p.moveInInterpolation = position[index].moveInInterpolation;
		p.moveInStart = new Vector2(position[index].moveInStart.x, position[index].moveInStart.y);
		p.moveOut = position[index].moveOut;
		p.moveOutTime = position[index].moveOutTime;
		p.moveOutInterpolation = position[index].moveOutInterpolation;
		p.moveOutStart = new Vector2(position[index].moveOutStart.x, position[index].moveOutStart.y);
		p.choiceInactiveAlpha = position[index].choiceInactiveAlpha;
		p.choiceDefineWidth = position[index].choiceDefineWidth;
		p.choiceWidth = position[index].choiceWidth;
		p.choiceOffsetX = position[index].choiceOffsetX;
		p.skinName = position[index].skinName;
		p.selectSkinName = position[index].selectSkinName;
		p.okSkinName = position[index].okSkinName;
		p.nameSkinName = position[index].nameSkinName;
		p.choiceColumns = position[index].choiceColumns;
		p.columnSpacing = position[index].columnSpacing;
		p.columnFill = position[index].columnFill;
		p.isDragWindow = position[index].isDragWindow;
		p.selectFirst = position[index].selectFirst;
		p.dragBounds = new Rect(position[index].dragBounds.x, position[index].dragBounds.y, 
				position[index].dragBounds.width, position[index].dragBounds.height);
		return p;
	}
	
	public override void Copy(int index)
	{
		name = ArrayHelper.Add(name[index], name);
		position = ArrayHelper.Add(this.GetCopy(index), position);
	}
	
	public void PreloadBoxes()
	{
		this.box = new Texture2D[this.position.Length];
		this.focusBox = new Texture2D[this.position.Length];
		this.PreloadNext();
	}
	
	public bool HasNextPreload()
	{
		return this.preloadIndex < this.position.Length;
	}
	
	public void PreloadNext()
	{
		if(!this.position[this.preloadIndex].autoCollapse || DataHolder.GameSettings().noAutoCollapse)
		{
			this.box[this.preloadIndex] = TextureDrawer.GetCleanTexture(
					TextureDrawer.GetNextPowerOfTwo(this.position[this.preloadIndex].boxBounds.width), 
					TextureDrawer.GetNextPowerOfTwo(this.position[this.preloadIndex].boxBounds.height));
			
			Rect b = new Rect((this.box[this.preloadIndex].width-this.position[this.preloadIndex].boxBounds.width)/2,
					(this.box[this.preloadIndex].height-this.position[this.preloadIndex].boxBounds.height)/2, 
					this.position[this.preloadIndex].boxBounds.width, this.position[this.preloadIndex].boxBounds.height);
			
			if(this.position[this.preloadIndex].isDragWindow)
			{
				this.position[this.preloadIndex].LoadSkins();
				this.box[this.preloadIndex] = TextureDrawer.SetImageTexture(this.box[this.preloadIndex], b, 
						GameHandler.GetSkin(this.position[this.preloadIndex].skin).window);
				this.focusBox[this.preloadIndex] = TextureDrawer.GetCleanTexture(
						this.box[this.preloadIndex].width, this.box[this.preloadIndex].height);
				this.focusBox[this.preloadIndex] = TextureDrawer.SetImageTexture(
						this.focusBox[this.preloadIndex], b, 
						GameHandler.GetSkin(this.position[this.preloadIndex].skin).windowSelected);
				this.focusBox[this.preloadIndex].Apply();
			}
			else if(this.position[this.preloadIndex].showBox)
			{
				this.position[this.preloadIndex].LoadSkins();
				this.box[this.preloadIndex] = TextureDrawer.SetImageTexture(this.box[this.preloadIndex], b, 
						GameHandler.GetSkin(this.position[this.preloadIndex].skin).box);
			}
			this.box[this.preloadIndex].Apply();
		}
		this.preloadIndex++;
	}
}