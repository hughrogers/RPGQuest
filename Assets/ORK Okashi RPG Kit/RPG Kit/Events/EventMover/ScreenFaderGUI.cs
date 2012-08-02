
using UnityEngine;
using System.Collections;

public class ScreenFaderGUI : ScreenFader
{
	void OnGUI()
	{
		if(this.tex)
		{
			GUI.depth = -1000;
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), this.tex);
		}
	}
}
