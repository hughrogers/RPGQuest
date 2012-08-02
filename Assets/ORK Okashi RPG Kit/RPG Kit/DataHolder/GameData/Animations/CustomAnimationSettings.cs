using UnityEngine;
using System.Collections;

public class CustomAnimationSettings
{
	public AnimationData[] animation = new AnimationData[0];
	
	private static string ANIMATION = "animation";
	
	public CustomAnimationSettings()
	{
		
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		ht.Add("animations", this.animation.Length.ToString());
		
		ArrayList s = new ArrayList();
		for(int i=0; i<this.animation.Length; i++)
		{
			if(this.animation[i].name != "")
			{
				Hashtable ht2 = this.animation[i].GetData(HashtableHelper.GetTitleHashtable(CustomAnimationSettings.ANIMATION));
				ht2.Add("id2", i.ToString());
				s.Add(ht2);
			}
		}
		if(s.Count > 0) ht.Add(XMLHandler.NODES, s);
		
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		this.animation = new AnimationData[int.Parse((string)ht["animations"])];
		for(int i=0; i<this.animation.Length; i++)
		{
			this.animation[i] = new AnimationData(0);
		}
		
		if(ht.ContainsKey(XMLHandler.NODES))
		{
			ArrayList s = ht[XMLHandler.NODES] as ArrayList;
			foreach(Hashtable ht2 in s)
			{
				if(ht2[XMLHandler.NODE_NAME] as string == CustomAnimationSettings.ANIMATION)
				{
					int id = int.Parse((string)ht2["id2"]);
					if(id < this.animation.Length)
					{
						this.animation[id].SetData(ht2);
					}
				}
			}
		}
	}
	
	/*
	============================================================================
	Editor functions
	============================================================================
	*/
	public void AddAnimation()
	{
		this.animation = ArrayHelper.Add(new AnimationData(0), this.animation);
	}
	
	public void RemoveAnimation(int index)
	{
		this.animation = ArrayHelper.Remove(index, this.animation);
	}
	
	/*
	============================================================================
	Animation handling functions
	============================================================================
	*/
	public void SetAnimationLayers(Combatant c)
	{
		Animation a = c.GetAnimationComponent();
		if(a != null)
		{
			for(int i=0; i<this.animation.Length; i++)
			{
				this.animation[i].Init(a, c);
			}
		}
	}
}
