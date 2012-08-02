// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Set the log level for network messages. Default is Off.\n\nOff: Only report errors, otherwise silent.\n\nInformational: Report informational messages like connectivity events.\n\nFull: Full debug level logging down to each individual message being reported.")]
	public class NetworkSetLogLevel : FsmStateAction
	{	
		[Tooltip("The log level")]
		public NetworkLogLevel logLevel;


		public override void Reset()
		{
			logLevel = NetworkLogLevel.Off;
			
		}

		public override void OnEnter()
		{
			Network.logLevel = logLevel;
			
			Finish();
		}
	}
}