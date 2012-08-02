// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Unregister this server from the master server.\n\n" +
		"Does nothing if the server is not registered or has already unregistered.")]
	public class MasterServerUnregisterHost : FsmStateAction
	{		
		public override void OnEnter()
		{
			MasterServer.UnregisterHost();
			
			Finish();
		}
	}
}