
using UnityEngine;
using System.Collections;

public class CameraControlSettings
{
	public CameraControlType type = CameraControlType.NONE;
	
	// follow
	public float distance = 10.0f;
	public float height = 5.0f;
	public float heightDamping = 2.0f;
	public float rotationDamping = 3.0f;
	
	// look
	public float damping = 6.0f;
	public bool smooth = true;
	
	// mobile
	public float minHeight = 5.0f;
	public float maxHeight = 30.0f;
	public bool allowRotation = true;
	public bool allowZoom = true;
	public float rotation = 0;
	public float rotationFactor = 1;
	public float zoomFactor = 1;
	public MouseTouchControl mouseTouch = new MouseTouchControl(false, 1, true, 2, 1);
	public string zoomPlusKey = "";
	public string zoomMinusKey = "";
	public float zoomKeyChange = 3;
	
	// first person
	public string onChild = "";
	public Vector3 offset = Vector3.zero;
	public string horizontalAxis = "";
	public string verticalAxis = "";
	public Vector2 sensitivity = new Vector2(15, 15);
	public bool lockCursor = false;
	
	// XML
	private static string ZOOMPLUSKEY = "zoompluskey";
	private static string ZOOMMINUSKEY = "zoomminuskey";
	private static string ONCHILD = "onchild";
	private static string HORIZONTALAXIS = "horizontalaxis";
	private static string VERTICALAXIS = "verticalaxis";
	
	public CameraControlSettings()
	{
		
	}
	
	public bool IsFollowControl()
	{
		return CameraControlType.FOLLOW.Equals(this.type);
	}
	
	public bool IsLookControl()
	{
		return CameraControlType.LOOK.Equals(this.type);
	}
	
	public bool IsMobileControl()
	{
		return CameraControlType.MOBILE.Equals(this.type);
	}
	
	public bool IsFirstPersonControl()
	{
		return CameraControlType.FIRST_PERSON.Equals(this.type);
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		ht.Add("type", this.type.ToString());
		if(this.IsFollowControl())
		{
			ht.Add("distance", this.distance.ToString());
			ht.Add("height", this.height.ToString());
			ht.Add("heightdamping", this.heightDamping.ToString());
			ht.Add("rotationdamping", this.rotationDamping.ToString());
		}
		else if(this.IsLookControl())
		{
			ht.Add("damping", this.damping.ToString());
			ht.Add("smooth", this.smooth.ToString());
		}
		else if(this.IsMobileControl())
		{
			ht.Add("distance", this.distance.ToString());
			ht.Add("height", this.height.ToString());
			ht.Add("minheight", this.minHeight.ToString());
			ht.Add("maxheight", this.maxHeight.ToString());
			ht.Add("heightdamping", this.heightDamping.ToString());
			ht.Add("allowrotation", this.allowRotation.ToString());
			ht.Add("allowzoom", this.allowZoom.ToString());
			ht.Add("rotation", this.rotation.ToString());
			ht.Add("rotationdamping", this.rotationDamping.ToString());
			ht.Add("rotationfactor", this.rotationFactor.ToString());
			ht.Add("zoomfactor", this.zoomFactor.ToString());
			ht = this.mouseTouch.GetData(ht);
			
			ht.Add("zoomkeychange", this.zoomKeyChange.ToString());
			ArrayList s = new ArrayList();
			if(this.zoomPlusKey != "")
			{
				s.Add(HashtableHelper.GetContentHashtable(CameraControlSettings.ZOOMPLUSKEY, this.zoomPlusKey));
			}
			if(this.zoomMinusKey != "")
			{
				s.Add(HashtableHelper.GetContentHashtable(CameraControlSettings.ZOOMMINUSKEY, this.zoomMinusKey));
			}
			if(s.Count > 0) ht.Add(XMLHandler.NODES, s);
		}
		else if(this.IsFirstPersonControl())
		{
			VectorHelper.ToHashtable(ref ht, this.offset, "ox", "oy", "oz");
			VectorHelper.ToHashtable(ref ht, this.sensitivity, "sx", "sy");
			if(this.lockCursor) ht.Add("lockcursor", "true");
			
			ArrayList s = new ArrayList();
			if(this.onChild != "")
			{
				s.Add(HashtableHelper.GetContentHashtable(CameraControlSettings.ONCHILD, this.onChild));
			}
			if(this.horizontalAxis != "")
			{
				s.Add(HashtableHelper.GetContentHashtable(CameraControlSettings.HORIZONTALAXIS, this.horizontalAxis));
			}
			if(this.verticalAxis != "")
			{
				s.Add(HashtableHelper.GetContentHashtable(CameraControlSettings.VERTICALAXIS, this.verticalAxis));
			}
			if(s.Count > 0) ht.Add(XMLHandler.NODES, s);
		}
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		if(ht.ContainsKey("type"))
		{
			this.type = (CameraControlType)System.Enum.Parse(
					typeof(CameraControlType), (string)ht["type"]);
			if(this.IsFollowControl())
			{
				this.distance = float.Parse((string)ht["distance"]);
				this.height = float.Parse((string)ht["height"]);
				this.heightDamping = float.Parse((string)ht["heightdamping"]);
				this.rotationDamping = float.Parse((string)ht["rotationdamping"]);
			}
			else if(this.IsLookControl())
			{
				this.damping = float.Parse((string)ht["damping"]);
				this.smooth = bool.Parse((string)ht["smooth"]);
			}
			else if(this.IsMobileControl())
			{
				this.distance = float.Parse((string)ht["distance"]);
				this.height = float.Parse((string)ht["height"]);
				this.minHeight = float.Parse((string)ht["minheight"]);
				this.maxHeight = float.Parse((string)ht["maxheight"]);
				this.heightDamping = float.Parse((string)ht["heightdamping"]);
				this.allowRotation = bool.Parse((string)ht["allowrotation"]);
				this.allowZoom = bool.Parse((string)ht["allowzoom"]);
				this.rotation = float.Parse((string)ht["rotation"]);
				this.mouseTouch.SetData(ht);
				if(ht.ContainsKey("rotationdamping"))
				{
					this.rotationDamping = float.Parse((string)ht["rotationdamping"]);
				}
				if(ht.ContainsKey("rotationfactor"))
				{
					this.rotationFactor = float.Parse((string)ht["rotationfactor"]);
				}
				if(ht.ContainsKey("zoomfactor"))
				{
					this.zoomFactor = float.Parse((string)ht["zoomfactor"]);
				}
				if(ht.ContainsKey("touchfingers"))
				{
					this.mouseTouch.useTouch = true;
					this.mouseTouch.touchCount = int.Parse((string)ht["touchfingers"]);
				}
				
				if(ht.ContainsKey("zoomkeychange"))
				{
					this.zoomKeyChange = float.Parse((string)ht["zoomkeychange"]);
				}
				if(ht.ContainsKey(XMLHandler.NODES))
				{
					ArrayList s = ht[XMLHandler.NODES] as ArrayList;
					foreach(Hashtable ht2 in s)
					{
						if(ht2[XMLHandler.NODE_NAME] as string == CameraControlSettings.ZOOMPLUSKEY)
						{
							this.zoomPlusKey = ht2[XMLHandler.CONTENT] as string;
						}
						else if(ht2[XMLHandler.NODE_NAME] as string == CameraControlSettings.ZOOMMINUSKEY)
						{
							this.zoomMinusKey = ht2[XMLHandler.CONTENT] as string;
						}
					}
				}
			}
			else if(this.IsFirstPersonControl())
			{
				this.offset = VectorHelper.FromHashtable(ht, "ox", "oy", "oz");
				this.sensitivity = VectorHelper.FromHashtable(ht, "sx", "sy");
				if(ht.ContainsKey("lockcursor")) this.lockCursor = true;
				
				if(ht.ContainsKey(XMLHandler.NODES))
				{
					ArrayList s = ht[XMLHandler.NODES] as ArrayList;
					foreach(Hashtable ht2 in s)
					{
						if(ht2[XMLHandler.NODE_NAME] as string == CameraControlSettings.ONCHILD)
						{
							this.onChild = ht2[XMLHandler.CONTENT] as string;
						}
						else if(ht2[XMLHandler.NODE_NAME] as string == CameraControlSettings.HORIZONTALAXIS)
						{
							this.horizontalAxis = ht2[XMLHandler.CONTENT] as string;
						}
						else if(ht2[XMLHandler.NODE_NAME] as string == CameraControlSettings.VERTICALAXIS)
						{
							this.verticalAxis = ht2[XMLHandler.CONTENT] as string;
						}
					}
				}
			}
		}
	}
	
	/*
	============================================================================
	Ingame functions
	============================================================================
	*/
	public void AddCameraControl(GameObject camera)
	{
		NoCameraControl check = camera.GetComponent<NoCameraControl>();
		if(check == null)
		{
			if(this.IsFollowControl())
			{
				SmoothFollow comp = camera.GetComponent<SmoothFollow>();
				if(comp == null)
				{
					comp = camera.AddComponent<SmoothFollow>();
				}
				comp.distance = this.distance;
				comp.height = this.height;
				comp.heightDamping = this.heightDamping;
				comp.rotationDamping = this.rotationDamping;
			}
			else if(this.IsLookControl())
			{
				SmoothLookAt comp = camera.GetComponent<SmoothLookAt>();
				if(comp == null)
				{
					comp = camera.AddComponent<SmoothLookAt>();
				}
				comp.damping = this.damping;
				comp.smooth = this.smooth;
			}
			else if(this.IsMobileControl())
			{
				TouchCamera comp = camera.GetComponent<TouchCamera>();
				if(comp == null)
				{
					comp = camera.AddComponent<TouchCamera>();
				}
				comp.distance = this.distance;
				comp.height = this.height;
				comp.minHeight = this.minHeight;
				comp.maxHeight = this.maxHeight;
				comp.heightDamping = this.heightDamping;
				comp.allowRotation = this.allowRotation;
				comp.allowZoom = this.allowZoom;
				comp.rotation = this.rotation;
				comp.rotationDamping = this.rotationDamping;
				comp.rotationFactor = this.rotationFactor;
				comp.zoomFactor = this.zoomFactor;
				comp.mouseTouch = this.mouseTouch;
				comp.zoomPlusKey = this.zoomPlusKey;
				comp.zoomMinusKey = this.zoomMinusKey;
				comp.zoomKeyChange = this.zoomKeyChange;
			}
			else if(this.IsFirstPersonControl())
			{
				FirstPersonCamera comp = camera.GetComponent<FirstPersonCamera>();
				if(comp == null)
				{
					comp = camera.AddComponent<FirstPersonCamera>();
				}
				comp.onChild = this.onChild;
				comp.offset = this.offset;
				comp.horizontalAxis = this.horizontalAxis;
				comp.verticalAxis = this.verticalAxis;
				comp.sensitivity = this.sensitivity;
				comp.lockCursor = this.lockCursor;
			}
		}
	}
	
	public void RemoveCameraControl(GameObject camera)
	{
		NoCameraControl check = camera.GetComponent<NoCameraControl>();
		if(check == null)
		{
			if(this.IsFollowControl())
			{
				SmoothFollow comp = camera.GetComponent<SmoothFollow>();
				if(comp != null) GameObject.Destroy(comp);
			}
			else if(this.IsLookControl())
			{
				SmoothLookAt comp = camera.GetComponent<SmoothLookAt>();
				if(comp != null) GameObject.Destroy(comp);
			}
			else if(this.IsMobileControl())
			{
				TouchCamera comp = camera.GetComponent<TouchCamera>();
				if(comp != null) GameObject.Destroy(comp);
			}
			else if(this.IsFirstPersonControl())
			{
				FirstPersonCamera comp = camera.GetComponent<FirstPersonCamera>();
				if(comp != null) GameObject.Destroy(comp);
			}
		}
	}
}
