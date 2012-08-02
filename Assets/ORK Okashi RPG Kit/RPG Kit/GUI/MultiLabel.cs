
using UnityEngine;

public class MultiLabel
{
	// scroll
	private Vector2 scroll = Vector2.zero;
	private float scrollSize = 0;
	private float newHeight = 0;
	
	// choice
	public int selection = 0;
	public int maxSelection = 0;
	public int maxCol = 0;
	public int colCount = 0;
	public ColumnFill colFill = ColumnFill.VERTICAL;
	
	// autoscroll
	public float selectionOffset = 0;
	public float selectionHeight = 0;
	public float viewHeight = 0;
	
	public bool newContent = true;
	private MultiContent content;
	private ChoiceContent[] choice;
	public Vector2[] choicePositions;
	public Vector2[] choiceSizes;
	
	// window helpers
	private Rect windowRect = new Rect(0, 0, 100, 100);
	protected DialoguePosition dp;
	private string text;
	private ChoiceContent[] choices;
	public SpeakerPortrait speakerPortrait;
	public StatusBar[] bar;
	private bool windowPress = false;
	
	// textures
	protected Texture2D bgTexture;
	protected Texture2D bgFocusTexture;
	protected Texture2D contentTexture;
	protected Texture2D vScrollTexture;
	protected Texture2D nameTexture;
	protected Texture2D okTexture;
	protected bool addScroll = false;
	public bool newTextures = false;
	
	public MultiLabel()
	{
		
	}
	
	/*
	============================================================================
	ORK GUI functions
	============================================================================
	*/
	// only for no autocollapse and no scrollable
	public void ReShow(string text)
	{
		text = MultiLabel.ReplaceSpecials(text);
		this.content = new MultiContent(text, dp);
		this.contentTexture = TextureDrawer.SetPixelColors(this.contentTexture, new Rect(dp.contentP2Offset.x,
				dp.contentP2Offset.y, dp.contentBounds.width, dp.contentBounds.height), Color.clear);
		this.contentTexture = this.content.GetTexture(this.contentTexture, dp);
		this.contentTexture.Apply();
	}
	
	public bool EqualContent(string text, ChoiceContent[] choices)
	{
		bool equals = false;
		if(this.bgTexture != null && this.contentTexture != null &&
			this.text == text && this.EqualChoices(choices))
		{
			equals = true;
		}
		return equals;
	}
	
	public bool EqualChoices(ChoiceContent[] choices)
	{
		bool equals = false;
		if(this.choice == null && choices == null) equals = true;
		else if(this.choice != null && choices != null &&
				this.choice.Length == choices.Length)
		{
			equals = true;
			for(int i=0; i<this.choice.Length; i++)
			{
				if(!this.choice[i].EqualsWithoutActive(choices[i]))
				{
					equals = false;
					break;
				}
			}
		}
		return equals;
	}
	
	public void CreateContent(DialoguePosition dp, string name, string text, ChoiceContent[] choices, SpeakerPortrait speakerPortrait, StatusBar[] bar)
	{
		this.dp = dp;
		// if same content > exit, nothing to do
		if(this.EqualContent(text, choices))
		{
			bool change = false;
			for(int i=0; i<this.choice.Length; i++)
			{
				if(this.choice[i].ActiveChange(dp, choices[i].active))
				{
					change = true;
					this.contentTexture = this.choice[i].SetTexture(this.contentTexture, dp, i==this.selection);
				}
			}
			if(change) this.contentTexture.Apply();
			return;
		}
		
		this.text = text;
		this.speakerPortrait = speakerPortrait;
		this.bar = bar;
		TextPosition textPosition = new TextPosition(dp.boxBounds, dp.boxPadding, dp.lineSpacing);
		textPosition.bounds.width -= (dp.boxPadding.x + dp.boxPadding.z);
		textPosition.bounds.height -= (dp.boxPadding.y + dp.boxPadding.w);
		
		text = MultiLabel.ReplaceSpecials(text);
		this.content = new MultiContent(text, dp);
		if(choices != null)
		{
			this.choice = choices;
			this.choicePositions = new Vector2[choices.Length];
			this.choiceSizes = new Vector2[choices.Length];
			this.maxSelection = choices.Length;
			for(int i=0; i<this.choice.Length; i++)
			{
				this.choice[i].InitContent(dp);
			}
		}
		else
		{
			this.choice = null;
			this.maxSelection = 0;
		}
		
		float contentHeightAdd = 0;
		if(this.choice != null)
		{
			colFill = dp.columnFill;
			if(ColumnFill.VERTICAL.Equals(colFill))
			{
				maxCol = (int)Mathf.Ceil(((float)choice.Length) / ((float)dp.choiceColumns));
				colCount = dp.choiceColumns;
			}
			else
			{
				maxCol = dp.choiceColumns;
				colCount = (int)Mathf.Ceil(((float)choice.Length) / ((float)dp.choiceColumns));
			}
			
			float addHlp = 0;
			if(dp.choiceColumns > 1)
			{
				for(int i=0; i<this.choice.Length; i++)
				{
					if(addHlp < this.choice[i].leftSize.y) addHlp = this.choice[i].leftSize.y;
					if(addHlp < this.choice[i].rightSize.y) addHlp = this.choice[i].rightSize.y;
					if(addHlp < this.choice[i].titleSize.y) addHlp = this.choice[i].titleSize.y;
				}
				if(ColumnFill.VERTICAL.Equals(colFill))
				{
					contentHeightAdd = addHlp*maxCol+dp.columnSpacing*(maxCol-1);
				}
				else
				{
					contentHeightAdd = addHlp*colCount+dp.columnSpacing*(colCount-1);
				}
			}
			else
			{
				for(int i=0; i<this.choice.Length; i++)
				{
					addHlp = this.choice[i].leftSize.y;
					if(addHlp < this.choice[i].rightSize.y) addHlp = this.choice[i].rightSize.y;
					if(addHlp < this.choice[i].titleSize.y) addHlp = this.choice[i].titleSize.y;
					contentHeightAdd += addHlp;
					if(i < this.choice.Length-1) contentHeightAdd += dp.columnSpacing;
				}
			}
		}
		
		if(dp.autoCollapse && !DataHolder.GameSettings().noAutoCollapse)
		{
			dp.boxBounds.height = this.content.yPos + dp.boxPadding.y + dp.boxPadding.w + contentHeightAdd;
			textPosition.bounds.height = this.content.yPos + contentHeightAdd;
		}
		this.addScroll = false;
		if(dp.scrollable && (this.content.yPos + contentHeightAdd) > textPosition.bounds.height)
		{
			textPosition.bounds.height = this.content.yPos + contentHeightAdd;
			textPosition.bounds.width -= dp.skin.verticalScrollbarThumb.fixedWidth;
			this.addScroll = true;
		}
		
		Rect b = this.AddBox(textPosition);
		float p2OffsetY = this.AddContent(textPosition);
		this.AddScroll(b, p2OffsetY, textPosition);
		this.AddName(name);
		this.AddOk();
		this.newTextures = true;
	}
	
	public Rect AddBox(TextPosition textPosition)
	{
		Rect b;
		// bg texture
		if(DataHolder.DialoguePositions().box == null || DataHolder.DialoguePositions().box[dp.realID] == null)
		{
			if(this.bgTexture == null)
			{
				this.bgTexture = TextureDrawer.GetCleanTexture(
						TextureDrawer.GetNextPowerOfTwo((int)dp.boxBounds.width), 
						TextureDrawer.GetNextPowerOfTwo((int)dp.boxBounds.height));
			}
			else
			{
				this.bgTexture.Resize(
						TextureDrawer.GetNextPowerOfTwo((int)dp.boxBounds.width), 
						TextureDrawer.GetNextPowerOfTwo((int)dp.boxBounds.height));
				this.bgTexture = TextureDrawer.SetPixelColors(this.bgTexture,
						new Rect(0, 0, this.bgTexture.width, this.bgTexture.height), Color.clear);
			}
			
			this.bgFocusTexture = null;
			
			b = new Rect((this.bgTexture.width-dp.boxBounds.width)/2,
					(this.bgTexture.height-dp.boxBounds.height)/2, dp.boxBounds.width, dp.boxBounds.height);
			if(dp.isDragWindow)
			{
				this.bgTexture = TextureDrawer.SetImageTexture(this.bgTexture, b, GameHandler.GetSkin(dp.skin).window);
				if(this.bgFocusTexture == null)
				{
					this.bgFocusTexture = TextureDrawer.GetCleanTexture(this.bgTexture.width, this.bgTexture.height);
				}
				else
				{
					this.bgFocusTexture.Resize(this.bgTexture.width, this.bgTexture.height);
					this.bgFocusTexture = TextureDrawer.SetPixelColors(this.bgFocusTexture,
							new Rect(0, 0, this.bgFocusTexture.width, this.bgFocusTexture.height), Color.clear);
				}
				this.bgFocusTexture = TextureDrawer.SetImageTexture(this.bgFocusTexture, b, GameHandler.GetSkin(dp.skin).windowSelected);
			}
			else if(dp.showBox)
			{
				this.bgTexture = TextureDrawer.SetImageTexture(this.bgTexture, b, GameHandler.GetSkin(dp.skin).box);
			}
			if(!this.addScroll || DataHolder.GameSettings().noScrollbar)
			{
				this.bgTexture.Apply();
				if(this.bgFocusTexture != null) this.bgFocusTexture.Apply();
			}
		}
		else if(this.addScroll && !DataHolder.GameSettings().noScrollbar)
		{
			if(this.bgTexture == null)
			{
				this.bgTexture = TextureDrawer.CopyTexture(DataHolder.DialoguePositions().box[dp.realID]);
			}
			else
			{
				this.bgTexture.Resize(DataHolder.DialoguePositions().box[dp.realID].width,
						DataHolder.DialoguePositions().box[dp.realID].height);
				this.bgTexture.SetPixels(DataHolder.DialoguePositions().box[dp.realID].GetPixels());
			}
			if(dp.isDragWindow)
			{
				if(this.bgFocusTexture == null)
				{
					this.bgFocusTexture = TextureDrawer.CopyTexture(DataHolder.DialoguePositions().focusBox[dp.realID]);
				}
				else
				{
					this.bgFocusTexture.Resize(DataHolder.DialoguePositions().focusBox[dp.realID].width,
							DataHolder.DialoguePositions().focusBox[dp.realID].height);
					this.bgFocusTexture.SetPixels(DataHolder.DialoguePositions().focusBox[dp.realID].GetPixels());
				}
			}
			b = new Rect((this.bgTexture.width-dp.boxBounds.width)/2,
					(this.bgTexture.height-dp.boxBounds.height)/2, dp.boxBounds.width, dp.boxBounds.height);
		}
		else
		{
			if(this.bgTexture == null) this.bgTexture = DataHolder.DialoguePositions().box[dp.realID];
			if(dp.isDragWindow && this.bgFocusTexture == null) this.bgFocusTexture = DataHolder.DialoguePositions().focusBox[dp.realID];
			b = new Rect((this.bgTexture.width-dp.boxBounds.width)/2,
					(this.bgTexture.height-dp.boxBounds.height)/2, dp.boxBounds.width, dp.boxBounds.height);
		}
		
		dp.contentBounds = new Rect(dp.boxPadding.x, dp.boxBounds.height-dp.boxPadding.y-
				(dp.boxBounds.height-dp.boxPadding.y-dp.boxPadding.w), 
				textPosition.bounds.width, dp.boxBounds.height-dp.boxPadding.y-dp.boxPadding.w);
		return b;
	}
	
	public float AddContent(TextPosition textPosition)
	{
		// create textures
		if(this.contentTexture == null)
		{
			this.contentTexture = TextureDrawer.GetCleanTexture(
					TextureDrawer.GetNextPowerOfTwo((int)dp.boxBounds.width),
					TextureDrawer.GetNextPowerOfTwo((int)textPosition.bounds.height));
		}
		else
		{
			this.contentTexture.Resize(
					TextureDrawer.GetNextPowerOfTwo((int)dp.boxBounds.width),
					TextureDrawer.GetNextPowerOfTwo((int)textPosition.bounds.height));
			this.contentTexture = TextureDrawer.SetPixelColors(this.contentTexture,
					new Rect(0, 0, this.contentTexture.width, this.contentTexture.height), Color.clear);
		}
		float p2OffsetY = this.contentTexture.height-textPosition.bounds.height;
		dp.contentP2Offset = new Vector2((this.contentTexture.width-dp.boxBounds.width)/2, 
				p2OffsetY);
		if(this.addScroll) dp.contentP2Offset.y = 0;
		this.contentTexture = this.content.GetTexture(this.contentTexture, dp);
		
		if(this.choice != null)
		{
			Vector3 pos = new Vector3(0, this.content.yPos, 0);
			
			// get title text length
			float choiceStartX = 0;
			float maxRightWidth = 0;
			for(int i=0; i<choice.Length; i++)
			{
				if(choice[i].title != null)
				{
					if(choice[i].titleSize.x > choiceStartX) choiceStartX = choice[i].titleSize.x;
				}
				if(choice[i].rightLabel != null && choice[i].alignRightSide)
				{
					if(choice[i].rightSize.x > maxRightWidth) maxRightWidth = choice[i].rightSize.x;
				}
			}
			if(choiceStartX > 0) choiceStartX += dp.columnSpacing;
			
			// choices
			pos.x = choiceStartX;
			if("" == text) pos.y -= textPosition.lineSpacing;
			
			float storedYPos = pos.y;
			int columnCount = dp.choiceColumns;
			if(this.choice.Length == 1) columnCount = 1;
			float colWidth = (textPosition.bounds.width / columnCount) - (dp.columnSpacing * (columnCount - 1)) - choiceStartX;
			
			for(int i=0; i<choice.Length; i++)
			{
				pos = this.choice[i].CreateTextures(dp, i, maxCol, colFill, storedYPos, pos, colWidth, 
						choiceStartX, maxRightWidth, i == choice.Length-1, false, this.bar);
				this.contentTexture = this.choice[i].SetTexture(this.contentTexture, dp, i==this.selection);
			}
		}
		return p2OffsetY;
	}
	
	public void AddScroll(Rect b, float p2OffsetY, TextPosition textPosition)
	{
		if(this.addScroll)
		{
			dp.vScrollBounds = new Rect(dp.boxBounds.width-dp.skin.verticalScrollbar.fixedWidth,
					(this.bgTexture.height-dp.contentBounds.height)/2, dp.skin.verticalScrollbar.fixedWidth, dp.contentBounds.height);
			if(!DataHolder.GameSettings().noScrollbar)
			{
				this.bgTexture = TextureDrawer.AddImageTexture(this.bgTexture, 
						new Rect(b.x+dp.vScrollBounds.x, dp.vScrollBounds.y,
							dp.vScrollBounds.width, dp.vScrollBounds.height),
						GameHandler.GetSkin(dp.skin).verticalScrollbar);
				this.bgTexture.Apply();
				if(this.bgFocusTexture != null)
				{
					this.bgFocusTexture = TextureDrawer.AddImageTexture(this.bgFocusTexture, 
							new Rect(b.x+dp.vScrollBounds.x, dp.vScrollBounds.y,
								dp.vScrollBounds.width, dp.vScrollBounds.height),
							GameHandler.GetSkin(dp.skin).verticalScrollbar);
					this.bgFocusTexture.Apply();
				}
			}
			
			dp.vScrollSize = new Vector2(dp.skin.verticalScrollbarThumb.fixedWidth,
					Mathf.Max(dp.contentBounds.height*dp.contentBounds.height/this.contentTexture.height, 
						dp.skin.verticalScrollbarThumb.normal.background.height));
			
			if(!DataHolder.GameSettings().noScrollbarThumb)
			{
				this.vScrollTexture = TextureDrawer.GetCleanTexture(
							TextureDrawer.GetNextPowerOfTwo((int)dp.vScrollSize.x), 
							TextureDrawer.GetNextPowerOfTwo((int)dp.vScrollSize.y));
				this.vScrollTexture = TextureDrawer.SetImageTexture(this.vScrollTexture, 
						new Rect((this.vScrollTexture.width-dp.skin.verticalScrollbarThumb.fixedWidth)/2,
							(this.vScrollTexture.height-dp.vScrollSize.y)/2, dp.skin.verticalScrollbarThumb.fixedWidth, dp.vScrollSize.y),
						GameHandler.GetSkin(dp.skin).verticalScrollbarThumb);
				this.vScrollTexture.Apply();
			}
			
			dp.scrollBoundTop = (this.contentTexture.height-
					dp.contentBounds.height)/this.contentTexture.height;
			dp.scrollBoundBottom = p2OffsetY/this.contentTexture.height;
			dp.scrollViewHeight = dp.contentBounds.height/this.contentTexture.height;
			dp.scrollSize.y = dp.scrollBoundTop;
			dp.scrollPixelVertical = dp.scrollSize.y/(this.contentTexture.height-
					dp.contentBounds.height);
			dp.vScrollTop = -(dp.contentBounds.height-dp.vScrollSize.y)/2;
			dp.vScrollPixel = (dp.contentBounds.height-dp.vScrollSize.y-1)/
					(dp.scrollBoundTop-dp.scrollBoundBottom);
			dp.vScrollRatio = textPosition.bounds.height/dp.vScrollBounds.height;
		}
		else if(this.vScrollTexture != null)
		{
			GameObject.Destroy(this.vScrollTexture);
			dp.scrollBoundTop = 0;
			dp.scrollBoundBottom = 0;
			dp.scrollViewHeight = 0;
			dp.scrollPixelVertical = 0;
			dp.scrollSize = Vector2.zero;
			dp.vScrollTop = 0;
			dp.vScrollPixel = 0;
			dp.vScrollRatio = 0;
		}
		
		if(this.contentTexture != null)
		{
			this.contentTexture.Apply();
		}
	}
	
	public void AddName(string name)
	{
		if("" != name && this.nameTexture == null)
		{
			GUIFont font = null;
			if(dp.nameSkin != null && dp.nameSkin.font != null) font = DataHolder.Fonts().GetFont(dp.nameSkin.font);
			else if(dp.skin != null && dp.skin.font != null) font = DataHolder.Fonts().GetFont(dp.skin.font);
			if(font != null)
			{
				if(dp.isDragWindow)
				{
					this.nameTexture = font.GetTextTexture(name);
					this.nameTexture.Apply();
				}
				else
				{
					LabelContent nameLabel = new LabelContent(new GUIContent(name), 
							0, 0, DataHolder.Color(0), DataHolder.Color(1), font);
					dp.nameBounds.width = nameLabel.bounds.width+dp.namePadding.x+dp.namePadding.z;
					this.nameTexture = TextureDrawer.GetCleanTexture(
							TextureDrawer.GetNextPowerOfTwo(dp.nameBounds.width),
							TextureDrawer.GetNextPowerOfTwo(dp.nameBounds.height));
					
					nameLabel.bounds.x = dp.namePadding.x+(this.nameTexture.width-dp.nameBounds.width)/2;
					nameLabel.bounds.y = dp.namePadding.y+(this.nameTexture.height-dp.nameBounds.height)/2;
					
					Rect b2 = new Rect((this.nameTexture.width-dp.nameBounds.width)/2,
							(this.nameTexture.height-dp.nameBounds.height)/2, dp.nameBounds.width, dp.nameBounds.height);
					if(dp.showNameBox)
					{
						this.nameTexture = TextureDrawer.SetImageTexture(
								this.nameTexture, b2, GameHandler.GetSkin(dp.skin).box);
					}
					this.nameTexture = font.AddTextTexture(this.nameTexture, nameLabel, 
						0, Vector2.zero, dp.showShadow, dp.shadowOffset);
					this.nameTexture.Apply();
				}
			}
		}
	}
	
	public void AddOk()
	{
		// ok button
		if(this.choice == null && !dp.hideButton && this.okTexture == null)
		{
			GUISkin okSkin = null;
			if(dp.okSkin != null) okSkin = dp.okSkin;
			else if(dp.skin != null) okSkin = dp.skin;
			
			if(okSkin != null && okSkin.font != null)
			{
				GUIFont font = DataHolder.Fonts().GetFont(okSkin.font);
				
				Vector2 vec2 = DataHolder.GameSettings().dialogueOkSize[GameHandler.GetLanguage()];
				this.okTexture = TextureDrawer.GetCleanTexture(
						TextureDrawer.GetNextPowerOfTwo((int)vec2.x), 
						TextureDrawer.GetNextPowerOfTwo((int)vec2.y));
				
				Rect okBounds = new Rect((this.okTexture.width-vec2.x)/2, 
						(this.okTexture.height-vec2.y)/2, vec2.x, vec2.y);
				
				this.okTexture = TextureDrawer.SetImageTexture(
						this.okTexture, okBounds, GameHandler.GetSkin(okSkin).button);
				
				Texture2D icon = DataHolder.GameSettings().dialogueOkIcon;
				Texture2D tex = font.GetTextTexture(DataHolder.GameSettings().dialogueOkText[GameHandler.GetLanguage()]);
				Vector2 iconSize = new Vector2(0, 0);
				if(icon != null)
				{
					iconSize.x += icon.width+font.GetTextSize(" ").x;
					iconSize.y += icon.height;
				}
				Vector2 okPlace = new Vector2((this.okTexture.width-tex.width-iconSize.x)/2, 
						(this.okTexture.height-Mathf.Max(tex.height, iconSize.y))/2);
				
				if(icon != null)
				{
					this.okTexture = TextureDrawer.AddPixels(this.okTexture, icon.GetPixels(),
							(int)okPlace.x, (int)okPlace.y, icon.width, icon.height);
				}
				this.okTexture = TextureDrawer.AddPixels(this.okTexture, tex.GetPixels(),
						(int)(okPlace.x+iconSize.x), (int)okPlace.y, tex.width, tex.height);
				this.okTexture.Apply();
				
				dp.okButtonBounds = new Rect(0, 0, vec2.x, vec2.y);
				ButtonPosition buttonPos = DataHolder.GameSettings().dialogueOkPosition;
				if(ButtonPosition.TOP_LEFT.Equals(buttonPos) || ButtonPosition.BOTTOM_LEFT.Equals(buttonPos))
				{
					dp.okButtonBounds.x = -(dp.boxBounds.width-okBounds.width)/2;
				}
				else if(ButtonPosition.TOP_RIGHT.Equals(buttonPos) || ButtonPosition.BOTTOM_RIGHT.Equals(buttonPos))
				{
					dp.okButtonBounds.x = (dp.boxBounds.width-okBounds.width)/2;
				}
				if(ButtonPosition.BOTTOM_LEFT.Equals(buttonPos) || ButtonPosition.BOTTOM_CENTER.Equals(buttonPos)
						|| ButtonPosition.BOTTOM_RIGHT.Equals(buttonPos))
				{
					dp.okButtonBounds.y = (dp.boxBounds.height-okBounds.height)/2;
				}
				else if(ButtonPosition.TOP_LEFT.Equals(buttonPos) || ButtonPosition.TOP_CENTER.Equals(buttonPos)
						|| ButtonPosition.TOP_RIGHT.Equals(buttonPos))
				{
					dp.okButtonBounds.y = -(dp.boxBounds.height-okBounds.height)/2;
				}
				vec2 = DataHolder.GameSettings().dialogueOkOffset[GameHandler.GetLanguage()];
				dp.okButtonBounds.x += vec2.x;
				dp.okButtonBounds.y += vec2.y;
			}
		}
	}
	
	public void DeleteTextures()
	{
		if(dp != null && DataHolder.DialoguePositions().box[dp.realID] == null ||
			(this.addScroll && !DataHolder.GameSettings().noScrollbar))
		{
			Object.Destroy(this.bgTexture);
			Object.Destroy(this.bgFocusTexture);
		}
		Object.Destroy(this.contentTexture);
		Object.Destroy(this.vScrollTexture);
		Object.Destroy(this.nameTexture);
		Object.Destroy(this.okTexture);
	}
	
	public Texture2D GetBackgroundTexture()
	{
		Texture2D texture = this.bgTexture;
		if(dp.isDragWindow && dp.IsFocused())
		{
			texture = this.bgFocusTexture;
		}
		return texture;
	}
	
	public Texture2D GetContentTexture()
	{
		return this.contentTexture;
	}
	
	public bool HasContentTexture()
	{
		return this.contentTexture != null;
	}
	
	public Texture2D GetVScrollTexture()
	{
		return this.vScrollTexture;
	}
	
	public bool HasVScrollTexture()
	{
		return this.vScrollTexture != null;
	}
	
	public Texture2D GetNameTexture()
	{
		return this.nameTexture;
	}
	
	public bool HasNameTexture()
	{
		return this.nameTexture != null;
	}
	
	public Texture2D GetOkTexture()
	{
		return this.okTexture;
	}
	
	public bool HasOkTexture()
	{
		return this.okTexture != null;
	}
	
	public virtual void Tick(float t)
	{
		if(this.contentTexture != null && this.content != null && this.content.HasNextLabel())
		{
			this.contentTexture = this.content.AddNextLabel(this.contentTexture, this.dp);
			if(!this.content.HasNextLabel()) this.contentTexture.Apply();
		}
	}
	
	/*
	============================================================================
	Interact functions
	============================================================================
	*/
	public void CheckChoice(Vector2 mousePosition)
	{
		if(this.choice != null)
		{
			float scrollOffsetY = 0;
			if(this.addScroll)
			{
				scrollOffsetY = -(dp.scrollBoundTop-dp.scrollSize.y)/dp.scrollPixelVertical;
			}
			int lastSelection = this.selection;
			for(int i=0; i<this.choice.Length; i++)
			{
				if(this.choice[i].CheckClicked(mousePosition, scrollOffsetY, 
					new Vector2(dp.currentPos.x+dp.boxPadding.x, dp.currentPos.y+dp.boxPadding.y)))
				{
					if(this.selection != i)
					{
						if(this.selection >= 0 && this.selection < this.choice.Length)
						{
							this.contentTexture = this.choice[this.selection].SetTexture(this.contentTexture, this.dp, false);
						}
						this.contentTexture = this.choice[i].SetTexture(this.contentTexture, this.dp, true);
						this.selection = i;
						this.contentTexture.Apply();
					}
					if(this.choice[i].isButton && !this.choice[i].doubleClick &&
						(!this.choice[i].selectFirst || lastSelection == this.selection))
					{
						this.dp.SetAcceptPressed(true);
					}
					else if(this.choice[i].selectFirst)
					{
						GameHandler.ChangeHappened(0, 0, 0);
					}
					break;
				}
			}
		}
	}
	
	public ChoiceContent CheckDrag(Vector2 mousePosition)
	{
		ChoiceContent cc = null;
		if(this.choice != null && this.dp.IsInWindow(mousePosition))
		{
			float scrollOffsetY = 0;
			if(this.addScroll)
			{
				scrollOffsetY = -(dp.scrollBoundTop-dp.scrollSize.y)/dp.scrollPixelVertical;
			}
			for(int i=0; i<this.choice.Length; i++)
			{
				if(this.choice[i].active && (this.choice[i].dragable || this.choice[i].doubleClick) &&
					this.choice[i].CheckClicked(mousePosition, scrollOffsetY, 
						new Vector2(dp.currentPos.x+dp.boxPadding.x, 
						dp.currentPos.y+dp.boxPadding.y)))
				{
					cc = this.choice[i];
					break;
				}
			}
		}
		return cc;
	}
	
	public ChoiceContent CheckDrop(Vector2 mousePosition)
	{
		ChoiceContent cc = null;
		if(this.choice != null && this.dp.IsInWindow(mousePosition))
		{
			float scrollOffsetY = 0;
			if(this.addScroll)
			{
				scrollOffsetY = -(dp.scrollBoundTop-dp.scrollSize.y)/dp.scrollPixelVertical;
			}
			for(int i=0; i<this.choice.Length; i++)
			{
				if(this.choice[i].droptarget &&
					this.choice[i].CheckClicked(mousePosition, scrollOffsetY, 
						new Vector2(dp.currentPos.x+dp.boxPadding.x, 
						dp.currentPos.y+dp.boxPadding.y)))
				{
					cc = this.choice[i];
					break;
				}
			}
			// return something cause drop is in window
			if(cc == null) cc = new ChoiceContent("");
		}
		return cc;
	}
	
	/*
	============================================================================
	Choice content array functions
	============================================================================
	*/
	public static ChoiceContent[] CreateChoiceContents(string[] choices)
	{
		ChoiceContent[] ch = null;
		if(choices != null)
		{
			ch = new ChoiceContent[choices.Length];
			for(int i=0; i<ch.Length; i++)
			{
				ch[i] = new ChoiceContent(choices[i]);
			}
		}
		return ch;
	}
	
	public static ChoiceContent[] CreateChoiceContents(GUIContent[] choices)
	{
		ChoiceContent[] ch = null;
		if(choices != null)
		{
			ch = new ChoiceContent[choices.Length];
			for(int i=0; i<ch.Length; i++)
			{
				ch[i] = new ChoiceContent(choices[i]);
			}
		}
		return ch;
	}
	
	public string GetUnshownText()
	{
		string txt = "";
		if(this.content.textPos < this.content.originalText.Length-1)
		{
			txt = this.GetColorString(this.content.currentColor)+this.GetShadowColorString(this.content.shadowColor)+
				this.content.originalText.Substring(this.content.textPos, this.content.originalText.Length-this.content.textPos);
		}
		return txt;
	}
	
	public bool IsChoice()
	{
		return this.choice != null;
	}
	
	/*
	============================================================================
	Unity GUI functions
	============================================================================
	*/
	public ChoiceContent GetDragOnPosition(Vector2 pos)
	{
		ChoiceContent cc = null;
		if(this.choice != null && 
				this.windowRect.x < pos.x && 
				this.windowRect.y < pos.y &&
				(this.windowRect.x + this.windowRect.width) > pos.x &&
				(this.windowRect.y + this.viewHeight) > pos.y)
		{
			for(int i=0; i<this.choice.Length; i++)
			{
				if(this.choice[i].active && (this.choice[i].dragable || this.choice[i].doubleClick) &&
					( this.choicePositions[i].x) < pos.x && 
					(this.choicePositions[i].y - this.scroll.y) < pos.y &&
					(this.choicePositions[i].x + this.choiceSizes[i].x) > pos.x &&
					(this.choicePositions[i].y + this.choiceSizes[i].y - this.scroll.y) > pos.y)
				{
					cc = this.choice[i];
					break;
				}
			}
		}
		return cc;
	}
	
	public ChoiceContent GetDropOnPosition(Vector2 pos)
	{
		ChoiceContent cc = null;
		if(this.choice != null && 
				this.windowRect.x < pos.x && 
				this.windowRect.y < pos.y &&
				(this.windowRect.x + this.windowRect.width) > pos.x &&
				(this.windowRect.y + this.viewHeight) > pos.y)
		{
			for(int i=0; i<this.choice.Length; i++)
			{
				if(this.choice[i].droptarget &&
					(this.choicePositions[i].x) < pos.x && 
					(this.choicePositions[i].y - this.scroll.y) < pos.y &&
					(this.choicePositions[i].x + this.choiceSizes[i].x) > pos.x &&
					(this.choicePositions[i].y + this.choiceSizes[i].y - this.scroll.y) > pos.y)
				{
					cc = this.choice[i];
					break;
				}
			}
			// return something cause drop is in window
			if(cc == null) cc = new ChoiceContent("");
		}
		return cc;
	}
	
	public void ResetScroll()
	{
		this.scroll = Vector2.zero;
		this.scrollSize = 0;
		this.newHeight = 0;
		this.selection = 0;
		this.maxSelection = 0;
		this.newContent = true;
	}
	
	public void SetSelection(int index)
	{
		if(this.contentTexture != null && this.selection != index)
		{
			if(this.selection >= 0 && this.selection < this.choice.Length)
			{
				this.contentTexture = this.choice[this.selection].SetTexture(this.contentTexture, this.dp, false);
			}
			this.selection = index;
			if(this.selection >= 0 && this.selection < this.choice.Length)
			{
				this.contentTexture = this.choice[this.selection].SetTexture(this.contentTexture, this.dp, true);
			}
			// set scroll
			if(this.addScroll)
			{
				float yPos = this.dp.contentBounds.y - this.dp.boxPadding.y + 
						(dp.scrollBoundTop-dp.scrollSize.y)/dp.scrollPixelVertical;
				
				Vector2 tp = this.choice[this.selection].GetTopPoint();
				Vector2 bp = this.choice[this.selection].GetBottomPoint();
				if(tp.y < yPos)
				{
					this.dp.scrollSize.y = dp.scrollBoundTop-tp.y*this.dp.scrollPixelVertical;
				}
				else if(yPos+this.dp.contentBounds.height < bp.y)
				{
					this.dp.scrollSize.y = dp.scrollBoundTop-bp.y*this.dp.scrollPixelVertical+
							this.dp.contentBounds.height*this.dp.scrollPixelVertical;
				}
				this.dp.ScrollBoundCheck();
			}
			
			this.contentTexture.Apply();
		}
		else
		{
			float tmp = index*this.selectionHeight+this.selectionOffset;
			if(tmp < this.scroll.y) this.scroll.y = tmp;
			else if((tmp+this.selectionHeight) > (this.scroll.y+this.viewHeight)) this.scroll.y = (tmp+this.selectionHeight)-this.viewHeight;
			
			if(index == 0) this.scroll.y = 0;
			else if(index == this.maxSelection-1) this.scroll.y = this.scrollSize;
			this.selection = index;
		}
	}
	
	public int GetSelection()
	{
		return this.selection;
	}
	
	public void ChangeSelectionHorizontal(int add)
	{
		if(ColumnFill.VERTICAL.Equals(this.colFill))
		{
			this.ChangeSelection(add*maxCol);
		}
		else
		{
			this.ChangeSelection(add);
		}
	}
	
	public void ChangeSelectionVertical(int add)
	{
		if(ColumnFill.VERTICAL.Equals(this.colFill))
		{
			this.ChangeSelection(add);
		}
		else
		{
			this.ChangeSelection(add*maxCol);
		}
	}
	
	public void ChangeSelection(int add)
	{
		if(maxCol <= 0) return;
		int index = this.selection+add;
		
		if(add == 1 && index % maxCol == 0) index -= maxCol;
		else if(add == -1 && (index+1) % maxCol == 0) index += maxCol;
		else if(add > 1 || add < -1)
		{
			if(index < 0) index += maxCol*colCount;
			else if(index >= this.maxSelection) index -= maxCol*colCount;
		}
		
		if(index < 0) index = 0;
		else if(index >= this.maxSelection) index = this.maxSelection-1;
		
		this.SetSelection(index);
	}
	
	public void ChangeScroll(float add)
	{
		if(this.contentTexture != null) this.dp.SetScroll(-add);
		else this.scroll.y += add;
	}
	
	public static string GetColorString(int color)
	{
		string val = "";
		
		if(color >= 0)
		{
			val = "#c"+color+"#";
		}
		
		return val;
	}
	
	public static string GetShadowColorString(int color)
	{
		string val = "";
		
		if(color >= 0)
		{
			val = "#s"+color+"#";
		}
		
		return val;
	}
	
	public string ShowText(string text, string name)
	{
		return this.ShowText(new DialoguePosition(), text, name);
	}
	
	public string ShowText(DialoguePosition dp, string text, string name)
	{
		return this.ShowText(dp, text, name, null);
	}
	
	public string ShowText(DialoguePosition dp, string text, string name, SpeakerPortrait speakerPortrait)
	{
		if(dp.skin) GUI.skin = dp.skin;
		
		Color c = GUI.backgroundColor;
		c.a = dp.alpha;
		GUI.backgroundColor = c;
		c = GUI.color;
		c.a = dp.alpha;
		GUI.color = c;
		
		if(this.newContent || this.content == null)
		{
			GUIStyle shadowStyle = new GUIStyle(GUI.skin.label);
			shadowStyle.normal.textColor = DataHolder.Color(1);
			GUIStyle textStyle = new GUIStyle(GUI.skin.label);
			textStyle.wordWrap = false;
			TextPosition textPosition = new TextPosition(dp.boxBounds, dp.boxPadding, dp.lineSpacing);
			textPosition.bounds.width -= (dp.boxPadding.x + dp.boxPadding.z);
			textPosition.bounds.height -= (dp.boxPadding.y + dp.boxPadding.w);
			
			text = MultiLabel.ReplaceSpecials(text);
			this.content = new MultiContent(text, textStyle, shadowStyle, textPosition, dp.scrollable);
			this.newContent = false;
		}
		
		if(speakerPortrait != null && !speakerPortrait.inBox)
		{
			speakerPortrait.ShowPortrait();
		}
		
		this.windowRect.x = dp.currentPos.x;
		this.windowRect.y = dp.currentPos.y;
		this.windowRect.width = dp.boxBounds.width;
		this.windowRect.height = dp.boxBounds.height;
		
		int windowID = dp.GetWindowID();
		
		this.dp = dp;
		this.text = text;
		this.speakerPortrait = speakerPortrait;
		
		if(dp.isDragWindow)
		{
			this.windowRect = GUI.Window(windowID, this.windowRect, TextWindow, name);
			if(dp.currentPos.x != this.windowRect.x || dp.currentPos.y != this.windowRect.y)
			{
				if(dp.focusable) GameHandler.WindowHandler().SetFocusID(windowID);
				
				if(DataHolder.GameSettings().saveWindowDrag)
				{
					if(this.windowRect.x < 0) this.windowRect.x = 0;
					else if((this.windowRect.x+this.windowRect.width) > DataHolder.GameSettings().defaultScreen.x)
					{
						this.windowRect.x = DataHolder.GameSettings().defaultScreen.x-this.windowRect.width;
					}
					if(this.windowRect.y < 0) this.windowRect.y = 0;
					else if((this.windowRect.y+this.windowRect.height) > DataHolder.GameSettings().defaultScreen.y)
					{
						this.windowRect.y = DataHolder.GameSettings().defaultScreen.y-this.windowRect.height;
					}
				}
				dp.currentPos.x = this.windowRect.x;
				dp.currentPos.y = this.windowRect.y;
				dp.SetBasePosition(this.windowRect.x, this.windowRect.y);
			}
		}
		else
		{
			if(name != "")
			{
				GUIStyle shadowStyle = new GUIStyle(GUI.skin.label);
				shadowStyle.normal.textColor = DataHolder.Color(1);
				GUIStyle textStyle = new GUIStyle(GUI.skin.label);
				textStyle.wordWrap = false;
				
				if(dp.nameSkin) GUI.skin = dp.nameSkin;
				Vector2 v = textStyle.CalcSize(new GUIContent(name));
				TextPosition namePosition = new TextPosition(dp.nameBounds, dp.namePadding, 0);
				namePosition.bounds.x = dp.currentPos.x+dp.nameOffset.x;
				namePosition.bounds.y = dp.currentPos.y+dp.nameOffset.y;
				namePosition.bounds.width = v.x + dp.namePadding.x + dp.namePadding.z;
				if(dp.showNameBox) GUI.Box(namePosition.bounds, "");
				namePosition.bounds.x += dp.namePadding.x;
				namePosition.bounds.y += dp.namePadding.y;
				namePosition.bounds.width -= (dp.namePadding.x + dp.namePadding.z);
				namePosition.bounds.height -= (dp.namePadding.y + dp.namePadding.w);
				if(dp.showShadow)
				{
					GUI.Label(
						new Rect(namePosition.bounds.x + dp.shadowOffset.x, namePosition.bounds.y + dp.shadowOffset.y, 
								namePosition.bounds.width, namePosition.bounds.height),
						name, shadowStyle); 
				}
				
				GUI.Label(new Rect(namePosition.bounds.x, namePosition.bounds.y, namePosition.bounds.width, namePosition.bounds.height),
						name, textStyle);
				if(dp.skin) GUI.skin = dp.skin;
			}
			
			if(dp.showBox)
			{
				this.windowRect = GUI.Window(windowID, this.windowRect, TextWindow, "", "box");
			}
			else
			{
				GUI.BeginGroup(this.windowRect);
				this.TextWindow(-1);
				GUI.EndGroup();
			}
		}
		
		c = GUI.backgroundColor;
		c.a = 1;
		GUI.backgroundColor = c;
		c = GUI.color;
		c.a = 1;
		GUI.color = c;
		
		if(this.content.textPos >= this.content.originalText.Length-1)
			return "";
		else
			return this.GetColorString(this.content.currentColor)+this.GetShadowColorString(this.content.shadowColor)+
				this.content.originalText.Substring(this.content.textPos, this.content.originalText.Length-this.content.textPos);
	}
	
	void TextWindow(int windowID)
	{
		if(windowID >= 0 && GameHandler.WindowHandler().GetFocusID() == windowID)
		{
			GUI.BringWindowToFront(windowID);
		}
		
		TextPosition textPosition = new TextPosition(dp.boxBounds, dp.boxPadding, dp.lineSpacing);
		textPosition.bounds.x = 0;
		textPosition.bounds.y = 0;
		if(dp.autoCollapse && this.newHeight > 0) textPosition.bounds.height = this.newHeight;
		
		if(dp.isDragWindow) GUI.DragWindow(dp.dragBounds);
		GUI.BeginGroup(textPosition.bounds);
		if(windowID >= 0 && Event.current.type == EventType.MouseUp && dp.focusable &&
			!GameHandler.WindowHandler().IsFocusBlocked())
		{
			GameHandler.WindowHandler().SetFocusID(windowID);
			GUI.BringWindowToFront(windowID);
		}
		
		if(speakerPortrait != null && speakerPortrait.inBox)
		{
			speakerPortrait.ShowPortrait();
		}
		
		textPosition.bounds.x += dp.boxPadding.x;
		textPosition.bounds.y += dp.boxPadding.y;
		textPosition.bounds.width -= (dp.boxPadding.x + dp.boxPadding.z);
		textPosition.bounds.height -= (dp.boxPadding.y + dp.boxPadding.w);
		
		GUIStyle shadowStyle = new GUIStyle(GUI.skin.label);
		shadowStyle.normal.textColor = DataHolder.Color(1);
		
		GUIStyle textStyle = new GUIStyle(GUI.skin.label);
		textStyle.wordWrap = false;
		
		Vector2 v = Vector2.zero;
		
		if(dp.scrollable)
		{
			Rect scrollRect = new Rect(textPosition.bounds.x, textPosition.bounds.y, textPosition.bounds.width-dp.boxPadding.z, this.scrollSize);
			if(this.scrollSize > textPosition.bounds.height) scrollRect.width -= GUI.skin.verticalScrollbar.fixedWidth+1;
			this.scroll = GUI.BeginScrollView(textPosition.bounds, this.scroll, scrollRect);
			GUI.BeginGroup(scrollRect);
			textPosition.bounds.width = scrollRect.width;
		}
		else
		{
			GUI.BeginGroup(textPosition.bounds);
		}
		
		for(int i=0; i<this.content.label.Length; i++)
		{
			if(dp.showShadow)
			{
				shadowStyle.normal.textColor = this.content.label[i].shadowColor;
				GUI.Label(
					new Rect(this.content.label[i].bounds.x + dp.shadowOffset.x, this.content.label[i].bounds.y + dp.shadowOffset.y, 
					this.content.label[i].bounds.width, this.content.label[i].bounds.height),
					this.content.label[i].content, shadowStyle); 
			}
			
			textStyle.normal.textColor = this.content.label[i].textColor;
			GUI.Label(this.content.label[i].bounds, this.content.label[i].content, textStyle);
		}
		
		GUI.EndGroup();
		if(dp.scrollable)
		{
			GUI.EndScrollView();
			this.scrollSize = this.content.yPos + v.y;
		}
		GUI.EndGroup();
		
		// show ok button
		if(!dp.hideButton)
		{
			Vector2 vec2 = DataHolder.GameSettings().dialogueOkSize[GameHandler.GetLanguage()];
			Rect okBounds = new Rect(0, 0, vec2.x, vec2.y);
			ButtonPosition buttonPos = DataHolder.GameSettings().dialogueOkPosition;
			if(ButtonPosition.TOP_CENTER.Equals(buttonPos) || ButtonPosition.BOTTOM_CENTER.Equals(buttonPos))
			{
				okBounds.x += (dp.boxBounds.width - okBounds.width) / 2;
			}
			else if(ButtonPosition.TOP_RIGHT.Equals(buttonPos) || ButtonPosition.BOTTOM_RIGHT.Equals(buttonPos))
			{
				okBounds.x += (dp.boxBounds.width - okBounds.width);
			}
			if(ButtonPosition.BOTTOM_LEFT.Equals(buttonPos) || ButtonPosition.BOTTOM_CENTER.Equals(buttonPos)
					|| ButtonPosition.BOTTOM_RIGHT.Equals(buttonPos))
			{
				if(dp.autoCollapse && this.newHeight > 0) okBounds.y += (this.newHeight - okBounds.height);
				else okBounds.y += (dp.boxBounds.height - okBounds.height);
			}
			vec2 = DataHolder.GameSettings().dialogueOkOffset[GameHandler.GetLanguage()];
			okBounds.x += vec2.x;
			okBounds.y += vec2.y;
			
			if(dp.okSkin)
			{
				GUI.skin = dp.okSkin;
			}
			GUIContent part = new GUIContent(DataHolder.GameSettings().GetOkButtonContent());
			if(GUI.Button(okBounds, part))
			{
				GameHandler.GetLevelHandler().PressOk();
			}
		}
		if(dp.autoCollapse)
		{
			this.newHeight = this.content.yPos + dp.boxPadding.y + dp.boxPadding.w;
			if(this.newHeight > 0) dp.boxBounds.height = this.newHeight;
		}
	}
	
	// one line
	public string ShowOneLine(DialoguePosition dp, string text)
	{
		return this.ShowOneLine(dp, text, 0, 1);
	}
	
	public string ShowOneLine(DialoguePosition dp, string text, int c1, int c2)
	{
		if(dp.skin) GUI.skin = dp.skin;
		
		Color c = GUI.backgroundColor;
		c.a = dp.alpha;
		GUI.backgroundColor = c;
		c = GUI.color;
		c.a = dp.alpha;
		GUI.color = c;
		
		TextPosition textPosition = new TextPosition(dp.boxBounds, dp.boxPadding, dp.lineSpacing);
		textPosition.bounds.x = dp.currentPos.x;
		textPosition.bounds.y = dp.currentPos.y;
		if(dp.autoCollapse && this.newHeight > 0) textPosition.bounds.height = this.newHeight;
		if(dp.showBox) GUI.Box(textPosition.bounds, "");
		textPosition.bounds.x += dp.boxPadding.x;
		textPosition.bounds.y += dp.boxPadding.y;
		textPosition.bounds.width -= (dp.boxPadding.x + dp.boxPadding.z);
		textPosition.bounds.height -= (dp.boxPadding.y + dp.boxPadding.w);
		
		GUIStyle shadowStyle = new GUIStyle(GUI.skin.label);
		shadowStyle.normal.textColor = DataHolder.Color(c2);
		
		GUIStyle textStyle = new GUIStyle(GUI.skin.label);
		textStyle.wordWrap = false;
		textStyle.normal.textColor = DataHolder.Color(c1);
		
		float xPos = 0;
		float yPos = 0;
		
		GUI.BeginGroup(textPosition.bounds);
		GUIContent part = new GUIContent(text);
		Vector2 v = textStyle.CalcSize(part);
		if(dp.alignCenter)
		{
			xPos = textPosition.bounds.width / 2;
			xPos -= v.x / 2;
		}
		if(dp.showShadow)
		{
			GUI.Label(
				new Rect(xPos + dp.shadowOffset.x, yPos + dp.shadowOffset.y, v.x, v.y),
				part, shadowStyle); 
		}
		
		GUI.Label(new Rect(xPos, yPos, v.x, v.y), part, textStyle);
		yPos += v.y;
		
		GUI.EndGroup();
		
		// show ok button on mouse control
		if(DataHolder.GameSettings().IsMouseAllowed() && !dp.hideButton)
		{
			Vector2 vec2 = DataHolder.GameSettings().dialogueOkSize[GameHandler.GetLanguage()];
			Rect okBounds = new Rect(dp.boxBounds.x, dp.boxBounds.y, vec2.x, vec2.y);
			ButtonPosition buttonPos = DataHolder.GameSettings().dialogueOkPosition;
			if(ButtonPosition.TOP_CENTER.Equals(buttonPos) || ButtonPosition.BOTTOM_CENTER.Equals(buttonPos))
			{
				okBounds.x += (dp.boxBounds.width - okBounds.width) / 2;
			}
			else if(ButtonPosition.TOP_RIGHT.Equals(buttonPos) || ButtonPosition.BOTTOM_RIGHT.Equals(buttonPos))
			{
				okBounds.x += (dp.boxBounds.width - okBounds.width);
			}
			if(ButtonPosition.BOTTOM_LEFT.Equals(buttonPos) || ButtonPosition.BOTTOM_CENTER.Equals(buttonPos)
					|| ButtonPosition.BOTTOM_RIGHT.Equals(buttonPos))
			{
				if(dp.autoCollapse && this.newHeight > 0) okBounds.y += (this.newHeight - okBounds.height);
				else okBounds.y += (dp.boxBounds.height - okBounds.height);
			}
			vec2 = DataHolder.GameSettings().dialogueOkOffset[GameHandler.GetLanguage()];
			okBounds.x += vec2.x;
			okBounds.y += vec2.y;
			
			part = new GUIContent(DataHolder.GameSettings().dialogueOkText[GameHandler.GetLanguage()]);
			v = textStyle.CalcSize(part);
			if(GUI.Button(okBounds, part))
			{
				GameHandler.GetLevelHandler().PressOk();
			}
		}
		if(dp.autoCollapse) this.newHeight = yPos + dp.boxPadding.y + dp.boxPadding.w;
		
		c = GUI.backgroundColor;
		c.a = 1;
		GUI.backgroundColor = c;
		c = GUI.color;
		c.a = 1;
		GUI.color = c;
		
		return "";
	}
	
	// choice handling
	public bool ShowChoice(DialoguePosition dp, string text, string name, string[] choices)
	{
		return this.ShowChoice(dp, text, name, choices, null);
	}
	
	public bool ShowChoice(DialoguePosition dp, string text, string name, string[] choices, SpeakerPortrait speakerPortrait)
	{
		ChoiceContent[] ch = new ChoiceContent[choices.Length];
		for(int i=0; i<ch.Length; i++)
		{
			ch[i] = new ChoiceContent(choices[i]);
		}
		return this.ShowChoice(dp, text, name, ch, speakerPortrait, null);
	}
	
	public bool ShowChoice(DialoguePosition dp, string text, string name, GUIContent[] choices)
	{
		return this.ShowChoice(dp, text, name, choices, null);
	}
	
	public bool ShowChoice(DialoguePosition dp, string text, string name, GUIContent[] choices, SpeakerPortrait speakerPortrait)
	{
		ChoiceContent[] ch = new ChoiceContent[choices.Length];
		for(int i=0; i<ch.Length; i++)
		{
			ch[i] = new ChoiceContent(choices[i]);
		}
		return this.ShowChoice(dp, text, name, ch, speakerPortrait, null);
	}
	
	public bool ShowChoice(DialoguePosition dp, string text, string name, ChoiceContent[] choices)
	{
		return this.ShowChoice(dp, text, name, choices, null, null);
	}
	
	public bool ShowChoice(DialoguePosition dp, string text, string name, ChoiceContent[] choices, SpeakerPortrait speakerPortrait, StatusBar[] bar)
	{
		bool press = false;
		if(dp.skin) GUI.skin = dp.skin;
		
		this.maxSelection = choices.Length;
		
		Color c = GUI.backgroundColor;
		c.a = dp.alpha;
		GUI.backgroundColor = c;
		c = GUI.color;
		c.a = dp.alpha;
		GUI.color = c;
		
		if(this.newContent || this.content == null)
		{
			GUIStyle shadowStyle = new GUIStyle(GUI.skin.label);
			shadowStyle.normal.textColor = DataHolder.Color(1);
			GUIStyle textStyle = new GUIStyle(GUI.skin.label);
			textStyle.wordWrap = false;
			TextPosition textPosition = new TextPosition(dp.boxBounds, dp.boxPadding, dp.lineSpacing);
			textPosition.bounds.width -= (dp.boxPadding.x + dp.boxPadding.z);
			textPosition.bounds.height -= (dp.boxPadding.y + dp.boxPadding.w);
			
			text = MultiLabel.ReplaceSpecials(text);
			this.content = new MultiContent(text, textStyle, shadowStyle, textPosition, dp.scrollable);
			this.choice = choices;
			this.choicePositions = new Vector2[choices.Length];
			this.choiceSizes = new Vector2[choices.Length];
			for(int i=0; i<choice.Length; i++)
			{
				choice[i].InitContent(textStyle, shadowStyle, textPosition, dp.scrollable, dp.selectFirst);
				if(dp.showShadow) choice[i].SetDragGUI(dp.skin, textStyle, shadowStyle, dp.choicePadding.x, dp.shadowOffset);
				else choice[i].SetDragGUI(dp.skin, textStyle, null, dp.choicePadding.x, dp.shadowOffset);
				this.choicePositions[i] = new Vector2(0, 0);
				this.choiceSizes[i] = new Vector2(0, 0);
			}
			this.newContent = false;
		}
		
		if(speakerPortrait != null && !speakerPortrait.inBox)
		{
			speakerPortrait.ShowPortrait();
		}
		
		this.windowRect.x = dp.currentPos.x;
		this.windowRect.y = dp.currentPos.y;
		this.windowRect.width = dp.boxBounds.width;
		this.windowRect.height = dp.boxBounds.height;
		
		int windowID = dp.GetWindowID();
		
		this.dp = dp;
		this.text = text;
		this.speakerPortrait = speakerPortrait;
		this.bar = bar;
		
		if(dp.isDragWindow)
		{
			this.windowRect = GUI.Window(windowID, this.windowRect, ChoiceWindow, name);
			if(dp.currentPos.x != this.windowRect.x || dp.currentPos.y != this.windowRect.y)
			{
				if(dp.focusable) GameHandler.WindowHandler().SetFocusID(windowID);
				
				if(DataHolder.GameSettings().saveWindowDrag)
				{
					if(this.windowRect.x < 0) this.windowRect.x = 0;
					else if((this.windowRect.x+this.windowRect.width) > DataHolder.GameSettings().defaultScreen.x)
					{
						this.windowRect.x = DataHolder.GameSettings().defaultScreen.x-this.windowRect.width;
					}
					if(this.windowRect.y < 0) this.windowRect.y = 0;
					else if((this.windowRect.y+this.windowRect.height) > DataHolder.GameSettings().defaultScreen.y)
					{
						this.windowRect.y = DataHolder.GameSettings().defaultScreen.y-this.windowRect.height;
					}
				}
				dp.currentPos.x = this.windowRect.x;
				dp.currentPos.y = this.windowRect.y;
				dp.SetBasePosition(this.windowRect.x, this.windowRect.y);
			}
		}
		else
		{
			if(name != "")
			{
				GUIStyle shadowStyle = new GUIStyle(GUI.skin.label);
				shadowStyle.normal.textColor = DataHolder.Color(1);
				GUIStyle textStyle = new GUIStyle(GUI.skin.label);
				textStyle.wordWrap = false;
				
				if(dp.nameSkin) GUI.skin = dp.nameSkin;
				Vector2 v = textStyle.CalcSize(new GUIContent(name));
				TextPosition namePosition = new TextPosition(dp.nameBounds, dp.namePadding, 0);
				namePosition.bounds.x = dp.currentPos.x+dp.nameOffset.x;
				namePosition.bounds.y = dp.currentPos.y+dp.nameOffset.y;
				namePosition.bounds.width = v.x + dp.namePadding.x + dp.namePadding.z;
				if(dp.showNameBox) GUI.Box(namePosition.bounds, "");
				namePosition.bounds.x += dp.namePadding.x;
				namePosition.bounds.y += dp.namePadding.y;
				namePosition.bounds.width -= (dp.namePadding.x + dp.namePadding.z);
				namePosition.bounds.height -= (dp.namePadding.y + dp.namePadding.w);
				if(dp.showShadow)
				{
					GUI.Label(
						new Rect(namePosition.bounds.x + dp.shadowOffset.x, namePosition.bounds.y + dp.shadowOffset.y, 
								namePosition.bounds.width, namePosition.bounds.height),
						name, shadowStyle); 
				}
				
				GUI.Label(new Rect(namePosition.bounds.x, namePosition.bounds.y, namePosition.bounds.width, namePosition.bounds.height),
						name, textStyle);
				if(dp.skin) GUI.skin = dp.skin;
			}
			
			if(dp.showBox)
			{
				this.windowRect = GUI.Window(windowID, this.windowRect, ChoiceWindow, "", "box");
			}
			else
			{
				GUI.BeginGroup(this.windowRect);
				this.ChoiceWindow(-1);
				GUI.EndGroup();
			}
		}
		
		if(this.windowPress)
		{
			press = true;
			this.windowPress = false;
		}
		
		c = GUI.backgroundColor;
		c.a = 1;
		GUI.backgroundColor = c;
		c = GUI.color;
		c.a = 1;
		GUI.color = c;
		
		return press;
	}
	
	void ChoiceWindow(int windowID)
	{
		if(windowID >= 0 && GameHandler.WindowHandler().GetFocusID() == windowID)
		{
			GUI.BringWindowToFront(windowID);
		}
		
		TextPosition textPosition = new TextPosition(dp.boxBounds, dp.boxPadding, dp.lineSpacing);
		textPosition.bounds.x = 0;
		textPosition.bounds.y = 0;
		if(dp.autoCollapse && this.newHeight > 0) textPosition.bounds.height = this.newHeight;
		
		if(dp.isDragWindow) GUI.DragWindow(dp.dragBounds);
		GUI.BeginGroup(textPosition.bounds);
		
		if(windowID >= 0 && Event.current.type == EventType.MouseUp && dp.focusable &&
			!GameHandler.WindowHandler().IsFocusBlocked())
		{
			GameHandler.WindowHandler().SetFocusID(windowID);
			GUI.BringWindowToFront(windowID);
		}
		
		if(speakerPortrait != null && speakerPortrait.inBox)
		{
			speakerPortrait.ShowPortrait();
		}
		
		Vector2 v = Vector2.zero;
		textPosition.bounds.x += dp.boxPadding.x;
		textPosition.bounds.y += dp.boxPadding.y;
		textPosition.bounds.width -= (dp.boxPadding.x + dp.boxPadding.z);
		textPosition.bounds.height -= (dp.boxPadding.y + dp.boxPadding.w);
		
		this.viewHeight = textPosition.bounds.height;
		
		GUIStyle shadowStyle = new GUIStyle(GUI.skin.label);
		shadowStyle.normal.textColor = DataHolder.Color(1);
		
		GUIStyle textStyle = new GUIStyle(GUI.skin.label);
		textStyle.wordWrap = false;
		
		if(dp.scrollable)
		{
			Rect scrollRect = new Rect(textPosition.bounds.x, textPosition.bounds.y, textPosition.bounds.width-dp.boxPadding.z, this.scrollSize);
			if(this.scrollSize > textPosition.bounds.height) scrollRect.width -= GUI.skin.verticalScrollbar.fixedWidth+1;
			this.scroll = GUI.BeginScrollView(textPosition.bounds, this.scroll, scrollRect);
			GUI.BeginGroup(scrollRect);
			textPosition.bounds.width = scrollRect.width;
		}
		else
		{
			GUI.BeginGroup(textPosition.bounds);
		}
		
		for(int i=0; i<this.content.label.Length; i++)
		{
			if(dp.showShadow)
			{
				shadowStyle.normal.textColor = this.content.label[i].shadowColor;
				GUI.Label(
					new Rect(this.content.label[i].bounds.x + dp.shadowOffset.x, this.content.label[i].bounds.y + dp.shadowOffset.y, 
					this.content.label[i].bounds.width, this.content.label[i].bounds.height),
					this.content.label[i].content, shadowStyle); 
			}
			
			textStyle.normal.textColor = this.content.label[i].textColor;
			GUI.Label(this.content.label[i].bounds, this.content.label[i].content, textStyle);
		}
		
		float xPos = this.content.xPos;
		float yPos = this.content.yPos;
		if("" != text) v = textStyle.CalcSize(this.content.label[this.content.label.Length-1].content);
		else v = Vector2.zero;
		
		// get title text length
		float choiceStartX = 0;
		float maxRightWidth = 0;
		for(int i=0; i<choice.Length; i++)
		{
			if(choice[i].title != null)
			{
				v = choice[i].titleSize;
				if(v.x > choiceStartX) choiceStartX = v.x;
			}
			if(choice[i].rightLabel != null && choice[i].alignRightSide)
			{
				if(choice[i].rightSize.x > maxRightWidth) maxRightWidth = choice[i].rightSize.x;
			}
		}
		if(choiceStartX > 0) choiceStartX += dp.columnSpacing;
		
		// choices
		xPos = choiceStartX;
		if("" == text) yPos -= textPosition.lineSpacing;
		this.selectionOffset = v.y + dp.columnSpacing;
		LevelHandler lh = GameHandler.GetLevelHandler();
		textStyle.alignment = TextAnchor.UpperLeft;
		shadowStyle.alignment = TextAnchor.UpperLeft;
		
		colFill = dp.columnFill;
		if(ColumnFill.VERTICAL.Equals(colFill))
		{
			maxCol = (int)Mathf.Ceil(((float)choice.Length) / ((float)dp.choiceColumns));
			colCount = dp.choiceColumns;
		}
		else
		{
			maxCol = dp.choiceColumns;
			colCount = (int)Mathf.Ceil(((float)choice.Length) / ((float)dp.choiceColumns));
		}
		
		int counter = 0;
		float storedYPos = yPos;
		int columnCount = dp.choiceColumns;
		if(this.choice.Length == 1) columnCount = 1;
		float colWidth = (textPosition.bounds.width / columnCount) - (dp.columnSpacing * (columnCount - 1)) - choiceStartX;
		float maxYPos = yPos;
		float cOffsetX = 0;
		
		int lastSelection = this.selection;
		for(int i=0; i<choice.Length; i++)
		{
			cOffsetX = dp.choiceOffsetX*i;
			if(counter >= maxCol)
			{
				counter = 0;
				if(ColumnFill.VERTICAL.Equals(colFill))
				{
					yPos = storedYPos;
					xPos += colWidth + dp.columnSpacing + choiceStartX;
				}
				else
				{
					xPos = choiceStartX;
					yPos += v.y + dp.columnSpacing;
				}
			}
			else if(i > 0)
			{
				if(ColumnFill.VERTICAL.Equals(colFill))
				{
					yPos +=  dp.columnSpacing;
				}
				else
				{
					xPos += colWidth + dp.columnSpacing + choiceStartX;
				}
			}
			
			float imageWidth = 0;
			float textAdd = 0;
			Rect cbounds;
			Color c;
			
			// title
			if(choiceStartX > 0 && choice[i].title != null && "" != choice[i].title.text)
			{
				v = choice[i].titleSize;
				cbounds = new Rect(xPos, yPos, v.x, v.y);
				if(dp.choiceDefineWidth) cbounds.x = cOffsetX;
				if(choice[i].title.image)
				{
					textStyle.alignment = TextAnchor.MiddleLeft;
					GUI.Label(new Rect(cbounds.x+dp.choicePadding.x-choiceStartX, cbounds.y, v.x, v.y), 
							new GUIContent(choice[i].title.image), textStyle);
					imageWidth = choice[i].title.image.width;
					textAdd = textStyle.CalcSize(new GUIContent(".")).x;
					textStyle.alignment = TextAnchor.UpperLeft;
				}
				for(int j=0; j<choice[i].titleLabel.label.Length; j++)
				{
					Rect b = new Rect(cbounds.x+choice[i].titleLabel.label[j].bounds.x + imageWidth+dp.choicePadding.x-choiceStartX+textAdd, 
										cbounds.y+choice[i].titleLabel.label[j].bounds.y, 
										cbounds.width,
										cbounds.height);
					if(dp.showShadow)
					{
						shadowStyle.normal.textColor = choice[i].titleLabel.label[j].shadowColor;
						GUI.Label(
							new Rect(b.x + dp.shadowOffset.x, 
										b.y + dp.shadowOffset.y, 
										b.width,
										b.height),
								choice[i].titleLabel.label[j].content, shadowStyle); 
					}
					textStyle.normal.textColor = choice[i].titleLabel.label[j].textColor;
					GUI.Label(b, choice[i].titleLabel.label[j].content, textStyle);
				}
			}
			
			c = GUI.backgroundColor;
			if(choice[i].active || dp.alpha < dp.choiceInactiveAlpha) c.a = dp.alpha;
			else c.a = dp.choiceInactiveAlpha;
			GUI.backgroundColor = c;
			c = GUI.color;
			if(choice[i].active || dp.alpha < dp.choiceInactiveAlpha) c.a = dp.alpha;
			else c.a = dp.choiceInactiveAlpha;
			GUI.color = c;
			
			if(this.selection == i && dp.selectSkin &&
					DataHolder.GameSettings().IsKeyboardAllowed() && choice[i].isButton)
			{
				GUI.skin = dp.selectSkin;
			}
			v = choice[i].leftSize;
			float cHeight = choice[i].rightSize.y;
			if(cHeight > v.y) v.y = cHeight;
			cbounds = new Rect(xPos, yPos, colWidth, v.y);
			if(dp.choiceDefineWidth)
			{
				cbounds.x = cOffsetX;
				cbounds.width = dp.choiceWidth;
			}
			this.choicePositions[i].x = cbounds.x+this.windowRect.x+textPosition.bounds.x;
			this.choicePositions[i].y = cbounds.y+this.windowRect.y+textPosition.bounds.y;
			if(v.x > colWidth) v.x = colWidth-(2*dp.choicePadding.x);
			this.choiceSizes[i].x = cbounds.width;
			this.choiceSizes[i].y = cbounds.height;
			
			if(choice[i].isButton)
			{
				if(GUI.Button(cbounds, ""))
				{
					this.selection = i;
					if(!choice[i].doubleClick && 
						(!choice[i].selectFirst || lastSelection == this.selection))
					{
						lh.PressOk();
						this.windowPress = true;
					}
					else if(choice[i].selectFirst)
					{
						GameHandler.ChangeHappened(0, 0, 0);
					}
				}
			}
			
			if(this.bar != null)
			{
				for(int j=0; j<this.bar.Length; j++)
				{
					this.bar[j].ShowBar(i, new Vector2(cbounds.x, cbounds.y));
				}
			}
			
			// left
			imageWidth = 0;
			textAdd = 0;
			if(choice[i].content.image)
			{
				textStyle.alignment = TextAnchor.MiddleLeft;
				GUI.Label(new Rect(cbounds.x+dp.choicePadding.x, cbounds.y, v.x, v.y), 
						new GUIContent(choice[i].content.image), textStyle);
				imageWidth = choice[i].content.image.width;
				textAdd = textStyle.CalcSize(new GUIContent(".")).x;
				textStyle.alignment = TextAnchor.UpperLeft;
			}
			
			if(choice[i].leftLabel != null)
			{
				for(int j=0; j<choice[i].leftLabel.label.Length; j++)
				{
					Rect b = new Rect(cbounds.x+choice[i].leftLabel.label[j].bounds.x + imageWidth+dp.choicePadding.x+textAdd, 
										cbounds.y+choice[i].leftLabel.label[j].bounds.y, 
										cbounds.width,
										cbounds.height);
					if(dp.showShadow)
					{
						shadowStyle.normal.textColor = choice[i].leftLabel.label[j].shadowColor;
						GUI.Label(
							new Rect(b.x + dp.shadowOffset.x, 
										b.y + dp.shadowOffset.y, 
										b.width,
										b.height),
								choice[i].leftLabel.label[j].content, shadowStyle); 
					}
					textStyle.normal.textColor = choice[i].leftLabel.label[j].textColor;
					GUI.Label(b, choice[i].leftLabel.label[j].content, textStyle);
				}
			}
			
			// right
			if(choice[i].rightLabel != null)
			{
				float xAdd = 0;
				if(choice[i].alignRightSide) xAdd = cbounds.x+cbounds.width-maxRightWidth-dp.choicePadding.x;
				else xAdd = cbounds.x+cbounds.width-choice[i].rightSize.x-dp.choicePadding.x;
				
				for(int j=0; j<choice[i].rightLabel.label.Length; j++)
				{
					Rect b = new Rect(xAdd+choice[i].rightLabel.label[j].bounds.x, 
										cbounds.y+choice[i].rightLabel.label[j].bounds.y, 
										choice[i].rightLabel.label[j].bounds.width,
										choice[i].rightLabel.label[j].bounds.height);
					if(dp.showShadow)
					{
						shadowStyle.normal.textColor = choice[i].rightLabel.label[j].shadowColor;
						GUI.Label(
							new Rect(b.x + dp.shadowOffset.x, 
										b.y + dp.shadowOffset.y, 
										b.width,
										b.height),
								choice[i].rightLabel.label[j].content, shadowStyle); 
					}
					textStyle.normal.textColor = choice[i].rightLabel.label[j].textColor;
					GUI.Label(b, choice[i].rightLabel.label[j].content, textStyle);
				}
			}
			
			if(this.selection == i && dp.skin)
			{
				GUI.skin = dp.skin;
			}
			if(ColumnFill.VERTICAL.Equals(colFill) || i == choice.Length-1) yPos += v.y;
			if(yPos > maxYPos) maxYPos = yPos;
			counter++;
			
			c = GUI.backgroundColor;
			c.a = 1;
			GUI.backgroundColor = c;
			c = GUI.color;
			c.a = 1;
			GUI.color = c;
		}
		yPos = maxYPos;
		
		this.selectionHeight = v.y + dp.columnSpacing;
		if(dp.autoCollapse)
		{
			this.newHeight = yPos + dp.boxPadding.y + dp.boxPadding.w;
			if(this.newHeight > 0) dp.boxBounds.height = this.newHeight;
		}
		GUI.EndGroup();
		if(dp.scrollable)
		{
			GUI.EndScrollView();
			this.scrollSize = yPos;
		}
		GUI.EndGroup();
	}
	
	// text repalcements
	public static string ReplaceSpecials(string text)
	{
		// money
		text = text.Replace("#m", GameHandler.GetMoney().ToString());
		// items
		int replace = MultiLabel.NextSpecial(text, "#in");
		while(replace != -1)
		{
			text = text.Replace("#in"+replace+"#", DataHolder.Items().GetName(replace));
			replace = MultiLabel.NextSpecial(text, "#in");
		}
		replace = MultiLabel.NextSpecial(text, "#ic");
		while(replace != -1)
		{
			text = text.Replace("#ic"+replace+"#", GameHandler.GetItemCount(replace).ToString());
			replace = MultiLabel.NextSpecial(text, "#ic");
		}
		// weapons
		replace = MultiLabel.NextSpecial(text, "#wn");
		while(replace != -1)
		{
			text = text.Replace("#wn"+replace+"#", DataHolder.Weapons().GetName(replace));
			replace = MultiLabel.NextSpecial(text, "#wn");
		}
		replace = MultiLabel.NextSpecial(text, "#wc");
		while(replace != -1)
		{
			text = text.Replace("#wc"+replace+"#", GameHandler.GetWeaponCount(replace).ToString());
			replace = MultiLabel.NextSpecial(text, "#wc");
		}
		replace = MultiLabel.NextSpecial(text, "#we");
		while(replace != -1)
		{
			text = text.Replace("#we"+replace+"#", GameHandler.GetEquippedWeaponCount(replace).ToString());
			replace = MultiLabel.NextSpecial(text, "#we");
		}
		// armors
		replace = MultiLabel.NextSpecial(text, "#an");
		while(replace != -1)
		{
			text = text.Replace("#an"+replace+"#", DataHolder.Armors().GetName(replace));
			replace = MultiLabel.NextSpecial(text, "#an");
		}
		replace = MultiLabel.NextSpecial(text, "#ac");
		while(replace != -1)
		{
			text = text.Replace("#ac"+replace+"#", GameHandler.GetArmorCount(replace).ToString());
			replace = MultiLabel.NextSpecial(text, "#ac");
		}
		replace = MultiLabel.NextSpecial(text, "#ae");
		while(replace != -1)
		{
			text = text.Replace("#ae"+replace+"#", GameHandler.GetEquippedArmorCount(replace).ToString());
			replace = MultiLabel.NextSpecial(text, "#ae");
		}
		// characters
		replace = MultiLabel.NextSpecial(text, "#cn");
		while(replace != -1)
		{
			text = text.Replace("#cn"+replace+"#", DataHolder.Characters().GetName(replace));
			replace = MultiLabel.NextSpecial(text, "#cn");
		}
		// enemies
		replace = MultiLabel.NextSpecial(text, "#en");
		while(replace != -1)
		{
			text = text.Replace("#en"+replace+"#", DataHolder.Enemies().GetName(replace));
			replace = MultiLabel.NextSpecial(text, "#en");
		}
		// status values
		replace = MultiLabel.NextSpecial(text, "#sv");
		while(replace != -1)
		{
			text = text.Replace("#sv"+replace+"#", DataHolder.StatusValues().GetName(replace));
			replace = MultiLabel.NextSpecial(text, "#sv");
		}
		// status effects
		replace = MultiLabel.NextSpecial(text, "#se");
		while(replace != -1)
		{
			text = text.Replace("#se"+replace+"#", DataHolder.Effects().GetName(replace));
			replace = MultiLabel.NextSpecial(text, "#se");
		}
		// elements
		replace = MultiLabel.NextSpecial(text, "#el");
		while(replace != -1)
		{
			text = text.Replace("#el"+replace+"#", DataHolder.Elements().GetName(replace));
			replace = MultiLabel.NextSpecial(text, "#el");
		}
		// skill types
		replace = MultiLabel.NextSpecial(text, "#st");
		while(replace != -1)
		{
			text = text.Replace("#st"+replace+"#", DataHolder.SkillTypes().GetName(replace));
			replace = MultiLabel.NextSpecial(text, "#st");
		}
		// item types
		replace = MultiLabel.NextSpecial(text, "#it");
		while(replace != -1)
		{
			text = text.Replace("#it"+replace+"#", DataHolder.ItemTypes().GetName(replace));
			replace = MultiLabel.NextSpecial(text, "#it");
		}
		// skills
		replace = MultiLabel.NextSpecial(text, "#sk");
		while(replace != -1)
		{
			text = text.Replace("#sk"+replace+"#", DataHolder.Skills().GetName(replace));
			replace = MultiLabel.NextSpecial(text, "#sk");
		}
		// equipment parts
		replace = MultiLabel.NextSpecial(text, "#ep");
		while(replace != -1)
		{
			text = text.Replace("#ep"+replace+"#", DataHolder.EquipmentParts().GetName(replace));
			replace = MultiLabel.NextSpecial(text, "#ep");
		}
		// classes
		replace = MultiLabel.NextSpecial(text, "#cl");
		while(replace != -1)
		{
			text = text.Replace("#cl"+replace+"#", DataHolder.Classes().GetName(replace));
			replace = MultiLabel.NextSpecial(text, "#cl");
		}
		// variables
		string rep = MultiLabel.NextSpecialString(text, "#var");
		while("" != rep)
		{
			text = text.Replace("#var"+rep+"#", GameHandler.GetVariable(rep));
			rep = MultiLabel.NextSpecialString(text, "#var");
		}
		// number variables
		rep = MultiLabel.NextSpecialString(text, "#nvar");
		while("" != rep)
		{
			text = text.Replace("#nvar"+rep+"#", GameHandler.GetNumberVariable(rep).ToString());
			rep = MultiLabel.NextSpecialString(text, "#nvar");
		}
		// number variables as int
		rep = MultiLabel.NextSpecialString(text, "#nivar");
		while("" != rep)
		{
			int v = (int)GameHandler.GetNumberVariable(rep);
			text = text.Replace("#nivar"+rep+"#", v.ToString());
			rep = MultiLabel.NextSpecialString(text, "#nivar");
		}
		// game time
		int pos = text.IndexOf("#time", 0);
		if(pos != -1 && (pos+1) < (text.Length-1))
		{
			text = text.Replace("#time", GameHandler.GetTimeString());
		}
		return DataHolder.Statistic.GetStatisticText(text);
	}
	
	public static int NextSpecial(string text, string special)
	{
		int value = -1;
		int pos = text.IndexOf(special, 0);
		if(pos != -1 && (pos+1) < (text.Length-1))
		{
			int pos2 = text.IndexOf("#", pos+1);
			if(pos2 != -1)
			{
				value = int.Parse(text.Substring(pos+special.Length, pos2-pos-special.Length));
			}
		}
		return value;
	}
	
	public static string NextSpecialString(string text, string special)
	{
		string value = "";
		int pos = text.IndexOf(special, 0);
		if(pos != -1 && (pos+1) < (text.Length-1))
		{
			int pos2 = text.IndexOf("#", pos+1);
			if(pos2 != -1)
			{
				value = text.Substring(pos+special.Length, pos2-pos-special.Length);
			}
		}
		return value;
	}
}