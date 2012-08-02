// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Test if your peer type is server.")]
	public class NetworkIsServer : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("True if running as server.")]
		public FsmBool isServer;

		[Tooltip("Event to send if running as server.")]
		public FsmEvent isServerEvent;

		[Tooltip("Event to send if not running as server.")]
		public FsmEvent isNotServerEvent;

		public override void Reset()
		{
			isServer = null;
		}

		public override void OnEnter()
		{
			DoCheckIsServer();
			
			Finish();
		}

		void DoCheckIsServer()
		{
			isServer.Value = Network.isServer;	
			
			if (Network.isServer && isServerEvent != null)
			{		
				Fsm.Event(isServerEvent);
			}
			else if (!Network.isServer && isNotServerEvent != null)
			{		
				Fsm.Event(isNotServerEvent);
			}
		}
	}
}