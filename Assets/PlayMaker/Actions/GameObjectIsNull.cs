// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Tests if a Game Object Variable has a null value. E.g., If the FindGameObject action failed to find an object.")]
	public class GameObjectIsNull : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmGameObject gameObject;
		public FsmEvent isNull;
		public FsmEvent isNotNull;
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			isNull = null;
			isNotNull = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoIsGameObjectNull();
			
			if (!everyFrame)
				Finish();
		}

		public override void OnUpdate()
		{
			DoIsGameObjectNull();
		}
		
		void DoIsGameObjectNull()
		{
			bool goIsNull = gameObject.Value == null;

			if (storeResult != null)
				storeResult.Value = goIsNull;

			Fsm.Event(goIsNull ? isNull : isNotNull);
		}
	}
}