
using System.IO;
using System.Collections;
using UnityEngine;

public class SaveHandler
{
	private static SaveHandler instance;
	
	private ArrayList addonSavers = new ArrayList();
	
	private string[] fileInfos;
	private string autoInfo;
	private bool filesChanged = false;
	
	private string retryData = "";
	private int lastIndex = -2;
	
	private static string SAVEGAMES = "savegames";
	private static string SAVEGAME = "savegame";
	private static string SCENE = "scene";
	private static string ITEMS = "items";
	private static string WEAPONS = "weapons";
	private static string ARMORS = "armors";
	private static string VARIABLES = "variables";
	private static string NUMBERVARIABLES = "numbervariables";
	private static string PARTY = "party";
	private static string DROPS = "drops";
	private static string ADDONS = "addons";
	private static string STATISTICS = "statistics";
	
	public static int AUTOSAVE_INDEX = -1;
	public static int RETRY_INDEX = -2;
	
	private SaveHandler()
	{
		if(instance != null)
		{
			Debug.Log("There is already an instance of SaveHandler!");
			return;
		}
		instance = this;
	}
	
	public static SaveHandler Instance()
	{
		if(instance == null)
		{
			new SaveHandler();
		}
		return instance;
	}
	
	// addon savers
	public static void RegisterAddonSaver(AddonSaver s)
	{
		SaveHandler.Instance().addonSavers.Add(s);
	}
	
	public static void SaveGame(int index)
	{
		SaveHandler.Instance().filesChanged = true;
		ArrayList data = new ArrayList();
		ArrayList subs = new ArrayList();
		Hashtable sv = new Hashtable();
		
		sv.Add(XMLHandler.NODE_NAME, SaveHandler.SAVEGAMES);
		
		Hashtable ht = new Hashtable();
		ArrayList s = new ArrayList();
		
		ht.Add(XMLHandler.NODE_NAME, SaveHandler.SAVEGAME);
		ht.Add("language", GameHandler.GetLanguage().ToString());
		ht.Add("difficulty", GameHandler.GetDifficulty().ToString());
		
		if(DataHolder.LoadSaveHUD().savePartyPosition)
		{
			s.Add(HashtableHelper.GetContentHashtable(SaveHandler.SCENE, 
					Application.loadedLevelName));
			ht.Add("areaname", GameHandler.GetAreaNameID().ToString());
		}
		
		if(DataHolder.LoadSaveHUD().saveTime)
		{
			ht.Add("gametime", GameHandler.GetGameTime().ToString());
		}
		if(DataHolder.LoadSaveHUD().saveMoney)
		{
			ht.Add("money", GameHandler.GetMoney().ToString());
		}
		
		if(GameHandler.GetMusicHandler().IsPlaying() && 
			DataHolder.LoadSaveHUD().savePartyPosition)
		{
			ht.Add("musicid", GameHandler.GetMusicHandler().GetCurrentID().ToString());
			ht.Add("musictime", GameHandler.GetMusicHandler().GetCurrentTime().ToString());
		}
		
		Camera cam = Camera.main;
		if(cam && DataHolder.LoadSaveHUD().savePartyPosition)
		{
			ht.Add("camx", cam.transform.position.x.ToString());
			ht.Add("camy", cam.transform.position.y.ToString());
			ht.Add("camz", cam.transform.position.z.ToString());
			ht.Add("camrotx", cam.transform.eulerAngles.x.ToString());
			ht.Add("camroty", cam.transform.eulerAngles.y.ToString());
			ht.Add("camrotz", cam.transform.eulerAngles.z.ToString());
			ht.Add("camfov", cam.fieldOfView.ToString());
		}
		
		if(DataHolder.LoadSaveHUD().saveRecipes)
		{
			ht = GameHandler.GetItemRecipeSaveData(ht);
		}
		
		if(DataHolder.LoadSaveHUD().saveItems)
		{
			s.Add(GameHandler.GetItemSaveData(
					HashtableHelper.GetTitleHashtable(SaveHandler.ITEMS)));
		}
		
		if(DataHolder.LoadSaveHUD().saveWeapons)
		{
			s.Add(GameHandler.GetWeaponSaveData(
					HashtableHelper.GetTitleHashtable(SaveHandler.WEAPONS)));
		}
		
		if(DataHolder.LoadSaveHUD().saveArmors)
		{
			s.Add(GameHandler.GetArmorSaveData(
					HashtableHelper.GetTitleHashtable(SaveHandler.ARMORS)));
		}
		
		if(!Selector.NONE.Equals(DataHolder.LoadSaveHUD().gameVariableSelector))
		{
			s.Add(GameHandler.GetVariableSaveData(
					HashtableHelper.GetTitleHashtable(SaveHandler.VARIABLES)));
		}
		
		if(!Selector.NONE.Equals(DataHolder.LoadSaveHUD().numberVariableSelector))
		{
			s.Add(GameHandler.GetNumberVariableSaveData(
					HashtableHelper.GetTitleHashtable(SaveHandler.NUMBERVARIABLES)));
		}
		
		if(DataHolder.LoadSaveHUD().saveParty)
		{
			s.Add(GameHandler.Party().GetSaveData(
					HashtableHelper.GetTitleHashtable(SaveHandler.PARTY)));
		}
		
		if(DataHolder.LoadSaveHUD().savePartyPosition)
		{
			s.Add(DataHolder.Statistic.GetSaveData(
					HashtableHelper.GetTitleHashtable(SaveHandler.STATISTICS)));
		}
		
		if(DataHolder.GameSettings().saveDrops)
		{
			s.Add(GameHandler.DropHandler().GetData(SaveHandler.DROPS));
		}
		
		ArrayList ss = new ArrayList();
		foreach(AddonSaver adSav in SaveHandler.Instance().addonSavers)
		{
			Hashtable ht2 = adSav.GetData();
			if(ht2 != null) ss.Add(ht2);
		}
		if(ss.Count > 0)
		{
			Hashtable ads = new Hashtable();
			ads.Add(XMLHandler.NODE_NAME, SaveHandler.ADDONS);
			ads.Add(XMLHandler.NODES, ss);
			s.Add(ads);
		}
		
		ht.Add(XMLHandler.NODES, s);
		subs.Add(ht);
		sv.Add(XMLHandler.NODES, subs);
		
		data.Add(sv);
		SaveHandler.SaveFile(index, XMLHandler.ParseArrayList(data));
	}
	
	public static void LoadGame(int index)
	{
		SaveHandler.Instance().lastIndex = index;
		ArrayList data = SaveHandler.LoadFile(index);
		if(data.Count > 0)
		{
			foreach(Hashtable entry in data)
			{
				if(entry[XMLHandler.NODE_NAME] as string == SaveHandler.SAVEGAMES)
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList subs = entry[XMLHandler.NODES] as ArrayList;
						foreach(Hashtable ht in subs)
						{
							if(ht[XMLHandler.NODE_NAME] as string == SaveHandler.SAVEGAME)
							{
								bool loaded = false;
								if(ht.ContainsKey("scene"))
								{
									loaded = true;
									SaveHandler.LoadScene((string)ht["scene"], ht, -1);
									return;
								}
								else if(ht.ContainsKey(XMLHandler.NODES))
								{
									ArrayList s = ht[XMLHandler.NODES] as ArrayList;
									foreach(Hashtable ht2 in s)
									{
										if(ht2[XMLHandler.NODE_NAME] as string == SaveHandler.SCENE)
										{
											loaded = true;
											SaveHandler.LoadScene((string)ht2[XMLHandler.CONTENT], ht, -1);
											return;
										}
									}
								}
								if(!loaded)
								{
									SaveHandler.LoadScene(DataHolder.LoadSaveHUD().saveSceneName,
											ht, DataHolder.LoadSaveHUD().saveSpawnID);
								}
							}
						}
					}
				}
			}
		}
	}
	
	private static void LoadScene(string scene, Hashtable ht, int sID)
	{
		GameHandler.GetLevelHandler().interactionList = new ArrayList();
		GameObject tmp = new GameObject();
		SceneChanger changer = tmp.AddComponent<SceneChanger>();
		changer.LoadSaveGame(scene, ht, sID);
		changer.ChangeScene();
	}
	
	public static void LoadSaveGame(Hashtable ht)
	{
		GameHandler.Instance().ClearData();
		GameHandler.ChangeHappened(2, 0, 0);
		if(ht.ContainsKey("language")) GameHandler.SetLanguage(int.Parse((string)ht["language"]));
		if(ht.ContainsKey("difficulty")) GameHandler.SetDifficulty(int.Parse((string)ht["difficulty"]));
		if(ht.ContainsKey("gametime")) GameHandler.SetGameTime(float.Parse((string)ht["gametime"]));
		if(ht.ContainsKey("areaname")) GameHandler.SetAreaNameID(int.Parse((string)ht["areaname"]));
		if(ht.ContainsKey("money")) GameHandler.SetMoney(int.Parse((string)ht["money"]));
		GameHandler.SetItemRecipeSaveData(ht);
		
		if(ht.ContainsKey(XMLHandler.NODES))
		{
			ArrayList s = ht[XMLHandler.NODES] as ArrayList;
			foreach(Hashtable ht2 in s)
			{
				if(ht2[XMLHandler.NODE_NAME] as string == SaveHandler.ITEMS)
				{
					GameHandler.SetItemSaveData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == SaveHandler.WEAPONS)
				{
					GameHandler.SetWeaponSaveData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == SaveHandler.ARMORS)
				{
					GameHandler.SetArmorSaveData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == SaveHandler.VARIABLES)
				{
					GameHandler.SetVariableSaveData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == SaveHandler.NUMBERVARIABLES)
				{
					GameHandler.SetNumberVariableSaveData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == SaveHandler.PARTY)
				{
					GameHandler.Party().SetSaveData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == SaveHandler.DROPS)
				{
					GameHandler.DropHandler().SetData(ht2);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == SaveHandler.ADDONS)
				{
					ArrayList ss = ht2[XMLHandler.NODES] as ArrayList;
					foreach(Hashtable ht3 in ss)
					{
						foreach(AddonSaver adSav in SaveHandler.Instance().addonSavers)
						{
							adSav.SetData(ht3);
						}
					}
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == SaveHandler.STATISTICS)
				{
					DataHolder.Statistic.SetSaveData(ht2);
				}
			}
		}
		if(ht.ContainsKey("musicid"))
		{
			GameHandler.GetMusicHandler().Stop();
			GameHandler.GetMusicHandler().PlayFromTime(int.Parse((string)ht["musicid"]), float.Parse((string)ht["musictime"]));
		}
		GameHandler.GetLevelHandler().InitGlobalEvents();
	}
	
	public static void SetCamPos(Hashtable ht)
	{
		Camera cam = Camera.main;
		if(cam && ht.ContainsKey("camx"))
		{
			cam.transform.position = new Vector3(
					float.Parse((string)ht["camx"]),
					float.Parse((string)ht["camy"]),
					float.Parse((string)ht["camz"]));
			cam.transform.eulerAngles = new Vector3(
					float.Parse((string)ht["camrotx"]),
					float.Parse((string)ht["camroty"]),
					float.Parse((string)ht["camrotz"]));
			cam.fieldOfView = float.Parse((string)ht["camfov"]);
		}
	}
	
	public static string GetFileInfo(int index)
	{
		if(SaveHandler.Instance().filesChanged ||
			SaveHandler.Instance().fileInfos == null || 
			(index >= 0 && SaveHandler.Instance().fileInfos[index] == null) ||
			SaveHandler.Instance().autoInfo == null)
		{
			SaveHandler.CreateInfos();
		}
		if(index == SaveHandler.AUTOSAVE_INDEX) return SaveHandler.Instance().autoInfo;
		else return SaveHandler.Instance().fileInfos[index];
	}
	
	public static void CreateInfos()
	{
		SaveHandler.Instance().filesChanged = false;
		SaveHandler.Instance().fileInfos = new string[DataHolder.LoadSaveHUD().maxSaveGames];
		for(int i=0; i<DataHolder.LoadSaveHUD().maxSaveGames; i++)
		{
			SaveHandler.Instance().fileInfos[i] = SaveHandler.Instance().CreateFileInfo(i);
		}
		SaveHandler.Instance().autoInfo = SaveHandler.Instance().CreateFileInfo(-1);
	}
	
	public static bool FileExists()
	{
		bool exists = SaveHandler.FileExists(-1);
		if(!exists)
		{
			for(int i=0; i<DataHolder.LoadSaveHUD().maxSaveGames; i++)
			{
				if(SaveHandler.FileExists(i))
				{
					exists = true;
					break;
				}
			}
		}
		return exists;
	}
	
	public string CreateFileInfo(int index)
	{
		string info = "";
		ArrayList data = SaveHandler.LoadFile(index);
		if(data.Count > 0)
		{
			info = DataHolder.LoadSaveHUD().GetFileInfoString();
			foreach(Hashtable entry in data)
			{
				if(entry[XMLHandler.NODE_NAME] as string == SaveHandler.SAVEGAMES)
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList subs = entry[XMLHandler.NODES] as ArrayList;
						foreach(Hashtable ht in subs)
						{
							if(ht[XMLHandler.NODE_NAME] as string == SaveHandler.SAVEGAME)
							{
								if(ht.ContainsKey("gametime"))
								{
									info = info.Replace("%t", GameHandler.GetTimeString(float.Parse((string)ht["gametime"])));
								}
								if(ht.ContainsKey("areaname"))
								{
									if("" != info) info += " ";
									info = info.Replace("%a", DataHolder.AreaName(int.Parse((string)ht["areaname"])));
								}
								
								ArrayList s = ht[XMLHandler.NODES] as ArrayList;
								foreach(Hashtable ht2 in s)
								{
									if(ht2[XMLHandler.NODE_NAME] as string == SaveHandler.PARTY)
									{
										if(ht2.ContainsKey("playerid"))
										{
											int id = int.Parse((string)ht2["playerid"]);
											ArrayList s2 = ht2[XMLHandler.NODES] as ArrayList;
											foreach(Hashtable ht3 in s2)
											{
												if(ht3[XMLHandler.NODE_NAME] as string == PartyHandler.ACTIVEPARTY)
												{
													if(ht3.ContainsKey(XMLHandler.NODES))
													{
														ArrayList s3 = ht3[XMLHandler.NODES] as ArrayList;
														foreach(Hashtable ht4 in s3)
														{
															if(ht4[XMLHandler.NODE_NAME] as string == PartyHandler.CHARACTER)
															{
																if(id == int.Parse((string)ht4["realid"]))
																{
																	info = info.Replace("%l", (string)ht4["level"]);
																}
																if(ht4.ContainsKey(XMLHandler.NODES))
																{
																	ArrayList s4 = ht4[XMLHandler.NODES] as ArrayList;
																	foreach(Hashtable ht5 in s4)
																	{
																		if(ht5[XMLHandler.NODE_NAME] as string == Character.NEWNAME)
																		{
																			string tmpName = ht5[XMLHandler.CONTENT] as string;
																			if(tmpName != "") info = info.Replace("%n", tmpName);
																		}
																	}
																}
															}
														}
													}
												}
											}
											info = info.Replace("%n", DataHolder.Characters().GetName(id));
										}
									}
								}
							}
						}
					}
				}
			}
		}
		else
		{
			info = DataHolder.LoadSaveHUD().GetEmptyInfoString();
		}
		if(index == SaveHandler.AUTOSAVE_INDEX) info = info.Replace("%i", DataHolder.LoadSaveHUD().GetAutoName());
		else info = info.Replace("%i", (index+1).ToString());
		return info;
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	private static void SaveFile(int index, string data)
	{
		SaveHandler.Instance().lastIndex = index;
		if(index == SaveHandler.RETRY_INDEX)
		{
			SaveHandler.Instance().retryData = data;
		}
		else
		{
			if(DataHolder.LoadSaveHUD().IsPlayerPrefs() || Application.isWebPlayer)
			{
				PlayerPrefs.SetString(SaveHandler.GetFileName(index), SecurityHandler.SaveGame(data));
			}
			else if(DataHolder.LoadSaveHUD().IsFile())
			{
				StreamWriter writer = new StreamWriter(Application.persistentDataPath+"/"+SaveHandler.GetFileName(index)+".save");
				writer.Write(SecurityHandler.SaveGame(data));
				writer.Flush();
				writer.Close();
			}
		}
	}
	
	private static ArrayList LoadFile(int index)
	{
		ArrayList data = new ArrayList();
		if(index == SaveHandler.RETRY_INDEX)
		{
			if(SaveHandler.Instance().retryData != "")
			{
				data = XMLHandler.ParseXML(SaveHandler.Instance().retryData);
			}
			else if(SaveHandler.Instance().lastIndex > SaveHandler.RETRY_INDEX)
			{
				data = SaveHandler.LoadFile(SaveHandler.Instance().lastIndex);
			}
		}
		else
		{
			if(SaveHandler.FileExists(index))
			{
				if(DataHolder.LoadSaveHUD().IsPlayerPrefs() || Application.isWebPlayer)
				{
					data = XMLHandler.ParseXML(SecurityHandler.LoadGame(PlayerPrefs.GetString(SaveHandler.GetFileName(index))));
				}
				else if(DataHolder.LoadSaveHUD().IsFile())
				{
					StreamReader reader = new StreamReader(Application.persistentDataPath+"/"+SaveHandler.GetFileName(index)+".save");
					data = XMLHandler.ParseXML(SecurityHandler.LoadGame(reader.ReadToEnd()));
					reader.Close();
				}
			}
		}
		return data;
	}
	
	public static bool FileExists(int index)
	{
		bool exists = false;
		if(DataHolder.LoadSaveHUD().IsPlayerPrefs() || Application.isWebPlayer)
		{
			exists = PlayerPrefs.HasKey(SaveHandler.GetFileName(index));
		}
		else if(DataHolder.LoadSaveHUD().IsFile())
		{
			exists = File.Exists(Application.persistentDataPath+"/"+SaveHandler.GetFileName(index)+".save");
		}
		return exists;
	}
	
	public static string GetFileName(int index)
	{
		if(index == SaveHandler.AUTOSAVE_INDEX) return "savegameAUTO";
		else return "savegame"+index;
	}
	
	public static bool RetryAvailable()
	{
		return SaveHandler.Instance().retryData != "" || 
				SaveHandler.Instance().lastIndex > SaveHandler.RETRY_INDEX;
	}
}