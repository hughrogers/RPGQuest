using UnityEngine;
using HutongGames.PlayMaker;

[ActionCategory(ActionCategory.Debug)]
[Tooltip("Draw gizmos shape")]
public class DebugDrawShape : FsmStateAction
{
	public enum ShapeType {Sphere, Cube, WireSphere, WireCube}
	
	[RequiredField]
	public FsmOwnerDefault gameObject;
	public ShapeType shape;
	public FsmColor color;
	[Tooltip("Use this for sphere gizmos")]
	public FsmFloat radius;
	[Tooltip("Use this for cube gizmos")]
	public FsmVector3 size;
	
	public override void Reset ()
	{
		gameObject = null;
		shape = ShapeType.Sphere;
		color = Color.grey;
		radius = 1f;
		size = new Vector3(1f, 1f, 1f);
	}
	
	public override void OnDrawGizmos ()
	{
		
		var ownerTransform = Fsm.GetOwnerDefaultTarget(gameObject).transform;
		if(ownerTransform == null)
			return;
		
		Gizmos.color = color.Value;
		
		switch(shape)
		{
			case ShapeType.Sphere:
				Gizmos.DrawSphere(ownerTransform.position, radius.Value);
				break;
			case ShapeType.WireSphere:
				Gizmos.DrawWireSphere(ownerTransform.position, radius.Value);
				break;
			case ShapeType.Cube:
				Gizmos.DrawCube(ownerTransform.position,size.Value);
				break;
			case ShapeType.WireCube:
				Gizmos.DrawWireCube(ownerTransform.position, size.Value);
				break;
			default:
				break;
		}
	}
}
