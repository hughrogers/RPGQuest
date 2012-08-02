// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Adds torque (rotational force) to a Game Object.")]
	public class AddTorque : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Rigidbody))]
		public FsmOwnerDefault gameObject;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("A Vector3 torque. Optionally override any axis with the X, Y, Z parameters.")]
		public FsmVector3 vector;

		[Tooltip("To leave unchanged, set to 'None'.")]
		public FsmFloat x;

		[Tooltip("To leave unchanged, set to 'None'.")]
		public FsmFloat y;

		[Tooltip("To leave unchanged, set to 'None'.")]
		public FsmFloat z;

		public Space space;

		public ForceMode forceMode;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			// default axis to variable dropdown with None selected.
			x = new FsmFloat { UseVariable = true };
			y = new FsmFloat { UseVariable = true };
			z = new FsmFloat { UseVariable = true };
			space = Space.World;
			forceMode = ForceMode.Force;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoAddTorque();
			
			if (!everyFrame)
			{
				Finish();
			}		
		}

		public override void OnFixedUpdate()
		{
			DoAddTorque();
		}

		void DoAddTorque()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}

			if (go.rigidbody == null)
			{
				LogWarning("Missing rigid body: " + go.name);
				return;
			}

			var torque = vector.IsNone ? new Vector3(x.Value, y.Value, z.Value) : vector.Value;
			
			// override any axis

			if (!x.IsNone) torque.x = x.Value;
			if (!y.IsNone) torque.y = y.Value;
			if (!z.IsNone) torque.z = z.Value;
					
			// apply
			
			if (space == Space.World)
			{
				go.rigidbody.AddTorque(torque, forceMode);
			}
			else
			{
				go.rigidbody.AddRelativeTorque(torque, forceMode);
			}
		}


	}
}