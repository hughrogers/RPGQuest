// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Gets the name of the specified FSMs current state. Either reference the fsm component directly, or find it on a game object.")]
	public class GetFsmState : FsmStateAction
	{
		public PlayMakerFSM fsmComponent;
		public FsmOwnerDefault gameObject;
		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of Fsm on Game Object")]
		public FsmString fsmName;
		[UIHint(UIHint.Variable)]
		public FsmString storeResult;
		public bool everyFrame;
		
		private PlayMakerFSM fsm;

		public override void Reset()
		{
			fsmComponent = null;
			gameObject = null;
			fsmName = "";
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetFsmState();
			
			if (!everyFrame)
				Finish();
		}

		public override void OnUpdate()
		{
			DoGetFsmState();
		}
		
		void DoGetFsmState()
		{
			if (fsm == null)
			{
				if (fsmComponent != null)
				{
					fsm = fsmComponent;
				}
				else
				{
					var go = Fsm.GetOwnerDefaultTarget(gameObject);
					if (go != null)
					{
						fsm = ActionHelpers.GetGameObjectFsm(go, fsmName.Value);
					}
				}
				
				if (fsm == null)
				{
					storeResult.Value = "";
					return;
				}
			}

			storeResult.Value = fsm.ActiveStateName;
		}

	}
}