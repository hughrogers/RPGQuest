
using System.Collections;
using UnityEditor;
using UnityEngine;


public class ProjectWindow : EditorWindow
{
	public int mWidth = 300;
	public int mWidth2 = 200;
	public int mWidth3 = 100;
	
	// section handling
	private int currentSection = 0;
	private string[] sections = new string[] {"Languages", "Text Colors", "Status Values", "Status Effects", "Elements", "Races", "Sizes", "Formulas", 
													"Base Attacks", "Skill Types", "Skills", "Item Types", "Items", "Equipment Parts", "Weapons", "Armors", 
													"Item Recipes", "Battle AI", "Classes", "Characters", "Enemies", "Game Settings", "Load Save HUD", "Difficulties", 
													"Main Menu", "Teleports", "Global Events"};
	
	private int LANGUAGES = 0;
	private int TEXT_COLORS = 1;
	private int STATUS_VALUES = 2;
	private int STATUS_EFFECTS = 3;
	private int ELEMENTS = 4;
	private int RACES = 5;
	private int SIZES = 6;
	private int FORMULAS = 7;
	private int BASE_ATTACKS = 8;
	private int SKILL_TYPES = 9;
	private int SKILLS = 10;
	private int ITEM_TYPES = 11;
	private int ITEMS = 12;
	private int EQUIPMENT_PARTS = 13;
	private int WEAPONS = 14;
	private int ARMORS = 15;
	private int ITEM_RECIPES = 16;
	private int BATTLE_AI = 17;
	private int CLASS = 18;
	private int CHARACTERS = 19;
	private int ENEMIES = 20;
	private int GAME_SETTINGS = 21;
	private int LOAD_SAVE_HUD = 22;
	private int DIFFICULTIES = 23;
	private int MAIN_MENU = 24;
	private int TELEPORTS = 25;
	private int GLOBAL_EVENTS = 26;
	
	// tabs
	private LanguageTab langTab = null;
	private TextColorTab colorTab = null;
	private StatusValueTab statusTab = null;
	private StatusEffectTab effectTab = null;
	private ElementTab elementTab = null;
	private EquipmentPartTab equippartTab = null;
	private FormulaTab formulaTab = null;
	private CharacterTab characterTab = null;
	private ClassTab classTab = null;
	private SkillTypeTab skilltypeTab = null;
	private SkillTab skillTab = null;
	private ItemTypeTab itemtypeTab = null;
	private ItemTab itemTab = null;
	private WeaponTab weaponTab = null;
	private ArmorTab armorTab = null;
	private EnemyTab enemyTab = null;
	private BattleAITab aiTab = null;
	private GameSettingsTab gsTab = null;
	private LoadSaveHUDTab lsHudTab = null;
	private MainMenuTab mmTab = null;
	private ItemRecipeTab recipeTab = null;
	private GlobalEventTab eventTab = null;
	private TeleportTab teleportTab = null;
	private BaseAttackTab attackTab = null;
	private RaceTab raceTab = null;
	private SizeTab sizeTab = null;
	private DifficultyTab difficultyTab = null;
	
	[MenuItem("RPG Kit/Project Editor")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		ProjectWindow window = (ProjectWindow)EditorWindow.GetWindow(typeof(ProjectWindow), false, "Project Editor");
		window.Reload();
		window.Show();
	}
	
	public void Reload()
	{
		DataHolder.Instance().Init();
		if(langTab == null) langTab = new LanguageTab(this);
		else langTab.Reload();
		
		if(colorTab == null) colorTab = new TextColorTab(this);
		else colorTab.Reload();
		
		if(statusTab == null) statusTab = new StatusValueTab(this);
		else statusTab.Reload();
		
		if(effectTab == null) effectTab = new StatusEffectTab(this);
		else effectTab.Reload();
		
		if(elementTab == null) elementTab = new ElementTab(this);
		else elementTab.Reload();
		
		if(equippartTab == null) equippartTab = new EquipmentPartTab(this);
		else equippartTab.Reload();
		
		if(formulaTab == null) formulaTab = new FormulaTab(this);
		else formulaTab.Reload();
		
		if(characterTab == null) characterTab = new CharacterTab(this);
		else characterTab.Reload();
		
		if(classTab == null) classTab = new ClassTab(this);
		else classTab.Reload();
		
		if(skilltypeTab == null) skilltypeTab = new SkillTypeTab(this);
		else skilltypeTab.Reload();
		
		if(skillTab == null) skillTab = new SkillTab(this);
		else skillTab.Reload();
		
		if(itemtypeTab == null) itemtypeTab = new ItemTypeTab(this);
		else itemtypeTab.Reload();
		
		if(itemTab == null) itemTab = new ItemTab(this);
		else itemTab.Reload();
		
		if(weaponTab == null) weaponTab = new WeaponTab(this);
		else weaponTab.Reload();
		
		if(armorTab == null) armorTab = new ArmorTab(this);
		else armorTab.Reload();
		
		if(enemyTab == null) enemyTab = new EnemyTab(this);
		else enemyTab.Reload();
		
		if(aiTab == null) aiTab = new BattleAITab(this);
		else aiTab.Reload();
		
		if(gsTab == null) gsTab = new GameSettingsTab(this);
		else gsTab.Reload();
		
		if(lsHudTab == null) lsHudTab = new LoadSaveHUDTab(this);
		else lsHudTab.Reload();
		
		if(mmTab == null) mmTab = new MainMenuTab(this);
		else mmTab.Reload();
		
		if(recipeTab == null) recipeTab = new ItemRecipeTab(this);
		else recipeTab.Reload();
		
		if(eventTab == null) eventTab = new GlobalEventTab(this);
		else eventTab.Reload();
		
		if(teleportTab == null) teleportTab = new TeleportTab(this);
		else teleportTab.Reload();
		
		if(attackTab == null) attackTab = new BaseAttackTab(this);
		else attackTab.Reload();
		
		if(raceTab == null) raceTab = new RaceTab(this);
		else raceTab.Reload();
		
		if(sizeTab == null) sizeTab = new SizeTab(this);
		else sizeTab.Reload();
		
		if(difficultyTab == null) difficultyTab = new DifficultyTab(this);
		else difficultyTab.Reload();
	}
	
	public void Save()
	{
		DataHolder.Languages().SaveData();
		DataHolder.Colors().SaveData();
		DataHolder.StatusValues().SaveData();
		DataHolder.Effects().SaveData();
		DataHolder.Elements().SaveData();
		DataHolder.EquipmentParts().SaveData();
		DataHolder.Formulas().SaveData();
		DataHolder.Characters().SaveData();
		DataHolder.Classes().SaveData();
		DataHolder.SkillTypes().SaveData();
		DataHolder.Skills().SaveData();
		DataHolder.ItemTypes().SaveData();
		DataHolder.Items().SaveData();
		DataHolder.Weapons().SaveData();
		DataHolder.Armors().SaveData();
		DataHolder.Enemies().SaveData();
		DataHolder.BattleAIs().SaveData();
		DataHolder.GameSettings().SaveData();
		DataHolder.LoadSaveHUD().SaveData();
		DataHolder.MainMenu().SaveData();
		DataHolder.ItemRecipes().SaveData();
		DataHolder.GlobalEvents().SaveData();
		DataHolder.Teleports().SaveData();
		DataHolder.BaseAttacks().SaveData();
		DataHolder.Races().SaveData();
		DataHolder.Sizes().SaveData();
		DataHolder.Difficulties().SaveData();
	}
	
	void OnGUI()
	{
		GUI.SetNextControlName("Toolbar");
		var prevSection = currentSection;
		currentSection = GUILayout.SelectionGrid(currentSection, sections, 8);
		if(prevSection != currentSection)
		{
			GUI.FocusControl("Toolbar");
		}
		GUILayout.Box(" ", GUILayout.ExpandWidth(true));
		
		if(currentSection == this.LANGUAGES)
		{
			this.langTab.ShowTab();
		}
		else if(currentSection == this.TEXT_COLORS)
		{
			this.colorTab.ShowTab();
		}
		else if(currentSection == this.STATUS_VALUES)
		{
			this.statusTab.ShowTab();
		}
		else if(currentSection == this.STATUS_EFFECTS)
		{
			this.effectTab.ShowTab();
		}
		else if(currentSection == this.ELEMENTS)
		{
			this.elementTab.ShowTab();
		}
		else if(currentSection == this.EQUIPMENT_PARTS)
		{
			this.equippartTab.ShowTab();
		}
		else if(currentSection == this.FORMULAS)
		{
			this.formulaTab.ShowTab();
		}
		else if(currentSection == this.CHARACTERS)
		{
			this.characterTab.ShowTab();
		}
		else if(currentSection == this.CLASS)
		{
			this.classTab.ShowTab();
		}
		else if(currentSection == this.SKILL_TYPES)
		{
			this.skilltypeTab.ShowTab();
		}
		else if(currentSection == this.SKILLS)
		{
			this.skillTab.ShowTab();
		}
		else if(currentSection == this.ITEM_TYPES)
		{
			this.itemtypeTab.ShowTab();
		}
		else if(currentSection == this.ITEMS)
		{
			this.itemTab.ShowTab();
		}
		else if(currentSection == this.WEAPONS)
		{
			this.weaponTab.ShowTab();
		}
		else if(currentSection == this.ARMORS)
		{
			this.armorTab.ShowTab();
		}
		else if(currentSection == this.ENEMIES)
		{
			this.enemyTab.ShowTab();
		}
		else if(currentSection == this.BATTLE_AI)
		{
			this.aiTab.ShowTab();
		}
		else if(currentSection == this.GAME_SETTINGS)
		{
			this.gsTab.ShowTab();
		}
		else if(currentSection == this.LOAD_SAVE_HUD)
		{
			this.lsHudTab.ShowTab();
		}
		else if(currentSection == this.MAIN_MENU)
		{
			this.mmTab.ShowTab();
		}
		else if(currentSection == this.ITEM_RECIPES)
		{
			this.recipeTab.ShowTab();
		}
		else if(currentSection == this.GLOBAL_EVENTS)
		{
			this.eventTab.ShowTab();
		}
		else if(currentSection == this.TELEPORTS)
		{
			this.teleportTab.ShowTab();
		}
		else if(currentSection == this.BASE_ATTACKS)
		{
			this.attackTab.ShowTab();
		}
		else if(currentSection == this.RACES)
		{
			this.raceTab.ShowTab();
		}
		else if(currentSection == this.SIZES)
		{
			this.sizeTab.ShowTab();
		}
		else if(currentSection == this.DIFFICULTIES)
		{
			this.difficultyTab.ShowTab();
		}
	
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		GUI.SetNextControlName("Reload");
		if(GUILayout.Button("Reload Settings"))
		{
			GUI.FocusControl("Reload");
			this.Reload();
		}
		GUI.SetNextControlName("Save");
		if(GUILayout.Button("Save Settings"))
		{
			GUI.FocusControl("Save");
			this.Save();
		}
		EditorGUILayout.EndHorizontal();
	}
	
	// add/remove
	
	public void AddLanguage(int lang)
	{
		DataHolder.StatusValues().AddLanguage(lang);
		DataHolder.Effects().AddLanguage(lang);
		DataHolder.Elements().AddLanguage(lang);
		DataHolder.EquipmentParts().AddLanguage(lang);
		DataHolder.Classes().AddLanguage(lang);
		DataHolder.SkillTypes().AddLanguage(lang);
		DataHolder.Skills().AddLanguage(lang);
		DataHolder.ItemTypes().AddLanguage(lang);
		DataHolder.Items().AddLanguage(lang);
		DataHolder.Weapons().AddLanguage(lang);
		DataHolder.Armors().AddLanguage(lang);
		DataHolder.Enemies().AddLanguage(lang);
		DataHolder.GameSettings().AddLanguage(lang);
		DataHolder.LoadSaveHUD().AddLanguage(lang);
		DataHolder.MainMenu().AddLanguage(lang);
		DataHolder.ItemRecipes().AddLanguage(lang);
		DataHolder.Teleports().AddLanguage(lang);
	}
	
	public void RemoveLanguage(int lang)
	{
		DataHolder.StatusValues().RemoveLanguage(lang);
		DataHolder.Effects().RemoveLanguage(lang);
		DataHolder.Elements().RemoveLanguage(lang);
		DataHolder.EquipmentParts().RemoveLanguage(lang);
		DataHolder.Classes().RemoveLanguage(lang);
		DataHolder.SkillTypes().RemoveLanguage(lang);
		DataHolder.Skills().RemoveLanguage(lang);
		DataHolder.ItemTypes().RemoveLanguage(lang);
		DataHolder.Items().RemoveLanguage(lang);
		DataHolder.Weapons().RemoveLanguage(lang);
		DataHolder.Armors().RemoveLanguage(lang);
		DataHolder.Enemies().RemoveLanguage(lang);
		DataHolder.GameSettings().RemoveLanguage(lang);
		DataHolder.LoadSaveHUD().RemoveLanguage(lang);
		DataHolder.MainMenu().RemoveLanguage(lang);
		DataHolder.ItemRecipes().RemoveLanguage(lang);
		DataHolder.Teleports().RemoveLanguage(lang);
	}
	
	public void AddStatusValue(int index)
	{
		DataHolder.Effects().AddStatusValue(index);
		DataHolder.Formulas().AddStatusValue(index);
		DataHolder.Characters().AddStatusValue(index, this.GetStatusValueData(index));
		DataHolder.Classes().AddStatusValue(index, this.GetStatusValueData(index));
		DataHolder.Skills().AddStatusValue(index);
		DataHolder.Weapons().AddStatusValue(index);
		DataHolder.Armors().AddStatusValue(index);
		DataHolder.Enemies().AddStatusValue(index);
		DataHolder.Items().AddStatusValue(index);
		DataHolder.BaseAttacks().AddStatusValue(index);
		DataHolder.Difficulties().AddStatusValue(index);
	}
	
	public void RemoveStatusValue(int index)
	{
		DataHolder.Effects().RemoveStatusValue(index);
		DataHolder.Formulas().RemoveStatusValue(index);
		DataHolder.Characters().RemoveStatusValue(index);
		DataHolder.Classes().RemoveStatusValue(index);
		DataHolder.Skills().RemoveStatusValue(index);
		DataHolder.Weapons().RemoveStatusValue(index);
		DataHolder.Armors().RemoveStatusValue(index);
		DataHolder.Enemies().RemoveStatusValue(index);
		DataHolder.BattleAIs().RemoveStatusValue(index);
		DataHolder.Items().RemoveStatusValue(index);
		DataHolder.BaseAttacks().RemoveStatusValue(index);
		DataHolder.Difficulties().RemoveStatusValue(index);
	}
	
	public void SetStatusValueType(int index, StatusValueType val)
	{
		DataHolder.Effects().SetStatusValueType(index, val);
		DataHolder.Characters().SetStatusValueType(index, val, this.GetStatusValueData(index));
		DataHolder.Classes().SetStatusValueType(index, val, this.GetStatusValueData(index));
		DataHolder.Skills().SetStatusValueType(index, val);
		DataHolder.Weapons().SetStatusValueType(index, val);
		DataHolder.Armors().SetStatusValueType(index, val);
		DataHolder.Enemies().SetStatusValueType(index, val);
		DataHolder.BaseAttacks().SetStatusValueType(index, val);
		DataHolder.Difficulties().SetStatusValueType(index, val);
	}
	
	public void StatusValueMinMaxChanged(int index, int min, int max)
	{
		DataHolder.Characters().StatusValueMinMaxChanged(index, min, max);
		DataHolder.Classes().StatusValueMinMaxChanged(index, min, max);
	}
	
	public void AddStatusEffect(int index)
	{
		DataHolder.Skills().AddStatusEffect(index);
		DataHolder.Items().AddStatusEffect(index);
		DataHolder.Weapons().AddStatusEffect(index);
		DataHolder.Armors().AddStatusEffect(index);
		DataHolder.Enemies().AddStatusEffect(index);
	}
	
	public void RemoveStatusEffect(int index)
	{
		DataHolder.Skills().RemoveStatusEffect(index);
		DataHolder.Items().RemoveStatusEffect(index);
		DataHolder.Weapons().RemoveStatusEffect(index);
		DataHolder.Armors().RemoveStatusEffect(index);
		DataHolder.Enemies().RemoveStatusEffect(index);
		DataHolder.BattleAIs().RemoveStatusEffect(index);
	}
	
	public void AddElement(int index)
	{
		DataHolder.Effects().AddElement(index);
		DataHolder.Skills().AddElement(index);
		DataHolder.Classes().AddElement(index);
		DataHolder.Armors().AddElement(index);
		DataHolder.Enemies().AddElement(index);
		DataHolder.Weapons().AddElement(index);
		DataHolder.Characters().AddElement();
	}
	
	public void RemoveElement(int index)
	{
		DataHolder.Effects().RemoveElement(index);
		DataHolder.Classes().RemoveElement(index);
		DataHolder.Skills().RemoveElement(index);
		DataHolder.Weapons().RemoveElement(index);
		DataHolder.Armors().RemoveElement(index);
		DataHolder.Enemies().RemoveElement(index);
		DataHolder.Characters().RemoveElement(index);
	}
	
	public void AddEquipmentPart(int index)
	{
		DataHolder.Classes().AddEquipmentPart(index);
		DataHolder.Weapons().AddEquipmentPart(index);
		DataHolder.Armors().AddEquipmentPart(index);
	}
	
	public void RemoveEquipmentPart(int index)
	{
		DataHolder.Classes().RemoveEquipmentPart(index);
		DataHolder.Weapons().RemoveEquipmentPart(index);
		DataHolder.Armors().RemoveEquipmentPart(index);
	}
	
	public void AddFormula(int index)
	{
		
	}
	
	public void RemoveFormula(int index)
	{
		DataHolder.Characters().RemoveFormula(index);
		DataHolder.Skills().RemoveFormula(index);
		DataHolder.Enemies().RemoveFormula(index);
		DataHolder.BaseAttacks().RemoveFormula(index);
	}
	
	public void AddClass(int index)
	{
		
	}
	
	public void RemoveClass(int index)
	{
		DataHolder.Characters().RemoveClass(index);
	}
	
	public void AddRace()
	{
		DataHolder.Classes().AddRace();
		DataHolder.Enemies().AddRace();
		DataHolder.Effects().AddRace();
		DataHolder.Weapons().AddRace();
		DataHolder.Armors().AddRace();
		DataHolder.Skills().AddRace();
		DataHolder.Characters().AddRace();
	}
	
	public void RemoveRace(int index)
	{
		DataHolder.Characters().RemoveRace(index);
		DataHolder.Enemies().RemoveRace(index);
		DataHolder.Classes().RemoveRace(index);
		DataHolder.Effects().RemoveRace(index);
		DataHolder.Weapons().RemoveRace(index);
		DataHolder.Armors().RemoveRace(index);
		DataHolder.Skills().RemoveRace(index);
		DataHolder.BattleAIs().RemoveRace(index);
	}
	
	public void AddSize()
	{
		DataHolder.Classes().AddSize();
		DataHolder.Enemies().AddSize();
		DataHolder.Effects().AddSize();
		DataHolder.Weapons().AddSize();
		DataHolder.Armors().AddSize();
		DataHolder.Skills().AddSize();
		DataHolder.Characters().AddSize();
	}
	
	public void RemoveSize(int index)
	{
		DataHolder.Characters().RemoveSize(index);
		DataHolder.Enemies().RemoveSize(index);
		DataHolder.Classes().RemoveSize(index);
		DataHolder.Effects().RemoveSize(index);
		DataHolder.Weapons().RemoveSize(index);
		DataHolder.Armors().RemoveSize(index);
		DataHolder.Skills().RemoveSize(index);
		DataHolder.BattleAIs().RemoveRace(index);
	}
	
	public void AddSkillType()
	{
		DataHolder.Effects().AddSkillType();
	}
	
	public void RemoveSkillType(int index)
	{
		DataHolder.Skills().RemoveSkillType(index);
		DataHolder.Effects().RemoveSkillType(index);
	}
	
	public void AddSkill(int index)
	{
		
	}
	
	public void RemoveSkill(int index)
	{
		DataHolder.Characters().RemoveSkill(index);
		DataHolder.Classes().RemoveSkill(index);
		DataHolder.Items().RemoveSkill(index);
		DataHolder.Weapons().RemoveSkill(index);
		DataHolder.Effects().RemoveSkill(index);
	}
	
	public void RemoveItemType(int index)
	{
		DataHolder.Items().RemoveItemType(index);
	}
	
	public void RemoveItem(int index)
	{
		DataHolder.Enemies().RemoveItem(index);
		DataHolder.BaseAttacks().RemoveItem(index);
		DataHolder.Skills().RemoveItem(index);
		DataHolder.ItemRecipes().RemoveItem(index);
	}
	
	public void AddWeapon(int index)
	{
		DataHolder.Classes().AddWeapon(index);
	}
	
	public void RemoveWeapon(int index)
	{
		DataHolder.Classes().RemoveWeapon(index);
		DataHolder.ItemRecipes().RemoveWeapon(index);
		DataHolder.BaseAttacks().RemoveWeapon(index);
		DataHolder.Skills().RemoveWeapon(index);
		DataHolder.Enemies().RemoveWeapon(index);
	}
	
	public void SetWeaponEPChanged(int index)
	{
		classTab.SetWeaponEPChanged(index);
	}
	
	public void AddArmor(int index)
	{
		DataHolder.Classes().AddArmor(index);
	}
	
	public void RemoveArmor(int index)
	{
		DataHolder.Classes().RemoveArmor(index);
		DataHolder.ItemRecipes().RemoveArmor(index);
		DataHolder.BaseAttacks().RemoveArmor(index);
		DataHolder.Skills().RemoveArmor(index);
		DataHolder.Enemies().RemoveArmor(index);
	}
	
	public void SetArmorEPChanged(int index)
	{
		classTab.SetArmorEPChanged(index);
	}
	
	public void RemoveEnemyAI(int index)
	{
		
	}
	
	public void AddItemRecipe(int index)
	{
		
	}
	
	public void RemoveItemRecipe(int index)
	{
		
	}
	
	public void RemoveBaseAttack(int index)
	{
		DataHolder.Weapons().RemoveBaseAttack(index);
		DataHolder.Characters().RemoveBaseAttack(index);
		DataHolder.Enemies().RemoveBaseAttack(index);
	}
	
	public void RemoveDifficulty(int index)
	{
		DataHolder.Effects().RemoveDifficulty(index);
		DataHolder.Skills().RemoveDifficulty(index);
		DataHolder.Weapons().RemoveDifficulty(index);
		DataHolder.Armors().RemoveDifficulty(index);
		DataHolder.Classes().RemoveDifficulty(index);
		DataHolder.Characters().RemoveDifficulty(index);
		DataHolder.Enemies().RemoveDifficulty(index);
	}
	
	// get count/name
	
	public int GetLangCount()
	{
		return DataHolder.Languages().GetDataCount();
	}
	
	public string GetLang(int index)
	{
		return DataHolder.Languages().GetName(index);
	}
	
	public int GetStatusValueCount()
	{
		return DataHolder.StatusValues().GetDataCount();
	}
	
	public string GetStatusValue(int index)
	{
		return DataHolder.StatusValues().GetName(index);
	}
	
	public string[] GetStatusValues()
	{
		return DataHolder.StatusValues().GetNameList(true);
	}
	
	public StatusValue GetStatusValueData(int index)
	{
		return DataHolder.StatusValues().GetStatusValueData(index);
	}
	
	public StatusValue[] GetStatusValuesData()
	{
		return DataHolder.StatusValues().GetStatusValuesData();
	}
	
	public bool IsStatusValueNormal(int index)
	{
		return DataHolder.StatusValues().value[index].IsNormal();
	}
	
	public bool IsStatusValueConsumable(int index)
	{
		return DataHolder.StatusValues().value[index].IsConsumable();
	}
	
	public bool IsStatusValueExperience(int index)
	{
		return DataHolder.StatusValues().value[index].IsExperience();
	}
	
	public int GetStatusEffectCount()
	{
		return DataHolder.Effects().GetDataCount();
	}
	
	public string GetStatusEffect(int index)
	{
		return DataHolder.Effects().GetName(index);
	}
	
	public string[] GetStatusEffects()
	{
		return DataHolder.Effects().GetNameList(true);
	}
	
	public int GetElementCount()
	{
		return DataHolder.Elements().GetDataCount();
	}
	
	public string GetElement(int index)
	{
		return DataHolder.Elements().GetName(index);
	}
	
	public string[] GetElements()
	{
		return DataHolder.Elements().GetNameList(true);
	}
	
	public int GetEquipmentPartCount()
	{
		return DataHolder.EquipmentParts().GetDataCount();
	}
	
	public string GetEquipmentPart(int index)
	{
		return DataHolder.EquipmentParts().GetName(index);
	}
	
	public int GetFormulaCount()
	{
		return DataHolder.Formulas().GetDataCount();
	}
	
	public string GetFormula(int index)
	{
		return DataHolder.Formulas().GetName(index);
	}
	
	public string[] GetFormulas()
	{
		return DataHolder.Formulas().GetNameList(true);
	}
	
	public int GetClassCount()
	{
		return DataHolder.Classes().GetDataCount();
	}
	
	public string GetClassNumber(int index)
	{
		return DataHolder.Classes().GetName(index);
	}
	
	public string[] GetClasses()
	{
		return DataHolder.Classes().GetNameList(true);
	}
	
	public int GetSkillTypeCount()
	{
		return DataHolder.SkillTypes().GetDataCount();
	}
	
	public string GetSkillType(int index)
	{
		return DataHolder.SkillTypes().GetName(index);
	}
	
	public string[] GetSkillTypes()
	{
		return DataHolder.SkillTypes().GetNameList(true);
	}
	
	public int GetSkillCount()
	{
		return DataHolder.Skills().GetDataCount();
	}
	
	public string GetSkill(int index)
	{
		return DataHolder.Skills().GetName(index);
	}
	
	public string[] GetSkills()
	{
		return DataHolder.Skills().GetNameList(true);
	}
	
	public int GetItemTypeCount()
	{
		return DataHolder.ItemTypes().GetDataCount();
	}
	
	public string GetItemType(int index)
	{
		return DataHolder.ItemTypes().GetName(index);
	}
	
	public string[] GetItemTypes()
	{
		return DataHolder.ItemTypes().GetNameList(true);
	}
	
	public int GetItemCount()
	{
		return DataHolder.Items().GetDataCount();
	}
	
	public string GetItem(int index)
	{
		return DataHolder.Items().GetName(index);
	}
	
	public string[] GetItems()
	{
		return DataHolder.Items().GetNameList(true);
	}
	
	public int GetWeaponCount()
	{
		return DataHolder.Weapons().GetDataCount();
	}
	
	public string GetWeapon(int index)
	{
		return DataHolder.Weapons().GetName(index);
	}
	
	public string[] GetWeapons()
	{
		return DataHolder.Weapons().GetNameList(true);
	}
	
	public bool IsWeaponEquipable(int index, bool[] ep)
	{
		return DataHolder.Weapons().weapon[index].IsEquipable(ep);
	}
	
	public int GetArmorCount()
	{
		return DataHolder.Armors().GetDataCount();
	}
	
	public string GetArmor(int index)
	{
		return DataHolder.Armors().GetName(index);
	}
	
	public string[] GetArmors()
	{
		return DataHolder.Armors().GetNameList(true);
	}
	
	public bool IsArmorEquipable(int index, bool[] ep)
	{
		return DataHolder.Armors().armor[index].IsEquipable(ep);
	}
	
	public int GetEnemyCount()
	{
		return DataHolder.Enemies().GetDataCount();
	}
	
	public string GetEnemy(int index)
	{
		return DataHolder.Enemies().GetName(index);
	}
	
	public int GetEnemyAICount()
	{
		return DataHolder.BattleAIs().GetDataCount();
	}
	
	public string GetEnemyAI(int index)
	{
		return DataHolder.BattleAIs().GetName(index);
	}
	
	public string[] GetEnemyAIs()
	{
		return DataHolder.BattleAIs().GetNameList(true);
	}
}