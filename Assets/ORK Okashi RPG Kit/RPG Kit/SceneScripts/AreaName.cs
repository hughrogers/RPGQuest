
using UnityEngine;

[AddComponentMenu("RPG Kit/Scenes/Area Name")]
public class AreaName : MonoBehaviour
{
	public int areaName = 0;
	public DialoguePosition dp;
	
	void Awake()
	{
		if(DataHolder.GameSettings().preloadAreaNames &&
			GUISystemType.ORK.Equals(DataHolder.GameSettings().guiSystemType))
		{
			GameHandler.GetLevelHandler().RegisterPreloadAN(this);
		}
	}
	
	public void Preload()
	{
		this.dp = DataHolder.DialoguePositions().GetCopy(DataHolder.GameSettings().areaNamePosition);
		this.dp.Preload("", DataHolder.AreaName(this.areaName), null, null, null);
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(GameHandler.GetPlayer() != null && other.transform == GameHandler.GetPlayer().transform)
		{
			if(this.dp == null)
			{
				GameHandler.SetAreaName(this.areaName);
			}
			else
			{
				GameHandler.SetAreaName(this.areaName, this.dp);
			}
		}
	}
	
	void OnDrawGizmos()
	{
		Gizmos.DrawIcon(transform.position, "AreaName.psd");
	}
	
	
	void OnDestroy()
	{
		if(this.dp != null) this.dp.multiLabel.DeleteTextures();
	}
}