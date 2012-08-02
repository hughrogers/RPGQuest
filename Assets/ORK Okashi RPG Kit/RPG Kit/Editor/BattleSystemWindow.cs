
using UnityEditor;
using UnityEngine;

public class BattleSystemWindow : EditorWindow
{
	public int mWidth = 300;
	// section handling
	public int currentSection = 0;
	private string[] sections = new string[] {"Battle System", "Battle Menu", "Battle Text", "Battle End"};
	
	private int BATTLE_SYSTEM = 0;
	private int BATTLE_MENU = 1;
	public int BATTLE_TEXT = 2;
	public int BATTLE_END = 3;
	
	private BattleSystemTab systemTab;
	private BattleMenuTab menuTab;
	private BattleTextTab textTab;
	private BattleEndTab endTab;
	
	[MenuItem("RPG Kit/Battle System/Battle System Editor")]
	static void Init()
	{
		DataHolder.BattleSystemData().LoadData();
		// Get existing open window or if none, make a new one:
		BattleSystemWindow window = (BattleSystemWindow)EditorWindow.GetWindow(
			typeof(BattleSystemWindow), false, "Battle System Editor");
		window.systemTab = new BattleSystemTab(window);
		window.menuTab = new BattleMenuTab(window);
		window.textTab = new BattleTextTab(window);
		window.endTab = new BattleEndTab(window);
		window.Show();
	}
	
	void Save()
	{
		DataHolder.BattleSystemData().SaveData();
	}
	
	void OnGUI()
	{
		GUI.SetNextControlName("Toolbar");
		var prevSection = currentSection;
		currentSection = GUILayout.SelectionGrid(currentSection, sections, 4);
		if(prevSection != currentSection)
		{
			GUI.FocusControl("Toolbar");
		}
		GUILayout.Box(" ", GUILayout.ExpandWidth(true));
		
		if(currentSection == this.BATTLE_SYSTEM)
		{
			this.systemTab.ShowTab();
		}
		else if(currentSection == this.BATTLE_MENU)
		{
			this.menuTab.ShowTab();
		}
		else if(currentSection == this.BATTLE_TEXT)
		{
			this.textTab.ShowTab();
		}
		else if(currentSection == this.BATTLE_END)
		{
			this.endTab.ShowTab();
		}
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		GUI.SetNextControlName("Reload");
		if(GUILayout.Button("Reload Settings"))
		{
			GUI.FocusControl("Reload");
			DataHolder.Instance().Init();
		}
		GUI.SetNextControlName("Save");
		if(GUILayout.Button("Save Settings"))
		{
			GUI.FocusControl("Save");
			this.Save();
		}
		EditorGUILayout.EndHorizontal();
	}
}