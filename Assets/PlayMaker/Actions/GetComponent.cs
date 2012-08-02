// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.UnityObject)]
	[Tooltip("Gets a Component attached to a GameObject and stores it in an Object variable. NOTE: Set the Object variable's Object Type to get a component of that type. E.g., set Object Type to UnityEngine.AudioListener to get the AudioListener component on the camera.")]
	public class GetComponent : FsmStateAction
	{
		public FsmOwnerDefault gameObject;
		[UIHint(UIHint.Variable)]
		public FsmObject storeComponent;
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			storeComponent = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetComponent();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetComponent();
		}
		
		void DoGetComponent()
		{
			if (storeComponent == null)
			{
				return;
			}
			
			var targetObject = Fsm.GetOwnerDefaultTarget(gameObject);
			
			if (targetObject == null)
			{
				return;
			}
			
			storeComponent.Value = targetObject.GetComponent(storeComponent.ObjectType);
		}
	}
}