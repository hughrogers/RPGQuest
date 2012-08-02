
using UnityEngine;
using System.Collections;

public class GUISkinWrapper
{
	public GUISkin skin;
	
	public SkinBG box;
	public SkinBG window;
	public SkinBG windowSelected;
	public SkinBG button;
	public SkinBG verticalScrollbar;
	public SkinBG verticalScrollbarThumb;
	
	public GUISkinWrapper(GUISkin s)
	{
		this.skin = s;
		if(this.skin != null)
		{
			this.box = new SkinBG(this.skin.box.normal.background, this.skin.box.border);
			this.window = new SkinBG(this.skin.window.normal.background, this.skin.window.border);
			this.windowSelected = new SkinBG(this.skin.window.onNormal.background, this.skin.window.border);
			this.button = new SkinBG(this.skin.button.normal.background, this.skin.button.border);
			this.verticalScrollbar = new SkinBG(this.skin.verticalScrollbar.normal.background, this.skin.verticalScrollbar.border);
			this.verticalScrollbarThumb = new SkinBG(this.skin.verticalScrollbarThumb.normal.background, this.skin.verticalScrollbarThumb.border);
		}
	}
}