// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUI)]
	[Tooltip("GUI Label.")]
	public class GUILabel : GUIContentAction
	{
		public override void OnGUI()
		{
			base.OnGUI();
			
			GUI.Label(rect, content, style.Value);
		}
	}
}