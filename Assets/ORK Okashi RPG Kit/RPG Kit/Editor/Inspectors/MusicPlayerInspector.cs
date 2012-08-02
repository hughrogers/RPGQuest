
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MusicPlayer))]
public class MusicPlayerInspector : BaseInspector
{
	public override void OnInspectorGUI()
	{
		GUILayout.Label("Music setup", EditorStyles.boldLabel);
		this.EventStartSettings((MusicPlayer)target);
		EditorGUILayout.Separator();
		
		((MusicPlayer)target).playType = (MusicPlayType)EditorGUILayout.EnumPopup("Play type", ((MusicPlayer)target).playType);
		if(!((MusicPlayer)target).playType.Equals(MusicPlayType.FADE_OUT) && !((MusicPlayer)target).playType.Equals(MusicPlayType.STOP))
		{
			((MusicPlayer)target).musicClip = EditorGUILayout.Popup("Music clip", ((MusicPlayer)target).musicClip, DataHolder.Music().GetNameList(true));
		}
		if(!((MusicPlayer)target).playType.Equals(MusicPlayType.PLAY) && !((MusicPlayer)target).playType.Equals(MusicPlayType.STOP))
		{
			((MusicPlayer)target).fadeTime = EditorGUILayout.FloatField("Fade time (s)", ((MusicPlayer)target).fadeTime);
			((MusicPlayer)target).interpolate = (EaseType)EditorGUILayout.EnumPopup("Interpolation", ((MusicPlayer)target).interpolate);
		}
		
		EditorGUILayout.Separator();
		this.VariableSettings((MusicPlayer)target);
		EditorGUILayout.Separator();
		
		if(GUI.changed)
            EditorUtility.SetDirty(target);
	}
}