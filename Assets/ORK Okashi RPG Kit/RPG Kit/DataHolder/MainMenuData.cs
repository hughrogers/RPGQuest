
using System.Collections;
using UnityEngine;

public class MainMenuData
{
	// general settings
	public int menuPosition = 0;
	public int aboutPosition = 0;
	
	public bool fadeOut = true;
	public float fadeOutTime = 0.5f;
	public EaseType fadeOutInterpolate = EaseType.Linear;
	
	public bool fadeIn = true;
	public float fadeInTime = 0.5f;
	public EaseType fadeInInterpolate = EaseType.Linear;
	
	// return to main menu
	public string mainMenuScene = "";
	public bool autoCall = false;
	public float waitTime = 0;
	public int questionPosition = 0;
	public string[] questionText = new string[0];
	public string[] questionYes = new string[0];
	public string[] questionNo = new string[0];
	
	// new game
	public string newGameScene = "";
	public bool showDifficulty = false;
	
	// show
	public bool showLoad = true;
	public bool showLanguage = true;
	public bool showAbout = true;
	public bool showExit = true;
	
	// texts
	public string[] newGameText = new string[0];
	public string[] loadGameText = new string[0];
	public string[] languageText = new string[0];
	public string[] aboutText = new string[0];
	public string[] exitText = new string[0];
	public string[] cancelText = new string[0];
	public string[] aboutInfo = new string[0];
	public string[] difficultyQuestion = new string[0];
	
	// icons
	public string newIconName = "";
	public Texture2D newIcon;
	public string loadIconName = "";
	public Texture2D loadIcon;
	public string languageIconName = "";
	public Texture2D languageIcon;
	public string aboutIconName = "";
	public Texture2D aboutIcon;
	public string exitIconName = "";
	public Texture2D exitIcon;
	
	public string iconPath = "Icons/MainMenu/";
	public string skinPath = "HUD/";
	
	// XML data
	private string dir = "ProjectSettings/";
	private string filename = "mainMenu";
	
	private static string MAINMENUS = "mainmenus";
	private static string MAINMENU = "mainmenu";
	private static string NEWGAMESCENE = "newgamescene";
	private static string NEWGAMETEXT = "newgametext";
	private static string LOADGAMETEXT = "loadgametext";
	private static string LANGUAGETEXT = "languagetext";
	private static string ABOUTTEXT = "abouttext";
	private static string EXITTEXT = "exittext";
	private static string CANCELTEXT = "canceltext";
	private static string ABOUTINFO = "aboutinfo";
	private static string MENUSCENE = "menuscene";
	private static string QUESTION = "question";
	private static string YES = "yes";
	private static string NO = "no";
	private static string DIFFICULTYQUESTION = "difficultyquestion";

	public MainMenuData()
	{
		LoadData();
	}
	
	public void LoadData()
	{
		ArrayList data = XMLHandler.LoadXML(dir+filename);
		
		int langs = DataHolder.Languages().GetDataCount();
		questionText = new string[langs];
		questionYes = new string[langs];
		questionNo = new string[langs];
		newGameText = new string[langs];
		loadGameText = new string[langs];
		languageText = new string[langs];
		aboutText = new string[langs];
		exitText = new string[langs];
		cancelText = new string[langs];
		aboutInfo = new string[langs];
		difficultyQuestion = new string[langs];
		
		for(int i=0; i<langs; i++)
		{
			questionText[i] = "Quit game?";
			questionYes[i] = "Yes";
			questionNo[i] = "No";
			newGameText[i] = "New game";
			loadGameText[i] = "Load game";
			languageText[i] = "Language";
			aboutText[i] = "About";
			exitText[i] = "Exit";
			cancelText[i] = "Cancel";
			aboutInfo[i] = "";
			difficultyQuestion[i] = "";
		}
		
		if(data.Count > 0)
		{
			foreach(Hashtable entry in data)
			{
				if(entry[XMLHandler.NODE_NAME] as string == MainMenuData.MAINMENUS)
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList subs = entry[XMLHandler.NODES] as ArrayList;
						foreach(Hashtable val in subs)
						{
							if(val[XMLHandler.NODE_NAME] as string == MainMenuData.MAINMENU)
							{
								if(val.ContainsKey("menuposition")) menuPosition = int.Parse((string)val["menuposition"]);
								if(val.ContainsKey("aboutposition")) aboutPosition = int.Parse((string)val["aboutposition"]);
								if(val.ContainsKey("questionposition")) questionPosition = int.Parse((string)val["questionposition"]);
								
								if(val.ContainsKey("newicon")) newIconName = val["newicon"] as string;
								if(val.ContainsKey("loadicon")) loadIconName = val["loadicon"] as string;
								if(val.ContainsKey("languageicon")) languageIconName = val["languageicon"] as string;
								if(val.ContainsKey("abouticon")) aboutIconName = val["abouticon"] as string;
								if(val.ContainsKey("exiticon")) exitIconName = val["exiticon"] as string;
								
								if(val.ContainsKey("showload")) showLoad = bool.Parse((string)val["showload"]);
								if(val.ContainsKey("showlanguage")) showLanguage = bool.Parse((string)val["showlanguage"]);
								if(val.ContainsKey("showabout")) showAbout = bool.Parse((string)val["showabout"]);
								if(val.ContainsKey("showexit")) showExit = bool.Parse((string)val["showexit"]);
								
								if(val.ContainsKey("fadeout")) fadeOut = bool.Parse((string)val["fadeout"]);
								if(val.ContainsKey("fadeouttime")) fadeOutTime = float.Parse((string)val["fadeouttime"]);
								if(val.ContainsKey("fadeoutinterpolate")) fadeOutInterpolate = (EaseType)System.Enum.Parse(typeof(EaseType), (string)val["fadeoutinterpolate"]);
								if(val.ContainsKey("fadein")) fadeIn = bool.Parse((string)val["fadein"]);
								if(val.ContainsKey("fadeintime")) fadeInTime = float.Parse((string)val["fadeintime"]);
								if(val.ContainsKey("fadeininterpolate")) fadeInInterpolate = (EaseType)System.Enum.Parse(typeof(EaseType), (string)val["fadeininterpolate"]);
								
								if(val.ContainsKey("waittime"))
								{
									this.autoCall = true;
									this.waitTime = float.Parse((string)val["waittime"]);
								}
								
								if(val.ContainsKey("showdifficulty")) this.showDifficulty = true;
								
								ArrayList s = val[XMLHandler.NODES] as ArrayList;
								foreach(Hashtable ht in s)
								{
									if(ht[XMLHandler.NODE_NAME] as string == MainMenuData.NEWGAMETEXT)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < langs) this.newGameText[id] = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == MainMenuData.LOADGAMETEXT)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < langs) this.loadGameText[id] = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == MainMenuData.LANGUAGETEXT)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < langs) this.languageText[id] = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == MainMenuData.ABOUTTEXT)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < langs) this.aboutText[id] = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == MainMenuData.EXITTEXT)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < langs) this.exitText[id] = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == MainMenuData.CANCELTEXT)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < langs) this.cancelText[id] = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == MainMenuData.ABOUTINFO)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < langs) this.aboutInfo[id] = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == MainMenuData.DIFFICULTYQUESTION)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < langs) this.difficultyQuestion[id] = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == MainMenuData.NEWGAMESCENE)
									{
										this.newGameScene = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == MainMenuData.MENUSCENE)
									{
										this.mainMenuScene = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == MainMenuData.QUESTION)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < langs) this.questionText[id] = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == MainMenuData.YES)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < langs) this.questionYes[id] = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == MainMenuData.NO)
									{
										int id = int.Parse((string)ht["id"]);
										if(id < langs) this.questionNo[id] = ht[XMLHandler.CONTENT] as string;
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
		
		sv.Add(XMLHandler.NODE_NAME, MainMenuData.MAINMENUS);
		
		Hashtable ht = new Hashtable();
		ArrayList s = new ArrayList();
		
		ht.Add(XMLHandler.NODE_NAME, MainMenuData.MAINMENU);
		ht.Add("menuposition", menuPosition.ToString());
		ht.Add("aboutposition", aboutPosition.ToString());
		ht.Add("questionposition", questionPosition.ToString());
		if("" != newIconName) ht.Add("newicon", newIconName);
		if("" != loadIconName) ht.Add("loadicon", loadIconName);
		if("" != languageIconName) ht.Add("languageicon", languageIconName);
		if("" != aboutIconName) ht.Add("abouticon", aboutIconName);
		if("" != exitIconName) ht.Add("exiticon", exitIconName);
		
		if(this.showDifficulty) ht.Add("showdifficulty", "true");
		
		ht.Add("showload", showLoad.ToString());
		ht.Add("showlanguage", showLanguage.ToString());
		ht.Add("showabout", showAbout.ToString());
		ht.Add("showexit", showExit.ToString());
		
		ht.Add("fadeout", fadeOut.ToString());
		ht.Add("fadeouttime", fadeOutTime.ToString());
		ht.Add("fadeoutinterpolate", fadeOutInterpolate.ToString());
		ht.Add("fadein", fadeIn.ToString());
		ht.Add("fadeintime", fadeInTime.ToString());
		ht.Add("fadeininterpolate", fadeInInterpolate.ToString());
		
		if(this.autoCall)
		{
			ht.Add("waittime", this.waitTime.ToString());
		}
		
		for(int i=0; i<newGameText.Length; i++)
		{
			Hashtable n = new Hashtable();
			n.Add(XMLHandler.NODE_NAME, MainMenuData.NEWGAMETEXT);
			n.Add("id", i.ToString());
			n.Add(XMLHandler.CONTENT, newGameText[i]);
			s.Add(n);
		}
		for(int i=0; i<loadGameText.Length; i++)
		{
			Hashtable n = new Hashtable();
			n.Add(XMLHandler.NODE_NAME, MainMenuData.LOADGAMETEXT);
			n.Add("id", i.ToString());
			n.Add(XMLHandler.CONTENT, loadGameText[i]);
			s.Add(n);
		}
		for(int i=0; i<languageText.Length; i++)
		{
			Hashtable n = new Hashtable();
			n.Add(XMLHandler.NODE_NAME, MainMenuData.LANGUAGETEXT);
			n.Add("id", i.ToString());
			n.Add(XMLHandler.CONTENT, languageText[i]);
			s.Add(n);
		}
		for(int i=0; i<aboutText.Length; i++)
		{
			Hashtable n = new Hashtable();
			n.Add(XMLHandler.NODE_NAME, MainMenuData.ABOUTTEXT);
			n.Add("id", i.ToString());
			n.Add(XMLHandler.CONTENT, aboutText[i]);
			s.Add(n);
		}
		for(int i=0; i<exitText.Length; i++)
		{
			Hashtable n = new Hashtable();
			n.Add(XMLHandler.NODE_NAME, MainMenuData.EXITTEXT);
			n.Add("id", i.ToString());
			n.Add(XMLHandler.CONTENT, exitText[i]);
			s.Add(n);
		}
		for(int i=0; i<cancelText.Length; i++)
		{
			Hashtable n = new Hashtable();
			n.Add(XMLHandler.NODE_NAME, MainMenuData.CANCELTEXT);
			n.Add("id", i.ToString());
			n.Add(XMLHandler.CONTENT, cancelText[i]);
			s.Add(n);
		}
		for(int i=0; i<aboutInfo.Length; i++)
		{
			Hashtable n = new Hashtable();
			n.Add(XMLHandler.NODE_NAME, MainMenuData.ABOUTINFO);
			n.Add("id", i.ToString());
			n.Add(XMLHandler.CONTENT, aboutInfo[i]);
			s.Add(n);
		}
		for(int i=0; i<questionText.Length; i++)
		{
			Hashtable n = new Hashtable();
			n.Add(XMLHandler.NODE_NAME, MainMenuData.QUESTION);
			n.Add("id", i.ToString());
			n.Add(XMLHandler.CONTENT, questionText[i]);
			s.Add(n);
		}
		for(int i=0; i<questionYes.Length; i++)
		{
			Hashtable n = new Hashtable();
			n.Add(XMLHandler.NODE_NAME, MainMenuData.YES);
			n.Add("id", i.ToString());
			n.Add(XMLHandler.CONTENT, questionYes[i]);
			s.Add(n);
		}
		for(int i=0; i<questionNo.Length; i++)
		{
			Hashtable n = new Hashtable();
			n.Add(XMLHandler.NODE_NAME, MainMenuData.NO);
			n.Add("id", i.ToString());
			n.Add(XMLHandler.CONTENT, questionNo[i]);
			s.Add(n);
		}
		for(int i=0; i<difficultyQuestion.Length; i++)
		{
			Hashtable n = new Hashtable();
			n.Add(XMLHandler.NODE_NAME, MainMenuData.DIFFICULTYQUESTION);
			n.Add("id", i.ToString());
			n.Add(XMLHandler.CONTENT, difficultyQuestion[i]);
			s.Add(n);
		}
		
		Hashtable ht2 = new Hashtable();
		ht2.Add(XMLHandler.NODE_NAME, MainMenuData.NEWGAMESCENE);
		ht2.Add(XMLHandler.CONTENT, newGameScene);
		s.Add(ht2);
		
		ht2 = new Hashtable();
		ht2.Add(XMLHandler.NODE_NAME, MainMenuData.MENUSCENE);
		ht2.Add(XMLHandler.CONTENT, mainMenuScene);
		s.Add(ht2);
		
		ht.Add(XMLHandler.NODES, s);
		subs.Add(ht);
		sv.Add(XMLHandler.NODES, subs);
		data.Add(sv);
		XMLHandler.SaveXML(dir, filename, data);
	}
	
	public void AddLanguage(int lang)
	{
		newGameText = ArrayHelper.Add("New game", newGameText);
		loadGameText = ArrayHelper.Add("Load game", loadGameText);
		languageText = ArrayHelper.Add("Language", languageText);
		aboutText = ArrayHelper.Add("About", aboutText);
		exitText = ArrayHelper.Add("Exit", exitText);
		cancelText = ArrayHelper.Add("cancel", cancelText);
		aboutInfo = ArrayHelper.Add("", aboutInfo);
		difficultyQuestion = ArrayHelper.Add("", difficultyQuestion);
	}
	
	public void RemoveLanguage(int lang)
	{
		newGameText = ArrayHelper.Remove(lang, newGameText);
		loadGameText = ArrayHelper.Remove(lang, loadGameText);
		languageText = ArrayHelper.Remove(lang, languageText);
		aboutText = ArrayHelper.Remove(lang, aboutText);
		exitText = ArrayHelper.Remove(lang, exitText);
		cancelText = ArrayHelper.Remove(lang, cancelText);
		aboutInfo = ArrayHelper.Remove(lang, aboutInfo);
		difficultyQuestion = ArrayHelper.Remove(lang, difficultyQuestion);
	}
	
	public void LoadResources()
	{
		if("" != this.newIconName)
		{
			this.newIcon = (Texture2D)Resources.Load(this.iconPath+this.newIconName, typeof(Texture2D));
		}
		if("" != this.loadIconName)
		{
			this.loadIcon = (Texture2D)Resources.Load(this.iconPath+this.loadIconName, typeof(Texture2D));
		}
		if("" != this.languageIconName)
		{
			this.languageIcon = (Texture2D)Resources.Load(this.iconPath+this.languageIconName, typeof(Texture2D));
		}
		if("" != this.aboutIconName)
		{
			this.aboutIcon = (Texture2D)Resources.Load(this.iconPath+this.aboutIconName, typeof(Texture2D));
		}
		if("" != this.exitIconName)
		{
			this.exitIcon = (Texture2D)Resources.Load(this.iconPath+this.exitIconName, typeof(Texture2D));
		}
	}
	
	public string GetCancelText()
	{
		return this.cancelText[GameHandler.GetLanguage()];
	}
	
	public string GetAboutInfo()
	{
		return this.aboutInfo[GameHandler.GetLanguage()];
	}
	
	public string GetDifficultyQuestion()
	{
		return this.difficultyQuestion[GameHandler.GetLanguage()];
	}
	
	public ChoiceContent[] GetMainMenuChoice()
	{
		ChoiceContent[] choice = new ChoiceContent[] {new ChoiceContent(new GUIContent(this.newGameText[GameHandler.GetLanguage()], this.newIcon))};
		if(this.showLoad) choice = ArrayHelper.Add(new ChoiceContent(new GUIContent(this.loadGameText[GameHandler.GetLanguage()], this.loadIcon), SaveHandler.FileExists()), choice);
		if(this.showLanguage) choice = ArrayHelper.Add(new ChoiceContent(new GUIContent(this.languageText[GameHandler.GetLanguage()], this.languageIcon)), choice);
		if(this.showAbout) choice = ArrayHelper.Add(new ChoiceContent(new GUIContent(this.aboutText[GameHandler.GetLanguage()], this.aboutIcon)), choice);
		if(this.showExit) choice = ArrayHelper.Add(new ChoiceContent(new GUIContent(this.exitText[GameHandler.GetLanguage()], this.exitIcon)), choice);
		return choice;
	}
	
	private ArrayList GetChoices()
	{
		ArrayList tmp = new ArrayList();
		tmp.Add("new");
		if(this.showLoad) tmp.Add("load");
		if(this.showLanguage) tmp.Add("language");
		if(this.showAbout) tmp.Add("about");
		if(this.showExit) tmp.Add("exit");
		return tmp;
	}
	
	public bool IsChoiceLoad(int selection)
	{
		return this.GetChoices()[selection] as string == "load";
	}
	
	public bool IsChoiceLanguage(int selection)
	{
		return this.GetChoices()[selection] as string == "language";
	}
	
	public bool IsChoiceAbout(int selection)
	{
		return this.GetChoices()[selection] as string == "about";
	}
	
	public bool IsChoiceExit(int selection)
	{
		return this.GetChoices()[selection] as string == "exit";
	}
	
	// question
	public string GetReturnQuestion()
	{
		return this.questionText[GameHandler.GetLanguage()];
	}
	
	public ChoiceContent[] GetYesNoChoice()
	{
		ChoiceContent[] choice = new ChoiceContent[2];
		choice[0] = new ChoiceContent(new GUIContent(this.questionYes[GameHandler.GetLanguage()]));
		choice[1] = new ChoiceContent(new GUIContent(this.questionNo[GameHandler.GetLanguage()]));
		return choice;
	}
}