
using UnityEditor;
using UnityEngine;

public class ItemRecipeTab : BaseTab
{
	public ItemRecipeTab(ProjectWindow pw) : base(pw)
	{
		this.Reload();
	}
	
	public void ShowTab()
	{
		EditorGUILayout.BeginVertical();
		
		// buttons
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Add Recipe", GUILayout.Width(pw.mWidth2)))
		{
			DataHolder.ItemRecipes().AddRecipe("New Recipe", "New Description", 
					pw.GetLangCount());
			selection = DataHolder.ItemRecipes().GetDataCount()-1;
			pw.AddItemRecipe(selection);
			GUI.FocusControl ("ID");
		}
		if(this.ShowCopyButton(DataHolder.ItemRecipes()))
		{
			pw.AddItemRecipe(selection);
		}
		if(DataHolder.ItemRecipes().GetDataCount() > 1)
		{
			if(this.ShowRemButton("Remove Recipe", DataHolder.ItemRecipes()))
			{
				pw.RemoveItemRecipe(selection);
			}
		}
		this.CheckSelection(DataHolder.ItemRecipes());
		EditorGUILayout.EndHorizontal();
		
		// status value list
		this.AddItemList(DataHolder.ItemRecipes());
		
		// value settings
		EditorGUILayout.BeginVertical();
		SP2 = EditorGUILayout.BeginScrollView(SP2);
		if(DataHolder.ItemRecipes().GetDataCount() > 0)
		{
			EditorGUILayout.BeginHorizontal();
			this.AddID("Recipe ID");
			this.AddMultiLangIcon("Recipe Name", DataHolder.ItemRecipes());
			
			EditorGUILayout.BeginVertical();
			EditorGUILayout.BeginVertical("box");
			fold6 = EditorGUILayout.Foldout(fold6, "Outcome");
			if(fold6)
			{
				EditorGUILayout.BeginHorizontal();
				DataHolder.ItemRecipe(selection).useFormula = EditorGUILayout.Toggle("Use formula", DataHolder.ItemRecipe(selection).useFormula, GUILayout.Width(pw.mWidth));
				if(DataHolder.ItemRecipe(selection).useFormula)
				{
					DataHolder.ItemRecipe(selection).formulaID = EditorGUILayout.Popup(DataHolder.ItemRecipe(selection).formulaID, 
							DataHolder.Formulas().GetNameList(true), GUILayout.Width(pw.mWidth*0.75f));
				}
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				DataHolder.ItemRecipe(selection).outcome = this.ItemSettings(DataHolder.ItemRecipe(selection).outcome);
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold5 = EditorGUILayout.Foldout(fold5, "Ingredients");
			if(fold5)
			{
				if(GUILayout.Button("Add", GUILayout.Width(pw.mWidth3)))
				{
					DataHolder.ItemRecipe(selection).AddIngredient();
				}
				for(int i=0; i<DataHolder.ItemRecipe(selection).ingredient.Length; i++)
				{
					EditorGUILayout.BeginVertical("box");
					EditorGUILayout.BeginHorizontal();
					GUILayout.Label ("Ingredient "+(i+1).ToString(), EditorStyles.boldLabel);
					if(DataHolder.ItemRecipe(selection).ingredient.Length > 1)
					{
						if(GUILayout.Button("Remove", GUILayout.Width(pw.mWidth3)))
						{
							DataHolder.ItemRecipe(selection).RemoveIngredient(i);
							return;
						}
					}
					GUILayout.FlexibleSpace();
					EditorGUILayout.EndHorizontal();
					DataHolder.ItemRecipe(selection).ingredient[i] = this.ItemSettings(DataHolder.ItemRecipe(selection).ingredient[i]);
					EditorGUILayout.EndVertical();
				}
			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
		}
		this.EndTab();
	}
	
	private ItemShort ItemSettings(ItemShort ingredient)
	{
		ItemDropType tmp = ingredient.type;
		ingredient.type = (ItemDropType)this.EnumToolbar("Type", (int)ingredient.type, typeof(ItemDropType));
		if(tmp != ingredient.type) ingredient.id = 0;
		if(ItemDropType.ITEM.Equals(ingredient.type))
		{
			ingredient.id = EditorGUILayout.Popup("Item", ingredient.id, DataHolder.Items().GetNameList(true), GUILayout.Width(pw.mWidth));
		}
		else if(ItemDropType.WEAPON.Equals(ingredient.type))
		{
			ingredient.id = EditorGUILayout.Popup("Weapon", ingredient.id, DataHolder.Weapons().GetNameList(true), GUILayout.Width(pw.mWidth));
		}
		else if(ItemDropType.ARMOR.Equals(ingredient.type))
		{
			ingredient.id = EditorGUILayout.Popup("Armor", ingredient.id, DataHolder.Armors().GetNameList(true), GUILayout.Width(pw.mWidth));
		}
		ingredient.quantity = EditorGUILayout.IntField("Quantity", ingredient.quantity, GUILayout.Width(pw.mWidth));
		if(ingredient.quantity < 1) ingredient.quantity = 1;
		EditorGUILayout.Separator();
		return ingredient;
	}
}