
using UnityEngine;
using System.Collections;

public class ItemRecipe
{
	public ItemShort[] ingredient = new ItemShort[1];
	public ItemShort outcome = new ItemShort();
	
	public bool useFormula = false;
	public int formulaID = 0;
	
	public ItemRecipe()
	{
		this.ingredient[0] = new ItemShort();
	}
	
	public void AddIngredient()
	{
		this.ingredient = ArrayHelper.Add(new ItemShort(), this.ingredient);
	}
	
	public void RemoveIngredient(int index)
	{
		this.ingredient = ArrayHelper.Remove(index, this.ingredient);
	}
	
	public ItemShort[] GetSummedUpIngredients()
	{
		return this.GetSummedUpIngredients(this.ingredient);
	}
	
	public ItemShort[] GetSummedUpIngredients(ItemShort[] items)
	{
		ItemShort[] summed = new ItemShort[0];
		for(int i=0; i<items.Length; i++)
		{
			bool found = false;
			for(int j=0; j<summed.Length; j++)
			{
				if(summed[j].type == items[i].type &&
					summed[j].id == items[i].id)
				{
					found = true;
					summed[j].quantity += items[i].quantity;
					break;
				}
			}
			if(!found)
			{
				summed = ArrayHelper.Add(new ItemShort(items[i].GetData(new Hashtable())), summed);
			}
		}
		return summed;
	}
	
	public bool CanCreate()
	{
		bool can = true;
		ItemShort[] summed = this.GetSummedUpIngredients();
		for(int i=0; i<summed.Length; i++)
		{
			if(!summed[i].InInventory())
			{
				can = false;
				break;
			}
		}
		return can;
	}
	
	public bool CanCreate(ItemShort[] items)
	{
		bool can = true;
		if(this.CanCreate())
		{
			items = this.GetSummedUpIngredients(items);
			ItemShort[] needed = this.GetSummedUpIngredients();
			bool[] ok = new bool[needed.Length];
			for(int i=0; i<needed.Length; i++)
			{
				for(int j=0; j<items.Length; j++)
				{
					if(items[j].Equals(needed[i]))
					{
						ok[i] = true;
						break;
					}
				}
				if(!ok[i])
				{
					can = false;
					break;
				}
			}
		}
		else can = false;
		return can;
	}
	
	public void CreateItem(Character c)
	{
		if(this.CanCreate())
		{
			this.Create(c);
		}
	}
	
	public void CreateItem(Character c, ItemShort[] items)
	{
		if(this.CanCreate(items))
		{
			this.Create(c);
		}
	}
	
	private void Create(Character c)
	{
		ItemShort[] needed = this.GetSummedUpIngredients();
		for(int i=0; i<needed.Length; i++)
		{
			needed[i].RemoveFromInventory();
		}
		if(!this.useFormula ||
			(this.useFormula && c != null && 
			DataHolder.GameSettings().GetRandom() <= DataHolder.Formula(this.formulaID).Calculate(c, c)))
		{
			DataHolder.Statistic.ItemCreated(this.outcome.id, this.outcome.type);
			this.outcome.AddToInventory();
		}
	}
}