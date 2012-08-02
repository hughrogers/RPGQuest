// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Level)]
	[Tooltip("Loads a Level by Index number. Before you can load a level, you have to add it to the list of levels defined in File->Build Settings...")]
	public class LoadLevelNum : FsmStateAction
	{
		[RequiredField]
		public FsmInt levelIndex;
		public bool additive;
		public FsmEvent loadedEvent;
		public FsmBool dontDestroyOnLoad;

		public override void Reset()
		{
			levelIndex = null;
			additive = false;
			loadedEvent = null;
			dontDestroyOnLoad = true;
		}

		public override void OnEnter()
		{
			if (dontDestroyOnLoad.Value)
			{
				// Have to get the root, since this FSM will be destroyed if a parent is destroyed.
				
				Transform root = Owner.transform.root;
				Object.DontDestroyOnLoad(root.gameObject);
			}

			if (additive)
				Application.LoadLevelAdditive(levelIndex.Value);
			else
				Application.LoadLevel(levelIndex.Value);

			Fsm.Event(loadedEvent);
			Finish();
		}
	}
}