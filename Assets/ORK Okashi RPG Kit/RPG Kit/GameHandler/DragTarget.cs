
using UnityEngine;
using System.Collections;

public abstract class DragTarget
{
	public abstract bool DragDropped(ChoiceContent drag, Vector2 pos);
	
	public abstract ChoiceContent DragStarted(Vector2 pos);
}
