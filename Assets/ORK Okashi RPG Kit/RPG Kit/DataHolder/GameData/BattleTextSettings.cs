
using System.Collections;
using UnityEngine;

public class BattleTextSettings
{
	public bool active = false;
	
	public string[] text = new string[0];
	
	public int color = 0;
	public bool showShadow = true;
	public int shadowColor = 1;
	public Vector2 shadowOffset = new Vector2(1, 1);
	
	public float visibleTime = 2.5f;
	
	public bool countToValue = false;
	public float startCountFrom = 0;
	public EaseType countInterpolate = EaseType.Linear;
	public float countTime = 1.0f;
	
	// position
	public string posChild = "";
	public bool localSpace = false;
	public Vector3 baseOffset = Vector3.zero;
	public Vector3 randomOffsetFrom = Vector3.zero;
	public Vector3 randomOffsetTo = Vector3.zero;
	
	// prefab
	public bool spawnPrefab = false;
	public string prefabName = "";
	public GameObject prefab = null;
	public float prefabTime = 0;
	public string prefabChild = "";
	public Vector3 prefabOffset = Vector3.zero;
	
	// audio
	public bool playAudio = false;
	public string audioName = "";
	public AudioClip audioClip = null;
	
	// fade
	public bool fadeIn = true;
	public float fadeInTime = 0.25f;
	public float fadeInStart = 0;
	public float fadeInEnd = 1;
	public EaseType fadeInInterpolate = EaseType.Linear;
	
	public bool fadeOut = true;
	public float fadeOutTime = 0.75f;
	public float fadeOutStart = 1;
	public float fadeOutEnd = 0;
	public EaseType fadeOutInterpolate = EaseType.Linear;
	
	// animation
	public bool animate = false;
	public bool xRandom = false;
	public float xSpeed = 0;
	public float xMin = 0;
	public float xMax = 1;
	
	public bool yRandom = false;
	public float ySpeed = 0;
	public float yMin = 0;
	public float yMax = 1;
	
	public bool zRandom = false;
	public float zSpeed = 0;
	public float zMin = 0;
	public float zMax = 1;
	
	// flash
	public bool flash = false;
	public bool fromCurrent = false;
	public bool flashChildren = false;
	public float flashTime = 0.1f;
	public EaseType flashInterpolate = EaseType.Linear;
	
	public bool flashAlpha = false;
	public float alphaStart = 1;
	public float alphaEnd = 1;
	
	public bool flashRed = false;
	public float redStart = 1;
	public float redEnd = 1;
	
	public bool flashGreen = false;
	public float greenStart = 1;
	public float greenEnd = 1;
	
	public bool flashBlue = false;
	public float blueStart = 1;
	public float blueEnd = 1;
	
	// XML
	private static string TEXTOPT = "textopt";
	private static string FLASH = "flash";
	private static string ANIMATE = "animate";
	private static string RANDOMOFFSET = "randomoffset";
	private static string BASEOFFSET = "baseoffset";
	private static string POSCHILD = "poschild";
	private static string PREFAB = "prefab";
	private static string PREFABCHILD = "prefabchild";
	private static string AUDIO = "audio";
	
	public BattleTextSettings()
	{
		this.InitTexts("%");
	}
	
	public BattleTextSettings(string txt)
	{
		this.InitTexts(txt);
	}
	
	public BattleTextSettings(Hashtable ht)
	{
		this.InitTexts("%");
		this.SetData(ht);
	}
	
	public void InitTexts(string txt)
	{
		this.text = new string[DataHolder.Languages().GetDataCount()];
		for(int i=0; i<this.text.Length; i++)
		{
			this.text[i] = txt;
		}
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(string name)
	{
		Hashtable ht = HashtableHelper.GetTitleHashtable(name);
		if(this.active)
		{
			ht.Add("color", this.color.ToString());
			if(this.showShadow)
			{
				ht.Add("shadowcolor", this.shadowColor.ToString());
				ht.Add("x", this.shadowOffset.x.ToString());
				ht.Add("y", this.shadowOffset.y.ToString());
			}
			ht.Add("visibletime", this.visibleTime.ToString());
			
			if(this.countToValue)
			{
				ht.Add("countinterpolate", this.countInterpolate.ToString());
				ht.Add("counttime", this.countTime.ToString());
				ht.Add("startcountfrom", this.startCountFrom.ToString());
			}
			
			if(this.fadeIn)
			{
				ht.Add("fadeintime", this.fadeInTime.ToString());
				ht.Add("fadeinstart", this.fadeInStart.ToString());
				ht.Add("fadeinend", this.fadeInEnd.ToString());
				ht.Add("fadeininterpolate", this.fadeInInterpolate.ToString());
			}
			if(this.fadeOut)
			{
				ht.Add("fadeouttime", this.fadeOutTime.ToString());
				ht.Add("fadeoutstart", this.fadeOutStart.ToString());
				ht.Add("fadeoutend", this.fadeOutEnd.ToString());
				ht.Add("fadeoutinterpolate", this.fadeOutInterpolate.ToString());
			}
			
			if(this.localSpace) ht.Add("localspace", "true");
			
			ArrayList s = new ArrayList();
			Hashtable ht2 = HashtableHelper.GetTitleHashtable(BattleTextSettings.BASEOFFSET);
			ht2.Add("x", this.baseOffset.x.ToString());
			ht2.Add("y", this.baseOffset.y.ToString());
			ht2.Add("z", this.baseOffset.z.ToString());
			s.Add(ht2);
			
			ht2 = HashtableHelper.GetTitleHashtable(BattleTextSettings.RANDOMOFFSET);
			ht2.Add("fx", this.randomOffsetFrom.x.ToString());
			ht2.Add("fy", this.randomOffsetFrom.y.ToString());
			ht2.Add("fz", this.randomOffsetFrom.z.ToString());
			ht2.Add("tx", this.randomOffsetTo.x.ToString());
			ht2.Add("ty", this.randomOffsetTo.y.ToString());
			ht2.Add("tz", this.randomOffsetTo.z.ToString());
			s.Add(ht2);
			
			if(this.animate)
			{
				ht2 = HashtableHelper.GetTitleHashtable(BattleTextSettings.ANIMATE);
				if(this.xRandom)
				{
					ht2.Add("xmin", this.xMin.ToString());
					ht2.Add("xmax", this.xMax.ToString());
				}
				else
				{
					ht2.Add("xspeed", this.xSpeed.ToString());
				}
				if(this.yRandom)
				{
					ht2.Add("ymin", this.yMin.ToString());
					ht2.Add("ymax", this.yMax.ToString());
				}
				else
				{
					ht2.Add("yspeed", this.ySpeed.ToString());
				}
				if(this.zRandom)
				{
					ht2.Add("zmin", this.zMin.ToString());
					ht2.Add("zmax", this.zMax.ToString());
				}
				else
				{
					ht2.Add("zspeed", this.zSpeed.ToString());
				}
				s.Add(ht2);
			}
			if(this.flash)
			{
				ht2 = HashtableHelper.GetTitleHashtable(BattleTextSettings.FLASH);
				ht2.Add("fromcurrent", "true");
				ht2.Add("flashtime", this.flashTime.ToString());
				ht2.Add("children", this.flashChildren.ToString());
				ht2.Add("flashinterpolate", this.flashInterpolate.ToString());
				if(this.flashAlpha)
				{
					ht2.Add("alphastart", this.alphaStart.ToString());
					ht2.Add("alphaend", this.alphaEnd.ToString());
				}
				if(this.flashRed)
				{
					ht2.Add("redstart", this.redStart.ToString());
					ht2.Add("redend", this.redEnd.ToString());
				}
				if(this.flashGreen)
				{
					ht2.Add("greenstart", this.greenStart.ToString());
					ht2.Add("greenend", this.greenEnd.ToString());
				}
				if(this.flashBlue)
				{
					ht2.Add("bluestart", this.blueStart.ToString());
					ht2.Add("blueend", this.blueEnd.ToString());
				}
				s.Add(ht2);
			}
			for(int i=0; i<this.text.Length; i++)
			{
				s.Add(HashtableHelper.GetContentHashtable(BattleTextSettings.TEXTOPT, this.text[i], i));
			}
			if(this.posChild != "") s.Add(HashtableHelper.GetContentHashtable(BattleTextSettings.POSCHILD, this.posChild));
			
			if(this.spawnPrefab)
			{
				ht2 = HashtableHelper.GetContentHashtable(BattleTextSettings.PREFAB, this.prefabName);
				ht2.Add("time", this.prefabTime.ToString());
				VectorHelper.ToHashtable(ref ht2, this.prefabOffset);
				s.Add(ht2);
				
				if(this.prefabChild != "")
				{
					s.Add(HashtableHelper.GetContentHashtable(BattleTextSettings.PREFABCHILD, this.prefabChild));
				}
			}
			if(this.playAudio)
			{
				s.Add(HashtableHelper.GetContentHashtable(BattleTextSettings.AUDIO, this.audioName));
			}
			
			ht.Add(XMLHandler.NODES, s);
		}
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		if(ht.ContainsKey("color"))
		{
			this.active = true;
			this.color = int.Parse((string)ht["color"]);
			if(ht.ContainsKey("shadowcolor"))
			{
				this.showShadow = true;
				this.shadowColor = int.Parse((string)ht["shadowcolor"]);
				this.shadowOffset.x = int.Parse((string)ht["x"]);
				this.shadowOffset.y = int.Parse((string)ht["y"]);
			}
			this.visibleTime = float.Parse((string)ht["visibletime"]);
			
			if(ht.ContainsKey("countinterpolate"))
			{
				this.countToValue = true;
				this.countInterpolate = (EaseType)System.Enum.Parse(typeof(EaseType), (string)ht["countinterpolate"]);
				this.countTime = float.Parse((string)ht["counttime"]);
				this.startCountFrom = float.Parse((string)ht["startcountfrom"]);
			}
			
			if(ht.ContainsKey("fadeintime"))
			{
				this.fadeIn = true;
				this.fadeInTime = float.Parse((string)ht["fadeintime"]);
				this.fadeInStart = float.Parse((string)ht["fadeinstart"]);
				this.fadeInEnd = float.Parse((string)ht["fadeinend"]);
				this.fadeInInterpolate = (EaseType)System.Enum.Parse(typeof(EaseType), (string)ht["fadeininterpolate"]);
			}
			else this.fadeIn = false;
			if(ht.ContainsKey("fadeouttime"))
			{
				this.fadeOut = true;
				this.fadeOutTime = float.Parse((string)ht["fadeouttime"]);
				this.fadeOutStart = float.Parse((string)ht["fadeoutstart"]);
				this.fadeOutEnd = float.Parse((string)ht["fadeoutend"]);
				this.fadeOutInterpolate =(EaseType) System.Enum.Parse(typeof(EaseType), (string)ht["fadeoutinterpolate"]);
			}
			else this.fadeOut = false;
			
			if(ht.ContainsKey("localspace")) this.localSpace = true;
			
			if(ht.ContainsKey(XMLHandler.NODES))
			{
				ArrayList s = ht[XMLHandler.NODES] as ArrayList;
				foreach(Hashtable ht2 in s)
				{
					if(ht2[XMLHandler.NODE_NAME] as string == BattleTextSettings.BASEOFFSET)
					{
						this.baseOffset.x = float.Parse((string)ht2["x"]);
						this.baseOffset.y = float.Parse((string)ht2["y"]);
						this.baseOffset.z = float.Parse((string)ht2["z"]);
					}
					else if(ht2[XMLHandler.NODE_NAME] as string == BattleTextSettings.RANDOMOFFSET)
					{
						this.randomOffsetFrom.x = float.Parse((string)ht2["fx"]);
						this.randomOffsetFrom.y = float.Parse((string)ht2["fy"]);
						this.randomOffsetFrom.z = float.Parse((string)ht2["fz"]);
						this.randomOffsetTo.x = float.Parse((string)ht2["tx"]);
						this.randomOffsetTo.y = float.Parse((string)ht2["ty"]);
						this.randomOffsetTo.z = float.Parse((string)ht2["tz"]);
					}
					else if(ht2[XMLHandler.NODE_NAME] as string == BattleTextSettings.ANIMATE)
					{
						this.animate = true;
						if(ht2.ContainsKey("xspeed")) this.xSpeed = float.Parse((string)ht2["xspeed"]);
						else
						{
							this.xRandom = true;
							this.xMin = float.Parse((string)ht2["xmin"]);
							this.xMax = float.Parse((string)ht2["xmax"]);
						}
						if(ht2.ContainsKey("yspeed")) this.ySpeed = float.Parse((string)ht2["yspeed"]);
						else
						{
							this.yRandom = true;
							this.yMin = float.Parse((string)ht2["ymin"]);
							this.yMax = float.Parse((string)ht2["ymax"]);
						}
						if(ht2.ContainsKey("zspeed")) this.zSpeed = float.Parse((string)ht2["zspeed"]);
						else
						{
							this.zRandom = true;
							this.zMin = float.Parse((string)ht2["zmin"]);
							this.zMax = float.Parse((string)ht2["zmax"]);
						}
					}
					else if(ht2[XMLHandler.NODE_NAME] as string == BattleTextSettings.FLASH)
					{
						this.flash = true;
						this.flashTime = float.Parse((string)ht2["flashtime"]);
						this.flashChildren = bool.Parse((string)ht2["children"]);
						this.flashInterpolate = (EaseType)System.Enum.Parse(typeof(EaseType), (string)ht2["flashinterpolate"]);
						if(ht2.ContainsKey("fromcurrent")) this.fromCurrent = true;
						if(ht2.ContainsKey("alphastart"))
						{
							this.flashAlpha = true;
							this.alphaStart = float.Parse((string)ht2["alphastart"]);
							this.alphaEnd = float.Parse((string)ht2["alphaend"]);
						}
						if(ht2.ContainsKey("redstart"))
						{
							this.flashRed = true;
							this.redStart = float.Parse((string)ht2["redstart"]);
							this.redEnd = float.Parse((string)ht2["redend"]);
						}
						if(ht2.ContainsKey("greenstart"))
						{
							this.flashGreen = true;
							this.greenStart = float.Parse((string)ht2["greenstart"]);
							this.greenEnd = float.Parse((string)ht2["greenend"]);
						}
						if(ht2.ContainsKey("bluestart"))
						{
							this.flashBlue = true;
							this.blueStart = float.Parse((string)ht2["bluestart"]);
							this.blueEnd = float.Parse((string)ht2["blueend"]);
						}
					}
					else if(ht2[XMLHandler.NODE_NAME] as string == BattleTextSettings.TEXTOPT)
					{
						int id = int.Parse((string)ht2["id"]);
						if(id < this.text.Length)
						{
							this.text[id] = ht2[XMLHandler.CONTENT] as string;
						}
					}
					else if(ht2[XMLHandler.NODE_NAME] as string == BattleTextSettings.POSCHILD)
					{
						this.posChild = ht2[XMLHandler.CONTENT] as string;
					}
					else if(ht2[XMLHandler.NODE_NAME] as string == BattleTextSettings.PREFAB)
					{
						this.spawnPrefab = true;
						this.prefabName = ht2[XMLHandler.CONTENT] as string;
						this.prefabTime = float.Parse((string)ht2["time"]);
						this.prefabOffset = VectorHelper.FromHashtable(ht2);
					}
					else if(ht2[XMLHandler.NODE_NAME] as string == BattleTextSettings.PREFABCHILD)
					{
						this.prefabChild = ht2[XMLHandler.CONTENT] as string;
					}
					else if(ht2[XMLHandler.NODE_NAME] as string == BattleTextSettings.AUDIO)
					{
						this.playAudio = true;
						this.audioName = ht2[XMLHandler.CONTENT] as string;
					}
				}
			}
		}
		else
		{
			this.active = false;
		}
	}
	
	/*
	============================================================================
	Text functions
	============================================================================
	*/
	public bool IsShowNumber()
	{
		return this.countToValue && GUISystemType.UNITY.Equals(DataHolder.GameSettings().guiSystemType);
	}
	
	public void ShowText(string t, Combatant combatant)
	{
		if(this.active && combatant.prefabInstance)
		{
			string txt = this.text[GameHandler.GetLanguage()];
			if(t != "") txt = txt.Replace("%", t);
			
			this.CreateObjects(combatant).ShowText(txt, combatant.prefabInstance, this);
		}
	}
	
	public void ShowNumber(int i, Combatant combatant)
	{
		if(this.active && combatant.prefabInstance)
		{
			this.CreateObjects(combatant).ShowNumber(i, combatant.prefabInstance, this);
		}
	}
	
	private BattleText CreateObjects(Combatant combatant)
	{
		BattleText bt = null;
		GameObject txtObj = new GameObject();
		Transform trans = TransformHelper.GetChild(this.posChild, combatant.prefabInstance.transform);
		txtObj.transform.position = trans.position;
		if(DataHolder.BattleSystemData().mountTexts)
		{
			txtObj.transform.parent = trans;
		}
		if(GUISystemType.ORK.Equals(DataHolder.GameSettings().guiSystemType))
			bt = txtObj.AddComponent<BattleText>();
		else
			bt = (BattleTextGUI)txtObj.AddComponent<BattleTextGUI>();
		
		if(this.spawnPrefab)
		{
			GameObject pref = this.GetPrefab();
			if(pref != null)
			{
				trans = TransformHelper.GetChild(this.prefabChild, combatant.prefabInstance.transform);
				pref.transform.position = trans.position+trans.TransformDirection(this.prefabOffset);
				pref.transform.rotation = trans.rotation;
				if(DataHolder.BattleSystemData().mountTexts)
				{
					pref.transform.parent = trans;
				}
				if(this.prefabTime > 0)
				{
					DestroyAfterTime comp = pref.AddComponent<DestroyAfterTime>();
					comp.time = this.prefabTime;
				}
			}
		}
		if(this.playAudio)
		{
			AudioSource s = combatant.GetAudioSource();
			if(s != null)
			{
				s.PlayOneShot(this.GetAudioClip());
			}
		}
		return bt;
	}
	
	public GameObject GetPrefab()
	{
		GameObject obj = null;
		if(this.prefab == null)
		{
			this.prefab = (GameObject)Resources.Load(BattleSystemData.PREFAB_PATH+this.prefabName, typeof(GameObject));
		}
		if(this.prefab != null) obj = (GameObject)GameObject.Instantiate(this.prefab);
		return obj;
	}
	
	public AudioClip GetAudioClip()
	{
		if(this.audioClip == null && this.audioName != "")
		{
			this.audioClip = (AudioClip)Resources.Load(BattleSystemData.AUDIO_PATH+
					this.audioName, typeof(AudioClip));
		}
		return this.audioClip;
	}
}