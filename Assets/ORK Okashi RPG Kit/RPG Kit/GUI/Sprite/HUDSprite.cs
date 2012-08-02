
using UnityEngine;
using System.Collections;

public class HUDSprite : BaseSprite
{
	public HUD hud;
	private float check = 0;
	
	void Start()
	{	
		this.InitMesh();
		if(this.hud.fadeIn) this.startColor.a = 0;
		this.renderer.material.mainTexture = this.hud.GetTexture();
		this.UpdateGUISize(false, GameHandler.GUIHandler().GetScreenRatio());
	}
	
	void LateUpdate()
	{
		if(this.hud.IsOutDone())
		{
			this.RemoveSprite();
		}
		else
		{
			if(this.check > DataHolder.GameSettings().hudRefreshRate)
			{
				if(this.hud.CheckChange()) this.UpdateGUISize(false, GameHandler.GUIHandler().GetScreenRatio());
				this.check = 0;
			}
			if(DataHolder.GameSettings().hudRefreshFrame) this.check++;
			else this.check += Time.deltaTime;
			
			if(mesh.colors[0].a != this.hud.alpha)
			{
				Color[] colors = new Color[mesh.colors.Length];
				for(int i=0; i<colors.Length; i++)
				{
					colors[i] = new Color(1, 1, 1, this.hud.alpha);
				}
				mesh.colors = colors;
			}
			Vector2 ratio = GameHandler.GUIHandler().GetScreenRatio();
			transform.position = new Vector3(
					-(Screen.width/2)+(this.hud.currentPos.x+(this.hud.bgSize.x/2)-this.hud.start.x)*ratio.x,
					0+(this.spriteID/100.0f), 
					-(Screen.height/2)+(this.hud.currentPos.y+(this.hud.bgSize.y/2)-this.hud.start.y)*ratio.y);
		}
	}
	
	public override void UpdateGUISize(bool updateChilds, Vector2 ratio)
	{
		this.UpdateBaseMesh(ratio);
	}
}
