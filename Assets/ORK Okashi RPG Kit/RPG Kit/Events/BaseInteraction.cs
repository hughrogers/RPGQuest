
using System.Collections;
using UnityEngine;

public class BaseInteraction : DropInteraction
{
	public EventStartType startType = EventStartType.NONE;
	public bool repeatExecution = false;
	public bool deactivateAfter = false;
	public float maxMouseDistance = 3;
	public string keyToPress = "";
	public bool keyPressInTrigger = false;
	public ItemDropType dropType = ItemDropType.ITEM;
	public int dropID = 0;
	public bool consumeDrop = false;
	
	// variable check
	public AIConditionNeeded needed = AIConditionNeeded.ALL;
	public bool autoDestroyOnVariables = true;
	public string[] variableKey = new string[0];
	public string[] variableValue = new string[0];
	public bool[] checkType = new bool[0];
	// number variables
	public string[] numberVarKey = new string[0];
	public float[] numberVarValue = new float[0];
	public bool[] numberCheckType = new bool[0];
	public ValueCheck[] numberValueCheck = new ValueCheck[0];
	
	// varialbe set
	public string[] setVariableKey = new string[0];
	public string[] setVariableValue = new string[0];
	// number variables
	public string[] setNumberVarKey = new string[0];
	public float[] setNumberVarValue = new float[0];
	public SimpleOperator[] setNumberOperator = new SimpleOperator[0];
	
	// ingame
	private bool isInTrigger = false;
	
	public void AddVariableCondition()
	{
		this.variableKey = ArrayHelper.Add("key", this.variableKey);
		this.variableValue = ArrayHelper.Add("value", this.variableValue);
		this.checkType = ArrayHelper.Add(true, this.checkType);
	}
	
	public void RemoveVariableCondition(int index)
	{
		this.variableKey = ArrayHelper.Remove(index, this.variableKey);
		this.variableValue = ArrayHelper.Remove(index, this.variableValue);
		this.checkType = ArrayHelper.Remove(index, this.checkType);
	}
	
	public void AddNumberVariableCondition()
	{
		this.numberVarKey = ArrayHelper.Add("key", this.numberVarKey);
		this.numberVarValue = ArrayHelper.Add(0, this.numberVarValue);
		this.numberCheckType = ArrayHelper.Add(true, this.numberCheckType);
		this.numberValueCheck = ArrayHelper.Add(ValueCheck.EQUALS, this.numberValueCheck);
	}
	
	public void RemoveNumberVariableCondition(int index)
	{
		this.numberVarKey = ArrayHelper.Remove(index, this.numberVarKey);
		this.numberVarValue = ArrayHelper.Remove(index, this.numberVarValue);
		this.numberCheckType = ArrayHelper.Remove(index, this.numberCheckType);
		this.numberValueCheck = ArrayHelper.Remove(index, this.numberValueCheck);
	}
	
	public bool CheckVariables()
	{
		bool apply = true;
		bool any = false;
		for(int i=0; i<this.variableKey.Length; i++)
		{
			bool check = GameHandler.CheckVariable(this.variableKey[i], this.variableValue[i]);
			
			if((check && this.checkType[i]) || (!check && !this.checkType[i]))
			{
				any = true;
			}
			else if(AIConditionNeeded.ALL.Equals(this.needed))
			{
				apply = false;
				break;
			}
		}
		if(apply)
		{
			for(int i=0; i<this.numberVarKey.Length; i++)
			{
				bool check = GameHandler.CheckNumberVariable(this.numberVarKey[i], 
						this.numberVarValue[i], this.numberValueCheck[i]);
				
				if((check && this.numberCheckType[i]) || (!check && !this.numberCheckType[i]))
				{
					any = true;
				}
				else if(AIConditionNeeded.ALL.Equals(this.needed))
				{
					apply = false;
					break;
				}
			}
		}
		if(AIConditionNeeded.ONE.Equals(this.needed) && !any && 
			(this.variableKey.Length > 0 || this.numberVarKey.Length > 0))
		{
			apply = false;
		}
		return apply;
	}
	
	public void AddVariableSet()
	{
		this.setVariableKey = ArrayHelper.Add("key", this.setVariableKey);
		this.setVariableValue = ArrayHelper.Add("value", this.setVariableValue);
	}
	
	public void RemoveVariableSet(int index)
	{
		this.setVariableKey = ArrayHelper.Remove(index, this.setVariableKey);
		this.setVariableValue = ArrayHelper.Remove(index, this.setVariableValue);
	}
	
	public void AddNumberVariableSet()
	{
		this.setNumberVarKey = ArrayHelper.Add("key", this.setNumberVarKey);
		this.setNumberVarValue = ArrayHelper.Add(0, this.setNumberVarValue);
		this.setNumberOperator = ArrayHelper.Add(SimpleOperator.ADD, this.setNumberOperator);
	}
	
	public void RemoveNumberVariableSet(int index)
	{
		this.setNumberVarKey = ArrayHelper.Remove(index, this.setNumberVarKey);
		this.setNumberVarValue = ArrayHelper.Remove(index, this.setNumberVarValue);
		this.setNumberOperator = ArrayHelper.Remove(index, this.setNumberOperator);
	}
	
	public void SetVariables()
	{
		for(int i=0; i<this.setVariableKey.Length; i++)
		{
			GameHandler.SetVariable(this.setVariableKey[i], this.setVariableValue[i]);
		}
		for(int i=0; i<this.setNumberVarKey.Length; i++)
		{
			if(SimpleOperator.ADD.Equals(this.setNumberOperator[i]))
			{
				GameHandler.SetNumberVariable(this.setNumberVarKey[i], 
						GameHandler.GetNumberVariable(this.setNumberVarKey[i])+this.setNumberVarValue[i]);
			}
			else if(SimpleOperator.SUB.Equals(this.setNumberOperator[i]))
			{
				GameHandler.SetNumberVariable(this.setNumberVarKey[i], 
						GameHandler.GetNumberVariable(this.setNumberVarKey[i])-this.setNumberVarValue[i]);
			}
			else if(SimpleOperator.SET.Equals(this.setNumberOperator[i]))
			{
				GameHandler.SetNumberVariable(this.setNumberVarKey[i], this.setNumberVarValue[i]);
			}
		}
	}
	
	public virtual void TouchInteract()
	{
		
	}
	
	public bool CheckTriggerEnter(Collider other)
	{
		bool check = false;
		if(other.gameObject == GameHandler.GetPlayer())
		{
			this.isInTrigger = true;
			if(EventStartType.TRIGGER_ENTER.Equals(this.startType) && 
				this.CheckVariables() && GameHandler.IsControlField())
			{
				check = true;
			}
		}
		return check;
	}
	
	public bool CheckTriggerExit(Collider other)
	{
		bool check = false;
		if(other.gameObject == GameHandler.GetPlayer())
		{
			this.isInTrigger = false;
			if(EventStartType.TRIGGER_EXIT.Equals(this.startType) && 
				this.CheckVariables() && GameHandler.IsControlField())
			{
				check = true;
			}
		}
		return check;
	}
	
	public bool KeyPress()
	{
		return !GameHandler.IsGamePaused() && GameHandler.IsControlField() && 
			EventStartType.KEY_PRESS.Equals(this.startType) && this.CheckVariables() &&
			ControlHandler.IsPressed(this.keyToPress) &&
			(!this.keyPressInTrigger || this.isInTrigger);
	}
	
	public bool CheckDrop(ChoiceContent drop)
	{
		bool check = false;
		if(drop != null && drop.dragID == this.dropID && (
			(DragType.ITEM.Equals(drop.dragType) && ItemDropType.ITEM.Equals(this.dropType)) ||
			(DragType.WEAPON.Equals(drop.dragType) && ItemDropType.WEAPON.Equals(this.dropType)) ||
			(DragType.ARMOR.Equals(drop.dragType) && ItemDropType.ARMOR.Equals(this.dropType))))
		{
			GameHandler.ChangeHappened(1, 0, 0);
			check = true;
			if(this.consumeDrop)
			{
				GameHandler.RemoveFromInventory(this.dropType, this.dropID, 1);
			}
		}
		return check;
	}
	
	public virtual bool Interact() { return false; }
}