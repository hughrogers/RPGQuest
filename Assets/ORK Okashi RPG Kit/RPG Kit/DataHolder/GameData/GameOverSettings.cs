
using UnityEngine;
using System.Collections;

public class GameOverSettings : QuestionInterface
{
	public string scene = "";
	
	public bool showChoice = false;
	public float waitTime = 0;
	
	public bool showRetry = true;
	public string[] retryText = new string[0];
	public string retryIconName = "";
	public Texture2D retryIcon = null;
	
	public bool showLoad = true;
	public string[] loadText = new string[0];
	public string loadIconName = "";
	public Texture2D loadIcon = null;
	
	public bool showExit = true;
	public string[] exitText = new string[0];
	public string exitIconName = "";
	public Texture2D exitIcon = null;
	
	// XML
	private static string SCENE = "scene";
	private static string RETRY = "retry";
	private static string RETRYICON = "retryicon";
	private static string LOAD = "load";
	private static string LOADICON = "loadicon";
	private static string EXIT = "exit";
	private static string EXITICON = "exiticon";
	
	// choices
	private static int RETRY_INDEX = 0;
	private static int LOAD_INDEX = 1;
	private static int EXIT_INDEX = 2;
	
	public GameOverSettings()
	{
		this.retryText = new string[DataHolder.LanguageCount];
		this.loadText = new string[this.retryText.Length];
		this.exitText = new string[this.retryText.Length];
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		ArrayList s = new ArrayList();
		
		if(this.scene != "")
		{
			s.Add(HashtableHelper.GetContentHashtable(GameOverSettings.SCENE, this.scene));
		}
		
		if(this.showChoice)
		{
			ht.Add("dialogueposition", this.dialoguePosition.ToString());
			ht.Add("waittime", this.waitTime.ToString());
			
			if(this.showRetry)
			{
				if(this.retryIconName != "")
				{
					s.Add(HashtableHelper.GetContentHashtable(GameOverSettings.RETRYICON, this.retryIconName));
				}
				HashtableHelper.AddContentHashtables(ref s, GameOverSettings.RETRY, this.retryText);
			}
			
			if(this.showLoad)
			{
				if(this.loadIconName != "")
				{
					s.Add(HashtableHelper.GetContentHashtable(GameOverSettings.LOADICON, this.loadIconName));
				}
				HashtableHelper.AddContentHashtables(ref s, GameOverSettings.LOAD, this.loadText);
			}
			
			if(this.showExit)
			{
				if(this.exitIconName != "")
				{
					s.Add(HashtableHelper.GetContentHashtable(GameOverSettings.EXITICON, this.exitIconName));
				}
				HashtableHelper.AddContentHashtables(ref s, GameOverSettings.EXIT, this.exitText);
			}
		}
		
		if(s.Count > 0) ht.Add(XMLHandler.NODES, s);
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		if(ht.ContainsKey("dialogueposition"))
		{
			this.showChoice = true;
			this.dialoguePosition = int.Parse((string)ht["dialogueposition"]);
			this.waitTime = float.Parse((string)ht["waittime"]);
		}
		
		if(ht.ContainsKey(XMLHandler.NODES))
		{
			ArrayList s = ht[XMLHandler.NODES] as ArrayList;
			foreach(Hashtable ht2 in s)
			{
				if(ht2[XMLHandler.NODE_NAME] as string == GameOverSettings.SCENE)
				{
					this.scene = ht2[XMLHandler.CONTENT] as string;
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == GameOverSettings.RETRYICON)
				{
					this.retryIconName = ht2[XMLHandler.CONTENT] as string;
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == GameOverSettings.LOADICON)
				{
					this.loadIconName = ht2[XMLHandler.CONTENT] as string;
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == GameOverSettings.EXITICON)
				{
					this.exitIconName = ht2[XMLHandler.CONTENT] as string;
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == GameOverSettings.RETRY)
				{
					HashtableHelper.GetContentString(ht2, ref this.retryText);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == GameOverSettings.LOAD)
				{
					HashtableHelper.GetContentString(ht2, ref this.loadText);
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == GameOverSettings.EXIT)
				{
					HashtableHelper.GetContentString(ht2, ref this.exitText);
				}
			}
		}
	}
	
	/*
	============================================================================
	Choice handling functions
	============================================================================
	*/
	public override void CreateChoices()
	{
		this.controlType = ControlType.SAVE;
		this.choices = new ChoiceContent[0];
		this.choiceActions = new int[0];
		if(this.showRetry)
		{
			if(this.retryIcon == null && this.retryIconName != "")
			{
				this.retryIcon = (Texture2D)Resources.Load(GameSettingsData.ICON_PATH+this.retryIconName, typeof(Texture2D));
			}
			this.choices = ArrayHelper.Add(new ChoiceContent(
					new GUIContent(this.retryText[GameHandler.GetLanguage()], this.retryIcon), 
					SaveHandler.RetryAvailable()), this.choices);
			this.choiceActions = ArrayHelper.Add(GameOverSettings.RETRY_INDEX, this.choiceActions);
		}
		if(this.showLoad)
		{
			if(this.loadIcon == null && this.loadIconName != "")
			{
				this.loadIcon = (Texture2D)Resources.Load(GameSettingsData.ICON_PATH+this.loadIconName, typeof(Texture2D));
			}
			this.choices = ArrayHelper.Add(new ChoiceContent(
					new GUIContent(this.loadText[GameHandler.GetLanguage()], this.loadIcon), 
					SaveHandler.FileExists()), this.choices);
			this.choiceActions = ArrayHelper.Add(GameOverSettings.LOAD_INDEX, this.choiceActions);
		}
		if(this.showExit)
		{
			if(this.exitIcon == null && this.exitIconName != "")
			{
				this.exitIcon = (Texture2D)Resources.Load(GameSettingsData.ICON_PATH+this.exitIconName, typeof(Texture2D));
			}
			this.choices = ArrayHelper.Add(new ChoiceContent(
					new GUIContent(this.exitText[GameHandler.GetLanguage()], this.exitIcon), true), this.choices);
			this.choiceActions = ArrayHelper.Add(GameOverSettings.EXIT_INDEX, this.choiceActions);
		}
	}
	
	public override bool ChoiceSelected(int index)
	{
		bool clear = false;
		if(this.choiceActions[index] == GameOverSettings.RETRY_INDEX)
		{
			SaveHandler.LoadGame(SaveHandler.RETRY_INDEX);
			clear = true;
		}
		else if(this.choiceActions[index] == GameOverSettings.LOAD_INDEX)
		{
			GameHandler.GetLevelHandler().CallLoadMenu();
		}
		else if(this.choiceActions[index] == GameOverSettings.EXIT_INDEX)
		{
			GameHandler.MainMenu();
			clear = true;
		}
		return clear;
	}
}
