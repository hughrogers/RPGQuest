
using UnityEngine;

public class DataHolder
{
	private static DataHolder instance;
	
	// data
	private AreaNameData areaNames;
	private ArmorData armors;
	private BattleAnimationData battleAnimations;
	private BattleSystemData battleSystem;
	private CameraPositionData cameraPositions;
	private CharacterData characters;
	private ClassData classes;
	private ColorData colors;
	private DialoguePositionData dialoguePositions;
	private ElementData elements;
	private BattleAIData battleAIs;
	private EnemyData enemies;
	private EquipmentPartData equipParts;
	private FormulaData formulas;
	private GameSettingsData gameSettings;
	private HUDData huds;
	private ItemData items;
	private ItemTypeData itemTypes;
	private LanguageData languages;
	private LoadSaveHUDData loadSaveHUD;
	private MainMenuData mainMenu;
	private MusicData music;
	private SkillData skills;
	private SkillTypeData skillTypes;
	private StatusEffectData effects;
	private StatusValueData statusValues;
	private WeaponData weapons;
	private ItemRecipeData recipes;
	private FontData fonts;
	private GlobalEventData globalEvents;
	private TeleportData teleports;
	private BaseAttackData attacks;
	private RaceData races;
	private SizeData sizes;
	private DifficultyData difficulties;
	
	private DataHolder()
	{
		if(instance != null)
		{
			Debug.Log("There is already an instance of DataHolder!");
			return;
		}
		instance = this;
		Init();
	}
	
	/*
	============================================================================
	Init functions
	============================================================================
	*/
	public void Init()
	{
		// first init languages
		languages = new LanguageData();
		
		statusValues = new  StatusValueData();
		elements = new  ElementData();
		races = new RaceData();
		sizes = new SizeData();
		areaNames = new AreaNameData();
		armors = new ArmorData();
		cameraPositions = new  CameraPositionData();
		attacks = new BaseAttackData();
		characters = new  CharacterData();
		classes = new  ClassData();
		colors = new  ColorData();
		dialoguePositions = new  DialoguePositionData();
		battleAIs = new  BattleAIData();
		enemies = new  EnemyData();
		equipParts = new  EquipmentPartData();
		formulas = new  FormulaData();
		gameSettings = new  GameSettingsData();
		items = new  ItemData();
		itemTypes = new  ItemTypeData();
		loadSaveHUD = new LoadSaveHUDData();
		mainMenu = new MainMenuData();
		skillTypes = new  SkillTypeData();
		effects = new  StatusEffectData();
		skills = new  SkillData();
		weapons = new  WeaponData();
		music = new MusicData();
		huds = new HUDData();
		recipes = new ItemRecipeData();
		fonts = new FontData();
		globalEvents = new GlobalEventData();
		teleports = new TeleportData();
		difficulties = new DifficultyData();
		
		// battle system
		battleAnimations = new BattleAnimationData();
		battleSystem = new BattleSystemData();
	}
	
	public static DataHolder Instance()
	{
		if(instance == null)
		{
			new DataHolder();
		}
		return instance;
	}
	
	/*
	============================================================================
	Data functions
	============================================================================
	*/
	public static AreaNameData AreaNames()
	{
		return DataHolder.Instance().areaNames;
	}
	
	public static string AreaName(int index)
	{
		return DataHolder.Instance().areaNames.GetName(index);
	}
	
	public static ArmorData Armors()
	{
		return DataHolder.Instance().armors;
	}
	
	public static Armor Armor(int index)
	{
		return DataHolder.Instance().armors.armor[index];
	}
	
	public static BattleAnimationData BattleAnimations()
	{
		return DataHolder.Instance().battleAnimations;
	}
	
	public static BattleAnimation BattleAnimation(int index)
	{
		return DataHolder.Instance().battleAnimations.animation[index];
	}
	
	public static BattleSystemData BattleSystemData()
	{
		return DataHolder.Instance().battleSystem;
	}
	
	public static BattleSystem BattleSystem()
	{
		return DataHolder.Instance().battleSystem.system;
	}
	
	public static BattleCam BattleCam()
	{
		return DataHolder.Instance().battleSystem.cam;
	}
	
	public static BattleControl BattleControl()
	{
		return DataHolder.Instance().battleSystem.control;
	}
	
	public static BattleMenu BattleMenu()
	{
		return DataHolder.Instance().battleSystem.menu;
	}
	
	public static BattleEnd BattleEnd()
	{
		return DataHolder.Instance().battleSystem.end;
	}
	
	public static CameraPositionData CameraPositions()
	{
		return DataHolder.Instance().cameraPositions;
	}
	
	public static CameraPosition CameraPosition(int index)
	{
		return DataHolder.Instance().cameraPositions.cam[index];
	}
	
	public static CharacterData Characters()
	{
		return DataHolder.Instance().characters;
	}
	
	public static Character Character(int index)
	{
		return DataHolder.Instance().characters.character[index];
	}
	
	public static ClassData Classes()
	{
		return DataHolder.Instance().classes;
	}
	
	public static Class Class(int index)
	{
		return DataHolder.Instance().classes.classes[index];
	}
	
	public static ColorData Colors()
	{
		return DataHolder.Instance().colors;
	}
	
	public static Color Color(int index)
	{
		Color c;
		if(index < DataHolder.Instance().colors.color.Length)
		{
			c = DataHolder.Instance().colors.color[index];
		}
		else
		{
			c = DataHolder.Instance().colors.color[0];
		}
		return c;
	}
	
	public static DialoguePositionData DialoguePositions()
	{
		return DataHolder.Instance().dialoguePositions;
	}
	
	public static DialoguePosition DialoguePosition(int index)
	{
		return DataHolder.Instance().dialoguePositions.position[index];
	}
	
	public static ElementData Elements()
	{
		return DataHolder.Instance().elements;
	}
	
	public static string Element(int index)
	{
		return DataHolder.Instance().elements.GetName(index);
	}
	
	public static BattleAIData BattleAIs()
	{
		return DataHolder.Instance().battleAIs;
	}
	
	public static BattleAI BattleAI(int index)
	{
		return DataHolder.Instance().battleAIs.ai[index];
	}
	
	public static EnemyData Enemies()
	{
		return DataHolder.Instance().enemies;
	}
	
	public static Enemy Enemy(int index)
	{
		return DataHolder.Instance().enemies.enemy[index];
	}
	
	public static EquipmentPartData EquipmentParts()
	{
		return DataHolder.Instance().equipParts;
	}
	
	public static string EquipmentPart(int index)
	{
		return DataHolder.Instance().equipParts.GetName(index);
	}
	
	public static FormulaData Formulas()
	{
		return DataHolder.Instance().formulas;
	}
	
	public static Formula Formula(int index)
	{
		return DataHolder.Instance().formulas.formula[index];
	}
	
	public static GameSettingsData GameSettings()
	{
		return DataHolder.Instance().gameSettings;
	}
	
	public static HUDData HUDs()
	{
		return DataHolder.Instance().huds;
	}
	
	public static HUD HUD(int index)
	{
		return DataHolder.Instance().huds.hud[index];
	}
	
	public static ItemData Items()
	{
		return DataHolder.Instance().items;
	}
	
	public static Item Item(int index)
	{
		return DataHolder.Instance().items.item[index];
	}
	
	public static ItemRecipeData ItemRecipes()
	{
		return DataHolder.Instance().recipes;
	}
	
	public static ItemRecipe ItemRecipe(int index)
	{
		return DataHolder.Instance().recipes.recipe[index];
	}
	
	public static ItemTypeData ItemTypes()
	{
		return DataHolder.Instance().itemTypes;
	}
	
	public static string ItemType(int index)
	{
		return DataHolder.Instance().itemTypes.GetName(index);
	}
	
	public static LanguageData Languages()
	{
		return DataHolder.Instance().languages;
	}
	
	public static string Language(int index)
	{
		return DataHolder.Instance().languages.GetName(index);
	}
	
	public static LoadSaveHUDData LoadSaveHUD()
	{
		return DataHolder.Instance().loadSaveHUD;
	}
	
	public static MainMenuData MainMenu()
	{
		return DataHolder.Instance().mainMenu;
	}
	
	public static SkillData Skills()
	{
		return DataHolder.Instance().skills;
	}
	
	public static Skill Skill(int index)
	{
		return DataHolder.Instance().skills.skill[index];
	}
	
	public static SkillTypeData SkillTypes()
	{
		return DataHolder.Instance().skillTypes;
	}
	
	public static string SkillType(int index)
	{
		return DataHolder.Instance().skillTypes.GetName(index);
	}
	
	public static StatusEffectData Effects()
	{
		return DataHolder.Instance().effects;
	}
	
	public static StatusEffect Effect(int index)
	{
		return DataHolder.Instance().effects.effect[index];
	}
	
	public static StatusValueData StatusValues()
	{
		return DataHolder.Instance().statusValues;
	}
	
	public static StatusValue StatusValue(int index)
	{
		return DataHolder.Instance().statusValues.value[index];
	}
	
	public static WeaponData Weapons()
	{
		return DataHolder.Instance().weapons;
	}
	
	public static Weapon Weapon(int index)
	{
		return DataHolder.Instance().weapons.weapon[index];
	}
	
	public static MusicData Music()
	{
		return DataHolder.Instance().music;
	}
	
	public static MusicClip MusicClip(int index)
	{
		return DataHolder.Instance().music.music[index];
	}
	
	public static FontData Fonts()
	{
		return DataHolder.Instance().fonts;
	}
	
	public static GlobalEventData GlobalEvents()
	{
		return DataHolder.Instance().globalEvents;
	}
	
	public static GlobalEvent GlobalEvent(int index)
	{
		return DataHolder.Instance().globalEvents.globalEvent[index];
	}
	
	public static TeleportData Teleports()
	{
		return DataHolder.Instance().teleports;
	}
	
	public static TeleportTarget Teleport(int index)
	{
		return DataHolder.Instance().teleports.teleport[index];
	}
	
	public static BaseAttackData BaseAttacks()
	{
		return DataHolder.Instance().attacks;
	}
	
	public static BaseAttack BaseAttack(int index)
	{
		return DataHolder.Instance().attacks.baseAttack[index];
	}
	
	public static RaceData Races()
	{
		return DataHolder.Instance().races;
	}
	
	public static string Race(int index)
	{
		return DataHolder.Instance().races.GetName(index);
	}
	
	public static SizeData Sizes()
	{
		return DataHolder.Instance().sizes;
	}
	
	public static string Size(int index)
	{
		return DataHolder.Instance().sizes.GetName(index);
	}
	
	public static DifficultyData Difficulties()
	{
		return DataHolder.Instance().difficulties;
	}
	
	public static Difficulty Difficulty(int index)
	{
		return DataHolder.Instance().difficulties.difficulty[index];
	}
	
	public static GameStatistic Statistic
	{
		get
		{
			return DataHolder.Instance().gameSettings.statistic;
		}
	}
	
	/*
	============================================================================
	Name functions
	============================================================================
	*/
	public static string GetItemName(ItemDropType type, int id)
	{
		string name = "";
		if(ItemDropType.ITEM.Equals(type)) name = DataHolder.Items().GetName(id);
		else if(ItemDropType.WEAPON.Equals(type)) name = DataHolder.Weapons().GetName(id);
		else if(ItemDropType.ARMOR.Equals(type)) name = DataHolder.Armors().GetName(id);
		return name;
	}
	
	/*
	============================================================================
	Count functions
	============================================================================
	*/
	public static int LanguageCount
	{
		get
		{
			return DataHolder.Languages().GetDataCount();
		}
	}
	
	public static int StatusValueCount
	{
		get
		{
			return DataHolder.StatusValues().GetDataCount();
		}
	}
	
	public static int EffectCount
	{
		get
		{
			return DataHolder.Effects().GetDataCount();
		}
	}
	
	public static int ElementCount
	{
		get
		{
			return DataHolder.Elements().GetDataCount();
		}
	}
	
	public static int RaceCount
	{
		get
		{
			return DataHolder.Races().GetDataCount();
		}
	}
	
	public static int SizeCount
	{
		get
		{
			return DataHolder.Sizes().GetDataCount();
		}
	}
	
	public static int EnemyCount
	{
		get
		{
			return DataHolder.Enemies().GetDataCount();
		}
	}
	
	public static int ItemCount
	{
		get
		{
			return DataHolder.Items().GetDataCount();
		}
	}
	
	public static int WeaponCount
	{
		get
		{
			return DataHolder.Weapons().GetDataCount();
		}
	}
	
	public static int ArmorCount
	{
		get
		{
			return DataHolder.Armors().GetDataCount();
		}
	}
	
	public static int GlobalEventCount
	{
		get
		{
			return DataHolder.Armors().GetDataCount();
		}
	}
	
	public static int SkillTypeCount
	{
		get
		{
			return DataHolder.SkillTypes().GetDataCount();
		}
	}
}