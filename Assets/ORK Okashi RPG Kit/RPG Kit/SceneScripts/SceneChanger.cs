
using System.Collections;
using UnityEngine;

[AddComponentMenu("RPG Kit/Scenes/Scene Changer")]
public class SceneChanger : BaseInteraction
{
	public string sceneName = "";
	public int spawnID = 0;
	
	public bool fadeOut = true;
	public float fadeOutTime = 0.5f;
	public EaseType fadeOutInterpolate = EaseType.Linear;
	
	public bool fadeIn = true;
	public float fadeInTime = 0.5f;
	public EaseType fadeInInterpolate = EaseType.Linear;
	
	private bool check = false;
	private Hashtable saveGameData = null;
	
	private bool newGame = false;
	
	void OnTriggerEnter(Collider other)
	{
		if(GameHandler.IsControlField() && 
			other.gameObject == GameHandler.GetPlayer() &&
			this.CheckVariables())
		{
			this.ChangeScene();
		}
	}
	
	public void LoadSaveGame(string sc, Hashtable ht, int sID)
	{
		this.sceneName = sc;
		this.spawnID = sID;
		this.saveGameData = ht;
		this.fadeOut = DataHolder.LoadSaveHUD().fadeOut;
		this.fadeOutTime = DataHolder.LoadSaveHUD().fadeOutTime;
		this.fadeOutInterpolate = DataHolder.LoadSaveHUD().fadeOutInterpolate;
		this.fadeIn = DataHolder.LoadSaveHUD().fadeIn;
		this.fadeInTime = DataHolder.LoadSaveHUD().fadeInTime;
		this.fadeInInterpolate = DataHolder.LoadSaveHUD().fadeInInterpolate;
	}
	
	public void NewGameScene()
	{
		this.sceneName = DataHolder.MainMenu().newGameScene;
		this.spawnID = -1;
		this.fadeOut = DataHolder.MainMenu().fadeOut;
		this.fadeOutTime = DataHolder.MainMenu().fadeOutTime;
		this.fadeOutInterpolate = DataHolder.MainMenu().fadeOutInterpolate;
		this.fadeIn = DataHolder.MainMenu().fadeIn;
		this.fadeInTime = DataHolder.MainMenu().fadeInTime;
		this.fadeInInterpolate = DataHolder.MainMenu().fadeInInterpolate;
		this.newGame = true;
	}
	
	public void ChangeScene()
	{
		StartCoroutine(ChangeScene2());
	}
	
	private IEnumerator ChangeScene2()
	{
		DontDestroyOnLoad(gameObject);
		GameHandler.SetControlType(ControlType.NONE);
		if(this.fadeOut)
		{
			GameHandler.GetLevelHandler().screenFader.FadeScreen(true, 0, 1, false, 0, 0, 
					false, 0, 0, false, 0, 0, this.fadeOutInterpolate, this.fadeOutTime);
			yield return new WaitForSeconds(this.fadeOutTime);
		}
		this.check = true;
		if(this.saveGameData != null)
		{
			GameHandler.GetLevelHandler().interactionList = new ArrayList();
			Application.LoadLevel(sceneName);
		}
		else
		{
			yield return null;
			GameHandler.LoadScene(this.sceneName);
		}
	}
	
	IEnumerator OnLevelWasLoaded(int level)
	{
		if(this.check)
		{
			if(this.newGame) GameHandler.GetLevelHandler().InitGlobalEvents();
			if(this.saveGameData != null)
			{
				SaveHandler.LoadSaveGame(this.saveGameData);
				SaveHandler.SetCamPos(this.saveGameData);
			}
			// spawn for events
			if(this.spawnID >= 0)
			{
				GameHandler.SpawnPlayer(this.spawnID);
			}
			if(!GameHandler.IsControlEvent()) GameHandler.SetControlType(ControlType.FIELD);
			yield return null;
			if(this.fadeIn)
			{
				GameHandler.GetLevelHandler().screenFader.FadeScreen(true, 1, 0, false, 0, 0, 
						false, 0, 0, false, 0, 0, this.fadeInInterpolate, this.fadeInTime);
			}
			yield return null;
			GameObject.Destroy(gameObject);
		}
	}
	
	void OnDrawGizmos()
	{
		Gizmos.DrawIcon(transform.position, "SceneChanger.psd");
	}
}