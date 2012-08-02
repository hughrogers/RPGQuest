// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Sends Events based on the value of a Boolean Variable.")]
	public class BoolTest : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmBool boolVariable;
		public FsmEvent isTrue;
		public FsmEvent isFalse;
		public bool everyFrame;

		public override void Reset()
		{
			boolVariable = null;
			isTrue = null;
			isFalse = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			Fsm.Event(boolVariable.Value ? isTrue : isFalse);
			
			if (!everyFrame)
				Finish();
		}
		
		public override void OnUpdate()
		{
			Fsm.Event(boolVariable.Value ? isTrue : isFalse);
		}
	}
}