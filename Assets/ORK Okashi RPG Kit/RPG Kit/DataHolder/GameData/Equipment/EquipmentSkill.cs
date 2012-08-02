
using UnityEngine;
using System.Collections;

public class EquipmentSkill
{
	public int skill = 0;
	public int skillLevel = 1;
	
	// requirements
	public bool requireLevel = false;
	public int level = 0;
	
	public bool requireClassLevel = false;
	public int classLevel = 0;
	
	public bool requireClass = false;
	public int classNumber = 0;
	
	public bool[] requireStatus;
	public int[] statusValue;
	public ValueCheck[] statusRequirement;
	
	public EquipmentSkill()
	{
		
	}
	
	public EquipmentSkill(int svCount)
	{
		requireStatus = new bool[svCount];
		statusValue = new int[svCount];
		statusRequirement = new ValueCheck[svCount];
	}
	
	public bool CanUseSkill(Character c)
	{
		bool can = true;
		if(this.requireLevel && c.currentLevel < this.level)
		{
			can = false;
		}
		if(can && this.requireClass && c.currentClass != this.classNumber)
		{
			can = false;
		}
		if(this.requireClassLevel && c.currentClassLevel < this.classLevel)
		{
			can = false;
		}
		if(can)
		{
			for(int i=0; i<this.requireStatus.Length; i++)
			{
				if(this.requireStatus[i])
				{
					if((ValueCheck.EQUALS.Equals(this.statusRequirement[i]) && this.statusValue[i] != c.status[i].GetValue()) ||
							(ValueCheck.LESS.Equals(this.statusRequirement[i]) && this.statusValue[i] >= c.status[i].GetValue()) ||
							(ValueCheck.GREATER.Equals(this.statusRequirement[i]) && this.statusValue[i] <= c.status[i].GetValue()))
					{
						can = false;
						break;
					}
				}
			}
		}
		return can;
	}
}
