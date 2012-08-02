
using UnityEngine;

public class Enemy : Combatant
{
	public int level = 1;
	public int classLevel = 1;
	public int money = 0;
	public int[] value;
	
	public ItemDrop[] itemDrop = new ItemDrop[0];
	public GameVariable variables = new GameVariable();
	
	// status effects
	public SkillEffect[] skillEffect;
	
	// element defence
	public int[] elementValue;
	// race attack factor
	public int[] raceValue;
	// size attack factor
	public int[] sizeValue;
	
	// steal item
	public bool stealItem = false;
	public float stealItemFactor = 1.0f;
	public ItemDropType stealItemType = ItemDropType.ITEM;
	public int stealItemID = 0;
	public bool stealItemOnce = false;
	// steal money
	public bool stealMoney = false;
	public float stealMoneyFactor = 1.0f;
	public int stealMoneyAmount = 0;
	public bool stealMoneyOnce = false;
	
	// ingame
	private BaseSpawner baseSpawner = null;
	public bool itemStolen = false;
	public bool moneyStolen = false;
	
	public string nameCount = "";
	
	public Enemy()
	{
		
	}
	
	public override string GetPrefabPath() { return EnemyData.PREFAB_PATH; }
	
	public void AddItemDrop(ItemDropType t, int i, int q, int c)
	{
		ItemDrop drop = new ItemDrop();
		drop.type = t;
		drop.itemID = i;
		drop.quantity = q;
		drop.chance = c;
		this.itemDrop = ArrayHelper.Add(drop, this.itemDrop);
	}
	
	public override void Init()
	{
		base.Init();
		this.currentLevel = level;
		this.currentClassLevel = classLevel;
		
		Difficulty difficulty = DataHolder.Difficulty(GameHandler.GetDifficulty());
		for(int i=0; i<this.value.Length; i++)
		{
			if(this.status[i].IsConsumable())
			{
				this.status[i].InitValue((int)(this.value[this.status[i].maxStatus]*difficulty.statusMultiplier[i]));
			}
			else
			{
				this.status[i].InitValue((int)(this.value[i]*difficulty.statusMultiplier[i]));
			}
		}
		
		this.element = new int[this.elementValue.Length];
		for(int i=0; i<this.elementValue.Length; i++)
		{
			this.element[i] = (int)(this.elementValue[i]*difficulty.elementMultiplier[i]);
		}
		this.raceDmgFactor = new int[this.raceValue.Length];
		for(int i=0; i<this.raceValue.Length; i++)
		{
			this.raceDmgFactor[i] = (int)(this.raceValue[i]*difficulty.raceMultiplier[i]);
		}
		this.sizeDmgFactor = new int[this.sizeValue.Length];
		for(int i=0; i<this.sizeValue.Length; i++)
		{
			this.sizeDmgFactor[i] = (int)(this.sizeValue[i]*difficulty.sizeMultiplier[i]);
		}
		
		this.ResetStatus();
		for(int i=0; i<this.status.Length; i++)
		{
			if(this.status[i].IsConsumable())
			{
				this.status[i].SetValue(this.status[this.status[i].maxStatus].GetValue(), false, false, false);
			}
		}
	}
	
	/*
	============================================================================
	Status value functions
	============================================================================
	*/
	public override void ResetStatus()
	{
		base.ResetStatus();
		for(int i=0; i<this.status.Length; i++)
		{
			this.status[i].CheckBounds(true, false);
		}
	}
	
	/*
	============================================================================
	Status effect functions
	============================================================================
	*/
	public override void SetStartEffects()
	{
		for(int i=0; i<this.skillEffect.Length; i++)
		{
			if(SkillEffect.ADD.Equals(this.skillEffect[i]))
			{
				this.AddEffect(i, this);
			}
		}
	}
	
	public override bool CanApplyEffect(int effectID)
	{
		return !SkillEffect.REMOVE.Equals(this.skillEffect[effectID]);
	}
	
	public override bool CanRemoveEffect(int effectID)
	{
		return !SkillEffect.ADD.Equals(this.skillEffect[effectID]);
	}
	
	public override void ChooseAction()
	{
		base.ChooseAction();
		if(this.IsStopMove() || (this.IsAutoAttack() && this.IsBlockAttack()) ||
				(this.IsAttackFriends() && this.IsBlockAttack()))
		{
			this.AddAction(new BattleAction(AttackSelection.NONE, this, this.battleID, -1, 0));
		}
		else if(this.IsAutoAttack())
		{
			this.AddAction(new BattleAction(AttackSelection.ATTACK, this, 
					this.GetRandomTarget(GameHandler.Party().GetBattleParty()), -1, 0));
		}
		else if(this.IsAttackFriends())
		{
			this.AddAction(new BattleAction(AttackSelection.ATTACK, this, 
					this.GetRandomTarget(DataHolder.BattleSystem().enemies), -1, 0));
		}
		else
		{
			this.AddAction(this.GetAIBehaviourAction(
					DataHolder.BattleSystem().enemies, GameHandler.Party().GetBattleParty()));
		}
	}
	
	public override bool HasSkill(int id, int lvl)
	{
		bool found = false;
		for(int i=0; i<this.aiBehaviour.Length; i++)
		{
			for(int j=0; j<this.aiBehaviour[i].attackSelection.Length; j++)
			{
				if(AttackSelection.SKILL.Equals(this.aiBehaviour[i].attackSelection[j]) && 
					this.aiBehaviour[i].useID[j] == id && this.aiBehaviour[i].useLevel[j] >= lvl)
				{
					found = true;
					break;
				}
			}
		}
		return found;
	}
	
	public override int GetElementDefence(int index)
	{
		int def = base.GetElementDefence(index);
		for(int i=0; i<this.effect.Length; i++)
		{
			def += this.effect[i].bonus.GetElementDefence(index);
			if(SimpleOperator.ADD.Equals(this.effect[i].elementOperator[index]))
			{
				def += this.effect[i].elementValue[index];
			}
			else if(SimpleOperator.SUB.Equals(this.effect[i].elementOperator[index]))
			{
				def -= this.effect[i].elementValue[index];
			}
			else if(SimpleOperator.SET.Equals(this.effect[i].elementOperator[index]))
			{
				def = this.effect[i].elementValue[index];
				break;
			}
		}
		return def;
	}
	
	public override bool UseBaseAttack(Combatant target, BattleAction ba, 
			bool counter, float damageFactor, float damageMultiplier)
	{
		bool hit = DataHolder.BaseAttack(this.baseAttack[this.baIndex]).Use(this, this.baseElement, target, damageFactor, damageMultiplier);
		
		if(!this.isDead && counter && this.battleID != target.battleID)
		{
			if(DataHolder.BattleSystem().IsRealTime()) target.UseCounter(this);
			else if(ba != null) ba.doCounter = new bool[] {target.UseCounter(this)};
		}
		
		return hit;
	}
	
	public override void Died()
	{
		base.Died();
		DataHolder.BattleSystem().EnemyDefeated(this.battleID);
		if(this.baseSpawner != null) this.baseSpawner.CheckRespawn(this);
	}
	
	public override string GetName()
	{
		string n = "";
		if(this.realID >= 0)
		{
			n = DataHolder.Enemies().GetName(this.realID);
			if(this.nameCount != "") n += " "+this.nameCount;
		}
		return n;
	}
	
	public Texture2D GetIcon()
	{
		Texture2D i = null;
		if(this.realID >= 0) i = DataHolder.Enemies().GetIcon(this.realID);
		return i;
	}
	
	public GUIContent GetContent()
	{
		GUIContent g = null;
		if(this.realID >= 0) g = DataHolder.Enemies().GetContent(this.realID);
			if(this.nameCount != "") g.text += " "+this.nameCount;
		return g;
	}
	
	public override void Escape()
	{
		base.Escape();
		DataHolder.BattleSystem().RemoveEnemy(this);
	}
	
	/*
	============================================================================
	Respawn functions
	============================================================================
	*/
	public void SetBaseSpawner(BaseSpawner spawner)
	{
		this.baseSpawner = spawner;
	}
}

public class ItemDrop
{
	public ItemDropType type = ItemDropType.ITEM;
	public int itemID = 0;
	public int quantity = 1;
	public float chance = 100;
	
	public ItemDrop()
	{
		
	}
	
	public bool IsItem()
	{
		return ItemDropType.ITEM.Equals(this.type);
	}
	
	public bool IsWeapon()
	{
		return ItemDropType.WEAPON.Equals(this.type);
	}
	
	public bool IsArmor()
	{
		return ItemDropType.ARMOR.Equals(this.type);
	}
	
	public void Drop(Vector3 position)
	{
		GameHandler.Drop(position, this.type, this.itemID, this.quantity);
	}
}