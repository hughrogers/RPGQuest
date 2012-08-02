// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Sends Events based on the current State of an FSM.")]
	public class FsmStateSwitch : FsmStateAction
	{
		[RequiredField]
		public FsmGameObject gameObject;
		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of Fsm on Game Object")]
		public FsmString fsmName;
		[CompoundArray("State Switches", "Compare State", "Send Event")]
		public FsmString[] compareTo;
		public FsmEvent[] sendEvent;
		[Tooltip("Repeat")]
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
			compareTo = new FsmString[1];
			sendEvent = new FsmEvent[1];
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoFsmStateSwitch();
			
			if (!everyFrame)
				Finish();
		}

		public override void OnUpdate()
		{
			DoFsmStateSwitch();
		}
		
		void DoFsmStateSwitch()
		{
			GameObject go = gameObject.Value;
			if (go == null) return;
			
			if (go != previousGo)
			{
				fsm = ActionHelpers.GetGameObjectFsm(go, fsmName.Value);
				previousGo = go;
			}
			
			if (fsm == null) return;
			
			string activeStateName = fsm.ActiveStateName;
			
			for (int i = 0; i < compareTo.Length; i++) 
			{
				if (activeStateName == compareTo[i].Value)
				{
					Fsm.Event(sendEvent[i]);
					return;
				}
			}
		}
	}
}