
using System.Collections;

public class BaseAttackData : BaseData
{
	public BaseAttack[] baseAttack = new BaseAttack[0];
	
	// XML data
	private string filename = "baseAttacks";
	
	//public static string PREFAB_PATH = "Prefabs/Attacks/";
	public static string AUDIO_PATH = "Audio/Attacks/";
	
	private static string BASEATTACKS = "baseattacks";
	private static string BASEATTACK = "baseattack";

	public BaseAttackData()
	{
		LoadData();
	}
	
	public void LoadData()
	{
		ArrayList data = XMLHandler.LoadXML(dir+filename);
		
		int svCount = DataHolder.StatusValueCount;
		
		if(data.Count > 0)
		{
			foreach(Hashtable entry in data)
			{
				if(entry[XMLHandler.NODE_NAME] as string == BaseAttackData.BASEATTACKS)
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList subs = entry[XMLHandler.NODES] as ArrayList;
						this.name = new string[subs.Count/2];
						this.baseAttack = new BaseAttack[subs.Count/2];
						foreach(Hashtable ht in subs)
						{
							if(ht[XMLHandler.NODE_NAME] as string == BaseAttackData.NAME)
							{
								int id = int.Parse((string)ht["id"]);
								if(id < this.name.Length)
								{
									this.name[id] = ht[XMLHandler.CONTENT] as string;
								}
							}
							else if(ht[XMLHandler.NODE_NAME] as string == BaseAttackData.BASEATTACK)
							{
								int id = int.Parse((string)ht["id"]);
								if(id < this.baseAttack.Length)
								{
									this.baseAttack[id] = new BaseAttack(svCount);
									this.baseAttack[id].SetData(ht);
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
		ArrayList data = new ArrayList();
		ArrayList subs = new ArrayList();
		Hashtable sv = new Hashtable();
		
		sv.Add(XMLHandler.NODE_NAME, BaseAttackData.BASEATTACKS);
		
		if(this.name != null)
		{
			for(int i=0; i<this.name.Length; i++)
			{
				subs.Add(HashtableHelper.GetContentHashtable(BaseAttackData.NAME, this.name[i], i));
			}
			for(int i=0; i<this.baseAttack.Length; i++)
			{
				subs.Add(this.baseAttack[i].GetData(HashtableHelper.GetTitleHashtable(BaseAttackData.BASEATTACK, i)));
			}
			if(subs.Count > 0) sv.Add(XMLHandler.NODES, subs);
		}
		
		data.Add(sv);
		XMLHandler.SaveXML(dir, filename, data);
	}
	
	public void AddAttack(string n)
	{
		if(this.name == null)
		{
			this.name = new string[] {n};
		}
		else
		{
			this.name = ArrayHelper.Add(n, this.name);
		}
		this.baseAttack = ArrayHelper.Add(new BaseAttack(DataHolder.StatusValueCount), this.baseAttack);
	}
	
	public override void RemoveData(int index)
	{
		this.name = ArrayHelper.Remove(index, this.name);
		if(this.name.Length == 0) this.name = null;
		this.baseAttack = ArrayHelper.Remove(index, this.baseAttack);
	}
	
	public override void Copy(int index)
	{
		this.name = ArrayHelper.Add(this.name[index], this.name);
		this.baseAttack = ArrayHelper.Add(this.baseAttack[index].GetCopy(), this.baseAttack);
	}
	
	public int[] ImportOldAttack(BaseAttack[] atk, string title)
	{
		int[] ids = new int[atk.Length];
		
		for(int i=0; i<atk.Length; i++)
		{
			ids[i] = this.GetIDForAttack(atk[i]);
			// add attack if no match found
			if(ids[i] == -1)
			{
				this.AddAttack(title+" Attack");
				ids[i] = this.baseAttack.Length-1;
				this.baseAttack[ids[i]].SetData(atk[i].GetData(new Hashtable()));
			}
		}
		
		return ids;
	}
	
	public int GetIDForAttack(BaseAttack atk)
	{
		int id = -1;
		for(int i=0; i<this.baseAttack.Length; i++)
		{
			if(this.baseAttack[i].CompareTo(atk))
			{
				id = i;
				break;
			}
		}
		return id;
	}
	
	public void AddStatusValue(int index)
	{
		for(int i=0; i<baseAttack.Length; i++)
		{
			baseAttack[i].consume = ArrayHelper.Add(
					new ValueChange(), baseAttack[i].consume);
			baseAttack[i].criticalConsume = ArrayHelper.Add(
					new ValueChange(), baseAttack[i].criticalConsume);
		}
	}
	
	public void RemoveStatusValue(int index)
	{
		for(int i=0; i<baseAttack.Length; i++)
		{
			baseAttack[i].consume = ArrayHelper.Remove(
					index, baseAttack[i].consume);
			baseAttack[i].criticalConsume = ArrayHelper.Remove(
					index, baseAttack[i].criticalConsume);
		}
	}
	
	public void SetStatusValueType(int index, StatusValueType val)
	{
		if(!StatusValueType.CONSUMABLE.Equals(val))
		{
			for(int i=0; i<baseAttack.Length; i++)
			{
				baseAttack[i].consume[index] = new ValueChange();
				baseAttack[i].criticalConsume[index] = new ValueChange();
			}
		}
	}
	
	public void RemoveFormula(int index)
	{
		for(int i=0; i<baseAttack.Length; i++)
		{
			baseAttack[i].hitFormula = this.CheckForIndex(index, baseAttack[i].hitFormula);
			for(int j=0; j<baseAttack[i].consume.Length; j++)
			{
				baseAttack[i].consume[j].formula = this.CheckForIndex(index, baseAttack[i].consume[j].formula);
			}
		}
	}
	
	public void RemoveItem(int index)
	{
		for(int i=0; i<baseAttack.Length; i++)
		{
			if(ItemDropType.ITEM.Equals(baseAttack[i].stealChance.itemType))
			{
				baseAttack[i].stealChance.itemID = this.CheckForIndex(index, baseAttack[i].stealChance.itemID);
			}
			for(int j=0; j<baseAttack[i].consumeItemID.Length; j++)
			{
				baseAttack[i].consumeItemID[j] = this.CheckForIndex(index, baseAttack[i].consumeItemID[j]);
			}
		}
	}
	
	public void RemoveWeapon(int index)
	{
		for(int i=0; i<baseAttack.Length; i++)
		{
			if(ItemDropType.WEAPON.Equals(baseAttack[i].stealChance.itemType))
			{
				baseAttack[i].stealChance.itemID = this.CheckForIndex(index, baseAttack[i].stealChance.itemID);
			}
		}
	}
	
	public void RemoveArmor(int index)
	{
		for(int i=0; i<baseAttack.Length; i++)
		{
			if(ItemDropType.ARMOR.Equals(baseAttack[i].stealChance.itemType))
			{
				baseAttack[i].stealChance.itemID = this.CheckForIndex(index, baseAttack[i].stealChance.itemID);
			}
		}
	}
}