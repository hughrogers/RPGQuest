
using UnityEngine;
using System.Collections;

public class GUIHandler : MonoBehaviour
{
	private Material material;
	private GameObject camObject;
	private Camera cam;
	
	private LayerMask UILayer = 1 << 31;
	
	private Vector2 lastScreenSize;
	private Vector2 ratio;
	
	private ArrayList spriteList = new ArrayList();
	
	void Awake()
	{
		DontDestroyOnLoad(transform);
		gameObject.layer = 31;
	}
	
	void Start()
	{
		transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
		
		camObject = new GameObject();
		camObject.transform.parent = gameObject.transform;
		camObject.AddComponent("Camera");

		cam = camObject.camera;
		cam.name = "GUICamera";
		cam.clearFlags = CameraClearFlags.Depth;
		cam.nearClipPlane = 0.3f;
		cam.farClipPlane = 50.0f;
		cam.depth = 100;
		cam.rect = new Rect( 0.0f, 0.0f, 1.0f, 1.0f );
		cam.orthographic = true;
		cam.orthographicSize = Screen.height/2;
		cam.transform.position = new Vector3(0, -1, 0);
		cam.transform.LookAt(gameObject.transform);
		cam.cullingMask = UILayer;
		
		this.ratio = new Vector2(Screen.width / DataHolder.GameSettings().defaultScreen.x,
					Screen.height / DataHolder.GameSettings().defaultScreen.y);
	}
	
	void LateUpdate()
	{
		if(this.lastScreenSize.x != Screen.width || this.lastScreenSize.y != Screen.height)
		{
			this.UpdateGUISize();
		}
	}
	
	public void SetMaterial(Material m)
	{
		this.material = m;
	}
	
	public Material GetMaterial()
	{
		return this.material;
	}
	
	public void UpdateGUISize()
	{
		this.lastScreenSize = new Vector2(Screen.width, Screen.height);
		cam.orthographicSize = Screen.height/2;
		this.ratio = new Vector2(Screen.width / DataHolder.GameSettings().defaultScreen.x,
					Screen.height / DataHolder.GameSettings().defaultScreen.y);
		
		foreach(BaseSprite sprite in this.spriteList)
		{
			sprite.UpdateGUISize(true, this.ratio);
		}
	}
	
	public Vector2 GetScreenRatio()
	{
		return this.ratio;
	}
	
	
	public Vector2 WorldToScreenPoint(Vector3 pos)
	{
		pos = this.cam.WorldToScreenPoint(pos);
		return new Vector2(pos.x/this.ratio.x, (Screen.height-pos.y)/this.ratio.y);
	}
	
	/*
	============================================================================
	Sprite functions
	============================================================================
	*/
	public BaseSprite AddSprite(BaseSprite sprite)
	{
		sprite.material = this.material;
		sprite.spriteID = this.spriteList.Count;
		this.spriteList.Add(sprite);
		return sprite;
	}
	
	public DPSprite AddDPSprite(DialoguePosition dp)
	{
		GameObject tmp = GameObject.CreatePrimitive(PrimitiveType.Plane);
		tmp.name = "_DPSprite";
		DPSprite sprite = (DPSprite)tmp.AddComponent("DPSprite");
		sprite.dialoguePosition = dp;
		return (DPSprite)this.AddSprite(sprite);
	}
	
	public void AddScreenFadeSprite(ScreenFader sf)
	{
		GameObject tmp = GameObject.CreatePrimitive(PrimitiveType.Plane);
		tmp.name = "_ScreenFadeSprite";
		ScreenFadeSprite sprite = (ScreenFadeSprite)tmp.AddComponent("ScreenFadeSprite");
		sprite.screenFader = sf;
		this.AddSprite(sprite);
	}
	
	public void AddBattleTextSprite(BattleText bt)
	{
		GameObject tmp = GameObject.CreatePrimitive(PrimitiveType.Plane);
		tmp.name = "_BattleTextSprite";
		BattleTextSprite sprite = (BattleTextSprite)tmp.AddComponent("BattleTextSprite");
		sprite.battleText = bt;
		this.AddSprite(sprite);
	}
	
	public DragSprite AddDragSprite(ChoiceContent cc)
	{
		DragSprite sprite = null;
		if(cc != null)
		{
			GameObject tmp = GameObject.CreatePrimitive(PrimitiveType.Plane);
			tmp.name = "_DragSprite";
			sprite = (DragSprite)tmp.AddComponent("DragSprite");
			sprite.drag = cc;
			this.AddSprite(sprite);
		}
		return sprite;
	}
	
	public void AddHUDSprite(HUD hud)
	{
		GameObject tmp = GameObject.CreatePrimitive(PrimitiveType.Plane);
		tmp.name = "_HUDSprite";
		HUDSprite sprite = (HUDSprite)tmp.AddComponent("HUDSprite");
		sprite.hud = hud;
		this.AddSprite(sprite);
	}
	
	public ImageSprite AddImageSprite(Texture2D t, Rect b)
	{
		GameObject tmp = GameObject.CreatePrimitive(PrimitiveType.Plane);
		tmp.name = "_ImageSprite";
		ImageSprite sprite = (ImageSprite)tmp.AddComponent("ImageSprite");
		sprite.texture = t;
		sprite.bounds = b;
		return (ImageSprite)this.AddSprite(sprite);
	}
	
	public void RemoveSprite(BaseSprite del)
	{
		this.spriteList.Remove(del);
		int i=0;
		foreach(BaseSprite sprite in this.spriteList)
		{
			sprite.spriteID = i;
			sprite.UpdateTextures();
			i++;
		}
	}
	
	public void FocusSprite(BaseSprite focus)
	{
		if(focus != null)
		{
			bool change = true;
			if(focus is DPSprite)
			{
				((DPSprite)focus).dialoguePosition.SetFocus();
				if(!((DPSprite)focus).dialoguePosition.focusable) change = false;
			}
			if(change)
			{
				this.spriteList.Remove(focus);
				this.spriteList.Insert(0, focus);
			}
		}
		int i=0;
		foreach(BaseSprite sprite in this.spriteList)
		{
			sprite.spriteID = i;
			sprite.UpdateTextures();
			i++;
		}
	}
	
	/*
	============================================================================
	Interact functions
	============================================================================
	*/
	public BaseSprite ClickOnGUI(Vector2 mousePosition)
	{
		BaseSprite clicked = null;
		foreach(BaseSprite sprite in this.spriteList)
		{
			if(sprite.IsClicked(mousePosition, this.ratio))
			{
				clicked = sprite;
				break;
			}
		}
		if(clicked == null) GameHandler.WindowHandler().SetFocusID(-1);
		if(!GameHandler.WindowHandler().IsFocusBlocked()) this.FocusSprite(clicked);
		return clicked;
	}
}
