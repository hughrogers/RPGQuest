
using System.Collections;
using UnityEngine;

[AddComponentMenu("RPG Kit/Scenes/Main Menu Call")]
public class MainMenuCall : MonoBehaviour
{
	public float wait = 0;
	
	IEnumerator Start()
	{
		if(this.wait > 0)
		{
			yield return new WaitForSeconds(this.wait);
		}
		GameHandler.GetLevelHandler().CallMainMenu();
	}
}