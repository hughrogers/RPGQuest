
using UnityEngine;
using System.Collections;

public class StatusBar
{
	public Rect bounds = new Rect(0, 0, 200, 5);
	public int statusID = 0;
	public ScaleMode scaleMode = ScaleMode.StretchToFill;
	public bool alphaBlend = true;
	public float imageAspect = 0;
	
	public bool showEmpty = false;
	public bool useImage = false;
	
	public int barColor = 0;
	public int emptyColor = 0;
	
	public string imageName = "";
	public Texture2D texture;
	public string emptyImageName = "";
	public Texture2D emptyTexture;
	
	// ingame
	private Character[] currentCharacter;
	
	public StatusBar()
	{
		
	}
	
	// data handling
	public Hashtable GetData(Hashtable ht)
	{
		ht.Add("x", this.bounds.x.ToString());
		ht.Add("y", this.bounds.y.ToString());
		ht.Add("w", this.bounds.width.ToString());
		ht.Add("h", this.bounds.height.ToString());
		ht.Add("status", this.statusID.ToString());
		ht.Add("empty", this.showEmpty.ToString());
		if(this.useImage)
		{
			ht.Add("scale", this.scaleMode.ToString());
			ht.Add("blend", this.alphaBlend.ToString());
			ht.Add("aspect", this.imageAspect.ToString());
			ArrayList s = new ArrayList();
			if("" != this.imageName)
			{
				Hashtable img = new Hashtable();
				img.Add(XMLHandler.NODE_NAME, "image");
				img.Add(XMLHandler.CONTENT, this.imageName);
				s.Add(img);
			}
			if("" != this.emptyImageName)
			{
				Hashtable img = new Hashtable();
				img.Add(XMLHandler.NODE_NAME, "emptyimage");
				img.Add(XMLHandler.CONTENT, this.emptyImageName);
				s.Add(img);
			}
			if(s.Count > 0) ht.Add(XMLHandler.NODES, s);
		}
		else
		{
			ht.Add("c1", this.barColor.ToString());
			ht.Add("c2", this.emptyColor.ToString());
		}
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		this.bounds = new Rect(float.Parse((string)ht["x"]), float.Parse((string)ht["y"]),
				float.Parse((string)ht["w"]), float.Parse((string)ht["h"]));
		this.statusID = int.Parse((string)ht["status"]);
		this.showEmpty = bool.Parse((string)ht["empty"]);
		if(ht.ContainsKey("scale"))
		{
			this.scaleMode = (ScaleMode)System.Enum.Parse(typeof(ScaleMode), (string)ht["scale"]);
			this.alphaBlend = bool.Parse((string)ht["blend"]);
			this.imageAspect = float.Parse((string)ht["aspect"]);
			
			if(ht.ContainsKey(XMLHandler.NODES))
			{
				ArrayList s = (ArrayList)ht[XMLHandler.NODES];
				foreach(Hashtable img in s)
				{
					if(img[XMLHandler.NODE_NAME] as string == "image")
					{
						this.imageName = (string)img[XMLHandler.CONTENT];
					}
					else if(img[XMLHandler.NODE_NAME] as string == "emptyimage")
					{
						this.emptyImageName = (string)img[XMLHandler.CONTENT];
					}
				}
			}
		}
		else
		{
			this.barColor = int.Parse((string)ht["c1"]);
			this.emptyColor = int.Parse((string)ht["c2"]);
		}
	}
	
	// ingame
	public void SetView(Character[] c)
	{
		this.currentCharacter = c;
	}
	
	public Texture2D GetImage()
	{
		if(this.texture == null && this.useImage)
		{
			this.texture = (Texture2D)Resources.Load(DataHolder.HUDs().resourcePath+this.imageName, typeof(Texture2D));
		}
		else if(this.texture == null)
		{
			this.texture = new Texture2D(1, 1);
			this.texture.SetPixel(0, 0, DataHolder.Color(this.barColor));
			this.texture.Apply();
		}
		return this.texture;
	}
	
	public Texture2D GetEmptyImage()
	{
		if(this.emptyTexture == null && this.useImage)
		{
			this.emptyTexture = (Texture2D)Resources.Load(DataHolder.HUDs().resourcePath+this.emptyImageName, typeof(Texture2D));
		}
		else if(this.emptyTexture == null)
		{
			this.emptyTexture = new Texture2D(1, 1);
			this.emptyTexture.SetPixel(0, 0, DataHolder.Color(this.emptyColor));
			this.emptyTexture.Apply();
		}
		return this.emptyTexture;
	}
	
	public void ShowBar(int index, Vector2 pos)
	{
		if(index < this.currentCharacter.Length && this.currentCharacter[index] != null)
		{
			float v1 = 0;
			float v2 = 0;
			if(this.statusID < 0)
			{
				v1 = this.currentCharacter[index].timeBar;
				v2 = DataHolder.BattleSystem().actionBorder;
			}
			else if(this.currentCharacter[index].status[this.statusID].IsConsumable())
			{
				v1 = this.currentCharacter[index].status[this.statusID].GetValue();
				v2 = this.currentCharacter[index].status[this.currentCharacter[index].status[this.statusID].maxStatus].GetValue();
			}
			else if(this.currentCharacter[index].status[this.statusID].IsNormal())
			{
				v1 = this.currentCharacter[index].status[this.statusID].GetValue();
				v2 = this.currentCharacter[index].status[this.statusID].maxValue;
			}
			else if(this.currentCharacter[index].status[this.statusID].IsExperience())
			{
				int current = this.currentCharacter[index].GetValueAtLevel(this.statusID, this.currentCharacter[index].currentLevel);
				v1 = this.currentCharacter[index].status[this.statusID].GetValue()-current;
				v2 = this.currentCharacter[index].GetValueAtLevel(this.statusID, this.currentCharacter[index].currentLevel+1)-current;
			}
			v2 /= 100;
			v1 /= v2;
			if(this.showEmpty)
			{
				GUI.DrawTexture(new Rect(this.bounds.x+pos.x, this.bounds.y+pos.y, this.bounds.width, this.bounds.height),
						this.GetEmptyImage(), this.scaleMode, this.alphaBlend, this.imageAspect);
			}
			GUI.DrawTexture(new Rect(this.bounds.x+pos.x, this.bounds.y+pos.y, this.bounds.width*v1/100, this.bounds.height),
					this.GetImage(), this.scaleMode, this.alphaBlend, this.imageAspect);
		}
	}
	
	public Texture2D AddBar(Texture2D tex, int index, Vector2 pos)
	{
		if(index < this.currentCharacter.Length && this.currentCharacter[index] != null)
		{
			float v1 = 0;
			float v2 = 0;
			if(this.statusID < 0)
			{
				v1 = this.currentCharacter[index].timeBar;
				v2 = DataHolder.BattleSystem().actionBorder;
			}
			else if(this.currentCharacter[index].status[this.statusID].IsConsumable())
			{
				v1 = this.currentCharacter[index].status[this.statusID].GetValue();
				v2 = this.currentCharacter[index].status[this.currentCharacter[index].status[this.statusID].maxStatus].GetValue();
			}
			else if(this.currentCharacter[index].status[this.statusID].IsNormal())
			{
				v1 = this.currentCharacter[index].status[this.statusID].GetValue();
				v2 = this.currentCharacter[index].status[this.statusID].maxValue;
			}
			else if(this.currentCharacter[index].status[this.statusID].IsExperience())
			{
				int current = this.currentCharacter[index].GetValueAtLevel(this.statusID, this.currentCharacter[index].currentLevel);
				v1 = this.currentCharacter[index].status[this.statusID].GetValue()-current;
				v2 = this.currentCharacter[index].GetValueAtLevel(this.statusID, this.currentCharacter[index].currentLevel+1)-current;
			}
			v2 /= 100;
			v1 /= v2;
			
			this.GetImage();
			if(this.showEmpty) this.GetEmptyImage();
			Rect b = new Rect(this.bounds.x, this.bounds.y, this.bounds.width, this.bounds.height);
			if(this.emptyTexture != null)
			{
				Vector2 scaledSize = TextureDrawer.GetScaledSize(this.texture, b, this.scaleMode);
				tex = TextureDrawer.AddTexture(tex, 
						new Rect(b.x+pos.x, tex.height-b.y-scaledSize.y, scaledSize.x, scaledSize.y),
						TextureDrawer.GetScaledPixels(this.emptyTexture, scaledSize, b, this.scaleMode));
			}
			if(this.texture != null)
			{
				b.width *= v1/100;
				Vector2 scaledSize = TextureDrawer.GetScaledSize(this.texture, b, this.scaleMode);
				tex = TextureDrawer.AddTexture(tex, 
						new Rect(b.x+pos.x, tex.height-b.y-scaledSize.y, scaledSize.x, scaledSize.y),
						TextureDrawer.GetScaledPixels(this.texture, scaledSize, b, this.scaleMode));
			}
		}
		return tex;
	}
}
