
using UnityEngine;

public class LabelContent
{
	public GUIContent content;
	public Rect bounds;
	public Color textColor;
	public Color shadowColor;
	
	public LabelContent(GUIContent c, Rect b, Color t, Color s)
	{
		this.content = c;
		this.bounds = b;
		this.textColor = t;
		this.shadowColor = s;
	}
	
	public LabelContent(GUIContent c, float x, float y, Color t, Color s, GUIFont font)
	{
		this.content = c;
		this.textColor = t;
		this.shadowColor = s;
		
		Vector2 size;
		if(this.content.image != null)
		{
			size = new Vector2(this.content.image.width, this.content.image.height);
		}
		else
		{
			size = font.GetTextSize(this.content.text);
		}
		this.bounds = new Rect(x, y, size.x, size.y);
	}
	
	public Texture2D GetTexture(GUIFont font)
	{
		Texture2D texture = null;
		if(this.content.image != null)
		{
			texture = this.content.image as Texture2D;
		}
		else
		{
			texture = font.GetTextTexture(this.content.text, (int)this.bounds.width, (int)this.bounds.height);
		}
		return texture;
	}
	
	public Texture2D AddTexture(GUIFont font, Texture2D texture, DialoguePosition dp)
	{
		if(this.content.image != null)
		{
			Texture2D tex = this.content.image as Texture2D;
			Rect b = new Rect(this.bounds.x, this.bounds.y, this.bounds.width, this.bounds.height);
			b.x += dp.boxPadding.x+dp.contentP2Offset.x;
			b.y = texture.height-b.y-b.height-dp.contentP2Offset.y;
			texture = TextureDrawer.AddTexture(texture, b, tex.GetPixels());
		}
		else texture = font.AddTextTexture(texture, this, dp, dp.contentP2Offset);
		return texture;
	}
	
	public Texture2D AddTextureNoOffset(GUIFont font, Texture2D texture, DialoguePosition dp)
	{
		if(this.content.image != null)
		{
			Texture2D tex = this.content.image as Texture2D;
			Rect b = new Rect(this.bounds.x, this.bounds.y, this.bounds.width, this.bounds.height);
			b.x += dp.boxPadding.x;
			b.y = texture.height-b.y-b.height;
			texture = TextureDrawer.AddTexture(texture, b, tex.GetPixels());
		}
		else texture = font.AddTextTexture(texture, this, 0, Vector2.zero, dp.showShadow, dp.shadowOffset);
		return texture;
	}
}