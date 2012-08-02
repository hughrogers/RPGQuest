
using System.Collections;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
	private GameObject fadeA;
	private AudioSource sourceA;
	
	private GameObject fadeB;
	private AudioSource sourceB;
	
	// 0 = none, 1 = sourceA, 2 = sourceB, 3 = A to B, 4 = B to A
	private int currentPlaying = 0;
	private bool ticking = false;
	
	private MusicClip currentMusic;
	private MusicClip lastMusic;
	
	private MusicClip storedMusic;
	private float storedTime = 0;
	
	public bool IsPlaying()
	{
		return this.currentPlaying > 0;
	}
	
	public int GetCurrentID()
	{
		int id = -1;
		if(this.currentMusic != null) id = this.currentMusic.realID;
		return id;
	}
	
	public float GetCurrentTime()
	{
		float t = -1;
		if(this.sourceA && (this.currentPlaying == 1 || this.currentPlaying == 4))
		{
			t = this.sourceA.time;
		}
		else if(this.sourceB && (this.currentPlaying == 2 || this.currentPlaying == 3))
		{
			t = this.sourceB.time;
		}
		return t;
	}
	
	void Awake()
	{
		DontDestroyOnLoad(transform);
		this.fadeA = new GameObject();
		this.sourceA = (AudioSource)this.fadeA.AddComponent("AudioSource");
		DontDestroyOnLoad(this.fadeA.transform);
		
		this.fadeB = new GameObject();
		this.sourceB = (AudioSource)this.fadeB.AddComponent("AudioSource");
		DontDestroyOnLoad(this.fadeB.transform);
	}
	
	public void StoreCurrent()
	{
		this.storedMusic = this.currentMusic;
		if(this.currentPlaying == 1 || this.currentPlaying == 4) this.storedTime = this.sourceA.time;
		else if(this.currentPlaying == 2 || this.currentPlaying == 3) this.storedTime = this.sourceB.time;
	}
	
	public void PlayStored()
	{
		this.Play(this.storedMusic);
		this.SetTime(this.storedTime);
	}
	
	public void FadeInStored(float time, EaseType type)
	{
		this.FadeIn(this.storedMusic, time, type);
		this.SetTime(this.storedTime);
	}
	
	public void FadeToStored(float time, EaseType type)
	{
		this.FadeTo(this.storedMusic, time, type);
		this.SetTime(this.storedTime);
	}
	
	public bool CheckPlay(MusicClip m)
	{
		bool play = true;
		if(this.currentMusic != null && m.realID == this.currentMusic.realID &&
				this.currentPlaying > 0 && !this.currentMusic.fadeOut)
		{
			play = false;
		}
		return play;
	}
	
	public void SetTime(float t)
	{
		if(this.currentPlaying == 1 || this.currentPlaying == 4) this.sourceA.time = t;
		else if(this.currentPlaying == 2 || this.currentPlaying == 3) this.sourceB.time = t;
	}
	
	// play voids
	public void Play(int index)
	{
		this.Play(DataHolder.Music().GetCopy(index));
	}
	
	public void PlayFromTime(int index, float t)
	{
		this.Play(DataHolder.Music().GetCopy(index));
		this.SetTime(t);
	}
	
	public void Play(MusicClip m)
	{
		if(this.CheckPlay(m))
		{
			this.sourceA.Stop();
			this.sourceB.Stop();
			this.lastMusic = this.currentMusic;
			this.currentMusic = m;
			this.sourceA.clip = this.currentMusic.GetClip();
			this.sourceA.volume = this.currentMusic.maxVolume;
			this.sourceA.loop = this.currentMusic.loop;
			this.sourceA.Play();
			this.currentPlaying = 1;
			if(this.currentMusic.HasLoops())
			{
				this.ticking = true;
				this.DoTick();
			}
		}
	}
	
	public void Stop()
	{
		this.sourceA.Stop();
		this.sourceB.Stop();
		this.currentPlaying = 0;
		this.ticking = false;
	}
	
	public void FadeIn(int index, float time, EaseType type)
	{
		this.FadeIn(DataHolder.Music().GetCopy(index), time, type);
	}
	
	public void FadeIn(MusicClip m, float time, EaseType type)
	{
		if(this.CheckPlay(m))
		{
			this.sourceA.Stop();
			this.sourceB.Stop();
			this.lastMusic = this.currentMusic;
			this.currentMusic = m;
			this.sourceA.clip = this.currentMusic.GetClip();
			this.sourceA.volume = 0;
			this.sourceA.loop = this.currentMusic.loop;
			this.currentMusic.FadeIn(time, type);
			this.sourceA.Play();
			this.currentPlaying = 1;
			if(this.currentMusic.HasLoops())
			{
				this.ticking = true;
				this.DoTick();
			}
		}
	}
	
	public void FadeOut(float time, EaseType type)
	{
		this.currentMusic.FadeOut(time, type);
	}
	
	public void FadeTo(int index, float time, EaseType type)
	{
		this.FadeTo(DataHolder.Music().GetCopy(index), time, type);
	}
	
	public void FadeTo(MusicClip m, float time, EaseType type)
	{
		if(this.currentPlaying == 0)
		{
			this.FadeIn(m, time, type);
		}
		else if(this.CheckPlay(m))
		{
			if(this.currentPlaying == 3)
			{
				this.sourceA.Stop();
				this.currentPlaying = 2;
			}
			else if(this.currentPlaying == 4)
			{
				this.sourceB.Stop();
				this.currentPlaying = 1;
			}
			
			this.lastMusic = this.currentMusic;
			this.currentMusic = m;
			
			this.lastMusic.FadeOut(time, type);
			this.currentMusic.FadeIn(time, type);
			
			if(this.currentPlaying == 1)
			{
				this.sourceB.clip = this.currentMusic.GetClip();
				this.sourceB.volume = 0;
				this.sourceB.loop = this.currentMusic.loop;
				this.sourceB.Play();
				this.currentPlaying = 3;
			}
			else if(this.currentPlaying == 2)
			{
				this.sourceA.clip = this.currentMusic.GetClip();
				this.sourceA.volume = 0;
				this.sourceA.loop = this.currentMusic.loop;
				this.sourceA.Play();
				this.currentPlaying = 4;
			}
			if(this.currentMusic.HasLoops())
			{
				this.ticking = true;
				this.DoTick();
			}
		}
	}
	
	public void DoTick()
	{
		StartCoroutine(DoTick2());
	}
	
	private IEnumerator DoTick2()
	{
		while(this.ticking && this.currentPlaying > 0)
		{
			yield return new WaitForSeconds(0.05f);
			if(this.currentMusic.HasLoops())
			{
				if(this.currentPlaying == 1)
				{
					this.currentMusic.CheckLoop(this.sourceA);
				}
				else if(this.currentPlaying == 2)
				{
					this.currentMusic.CheckLoop(this.sourceB);
				}
				else if(this.currentPlaying == 3)
				{
					this.lastMusic.CheckLoop(this.sourceA);
					this.currentMusic.CheckLoop(this.sourceB);
				}
				else if(this.currentPlaying == 4)
				{
					this.lastMusic.CheckLoop(this.sourceB);
					this.currentMusic.CheckLoop(this.sourceA);
				}
			}
		}
	}
	
	void Update()
	{
		if(this.currentPlaying == 1)
		{
			this.currentMusic.DoFade(Time.deltaTime, this.sourceA);
		}
		else if(this.currentPlaying == 2)
		{
			this.currentMusic.DoFade(Time.deltaTime, this.sourceB);
		}
		else if(this.currentPlaying == 3)
		{
			this.lastMusic.DoFade(Time.deltaTime, this.sourceA);
			this.currentMusic.DoFade(Time.deltaTime, this.sourceB);
		}
		else if(this.currentPlaying == 4)
		{
			this.lastMusic.DoFade(Time.deltaTime, this.sourceB);
			this.currentMusic.DoFade(Time.deltaTime, this.sourceA);
		}
		
		if(this.sourceA.isPlaying && !this.sourceB.isPlaying) this.currentPlaying = 1;
		else if(this.sourceB.isPlaying && !this.sourceA.isPlaying) this.currentPlaying = 2;
		else if(!this.sourceA.isPlaying && !this.sourceB.isPlaying) this.currentPlaying = 0;
	}
}