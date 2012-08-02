
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class LoadSaveHUDData
{
	public bool encryptData = false;
	
	public int maxSaveGames = 3;
	public SaveGameType saveGameType =  SaveGameType.PLAYER_PREFS;
	
	public int savePosition = 0;
	public int saveQuestionPosition = 0;
	
	public bool fadeOut = true;
	public float fadeOutTime = 0.5f;
	public EaseType fadeOutInterpolate = EaseType.Linear;
	
	public bool fadeIn = true;
	public float fadeInTime = 0.5f;
	public EaseType fadeInInterpolate = EaseType.Linear;
	
	// texts
	public string[] saveText = new string[] {"Select save file"};
	public string[] loadText = new string[] {"Select load file"};
	public string[] saveYesText = new string[] {"Yes"};
	public string[] saveNoText = new string[] {"No"};
	public string[] saveQuestionText = new string[] {"Save file %i?"};
	public string[] loadQuestionText = new string[] {"Load file %i?"};
	
	// layout
	public string[] fileInfoLayout = new string[] {"File %i: %n Lvl. %l - %a, %t"};
	public string[] emptyInfoLayout = new string[] {"File %i: empty"};
	
	// save point
	public int spPosition = 0;
	public bool showChoice = true;
	public bool showLoad  = true;
	
	public string[] spSaveText = new string[] {"Save"};
	public string[] spLoadText = new string[] {"Load"};
	public string[] spCancelText = new string[] {"Cancel"};
	
	public string fileIconName = "";
	public Texture2D fileIcon;
	
	public string iconPath = "Icons/LoadSaveHUD/";
	public string skinPath = "HUD/";
	
	// save options
	public bool saveStatistics = true;
	public bool saveTime = true;
	public bool saveParty = true;
	public bool savePartyPosition = true;
	public string saveSceneName = "";
	public int saveSpawnID = 0;
	
	public bool saveItems = true;
	public bool saveWeapons = true;
	public bool saveArmors = true;
	public bool saveRecipes = true;
	public bool saveMoney = true;
	
	public Selector gameVariableSelector = Selector.ALL;
	public string[] gameVariableList = new string[0];
	
	public Selector numberVariableSelector = Selector.ALL;
	public string[] numberVariableList = new string[0];
	
	// auto save
	public string[] autoFileName = new string[] {"AUTO"};
	public bool showAutoSaveMessage = false;
	public int autoMessagePosition = 0;
	public float autoVisibilityTime = 3;
	public string[] autoSaveMessage = new string[] {"Autosave"};
	
	// XML data
	private string dir = "ProjectSettings/";
	private string filename = "loadSaveHUD";
	
	private static string LOADSAVEHUDS = "loadsavehuds";
	private static string LOADSAVEHUD = "loadsavehud";
	private static string SAVETEXT = "savetext";
	private static string LOADTEXT = "loadtext";
	private static string SAVEYESTEXT = "saveyestext";
	private static string SAVENOTEXT = "savenotext";
	private static string SAVEQUESTIONTEXT = "savequestiontext";
	private static string LOADQUESTIONTEXT = "loadquestiontext";
	private static string FILEINFOLAYOUT = "fileinfolayout";
	private static string EMPTYINFOLAYOUT = "emptyinfolayout";
	private static string SPSAVETEXT = "spsavetext";
	private static string SPLOADTEXT = "sploadtext";
	private static string SPCANCELTEXT = "spcanceltext";
	private static string GAMEVARLIST = "gamevarlist";
	private static string NUMBERVARLIST = "numbervarlist";
	private static string SAVESCENENAME = "savescenename";
	private static string AUTOFILE = "autofile";
	private static string AUTOMESSAGE = "automessage";

	public LoadSaveHUDData()
	{
		LoadData();
	}
	
	public void LoadData()
	{
		ArrayList data = XMLHandler.LoadXML(dir+filename);
		
		int langs = DataHolder.LanguageCount;
		this.saveText = new string[langs];
		this.loadText = new string[langs];
		this.saveYesText = new string[langs];
		this.saveNoText = new string[langs];
		this.saveQuestionText = new string[langs];
		this.loadQuestionText = new string[langs];
		this.fileInfoLayout = new string[langs];
		this.emptyInfoLayout = new string[langs];
		this.spSaveText = new string[langs];
		this.spLoadText = new string[langs];
		this.spCancelText = new string[langs];
		this.autoFileName = new string[langs];
		this.autoSaveMessage = new string[langs];
		
		for(int i=0; i<langs; i++)
		{
			this.saveText[i] = "Select save file";
			this.loadText[i] = "Select load file";
			this.saveYesText[i] = "Yes";
			this.saveNoText[i] = "No";
			this.saveQuestionText[i] = "Save file %i?";
			this.loadQuestionText[i] = "Load file %i?";
			this.fileInfoLayout[i] = "File %i: %n Lvl. %l - %a, %t";
			this.emptyInfoLayout[i] = "File %i: empty";
			this.spSaveText[i] = "Save";
			this.spLoadText[i] = "Load";
			this.spCancelText[i] = "Cancel";
			this.autoFileName[i] = "AUTO";
			this.autoSaveMessage[i] = "Autosave";
		}
		
		if(data.Count > 0)
		{
			foreach(Hashtable entry in data)
			{
				if(entry[XMLHandler.NODE_NAME] as string == LoadSaveHUDData.LOADSAVEHUDS)
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList subs = entry[XMLHandler.NODES] as ArrayList;
						foreach(Hashtable val in subs)
						{
							if(val[XMLHandler.NODE_NAME] as string == LoadSaveHUDData.LOADSAVEHUD)
							{
								if(val.ContainsKey("encryptdata")) encryptData = true;
								
								if(val.ContainsKey("maxsavegames")) maxSaveGames = int.Parse((string)val["maxsavegames"]);
								if(val.ContainsKey("savegametype"))
								{
									saveGameType = (SaveGameType)System.Enum.Parse(
											typeof(SaveGameType), (string)val["savegametype"]);
								}
								
								if(val.ContainsKey("saveposition")) savePosition = int.Parse((string)val["saveposition"]);
								if(val.ContainsKey("savequestionposition")) saveQuestionPosition = int.Parse((string)val["savequestionposition"]);
								if(val.ContainsKey("fileicon")) fileIconName = val["fileicon"] as string;
								if(val.ContainsKey("showchoice")) showChoice = bool.Parse((string)val["showchoice"]);
								if(val.ContainsKey("showload")) showLoad = bool.Parse((string)val["showload"]);
								if(val.ContainsKey("sppos")) spPosition = int.Parse((string)val["sppos"]);
								
								if(val.ContainsKey("fadeout")) fadeOut = bool.Parse((string)val["fadeout"]);
								if(val.ContainsKey("fadeouttime")) fadeOutTime = float.Parse((string)val["fadeouttime"]);
								if(val.ContainsKey("fadeoutinterpolate")) fadeOutInterpolate = (EaseType)System.Enum.Parse(typeof(EaseType), (string)val["fadeoutinterpolate"]);
								if(val.ContainsKey("fadein")) fadeIn = bool.Parse((string)val["fadein"]);
								if(val.ContainsKey("fadeintime")) fadeInTime = float.Parse((string)val["fadeintime"]);
								if(val.ContainsKey("fadeininterpolate")) fadeInInterpolate = (EaseType)System.Enum.Parse(typeof(EaseType), (string)val["fadeininterpolate"]);
								
								if(val.ContainsKey("savestatistics")) this.saveStatistics = bool.Parse((string)val["savestatistics"]);
								if(val.ContainsKey("savetime")) this.saveTime = bool.Parse((string)val["savetime"]);
								if(val.ContainsKey("savestatistics")) this.saveStatistics = bool.Parse((string)val["savestatistics"]);
								if(val.ContainsKey("saveparty")) this.saveParty = bool.Parse((string)val["saveparty"]);
								if(val.ContainsKey("savepartyposition")) this.savePartyPosition = bool.Parse((string)val["savepartyposition"]);
								if(val.ContainsKey("savespawnid")) this.saveSpawnID = int.Parse((string)val["savespawnid"]);
								if(val.ContainsKey("saveitems")) this.saveItems = bool.Parse((string)val["saveitems"]);
								if(val.ContainsKey("saveweapons")) this.saveWeapons = bool.Parse((string)val["saveweapons"]);
								if(val.ContainsKey("savearmors")) this.saveArmors = bool.Parse((string)val["savearmors"]);
								if(val.ContainsKey("saverecipes")) this.saveRecipes = bool.Parse((string)val["saverecipes"]);
								if(val.ContainsKey("savemoney")) this.saveMoney = bool.Parse((string)val["savemoney"]);
								if(val.ContainsKey("gamevarselector")) this.gameVariableSelector = (Selector)System.Enum.Parse(typeof(Selector), (string)val["gamevarselector"]);
								if(val.ContainsKey("numbervarselector")) this.numberVariableSelector = (Selector)System.Enum.Parse(typeof(Selector), (string)val["numbervarselector"]);
								
								if(val.ContainsKey("gamevariableslist"))
								{
									this.gameVariableList = new string[int.Parse((string)val["gamevariableslist"])];
									for(int i=0; i<this.gameVariableList.Length; i++)
									{
										this.gameVariableList[i] = "";
									}
								}
								if(val.ContainsKey("numbervariablelist"))
								{
									this.numberVariableList = new string[int.Parse((string)val["numbervariablelist"])];
									for(int i=0; i<this.numberVariableList.Length; i++)
									{
										this.numberVariableList[i] = "";
									}
								}
								
								if(val.ContainsKey("automessagepos"))
								{
									this.showAutoSaveMessage = true;
									this.autoMessagePosition = int.Parse((string)val["automessagepos"]);
									this.autoVisibilityTime = float.Parse((string)val["autovisibilitytime"]);
								}
								
								ArrayList s = val[XMLHandler.NODES] as ArrayList;
								foreach(Hashtable ht in s)
								{
									if(ht[XMLHandler.NODE_NAME] as string == LoadSaveHUDData.SAVETEXT)
									{
										HashtableHelper.GetContentString(ht, ref this.saveText);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == LoadSaveHUDData.LOADTEXT)
									{
										HashtableHelper.GetContentString(ht, ref this.loadText);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == LoadSaveHUDData.SAVEYESTEXT)
									{
										HashtableHelper.GetContentString(ht, ref this.saveYesText);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == LoadSaveHUDData.SAVENOTEXT)
									{
										HashtableHelper.GetContentString(ht, ref this.saveNoText);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == LoadSaveHUDData.SAVEQUESTIONTEXT)
									{
										HashtableHelper.GetContentString(ht, ref this.saveQuestionText);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == LoadSaveHUDData.LOADQUESTIONTEXT)
									{
										HashtableHelper.GetContentString(ht, ref this.loadQuestionText);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == LoadSaveHUDData.FILEINFOLAYOUT)
									{
										HashtableHelper.GetContentString(ht, ref this.fileInfoLayout);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == LoadSaveHUDData.EMPTYINFOLAYOUT)
									{
										HashtableHelper.GetContentString(ht, ref this.emptyInfoLayout);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == LoadSaveHUDData.SPSAVETEXT)
									{
										HashtableHelper.GetContentString(ht, ref this.spSaveText);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == LoadSaveHUDData.SPLOADTEXT)
									{
										HashtableHelper.GetContentString(ht, ref this.spLoadText);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == LoadSaveHUDData.SPCANCELTEXT)
									{
										HashtableHelper.GetContentString(ht, ref this.spCancelText);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == LoadSaveHUDData.SAVESCENENAME)
									{
										this.saveSceneName = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == LoadSaveHUDData.GAMEVARLIST)
									{
										HashtableHelper.GetContentString(ht, ref this.gameVariableList);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == LoadSaveHUDData.NUMBERVARLIST)
									{
										HashtableHelper.GetContentString(ht, ref this.numberVariableList);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == LoadSaveHUDData.AUTOFILE)
									{
										HashtableHelper.GetContentString(ht, ref this.autoFileName);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == LoadSaveHUDData.AUTOMESSAGE)
									{
										HashtableHelper.GetContentString(ht, ref this.autoSaveMessage);
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
		ArrayList data = new ArrayList();
		ArrayList subs = new ArrayList();
		Hashtable sv = new Hashtable();
		
		sv.Add(XMLHandler.NODE_NAME, LoadSaveHUDData.LOADSAVEHUDS);
		
		Hashtable ht = new Hashtable();
		ArrayList s = new ArrayList();
		
		ht.Add(XMLHandler.NODE_NAME, LoadSaveHUDData.LOADSAVEHUD);
		if(encryptData) ht.Add("encryptdata", "true");
		ht.Add("maxsavegames", maxSaveGames.ToString());
		ht.Add("savegametype", saveGameType.ToString());
		ht.Add("saveposition", savePosition.ToString());
		ht.Add("savequestionposition", saveQuestionPosition.ToString());
		if("" != fileIconName) ht.Add("fileicon", fileIconName);
		
		ht.Add("fadeout", fadeOut.ToString());
		ht.Add("fadeouttime", fadeOutTime.ToString());
		ht.Add("fadeoutinterpolate", fadeOutInterpolate.ToString());
		ht.Add("fadein", fadeIn.ToString());
		ht.Add("fadeintime", fadeInTime.ToString());
		ht.Add("fadeininterpolate", fadeInInterpolate.ToString());
		
		ht.Add("savestatistics", saveStatistics.ToString());
		ht.Add("savetime", saveTime.ToString());
		ht.Add("saveparty", saveParty.ToString());
		ht.Add("savepartyposition", savePartyPosition.ToString());
		if(!savePartyPosition)
		{
			ht.Add("savespawnid", saveSpawnID.ToString());
			if(saveSceneName != "")
			{
				s.Add(HashtableHelper.GetContentHashtable(
						LoadSaveHUDData.SAVESCENENAME, saveSceneName));
			}
		}
		ht.Add("saveitems", saveItems.ToString());
		ht.Add("saveweapons", saveWeapons.ToString());
		ht.Add("savearmors", saveArmors.ToString());
		ht.Add("saverecipes", saveRecipes.ToString());
		ht.Add("savemoney", saveMoney.ToString());
		ht.Add("gamevarselector", gameVariableSelector.ToString());
		ht.Add("numbervarselector", numberVariableSelector.ToString());
		
		if(!Selector.NONE.Equals(this.gameVariableSelector) && gameVariableList.Length > 0)
		{
			ht.Add("gamevariableslist", gameVariableList.Length.ToString());
			for(int i=0; i<gameVariableList.Length; i++)
			{
				s.Add(HashtableHelper.GetContentHashtable(
						LoadSaveHUDData.GAMEVARLIST, gameVariableList[i], i));
			}
		}
		if(!Selector.NONE.Equals(this.numberVariableSelector) && numberVariableList.Length > 0)
		{
			ht.Add("numbervariablelist", numberVariableList.Length.ToString());
			for(int i=0; i<numberVariableList.Length; i++)
			{
				s.Add(HashtableHelper.GetContentHashtable(
						LoadSaveHUDData.NUMBERVARLIST, numberVariableList[i], i));
			}
		}
		
		for(int i=0; i<saveText.Length; i++)
		{
			s.Add(HashtableHelper.GetContentHashtable(
					LoadSaveHUDData.SAVETEXT, saveText[i], i));
		}
		for(int i=0; i<loadText.Length; i++)
		{
			s.Add(HashtableHelper.GetContentHashtable(
					LoadSaveHUDData.LOADTEXT, loadText[i], i));
		}
		for(int i=0; i<saveYesText.Length; i++)
		{
			s.Add(HashtableHelper.GetContentHashtable(
					LoadSaveHUDData.SAVEYESTEXT, saveYesText[i], i));
		}
		for(int i=0; i<saveNoText.Length; i++)
		{
			s.Add(HashtableHelper.GetContentHashtable(
					LoadSaveHUDData.SAVENOTEXT, saveNoText[i], i));
		}
		for(int i=0; i<saveQuestionText.Length; i++)
		{
			s.Add(HashtableHelper.GetContentHashtable(
					LoadSaveHUDData.SAVEQUESTIONTEXT, saveQuestionText[i], i));
		}
		for(int i=0; i<loadQuestionText.Length; i++)
		{
			s.Add(HashtableHelper.GetContentHashtable(
					LoadSaveHUDData.LOADQUESTIONTEXT, loadQuestionText[i], i));
		}
		for(int i=0; i<fileInfoLayout.Length; i++)
		{
			s.Add(HashtableHelper.GetContentHashtable(
					LoadSaveHUDData.FILEINFOLAYOUT, fileInfoLayout[i], i));
		}
		for(int i=0; i<emptyInfoLayout.Length; i++)
		{
			s.Add(HashtableHelper.GetContentHashtable(
					LoadSaveHUDData.EMPTYINFOLAYOUT, emptyInfoLayout[i], i));
		}
		
		ht.Add("showchoice", this.showChoice.ToString());
		ht.Add("showload", this.showLoad.ToString());
		ht.Add("sppos", this.spPosition.ToString());
		for(int i=0; i<spSaveText.Length; i++)
		{
			s.Add(HashtableHelper.GetContentHashtable(
					LoadSaveHUDData.SPSAVETEXT, spSaveText[i], i));
		}
		for(int i=0; i<spLoadText.Length; i++)
		{
			s.Add(HashtableHelper.GetContentHashtable(
					LoadSaveHUDData.SPLOADTEXT, spLoadText[i], i));
		}
		for(int i=0; i<spCancelText.Length; i++)
		{
			s.Add(HashtableHelper.GetContentHashtable(
					LoadSaveHUDData.SPCANCELTEXT, spCancelText[i], i));
		}
		
		// auto save
		for(int i=0; i<this.autoFileName.Length; i++)
		{
			s.Add(HashtableHelper.GetContentHashtable(
					LoadSaveHUDData.AUTOFILE, this.autoFileName[i], i));
		}
		if(this.showAutoSaveMessage)
		{
			ht.Add("automessagepos", this.autoMessagePosition.ToString());
			ht.Add("autovisibilitytime", this.autoVisibilityTime.ToString());
			for(int i=0; i<this.autoSaveMessage.Length; i++)
			{
				s.Add(HashtableHelper.GetContentHashtable(
						LoadSaveHUDData.AUTOMESSAGE, this.autoSaveMessage[i], i));
			}
		}
		
		ht.Add(XMLHandler.NODES, s);
		subs.Add(ht);
		sv.Add(XMLHandler.NODES, subs);
		data.Add(sv);
		XMLHandler.SaveXML(dir, filename, data);
	}
	
	/*
	============================================================================
	Language functions
	============================================================================
	*/
	public void AddLanguage(int lang)
	{
		saveText = ArrayHelper.Add("Select save file", saveText);
		loadText = ArrayHelper.Add("Select load file", loadText);
		saveYesText = ArrayHelper.Add("Yes", saveYesText);
		saveNoText = ArrayHelper.Add("No", saveNoText);
		saveQuestionText = ArrayHelper.Add("Save file %i?", saveQuestionText);
		loadQuestionText = ArrayHelper.Add("Load file %i?", loadQuestionText);
		fileInfoLayout = ArrayHelper.Add("File %i: %n Lvl. %l - %a, %t", fileInfoLayout);
		emptyInfoLayout = ArrayHelper.Add("File %i: empty", emptyInfoLayout);
		spSaveText = ArrayHelper.Add("Save", spSaveText);
		spLoadText = ArrayHelper.Add("Load", spLoadText);
		spCancelText = ArrayHelper.Add("Cancel", spCancelText);
		autoFileName = ArrayHelper.Add("AUTO", autoFileName);
		autoSaveMessage = ArrayHelper.Add("Autosave", autoSaveMessage);
	}
	
	public void RemoveLanguage(int lang)
	{
		saveText = ArrayHelper.Remove(lang, saveText);
		loadText = ArrayHelper.Remove(lang, loadText);
		saveYesText = ArrayHelper.Remove(lang, saveYesText);
		saveNoText = ArrayHelper.Remove(lang, saveNoText);
		saveQuestionText = ArrayHelper.Remove(lang, saveQuestionText);
		loadQuestionText = ArrayHelper.Remove(lang, loadQuestionText);
		fileInfoLayout = ArrayHelper.Remove(lang, fileInfoLayout);
		emptyInfoLayout = ArrayHelper.Remove(lang, emptyInfoLayout);
		spSaveText = ArrayHelper.Remove(lang, spSaveText);
		spLoadText = ArrayHelper.Remove(lang, spLoadText);
		spCancelText = ArrayHelper.Remove(lang, spCancelText);
		autoFileName = ArrayHelper.Remove(lang, autoFileName);
		autoSaveMessage = ArrayHelper.Remove(lang, autoSaveMessage);
	}
	
	public void LoadResources()
	{
		if("" != this.fileIconName)
		{
			this.fileIcon = (Texture2D)Resources.Load(this.iconPath+this.fileIconName, typeof(Texture2D));
		}
	}
	
	public string GetSaveString()
	{
		return this.saveText[GameHandler.GetLanguage()];
	}
	
	public string GetLoadstring()
	{
		return this.loadText[GameHandler.GetLanguage()];
	}
	
	public string GetYesString()
	{
		return this.saveYesText[GameHandler.GetLanguage()];
	}
	
	public string GetNoString()
	{
		return this.saveNoText[GameHandler.GetLanguage()];
	}
	
	public string GetSaveQuestionString(int index)
	{
		string tmp = this.saveQuestionText[GameHandler.GetLanguage()];
		tmp = tmp.Replace("%i", (index+1).ToString());
		return tmp;
	}
	
	public string GetLoadQuestionString(int index)
	{
		string tmp = this.loadQuestionText[GameHandler.GetLanguage()];
		if(index == SaveHandler.AUTOSAVE_INDEX) tmp = tmp.Replace("%i", DataHolder.LoadSaveHUD().GetAutoName());
		else tmp = tmp.Replace("%i", (index+1).ToString());
		return tmp;
	}
	
	public string GetFileInfoString()
	{
		return this.fileInfoLayout[GameHandler.GetLanguage()];
	}
	
	public string GetEmptyInfoString()
	{
		return this.emptyInfoLayout[GameHandler.GetLanguage()];
	}
	
	public ChoiceContent[] GetSavePointChoice()
	{
		ChoiceContent[] choice;
		if(this.showLoad)
		{
			choice = new ChoiceContent[3];
			choice[0] = new ChoiceContent(new GUIContent(this.spSaveText[GameHandler.GetLanguage()]));
			choice[1] = new ChoiceContent(new GUIContent(this.spLoadText[GameHandler.GetLanguage()]));
			choice[2] = new ChoiceContent(new GUIContent(this.spCancelText[GameHandler.GetLanguage()]));
		}
		else
		{
			choice = new ChoiceContent[2];
			choice[0] = new ChoiceContent(new GUIContent(this.spSaveText[GameHandler.GetLanguage()]));
			choice[1] = new ChoiceContent(new GUIContent(this.spCancelText[GameHandler.GetLanguage()]));
		}
		return choice;
	}
	
	public ChoiceContent[] GetYesNoChoice()
	{
		ChoiceContent[] choice = new ChoiceContent[2];
		choice[0] = new ChoiceContent(new GUIContent(this.saveYesText[GameHandler.GetLanguage()]));
		choice[1] = new ChoiceContent(new GUIContent(this.saveNoText[GameHandler.GetLanguage()]));
		return choice;
	}
	
	public ChoiceContent[] GetSaveFileChoice()
	{
		ChoiceContent[] choice = new ChoiceContent[this.maxSaveGames+1];
		for(int i=0; i<choice.Length-1; i++)
		{
			choice[i] = new ChoiceContent(new GUIContent(SaveHandler.GetFileInfo(i), this.fileIcon));
		}
		choice[choice.Length-1] = new ChoiceContent(new GUIContent(this.spCancelText[GameHandler.GetLanguage()]));
		return choice;
	}
	
	public ChoiceContent[] GetLoadFileChoice()
	{
		ChoiceContent[] choice;
		int offset = 0;
		if(SaveHandler.FileExists(SaveHandler.AUTOSAVE_INDEX))
		{
			choice = new ChoiceContent[this.maxSaveGames+2];
			offset = 1;
			choice[0] = new ChoiceContent(new GUIContent(SaveHandler.GetFileInfo(-1), this.fileIcon), true);
		}
		else choice = new ChoiceContent[this.maxSaveGames+1];
		for(int i=0; i<choice.Length-1-offset; i++)
		{
			choice[i+offset] = new ChoiceContent(new GUIContent(SaveHandler.GetFileInfo(i), this.fileIcon), SaveHandler.FileExists(i));
		}
		choice[choice.Length-1] = new ChoiceContent(new GUIContent(this.spCancelText[GameHandler.GetLanguage()]));
		return choice;
	}
	
	public ChoiceContent[] GetFileList(bool isLoad)
	{
		ChoiceContent[] choice;
		int offset = 0;
		if(isLoad && SaveHandler.FileExists(SaveHandler.AUTOSAVE_INDEX))
		{
			choice = new ChoiceContent[this.maxSaveGames+1];
			offset = 1;
			choice[0] = new ChoiceContent(new GUIContent(
					SaveHandler.GetFileInfo(SaveHandler.AUTOSAVE_INDEX), this.fileIcon), true);
		}
		else choice = new ChoiceContent[this.maxSaveGames];
		for(int i=0; i<choice.Length-offset; i++)
		{
			if(isLoad)
			{
				choice[i+offset] = new ChoiceContent(new GUIContent(SaveHandler.GetFileInfo(i), this.fileIcon), SaveHandler.FileExists(i));
			}
			else
			{
				choice[i] = new ChoiceContent(new GUIContent(SaveHandler.GetFileInfo(i), this.fileIcon));
			}
		}
		return choice;
	}
	
	/*
	============================================================================
	Auto save functions
	============================================================================
	*/
	public string GetAutoName()
	{
		return this.autoFileName[GameHandler.GetLanguage()];
	}
	
	public void ShowAutoSaveMessage()
	{
		GameHandler.GetLevelHandler().ShowInfo(
				this.autoSaveMessage[GameHandler.GetLanguage()], 
				this.autoMessagePosition, this.autoVisibilityTime);
	}
	
	/*
	============================================================================
	Save file type functions
	============================================================================
	*/
	public bool IsPlayerPrefs()
	{
		return SaveGameType.PLAYER_PREFS.Equals(this.saveGameType);
	}
	
	public bool IsFile()
	{
		return SaveGameType.FILE.Equals(this.saveGameType);
	}
	
	/*
	============================================================================
	Save data functions
	============================================================================
	*/
	public bool CanSaveGameVariable(string key)
	{
		bool save = Selector.ALL.Equals(this.gameVariableSelector);
		for(int i=0; i<this.gameVariableList.Length; i++)
		{
			if(this.gameVariableList[i] == key)
			{
				save = !save;
				break;
			}
		}
		return save;
	}
	
	public bool CanSaveNumberVariable(string key)
	{
		bool save = Selector.ALL.Equals(this.numberVariableSelector);
		for(int i=0; i<this.numberVariableList.Length; i++)
		{
			if(this.numberVariableList[i] == key)
			{
				save = !save;
				break;
			}
		}
		return save;
	}
}