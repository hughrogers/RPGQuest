// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Device)]
	[Tooltip("Causes the device to vibrate for half a second.")]
	public class DeviceVibrate : FsmStateAction
	{
		public override void Reset()
		{}

		public override void OnEnter()
		{
#if (UNITY_IPHONE || UNITY_ANDROID)
			
#if UNITY_3_5
			
			Handheld.Vibrate();

#else
			
			iPhoneUtils.Vibrate();
			
#endif

#endif
		}
	}
}