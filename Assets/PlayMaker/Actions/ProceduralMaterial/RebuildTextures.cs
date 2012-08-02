﻿// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Substance")]
	[Tooltip("Rebuilds all dirty textures. By default the rebuild is spread over multiple frames so it won't halt the game. Check Immediately to rebuild all textures in a single frame.")]
	public class RebuildTextures : FsmStateAction
	{
		[RequiredField]
		public FsmMaterial substanceMaterial;
		
		[RequiredField]
		public FsmBool immediately;
		
		public bool everyFrame;

		public override void Reset()
		{
			substanceMaterial = null;
			immediately = false;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoRebuildTextures();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoRebuildTextures();
		}

		void DoRebuildTextures()
		{
#if !(UNITY_IPHONE || UNITY_ANDROID || UNITY_NACL)
	
			var substance = substanceMaterial.Value as ProceduralMaterial;

			if (substance == null)
			{
				LogError("Not a substance material!");
				return;
			}

			if (!immediately.Value)
			{
				substance.RebuildTextures();
			}
			else
			{
				substance.RebuildTexturesImmediately();
			}

#endif
		}
	}
}