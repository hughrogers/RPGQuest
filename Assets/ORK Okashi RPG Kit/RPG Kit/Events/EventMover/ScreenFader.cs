
using System.Collections;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
	private float time;
	private float time2;
	private Function interpolate;
	
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
	
	public Texture2D tex;
	
	private bool fadingScreen = false;
	private bool flash = false;
	
	public Color currentColor;
	
	public void FadeScreen(bool fa, float als, float ae, bool fr, float rs, float re,
			bool fg, float gs, float ge, bool fb, float bs, float be, EaseType et, float t)
	{
		StartCoroutine(FadeScreen2(fa, als, ae, fr, rs, re, fg, gs, ge, fb, bs, be, et, t));
	}
	
	private IEnumerator FadeScreen2(bool fa, float als, float ae, bool fr, float rs, float re,
			bool fg, float gs, float ge, bool fb, float bs, float be, EaseType et, float t)
	{
		this.fadingScreen = false;
		this.flash = false;
		yield return null;
		
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
		
		this.currentColor = new Color(0, 0, 0, 0);
		if(this.fadeAlpha) this.currentColor.a = Interpolate.Ease(this.interpolate, this.alphaStart, this.alphaDistance, this.time, this.time2);
		if(this.fadeRed) this.currentColor.r = Interpolate.Ease(this.interpolate, this.redStart, this.redDistance, this.time, this.time2);
		if(this.fadeGreen) this.currentColor.g = Interpolate.Ease(this.interpolate, this.greenStart, this.greenDistance, this.time, this.time2);
		if(this.fadeBlue) this.currentColor.b = Interpolate.Ease(this.interpolate, this.blueStart, this.blueDistance, this.time, this.time2);
		this.tex = new Texture2D(1, 1);
		this.tex.SetPixel(0, 0, this.currentColor);
		this.tex.Apply();
		DontDestroyOnLoad(this.tex);
		if(GUISystemType.ORK.Equals(DataHolder.GameSettings().guiSystemType))
		{
			GameHandler.GUIHandler().AddScreenFadeSprite(this);
		}
		this.fadingScreen = true;
	}
	
	public void FlashScreen(bool fa, float als, float ae, bool fr, float rs, float re,
			bool fg, float gs, float ge, bool fb, float bs, float be, EaseType et, float t)
	{
		StartCoroutine(FlashScreen2(fa, als, ae, fr, rs, re, fg, gs, ge, fb, bs, be, et, t));
	}
	
	private IEnumerator FlashScreen2(bool fa, float als, float ae, bool fr, float rs, float re,
			bool fg, float gs, float ge, bool fb, float bs, float be, EaseType et, float t)
	{
		this.fadingScreen = false;
		this.flash = false;
		yield return null;
		
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
		this.time2 = t/2;
		
		this.currentColor = new Color(0, 0, 0, 0);
		if(this.fadeAlpha) this.currentColor.a = Interpolate.Ease(this.interpolate, this.alphaStart, this.alphaDistance, this.time, this.time2);
		if(this.fadeRed) this.currentColor.r = Interpolate.Ease(this.interpolate, this.redStart, this.redDistance, this.time, this.time2);
		if(this.fadeGreen) this.currentColor.g = Interpolate.Ease(this.interpolate, this.greenStart, this.greenDistance, this.time, this.time2);
		if(this.fadeBlue) this.currentColor.b = Interpolate.Ease(this.interpolate, this.blueStart, this.blueDistance, this.time, this.time2);
		this.tex = new Texture2D(1, 1);
		this.tex.SetPixel(0, 0, this.currentColor);
		this.tex.Apply();
		DontDestroyOnLoad(this.tex);
		if(GUISystemType.ORK.Equals(DataHolder.GameSettings().guiSystemType))
		{
			GameHandler.GUIHandler().AddScreenFadeSprite(this);
		}
		this.fadingScreen = true;
		this.flash = true;
	}
	
	void Update()
	{
		if(this.fadingScreen && !GameHandler.IsGamePaused())
		{
			this.time += GameHandler.DeltaTime;
			if(this.fadeAlpha) this.currentColor.a = Interpolate.Ease(this.interpolate, this.alphaStart, this.alphaDistance, this.time, this.time2);
			if(this.fadeRed) this.currentColor.r = Interpolate.Ease(this.interpolate, this.redStart, this.redDistance, this.time, this.time2);
			if(this.fadeGreen) this.currentColor.g = Interpolate.Ease(this.interpolate, this.greenStart, this.greenDistance, this.time, this.time2);
			if(this.fadeBlue) this.currentColor.b = Interpolate.Ease(this.interpolate, this.blueStart, this.blueDistance, this.time, this.time2);
			this.tex.SetPixel(0, 0, this.currentColor);
			this.tex.Apply();
			if(this.time >= this.time2)
			{
				if(this.flash)
				{
					this.Revert(false);
				}
				else
				{
					this.fadingScreen = false;
					if((this.alphaStart + this.alphaDistance) == 0)
					{
						GameObject.Destroy(this.tex);
					}
				}
			}
		}
	}
	
	private void Revert(bool doFlash)
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
		
		this.time = 0;
		this.flash = doFlash;
	}
}