
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[AddComponentMenu("RPG Kit/Controls/Interaction Controller")]
public class InteractionController : MonoBehaviour
{
	private bool interacting = false;
	
	void OnTriggerEnter(Collider other)
	{
		GameObject player = GameHandler.Party().GetPlayer();
		if(player != null && this.transform.root == player.transform)
		{
			BaseInteraction[] interactions = other.gameObject.GetComponentsInChildren<BaseInteraction>();
			foreach(BaseInteraction interaction in interactions)
			{
				if(interaction && EventStartType.INTERACT.Equals(interaction.startType) && 
					!GameHandler.GetLevelHandler().InteractionListContains(other.gameObject))
				{
					GameHandler.GetLevelHandler().interactionList.Add(other.gameObject);
					break;
				}
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		GameObject player = GameHandler.Party().GetPlayer();
		if(player != null && this.transform.root == player.transform)
		{
			BaseInteraction[] interactions = other.gameObject.GetComponentsInChildren<BaseInteraction>();
			foreach(BaseInteraction interaction in interactions)
			{
				if(interaction && EventStartType.INTERACT.Equals(interaction.startType))
				{
					GameHandler.GetLevelHandler().interactionList.Remove(other.gameObject);
					break;
				}
			}
		}
	}

	public bool Interact(ArrayList list)
	{
		bool val = false;
		if(!interacting && GameHandler.IsControlField())
		{
			for(int i=0; i<list.Count; i++)
			{
				if(list[i] != null)
				{
					BaseInteraction[] interactions = ((GameObject)list[i]).GetComponents<BaseInteraction>();
					foreach(BaseInteraction interaction in interactions)
					{
						if(interaction != null && interaction.enabled &&
							EventStartType.INTERACT.Equals(interaction.startType))
						{
							StartCoroutine(BlockInteraction());
							val = interaction.Interact();
							if(val) return val;
						}
					}
				}
			}
		}
		return val;
	}

	public IEnumerator BlockInteraction()
	{
		interacting = true;
		yield return null;
		interacting = false;
	}
}