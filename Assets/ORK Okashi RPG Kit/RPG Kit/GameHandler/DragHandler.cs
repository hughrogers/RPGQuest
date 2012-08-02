
using UnityEngine;
using System.Collections;

public class DragHandler : MonoBehaviour
{
	private ArrayList targets = new ArrayList();
	
	private ChoiceContent drag;
	private ChoiceContent dummyDrag;
	private Vector3 mp;
	
	// double click handling
	private float lastClickTime = 0;
	private ChoiceContent clickDrag;
	
	// gui click
	private BaseSprite dragGUI;
	private DragSprite dragSprite;
	
	// drag target register
	public void RegisterDragTarget(DragTarget t)
	{
		this.targets.Add(t);
	}
	
	void Awake()
	{
		DontDestroyOnLoad(transform);
	}
	
	void Update()
	{
		Vector3 point = Vector3.zero;
		if(GameHandler.IsControlBattle() && 
			DataHolder.BattleMenu().mouseTouch.Interacted(ref point) &&
			this.clickDrag == null && this.drag == null)
		{
			this.CheckDropInteraction(point, null, true);
		}
		
		if(GUISystemType.ORK.Equals(DataHolder.GameSettings().guiSystemType))
		{
			if(Input.GetMouseButtonDown(0))
			{
				if(this.dummyDrag != null && 
					(Time.time-this.lastClickTime) > DataHolder.GameSettings().clickTimeout)
				{
					this.dummyDrag = null;
				}
				if(this.clickDrag == null)
				{
					this.dragGUI = GameHandler.GUIHandler().ClickOnGUI(this.GetMousePosition());
				}
				else
				{
					if(this.CheckDropInteraction(Input.mousePosition, this.clickDrag, false))
					{
						GameHandler.ChangeHappened(0, 0, 0);
					}
					else
					{
						DataHolder.GameSettings().PlayFailAudio(GameHandler.GetLevelHandler().audio);
					}
					this.clickDrag = null;
					if(this.dragSprite != null) this.dragSprite.RemoveSprite();
				}
			}
			else if(this.dragGUI != null && Input.GetMouseButtonUp(0))
			{
				this.dragGUI.ReleaseClick(this.GetMousePosition());
				this.dragGUI = null;
				this.DropDrag();
			}
			else if(Input.GetMouseButton(0))
			{
				if(this.dragGUI != null) this.dragGUI.Drag();
				if(this.dragSprite != null) this.dragSprite.SetPosition(this.GetMousePosition());
				if(this.drag == null && this.dummyDrag != null &&
					this.mp != Input.mousePosition)
				{
					if(this.dummyDrag.dragable)
					{
						this.drag = this.dummyDrag;
						this.dragSprite = GameHandler.GUIHandler().AddDragSprite(this.drag);
					}
					this.dummyDrag = null;
				}
			}
			else if(this.dragSprite != null)
			{
				this.dragSprite.SetPosition(this.GetMousePosition());
			}
		}
		else
		{
			// drag+drop
			if(Input.GetMouseButtonDown(0))
			{
				if(this.clickDrag == null)
				{
					this.GetDrag();
					this.mp = Input.mousePosition;
					if(this.dummyDrag != null && this.dummyDrag.doubleClick && 
						(Time.time-this.lastClickTime) < DataHolder.GameSettings().clickTimeout)
					{
						DataHolder.GameSettings().PlayAcceptAudio(GameHandler.GetLevelHandler().audio);
						this.clickDrag = this.dummyDrag;
						this.dummyDrag = null;
					}
					if(this.dummyDrag != null) this.lastClickTime = Time.time;
				}
				else
				{
					if(this.CheckDropInteraction(Input.mousePosition, this.clickDrag, false))
					{
						GameHandler.ChangeHappened(0, 0, 0);
					}
					else
					{
						DataHolder.GameSettings().PlayFailAudio(GameHandler.GetLevelHandler().audio);
					}
					this.clickDrag = null;
				}
			}
			else if(Input.GetMouseButtonUp(0))
			{
				this.DropDrag();
			}
			else if(this.drag == null && this.dummyDrag != null &&
				Input.GetMouseButton(0) && this.mp != Input.mousePosition)
			{
				if(this.dummyDrag.dragable) this.drag = this.dummyDrag;
				this.dummyDrag = null;
			}
		}
		
		if((Input.GetMouseButtonUp(0) || Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended) &
			this.drag == null && this.dummyDrag == null)
		{
			if(!GameHandler.GetLevelHandler().CheckHUDClick(this.GetMousePosition()) && !Input.GetMouseButtonUp(0))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
				RaycastHit hit;
				if(Physics.Raycast(ray, out hit))
				{
					BaseInteraction[] interactions = hit.transform.gameObject.GetComponentsInChildren<BaseInteraction>();
					foreach(BaseInteraction interaction in interactions)
					{
						interaction.TouchInteract();
					}
				}
			}
		}
	}
	
	private Vector2 GetMousePosition()
	{
		Vector3 hlp = Input.mousePosition;
		hlp.y = Screen.height - hlp.y;
		hlp = GameHandler.GetLevelHandler().revertMatrix.MultiplyPoint3x4(hlp);
		return new Vector2(hlp.x, hlp.y);
	}
	
	public void SetDrag(ChoiceContent cc)
	{
		this.dummyDrag = cc;
		this.mp = Input.mousePosition;
		if(this.dummyDrag != null && this.dummyDrag.doubleClick && 
			(Time.time-this.lastClickTime) < DataHolder.GameSettings().clickTimeout)
		{
			DataHolder.GameSettings().PlayAcceptAudio(GameHandler.GetLevelHandler().audio);
			this.clickDrag = this.dummyDrag;
			this.dummyDrag = null;
			this.dragSprite = GameHandler.GUIHandler().AddDragSprite(this.clickDrag);
		}
		if(this.dummyDrag != null) this.lastClickTime = Time.time;
	}
	
	private void GetDrag()
	{
		Vector2 pos = this.GetMousePosition();
		foreach(DragTarget t in this.targets)
		{
			this.dummyDrag = t.DragStarted(pos);
			if(this.dummyDrag != null) break;
		}
		if(this.dummyDrag == null)
		{
			this.dummyDrag = GameHandler.GetLevelHandler().BattleDragStarted(pos);
		}
	}
	
	private void DropDrag()
	{
		if(this.drag != null)
		{
			bool found = false;
			Vector2 pos = this.GetMousePosition();
			foreach(DragTarget t in this.targets)
			{
				found = t.DragDropped(this.drag, pos);
				if(found) break;
			}
			if(!found && (DragOrigin.INVENTORY.Equals(this.drag.dragOrigin) ||
				DragOrigin.BATTLE_MENU.Equals(this.drag.dragOrigin)))
			{
				found = this.CheckDropInteraction(Input.mousePosition, this.drag, false);
				if(!found && DragOrigin.INVENTORY.Equals(this.drag.dragOrigin))
				{
					if(DragType.ITEM.Equals(this.drag.dragType) && DataHolder.Item(this.drag.dragID).dropable)
					{
						GameHandler.DropFromInventory(ItemDropType.ITEM, this.drag.dragID, 1);
					}
					else if(DragType.WEAPON.Equals(this.drag.dragType) && DataHolder.Weapon(this.drag.dragID).dropable)
					{
						GameHandler.DropFromInventory(ItemDropType.WEAPON, this.drag.dragID, 1);
					}
					else if(DragType.ARMOR.Equals(this.drag.dragType) && DataHolder.Armor(this.drag.dragID).dropable)
					{
						GameHandler.DropFromInventory(ItemDropType.ARMOR, this.drag.dragID, 1);
					}
				}
				else if(!found)
				{
					DataHolder.GameSettings().PlayFailAudio(GameHandler.GetLevelHandler().audio);
				}
			}
			this.drag = null;
			if(this.dragSprite != null) this.dragSprite.RemoveSprite();
			GameHandler.ChangeHappened(0, 0, 0);
		}
	}
	
	private bool CheckDropInteraction(Vector3 point, ChoiceContent cc, bool force)
	{
		bool found = false;
		if(cc != null || force)
		{
			Ray ray = Camera.main.ScreenPointToRay(point);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit))
			{
				DropInteraction[] interactions = hit.transform.root.transform.GetComponentsInChildren<DropInteraction>();
				foreach(DropInteraction interaction in interactions)
				{
					if(interaction != null)
					{
						found = interaction.DropInteract(cc);
						if(found) break;
					}
				}
			}
		}
		if(found) DataHolder.GameSettings().PlayAcceptAudio(GameHandler.GetLevelHandler().audio);
		return found;
	}
	
	public void DrawDrag(Matrix4x4 guiMatrix)
	{
		if(this.drag != null || this.clickDrag != null)
		{
			GUI.matrix = guiMatrix;
			GUI.depth = 0;
			Vector2 pos = this.GetMousePosition();
			if(this.drag != null) this.drag.DrawDrag(pos.x, pos.y);
			else if(this.clickDrag != null) this.clickDrag.DrawDrag(pos.x, pos.y);
		}
	}
}
