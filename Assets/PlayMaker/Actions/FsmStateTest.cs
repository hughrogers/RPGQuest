// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Tests if an FSM is in the specified State.")]
	public class FsmStateTest : FsmStateAction
	{
		[RequiredField]
		public FsmGameObject gameObject;
		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of Fsm on Game Object")]
		public FsmString fsmName;
		[RequiredField]
		public FsmString stateName;
		public FsmEvent trueEvent;
		public FsmEvent falseEvent;
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;
		public bool everyFrame;
		
		// store game object last frame so we know when it's changed
		// and have to cache a new fsm
		GameObject previousGo;
		
		// cach the fsm component since that's an expensive operation
		PlayMakerFSM fsm;

		public override void Reset()
		{
			gameObject = null;
			fsmName = null;
			stateName = null;
			trueEvent = null;
			falseEvent = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoFsmStateTest();
			
			if (!everyFrame)
				Finish();
		}
		
		public override void OnUpdate()
		{
			DoFsmStateTest();
		}	
		
		void DoFsmStateTest()
		{
			GameObject go = gameObject.Value;
			if (go == null) return;
			
			if (go != previousGo)
			{
				fsm = ActionHelpers.GetGameObjectFsm(go, fsmName.Value);
				previousGo = go;
			}
			
			if (fsm == null) return;
			
			bool isState = false;
			
			if (fsm.ActiveStateName == stateName.Value)
			{
				Fsm.Event(trueEvent);
				isState = true;
			}
			else 
			{
				Fsm.Event(falseEvent);
			}

			storeResult.Value = isState;
		}


	}
}