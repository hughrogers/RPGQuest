
using UnityEngine;
using System.Collections;

public class SkinBG
{
	public Color[] tl;
	public Color[] tc;
	public Color[] tr;
	public Color[] cl;
	public Color[] cc;
	public Color[] cr;
	public Color[] bl;
	public Color[] bc;
	public Color[] br;
	
	public RectOffset border;
	public Vector2 size;
	
	public SkinBG(Texture2D bgTex, RectOffset b)
	{
		size = new Vector2(bgTex.width, bgTex.height);
		border = b;
		if(border.left > 0)
		{
			// bottom left
			this.bl = TextureDrawer.GetSecuredPixels(bgTex, 0, 0, border.left, border.bottom);
			// left center
			this.cl = TextureDrawer.GetSecuredPixels(bgTex, 0, border.bottom, border.left, bgTex.height-border.vertical);
			// top left
			this.tl =TextureDrawer.GetSecuredPixels(bgTex, 0, bgTex.height-border.top, border.left, border.top);
		}
		
		// bottom center
		this.bc = TextureDrawer.GetSecuredPixels(bgTex, border.left, 0, bgTex.width-border.horizontal, border.bottom);
		// center center
		this.cc = TextureDrawer.GetSecuredPixels(bgTex, border.left, border.bottom, bgTex.width-border.horizontal, bgTex.height-border.vertical);
		// top center
		this.tc = TextureDrawer.GetSecuredPixels(bgTex, border.left, bgTex.height-border.top, bgTex.width-border.horizontal, border.top);
		
		if(border.right > 0)
		{
			// bottom right
			this.br = TextureDrawer.GetSecuredPixels(bgTex, bgTex.width-border.right, 0, border.right, border.bottom);
			// right center
			this.cr = TextureDrawer.GetSecuredPixels(bgTex, bgTex.width-border.right, border.bottom, border.right, bgTex.height-border.vertical);
			// top right
			this.tr = TextureDrawer.GetSecuredPixels(bgTex, bgTex.width-border.right, bgTex.height-border.top, border.right, border.top);
		}
	}
}
