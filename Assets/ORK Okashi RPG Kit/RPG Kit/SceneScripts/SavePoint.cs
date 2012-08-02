
using UnityEngine;

[AddComponentMenu("RPG Kit/Events/Save Point")]
public class SavePoint : BaseInteraction
{
	public SavePointType savePointType = SavePointType.SAVE_POINT;
	public bool destroyAfter = false;
	
	void Start()
	{
		if(EventStartType.AUTOSTART.Equals(this.startType) && this.CheckVariables())
		{
			this.CallSaveMenu();
		}
	}
	
	void Update()
	{
		if(this.KeyPress()) this.CallSaveMenu();
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(this.CheckTriggerEnter(other))
		{
			this.CallSaveMenu();
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if(this.CheckTriggerExit(other))
		{
			this.CallSaveMenu();
		}
	}
	
	public override void TouchInteract()
	{
		this.OnMouseUp();
	}
	
	void OnMouseUp()
	{
		if(EventStartType.INTERACT.Equals(this.startType) && this.CheckVariables() && 
				this.gameObject.active && DataHolder.GameSettings().IsMouseAllowed())
		{
			GameObject p = GameHandler.GetPlayer();
			if(p && Vector3.Distance(p.transform.position, this.transform.position) < this.maxMouseDistance)
			{
				this.CallSaveMenu();
			}
		}
	}
	
	public override bool Interact()
	{
		bool val = false;
		if(EventStartType.INTERACT.Equals(this.startType) && this.CheckVariables() && this.gameObject.active)
		{
			this.CallSaveMenu();
			val = true;
		}
		return val;
	}
	
	public override bool DropInteract(ChoiceContent drop)
	{
		bool val = false;
		if(EventStartType.DROP.Equals(this.startType) &&
			this.CheckVariables() && this.gameObject.active && this.CheckDrop(drop))
		{
			this.CallSaveMenu();
			val = true;
		}
		return val;
	}
	
	public void CallSaveMenu()
	{
		if(SavePointType.AUTO_SAVE.Equals(this.savePointType))
		{
			this.SetVariables();
			SaveHandler.SaveGame(SaveHandler.AUTOSAVE_INDEX);
			SaveHandler.CreateInfos();
			if(DataHolder.LoadSaveHUD().showAutoSaveMessage)
			{
				DataHolder.LoadSaveHUD().ShowAutoSaveMessage();
			}
			if(this.destroyAfter) GameObject.Destroy(this.gameObject);
		}
		else if(SavePointType.RETRY_POINT.Equals(this.savePointType))
		{
			this.SetVariables();
			SaveHandler.SaveGame(SaveHandler.RETRY_INDEX);
			if(this.destroyAfter) GameObject.Destroy(this.gameObject);
		}
		else if(SavePointType.SAVE_POINT.Equals(this.savePointType) &&
			GameHandler.IsControlField())
		{
			GameHandler.SetControlType(ControlType.SAVE);
			SaveHandler.CreateInfos();
			if(DataHolder.LoadSaveHUD().showChoice)
			{
				GameHandler.GetLevelHandler().CallSavePointMenu();
			}
			else
			{
				GameHandler.GetLevelHandler().CallSaveMenu();
			}
		}
	}
	
	void OnDrawGizmos()
	{
		Gizmos.DrawIcon(transform.position, "SavePoint.psd");
	}
}