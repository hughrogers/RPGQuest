
using UnityEngine;
using System.Collections;

public class GroupCondition
{
	public bool firstMove = false;
	public int firstMoveRounds = 1;
	public float timebar = 0;
	
	public bool[] setStatus;
	public int[] status;
	
	public SkillEffect[] effect;
	
	public GroupCondition()
	{
		this.setStatus = new bool[DataHolder.StatusValueCount];
		this.status = new int[this.setStatus.Length];
		this.effect = new SkillEffect[DataHolder.EffectCount];
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		ArrayList s = new ArrayList();
		
		if(this.firstMove)
		{
			ht.Add("firstmove", "true");
			ht.Add("firstmoverounds", this.firstMoveRounds.ToString());
		}
		ht.Add("timebar", this.timebar.ToString());
		
		for(int i=0; i<this.setStatus.Length; i++)
		{
			if(this.setStatus[i] && DataHolder.StatusValue(i).IsConsumable())
			{
				Hashtable ht2 = HashtableHelper.GetTitleHashtable(XMLName.VALUE, i);
				ht2.Add("value", this.status[i].ToString());
				s.Add(ht2);
			}
		}
		
		for(int i=0; i<this.effect.Length; i++)
		{
			if(!SkillEffect.NONE.Equals(this.effect[i]))
			{
				Hashtable ht2 = HashtableHelper.GetTitleHashtable(XMLName.EFFECT, i);
				ht2.Add("type", this.effect[i].ToString());
				s.Add(ht2);
			}
		}
		
		if(s.Count > 0) ht.Add(XMLHandler.NODES, s);
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		if(ht.ContainsKey("firstmove"))
		{
			this.firstMove = true;
			this.firstMoveRounds = int.Parse((string)ht["firstmoverounds"]);
		}
		this.timebar = float.Parse((string)ht["timebar"]);
		
		if(ht.ContainsKey(XMLHandler.NODES))
		{
			ArrayList s = ht[XMLHandler.NODES] as ArrayList;
			foreach(Hashtable ht2 in s)
			{
				if(ht2[XMLHandler.NODE_NAME] as string == XMLName.VALUE)
				{
					int id = int.Parse((string)ht2["id"]);
					if(id < this.setStatus.Length && 
						DataHolder.StatusValue(id).IsConsumable())
					{
						this.setStatus[id] = true;
						this.status[id] = int.Parse((string)ht2["value"]);
					}
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == XMLName.EFFECT)
				{
					int id = int.Parse((string)ht2["id"]);
					if(id < this.effect.Length)
					{
						this.effect[id] = (SkillEffect)System.Enum.Parse(
								typeof(SkillEffect), (string)ht2["type"]);
					}
				}
			}
		}
	}
	
	/*
	============================================================================
	Condition functions
	============================================================================
	*/
	public void ApplyCondition(Combatant c)
	{
		if(DataHolder.BattleSystem().IsActiveTime())
		{
			c.timeBar = this.timebar;
			if(c.timeBar > DataHolder.BattleSystem().maxTimebar)
			{
				c.timeBar = DataHolder.BattleSystem().maxTimebar;
			}
		}
		
		for(int i=0; i<this.setStatus.Length; i++)
		{
			if(this.setStatus[i])
			{
				c.status[i].SetValue(this.status[i], true, false, false);
			}
		}
		
		for(int i=0; i<this.effect.Length; i++)
		{
			if(SkillEffect.ADD.Equals(this.effect[i]))
			{
				c.AddEffect(i, c);
			}
			else if(SkillEffect.REMOVE.Equals(this.effect[i]))
			{
				c.RemoveEffect(i);
			}
		}
	}
}
