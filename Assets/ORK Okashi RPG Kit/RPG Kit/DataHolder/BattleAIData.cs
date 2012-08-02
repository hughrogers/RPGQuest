
using System.Collections;

public class BattleAIData : BaseData
{
	public BattleAI[] ai = new BattleAI[0];
	
	// XML data
	private string filename = "enemyai";
	
	private static string AIS = "ais";
	private static string AI = "ai";
	private static string CONDITION = "condition";

	public BattleAIData()
	{
		LoadData();
	}
	
	public void LoadData()
	{
		ArrayList data = XMLHandler.LoadXML(dir+filename);
		
		if(data.Count > 0)
		{
			foreach(Hashtable entry in data)
			{
				if(entry[XMLHandler.NODE_NAME] as string == BattleAIData.AIS)
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList subs = entry[XMLHandler.NODES] as ArrayList;
						name = new string[subs.Count];
						ai = new BattleAI[subs.Count];
						foreach(Hashtable val in subs)
						{
							if(val[XMLHandler.NODE_NAME] as string == BattleAIData.AI)
							{
								int i = int.Parse((string)val["id"]);
								ai[i] = new BattleAI();
								ai[i].needed = (AIConditionNeeded)System.Enum.Parse(typeof(AIConditionNeeded), (string)val["needed"]);
								ai[i].condition = new AICondition[int.Parse((string)val["conditions"])];
								
								ArrayList s = val[XMLHandler.NODES] as ArrayList;
								foreach(Hashtable ht in s)
								{
									if(ht[XMLHandler.NODE_NAME] as string == BattleAIData.NAME)
									{
										name[i] = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == BattleAIData.CONDITION)
									{
										int j = int.Parse((string)ht["id"]);
										ai[i].condition[j] = new AICondition();
										ai[i].condition[j].target = (AIConditionTarget)System.Enum.Parse(typeof(AIConditionTarget), (string)ht["target"]);
										ai[i].condition[j].type = (AIConditionType)System.Enum.Parse(typeof(AIConditionType), (string)ht["type"]);
										if(AIConditionType.STATUS.Equals(ai[i].condition[j].type))
										{
											ai[i].condition[j].statusID = int.Parse((string)ht["statusid"]);
											ai[i].condition[j].statusSetter = (ValueSetter)System.Enum.Parse(typeof(ValueSetter), (string)ht["statussetter"]);
											ai[i].condition[j].statusValue = int.Parse((string)ht["statusvalue"]);
											ai[i].condition[j].statusCheck = (ValueCheck)System.Enum.Parse(typeof(ValueCheck), (string)ht["statuscheck"]);
											ai[i].condition[j].checkStatusID = int.Parse((string)ht["checkstatusid"]);
										}
										else if(AIConditionType.EFFECT.Equals(ai[i].condition[j].type))
										{
											ai[i].condition[j].effectID = int.Parse((string)ht["effectid"]);
											ai[i].condition[j].effectActive = (ActiveSelection)System.Enum.Parse(typeof(ActiveSelection), (string)ht["effectactive"]);
										}
										else if(AIConditionType.ELEMENT.Equals(ai[i].condition[j].type))
										{
											ai[i].condition[j].elementID = int.Parse((string)ht["elementid"]);
											ai[i].condition[j].elementValue = int.Parse((string)ht["elementvalue"]);
											ai[i].condition[j].elementCheck = (ValueCheck)System.Enum.Parse(typeof(ValueCheck), (string)ht["elementcheck"]);
										}
										else if(AIConditionType.TURN.Equals(ai[i].condition[j].type))
										{
											ai[i].condition[j].turn = int.Parse((string)ht["turn"]);
											ai[i].condition[j].everyTurn = bool.Parse((string)ht["everyturn"]);
										}
										else if(AIConditionType.CHANCE.Equals(ai[i].condition[j].type))
										{
											ai[i].condition[j].chance = int.Parse((string)ht["chance"]);
										}
										else if(AIConditionType.DEATH.Equals(ai[i].condition[j].type))
										{
											ai[i].condition[j].isDead = bool.Parse((string)ht["isdead"]);
										}
										else if(AIConditionType.RACE.Equals(ai[i].condition[j].type))
										{
											ai[i].condition[j].raceID = int.Parse((string)ht["race"]);
										}
										else if(AIConditionType.SIZE.Equals(ai[i].condition[j].type))
										{
											ai[i].condition[j].sizeID = int.Parse((string)ht["size"]);
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}
	
	public void SaveData()
	{
		if(name == null) return;
		ArrayList data = new ArrayList();
		ArrayList subs = new ArrayList();
		Hashtable sv = new Hashtable();
		
		sv.Add(XMLHandler.NODE_NAME, BattleAIData.AIS);
		
		for(int i=0; i<name.Length; i++)
		{
			Hashtable ht = new Hashtable();
			ArrayList s = new ArrayList();
			
			ht.Add(XMLHandler.NODE_NAME, BattleAIData.AI);
			ht.Add("id", i.ToString());
			ht.Add("needed", ai[i].needed.ToString());
			
			Hashtable n = new Hashtable();
			n.Add(XMLHandler.NODE_NAME, BattleAIData.NAME);
			n.Add(XMLHandler.CONTENT, name[i]);
			s.Add(n);
			
			ht.Add("conditions", ai[i].condition.Length.ToString());
			for(int j=0; j<ai[i].condition.Length; j++)
			{
				Hashtable c = new Hashtable();
				c.Add(XMLHandler.NODE_NAME, BattleAIData.CONDITION);
				c.Add("id", j.ToString());
				c.Add("target", ai[i].condition[j].target.ToString());
				c.Add("type", ai[i].condition[j].type.ToString());
				if(AIConditionType.STATUS.Equals(ai[i].condition[j].type))
				{
					c.Add("statusid", ai[i].condition[j].statusID.ToString());
					c.Add("statussetter", ai[i].condition[j].statusSetter.ToString());
					c.Add("statusvalue", ai[i].condition[j].statusValue.ToString());
					c.Add("statuscheck", ai[i].condition[j].statusCheck.ToString());
					c.Add("checkstatusid", ai[i].condition[j].checkStatusID.ToString());
				}
				else if(AIConditionType.EFFECT.Equals(ai[i].condition[j].type))
				{
					c.Add("effectid", ai[i].condition[j].effectID.ToString());
					c.Add("effectactive", ai[i].condition[j].effectActive.ToString());
				}
				else if(AIConditionType.ELEMENT.Equals(ai[i].condition[j].type))
				{
					c.Add("elementid", ai[i].condition[j].elementID.ToString());
					c.Add("elementvalue", ai[i].condition[j].elementValue.ToString());
					c.Add("elementcheck", ai[i].condition[j].elementCheck.ToString());
				}
				else if(AIConditionType.TURN.Equals(ai[i].condition[j].type))
				{
					c.Add("turn", ai[i].condition[j].turn.ToString());
					c.Add("everyturn", ai[i].condition[j].everyTurn.ToString());
				}
				else if(AIConditionType.CHANCE.Equals(ai[i].condition[j].type))
				{
					c.Add("chance", ai[i].condition[j].chance.ToString());
				}
				else if(AIConditionType.DEATH.Equals(ai[i].condition[j].type))
				{
					c.Add("isdead", ai[i].condition[j].isDead.ToString());
				}
				else if(AIConditionType.RACE.Equals(ai[i].condition[j].type))
				{
					c.Add("race", ai[i].condition[j].raceID.ToString());
				}
				else if(AIConditionType.SIZE.Equals(ai[i].condition[j].type))
				{
					c.Add("size", ai[i].condition[j].sizeID.ToString());
				}
				s.Add(c);
			}
			
			ht.Add(XMLHandler.NODES, s);
			subs.Add(ht);
		}
		sv.Add(XMLHandler.NODES, subs);
		
		data.Add(sv);
		XMLHandler.SaveXML(dir, filename, data);
	}
	
	public void AddAI(string n)
	{
		if(name == null)
		{
			name = new string[] {n};
		}
		else
		{
			name = ArrayHelper.Add(n, name);
		}
		ai = ArrayHelper.Add(new BattleAI(), ai);
	}
	
	public override void RemoveData(int index)
	{
		name = ArrayHelper.Remove(index, name);
		if(name.Length == 0) name = null;
		ai = ArrayHelper.Remove(index, ai);
	}
	
	public override void Copy(int index)
	{
		this.AddAI(name[index]);
		ai[ai.Length-1].needed = ai[index].needed;
		
		ai[ai.Length-1].condition = new AICondition[ai[index].condition.Length];
		for(int i=0; i<ai[index].condition.Length; i++)
		{
			ai[ai.Length-1].condition[i] = new AICondition();
			ai[ai.Length-1].condition[i].target = ai[index].condition[i].target;
			ai[ai.Length-1].condition[i].type = ai[index].condition[i].type;
			ai[ai.Length-1].condition[i].statusID = ai[index].condition[i].statusID;
			ai[ai.Length-1].condition[i].statusSetter = ai[index].condition[i].statusSetter;
			ai[ai.Length-1].condition[i].statusValue = ai[index].condition[i].statusValue;
			ai[ai.Length-1].condition[i].statusCheck = ai[index].condition[i].statusCheck;
			ai[ai.Length-1].condition[i].checkStatusID = ai[index].condition[i].checkStatusID;
			ai[ai.Length-1].condition[i].effectID = ai[index].condition[i].effectID;
			ai[ai.Length-1].condition[i].effectActive = ai[index].condition[i].effectActive;
			ai[ai.Length-1].condition[i].elementID = ai[index].condition[i].elementID;
			ai[ai.Length-1].condition[i].elementValue = ai[index].condition[i].elementValue;
			ai[ai.Length-1].condition[i].elementCheck = ai[index].condition[i].elementCheck;
			ai[ai.Length-1].condition[i].turn = ai[index].condition[i].turn;
			ai[ai.Length-1].condition[i].everyTurn = ai[index].condition[i].everyTurn;
			ai[ai.Length-1].condition[i].chance = ai[index].condition[i].chance;
			ai[ai.Length-1].condition[i].isDead = ai[index].condition[i].isDead;
			ai[ai.Length-1].condition[i].raceID = ai[index].condition[i].raceID;
			ai[ai.Length-1].condition[i].sizeID = ai[index].condition[i].sizeID;
		}
	}
	
	public void AddCondition(int index)
	{
		ai[index].condition = ArrayHelper.Add(new AICondition(), ai[index].condition);
	}
	
	public void RemoveCondition(int index, int c)
	{
		ai[index].condition = ArrayHelper.Remove(c, ai[index].condition);
	}
	
	public void RemoveStatusValue(int index)
	{
		for(int i=0; i<ai.Length; i++)
		{
			for(int j=0; j<ai[i].condition.Length; j++)
			{
				if(ai[i].condition[j].statusID == index)
				{
					ai[i].condition[j].statusID = 0;
				}
				else if(ai[i].condition[j].statusID > index)
				{
					ai[i].condition[j].statusID -= 1;
				}
				if(ai[i].condition[j].checkStatusID == index)
				{
					ai[i].condition[j].checkStatusID = 0;
				}
				else if(ai[i].condition[j].checkStatusID > index)
				{
					ai[i].condition[j].checkStatusID -= 1;
				}
			}
		}
	}
	
	public void RemoveStatusEffect(int index)
	{
		for(int i=0; i<ai.Length; i++)
		{
			for(int j=0; j<ai[i].condition.Length; j++)
			{
				if(ai[i].condition[j].effectID == index)
				{
					ai[i].condition[j].effectID = 0;
				}
				else if(ai[i].condition[j].effectID > index)
				{
					ai[i].condition[j].effectID -= 1;
				}
			}
		}
	}
	
	public void RemoveRace(int index)
	{
		for(int i=0; i<ai.Length; i++)
		{
			for(int j=0; j<ai[i].condition.Length; j++)
			{
				ai[i].condition[j].raceID = this.CheckForIndex(index, ai[i].condition[j].raceID);
			}
		}
	}
	
	public void RemoveSize(int index)
	{
		for(int i=0; i<ai.Length; i++)
		{
			for(int j=0; j<ai[i].condition.Length; j++)
			{
				ai[i].condition[j].sizeID = this.CheckForIndex(index, ai[i].condition[j].sizeID);
			}
		}
	}
}