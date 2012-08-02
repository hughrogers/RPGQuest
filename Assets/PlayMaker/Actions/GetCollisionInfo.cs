// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Gets info on the last collision event and store in variables.")]
	public class GetCollisionInfo : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		public FsmGameObject gameObjectHit;
		[UIHint(UIHint.Variable)]
		public FsmVector3 relativeVelocity;
		[UIHint(UIHint.Variable)]
		public FsmFloat relativeSpeed;
		[UIHint(UIHint.Variable)]
		public FsmVector3 contactPoint;
		[UIHint(UIHint.Variable)]
		public FsmVector3 contactNormal;
		[UIHint(UIHint.Variable)]
		[Tooltip("Useful for triggering different effects. Audio, particles...")]
		public FsmString physicsMaterialName;

		public override void Reset()
		{
			gameObjectHit = null;
			relativeVelocity = null;
			relativeSpeed = null;
			contactPoint = null;
			contactNormal = null;
			physicsMaterialName = null;
		}

		void StoreCollisionInfo()
		{
			if (Fsm.CollisionInfo == null) return;
			
			gameObjectHit.Value = Fsm.CollisionInfo.gameObject;
			relativeSpeed.Value = Fsm.CollisionInfo.relativeVelocity.magnitude;
			relativeVelocity.Value = Fsm.CollisionInfo.relativeVelocity;
			physicsMaterialName.Value = Fsm.CollisionInfo.collider.material.name;

			if (Fsm.CollisionInfo.contacts != null && Fsm.CollisionInfo.contacts.Length > 0)
			{
				contactPoint.Value = Fsm.CollisionInfo.contacts[0].point;
				contactNormal.Value = Fsm.CollisionInfo.contacts[0].normal;
			}
		}

		public override void OnEnter()
		{
			StoreCollisionInfo();
			
			Finish();
		}
	}
}