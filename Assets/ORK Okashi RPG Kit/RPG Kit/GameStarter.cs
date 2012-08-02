
using UnityEngine;

public class GameStarter : MonoBehaviour
{
	public Material guiMaterial;
	
	void Awake()
	{
		DataHolder.Instance();
		DataHolder.GameSettings().LoadResources();
		DataHolder.LoadSaveHUD().LoadResources();
		DataHolder.MainMenu().LoadResources();
		DataHolder.BattleSystemData().LoadResources();
		GameHandler.Instance();
		GameHandler.GUIHandler().SetMaterial(guiMaterial);
	}
}