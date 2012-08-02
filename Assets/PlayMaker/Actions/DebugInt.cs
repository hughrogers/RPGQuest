// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Debug)]
	[Tooltip("Logs the value of an Integer Variable in the PlayMaker Log Window.")]
	public class DebugInt : FsmStateAction
	{
		public LogLevel logLevel;
		[UIHint(UIHint.Variable)]
		public FsmInt intVariable;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			intVariable = null;
		}

		public override void OnEnter()
		{
			string text = "None";
			
			if (!intVariable.IsNone)
			{
				text = intVariable.Name + ": " + intVariable.Value;
			}

			ActionHelpers.DebugLog(Fsm, logLevel, text);

			Finish();
		}
	}
}