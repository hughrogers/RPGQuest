// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Vector3)]
	[Tooltip("Get Vector3 Length.")]
	public class SetHandsPosition : FsmStateAction
	{
		public FsmOwnerDefault gameObject;
		[RequiredField]
		public FsmFloat HandsDist;
		public FsmFloat HandsAngle;
		[RequiredField]
		public FsmVector3 SelectedPoint;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmGameObject Hand01Obj;
		public FsmGameObject Hand02Obj;
		
		public override void Reset()
		{
			HandsDist = new FsmFloat { UseVariable = true };
			HandsAngle = new FsmFloat { UseVariable = true };
			HandsDist.Value = 1;
			gameObject = null;
			SelectedPoint = null;
			Hand01Obj = null;
			Hand02Obj = null;
		}

		public override void OnEnter()
		{
			DoCalc();
			Finish();
		}
		
		void DoCalc()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null || SelectedPoint  == null) return;
			float handspace = HandsDist.Value;
			if (Input.GetMouseButton(0))
				handspace /= 2;
			handspace = handspace*(0.25f+0.75f*Mathf.Abs(Mathf.Cos(HandsAngle.Value*Mathf.PI/18.0f)));
			Transform transform = go.transform;
			Vector3 vecDest = transform.position;
			Vector3 vec01 = transform.InverseTransformPoint(SelectedPoint.Value);
			vec01.x = vec01.z =  0;
			vecDest = transform.TransformPoint(vec01);
			Vector3 crntAngle = transform.eulerAngles;
			
				
			if (Hand01Obj != null)
			{
				Vector3 vec;
				vec.x = handspace; vec.y = 0;vec.z = 0;
				transform.Rotate( 0, 0, HandsAngle.Value*10);
				vec = transform.TransformDirection(vec);
				transform.eulerAngles = crntAngle;
				vec += SelectedPoint.Value;
				vec -= vecDest;
				vec.Normalize();
				Vector3 tPos = vec + vecDest;
				vec *= -1;
				RaycastHit hit;
				Ray ray = new Ray( tPos, vec);
				Collider collider = go.transform.collider;
				if (collider.Raycast ( ray, out hit, 100)) {
					Hand01Obj.Value.transform.position = hit.point;
					Hand01Obj.Value.transform.forward = hit.normal;
				}
			}
			if (Hand02Obj != null)
			{
				Vector3 vec;
				vec.x = -handspace; vec.y = 0;vec.z = 0;
				transform.Rotate( 0, 0, HandsAngle.Value*10);
				vec = transform.TransformDirection(vec);
				transform.eulerAngles = crntAngle;
				vec += SelectedPoint.Value;
				vec -= vecDest;
				vec.Normalize();
				Vector3 tPos = vec + vecDest;
				vec *= -1;
				RaycastHit hit;
				Ray ray = new Ray( tPos, vec);
				Collider collider = go.transform.collider;
				if (collider.Raycast ( ray, out hit, 100)) {
					Hand02Obj.Value.transform.position = hit.point;
					Hand02Obj.Value.transform.forward = hit.normal;
				}
			}
		}
	}
}