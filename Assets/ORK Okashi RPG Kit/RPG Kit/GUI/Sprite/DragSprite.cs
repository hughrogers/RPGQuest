
using UnityEngine;
using System.Collections;

public class DragSprite : BaseSprite
{
	public ChoiceContent drag;
	
	public void SetPosition(Vector2 mousePosition)
	{
		Vector2 ratio = GameHandler.GUIHandler().GetScreenRatio();
		transform.position = new Vector3(
				-(Screen.width/2)+(mousePosition.x+this.drag.halfDrag.x)*ratio.x,
				-0.1f, 
				-(Screen.height/2)+(mousePosition.y+this.drag.halfDrag.y)*ratio.y);
	}
	
	public override void UpdateGUISize(bool updateChilds, Vector2 ratio)
	{
		this.renderer.material.mainTexture = this.drag.dragTexture;
		this.UpdateBaseMesh(ratio);
	}
}
