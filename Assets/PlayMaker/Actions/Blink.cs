// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Effects)]
	[Tooltip("Turns a Game Object on/off in a regular repeating pattern.")]
	public class Blink : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		[HasFloatSlider(0, 5)]
		public FsmFloat timeOff;
		[HasFloatSlider(0, 5)]
		public FsmFloat timeOn;
		[Tooltip("Should the object start in the active/visible state?")]
		public FsmBool startOn;
		[Tooltip("Check this to keep the object active, but not rendered.")]
		public bool rendererOnly;
		[Tooltip("Ignore TimeScale. Useful if the game is paused.")]
		public bool realTime;
		
		private float startTime;
		private float timer;
		private bool blinkOn;
		
		public override void Reset()
		{
			gameObject = null;
			timeOff = 0.5f;
			timeOn = 0.5f;
			rendererOnly = true;
			startOn = false;
			realTime = false;
		}
	
		public override void OnEnter()
		{
			startTime = FsmTime.RealtimeSinceStartup;
			timer = 0f;
			
			UpdateBlinkState(startOn.Value);
		}
		
		public override void OnUpdate()
		{
			// update time
			
			if (realTime)
			{
				timer = FsmTime.RealtimeSinceStartup - startTime;
			}
			else
			{
				timer += Time.deltaTime;
			}
			
			// update blink
			
			if (blinkOn && timer > timeOn.Value)
			{
				UpdateBlinkState(false);
			}
			
			if (blinkOn == false && timer > timeOff.Value)
			{
				UpdateBlinkState(true);
			}
		}
			
		void UpdateBlinkState(bool state)
		{
			GameObject go = gameObject.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : gameObject.GameObject.Value;
			if (go == null) return;
			
			if (rendererOnly)
			{
				if (go.renderer != null)
					go.renderer.enabled = state;
			}
			else
			{
				go.active = state;
			}
			
			blinkOn = state;
			
			// reset timer
			
			startTime = FsmTime.RealtimeSinceStartup;
			timer = 0f;
		}
			
			


		
	}
}

