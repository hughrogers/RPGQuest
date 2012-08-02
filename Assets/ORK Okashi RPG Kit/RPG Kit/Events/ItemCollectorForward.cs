
using UnityEngine;
using System.Collections;

public class ItemCollectorForward : BaseInteraction
{
	void OnTriggerEnter(Collider other)
	{
		if(transform.parent != null)
		{
			((ItemCollector)transform.parent.GetComponent(typeof(ItemCollector))).OnTriggerEnter(other);
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if(transform.parent != null)
		{
			((ItemCollector)transform.parent.GetComponent(typeof(ItemCollector))).OnTriggerEnter(other);
		}
	}
	
	public override void TouchInteract()
	{
		if(transform.parent != null)
		{
			((ItemCollector)transform.parent.GetComponent(typeof(ItemCollector))).TouchInteract();
		}
	}
	
	void OnMouseUp()
	{
		if(transform.parent != null)
		{
			((ItemCollector)transform.parent.GetComponent(typeof(ItemCollector))).OnMouseUp();
		}
	}
	
	public override bool Interact()
	{
		bool val = false;
		if(transform.parent != null)
		{
			val = ((ItemCollector)transform.parent.GetComponent(typeof(ItemCollector))).Interact();
		}
		return val;
	}
	
	public override bool DropInteract(ChoiceContent drop)
	{
		bool val = false;
		if(transform.parent != null)
		{
			val = ((ItemCollector)transform.parent.GetComponent(typeof(ItemCollector))).DropInteract(drop);
		}
		return val;
	}
}
