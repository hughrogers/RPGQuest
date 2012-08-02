﻿// (c) copyright Hutong Games, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Animation)]
	[Tooltip("Removes a mixing transform previously added with Add Mixing Transform. If transform has been added as recursive, then it will be removed as recursive. Once you remove all mixing transforms added to animation state all curves become animated again.")]
	public class RemoveMixingTransform : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Animation))]
		[Tooltip("The GameObject playing the animation.")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Tooltip("The name of the animation.")]
		public FsmString animationName;
		[RequiredField]
		
		[Tooltip("The mixing transform to remove. E.g., root/upper_body/left_shoulder")]
		public FsmString transfrom;

		public override void Reset()
		{
			gameObject = null;
			animationName = "";
		}

		public override void OnEnter()
		{
			DoRemoveMixingTransform();
			
			Finish();
		}

		void DoRemoveMixingTransform()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null || go.animation == null)
			{
				return;
			}

			var animClip = go.animation[animationName.Value];

			if (animClip == null)
			{
				return;
			}

			var mixingTransform = go.transform.Find(transfrom.Value);
			animClip.AddMixingTransform(mixingTransform);
		}
	}
}