// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Device)]
	[Tooltip("Plays a full-screen movie on a handheld device. Please consult the Unity docs for Handheld.PlayFullScreenMovie for proper usage.")]
	public class DevicePlayFullScreenMovie : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Note that player will stream movie directly from the iPhone disc, therefore you have to provide movie as a separate files and not as an usual asset.\nYou will have to create a folder named StreamingAssets inside your Unity project (inside your Assets folder). Store your movies inside that folder. Unity will automatically copy contents of that folder into the iPhone application bundle.")]
		public FsmString moviePath;

		[RequiredField]
		[Tooltip("THis action will initiate a transition that fades the screen from your current content to the designated background color of the player. When playback finishes, the player uses another fade effect to transition back to your content.")]
		public FsmColor fadeColor;

#if (UNITY_IPHONE || UNITY_ANDROID)
		
	#if UNITY_3_5

		[Tooltip("Options for displaying movie playback controls. See Unity docs.")]
		public FullScreenMovieControlMode movieControlMode;

		[Tooltip("Scaling modes for displaying movies.. See Unity docs.")]
		public FullScreenMovieScalingMode movieScalingMode;
	#else

		[Tooltip("Options for displaying movie playback controls. See Unity docs.")]
		public iPhoneMovieControlMode movieControlMode;

		[Tooltip("Scaling modes for displaying movies.. See Unity docs.")]
		public iPhoneMovieScalingMode movieScalingMode;

	#endif
		public override void Reset()
		{
			moviePath = "";
			fadeColor = Color.black;

	#if UNITY_3_5
			movieControlMode = FullScreenMovieControlMode.Full;
			movieScalingMode = FullScreenMovieScalingMode.AspectFit;
	#else
			movieControlMode = iPhoneMovieControlMode.Full;
			movieScalingMode = iPhoneMovieScalingMode.AspectFit;
	#endif
		}

		public override void OnEnter()
		{

			
	#if UNITY_3_5
			
			Handheld.PlayFullScreenMovie(moviePath.Value, fadeColor.Value, movieControlMode, movieScalingMode);

	#else
			
			iPhoneUtils.PlayMovie(moviePath.Value, fadeColor.Value, movieControlMode, movieScalingMode);
			
	#endif


		}
		
#else
		
		[ActionSection("Current platform is not IOS or Android")]
		public bool RemindMeAtRuntime;
		
		public override void Reset()
		{
			RemindMeAtRuntime = true;
		}
		public override void OnEnter()
		{
			if (RemindMeAtRuntime)
			{
				Debug.LogWarning("Current platform is not IOS or Android, DevicePlayFullScreenMovie action only works for IOS and Android");
			}
		}
		
#endif
		
	}
}