// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Enables/Disables an FSM component on a Game Object.\nOptionally reverse the action on exit.")]
	public class EnableFSM : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of Fsm on Game Object")]
		public FsmString fsmName;
		public FsmBool enable;
		public FsmBool resetOnExit;

		private PlayMakerFSM fsmComponent;
		
		public override void Reset()
		{
			gameObject = null;
			fsmName = "";
			enable = true;
			resetOnExit = true;
		}

		public override void OnEnter()
		{
			DoEnableFSM();
			
			Finish();
		}

		void DoEnableFSM()
		{
			GameObject go = gameObject.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : gameObject.GameObject.Value;

			if (go == null) return;
			
			if (!string.IsNullOrEmpty(fsmName.Value))
			{
				// find by FSM component name
				
				var fsmComponents = go.GetComponents<PlayMakerFSM>();
				foreach (var component in fsmComponents)
				{
					if (component.FsmName == fsmName.Value)
					{
						fsmComponent = component;
						break;
					}
				}
			}
			else
			{
				// get first FSM component
				
				fsmComponent = go.GetComponent<PlayMakerFSM>();
			}
			
			if (fsmComponent == null)
			{
				// TODO: Not sure if this is an error condition... 
				LogError("Missing FsmComponent!");
				return;
			}

			fsmComponent.enabled = enable.Value;
		}

		public override void OnExit()
		{
			if (fsmComponent == null) return;

			if (resetOnExit.Value)
			{
				fsmComponent.enabled = !enable.Value;
			}
		}

	}
}