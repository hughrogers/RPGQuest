
using UnityEngine;

public class TextPosition
{
	public Rect bounds;
	public Vector4 padding = new Vector4(10, 10, 10, 10);
	public float lineSpacing = 10;
	public bool showBox = true;
	
	public TextPosition(float x, float y, float width, float height) : this(new Rect(x, y, width, height))
	{
		
	}
	
	public TextPosition(Rect b)
	{
		this.bounds = b;
	}
	
	public TextPosition(Rect b, Vector4 p) : this(b, p, 10)
	{
		
	}
	
	public TextPosition(Rect b, float ls) : this(b, new Vector4(10, 10, 10, 10), ls)
	{
		
	}
	
	public TextPosition(Rect b, Vector4 p, float ls) : this(b, p, ls, true)
	{
		
	}
	
	public TextPosition(Rect b, Vector4 p, float ls, bool s)
	{
		this.bounds = b;
		this.padding = p;
		this.lineSpacing = ls;
		this.showBox = s;
	}
}