
using UnityEngine;
using System.Collections;

public class BattleTextGUI : BattleText
{
	void OnGUI()
	{
		if(this.text != null && this.cam != null && this.settings != null)
		{
			Vector3 pos = this.GetScreenPosition();
			
			LevelHandler lh = GameHandler.GetLevelHandler();
			if(DataHolder.BattleSystemData().textSkin) GUI.skin = DataHolder.BattleSystemData().textSkin;
			
			GUIContent content = new GUIContent(this.text);
			
			if(this.counting)
			{
				content.text = content.text.Replace("%", this.currentValue.ToString());
			}
			
			GUI.matrix = lh.guiMatrix;
			pos = lh.revertMatrix.MultiplyPoint3x4(pos);
			
			GUIStyle textStyle = new GUIStyle(GUI.skin.label);
			Vector2 v = textStyle.CalcSize(content);
			pos.x -= v.x/2;
			pos.y -= v.y/2;
			
			if(this.settings.showShadow)
			{
				textStyle.normal.textColor = this.shadowColor;
				GUI.Label(new Rect(pos.x+this.settings.shadowOffset.x, pos.y + this.settings.shadowOffset.y, v.x, v.y), content, textStyle); 
			}
			
			textStyle.normal.textColor = this.color;
			GUI.Label(new Rect(pos.x, pos.y, v.x, v.y), content, textStyle);
		}
	}
}
