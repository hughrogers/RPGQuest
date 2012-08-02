// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Tests if all the Bool Variables are False.\nSend an event or store the result.")]
	public class BoolNoneTrue : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmBool[] boolVariables;
		public FsmEvent sendEvent;
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;
		public bool everyFrame;

		public override void Reset()
		{
			boolVariables = null;
			sendEvent = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoNoneTrue();
			
			if (!everyFrame)
				Finish();		
		}
		
		public override void OnUpdate()
		{
			DoNoneTrue();
		}
		
		void DoNoneTrue()
		{
			if (boolVariables.Length == 0) return;
			
			bool noneTrue = true;
			
			for (int i = 0; i < boolVariables.Length; i++) 
			{
				if (boolVariables[i].Value)
				{
					noneTrue = false;
					break;
				}
			}
			
			if (noneTrue)
				Fsm.Event(sendEvent);
			
			storeResult.Value = noneTrue;
		}
	}
}