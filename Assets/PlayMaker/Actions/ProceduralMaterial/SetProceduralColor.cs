﻿// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Substance")]
	[Tooltip("Set a named color property in a Substance material. NOTE: Use Rebuild Textures after setting Substance properties.")]
	public class SetProceduralColor : FsmStateAction
	{
		[RequiredField]
		public FsmMaterial substanceMaterial;
		[RequiredField]
		public FsmString colorProperty;
		[RequiredField]
		public FsmColor colorValue;
		[Tooltip("NOTE: Updating procedural materials every frame can be very slow!")]
		public bool everyFrame;

		public override void Reset()
		{
			substanceMaterial = null;
			colorProperty = "";
			colorValue = Color.white;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetProceduralFloat();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoSetProceduralFloat();
		}

		void DoSetProceduralFloat()
		{
#if !(UNITY_IPHONE || UNITY_ANDROID || UNITY_NACL)

			var substance = substanceMaterial.Value as ProceduralMaterial;

			if (substance == null)
			{
				LogError("Not a substance material!");
				return;
			}

			substance.SetProceduralColor(colorProperty.Value, colorValue.Value);
#endif
		}
	}
}