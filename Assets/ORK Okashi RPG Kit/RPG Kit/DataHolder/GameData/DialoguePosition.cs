
using UnityEngine;

public class DialoguePosition
{
	// text box
	public Rect boxBounds = new Rect(0, 0, 1280, 800);
	public Vector4 boxPadding = new Vector4(10, 10, 10, 10);
	public float lineSpacing = 10;
	public bool showBox = true;
	
	// name box
	public Rect nameBounds = new Rect(0, 750, 150, 50);
	public Vector4 namePadding = new Vector4(5, 1, 5, 1);
	public bool showNameBox = true;
	
	// text shadow
	public bool showShadow = true;
	public Vector2 shadowOffset = new Vector2(1, 1);
	
	// scroll
	public bool scrollable = false;
	public bool autoCollapse = false;
	
	// one line
	public bool oneline = false;
	public bool alignCenter = false;
	
	public bool hideButton = false;
	
	// drag window
	public bool isDragWindow = false;
	public Rect dragBounds = new Rect(0, 0, 1920, 20);
	
	// choice
	public Vector2 choicePadding = new Vector2(5, 5);
	public float choiceInactiveAlpha = 0.5f;
	public bool choiceDefineWidth = false;
	public float choiceWidth = 300;
	public float choiceOffsetX = 0;
	public int choiceColumns = 1;
	public float columnSpacing = 10;
	public ColumnFill columnFill = ColumnFill.VERTICAL;
	public bool selectFirst = false;
	
	// skins
	public string skinName = "";
	public GUISkin skin;
	public string selectSkinName = "";
	public GUISkin selectSkin;
	public string okSkinName = "";
	public GUISkin okSkin;
	public string nameSkinName = "";
	public GUISkin nameSkin;
	
	// fading
	public bool fadeIn = false;
	public float fadeInTime = 0.3f;
	public EaseType fadeInInterpolation = EaseType.Linear;
	
	public bool fadeOut = false;
	public float fadeOutTime = 0.3f;
	public EaseType fadeOutInterpolation = EaseType.Linear;
	
	// moving
	public bool moveIn = false;
	public float moveInTime = 0.3f;
	public Vector2 moveInStart = Vector2.zero;
	public EaseType moveInInterpolation = EaseType.Linear;
	
	public bool moveOut = false;
	public float moveOutTime = 0.3f;
	public Vector2 moveOutStart = Vector2.zero;
	public EaseType moveOutInterpolation = EaseType.Linear;
	
	// ingame
	public int realID = -1;
	
	private Function interpolateIn;
	private Function interpolateOut;
	private float time = 0;
	public bool inDone = false;
	public bool outDone = false;
	
	private Function interpolateMIn;
	private Function interpolateMOut;
	private float mTime = 0;
	public bool mInDone = false;
	public bool mOutDone = false;
	public float mDistanceX = 0;
	public float mDistanceY = 0;
	
	public float alpha = 1;
	public Vector2 currentPos = Vector2.zero;
	public Vector2 nameOffset = Vector2.zero;
	
	public bool isFading = false;
	public bool isMoving = false;
	
	private int windowID = -2;
	public bool focusable = true;
	
	// sprite
	public MultiLabel multiLabel = new MultiLabel();
	public Rect contentBounds;
	public Vector2 contentP2Offset;
	public Rect vScrollBounds;
	public Vector2 vScrollSize;
	public Rect okButtonBounds;
	
	// scrolling
	public float scrollBoundTop = 0;
	public float scrollBoundBottom = 0;
	public float scrollViewHeight = 0;
	public float scrollPixelVertical = 0;
	public Vector2 scrollSize = Vector2.zero;
	
	public float vScrollTop = 0;
	public float vScrollPixel = 0;
	public float vScrollRatio = 0;
	
	public bool preloaded = false;
	public bool destroyTextures = true;
	
	public Color bgColor = new Color(1, 1, 1, 1);
	public Color fgColor = new Color(1, 1, 1, 1);
	
	public DPSprite sprite;
	private bool acceptPressed = false;
	
	public DialoguePosition()
	{
		
	}
	
	/*
	============================================================================
	Accept interaction functions
	============================================================================
	*/
	public bool GetAcceptPressed()
	{
		bool val = this.acceptPressed;
		this.acceptPressed = false;
		return val;
	}
	
	public void SetAcceptPressed(bool val)
	{
		this.acceptPressed = val;
	}
	
	/*
	============================================================================
	Window focus functions
	============================================================================
	*/
	public int GetWindowID()
	{
		this.windowID = GameHandler.WindowHandler().GetID(this.windowID);
		return this.windowID;
	}
	
	public void SetFocus()
	{
		if(this.focusable) GameHandler.WindowHandler().SetFocusID(this.GetWindowID());
	}
	
	public bool IsFocused()
	{
		return this.windowID == GameHandler.WindowHandler().GetFocusID();
	}
	
	public void SetBasePosition(float newX, float newY)
	{
		if(this.boxBounds.x != newX || this.boxBounds.y != newY)
		{
			if(this.moveIn)
			{
				float diff = this.boxBounds.x - this.moveInStart.x;
				this.moveInStart.x = newX - diff;
				diff = this.boxBounds.y - this.moveInStart.y;
				this.moveInStart.y = newY - diff;
			}
			if(this.moveOut)
			{
				float diff = this.boxBounds.x - this.moveOutStart.x;
				this.moveOutStart.x = newX - diff;
				diff = this.boxBounds.y - this.moveOutStart.y;
				this.moveOutStart.y = newY - diff;
			}
			this.boxBounds.x = newX;
			this.boxBounds.y = newY;
		}
	}
	
	/*
	============================================================================
	Init functions
	============================================================================
	*/
	public void Show(string name, string text, ChoiceContent[] choices, SpeakerPortrait speakerPortrait, StatusBar[] bar, bool dt)
	{
		this.destroyTextures = dt;
		if(this.skin == null) this.LoadSkins();
		if(this.fadeIn && !this.inDone) this.alpha = 0;
		if(this.sprite == null)
		{
			this.multiLabel.CreateContent(this, name, text, choices, speakerPortrait, bar);
			this.sprite = GameHandler.GUIHandler().AddDPSprite(this);
		}
		else
		{
			this.multiLabel.CreateContent(this, name, text, choices, speakerPortrait, bar);
		}
	}
	
	public void Preload(string name, string text, ChoiceContent[] choices, SpeakerPortrait speakerPortrait, StatusBar[] bar)
	{
		if(this.skin == null) this.LoadSkins();
		if(this.fadeIn && !this.inDone) this.alpha = 0;
		this.multiLabel.CreateContent(this, name, text, choices, speakerPortrait, bar);
		this.preloaded = true;
		this.destroyTextures = false;
	}
	
	public void LoadSkins()
	{
		if(!this.skin && "" != this.skinName)
		{
			this.skin = (GUISkin)Resources.Load(DataHolder.DialoguePositions().skinPath+this.skinName, typeof(GUISkin));
		}
		if(!this.selectSkin && "" != this.selectSkinName)
		{
			this.selectSkin = (GUISkin)Resources.Load(DataHolder.DialoguePositions().skinPath+this.selectSkinName, typeof(GUISkin));
		}
		if(!this.okSkin && "" != this.okSkinName)
		{
			this.okSkin = (GUISkin)Resources.Load(DataHolder.DialoguePositions().skinPath+this.okSkinName, typeof(GUISkin));
		}
		if(!this.nameSkin && "" != this.nameSkinName)
		{
			this.nameSkin = (GUISkin)Resources.Load(DataHolder.DialoguePositions().skinPath+this.nameSkinName, typeof(GUISkin));
		}
	}
	
	public void InitIn()
	{
		if(this.skin == null) this.LoadSkins();
		this.outDone = false;
		this.mOutDone = false;
		this.nameOffset = new Vector2(this.nameBounds.x-this.boxBounds.x, this.nameBounds.y-this.boxBounds.y);
		if(this.fadeIn)
		{
			this.interpolateIn = Interpolate.Ease(this.fadeInInterpolation);
			this.time = 0;
			this.alpha = 0;
			this.isFading = true;
			this.inDone = false;
		}
		else
		{
			this.isFading = false;
			this.inDone = true;
		}
		if(this.moveIn)
		{
			this.interpolateMIn = Interpolate.Ease(this.moveInInterpolation);
			this.mTime = 0;
			this.mDistanceX = this.boxBounds.x - this.moveInStart.x;
			this.mDistanceY = this.boxBounds.y - this.moveInStart.y;
			this.currentPos = new Vector2(this.moveInStart.x, this.moveInStart.y);
			this.isMoving = true;
			this.mInDone = false;
		}
		else
		{
			this.currentPos = new Vector2(this.boxBounds.x, this.boxBounds.y);
			this.isMoving = false;
			this.mInDone = true;
		}
		this.Register();
	}
	
	public void InitOut()
	{
		this.inDone = true;
		this.mInDone = true;
		if(this.fadeOut)
		{
			this.interpolateOut = Interpolate.Ease(this.fadeOutInterpolation);
			this.time = 0;
			this.alpha = 1;
			this.isFading = true;
			this.outDone = false;
		}
		else
		{
			this.isFading = false;
			this.outDone = true;
		}
		if(this.moveOut)
		{
			this.interpolateMOut = Interpolate.Ease(this.moveOutInterpolation);
			this.mTime = 0;
			this.mDistanceX = this.moveOutStart.x - this.currentPos.x;
			this.mDistanceY = this.moveOutStart.y - this.currentPos.y;
			this.isMoving = true;
			this.mOutDone = false;
		}
		else
		{
			this.isMoving = false;
			this.mOutDone = true;
		}
		this.Unregister();
	}
	
	public void Reset()
	{
		this.time = 0;
		this.mTime = 0;
		this.inDone = false;
		this.outDone = false;
		this.mInDone = false;
		this.mOutDone = false;
		this.isFading = false;
		this.isMoving = false;
		this.interpolateIn = null;
		this.interpolateOut = null;
		this.interpolateMIn = null;
		this.interpolateMOut = null;
		this.mDistanceX = 0;
		this.mDistanceY = 0;
		this.currentPos = Vector2.zero;
		
		this.alpha = 1;
		this.bgColor = new Color(1, 1, 1, 1);
		this.fgColor = new Color(1, 1, 1, 1);
		this.acceptPressed = false;
	}
	
	public void Reset2()
	{
		this.contentBounds = new Rect(0, 0, 0, 0);
		this.contentP2Offset = Vector2.zero;
		this.vScrollBounds = new Rect(0, 0, 0, 0);
		this.vScrollSize = Vector2.zero;
		this.okButtonBounds = new Rect(0, 0, 0, 0);
		this.scrollBoundTop = 0;
		this.scrollBoundBottom = 0;
		this.scrollViewHeight = 0;
		this.scrollPixelVertical = 0;
		this.scrollSize = Vector2.zero;
		this.vScrollTop = 0;
		this.vScrollPixel = 0;
		this.vScrollRatio = 0;
	}
	
	/*
	============================================================================
	Check functions
	============================================================================
	*/
	public bool IsFadingIn()
	{
		return (!this.inDone && this.interpolateIn != null);
	}
	
	public bool IsFadingOut()
	{
		return (this.inDone && !this.outDone && this.interpolateOut != null);
	}
	
	public bool IsOutDone()
	{
		return this.outDone && this.mOutDone;
	}
	
	public bool CanControl()
	{
		return !this.isFading && !this.isMoving &&
				this.inDone && this.mInDone &&
				!this.outDone && !this.mOutDone;
	}
	
	public void SetOutDone()
	{
		this.outDone = true;
		this.mOutDone = true;
	}
	
	/*
	============================================================================
	Fading/Moving functions
	============================================================================
	*/
	public void Register()
	{
		GameHandler.WindowHandler().AddPosition(this);
	}
	
	public void Unregister()
	{
		GameHandler.WindowHandler().RemovePosition(this);
	}
	
	public void Tick(float t)
	{
		this.DoFade(t);
		this.multiLabel.Tick(t);
	}
	
	public void DoFade(float t)
	{
		this.DoFadeIn(t);
		this.DoFadeOut(t);
		this.DoMoveIn(t);
		this.DoMoveOut(t);
	}
	
	public void DoFadeIn(float t)
	{
		if(!this.inDone && this.interpolateIn != null)
		{
			this.time += t;
			this.alpha = Interpolate.Ease(this.interpolateIn, 0, 1, this.time, this.fadeInTime);
			if(this.time >= this.fadeInTime)
			{
				this.isFading = false;
				this.inDone = true;
			}
		}
	}
	
	public void DoFadeOut(float t)
	{
		if(this.inDone && !this.outDone && this.interpolateOut != null)
		{
			this.time += t;
			this.alpha = Interpolate.Ease(this.interpolateOut, 1, -1, this.time, this.fadeOutTime);
			if(this.time >= this.fadeOutTime)
			{
				this.isFading = false;
				this.outDone = true;
			}
		}
	}
	
	public void DoMoveIn(float t)
	{
		if(!this.mInDone && this.interpolateMIn != null)
		{
			this.mTime += t;
			if(this.mDistanceX != 0)
			{
				this.currentPos.x = Interpolate.Ease(this.interpolateMIn, this.moveInStart.x, this.mDistanceX, this.mTime, this.moveInTime);
			}
			if(this.mDistanceY != 0)
			{
				this.currentPos.y = Interpolate.Ease(this.interpolateMIn, this.moveInStart.y, this.mDistanceY, this.mTime, this.moveInTime);
			}
			if(this.mTime >= this.moveInTime)
			{
				this.isMoving = false;
				this.mInDone = true;
			}
		}
	}
	
	public void DoMoveOut(float t)
	{
		if(!this.mOutDone && this.interpolateMOut != null)
		{
			this.mTime += t;
			if(this.mDistanceX != 0)
			{
				this.currentPos.x = Interpolate.Ease(this.interpolateMOut, this.boxBounds.x, this.mDistanceX, this.mTime, this.moveOutTime);
			}
			if(this.mDistanceY != 0)
			{
				this.currentPos.y = Interpolate.Ease(this.interpolateMOut, this.boxBounds.y, this.mDistanceY, this.mTime, this.moveOutTime);
			}
			if(this.mTime >= this.moveOutTime)
			{
				this.isMoving = false;
				this.mOutDone = true;
			}
		}
	}
	
	/*
	============================================================================
	Interact functions
	============================================================================
	*/
	public bool IsInWindow(Vector2 position)
	{
		return this.currentPos.x < position.x && 
				this.currentPos.y < position.y &&
				(this.currentPos.x + this.boxBounds.width) > position.x &&
				(this.currentPos.y + this.boxBounds.height) > position.y;
	}
	
	public bool IsInDrag(Vector2 position)
	{
		return this.isDragWindow && 
				(this.currentPos.x + this.dragBounds.x) < position.x && 
				(this.currentPos.y + this.dragBounds.y) < position.y &&
				(this.currentPos.x + this.dragBounds.x + this.dragBounds.width) > position.x &&
				(this.currentPos.y + this.dragBounds.y + this.dragBounds.height) > position.y;
	}
	
	public bool IsInContent(Vector2 position)
	{
		return (this.currentPos.x + this.contentBounds.x) < position.x && 
				(this.currentPos.y + this.contentBounds.y) < position.y &&
				(this.currentPos.x + this.contentBounds.x + this.contentBounds.width) > position.x &&
				(this.currentPos.y + this.contentBounds.y + this.contentBounds.height) > position.y;
	}
	
	public bool IsInVScroll(Vector2 position, Vector2 sp)
	{
		return (sp.x - this.vScrollSize.x/2) <= position.x && 
				(sp.y - this.vScrollSize.y/2) <= position.y &&
				(sp.x + this.vScrollSize.x/2) >= position.x &&
				(sp.y + this.vScrollSize.y/2) >= position.y;
	}
	
	public bool IsAboveVScroll(Vector2 position, Vector2 sp)
	{
		return (this.currentPos.x + this.vScrollBounds.x) < position.x && 
				(this.currentPos.y + this.contentBounds.y) < position.y &&
				(this.currentPos.x + this.vScrollBounds.x + this.vScrollBounds.width) > position.x &&
				(this.currentPos.y + this.contentBounds.y + this.contentBounds.height) > position.y &&
				(sp.y - this.vScrollSize.y/2) > position.y;
	}
	
	public bool IsBelowVScroll(Vector2 position, Vector2 sp)
	{
		return (this.currentPos.x + this.vScrollBounds.x) < position.x && 
				(this.currentPos.y + this.contentBounds.y) < position.y &&
				(this.currentPos.x + this.vScrollBounds.x + this.vScrollBounds.width) > position.x &&
				(this.currentPos.y + this.contentBounds.y + this.contentBounds.height) > position.y &&
				(sp.y + this.vScrollSize.y/2) < position.y;
	}
	
	public bool IsInOkButton(Vector2 position)
	{
		Vector2 hlp = new Vector2(this.currentPos.x + (this.boxBounds.width/2) + this.okButtonBounds.x, 
				this.currentPos.y + (this.boxBounds.height/2) + this.okButtonBounds.y);
		return (hlp.x - this.okButtonBounds.width/2) < position.x && 
				(hlp.y - this.okButtonBounds.height/2) < position.y &&
				(hlp.x + this.okButtonBounds.width/2) > position.x &&
				(hlp.y + this.okButtonBounds.height/2) > position.y;
	}
	
	/*
	============================================================================
	Scroll functions
	============================================================================
	*/
	public void SetScroll(float vertical)
	{
		this.scrollSize.y += vertical*this.scrollPixelVertical;
		this.ScrollBoundCheck();
	}
	
	public void ScrollBoundCheck()
	{
		if(this.scrollSize.y > this.scrollBoundTop) this.scrollSize.y = this.scrollBoundTop;
		else if(this.scrollSize.y < this.scrollBoundBottom) this.scrollSize.y = this.scrollBoundBottom;
	}
}