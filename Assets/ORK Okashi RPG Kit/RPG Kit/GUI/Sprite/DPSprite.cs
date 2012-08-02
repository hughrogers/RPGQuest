
using UnityEngine;
using System.Collections;

public class DPSprite : BaseSprite
{
	public DialoguePosition dialoguePosition;
	
	// interact
	protected Vector3 clickPosition;
	protected Vector2 initialClickPosition;
	protected bool doDrag = false;
	protected bool doScroll = false;
	protected bool doBarScroll = false;
	
	// content
	protected DPSprite contentSprite;
	protected DPSprite vScrollSprite;
	protected DPSprite nameSprite;
	protected DPSprite okSprite;
	protected DPSprite speakerSprite;
	
	/*
	============================================================================
	Creation functions
	============================================================================
	*/
	void Start()
	{	
		this.InitMesh();
		if(this.dialoguePosition.fadeIn) this.startColor.a = 0;
		if(gameObject.name == "_DPSprite")
		{
			this.AddContent();
			this.AddName();
			this.AddOk();
			this.AddSpeaker();
			this.dialoguePosition.InitIn();
			this.renderer.material.mainTexture = this.dialoguePosition.multiLabel.GetBackgroundTexture();
		}
		else if(gameObject.name == "_Content")
		{
			this.renderer.material.mainTexture = this.dialoguePosition.multiLabel.GetContentTexture();
			this.renderer.material.mainTextureOffset = this.dialoguePosition.scrollSize;
		}
		else if(gameObject.name == "_VScroll")
		{
			this.renderer.material.mainTexture = this.dialoguePosition.multiLabel.GetVScrollTexture();
		}
		else if(gameObject.name == "_Name")
		{
			this.renderer.material.mainTexture = this.dialoguePosition.multiLabel.GetNameTexture();
		}
		else if(gameObject.name == "_Ok")
		{
			this.renderer.material.mainTexture = this.dialoguePosition.multiLabel.GetOkTexture();
		}
		else if(gameObject.name == "_Speaker")
		{
			this.renderer.material.mainTexture = this.dialoguePosition.multiLabel.speakerPortrait.image;
		}
		
		this.UpdateGUISize(false, GameHandler.GUIHandler().GetScreenRatio());
	}
	
	private DPSprite AddChild(string name)
	{
		GameObject tmp = GameObject.CreatePrimitive(PrimitiveType.Plane);
		tmp.transform.position = this.transform.position;
		tmp.transform.parent = this.transform;
		tmp.name = name;
		DPSprite sprite = (DPSprite)tmp.AddComponent("DPSprite");
		sprite.material = this.material;
		sprite.dialoguePosition = this.dialoguePosition;
		return sprite;
	}
	
	public void AddContent()
	{
		if(this.dialoguePosition.multiLabel.HasContentTexture())
		{
			this.contentSprite = this.AddChild("_Content");
			if(this.dialoguePosition.multiLabel.HasVScrollTexture())
			{
				this.vScrollSprite = this.AddChild("_VScroll");
			}
		}
	}
	
	public void AddName()
	{
		if(this.dialoguePosition.multiLabel.HasNameTexture())
		{
			this.nameSprite = this.AddChild("_Name");
		}
	}
	
	public void AddOk()
	{
		if(this.dialoguePosition.multiLabel.HasOkTexture())
		{
			this.okSprite = this.AddChild("_Ok");
		}
	}
	
	public void AddSpeaker()
	{
		if(this.dialoguePosition.multiLabel.speakerPortrait != null)
		{
			this.speakerSprite = this.AddChild("_Speaker");
		}
	}
	
	/*
	============================================================================
	Color functions
	============================================================================
	*/
	public void SetBGColor(Color c)
	{
		if(gameObject.name == "_DPSprite")
		{
			this.SetColor(c);
			if(this.vScrollSprite != null) this.vScrollSprite.SetColor(c);
			if(this.nameSprite != null) this.nameSprite.SetColor(c);
		}
	}
	
	public void SetFGColor(Color c)
	{
		if(gameObject.name == "_DPSprite")
		{
			if(this.contentSprite != null) this.contentSprite.SetColor(c);
			if(this.okSprite != null) this.okSprite.SetColor(c);
			if(this.speakerSprite != null) this.speakerSprite.SetColor(c);
		}
	}
	
	public void SetColor(Color c)
	{
		if(mesh != null)
		{
			Color[] colors = new Color[mesh.colors.Length];
			for(int i=0; i<colors.Length; i++)
			{
				colors[i] = new Color(c.r, c.g, c.b, this.dialoguePosition.alpha);
			}
			mesh.colors = colors;
		}
	}
	
	private bool CheckColors(Color c)
	{
		return mesh != null &&
			(mesh.colors[0].r != c.r ||
			mesh.colors[0].g != c.g ||
			mesh.colors[0].b != c.b ||
			mesh.colors[0].a != this.dialoguePosition.alpha);
	}
	
	/*
	============================================================================
	Update functions
	============================================================================
	*/
	void Update()
	{
		if(gameObject.name == "_DPSprite")
		{
			this.dialoguePosition.Tick(Time.deltaTime);
			if(this.dialoguePosition.IsOutDone())
			{
				this.RemoveSprite();
			}
			else
			{
				if(this.spriteID != 0 && this.dialoguePosition.IsFocused())
				{
					GameHandler.GUIHandler().FocusSprite(this);
				}
				else
				{
					if(this.CheckColors(this.dialoguePosition.bgColor))
					{
						this.SetBGColor(this.dialoguePosition.bgColor);
						this.SetFGColor(this.dialoguePosition.fgColor);
					}
				}
			}
		}
	}
	
	void LateUpdate()
	{
		if(gameObject.name == "_DPSprite")
		{
			Vector2 ratio = GameHandler.GUIHandler().GetScreenRatio();
			if(this.dialoguePosition.multiLabel.newTextures)
			{
				if(this.vScrollSprite == null && this.dialoguePosition.multiLabel.HasVScrollTexture())
				{
					this.vScrollSprite = this.AddChild("_VScroll");
				}
				else if(this.vScrollSprite != null && !this.dialoguePosition.multiLabel.HasVScrollTexture())
				{
					GameObject.Destroy(this.vScrollSprite.gameObject);
				}
				this.UpdateTextures();
				this.UpdateGUISize(true, ratio);
			}
			transform.position = new Vector3(
					-(Screen.width/2)+(this.dialoguePosition.currentPos.x+
						(this.dialoguePosition.boxBounds.width/2))*ratio.x,
					0+(this.spriteID/100.0f), 
					-(Screen.height/2)+(this.dialoguePosition.currentPos.y+
						(this.dialoguePosition.boxBounds.height/2))*ratio.y);
		}
		else if(gameObject.name == "_Content")
		{
			Vector2 ratio = GameHandler.GUIHandler().GetScreenRatio();
			transform.localPosition = new Vector3(0,
					-0.002f, (this.dialoguePosition.contentBounds.height/2)*ratio.y);
			this.renderer.material.mainTextureOffset = this.dialoguePosition.scrollSize;
		}
		else if(gameObject.name == "_VScroll")
		{
			Vector2 ratio = GameHandler.GUIHandler().GetScreenRatio();
			transform.localPosition = new Vector3((this.dialoguePosition.vScrollBounds.x/2)*ratio.x,
					-0.003f, 
					(this.dialoguePosition.vScrollTop-this.dialoguePosition.vScrollPixel*
					(this.dialoguePosition.scrollSize.y-this.dialoguePosition.scrollBoundTop))*ratio.y);
		}
		else if(gameObject.name == "_Name")
		{
			Vector2 ratio = GameHandler.GUIHandler().GetScreenRatio();
			if(this.dialoguePosition.isDragWindow)
			{
				transform.localPosition = new Vector3(0, -0.004f, 
						-((this.dialoguePosition.boxBounds.height-this.renderer.material.mainTexture.height)/2)*ratio.y);

			}
			else
			{
				transform.position = new Vector3(
						-(Screen.width/2)+(this.dialoguePosition.currentPos.x+
							this.dialoguePosition.nameOffset.x+
							(this.dialoguePosition.nameBounds.width/2))*ratio.x,
						transform.parent.position.y-0.004f, 
						-(Screen.height/2)+(this.dialoguePosition.currentPos.y+
							this.dialoguePosition.nameOffset.y+
							(this.dialoguePosition.nameBounds.height/2))*ratio.y);
			}
		}
		else if(gameObject.name == "_Ok")
		{
			Vector2 ratio = GameHandler.GUIHandler().GetScreenRatio();
			transform.localPosition = new Vector3(
					this.dialoguePosition.okButtonBounds.x*ratio.x,
					-0.005f, 
					this.dialoguePosition.okButtonBounds.y*ratio.y);
		}
		else if(gameObject.name == "_Speaker")
		{
			Vector2 ratio = GameHandler.GUIHandler().GetScreenRatio();
			if(this.dialoguePosition.multiLabel.speakerPortrait.inBox)
			{
				transform.position = new Vector3(
					-(Screen.width/2)+(this.dialoguePosition.currentPos.x+
						this.dialoguePosition.multiLabel.speakerPortrait.position.x+
						(this.renderer.material.mainTexture.width/2))*ratio.x,
					transform.parent.position.y-0.001f,
					-(Screen.height/2)+(this.dialoguePosition.currentPos.y+
						this.dialoguePosition.multiLabel.speakerPortrait.position.y+
						(this.renderer.material.mainTexture.height/2))*ratio.y);
			}
			else
			{
				transform.position = new Vector3(
					-(Screen.width/2)+(this.dialoguePosition.multiLabel.speakerPortrait.position.x+
						(this.renderer.material.mainTexture.width/2))*ratio.x,
					transform.parent.position.y+0.001f,
					-(Screen.height/2)+(this.dialoguePosition.multiLabel.speakerPortrait.position.y+
						(this.renderer.material.mainTexture.height/2))*ratio.y);
			}
		}
	}
	
	public override void UpdateGUISize(bool updateChilds, Vector2 ratio)
	{
		this.dialoguePosition.multiLabel.newTextures = false;
		if(this.renderer.material.mainTexture == null) return;
		
		if(gameObject.name == "_DPSprite" || gameObject.name == "_VScroll" || 
			gameObject.name == "_Name" || gameObject.name == "_Ok")
		{
			this.UpdateBaseMesh(ratio);
		}
		else if(gameObject.name == "_Speaker")
		{
			if(ratio.x > ratio.y) this.UpdateBaseMesh(new Vector2(ratio.x, ratio.x));
			else this.UpdateBaseMesh(new Vector2(ratio.y, ratio.y));
		}
		else if(gameObject.name == "_Content")
		{
			float w = (this.renderer.material.mainTexture.width/2)*ratio.x;
			float h = this.renderer.material.mainTexture.height*ratio.y;
			float h2 = this.dialoguePosition.contentBounds.height*ratio.y;
			
			Vector3[] vertices = new Vector3[6];
			Color[] colors = new Color[6];
			Vector2[] uvs = new Vector2[6];
			int[] triangles = new int[6];
			
			vertices[0] = new Vector3(-w, 0, -h2);
			vertices[1] = new Vector3(-w, 0, 0);
			vertices[2] = new Vector3(w, 0, -h2);
			vertices[3] = new Vector3(w, 0, 0);
			vertices[4] = new Vector3(-w, 0, h-h2);
			vertices[5] = new Vector3(w, 0, h-h2);
			
			float h3 = 1-(h-h2)/h;
			uvs[0] = new Vector2(0.0f, h3);
			uvs[1] = new Vector2(0.0f, 0.0f);
			uvs[2] = new Vector2(1.0f, h3);
			uvs[3] = new Vector2(1.0f, 0.0f);
			// hidden
			uvs[4] = new Vector2(0.0f, 1.0f);
			uvs[5] = new Vector2(1.0f, 1.0f);
			
			for(int i=0; i<6; i++)
			{
				colors[i] = this.startColor;
			}
			
			triangles[0] = 0;
			triangles[1] = 2;
			triangles[2] = 1;
			triangles[3] = 2;
			triangles[4] = 3;
			triangles[5] = 1;

			if(mesh != null)
			{
				mesh.Clear();
				mesh.vertices = vertices;
				mesh.colors = colors;
				mesh.uv = uvs;
				mesh.triangles = triangles;
			}
		}
		
		this.LateUpdate();
		if(updateChilds)
		{
			if(this.contentSprite != null)
			{
				this.contentSprite.UpdateGUISize(false, ratio);
				this.contentSprite.LateUpdate();
			}
			if(this.vScrollSprite != null)
			{
				this.vScrollSprite.UpdateGUISize(false, ratio);
				this.vScrollSprite.LateUpdate();
			}
			if(this.nameSprite != null)
			{
				this.nameSprite.UpdateGUISize(false, ratio);
				this.nameSprite.LateUpdate();
			}
			if(this.okSprite != null)
			{
				this.okSprite.UpdateGUISize(false, ratio);
				this.okSprite.LateUpdate();
			}
			if(this.speakerSprite != null)
			{
				this.speakerSprite.UpdateGUISize(false, ratio);
				this.speakerSprite.LateUpdate();
			}
		}
	}
	
	public override void UpdateTextures()
	{
		if(gameObject.name == "_DPSprite")
		{
			this.renderer.material.mainTexture = this.dialoguePosition.multiLabel.GetBackgroundTexture();
			if(this.contentSprite != null) this.contentSprite.UpdateTextures();
			if(this.vScrollSprite != null) this.vScrollSprite.UpdateTextures();
			if(this.nameSprite != null) this.nameSprite.UpdateTextures();
			if(this.okSprite != null) this.okSprite.UpdateTextures();
			if(this.speakerSprite != null) this.speakerSprite.UpdateTextures();
		}
		else if(gameObject.name == "_Content")
		{
			this.renderer.material.mainTexture = this.dialoguePosition.multiLabel.GetContentTexture();
		}
		else if(gameObject.name == "_VScroll")
		{
			this.renderer.material.mainTexture = this.dialoguePosition.multiLabel.GetVScrollTexture();
		}
		else if(gameObject.name == "_Name")
		{
			this.renderer.material.mainTexture = this.dialoguePosition.multiLabel.GetNameTexture();
		}
		else if(gameObject.name == "_Ok")
		{
			this.renderer.material.mainTexture = this.dialoguePosition.multiLabel.GetOkTexture();
		}
		else if(gameObject.name == "_Speaker")
		{
			this.renderer.material.mainTexture = this.dialoguePosition.multiLabel.speakerPortrait.image;
		}
	}
	
	void OnDestroy()
	{
		if(gameObject.name == "_DPSprite" && this.dialoguePosition.destroyTextures)
		{
			this.dialoguePosition.multiLabel.DeleteTextures();
		}
	}
	
	/*
	============================================================================
	Interact functions
	============================================================================
	*/
	public override bool IsClicked(Vector2 mousePosition, Vector2 ratio)
	{
		bool clicked = false;
		if(this.dialoguePosition.IsInWindow(mousePosition))
		{
			clicked = true;
			this.initialClickPosition = mousePosition;
			if(this.okSprite != null && this.dialoguePosition.IsInOkButton(mousePosition))
			{
				// do nothing
			}
			else if(this.dialoguePosition.IsInDrag(mousePosition))
			{
				this.clickPosition = Input.mousePosition;
				this.clickPosition.x -= this.dialoguePosition.currentPos.x*ratio.x;
				this.clickPosition.y += this.dialoguePosition.currentPos.y*ratio.y;
				this.doDrag = true;
			}
			else if(this.vScrollSprite != null && this.dialoguePosition.IsInVScroll(mousePosition, 
					GameHandler.GUIHandler().WorldToScreenPoint(this.vScrollSprite.transform.position)))
			{
				this.clickPosition = Input.mousePosition;
				this.doBarScroll = true;
			}
			else if(this.contentSprite != null && this.vScrollSprite != null && this.dialoguePosition.IsAboveVScroll(mousePosition,
					GameHandler.GUIHandler().WorldToScreenPoint(this.vScrollSprite.transform.position)))
			{
				this.dialoguePosition.SetScroll(this.dialoguePosition.scrollViewHeight/this.dialoguePosition.scrollPixelVertical);
			}
			else if(this.contentSprite != null && this.vScrollSprite != null && this.dialoguePosition.IsBelowVScroll(mousePosition,
					GameHandler.GUIHandler().WorldToScreenPoint(this.vScrollSprite.transform.position)))
			{
				this.dialoguePosition.SetScroll(-this.dialoguePosition.scrollViewHeight/this.dialoguePosition.scrollPixelVertical);
			}
			else if(this.contentSprite != null && this.dialoguePosition.IsInContent(mousePosition))
			{
				ChoiceContent cc = this.dialoguePosition.multiLabel.CheckDrag(mousePosition);
				// no drag
				if(cc == null)
				{
					this.clickPosition = Input.mousePosition;
					this.doScroll = true;
				}
				// drag
				else
				{
					GameHandler.DragHandler().SetDrag(cc);
				}
			}
		}
		else if(this.okSprite != null && this.dialoguePosition.IsInOkButton(mousePosition))
		{
			clicked = true;
			this.initialClickPosition = mousePosition;
		}
		return clicked;
	}
	
	public override void ReleaseClick(Vector2 mousePosition)
	{
		if(Vector2.Distance(this.initialClickPosition, mousePosition) < DataHolder.GameSettings().maxClickDistance &&
			this.dialoguePosition.CanControl())
		{
			// check ok button
			if(this.okSprite != null && this.dialoguePosition.IsInOkButton(mousePosition))
			{
				GameHandler.GetLevelHandler().PressOk();
			}
			// check for choice
			else if(this.contentSprite != null && this.dialoguePosition.IsInContent(mousePosition))
			{
				this.dialoguePosition.multiLabel.CheckChoice(mousePosition);
			}
		}
		this.doDrag = false;
		this.doScroll = false;
		this.doBarScroll = false;
	}
	
	public override void Drag()
	{
		if(this.doDrag)
		{
			Vector2 ratio = GameHandler.GUIHandler().GetScreenRatio();
			this.dialoguePosition.currentPos.x = (-this.clickPosition.x+Input.mousePosition.x)/ratio.x;
			this.dialoguePosition.currentPos.y = (this.clickPosition.y-Input.mousePosition.y)/ratio.y;
			if(DataHolder.GameSettings().saveWindowDrag)
			{
				if(this.dialoguePosition.currentPos.x < 0) this.dialoguePosition.currentPos.x = 0;
				else if((this.dialoguePosition.currentPos.x+this.dialoguePosition.boxBounds.width) > DataHolder.GameSettings().defaultScreen.x)
				{
					this.dialoguePosition.currentPos.x = DataHolder.GameSettings().defaultScreen.x-this.dialoguePosition.boxBounds.width;
				}
				if(this.dialoguePosition.currentPos.y < 0) this.dialoguePosition.currentPos.y = 0;
				else if((this.dialoguePosition.currentPos.y+this.dialoguePosition.boxBounds.height) > DataHolder.GameSettings().defaultScreen.y)
				{
					this.dialoguePosition.currentPos.y = DataHolder.GameSettings().defaultScreen.y-this.dialoguePosition.boxBounds.height;
				}
			}
		}
		else if(this.doScroll)
		{
			Vector2 ratio = GameHandler.GUIHandler().GetScreenRatio();
			this.dialoguePosition.SetScroll((this.clickPosition.y-Input.mousePosition.y)/ratio.y);
			this.clickPosition = Input.mousePosition;
		}
		else if(this.doBarScroll)
		{
			Vector2 ratio = GameHandler.GUIHandler().GetScreenRatio();
			this.dialoguePosition.SetScroll(-((this.clickPosition.y-Input.mousePosition.y)/ratio.y)*this.dialoguePosition.vScrollRatio);
			this.clickPosition = Input.mousePosition;
		}
	}
}
