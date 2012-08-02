
using UnityEngine;
using System.Collections;

public class TransformHelper
{
	public static Transform GetChild(string name, Transform t)
	{
		Transform t2 = null;
		if(name != "") t2 = t.Find(name);
		if(t2 == null) t2 = t;
		return t2;
	}
	
	public static void Mount(Transform parent, Transform child, 
			bool setToPosition, bool localSpace, Vector3 positionOffset, 
			bool useRotation, Vector3 rotationOffset)
	{
		if(setToPosition)
		{
			child.position = parent.position;
		}
		if(useRotation)
		{
			child.eulerAngles = parent.eulerAngles;
			child.eulerAngles += rotationOffset;
		}
		child.parent = parent;
		if(setToPosition)
		{
			if(localSpace) child.localPosition = positionOffset;
			else child.position += positionOffset;
		}
	}
}
