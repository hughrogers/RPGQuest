
using System.Collections;
using UnityEngine;

public class PlaySoundAStep : AnimationStep
{
	public PlaySoundAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		float t = 0;
		AudioClip[] clip = null;
		if(this.show4)
		{
			if(StatusOrigin.USER.Equals(this.statusOrigin2) && battleAnimation.battleAction.user != null)
			{
				clip = new AudioClip[] {battleAnimation.battleAction.user.GetAudioClip(this.audioID)};
			}
			else if(StatusOrigin.TARGET.Equals(this.statusOrigin2))
			{
				clip = new AudioClip[battleAnimation.battleAction.target.Length];
				for(int i=0; i<battleAnimation.battleAction.target.Length; i++)
				{
					if(battleAnimation.battleAction.target[i] != null &&
						battleAnimation.battleAction.target[i].prefabInstance != null)
					{
						clip[i] = battleAnimation.battleAction.target[i].GetAudioClip(this.audioID);
					}
				}
			}
		}
		else clip = new AudioClip[] {battleAnimation.audioClip[this.audioID]};
		
		bool played = false;
		GameObject[] list = battleAnimation.GetAnimationObjects(this.animationObject, this.prefabID2);
		if(clip != null && clip.Length > 0 && 
			list != null && list.Length > 0)
		{
			for(int i=0; i<list.Length; i++)
			{
				if(list[i] != null && this.pathToChild != "")
				{
					Transform tr = list[i].transform.Find(this.pathToChild);
					if(tr != null) list[i] = tr.gameObject;
				}
				if(list[i] != null && i < clip.Length && 
					this.PlayAudio(list[i], clip[i]))
				{
					played = true;
				}
			}
		}
		
		if(played) t = this.MaxClipLength(clip);
		if(wait && t > 0)
		{
			battleAnimation.StartTime(t, this.next);
		}
		else
		{
			battleAnimation.StepFinished(this.next);
		}
	}
	
	private bool PlayAudio(GameObject actor, AudioClip clip)
	{
		bool b = false;
		if(actor != null && clip != null)
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
				actor.audio.PlayOneShot(clip, this.volume);
			}
			else
			{
				actor.audio.clip = clip;
				actor.audio.Play();
			}
			b = true;
		}
		return b;
	}
	
	private float MaxClipLength(AudioClip[] clip)
	{
		float t = 0;
		for(int i=0; i<clip.Length; i++)
		{
			if(clip[i] != null && clip[i].length > t) t = clip[i].length;
		}
		return t;
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("audio", this.audioID.ToString());
		ht.Add("wait", this.wait.ToString());
		ht.Add("show", this.show.ToString());
		ht.Add("show3", this.show3.ToString());
		ht.Add("show4", this.show4.ToString());
		ht.Add("statusorigin2", this.statusOrigin2.ToString());
		ht.Add("volume", this.volume.ToString());
		ht.Add("speed", this.speed.ToString());
		ht.Add("audiorolloffmode", this.audioRolloffMode.ToString());
		ht.Add("float1", this.float1.ToString());
		ht.Add("float2", this.float2.ToString());
		ht.Add("animationobject", this.animationObject.ToString());
		ht.Add("prefab2", this.prefabID2);
		
		ArrayList subs = new ArrayList();
		subs.Add(HashtableHelper.GetContentHashtable("pathtochild", this.pathToChild));
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}

public class StopSoundAStep : AnimationStep
{
	public StopSoundAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		GameObject[] list = battleAnimation.GetAnimationObjects(this.animationObject, this.prefabID2);
		for(int i=0; i<list.Length; i++)
		{
			if(list[i] != null && this.pathToChild != "")
			{
				Transform t = list[i].transform.Find(this.pathToChild);
				if(t != null) list[i] = t.gameObject;
			}
			this.StopAudio(list[i]);
		}
		battleAnimation.StepFinished(this.next);
	}
	
	private void StopAudio(GameObject actor)
	{
		if(actor != null && actor.audio)
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
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("show", this.show.ToString());
		ht.Add("animationobject", this.animationObject.ToString());
		ht.Add("prefab2", this.prefabID2);
		
		ArrayList subs = new ArrayList();
		subs.Add(HashtableHelper.GetContentHashtable("pathtochild", this.pathToChild));
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}