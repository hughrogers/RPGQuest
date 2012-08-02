// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Test if your peer type is client.")]
	public class NetworkIsClient : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("True if running as client.")]
		public FsmBool isClient;

		[Tooltip("Event to send if running as client.")]
		public FsmEvent isClientEvent;

		[Tooltip("Event to send if not running as client.")]
		public FsmEvent isNotClientEvent;

		public override void Reset()
		{
			isClient = null;
		}

		public override void OnEnter()
		{
			DoCheckIsClient();
			
			Finish();
		}

		void DoCheckIsClient()
		{
			isClient.Value = Network.isClient;
			
			if (Network.isClient && isClientEvent != null)
			{		
				Fsm.Event(isClientEvent);
			}
			else if (!Network.isClient && isNotClientEvent != null)
			{		
				Fsm.Event(isNotClientEvent);
			}
		}
	}
}