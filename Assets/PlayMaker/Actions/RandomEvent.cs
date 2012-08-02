// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Sends a Random State Event after an optional delay. Use this to transition to a random state from the current state.")]
	public class RandomEvent : FsmStateAction
	{
		[HasFloatSlider(0, 10)]
		public FsmFloat delay;

		DelayedEvent delayedEvent;

		public override void Reset()
		{
			delay = null;
		}

		public override void OnEnter()
		{
			if (State.Transitions.Length == 0)
			{
				return;
			}
			
			if (delay.Value < 0.001f)
			{
				Fsm.Event(GetRandomEvent());
				Finish();
			}
			else
			{
				delayedEvent = Fsm.DelayedEvent(GetRandomEvent(), delay.Value);
			}
		}

		public override void OnUpdate()
		{
			if (DelayedEvent.WasSent(delayedEvent))
			{
				Finish();
			}
		}

		FsmEvent GetRandomEvent()
		{
			var randomIndex = Random.Range(0, State.Transitions.Length);
			return State.Transitions[randomIndex].FsmEvent;
		}

	}
}