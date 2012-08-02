
using UnityEngine;
using System.Collections;

public class WindowHandler
{
	// ids
	private int freeID = 0;
	// Hashtable(int id, float time)
	private Hashtable list;
	private int focusID = -1;
	private bool blockFocus = false;
	
	// checking
	private float removeTimeout = 5;
	private float checkTimeout = 10;
	private float lastTime = 0;
	
	private DialoguePosition[] shown = new DialoguePosition[0];
	
	public WindowHandler()
	{
		this.list = new Hashtable();
	}
	
	public void BlockFocus(bool block)
	{
		this.blockFocus = block;
	}
	
	public bool IsFocusBlocked()
	{
		return this.blockFocus;
	}
	
	public void  SetFocusID(int id)
	{
		if(!this.blockFocus)
		{
			this.focusID = id;
		}
	}
	
	public int GetFocusID()
	{
		return this.focusID;
	}
	
	public int GetID(int id)
	{
		// check old id and update timestamp
		if(id >= 0 && list.ContainsKey(id))
		{
			list[id] = Time.time;
		}
		// get new id
		else
		{
			id = this.freeID;
			this.freeID++;
			this.list.Add(id, Time.time);
		}
		return id;
	}
	
	public void Tick(float t)
	{
		if((Time.time - this.lastTime) > this.checkTimeout)
		{
			this.CheckIDs();
		}
	}
	
	public void CheckIDs()
	{
		this.lastTime = Time.time;
		int highest = -1;
		ArrayList remove = new ArrayList();
		foreach(DictionaryEntry entry in this.list)
		{
			int id = (int)entry.Key;
			if((Time.time - (float)entry.Value) > this.removeTimeout)
			{
				remove.Add(id);
			}
			else if(id > highest)
			{
				highest = id;
			}
		}
		foreach(int id in remove) this.list.Remove(id);
		this.freeID = highest+1;
	}
	
	/*
	============================================================================
	GUI click functions
	============================================================================
	*/
	public void AddPosition(DialoguePosition dp)
	{
		this.shown = ArrayHelper.Add(dp, this.shown);
	}
	
	public void RemovePosition(DialoguePosition dp)
	{
		this.shown = ArrayHelper.Remove(dp, this.shown);
	}
	
	public bool IsInGUI(Vector3 point)
	{
		point = VectorHelper.ScreenToGUI(point);
		bool inGUI = false;
		// dialogue positions
		for(int i=0; i<this.shown.Length; i++)
		{
			if(VectorHelper.InGUIRect(point, this.shown[i].boxBounds))
			{
				inGUI = true;
				break;
			}
		}
		// HUD
		if(!inGUI)
		{
			for(int i=0; i<GameHandler.GetLevelHandler().hud.Length; i++)
			{
				if(GameHandler.GetLevelHandler().hud[i].IsInHUD(point))
				{
					inGUI = true;
					break;
				}
			}
		}
		return inGUI;
	}
}
