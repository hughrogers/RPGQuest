// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{	
	[ActionCategory(ActionCategory.Device)]
	[Tooltip("Starts location service updates. Last location coordinates can be retrieved with GetLocationInfo.")]
	public class StartLocationServiceUpdates : FsmStateAction
	{
		[Tooltip("Maximum time to wait in seconds before failing.")]
		public FsmFloat maxWait;
		public FsmFloat desiredAccuracy;
		public FsmFloat updateDistance;
		[Tooltip("Event to send when the location services have started.")]
		public FsmEvent successEvent;
		[Tooltip("Event to send if the location services fail to start.")]
		public FsmEvent failedEvent;
		
#if UNITY_IPHONE
		float startTime;
#endif		
		public override void Reset()
		{
			maxWait = 20;
			desiredAccuracy = 10;
			updateDistance = 10;
			successEvent = null;
			failedEvent = null;
		}

		public override void OnEnter()
		{
#if UNITY_IPHONE
			startTime = FsmTime.RealtimeSinceStartup;
			
#if UNITY_3_5
  			Input.location.Start(desiredAccuracy.Value, updateDistance.Value);			
#else
  			iPhoneSettings.StartLocationServiceUpdates(desiredAccuracy.Value, updateDistance.Value);
#endif


#else
			Finish();
#endif
		}
		
		public override void OnUpdate()
		{
#if UNITY_IPHONE
			
#if UNITY_3_5

			if (Input.location.status == LocationServiceStatus.Failed ||
				Input.location.status == LocationServiceStatus.Stopped ||
				(FsmTime.RealtimeSinceStartup - startTime) > maxWait.Value )
			{
				Fsm.Event(failedEvent);
				Finish();
			}
			
			if (Input.location.status == LocationServiceStatus.Running)
			{
				Fsm.Event(successEvent);
				Finish();
			}
			
#else
			
			if (iPhoneSettings.locationServiceStatus == LocationServiceStatus.Failed ||
				iPhoneSettings.locationServiceStatus == LocationServiceStatus.Stopped ||
				(FsmTime.RealtimeSinceStartup - startTime) > maxWait.Value )
			{
				Fsm.Event(failedEvent);
				Finish();
			}
			
			if (iPhoneSettings.locationServiceStatus == LocationServiceStatus.Running)
			{
				Fsm.Event(successEvent);
				Finish();
			}
#endif
			
#endif
		}
	}
}