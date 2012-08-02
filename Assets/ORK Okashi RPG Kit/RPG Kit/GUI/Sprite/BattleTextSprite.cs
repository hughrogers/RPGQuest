
using UnityEngine;
using System.Collections;

public class BattleTextSprite : BaseSprite
{
	public BattleText battleText;
	
	void Start()
	{	
		this.InitMesh();
		this.startColor.a = this.battleText.settings.fadeInStart;
		GUIFont font = null;
		if(DataHolder.BattleSystemData().textSkin != null) 
			font = DataHolder.Fonts().GetFont(DataHolder.BattleSystemData().textSkin.font);
		if(font != null)
		{
			LabelContent label = new LabelContent(new GUIContent(this.battleText.text), 0, 0, 
					this.battleText.color, this.battleText.shadowColor, font);
			Texture2D texture = TextureDrawer.GetCleanTexture(
					TextureDrawer.GetNextPowerOfTwo(label.bounds.width),
					TextureDrawer.GetNextPowerOfTwo(label.bounds.height));
			texture = font.AddTextTexture(texture, label, 0, 
					new Vector2((texture.width-label.bounds.width)/2, (texture.height-label.bounds.height)/2),
					this.battleText.settings.showShadow, this.battleText.settings.shadowOffset);
			texture.Apply();
			this.renderer.material.mainTexture = texture;
			transform.position = new Vector3(0, -0.5f, 0);
			this.UpdateGUISize(false, GameHandler.GUIHandler().GetScreenRatio());
		}
		else GameObject.Destroy(this.gameObject);
	}
	
	void LateUpdate()
	{
		if(this.battleText == null)
		{
			GameObject.Destroy(this.gameObject);
		}
		else
		{
			if(mesh.colors[0].a != this.battleText.alpha)
			{
				Color[] colors = new Color[mesh.colors.Length];
				for(int i=0; i<colors.Length; i++)
				{
					colors[i] = new Color(1, 1, 1, this.battleText.alpha);
				}
				mesh.colors = colors;
			}
			Vector3 pos = this.battleText.GetScreenPosition();
			transform.position = new Vector3(
						-(Screen.width/2)+pos.x,
						0+(this.spriteID/100.0f), 
						-(Screen.height/2)+pos.y);
		}
	}
	
	public override void UpdateGUISize(bool updateChilds, Vector2 ratio)
	{
		this.UpdateBaseMesh(ratio);
	}
	
	void OnDestroy()
	{
		GameHandler.GUIHandler().RemoveSprite(this);
		GameObject.Destroy(this.renderer.material.mainTexture);
	}
}
