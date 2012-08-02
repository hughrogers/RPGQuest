
using UnityEngine;

public class BattleText : MonoBehaviour
{
	public string text;
	
	public BattleTextSettings settings;
	
	public Color color;
	public Color shadowColor;
	
	protected float time = 0;
	public float alpha = 1;
	
	protected float inDistance;
	protected float outDistance;
	protected Function interpolateIn;
	protected Function interpolateOut;
	
	protected float xSpeed = 0;
	protected float ySpeed = 0;
	protected float zSpeed = 0;
	
	protected Camera cam;
	
	protected GameObject combatant;
	
	// count to value
	protected bool counting = false;
	protected int value = 0;
	protected int currentValue = 0;
	protected int startValue = 0;
	protected int countDistance = 0;
	protected Function countInterpolate;
	
	public void ShowNumber(int i, GameObject c, BattleTextSettings ts)
	{
		this.combatant = c;
		this.settings = ts;
		if(this.InitSettings())
		{
			this.text = this.settings.text[GameHandler.GetLanguage()];
			this.value = i;
			this.startValue = (int)((this.value/100.0f)*this.settings.startCountFrom);
			this.countDistance = this.value - this.startValue;
			this.countInterpolate = Interpolate.Ease(this.settings.countInterpolate);
			this.cam = Camera.main;
			this.counting = true;
		}
	}
	
	public void ShowText(string t, GameObject c, BattleTextSettings ts)
	{
		this.combatant = c;
		this.settings = ts;
		if(this.InitSettings())
		{
			this.text = t;
			this.cam = Camera.main;
			if(!GameHandler.GetLevelHandler().useGUILayout) GameHandler.GUIHandler().AddBattleTextSprite(this);
		}
	}
	
	private bool InitSettings()
	{
		bool init = false;
		if(this.settings != null && this.settings.active)
		{
			this.color = DataHolder.Color(this.settings.color);
			if(this.settings.showShadow) this.shadowColor = DataHolder.Color(this.settings.shadowColor);
			
			if(this.settings.fadeIn)
			{
				this.alpha = this.settings.fadeInStart;
				this.color.a = this.alpha;
				if(this.settings.showShadow) this.shadowColor.a = this.alpha;
				this.inDistance = this.settings.fadeInEnd - this.settings.fadeInStart;
				this.interpolateIn = Interpolate.Ease(this.settings.fadeInInterpolate);
			}
			
			if(this.settings.fadeOut)
			{
				this.outDistance = this.settings.fadeOutEnd - this.settings.fadeOutStart;
				this.interpolateOut = Interpolate.Ease(this.settings.fadeOutInterpolate);
			}
			
			if(this.settings.localSpace)
			{
				this.transform.localPosition += this.settings.baseOffset+new Vector3(
						Random.Range(this.settings.randomOffsetFrom.x, this.settings.randomOffsetTo.x),
						Random.Range(this.settings.randomOffsetFrom.y, this.settings.randomOffsetTo.y),
						Random.Range(this.settings.randomOffsetFrom.z, this.settings.randomOffsetTo.z));
			}
			else
			{
				this.transform.position += this.settings.baseOffset+new Vector3(
						Random.Range(this.settings.randomOffsetFrom.x, this.settings.randomOffsetTo.x),
						Random.Range(this.settings.randomOffsetFrom.y, this.settings.randomOffsetTo.y),
						Random.Range(this.settings.randomOffsetFrom.z, this.settings.randomOffsetTo.z));
			}
			
			if(this.settings.animate)
			{
				if(this.settings.xRandom) this.xSpeed = Random.Range(this.settings.xMin, this.settings.xMax);
				else this.xSpeed = this.settings.xSpeed;
				if(this.settings.yRandom) this.ySpeed = Random.Range(this.settings.yMin, this.settings.yMax);
				else this.ySpeed = this.settings.ySpeed;
				if(this.settings.zRandom) this.zSpeed = Random.Range(this.settings.zMin, this.settings.zMax);
				else this.zSpeed = this.settings.zSpeed;
			}
			
			if(this.settings.flash)
			{
				EventFader comp = (EventFader)this.combatant.GetComponent("EventFader");
				if(comp == null)
				{
					comp = (EventFader)this.combatant.AddComponent("EventFader");
				}
				if(this.settings.fromCurrent)
				{
					comp.StartCoroutine(comp.FlashCurrent(this.settings.flashChildren, this.settings.flashAlpha, this.settings.alphaEnd, 
							this.settings.flashRed, this.settings.redEnd, this.settings.flashGreen, this.settings.greenEnd, 
							this.settings.flashBlue, this.settings.blueEnd, this.settings.flashInterpolate, this.settings.flashTime, 
							false, "_Color", false));
				}
				else
				{
					comp.StartCoroutine(comp.Flash(this.settings.flashChildren, this.settings.flashAlpha, this.settings.alphaStart, this.settings.alphaEnd, 
							this.settings.flashRed, this.settings.redStart, this.settings.redEnd, this.settings.flashGreen, 
							this.settings.greenStart, this.settings.greenEnd, this.settings.flashBlue, this.settings.blueStart, 
							this.settings.blueEnd, this.settings.flashInterpolate, this.settings.flashTime, 
							false, "_Color", false));
				}
			}
			init = true;
		}
		else
		{
			GameObject.Destroy(this.gameObject);
		}
		return init;
	}
	
	public Vector3 GetScreenPosition()
	{
		Vector3 pos = this.cam.WorldToScreenPoint(this.transform.position);
		pos.y = Screen.height - pos.y;
		return pos;
	}
	
	void Update()
	{
		if(this.text != null && this.cam != null && 
			this.settings != null && this.time < this.settings.visibleTime)
		{
			float t = GameHandler.DeltaTime;
			this.time += t;
			if(this.interpolateIn != null)
			{
				if(this.time <= this.settings.fadeInTime)
				{
					this.alpha = Interpolate.Ease(this.interpolateIn, this.settings.fadeInStart, this.inDistance, this.time, this.settings.fadeInTime);
				}
				if(this.time >= this.settings.fadeInTime)
				{
					this.alpha = this.settings.fadeInEnd;
					this.interpolateIn = null;
				}
			}
			else if(this.interpolateOut != null)
			{
				if(this.time >= (this.settings.visibleTime-this.settings.fadeOutTime))
				{
					this.alpha = Interpolate.Ease(this.interpolateOut, this.settings.fadeOutStart, this.outDistance, 
							this.time-(this.settings.visibleTime-this.settings.fadeOutTime), this.settings.fadeOutTime);
				}
				if(this.time >= this.settings.visibleTime)
				{
					this.alpha = this.settings.fadeOutEnd;
					this.interpolateOut = null;
				}
			}
			
			if(this.countInterpolate != null)
			{
				if(this.time <= this.settings.countTime)
				{
					this.currentValue = (int)Interpolate.Ease(this.countInterpolate, this.startValue, this.countDistance, this.time, this.settings.countTime);
				}
				if(this.time >= this.settings.countTime)
				{
					this.currentValue = this.value;
					this.countInterpolate = null;
					this.text = this.text.Replace("%", this.currentValue.ToString());
					this.counting = false;
				}
			}
			
			this.color.a = this.alpha;
			if(this.settings.showShadow) this.shadowColor.a = this.alpha;
			
			if(this.settings.animate)
			{
				Vector3 p = this.transform.position;
				if(this.xSpeed != 0) p.x += this.xSpeed*t;
				if(this.ySpeed != 0) p.y += this.ySpeed*t;
				if(this.zSpeed != 0) p.z += this.zSpeed*t;
				this.transform.position = p;
			}
		}
		else if(this.time >= this.settings.visibleTime)
		{
			GameObject.Destroy(this.gameObject);
		}
	}
}