// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Character)]
	[Tooltip("Gets the Collision Flags from a Character Controller on a Game Object. Collision flags give you a broad overview of where the character collided with any other object.")]
	public class GetControllerCollisionFlags : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(CharacterController))]
		public FsmOwnerDefault gameObject;
		[UIHint(UIHint.Variable)]
		public FsmBool isGrounded;
		[UIHint(UIHint.Variable)]
		public FsmBool none;
		[UIHint(UIHint.Variable)]
		public FsmBool sides;
		[UIHint(UIHint.Variable)]
		public FsmBool above;
		[UIHint(UIHint.Variable)]
		public FsmBool below;
		
		GameObject previousGo; // remember so we can get new controller only when it changes.
		CharacterController controller;
		
		public override void Reset()
		{
			gameObject = null;
			isGrounded = null;
			none = null;
			sides = null;
			above = null;
			below = null;
		}

		public override void OnUpdate()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) return;
		
			if (go != previousGo)
			{
				controller = go.GetComponent<CharacterController>();
				previousGo = go;
			}
			
			if (controller != null)
			{
				isGrounded.Value = controller.isGrounded;
				none.Value = (controller.collisionFlags & CollisionFlags.None) != 0;
				sides.Value = (controller.collisionFlags & CollisionFlags.Sides) != 0;
				above.Value = (controller.collisionFlags & CollisionFlags.Above) != 0;
				below.Value = (controller.collisionFlags & CollisionFlags.Below) != 0;
			}
			
		}
	}
}
