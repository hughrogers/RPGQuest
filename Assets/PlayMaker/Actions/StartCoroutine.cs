// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.ScriptControl)]
	[Tooltip("Start a Coroutine in a Behaviour on a Game Object. See Unity StartCoroutine docs.")]
	public class StartCoroutine : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		[RequiredField]
		[UIHint(UIHint.Behaviour)]
		public FsmString behaviour;
		[RequiredField]
		[UIHint(UIHint.Coroutine)]
		public FunctionCall functionCall;
		public bool stopOnExit;

		public override void Reset()
		{
			gameObject = null;
			behaviour = null;
			functionCall = null;
			stopOnExit = false;
		}

		MonoBehaviour component;

		public override void OnEnter()
		{
			DoStartCoroutine();
			
			Finish();
		}

		void DoStartCoroutine()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}

			component = go.GetComponent(behaviour.Value) as MonoBehaviour;

			if (component == null)
			{
				LogWarning("StartCoroutine: " + go.name + " missing behaviour: " + behaviour.Value);
				return;
			}

			switch (functionCall.ParameterType)
			{
				case "None":
					component.StartCoroutine(functionCall.FunctionName);
					return;

				case "int":
					component.StartCoroutine(functionCall.FunctionName, functionCall.IntParameter.Value);
					return;

				case "float":
					component.StartCoroutine(functionCall.FunctionName, functionCall.FloatParameter.Value);
					return;

				case "string":
					component.StartCoroutine(functionCall.FunctionName, functionCall.StringParameter.Value);
					return;

				case "bool":
					component.StartCoroutine(functionCall.FunctionName, functionCall.BoolParameter.Value);
					return;

				case "Vector3":
					component.StartCoroutine(functionCall.FunctionName, functionCall.Vector3Parameter.Value);
					return;

				case "Rect":
					component.StartCoroutine(functionCall.FunctionName, functionCall.RectParamater.Value);
					return;
				
				case "GameObject":
					component.StartCoroutine(functionCall.FunctionName, functionCall.GameObjectParameter.Value);
					return;

				case "Material":
					component.StartCoroutine(functionCall.FunctionName, functionCall.MaterialParameter.Value);
					break;

				case "Texture":
					component.StartCoroutine(functionCall.FunctionName, functionCall.TextureParameter.Value);
					break;

				case "Quaternion":
					component.StartCoroutine(functionCall.FunctionName, functionCall.QuaternionParameter.Value);
					break;

				case "Object":
					component.StartCoroutine(functionCall.FunctionName, functionCall.ObjectParameter.Value);
					return;
			}
		}

		public override void OnExit()
		{
			if (component == null)
			{
				return;
			}

			if (stopOnExit)
			{
				component.StopCoroutine(functionCall.FunctionName);
			}
		}

	}
}