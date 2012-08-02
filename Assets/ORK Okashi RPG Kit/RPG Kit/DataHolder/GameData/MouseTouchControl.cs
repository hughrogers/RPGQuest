
using UnityEngine;
using System.Collections;

public class MouseTouchControl
{
	public bool useClick = false;
	public int mouseClick = 0;
	public bool useTouch = false;
	public int touchCount = 1;
	
	public int clickCount = 1;
	
	public MouseTouch mode = MouseTouch.START;
	
	// ingame
	private int count = 0;
	private float lastClickTime = 0;
	
	private Vector3 lastPosition = Vector3.zero;
	private Vector3 lastChange = Vector3.zero;
	private bool isPressed = false;
	
	private int maxTouches = 0;
	
	public MouseTouchControl()
	{
		
	}
	
	public MouseTouchControl(bool active)
	{
		this.useClick = active;
		this.useTouch = active;
	}
	
	public MouseTouchControl(bool uc, int mc, bool ut, int tc, int cc)
	{
		this.useClick = uc;
		this.mouseClick = mc;
		this.useTouch = ut;
		this.touchCount = tc;
		this.clickCount = cc;
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		if(this.useClick)
		{
			ht.Add("mouseclick", this.mouseClick.ToString());
		}
		if(this.useTouch)
		{
			ht.Add("touchcount", this.touchCount.ToString());
		}
		if(this.useClick || this.useTouch)
		{
			ht.Add("clickcount", this.clickCount.ToString());
			ht.Add("mode", this.mode.ToString());
		}
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		if(ht.ContainsKey("mouseclick"))
		{
			this.useClick = true;
			this.mouseClick = int.Parse((string)ht["mouseclick"]);
		}
		else this.useClick = false;
		if(ht.ContainsKey("touchcount"))
		{
			this.useTouch = true;
			this.touchCount = int.Parse((string)ht["touchcount"]);
		}
		else this.useTouch = false;
		if(ht.ContainsKey("clickcount"))
		{
			this.clickCount = int.Parse((string)ht["clickcount"]);
		}
		if(ht.ContainsKey("mode"))
		{
			this.mode = (MouseTouch)System.Enum.Parse(typeof(MouseTouch), (string)ht["mode"]);
		}
	}
	
	/*
	============================================================================
	Control handling functions
	============================================================================
	*/
	public bool Active()
	{
		return this.useClick || this.useTouch;
	}
	
	/*
	============================================================================
	Check functions
	============================================================================
	*/
	private bool CheckTouchBegan()
	{
		bool began = false;
		for(int i=0; i<this.touchCount; i++)
		{
			if(Input.touches[i].phase == TouchPhase.Began)
			{
				began = true;
				break;
			}
		}
		return began;
	}
	
	private bool CheckTouchMoved()
	{
		bool moved = false;
		for(int i=0; i<this.touchCount; i++)
		{
			if(Input.touches[i].phase == TouchPhase.Moved)
			{
				moved = true;
				break;
			}
		}
		return moved;
	}
	
	private bool CheckTouchEnded()
	{
		bool ended = false;
		for(int i=0; i<this.touchCount; i++)
		{
			if(Input.touches[i].phase == TouchPhase.Ended)
			{
				ended = true;
				break;
			}
		}
		return ended;
	}
	
	/*
	============================================================================
	Control handling functions
	============================================================================
	*/
	public Vector3 GetLastChange()
	{
		return this.lastChange;
	}
	
	public bool Interacted(ref Vector3 point)
	{
		bool interacted = false;
		
		if(Input.touchCount == 0)
		{
			this.maxTouches = 0;
		}
		else if(Input.touchCount > this.maxTouches)
		{
			this.maxTouches = Input.touchCount;
		}
		
		if(MouseTouch.START.Equals(this.mode))
		{
			interacted = this.Started(ref point);
		}
		else if(MouseTouch.MOVE.Equals(this.mode))
		{
			this.Started(ref point);
			this.Ended(ref point);
			interacted = this.Moved(ref point);
		}
		else if(MouseTouch.END.Equals(this.mode))
		{
			interacted = this.Ended(ref point);
		}
		
		return interacted;
	}
	
	private bool Started(ref Vector3 point)
	{
		bool started = false;
		
		if((Time.time-this.lastClickTime) > DataHolder.GameSettings().clickTimeout)
		{
			this.count = 0;
		}
		
		if(this.useClick && Input.GetMouseButtonDown(this.mouseClick))
		{
			point = Input.mousePosition;
			started = true;
			this.count++;
			this.lastClickTime = Time.time;
		}
		else if(this.useTouch && this.touchCount > 0 && 
			Input.touchCount == this.touchCount && 
			this.maxTouches == this.touchCount && 
			this.CheckTouchBegan())
		{
			point = VectorHelper.GetTouchPosition(this.touchCount);
			started = true;
			this.count++;
			this.lastClickTime = Time.time;
		}
		if(started && this.clickCount != count)
		{
			started = false;
		}
		if(started)
		{
			if(GameHandler.WindowHandler().IsInGUI(point))
			{
				started = false;
			}
		}
		if(started)
		{
			this.lastPosition = point;
			this.isPressed = true;
		}
		return started;
	}
	
	private bool Moved(ref Vector3 point)
	{
		bool moved = false;
		
		if(this.isPressed)
		{
			if(this.useClick && Input.GetMouseButton(this.mouseClick))
			{
				point = Input.mousePosition;
				this.lastChange = Input.mousePosition-this.lastPosition;
				this.lastPosition = Input.mousePosition;
				moved = true;
			}
			else if(this.useTouch && this.touchCount > 0 && 
				Input.touchCount == this.touchCount && 
				this.maxTouches == this.touchCount && 
				this.CheckTouchMoved())
			{
				point = VectorHelper.GetTouchPosition(this.touchCount);
				this.lastChange = VectorHelper.GetTouchMove(this.touchCount);
				moved = true;
			}
			
			if(moved && point == Vector3.zero)
			{
				moved = false;
			}
		}
		
		return moved;
	}
	
	private bool Ended(ref Vector3 point)
	{
		bool ended = false;
		
		if(this.useClick && Input.GetMouseButtonUp(this.mouseClick))
		{
			point = Input.mousePosition;
			ended = true;
		}
		else if(this.useTouch && this.touchCount > 0 && 
			Input.touchCount == this.touchCount && 
			this.maxTouches == this.touchCount && 
			this.CheckTouchEnded())
		{
			point = VectorHelper.GetTouchPosition(this.touchCount);
			ended = true;
		}
		if(ended) this.isPressed = false;
		return ended;
	}
}
