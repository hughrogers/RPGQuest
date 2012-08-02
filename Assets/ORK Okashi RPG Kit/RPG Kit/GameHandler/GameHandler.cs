
using System.Collections;
using UnityEngine;

public class GameHandler
{
	private static GameHandler instance;
	
	private bool pauseGame = false;
	private int langID = 0;
	private int difficultyID = 0;
	
	// ingame
	private ControlType currentControl = ControlType.NONE;
	private int areaName = -1;
	private float gameTime = 0;
	private int inBattleArea = 0;
	private BattleArea battleArea = null;
	private int blockControl = 0;
	private ArrayList noSpawns = new ArrayList();
	private ArrayList noClickMove = new ArrayList();
	
	// handlers
	private PartyHandler partyHandler;
	private MusicHandler musicHandler;
	private LevelHandler levelHandler;
	private DropHandler dropHandler;
	private DragHandler dragHandler;
	private GUIHandler guiHandler;
	private Hashtable skinWrappers = new Hashtable();
	
	// Hashtable(itemID, itemCount)
	private Hashtable items;
	private Hashtable weapons;
	private Hashtable armors;
	private int money;
	private Hashtable variables;
	private Hashtable numberVariables;
	private ArrayList recipes;
	
	// other handlers
	private ArrayList changeNotifiers = new ArrayList();
	private WindowHandler windowHandler;
	
	// ORK time factor
	private float timeFactor = 1.0f;
	private int freezeCount = 0;
	
	private GameHandler()
	{
		if(instance != null)
		{
			Debug.Log("There is already an instance of GameHandler!");
			return;
		}
		instance = this;
		this.Init();
	}
	
	/*
	============================================================================
	Init functions
	============================================================================
	*/
	public void Init()
	{
		this.ClearData();
		this.partyHandler = new PartyHandler();
		
		if(Application.isPlaying)
		{
			if(this.levelHandler == null)
			{
				GameObject tmp = new GameObject("LevelHandler");
				if(GUISystemType.ORK.Equals(DataHolder.GameSettings().guiSystemType))
				{
					this.levelHandler = (LevelHandler)tmp.AddComponent("LevelHandler");
				}
				else
				{
					this.levelHandler = (LevelHandlerGUI)tmp.AddComponent("LevelHandlerGUI");
				}
			}
			if(this.musicHandler == null)
			{
				GameObject tmp = new GameObject("MusicHandler");
				this.musicHandler = (MusicHandler)tmp.AddComponent("MusicHandler");
			}
			if(this.dropHandler == null)
			{
				GameObject tmp = new GameObject("DropHandler");
				this.dropHandler = (DropHandler)tmp.AddComponent("DropHandler");
				this.dropHandler.ClearData();
			}
			if(this.dragHandler == null)
			{
				GameObject tmp = new GameObject("DragHandler");
				this.dragHandler = (DragHandler)tmp.AddComponent("DragHandler");
			}
			if(this.windowHandler == null)
			{
				this.windowHandler = new WindowHandler();
			}
			if(this.guiHandler == null)
			{
				GameObject tmp = new GameObject("GUIHandler");
				this.guiHandler = (GUIHandler)tmp.AddComponent("GUIHandler");
			}
		}
	}
	
	public static GameHandler Instance()
	{
		if(instance == null)
		{
			new GameHandler();
		}
		return instance;
	}
	
	public void ClearData()
	{
		if(this.dropHandler != null) this.dropHandler.ClearData();
		this.items = new Hashtable();
		this.weapons = new Hashtable();
		this.armors = new Hashtable();
		this.money = 0;
		this.variables = new Hashtable();
		this.numberVariables = new Hashtable();
		this.recipes = new ArrayList();
		this.inBattleArea = 0;
		this.battleArea = null;
	}
	
	public static void StartNewGame()
	{
		DataHolder.Statistic.Clear();
		GameHandler.GetMusicHandler().Stop();
		GameObject tmp = new GameObject("NewGameSceneChanger");
		SceneChanger changer = tmp.AddComponent<SceneChanger>();
		GameHandler.Instance().Init();
		changer.NewGameScene();
		changer.ChangeScene();
	}
	
	/*
	============================================================================
	Handler functions
	============================================================================
	*/
	public static GUIHandler GUIHandler()
	{
		return GameHandler.Instance().guiHandler;
	}
	
	public static GUISkinWrapper GetSkin(GUISkin skin)
	{
		GUISkinWrapper wrapper = null;
		if(skin != null)
		{
			if(!GameHandler.Instance().skinWrappers.ContainsKey(skin.name))
			{
				GameHandler.Instance().skinWrappers.Add(skin.name, new GUISkinWrapper(skin));
			}
			wrapper = (GUISkinWrapper)GameHandler.Instance().skinWrappers[skin.name];
		}
		return wrapper;
	}
	
	public static WindowHandler WindowHandler()
	{
		return GameHandler.Instance().windowHandler;
	}
	
	public static LevelHandler GetLevelHandler()
	{
		return GameHandler.Instance().levelHandler;
	}
	
	public static MusicHandler GetMusicHandler()
	{
		return GameHandler.Instance().musicHandler;
	}
	
	public static DragHandler DragHandler()
	{
		return GameHandler.Instance().dragHandler;
	}
	
	public static DropHandler DropHandler()
	{
		return GameHandler.Instance().dropHandler;
	}
	
	/*
	============================================================================
	Change notifier functions
	============================================================================
	*/
	public static void RegisterChangeNotifier(ChangeNotifier c)
	{
		GameHandler.Instance().changeNotifiers.Add(c);
	}
	
	public static void ChangeHappened(int id, int info, int info2)
	{
		foreach(ChangeNotifier c in GameHandler.Instance().changeNotifiers)
		{
			c.ChangeHappened(id, info, info2);
		}
	}
	
	/*
	============================================================================
	Time functions
	============================================================================
	*/
	public static float TimeFactor
	{
		get
		{
			return GameHandler.Instance().freezeCount > 0 ? 0 : GameHandler.Instance().timeFactor;
		}
	}
	
	public static float DeltaTime
	{
		get
		{
			return Time.deltaTime*GameHandler.TimeFactor*DataHolder.Difficulties().GetTimeFactor();
		}
	}
	
	public static float DeltaMovementTime
	{
		get
		{
			return Time.deltaTime*GameHandler.TimeFactor*DataHolder.Difficulties().GetMovementFactor();
		}
	}
	
	public static float DeltaBattleTime
	{
		get
		{
			return Time.deltaTime*GameHandler.TimeFactor*DataHolder.Difficulties().GetBattleFactor();
		}
	}
	
	public static float AnimationFactor
	{
		get
		{
			return GameHandler.TimeFactor*DataHolder.Difficulties().GetAnimationFactor();
		}
	}
	
	public static void FreezeTime(bool b)
	{
		if(b) GameHandler.Instance().freezeCount++;
		else GameHandler.Instance().freezeCount--;
	}
	
	/*
	============================================================================
	Game time functions
	============================================================================
	*/
	public static float GetGameTime()
	{
		return GameHandler.Instance().gameTime;
	}
	
	public static void SetGameTime(float t)
	{
		GameHandler.Instance().gameTime = t;
	}
	
	public static void AddGameTime(float t)
	{
		GameHandler.Instance().gameTime += t;
	}
	
	public static string GetTimeString()
	{
		return GameHandler.GetTimeString(GameHandler.GetGameTime());
	}
	
	public static string GetTimeString(float t)
	{
		int h = (int)(t / 3600);
		t -= h * 3600;
		int m = (int)(t / 60);
		t -= m * 60;
		int s = (int)t;
		string time = h.ToString() + ":";
		if(m < 10) time += "0";
		time += m.ToString() + ":";
		if(s < 10) time += "0";
		time += s.ToString();
		return time;
	}
	
	/*
	============================================================================
	Pause functions
	============================================================================
	*/
	public static void PauseGame(bool pause)
	{
		GameHandler.Instance().pauseGame = pause;
		if(DataHolder.GameSettings().freezePause)
		{
			if(GameHandler.IsGamePaused()) GameHandler.FreezeTime(true);
			else GameHandler.FreezeTime(false);
		}
	}
	
	public static bool IsGamePaused()
	{
		return GameHandler.Instance().pauseGame;
	}
	
	/*
	============================================================================
	Battle area functions
	============================================================================
	*/
	public static bool IsInBattleArea()
	{
		return GameHandler.Instance().inBattleArea > 0;
	}
	
	public static void SetBattleArea(BattleArea area)
	{
		GameHandler.Instance().battleArea = area;
	}
	
	public static BattleArea GetBattleArea()
	{
		return GameHandler.Instance().battleArea;
	}
	
	public static void SetInBattleArea(int add)
	{
		if(DataHolder.BattleSystem().IsRealTime() && 
			DataHolder.GameSettings().onlyInBattleArea &&
			GameHandler.Instance().inBattleArea == 0 && add > 0)
		{
			GameHandler.Party().SpawnParty(false);
		}
		GameHandler.Instance().inBattleArea += add;
		if(GameHandler.Instance().inBattleArea <= 0)
		{
			DataHolder.BattleSystem().ClearBattle(false);
			GameHandler.ClearInBattleArea();
		}
	}
	
	public static void ClearInBattleArea()
	{
		GameHandler.Instance().inBattleArea = 0;
		GameHandler.Instance().battleArea = null;
		if(DataHolder.BattleSystem().IsRealTime() && 
			DataHolder.GameSettings().onlyInBattleArea)
		{
			GameHandler.Party().DestroySpawnedParty();
		}
	}
	
	/*
	============================================================================
	Zone functions
	============================================================================
	*/
	public static void AddNoSpawn(NoEnemySpawn s)
	{
		GameHandler.Instance().noSpawns.Add(s);
	}
	
	public static bool WithinNoSpawn(Vector3 point)
	{
		bool within = false;
		foreach(NoEnemySpawn s in GameHandler.Instance().noSpawns)
		{
			if(s.IsWithin(point))
			{
				within = true;
				break;
			}
		}
		return within;
	}
	
	public static void ClearNoSpawns()
	{
		GameHandler.Instance().noSpawns = new ArrayList();
	}
	
	public static void AddNoClickMove(NoClickMove s)
	{
		GameHandler.Instance().noClickMove.Add(s);
	}
	
	public static bool WithinNoClickMove(Vector3 point)
	{
		bool within = false;
		foreach(NoClickMove s in GameHandler.Instance().noClickMove)
		{
			if(s.IsWithin(point))
			{
				within = true;
				break;
			}
		}
		return within;
	}
	
	public static void ClearNoClickMoves()
	{
		GameHandler.Instance().noClickMove = new ArrayList();
	}
	
	/*
	============================================================================
	General game functions
	============================================================================
	*/
	public static void GameOver()
	{
		if("" != DataHolder.GameSettings().gameOver.scene)
		{
			GameHandler.GetLevelHandler().interactionList = new ArrayList();
			GameHandler.Party().DestroyInstances();
			Application.LoadLevel(DataHolder.GameSettings().gameOver.scene);
			DataHolder.Instance();
			DataHolder.GameSettings().LoadResources();
			DataHolder.LoadSaveHUD().LoadResources();
			DataHolder.MainMenu().LoadResources();
			DataHolder.BattleSystemData().LoadResources();
			GameHandler.Instance();
			GameHandler.ChangeHappened(2, 0, 0);
		}
		GameHandler.GetLevelHandler().CallGameOverChoice();
	}
	
	public static void MainMenu()
	{
		if("" != DataHolder.MainMenu().mainMenuScene)
		{
			GameHandler.GetLevelHandler().ClearGlobalEvents();
			GameHandler.GetLevelHandler().interactionList = new ArrayList();
			GameHandler.Party().DestroyInstances();
			GameHandler.SetControlType(ControlType.NONE);
			Application.LoadLevel(DataHolder.MainMenu().mainMenuScene);
			DataHolder.Instance();
			DataHolder.GameSettings().LoadResources();
			DataHolder.LoadSaveHUD().LoadResources();
			DataHolder.MainMenu().LoadResources();
			DataHolder.BattleSystemData().LoadResources();
			GameHandler.Instance();
			GameHandler.ChangeHappened(2, 0, 0);
		}
	}
	
	public static void LoadScene(string sceneName)
	{
		GameHandler.GetLevelHandler().interactionList = new ArrayList();
		GameHandler.Party().DestroyInstances();
		Application.LoadLevel(sceneName);
	}
	
	/*
	============================================================================
	Language functions
	============================================================================
	*/
	public static int GetLanguage()
	{
		return GameHandler.Instance().langID;
	}
	
	public static void SetLanguage(int id)
	{
		GameHandler.Instance().langID = id;
	}
	
	/*
	============================================================================
	Difficulty functions
	============================================================================
	*/
	public static int GetDifficulty()
	{
		return GameHandler.Instance().difficultyID;
	}
	
	public static void SetDifficulty(int id)
	{
		GameHandler.Instance().difficultyID = id;
	}
	
	/*
	============================================================================
	Game control functions
	============================================================================
	*/
	public static ControlType GetControlType()
	{
		return GameHandler.Instance().currentControl;
	}
	
	public static void SetControlType(ControlType ct)
	{
		GameHandler.Instance().currentControl = ct;
	}
	
	public static bool IsControlType(ControlType ct)
	{
		return GameHandler.Instance().currentControl.Equals(ct);
	}
	
	public static bool IsControlNone()
	{
		return ControlType.NONE.Equals(GameHandler.Instance().currentControl);
	}
	
	public static bool IsControlField()
	{
		return ControlType.FIELD.Equals(GameHandler.Instance().currentControl);
	}
	
	public static bool IsControlBattle()
	{
		return ControlType.BATTLE.Equals(GameHandler.Instance().currentControl) ||
			(GameHandler.IsControlField() && GameHandler.IsInBattleArea());
	}
	
	public static bool IsControlEvent()
	{
		return ControlType.EVENT.Equals(GameHandler.Instance().currentControl);
	}
	
	public static bool IsControlMenu()
	{
		return ControlType.MENU.Equals(GameHandler.Instance().currentControl);
	}
	
	public static bool IsControlSave()
	{
		return ControlType.SAVE.Equals(GameHandler.Instance().currentControl);
	}
	
	/*
	============================================================================
	Block control functions
	============================================================================
	*/
	public static bool IsBlockControl()
	{
		return GameHandler.Instance().blockControl > 0;
	}
	
	public static void SetBlockControl(int add)
	{
		GameHandler.Instance().blockControl += add;
		if(GameHandler.Instance().blockControl < 0)
		{
			GameHandler.Instance().blockControl = 0;
		}
	}
	
	public static void RestoreControl()
	{
		GameHandler.Instance().blockControl = 0;
	}
	
	/*
	============================================================================
	Area name functions
	============================================================================
	*/
	public static string GetAreaName()
	{
		return DataHolder.AreaName(GameHandler.Instance().areaName);
	}
	
	public static int GetAreaNameID()
	{
		return GameHandler.Instance().areaName;
	}
	
	public static void SetAreaNameID(int id)
	{
		GameHandler.Instance().areaName = id;
	}
	
	public static void SetAreaName(int id)
	{
		int tmp = GameHandler.Instance().areaName;
		GameHandler.Instance().areaName = id;
		if(id != tmp)
		{
			GameSettingsData gs = DataHolder.GameSettings();
			if(gs.showAreaNames)
			{
				GameHandler.GetLevelHandler().ShowInfo(DataHolder.AreaName(id), gs.areaNamePosition, gs.areaNameVisibleTime);
			}
		}
	}
	
	public static void SetAreaName(int id, DialoguePosition dp)
	{
		int tmp = GameHandler.Instance().areaName;
		GameHandler.Instance().areaName = id;
		if(id != tmp)
		{
			GameSettingsData gs = DataHolder.GameSettings();
			if(gs.showAreaNames)
			{
				GameHandler.GetLevelHandler().ShowInfo(dp, gs.areaNameVisibleTime);
			}
		}
	}
	
	/*
	============================================================================
	Party functions
	============================================================================
	*/
	public static PartyHandler Party()
	{
		return GameHandler.Instance().partyHandler;
	}
	
	public static GameObject GetPlayer()
	{
		return GameHandler.Instance().partyHandler.GetPlayer();
	}
	
	public static void SpawnPlayer(int spawnID)
	{
		GameHandler.Instance().partyHandler.SpawnPlayer(spawnID);
	}
	
	/*
	============================================================================
	Hashtable functions
	============================================================================
	*/
	private static int GetCount(int id , Hashtable ht)
	{
		int count = 0;
		if(ht.ContainsKey(id))
		{
			count = (int)ht[id];
		}
		return count;
	}
	
	public static int AddCount(int id, int n, Hashtable ht)
	{
		int count = GameHandler.GetCount(id, ht);
		if(ht.ContainsKey(id))
		{
			ht.Remove(id);
		}
		count += n;
		ht.Add(id, count);
		return count;
	}
	
	public static int RemoveCount(int id, int n, Hashtable ht)
	{
		int count = GameHandler.GetCount(id, ht);
		if(ht.ContainsKey(id))
		{
			ht.Remove(id);
		}
		count -= n;
		if(count > 0)
		{
			ht.Add(id, count);
		}
		return count;
	}
	
	/*
	============================================================================
	Inventory functions
	============================================================================
	*/
	public static void SellFromInventory(ItemDropType type, int itemID, int quantity, int price)
	{
		GameHandler.AddMoney(quantity*price);
		GameHandler.RemoveFromInventory(type, itemID, quantity);
	}
	
	public static void BuyToInventory(ItemDropType type, int itemID, int quantity, int price)
	{
		GameHandler.SubMoney(quantity*price);
		GameHandler.AddToInventory(type, itemID, quantity);
	}
	
	public static void AddToInventory(ItemDropType type, int itemID, int quantity)
	{
		if(ItemDropType.ITEM.Equals(type))
		{
			GameHandler.AddItem(itemID, quantity);
		}
		else if(ItemDropType.WEAPON.Equals(type))
		{
			GameHandler.AddWeapon(itemID, quantity);
		}
		else if(ItemDropType.ARMOR.Equals(type))
		{
			GameHandler.AddArmor(itemID, quantity);
		}
	}
	
	public static void Drop(Vector3 position, ItemDropType type, int itemID, int quantity)
	{
		GameHandler.DropHandler().Drop(position, type, itemID, quantity);
	}
	
	public static void DropFromInventory(ItemDropType type, int itemID, int quantity)
	{
		GameHandler.RemoveFromInventory(type, itemID, quantity);
		GameHandler.DropHandler().Drop(type, itemID, quantity);
	}
	
	public static void RemoveFromInventory(ItemDropType type, int itemID, int quantity)
	{
		if(ItemDropType.ITEM.Equals(type))
		{
			GameHandler.RemoveItem(itemID, quantity);
		}
		else if(ItemDropType.WEAPON.Equals(type))
		{
			GameHandler.RemoveWeapon(itemID, quantity);
		}
		else if(ItemDropType.ARMOR.Equals(type))
		{
			GameHandler.RemoveArmor(itemID, quantity);
		}
	}
	
	public static bool HasInInventory(ItemDropType type, int itemID, int quantity)
	{
		bool has = false;
		if(ItemDropType.ITEM.Equals(type))
		{
			has = GameHandler.HasItem(itemID, quantity);
		}
		else if(ItemDropType.WEAPON.Equals(type))
		{
			has = GameHandler.HasWeapon(itemID, quantity);
		}
		else if(ItemDropType.ARMOR.Equals(type))
		{
			has = GameHandler.HasArmor(itemID, quantity);
		}
		return has;
	}
	
	/*
	============================================================================
	Item functions
	============================================================================
	*/
	public static Hashtable Items()
	{
		return GameHandler.Instance().items;
	}
	
	public static bool HasItem(int id, int n)
	{
		return GameHandler.GetCount(id, GameHandler.Items()) >= n;
	}
	
	public static int GetItemCount(int id)
	{
		return GameHandler.GetCount(id, GameHandler.Items());
	}
	
	public static int AddItem(int id)
	{
		return GameHandler.AddCount(id, 1, GameHandler.Items());
	}
	
	public static int AddItem(int id, int n)
	{
		return GameHandler.AddCount(id, n, GameHandler.Items());
	}
	
	public static int RemoveItem(int id)
	{
		return GameHandler.RemoveCount(id, 1, GameHandler.Items());
	}
	
	public static int RemoveItem(int id, int n)
	{
		return GameHandler.RemoveCount(id, n, GameHandler.Items());
	}
	
	public static bool HasItemType(int id)
	{
		bool has = false;
		foreach(DictionaryEntry entry in GameHandler.Instance().items)
		{
			if(DataHolder.Item((int)entry.Key).itemType == id)
			{
				has = true;
				break;
			}
		}
		return has;
	}
	
	public static Hashtable GetItemsByType(int id)
	{
		Hashtable ibt = new Hashtable();
		foreach(DictionaryEntry entry in GameHandler.Instance().items)
		{
			if(DataHolder.Item((int)entry.Key).itemType == id)
			{
				ibt.Add(entry.Key, entry.Value);
			}
		}
		return ibt;
	}
	
	public static ArrayList GetStealableItems()
	{
		ArrayList list = new ArrayList();
		foreach(DictionaryEntry entry in GameHandler.Instance().items)
		{
			if(DataHolder.Item((int)entry.Key).stealable)
			{
				list.Add((int)entry.Key);
			}
		}
		return list;
	}
	
	/*
	============================================================================
	Weapon functions
	============================================================================
	*/
	public static Hashtable Weapons()
	{
		return GameHandler.Instance().weapons;
	}
	
	public static bool HasWeapon(int id, int n)
	{
		return GameHandler.GetCount(id, GameHandler.Weapons()) >= n;
	}
	
	public static int GetWeaponCount(int id)
	{
		return GameHandler.GetCount(id, GameHandler.Weapons());
	}
	
	public static int GetEquippedWeaponCount(int id)
	{
		int count = 0;
		bool multi = DataHolder.Weapon(id).IsMulti();
		Character[] cs = GameHandler.Party().GetParty();
		for(int i=0; i<cs.Length; i++)
		{
			bool added = false;
			for(int j=0; j<cs[i].equipment.Length; j++)
			{
				if(cs[i].equipment[j].IsWeapon() && cs[i].equipment[j].equipID == id
						&& ((multi && !added) || !multi))
				{
					count++;
					added = true;
				}
			}
		}
		return count;
	}
	
	public static int AddWeapon(int id)
	{
		return GameHandler.AddCount(id, 1, GameHandler.Weapons());
	}
	
	public static int AddWeapon(int id, int n)
	{
		return GameHandler.AddCount(id, n, GameHandler.Weapons());
	}
	
	public static int RemoveWeapon(int id)
	{
		return GameHandler.RemoveCount(id, 1, GameHandler.Weapons());
	}
	
	public static int RemoveWeapon(int id, int n)
	{
		return GameHandler.RemoveCount(id, n, GameHandler.Weapons());
	}
	
	/*
	============================================================================
	Armor functions
	============================================================================
	*/
	public static Hashtable Armors()
	{
		return GameHandler.Instance().armors;
	}
	
	public static bool HasArmor(int id, int n)
	{
		return GameHandler.GetCount(id, GameHandler.Armors()) >= n;
	}
	
	public static int GetArmorCount(int id)
	{
		return GameHandler.GetCount(id, GameHandler.Armors());
	}
	
	public static int GetEquippedArmorCount(int id)
	{
		int count = 0;
		bool multi = DataHolder.Armor(id).IsMulti();
		Character[] cs = GameHandler.Party().GetParty();
		for(int i=0; i<cs.Length; i++)
		{
			bool added = false;
			for(int j=0; j<cs[i].equipment.Length; j++)
			{
				if(cs[i].equipment[j].IsArmor() && cs[i].equipment[j].equipID == id
						&& ((multi && !added) || !multi))
				{
					count++;
					added = true;
				}
			}
		}
		return count;
	}
	
	public static int AddArmor(int id)
	{
		return GameHandler.AddCount(id, 1, GameHandler.Armors());
	}
	
	public static int AddArmor(int id, int n)
	{
		return GameHandler.AddCount(id, n, GameHandler.Armors());
	}
	
	public static int RemoveArmor(int id)
	{
		return GameHandler.RemoveCount(id, 1, GameHandler.Armors());
	}
	
	public static int RemoveArmor(int id, int n)
	{
		return GameHandler.RemoveCount(id, n, GameHandler.Armors());
	}
	
	/*
	============================================================================
	Equipment functions
	============================================================================
	*/
	public static bool HasEquipmentPart(int id)
	{
		bool has = false;
		foreach(DictionaryEntry entry in GameHandler.Instance().weapons)
		{
			if(DataHolder.Weapon((int)entry.Key).equipPart[id])
			{
				has = true;
				break;
			}
		}
		if(!has)
		{
			foreach(DictionaryEntry entry in GameHandler.Instance().armors)
			{
				if(DataHolder.Armor((int)entry.Key).equipPart[id])
				{
					has = true;
					break;
				}
			}
		}
		return has;
	}
	
	public static bool HasEquipmentPart(int id, int classID)
	{
		bool has = false;
		EquipShort[] equip = GameHandler.GetEquipmentByPart(id);
		if(equip.Length > 0)
		{
			Class cl = DataHolder.Class(classID);
			for(int i=0; i<equip.Length; i++)
			{
				if(equip[i].IsWeapon() && cl.weapon[equip[i].equipID])
				{
					has = true;
					break;
				}
				else if(equip[i].IsArmor() && cl.armor[equip[i].equipID])
				{
					has = true;
					break;
				}
			}
		}
		return has;
	}
	
	public static EquipShort[] GetEquipment()
	{
		EquipShort[] es = new EquipShort[GameHandler.Instance().weapons.Count+GameHandler.Instance().armors.Count];
		int i = 0;
		foreach(DictionaryEntry entry in GameHandler.Instance().weapons)
		{
			es[i] = new EquipShort(EquipSet.WEAPON, (int)entry.Key);
			i++;
		}
		foreach(DictionaryEntry entry in GameHandler.Instance().armors)
		{
			es[i] = new EquipShort(EquipSet.ARMOR, (int)entry.Key);
			i++;
		}
		return es;
	}
	
	public static EquipShort[] GetEquipmentByPart(int id)
	{
		ArrayList wp = new ArrayList();
		int i = 0;
		foreach(DictionaryEntry entry in GameHandler.Instance().weapons)
		{
			if(DataHolder.Weapon((int)entry.Key).equipPart[id])
			{
				wp.Add((int)entry.Key);
				i++;
			}
		}
		ArrayList am = new ArrayList();
		foreach(DictionaryEntry entry in GameHandler.Instance().armors)
		{
			if(DataHolder.Armor((int)entry.Key).equipPart[id])
			{
				am.Add((int)entry.Key);
				i++;
			}
		}
		
		EquipShort[] es = new EquipShort[i];
		i = 0;
		foreach(int entry in wp)
		{
			es[i] = new EquipShort(EquipSet.WEAPON, entry);
			i++;
		}
		foreach(int entry in am)
		{
			es[i] = new EquipShort(EquipSet.ARMOR, entry);
			i++;
		}
		return es;
	}
	
	/*
	============================================================================
	Money functions
	============================================================================
	*/
	public static int GetMoney()
	{
		return GameHandler.Instance().money;
	}
	
	public static void SetMoney(int m)
	{
		GameHandler.Instance().money = m;
	}
	
	public static bool HasEnoughMoney(int m)
	{
		return GameHandler.Instance().money >= m;
	}
	
	public static int AddMoney(int m)
	{
		GameHandler.Instance().money += m;
		return GameHandler.Instance().money;
	}
	
	public static int SubMoney(int m)
	{
		GameHandler.Instance().money -= m;
		if(!GameHandler.HasEnoughMoney(0))
		{
			GameHandler.SetMoney(0);
		}
		return GameHandler.Instance().money;
	}
	
	/*
	============================================================================
	Variable functions
	============================================================================
	*/
	public static string GetVariable(string key)
	{
		return GameHandler.Instance().variables[key] as string;
	}
	
	public static void SetVariable(string key, string value)
	{
		if(GameHandler.Instance().variables.ContainsKey(key))
		{
			GameHandler.Instance().variables[key] = value;
		}
		else
		{
			GameHandler.Instance().variables.Add(key, value);
		}
	}
	
	public static void RemoveVariable(string key)
	{
		GameHandler.Instance().variables.Remove(key);
	}
	
	public static bool CheckVariable(string key, string value)
	{
		bool check = false;
		if(GameHandler.Instance().variables.ContainsKey(key) && 
			GameHandler.Instance().variables[key] as string == value)
		{
			check = true;
		}
		return check;
	}
	
	/*
	============================================================================
	Number variable functions
	============================================================================
	*/
	public static float GetNumberVariable(string key)
	{
		float value = 0;
		if(GameHandler.Instance().numberVariables.ContainsKey(key))
		{
			value = (float)GameHandler.Instance().numberVariables[key];
		}
		return value;
	}
	
	public static void SetNumberVariable(string key, float value)
	{
		if(GameHandler.Instance().numberVariables.ContainsKey(key))
		{
			GameHandler.Instance().numberVariables[key] = value;
		}
		else
		{
			GameHandler.Instance().numberVariables.Add(key, value);
		}
	}
	
	public static void RemoveNumberVariable(string key)
	{
		GameHandler.Instance().numberVariables.Remove(key);
	}
	
	public static bool CheckNumberVariable(string key, float value, ValueCheck type)
	{
		bool check = false;
		if(GameHandler.Instance().numberVariables.ContainsKey(key) &&
			((ValueCheck.EQUALS.Equals(type) && (float)GameHandler.Instance().numberVariables[key] == value) ||
			(ValueCheck.LESS.Equals(type) && (float)GameHandler.Instance().numberVariables[key] < value) ||
			(ValueCheck.GREATER.Equals(type) && (float)GameHandler.Instance().numberVariables[key] > value)))
		{
			check = true;
		}
		return check;
	}
	
	/*
	============================================================================
	Check functions
	============================================================================
	*/
	public static bool HasItems()
	{
		return GameHandler.Instance().items.Count > 0;
	}
	
	public static bool HasWeapons()
	{
		return GameHandler.Instance().weapons.Count > 0;
	}
	
	public static bool HasArmors()
	{
		return GameHandler.Instance().armors.Count > 0;
	}
	
	public static bool HasEquipment()
	{
		return GameHandler.HasWeapons() || GameHandler.HasArmors();
	}
	
	public static bool HasInventory()
	{
		return GameHandler.HasItems() || GameHandler.HasEquipment();
	}
	
	/*
	============================================================================
	Recipe functions
	============================================================================
	*/
	public static void LearnRecipe(int index)
	{
		if(!GameHandler.RecipeKnown(index))
		{
			GameHandler.Instance().recipes.Add(index);
		}
	}
	
	public static bool RecipeKnown(int index)
	{
		return GameHandler.Instance().recipes.Contains(index);
	}
	
	public static ArrayList GetKnownRecipes()
	{
		return GameHandler.Instance().recipes;
	}
	
	public static bool KnowsRecipes()
	{
		return GameHandler.Instance().recipes.Count > 0;
	}
	
	/*
	============================================================================
	Save handling functions
	============================================================================
	*/
	public static Hashtable GetItemSaveData(Hashtable ht)
	{
		foreach(DictionaryEntry entry in GameHandler.Instance().items)
		{
			ht.Add(entry.Key, entry.Value);
		}
		return ht;
	}
	
	public static Hashtable GetWeaponSaveData(Hashtable ht)
	{
		foreach(DictionaryEntry entry in GameHandler.Instance().weapons)
		{
			ht.Add(entry.Key, entry.Value);
		}
		return ht;
	}
	
	public static Hashtable GetArmorSaveData(Hashtable ht)
	{
		foreach(DictionaryEntry entry in GameHandler.Instance().armors)
		{
			ht.Add(entry.Key, entry.Value);
		}
		return ht;
	}
	
	public static Hashtable GetVariableSaveData(Hashtable ht)
	{
		foreach(DictionaryEntry entry in GameHandler.Instance().variables)
		{
			if(DataHolder.LoadSaveHUD().CanSaveGameVariable(entry.Key as string))
			{
				ht.Add(entry.Key, entry.Value);
			}
		}
		return ht;
	}
	
	public static Hashtable GetNumberVariableSaveData(Hashtable ht)
	{
		foreach(DictionaryEntry entry in GameHandler.Instance().numberVariables)
		{
			if(DataHolder.LoadSaveHUD().CanSaveNumberVariable(entry.Key as string))
			{
				ht.Add(entry.Key, entry.Value);
			}
		}
		return ht;
	}
	
	public static Hashtable GetItemRecipeSaveData(Hashtable ht)
	{
		string tmp = "";
		int i = 0;
		foreach(int id in GameHandler.Instance().recipes)
		{
			if(i == 0) tmp += id;
			else tmp += ";"+id;
			i++;
		}
		if("" != tmp)
		{
			ht.Add("recipes", tmp);
		}
		return ht; 
	}
	
	/*
	============================================================================
	Load handling functions
	============================================================================
	*/
	public static void SetItemSaveData(Hashtable ht)
	{
		GameHandler.Instance().items = new Hashtable();
		foreach(DictionaryEntry entry in ht)
		{
			if(XMLHandler.NODE_NAME != entry.Key as string)
			{
				GameHandler.Instance().items.Add(int.Parse((string)entry.Key), int.Parse((string)entry.Value));
			}
		}
	}
	
	public static void SetWeaponSaveData(Hashtable ht)
	{
		GameHandler.Instance().weapons = new Hashtable();
		foreach(DictionaryEntry entry in ht)
		{
			if(XMLHandler.NODE_NAME != entry.Key as string)
			{
				GameHandler.Instance().weapons.Add(int.Parse((string)entry.Key), int.Parse((string)entry.Value));
			}
		}
	}
	
	public static void SetArmorSaveData(Hashtable ht)
	{
		GameHandler.Instance().armors = new Hashtable();
		foreach(DictionaryEntry entry in ht)
		{
			if(XMLHandler.NODE_NAME != entry.Key as string)
			{
				GameHandler.Instance().armors.Add(int.Parse((string)entry.Key), int.Parse((string)entry.Value));
			}
		}
	}
	
	public static void SetVariableSaveData(Hashtable ht)
	{
		GameHandler.Instance().variables = new Hashtable();
		foreach(DictionaryEntry entry in ht)
		{
			if(XMLHandler.NODE_NAME != entry.Key as string)
			{
				GameHandler.Instance().variables.Add(entry.Key, entry.Value);
			}
		}
	}
	
	public static void SetNumberVariableSaveData(Hashtable ht)
	{
		GameHandler.Instance().numberVariables = new Hashtable();
		foreach(DictionaryEntry entry in ht)
		{
			if(XMLHandler.NODE_NAME != entry.Key as string)
			{
				GameHandler.Instance().numberVariables.Add(entry.Key, float.Parse((string)entry.Value));
			}
		}
	}
	
	public static void SetItemRecipeSaveData(Hashtable ht)
	{
		if(ht.ContainsKey("recipes"))
		{
			string tmp = (string)ht["recipes"];
			string[] id = tmp.Split(new char[] {';'});
			for(int i=0; i<id.Length; i++)
			{
				GameHandler.LearnRecipe(int.Parse(id[i]));
			}
		}
	}
}