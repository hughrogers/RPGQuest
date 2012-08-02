
using System.Collections;
using UnityEngine;

public class HUD
{
	public bool[] controlType = new bool[0];
	public bool onInteraction = false;
	
	public string skinName = "";
	public GUISkin skin;
	
	public bool showBox = false;
	public Rect bounds = new Rect(0, 0, 600, 300);
	public bool onlyOne = false;
	public Vector2 offset = Vector2.zero;
	
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
	
	public HUDElement[] element = new HUDElement[0];
	public HUDClick hudClick = HUDClick.NONE;
	public int screenIndex = 0;
	
	// ingame
	public int realID = 0;
	public bool lastCheck = false;
	
	private Function interpolateIn;
	private Function interpolateOut;
	private float time = 0;
	public bool inDone = true;
	public bool outDone = true;
	
	private Function interpolateMIn;
	private Function interpolateMOut;
	private float mTime = 0;
	public bool mInDone = true;
	public bool mOutDone = true;
	public float mDistanceX = 0;
	public float mDistanceY = 0;
	
	public float alpha = 1;
	public Vector2 currentPos = Vector2.zero;
	
	// ORK gui
	private Color[] bgTexture;
	public Vector2 bgSize;
	public Vector2 start;
	public Vector2 p2Offset;
	private Texture2D texture;
	private int[] lastParty;
	private Vector2[] partyPos;
	
	public HUD()
	{
		
	}
	
	public void AddElement()
	{
		element = ArrayHelper.Add(new HUDElement(), element);
	}
	
	public void RemoveElement(int index)
	{
		element = ArrayHelper.Remove(index, element);
	}
	
	public void MoveUp(int index)
	{
		HUDElement tmp = this.element[index-1];
		this.element[index-1] = this.element[index];
		this.element[index] = tmp;
	}
	
	public void MoveDown(int index)
	{
		HUDElement tmp = this.element[index+1];
		this.element[index+1] = this.element[index];
		this.element[index] = tmp;
	}
	
	// ingame
	public GUISkin GetSkin()
	{
		if(this.skin == null && this.skinName != null && "" != this.skinName)
		{
			this.skin = (GUISkin)Resources.Load(DataHolder.HUDs().resourcePath+this.skinName, typeof(GUISkin));
		}
		return this.skin;
	}
	
	// fading moving
	public void InitIn()
	{
		this.outDone = false;
		this.mOutDone = false;
		if(this.fadeIn)
		{
			this.interpolateIn = Interpolate.Ease(this.fadeInInterpolation);
			this.interpolateOut = null;
			this.time = 0;
			this.alpha = 0;
			this.inDone = false;
		}
		else
		{
			this.alpha = 1;
			this.inDone = true;
		}
		if(this.moveIn)
		{
			this.interpolateMIn = Interpolate.Ease(this.moveInInterpolation);
			this.interpolateMOut = null;
			this.mTime = 0;
			this.mDistanceX = this.bounds.x - this.moveInStart.x;
			this.mDistanceY = this.bounds.y - this.moveInStart.y;
			this.currentPos = new Vector2(this.moveInStart.x, this.moveInStart.y);
			this.mInDone = false;
		}
		else
		{
			this.currentPos = new Vector2(this.bounds.x, this.bounds.y);
			this.mInDone = false;
		}
		if(GUISystemType.ORK.Equals(DataHolder.GameSettings().guiSystemType))
		{
			GameHandler.GUIHandler().AddHUDSprite(this);
		}
	}
	
	public void InitOut()
	{
		this.inDone = true;
		this.mInDone = true;
		if(this.fadeOut)
		{
			this.interpolateOut = Interpolate.Ease(this.fadeOutInterpolation);
			this.interpolateIn = null;
			this.time = 0;
			this.alpha = 1;
			this.outDone = false;
		}
		else
		{
			this.outDone = true;
		}
		if(this.moveOut)
		{
			this.interpolateMOut = Interpolate.Ease(this.moveOutInterpolation);
			this.interpolateMIn = null;
			this.mTime = 0;
			this.mDistanceX = this.moveOutStart.x - this.currentPos.x;
			this.mDistanceY = this.moveOutStart.y - this.currentPos.y;
			this.mOutDone = false;
		}
		else
		{
			this.mOutDone = true;
		}
	}
	
	public void DoFade(float t, int interacts)
	{
		if(this.IsVisible(interacts) && !this.lastCheck)
		{
			this.lastCheck = true;
			this.InitIn();
		}
		else if(!this.IsVisible(interacts) && this.lastCheck)
		{
			this.lastCheck = false;
			this.InitOut();
		}
		
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
				this.mInDone = true;
			}
		}
	}
	
	public void DoMoveOut(float t)
	{
		if(this.mInDone && !this.mOutDone && this.interpolateMOut != null)
		{
			this.mTime += t;
			if(this.mDistanceX != 0)
			{
				this.currentPos.x = Interpolate.Ease(this.interpolateMOut, this.bounds.x, this.mDistanceX, this.mTime, this.moveOutTime);
			}
			if(this.mDistanceY != 0)
			{
				this.currentPos.y = Interpolate.Ease(this.interpolateMOut, this.bounds.y, this.mDistanceY, this.mTime, this.moveOutTime);
			}
			if(this.mTime >= this.moveOutTime)
			{
				this.mOutDone = true;
			}
		}
	}
	
	public bool IsOutDone()
	{
		return this.outDone && this.mOutDone;
	}
	
	public bool IsVisible(int interacts)
	{
		bool visible = ((this.controlType[(int)GameHandler.GetControlType()] || 
			(this.controlType[(int)ControlType.BATTLE] && GameHandler.IsControlBattle())) && 
			(!this.onInteraction || (this.onInteraction && interacts > 0)));
		if(GameHandler.IsControlBattle() && !this.controlType[(int)ControlType.BATTLE])
		{
			visible = false;
		}
		return visible;
	}
	
	public void ShowHUD(Matrix4x4 matrix, int interacts)
	{
		if(this.IsVisible(interacts) || 
			(this.fadeIn && !this.inDone) || (this.fadeOut && !this.outDone) ||
			(this.moveIn && !this.mInDone) || (this.moveOut && !this.mOutDone))
		{
			GUI.matrix = matrix;
			GUI.skin = this.GetSkin();
			
			Color ca = GUI.backgroundColor;
			ca.a = this.alpha;
			GUI.backgroundColor = ca;
			ca = GUI.color;
			ca.a = this.alpha;
			GUI.color = ca;
			
			Character[] party = GameHandler.Party().GetBattleParty();
			int count = party.Length;
			if(this.onlyOne && party.Length > 0) count = 1;
			for(int i=0; i<count; i++)
			{
				Rect pos = new Rect(this.currentPos.x+this.offset.x*i, this.currentPos.y+this.offset.y*i,
						this.bounds.width, this.bounds.height);
				if(this.showBox)
				{
					GUI.Box(pos, "");
				}
				GUI.BeginGroup(pos);
				for(int j=0; j<this.element.Length; j++)
				{
					this.element[j].ShowElement(party[i]);
				}
				GUI.EndGroup();
			}
			ca = GUI.backgroundColor;
			ca.a = 1;
			GUI.backgroundColor = ca;
			ca = GUI.color;
			ca.a = 1;
			GUI.color = ca;
		}
	}
	
	/*
	============================================================================
	HUD click functions
	============================================================================
	*/
	public bool ClickHUD(Vector2 mp, int interacts)
	{
		bool clicked = false;
		if(!GameHandler.IsGamePaused() && GameHandler.IsControlField() && 
			this.IsVisible(interacts) && !HUDClick.NONE.Equals(this.hudClick))
		{
			Character[] party = GameHandler.Party().GetBattleParty();
			int count = party.Length;
			if(this.onlyOne && party.Length > 0) count = 1;
			for(int i=0; i<count; i++)
			{
				Vector2 pos = new Vector2(this.currentPos.x+this.offset.x*i, this.currentPos.y+this.offset.y*i);
				if(pos.x < mp.x && mp.x < pos.x+this.bounds.width &&
					pos.y < mp.y && mp.y < pos.y+this.bounds.height)
				{
					if(HUDClick.INTERACT.Equals(this.hudClick))
					{
						GameHandler.GetLevelHandler().StartInteraction();
					}
					else if(HUDClick.MENU.Equals(this.hudClick))
					{
						GameHandler.ChangeHappened(3, -1, party[i].realID);
					}
					else if(HUDClick.MENUSCREEN.Equals(this.hudClick))
					{
						GameHandler.ChangeHappened(3, this.screenIndex, party[i].realID);
					}
					else if(HUDClick.BATTLEMENU.Equals(this.hudClick) && 
						DataHolder.BattleSystem().IsRealTime() && 
						GameHandler.IsInBattleArea() &&
						party[i].CanChooseAction())
					{
						if(this.screenIndex == 0) party[i].ChooseAction();
						else if(this.screenIndex == 1) party[i].CallItemMenu();
						else if(this.screenIndex == 2) party[i].CallSkillMenu();
					}
					clicked = true;
					break;
				}
			}
		}
		return clicked;
	}
	
	public bool IsInHUD(Vector3 point)
	{
		bool inHUD = false;
		if(!HUDClick.NONE.Equals(this.hudClick))
		{
			Character[] party = GameHandler.Party().GetBattleParty();
			int count = party.Length;
			if(this.onlyOne && party.Length > 0) count = 1;
			for(int i=0; i<count; i++)
			{
				Vector2 pos = new Vector2(this.currentPos.x+this.offset.x*i, this.currentPos.y+this.offset.y*i);
				if(pos.x < point.x && point.x < pos.x+this.bounds.width &&
					pos.y < point.y && point.y < pos.y+this.bounds.height)
				{
					inHUD = true;
				}
			}
		}
		return inHUD;
	}
	
	/*
	============================================================================
	ORK GUI functions
	============================================================================
	*/
	public void CreateTextures()
	{
		if(this.skin == null) this.GetSkin();
		if(this.bgTexture == null) this.CreateBGTexture();
		this.CreateTexture(GameHandler.Party().GetBattleParty());
	}
	
	public void CreateBGTexture()
	{
		Texture2D tex = TextureDrawer.GetCleanTexture((int)this.bounds.width, 
				(int)this.bounds.height);
		if(this.showBox)
		{
			tex = TextureDrawer.SetImageTexture(tex, 
					new Rect(0, 0, this.bounds.width, this.bounds.height), 
					GameHandler.GetSkin(this.skin).box);
		}
		for(int i=0; i<this.element.Length; i++)
		{
			if(this.element[i].showBox)
			{
				tex = TextureDrawer.AddImageTexture(tex, this.element[i].bounds, 
						GameHandler.GetSkin(this.skin).box);
			}
		}
		this.bgTexture = tex.GetPixels();
		GameObject.Destroy(tex);
	}
	
	private void CreateTexture(Character[] party)
	{
		int count = party.Length;
		if(this.onlyOne && party.Length > 0) count = 1;
		this.lastParty = new int[count];
		this.partyPos = new Vector2[count];
		
		this.bgSize = new Vector2(this.bounds.width+Mathf.Abs(this.offset.x*(count-1)),
				this.bounds.height+Mathf.Abs(this.offset.y*(count-1)));
		
		if(this.texture == null)
		{
			this.texture = TextureDrawer.GetCleanTexture(
					TextureDrawer.GetNextPowerOfTwo(this.bgSize.x),
					TextureDrawer.GetNextPowerOfTwo(this.bgSize.y));
		}
		else
		{
			this.texture.Resize(
					TextureDrawer.GetNextPowerOfTwo(this.bgSize.x),
					TextureDrawer.GetNextPowerOfTwo(this.bgSize.y));
			this.texture = TextureDrawer.ClearTexture(this.texture);
		}
		this.p2Offset = new Vector2((this.texture.width-this.bgSize.x)/2,
				(this.texture.height-this.bgSize.y)/2);
		this.start = Vector2.zero;
		if(this.offset.x < 0) this.start.x = Mathf.Abs(this.offset.x*(count-1));
		if(this.offset.y < 0) this.start.y = Mathf.Abs(this.offset.y*(count-1));
		
		GUIFont font = DataHolder.Fonts().GetFont(this.skin.font);
		
		Vector2 pos = Vector2.zero;
		for(int i=0; i<count; i++)
		{
			this.lastParty[i] = party[i].realID;
			pos.x = this.start.x+this.p2Offset.x+this.offset.x*i;
			pos.y = this.texture.height-(this.start.y+this.p2Offset.y+this.offset.y*i+this.bounds.height);
			this.partyPos[i] = new Vector2(pos.x, pos.y);
			this.AddCharacter(pos, party[i], font);
		}
		this.texture.Apply();
	}
	
	private void AddCharacter(Vector2 pos, Character c, GUIFont font)
	{
		this.texture.SetPixels((int)pos.x, (int)pos.y,
				(int)this.bounds.width, (int)this.bounds.height, this.bgTexture);
		pos.y += this.bounds.height;
		for(int j=0; j<this.element.Length; j++)
		{
			this.texture = this.element[j].AddTexture(this.texture, c, pos, font, this.realID);
		}
	}
	
	public Texture2D GetTexture()
	{
		if(this.texture == null) this.CreateTextures();
		return this.texture;
	}
	
	public bool CheckChange()
	{
		bool sizeChange = false;
		Character[] party = GameHandler.Party().GetBattleParty();
		int count = party.Length;
		if(this.onlyOne && party.Length > 0) count = 1;
		GUIFont font = null;
		// same party size
		if(this.lastParty.Length == count)
		{
			bool change = false;
			for(int i=0; i<count; i++)
			{
				// same IDs
				if(party[i].realID == this.lastParty[i])
				{
					for(int j=0; j<this.element.Length; j++)
					{
						if(this.element[j].Changed(party[i], this.realID))
						{
							change = true;
							if(font == null) font = DataHolder.Fonts().GetFont(this.skin.font);
							this.AddCharacter(new Vector2(this.partyPos[i].x, this.partyPos[i].y), party[i], font);
							break;
						}
					}
				}
				// ID changed
				else
				{
					change = true;
					this.lastParty[i] = party[i].realID;
					if(font == null) font = DataHolder.Fonts().GetFont(this.skin.font);
					this.AddCharacter(new Vector2(this.partyPos[i].x, this.partyPos[i].y), party[i], font);
				}
			}
			if(change) this.texture.Apply();
		}
		// party size changed
		else
		{
			this.CreateTexture(party);
			sizeChange = true;
		}
		return sizeChange;
	}
}

public class HUDElement
{
	public bool fold = true;
	
	public bool showBox = false;
	public Rect bounds = new Rect(0, 0, 300, 100);
	public HUDElementType type = HUDElementType.TEXT;
	public TextAnchor textAnchor = TextAnchor.UpperLeft;
	
	// display options
	public HUDDisplayType displayType = HUDDisplayType.TEXT;
	
	// font options
	public bool showShadow = true;
	public int textColor = 0;
	public int shadowColor = 1;
	public Vector2 shadowOffset = new Vector2(1, 1);
	
	// text
	public string[] text = new string[] {""};
	public string divider = "";
	// name
	public HUDNameType nameType = HUDNameType.CHARACTER;
	// image
	public string imageName = "";
	public Texture2D texture;
	public ScaleMode scaleMode = ScaleMode.StretchToFill;
	public bool alphaBlend = true;
	public float imageAspect = 0;
	// status
	public int statusID = 0;
	public bool showMax = true;
	// effects
	public int rows = 1;
	public int columns = 5;
	public float spacing = 3;
	
	// general bar
	public bool useImage = false;
	public int barColor = 0;
	// general text
	public HUDContentType contentType = HUDContentType.TEXT;
	
	// variable
	public string variableName = "";
	public bool numberVariable = false;
	public bool asInt = false;
	
	// ingame
	private string lastVarText = "";
	
	public HUDElement()
	{
		
	}
	
	public Hashtable GetTextOptions(Hashtable ht)
	{
		ht.Add("c1", this.textColor.ToString());
		ht.Add("shadow", this.showShadow.ToString());
		if(this.showShadow)
		{
			ht.Add("c2", this.shadowColor.ToString());
			ht.Add("ox", this.shadowOffset.x.ToString());
			ht.Add("oy", this.shadowOffset.y.ToString());
		}
		return ht;
	}
	
	public void SetTextOptions(Hashtable ht)
	{
		if(ht.ContainsKey("c1")) this.textColor = int.Parse((string)ht["c1"]);
		if(ht.ContainsKey("c2")) this.shadowColor = int.Parse((string)ht["c2"]);
		if(ht.ContainsKey("shadow")) this.showShadow = bool.Parse((string)ht["shadow"]);
		if(ht.ContainsKey("ox")) this.shadowOffset.x = float.Parse((string)ht["ox"]);
		if(ht.ContainsKey("oy")) this.shadowOffset.y = float.Parse((string)ht["oy"]);
	}
	
	public Hashtable GetImageOptions(Hashtable ht)
	{
		if(this.imageName != null && "" != this.imageName)
		{
			ht.Add(XMLHandler.CONTENT, this.imageName);
			ht.Add("scalemode", this.scaleMode.ToString());
			ht.Add("alphablend", this.alphaBlend.ToString());
			ht.Add("imageaspect", this.imageAspect.ToString());
		}
		return ht;
	}
	
	public void SetImageOptions(Hashtable ht)
	{
		if(ht.ContainsKey(XMLHandler.CONTENT)) this.imageName = ht[XMLHandler.CONTENT] as string;
		if(ht.ContainsKey("scalemode")) this.scaleMode = (ScaleMode)System.Enum.Parse(typeof(ScaleMode), (string)ht["scalemode"]);
		if(ht.ContainsKey("alphablend")) this.alphaBlend = bool.Parse((string)ht["alphablend"]);
		if(ht.ContainsKey("imageaspect")) this.imageAspect = float.Parse((string)ht["imageaspect"]);
	}
	
	// ingame
	public Texture2D GetImage()
	{
		if(this.texture == null && (HUDElementType.IMAGE.Equals(this.type) || this.useImage))
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
	
	public string GetVariableText()
	{
		string txt = this.text[GameHandler.GetLanguage()];
		if(this.numberVariable && this.asInt)
		{
			txt = txt.Replace("#v", ((int)GameHandler.GetNumberVariable(this.variableName)).ToString());
		}
		else if(this.numberVariable)
		{
			txt = txt.Replace("#v", GameHandler.GetNumberVariable(this.variableName).ToString());
		}
		else
		{
			txt = txt.Replace("#v", GameHandler.GetVariable(this.variableName));
		}
		return txt;
	}
	
	public void ShowElement(Character c)
	{
		if(this.showBox)
		{
			GUI.Box(this.bounds, "");
		}
		
		GUIStyle guiStyle = new GUIStyle(GUI.skin.label);
		guiStyle.alignment = this.textAnchor;
		
		if(HUDElementType.TEXT.Equals(this.type))
		{
			if(this.showShadow)
			{
				guiStyle.normal.textColor = DataHolder.Color(this.shadowColor);
				GUI.Label(
					new Rect(this.bounds.x + this.shadowOffset.x, this.bounds.y + this.shadowOffset.y, 
							this.bounds.width, this.bounds.height), this.text[GameHandler.GetLanguage()], guiStyle); 
			}
			guiStyle.normal.textColor = DataHolder.Color(this.textColor);
			GUI.Label(this.bounds, this.text[GameHandler.GetLanguage()], guiStyle);
		}
		else if(HUDElementType.IMAGE.Equals(this.type))
		{
			GUI.DrawTexture(this.bounds, this.GetImage(), this.scaleMode, this.alphaBlend, this.imageAspect);
		}
		else if(HUDElementType.NAME.Equals(this.type))
		{
			GUIContent gc = null;
			if(HUDContentType.TEXT.Equals(this.contentType))
			{
				if(HUDNameType.CHARACTER.Equals(this.nameType)) gc = new GUIContent(c.GetName());
				else if(HUDNameType.CLASS.Equals(this.nameType)) gc = new GUIContent(DataHolder.Classes().GetName(c.currentClass));
				else if(HUDNameType.STATUS.Equals(this.nameType)) gc = new GUIContent(DataHolder.StatusValues().GetName(this.statusID));
			}
			else if(HUDContentType.ICON.Equals(this.contentType))
			{
				if(HUDNameType.CHARACTER.Equals(this.nameType)) gc = new GUIContent(c.GetIcon());
				else if(HUDNameType.CLASS.Equals(this.nameType)) gc = new GUIContent(DataHolder.Classes().GetIcon(c.currentClass));
				else if(HUDNameType.STATUS.Equals(this.nameType)) gc = new GUIContent(DataHolder.StatusValues().GetIcon(this.statusID));
			}
			else if(HUDContentType.BOTH.Equals(this.contentType))
			{
				if(HUDNameType.CHARACTER.Equals(this.nameType)) gc = c.GetContent();
				else if(HUDNameType.CLASS.Equals(this.nameType)) gc = DataHolder.Classes().GetContent(c.currentClass);
				else if(HUDNameType.STATUS.Equals(this.nameType)) gc = DataHolder.StatusValues().GetContent(this.statusID);
			}
			if(gc != null)
			{
				if(this.showShadow)
				{
					guiStyle.normal.textColor = DataHolder.Color(this.shadowColor);
					GUI.Label(
						new Rect(this.bounds.x + this.shadowOffset.x, this.bounds.y + this.shadowOffset.y, 
								this.bounds.width, this.bounds.height), gc, guiStyle); 
				}
				guiStyle.normal.textColor = DataHolder.Color(this.textColor);
				GUI.Label(this.bounds, gc, guiStyle);
			}
		}
		else if(HUDElementType.STATUS.Equals(this.type))
		{
			if(HUDDisplayType.TEXT.Equals(this.displayType))
			{
				string txt = c.status[this.statusID].GetValue().ToString();
				if(c.status[this.statusID].IsConsumable() && this.showMax)
				{
					txt += this.divider+c.status[c.status[this.statusID].maxStatus].GetValue().ToString();
				}
				if(this.showShadow)
				{
					guiStyle.normal.textColor = DataHolder.Color(this.shadowColor);
					GUI.Label(
						new Rect(this.bounds.x + this.shadowOffset.x, this.bounds.y + this.shadowOffset.y, 
								this.bounds.width, this.bounds.height), txt, guiStyle); 
				}
				guiStyle.normal.textColor = DataHolder.Color(this.textColor);
				GUI.Label(this.bounds, txt, guiStyle);
			}
			else if(HUDDisplayType.BAR.Equals(this.displayType) && c.status[this.statusID].IsConsumable())
			{
				float v1 = c.status[this.statusID].GetValue();
				float v2 = c.status[c.status[this.statusID].maxStatus].GetValue();
				v2 /= 100;
				v1 /= v2;
				GUI.DrawTexture(new Rect(this.bounds.x, this.bounds.y, this.bounds.width*v1/100, this.bounds.height),
						this.GetImage(), this.scaleMode, this.alphaBlend, this.imageAspect);
			}
		}
		else if(HUDElementType.TIMEBAR.Equals(this.type) ||
			HUDElementType.USED_TIMEBAR.Equals(this.type) ||
			HUDElementType.CASTTIME.Equals(this.type))
		{
			float v1 = 0;
			float v2 = 0;
			if(HUDElementType.TIMEBAR.Equals(this.type))
			{
				v1 = c.timeBar;
				v2 = DataHolder.BattleSystem().maxTimebar;
			}
			else if(HUDElementType.USED_TIMEBAR.Equals(this.type))
			{
				v1 = c.usedTimeBar;
				v2 = DataHolder.BattleSystem().maxTimebar;
			}
			else if(HUDElementType.CASTTIME.Equals(this.type))
			{
				v1 = c.GetSkillCastTime();
				v2 = c.GetSkillCastTimeMax();
			}
			if(HUDDisplayType.TEXT.Equals(this.displayType))
			{
				string txt = ((int)v1).ToString();
				if(this.showShadow)
				{
					guiStyle.normal.textColor = DataHolder.Color(this.shadowColor);
					GUI.Label(
						new Rect(this.bounds.x + this.shadowOffset.x, this.bounds.y + this.shadowOffset.y, 
								this.bounds.width, this.bounds.height), txt, guiStyle); 
				}
				guiStyle.normal.textColor = DataHolder.Color(this.textColor);
				GUI.Label(this.bounds, txt, guiStyle);
			}
			else if(HUDDisplayType.BAR.Equals(this.displayType) && v2 > 0)
			{
				v2 /= 100;
				v1 /= v2;
				GUI.DrawTexture(new Rect(this.bounds.x, this.bounds.y, this.bounds.width*v1/100, this.bounds.height),
						this.GetImage(), this.scaleMode, this.alphaBlend, this.imageAspect);
			}
		}
		else if(HUDElementType.EFFECT.Equals(this.type))
		{
			int maxView = this.rows*this.columns;
			float cellWidth = this.bounds.width;
			float cellHeight = this.bounds.height;
			cellWidth -= this.spacing*(this.columns-1);
			cellHeight -= this.spacing*(this.rows-1);
			cellWidth /= this.columns;
			cellHeight /= this.rows;
			int currentColumn = 0;
			int currentRow = 0;
			for(int i=0; i<c.effect.Length; i++)
			{
				if(i >= maxView)
				{
					break;
				}
				else
				{
					GUIContent gc = null;
					if(HUDContentType.TEXT.Equals(this.contentType))
					{
						gc = new GUIContent(DataHolder.Effects().GetName(c.effect[i].realID));
					}
					else if(HUDContentType.ICON.Equals(this.contentType))
					{
						gc = new GUIContent(DataHolder.Effects().GetIcon(c.effect[i].realID));
					}
					else if(HUDContentType.BOTH.Equals(this.contentType))
					{
						gc = DataHolder.Effects().GetContent(c.effect[i].realID);
					}
					if(gc != null)
					{
						Rect pos = new Rect(this.bounds.x+cellWidth*currentColumn+this.spacing*currentColumn, 
								this.bounds.y+cellHeight*currentRow+this.spacing*currentRow, cellWidth, cellHeight);
						if(this.showShadow && (HUDContentType.TEXT.Equals(this.contentType) ||
							HUDContentType.BOTH.Equals(this.contentType)))
						{
							guiStyle.normal.textColor = DataHolder.Color(this.shadowColor);
							GUI.Label(
								new Rect(pos.x + this.shadowOffset.x, pos.y + this.shadowOffset.y, 
										pos.width, pos.height), gc, guiStyle); 
						}
						guiStyle.normal.textColor = DataHolder.Color(this.textColor);
						GUI.Label(pos, gc, guiStyle);
						currentColumn++;
					}
					if(currentColumn >= this.columns)
					{
						currentColumn = 0;
						currentRow++;
					}
				}
			}
		}
		else if(HUDElementType.VARIABLE.Equals(this.type))
		{
			string txt = this.GetVariableText();
			if(txt != null && txt != "")
			{
				if(this.showShadow)
				{
					guiStyle.normal.textColor = DataHolder.Color(this.shadowColor);
					GUI.Label(
						new Rect(this.bounds.x + this.shadowOffset.x, this.bounds.y + this.shadowOffset.y, 
								this.bounds.width, this.bounds.height), txt, guiStyle); 
				}
				guiStyle.normal.textColor = DataHolder.Color(this.textColor);
				GUI.Label(this.bounds, txt, guiStyle);
			}
		}
	}
	
	/*
	============================================================================
	ORK GUI functions
	============================================================================
	*/
	private Rect GetAnchoredRect(Vector2 pos, Vector2 size, Rect b)
	{
		Rect rect = new Rect(b.x, b.y, size.x, size.y);
		if(TextAnchor.UpperCenter.Equals(this.textAnchor))
		{
			rect.x += (b.width-rect.width)/2;
		}
		else if(TextAnchor.UpperRight.Equals(this.textAnchor))
		{
			rect.x += b.width-rect.width;
		}
		else if(TextAnchor.MiddleLeft.Equals(this.textAnchor))
		{
			rect.y += (b.height-rect.height)/2;
		}
		else if(TextAnchor.MiddleCenter.Equals(this.textAnchor))
		{
			rect.x += (b.width-rect.width)/2;
			rect.y += (b.height-rect.height)/2;
		}
		else if(TextAnchor.MiddleRight.Equals(this.textAnchor))
		{
			rect.x += b.width-rect.width;
			rect.y += (b.height-rect.height)/2;
		}
		else if(TextAnchor.LowerLeft.Equals(this.textAnchor))
		{
			rect.y += b.height-rect.height;
		}
		else if(TextAnchor.LowerCenter.Equals(this.textAnchor))
		{
			rect.x += (b.width-rect.width)/2;
			rect.y += b.height-rect.height;
		}
		else if(TextAnchor.LowerRight.Equals(this.textAnchor))
		{
			rect.x += b.width-rect.width;
			rect.y += b.height-rect.height;
		}
		
		rect.x += pos.x;
		rect.y = pos.y-rect.y-rect.height;
		return rect;
	}
	
	public Texture2D AddTexture(Texture2D hudTexture, Character c, Vector2 pos, GUIFont font, int hudID)
	{
		if(HUDElementType.TEXT.Equals(this.type))
		{
			hudTexture = font.AddTextTexture(hudTexture, this.text[GameHandler.GetLanguage()], 
					this.GetAnchoredRect(pos, font.GetTextSize(this.text[GameHandler.GetLanguage()]), this.bounds), 
					DataHolder.Color(this.textColor), DataHolder.Color(this.shadowColor), 0, 
					Vector2.zero, this.showShadow, this.shadowOffset, false);
		}
		else if(HUDElementType.IMAGE.Equals(this.type))
		{
			this.GetImage();
			if(this.texture != null)
			{
				Vector2 scaledSize = TextureDrawer.GetScaledSize(this.texture, this.bounds, this.scaleMode);
				hudTexture = TextureDrawer.AddTexture(hudTexture, 
						new Rect(this.bounds.x+pos.x, pos.y-this.bounds.y-scaledSize.y, scaledSize.x, scaledSize.y),
						TextureDrawer.GetScaledPixels(this.texture, scaledSize, this.bounds, this.scaleMode));
			}
		}
		else if(HUDElementType.NAME.Equals(this.type))
		{
			GUIContent gc = null;
			if(HUDContentType.TEXT.Equals(this.contentType))
			{
				if(HUDNameType.CHARACTER.Equals(this.nameType)) gc = new GUIContent(c.GetName());
				else if(HUDNameType.CLASS.Equals(this.nameType)) gc = new GUIContent(DataHolder.Classes().GetName(c.currentClass));
				else if(HUDNameType.STATUS.Equals(this.nameType)) gc = new GUIContent(DataHolder.StatusValues().GetName(this.statusID));
			}
			else if(HUDContentType.ICON.Equals(this.contentType))
			{
				if(HUDNameType.CHARACTER.Equals(this.nameType)) gc = new GUIContent(c.GetIcon());
				else if(HUDNameType.CLASS.Equals(this.nameType)) gc = new GUIContent(DataHolder.Classes().GetIcon(c.currentClass));
				else if(HUDNameType.STATUS.Equals(this.nameType)) gc = new GUIContent(DataHolder.StatusValues().GetIcon(this.statusID));
			}
			else if(HUDContentType.BOTH.Equals(this.contentType))
			{
				if(HUDNameType.CHARACTER.Equals(this.nameType)) gc = c.GetContent();
				else if(HUDNameType.CLASS.Equals(this.nameType)) gc = DataHolder.Classes().GetContent(c.currentClass);
				else if(HUDNameType.STATUS.Equals(this.nameType)) gc = DataHolder.StatusValues().GetContent(this.statusID);
			}
			if(gc != null)
			{
				Vector2 size = Vector2.zero;
				float gap = 0;
				if("" != gc.text)
				{
					size = font.GetTextSize(gc.text);
					if(gc.image != null)
					{
						gap = font.GetTextSize(" ").x;
						size.x += gap;
					}
				}
				if(gc.image != null)
				{
					size.x += gc.image.width;
					if(size.y < gc.image.height) size.y = gc.image.height;
				}
				
				Rect rect = this.GetAnchoredRect(pos, size, this.bounds);
				if(gc.image != null)
				{
					Texture2D tex = gc.image as Texture2D;
					hudTexture = TextureDrawer.AddTexture(hudTexture,
							new Rect(rect.x, rect.y, gc.image.width, gc.image.height),
							tex.GetPixels());
					rect.x += gc.image.width+gap;
					rect.width -= gc.image.width+gap;
				}
				if("" != gc.text)
				{
					hudTexture = font.AddTextTexture(hudTexture, gc.text, rect, 
						DataHolder.Color(this.textColor), DataHolder.Color(this.shadowColor), 0, 
						Vector2.zero, this.showShadow, this.shadowOffset, false);
				}
			}
		}
		else if(HUDElementType.STATUS.Equals(this.type))
		{
			c.status[this.statusID].lastValueHUD[hudID] = c.status[this.statusID].GetValue();
			if(HUDDisplayType.TEXT.Equals(this.displayType))
			{
				string txt = c.status[this.statusID].GetValue().ToString();
				if(c.status[this.statusID].IsConsumable() && this.showMax)
				{
					txt += this.divider+c.status[c.status[this.statusID].maxStatus].GetValue().ToString();
				}
				hudTexture = font.AddTextTexture(hudTexture, txt, 
						this.GetAnchoredRect(pos, font.GetTextSize(txt), this.bounds), 
						DataHolder.Color(this.textColor), DataHolder.Color(this.shadowColor), 0, 
						Vector2.zero, this.showShadow, this.shadowOffset, false);
			}
			else if(HUDDisplayType.BAR.Equals(this.displayType) && c.status[this.statusID].IsConsumable())
			{
				float v1 = c.status[this.statusID].GetValue();
				float v2 = c.status[c.status[this.statusID].maxStatus].GetValue();
				v2 /= 100;
				v1 /= v2;
				
				this.GetImage();
				if(this.texture != null)
				{
					Rect b = new Rect(this.bounds.x, this.bounds.y, this.bounds.width*v1/100, this.bounds.height);
					Vector2 scaledSize = TextureDrawer.GetScaledSize(this.texture, b, this.scaleMode);
					hudTexture = TextureDrawer.AddTexture(hudTexture, 
							new Rect(b.x+pos.x, pos.y-b.y-scaledSize.y, scaledSize.x, scaledSize.y),
							TextureDrawer.GetScaledPixels(this.texture, scaledSize, b, this.scaleMode));
				}
			}
		}
		else if(HUDElementType.TIMEBAR.Equals(this.type) ||
			HUDElementType.USED_TIMEBAR.Equals(this.type) ||
			HUDElementType.CASTTIME.Equals(this.type))
		{
			float v1 = 0;
			float v2 = 0;
			if(HUDElementType.TIMEBAR.Equals(this.type))
			{
				v1 = c.timeBar;
				v2 = DataHolder.BattleSystem().maxTimebar;
				c.timeBarHUD[hudID] = v1;
			}
			else if(HUDElementType.USED_TIMEBAR.Equals(this.type))
			{
				v1 = c.usedTimeBar;
				v2 = DataHolder.BattleSystem().maxTimebar;
				c.usedTimeBarHUD[hudID] = v1;
			}
			else if(HUDElementType.CASTTIME.Equals(this.type))
			{
				v1 = c.GetSkillCastTime();
				v2 = c.GetSkillCastTimeMax();
				c.castTimeHUD[hudID] = v1;
			}
			if(HUDDisplayType.TEXT.Equals(this.displayType))
			{
				string txt = ((int)v1).ToString();
				hudTexture = font.AddTextTexture(hudTexture, txt, 
						this.GetAnchoredRect(pos, font.GetTextSize(txt), this.bounds), 
						DataHolder.Color(this.textColor), DataHolder.Color(this.shadowColor), 0, 
						Vector2.zero, this.showShadow, this.shadowOffset, false);
			}
			else if(HUDDisplayType.BAR.Equals(this.displayType) && v2 > 0)
			{
				v2 /= 100;
				v1 /= v2;
				
				this.GetImage();
				if(this.texture != null)
				{
					Rect b = new Rect(this.bounds.x, this.bounds.y, this.bounds.width*v1/100, this.bounds.height);
					Vector2 scaledSize = TextureDrawer.GetScaledSize(this.texture, b, this.scaleMode);
					hudTexture = TextureDrawer.AddTexture(hudTexture, 
							new Rect(b.x+pos.x, pos.y-b.y-scaledSize.y, scaledSize.x, scaledSize.y),
							TextureDrawer.GetScaledPixels(this.texture, scaledSize, b, this.scaleMode));
				}
			}
		}
		else if(HUDElementType.EFFECT.Equals(this.type))
		{
			c.effectHUD = false;
			int maxView = this.rows*this.columns;
			float cellWidth = this.bounds.width;
			float cellHeight = this.bounds.height;
			cellWidth -= this.spacing*(this.columns-1);
			cellHeight -= this.spacing*(this.rows-1);
			cellWidth /= this.columns;
			cellHeight /= this.rows;
			int currentColumn = 0;
			int currentRow = 0;
			for(int i=0; i<c.effect.Length; i++)
			{
				if(i >= maxView)
				{
					break;
				}
				else
				{
					GUIContent gc = null;
					if(HUDContentType.TEXT.Equals(this.contentType))
					{
						gc = new GUIContent(DataHolder.Effects().GetName(c.effect[i].realID));
					}
					else if(HUDContentType.ICON.Equals(this.contentType))
					{
						gc = new GUIContent(DataHolder.Effects().GetIcon(c.effect[i].realID));
					}
					else if(HUDContentType.BOTH.Equals(this.contentType))
					{
						gc = DataHolder.Effects().GetContent(c.effect[i].realID);
					}
					if(gc != null)
					{
						Rect rect = new Rect(this.bounds.x+cellWidth*currentColumn+this.spacing*currentColumn, 
								this.bounds.y+cellHeight*currentRow+this.spacing*currentRow, cellWidth, cellHeight);
						
						Vector2 size = Vector2.zero;
						float gap = 0;
						if("" != gc.text)
						{
							size = font.GetTextSize(gc.text);
							if(gc.image != null)
							{
								gap = font.GetTextSize(" ").x;
								size.x += gap;
							}
						}
						if(gc.image != null)
						{
							size.x += gc.image.width;
							if(size.y < gc.image.height) size.y = gc.image.height;
						}
						
						rect = this.GetAnchoredRect(pos, size, rect);
						if(gc.image != null)
						{
							Texture2D tex = gc.image as Texture2D;
							hudTexture = TextureDrawer.AddTexture(hudTexture,
									new Rect(rect.x, rect.y, gc.image.width, gc.image.height),
									tex.GetPixels());
							rect.x += gc.image.width+gap;
							rect.width -= gc.image.width+gap;
						}
						if("" != gc.text)
						{
							hudTexture = font.AddTextTexture(hudTexture, gc.text, rect, 
								DataHolder.Color(this.textColor), DataHolder.Color(this.shadowColor), 0, 
								Vector2.zero, this.showShadow, this.shadowOffset, false);
						}
						currentColumn++;
					}
					if(currentColumn >= this.columns)
					{
						currentColumn = 0;
						currentRow++;
					}
				}
			}
		}
		else if(HUDElementType.VARIABLE.Equals(this.type))
		{
			string txt = this.GetVariableText();
			if(txt != null && txt != "")
			{
				hudTexture = font.AddTextTexture(hudTexture, txt, 
						this.GetAnchoredRect(pos, font.GetTextSize(txt), this.bounds), 
						DataHolder.Color(this.textColor), DataHolder.Color(this.shadowColor), 0, 
						Vector2.zero, this.showShadow, this.shadowOffset, false);
			}
			this.lastVarText = txt;
		}
		return hudTexture;
	}
	
	public bool Changed(Character c, int hudID)
	{
		bool change = false;
		if(HUDElementType.STATUS.Equals(this.type) && 
			c.status[this.statusID].lastValueHUD[hudID] != c.status[this.statusID].GetValue())
		{
			change = true;
		}
		else if(HUDElementType.TIMEBAR.Equals(this.type) &&
			c.timeBarHUD[hudID] != c.timeBar)
		{
			change = true;
		}
		else if(HUDElementType.USED_TIMEBAR.Equals(this.type) &&
			c.usedTimeBarHUD[hudID] != c.usedTimeBar)
		{
			change = true;
		}
		else if(HUDElementType.CASTTIME.Equals(this.type) &&
			c.castTimeHUD[hudID] != c.GetSkillCastTime())
		{
			change = true;
		}
		else if(HUDElementType.EFFECT.Equals(this.type) && c.effectHUD)
		{
			change = true;
		}
		else if(HUDElementType.VARIABLE.Equals(this.type) && this.lastVarText != this.GetVariableText())
		{
			change = true;
		}
		return change;
	}
}