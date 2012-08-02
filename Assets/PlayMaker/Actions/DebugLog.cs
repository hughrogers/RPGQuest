// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Debug)]
	[Tooltip("Sends a log message to the PlayMaker Log Window.")]
	public class DebugLog : FsmStateAction
	{
		public LogLevel logLevel;
		public FsmString text;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			text = "";
		}

		public override void OnEnter()
		{
			if (!string.IsNullOrEmpty(text.Value))
				ActionHelpers.DebugLog(Fsm, logLevel, text.Value);

			Finish();
		}
	}
}