
using UnityEngine;
using System.Collections;

public class RecalculationHelper
{
	public static Rect RecalculateRect(Rect rect, float factorX, float factorY)
	{
		rect.x *= factorX;
		rect.y *= factorY;
		rect.width *= factorX;
		rect.height *= factorY;
		return rect;
	}
	
	public static Vector4 RecalculateVector4(Vector4 v4, float factorX, float factorY)
	{
		v4.x *= factorX;
		v4.y *= factorY;
		v4.z *= factorX;
		v4.w *= factorY;
		return v4;
	}
	
	public static Vector2 RecalculateVector2(Vector2 v2, float factorX, float factorY)
	{
		v2.x *= factorX;
		v2.y *= factorY;
		return v2;
	}
}
