
using UnityEngine;
using System.Collections;

public class ControlHandler
{
	/*
	============================================================================
	Button functions
	============================================================================
	*/
	public static bool IsPressed(string key, InputHandling inputHandling)
	{
		bool pressed = false;
		if(key != null && key != "" && 
			((InputHandling.BUTTON_DOWN.Equals(inputHandling) && Input.GetButtonDown(key)) ||
			(InputHandling.BUTTON_UP.Equals(inputHandling) && Input.GetButtonUp(key)) ||
			(InputHandling.KEY_DOWN.Equals(inputHandling) && Input.GetKeyDown(key)) ||
			(InputHandling.KEY_UP.Equals(inputHandling) && Input.GetKeyUp(key))))
		{
			pressed = true;
		}
		return pressed;
	}
	
	public static bool IsPressed(string key)
	{
		return ControlHandler.IsPressed(key, DataHolder.GameSettings().inputHandling);
	}
	
	public static bool IsHeld(string key, InputHandling inputHandling)
	{
		bool held = false;
		if(key != null && key != "" && 
			(((InputHandling.BUTTON_DOWN.Equals(inputHandling) || 
			InputHandling.BUTTON_UP.Equals(inputHandling)) && Input.GetButton(key)) ||
			((InputHandling.KEY_DOWN.Equals(inputHandling) || 
			InputHandling.KEY_UP.Equals(inputHandling)) && Input.GetKey(key))))
		{
			held = true;
		}
		return held;
	}
	
	public static bool IsHeld(string key)
	{
		return ControlHandler.IsHeld(key, DataHolder.GameSettings().inputHandling);
	}
	
	public static bool IsAcceptPressed()
	{
		return ControlHandler.IsPressed(DataHolder.GameSettings().acceptKey);
	}
	
	public static bool IsCancelPressed()
	{
		return ControlHandler.IsPressed(DataHolder.GameSettings().cancelKey);
	}
	
	public static bool IsSkillPlusPressed()
	{
		return ControlHandler.IsPressed(DataHolder.GameSettings().skillPlusKey);
	}
	
	public static bool IsSkillMinusPressed()
	{
		return ControlHandler.IsPressed(DataHolder.GameSettings().skillMinusKey);
	}
	
	public static bool IsCharPlusPressed()
	{
		return ControlHandler.IsPressed(DataHolder.GameSettings().charPlusKey);
	}
	
	public static bool IsCharMinusPressed()
	{
		return ControlHandler.IsPressed(DataHolder.GameSettings().charMinusKey);
	}
	
	/*
	============================================================================
	Axis functions
	============================================================================
	*/
	public static float GetAxis(string key)
	{
		float axis = 0;
		if(key != null && key != "")
		{
			axis = Input.GetAxisRaw(key);
		}
		return axis;
	}
	
	public static float GetVertical()
	{
		return ControlHandler.GetAxis(DataHolder.GameSettings().verticalKey);
	}
	
	public static float GetHorizontal()
	{
		return ControlHandler.GetAxis(DataHolder.GameSettings().horizontalKey);
	}
}
