// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Tests if a Game Object is a Child of another Game Object.")]
	public class GameObjectIsChildOf : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		public FsmGameObject isChildOf;
		
		public FsmEvent trueEvent;
		
		public FsmEvent falseEvent;
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		public override void Reset()
		{
			gameObject = null;
			isChildOf = null;
			trueEvent = null;
			falseEvent = null;
			storeResult = null;
		}

		public override void OnEnter()
		{
			DoIsChildOf(Fsm.GetOwnerDefaultTarget(gameObject));
			
			Finish();
		}

		void DoIsChildOf(GameObject go)
		{
			if (go == null || isChildOf == null)
			{
				return;
			}
			
			var isChild = go.transform.IsChildOf(isChildOf.Value.transform);

			storeResult.Value = isChild;
			
			Fsm.Event(isChild ? trueEvent : falseEvent);
		}
	}
}