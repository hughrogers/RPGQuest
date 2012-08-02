﻿// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Camera)]
	[Tooltip("Sets the Culling Mask used by the Camera.")]
	public class SetCameraCullingMask : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Camera))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("Cull these layers.")]
		[UIHint(UIHint.Layer)]
		public FsmInt[] cullingMask;
		
		[Tooltip("Invert the mask, so you cull all layers except those defined above.")]
		public FsmBool invertMask;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			cullingMask = new FsmInt[0];
			invertMask = false;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetCameraCullingMask();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoSetCameraCullingMask();
		}

		void DoSetCameraCullingMask()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) return;

			var camera = go.camera;
			if (camera == null)
			{
				LogError("Missing Camera Component!");
				return;
			}

			camera.cullingMask = ActionHelpers.LayerArrayToLayerMask(cullingMask, invertMask.Value);
		}
	}
}