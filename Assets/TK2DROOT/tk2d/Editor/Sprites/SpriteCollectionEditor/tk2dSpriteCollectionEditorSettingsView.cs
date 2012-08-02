using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace tk2dEditor.SpriteCollectionEditor
{
	public class SettingsView	
	{
		public bool show = false;
		Vector2 settingsScrollbar = Vector2.zero;
		int[] padAmountValues = null;
		string[] padAmountLabels = null;
		bool displayAtlasFoldout = true;
		
		IEditorHost host;
		public SettingsView(IEditorHost host)
		{
			this.host = host;
		}
		
		SpriteCollectionProxy SpriteCollection { get { return host.SpriteCollection; } }
		
		void DrawMaterialEditor()
		{
			// Upgrade
			int numAltMaterials = 0;
			foreach (var v in SpriteCollection.altMaterials)
				if (v != null) numAltMaterials++;
			
			if ((SpriteCollection.altMaterials.Length == 0 || numAltMaterials == 0) && SpriteCollection.atlasMaterials.Length != 0)
				SpriteCollection.altMaterials = new Material[1] { SpriteCollection.atlasMaterials[0] };
			
			if (SpriteCollection.altMaterials.Length > 0)
			{
				GUILayout.BeginHorizontal();
				bool displayMaterialFoldout = EditorGUILayout.Foldout(true, "Materials");
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("+", EditorStyles.miniButton))
				{
					Material source = null;
					int i;
					for (i = 0; i < SpriteCollection.altMaterials.Length; ++i)
					{
						if (SpriteCollection.altMaterials[i] != null)
						{
							source = SpriteCollection.altMaterials[i];
							break;
						}
					}
					for (i = 0; i < SpriteCollection.altMaterials.Length; ++i)
					{
						if (SpriteCollection.altMaterials[i] == null)
							break;
					}
					if (i == SpriteCollection.altMaterials.Length)
						System.Array.Resize(ref SpriteCollection.altMaterials, SpriteCollection.altMaterials.Length + 1);
					
					Material mtl;
					if (source == null)
					{
						mtl = new Material( Shader.Find(SpriteCollection.premultipliedAlpha?"tk2d/PremulVertexColor":"tk2d/BlendVertexColor") );
						string assetPath = SpriteCollection.GetAssetPath();
						string dirName = System.IO.Path.GetDirectoryName(assetPath);
						string fileName = System.IO.Path.GetFileNameWithoutExtension(assetPath);
						string targetPath = dirName + "/" + fileName + " AltMaterial.mat";
						targetPath = AssetDatabase.GenerateUniqueAssetPath(targetPath);
			            AssetDatabase.CreateAsset(mtl, targetPath);
						AssetDatabase.SaveAssets();
						mtl = AssetDatabase.LoadAssetAtPath(targetPath, typeof(Material)) as Material;
					}
					else
					{
						string sourcePath = AssetDatabase.GetAssetPath(source);
						string targetPath = AssetDatabase.GenerateUniqueAssetPath(sourcePath);
						AssetDatabase.CopyAsset(sourcePath, targetPath);
						AssetDatabase.SaveAssets();
						AssetDatabase.Refresh();
						mtl = AssetDatabase.LoadAssetAtPath(targetPath, typeof(Material)) as Material;
					}
					
					SpriteCollection.altMaterials[i] = mtl;
					SpriteCollection.Trim();
				}
				GUILayout.EndHorizontal();
				if (displayMaterialFoldout && SpriteCollection.altMaterials != null)
				{
					EditorGUI.indentLevel++;
					
					for (int i = 0; i < SpriteCollection.altMaterials.Length; ++i)
					{
						if (SpriteCollection.altMaterials[i] == null)
							continue;
						
						bool deleteMaterial = false;
						
						Material newMaterial = EditorGUILayout.ObjectField(SpriteCollection.altMaterials[i], typeof(Material), false) as Material;
						if (newMaterial == null)
						{
							// Can't delete the last one
							if (numAltMaterials > 1)
							{
								bool inUse = false;
								foreach (var v in SpriteCollection.textureParams)
								{
									if (v.materialId == i)
									{
										inUse = true;
										break;
									}
								}
								
								if (inUse)
								{
									if (EditorUtility.DisplayDialog("Delete material", 
										"This material is in use. Deleting it will reset materials on " +
										"sprites that use this material.\n" +
										"Do you wish to proceed?", "Yes", "Cancel"))
									{
										deleteMaterial = true;
									}
								}
								else
								{
									deleteMaterial = true;
								}
							}
						}
						else
						{
							SpriteCollection.altMaterials[i] = newMaterial;
						}
						
						if (deleteMaterial)
						{
							SpriteCollection.altMaterials[i] = null;
							
							// fix up all existing materials
							int targetMaterialId;
							for (targetMaterialId = 0; targetMaterialId < SpriteCollection.altMaterials.Length; ++targetMaterialId)
								if (SpriteCollection.altMaterials[targetMaterialId] != null)
									break;
							foreach (var sprite in SpriteCollection.textureParams)
							{
								if (sprite.materialId == i)
									sprite.materialId = targetMaterialId;
							}
							
							SpriteCollection.Trim();
						}
					}
										
					EditorGUI.indentLevel--;
				}
			}			
		}
		
		public void Draw()
		{
			if (SpriteCollection == null)
				return;
			
			// initialize internal stuff
			if (padAmountValues == null || padAmountValues.Length == 0)
			{
				int MAX_PAD_AMOUNT = 18;
				padAmountValues = new int[MAX_PAD_AMOUNT];
				padAmountLabels = new string[MAX_PAD_AMOUNT];
				for (int i = 0; i < MAX_PAD_AMOUNT; ++i)
				{
					padAmountValues[i] = -1 + i;
					padAmountLabels[i] = (i==0)?"Default":((i-1).ToString());
				}
			}
	
			GUILayout.BeginHorizontal();
			
			GUILayout.BeginVertical(tk2dEditorSkin.SC_BodyBackground, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
			GUILayout.EndVertical();
			
			
			int inspectorWidth = host.InspectorWidth;
			EditorGUIUtility.LookLikeControls(130.0f, 100.0f);
			
			settingsScrollbar = GUILayout.BeginScrollView(settingsScrollbar, GUILayout.ExpandHeight(true), GUILayout.Width(inspectorWidth));
	
			GUILayout.BeginVertical(tk2dEditorSkin.SC_InspectorHeaderBG, GUILayout.ExpandWidth(true));
			GUILayout.Label("Settings", EditorStyles.largeLabel);
			SpriteCollection.spriteCollection = EditorGUILayout.ObjectField("Data object", SpriteCollection.spriteCollection, typeof(tk2dSpriteCollectionData), false) as tk2dSpriteCollectionData;
			GUILayout.EndVertical();
			
			GUILayout.BeginVertical(tk2dEditorSkin.SC_InspectorBG, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
			SpriteCollection.premultipliedAlpha = EditorGUILayout.Toggle("Premultiplied Alpha", SpriteCollection.premultipliedAlpha);
			SpriteCollection.pixelPerfectPointSampled = EditorGUILayout.Toggle("Point Sampled", SpriteCollection.pixelPerfectPointSampled);
			SpriteCollection.physicsDepth = EditorGUILayout.FloatField("Collider depth", SpriteCollection.physicsDepth);
			SpriteCollection.disableTrimming = EditorGUILayout.Toggle("Disable Trimming", SpriteCollection.disableTrimming);
			SpriteCollection.normalGenerationMode = (tk2dSpriteCollection.NormalGenerationMode)EditorGUILayout.EnumPopup("Normal Generation", SpriteCollection.normalGenerationMode);
			SpriteCollection.padAmount = EditorGUILayout.IntPopup("Pad Amount", SpriteCollection.padAmount, padAmountLabels, padAmountValues);
	
			SpriteCollection.useTk2dCamera = EditorGUILayout.Toggle("Use tk2dCamera", SpriteCollection.useTk2dCamera);
			if (!SpriteCollection.useTk2dCamera)
			{
				EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
				SpriteCollection.targetHeight = EditorGUILayout.IntField("Target Height", SpriteCollection.targetHeight);
				SpriteCollection.targetOrthoSize = EditorGUILayout.FloatField("Target Ortho Size", SpriteCollection.targetOrthoSize);
				EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
			}
			
			displayAtlasFoldout = EditorGUILayout.Foldout(displayAtlasFoldout, "Atlas");
			if (displayAtlasFoldout)
			{
				EditorGUI.indentLevel++;
				
				int[] allowedAtlasSizes = { 64, 128, 256, 512, 1024, 2048, 4096 };
				string[] allowedAtlasSizesString = new string[allowedAtlasSizes.Length];
				for (int i = 0; i < allowedAtlasSizes.Length; ++i)
					allowedAtlasSizesString[i] = allowedAtlasSizes[i].ToString();
	
				SpriteCollection.forceTextureSize = EditorGUILayout.Toggle("Force Atlas Size", SpriteCollection.forceTextureSize);
				EditorGUI.indentLevel++;
				if (SpriteCollection.forceTextureSize)
				{
					SpriteCollection.forcedTextureWidth = EditorGUILayout.IntPopup("Width", SpriteCollection.forcedTextureWidth, allowedAtlasSizesString, allowedAtlasSizes);
					SpriteCollection.forcedTextureHeight = EditorGUILayout.IntPopup("Height", SpriteCollection.forcedTextureHeight, allowedAtlasSizesString, allowedAtlasSizes);
				}
				else
				{
					SpriteCollection.maxTextureSize = EditorGUILayout.IntPopup("Max Size", SpriteCollection.maxTextureSize, allowedAtlasSizesString, allowedAtlasSizes);
					SpriteCollection.forceSquareAtlas = EditorGUILayout.Toggle("Force Square", SpriteCollection.forceSquareAtlas);
				}
				EditorGUI.indentLevel--;
				
				SpriteCollection.textureCompression = (tk2dSpriteCollection.TextureCompression)EditorGUILayout.EnumPopup("Compression", SpriteCollection.textureCompression);
				
				bool allowMultipleAtlases = EditorGUILayout.Toggle("Multiple Atlases", SpriteCollection.allowMultipleAtlases);
				if (allowMultipleAtlases != SpriteCollection.allowMultipleAtlases)
				{
					// Disallow switching if using unsupported features
					if (allowMultipleAtlases == true)
					{
						bool hasDicing = false;
						for (int i = 0; i < SpriteCollection.textureRefs.Count; ++i)
						{
							if (SpriteCollection.textureRefs[i] != null &
								SpriteCollection.textureParams[i].dice)
							{
								hasDicing = true;
								break;
							}
						}
						
						if (SpriteCollection.fonts.Count > 0 || hasDicing)
						{
							EditorUtility.DisplayDialog("Multiple atlases", 
										"Multiple atlases not allowed. This sprite collection contains fonts and/or " +
										"contains diced sprites.", "Ok");
							allowMultipleAtlases = false;
						}
					}
					
					SpriteCollection.allowMultipleAtlases = allowMultipleAtlases;
				}

				if (SpriteCollection.allowMultipleAtlases)
				{
					tk2dGuiUtility.InfoBox("Sprite collections with multiple atlas spanning enabled cannot be used with the Static Sprite" +
						" Batcher, Fonts, the TileMap Editor and doesn't support Sprite Dicing and material level optimizations.\n\n" +
						"Avoid using it unless you are simply importing a" +
						" large sequence of sprites for an animation.", tk2dGuiUtility.WarningLevel.Info);
				}
				
				if (SpriteCollection.allowMultipleAtlases)
				{
					EditorGUILayout.LabelField("Num Atlases", SpriteCollection.atlasTextures.Length.ToString());
				}
				else
				{
					EditorGUILayout.LabelField("Atlas Width", SpriteCollection.atlasWidth.ToString());
					EditorGUILayout.LabelField("Atlas Height", SpriteCollection.atlasHeight.ToString());
					EditorGUILayout.LabelField("Atlas Wastage", SpriteCollection.atlasWastage.ToString("0.00") + "%");
				}
				
				EditorGUI.indentLevel--;
			}
			
			// Materials
			if (!SpriteCollection.allowMultipleAtlases)
			{
				DrawMaterialEditor();
			}
			
			GUILayout.EndVertical();
			GUILayout.EndScrollView();

			// make dragable
			tk2dPreferences.inst.spriteCollectionInspectorWidth -= (int)tk2dGuiUtility.DragableHandle(4819284, GUILayoutUtility.GetLastRect(), 0, tk2dGuiUtility.DragDirection.Horizontal);

			GUILayout.EndHorizontal();
		}		
	}
}
