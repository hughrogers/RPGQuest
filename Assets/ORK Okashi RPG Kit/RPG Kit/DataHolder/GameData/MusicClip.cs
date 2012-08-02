
using UnityEngine;

public class MusicClip
{
	public string clipName = "";
	public float maxVolume = 1;
	public bool loop = false;
	
	public float[] checkTime = new float[0];
	public float[] setTime = new float[0];
	
	// ingame
	public int realID = 0;
	private AudioClip clip;
	public int currentLoop = 0;
	
	// fading
	public float time = 0;
	public float time2 = 0;
	public Function interpolate;
	public bool fadeIn = false;
	public bool fadeOut = false;
	
	public MusicClip()
	{
		
	}
	
	public void AddLoop()
	{
		this.checkTime = ArrayHelper.Add(0, this.checkTime);
		this.setTime = ArrayHelper.Add(0, this.setTime);
	}
	
	public void RemoveLoop(int index)
	{
		this.checkTime = ArrayHelper.Remove(index, this.checkTime);
		this.setTime = ArrayHelper.Remove(index, this.setTime);
	}
	
	public AudioClip GetClip()
	{
		if(!this.clip && "" != this.clipName)
		{
			this.clip = (AudioClip)Resources.Load(DataHolder.Music().clipPath+this.clipName, typeof(AudioClip));
		}
		return clip;
	}
	
	public bool HasLoops()
	{
		return this.checkTime.Length > 0;
	}
	
	public void NextLoop()
	{
		this.currentLoop++;
		if(this.currentLoop >= this.checkTime.Length) this.currentLoop = 0;
	}
	
	public void CheckLoop(AudioSource audio)
	{
		if(this.checkTime.Length > 0 && audio.time >= this.checkTime[this.currentLoop])
		{
			audio.time = this.setTime[this.currentLoop];
			this.NextLoop();
		}
	}
	
	// fading
	public void FadeIn(float t, EaseType type)
	{
		this.fadeOut = false;
		this.time = 0;
		this.time2 = t;
		this.interpolate = Interpolate.Ease(type);
		this.fadeIn = true;
	}
	
	public void FadeOut(float t, EaseType type)
	{
		this.fadeIn = false;
		this.time = 0;
		this.time2 = t;
		this.interpolate = Interpolate.Ease(type);
		this.fadeOut = true;
	}
	
	public void DoFade(float t, AudioSource audio)
	{
		if(this.fadeIn)
		{
			this.time += t;
			audio.volume = Interpolate.Ease(this.interpolate, 0, this.maxVolume, this.time, this.time2);
			if(this.time >= this.time2)
			{
				this.fadeIn = false;
			}
		}
		else if(this.fadeOut)
		{
			this.time += t;
			audio.volume = Interpolate.Ease(this.interpolate, this.maxVolume, 0-this.maxVolume, this.time, this.time2);
			if(this.time >= this.time2)
			{
				this.fadeOut = false;
				audio.Stop();
			}
		}
	}
}