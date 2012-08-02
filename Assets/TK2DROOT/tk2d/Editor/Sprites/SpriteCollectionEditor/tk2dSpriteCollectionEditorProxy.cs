using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace tk2dEditor.SpriteCollectionEditor
{
	// As nasty as this is, its a necessary evil for backwards compatibility
	public class SpriteCollectionProxy
	{
		public SpriteCollectionProxy()
		{
		}
		
		public SpriteCollectionProxy(tk2dSpriteCollection obj)
		{
			this.obj = obj;
			CopyFromSource();
		}
		
		public void CopyFromSource()
		{
			textureParams = new List<tk2dSpriteCollectionDefinition>(obj.textureParams.Length);
			foreach (var v in obj.textureParams)
			{
				if (v == null) 
					textureParams.Add(null);
				else 
				{
					var t = new tk2dSpriteCollectionDefinition();
					t.CopyFrom(v);
					textureParams.Add(t);
				}
			}
			
			textureRefs = new List<Texture2D>(obj.textureRefs.Length);
			foreach (var v in obj.textureRefs)
				textureRefs.Add(v);
			
			spriteSheets = new List<tk2dSpriteSheetSource>();
			if (obj.spriteSheets != null)
			{
				foreach (var v in obj.spriteSheets)
				{
					if (v == null) 
						spriteSheets.Add(null);
					else
					{
						var t = new tk2dSpriteSheetSource();
						t.CopyFrom(v);
						spriteSheets.Add(t);
					}
				}
			}
			
			fonts = new List<tk2dSpriteCollectionFont>();
			if (obj.fonts != null)
			{
				foreach (var v in obj.fonts)
				{
					if (v == null)
						fonts.Add(null);
					else
					{
						var t = new tk2dSpriteCollectionFont();
						t.CopyFrom(v);
						fonts.Add(t);
					}
				}
			}

			UpgradeLegacySpriteSheets();
			
			var target = this;
			var source = obj;
			
			target.maxTextureSize = source.maxTextureSize;
			target.forceTextureSize = source.forceTextureSize;
			target.forcedTextureWidth = source.forcedTextureWidth;
			target.forcedTextureHeight = source.forcedTextureHeight;
			
			target.textureCompression = source.textureCompression;
			target.atlasWidth = source.atlasWidth;
			target.atlasHeight = source.atlasHeight;
			target.forceSquareAtlas = source.forceSquareAtlas;
			target.atlasWastage = source.atlasWastage;
			target.allowMultipleAtlases = source.allowMultipleAtlases;
			
			target.spriteCollection = source.spriteCollection;
			target.premultipliedAlpha = source.premultipliedAlpha;
			
			CopyArray(ref target.altMaterials, source.altMaterials);
			CopyArray(ref target.atlasMaterials, source.atlasMaterials);
			CopyArray(ref target.atlasTextures, source.atlasTextures);
			
			target.useTk2dCamera = source.useTk2dCamera;
			target.targetHeight = source.targetHeight;
			target.targetOrthoSize = source.targetOrthoSize;
			target.pixelPerfectPointSampled = source.pixelPerfectPointSampled;
			target.physicsDepth = source.physicsDepth;
			target.disableTrimming = source.disableTrimming;
			target.normalGenerationMode = source.normalGenerationMode;
			target.padAmount = source.padAmount;
			target.autoUpdate = source.autoUpdate;
			target.editorDisplayScale = source.editorDisplayScale;
		}
		
		void CopyArray<T>(ref T[] dest, T[] source)
		{
			if (source == null)
			{
				dest = new T[0];
			}
			else
			{
				dest = new T[source.Length];
				for (int i = 0; i < source.Length; ++i)
					dest[i] = source[i];
			}
		}
		
		void UpgradeLegacySpriteSheets()
		{
			if (spriteSheets != null)
			{
				for (int i = 0; i < spriteSheets.Count; ++i)
				{
					var spriteSheet = spriteSheets[i];
					if (spriteSheet != null && spriteSheet.version == 0)
					{
						if (spriteSheet.texture == null)
						{
							spriteSheet.active = false;
						}
						else
						{
							spriteSheet.tileWidth = spriteSheet.texture.width / spriteSheet.tilesX;
							spriteSheet.tileHeight = spriteSheet.texture.height / spriteSheet.tilesY;
							spriteSheet.active = true;
							
							for (int j = 0; j < textureParams.Count; ++j)
							{
								var param = textureParams[j];
								if (param.fromSpriteSheet && textureRefs[j] == spriteSheet.texture)
								{
									param.fromSpriteSheet = false;
									param.hasSpriteSheetId = true;
									param.spriteSheetId = i;
									
									param.spriteSheetX = param.regionId % spriteSheet.tilesX;
									param.spriteSheetY = param.regionId / spriteSheet.tilesX;
								}
							}
						}
						
						spriteSheet.version = tk2dSpriteSheetSource.CURRENT_VERSION;
					}
				}				
			}
		}
		
		public void CopyToTarget()
		{
			obj.textureParams = textureParams.ToArray();
			obj.textureRefs = textureRefs.ToArray();
			obj.spriteSheets = spriteSheets.ToArray();
			obj.fonts = fonts.ToArray();

			var target = obj;
			var source = this;
			
			target.maxTextureSize = source.maxTextureSize;
			target.forceTextureSize = source.forceTextureSize;
			target.forcedTextureWidth = source.forcedTextureWidth;
			target.forcedTextureHeight = source.forcedTextureHeight;
			
			target.textureCompression = source.textureCompression;
			target.atlasWidth = source.atlasWidth;
			target.atlasHeight = source.atlasHeight;
			target.forceSquareAtlas = source.forceSquareAtlas;
			target.atlasWastage = source.atlasWastage;
			target.allowMultipleAtlases = source.allowMultipleAtlases;
			
			target.spriteCollection = source.spriteCollection;
			target.premultipliedAlpha = source.premultipliedAlpha;
			
			CopyArray(ref target.altMaterials, source.altMaterials);
			CopyArray(ref target.atlasMaterials, source.atlasMaterials);
			CopyArray(ref target.atlasTextures, source.atlasTextures);

			target.useTk2dCamera = source.useTk2dCamera;
			target.targetHeight = source.targetHeight;
			target.targetOrthoSize = source.targetOrthoSize;
			target.pixelPerfectPointSampled = source.pixelPerfectPointSampled;
			target.physicsDepth = source.physicsDepth;
			target.disableTrimming = source.disableTrimming;
			target.normalGenerationMode = source.normalGenerationMode;
			target.padAmount = source.padAmount; 
			target.autoUpdate = source.autoUpdate;
			target.editorDisplayScale = source.editorDisplayScale;
		}
		
		public bool AllowAltMaterials
		{
			get
			{
				return !allowMultipleAtlases;
			}
		}
		
		public int FindOrCreateEmptySpriteSlot()
		{
			for (int index = 0; index < textureRefs.Count; ++index)
			{
				if (textureRefs[index] == null)
					return index;
			}
			textureRefs.Add(null);
			textureParams.Add(new tk2dSpriteCollectionDefinition());
			return textureRefs.Count - 1;
		}
		
		public int FindOrCreateEmptyFontSlot()
		{
			for (int index = 0; index < fonts.Count; ++index)
			{
				if (!fonts[index].active)
				{
					fonts[index].active = true;
					return index;
				}
			}
			var font = new tk2dSpriteCollectionFont();
			font.active = true;
			fonts.Add(font);
			return fonts.Count - 1;
		}
		
		public int FindOrCreateEmptySpriteSheetSlot()
		{
			for (int index = 0; index < spriteSheets.Count; ++index)
			{
				if (!spriteSheets[index].active)
				{
					spriteSheets[index].active = true;
					spriteSheets[index].version = tk2dSpriteSheetSource.CURRENT_VERSION;
					return index;
				}
			}
			var spriteSheet = new tk2dSpriteSheetSource();
			spriteSheet.active = true;
			spriteSheet.version = tk2dSpriteSheetSource.CURRENT_VERSION;
			spriteSheets.Add(spriteSheet);
			return spriteSheets.Count - 1;
		}
		
		public string FindUniqueTextureName(string name)
		{
			List<string> textureNames = new List<string>();
			foreach (var entry in textureParams)
			{
				textureNames.Add(entry.name);
			}
			if (textureNames.IndexOf(name) == -1)
				return name;
			int count = 1;
			do 
			{
				string currName = name + " " + count.ToString();
				if (textureNames.IndexOf(currName) == -1)
					return currName;
				++count;
			} while(count < 1024); // arbitrary large number
			return name; // failed to find a name
		}
		
		public bool Empty { get { return textureRefs.Count == 0 && fonts.Count == 0 && spriteSheets.Count == 0; } }
		
		// Call after deleting anything
		public void Trim()
		{
			int lastIndex = textureRefs.Count - 1;
			while (lastIndex >= 0)
			{
				if (textureRefs[lastIndex] != null)
					break;
				lastIndex--;
			}
			int count = textureRefs.Count - 1 - lastIndex;
			if (count > 0)
			{
				textureRefs.RemoveRange( lastIndex + 1, count );
				textureParams.RemoveRange( lastIndex + 1, count );
			}
			
			lastIndex = fonts.Count - 1;
			while (lastIndex >= 0)
			{
				if (fonts[lastIndex].active)
					break;
				lastIndex--;
			}
			count = fonts.Count - 1 - lastIndex;
			if (count > 0) fonts.RemoveRange(lastIndex + 1, count);
			
			lastIndex = spriteSheets.Count - 1;
			while (lastIndex >= 0)
			{
				if (spriteSheets[lastIndex].active)
					break;
				lastIndex--;
			}
			count = spriteSheets.Count - 1 - lastIndex;
			if (count > 0) spriteSheets.RemoveRange(lastIndex + 1, count);
			
			lastIndex = atlasMaterials.Length - 1;
			while (lastIndex >= 0)
			{
				if (atlasMaterials[lastIndex] != null)
					break;
				lastIndex--;
			}
			count = atlasMaterials.Length - 1 - lastIndex;
			if (count > 0) 
				System.Array.Resize(ref atlasMaterials, lastIndex + 1);
		}
		
		public int GetSpriteSheetId(tk2dSpriteSheetSource spriteSheet)
		{
			for (int index = 0; index < spriteSheets.Count; ++index)
				if (spriteSheets[index] == spriteSheet) return index;
			return 0;
		}
		
		// Delete all sprites from a spritesheet
		public void DeleteSpriteSheet(tk2dSpriteSheetSource spriteSheet)
		{
			int index = GetSpriteSheetId(spriteSheet);
			
			for (int i = 0; i < textureParams.Count; ++i)
			{
				if (textureParams[i].hasSpriteSheetId && textureParams[i].spriteSheetId == index)
				{
					textureRefs[i] = null;
					textureParams[i] = new tk2dSpriteCollectionDefinition();
				}
			}
			
			spriteSheets[index] = new tk2dSpriteSheetSource();
			Trim();
		}
		
		public string GetAssetPath()
		{
			return AssetDatabase.GetAssetPath(obj);
		}

		public bool Ready { get { return obj != null; } }
		tk2dSpriteCollection obj;
		

		// Mirrored data objects
		public List<Texture2D> textureRefs = new List<Texture2D>();
		public List<tk2dSpriteCollectionDefinition> textureParams = new List<tk2dSpriteCollectionDefinition>();
		public List<tk2dSpriteSheetSource> spriteSheets = new List<tk2dSpriteSheetSource>();
		public List<tk2dSpriteCollectionFont> fonts = new List<tk2dSpriteCollectionFont>();
		
		// Mirrored from sprite collection
		public int maxTextureSize;
		public tk2dSpriteCollection.TextureCompression textureCompression;
		public int atlasWidth, atlasHeight;
		public bool forceSquareAtlas;
		public float atlasWastage;
		public bool allowMultipleAtlases;
		public tk2dSpriteCollectionData spriteCollection;
	    public bool premultipliedAlpha;
		
		public Material[] altMaterials;
		public Material[] atlasMaterials;
		public Texture2D[] atlasTextures;
		
		public bool useTk2dCamera;
		public int targetHeight;
		public float targetOrthoSize;
		public bool pixelPerfectPointSampled;
		public float physicsDepth;
		public bool disableTrimming;
		
		public bool forceTextureSize = false;
		public int forcedTextureWidth = 1024;
		public int forcedTextureHeight = 1024;
		
		public tk2dSpriteCollection.NormalGenerationMode normalGenerationMode;
		public int padAmount;
		public bool autoUpdate;
		
		public float editorDisplayScale;
	}
}

