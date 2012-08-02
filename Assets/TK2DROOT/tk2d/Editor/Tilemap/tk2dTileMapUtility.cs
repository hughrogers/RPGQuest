using UnityEngine;
using System.Collections.Generic;

using tk2dRuntime.TileMap;

namespace tk2dEditor.TileMap
{
	public static class TileMapUtility
	{
		public static int MaxWidth = 512;
		public static int MaxHeight = 512;
		public static int MaxLayers = 32;
		
		public static void ResizeTileMap(tk2dTileMap tileMap, int width, int height, int partitionSizeX, int partitionSizeY)
		{
			int w = Mathf.Clamp(width, 1, MaxWidth);
			int h = Mathf.Clamp(height, 1, MaxHeight);
			
			tk2dRuntime.TileMap.BuilderUtil.InitDataStore(tileMap);

			// copy into new tilemap
			Layer[] layers = new Layer[tileMap.Layers.Length];
			for (int layerId = 0; layerId < tileMap.Layers.Length; ++layerId)
			{
				var srcLayer = tileMap.Layers[layerId];
				layers[layerId] = new Layer(srcLayer.hash, width, height, partitionSizeX, partitionSizeY);
				var destLayer = layers[layerId];
				
				if (srcLayer.IsEmpty)
					continue;
				
				int hcopy = Mathf.Min(tileMap.height, h);
				int wcopy = Mathf.Min(tileMap.width, w);
				
				for (int y = 0; y < hcopy; ++y)
				{
					for (int x = 0; x < wcopy; ++x)
					{
						destLayer.SetTile(x, y, srcLayer.GetTile(x, y));
					}
				}
				
				destLayer.Optimize();
			}
			
			// copy new colors
			bool copyColors = (tileMap.ColorChannel != null && !tileMap.ColorChannel.IsEmpty);
			ColorChannel targetColors = new ColorChannel(width, height, partitionSizeX, partitionSizeY);
			if (copyColors)
			{
				int hcopy = Mathf.Min(tileMap.height, h) + 1;
				int wcopy = Mathf.Min(tileMap.width, w) + 1;
				for (int y = 0; y < hcopy; ++y)
				{
					for (int x = 0; x < wcopy; ++x)
					{
						targetColors.SetColor(x, y, tileMap.ColorChannel.GetColor(x, y));
					}
				}
				
				targetColors.Optimize();
			}
		
			tileMap.ColorChannel = targetColors;
			tileMap.Layers = layers;
			tileMap.width = w;
			tileMap.height = h;
			tileMap.partitionSizeX = partitionSizeX;
			tileMap.partitionSizeY = partitionSizeY;
			
			tk2dRuntime.TileMap.BuilderUtil.CleanRenderData(tileMap);
		}
		
		// Returns index of newly added layer
		public static int AddNewLayer(tk2dTileMap tileMap)
		{
			var existingLayers = tileMap.data.Layers;
			// find a unique hash
			bool duplicateHash = false;
			int hash;
			do
			{
				duplicateHash = false;
				hash = Random.Range(0, int.MaxValue);
				foreach (var layer in existingLayers) 
					if (layer.hash == hash) 
						duplicateHash = true;
			} while (duplicateHash == true);
			
			var newLayer = new tk2dRuntime.TileMap.LayerInfo();
			newLayer.name = "New Layer";
			newLayer.hash = hash;
			newLayer.z = 0.1f;
			tileMap.data.tileMapLayers.Add(newLayer);
			
			// remap tilemap
			tk2dRuntime.TileMap.BuilderUtil.InitDataStore(tileMap);
			
			return tileMap.data.NumLayers - 1;
		}
		
		public static int FindOrCreateLayer(tk2dTileMap tileMap, string name)
		{
			int index = 0;
			foreach (var v in tileMap.data.Layers)
			{
				if (v.name == name)
					return index;
				++index;
			}
			index = AddNewLayer(tileMap);
			tileMap.data.Layers[index].name = name;
			return index;
		}
		
		public static void DeleteLayer(tk2dTileMap tileMap, int layerToDelete)
		{
			// Just in case
			if (tileMap.data.NumLayers <= 1)
				return;
			
			tk2dRuntime.TileMap.BuilderUtil.CleanRenderData(tileMap);
			tileMap.data.tileMapLayers.RemoveAt(layerToDelete);
			tk2dRuntime.TileMap.BuilderUtil.InitDataStore(tileMap);
		}
		
		public static void MoveLayer(tk2dTileMap tileMap, int layer, int direction)
		{
			tk2dRuntime.TileMap.BuilderUtil.CleanRenderData(tileMap);
			var tmp = tileMap.data.tileMapLayers[layer];
			tileMap.data.tileMapLayers[layer] = tileMap.data.tileMapLayers[layer + direction];
			tileMap.data.tileMapLayers[layer + direction] = tmp;
			tk2dRuntime.TileMap.BuilderUtil.InitDataStore(tileMap);
		}
	}
}
