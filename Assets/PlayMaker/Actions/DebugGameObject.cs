// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Debug)]
	[Tooltip("Logs the value of a Game Object Variable in the PlayMaker Log Window.")]
	public class DebugGameObject : FsmStateAction
	{
		public LogLevel logLevel;
		[UIHint(UIHint.Variable)]
		public FsmGameObject gameObject;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			gameObject = null;
		}

		public override void OnEnter()
		{
			string text = "None";
			
			if (!gameObject.IsNone)
			{
				text = gameObject.Name + ": " + gameObject;
			}
			
			ActionHelpers.DebugLog(Fsm, logLevel, text);
			
			Finish();
		}
	}
}