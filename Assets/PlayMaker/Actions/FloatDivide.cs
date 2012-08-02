// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Divides a one Float by another.")]
	public class FloatDivide : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat floatVariable;
		[RequiredField]
		public FsmFloat divideBy;
		public bool everyFrame;

		public override void Reset()
		{
			floatVariable = null;
			divideBy = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			floatVariable.Value /= divideBy.Value;
			
			if (!everyFrame)
				Finish();
		}

		public override void OnUpdate()
		{
			floatVariable.Value /= divideBy.Value;
		}
	}
}