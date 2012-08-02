// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Animation)]
	[Tooltip("Enables/Disables an Animation on a Game Object.\nAnimation time is paused while disabled. Animation must also have a non zero weight to play.")]
	public class EnableAnimation : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Animation))]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[UIHint(UIHint.Animation)]
		public FsmString animName;
		
		[RequiredField]
		public FsmBool enable;
		
		public FsmBool resetOnExit;
		
		private AnimationState anim;
		
		public override void Reset()
		{
			gameObject = null;
			animName = null;
			enable = true;
			resetOnExit = false;
		}

		public override void OnEnter()
		{
			DoEnableAnimation(Fsm.GetOwnerDefaultTarget(gameObject));
			
			Finish();
		}

		void DoEnableAnimation(GameObject go)
		{
			if (go == null)
			{
				return;
			}

			if (go.animation == null)
			{
				LogError("Missing animation component!");
				return;
			}

			anim = go.animation[animName.Value];
			
			if (anim != null)
			{
				anim.enabled = enable.Value;
			}
		}
		
		public override void OnExit()
		{
			if (resetOnExit.Value && anim != null)
			{
				anim.enabled = !enable.Value;
			}
		}
	}
}