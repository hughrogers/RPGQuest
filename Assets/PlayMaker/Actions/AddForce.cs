// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Adds a force to a Game Object. Use Vector3 variable and/or Float variables for each axis.")]
	public class AddForce : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Rigidbody))]
		public FsmOwnerDefault gameObject;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("Optionally apply the force at a position on the object. This will also add some torque. The position is often returned from MousePick or GetCollisionInfo actions.")]
		public FsmVector3 atPosition;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("A Vector3 force to add. Optionally override any axis with the X, Y, Z parameters.")]
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
			atPosition = new FsmVector3 { UseVariable = true };
			vector = null;
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
			DoAddForce();
			
			if (!everyFrame)
			{
				Finish();
			}		
		}

		public override void OnFixedUpdate()
		{
			DoAddForce();
		}

		void DoAddForce()
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

			var force = vector.IsNone ? new Vector3(x.Value, y.Value, z.Value) : vector.Value;
			
			// override any axis

			if (!x.IsNone) force.x = x.Value;
			if (!y.IsNone) force.y = y.Value;
			if (!z.IsNone) force.z = z.Value;
					
			// apply force			
				
			if (space == Space.World)
			{
				if (!atPosition.IsNone)
				{
					go.rigidbody.AddForceAtPosition(force, atPosition.Value);
				}
				else
				{
					go.rigidbody.AddForce(force);
				}
			}
			else
			{
				go.rigidbody.AddRelativeForce(force);
			}
		}


	}
}