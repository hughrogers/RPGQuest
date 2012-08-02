// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.
// JeanFabre: This version allow setting the variable to null. 

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Set the value of a Game Object Variable in another FSM. Accept null reference")]
	public class SetFsmGameObject : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		
		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;
		
		[RequiredField]
		[UIHint(UIHint.FsmGameObject)]
		public FsmString variableName;
		
		public FsmGameObject setValue;
		
		public bool everyFrame;

		private GameObject goLastFrame;
		private PlayMakerFSM fsm;
		
		public override void Reset()
		{
			gameObject = null;
			fsmName = "";
			setValue = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetFsmGameObject();
			
			if (!everyFrame)
			{
				Finish();
			}		
		}

		void DoSetFsmGameObject()
		{
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
			
			var fsmGameObject = fsm.FsmVariables.GetFsmGameObject(variableName.Value);
			
			if (fsmGameObject != null)

			{
				fsmGameObject.Value = setValue == null ? null : setValue.Value;
			}
		}

		public override void OnUpdate()
		{
			DoSetFsmGameObject();
		}

	}
}