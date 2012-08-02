
using UnityEditor;
using UnityEngine;
using System.Collections;

public class FontSaver
{
	private SerializedObject font;
	private string fontName;
	public string realFontName;
	private Vector2 textureSize;
	
	private bool limitFontExport = true;
	private int fontLimit = 512;
	private string ignoredChars = "";
	
	private bool fold = true;
	
	public FontSaver(Font f)
	{
		this.font = new SerializedObject(f);
		this.fontName = this.font.FindProperty("m_Name").stringValue;
		this.realFontName = f.ToString();
		this.textureSize = new Vector2(f.material.mainTexture.width, f.material.mainTexture.height);
	}
	
	public void ShowSettings()
	{
		EditorGUILayout.BeginVertical("box");
		fold = EditorGUILayout.Foldout(fold, this.fontName);
		if(fold)
		{
			this.limitFontExport = EditorGUILayout.Toggle("Limit font export", this.limitFontExport);
			if(this.limitFontExport)
			{
				this.fontLimit = EditorGUILayout.IntField("Last letter code", this.fontLimit);
			}
			this.ignoredChars = EditorGUILayout.TextField("Ignored chars", this.ignoredChars);
			EditorGUILayout.Separator();
		}
		EditorGUILayout.EndVertical();
	}
	
	public GUIFont GetFontData()
	{
		GUIFont gf = new GUIFont();
		
		gf.kerning = font.FindProperty("m_Kerning").floatValue;
		
		gf.asciiStartOffset = font.FindProperty("m_AsciiStartOffset").intValue;
		gf.lineSpacing = font.FindProperty("m_LineSpacing").floatValue;
		
		int size = font.FindProperty("m_CharacterRects.Array.size").intValue+this.ignoredChars.Length;
		if(this.limitFontExport) size = Mathf.Min(size, this.fontLimit+1);
		gf.letter = new GUILetter[size];
		int ignoreOffset = 0;
		for(int i=0; i<size; i++)
		{
			for(int j=0; j<this.ignoredChars.Length; j++)
			{
				if(i == this.ignoredChars[j]-32)
				{
					Rect r = new Rect(0, 0, 0, 0);
					gf.letter[i] = new GUILetter(r, r, textureSize, 0);
					i++;
					ignoreOffset--;
				}
			}
			gf.letter[i] = new GUILetter(
					font.FindProperty("m_CharacterRects.Array.data["+(i+ignoreOffset)+"].uv").rectValue,
					font.FindProperty("m_CharacterRects.Array.data["+(i+ignoreOffset)+"].vert").rectValue,
					textureSize,
					font.FindProperty("m_CharacterRects.Array.data["+(i+ignoreOffset)+"].width").floatValue);
		}
		size = font.FindProperty("m_KerningValues.Array.size").intValue;
		for(int i=0; i<size; i++)
		{
			int first = font.FindProperty("m_KerningValues.Array.data["+i+"].first.first").intValue;
			int second = font.FindProperty("m_KerningValues.Array.data["+i+"].first.second").intValue;
			float value = font.FindProperty("m_KerningValues.Array.data["+i+"].second").floatValue;
			gf.kerningPairs.Add(first+"/"+second, value.ToString());
		}
		return gf;
	}
}
