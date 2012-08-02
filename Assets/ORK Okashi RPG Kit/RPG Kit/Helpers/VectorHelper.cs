
using UnityEngine;
using System.Collections;

public class VectorHelper
{
	/*
	============================================================================
	Hashtable functions
	============================================================================
	*/
	public static Vector3 FromHashtable(Hashtable ht)
	{
		return VectorHelper.FromHashtable(ht, "x", "y", "z");
	}
	
	public static Vector3 FromHashtable(Hashtable ht, string x, string y, string z)
	{
		return new Vector3(
				float.Parse((string)ht[x]),
				float.Parse((string)ht[y]),
				float.Parse((string)ht[z]));
	}
	
	public static void ToHashtable(ref Hashtable ht, Vector3 v)
	{
		VectorHelper.ToHashtable(ref ht, v, "x", "y", "z");
	}
	
	public static void ToHashtable(ref Hashtable ht, Vector3 v, string x, string y, string z)
	{
		ht.Add(x, v.x.ToString());
		ht.Add(y, v.y.ToString());
		ht.Add(z, v.z.ToString());
	}
	
	public static Vector2 FromHashtable(Hashtable ht, string x, string y)
	{
		return new Vector2(
				float.Parse((string)ht[x]),
				float.Parse((string)ht[y]));
	}
	
	public static void ToHashtable(ref Hashtable ht, Vector2 v, string x, string y)
	{
		ht.Add(x, v.x.ToString());
		ht.Add(y, v.y.ToString());
	}
	
	/*
	============================================================================
	Transformation functions
	============================================================================
	*/
	public static Vector3 GetDirection(Vector3 origin, Vector3 target)
	{
		return target-origin;
	}
	
	public static Vector3 ScreenToGUI(Vector3 point)
	{
		point.y = Screen.height - point.y;
		point = GameHandler.GetLevelHandler().revertMatrix.MultiplyPoint3x4(point);
		return point;
	}
	
	/*
	============================================================================
	Screen functions
	============================================================================
	*/
	public static Vector3 GetScreenCenter()
	{
		Vector3 center = Vector3.zero;
		Camera cam = Camera.main;
		if(cam != null) center = new Vector3(cam.pixelWidth/2, cam.pixelHeight/2, 0);
		else center = new Vector3(Screen.width/2, Screen.height/2, 0);
		return center;
	}
	
	public static Vector3 GetTouchPosition(int touchCount)
	{
		Vector3 pos = Vector3.zero;
		if(touchCount > 0)
		{
			for(int i=0; i<touchCount; i++)
			{
				pos.x += Input.touches[i].position.x;
				pos.y += Input.touches[i].position.y;
			}
			pos /= touchCount;
		}
		return pos;
	}
	
	public static Vector3 GetTouchMove(int touchCount)
	{
		Vector3 pos = Vector3.zero;
		if(touchCount > 0)
		{
			for(int i=0; i<touchCount; i++)
			{
				pos.x += Input.touches[i].deltaPosition.x;
				pos.y += Input.touches[i].deltaPosition.y;
			}
			pos /= touchCount;
		}
		return pos;
	}
	
	/*
	============================================================================
	GUI functions
	============================================================================
	*/
	public static bool InGUIRect(Vector3 point, Rect rect)
	{
		return rect.x < point.x && (rect.x+rect.width) > point.x &&
				rect.y < point.y && (rect.y+rect.height) > point.y;
	}
	
	/*
	============================================================================
	Limit functions
	============================================================================
	*/
	public static void LimitVector3(ref Vector3 v, float min, float max)
	{
		if(v.x < min) v.x = min;
		else if(v.x > max) v.x = max;
		if(v.y < min) v.y = min;
		else if(v.y > max) v.y = max;
		if(v.z < min) v.z = min;
		else if(v.z > max) v.z = max;
	}
	
	/*
	============================================================================
	Distance functions
	============================================================================
	*/
	public static float Distance(Vector3 v1, Vector3 v2, bool ignoreYDistance)
	{
		if(ignoreYDistance) v1.y = v2.y;
		return Vector3.Distance(v1, v2);
	}
}
