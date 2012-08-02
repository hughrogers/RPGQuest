// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Audio)]
	[Tooltip("Sets the Volume of the Audio Clip played by the AudioSource component on a Game Object.")]
	public class SetAudioVolume : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(AudioSource))]
		public FsmOwnerDefault gameObject;
		[HasFloatSlider(0,1)]
		public FsmFloat volume;
		public bool everyFrame;
		
		public override void Reset()
		{
			gameObject = null;
			volume = 1;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetAudioVolume();
			
			if (!everyFrame)
				Finish();
		}
		
		public override void OnUpdate ()
		{
			DoSetAudioVolume();
		}
		
		void DoSetAudioVolume()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go != null)
			{
				var audio = go.audio;
				if (audio != null)
				{
					if (!volume.IsNone)
						audio.volume = volume.Value;
				}
			}
		}
	}
}