
using UnityEngine;
using System.Collections;

public class BaseColliderZone : MonoBehaviour
{
	public bool IsWithin(Vector3 point)
	{
		bool within = false;
		point.y = this.collider.bounds.center.y;
		if(this.collider.bounds.Contains(point))
		{
			point.y = this.collider.bounds.max.y+1;
			RaycastHit hit;
			if(this.collider.Raycast(new Ray(point, -Vector3.up), out hit, this.collider.bounds.size.y))
			{
				within = true;
			}
		}
		return within;
	}
}
