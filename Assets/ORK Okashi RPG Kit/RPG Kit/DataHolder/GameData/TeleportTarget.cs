
using UnityEngine;
using System.Collections;

public class TeleportTarget
{
	// conditions
	public VariableCondition variables = new VariableCondition();
	
	// teleport settings
	public string sceneName = "";
	public int spawnID = 0;
	
	public bool fadeOut = true;
	public float fadeOutTime = 0.5f;
	public EaseType fadeOutInterpolate = EaseType.Linear;
	
	public bool fadeIn = true;
	public float fadeInTime = 0.5f;
	public EaseType fadeInInterpolate = EaseType.Linear;
	
	// XML
	private static string SCENE = "scene";
	private static string VARIABLES = "variables";
	
	public TeleportTarget()
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
		ht.Add("spawn", this.spawnID.ToString());
		if(this.fadeOut)
		{
			ht.Add("outtime", this.fadeOutTime.ToString());
			ht.Add("outtype", this.fadeOutInterpolate.ToString());
		}
		if(this.fadeIn)
		{
			ht.Add("intime", this.fadeInTime.ToString());
			ht.Add("intype", this.fadeInInterpolate.ToString());
		}
		
		if(this.sceneName != "")
		{
			s.Add(HashtableHelper.GetContentHashtable(TeleportTarget.SCENE, this.sceneName));
		}
		s.Add(this.variables.GetData(TeleportTarget.VARIABLES));
		ht.Add(XMLHandler.NODES, s);
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		this.spawnID = int.Parse((string)ht["spawn"]);
		
		if(ht.ContainsKey("outtime"))
		{
			this.fadeOutTime = float.Parse((string)ht["outtime"]);
			this.fadeOutInterpolate = (EaseType)System.Enum.Parse(
					typeof(EaseType), (string)ht["outtype"]);
		}
		if(ht.ContainsKey("intime"))
		{
			this.fadeInTime = float.Parse((string)ht["intime"]);
			this.fadeInInterpolate = (EaseType)System.Enum.Parse(
					typeof(EaseType), (string)ht["intype"]);
		}
		
		if(ht.ContainsKey(XMLHandler.NODES))
		{
			ArrayList s = ht[XMLHandler.NODES] as ArrayList;
			foreach(Hashtable ht2 in s)
			{
				if(ht2[XMLHandler.NODE_NAME] as string == TeleportTarget.SCENE)
				{
					this.sceneName = ht2[XMLHandler.CONTENT] as string;
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == TeleportTarget.VARIABLES)
				{
					this.variables.SetData(ht2);
				}
			}
		}
	}
	
	public TeleportTarget GetCopy()
	{
		TeleportTarget tt = new TeleportTarget();
		tt.SetData(this.GetData(new Hashtable()));
		return tt;
	}
	
	/*
	============================================================================
	Teleport functions
	============================================================================
	*/
	public void Use()
	{
		GameObject tmp = new GameObject();
		SceneChanger changer = tmp.AddComponent<SceneChanger>();
		if(changer != null)
		{
			changer.sceneName = this.sceneName;
			changer.spawnID = this.spawnID;
			changer.fadeOut = this.fadeOut;
			changer.fadeOutTime = this.fadeOutTime;
			changer.fadeOutInterpolate = this.fadeOutInterpolate;
			changer.fadeIn = this.fadeIn;
			changer.fadeInTime = this.fadeInTime;
			changer.fadeInInterpolate = this.fadeInInterpolate;
			changer.ChangeScene();
		}
	}
	
	public bool Available()
	{
		return this.variables.CheckVariables();
	}
}
