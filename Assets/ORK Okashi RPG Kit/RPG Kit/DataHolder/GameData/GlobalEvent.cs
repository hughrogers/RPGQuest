
using UnityEngine;
using System.Collections;

public class GlobalEvent
{
	// execution data
	public GlobalEventType eventType = GlobalEventType.CALL;
	public float timeout = 1;
	public bool inPause = false;
	public bool[] controlType = new bool[System.Enum.GetNames(typeof(ControlType)).Length];
	public VariableCondition variables = new VariableCondition();
	
	// event data
	public string eventFile = "";
	
	public string[] prefabName = new string[0];
	public GameObject[] prefab = new GameObject[0];
	
	public string[] audioName = new string[0];
	public AudioClip[] audioClip = new AudioClip[0];
	
	// XML
	private static string EVENTFILE = "eventfile";
	private static string CONTROLTYPE = "controltype";
	private static string VARIABLES = "variables";
	private static string PREFAB = "prefab";
	private static string AUDIOCLIP = "audioclip";
	
	// ingame
	public GameEvent gameEvent = new GameEvent();
	public bool fileOk = false;
	
	private float timer = 0;
	private bool eventStarted = false;
	private bool eventFinished = false;
	
	private GameEvent callingEvent = null;
	
	public GlobalEvent()
	{
		
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		ArrayList s = new ArrayList();
		s.Add(HashtableHelper.GetContentHashtable(GlobalEvent.EVENTFILE, this.eventFile));
		
		ht.Add("type", this.eventType.ToString());
		
		if(this.IsAutoType())
		{
			ht.Add("timeout", this.timeout.ToString());
			ht.Add("inpause", this.inPause.ToString());
			
			for(int i=0; i<this.controlType.Length; i++)
			{
				if(this.controlType[i])
				{
					s.Add(HashtableHelper.GetTitleHashtable(GlobalEvent.CONTROLTYPE, i));
				}
			}
			s.Add(this.variables.GetData(GlobalEvent.VARIABLES));
		}
		
		ht.Add("prefabs", this.prefabName.Length.ToString());
		for(int i=0; i<this.prefabName.Length; i++)
		{
			if(this.prefabName[i] != "")
			{
				s.Add(HashtableHelper.GetContentHashtable(GlobalEvent.PREFAB, this.prefabName[i], i));
			}
		}
		
		ht.Add("audioclips", this.audioName.Length.ToString());
		for(int i=0; i<this.audioName.Length; i++)
		{
			if(this.audioName[i] != "")
			{
				s.Add(HashtableHelper.GetContentHashtable(GlobalEvent.AUDIOCLIP, this.audioName[i], i));
			}
		}
		
		ht.Add(XMLHandler.NODES, s);
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		this.eventType = (GlobalEventType)System.Enum.Parse(typeof(GlobalEventType), (string)ht["type"]);
		if(ht.ContainsKey("timeout"))
		{
			this.timeout = float.Parse((string)ht["timeout"]);
		}
		if(ht.ContainsKey("inpause"))
		{
			this.inPause = bool.Parse((string)ht["inpause"]);
		}
		
		int count = int.Parse((string)ht["prefabs"]);
		this.prefabName = new string[count];
		this.prefab = new GameObject[count];
		for(int i=0; i<count; i++) this.prefabName[i] = "";
		
		count = int.Parse((string)ht["audioclips"]);
		this.audioName = new string[count];
		this.audioClip = new AudioClip[count];
		for(int i=0; i<count; i++) this.audioName[i] = "";
		
		if(ht.ContainsKey(XMLHandler.NODES))
		{
			ArrayList s = ht[XMLHandler.NODES] as ArrayList;
			foreach(Hashtable ht2 in s)
			{
				if(ht2[XMLHandler.NODE_NAME] as string == GlobalEvent.EVENTFILE)
				{
					this.eventFile = ht2[XMLHandler.CONTENT] as string;
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == GlobalEvent.CONTROLTYPE)
				{
					int id = int.Parse((string)ht2["id"]);
					if(id < this.controlType.Length) this.controlType[id] = true;
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == GlobalEvent.VARIABLES)
				{
					this.variables.SetData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == GlobalEvent.PREFAB)
				{
					int id = int.Parse((string)ht2["id"]);
					if(id < this.prefabName.Length)
					{
						this.prefabName[id] = ht2[XMLHandler.CONTENT] as string;
					}
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == GlobalEvent.AUDIOCLIP)
				{
					int id = int.Parse((string)ht2["id"]);
					if(id < this.audioName.Length)
					{
						this.audioName[id] = ht2[XMLHandler.CONTENT] as string;
					}
				}
			}
		}
		this.LoadEvent();
	}
	
	public GlobalEvent GetCopy()
	{
		GlobalEvent ge = new GlobalEvent();
		ge.SetData(this.GetData(new Hashtable()));
		return ge;
	}
	
	/*
	============================================================================
	Event file functions
	============================================================================
	*/
	public string GetEventFile()
	{
		return "Assets/Resources/Events/"+this.eventFile+".xml";
	}
	
	public bool LoadEvent()
	{
		if(this.eventFile != "" && this.gameEvent.LoadEventData(this.GetEventFile()))
		{
			if(Application.isPlaying)
			{
				this.LoadResources();
				this.gameEvent.prefab = this.prefab;
				this.gameEvent.audioClip = this.audioClip;
			}
			this.fileOk = true;
			this.timer = 0;
			this.eventStarted = false;
		}
		return this.fileOk;
	}
	
	public void LoadResources()
	{
		for(int i=0; i<this.prefab.Length; i++)
		{
			if(this.prefab[i] == null && this.prefabName[i] != null &&
				this.prefabName[i] != "")
			{
				this.prefab[i] = (GameObject)Resources.Load(GlobalEventData.PREFAB_PATH+
						this.prefabName[i], typeof(GameObject));
			}
		}
		
		for(int i=0; i<this.audioClip.Length; i++)
		{
			if(this.audioClip[i] == null && this.audioName[i] != null &&
				this.audioName[i] != "")
			{
				this.audioClip[i] = (AudioClip)Resources.Load(GlobalEventData.AUDIO_PATH+
						this.audioName[i], typeof(AudioClip));
			}
		}
	}
	
	/*
	============================================================================
	Event execution functions
	============================================================================
	*/
	public bool IsCallType()
	{
		return GlobalEventType.CALL.Equals(this.eventType);
	}
	
	public bool IsAutoType()
	{
		return GlobalEventType.AUTO.Equals(this.eventType);
	}
	
	public bool IsFinished()
	{
		return this.eventFinished;
	}
	
	public void Tick(float t)
	{
		if(this.eventStarted)
		{
			this.gameEvent.TimeTick(t);
		}
		else if(this.fileOk && this.IsAutoType())
		{
			this.timer += t;
			
			if(this.timer >= this.timeout)
			{
				this.StartEvent();
				this.timer = 0;
			}
		}
	}
	
	public void StartEvent()
	{
		bool mode = (this.controlType[(int)GameHandler.GetControlType()] || 
			(this.controlType[(int)ControlType.BATTLE] && GameHandler.IsControlBattle()));
		if(GameHandler.IsControlBattle() && !this.controlType[(int)ControlType.BATTLE])
		{
			mode = false;
		}
		if(mode && (this.inPause || !GameHandler.IsGamePaused()) && 
			this.variables.CheckVariables())
		{
			this.eventStarted = true;
			this.gameEvent.StartEvent(this);
		}
	}
	
	public bool StartFromEvent(GameEvent e)
	{
		bool started = false;
		if(!this.eventStarted)
		{
			this.callingEvent = e;
			this.eventStarted = true;
			this.gameEvent.StartEvent(this);
			started = true;
		}
		return started;
	}
	
	public void EventFinished()
	{
		this.eventStarted = false;
		if(this.callingEvent != null)
		{
			this.callingEvent.GlobalEventFinished();
			this.callingEvent = null;
		}
		this.eventFinished = true;
	}
	
	public bool Call()
	{
		bool ok = false;
		if(this.fileOk && !this.eventStarted)
		{
			this.eventStarted = true;
			this.gameEvent.StartEvent(this);
			ok = true;
		}
		return ok;
	}
}
