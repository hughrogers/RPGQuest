
using UnityEngine;
using System.Collections;

public class AnimationHelper
{
	public static float GetLength(Animation animation, string n)
	{
		float t = 0;
		if(animation != null && n != "" && animation[n] != null)
		{
			t = animation[n].length/animation[n].speed;
		}
		return t;
	}
}
