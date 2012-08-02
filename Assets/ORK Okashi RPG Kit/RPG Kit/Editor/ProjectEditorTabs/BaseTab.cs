
using UnityEditor;
using UnityEngine;

public class BaseTab : EditorTab
{
	protected ProjectWindow pw;
	
	protected Texture2D tmpIcon;
	
	public BaseTab(ProjectWindow pw) : base()
	{
		this.pw = pw;
	}
	
	public void Reload()
	{
		selection = 0;
		tmpIcon = null;
	}
	
	public void AddItemList(BaseData data)
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical(GUILayout.Width(pw.mWidth));
		EditorGUILayout.Separator();
		EditorGUILayout.BeginVertical("box");
		SP1 = EditorGUILayout.BeginScrollView(SP1);
		
		if(data.GetDataCount() > 0)
		{
			var prev = selection;
			selection = GUILayout.SelectionGrid(selection, data.GetNameList(true), 1);
			if(prev != selection)
			{
				this.tmpIcon = null;
				GUI.FocusControl("ID");
			}
		}
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndVertical();
	}
	
	public void AddItemList(BaseLangData data)
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical(GUILayout.Width(pw.mWidth));
		EditorGUILayout.Separator();
		EditorGUILayout.BeginVertical("box");
		SP1 = EditorGUILayout.BeginScrollView(SP1);
		
		if(data.GetDataCount() > 0)
		{
			var prev = selection;
			selection = GUILayout.SelectionGrid(selection, data.GetNameList(true), 1);
			if(prev != selection)
			{
				this.tmpIcon = null;
				GUI.FocusControl("ID");
			}
		}
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndVertical();
	}
	
	public void AddItemListFilter(BaseLangData data, string title, string[] filterList)
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical(GUILayout.Width(pw.mWidth));
		EditorGUILayout.Separator();
		EditorGUILayout.BeginVertical("box");
		SP1 = EditorGUILayout.BeginScrollView(SP1);
		
		bool hlp = data.filter.useFilter[0];
		int hlp2 = data.filter.filterID[0];
		data.filter.useFilter[0] = EditorGUILayout.Toggle("Filter by "+title, data.filter.useFilter[0]);
		if(data.filter.useFilter[0])
		{
			data.filter.filterID[0] = EditorGUILayout.Popup(data.filter.filterID[0], filterList);
		}
		if(hlp != data.filter.useFilter[0] || hlp2 != data.filter.filterID[0])
		{
			data.CreateFilterList(true);
		}
		
		EditorGUILayout.Separator();
		if(data.GetDataCount() > 0)
		{
			int prev = selection;
			if(data.filter.useFilter[0])
			{
				hlp2 = GUILayout.SelectionGrid(data.filter.GetFakeID(selection), data.filter.nameList, 1);
				if(hlp2 < data.filter.realID.Length) selection = data.filter.realID[hlp2];
				else selection = -1;
			}
			else
			{
				selection = GUILayout.SelectionGrid(selection, data.GetNameList(true), 1);
			}
			if(prev != selection)
			{
				this.tmpIcon = null;
				GUI.FocusControl("ID");
			}
		}
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndVertical();
	}
	
	public void AddItemListFilter(BaseLangData data, string title, string[] filterList, string title2, string[] filterList2)
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical(GUILayout.Width(pw.mWidth));
		EditorGUILayout.Separator();
		EditorGUILayout.BeginVertical("box");
		SP1 = EditorGUILayout.BeginScrollView(SP1);
		
		bool hlp = data.filter.useFilter[0];
		int hlp2 = data.filter.filterID[0];
		data.filter.useFilter[0] = EditorGUILayout.Toggle("Filter by "+title, data.filter.useFilter[0]);
		if(data.filter.useFilter[0])
		{
			data.filter.filterID[0] = EditorGUILayout.Popup(data.filter.filterID[0], filterList);
		}
		
		bool hlp3 = data.filter.useFilter[1];
		int hlp4 = data.filter.filterID[1];
		data.filter.useFilter[1] = EditorGUILayout.Toggle("Filter by "+title2, data.filter.useFilter[1]);
		if(data.filter.useFilter[1])
		{
			data.filter.filterID[1] = EditorGUILayout.Popup(data.filter.filterID[1], filterList2);
		}
		
		if(hlp != data.filter.useFilter[0] || hlp2 != data.filter.filterID[0] ||
			hlp3 != data.filter.useFilter[1] || hlp4 != data.filter.filterID[1])
		{
			data.CreateFilterList(true);
		}
		
		EditorGUILayout.Separator();
		if(data.GetDataCount() > 0)
		{
			int prev = selection;
			if(data.filter.useFilter[0] || data.filter.useFilter[1])
			{
				hlp2 = GUILayout.SelectionGrid(data.filter.GetFakeID(selection), data.filter.nameList, 1);
				if(hlp2 < data.filter.realID.Length) selection = data.filter.realID[hlp2];
				else if(hlp4 < data.filter.realID.Length) selection = data.filter.realID[hlp4];
				else selection = -1;
			}
			else
			{
				selection = GUILayout.SelectionGrid(selection, data.GetNameList(true), 1);
			}
			if(prev != selection)
			{
				this.tmpIcon = null;
				GUI.FocusControl("ID");
			}
		}
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndVertical();
	}
	
	public void AddID(string text)
	{
		EditorGUILayout.BeginVertical();
		EditorGUILayout.Separator();
		GUI.SetNextControlName("ID");
		EditorGUILayout.LabelField(text, selection.ToString(), GUILayout.Width(pw.mWidth));
	}
	
	public void AddMultiLang(string text, BaseLangData data)
	{
		EditorGUILayout.BeginVertical("box");
		fold1 = EditorGUILayout.Foldout(fold1, "Name/Description");
		if(fold1)
		{
			for(int i=0; i<pw.GetLangCount(); i++)
			{
				GUILayout.Label (pw.GetLang(i), EditorStyles.boldLabel);
				data.name[i].text[selection] = EditorGUILayout.TextField(text, data.name[i].text[selection]);
				data.description[i].text[selection] = EditorGUILayout.TextField("Description", data.description[i].text[selection]);
			}
			this.Separate();
		}
		EditorGUILayout.EndVertical();
	}
	
	public void AddMultiLangIcon(string text, BaseLangData data)
	{
		EditorGUILayout.BeginVertical("box");
		fold1 = EditorGUILayout.Foldout(fold1, "Name/Description/Icon");
		if(fold1)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			for(int i=0; i<pw.GetLangCount(); i++)
			{
				GUILayout.Label (pw.GetLang(i), EditorStyles.boldLabel);
				data.name[i].text[selection] = EditorGUILayout.TextField(text, data.name[i].text[selection]);
				data.description[i].text[selection] = EditorGUILayout.TextField("Description", data.description[i].text[selection]);
			}
			EditorGUILayout.EndVertical();
			if(this.tmpIcon == null && data.icon[selection] != null && "" != data.icon[selection])
			{
				this.tmpIcon = (Texture2D)Resources.Load(data.GetIconPath()+data.icon[selection], typeof(Texture2D));
			}
			this.tmpIcon = (Texture2D)EditorGUILayout.ObjectField("Icon", this.tmpIcon, typeof(Texture2D), false);
			if(this.tmpIcon) data.icon[selection] = this.tmpIcon.name;
			else data.icon[selection] = "";
			EditorGUILayout.EndHorizontal();
			this.Separate();
		}
		EditorGUILayout.EndVertical();
	}
	
	public bool ShowRemButton(string text, BaseData data)
	{
		GUI.SetNextControlName("Rem");
		bool press = GUILayout.Button(text, GUILayout.Width(pw.mWidth2));
		if(press)
		{
			GUI.FocusControl("Rem");
			data.RemoveData(selection);
		}
		return press;
	}
	
	public bool ShowRemButton(string text, BaseLangData data)
	{
		GUI.SetNextControlName("Rem");
		bool press = GUILayout.Button(text, GUILayout.Width(pw.mWidth2));
		if(press)
		{
			GUI.FocusControl("Rem");
			data.RemoveData(selection);
		}
		return press;
	}
	
	public bool ShowCopyButton(BaseData data)
	{
		GUI.SetNextControlName("Copy");
		bool press = GUILayout.Button("Copy", GUILayout.Width(pw.mWidth2));
		if(press)
		{
			GUI.FocusControl("Copy");
			data.Copy(selection);
			selection = data.GetDataCount()-1;
		}
		return press;
	}
	
	public bool ShowCopyButton(BaseLangData data)
	{
		GUI.SetNextControlName("Copy");
		bool press = GUILayout.Button("Copy", GUILayout.Width(pw.mWidth2));
		if(press)
		{
			GUI.FocusControl("Copy");
			data.Copy(selection);
			selection = data.GetDataCount()-1;
		}
		return press;
	}
	
	public void EndTab()
	{
		EditorGUILayout.Separator();
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();
	}
	
	public void CheckSelection(BaseData data)
	{
		if(selection > data.GetDataCount()-1)
		{
			selection = data.GetDataCount()-1;
		}
	}
	
	public void CheckSelection(BaseLangData data)
	{
		if(selection > data.GetDataCount()-1)
		{
			selection = data.GetDataCount()-1;
		}
	}
}