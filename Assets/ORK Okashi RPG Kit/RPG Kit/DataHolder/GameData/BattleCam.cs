
using UnityEngine;
using System.Collections;

public class BattleCam
{
	public bool blockAnimationCams = false;
	public bool useCombatantCenter = false;
	public bool rememberPosition = false;
	
	public Vector3 rotationAxis = Vector3.zero;
	public float rotationSpeed = 0;
	
	public bool limitRotation = false;
	public Vector3 minRotation = Vector3.zero;
	public Vector3 maxRotation = Vector3.zero;
	
	public float lookAtDamping = 5.0f;
	
	// damage
	public bool lookAtLatestDamage = false;
	public string damageLookAtChild = "";
	
	public int[] latestDamageID = new int[0];
	public Vector3[] ldRotationAxis = new Vector3[0];
	public float[] ldRotationSpeed = new float[0];
	
	// user
	public bool lookAtLatestUser = false;
	public string userLookAtChild = "";
	
	public int[] latestUserID = new int[0];
	public Vector3[] luRotationAxis = new Vector3[0];
	public float[] luRotationSpeed = new float[0];
	
	// menu
	public bool lookAtMenuUser = false;
	public string menuLookAtChild = "";
	
	public int[] menuUserID = new int[0];
	public Vector3[] muRotationAxis = new Vector3[0];
	public float[] muRotationSpeed = new float[0];
	
	// selection
	public bool lookAtSelection = false;
	public string selectionLookAtChild = "";
	
	public int[] selectionID = new int[0];
	public Vector3[] sRotationAxis = new Vector3[0];
	public float[] sRotationSpeed = new float[0];
	
	public int[] noBlockAnimation = new int[0];
	
	private static string LATESTDAMAGE = "latestdamage";
	private static string LATESTUSER = "latestuser";
	private static string MENUUSER = "menuuser";
	private static string SELECTION = "selection";
	private static string NOBLOCK = "noblock";
	private static string DAMAGECHILD = "damagechild";
	private static string USERCHILD = "userchild";
	private static string MENUCHILD = "menuchild";
	private static string SELECTIONCHILD = "selectionchild";
	
	// ingame
	private bool lookingAtSomething = false;
	private bool forceReset = false;
	private int blockedByAnimation = 0;
	private Transform camera = null;
	private bool activated = false;
	
	// targets
	private int ldIndex = 0;
	private Transform ldTarget = null;
	
	private int luIndex = 0;
	private Transform luTarget = null;
	
	private int muIndex = 0;
	private Transform muTarget = null;
	
	private int sIndex = 0;
	private Transform sTarget = null;
	
	private BattleArena battleArena = null;
	
	private int lastLookAtIndex = -1;
	private Vector3 lastPos = Vector3.zero;
	private Quaternion lastRot = Quaternion.identity;
	
	private Vector3 useRotationAxis = Vector3.zero;
	private Vector3 startRotationAxis = Vector3.zero;
	
	public BattleCam()
	{
		
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		if(this.blockAnimationCams)
		{
			ArrayList s = new ArrayList();
			
			if(this.useCombatantCenter) ht.Add("combatantcenter", "true");
			if(this.rememberPosition) ht.Add("rememberpos", "true");
			VectorHelper.ToHashtable(ref ht, this.rotationAxis, "rx", "ry", "rz");
			if(this.limitRotation)
			{
				VectorHelper.ToHashtable(ref ht, this.minRotation, "minx", "miny", "minz");
				VectorHelper.ToHashtable(ref ht, this.maxRotation, "maxx", "maxy", "maxz");
			}
			ht.Add("speed", this.rotationSpeed.ToString());
			ht.Add("lookatdamping", this.lookAtDamping.ToString());
			
			if(this.lookAtLatestDamage)
			{
				ht.Add("lookatlatestdamage", "true");
				if(this.damageLookAtChild != "")
				{
					s.Add(HashtableHelper.GetContentHashtable(BattleCam.DAMAGECHILD, this.damageLookAtChild));
				}
			}
			else if(this.latestDamageID.Length > 0)
			{
				ht.Add("latestdamages", this.latestDamageID.Length.ToString());
				for(int i=0; i<this.latestDamageID.Length; i++)
				{
					Hashtable ht2 = HashtableHelper.GetTitleHashtable(BattleCam.LATESTDAMAGE, i);
					ht2.Add("campos", this.latestDamageID[i].ToString());
					ht2.Add("rx", this.ldRotationAxis[i].x.ToString());
					ht2.Add("ry", this.ldRotationAxis[i].y.ToString());
					ht2.Add("rz", this.ldRotationAxis[i].z.ToString());
					ht2.Add("speed", this.ldRotationSpeed[i].ToString());
					s.Add(ht2);
				}
			}
			if(this.lookAtLatestUser)
			{
				ht.Add("lookatlatestuser", "true");
				if(this.userLookAtChild != "")
				{
					s.Add(HashtableHelper.GetContentHashtable(BattleCam.USERCHILD, this.userLookAtChild));
				}
			}
			else if(this.latestUserID.Length > 0)
			{
				ht.Add("latestusers", this.latestUserID.Length.ToString());
				for(int i=0; i<this.latestUserID.Length; i++)
				{
					Hashtable ht2 = HashtableHelper.GetTitleHashtable(BattleCam.LATESTUSER, i);
					ht2.Add("campos", this.latestUserID[i].ToString());
					ht2.Add("rx", this.luRotationAxis[i].x.ToString());
					ht2.Add("ry", this.luRotationAxis[i].y.ToString());
					ht2.Add("rz", this.luRotationAxis[i].z.ToString());
					ht2.Add("speed", this.luRotationSpeed[i].ToString());
					s.Add(ht2);
				}
			}
			if(this.lookAtMenuUser)
			{
				ht.Add("lookatmenuuser", "true");
				if(this.menuLookAtChild != "")
				{
					s.Add(HashtableHelper.GetContentHashtable(BattleCam.MENUCHILD, this.menuLookAtChild));
				}
			}
			else if(this.menuUserID.Length > 0)
			{
				ht.Add("menuusers", this.menuUserID.Length.ToString());
				for(int i=0; i<this.menuUserID.Length; i++)
				{
					Hashtable ht2 = HashtableHelper.GetTitleHashtable(BattleCam.MENUUSER, i);
					ht2.Add("campos", this.menuUserID[i].ToString());
					ht2.Add("rx", this.muRotationAxis[i].x.ToString());
					ht2.Add("ry", this.muRotationAxis[i].y.ToString());
					ht2.Add("rz", this.muRotationAxis[i].z.ToString());
					ht2.Add("speed", this.muRotationSpeed[i].ToString());
					s.Add(ht2);
				}
			}
			if(this.lookAtSelection)
			{
				ht.Add("lookatselection", "true");
				if(this.selectionLookAtChild != "")
				{
					s.Add(HashtableHelper.GetContentHashtable(BattleCam.SELECTIONCHILD, this.selectionLookAtChild));
				}
			}
			else if(this.selectionID.Length > 0)
			{
				ht.Add("selections", this.selectionID.Length.ToString());
				for(int i=0; i<this.selectionID.Length; i++)
				{
					Hashtable ht2 = HashtableHelper.GetTitleHashtable(BattleCam.SELECTION, i);
					ht2.Add("campos", this.selectionID[i].ToString());
					ht2.Add("rx", this.sRotationAxis[i].x.ToString());
					ht2.Add("ry", this.sRotationAxis[i].y.ToString());
					ht2.Add("rz", this.sRotationAxis[i].z.ToString());
					ht2.Add("speed", this.sRotationSpeed[i].ToString());
					s.Add(ht2);
				}
			}
			
			if(this.noBlockAnimation.Length > 0)
			{
				ht.Add("noblocks", this.noBlockAnimation.Length.ToString());
				for(int i=0; i<this.noBlockAnimation.Length; i++)
				{
					Hashtable ht2 = HashtableHelper.GetTitleHashtable(BattleCam.NOBLOCK, i);
					ht2.Add("aid", this.noBlockAnimation[i].ToString());
					s.Add(ht2);
				}
			}
			if(s.Count > 0) ht.Add(XMLHandler.NODES, s);
		}
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		if(ht.ContainsKey("rx"))
		{
			this.blockAnimationCams = true;
			if(ht.ContainsKey("combatantcenter")) this.useCombatantCenter = true;
			if(ht.ContainsKey("rememberpos")) this.rememberPosition = true;
			
			this.rotationAxis = VectorHelper.FromHashtable(ht, "rx", "ry", "rz");
			if(ht.ContainsKey("minx"))
			{
				this.limitRotation = true;
				this.minRotation = VectorHelper.FromHashtable(ht, "minx", "miny", "minz");
			}
			if(ht.ContainsKey("maxx"))
			{
				this.limitRotation = true;
				this.maxRotation = VectorHelper.FromHashtable(ht, "maxx", "maxy", "maxz");
			}
			this.rotationSpeed = float.Parse((string)ht["speed"]);
			this.lookAtDamping = float.Parse((string)ht["lookatdamping"]);
			
			if(ht.ContainsKey("lookatlatestdamage")) this.lookAtLatestDamage = true;
			if(ht.ContainsKey("lookatlatestuser")) this.lookAtLatestUser = true;
			if(ht.ContainsKey("lookatmenuuser")) this.lookAtMenuUser = true;
			if(ht.ContainsKey("lookatselection")) this.lookAtSelection = true;
			if(ht.ContainsKey("latestdamages"))
			{
				int count = int.Parse((string)ht["latestdamages"]);
				this.latestDamageID = new int[count];
				this.ldRotationAxis = new Vector3[count];
				this.ldRotationSpeed = new float[count];
			}
			if(ht.ContainsKey("latestusers"))
			{
				int count = int.Parse((string)ht["latestusers"]);
				this.latestUserID = new int[count];
				this.luRotationAxis = new Vector3[count];
				this.luRotationSpeed = new float[count];
			}
			if(ht.ContainsKey("menuusers"))
			{
				int count = int.Parse((string)ht["menuusers"]);
				this.menuUserID = new int[count];
				this.muRotationAxis = new Vector3[count];
				this.muRotationSpeed = new float[count];
			}
			if(ht.ContainsKey("selections"))
			{
				int count = int.Parse((string)ht["selections"]);
				this.selectionID = new int[count];
				this.sRotationAxis = new Vector3[count];
				this.sRotationSpeed = new float[count];
			}
			if(ht.ContainsKey("noblocks"))
			{
				this.noBlockAnimation = new int[int.Parse((string)ht["noblocks"])];
			}
			
			if(ht.ContainsKey(XMLHandler.NODES))
			{
				ArrayList s = ht[XMLHandler.NODES] as ArrayList;
				foreach(Hashtable ht2 in s)
				{
					if(ht2[XMLHandler.NODE_NAME] as string == BattleCam.LATESTDAMAGE)
					{
						int id = int.Parse((string)ht2["id"]);
						if(id < this.latestDamageID.Length)
						{
							this.latestDamageID[id] = int.Parse((string)ht2["campos"]);
							this.ldRotationAxis[id] = new Vector3(
									float.Parse((string)ht2["rx"]), 
									float.Parse((string)ht2["ry"]), 
									float.Parse((string)ht2["rz"]));
							this.ldRotationSpeed[id] = float.Parse((string)ht2["speed"]);
						}
					}
					else if(ht2[XMLHandler.NODE_NAME] as string == BattleCam.LATESTUSER)
					{
						int id = int.Parse((string)ht2["id"]);
						if(id < this.latestUserID.Length)
						{
							this.latestUserID[id] = int.Parse((string)ht2["campos"]);
							this.luRotationAxis[id] = new Vector3(
									float.Parse((string)ht2["rx"]), 
									float.Parse((string)ht2["ry"]), 
									float.Parse((string)ht2["rz"]));
							this.luRotationSpeed[id] = float.Parse((string)ht2["speed"]);
						}
					}
					else if(ht2[XMLHandler.NODE_NAME] as string == BattleCam.MENUUSER)
					{
						int id = int.Parse((string)ht2["id"]);
						if(id < this.menuUserID.Length)
						{
							this.menuUserID[id] = int.Parse((string)ht2["campos"]);
							this.muRotationAxis[id] = new Vector3(
									float.Parse((string)ht2["rx"]), 
									float.Parse((string)ht2["ry"]), 
									float.Parse((string)ht2["rz"]));
							this.muRotationSpeed[id] = float.Parse((string)ht2["speed"]);
						}
					}
					else if(ht2[XMLHandler.NODE_NAME] as string == BattleCam.SELECTION)
					{
						int id = int.Parse((string)ht2["id"]);
						if(id < this.selectionID.Length)
						{
							this.selectionID[id] = int.Parse((string)ht2["campos"]);
							this.sRotationAxis[id] = new Vector3(
									float.Parse((string)ht2["rx"]), 
									float.Parse((string)ht2["ry"]), 
									float.Parse((string)ht2["rz"]));
							this.sRotationSpeed[id] = float.Parse((string)ht2["speed"]);
						}
					}
					else if(ht2[XMLHandler.NODE_NAME] as string == BattleCam.NOBLOCK)
					{
						int id = int.Parse((string)ht2["id"]);
						if(id < this.noBlockAnimation.Length)
						{
							this.noBlockAnimation[id] = int.Parse((string)ht2["aid"]);
						}
					}
					else if(ht2[XMLHandler.NODE_NAME] as string == BattleCam.DAMAGECHILD)
					{
						this.damageLookAtChild = ht2[XMLHandler.CONTENT] as string;
					}
					else if(ht2[XMLHandler.NODE_NAME] as string == BattleCam.USERCHILD)
					{
						this.userLookAtChild = ht2[XMLHandler.CONTENT] as string;
					}
					else if(ht2[XMLHandler.NODE_NAME] as string == BattleCam.MENUCHILD)
					{
						this.menuLookAtChild = ht2[XMLHandler.CONTENT] as string;
					}
					else if(ht2[XMLHandler.NODE_NAME] as string == BattleCam.SELECTIONCHILD)
					{
						this.selectionLookAtChild = ht2[XMLHandler.CONTENT] as string;
					}
				}
			}
		}
	}
	
	/*
	============================================================================
	Latest damage functions
	============================================================================
	*/
	public void AddLatestDamage()
	{
		this.latestDamageID = ArrayHelper.Add(0, this.latestDamageID);
		this.ldRotationAxis = ArrayHelper.Add(Vector3.zero, this.ldRotationAxis);
		this.ldRotationSpeed = ArrayHelper.Add(0, this.ldRotationSpeed);
	}
	
	public void RemoveLatestDamage(int index)
	{
		this.latestDamageID = ArrayHelper.Remove(index, this.latestDamageID);
		this.ldRotationAxis = ArrayHelper.Remove(index, this.ldRotationAxis);
		this.ldRotationSpeed = ArrayHelper.Remove(index, this.ldRotationSpeed);
	}
	
	public void SetLatestDamage(Transform t)
	{
		if(this.ldTarget != t)
		{
			this.ldTarget = t;
			this.luTarget = null;
			if(this.ldTarget != null)
			{
				if(this.lookAtLatestDamage && this.damageLookAtChild != "")
				{
					this.ldTarget = TransformHelper.GetChild(this.damageLookAtChild, this.ldTarget);
				}
				else if(this.latestDamageID.Length > 0)
				{
					this.ldIndex = Random.Range(0, this.latestDamageID.Length);
					DataHolder.CameraPosition(this.latestDamageID[this.ldIndex]).Use(this.camera, this.ldTarget);
				}
			}
		}
	}
	
	/*
	============================================================================
	Latest user functions
	============================================================================
	*/
	public void AddLatestUser()
	{
		this.latestUserID = ArrayHelper.Add(0, this.latestUserID);
		this.luRotationAxis = ArrayHelper.Add(Vector3.zero, this.luRotationAxis);
		this.luRotationSpeed = ArrayHelper.Add(0, this.luRotationSpeed);
	}
	
	public void RemoveLatestUser(int index)
	{
		this.latestUserID = ArrayHelper.Remove(index, this.latestUserID);
		this.luRotationAxis = ArrayHelper.Remove(index, this.luRotationAxis);
		this.luRotationSpeed = ArrayHelper.Remove(index, this.luRotationSpeed);
	}
	
	public void SetLatestUser(Transform t)
	{
		if(this.luTarget != t)
		{
			this.luTarget = t;
			this.ldTarget = null;
			if(this.luTarget != null)
			{
				if(this.lookAtLatestUser && this.userLookAtChild != "")
				{
					this.luTarget = TransformHelper.GetChild(this.userLookAtChild, this.luTarget);
				}
				else if(this.latestUserID.Length > 0)
				{
					this.luIndex = Random.Range(0, this.latestUserID.Length);
					DataHolder.CameraPosition(this.latestUserID[this.luIndex]).Use(this.camera, this.luTarget);
				}
			}
		}
	}
	
	/*
	============================================================================
	Menu user functions
	============================================================================
	*/
	public void AddMenuUser()
	{
		this.menuUserID = ArrayHelper.Add(0, this.menuUserID);
		this.muRotationAxis = ArrayHelper.Add(Vector3.zero, this.muRotationAxis);
		this.muRotationSpeed = ArrayHelper.Add(0, this.muRotationSpeed);
	}
	
	public void RemoveMenuUser(int index)
	{
		this.menuUserID = ArrayHelper.Remove(index, this.menuUserID);
		this.muRotationAxis = ArrayHelper.Remove(index, this.muRotationAxis);
		this.muRotationSpeed = ArrayHelper.Remove(index, this.muRotationSpeed);
	}
	
	public void SetMenuUser(Transform t)
	{
		if(this.muTarget != t)
		{
			this.muTarget = t;
			this.ldTarget = null;
			this.luTarget = null;
			if(this.muTarget != null)
			{
				if(this.lookAtMenuUser && this.menuLookAtChild != "")
				{
					this.muTarget = TransformHelper.GetChild(this.menuLookAtChild, this.muTarget);
				}
				else if(this.menuUserID.Length > 0)
				{
					this.muIndex = Random.Range(0, this.menuUserID.Length);
					DataHolder.CameraPosition(this.menuUserID[this.muIndex]).Use(this.camera, this.muTarget);
				}
			}
		}
	}
	
	/*
	============================================================================
	Selection functions
	============================================================================
	*/
	public void AddSelection()
	{
		this.selectionID = ArrayHelper.Add(0, this.selectionID);
		this.sRotationAxis = ArrayHelper.Add(Vector3.zero, this.sRotationAxis);
		this.sRotationSpeed = ArrayHelper.Add(0, this.sRotationSpeed);
	}
	
	public void RemoveSelection(int index)
	{
		this.selectionID = ArrayHelper.Remove(index, this.selectionID);
		this.sRotationAxis = ArrayHelper.Remove(index, this.sRotationAxis);
		this.sRotationSpeed = ArrayHelper.Remove(index, this.sRotationSpeed);
	}
	
	public void SetSelection(Transform t)
	{
		if(this.sTarget != t)
		{
			this.sTarget = t;
			this.ldTarget = null;
			this.luTarget = null;
			if(this.sTarget != null)
			{
				if(this.lookAtSelection && this.selectionLookAtChild != "")
				{
					this.sTarget = TransformHelper.GetChild(this.selectionLookAtChild, this.sTarget);
				}
				else if(this.selectionID.Length > 0)
				{
					this.sIndex = Random.Range(0, this.selectionID.Length);
					DataHolder.CameraPosition(this.selectionID[this.sIndex]).Use(this.camera, this.sTarget);
				}
			}
		}
	}
	
	/*
	============================================================================
	Block functions
	============================================================================
	*/
	public void AddNoBlock()
	{
		this.noBlockAnimation = ArrayHelper.Add(0, this.noBlockAnimation);
	}
	
	public void RemoveNoBlock(int index)
	{
		this.noBlockAnimation = ArrayHelper.Remove(index, this.noBlockAnimation);
	}
	
	public bool IsAnimationCamBlocked(int id)
	{
		bool blocked = false;
		if(this.blockAnimationCams)
		{
			blocked = true;
			for(int i = 0; i<this.noBlockAnimation.Length; i++)
			{
				if(this.noBlockAnimation[i] == id)
				{
					blocked = false;
					break;
				}
			}
		}
		return blocked;
	}
	
	public void BlockedByAnimation(bool block)
	{
		if(block) this.blockedByAnimation++;
		else this.blockedByAnimation--;
		this.forceReset = true;
	}
	
	/*
	============================================================================
	Camera functions
	============================================================================
	*/
	public void Tick()
	{
		if(this.blockAnimationCams && this.blockedByAnimation == 0 && 
			this.activated && this.camera != null)
		{
			int index = -1;
			bool looking = false;
			bool resetBase = true;
			
			if(this.sTarget != null && (this.lookAtSelection || 
				(this.selectionID.Length > 0 && this.sIndex < this.selectionID.Length)))
			{
				if(this.lookAtSelection)
				{
					index = 0;
					Quaternion rotation = Quaternion.LookRotation(this.sTarget.position-this.camera.position);
					this.camera.rotation = Quaternion.Slerp(this.camera.rotation, rotation, this.lookAtDamping*Time.deltaTime);
				}
				else
				{
					if(this.sRotationSpeed[this.sIndex] != 0)
					{
						this.camera.RotateAround(this.sTarget.position, this.sRotationAxis[this.sIndex], 
								this.sRotationSpeed[this.sIndex]*Time.deltaTime);
					}
					looking = true;
				}
				resetBase = false;
				this.lookingAtSomething = true;
			}
			if(resetBase && this.muTarget != null && (this.lookAtMenuUser || 
				(this.menuUserID.Length > 0 && this.muIndex < this.menuUserID.Length)))
			{
				if(this.lookAtMenuUser)
				{
					index = 1;
					Quaternion rotation = Quaternion.LookRotation(this.muTarget.position-this.camera.position);
					this.camera.rotation = Quaternion.Slerp(this.camera.rotation, rotation, this.lookAtDamping*Time.deltaTime);
				}
				else
				{
					if(this.muRotationSpeed[this.muIndex] != 0)
					{
						this.camera.RotateAround(this.muTarget.position, this.muRotationAxis[this.muIndex], 
								this.muRotationSpeed[this.muIndex]*Time.deltaTime);
					}
					looking = true;
				}
				resetBase = false;
				this.lookingAtSomething = true;
			}
			if(resetBase && this.ldTarget != null && (this.lookAtLatestDamage || 
				(this.latestDamageID.Length > 0 && this.ldIndex < this.latestDamageID.Length)))
			{
				if(this.lookAtLatestDamage)
				{
					index = 2;
					Quaternion rotation = Quaternion.LookRotation(this.ldTarget.position-this.camera.position);
					this.camera.rotation = Quaternion.Slerp(this.camera.rotation, rotation, this.lookAtDamping*Time.deltaTime);
				}
				else
				{
					if(this.ldRotationSpeed[this.ldIndex] != 0)
					{
						this.camera.RotateAround(this.ldTarget.position, this.ldRotationAxis[this.ldIndex], 
								this.ldRotationSpeed[this.ldIndex]*Time.deltaTime);
					}
					looking = true;
				}
				resetBase = false;
				this.lookingAtSomething = true;
			}
			if(resetBase && this.luTarget != null && (this.lookAtLatestUser || 
				(this.latestUserID.Length > 0 && this.luIndex < this.latestUserID.Length)))
			{
				if(this.lookAtLatestUser)
				{
					index = 2;
					Quaternion rotation = Quaternion.LookRotation(this.luTarget.position-this.camera.position);
					this.camera.rotation = Quaternion.Slerp(this.camera.rotation, rotation, this.lookAtDamping*Time.deltaTime);
				}
				else
				{
					if(this.luRotationSpeed[this.luIndex] != 0)
					{
						this.camera.RotateAround(this.luTarget.position, this.luRotationAxis[this.luIndex], 
								this.luRotationSpeed[this.luIndex]*Time.deltaTime);
					}
					looking = true;
				}
				resetBase = false;
				this.lookingAtSomething = true;
			}
			
			if(!looking)
			{
				if(this.forceReset || 
					(this.lookingAtSomething && 
					(resetBase || index != this.lastLookAtIndex)))
				{
					if(this.rememberPosition)
					{
						this.camera.position = this.lastPos;
						this.camera.rotation = this.lastRot;
					}
					else if(this.useCombatantCenter)
					{
						DataHolder.CameraPosition(this.battleArena.baseCamPos).Use(
								this.camera, DataHolder.BattleSystem().GetCombatantCenter().transform);
					}
					else
					{
						DataHolder.CameraPosition(this.battleArena.baseCamPos).Use(
								this.camera, this.battleArena.transform);
					}
					this.forceReset = false;
					this.lookingAtSomething = false;
				}
				if(this.rotationSpeed != 0)
				{
					if(this.useCombatantCenter)
					{
						this.camera.RotateAround(DataHolder.BattleSystem().GetCombatantCenter().transform.position, 
								this.useRotationAxis, this.rotationSpeed*Time.deltaTime);
					}
					else
					{
						this.camera.RotateAround(this.battleArena.transform.position, 
								this.useRotationAxis, this.rotationSpeed*Time.deltaTime);
					}
					if(this.limitRotation)
					{
						if(this.camera.eulerAngles.x < this.startRotationAxis.x+this.minRotation.x)
						{
							this.useRotationAxis.x = -this.useRotationAxis.x;
						}
						else if(this.camera.eulerAngles.x > this.startRotationAxis.x+this.maxRotation.x)
						{
							this.useRotationAxis.x = -this.useRotationAxis.x;
						}
						if(this.camera.eulerAngles.y < this.startRotationAxis.y+this.minRotation.y)
						{
							this.useRotationAxis.y = -this.useRotationAxis.y;
						}
						else if(this.camera.eulerAngles.y > this.startRotationAxis.y+this.maxRotation.y)
						{
							this.useRotationAxis.y = -this.useRotationAxis.y;
						}
						if(this.camera.eulerAngles.z < this.startRotationAxis.z+this.minRotation.z)
						{
							this.useRotationAxis.z = -this.useRotationAxis.z;
						}
						else if(this.camera.eulerAngles.z > this.startRotationAxis.z+this.maxRotation.z)
						{
							this.useRotationAxis.z = -this.useRotationAxis.z;
						}
					}
				}
				if(this.rememberPosition)
				{
					this.lastPos = this.camera.position;
					this.lastRot = this.camera.rotation;
				}
			}
			this.lastLookAtIndex = index;
		}
	}
	
	public void StartBattle(BattleArena arena)
	{
		this.useRotationAxis = this.rotationAxis;
		this.startRotationAxis = Vector3.zero;
		this.lookingAtSomething = false;
		if(Camera.main)
		{
			this.camera = Camera.main.transform;
			if(this.rememberPosition)
			{
				this.lastPos = this.camera.position;
				this.lastRot = this.camera.rotation;
				this.startRotationAxis = this.camera.eulerAngles;
			}
		}
		this.ldTarget = null;
		this.luTarget = null;
		this.muTarget = null;
		this.sTarget = null;
		if(arena != null)
		{
			this.battleArena = arena;
		}
		this.lastLookAtIndex = -1;
		this.blockedByAnimation = 0;
		this.forceReset = false;
		this.activated = true;
	}
	
	public void EndBattle()
	{
		this.activated = false;
		this.blockedByAnimation = 0;
		this.lookingAtSomething = false;
		this.forceReset = false;
		this.camera = null;
		this.ldTarget = null;
		this.luTarget = null;
		this.muTarget = null;
		this.sTarget = null;
		this.battleArena = null;
		this.lastLookAtIndex = -1;
		this.lastPos = Vector3.zero;
		this.lastRot = Quaternion.identity;
	}
}
