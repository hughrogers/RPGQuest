
using UnityEngine;
using System.Collections;

public class GUIFont
{
	private Font font;
	
	public float kerning = 1;
	public float lineSpacing = 29;
	public int asciiStartOffset = 0;
	public GUILetter[] letter;
	public Hashtable kerningPairs = new Hashtable();
	
	public static string LETTER = "letter";
	public static string KERNING = "kerning";
	
	public bool preloaded = false;
	
	public GUIFont()
	{
		
	}
	
	public GUIFont(Hashtable ht)
	{
		this.SetData(ht);
	}
	
	public void SetFont(Font f)
	{
		this.font = f;
	}
	
	public void Preload()
	{
		this.preloaded = true;
		Texture2D texture = this.font.material.mainTexture as Texture2D;
		for(int i=0; i<this.letter.Length; i++) this.letter[i].InitColors(texture);
	}
	
	/*
	============================================================================
	Text functions
	============================================================================
	*/
	public int GetFittingTextLength(string text, float space)
	{
		int i = 0;
		float size = 0;
		for(i=0; i<text.Length; i++)
		{
			GUILetter gl = this.GetLetter(text[i]);
			if(gl != null)
			{
				if(i == 0 && gl.placing.x < 0) size -= gl.placing.x;
				if(i == text.Length-1) size += gl.placing.x+gl.bounds.width;
				else size += gl.width;
				if(i < text.Length-1) size += this.GetLetterKerning(text[i], text[i+1]);
				if(size > space)
				{
					i--;
					break;
				}
			}
		}
		return i;
	}
	
	public Vector2 GetTextSize(string text)
	{
		Vector2 size = new Vector2(0, Mathf.Ceil(this.lineSpacing));
		for(int i=0; i<text.Length; i++)
		{
			GUILetter gl = this.GetLetter(text[i]);
			if(gl != null)
			{
				if(i == 0 && gl.placing.x < 0) size.x -= gl.placing.x;
				if(i == text.Length-1) size.x += gl.placing.x+gl.bounds.width;
				else size.x += gl.width;
				if(i < text.Length-1) size.x += this.GetLetterKerning(text[i], text[i+1]);
				if(size.y+gl.placing.y-gl.bounds.height < 0) size.y -= size.y+gl.placing.y-gl.bounds.height;
			}
		}
		return size;
	}
	
	public Texture2D GetTextTexture(string text)
	{
		Vector2 size = this.GetTextSize(text);
		return this.GetTextTexture(text, (int)size.x, (int)size.y);
	}
	
	public Texture2D GetTextTexture(string text, int width, int height)
	{
		Texture2D texture = TextureDrawer.GetCleanTexture(width, height);
		texture.wrapMode = TextureWrapMode.Clamp;
		int yOffset = (int)(height - Mathf.Ceil(this.lineSpacing));
		int posX = 0;
		for(int i=0; i<text.Length; i++)
		{
			GUILetter gl = this.GetLetter(text[i]);
			if(gl != null)
			{
				if(i == 0 && gl.placing.x < 0) posX -= (int)gl.placing.x;
				texture = TextureDrawer.AddPixels(texture, gl.GetLetterColors(this.font.material.mainTexture as Texture2D), 
						posX+(int)gl.placing.x, (int)Mathf.Ceil(this.lineSpacing+gl.placing.y-gl.bounds.height)+yOffset, 
						(int)gl.bounds.width, (int)gl.bounds.height);
				posX += (int)gl.width;
				if(i < text.Length-1) posX += (int)this.GetLetterKerning(text[i], text[i+1]);
			}
		}
		return texture;
	}
	
	public Texture2D AddTextTexture(Texture2D texture, LabelContent label, DialoguePosition dp, Vector2 p2Offset)
	{
		return this.AddTextTexture(texture, label, dp.boxPadding.x, p2Offset, dp.showShadow, dp.shadowOffset);
	}
	
	public Texture2D AddTextTexture(Texture2D texture, LabelContent label, 
			float paddingX, Vector2 p2Offset, bool showShadow, Vector2 shadowOffset)
	{
		return this.AddTextTexture(texture, label.content.text, label.bounds, label.textColor, label.shadowColor,
			paddingX, p2Offset, showShadow, shadowOffset, true);
	}
	
	public Texture2D AddTextTexture(Texture2D texture, string text, Rect bounds, Color textColor, Color shadowColor,
			float paddingX, Vector2 p2Offset, bool showShadow, Vector2 shadowOffset, bool calcY)
	{
		int xOffset = (int)(bounds.x+paddingX+p2Offset.x);
		int yOffset = (int)(bounds.height - Mathf.Ceil(this.lineSpacing)-p2Offset.y);
		if(calcY) yOffset += (int)(texture.height-bounds.y-bounds.height);
		else yOffset += (int)bounds.y;
		int posX = 0;
		Texture2D fontTexture = this.font.material.mainTexture as Texture2D;
		for(int i=0; i<text.Length; i++)
		{
			GUILetter gl = this.GetLetter(text[i]);
			if(gl != null)
			{
				if(i == 0 && gl.placing.x < 0) posX -= (int)gl.placing.x;
				if(showShadow)
				{
					texture = TextureDrawer.AddPixels(texture, TextureDrawer.Colorize(gl.GetLetterColors(fontTexture), shadowColor), 
							posX+(int)gl.placing.x+xOffset+(int)shadowOffset.x, 
							(int)Mathf.Ceil(this.lineSpacing+gl.placing.y-gl.bounds.height)+yOffset-(int)shadowOffset.y, 
							(int)gl.bounds.width, (int)gl.bounds.height);
				}
				texture = TextureDrawer.AddPixels(texture, TextureDrawer.Colorize(gl.GetLetterColors(fontTexture), textColor), 
						posX+(int)gl.placing.x+xOffset, (int)Mathf.Ceil(this.lineSpacing+gl.placing.y-gl.bounds.height)+yOffset, 
						(int)gl.bounds.width, (int)gl.bounds.height);
				posX += (int)gl.width;
				if(i < text.Length-1) posX += (int)this.GetLetterKerning(text[i], text[i+1]);
			}
		}
		return texture;
	}
	
	/*
	============================================================================
	Letter functions
	============================================================================
	*/
	public GUILetter GetLetter(char c)
	{
		GUILetter gl = null;
		int code = c-32-this.asciiStartOffset;
		if(code >= 0 && code < this.letter.Length) gl = this.letter[code];
		//else Debug.Log("Character not found: "+c);
		return gl;
	}
	
	/*
	============================================================================
	Kerning functions
	============================================================================
	*/
	public float GetLetterKerning(int first, int second)
	{
		float value = 0;
		string key = first+"/"+second;
		if(this.kerningPairs.ContainsKey(key))
		{
			value = float.Parse((string)this.kerningPairs[key])*this.kerning;
		}
		return value;
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		ArrayList s = new ArrayList();
		ht.Add("aso", this.asciiStartOffset.ToString());
		ht.Add("linespacing", this.lineSpacing.ToString());
		ht.Add("kerning", this.kerning.ToString());
		ht.Add("letters", this.letter.Length.ToString());
		for(int i=0; i<this.letter.Length; i++)
		{
			s.Add(this.letter[i].GetData(HashtableHelper.GetTitleHashtable(GUIFont.LETTER, i)));
		}
		foreach(DictionaryEntry entry in this.kerningPairs)
		{
			Hashtable ht2 = HashtableHelper.GetTitleHashtable(GUIFont.KERNING);
			ht2.Add("key", entry.Key);
			ht2.Add("value", entry.Value);
			s.Add(ht2);
		}
		if(s.Count > 0) ht.Add(XMLHandler.NODES, s);
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		this.asciiStartOffset = int.Parse((string)ht["aso"]);
		this.lineSpacing = float.Parse((string)ht["linespacing"]);
		this.kerning = float.Parse((string)ht["kerning"]);
		this.letter = new GUILetter[int.Parse((string)ht["letters"])];
		if(ht.ContainsKey(XMLHandler.NODES))
		{
			ArrayList s = ht[XMLHandler.NODES] as ArrayList;
			foreach(Hashtable ht2 in s)
			{
				if(ht2[XMLHandler.NODE_NAME] as string == GUIFont.LETTER)
				{
					this.letter[int.Parse((string)ht2["id"])] = new GUILetter(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == GUIFont.KERNING)
				{
					this.kerningPairs.Add(ht2["key"] as string, ht2["value"] as string);
				}
			}
		}
	}
}