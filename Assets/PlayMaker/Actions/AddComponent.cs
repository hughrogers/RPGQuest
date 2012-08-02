// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Adds a Component to a Game Object. Use this to change the behaviour of objects on the fly. Optionally remove the Component on exiting the state.")]
	public class AddComponent : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to add the Component to.")]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[Tooltip("The Component to add to the Game Object.")]
		[UIHint(UIHint.ScriptComponent)]
		public FsmString component;
		
		[Tooltip("Remove the Component when this State is exited.")]
		public FsmBool removeOnExit;

		Component addedComponent;

		public override void Reset()
		{
			gameObject = null;
			component = null;
		}

		public override void OnEnter()
		{
			DoAddComponent();
			
			Finish();
		}

		public override void OnExit()
		{
			if (removeOnExit.Value && addedComponent != null)
			{
				Object.Destroy(addedComponent);
			}
		}

		void DoAddComponent()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);

			addedComponent = go.AddComponent(component.Value);

			if (addedComponent == null)
			{
				LogError("Can't add component: " + component.Value);
			}
		}
	}
}