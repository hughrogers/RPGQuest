
using UnityEngine;

[AddComponentMenu("RPG Kit/Scenes/Spawn Point")]
public class SpawnPoint : MonoBehaviour
{
	public int ID = 0;
	public bool onGround = true;
	public float distance = 100.0f;
	public LayerMask layerMask = 1;
	public Vector3 offset = Vector3.zero;
	
	void Awake()
	{
		this.SetOnGround();
	}
	
	public void SetOnGround()
	{
		if(this.onGround)
		{
			RaycastHit hit = new RaycastHit();
			if(Physics.Raycast(transform.position, -Vector3.up, out hit, this.distance, this.layerMask))
			{
				transform.position = hit.point+this.offset;
			}
		}
	}
	
	void OnDrawGizmos()
	{
		Gizmos.DrawIcon(transform.position, "SpawnPoint.psd");
	}
}