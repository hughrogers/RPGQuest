
using UnityEngine;
using System.Collections;

public class PlayerControlSettings
{
	public PlayerControlType type = PlayerControlType.NONE;
	
	// player controller
	public bool moveDead = true;
	public bool useCharacterSpeed = false;
	public float runSpeed = 8.0f;
	public float gravity = Physics.gravity.y;
	public float speedSmoothing = 10.0f;
	public float rotateSpeed = 500.0f;
	public bool firstPerson = false;
	public bool useCamDirection = true;
	public string verticalAxis = "Vertical";
	public string horizontalAxis = "Horizontal";
	public bool useJump = false;
	public string jumpKey = "";
	public float jumpDuration = 0.5f;
	public float jumpSpeed = -Physics.gravity.y;
	public EaseType jumpInterpolation = EaseType.EaseOutQuad;
	public float inAirModifier = 0.5f;
	public float jumpMaxGroundAngle = 45;
	public bool useSprint = false;
	public string sprintKey = "";
	public float sprintFactor = 2.0f;
	public bool useEnergy = false;
	public bool maxEFormula = false;
	public float maxEnergy = 10.0f;
	public int meFormula = 0;
	public bool energyCFormula = false;
	public float energyConsume = 1.0f;
	public int ecFormula = 0;
	public bool energyRFormula = false;
	public float energyRegeneration = 1.0f;
	public int erFormula = 0;
	
	// click player controller
	public MouseTouchControl mouseTouch = new MouseTouchControl(true);
	public float raycastDistance = 100.0f;
	public int layerMask = 1;
	public string cursorObjectName = "";
	public GameObject cursorObject;
	public Vector3 cursorOffset = Vector3.zero;
	public float minimumMoveDistance = 0.2f;
	public bool ignoreYDistance = true;
	public bool useEventMover = false;
	public bool autoRemoveCursor = true;
	public bool autoStopMove = true;
	
	public static string VERTICALAXIS = "verticalaxis";
	public static string HORIZONTALAXIS = "horizontalaxis";
	public static string CURSOROBJECT = "cursorobject";
	public static string JUMPKEY = "jumpkey";
	public static string SPRINTKEY = "sprintkey";
	
	public PlayerControlSettings()
	{
		
	}
	
	public bool IsDefaultControl()
	{
		return PlayerControlType.DEFAULT.Equals(this.type);
	}
	
	public bool IsMobileControl()
	{
		return PlayerControlType.MOBILE.Equals(this.type);
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		ht.Add("type", this.type.ToString());
		ArrayList s = new ArrayList();
		if(this.IsDefaultControl())
		{
			ht.Add("movedead", this.moveDead.ToString());
			if(this.useCharacterSpeed) ht.Add("usecharacterspeed", "true");
			if(this.firstPerson) ht.Add("firstperson", "true");
			ht.Add("usecamdir", this.useCamDirection.ToString());
			ht.Add("runspeed", this.runSpeed.ToString());
			ht.Add("gravity", this.gravity.ToString());
			ht.Add("speedsmoothing", this.speedSmoothing.ToString());
			ht.Add("rotatespeed", this.rotateSpeed.ToString());
			
			s.Add(HashtableHelper.GetContentHashtable(
					PlayerControlSettings.VERTICALAXIS, this.verticalAxis));
			s.Add(HashtableHelper.GetContentHashtable(
					PlayerControlSettings.HORIZONTALAXIS, this.horizontalAxis));
			
			if(this.useJump)
			{
				ht.Add("jumpduration", this.jumpDuration.ToString());
				ht.Add("jumpspeed", this.jumpSpeed.ToString());
				ht.Add("jumpinterpolation", this.jumpInterpolation.ToString());
				ht.Add("inairmodifier", this.inAirModifier.ToString());
				ht.Add("jumpmaxgroundangle", this.jumpMaxGroundAngle.ToString());
				s.Add(HashtableHelper.GetContentHashtable(
						PlayerControlSettings.JUMPKEY, this.jumpKey));
			}
			if(this.useSprint)
			{
				ht.Add("sprintfactor", this.sprintFactor.ToString());
				s.Add(HashtableHelper.GetContentHashtable(
						PlayerControlSettings.SPRINTKEY, this.sprintKey));
				if(this.useEnergy)
				{
					ht.Add("useenergy", "true");
					if(this.maxEFormula) ht.Add("meformula", this.meFormula.ToString());
					else ht.Add("maxenergy", this.maxEnergy.ToString());
					if(this.energyCFormula) ht.Add("ecformula", this.ecFormula.ToString());
					else ht.Add("energyconsume", this.energyConsume.ToString());
					if(this.energyRFormula) ht.Add("erformula", this.erFormula.ToString());
					else ht.Add("energyregeneration", this.energyRegeneration.ToString());
				}
			}
		}
		else if(this.IsMobileControl())
		{
			ht = this.mouseTouch.GetData(ht);
			ht.Add("movedead", this.moveDead.ToString());
			if(this.useCharacterSpeed) ht.Add("usecharacterspeed", "true");
			ht.Add("runspeed", this.runSpeed.ToString());
			ht.Add("gravity", this.gravity.ToString());
			ht.Add("speedsmoothing", this.speedSmoothing.ToString());
			ht.Add("raycastdistance", this.raycastDistance.ToString());
			ht.Add("layermask", this.layerMask.ToString());
			ht.Add("x", this.cursorOffset.x.ToString());
			ht.Add("y", this.cursorOffset.y.ToString());
			ht.Add("z", this.cursorOffset.z.ToString());
			ht.Add("minimummovedistance", this.minimumMoveDistance.ToString());
			ht.Add("ignoreydistance", this.ignoreYDistance.ToString());
			ht.Add("useeventmover", this.useEventMover.ToString());
			ht.Add("autoremovecursor", this.autoRemoveCursor.ToString());
			ht.Add("autostopmove", this.autoStopMove.ToString());
			
			s.Add(HashtableHelper.GetContentHashtable(
					PlayerControlSettings.CURSOROBJECT, this.cursorObjectName));
		}
		if(s.Count > 0) ht.Add(XMLHandler.NODES, s);
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		if(ht.ContainsKey("type"))
		{
			this.type = (PlayerControlType)System.Enum.Parse(
					typeof(PlayerControlType), (string)ht["type"]);
			if(this.IsDefaultControl())
			{
				if(ht.ContainsKey("movedead")) this.moveDead = bool.Parse((string)ht["movedead"]);
				if(ht.ContainsKey("usecharacterspeed")) this.useCharacterSpeed = true;
				if(ht.ContainsKey("firstperson")) this.firstPerson = true;
				if(ht.ContainsKey("usecamdir")) this.useCamDirection = bool.Parse((string)ht["usecamdir"]);
				this.runSpeed = float.Parse((string)ht["runspeed"]);
				this.gravity = float.Parse((string)ht["gravity"]);
				this.speedSmoothing = float.Parse((string)ht["speedsmoothing"]);
				this.rotateSpeed = float.Parse((string)ht["rotatespeed"]);
				if(ht.ContainsKey("jumpduration"))
				{
					this.useJump = true;
					this.jumpDuration = float.Parse((string)ht["jumpduration"]);
					this.jumpSpeed = float.Parse((string)ht["jumpspeed"]);
					this.inAirModifier = float.Parse((string)ht["inairmodifier"]);
					if(ht.ContainsKey("jumpmaxgroundangle"))
					{
						this.jumpMaxGroundAngle = float.Parse((string)ht["jumpmaxgroundangle"]);
					}
					this.jumpInterpolation = (EaseType)System.Enum.Parse(typeof(EaseType), (string)ht["jumpinterpolation"]);
				}
				if(ht.ContainsKey("sprintfactor"))
				{
					this.useSprint = true;
					this.sprintFactor = float.Parse((string)ht["sprintfactor"]);
					if(ht.ContainsKey("useenergy"))
					{
						this.useEnergy = true;
						if(ht.ContainsKey("meformula"))
						{
							this.maxEFormula = true;
							this.meFormula = int.Parse((string)ht["meformula"]);
						}
						else if(ht.ContainsKey("maxenergy"))
						{
							this.maxEnergy = float.Parse((string)ht["maxenergy"]);
						}
						if(ht.ContainsKey("ecformula"))
						{
							this.energyCFormula = true;
							this.ecFormula = int.Parse((string)ht["ecformula"]);
						}
						else if(ht.ContainsKey("energyconsume"))
						{
							this.energyConsume = float.Parse((string)ht["energyconsume"]);
						}
						if(ht.ContainsKey("erformula"))
						{
							this.energyRFormula = true;
							this.erFormula = int.Parse((string)ht["erformula"]);
						}
						else if(ht.ContainsKey("energyregeneration"))
						{
							this.energyRegeneration = float.Parse((string)ht["energyregeneration"]);
						}
					}
				}
			}
			else if(this.IsMobileControl())
			{
				this.mouseTouch.SetData(ht);
				if(ht.ContainsKey("movedead")) this.moveDead = bool.Parse((string)ht["movedead"]);
				if(ht.ContainsKey("usecharacterspeed")) this.useCharacterSpeed = true;
				this.runSpeed = float.Parse((string)ht["runspeed"]);
				this.gravity = float.Parse((string)ht["gravity"]);
				this.speedSmoothing = float.Parse((string)ht["speedsmoothing"]);
				this.raycastDistance = float.Parse((string)ht["raycastdistance"]);
				this.layerMask = int.Parse((string)ht["layermask"]);
				
				this.cursorOffset = new Vector3(
						float.Parse((string)ht["x"]), 
						float.Parse((string)ht["y"]), 
						float.Parse((string)ht["z"]));
				
				this.minimumMoveDistance = float.Parse((string)ht["minimummovedistance"]);
				this.ignoreYDistance = bool.Parse((string)ht["ignoreydistance"]);
				this.useEventMover = bool.Parse((string)ht["useeventmover"]);
				this.autoRemoveCursor = bool.Parse((string)ht["autoremovecursor"]);
				this.autoStopMove = bool.Parse((string)ht["autostopmove"]);
			}
			if(ht.ContainsKey(XMLHandler.NODES))
			{
				ArrayList s = ht[XMLHandler.NODES] as ArrayList;
				foreach(Hashtable ht2 in s)
				{
					if(ht2[XMLHandler.NODE_NAME] as string == PlayerControlSettings.VERTICALAXIS)
					{
						this.verticalAxis = ht2[XMLHandler.CONTENT] as string;
					}
					else if(ht2[XMLHandler.NODE_NAME] as string == PlayerControlSettings.HORIZONTALAXIS)
					{
						this.horizontalAxis = ht2[XMLHandler.CONTENT] as string;
					}
					else if(ht2[XMLHandler.NODE_NAME] as string == PlayerControlSettings.CURSOROBJECT)
					{
						this.cursorObjectName = ht2[XMLHandler.CONTENT] as string;
					}
					else if(ht2[XMLHandler.NODE_NAME] as string == PlayerControlSettings.JUMPKEY)
					{
						this.jumpKey = ht2[XMLHandler.CONTENT] as string;
					}
					else if(ht2[XMLHandler.NODE_NAME] as string == PlayerControlSettings.SPRINTKEY)
					{
						this.sprintKey = ht2[XMLHandler.CONTENT] as string;
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
	public void AddPlayerControl(GameObject player)
	{
		DataHolder.GameSettings().AddInteractionController(player);
		
		if(this.IsDefaultControl())
		{
			PlayerController comp = player.GetComponent<PlayerController>();
			if(comp == null)
			{
				comp = player.AddComponent<PlayerController>();
			}
			comp.moveDead = this.moveDead;
			comp.useCharacterSpeed = this.useCharacterSpeed;
			comp.runSpeed = this.runSpeed;
			comp.gravity = this.gravity;
			comp.speedSmoothing = this.speedSmoothing;
			comp.rotateSpeed = this.rotateSpeed;
			comp.verticalAxis = this.verticalAxis;
			comp.horizontalAxis = this.horizontalAxis;
			comp.useJump = this.useJump;
			comp.jumpKey = this.jumpKey;
			comp.jumpDuration = this.jumpDuration;
			comp.jumpSpeed = this.jumpSpeed;
			comp.jumpInterpolation = this.jumpInterpolation;
			comp.inAirModifier = this.inAirModifier;
			comp.jumpMaxGroundAngle = this.jumpMaxGroundAngle;
			comp.useSprint = this.useSprint;
			comp.sprintKey = this.sprintKey;
			comp.useEnergy = this.useEnergy;
			comp.maxEFormula = this.maxEFormula;
			comp.maxEnergy = this.maxEnergy;
			comp.meFormula = this.meFormula;
			comp.energyCFormula = this.energyCFormula;
			comp.ecFormula = this.ecFormula;
			comp.energyRFormula = this.energyRFormula;
			comp.energyRegeneration = this.energyRegeneration;
			comp.erFormula = this.erFormula;
			comp.firstPerson = this.firstPerson;
			comp.useCamDirection = this.useCamDirection;
		}
		else if(this.IsMobileControl())
		{
			ClickPlayerController comp = player.GetComponent<ClickPlayerController>();
			if(comp == null)
			{
				comp = player.AddComponent<ClickPlayerController>();
			}
			comp.mouseTouch.SetData(this.mouseTouch.GetData(new Hashtable()));
			comp.moveDead = this.moveDead;
			comp.useCharacterSpeed = this.useCharacterSpeed;
			comp.runSpeed = this.runSpeed;
			comp.gravity = this.gravity;
			comp.speedSmoothing = this.speedSmoothing;
			comp.raycastDistance = this.raycastDistance;
			comp.layerMask = 1 << this.layerMask;
			comp.cursorOffset = this.cursorOffset;
			comp.minimumMoveDistance = this.minimumMoveDistance;
			comp.ignoreYDistance = this.ignoreYDistance;
			comp.useEventMover = this.useEventMover;
			comp.autoRemoveCursor = this.autoRemoveCursor;
			comp.autoStopMove = this.autoStopMove;
			
			comp.cursorObject = (GameObject)Resources.Load(
					BattleSystemData.PREFAB_PATH+
					this.cursorObjectName, typeof(GameObject));
		}
	}
	
	public void RemovePlayerControl(GameObject player)
	{
		if(this.IsDefaultControl())
		{
			PlayerController comp = player.GetComponent<PlayerController>();
			if(comp != null) GameObject.Destroy(comp);
		}
		else if(this.IsMobileControl())
		{
			ClickPlayerController comp = player.GetComponent<ClickPlayerController>();
			if(comp != null)
			{
				comp.ClearCursor();
				GameObject.Destroy(comp);
			}
		}
	}
}
