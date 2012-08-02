
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VariableChecker))]
public class VariableCheckerInspector : BaseInspector
{
	public override void OnInspectorGUI()
	{
		GUILayout.Label("Check setup", EditorStyles.boldLabel);
		this.EventStartSettings((VariableChecker)target);
		EditorGUILayout.Separator();
		
		((VariableChecker)target).destroy = EditorGUILayout.Toggle("Destroy", ((VariableChecker)target).destroy);
		if(!((VariableChecker)target).destroy)
		{
			((VariableChecker)target).stopAnimation = EditorGUILayout.Toggle("Stop animation", ((VariableChecker)target).stopAnimation);
			if(((VariableChecker)target).stopAnimation)
			{
				((VariableChecker)target).stopAll = EditorGUILayout.Toggle("Stop all", ((VariableChecker)target).stopAll);
				if(!((VariableChecker)target).stopAll)
				{
					((VariableChecker)target).animationName = EditorGUILayout.TextField("Animation", ((VariableChecker)target).animationName);
				}
			}
			else
			{
				((VariableChecker)target).animationName = EditorGUILayout.TextField("Animation", ((VariableChecker)target).animationName);
				((VariableChecker)target).number = EditorGUILayout.Popup("Play type", ((VariableChecker)target).number, ((VariableChecker)target).playOptions);
				// play mode
				if(((VariableChecker)target).playOptions[((VariableChecker)target).number] == "Play" || ((VariableChecker)target).playOptions[((VariableChecker)target).number] == "CrossFade" ||
						((VariableChecker)target).playOptions[((VariableChecker)target).number] == "PlayQueued" || ((VariableChecker)target).playOptions[((VariableChecker)target).number] == "CrossFadeQueued")
				{
					((VariableChecker)target).playMode = (PlayMode)EditorGUILayout.EnumPopup("Play mode", ((VariableChecker)target).playMode);
				}
				// fade Length
				if(((VariableChecker)target).playOptions[((VariableChecker)target).number] == "CrossFade" || ((VariableChecker)target).playOptions[((VariableChecker)target).number] == "Blend" ||
						((VariableChecker)target).playOptions[((VariableChecker)target).number] == "CrossFadeQueued")
				{
					((VariableChecker)target).time = EditorGUILayout.FloatField("Fade Length", ((VariableChecker)target).time);
				}
				// ((VariableChecker)target) weight
				if(((VariableChecker)target).playOptions[((VariableChecker)target).number] == "Blend")
				{
					((VariableChecker)target).speed = EditorGUILayout.FloatField("Target weight", ((VariableChecker)target).speed);
				}
				// queue mode
				if(((VariableChecker)target).playOptions[((VariableChecker)target).number] == "PlayQueued" || ((VariableChecker)target).playOptions[((VariableChecker)target).number] == "CrossFadeQueued")
				{
					((VariableChecker)target).queueMode = (QueueMode)EditorGUILayout.EnumPopup("Queue mode", ((VariableChecker)target).queueMode);
				}
			}
		}
		
		EditorGUILayout.Separator();
		this.VariableSettings((VariableChecker)target);
		
		EditorGUILayout.Separator();
		
		if(GUI.changed)
            EditorUtility.SetDirty(target);
	}
}