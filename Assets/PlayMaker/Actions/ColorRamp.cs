// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Color)]
	[Tooltip("Samples a Color on a continuous Colors gradient.")]
	public class ColorRamp : FsmStateAction
	{
		[RequiredField]
		public FsmColor[] colors;
		[RequiredField]
		public FsmFloat sampleAt;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmColor storeColor;
		public bool everyFrame;
		
		public override void Reset()
		{
			colors = new FsmColor[3];
			sampleAt = 0;
			storeColor = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoColorRamp();
			
			if (!everyFrame)
				Finish();
		}
		
		public override void OnUpdate()
		{
			DoColorRamp();
		}
		
		void DoColorRamp()
		{
			if (colors == null) return;
			if (colors.Length == 0) return;
			if (sampleAt == null) return;
			if (storeColor == null) return;
			
			Color lerpColor;
			float lerpAmount = Mathf.Clamp(sampleAt.Value, 0, colors.Length-1);

			if (lerpAmount == 0)
				lerpColor = colors[0].Value;
			else if (lerpAmount == colors.Length)
				lerpColor = colors[colors.Length-1].Value;
			else
			{
				Color color1 = colors[Mathf.FloorToInt(lerpAmount)].Value;
				Color color2 = colors[Mathf.CeilToInt(lerpAmount)].Value;
				lerpAmount -= Mathf.Floor(lerpAmount);
				
				lerpColor = Color.Lerp(color1, color2, lerpAmount);
			}
			
			storeColor.Value = lerpColor;
		}
		
		public override string ErrorCheck ()
		{
			if(colors.Length < 2)
				return "Define at least 2 colors to make a gradient.";
			return null;
		}
	}
}