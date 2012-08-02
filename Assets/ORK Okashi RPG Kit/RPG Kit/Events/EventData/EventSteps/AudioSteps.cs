
using System.Collections;
using UnityEngine;

public class PlaySoundStep : EventStep
{
	public PlaySoundStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		GameObject actor = null;
		if(this.show2)
		{
			if(gameEvent.waypoint[this.actorID]) actor = gameEvent.waypoint[this.actorID].gameObject;
		}
		else
		{
			actor = gameEvent.actor[this.actorID].GetActor();
		}
		if(actor && gameEvent.audioClip[this.audioID])
		{
			if(!actor.audio) actor.AddComponent("AudioSource");
			actor.audio.pitch = this.speed;
			actor.audio.volume = this.volume;
			actor.audio.rolloffMode = this.audioRolloffMode;
			actor.audio.minDistance = this.float1;
			actor.audio.maxDistance = this.float2;
			actor.audio.loop = this.show3;
			if(this.show)
			{
				actor.audio.PlayOneShot(gameEvent.audioClip[this.audioID], this.volume);
			}
			else
			{
				actor.audio.clip = gameEvent.audioClip[this.audioID];
				actor.audio.Play();
			}
		}
		if(wait && actor && gameEvent.audioClip[this.audioID])
		{
			gameEvent.StartTime(gameEvent.audioClip[this.audioID].length, this.next);
		}
		else
		{
			gameEvent.StepFinished(this.next);
		}
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("audio", this.audioID.ToString());
		ht.Add("wait", this.wait.ToString());
		ht.Add("show", this.show.ToString());
		ht.Add("show2", this.show2.ToString());
		ht.Add("show3", this.show3.ToString());
		ht.Add("actor", this.actorID.ToString());
		ht.Add("volume", this.volume.ToString());
		ht.Add("speed", this.speed.ToString());
		ht.Add("audiorolloffmode", this.audioRolloffMode.ToString());
		ht.Add("float1", this.float1.ToString());
		ht.Add("float2", this.float2.ToString());
		return ht;
	}
}

public class StopSoundStep : EventStep
{
	public StopSoundStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		GameObject actor = null;
		if(this.show2)
		{
			if(gameEvent.waypoint[this.actorID]) actor = gameEvent.waypoint[this.actorID].gameObject;
		}
		else
		{
			actor = gameEvent.actor[this.actorID].GetActor();
		}
		if(actor && actor.audio)
		{
			if(this.show)
			{
				actor.audio.Pause();
			}
			else
			{
				actor.audio.Stop();
			}
		}
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("show", this.show.ToString());
		ht.Add("show2", this.show2.ToString());
		ht.Add("actor", this.actorID.ToString());
		return ht;
	}
}

public class PlayMusicStep : EventStep
{
	public PlayMusicStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		if(MusicPlayType.PLAY.Equals(this.playType))
		{
			if(this.show) GameHandler.GetMusicHandler().PlayStored();
			else GameHandler.GetMusicHandler().Play(this.musicID);
		}
		else if(MusicPlayType.STOP.Equals(this.playType))
		{
			GameHandler.GetMusicHandler().Stop();
		}
		else if(MusicPlayType.FADE_IN.Equals(this.playType))
		{
			if(this.show) GameHandler.GetMusicHandler().FadeInStored(this.float1, this.interpolate);
			else GameHandler.GetMusicHandler().FadeIn(this.musicID, this.float1, this.interpolate);
		}
		else if(MusicPlayType.FADE_OUT.Equals(this.playType))
		{
			GameHandler.GetMusicHandler().FadeOut(this.float1, this.interpolate);
		}
		else if(MusicPlayType.FADE_TO.Equals(this.playType))
		{
			if(this.show) GameHandler.GetMusicHandler().FadeToStored(this.float1, this.interpolate);
			else GameHandler.GetMusicHandler().FadeTo(this.musicID, this.float1, this.interpolate);
		}
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("show", this.show.ToString());
		ht.Add("float1", this.float1.ToString());
		ht.Add("music", this.musicID.ToString());
		ht.Add("interpolate", this.interpolate.ToString());
		ht.Add("musicplaytype", this.playType.ToString());
		return ht;
	}
}

public class StoreMusicStep : EventStep
{
	public StoreMusicStep(GameEventType t) : base(t)
	{
		
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		GameHandler.GetMusicHandler().StoreCurrent();
		gameEvent.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		return base.GetData();
	}
}