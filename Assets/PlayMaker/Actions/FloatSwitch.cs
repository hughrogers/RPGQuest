// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Sends an Event based on the value of a Float Variable. The float could represent distance, angle to a target, health left... The array sets up float ranges that correspond to Events.")]
	public class FloatSwitch : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat floatVariable;
		[CompoundArray("Float Switches", "Less Than", "Send Event")]
		public FsmFloat[] lessThan;
		public FsmEvent[] sendEvent;
		public bool everyFrame;

		public override void Reset()
		{
			floatVariable = null;
			lessThan = new FsmFloat[1];
			sendEvent = new FsmEvent[1];
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoFloatSwitch();
			
			if (!everyFrame)
				Finish();
		}

		public override void OnUpdate()
		{
			DoFloatSwitch();
		}
		
		void DoFloatSwitch()
		{
			if (floatVariable.IsNone)
				return;
			
			for (int i = 0; i < lessThan.Length; i++) 
			{
				if (floatVariable.Value < lessThan[i].Value)
				{
					Fsm.Event(sendEvent[i]);
					return;
				}
			}
		}
	}
}