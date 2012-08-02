using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace tk2dEditor
{
	
	class BrushDictData
	{
		public tk2dTileMapEditorBrush brush;
		public int brushHash;
		public Mesh mesh;
		public Rect rect;
	}
	
	public class BrushRenderer
	{
		tk2dTileMap tileMap;
		tk2dSpriteCollectionData spriteCollection;
		Dictionary<tk2dTileMapEditorBrush, BrushDictData> brushLookupDict = new  Dictionary<tk2dTileMapEditorBrush, BrushDictData>();
		
		public BrushRenderer(tk2dTileMap tileMap)
		{
			this.tileMap = tileMap;
			this.spriteCollection = tileMap.spriteCollection;
		}
		
		public void Destroy()
		{
			foreach (var v in brushLookupDict.Values)
			{
				Mesh.DestroyImmediate(v.mesh);
				v.mesh = null;
			}
		}
		
		// Build a mesh for a list of given sprites
		void BuildMeshForBrush(tk2dTileMapEditorBrush brush, BrushDictData dictData, int tilesPerRow)
		{
			List<Vector3> vertices = new List<Vector3>();
			List<Vector2> uvs = new List<Vector2>();
			List<int> triangles = new List<int>();
			
			// bounds of tile
			Vector3 spriteBounds = spriteCollection.FirstValidDefinition.untrimmedBoundsData[1];
			Vector3 tileSize = brush.overrideWithSpriteBounds?
								spriteBounds:
								tileMap.data.tileSize;
			float layerOffset = 0.001f;
			
			Vector3 boundsMin = new Vector3(1.0e32f, 1.0e32f, 1.0e32f);
			Vector3 boundsMax = new Vector3(-1.0e32f, -1.0e32f, -1.0e32f);
		
			float tileOffsetX = 0, tileOffsetY = 0;
			if (!brush.overrideWithSpriteBounds)
				tileMap.data.GetTileOffset(out tileOffsetX, out tileOffsetY);
			
			if (brush.type == tk2dTileMapEditorBrush.Type.MultiSelect)
			{
				int tileX = 0;
				int tileY = brush.multiSelectTiles.Length / tilesPerRow;
				if ((brush.multiSelectTiles.Length % tilesPerRow) == 0) tileY -=1;
				foreach (var uncheckedSpriteId in brush.multiSelectTiles)
				{
					float xOffset = (tileY & 1) * tileOffsetX;
				
					// The origin of the tile in mesh space
					Vector3 tileOrigin = new Vector3((tileX + xOffset) * tileSize.x, tileY * tileSize.y, 0.0f);
					//if (brush.overrideWithSpriteBounds)
					{
						boundsMin = Vector3.Min(boundsMin, tileOrigin);
						boundsMax = Vector3.Max(boundsMax, tileOrigin + tileSize);
					}

					if (uncheckedSpriteId != -1)
					{
						int indexRoot = vertices.Count;
						int spriteId = Mathf.Clamp(uncheckedSpriteId, 0, spriteCollection.Count - 1);
						var sprite = spriteCollection.spriteDefinitions[spriteId];
			
						for (int j = 0; j < sprite.positions.Length; ++j)
						{
							// Sprite vertex, centered around origin
							Vector3 centeredSpriteVertex = sprite.positions[j] - sprite.untrimmedBoundsData[0];
							
							// Offset so origin is at bottom left
							Vector3 v = centeredSpriteVertex + sprite.untrimmedBoundsData[1] * 0.5f;
							
							boundsMin = Vector3.Min(boundsMin, tileOrigin + v);
							boundsMax = Vector3.Max(boundsMax, tileOrigin + v);
							
							vertices.Add(tileOrigin + v);
							uvs.Add(sprite.uvs[j]);
						}
						
						for (int j = 0; j < sprite.indices.Length; ++j)
						{
							triangles.Add(indexRoot + sprite.indices[j]);
						}
					}
					
					tileX += 1;
					if (tileX == tilesPerRow)
					{
						tileX = 0;
						tileY -= 1;
					}
				}				
			}
			else
			{
				// the brush is centered around origin, x to the right, y up
				foreach (var tile in brush.tiles)
				{
					float xOffset = (tile.y & 1) * tileOffsetX;
					
					// The origin of the tile in mesh space
					Vector3 tileOrigin = new Vector3((tile.x + xOffset) * tileSize.x, tile.y * tileSize.y, tile.layer * layerOffset);
					
					//if (brush.overrideWithSpriteBounds)
					{
						boundsMin = Vector3.Min(boundsMin, tileOrigin);
						boundsMax = Vector3.Max(boundsMax, tileOrigin + tileSize);
					}


					if (tile.spriteId == -1)
						continue;
					
					int indexRoot = vertices.Count;
					tile.spriteId = (ushort)Mathf.Clamp(tile.spriteId, 0, spriteCollection.Count - 1);
					var sprite = spriteCollection.spriteDefinitions[tile.spriteId];
		
					for (int j = 0; j < sprite.positions.Length; ++j)
					{
						// Sprite vertex, centered around origin
						Vector3 centeredSpriteVertex = sprite.positions[j] - sprite.untrimmedBoundsData[0];
						
						// Offset so origin is at bottom left
						Vector3 v = centeredSpriteVertex + sprite.untrimmedBoundsData[1] * 0.5f;

						boundsMin = Vector3.Min(boundsMin, tileOrigin + v);
						boundsMax = Vector3.Max(boundsMax, tileOrigin + v);
						
						vertices.Add(tileOrigin + v);
						uvs.Add(sprite.uvs[j]);
					}
					
					for (int j = 0; j < sprite.indices.Length; ++j)
					{
						triangles.Add(indexRoot + sprite.indices[j]);
					}
				}
			}
			
			if (dictData.mesh == null)
			{
				dictData.mesh = new Mesh();
				dictData.mesh.hideFlags = HideFlags.DontSave;
			}
			
			Mesh mesh = dictData.mesh;
			mesh.Clear();
			mesh.vertices = vertices.ToArray(); 
			Color[] colors = new Color[vertices.Count];
			for (int i = 0; i < vertices.Count; ++i)
				colors[i] = Color.white;
			mesh.colors = colors;
			mesh.uv = uvs.ToArray();
			mesh.triangles = triangles.ToArray();
			
			dictData.brush = brush;
			dictData.brushHash = brush.brushHash;
			dictData.mesh = mesh;
			dictData.rect = new Rect(boundsMin.x, boundsMin.y, boundsMax.x - boundsMin.x, boundsMax.y - boundsMin.y);
		}
		
		BrushDictData GetDictDataForBrush(tk2dTileMapEditorBrush brush, int tilesPerRow)
		{
			BrushDictData dictEntry;
			if (brushLookupDict.TryGetValue(brush, out dictEntry))
			{
				if (brush.brushHash != dictEntry.brushHash)
				{
					BuildMeshForBrush(brush, dictEntry, tilesPerRow);
				}
				return dictEntry;
			}
			else
			{
				dictEntry = new BrushDictData();
				BuildMeshForBrush(brush, dictEntry, tilesPerRow);
				brushLookupDict[brush] = dictEntry;
				return dictEntry;
			}
		}
		
		float lastScale;
		public float LastScale { get { return lastScale; } }
		
		public Rect DrawBrush(tk2dTileMap tileMap, tk2dTileMapEditorBrush brush, float scale, bool forceUnitSpacing, int tilesPerRow)
		{
			var dictData = GetDictDataForBrush(brush, tilesPerRow);
			Mesh atlasViewMesh = dictData.mesh;
			Rect atlasViewRect = BrushToScreenRect(dictData.rect);
			
			float width = atlasViewRect.width * scale;
			float height = atlasViewRect.height * scale;
			
			float maxScreenWidth = Screen.width - 16;
			if (width > maxScreenWidth)
			{
				height = height * maxScreenWidth / width;
				width = maxScreenWidth;
			}
			
			Rect rect = GUILayoutUtility.GetRect(width, height, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
			scale = width / atlasViewRect.width;
			lastScale = scale;
			
			if (Event.current.type == EventType.Repaint)
			{
				tileMap.spriteCollection.materials[0].SetPass(0);
				Matrix4x4 mat = new Matrix4x4();
				var spriteDef = tileMap.spriteCollection.spriteDefinitions[0];
				mat.SetTRS(new Vector3(rect.x, 
									   rect.y + height, 0), Quaternion.identity, new Vector3(scale / spriteDef.texelSize.x, -scale / spriteDef.texelSize.y, 1));
				Graphics.DrawMeshNow(atlasViewMesh, mat * GUI.matrix);
			}
			
			return rect;
		}
		
		public Vector3 TexelSize
		{
			get
			{
				return spriteCollection.spriteDefinitions[0].texelSize;
			}
		}
		
		public Rect BrushToScreenRect(Rect rect)
		{
			Vector3 texelSize = TexelSize;
				
			int w = (int)(rect.width / texelSize.x);
			int h = (int)(rect.height / texelSize.y);
			
			return new Rect(0, 0, w, h);
		}
		
		public Rect TileSizePixels
		{
			get 
			{
				Vector3 texelSize = TexelSize;
				Vector3 tileSize = spriteCollection.spriteDefinitions[0].untrimmedBoundsData[1];
				return new Rect(0, 0, tileSize.x / texelSize.x, tileSize.y / texelSize.y);
			}
		}
	}

}
