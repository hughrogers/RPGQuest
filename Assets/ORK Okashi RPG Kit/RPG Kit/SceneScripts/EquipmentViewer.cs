
using UnityEngine;

[AddComponentMenu("RPG Kit/Controls/Equipment Viewer")]
public class EquipmentViewer : MonoBehaviour
{
	public int partID = 0;
	public bool[] controlType = new bool[System.Enum.GetNames(typeof(ControlType)).Length];
	
	public string[] childName = new string[0];
	public string[] linkTo = new string[0];
	
	private int lastID = -1;
	private EquipSet lastType = EquipSet.NONE;
	private GameObject prefabInstance;
	
	private Character character = null;
	
	private Transform[] childs = new Transform[0];
	private Transform[] links = new Transform[0];
	
	public void AddLink()
	{
		this.childName = ArrayHelper.Add("", this.childName);
		this.linkTo = ArrayHelper.Add("", this.linkTo);
	}
	
	public void RemoveLink(int index)
	{
		this.childName = ArrayHelper.Remove(index, this.childName);
		this.linkTo = ArrayHelper.Remove(index, this.linkTo);
	}
	
	void Start()
	{
		CombatantClick cc = (CombatantClick)this.transform.root.GetComponent(typeof(CombatantClick));
		if(cc == null)
		{
			cc = (CombatantClick)this.transform.root.GetComponentInChildren(typeof(CombatantClick));
		}
		if(cc != null && cc.combatant is Character)
		{
			this.character = cc.combatant as Character;
		}
		this.CheckEquipment();
	}
	
	void Update()
	{
		if(!GameHandler.IsGamePaused())
		{
			this.CheckEquipment();
		}
	}
	
	public void CheckEquipment()
	{
		if(this.character != null)
		{
			if(this.character.equipment[this.partID].type != this.lastType ||
				(this.character.equipment[this.partID].type == this.lastType && this.character.equipment[this.partID].equipID != this.lastID))
			{
				this.lastType = this.character.equipment[this.partID].type;
				this.lastID = this.character.equipment[this.partID].equipID;
				if(this.prefabInstance) GameObject.Destroy(this.prefabInstance);
				
				if(this.character.equipment[this.partID].IsWeapon())
				{
					this.prefabInstance = DataHolder.Weapon(this.lastID).GetPrefabInstance();
				}
				else if(this.character.equipment[this.partID].IsArmor())
				{
					this.prefabInstance = DataHolder.Armor(this.lastID).GetPrefabInstance();
				}
				
				if(this.prefabInstance)
				{
					this.prefabInstance.transform.position = this.transform.position;
					this.prefabInstance.transform.rotation = this.transform.rotation;
					this.prefabInstance.transform.parent = this.transform;
					
					if(this.childName.Length > 0)
					{
						this.childs = new Transform[this.childName.Length];
						this.links = new Transform[this.childName.Length];
						
						for(int i=0; i<this.childName.Length; i++)
						{
							this.childs[i] = this.prefabInstance.transform.Find(this.childName[i]);
							this.links[i] = this.transform.root.Find(this.linkTo[i]);
						}
					}
				}
			}
			if(this.prefabInstance)
			{
				if(!this.prefabInstance.active && this.IsVisible())
				{
					this.prefabInstance.SetActiveRecursively(true);
				}
				else if(this.prefabInstance.active && !this.IsVisible())
				{
					this.prefabInstance.SetActiveRecursively(false);
				}
			}
		}
	}
	
	private bool IsVisible()
	{
		bool visible = (this.controlType[(int)GameHandler.GetControlType()] || 
			(this.controlType[(int)ControlType.BATTLE] && GameHandler.IsControlBattle()));
		if(GameHandler.IsControlBattle() && !this.controlType[(int)ControlType.BATTLE])
		{
			visible = false;
		}
		return visible;
	}
	
	void LateUpdate()
	{
		if(!GameHandler.IsGamePaused() && this.childs.Length > 0)
		{
			for(int i=0; i<this.childs.Length; i++)
			{
				if(this.childs[i] != null && this.links[i] != null)
				{
					this.childs[i].position = this.links[i].position;
					this.childs[i].rotation = this.links[i].rotation;
				}
			}
		}
	}
}