// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Debug)]
	[Tooltip("Logs the value of a Float Variable in the PlayMaker Log Window.")]
	public class DebugFloat : FsmStateAction
	{
		public LogLevel logLevel;
		[UIHint(UIHint.Variable)]
		public FsmFloat floatVariable;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			floatVariable = null;
		}

		public override void OnEnter()
		{
			string text = "None";
			
			if (!floatVariable.IsNone)
			{
				text = floatVariable.Name + ": " + floatVariable.Value;
			}

			ActionHelpers.DebugLog(Fsm, logLevel, text);

			Finish();
		}
	}
}