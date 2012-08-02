using UnityEngine;
using System.Collections;

public abstract class QuestionInterface
{
	public int dialoguePosition = 0;
	public string message = "";
	public ChoiceContent[] choices;
	public int[] choiceActions;
	
	public ControlType controlType = ControlType.EVENT;
	
	public abstract void CreateChoices();
	
	public abstract bool ChoiceSelected(int index);
}
