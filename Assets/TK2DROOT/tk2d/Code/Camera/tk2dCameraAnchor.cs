using UnityEngine;
using System.Collections;

[AddComponentMenu("2D Toolkit/Camera/tk2dCameraAnchor")]
[ExecuteInEditMode]
/// <summary>
/// Anchors children to anchor position, offset by number of pixels
/// </summary>
public class tk2dCameraAnchor : MonoBehaviour 
{
	/// <summary>
	/// Anchor.
	/// </summary>
    public enum Anchor
    {
		/// <summary>Upper left</summary>
		UpperLeft,
		/// <summary>Upper center</summary>
		UpperCenter,
		/// <summary>Upper right</summary>
		UpperRight,
		/// <summary>Middle left</summary>
		MiddleLeft,
		/// <summary>Middle center</summary>
		MiddleCenter,
		/// <summary>Middle right</summary>
		MiddleRight,
		/// <summary>Lower left</summary>
		LowerLeft,
		/// <summary>Lower center</summary>
		LowerCenter,
		/// <summary>Lower right</summary>
		LowerRight,
    }
	
	/// <summary>
	/// Anchor location
	/// </summary>
	public Anchor anchor;
	/// <summary>
	/// Offset in pixels
	/// </summary>
	public Vector2 offset = Vector2.zero;
	
	public tk2dCamera tk2dCamera;
	Transform _transform;
	
	void Awake()
	{
		_transform = transform;
	}
	
	void Start()
	{
		UpdateTransform();
	}
	
	void UpdateTransform()
	{
		if (tk2dCamera != null)
		{
			Vector2 scaledResolution = tk2dCamera.ScaledResolution;

			Vector3 position = _transform.localPosition;	
			Vector3 anchoredPosition = Vector3.zero;
			switch (anchor)
			{
			case Anchor.UpperLeft: 		anchoredPosition = new Vector3(0, scaledResolution.y, position.z); break;
			case Anchor.UpperCenter: 	anchoredPosition = new Vector3(scaledResolution.x / 2.0f, scaledResolution.y, position.z); break;
			case Anchor.UpperRight: 	anchoredPosition = new Vector3(scaledResolution.x, scaledResolution.y, position.z); break;
			case Anchor.MiddleLeft: 	anchoredPosition = new Vector3(0, scaledResolution.y / 2.0f, position.z); break;
			case Anchor.MiddleCenter: 	anchoredPosition = new Vector3(scaledResolution.x / 2.0f, scaledResolution.y / 2.0f, position.z); break;
			case Anchor.MiddleRight: 	anchoredPosition = new Vector3(scaledResolution.x, scaledResolution.y / 2.0f, position.z); break;
			case Anchor.LowerLeft: 		anchoredPosition = new Vector3(0, 0, position.z); break;
			case Anchor.LowerCenter: 	anchoredPosition = new Vector3(scaledResolution.x / 2.0f, 0, position.z); break;
			case Anchor.LowerRight: 	anchoredPosition = new Vector3(scaledResolution.x, 0, position.z); break;
			}
			
			var newPosition = anchoredPosition + new Vector3(offset.x, offset.y, 0);
			var oldPosition = _transform.localPosition;
			if (oldPosition != newPosition)
				_transform.localPosition = newPosition;
		}
	}

	public void ForceUpdateTransform()
	{
		UpdateTransform();
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdateTransform();
	}
}
