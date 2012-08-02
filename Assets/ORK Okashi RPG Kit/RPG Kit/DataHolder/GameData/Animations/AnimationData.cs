
using UnityEngine;
using System.Collections;

public class AnimationData
{
	public string name = "";
	public int layer = 0;
	
	public bool setSpeed = false;
	public int speedFormula = 0;
	
	public AnimationData(int l)
	{
		this.layer = l;
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		ht.Add(XMLHandler.CONTENT, this.name);
		ht.Add("id", this.layer.ToString());
		if(this.setSpeed)
		{
			ht.Add("speed", this.speedFormula.ToString());
		}
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		this.name = ht[XMLHandler.CONTENT] as string;
		if(ht.ContainsKey("id"))
		{
			this.layer = int.Parse((string)ht["id"]);
		}
		if(ht.ContainsKey("speed"))
		{
			this.setSpeed = true;
			this.speedFormula = int.Parse((string)ht["speed"]);
		}
	}
	
	/*
	============================================================================
	In-game functions
	============================================================================
	*/
	public void Init(Animation animation, Combatant c)
	{
		if(this.name != "" && animation[this.name] != null)
		{
			animation[this.name].layer = layer;
			if(this.setSpeed && c != null)
			{
				animation[this.name].speed = DataHolder.Formula(this.speedFormula).Calculate(c, c)*GameHandler.AnimationFactor;
			}
		}
	}
}
