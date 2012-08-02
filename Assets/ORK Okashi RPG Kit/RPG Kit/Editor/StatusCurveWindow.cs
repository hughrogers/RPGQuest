
using System.Collections;
using UnityEditor;
using UnityEngine;

public class StatusCurveWindow : EditorWindow
{
	private Texture2D tex;
	private Vector2 SP = new Vector2(0, 0);
	private Vector2 SP2 = new Vector2(0, 0);
	
	private int curveLevel1 = 1;
	private int curveLevelMax = 255;
	private int[] lvlPoint = new int[0];
	private int[] valPoint = new int[0];
	
	private int[] value;
	private int[] oldValue;
	private StatusValue status;
	
	private Color c1 = new Color(1, 0, 0, 1);
	private Color c2 = new Color(0, 1, 0, 1);
	
	public static void Init(string title, ref int[] development, StatusValue sv)
	{
		// Get existing open window or if none, make a new one:
		StatusCurveWindow window = (StatusCurveWindow)EditorWindow.GetWindow(typeof(StatusCurveWindow), true, title);
		window.tex = new Texture2D(1, 1);
		window.value = new int[development.Length];
		System.Array.Copy(development, window.value, development.Length);
		window.oldValue = development;
		window.status = sv;
		window.curveLevel1 = sv.minValue;
		window.curveLevelMax = sv.maxValue;
		Rect pos = window.position;
		pos.x = 100;
		pos.y = 100;
		pos.width = 200+window.value.Length*2+200;
		pos.height = 330;
		window.position = pos;
		window.Show();
	}
	
	void OnGUI()
	{
		Color tmpColor = GUI.color;
		float o = 250.0f/status.maxValue;
		for(int i=0; i<value.Length; i++)
		{
			if(i % 2 == 0)
			{
				GUI.color = c2;
			}
			else
			{
				GUI.color = c1;
			}
			GUI.DrawTexture(new Rect(200+(i*2), (status.maxValue-value[i])*o, 2, value[i]*o), tex);
		}
		
		GUI.color = tmpColor;
		GUILayout.BeginArea(new Rect(0, 0, 200, 300));
		GUILayout.Label ("Curve Settings", EditorStyles.boldLabel);
		if(GUILayout.Button("Generate Curve"))
		{
			this.GenerateCurve();
		}
		if(GUILayout.Button("Add Point"))
		{
			if(lvlPoint.Length > 0)
			{
				lvlPoint = ArrayHelper.Add(lvlPoint[lvlPoint.Length-1]+1, lvlPoint);
			}
			else
			{
				lvlPoint = ArrayHelper.Add(2, lvlPoint);
			}
			
			if(valPoint.Length > 0)
			{
				valPoint = ArrayHelper.Add(valPoint[valPoint.Length-1]+1, valPoint);
			}
			else
			{
				valPoint = ArrayHelper.Add(status.minValue+1, valPoint);
			}
		}
		curveLevel1 = EditorGUILayout.IntField("Level 1 Value", curveLevel1);
		if(curveLevel1 < status.minValue)
		{
			curveLevel1 = status.minValue;
		}
		else if(curveLevel1 > status.maxValue)
		{
			curveLevel1 = status.maxValue;
		}
		if(lvlPoint.Length > 0)
		{
			SP2 = EditorGUILayout.BeginScrollView(SP2);
			EditorGUILayout.Separator();
			for(int i=0; i<lvlPoint.Length; i++)
			{
				int mx = value.Length-1;
				if(i<lvlPoint.Length-1)
				{
					mx = lvlPoint[i+1];
				}
				if(i == 0)
				{
					lvlPoint[i] = EditorGUILayout.IntField("Level", lvlPoint[i]);
					if(lvlPoint[i] < 2)
					{
						lvlPoint[i] = 2;
					}
				}
				else
				{
					lvlPoint[i] = EditorGUILayout.IntField("Level", lvlPoint[i]);
					if(lvlPoint[i] < lvlPoint[i-1]+1)
					{
						lvlPoint[i] = lvlPoint[i-1]+1;
					}
				}
				if(lvlPoint[i] > mx)
				{
					lvlPoint[i] = mx;
				}
				valPoint[i] = EditorGUILayout.IntField("Value", valPoint[i]);
				if(valPoint[i] < status.minValue)
				{
					valPoint[i] = status.minValue;
				}
				else if(valPoint[i] > status.maxValue)
				{
					valPoint[i] = status.maxValue;
				}
				EditorGUILayout.Separator();
			}
			EditorGUILayout.EndScrollView();
		}
		curveLevelMax = EditorGUILayout.IntField("Max. Level Value", curveLevelMax);
		if(curveLevelMax < status.minValue)
		{
			curveLevelMax = status.minValue;
		}
		else if(curveLevelMax > status.maxValue)
		{
			curveLevelMax = status.maxValue;
		}
		if(lvlPoint.Length>0)
		{
			if(GUILayout.Button("Remove Point"))
			{
				lvlPoint = ArrayHelper.Remove(lvlPoint.Length-1, lvlPoint);
				valPoint = ArrayHelper.Remove(lvlPoint.Length-1, lvlPoint);
			}
		}
		GUILayout.EndArea();
		
		GUILayout.BeginArea(new Rect(200+value.Length*2, 0, 200, 300));
		GUILayout.Label ("Level Values", EditorStyles.boldLabel);
		SP = EditorGUILayout.BeginScrollView(SP);
		for(int i=0; i<value.Length; i++)
		{
			value[i] = EditorGUILayout.IntField("Level "+(i+1).ToString(), value[i]);
			if(value[i] < status.minValue)
			{
				value[i] = status.minValue;
			}
			else if(value[i] > status.maxValue)
			{
				value[i] = status.maxValue;
			}
		}
		EditorGUILayout.EndScrollView();
		GUILayout.EndArea();
		
		GUILayout.BeginArea(new Rect(4, 310, 408, 30));
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Ok"))
		{
			System.Array.Copy(value, oldValue, value.Length);
			this.Close();
		}
		if(GUILayout.Button("Cancel"))
		{
			this.Close();
		}
		EditorGUILayout.EndHorizontal();
		GUILayout.EndArea();
	}
	
	private void GenerateCurve()
	{
		value = new int[value.Length];
		value[0] = curveLevel1;
		value[value.Length-1] = curveLevelMax;
		
		float increase = 0;
		if(lvlPoint.Length > 0)
		{
			for(int i=0; i<lvlPoint.Length; i++)
			{
				int lastLevel = 0;
				int start = 0;
				
				if(i == 0)
				{
					lastLevel = 1;
					start = 2;
					increase = valPoint[0] - curveLevel1;
					increase /= lvlPoint[0];
				}
				else
				{
					lastLevel = lvlPoint[i-1];
					start = lvlPoint[i-1]+1;
					increase = valPoint[i] - valPoint[i-1];
					increase /= (lvlPoint[i] - lvlPoint[i-1]);
				}
				value[lvlPoint[i]-1] = valPoint[i];
				for(int j=start; j<lvlPoint[i]; j++)
				{
					value[j-1] = (int)(value[lastLevel-1] + increase*(j-lastLevel));
				}
			}
			increase = curveLevelMax - valPoint[valPoint.Length-1];
			increase /= (value.Length - lvlPoint[lvlPoint.Length-1]);
			for(int j=lvlPoint[lvlPoint.Length-1]+1; j<value.Length; j++)
			{
				value[j-1] = (int)(valPoint[valPoint.Length-1] + increase*(j-lvlPoint[lvlPoint.Length-1]));
			}
		}
		else
		{
			increase = curveLevelMax - curveLevel1;
			increase /= value.Length;
			
			for(int j=2; j<value.Length; j++)
			{
				value[j-1] = (int)(value[0] + increase*j);
			}
		}
	}
	
	/*void OnLostFocus()
	{
		Focus();
	}*/
}