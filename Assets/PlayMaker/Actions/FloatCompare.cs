// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Sends Events based on the comparison of 2 Floats.")]
	public class FloatCompare : FsmStateAction
	{
		[RequiredField]
		public FsmFloat float1;
		[RequiredField]
		public FsmFloat float2;
		[RequiredField]
		public FsmFloat tolerance;
		[Tooltip("Event sent if Float 1 equals Float 2 (within Tolerance)")]
		public FsmEvent equal;
		[Tooltip("Event sent if Float 1 is less than Float 2")]
		public FsmEvent lessThan;
		[Tooltip("Event sent if Float 1 is greater than Float 2")]
		public FsmEvent greaterThan;
		public bool everyFrame;

		public override void Reset()
		{
			float1 = 0f;
			float2 = 0f;
			tolerance = 0f;
			equal = null;
			lessThan = null;
			greaterThan = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoCompare();
			
			if (!everyFrame)
				Finish();
		}

		public override void OnUpdate()
		{
			DoCompare();
		}

		void DoCompare()
		{

			if (Mathf.Abs(float1.Value - float2.Value) <= tolerance.Value)
			{
				Fsm.Event(equal);
				return;
			}

			if (float1.Value < float2.Value)
			{
				Fsm.Event(lessThan);
				return;
			}

			if (float1.Value > float2.Value)
			{
				Fsm.Event(greaterThan);
			}

		}

		public override string ErrorCheck()
		{
			if (FsmEvent.IsNullOrEmpty(equal) &&
				FsmEvent.IsNullOrEmpty(lessThan) &&
				FsmEvent.IsNullOrEmpty(greaterThan))
				return "Action sends no events!";
			return "";
		}
	}
}