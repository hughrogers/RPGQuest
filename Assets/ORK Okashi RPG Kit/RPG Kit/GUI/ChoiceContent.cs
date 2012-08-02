
using UnityEngine;

public class ChoiceContent
{
	public GUIContent content;
	public bool active = true;
	public string info = "";
	public GUIContent title;
	public bool isButton = true;
	
	public MultiContent titleLabel;
	public MultiContent leftLabel;
	public MultiContent rightLabel;
	
	public Vector2 titleSize = Vector2.zero;
	public Vector2 leftSize = Vector2.zero;
	public Vector2 rightSize = Vector2.zero;
	
	public bool alignRightSide = false;
	public bool selectFirst = false;
	
	// drag stuff
	public bool dragable = false;
	public bool doubleClick = false;
	public DragType dragType = DragType.NONE;
	public DragOrigin dragOrigin = DragOrigin.NONE;
	public int dragID = 0;
	public int dragLevel = 0;
	public int dragPrice = 0;
	public GUISkin dragSkin;
	public GUIStyle dragStyle;
	public GUIStyle dragShadowStyle;
	public float dragPadding = 0;
	public Vector2 dragShadowOffset;
	
	// drop stuff
	public bool droptarget = false;
	public DropType dropType = DropType.NONE;
	public DragOrigin dragDestination = DragOrigin.NONE;
	public int dropID = 0;
	public int characterID = -1;
	
	// textures
	private Rect textureBounds;
	private Rect buttonBounds;
	private Color[] normalTexture;
	private Color[] selectTexture;
	public Texture2D dragTexture;
	public Vector2 halfDrag;
	
	public ChoiceContent(string text) : this(new GUIContent(text))
	{
		
	}
	
	public ChoiceContent(GUIContent c) : this(c, true)
	{
		
	}
	
	public ChoiceContent(GUIContent c, bool a) : this(c, a, "")
	{
		
	}
	
	public ChoiceContent(GUIContent c, bool a, string i) : this(c, a, i, null)
	{
		
	}
	
	public ChoiceContent(GUIContent c, bool a, string i, GUIContent t)
	{
		this.content = c;
		this.active = a;
		this.info = i;
		this.title = t;
		if(this.info != null) this.info = MultiLabel.ReplaceSpecials(this.info);
		if(this.content != null && this.content.text != null) this.content.text = MultiLabel.ReplaceSpecials(this.content.text);
		if(this.title != null && this.title.text != null) this.title.text = MultiLabel.ReplaceSpecials(this.title.text);
	}
	
	/*
	============================================================================
	Comparison functions
	============================================================================
	*/
	public bool Equals(ChoiceContent cc)
	{
		bool equals = false;
		if(this.info == cc.info && this.active == cc.active && this.isButton == cc.isButton &&
			this.CompareGUIContent(this.content, cc.content) &&
			this.CompareGUIContent(this.title, cc.title))
		{
			equals = true;
		}
		return equals;
	}
	
	public bool EqualsWithoutActive(ChoiceContent cc)
	{
		bool equals = false;
		if(this.info == cc.info && this.isButton == cc.isButton &&
			this.CompareGUIContent(this.content, cc.content) &&
			this.CompareGUIContent(this.title, cc.title))
		{
			equals = true;
		}
		return equals;
	}
	
	public bool CompareGUIContent(GUIContent gc1, GUIContent gc2)
	{
		bool equals = false;
		if((gc1 == null && gc2 == null) || 
			(gc1 != null && gc2 != null && gc1.text == gc2.text &&
				gc1.image == gc2.image))
		{
			equals = true;
		}
		return equals;
	}
	
	/*
	============================================================================
	Unity GUI functions
	============================================================================
	*/
	public void InitContent(GUIStyle textStyle, GUIStyle shadowStyle, TextPosition textPosition, bool scrollable, bool sf)
	{
		this.selectFirst = sf;
		if(this.title != null && "" != this.title.text)
		{
			this.titleLabel = new MultiContent(this.title.text, textStyle, shadowStyle, textPosition, scrollable);
			this.titleSize = this.CalcSize(this.titleLabel, this.title.image, textStyle);
		}
		if(this.content != null && "" != this.content.text)
		{
			this.leftLabel = new MultiContent(this.content.text, textStyle, shadowStyle, textPosition, scrollable);
			this.leftSize = this.CalcSize(this.leftLabel, this.content.image, textStyle);
		}
		else
		{
			this.leftLabel = new MultiContent(" ", textStyle, shadowStyle, textPosition, scrollable);
			this.leftSize = this.CalcSize(this.leftLabel, this.content.image, textStyle);
		}
		if("" != this.info)
		{
			this.rightLabel = new MultiContent(this.info, textStyle, shadowStyle, textPosition, scrollable);
			this.rightSize = this.CalcSize(this.rightLabel, null, textStyle);
			this.alignRightSide = this.info.IndexOf("#px", 0) != -1;
		}
	}
	
	private Vector2 CalcSize(MultiContent mc, Texture img, GUIStyle textStyle)
	{
		Vector2 size = Vector2.zero;
		if(mc != null)
		{
			float currentY = 0;
			float currentWidth = 0;
			float currentHeight = 0;
			for(int i=0; i<mc.label.Length; i++)
			{
				if(mc.label[i].bounds.y == currentY)
				{
					currentWidth = mc.label[i].bounds.x+mc.label[i].bounds.width;
					if(mc.label[i].bounds.height > currentHeight)
						currentHeight = mc.label[i].bounds.height;
				}
				else
				{
					currentY = mc.label[i].bounds.y;
					size.y += currentHeight;
					if(currentWidth > size.x)
						size.x = currentWidth;
					currentWidth = mc.label[i].bounds.x+mc.label[i].bounds.width;
				}
			}
			if(currentWidth > size.x) size.x = currentWidth;
			size.y = currentY+currentHeight;
		}
		if(img != null)
		{
			Vector2 v = textStyle.CalcSize(new GUIContent(img));
			size.x += v.x;
			if(v.y > size.y) size.y = v.y;
			size.x += textStyle.CalcSize(new GUIContent(".")).x;
		}
		return size;
	}
	
	/*
	============================================================================
	Drag and drop functions
	============================================================================
	*/
	public void SetDrop(DropType t, DragOrigin d, int id, int charID)
	{
		this.dropType = t;
		this.dragDestination = d;
		this.dropID = id;
		this.characterID = charID;
		this.droptarget = true;
	}
	
	public void SetDrag(DragType t, DragOrigin o, int id, int ul)
	{
		this.dragType = t;
		this.dragOrigin = o;
		this.dragID = id;
		this.dragLevel = ul;
		this.dragable = true;
	}
	
	public void SetDragGUI(GUISkin s, GUIStyle txt, GUIStyle sh, float p, Vector2 so)
	{
		this.dragSkin = s;
		this.dragStyle = txt;
		this.dragShadowStyle = sh;
		this.dragPadding = p;
		this.dragShadowOffset = so;
	}
	
	public void DrawDrag(float xPos, float yPos)
	{
		if(this.dragSkin != null) GUI.skin = this.dragSkin;
		
		Vector2 v = this.leftSize;
		float cHeight = this.rightSize.y;
		if(cHeight > v.y) v.y = cHeight;
		float colWidth = this.leftSize.x+this.rightSize.x+4*this.dragPadding;
		Rect cbounds = new Rect(xPos, yPos, colWidth, v.y);
		
		if(this.isButton)
		{
			GUI.Button(cbounds, "");
		}
		
		// left
		float imageWidth = 0;
		float textAdd = 0;
		if(this.content.image)
		{
			this.dragStyle.alignment = TextAnchor.MiddleLeft;
			GUI.Label(new Rect(cbounds.x+this.dragPadding, cbounds.y, v.x, v.y), 
					new GUIContent(this.content.image), this.dragStyle);
			imageWidth = this.content.image.width;
			textAdd = this.dragStyle.CalcSize(new GUIContent(".")).x;
			this.dragStyle.alignment = TextAnchor.UpperLeft;
		}
		
		if(this.leftLabel != null)
		{
			for(int j=0; j<this.leftLabel.label.Length; j++)
			{
				Rect b = new Rect(cbounds.x+this.leftLabel.label[j].bounds.x + imageWidth+this.dragPadding+textAdd, 
									cbounds.y+this.leftLabel.label[j].bounds.y, cbounds.width, cbounds.height);
				if(this.dragShadowStyle != null)
				{
					this.dragShadowStyle.normal.textColor = this.leftLabel.label[j].shadowColor;
					GUI.Label(
						new Rect(b.x + this.dragShadowOffset.x, 
									b.y + this.dragShadowOffset.y, 
									b.width,
									b.height),
							this.leftLabel.label[j].content, this.dragShadowStyle); 
				}
				this.dragStyle.normal.textColor = this.leftLabel.label[j].textColor;
				GUI.Label(b, this.leftLabel.label[j].content, this.dragStyle);
			}
		}
		
		// right
		if(this.rightLabel != null)
		{
			float xAdd = 0;
			if(this.alignRightSide) xAdd = cbounds.x+cbounds.width-this.dragPadding;
			else xAdd = cbounds.x+cbounds.width-this.rightSize.x-this.dragPadding;
			
			for(int j=0; j<this.rightLabel.label.Length; j++)
			{
				Rect b = new Rect(xAdd+this.rightLabel.label[j].bounds.x, 
									cbounds.y+this.rightLabel.label[j].bounds.y, 
									this.rightLabel.label[j].bounds.width,
									this.rightLabel.label[j].bounds.height);
				if(this.dragShadowStyle != null)
				{
					this.dragShadowStyle.normal.textColor = this.rightLabel.label[j].shadowColor;
					GUI.Label(
						new Rect(b.x + this.dragShadowOffset.x, 
									b.y + this.dragShadowOffset.y, 
									b.width,
									b.height),
							this.rightLabel.label[j].content, this.dragShadowStyle); 
				}
				this.dragStyle.normal.textColor = this.rightLabel.label[j].textColor;
				GUI.Label(b, this.rightLabel.label[j].content, this.dragStyle);
			}
		}
	}
	
	/*
	============================================================================
	ORK GUI system functions
	============================================================================
	*/
	public void InitContent(DialoguePosition dp)
	{
		this.selectFirst = dp.selectFirst;
		if(this.title != null && "" != this.title.text)
		{
			this.titleLabel = new MultiContent(this.title.text, dp);
			this.titleSize = this.CalcSize(this.titleLabel, this.title.image);
		}
		if(this.content != null && "" != this.content.text)
		{
			this.leftLabel = new MultiContent(this.content.text, dp);
			this.leftSize = this.CalcSize(this.leftLabel, this.content.image);
		}
		else
		{
			this.leftLabel = new MultiContent(" ", dp);
			this.leftSize = this.CalcSize(this.leftLabel, this.content.image);
		}
		if("" != this.info)
		{
			this.rightLabel = new MultiContent(this.info, dp);
			this.rightSize = this.CalcSize(this.rightLabel, null);
			this.alignRightSide = this.info.IndexOf("#px", 0) != -1;
		}
	}
	
	private Vector2 CalcSize(MultiContent mc, Texture img)
	{
		Vector2 size = Vector2.zero;
		if(mc != null)
		{
			float currentY = 0;
			float currentWidth = 0;
			float currentHeight = 0;
			for(int i=0; i<mc.label.Length; i++)
			{
				if(mc.label[i].bounds.y == currentY)
				{
					currentWidth = mc.label[i].bounds.x+mc.label[i].bounds.width;
					if(mc.label[i].bounds.height > currentHeight)
						currentHeight = mc.label[i].bounds.height;
				}
				else
				{
					currentY = mc.label[i].bounds.y;
					size.y += currentHeight;
					if(currentWidth > size.x)
						size.x = currentWidth;
					currentWidth = mc.label[i].bounds.x+mc.label[i].bounds.width;
				}
			}
			if(currentWidth > size.x) size.x = currentWidth;
			size.y = currentY+currentHeight;
		}
		if(img != null)
		{
			size.x += img.width;
			if(img.height > size.y) size.y = img.height;
			size.x += mc.font.GetTextSize(" ").x;
		}
		return size;
	}
	
	/*
	============================================================================
	Texture functions
	============================================================================
	*/
	public bool ActiveChange(DialoguePosition dp, bool newActive)
	{
		bool change = false;
		if(this.active != newActive)
		{
			this.active = newActive;
			if(this.active)
			{
				this.normalTexture = TextureDrawer.DivideAlpha(this.normalTexture, dp.choiceInactiveAlpha);
				if(this.selectTexture != null && this.selectTexture.Length > 0)
				{
					this.selectTexture = TextureDrawer.DivideAlpha(this.selectTexture, dp.choiceInactiveAlpha);
				}
			}
			else 
			{
				this.normalTexture = TextureDrawer.MultiplyAlpha(this.normalTexture, dp.choiceInactiveAlpha);
				if(this.selectTexture != null && this.selectTexture.Length > 0)
				{
					this.selectTexture = TextureDrawer.MultiplyAlpha(this.selectTexture, dp.choiceInactiveAlpha);
				}
			}
			change = true;
		}
		return change;
	}
	
	public bool CheckClicked(Vector2 position, float scrollOffsetY, Vector2 offset)
	{
		return (offset.x + this.textureBounds.x + this.buttonBounds.x) < position.x && 
				(offset.y + this.textureBounds.y + this.buttonBounds.y + scrollOffsetY) < position.y &&
				(offset.x + this.textureBounds.x + this.buttonBounds.x + this.buttonBounds.width) > position.x &&
				(offset.y + this.textureBounds.y + this.buttonBounds.y + this.buttonBounds.height + scrollOffsetY) > position.y;
	}
	
	public Vector2 GetTopPoint()
	{
		return new Vector2(this.textureBounds.x+this.textureBounds.width/2, this.textureBounds.y);
	}
	
	public Vector2 GetBottomPoint()
	{
		return new Vector2(this.textureBounds.x+this.textureBounds.width/2, this.textureBounds.y+this.textureBounds.height);
	}
	
	public Texture2D SetTexture(Texture2D texture, DialoguePosition dp, bool selected)
	{
		return this.SetTexture(texture, dp, dp.boxPadding, dp.contentP2Offset, selected);
	}
	
	public Texture2D SetTexture(Texture2D texture, DialoguePosition dp, 
			Vector4 padding, Vector2 p2Offset, bool selected)
	{
		if(selected && this.selectTexture != null)
		{
			texture = TextureDrawer.SetPixels(texture, this.selectTexture, 
					(int)(p2Offset.x+padding.x+this.textureBounds.x), 
					(int)(texture.height-this.textureBounds.y-this.textureBounds.height-p2Offset.y),
					(int)this.textureBounds.width, (int)this.textureBounds.height);
		}
		else
		{
			texture = TextureDrawer.SetPixels(texture, this.normalTexture,
					(int)(p2Offset.x+padding.x+this.textureBounds.x), 
					(int)(texture.height-this.textureBounds.y-this.textureBounds.height-p2Offset.y),
					(int)this.textureBounds.width, (int)this.textureBounds.height);
		}
		return texture;
	}
	
	public bool HasTextures()
	{
		return this.normalTexture != null && this.normalTexture.Length > 0;
	}
	
	public void SetTexturePosition(float x, float y)
	{
		this.textureBounds.x = x;
		this.textureBounds.y = y;
	}
	
	public Vector3 CreateTextures(DialoguePosition dp, int i, int maxCol, ColumnFill colFill, float storedYPos, 
			Vector3 pos, float colWidth, float choiceStartX, float maxRightWidth, bool last, bool center, StatusBar[] bar)
	{
		this.selectFirst = dp.selectFirst;
		float cOffsetX = 0;
		if(dp.choiceDefineWidth)
		{
			cOffsetX = dp.choiceOffsetX*i;
			colWidth = dp.choiceWidth;
		}
		
		float tHeight = this.titleSize.y;
		if(this.leftSize.y > tHeight) tHeight = this.leftSize.y;
		if(this.rightSize.y > tHeight) tHeight = this.rightSize.y;
		Texture2D tex = TextureDrawer.GetCleanTexture((int)(cOffsetX+colWidth+choiceStartX), (int)tHeight);
		Texture2D texSel = null;
		
		if(dp.selectSkin && this.isButton)
		{
			texSel = TextureDrawer.GetCleanTexture(tex.width, tex.height);
		}
		
		this.buttonBounds = new Rect(choiceStartX+cOffsetX, 0, tex.width-choiceStartX-cOffsetX, tex.height);
		if(this.isButton)
		{
			tex = TextureDrawer.SetImageTexture(tex, this.buttonBounds, 
					GameHandler.GetSkin(dp.skin).button);
			if(texSel != null) texSel = TextureDrawer.SetImageTexture(texSel, this.buttonBounds, 
						GameHandler.GetSkin(dp.selectSkin).button);
		}
		
		if(pos.z >= maxCol)
		{
			pos.z = 0;
			if(ColumnFill.VERTICAL.Equals(colFill))
			{
				pos.y = storedYPos;
				pos.x += colWidth + dp.columnSpacing + choiceStartX;
			}
			else
			{
				pos.x = choiceStartX;
				pos.y += dp.columnSpacing;
			}
		}
		else if(i > 0)
		{
			if(ColumnFill.VERTICAL.Equals(colFill))
			{
				pos.y +=  dp.columnSpacing;
			}
			else
			{
				pos.x += colWidth + dp.columnSpacing + choiceStartX;
			}
		}
		this.textureBounds = new Rect(pos.x-choiceStartX, pos.y, tex.width, tex.height);
		
		float imageWidth = 0;
		float textAdd = 0;
		GUIFont font = DataHolder.Fonts().GetFont(dp.skin.font);
		GUIFont fontSel = null;
		if(texSel != null && dp.selectSkin.font != null) fontSel = DataHolder.Fonts().GetFont(dp.selectSkin.font);
		
		// title
		if(choiceStartX > 0 && this.title != null && "" != this.title.text)
		{
			if(this.title.image)
			{
				Texture2D tex2 = this.title.image as Texture2D;
				Rect b = new Rect(cOffsetX+dp.choicePadding.x, (tex.height-tex2.height)/2, tex2.width, tex2.height);
				b.y = tex.height-b.y-b.height;
				tex = TextureDrawer.AddTexture(tex, b, tex2.GetPixels());
				if(texSel != null) texSel = TextureDrawer.AddTexture(texSel, b, tex2.GetPixels());
				
				imageWidth = this.title.image.width+dp.choicePadding.x;
				textAdd = font.GetTextSize(" ").x;
			}
			for(int j=0; j<this.titleLabel.label.Length; j++)
			{
				this.titleLabel.label[j].bounds.x += cOffsetX+imageWidth+textAdd;
				tex = this.titleLabel.label[j].AddTextureNoOffset(font, tex, dp);
				if(texSel != null) texSel = this.titleLabel.label[j].AddTextureNoOffset(fontSel, texSel, dp);
			}
		}
		
		if(bar != null)
		{
			for(int j=0; j<bar.Length; j++)
			{
				tex = bar[j].AddBar(tex, i, new Vector2(this.buttonBounds.x, this.buttonBounds.y));
				if(texSel != null) texSel = bar[j].AddBar(texSel, i, new Vector2(this.buttonBounds.x, this.buttonBounds.y));
			}
		}
		
		// left
		float centerOffset = 0;
		if(center)
		{
			centerOffset = ((this.buttonBounds.width-this.leftSize.x)/2)-dp.choicePadding.x;
		}
		imageWidth = 0;
		textAdd = 0;
		if(this.content.image)
		{
			Texture2D tex2 = this.content.image as Texture2D;
			Rect b = new Rect(this.buttonBounds.x+dp.choicePadding.x+centerOffset, 
					(tex.height-tex2.height)/2, tex2.width, tex2.height);
			b.y = tex.height-b.y-b.height;
			tex = TextureDrawer.AddTexture(tex, b, tex2.GetPixels());
			if(texSel != null) texSel = TextureDrawer.AddTexture(texSel, b, tex2.GetPixels());
			
			imageWidth = this.content.image.width+dp.choicePadding.x;
			textAdd = font.GetTextSize(" ").x;
		}
		
		if(this.leftLabel != null)
		{
			for(int j=0; j<this.leftLabel.label.Length; j++)
			{
				this.leftLabel.label[j].bounds.x += this.buttonBounds.x+imageWidth+textAdd+dp.choicePadding.x+centerOffset;
				tex = this.leftLabel.label[j].AddTextureNoOffset(font, tex, dp);
				if(texSel != null) texSel = this.leftLabel.label[j].AddTextureNoOffset(fontSel, texSel, dp);
			}
		}
		
		// right
		if(this.rightLabel != null)
		{
			float xAdd = 0;
			if(this.alignRightSide) xAdd = this.buttonBounds.x+this.buttonBounds.width-maxRightWidth-dp.choicePadding.x;
			else xAdd = this.buttonBounds.x+this.buttonBounds.width-this.rightSize.x-dp.choicePadding.x;
			
			for(int j=0; j<this.rightLabel.label.Length; j++)
			{
				this.rightLabel.label[j].bounds.x += xAdd;
				tex = this.rightLabel.label[j].AddTextureNoOffset(font, tex, dp);
				if(texSel != null) texSel = this.rightLabel.label[j].AddTextureNoOffset(fontSel, texSel, dp);
			}
		}
		
		if(ColumnFill.VERTICAL.Equals(colFill) || last) pos.y += tHeight;
		pos.z += 1;
		
		this.normalTexture = tex.GetPixels();
		if(texSel != null) this.selectTexture = texSel.GetPixels();
		
		if(this.dragable || this.doubleClick)
		{
			float w = this.leftSize.x+this.rightSize.x+dp.choicePadding.x*2+font.GetTextSize("   ").x;
			if(w > this.buttonBounds.width)
			{
				this.dragTexture = TextureDrawer.GetCleanTexture(
						TextureDrawer.GetNextPowerOfTwo(this.buttonBounds.width),
						TextureDrawer.GetNextPowerOfTwo(this.buttonBounds.height));
				
				this.dragTexture = TextureDrawer.SetPixels(this.dragTexture, 
						tex.GetPixels((int)this.buttonBounds.x, (int)this.buttonBounds.y,
							(int)this.buttonBounds.width, (int)this.buttonBounds.height),
						(int)((this.dragTexture.width-this.buttonBounds.width)/2),
						(int)((this.dragTexture.height-this.buttonBounds.height)/2), (int)this.buttonBounds.width,
						(int)this.buttonBounds.height);
				
				this.halfDrag = new Vector2(this.buttonBounds.width/2, this.buttonBounds.height/2);
			}
			else
			{
				this.dragTexture = TextureDrawer.GetCleanTexture(
						TextureDrawer.GetNextPowerOfTwo(w),
						TextureDrawer.GetNextPowerOfTwo(this.buttonBounds.height));
				
				Vector2 offset = new Vector2((this.dragTexture.width-w)/2,
						(this.dragTexture.height-this.buttonBounds.height)/2);
				float rw = this.rightSize.x+dp.choicePadding.x;
				float lw = w-rw;
				
				this.dragTexture = TextureDrawer.SetPixels(this.dragTexture, 
						(int)offset.x, (int)offset.y, tex, (int)this.buttonBounds.x, 
						(int)this.buttonBounds.y, (int)lw, (int)this.buttonBounds.height);
				
				this.dragTexture = TextureDrawer.SetPixels(this.dragTexture,
						(int)(offset.x+lw-1), (int)offset.y, tex, 
						(int)(this.buttonBounds.x+this.buttonBounds.width-rw+1), 
						(int)this.buttonBounds.y, (int)rw, (int)this.buttonBounds.height);
				
				this.halfDrag = new Vector2(w/2, this.buttonBounds.height/2);
			}
			this.dragTexture.Apply();
		}
		
		if(!this.active)
		{
			this.normalTexture = TextureDrawer.MultiplyAlpha(this.normalTexture, dp.choiceInactiveAlpha);
			if(texSel != null) this.selectTexture = TextureDrawer.MultiplyAlpha(this.selectTexture, dp.choiceInactiveAlpha);
		}
		return new Vector3(pos.x, pos.y, pos.z);
	}
}
