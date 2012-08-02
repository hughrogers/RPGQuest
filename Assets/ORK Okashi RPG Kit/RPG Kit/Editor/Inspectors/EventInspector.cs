
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EventInteraction))]
public class EventInspector : BaseInspector
{
	private bool firstLoad = false;
	
	public override void OnInspectorGUI()
	{
		string tmpCheck = ((EventInteraction)target).eventFile;
		((EventInteraction)target).eventFile = EditorGUILayout.TextField("Event file", ((EventInteraction)target).eventFile);
		if(tmpCheck != ((EventInteraction)target).eventFile)
		{
			((EventInteraction)target).fileOk = false;
		}
		if(((EventInteraction)target).fileOk && !this.firstLoad)
		{
			((EventInteraction)target).LoadEvent();
		}
		
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Check Event", GUILayout.Width(150)))
		{
			((EventInteraction)target).LoadEvent();
			if(((EventInteraction)target).fileOk)
			{
				Transform[] tmp = ((EventInteraction)target).actor;
				((EventInteraction)target).actor = new Transform[((EventInteraction)target).gameEvent.actor.Length];
				for(int i=0; i<((EventInteraction)target).actor.Length; i++)
				{
					if(i < tmp.Length) ((EventInteraction)target).actor[i] = tmp[i];
				}
				tmp = ((EventInteraction)target).waypoint;
				((EventInteraction)target).waypoint = new Transform[((EventInteraction)target).gameEvent.waypoints];
				for(int i=0; i<((EventInteraction)target).waypoint.Length; i++)
				{
					if(i < tmp.Length) ((EventInteraction)target).waypoint[i] = tmp[i];
				}
				GameObject[] tmp2 = ((EventInteraction)target).prefab;
				((EventInteraction)target).prefab = new GameObject[((EventInteraction)target).gameEvent.prefabs];
				for(int i=0; i<((EventInteraction)target).prefab.Length; i++)
				{
					if(i < tmp2.Length) ((EventInteraction)target).prefab[i] = tmp2[i];
				}
				AudioClip[] tmp3 = ((EventInteraction)target).audioClip;
				((EventInteraction)target).audioClip = new AudioClip[((EventInteraction)target).gameEvent.audioClips];
				for(int i=0; i<((EventInteraction)target).audioClip.Length; i++)
				{
					if(i < tmp3.Length) ((EventInteraction)target).audioClip[i] = tmp3[i];
				}
			}
		}
		if(((EventInteraction)target).fileOk)
		{
			if(GUILayout.Button("Edit Event", GUILayout.Width(150)))
			{
				EventWindow.Init(((EventInteraction)target).GetEventFile());
				GUIUtility.ExitGUI();
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
			
			GUILayout.Label("Event setup", EditorStyles.boldLabel);
			this.EventStartSettings((EventInteraction)target);
			
			
			if(EventStartType.AUTOSTART.Equals(((EventInteraction)target).startType))
			{
				((EventInteraction)target).repeatExecution = EditorGUILayout.Toggle("Repeat event", 
						((EventInteraction)target).repeatExecution);
			}
			
			if(!EventStartType.AUTOSTART.Equals(((EventInteraction)target).startType) || 
				!((EventInteraction)target).repeatExecution)
			{
				((EventInteraction)target).deactivateAfter = EditorGUILayout.Toggle("Deactivate after event", 
						((EventInteraction)target).deactivateAfter);
			}
			
			((EventInteraction)target).timeBefore = EditorGUILayout.FloatField("Start after (s)", 
					((EventInteraction)target).timeBefore);
			((EventInteraction)target).timeAfter = EditorGUILayout.FloatField("Reenable after (s)", 
					((EventInteraction)target).timeAfter);
			
			if(EventStartType.INTERACT.Equals(((EventInteraction)target).startType))
			{
				((EventInteraction)target).turnToEvent = EditorGUILayout.Toggle("Turn to event", 
						((EventInteraction)target).turnToEvent);
				((EventInteraction)target).turnToPlayer = EditorGUILayout.Toggle("Turn to player", 
						((EventInteraction)target).turnToPlayer);
			}
			EditorGUILayout.Separator();
			
			this.VariableSettings((EventInteraction)target);
			
			if(!((EventInteraction)target).gameEvent.mainCamera)
			{
				EditorGUILayout.Separator();
				GUILayout.Label("Camera setup", EditorStyles.boldLabel);
				((EventInteraction)target).cam = (Transform)EditorGUILayout.ObjectField("Camera", ((EventInteraction)target).cam, typeof(Transform), true);
			}
			if(((EventInteraction)target).actor.Length > 0)
			{
				EditorGUILayout.Separator();
				GUILayout.Label("Actor setup", EditorStyles.boldLabel);
				for(int i=0; i<((EventInteraction)target).actor.Length; i++)
				{
					if(((EventInteraction)target).gameEvent.actor[i].isPlayer)
					{
						EditorGUILayout.LabelField("Actor "+i, "Player");
					}
					else
					{
						((EventInteraction)target).actor[i] = (Transform)EditorGUILayout.ObjectField("Actor "+i, ((EventInteraction)target).actor[i], typeof(Transform), true);
					}
				}
			}
			if(((EventInteraction)target).waypoint.Length > 0)
			{
				EditorGUILayout.Separator();
				GUILayout.Label("Waypoint setup", EditorStyles.boldLabel);
				for(int i=0; i<((EventInteraction)target).waypoint.Length; i++)
				{
					((EventInteraction)target).waypoint[i] = (Transform)EditorGUILayout.ObjectField("Waypoint "+i, ((EventInteraction)target).waypoint[i], typeof(Transform), true);
				}
			}
			if(((EventInteraction)target).prefab.Length > 0)
			{
				EditorGUILayout.Separator();
				GUILayout.Label("Prefab setup", EditorStyles.boldLabel);
				for(int i=0; i<((EventInteraction)target).prefab.Length; i++)
				{
					((EventInteraction)target).prefab[i] = (GameObject)EditorGUILayout.ObjectField("Prefab "+i, ((EventInteraction)target).prefab[i], typeof(GameObject), true);
				}
			}
			if(((EventInteraction)target).audioClip.Length > 0)
			{
				EditorGUILayout.Separator();
				GUILayout.Label("Audio clip setup", EditorStyles.boldLabel);
				for(int i=0; i<((EventInteraction)target).audioClip.Length; i++)
				{
					((EventInteraction)target).audioClip[i] =(AudioClip)EditorGUILayout.ObjectField("Audio clip "+i, ((EventInteraction)target).audioClip[i], typeof(AudioClip), true);
				}
			}
		}
		else
		{
			if(GUILayout.Button("New Event", GUILayout.Width(150)))
			{
				EventWindow.Init();
				GUIUtility.ExitGUI();
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
		}
		
		if(GUI.changed)
            EditorUtility.SetDirty(target);
	}
}