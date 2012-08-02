// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("TESTS")]
	public class TestAction : FsmStateAction
	{
		public FsmMaterial[] materials;

		public override void Reset()
		{
			materials = new FsmMaterial[3];
		}

		public override void OnUpdate()
		{
		}
	}
}