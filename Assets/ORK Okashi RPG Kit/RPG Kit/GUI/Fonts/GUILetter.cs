
using UnityEngine;
using System.Collections;

public class GUILetter
{
	public Rect bounds;
	public Vector2 placing;
	public float width;
	
	private Color[] colors;
	private float[] alpha;
	
	public GUILetter(Rect uv, Rect vert, Vector2 textureSize, float w)
	{
		this.bounds = new Rect(
				Mathf.Round((uv.x+uv.width)*textureSize.x-vert.width), 
				Mathf.Round((uv.y+uv.height)*textureSize.y+vert.height), 
				vert.width, 
				(-vert.height));
		this.placing = new Vector2(vert.x, vert.y);
		this.width = w;
	}
	
	public GUILetter(Hashtable ht)
	{
		this.SetData(ht);
	}
	
	public void InitColors(Texture2D texture)
	{
		this.colors = texture.GetPixels((int)this.bounds.x, (int)this.bounds.y, 
				(int)this.bounds.width, (int)this.bounds.height);
	}
	
	public Color[] GetLetterColors(Texture2D texture)
	{
		if(this.colors == null) this.InitColors(texture);
		Color[] cols = new Color[this.colors.Length];
		for(int i=0; i<cols.Length; i++) cols[i] = this.colors[i];
		return cols;
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		ht.Add("x", this.bounds.x.ToString());
		ht.Add("y", this.bounds.y.ToString());
		ht.Add("w", this.bounds.width.ToString());
		ht.Add("h", this.bounds.height.ToString());
		ht.Add("px", this.placing.x.ToString());
		ht.Add("py", this.placing.y.ToString());
		ht.Add("width", this.width.ToString());
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		this.bounds = new Rect(
				float.Parse((string)ht["x"]), float.Parse((string)ht["y"]),
				float.Parse((string)ht["w"]), float.Parse((string)ht["h"]));
		this.placing = new Vector2(
				float.Parse((string)ht["px"]), float.Parse((string)ht["py"]));
		this.width = float.Parse((string)ht["width"]);
	}
}