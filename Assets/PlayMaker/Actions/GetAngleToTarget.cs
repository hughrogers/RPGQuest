// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Transform)]
	[Tooltip("Gets the Angle between a Game Object's forward axis and a Target. The Target can be defined as a Game Object or a world Position. If you specify both, then the Position will be used as a local offset from the Object's position.")]
	public class GetAngleToTarget : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		
		public FsmGameObject targetObject;
		
		public FsmVector3 targetPosition;
		
		public FsmBool ignoreHeight;
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeAngle;
		
		public bool everyFrame;
		
		public override void Reset()
		{
			gameObject = null;
			targetObject = null;
			targetPosition = new FsmVector3 { UseVariable = true};
			ignoreHeight = true;
			storeAngle = null;
			everyFrame = false;
		}

		public override void OnLateUpdate()
		{
			DoGetAngleToTarget();
			
			if (!everyFrame)
			{
				Finish();
			}
		}

		void DoGetAngleToTarget()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}
			
			var goTarget = targetObject.Value;
			if (goTarget == null && targetPosition.IsNone)
			{
				return;
			}

			Vector3 targetPos;
			if (goTarget != null)
			{
				targetPos = !targetPosition.IsNone ? 
					goTarget.transform.TransformPoint(targetPosition.Value) : 
					goTarget.transform.position;
			}
			else
			{
				targetPos = targetPosition.Value;
			}

			if (ignoreHeight.Value)
			{
				targetPos.y = go.transform.position.y;
			}
			
			var targetDir = targetPos - go.transform.position;
			
			storeAngle.Value = Vector3.Angle(targetDir, go.transform.forward);
		}

	}
}