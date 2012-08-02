
using UnityEngine;
using System.Collections;

public class CharacterControlMap
{
	public InputHandling inputHandling = InputHandling.BUTTON_DOWN;
	public float axisTimeout = 0.5f;
	
	public string[] attackKey = new string[0];
	public bool[] attackAxis = new bool[0];
	public float[] attackTrigger = new float[0];
	public bool[] attackAir = new bool[0];
	public float[] attackSpeed = new float[0];
	
	public int[] skillID = new int[0];
	public string[] skillKey = new string[0];
	public bool[] skillAxis = new bool[0];
	public float[] skillTrigger = new float[0];
	public bool[] skillAir = new bool[0];
	public float[] skillSpeed = new float[0];
	
	public int[] itemID = new int[0];
	public string[] itemKey = new string[0];
	public bool[] itemAxis = new bool[0];
	public float[] itemTrigger = new float[0];
	public bool[] itemAir = new bool[0];
	public float[] itemSpeed = new float[0];
	
	private static string ATTACK = "attack";
	private static string SKILL = "skill";
	private static string ITEM = "item";
	
	// ingame
	private float timeout = 0;
	
	public CharacterControlMap()
	{
		
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		ht.Add("inputhandling", this.inputHandling.ToString());
		ht.Add("axistimeout", this.axisTimeout.ToString());
		ArrayList s = new ArrayList();
		if(this.attackKey.Length > 0)
		{
			ht.Add("attacks", this.attackKey.Length.ToString());
			for(int i=0; i<this.attackKey.Length; i++)
			{
				Hashtable ht2 = HashtableHelper.GetContentHashtable(
						CharacterControlMap.ATTACK, this.attackKey[i], i);
				if(this.attackAir[i])
				{
					ht2.Add("air", "true");
				}
				else ht2.Add("speed", this.attackSpeed[i].ToString());
				if(this.attackAxis[i])
				{
					ht2.Add("axis", this.attackTrigger[i].ToString());
				}
				s.Add(ht2);
			}
		}
		if(this.skillID.Length > 0)
		{
			ht.Add("skills", this.skillID.Length.ToString());
			for(int i=0; i<this.skillID.Length; i++)
			{
				Hashtable ht2 = HashtableHelper.GetContentHashtable(
						CharacterControlMap.SKILL, this.skillKey[i], i);
				ht2.Add("id2", this.skillID[i].ToString());
				if(this.skillAir[i])
				{
					ht2.Add("air", "true");
				}
				else ht2.Add("speed", this.skillSpeed[i].ToString());
				if(this.skillAxis[i])
				{
					ht2.Add("axis", this.skillTrigger[i].ToString());
				}
				s.Add(ht2);
			}
		}
		if(this.itemID.Length > 0)
		{
			ht.Add("items", this.itemID.Length.ToString());
			for(int i=0; i<this.itemID.Length; i++)
			{
				Hashtable ht2 = HashtableHelper.GetContentHashtable(
						CharacterControlMap.ITEM, this.itemKey[i], i);
				ht2.Add("id2", this.itemID[i].ToString());
				if(this.itemAir[i])
				{
					ht2.Add("air", "true");
				}
				else ht2.Add("speed", this.itemSpeed[i].ToString());
				if(this.itemAxis[i])
				{
					ht2.Add("axis", this.itemTrigger[i].ToString());
				}
				s.Add(ht2);
			}
		}
		if(s.Count > 0) ht.Add(XMLHandler.NODES, s);
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		this.inputHandling = (InputHandling)System.Enum.Parse(
				typeof(InputHandling), (string)ht["inputhandling"]);
		this.axisTimeout = float.Parse((string)ht["axistimeout"]);
		if(ht.ContainsKey("attacks"))
		{
			int count = int.Parse((string)ht["attacks"]);
			this.attackKey = new string[count];
			this.attackAir = new bool[count];
			this.attackAxis = new bool[count];
			this.attackTrigger = new float[count];
			this.attackSpeed = new float[count];
		}
		if(ht.ContainsKey("skills"))
		{
			int count = int.Parse((string)ht["skills"]);
			this.skillID = new int[count];
			this.skillKey = new string[count];
			this.skillAir = new bool[count];
			this.skillAxis = new bool[count];
			this.skillTrigger = new float[count];
			this.skillSpeed = new float[count];
		}
		if(ht.ContainsKey("items"))
		{
			int count = int.Parse((string)ht["items"]);
			this.itemID = new int[count];
			this.itemKey = new string[count];
			this.itemAir = new bool[count];
			this.itemAxis = new bool[count];
			this.itemTrigger = new float[count];
			this.itemSpeed = new float[count];
		}
		if(ht.ContainsKey(XMLHandler.NODES))
		{
			ArrayList s = ht[XMLHandler.NODES] as ArrayList;
			foreach(Hashtable ht2 in s)
			{
				if(ht2[XMLHandler.NODE_NAME] as string == CharacterControlMap.ATTACK)
				{
					int id = int.Parse((string)ht2["id"]);
					if(id < this.attackKey.Length)
					{
						this.attackKey[id] = ht2[XMLHandler.CONTENT] as string;
						if(ht2.ContainsKey("axis"))
						{
							this.attackAxis[id] = true;
							this.attackTrigger[id] = float.Parse((string)ht2["axis"]);
						}
						if(ht2.ContainsKey("air")) this.attackAir[id] = true;
						if(ht2.ContainsKey("speed"))
						{
							this.attackSpeed[id] = float.Parse((string)ht2["speed"]);
						}
					}
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == CharacterControlMap.SKILL)
				{
					int id = int.Parse((string)ht2["id"]);
					if(id < this.skillID.Length)
					{
						this.skillID[id] = int.Parse((string)ht2["id2"]);
						this.skillKey[id] = ht2[XMLHandler.CONTENT] as string;
						if(ht2.ContainsKey("axis"))
						{
							this.skillAxis[id] = true;
							this.skillTrigger[id] = float.Parse((string)ht2["axis"]);
						}
						if(ht2.ContainsKey("air")) this.skillAir[id] = true;
						if(ht2.ContainsKey("speed"))
						{
							this.skillSpeed[id] = float.Parse((string)ht2["speed"]);
						}
					}
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == CharacterControlMap.ITEM)
				{
					int id = int.Parse((string)ht2["id"]);
					if(id < this.itemID.Length)
					{
						this.itemID[id] = int.Parse((string)ht2["id2"]);
						this.itemKey[id] = ht2[XMLHandler.CONTENT] as string;
						if(ht2.ContainsKey("axis"))
						{
							this.itemAxis[id] = true;
							this.itemTrigger[id] = float.Parse((string)ht2["axis"]);
						}
						if(ht2.ContainsKey("air")) this.itemAir[id] = true;
						if(ht2.ContainsKey("speed"))
						{
							this.itemSpeed[id] = float.Parse((string)ht2["speed"]);
						}
					}
				}
			}
		}
	}
	
	public void AddAttack()
	{
		this.attackKey = ArrayHelper.Add("", this.attackKey);
		this.attackAxis = ArrayHelper.Add(false, this.attackAxis);
		this.attackTrigger = ArrayHelper.Add(0, this.attackTrigger);
		this.attackAir = ArrayHelper.Add(false, this.attackAir);
		this.attackSpeed = ArrayHelper.Add(0, this.attackSpeed);
	}
	
	public void RemoveAttack(int index)
	{
		this.attackKey = ArrayHelper.Remove(index, this.attackKey);
		this.attackAxis = ArrayHelper.Remove(index, this.attackAxis);
		this.attackTrigger = ArrayHelper.Remove(index, this.attackTrigger);
		this.attackAir = ArrayHelper.Remove(index, this.attackAir);
		this.attackSpeed = ArrayHelper.Remove(index, this.attackSpeed);
	}
	
	public void AddSkill()
	{
		this.skillID = ArrayHelper.Add(0, this.skillID);
		this.skillKey = ArrayHelper.Add("", this.skillKey);
		this.skillAxis = ArrayHelper.Add(false, this.skillAxis);
		this.skillTrigger = ArrayHelper.Add(0, this.skillTrigger);
		this.skillAir = ArrayHelper.Add(false, this.skillAir);
		this.skillSpeed = ArrayHelper.Add(0, this.skillSpeed);
	}
	
	public void RemoveSkill(int index)
	{
		this.skillID = ArrayHelper.Remove(index, this.skillID);
		this.skillKey = ArrayHelper.Remove(index, this.skillKey);
		this.skillAxis = ArrayHelper.Remove(index, this.skillAxis);
		this.skillTrigger = ArrayHelper.Remove(index, this.skillTrigger);
		this.skillAir = ArrayHelper.Remove(index, this.skillAir);
		this.skillSpeed = ArrayHelper.Remove(index, this.skillSpeed);
	}
	
	public void AddItem()
	{
		this.itemID = ArrayHelper.Add(0, this.itemID);
		this.itemKey = ArrayHelper.Add("", this.itemKey);
		this.itemAxis = ArrayHelper.Add(false, this.itemAxis);
		this.itemTrigger = ArrayHelper.Add(0, this.itemTrigger);
		this.itemAir = ArrayHelper.Add(false, this.itemAir);
		this.itemSpeed = ArrayHelper.Add(0, this.itemSpeed);
	}
	
	public void RemoveItem(int index)
	{
		this.itemID = ArrayHelper.Remove(index, this.itemID);
		this.itemKey = ArrayHelper.Remove(index, this.itemKey);
		this.itemAxis = ArrayHelper.Remove(index, this.itemAxis);
		this.itemTrigger = ArrayHelper.Remove(index, this.itemTrigger);
		this.itemAir = ArrayHelper.Remove(index, this.itemAir);
		this.itemSpeed = ArrayHelper.Remove(index, this.itemSpeed);
	}
	
	/*
	============================================================================
	Control functions
	============================================================================
	*/
	private bool ControlAccepted(string key, bool axis, float trigger)
	{
		bool accept = false;
		if(axis)
		{
			float a = ControlHandler.GetAxis(key);
			if((trigger < 0 && a < trigger) ||
				(trigger > 0 && a > trigger))
			{
				accept = true;
				this.timeout = this.axisTimeout;
			}
		}
		else if(ControlHandler.IsPressed(key, this.inputHandling))
		{
			accept = true;
		}
		return accept;
	}
	
	public void Tick(BattleControl control, Character c)
	{
		if(this.timeout > 0) this.timeout -= Time.deltaTime;
		else if(c.CanChooseAction() && !c.autoAttackStarted)
		{
			bool found = false;
			bool inAir = c.IsInAir();
			float speed = c.GetCurrentSpeed();
			// skill
			for(int i=0; i<this.skillID.Length; i++)
			{
				if(inAir == this.skillAir[i] && (this.skillAir[i] || speed >= this.skillSpeed[i]) && 
					this.ControlAccepted(this.skillKey[i], this.skillAxis[i], this.skillTrigger[i]))
				{
					if(c.HasSkill(this.skillID[i], 0))
					{
						SkillLearn sk = c.GetSkill(this.skillID[i]);
						sk.SetHighestUseLevel(c);
						if(sk.CanUse(c))
						{
							if(DataHolder.Skill(sk.skillID).TargetSelf())
							{
								c.AddSkillAction(sk.skillID, sk.GetLevel(), c.battleID, true);
							}
							else if(DataHolder.Skill(sk.skillID).TargetNone() && 
								!DataHolder.Skill(sk.skillID).targetRaycast.NeedInteraction())
							{
								c.AddSkillAction(sk.skillID, sk.GetLevel(), BattleAction.NONE, true);
							}
							else if(DataHolder.Skill(sk.skillID).TargetAllyGroup())
							{
								c.AddSkillAction(sk.skillID, sk.GetLevel(), BattleAction.ALL_CHARACTERS, true);
							}
							else if(DataHolder.Skill(sk.skillID).TargetEnemyGroup())
							{
								c.AddSkillAction(sk.skillID, sk.GetLevel(), BattleAction.ALL_ENEMIES, true);
							}
							// party target
							else if(DataHolder.Skill(sk.skillID).TargetSingleEnemy() &&
								control.usePartyTarget && control.partyTarget != null)
							{
								c.AddSkillAction(sk.skillID, sk.GetLevel(), BattleAction.PARTY_TARGET, true);
							}
							// call target selection
							else
							{
								c.CallTargetMenu(BattleMenu.SKILL, sk.skillID, sk.GetLevel());
							}
							// only if has and can be used, else go to the next skill
							found = true;
							break;
						}
					}
				}
			}
			// item
			if(!found && this.timeout <= 0)
			{
				for(int i=0; i<this.itemID.Length; i++)
				{
					if(inAir == this.itemAir[i] && (this.itemAir[i] || speed >= this.itemSpeed[i]) && 
						this.ControlAccepted(this.itemKey[i], this.itemAxis[i], this.itemTrigger[i]))
					{
						if(GameHandler.HasItem(this.itemID[i], 1))
						{
							if(DataHolder.Item(this.itemID[i]).useable)
							{
								if(DataHolder.Item(this.itemID[i]).TargetSelf())
								{
									c.AddItemAction(this.itemID[i], c.battleID, true);
								}
								else if(DataHolder.Item(this.itemID[i]).TargetNone() && 
									!DataHolder.Item(this.itemID[i]).targetRaycast.NeedInteraction())
								{
									c.AddItemAction(this.itemID[i], BattleAction.NONE, true);
								}
								else if(DataHolder.Item(this.itemID[i]).TargetAllyGroup())
								{
									c.AddItemAction(this.itemID[i], BattleAction.ALL_CHARACTERS, true);
								}
								else if(DataHolder.Item(this.itemID[i]).TargetEnemyGroup())
								{
									c.AddItemAction(this.itemID[i], BattleAction.ALL_ENEMIES, true);
								}
								// party target
								else if(DataHolder.Item(this.itemID[i]).TargetSingleEnemy() &&
									control.usePartyTarget && control.partyTarget != null)
								{
									c.AddItemAction(this.itemID[i], BattleAction.PARTY_TARGET, true);
								}
								// call target selection
								else
								{
									c.CallTargetMenu(BattleMenu.ITEM, this.itemID[i], 0);
								}
								// only if has and can be used, else go to the next item
								found = true;
								break;
							}
						}
					}
				}
			}
			// attack
			if(!found && this.timeout <= 0)
			{
				for(int i=0; i<this.attackKey.Length; i++)
				{
					if(inAir == this.attackAir[i] && (this.attackAir[i] || speed >= this.attackSpeed[i]) && 
						this.ControlAccepted(this.attackKey[i], this.attackAxis[i], this.attackTrigger[i]))
					{
						if(c.baIndex == i || i == 0)
						{
							if(i == 0 && this.attackKey.Length > 1) c.ResetBaseAttack();
							if(control.attackPartyTarget && control.partyTarget != null && 
								(!control.aptRange || c.InAttackRange(control.partyTarget)))
							{
								c.AddAttackAction(BattleAction.PARTY_TARGET, true);
							}
							else c.AddAttackAction(BattleAction.NONE, true);
						}
						break;
					}
				}
			}
		}
	}
}
