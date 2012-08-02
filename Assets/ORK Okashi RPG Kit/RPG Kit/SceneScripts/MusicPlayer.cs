
using UnityEngine;

[AddComponentMenu("RPG Kit/Events/Music Player")]
public class MusicPlayer : BaseInteraction
{
	public MusicPlayType playType = MusicPlayType.PLAY;
	public int musicClip = 0;
	public float fadeTime = 1;
	public EaseType interpolate = EaseType.Linear;
	
	void Start()
	{
		// start event when autostart
		if(EventStartType.AUTOSTART.Equals(this.startType) && this.CheckVariables())
		{
			this.PlayMusic();
		}
	}
	
	void Update()
	{
		if(this.KeyPress()) this.PlayMusic();
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(this.CheckTriggerEnter(other))
		{
			this.PlayMusic();
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if(this.CheckTriggerExit(other))
		{
			this.PlayMusic();
		}
	}
	
	public override void TouchInteract()
	{
		this.OnMouseUp();
	}
	
	void OnMouseUp()
	{
		if(EventStartType.INTERACT.Equals(this.startType) && this.CheckVariables() && 
				this.gameObject.active && DataHolder.GameSettings().IsMouseAllowed())
		{
			GameObject p = GameHandler.GetPlayer();
			if(p && Vector3.Distance(p.transform.position, this.transform.position) < this.maxMouseDistance)
			{
				this.PlayMusic();
			}
		}
	}
	
	public override bool Interact()
	{
		bool val = false;
		// start event on interaction here
		if(EventStartType.INTERACT.Equals(this.startType) && this.CheckVariables() && this.gameObject.active)
		{
			this.PlayMusic();
			val = true;
		}
		return val;
	}
	
	public override bool DropInteract(ChoiceContent drop)
	{
		bool val = false;
		if(EventStartType.DROP.Equals(this.startType) &&
			this.CheckVariables() && this.gameObject.active && this.CheckDrop(drop))
		{
			this.PlayMusic();
			val = true;
		}
		return val;
	}
	
	public void PlayMusic()
	{
		if(MusicPlayType.PLAY.Equals(this.playType))
		{
			GameHandler.GetMusicHandler().Play(this.musicClip);
		}
		else if(MusicPlayType.STOP.Equals(this.playType))
		{
			GameHandler.GetMusicHandler().Stop();
		}
		else if(MusicPlayType.FADE_IN.Equals(this.playType))
		{
			GameHandler.GetMusicHandler().FadeIn(this.musicClip, this.fadeTime, this.interpolate);
		}
		else if(MusicPlayType.FADE_OUT.Equals(this.playType))
		{
			GameHandler.GetMusicHandler().FadeOut(this.fadeTime, this.interpolate);
		}
		else if(MusicPlayType.FADE_TO.Equals(this.playType))
		{
			GameHandler.GetMusicHandler().FadeTo(this.musicClip, this.fadeTime, this.interpolate);
		}
	}
	
	void OnDrawGizmos()
	{
		Gizmos.DrawIcon(transform.position, "MusicPlayer.psd");
	}
}