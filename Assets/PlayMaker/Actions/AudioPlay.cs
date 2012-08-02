// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Audio)]
	[Tooltip("Plays the Audio Clip set with Set Audio Clip or in the Audio Source inspector on a Game Object. Optionally plays a one shot Audio Clip.")]
	public class AudioPlay : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(AudioSource))]
		[Tooltip("The GameObject with the AudioSource component.")]
		public FsmOwnerDefault gameObject;
		
		[HasFloatSlider(0,1)]
		public FsmFloat volume;
		
		[ObjectType(typeof(AudioClip))]
		[Tooltip("Optionally play a 'one shot' AudioClip.")]
		public FsmObject oneShotClip;
		
		[Tooltip("Event to send when the AudioClip finished playing.")]
		public FsmEvent finishedEvent;

		AudioSource audio;
		
		public override void Reset()
		{
			gameObject = null;
			volume = 1f;
			oneShotClip = null;
		}

		public override void OnEnter()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go != null)
			{
				// cache the AudioSource component
				
				audio = go.audio;
				if (audio != null)
				{
					var audioClip = oneShotClip.Value as AudioClip;

					if (audioClip == null)
					{
						audio.Play();
						
						if (!volume.IsNone)
						{
							audio.volume = volume.Value;
						}
						
						return;
					}
					
					if (!volume.IsNone)
					{
						audio.PlayOneShot(audioClip, volume.Value);
					}
					else
					{
						audio.PlayOneShot(audioClip);
					}
						
					return;
				}
			}
			
			// Finish if failed to play sound
			
			Finish();
		}
		
		public override void OnUpdate ()
		{
			if (audio == null)
			{
				Finish();
			}
			else
			{
				if (!audio.isPlaying)
				{
					Fsm.Event(finishedEvent);
					Finish();
				}
			}
		}
	}
}