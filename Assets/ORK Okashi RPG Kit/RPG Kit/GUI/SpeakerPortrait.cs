
using UnityEngine;

public class SpeakerPortrait
{
	public bool inBox = false;
	public Vector2 position = Vector2.zero;
	public Texture2D image;
	
	public SpeakerPortrait(string imageName, Vector2 pos, bool ib)
	{
		this.position = pos;
		this.inBox = ib;
		this.image = (Texture2D)Resources.Load(SpeakerPortrait.GetIconPath()+imageName, typeof(Texture2D));
		this.image.wrapMode = TextureWrapMode.Clamp;
	}
	
	public static string GetIconPath() { return "Portraits/"; }
	
	public void ShowPortrait()
	{
		// scale by height
		Vector3 pos = new Vector3(this.position.x, this.position.y);
		pos = GameHandler.GetLevelHandler().guiMatrix.MultiplyPoint3x4(pos);
		Rect speakerPos = new Rect(pos.x, pos.y, 
				this.image.width, this.image.height);
		Vector2 ratio = new Vector2(Screen.width / DataHolder.GameSettings().defaultScreen.x,
					Screen.height / DataHolder.GameSettings().defaultScreen.y);
		speakerPos.width *= ratio.y;
		speakerPos.height *= ratio.y;
		GUI.matrix = GameHandler.GetLevelHandler().defaultMatrix;
		GUI.DrawTexture(speakerPos, this.image, ScaleMode.ScaleToFit);
		GUI.matrix = GameHandler.GetLevelHandler().guiMatrix;
	}
}
