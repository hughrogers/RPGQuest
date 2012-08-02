
using System.Collections;
using UnityEngine;

public class BattleSystemData
{
	public BattleSystem system;
	public BattleMenu menu;
	public BattleEnd end;
	public BattleCam cam;
	public BattleControl control;
	
	// info text settings
	public bool showInfo = false;
	public int infoPosition = 0;
	public float infoShowTime = 3;
	public bool showSkills = true;
	public bool showItems = true;
	public bool showDefend = true;
	public bool showEscape = true;
	public bool showCounter = true;
	public bool showStealItem = true;
	public bool showStealItemFail = true;
	public bool showStealMoney = true;
	public bool showStealMoneyFail = true;
	
	// battle text settings
	public bool showUserDamage = false;
	public bool mountTexts = false;
	public BattleTextSettings effectTextSettings;
	public BattleTextSettings missTextSettings;
	public BattleTextSettings blockTextSettings;
	public BattleTextSettings castCancelTextSettings;
	public BattleTextSettings levelUpTextSettings;
	public BattleTextSettings classLevelUpTextSettings;
	
	public ArrayList counterText = new ArrayList();
	public ArrayList allAlliesText = new ArrayList();
	public ArrayList allEnemiesText = new ArrayList();
	public ArrayList stealItemText = new ArrayList();
	public ArrayList stealItemFailText = new ArrayList();
	public ArrayList stealMoneyText = new ArrayList();
	public ArrayList stealMoneyFailText = new ArrayList();
	
	public string textSkinName = "";
	public GUISkin textSkin;
	
	public string skinPath = "HUD/";
	
	// battle messages
	public bool showBattleMessage = false;
	public int battleMessagePosition = 0;
	public float battleMessageShowTime = 3;
	
	public int battleStartColor = 0;
	public int battleStartSColor = 1;
	public ArrayList battleStartText = new ArrayList();
	
	public int battleVictoryColor = 0;
	public int battleVictorySColor = 1;
	public ArrayList battleVictoryText = new ArrayList();
	
	public int battleDefeatColor = 0;
	public int battleDefeatSColor = 1;
	public ArrayList battleDefeatText = new ArrayList();
	
	public int battleEscapeColor = 0;
	public int battleEscapeSColor = 1;
	public ArrayList battleEscapeText = new ArrayList();
	
	public static string PREFAB_PATH = "Prefabs/BattleSystem/";
	public static string ICON_PATH = "Icons/BattleSystem/";
	public static string AUDIO_PATH = "Audio/BattleSystem/";
	
	// XML data
	private string dir = "ProjectSettings/";
	private string filename = "battleSystem";
	
	private static string BATTLESYSTEM = "battlesystem";
	private static string SYSTEM = "system";
	private static string BONUS = "bonus";
	private static string REVIVE = "revive";
	private static string STARTSTATUS = "startstatus";
	private static string ENDSTATUS = "endstatus";
	private static string PARTYADVANTAGE = "partyadvantage";
	private static string ENEMIESADVANTAGE = "enemiesadvantage";
	private static string PARTYSPOT = "partyspot";
	private static string ENEMYSPOT = "enemyspot";
	
	private static string MENU = "menu";
	private static string BACK = "back";
	private static string BACKICON = "backicon";
	private static string ATTACK = "attack";
	private static string ATTACKICON = "attackicon";
	public static string SKILL = "skill";
	private static string SKILLICON = "skillicon";
	public static string ITEM = "item";
	private static string ITEMICON = "itemicon";
	private static string DEFEND = "defend";
	private static string DEFENDICON = "defendicon";
	private static string ESCAPE = "escape";
	private static string ESCAPEICON = "escapeicon";
	private static string ENDTURN = "endturn";
	private static string ENDTURNICON = "endturnicon";
	private static string TARGET = "target";
	private static string CURSOR = "cursor";
	private static string CURSORCHILD = "cursorchild";
	
	private static string TEXT = "text";
	private static string INFO = "info";
	private static string DAMAGE = "damage";
	private static string REFRESH = "refresh";
	private static string EFFECT = "effect";
	private static string MISS = "miss";
	private static string COUNTER = "counter";
	private static string ALLIES = "allies";
	private static string ENEMIES = "enemies";
	private static string BATTLESTART = "battlestart";
	private static string BATTLEVICTORY = "battlevictory";
	private static string BATTLEDEFEAT = "battledefeat";
	private static string BATTLEESCAPE = "battleescape";
	private static string MISSSETTINGS = "misssettings";
	private static string BLOCKSETTINGS = "blocksettings";
	private static string SKIN = "skin";
	private static string CASTCANCEL = "castcancel";
	private static string CASTCANCELSETTINGS = "castcancelsettings";
	private static string LEVELUPSETTINGS = "levelupsettings";
	private static string CLASSLEVELUPSETTINGS = "classlevelupsettings";
	private static string STEALITEM = "stealitem";
	private static string STEALITEMFAIL = "stealitemfail";
	private static string STEALMONEY = "stealmoney";
	private static string STEALMONEYFAIL = "stealmoneyfail";
	
	private static string BATTLEEND = "battleend";
	public static string LEVELUP = "levelup";
	public static string CLASSLEVELUP = "classlevelup";
	public static string MONEY = "money";
	public static string STATUS = "status";
	public static string EXPERIENCE = "experience";
	
	private static string BATTLECAM = "battlecam";
	private static string BATTLECONTROL = "battlecontrol";

	public BattleSystemData()
	{
		LoadData();
	}
	
	public void LoadData()
	{
		ArrayList data = XMLHandler.LoadXML(dir+filename);
		
		this.system = new BattleSystem();
		this.menu = new BattleMenu();
		this.end = new BattleEnd();
		this.cam = new BattleCam();
		this.control = new BattleControl();
		
		this.effectTextSettings = new BattleTextSettings();
		this.missTextSettings = new BattleTextSettings("Miss");
		this.blockTextSettings = new BattleTextSettings("Block");
		this.castCancelTextSettings = new BattleTextSettings("Cast canceled");
		this.levelUpTextSettings = new BattleTextSettings("Level up!");
		this.classLevelUpTextSettings = new BattleTextSettings("Class level up!");
		this.counterText = new ArrayList();
		this.allAlliesText = new ArrayList();
		this.allEnemiesText = new ArrayList();
		this.stealItemText = new ArrayList();
		this.stealItemFailText = new ArrayList();
		this.stealMoneyText = new ArrayList();
		this.stealMoneyFailText = new ArrayList();
		this.battleStartText = new ArrayList();
		this.battleVictoryText = new ArrayList();
		this.battleDefeatText = new ArrayList();
		this.battleEscapeText = new ArrayList();
		
		for(int i=0; i<DataHolder.Languages().GetDataCount(); i++)
		{
			this.counterText.Add("Counter attack");
			this.allAlliesText.Add("All allies");
			this.allEnemiesText.Add("All enemies");
			this.stealItemText.Add("%n stole %");
			this.stealItemFailText.Add("Steal failed");
			this.stealMoneyText.Add("%n stole %");
			this.stealMoneyFailText.Add("Steal failed");
			this.battleStartText.Add("Fight!");
			this.battleVictoryText.Add("Victory");
			this.battleDefeatText.Add("You are defeated");
			this.battleEscapeText.Add("You escaped");
		}
		
		if(data.Count > 0)
		{
			foreach(Hashtable entry in data)
			{
				if(entry[XMLHandler.NODE_NAME] as string == BattleSystemData.BATTLESYSTEM)
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList subs = entry[XMLHandler.NODES] as ArrayList;
						foreach(Hashtable val in subs)
						{
							if(val[XMLHandler.NODE_NAME] as string == BattleSystemData.SYSTEM)
							{
								this.system.type = (BattleSystemType)System.Enum.Parse(
										typeof(BattleSystemType), (string)val["type"]);
								if(val.ContainsKey("enemycounting"))
								{
									this.system.enemyCounting = (EnemyCounting)System.Enum.Parse(
											typeof(EnemyCounting), (string)val["enemycounting"]);
								}
								this.system.turnCalc = int.Parse((string)val["turncalc"]);
								if(val.ContainsKey("starttimecalc")) this.system.startTimeCalc = int.Parse((string)val["starttimecalc"]);
								this.system.defendFormula = int.Parse((string)val["defendformula"]);
								this.system.escapeFormula = int.Parse((string)val["escapeformula"]);
								this.system.actionBorder = int.Parse((string)val["actionborder"]);
								if(val.ContainsKey("menuborder"))
								{
									this.system.menuBorder = int.Parse((string)val["menuborder"]);
								}
								else this.system.menuBorder = this.system.actionBorder;
								if(val.ContainsKey("maxtimebar"))
								{
									this.system.maxTimebar = int.Parse((string)val["maxtimebar"]);
								}
								else this.system.maxTimebar = this.system.actionBorder;
								
								if(val.ContainsKey("enablemultichoice")) this.system.enableMultiChoice = true;
								if(val.ContainsKey("useallactions")) this.system.useAllActions = true;
								
								if(val.ContainsKey("usetimebaraction"))
								{
									this.system.useTimebarAction = (UseTimebarAction)System.Enum.Parse(
											typeof(UseTimebarAction), (string)val["usetimebaraction"]);
								}
								
								this.system.activeCommand = bool.Parse((string)val["activecommand"]);
								this.system.actionPause = bool.Parse((string)val["actionpause"]);
								if(val.ContainsKey("dynamiccombat"))
								{
									this.system.dynamicCombat = true;
									if(val.ContainsKey("mintimebetween"))
									{
										this.system.minTimeBetween = float.Parse((string)val["mintimebetween"]);
									}
								}
								if(val.ContainsKey("playdamageanim")) this.system.playDamageAnim = true;
								if(val.ContainsKey("blockautoatkmenu")) this.system.blockAutoAttackMenu = true;
								if(val.ContainsKey("turnbonuses"))
								{
									this.system.turnBonuses = true;
									this.system.statusBonus = new int[int.Parse((string)val["turnbonuses"])];
								}
								if(val.ContainsKey("revivesets"))
								{
									this.system.reviveAfterBattle = true;
									int count = int.Parse((string)val["revivesets"]);
									this.system.reviveSetStatus = new bool[count];
									this.system.reviveStatus = new int[count];
								}
								if(val.ContainsKey("startstatuses"))
								{
									this.system.startBattleStatusSettings = true;
									int count = int.Parse((string)val["startstatuses"]);
									this.system.startSetStatus = new bool[count];
									this.system.startStatus = new int[count];
								}
								if(val.ContainsKey("endstatuses"))
								{
									this.system.endBattleStatusSettings = true;
									int count = int.Parse((string)val["endstatuses"]);
									this.system.endSetStatus = new bool[count];
									this.system.endStatus = new int[count];
								}
								if(val.ContainsKey("attacktimebaruse"))
								{
									this.system.attackEndTurn = false;
									this.system.attackTimebarUse = float.Parse((string)val["attacktimebaruse"]);
								}
								if(val.ContainsKey("itemtimebaruse"))
								{
									this.system.itemEndTurn = false;
									this.system.itemTimebarUse = float.Parse((string)val["itemtimebaruse"]);
								}
								if(val.ContainsKey("defendtimebaruse"))
								{
									this.system.defendEndTurn = false;
									this.system.defendTimebarUse = float.Parse((string)val["defendtimebaruse"]);
								}
								if(val.ContainsKey("escapetimebaruse"))
								{
									this.system.escapeEndTurn = false;
									this.system.escapeTimebarUse = float.Parse((string)val["escapetimebaruse"]);
								}
								if(val.ContainsKey("atbtickinterval"))
								{
									this.system.atbTickInterval = float.Parse((string)val["atbtickinterval"]);
								}
								if(val.ContainsKey("battlerange"))
								{
									this.system.battleRange = float.Parse((string)val["battlerange"]);
								}
								if(val.ContainsKey("airange"))
								{
									this.system.aiRange = float.Parse((string)val["airange"]);
								}
								if(val.ContainsKey("airechecktime"))
								{
									this.system.aiRecheckTime = float.Parse((string)val["airechecktime"]);
								}
								if(val.ContainsKey("blockcontrolmenu")) this.system.blockControlMenu = true;
								if(val.ContainsKey("blockcontrolaction")) this.system.blockControlAction = true;
								if(val.ContainsKey("blockmse")) this.system.blockMSE = true;
								if(val.ContainsKey("freezeaction")) this.system.freezeAction = true;
								
								if(val.ContainsKey("enemyspots"))
								{
									this.system.enemySpot = new Vector3[int.Parse((string)val["enemyspots"])];
									this.system.enemySpotPA = new Vector3[this.system.enemySpot.Length];
									this.system.enemySpotEA = new Vector3[this.system.enemySpot.Length];
								}
								if(val.ContainsKey("spotdistance"))
								{
									this.system.spotOnGround = true;
									this.system.spotDistance = float.Parse((string)val["spotdistance"]);
									this.system.spotLayer = int.Parse((string)val["spotlayer"]);
									if(val.ContainsKey("spotx"))
									{
										this.system.spotOffset = VectorHelper.FromHashtable(val, "spotx", "spoty", "spotz");
									}
								}
								if(val.ContainsKey("enablepaspots")) this.system.enablePASpots = true;
								if(val.ContainsKey("enableeaspots")) this.system.enableEASpots = true;
								
								if(val.ContainsKey(XMLHandler.NODES))
								{
									ArrayList s = val[XMLHandler.NODES] as ArrayList;
									foreach(Hashtable ht in s)
									{
										if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.BONUS)
										{
											this.system.statusBonus[int.Parse((string)ht["id"])] = int.Parse((string)ht["value"]);
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.REVIVE)
										{
											int id = int.Parse((string)ht["id"]);
											this.system.reviveSetStatus[id] = true;
											this.system.reviveStatus[id] = int.Parse((string)ht["value"]);
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.STARTSTATUS)
										{
											int id = int.Parse((string)ht["id"]);
											this.system.startSetStatus[id] = true;
											this.system.startStatus[id] = int.Parse((string)ht["value"]);
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.ENDSTATUS)
										{
											int id = int.Parse((string)ht["id"]);
											this.system.endSetStatus[id] = true;
											this.system.endStatus[id] = int.Parse((string)ht["value"]);
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.PARTYADVANTAGE)
										{
											this.system.partyAdvantage.SetData(ht);
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.ENEMIESADVANTAGE)
										{
											this.system.enemiesAdvantage.SetData(ht);
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.PARTYSPOT)
										{
											int id = int.Parse((string)ht["id"]);
											if(id < this.system.partySpot.Length)
											{
												this.system.partySpot[id] = VectorHelper.FromHashtable(ht);
												if(ht.ContainsKey("pax"))
												{
													this.system.partySpotPA[id] = VectorHelper.FromHashtable(ht, "pax", "pay", "paz");
												}
												if(ht.ContainsKey("eax"))
												{
													this.system.partySpotEA[id] = VectorHelper.FromHashtable(ht, "eax", "eay", "eaz");
												}
											}
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.ENEMYSPOT)
										{
											int id = int.Parse((string)ht["id"]);
											if(id < this.system.enemySpot.Length)
											{
												this.system.enemySpot[id] = VectorHelper.FromHashtable(ht);
												if(ht.ContainsKey("pax"))
												{
													this.system.enemySpotPA[id] = VectorHelper.FromHashtable(ht, "pax", "pay", "paz");
												}
												if(ht.ContainsKey("eax"))
												{
													this.system.enemySpotEA[id] = VectorHelper.FromHashtable(ht, "eax", "eay", "eaz");
												}
											}
										}
									}
								}
							}
							else if(val[XMLHandler.NODE_NAME] as string == BattleSystemData.MENU)
							{
								if(val.ContainsKey("dialogueposition")) this.menu.dialoguePosition = int.Parse((string)val["dialogueposition"]);
								if(val.ContainsKey("targetposition")) this.menu.targetPosition = int.Parse((string)val["targetposition"]);
								if(val.ContainsKey("skillposition")) this.menu.skillPosition = int.Parse((string)val["skillposition"]);
								if(val.ContainsKey("itemposition")) this.menu.itemPosition = int.Parse((string)val["itemposition"]);
								if(val.ContainsKey("showattack")) this.menu.showAttack = bool.Parse((string)val["showattack"]);
								if(val.ContainsKey("showskills")) this.menu.showSkills = bool.Parse((string)val["showskills"]);
								if(val.ContainsKey("combineskills")) this.menu.combineSkills = bool.Parse((string)val["combineskills"]);
								if(val.ContainsKey("showitems")) this.menu.showItems = bool.Parse((string)val["showitems"]);
								if(val.ContainsKey("combineitems")) this.menu.combineItems = bool.Parse((string)val["combineitems"]);
								if(val.ContainsKey("showdefend")) this.menu.showDefend = bool.Parse((string)val["showdefend"]);
								if(val.ContainsKey("showescape")) this.menu.showEscape = bool.Parse((string)val["showescape"]);
								if(val.ContainsKey("showendturn")) this.menu.showEndTurn = bool.Parse((string)val["showendturn"]);
								if(val.ContainsKey("attackicon")) this.menu.attackIconName = val["attackicon"] as string;
								if(val.ContainsKey("skillicon")) this.menu.skillIconName = val["skillicon"] as string;
								if(val.ContainsKey("itemicon")) this.menu.itemIconName = val["itemicon"] as string;
								if(val.ContainsKey("defendicon")) this.menu.defendIconName = val["defendicon"] as string;
								if(val.ContainsKey("escapeicon")) this.menu.escapeIconName = val["escapeicon"] as string;
								if(val.ContainsKey("drag")) this.menu.enableDrag = true;
								if(val.ContainsKey("doubleclick")) this.menu.enableDoubleClick = true;
								if(val.ContainsKey("addback")) this.menu.addBack = true;
								if(val.ContainsKey("backfirst")) this.menu.backFirst = true;
								this.menu.mouseTouch.SetData(val);
								
								if(val.ContainsKey("order")) this.menu.SetOrder(val["order"] as string);
								
								if(val.ContainsKey(XMLHandler.NODES))
								{
									ArrayList s = val[XMLHandler.NODES] as ArrayList;
									foreach(Hashtable ht in s)
									{
										if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.BACK)
										{
											int id = int.Parse((string)ht["id"]);
											if(id < this.menu.backName.Count) this.menu.backName[id] = (string)ht[XMLHandler.CONTENT];
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.ATTACK)
										{
											int id = int.Parse((string)ht["id"]);
											if(id < this.menu.attackName.Count) this.menu.attackName[id] = (string)ht[XMLHandler.CONTENT];
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.SKILL)
										{
											int id = int.Parse((string)ht["id"]);
											if(id < this.menu.skillName.Count) this.menu.skillName[id] = (string)ht[XMLHandler.CONTENT];
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.ITEM)
										{
											int id = int.Parse((string)ht["id"]);
											if(id < this.menu.itemName.Count) this.menu.itemName[id] = (string)ht[XMLHandler.CONTENT];
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.DEFEND)
										{
											int id = int.Parse((string)ht["id"]);
											if(id < this.menu.defendName.Count) this.menu.defendName[id] = (string)ht[XMLHandler.CONTENT];
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.ESCAPE)
										{
											int id = int.Parse((string)ht["id"]);
											if(id < this.menu.escapeName.Count) this.menu.escapeName[id] = (string)ht[XMLHandler.CONTENT];
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.ENDTURN)
										{
											int id = int.Parse((string)ht["id"]);
											if(id < this.menu.endTurnName.Count) this.menu.endTurnName[id] = (string)ht[XMLHandler.CONTENT];
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.BACKICON)
										{
											this.menu.backIconName = (string)ht[XMLHandler.CONTENT];
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.ATTACKICON)
										{
											this.menu.attackIconName = (string)ht[XMLHandler.CONTENT];
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.SKILLICON)
										{
											this.menu.skillIconName = (string)ht[XMLHandler.CONTENT];
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.ITEMICON)
										{
											this.menu.itemIconName = (string)ht[XMLHandler.CONTENT];
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.DEFENDICON)
										{
											this.menu.defendIconName = (string)ht[XMLHandler.CONTENT];
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.ESCAPEICON)
										{
											this.menu.escapeIconName = (string)ht[XMLHandler.CONTENT];
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.ENDTURNICON)
										{
											this.menu.endTurnIconName = (string)ht[XMLHandler.CONTENT];
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.CURSOR)
										{
											this.menu.useTargetCursor = true;
											this.menu.cursorPrefabName = ht[XMLHandler.CONTENT] as string;
											this.menu.cursorOffset.x = float.Parse((string)ht["cox"]);
											this.menu.cursorOffset.y = float.Parse((string)ht["coy"]);
											this.menu.cursorOffset.z = float.Parse((string)ht["coz"]);
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.CURSORCHILD)
										{
											this.menu.cursorChildName = ht[XMLHandler.CONTENT] as string;
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.TARGET)
										{
											this.menu.useTargetMenu = bool.Parse((string)ht["targetmenu"]);
											this.menu.useTargetBlink = bool.Parse((string)ht["targetblink"]);
											if(this.menu.useTargetBlink)
											{
												if(ht.ContainsKey("fromcurrent")) this.menu.fromCurrent = true;
												this.menu.blinkChildren = bool.Parse((string)ht["children"]);
												this.menu.blinkTime = float.Parse((string)ht["time"]);
												this.menu.blinkInterpolation = (EaseType)System.Enum.Parse(typeof(EaseType), (string)ht["interpolation"]);
												if(ht.ContainsKey("as"))
												{
													this.menu.aBlink = true;
													this.menu.aStart = float.Parse((string)ht["as"]);
													this.menu.aEnd = float.Parse((string)ht["ae"]);
												}
												if(ht.ContainsKey("rs"))
												{
													this.menu.rBlink = true;
													this.menu.rStart = float.Parse((string)ht["rs"]);
													this.menu.rEnd = float.Parse((string)ht["re"]);
												}
												if(ht.ContainsKey("gs"))
												{
													this.menu.gBlink = true;
													this.menu.gStart = float.Parse((string)ht["gs"]);
													this.menu.gEnd = float.Parse((string)ht["ge"]);
												}
												if(ht.ContainsKey("bs"))
												{
													this.menu.bBlink = true;
													this.menu.bStart = float.Parse((string)ht["bs"]);
													this.menu.bEnd = float.Parse((string)ht["be"]);
												}
											}
										}
									}
								}
							}
							else if(val[XMLHandler.NODE_NAME] as string == BattleSystemData.TEXT)
							{
								this.showUserDamage = bool.Parse((string)val["showuserdamage"]);
								if(val.ContainsKey("mounttexts")) this.mountTexts = true;
								
								if(val.ContainsKey("bmpos"))
								{
									this.showBattleMessage = true;
									this.battleMessagePosition = int.Parse((string)val["bmpos"]);
									this.battleMessageShowTime = float.Parse((string)val["bmtime"]);
								}
								
								if(val.ContainsKey("bscolor"))
								{
									this.battleStartColor = int.Parse((string)val["bscolor"]);
									this.battleStartSColor = int.Parse((string)val["bsshadowcolor"]);
								}
								if(val.ContainsKey("bvcolor"))
								{
									this.battleVictoryColor = int.Parse((string)val["bvcolor"]);
									this.battleVictorySColor = int.Parse((string)val["bvshadowcolor"]);
								}
								if(val.ContainsKey("bdcolor"))
								{
									this.battleDefeatColor = int.Parse((string)val["bdcolor"]);
									this.battleDefeatSColor = int.Parse((string)val["bdshadowcolor"]);
								}
								if(val.ContainsKey("becolor"))
								{
									this.battleEscapeColor = int.Parse((string)val["becolor"]);
									this.battleEscapeSColor = int.Parse((string)val["beshadowcolor"]);
								}
								
								if(val.ContainsKey(XMLHandler.NODES))
								{
									ArrayList s = val[XMLHandler.NODES] as ArrayList;
									foreach(Hashtable ht in s)
									{
										if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.INFO)
										{
											this.showInfo = true;
											this.infoPosition = int.Parse((string)ht["position"]);
											this.infoShowTime = float.Parse((string)ht["time"]);
											if(ht.ContainsKey("showskills")) this.showSkills = bool.Parse((string)ht["showskills"]);
											if(ht.ContainsKey("showitems")) this.showItems = bool.Parse((string)ht["showitems"]);
											if(ht.ContainsKey("showdefend")) this.showDefend = bool.Parse((string)ht["showdefend"]);
											if(ht.ContainsKey("showescape")) this.showEscape = bool.Parse((string)ht["showescape"]);
											if(ht.ContainsKey("showcounter")) this.showCounter = bool.Parse((string)ht["showcounter"]);
											if(ht.ContainsKey("showstealitem")) this.showStealItem = bool.Parse((string)ht["showstealitem"]);
											if(ht.ContainsKey("showstealitemfail")) this.showStealItemFail = bool.Parse((string)ht["showstealitemfail"]);
											if(ht.ContainsKey("showstealmoney")) this.showStealMoney = bool.Parse((string)ht["showstealmoney"]);
											if(ht.ContainsKey("showstealmoneyfail")) this.showStealMoneyFail = bool.Parse((string)ht["showstealmoneyfail"]);
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.DAMAGE)
										{
											DataHolder.StatusValues().AddDamageTextSettings(int.Parse((string)ht["id"]), ht);
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.REFRESH)
										{
											DataHolder.StatusValues().AddRefreshTextSettings(int.Parse((string)ht["id"]), ht);
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.EFFECT)
										{
											this.effectTextSettings.SetData(ht);
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.MISSSETTINGS)
										{
											this.missTextSettings.SetData(ht);
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.MISS)
										{
											int id = int.Parse((string)ht["id"]);
											if(id < this.missTextSettings.text.Length)
											{
												this.missTextSettings.text[id] = ht[XMLHandler.CONTENT] as string;
											}
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.COUNTER)
										{
											int id = int.Parse((string)ht["id"]);
											if(id < this.counterText.Count) this.counterText[id] = ht[XMLHandler.CONTENT];
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.ALLIES)
										{
											int id = int.Parse((string)ht["id"]);
											if(id < this.allAlliesText.Count) this.allAlliesText[id] = ht[XMLHandler.CONTENT];
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.ENEMIES)
										{
											int id = int.Parse((string)ht["id"]);
											if(id < this.allEnemiesText.Count) this.allEnemiesText[id] = ht[XMLHandler.CONTENT];
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.BATTLESTART)
										{
											int id = int.Parse((string)ht["id"]);
											if(id < this.battleStartText.Count) this.battleStartText[id] = ht[XMLHandler.CONTENT];
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.BATTLEVICTORY)
										{
											int id = int.Parse((string)ht["id"]);
											if(id < this.battleVictoryText.Count) this.battleVictoryText[id] = ht[XMLHandler.CONTENT];
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.BATTLEDEFEAT)
										{
											int id = int.Parse((string)ht["id"]);
											if(id < this.battleDefeatText.Count) this.battleDefeatText[id] = ht[XMLHandler.CONTENT];
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.BATTLEESCAPE)
										{
											int id = int.Parse((string)ht["id"]);
											if(id < this.battleEscapeText.Count) this.battleEscapeText[id] = ht[XMLHandler.CONTENT];
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.SKIN)
										{
											this.textSkinName = ht[XMLHandler.CONTENT] as string;
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.CASTCANCEL)
										{
											int id = int.Parse((string)ht["id"]);
											if(id < this.castCancelTextSettings.text.Length)
											{
												this.castCancelTextSettings.text[id] = ht[XMLHandler.CONTENT] as string;
											}
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.LEVELUP)
										{
											int id = int.Parse((string)ht["id"]);
											if(id < this.levelUpTextSettings.text.Length)
											{
												this.levelUpTextSettings.text[id] = ht[XMLHandler.CONTENT] as string;
											}
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.CASTCANCELSETTINGS)
										{
											this.castCancelTextSettings.SetData(ht);
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.LEVELUPSETTINGS)
										{
											this.levelUpTextSettings.SetData(ht);
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.CLASSLEVELUPSETTINGS)
										{
											this.classLevelUpTextSettings.SetData(ht);
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.BLOCKSETTINGS)
										{
											this.blockTextSettings.SetData(ht);
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.STEALITEM)
										{
											int id = int.Parse((string)ht["id"]);
											if(id < this.stealItemText.Count) this.stealItemText[id] = ht[XMLHandler.CONTENT];
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.STEALITEMFAIL)
										{
											int id = int.Parse((string)ht["id"]);
											if(id < this.stealItemFailText.Count) this.stealItemFailText[id] = ht[XMLHandler.CONTENT];
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.STEALMONEY)
										{
											int id = int.Parse((string)ht["id"]);
											if(id < this.stealMoneyText.Count) this.stealMoneyText[id] = ht[XMLHandler.CONTENT];
										}
										else if(ht[XMLHandler.NODE_NAME] as string == BattleSystemData.STEALMONEYFAIL)
										{
											int id = int.Parse((string)ht["id"]);
											if(id < this.stealMoneyFailText.Count) this.stealMoneyFailText[id] = ht[XMLHandler.CONTENT];
										}
									}
								}
							}
							else if(val[XMLHandler.NODE_NAME] as string == BattleSystemData.BATTLEEND)
							{
								this.end.SetData(val);
							}
							else if(val[XMLHandler.NODE_NAME] as string == BattleSystemData.BATTLECAM)
							{
								this.cam.SetData(val);
							}
							else if(val[XMLHandler.NODE_NAME] as string == BattleSystemData.BATTLECONTROL)
							{
								this.control.SetData(val);
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
		
		sv.Add(XMLHandler.NODE_NAME, BattleSystemData.BATTLESYSTEM);
		ArrayList s = new ArrayList();
		
		// system
		Hashtable ht = new Hashtable();
		ht.Add(XMLHandler.NODE_NAME, BattleSystemData.SYSTEM);
		ht.Add("type", this.system.type.ToString());
		ht.Add("enemycounting", this.system.enemyCounting.ToString());
		ht.Add("turncalc", this.system.turnCalc.ToString());
		ht.Add("starttimecalc", this.system.startTimeCalc.ToString());
		ht.Add("defendformula", this.system.defendFormula.ToString());
		ht.Add("escapeformula", this.system.escapeFormula.ToString());
		ht.Add("activecommand", this.system.activeCommand.ToString());
		ht.Add("menuborder", this.system.menuBorder.ToString());
		ht.Add("actionborder", this.system.actionBorder.ToString());
		ht.Add("maxtimebar", this.system.maxTimebar.ToString());
		if(this.system.enableMultiChoice)
		{
			ht.Add("enablemultichoice", "true");
			if(this.system.useAllActions) ht.Add("useallactions", "true");
			ht.Add("usetimebaraction", this.system.useTimebarAction.ToString());
		}
		ht.Add("actionpause", this.system.actionPause.ToString());
		ht.Add("atbtickinterval", this.system.atbTickInterval.ToString());
		if(this.system.blockControlMenu) ht.Add("blockcontrolmenu", "true");
		if(this.system.blockControlAction) ht.Add("blockcontrolaction", "true");
		if(this.system.blockMSE) ht.Add("blockmse", "true");
		if(this.system.freezeAction) ht.Add("freezeaction", "true");
		if(this.system.dynamicCombat)
		{
			ht.Add("dynamiccombat", "true");
			if(this.system.minTimeBetween > 0)
			{
				ht.Add("mintimebetween", this.system.minTimeBetween.ToString());
			}
		}
		if(this.system.playDamageAnim) ht.Add("playdamageanim", "true");
		if(this.system.blockAutoAttackMenu) ht.Add("blockautoatkmenu", "true");
		if(!this.system.attackEndTurn)
		{
			ht.Add("attacktimebaruse", this.system.attackTimebarUse.ToString());
		}
		if(!this.system.itemEndTurn)
		{
			ht.Add("itemtimebaruse", this.system.itemTimebarUse.ToString());
		}
		if(!this.system.defendEndTurn)
		{
			ht.Add("defendtimebaruse", this.system.defendTimebarUse.ToString());
		}
		if(!this.system.escapeEndTurn)
		{
			ht.Add("escapetimebaruse", this.system.escapeTimebarUse.ToString());
		}
		if(this.system.turnBonuses)
		{
			ht.Add("turnbonuses", this.system.statusBonus.Length.ToString());
			for(int i=0; i<this.system.statusBonus.Length; i++)
			{
				if(this.system.statusBonus[i] != 0)
				{
					Hashtable ht2 = new Hashtable();
					ht2.Add(XMLHandler.NODE_NAME, BattleSystemData.BONUS);
					ht2.Add("id", i.ToString());
					ht2.Add("value", this.system.statusBonus[i].ToString());
					s.Add(ht2);
				}
			}
		}
		if(this.system.reviveAfterBattle)
		{
			ht.Add("revivesets", this.system.reviveSetStatus.Length.ToString());
			for(int i=0; i<this.system.reviveSetStatus.Length; i++)
			{
				if(this.system.reviveSetStatus[i])
				{
					Hashtable ht2 = new Hashtable();
					ht2.Add(XMLHandler.NODE_NAME, BattleSystemData.REVIVE);
					ht2.Add("id", i.ToString());
					ht2.Add("value", this.system.reviveStatus[i].ToString());
					s.Add(ht2);
				}
			}
		}
		if(this.system.startBattleStatusSettings)
		{
			ht.Add("startstatuses", this.system.startSetStatus.Length.ToString());
			for(int i=0; i<this.system.startSetStatus.Length; i++)
			{
				if(this.system.startSetStatus[i])
				{
					Hashtable ht2 = new Hashtable();
					ht2.Add(XMLHandler.NODE_NAME, BattleSystemData.STARTSTATUS);
					ht2.Add("id", i.ToString());
					ht2.Add("value", this.system.startStatus[i].ToString());
					s.Add(ht2);
				}
			}
		}
		if(this.system.endBattleStatusSettings)
		{
			ht.Add("endstatuses", this.system.endSetStatus.Length.ToString());
			for(int i=0; i<this.system.endSetStatus.Length; i++)
			{
				if(this.system.endSetStatus[i])
				{
					Hashtable ht2 = new Hashtable();
					ht2.Add(XMLHandler.NODE_NAME, BattleSystemData.ENDSTATUS);
					ht2.Add("id", i.ToString());
					ht2.Add("value", this.system.endStatus[i].ToString());
					s.Add(ht2);
				}
			}
		}
		if(this.system.IsRealTime())
		{
			ht.Add("battlerange", this.system.battleRange.ToString());
			ht.Add("airange", this.system.aiRange.ToString());
			ht.Add("airechecktime", this.system.aiRecheckTime.ToString());
		}
		else
		{
			if(this.system.spotOnGround)
			{
				ht.Add("spotdistance", this.system.spotDistance.ToString());
				ht.Add("spotlayer", this.system.spotLayer.ToString());
				if(this.system.spotOffset != Vector3.zero)
				{
					VectorHelper.ToHashtable(ref ht, this.system.spotOffset, "spotx", "spoty", "spotz");
				}
			}
			if(this.system.enablePASpots) ht.Add("enablepaspots", "true");
			if(this.system.enableEASpots) ht.Add("enableeaspots", "true");
			for(int i=0; i<this.system.partySpot.Length; i++)
			{
				Hashtable ht2 = HashtableHelper.GetTitleHashtable(BattleSystemData.PARTYSPOT, i);
				VectorHelper.ToHashtable(ref ht2, this.system.partySpot[i]);
				if(this.system.enablePASpots) VectorHelper.ToHashtable(ref ht2, this.system.partySpotPA[i], "pax", "pay", "paz");
				if(this.system.enableEASpots) VectorHelper.ToHashtable(ref ht2, this.system.partySpotEA[i], "eax", "eay", "eaz");
				s.Add(ht2);
			}
			ht.Add("enemyspots", this.system.enemySpot.Length.ToString());
			for(int i=0; i<this.system.enemySpot.Length; i++)
			{
				Hashtable ht2 = HashtableHelper.GetTitleHashtable(BattleSystemData.ENEMYSPOT, i);
				VectorHelper.ToHashtable(ref ht2, this.system.enemySpot[i]);
				if(this.system.enablePASpots) VectorHelper.ToHashtable(ref ht2, this.system.enemySpotPA[i], "pax", "pay", "paz");
				if(this.system.enableEASpots) VectorHelper.ToHashtable(ref ht2, this.system.enemySpotEA[i], "eax", "eay", "eaz");
				s.Add(ht2);
			}
			s.Add(this.system.partyAdvantage.GetData(
					HashtableHelper.GetTitleHashtable(BattleSystemData.PARTYADVANTAGE)));
			s.Add(this.system.enemiesAdvantage.GetData(
					HashtableHelper.GetTitleHashtable(BattleSystemData.ENEMIESADVANTAGE)));
		}
		
		if(s.Count > 0)
		{
			ht.Add(XMLHandler.NODES, s);
		}
		subs.Add(ht);
		
		// menu
		s = new ArrayList();
		ht = new Hashtable();
		ht.Add(XMLHandler.NODE_NAME, BattleSystemData.MENU);
		ht.Add("dialogueposition", this.menu.dialoguePosition.ToString());
		ht.Add("targetposition", this.menu.targetPosition.ToString());
		ht.Add("skillposition", this.menu.skillPosition.ToString());
		ht.Add("itemposition", this.menu.itemPosition.ToString());
		ht.Add("showattack", this.menu.showAttack.ToString());
		ht.Add("showskills", this.menu.showSkills.ToString());
		ht.Add("combineskills", this.menu.combineSkills.ToString());
		ht.Add("showitems", this.menu.showItems.ToString());
		ht.Add("combineitems", this.menu.combineItems.ToString());
		ht.Add("showdefend", this.menu.showDefend.ToString());
		ht.Add("showescape", this.menu.showEscape.ToString());
		ht.Add("showendturn", this.menu.showEndTurn.ToString());
		ht.Add("order", this.menu.GetOrder());
		if(this.menu.enableDrag) ht.Add("drag", "true");
		if(this.menu.enableDoubleClick) ht.Add("doubleclick", "true");
		ht = this.menu.mouseTouch.GetData(ht);
		
		if(this.menu.addBack)
		{
			ht.Add("addback", "true");
			if(this.menu.backFirst) ht.Add("backfirst", "true");
			for(int i=0; i<this.menu.backName.Count; i++)
			{
				s.Add(HashtableHelper.GetContentHashtable(
						BattleSystemData.BACK, this.menu.backName[i].ToString(), i));
			}
			if(this.menu.backIconName != "") s.Add(HashtableHelper.GetContentHashtable(
					BattleSystemData.BACKICON, this.menu.backIconName));
		}
		if(this.menu.showAttack)
		{
			for(int i=0; i<this.menu.attackName.Count; i++)
			{
				s.Add(HashtableHelper.GetContentHashtable(
						BattleSystemData.ATTACK, this.menu.attackName[i].ToString(), i));
			}
			if(this.menu.attackIconName != "") s.Add(HashtableHelper.GetContentHashtable(
					BattleSystemData.ATTACKICON, this.menu.attackIconName));
		}
		if(this.menu.showSkills)
		{
			for(int i=0; i<this.menu.skillName.Count; i++)
			{
				s.Add(HashtableHelper.GetContentHashtable(
						BattleSystemData.SKILL, this.menu.skillName[i].ToString(), i));
			}
			if(this.menu.skillIconName != "") s.Add(HashtableHelper.GetContentHashtable(
					BattleSystemData.SKILLICON, this.menu.skillIconName));
		}
		if(this.menu.showItems)
		{
			for(int i=0; i<this.menu.itemName.Count; i++)
			{
				s.Add(HashtableHelper.GetContentHashtable(
						BattleSystemData.ITEM, this.menu.itemName[i].ToString(), i));
			}
			if(this.menu.itemIconName != "") s.Add(HashtableHelper.GetContentHashtable(
					BattleSystemData.ITEMICON, this.menu.itemIconName));
		}
		if(this.menu.showDefend)
		{
			for(int i=0; i<this.menu.defendName.Count; i++)
			{
				s.Add(HashtableHelper.GetContentHashtable(
						BattleSystemData.DEFEND, this.menu.defendName[i].ToString(), i));
			}
			if(this.menu.defendIconName != "") s.Add(HashtableHelper.GetContentHashtable(
					BattleSystemData.DEFENDICON, this.menu.defendIconName));
		}
		if(this.menu.showEscape)
		{
			for(int i=0; i<this.menu.escapeName.Count; i++)
			{
				s.Add(HashtableHelper.GetContentHashtable(
						BattleSystemData.ESCAPE, this.menu.escapeName[i].ToString(), i));
			}
			if(this.menu.escapeIconName != "") s.Add(HashtableHelper.GetContentHashtable(
					BattleSystemData.ESCAPEICON, this.menu.escapeIconName));
		}
		if(this.menu.showEndTurn)
		{
			for(int i=0; i<this.menu.endTurnName.Count; i++)
			{
				s.Add(HashtableHelper.GetContentHashtable(
						BattleSystemData.ENDTURN, this.menu.endTurnName[i].ToString(), i));
			}
			if(this.menu.endTurnIconName != "") s.Add(HashtableHelper.GetContentHashtable(
					BattleSystemData.ENDTURNICON, this.menu.endTurnIconName));
		}
		
		Hashtable ht3 = new Hashtable();
		ht3.Add(XMLHandler.NODE_NAME, BattleSystemData.TARGET);
		ht3.Add("targetmenu", this.menu.useTargetMenu.ToString());
		ht3.Add("targetblink", this.menu.useTargetBlink.ToString());
		if(this.menu.useTargetBlink)
		{
			if(this.menu.fromCurrent) ht3.Add("fromcurrent", "true");
			ht3.Add("children", this.menu.blinkChildren.ToString());
			ht3.Add("time", this.menu.blinkTime.ToString());
			ht3.Add("interpolation", this.menu.blinkInterpolation.ToString());
			if(this.menu.aBlink)
			{
				ht3.Add("as", this.menu.aStart.ToString());
				ht3.Add("ae", this.menu.aEnd.ToString());
			}
			if(this.menu.rBlink)
			{
				ht3.Add("rs", this.menu.rStart.ToString());
				ht3.Add("re", this.menu.rEnd.ToString());
			}
			if(this.menu.gBlink)
			{
				ht3.Add("gs", this.menu.gStart.ToString());
				ht3.Add("ge", this.menu.gEnd.ToString());
			}
			if(this.menu.bBlink)
			{
				ht3.Add("bs", this.menu.bStart.ToString());
				ht3.Add("be", this.menu.bEnd.ToString());
			}
		}
		s.Add(ht3);
		
		if(this.menu.useTargetCursor && "" != this.menu.cursorPrefabName)
		{
			Hashtable ht2 = HashtableHelper.GetContentHashtable(
						BattleSystemData.CURSOR, this.menu.cursorPrefabName);
			ht2.Add("cox", this.menu.cursorOffset.x.ToString());
			ht2.Add("coy", this.menu.cursorOffset.y.ToString());
			ht2.Add("coz", this.menu.cursorOffset.z.ToString());
			s.Add(ht2);
			if(this.menu.cursorChildName != "")
			{
				s.Add(HashtableHelper.GetContentHashtable(
						BattleSystemData.CURSORCHILD, this.menu.cursorChildName));
			}
		}
		
		if(s.Count > 0)
		{
			ht.Add(XMLHandler.NODES, s);
		}
		subs.Add(ht);
		
		// text
		s = new ArrayList();
		ht = new Hashtable();
		ht.Add(XMLHandler.NODE_NAME, BattleSystemData.TEXT);
		ht.Add("showuserdamage", this.showUserDamage.ToString());
		if(this.mountTexts) ht.Add("mounttexts", "true");
		
		ht.Add("bscolor", this.battleStartColor.ToString());
		ht.Add("bsshadowcolor", this.battleStartSColor.ToString());
		ht.Add("bvcolor", this.battleVictoryColor.ToString());
		ht.Add("bvshadowcolor", this.battleVictorySColor.ToString());
		ht.Add("bdcolor", this.battleDefeatColor.ToString());
		ht.Add("bdshadowcolor", this.battleDefeatSColor.ToString());
		ht.Add("becolor", this.battleEscapeColor.ToString());
		ht.Add("beshadowcolor", this.battleEscapeSColor.ToString());
		
		if(this.showInfo)
		{
			Hashtable ht2 = new Hashtable();
			ht2.Add(XMLHandler.NODE_NAME, BattleSystemData.INFO);
			ht2.Add("position", this.infoPosition.ToString());
			ht2.Add("time", this.infoShowTime.ToString());
			ht2.Add("showskills", this.showSkills.ToString());
			ht2.Add("showitems", this.showItems.ToString());
			ht2.Add("showdefend", this.showDefend.ToString());
			ht2.Add("showescape", this.showEscape.ToString());
			ht2.Add("showcounter", this.showCounter.ToString());
			ht2.Add("showstealitem", this.showStealItem.ToString());
			ht2.Add("showstealitemfail", this.showStealItemFail.ToString());
			ht2.Add("showstealmoney", this.showStealMoney.ToString());
			ht2.Add("showstealmoneyfail", this.showStealMoneyFail.ToString());
			s.Add(ht2);
		}
		if(this.showBattleMessage)
		{
			ht.Add("bmpos", this.battleMessagePosition.ToString());
			ht.Add("bmtime", this.battleMessageShowTime.ToString());
		}
		
		if("" != this.textSkinName)
		{
			Hashtable ht2 = new Hashtable();
			ht2.Add(XMLHandler.NODE_NAME, BattleSystemData.SKIN);
			ht2.Add(XMLHandler.CONTENT, this.textSkinName);
			s.Add(ht2);
		}
		for(int i=0; i<this.counterText.Count; i++)
		{
			Hashtable ht2 = new Hashtable();
			ht2.Add(XMLHandler.NODE_NAME, BattleSystemData.COUNTER);
			ht2.Add("id", i.ToString());
			ht2.Add(XMLHandler.CONTENT, this.counterText[i]);
			s.Add(ht2);
		}
		for(int i=0; i<this.allAlliesText.Count; i++)
		{
			Hashtable ht2 = new Hashtable();
			ht2.Add(XMLHandler.NODE_NAME, BattleSystemData.ALLIES);
			ht2.Add("id", i.ToString());
			ht2.Add(XMLHandler.CONTENT, this.allAlliesText[i]);
			s.Add(ht2);
		}
		for(int i=0; i<this.allEnemiesText.Count; i++)
		{
			Hashtable ht2 = new Hashtable();
			ht2.Add(XMLHandler.NODE_NAME, BattleSystemData.ENEMIES);
			ht2.Add("id", i.ToString());
			ht2.Add(XMLHandler.CONTENT, this.allEnemiesText[i]);
			s.Add(ht2);
		}
		for(int i=0; i<this.battleStartText.Count; i++)
		{
			Hashtable ht2 = new Hashtable();
			ht2.Add(XMLHandler.NODE_NAME, BattleSystemData.BATTLESTART);
			ht2.Add("id", i.ToString());
			ht2.Add(XMLHandler.CONTENT, this.battleStartText[i]);
			s.Add(ht2);
		}
		for(int i=0; i<this.battleVictoryText.Count; i++)
		{
			Hashtable ht2 = new Hashtable();
			ht2.Add(XMLHandler.NODE_NAME, BattleSystemData.BATTLEVICTORY);
			ht2.Add("id", i.ToString());
			ht2.Add(XMLHandler.CONTENT, this.battleVictoryText[i]);
			s.Add(ht2);
		}
		for(int i=0; i<this.battleDefeatText.Count; i++)
		{
			Hashtable ht2 = new Hashtable();
			ht2.Add(XMLHandler.NODE_NAME, BattleSystemData.BATTLEDEFEAT);
			ht2.Add("id", i.ToString());
			ht2.Add(XMLHandler.CONTENT, this.battleDefeatText[i]);
			s.Add(ht2);
		}
		for(int i=0; i<this.battleEscapeText.Count; i++)
		{
			Hashtable ht2 = new Hashtable();
			ht2.Add(XMLHandler.NODE_NAME, BattleSystemData.BATTLEESCAPE);
			ht2.Add("id", i.ToString());
			ht2.Add(XMLHandler.CONTENT, this.battleEscapeText[i]);
			s.Add(ht2);
		}
		for(int i=0; i<this.stealItemText.Count; i++)
		{
			s.Add(HashtableHelper.GetContentHashtable(
					BattleSystemData.STEALITEM, this.stealItemText[i] as string, i));
		}
		for(int i=0; i<this.stealItemFailText.Count; i++)
		{
			s.Add(HashtableHelper.GetContentHashtable(
					BattleSystemData.STEALITEMFAIL, this.stealItemFailText[i] as string, i));
		}
		for(int i=0; i<this.stealMoneyText.Count; i++)
		{
			s.Add(HashtableHelper.GetContentHashtable(
					BattleSystemData.STEALMONEY, this.stealMoneyText[i] as string, i));
		}
		for(int i=0; i<this.stealMoneyFailText.Count; i++)
		{
			s.Add(HashtableHelper.GetContentHashtable(
					BattleSystemData.STEALMONEYFAIL, this.stealMoneyFailText[i] as string, i));
		}
		s.Add(this.effectTextSettings.GetData(BattleSystemData.EFFECT));
		s.Add(this.missTextSettings.GetData(BattleSystemData.MISSSETTINGS));
		s.Add(this.blockTextSettings.GetData(BattleSystemData.BLOCKSETTINGS));
		s.Add(this.castCancelTextSettings.GetData(BattleSystemData.CASTCANCELSETTINGS));
		s.Add(this.levelUpTextSettings.GetData(BattleSystemData.LEVELUPSETTINGS));
		s.Add(this.classLevelUpTextSettings.GetData(BattleSystemData.CLASSLEVELUPSETTINGS));
		
		ht.Add(XMLHandler.NODES, s);
		subs.Add(ht);
		
		subs.Add(this.end.GetData(HashtableHelper.GetTitleHashtable(BattleSystemData.BATTLEEND)));
		subs.Add(this.cam.GetData(HashtableHelper.GetTitleHashtable(BattleSystemData.BATTLECAM)));
		subs.Add(this.control.GetData(HashtableHelper.GetTitleHashtable(BattleSystemData.BATTLECONTROL)));
		
		sv.Add(XMLHandler.NODES, subs);
		data.Add(sv);
		XMLHandler.SaveXML(dir, filename, data);
	}
	
	public void LoadResources()
	{
		if(!this.textSkin && "" != this.textSkinName)
		{
			this.textSkin = (GUISkin)Resources.Load(this.skinPath+this.textSkinName, typeof(GUISkin));
		}
	}
	
	public string GetCounterText()
	{
		return this.counterText[GameHandler.GetLanguage()] as string;
	}
	
	public string GetAllAlliesText()
	{
		return this.allAlliesText[GameHandler.GetLanguage()] as string;
	}
	
	public string GetAllEnemiesText()
	{
		return this.allEnemiesText[GameHandler.GetLanguage()] as string;
	}
	
	public string GetBattleStartText()
	{
		return this.battleStartText[GameHandler.GetLanguage()] as string;
	}
	
	public string GetBattleVictoryText()
	{
		return this.battleVictoryText[GameHandler.GetLanguage()] as string;
	}
	
	public string GetBattleDefeatText()
	{
		return this.battleDefeatText[GameHandler.GetLanguage()] as string;
	}
	
	public string GetBattleEscapeText()
	{
		return this.battleEscapeText[GameHandler.GetLanguage()] as string;
	}
	
	public string GetStealItemText(string name, string item)
	{
		string txt = this.stealItemText[GameHandler.GetLanguage()] as string;
		txt = txt.Replace("%n", name);
		txt = txt.Replace("%", item);
		return txt;
	}
	
	public string GetStealItemFailText(string name)
	{
		string txt = this.stealItemFailText[GameHandler.GetLanguage()] as string;
		txt = txt.Replace("%n", name);
		return txt;
	}
	
	public string GetStealMoneyText(string name, string money)
	{
		string txt = this.stealMoneyText[GameHandler.GetLanguage()] as string;
		txt = txt.Replace("%n", name);
		txt = txt.Replace("%", money);
		return txt;
	}
	
	public string GetStealMoneyFailText(string name)
	{
		string txt = this.stealMoneyFailText[GameHandler.GetLanguage()] as string;
		txt = txt.Replace("%n", name);
		return txt;
	}
}