// (c) copyright Hutong Games, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Animation)]
	[Tooltip("Adds a named Animation Clip to a Game Object. Optionally trims the Animation.")]
	public class AddAnimationClip : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Animation))]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[ObjectType(typeof(AnimationClip))]
		[Tooltip("NOTE: Make sure the clip is compatible with the object's hierarchy.")]
		public FsmObject animationClip;
		
		[RequiredField]
		[Tooltip("Name the animation. Used by other actions to reference this animation.")]
		public FsmString animationName;
		
		[Tooltip("Optionally trim the animation by specifying a first and last frame.")]
		public FsmInt firstFrame;
		
		[Tooltip("Optionally trim the animation by specifying a first and last frame.")]
		public FsmInt lastFrame;
		
		[Tooltip("Add an extra looping frame that matches the first frame.")]
		public FsmBool addLoopFrame;

		public override void Reset()
		{
			gameObject = null;
			animationClip = null;
			animationName = "";
			firstFrame = 0;
			lastFrame = 0;
			addLoopFrame = false;
		}

		public override void OnEnter()
		{
			DoAddAnimationClip();
			Finish();
		}

		void DoAddAnimationClip()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null || go.animation == null)
			{
				return;
			}

			var animClip = animationClip.Value as AnimationClip;

			if (animClip == null)
			{
				return;
			}

			if (firstFrame.Value == 0 && lastFrame.Value == 0)
			{
				go.animation.AddClip(animClip, animationName.Value);
			}
			else
			{
				go.animation.AddClip(animClip, animationName.Value, firstFrame.Value, lastFrame.Value, addLoopFrame.Value);
			}
		}
	}
}