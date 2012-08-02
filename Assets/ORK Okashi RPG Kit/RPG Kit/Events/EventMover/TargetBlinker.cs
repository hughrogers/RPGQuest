
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class TargetBlinker : MonoBehaviour
{
	private EventFader fader = null;
	
	private float time;
	private float time2;
	private Function interpolate;
	
	private bool fadeChildren;
	private bool fadeAlpha;
	private float alphaDistance;
	private float alphaStart;
	private float alphaEnd;
	private bool fadeRed;
	private float redDistance;
	private float redStart;
	private float redEnd;
	private bool fadeGreen;
	private float greenDistance;
	private float greenStart;
	private float greenEnd;
	private bool fadeBlue;
	private float blueDistance;
	private float blueStart;
	private float blueEnd;
	
	public bool fading = false;
	private bool flash = false;
	private bool blink = false;
	
	private bool useCurrent = false;
	private int currentIndex = 0;
	private List<Color> currentColors = null;
	private bool reverting = false;
	
	public void Clear()
	{
		this.fading = false;
		this.flash = false;
		this.blink = false;
		this.reverting = false;
		this.useCurrent = false;
	}
	
	/*
	============================================================================
	Blink functions
	============================================================================
	*/
	public IEnumerator Blink(bool fc, bool fa, float als, float ae, bool fr, float rs, float re,
			bool fg, float gs, float ge, bool fb, float bs, float be, EaseType et, float t)
	{
		this.Clear();
		
		this.fadeChildren = fc;
		this.fadeAlpha = fa;
		this.alphaStart = als;
		this.alphaEnd = ae;
		this.alphaDistance = ae - als;
		this.fadeRed = fr;
		this.redStart = rs;
		this.redEnd = re;
		this.redDistance = re - rs;
		this.fadeGreen = fg;
		this.greenStart = gs;
		this.greenEnd = ge;
		this.greenDistance = ge - gs;
		this.fadeBlue = fb;
		this.blueStart = bs;
		this.blueEnd = be;
		this.blueDistance = be - bs;
		this.interpolate = Interpolate.Ease(et);
		this.time = 0;
		this.time2 = t;
		
		yield return null;
		this.fading = true;
		this.flash = true;
		this.blink = true;
	}
	
	public IEnumerator BlinkCurrent(bool fc, bool fa, float ae, bool fr, float re,
			bool fg, float ge, bool fb, float be, EaseType et, float t)
	{
		this.Clear();
		
		this.fadeChildren = fc;
		this.fadeAlpha = fa;
		this.alphaEnd = ae;
		this.fadeRed = fr;
		this.redEnd = re;
		this.fadeGreen = fg;
		this.greenEnd = ge;
		this.fadeBlue = fb;
		this.blueEnd = be;
		this.interpolate = Interpolate.Ease(et);
		this.time = 0;
		this.time2 = t;
		
		this.Store();
		
		yield return null;
		this.useCurrent = true;
		this.fading = true;
		this.flash = true;
		this.blink = true;
	}
	
	public void StopBlink()
	{
		if(this.blink)
		{
			if(this.useCurrent)
			{
				this.time = this.time2;
				this.reverting = true;
				this.currentIndex = 0;
				if(this.renderer)
				{
					this.DoFadeCurrent(this.renderer);
				}
				if(this.fadeChildren)
				{
					this.FadeChildrenCurrent(this.transform);
				}
			}
			else
			{
				float a = 1;
				float r = 1;
				float g = 1;
				float b = 1;
				if(this.flash)
				{
					a = this.alphaStart;
					r = this.redStart;
					g = this.greenStart;
					b = this.blueStart;
				}
				else
				{
					a = this.alphaEnd;
					r = this.redEnd;
					g = this.greenEnd;
					b = this.blueEnd;
				}
				
				if(this.renderer) this.DoFade(this.renderer, a, r, g, b);
				if(this.fadeChildren)
				{
					this.FadeChildren(this.transform, a, r, g, b);
				}
			}
			this.Clear();
		}
	}
	
	/*
	============================================================================
	Change functions
	============================================================================
	*/
	private bool CheckFader()
	{
		bool check = true;
		if(fader == null)
		{
			fader = this.GetComponent<EventFader>();
		}
		if(fader != null && fader.fading)
		{
			check = false;
		}
		return check;
	}
	
	void Update()
	{
		if(this.fading && !GameHandler.IsGamePaused() && this.CheckFader())
		{	
			this.time += GameHandler.DeltaTime;
			
			if(this.useCurrent)
			{
				this.currentIndex = 0;
				if(this.renderer)
				{
					this.DoFadeCurrent(this.renderer);
				}
				if(this.fadeChildren)
				{
					this.FadeChildrenCurrent(this.transform);
				}
			}
			else
			{
				float a = 1;
				float r = 1;
				float g = 1;
				float b = 1;
				if(this.fadeAlpha)
				{
					a = Interpolate.Ease(this.interpolate, this.alphaStart, this.alphaDistance, this.time, this.time2);
				}
				if(this.fadeRed)
				{
					 r = Interpolate.Ease(this.interpolate, this.redStart, this.redDistance, this.time, this.time2);
				}
				if(this.fadeGreen)
				{
					g = Interpolate.Ease(this.interpolate, this.greenStart, this.greenDistance, this.time, this.time2);
				}
				if(this.fadeBlue)
				{
					b = Interpolate.Ease(this.interpolate, this.blueStart, this.blueDistance, this.time, this.time2);
				}
				
				if(this.renderer)
				{
					this.DoFade(this.renderer, a, r, g, b);
				}
				if(this.fadeChildren)
				{
					this.FadeChildren(this.transform, a, r, g, b);
				}
			}
			
			if(this.time >= this.time2)
			{
				if(this.flash)
				{
					this.Revert(false);
				}
				else if(this.blink)
				{
					this.Revert(true);
				}
				else
				{
					this.fading = false;
					this.currentColors = null;
				}
			}
		}
	}
	
	/*
	============================================================================
	Child functions
	============================================================================
	*/
	private void FadeChildren(Transform p, float a, float r, float g, float b)
	{
		foreach(Transform child in p)
		{
			if(child.renderer)
			{
				this.DoFade(child.renderer, a, r, g, b);
			}
			this.FadeChildren(child, a, r, g, b);
		}
	}
	
	private void FadeChildrenCurrent(Transform p)
	{
		foreach(Transform child in p)
		{
			if(child.renderer)
			{
				this.DoFadeCurrent(child.renderer);
			}
			this.FadeChildrenCurrent(child);
		}
	}
	
	/*
	============================================================================
	Fading functions
	============================================================================
	*/
	private void DoFade(Renderer rend, float a, float r, float g, float b)
	{
		for(int i=0; i<rend.materials.Length; i++)
		{
			if(rend.materials[i].HasProperty("_Color"))
			{
				Color c = rend.materials[i].color;
				if(this.fadeAlpha) c.a = a;
				if(this.fadeRed) c.r = r;
				if(this.fadeGreen) c.g = g;
				if(this.fadeBlue) c.b = b;
				rend.materials[i].color = c;
			}
		}
	}
	
	private void DoFadeCurrent(Renderer rend)
	{
		for(int i=0; i<rend.materials.Length; i++)
		{
			if(rend.materials[i].HasProperty("_Color") && 
				this.currentIndex < this.currentColors.Count)
			{
				Color c = rend.materials[i].color;
				if(this.fadeAlpha)
				{
					if(this.reverting)
					{
						c.a = Interpolate.Ease(this.interpolate, 
								this.alphaEnd, 
								this.currentColors[this.currentIndex].a-this.alphaEnd, this.time, this.time2);
					}
					else
					{
						c.a = Interpolate.Ease(this.interpolate, 
								this.currentColors[this.currentIndex].a, 
								this.alphaEnd-this.currentColors[this.currentIndex].a, this.time, this.time2);
					}
				}
				if(this.fadeRed)
				{
					if(this.reverting)
					{
						c.r = Interpolate.Ease(this.interpolate, 
								this.redEnd, 
								this.currentColors[this.currentIndex].r-this.redEnd, this.time, this.time2);
					}
					else
					{
						c.r = Interpolate.Ease(this.interpolate, 
								this.currentColors[this.currentIndex].r, 
								this.redEnd-this.currentColors[this.currentIndex].r, this.time, this.time2);
					}
				}
				if(this.fadeGreen)
				{
					if(this.reverting)
					{
						c.g = Interpolate.Ease(this.interpolate, 
								this.greenEnd, 
								this.currentColors[this.currentIndex].g-this.greenEnd, this.time, this.time2);
					}
					else
					{
						c.g = Interpolate.Ease(this.interpolate, 
								this.currentColors[this.currentIndex].g, 
								this.greenEnd-this.currentColors[this.currentIndex].g, this.time, this.time2);
					}
				}
				if(this.fadeBlue)
				{
					if(this.reverting)
					{
						c.b = Interpolate.Ease(this.interpolate, 
								this.blueEnd, 
								this.currentColors[this.currentIndex].b-this.blueEnd, this.time, this.time2);
					}
					else
					{
						c.b = Interpolate.Ease(this.interpolate, 
								this.currentColors[this.currentIndex].b, 
								this.blueEnd-this.currentColors[this.currentIndex].b, this.time, this.time2);
					}
				}
				rend.materials[i].color = c;
				this.currentIndex++;
			}
		}
	}
	
	/*
	============================================================================
	Revert functions
	============================================================================
	*/
	private void Revert(bool doFlash)
	{
		if(this.useCurrent)
		{
			this.reverting = !doFlash;
		}
		else
		{
			float tmp = this.alphaStart;
			this.alphaStart = this.alphaEnd;
			this.alphaEnd = tmp;
			this.alphaDistance = this.alphaEnd - this.alphaStart;
			tmp = this.redStart;
			this.redStart = this.redEnd;
			this.redEnd = tmp;
			this.redDistance = this.redEnd - this.redStart;
			tmp = this.blueStart;
			this.blueStart = this.blueEnd;
			this.blueEnd = tmp;
			this.blueDistance = this.blueEnd - this.blueStart;
			tmp = this.greenStart;
			this.greenStart = this.greenEnd;
			this.greenEnd = tmp;
			this.greenDistance = this.greenEnd - this.greenStart;
		}
		
		this.time = 0;
		this.flash = doFlash;
	}
	
	/*
	============================================================================
	Store functions
	============================================================================
	*/
	private void Store()
	{
		if(this.currentColors == null)
		{
			this.currentColors = new List<Color>();
			if(this.renderer) this.StoreCurrent(this.renderer);
			if(this.fadeChildren)
			{
				this.StoreChilds(this.transform);
			}
		}
	}
	
	private void StoreChilds(Transform p)
	{
		foreach(Transform child in p)
		{
			if(child.renderer)
			{
				this.StoreCurrent(child.renderer);
			}
			this.StoreChilds(child);
		}
	}
	
	private void StoreCurrent(Renderer rend)
	{
		for(int i=0; i<rend.materials.Length; i++)
		{
			if(rend.materials[i].HasProperty("_Color"))
			{
				this.currentColors.Add(rend.materials[i].color);
			}
		}
	}
}