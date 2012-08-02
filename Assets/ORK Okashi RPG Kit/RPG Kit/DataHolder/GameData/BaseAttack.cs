
using UnityEngine;
using System.Collections;

public class BaseAttack
{	
	public float availableTime = 0;
	
	public bool absorb = false;
	public int absorbValue = 0;
	
	public bool hitChance = false;
	public int hitFormula = 0;
	
	public UseRange useRange = new UseRange();
	
	public ValueChange[] consume;
	
	public bool hasCritical = false;
	public ValueChange[] criticalConsume;
	
	public bool overrideAnimation = false;
	public int animationID = 0;
	
	public string audioName = "";
	public string criticalAudioName = "";
	
	// item consume
	public int[] consumeItemID = new int[0];
	public int[] consumeItemQuantity = new int[0];
	
	// ingame
	private AudioClip attackClip = null;
	private AudioClip criticalClip = null;
	
	public StealChance stealChance = new StealChance();
	
	// XML
	private static string CONSUME = "consume";
	private static string CRITICALCONSUME = "criticalconsume";
	private static string AUDIOCLIP = "audioclip";
	private static string CRITICALAUDIOCLIP = "criticalaudioclip";
	private static string ITEMCONSUME = "itemconsume";
	private static string STEALCHANCE = "stealchance";
	
	public BaseAttack(int count)
	{
		this.consume = new ValueChange[count];
		this.criticalConsume = new ValueChange[count];
		for(int i=0; i<count; i++)
		{
			this.consume[i] = new ValueChange();
			this.criticalConsume[i] = new ValueChange();
		}
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		if(this.availableTime > 0) ht.Add("availabletime", this.availableTime.ToString());
		if(this.absorb) ht.Add("absorbvalue", this.absorbValue.ToString());
		if(this.hitChance) ht.Add("hitchance", this.hitFormula.ToString());
		if(this.overrideAnimation) ht.Add("animation", this.animationID.ToString());
		if(this.hasCritical) ht.Add("critical", "true");
		ht = this.useRange.GetData(ht);
		
		ArrayList s = new ArrayList();
		
		for(int i=0; i<this.consume.Length; i++)
		{
			if(this.consume[i].active)
			{
				s.Add(this.consume[i].GetData(HashtableHelper.GetTitleHashtable(BaseAttack.CONSUME, i)));
			}
			if(this.criticalConsume[i].active)
			{
				s.Add(this.criticalConsume[i].GetData(HashtableHelper.GetTitleHashtable(BaseAttack.CRITICALCONSUME, i)));
			}
		}
		
		if(this.audioName != "") s.Add(HashtableHelper.GetContentHashtable(BaseAttack.AUDIOCLIP, this.audioName));
		if(this.criticalAudioName != "") s.Add(HashtableHelper.GetContentHashtable(BaseAttack.CRITICALAUDIOCLIP, this.criticalAudioName));
		
		ht.Add("itemconsumes", this.consumeItemID.Length.ToString());
		for(int i=0; i<this.consumeItemID.Length; i++)
		{
			Hashtable ht2 = HashtableHelper.GetTitleHashtable(BaseAttack.ITEMCONSUME, i);
			ht2.Add("id2", this.consumeItemID[i].ToString());
			ht2.Add("quantity", this.consumeItemQuantity[i].ToString());
			s.Add(ht2);
		}
		
		s.Add(this.stealChance.GetData(HashtableHelper.GetTitleHashtable(BaseAttack.STEALCHANCE)));
		
		if(s.Count > 0) ht.Add(XMLHandler.NODES, s);
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		if(ht.ContainsKey("availabletime"))
		{
			this.availableTime = float.Parse((string)ht["availabletime"]);
		}
		if(ht.ContainsKey("absorbvalue"))
		{
			this.absorb = true;
			this.absorbValue = int.Parse((string)ht["absorbvalue"]);
		}
		if(ht.ContainsKey("hitchance"))
		{
			this.hitChance = true;
			this.hitFormula = int.Parse((string)ht["hitchance"]);
		}
		if(ht.ContainsKey("animation"))
		{
			this.overrideAnimation = true;
			this.animationID = int.Parse((string)ht["animation"]);
		}
		if(ht.ContainsKey("critical")) this.hasCritical = true;
		
		this.useRange.SetData(ht);
		
		if(ht.ContainsKey("itemconsumes"))
		{
			int count = int.Parse((string)ht["itemconsumes"]);
			this.consumeItemID = new int[count];
			this.consumeItemQuantity = new int[count];
		}
		
		if(ht.ContainsKey(XMLHandler.NODES))
		{
			ArrayList s = ht[XMLHandler.NODES] as ArrayList;
			foreach(Hashtable ht2 in s)
			{
				if(ht2[XMLHandler.NODE_NAME] as string == BaseAttack.CONSUME)
				{
					int id = int.Parse((string)ht2["id"]);
					if(id < this.consume.Length) this.consume[id].SetData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == BaseAttack.CRITICALCONSUME)
				{
					int id = int.Parse((string)ht2["id"]);
					if(id < this.criticalConsume.Length) this.criticalConsume[id].SetData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == BaseAttack.AUDIOCLIP)
				{
					this.audioName = ht2[XMLHandler.CONTENT] as string;
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == BaseAttack.CRITICALAUDIOCLIP)
				{
					this.criticalAudioName = ht2[XMLHandler.CONTENT] as string;
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == BaseAttack.ITEMCONSUME)
				{
					int id = int.Parse((string)ht2["id"]);
					if(id < this.consumeItemID.Length)
					{
						this.consumeItemID[id] = int.Parse((string)ht2["id2"]);
						this.consumeItemQuantity[id] = int.Parse((string)ht2["quantity"]);
					}
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == BaseAttack.STEALCHANCE)
				{
					this.stealChance.SetData(ht2);
				}
			}
		}
	}
	
	public BaseAttack GetCopy()
	{
		BaseAttack ba = new BaseAttack(this.consume.Length);
		ba.SetData(this.GetData(new Hashtable()));
		return ba;
	}
	
	public bool CompareTo(BaseAttack atk)
	{
		bool same = false;
		if(this.availableTime == atk.availableTime && 
			this.absorb == atk.absorb && 
			this.absorbValue == atk.absorbValue && 
			this.hitChance == atk.hitChance && 
			this.hitFormula == atk.hitFormula && 
			this.hitFormula == atk.hitFormula && 
			this.useRange.CompareTo(atk.useRange) &&
			this.hasCritical == atk.hasCritical && 
			this.overrideAnimation == atk.overrideAnimation && 
			this.animationID == atk.animationID && 
			this.audioName == atk.audioName && 
			this.criticalAudioName == atk.criticalAudioName)
		{
			same = true;
			for(int i=0; i<this.consume.Length; i++)
			{
				if(!this.consume[i].CompareTo(atk.consume[i]))
				{
					return false;
				}
			}
			for(int i=0; i<this.criticalConsume.Length; i++)
			{
				if(!this.criticalConsume[i].CompareTo(atk.criticalConsume[i]))
				{
					return false;
				}
			}
		}
		return same;
	}
	
	/*
	============================================================================
	Item consume functions
	============================================================================
	*/
	public void AddItemConsume()
	{
		this.consumeItemID = ArrayHelper.Add(0, this.consumeItemID);
		this.consumeItemQuantity = ArrayHelper.Add(1, this.consumeItemQuantity);
	}
	
	public void RemoveItemConsume(int index)
	{
		this.consumeItemID = ArrayHelper.Remove(index, this.consumeItemID);
		this.consumeItemQuantity = ArrayHelper.Remove(index, this.consumeItemQuantity);
	}
	
	public bool ConsumeItems()
	{
		bool ok = true;
		for(int i=0; i<this.consumeItemID.Length; i++)
		{
			if(GameHandler.HasItem(this.consumeItemID[i], this.consumeItemQuantity[i]))
			{
				GameHandler.RemoveItem(this.consumeItemID[i], this.consumeItemQuantity[i]);
			}
			else
			{
				ok = false;
				break;
			}
		}
		return ok;
	}
	
	public bool CheckItems()
	{
		bool ok = true;
		for(int i=0; i<this.consumeItemID.Length; i++)
		{
			if(!GameHandler.HasItem(this.consumeItemID[i], this.consumeItemQuantity[i]))
			{
				ok = false;
				break;
			}
		}
		return ok;
	}
	
	/*
	============================================================================
	Audio functions
	============================================================================
	*/
	public AudioClip GetAttackAudio()
	{
		if(this.attackClip == null && this.audioName != "")
		{
			this.attackClip = (AudioClip)Resources.Load(BaseAttackData.AUDIO_PATH+
					this.audioName, typeof(AudioClip));
		}
		return this.attackClip;
	}
	
	public AudioClip GetCriticalAudio()
	{
		if(this.criticalClip == null && this.criticalAudioName != "")
		{
			this.criticalClip = (AudioClip)Resources.Load(BaseAttackData.AUDIO_PATH+
					this.criticalAudioName, typeof(AudioClip));
		}
		return this.criticalClip;
	}
	
	/*
	============================================================================
	Use functions
	============================================================================
	*/
	public bool Use(Combatant user, int element, Combatant target, float damageFactor, float damageMultiplier)
	{
		bool hit = false;
		
		if(target.IsBlockBaseAttacks())
		{
			DataHolder.BattleSystemData().blockTextSettings.ShowText("", target);
		}
		else if((user is Enemy || this.ConsumeItems()) && 
			(!this.hitChance || DataHolder.GameSettings().GetRandom() <= 
			(DataHolder.Formulas().formula[this.hitFormula].Calculate(user, target) + user.GetHitBonus())))
		{
			hit = true;
			
			int eID = user.GetEffectAttackElement();
			if(eID < 0)
			{
				eID = element;
			}
			
			ValueChange[] changes = this.consume;
			AudioClip clip = null;
			
			if(this.hasCritical && DataHolder.GameSettings().GetRandom() <= 
				DataHolder.Formula(user.baseCritical).Calculate(user, target) + user.GetCriticalBonus())
			{
				changes = this.criticalConsume;
				clip = this.GetCriticalAudio();
			}
			
			if(clip == null)
			{
				clip = this.GetAttackAudio();
			}
			
			if(clip != null && target.prefabInstance != null)
			{
				AudioSource s = target.GetAudioSource();
				if(s != null)
				{
					s.PlayOneShot(clip);
				}
			}
			
			for(int i=0; i<changes.Length; i++)
			{
				if(changes[i].active)
				{
					int change = changes[i].ChangeValue(i, eID, user, target, true, damageFactor, damageMultiplier);
					if(this.absorb)
					{
						change *= this.absorbValue;
						change /= 100;
						user.status[i].AddValue(change, true, false, true);
					}
				}
			}
			
			this.stealChance.Steal(user, target);
		}
		else
		{
			DataHolder.BattleSystemData().missTextSettings.ShowText("", target);
		}
		
		return hit;
	}
}
