
using System.Collections;

public class ItemRecipeData : BaseLangData
{
	public ItemRecipe[] recipe = new ItemRecipe[0];
	
	// XML data
	private string filename = "itemRecipes";
	
	private static string ITEMRECIPES = "itemrecipes";
	private static string ITEMRECIPE = "itemrecipe";
	private static string INGREDIENT = "ingredient";

	public ItemRecipeData()
	{
		LoadData();
	}
	
	public override string GetIconPath() { return "Icons/ItemRecipe/"; }
	
	public void LoadData()
	{
		ArrayList data = XMLHandler.LoadXML(dir+filename);
		
		if(data.Count > 0)
		{
			foreach(Hashtable entry in data)
			{
				if(entry[XMLHandler.NODE_NAME] as string == ItemRecipeData.ITEMRECIPES)
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList subs = entry[XMLHandler.NODES] as ArrayList;
						icon = new string[subs.Count];
						recipe = new ItemRecipe[subs.Count];
						
						foreach(Hashtable val in subs)
						{
							if(val[XMLHandler.NODE_NAME] as string == ItemRecipeData.ITEMRECIPE)
							{
								int i = int.Parse((string)val["id"]);
								icon[i] = "";
								recipe[i] = new ItemRecipe();
								recipe[i].outcome.SetData(val);
								recipe[i].ingredient = new ItemShort[int.Parse((string)val["ingredients"])];
								
								if(val.ContainsKey("formula"))
								{
									recipe[i].useFormula = true;
									recipe[i].formulaID = int.Parse((string)val["formula"]);
								}
								
								ArrayList s = val[XMLHandler.NODES] as ArrayList;
								foreach(Hashtable ht in s)
								{
									this.LoadLanguages(ht, i, subs.Count);
									if(ht[XMLHandler.NODE_NAME] as string == ItemRecipeData.INGREDIENT)
									{
										recipe[i].ingredient[int.Parse((string)ht["id"])] = new ItemShort(ht);
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
		if(name.Length > 0)
		{
			ArrayList data = new ArrayList();
			ArrayList subs = new ArrayList();
			
			Hashtable sv = new Hashtable();
			sv.Add(XMLHandler.NODE_NAME, ItemRecipeData.ITEMRECIPES);
			
			for(int i=0; i<name[0].Count(); i++)
			{
				Hashtable val = new Hashtable();
				ArrayList s = new ArrayList();
				
				val.Add(XMLHandler.NODE_NAME, ItemRecipeData.ITEMRECIPE);
				val.Add("id", i.ToString());
				val = recipe[i].outcome.GetData(val);
				
				if(recipe[i].useFormula)
				{
					val.Add("formula", recipe[i].formulaID.ToString());
				}
				
				val.Add("ingredients", recipe[i].ingredient.Length.ToString());
				for(int j=0; j<recipe[i].ingredient.Length; j++)
				{
					Hashtable ing = new Hashtable();
					ing.Add(XMLHandler.NODE_NAME, ItemRecipeData.INGREDIENT);
					ing.Add("id", j.ToString());
					ing = recipe[i].ingredient[j].GetData(ing);
					s.Add(ing);
				}
				
				s = this.SaveLanguages(s, i);
				val.Add(XMLHandler.NODES, s);
				subs.Add(val);
			}
			sv.Add(XMLHandler.NODES, subs);
			data.Add(sv);
			
			XMLHandler.SaveXML(dir, filename, data);
		}
	}
	
	public void AddRecipe(string n, string d, int count)
	{
		base.AddBaseData(n, d, count);
		this.recipe = ArrayHelper.Add(new ItemRecipe(), this.recipe);
	}
	
	public override void RemoveData(int index)
	{
		base.RemoveData(index);
		this.recipe = ArrayHelper.Remove(index, this.recipe);
	}
	
	public ItemRecipe GetCopy(int index)
	{
		ItemRecipe ir = new ItemRecipe();
		ir.outcome = new ItemShort(this.recipe[index].outcome.GetData(new Hashtable()));
		ir.useFormula = this.recipe[index].useFormula;
		ir.formulaID = this.recipe[index].formulaID;
		ir.ingredient = new ItemShort[this.recipe[index].ingredient.Length];
		for(int i=0; i<this.recipe[index].ingredient.Length; i++)
		{
			ir.ingredient[i] = new ItemShort(this.recipe[index].ingredient[i].GetData(new Hashtable()));
		}
		return ir;
	}
	
	public override void Copy(int index)
	{
		base.Copy(index);
		this.recipe = ArrayHelper.Add(this.GetCopy(index), this.recipe);
	}
	
	public void CreateItem(Character c, ItemShort[] items)
	{
		bool created = false;
		for(int i=0; i<this.recipe.Length; i++)
		{
			if(this.recipe[i].CanCreate(items))
			{
				this.recipe[i].CreateItem(c, items);
				created = true;
				break;
			}
		}
		if(!created)
		{
			for(int i=0; i<items.Length; i++)
			{
				items[i].RemoveFromInventory();
			}
		}
	}
	
	public void RemoveItem(int index)
	{
		for(int i=0; i<this.recipe.Length; i++)
		{
			if(ItemDropType.ITEM.Equals(this.recipe[i].outcome.type))
			{
				this.recipe[i].outcome.id = this.CheckForIndex(index, this.recipe[i].outcome.id);
			}
			for(int j=0; j<this.recipe[i].ingredient.Length; j++)
			{
				if(ItemDropType.ITEM.Equals(this.recipe[i].outcome.type))
				{
					this.recipe[i].ingredient[j].id = this.CheckForIndex(index, this.recipe[i].ingredient[j].id);
				}
			}
		}
	}
	
	public void RemoveWeapon(int index)
	{
		for(int i=0; i<this.recipe.Length; i++)
		{
			if(ItemDropType.WEAPON.Equals(this.recipe[i].outcome.type))
			{
				this.recipe[i].outcome.id = this.CheckForIndex(index, this.recipe[i].outcome.id);
			}
			for(int j=0; j<this.recipe[i].ingredient.Length; j++)
			{
				if(ItemDropType.WEAPON.Equals(this.recipe[i].outcome.type))
				{
					this.recipe[i].ingredient[j].id = this.CheckForIndex(index, this.recipe[i].ingredient[j].id);
				}
			}
		}
	}
	
	public void RemoveArmor(int index)
	{
		for(int i=0; i<this.recipe.Length; i++)
		{
			if(ItemDropType.ARMOR.Equals(this.recipe[i].outcome.type))
			{
				this.recipe[i].outcome.id = this.CheckForIndex(index, this.recipe[i].outcome.id);
			}
			for(int j=0; j<this.recipe[i].ingredient.Length; j++)
			{
				if(ItemDropType.ARMOR.Equals(this.recipe[i].outcome.type))
				{
					this.recipe[i].ingredient[j].id = this.CheckForIndex(index, this.recipe[i].ingredient[j].id);
				}
			}
		}
	}
}