﻿// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Set the value of a Texture Variable in another FSM.")]
	public class SetFsmTexture : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject that owns the FSM.")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;

		[RequiredField]
		[UIHint(UIHint.FsmMaterial)]
		public FsmString variableName;

		[RequiredField]
		public FsmTexture setValue;

		public bool everyFrame;

		GameObject goLastFrame;
		PlayMakerFSM fsm;

		public override void Reset()
		{
			gameObject = null;
			fsmName = "";
			variableName = "";
			setValue = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetFsmBool();

			if (!everyFrame)
			{
				Finish();
			}
		}

		void DoSetFsmBool()
		{
			if (setValue == null)
			{
				return;
			}

			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}

			if (go != goLastFrame)
			{
				goLastFrame = go;

				// only get the fsm component if go has changed

				fsm = ActionHelpers.GetGameObjectFsm(go, fsmName.Value);
			}

			if (fsm == null)
			{
				return;
			}

			var fsmVar = fsm.FsmVariables.GetFsmTexture(variableName.Value);

			if (fsmVar != null)
			{
				fsmVar.Value = setValue.Value;
			}
		}

		public override void OnUpdate()
		{
			DoSetFsmBool();
		}

	}
}