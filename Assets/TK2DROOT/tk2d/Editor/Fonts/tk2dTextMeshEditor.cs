using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(tk2dTextMesh))]
class tk2dTextMeshEditor : Editor
{
	tk2dFont[] allBmFontImporters = null;	// all generators
	Vector2 gradientScroll;
	
    public override void OnInspectorGUI()
    {
        tk2dTextMesh textMesh = (tk2dTextMesh)target;
		
		// maybe cache this if its too slow later
		if (allBmFontImporters == null) allBmFontImporters = tk2dEditorUtility.GetOrCreateIndex().GetFonts();
		
		if (allBmFontImporters != null)
        {
			if (textMesh.font == null)
			{
				textMesh.font = allBmFontImporters[0].data;
			}
			
			int currId = -1;
			string[] fontNames = new string[allBmFontImporters.Length];
			for (int i = 0; i < allBmFontImporters.Length; ++i)
			{
				fontNames[i] = allBmFontImporters[i].name;
				if (allBmFontImporters[i].data == textMesh.font)
				{
					currId = i;
				}
			}
			
			int newId = EditorGUILayout.Popup("Font", currId, fontNames);
			if (newId != currId)
			{
				textMesh.font = allBmFontImporters[newId].data;
				textMesh.renderer.material = textMesh.font.material;
			}
			
			EditorGUILayout.BeginHorizontal();
			textMesh.maxChars = EditorGUILayout.IntField("Max Chars", textMesh.maxChars);
			if (textMesh.maxChars < 1) textMesh.maxChars = 1;
			if (textMesh.maxChars > 16000) textMesh.maxChars = 16000;
			if (GUILayout.Button("Fit", GUILayout.MaxWidth(32.0f)))
			{
				textMesh.maxChars = textMesh.NumTotalCharacters();
				GUI.changed = true;
			}
			EditorGUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Text");
			textMesh.text = EditorGUILayout.TextArea(textMesh.text, GUILayout.Height(64));
			GUILayout.EndHorizontal();
			
			textMesh.anchor = (TextAnchor)EditorGUILayout.EnumPopup("Anchor", textMesh.anchor);
			textMesh.kerning = EditorGUILayout.Toggle("Kerning", textMesh.kerning);
			textMesh.spacing = EditorGUILayout.FloatField("Spacing", textMesh.spacing);
			textMesh.lineSpacing = EditorGUILayout.FloatField("Line Spacing", textMesh.lineSpacing);
			textMesh.scale = EditorGUILayout.Vector3Field("Scale", textMesh.scale);
			
			if (textMesh.font.textureGradients && textMesh.font.gradientCount > 0)
			{
				//textMesh.textureGradient = EditorGUILayout.IntSlider("Gradient", textMesh.textureGradient, 0, textMesh.font.gradientCount - 1);
				
				GUILayout.BeginHorizontal();
				
				EditorGUILayout.PrefixLabel("TextureGradient");
				
				// Draw gradient scroller
				bool drawGradientScroller = true;
				if (drawGradientScroller)
				{
					textMesh.textureGradient = textMesh.textureGradient % textMesh.font.gradientCount;
					
					gradientScroll = EditorGUILayout.BeginScrollView(gradientScroll, GUILayout.ExpandHeight(false));
					Rect r = GUILayoutUtility.GetRect(textMesh.font.gradientTexture.width, textMesh.font.gradientTexture.height, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
					GUI.DrawTexture(r, textMesh.font.gradientTexture);
					
					Rect hr = r;
					hr.width /= textMesh.font.gradientCount;
					hr.x += hr.width * textMesh.textureGradient;
					float ox = hr.width / 8;
					float oy = hr.height / 8;
					Vector3[] rectVerts = { new Vector3(hr.x + 0.5f + ox, hr.y + oy, 0), new Vector3(hr.x + hr.width - ox, hr.y + oy, 0), new Vector3(hr.x + hr.width - ox, hr.y + hr.height -  0.5f - oy, 0), new Vector3(hr.x + ox, hr.y + hr.height - 0.5f - oy, 0) };
					Handles.DrawSolidRectangleWithOutline(rectVerts, new Color(0,0,0,0.2f), new Color(0,0,0,1));
					
					if (GUIUtility.hotControl == 0 && Event.current.type == EventType.MouseDown && r.Contains(Event.current.mousePosition))
					{
						textMesh.textureGradient = (int)(Event.current.mousePosition.x / (textMesh.font.gradientTexture.width / textMesh.font.gradientCount));
						GUI.changed = true;
					}
	
					EditorGUILayout.EndScrollView();
				}
				
				
				GUILayout.EndHorizontal();
				
				textMesh.inlineStyling = EditorGUILayout.Toggle("Inline Styling", textMesh.inlineStyling);
				if (textMesh.inlineStyling)
				{
					Color bg = GUI.backgroundColor;
					GUI.backgroundColor = new Color32(154, 176, 203, 255);
					GUILayout.TextArea("Inline style commands\n" +
					                   "^0-9 - select gradient\n" +
									   "^^ - print ^");
					GUI.backgroundColor = bg;						
				}
			}
			
			EditorGUILayout.BeginHorizontal();
			
			if (GUILayout.Button("HFlip"))
			{
				Vector3 s = textMesh.scale;
				s.x *= -1.0f;
				textMesh.scale = s;
				GUI.changed = true;
			}
			if (GUILayout.Button("VFlip"))
			{
				Vector3 s = textMesh.scale;
				s.y *= -1.0f;
				textMesh.scale = s;
				GUI.changed = true;
			}			

			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			
			if (GUILayout.Button("Bake Scale"))
			{
				tk2dScaleUtility.Bake(textMesh.transform);
				GUI.changed = true;
			}
			
			GUIContent pixelPerfectButton = new GUIContent("1:1", "Make Pixel Perfect");
			if ( GUILayout.Button(pixelPerfectButton ))
			{
				if (tk2dPixelPerfectHelper.inst) tk2dPixelPerfectHelper.inst.Setup();
				textMesh.MakePixelPerfect();
				GUI.changed = true;
			}
			textMesh.pixelPerfect = GUILayout.Toggle(textMesh.pixelPerfect, "Always", GUILayout.Width(60.0f));
			
			EditorGUILayout.EndHorizontal();
			
			textMesh.useGradient = EditorGUILayout.Toggle("Use Gradient", textMesh.useGradient);
			if (textMesh.useGradient)
			{
				textMesh.color = EditorGUILayout.ColorField("Top Color", textMesh.color);
				textMesh.color2 = EditorGUILayout.ColorField("Bottom Color", textMesh.color2);
			}
			else
			{
				textMesh.color = EditorGUILayout.ColorField("Color", textMesh.color);
			}
			
			if (GUI.changed)
			{
				textMesh.Commit();
				EditorUtility.SetDirty(textMesh);
			}
		}
	}

    [MenuItem("GameObject/Create Other/tk2d/TextMesh", false, 13905)]
    static void DoCreateTextMesh()
    {
		tk2dFontData fontData = null;
		Material material = null;
		
		// Find reference in scene
        tk2dTextMesh dupeMesh = GameObject.FindObjectOfType(typeof(tk2dTextMesh)) as tk2dTextMesh;
		if (dupeMesh) 
		{
			fontData = dupeMesh.font;
			material = dupeMesh.GetComponent<MeshRenderer>().sharedMaterial;
		}
		
		// Find in library
		if (fontData == null)
		{
			tk2dFont[] allFontData = tk2dEditorUtility.GetOrCreateIndex().GetFonts();
			foreach (var v in allFontData)
			{
				if (v.data != null)
				{
					fontData = v.data;
					material = fontData.material;
				}
			}
		}
		
		if (fontData == null)
		{
			EditorUtility.DisplayDialog("Create TextMesh", "Unable to create text mesh as no Fonts have been found.", "Ok");
			return;
		}

		GameObject go = tk2dEditorUtility.CreateGameObjectInScene("TextMesh");
        tk2dTextMesh textMesh = go.AddComponent<tk2dTextMesh>();
		textMesh.font = fontData;
		textMesh.text = "New TextMesh";
		textMesh.Commit();
		textMesh.GetComponent<MeshRenderer>().material = material;
		
		Selection.activeGameObject = go;
		Undo.RegisterCreatedObjectUndo(go, "Create TextMesh");
    }
}
