// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.ScriptControl)]
	[Tooltip("Enables/Disables a Behaviour on a Game Object. Optionally reset the Behaviour on exit - useful if you only want the Behaviour to be active while this state is active.")]
	public class EnableBehaviour : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		
		[UIHint(UIHint.Behaviour)]
		public FsmString behaviour;
		
		[Tooltip("Optionally drag a component directly into this field")]
		public Component component;
		
		[RequiredField]
		public FsmBool enable;
		
		public FsmBool resetOnExit;

		public override void Reset()
		{
			gameObject = null;
			behaviour = null;
			component = null;
			enable = true;
			resetOnExit = true;
		}

		Behaviour componentTarget;

		public override void OnEnter()
		{
			DoEnableBehaviour(Fsm.GetOwnerDefaultTarget(gameObject));
			
			Finish();
		}

		void DoEnableBehaviour(GameObject go)
		{
			if (go == null)
			{
				return;
			}

			if (component != null)
			{
				componentTarget = component as Behaviour;
			}
			else
			{
				componentTarget = go.GetComponent(behaviour.Value) as Behaviour;
			}

			if (componentTarget == null)
			{
				LogWarning(" " + go.name + " missing behaviour: " + behaviour.Value);
				return;
			}

			componentTarget.enabled = enable.Value;
		}

		public override void OnExit()
		{
			if (componentTarget == null)
			{
				return;
			}

			if (resetOnExit.Value)
			{
				componentTarget.enabled = !enable.Value;
			}
		}

	}
}