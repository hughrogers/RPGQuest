
using UnityEditor;
using UnityEngine;

public class EditorTab
{
	protected int selection = 0;
	protected Vector2 SP1 = new Vector2(0, 0);
	protected Vector2 SP2 = new Vector2(0, 0);
	protected Vector2 SP3 = new Vector2(0, 0);
	protected Vector2 SP4 = new Vector2(0, 0);
	protected bool fold1 = true;
	protected bool fold2 = true;
	protected bool fold3 = true;
	protected bool fold4 = true;
	protected bool fold5 = true;
	protected bool fold6 = true;
	protected bool fold7 = true;
	protected bool fold8 = true;
	protected bool fold9 = true;
	protected bool fold10 = true;
	protected bool fold11 = true;
	protected bool fold12 = true;
	protected bool fold13 = true;
	protected bool fold14 = true;
	protected bool fold15 = true;
	protected bool fold16 = true;
	protected bool fold17 = true;
	protected bool fold18 = true;
	protected bool fold19 = true;
	protected bool fold20 = true;
	
	public EditorTab()
	{
		
	}
	
	public void Separate()
	{
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
	}
	
	public static int EnumToolbar(string text, int index, System.Type e)
	{
		return EditorTab.EnumToolbar(text, index, e, 200);
	}
	
	public static int EnumToolbar(string text, int index, System.Type e, int width)
	{
		string[] names = System.Enum.GetNames(e);
		if(text != "")
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel(text);
		}
		index = GUILayout.Toolbar(index, names, GUILayout.Width(width));
		if(text != "")
		{
			EditorGUILayout.EndHorizontal();
		}
		return index;
	}
	
	public static int ChanceCheck(int chance)
	{
		if(chance < 0) chance = 0;
		else if(chance > 100) chance = 100;
		return chance;
	}
	
	public static int MinMaxCheck(int check, int min, int max)
	{
		if(check < min) check = min;
		else if(check > max) check = max;
		return check;
	}
}