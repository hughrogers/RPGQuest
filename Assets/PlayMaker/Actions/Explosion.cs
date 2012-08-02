// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Applies an explosion Force to all Game Objects with a Rigid Body inside a Radius.")]
	public class Explosion : FsmStateAction
	{
		[RequiredField]
		public FsmVector3 center;
		[RequiredField]
		public FsmFloat force;
		[RequiredField]
		public FsmFloat radius;
		public FsmFloat upwardsModifier;
		public ForceMode forceMode;
		[UIHint(UIHint.Layer)]
		public FsmInt layer;
		[UIHint(UIHint.Layer)]
		public FsmInt[] layerMask;
		[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
		public FsmBool invertMask;
		public bool everyFrame;

		public override void Reset()
		{
			center = null;
			upwardsModifier = 0f;
			forceMode = ForceMode.Force;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoExplosion();
			
			if (!everyFrame)
				Finish();		
		}

		public override void OnFixedUpdate()
		{
			DoExplosion();
		}

		void DoExplosion()
		{
			var colliders = Physics.OverlapSphere(center.Value, radius.Value);
			
			foreach (var hit in colliders)
			{
				if (hit.rigidbody != null && ShouldApplyForce(hit.gameObject))
				{
					hit.rigidbody.AddExplosionForce(force.Value, center.Value, radius.Value, upwardsModifier.Value, forceMode);
				}
			}
		}
		
		bool ShouldApplyForce(GameObject go)
		{
			int mask = ActionHelpers.LayerArrayToLayerMask(layerMask, invertMask.Value);
			
			return ((1 << go.layer) & mask) > 0;
		}
	}
}