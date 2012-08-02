
using UnityEngine;

[AddComponentMenu("RPG Kit/Scenes/Place On Ground")]
public class PlaceOnGround : MonoBehaviour
{
	public float distance = 100.0f;
	public LayerMask layerMask = 1;
	public Vector3 offset = Vector3.zero;
	
	void Start()
	{
		this.Place();
	}
	
	public void Place()
	{
		RaycastHit hit = new RaycastHit();
		if(Physics.Raycast(transform.position, -Vector3.up, out hit, this.distance, this.layerMask))
		{
			transform.position = hit.point+this.offset;
		}
	}
}