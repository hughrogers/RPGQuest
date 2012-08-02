
using UnityEngine;
using System.Collections;

public class ImageSprite : BaseSprite
{
	public Texture2D texture;
	public Rect bounds;
	
	void Start()
	{	
		this.InitMesh();
		this.renderer.material.mainTexture = this.texture;
		this.UpdateGUISize(false, GameHandler.GUIHandler().GetScreenRatio());
	}
	
	void LateUpdate()
	{
		Vector2 ratio = GameHandler.GUIHandler().GetScreenRatio();
		transform.position = new Vector3(
					-(Screen.width/2)+(this.bounds.x+(this.bounds.width/2))*ratio.x,
					49-(this.spriteID/100.0f), 
					-(Screen.height/2)+(this.bounds.y+(this.bounds.height/2))*ratio.y);
	}
	
	public override void UpdateGUISize(bool updateChilds, Vector2 ratio)
	{
		this.UpdateBaseMesh(ratio);
	}
}
