using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class tk2dTileMapSceneGUI
{
	ITileMapEditorHost host;
	tk2dTileMap tileMap;
	tk2dTileMapData tileMapData;
	tk2dTileMapEditorData editorData;

	int cursorX0 = 0, cursorY0 = 0;
	int cursorX = 0, cursorY = 0;
	int vertexCursorX = 0, vertexCursorY = 0;
	
	Color tileSelectionFillColor = new Color32(0, 128, 255, 32);
	Color tileSelectionOutlineColor = new Color32(0,200,255,255);	
	
	bool pickup = false;
	bool erase = false;
	
	public tk2dTileMapSceneGUI(ITileMapEditorHost host, tk2dTileMap tileMap, tk2dTileMapEditorData editorData)
	{
		this.host = host;
		this.tileMap = tileMap;
		this.editorData = editorData;
		this.tileMapData = tileMap.data;
		
		// create default brush
		if (tileMap.spriteCollection && this.editorData)
		{
			this.editorData.InitBrushes(tileMap.spriteCollection);
			EditorUtility.SetDirty(this.editorData);
		}
	}
	
	public void Destroy()
	{
	}
	
	void DrawCursorAt(int x, int y)
	{
		switch (tileMap.data.tileType)
		{
		case tk2dTileMapData.TileType.Rectangular:
			{
				float xOffsetMult, yOffsetMult;
				tileMap.data.GetTileOffset(out xOffsetMult, out yOffsetMult);
				float xOffset = (y & 1) * xOffsetMult;
				Vector3 p0 = new Vector3(tileMapData.tileOrigin.x + (x + xOffset) * tileMapData.tileSize.x, tileMapData.tileOrigin.y + y * tileMapData.tileSize.y, 0);
				Vector3 p1 = new Vector3(p0.x + tileMapData.tileSize.x, p0.y + tileMapData.tileSize.y, 0);
				Vector3[] v = new Vector3[4];
				v[0] = new Vector3(p0.x, p0.y, 0);
				v[1] = new Vector3(p1.x, p0.y, 0);
				v[2] = new Vector3(p1.x, p1.y, 0);
				v[3] = new Vector3(p0.x, p1.y, 0);
				
				for (int i = 0; i < v.Length; ++i)
					v[i] = tileMap.transform.TransformPoint(v[i]);
				
				Handles.DrawSolidRectangleWithOutline(v, tileSelectionFillColor, tileSelectionOutlineColor);
			}	
			break;
		case tk2dTileMapData.TileType.Isometric:
			{
				float xOffsetMult, yOffsetMult;
				tileMap.data.GetTileOffset(out xOffsetMult, out yOffsetMult);
				float xOffset = (y & 1) * xOffsetMult;
				Vector3 p0 = new Vector3(tileMapData.tileOrigin.x + (x + xOffset) * tileMapData.tileSize.x, tileMapData.tileOrigin.y + y * tileMapData.tileSize.y, 0);
				Vector3 p1 = new Vector3(p0.x + tileMapData.tileSize.x, p0.y + tileMapData.tileSize.y * 2, 0);
				Vector3[] v = new Vector3[4];
				v[0] = new Vector3(p0.x + (p1.x-p0.x)*0.5f, p0.y, 0);
				v[1] = new Vector3(p1.x, p0.y + (p1.y-p0.y)*0.5f, 0);
				v[2] = new Vector3(p1.x - (p1.x-p0.x)*0.5f, p1.y, 0);
				v[3] = new Vector3(p0.x, p1.y - (p1.y-p0.y)*0.5f, 0);
				
				for (int i = 0; i < v.Length; ++i)
					v[i] = tileMap.transform.TransformPoint(v[i]);
				
				Handles.DrawSolidRectangleWithOutline(v, tileSelectionFillColor, tileSelectionOutlineColor);
			}	
			break;
		}
	}
	
	void DrawTileCursor(tk2dTileMapEditorBrush brush)
	{
		float xOffsetMult, yOffsetMult;
		tileMap.data.GetTileOffset(out xOffsetMult, out yOffsetMult);
		
		if (brush.paintMode == tk2dTileMapEditorBrush.PaintMode.Random ||
			brush.paintMode == tk2dTileMapEditorBrush.PaintMode.Edged ||
			pickup ||
			erase)
		{
			int x0 = cursorX;
			int y0 = cursorY;
			x0 = Mathf.Min(cursorX, cursorX0);
			y0 = Mathf.Min(cursorY, cursorY0);
			int x1 = Mathf.Max(cursorX, cursorX0);
			int y1 = Mathf.Max(cursorY, cursorY0);

			for (int y = y0; y <= y1; ++y)
			{
				for (int x = x0; x <= x1; ++x)
				{
					DrawCursorAt(x, y);
				}
			}
		}
		else if (brush.type == tk2dTileMapEditorBrush.Type.MultiSelect)
		{
			DrawCursorAt(cursorX, cursorY);
		}
		else
		{
			int xoffset = 0;
			if (tileMap.data.tileType == tk2dTileMapData.TileType.Isometric &&  (cursorY & 1) == 1) 
				xoffset = 1;
			
			foreach (var tile in brush.tiles)
			{
				int thisRowXOffset = (((cursorY + tile.y) & 1) == 0)?xoffset:0;
				DrawCursorAt(cursorX + tile.x + thisRowXOffset, cursorY + tile.y);
			}
		}
	}

	void DrawPaintCursor()
	{
		float layerZ = 0.0f;
		
		Vector3 p0 = new Vector3(tileMapData.tileOrigin.x + vertexCursorX * tileMapData.tileSize.x, tileMapData.tileOrigin.y + vertexCursorY * tileMapData.tileSize.y, layerZ);
		float radius = Mathf.Max(tileMapData.tileSize.x, tileMapData.tileSize.y) * editorData.brushRadius;
		
		Color oldColor = Handles.color;
		Handles.color = editorData.brushColor * new Color(1,1,1,0.5f);
		Handles.DrawSolidDisc(tileMap.transform.TransformPoint(p0), tileMap.transform.TransformDirection(Vector3.forward), radius);
		Handles.color = oldColor;
	}
	
	void DrawOutline()
	{
		Vector3 p0 = tileMapData.tileOrigin;
		Vector3 p1 = new Vector3(p0.x + tileMapData.tileSize.x * tileMap.width, p0.y + tileMapData.tileSize.y * tileMap.height, 0);
		
		Vector3[] v = new Vector3[5];
		v[0] = new Vector3(p0.x, p0.y, 0);
		v[1] = new Vector3(p1.x, p0.y, 0);
		v[2] = new Vector3(p1.x, p1.y, 0);
		v[3] = new Vector3(p0.x, p1.y, 0);
		v[4] = new Vector3(p0.x, p0.y, 0);
		
		for (int i = 0; i < 5; ++i)
		{
			v[i] = tileMap.transform.TransformPoint(v[i]);
		}
		
		Handles.DrawPolyLine(v);
	}
	
	public static int tileMapHashCode = "TileMap".GetHashCode();
	
	bool UpdateCursorPosition()
	{
		bool isInside = false;
		
		Plane p = new Plane(tileMap.transform.forward, tileMap.transform.position);
		Ray r = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
		float hitD = 0.0f;
		
		if (p.Raycast(r, out hitD))
		{
			float fx, fy;
			if (tileMap.GetTileFracAtPosition(r.GetPoint(hitD), out fx, out fy))
			{
				int x = (int)(fx);
				int y = (int)(fy);
				
				cursorX = Mathf.Clamp(x, 0, tileMap.width - 1);
				cursorY = Mathf.Clamp(y, 0, tileMap.height - 1);
				vertexCursorX = (int)Mathf.Round(fx);
				vertexCursorY = (int)Mathf.Round(fy);
				
				HandleUtility.Repaint();
				
				isInside = true;
			}
		}
		
		return isInside;
	}
	
	bool IsCursorInside()
	{
		return UpdateCursorPosition();
	}
	
	void Splat()
	{
		tk2dTileMapEditorBrush activeBrush = editorData.activeBrush;
		
		switch (editorData.editMode)
		{
		case tk2dTileMapEditorData.EditMode.Paint: 
			SplatBrushAt(activeBrush, cursorX, cursorY, editorData.layer); 
			host.BuildIncremental();
			break;
		case tk2dTileMapEditorData.EditMode.Color: 
			PaintAt(vertexCursorX, vertexCursorY); 
			host.BuildIncremental();
			break;
		}
	}

	void CommitSplats()
	{
		if (editorData.editMode == tk2dTileMapEditorData.EditMode.Paint)
			CommitBrushSplats(editorData.activeBrush);
	}
	
	void PickUp()
	{
		if (editorData.editMode == tk2dTileMapEditorData.EditMode.Paint)
			PickUpBrush(editorData.activeBrush, false);
		else if (editorData.editMode == tk2dTileMapEditorData.EditMode.Color)
			PickUpColor();
	}
	
	void Erase()
	{
		if (editorData.editMode == tk2dTileMapEditorData.EditMode.Paint)
		{
			int x0 = Mathf.Min(cursorX, cursorX0);
			int x1 = Mathf.Max(cursorX, cursorX0);
			int y0 = Mathf.Min(cursorY, cursorY0);
			int y1 = Mathf.Max(cursorY, cursorY0);

			tileMap.DeleteSprites(editorData.layer, x0, y0, x1, y1);
			host.BuildIncremental();
		}
	}
	
	void CheckVisible(int layer)
	{
		if (tileMap != null && 
			tileMap.Layers != null &&
			layer < tileMap.Layers.Length &&
			tileMap.Layers[layer].gameObject != null &&
			tileMap.Layers[layer].gameObject.active == false)
		{
			tileMap.Layers[layer].gameObject.SetActiveRecursively(true);
		}
	}
	
	void DrawTileProperties()
	{
		DrawCursorAt(0, 0);
	}
	
	public void OnSceneGUI()
	{
		// Always draw the outline
		DrawOutline();
		
		if (Application.isPlaying || !tileMap.AllowEdit)
			return;
		
		if (editorData.editMode == tk2dTileMapEditorData.EditMode.Settings)
		{
			if (editorData.setupMode == tk2dTileMapEditorData.SetupMode.TileProperties)
				DrawTileProperties();
			return;
		}
		
		if (editorData.editMode != tk2dTileMapEditorData.EditMode.Paint && 
			editorData.editMode != tk2dTileMapEditorData.EditMode.Color)
			return;

		if (editorData.editMode == tk2dTileMapEditorData.EditMode.Color &&
			!tileMap.HasColorChannel())
			return;
		
		int controlID = GUIUtility.GetControlID(tileMapHashCode, FocusType.Passive);
		EventType controlEventType = Event.current.GetTypeForControl(controlID);
		switch (controlEventType)
		{
		case EventType.MouseDown:
		case EventType.MouseDrag:
			if ((controlEventType == EventType.MouseDrag && GUIUtility.hotControl != controlID) ||
				Event.current.button != 0)
			{
				return;
			}
			
			if (Event.current.type == EventType.MouseDown)
			{
				CheckVisible(editorData.layer);
				
				if (IsCursorInside() && !Event.current.shift)
				{
					bool pickupKeyDown = (Application.platform == RuntimePlatform.OSXEditor)?Event.current.control:Event.current.alt;
					bool eraseKeyDown = false;
					if (Application.platform == RuntimePlatform.OSXEditor)
					{
						if (Event.current.command && !Event.current.alt)
							eraseKeyDown = true;
					}
					else eraseKeyDown = Event.current.control;
					
					if (pickupKeyDown)
					{
						pickup = true;
						GUIUtility.hotControl = controlID;
					}
					else if (eraseKeyDown)
					{
						Undo.RegisterUndo(tileMap, "Erased tile map");
						erase = true;
						GUIUtility.hotControl = controlID;
					}
					else if (!Event.current.command && !Event.current.alt && !Event.current.control)
					{
						GUIUtility.hotControl = controlID;
						randomSeed = Random.Range(0, int.MaxValue);
						Undo.RegisterUndo(tileMap, "Painted tile map");
						Splat();
					}
				}
			}
		
			if (Event.current.type == EventType.MouseDrag && GUIUtility.hotControl == controlID)
			{
				UpdateCursorPosition();
				if (!pickup && !erase)
				{
					Splat();	
				}
			}
			
			break;
			
		case EventType.MouseUp:
			if (Event.current.button == 0 && GUIUtility.hotControl == controlID)
			{
				GUIUtility.hotControl = 0;
				
				if (pickup)
				{
					PickUp();
					pickup = false;
				}
				else if (erase)
				{
					Erase();
					erase = false;
				}
				else
				{
					CommitSplats();
				}
				
				cursorX0 = cursorX;
				cursorY0 = cursorY;
				
				HandleUtility.Repaint();
			}
			break;
			
		case EventType.Layout:
			//HandleUtility.AddDefaultControl(controlID);
			break;
			
		case EventType.MouseMove:
			UpdateCursorPosition();
			cursorX0 = cursorX;
			cursorY0 = cursorY;
			break;
		}
		
		tk2dTileMapEditorBrush activeBrush = editorData.activeBrush;
		
		switch (editorData.editMode)
		{
		case tk2dTileMapEditorData.EditMode.Paint: DrawTileCursor(activeBrush); break;
		case tk2dTileMapEditorData.EditMode.Color: DrawPaintCursor(); break;
		}
		
	}
	
	void SplatTile(int x, int y, int layerId, int spriteId)
	{
		if (x >= 0 && x < tileMap.width &&
			y >= 0 && y < tileMap.height &&
			layerId >= 0 && layerId < tileMap.data.NumLayers)
		{
			var layer =	tileMap.Layers[layerId];
			layer.SetTile(x, y, spriteId);
		}
	}
	
	int randomSeed = 0;
	
	void SplatBrushAt(tk2dTileMapEditorBrush brush, int cx, int cy, int layer)
	{
		// only commit at the end when in rect mode
		if (brush.paintMode == tk2dTileMapEditorBrush.PaintMode.Random ||
			brush.paintMode == tk2dTileMapEditorBrush.PaintMode.Edged)
		{
			return;
		}
		
		if (brush.type == tk2dTileMapEditorBrush.Type.MultiSelect)
		{
			// seed a rng with the current tileId and the random seed generated when mouse press is first initiated 
			// this will give a conistent distribution per mouse press & drag
			// but will be unique(ish) per press
			var rng = new System.Random(randomSeed + cy * tileMap.width + cx);
			int tileId = brush.multiSelectTiles[rng.Next(brush.multiSelectTiles.Length)];
			SplatTile(cx, cy, editorData.layer, tileId);
		}
		else
		{
			int xoffset = 0;
			if (tileMap.data.tileType == tk2dTileMapData.TileType.Isometric &&  (cursorY & 1) == 1) 
				xoffset = 1;
			
			foreach (var tile in brush.tiles)
			{
				int thisRowXOffset = (((cursorY + tile.y) & 1) == 0)?xoffset:0;
				SplatTile(tile.x + cx + thisRowXOffset, tile.y + cy, tile.layer + layer, tile.spriteId);
			}
		}
	}
	
	void PickUpBrush(tk2dTileMapEditorBrush brush, bool allLayers)
	{
		int x0 = Mathf.Min(cursorX, cursorX0);
		int x1 = Mathf.Max(cursorX, cursorX0);
		int y0 = Mathf.Min(cursorY, cursorY0);
		int y1 = Mathf.Max(cursorY, cursorY0);
		int numTilesX = x1 - x0 + 1;
		int numTilesY = y1 - y0 + 1;
		
		List<tk2dSparseTile> sparseTile = new List<tk2dSparseTile>();
		List<int> tiles = new List<int>();
		
		int numLayers = tileMap.data.NumLayers;
		int startLayer = 0;
		int endLayer = numLayers;
		
		if (allLayers)
		{
			brush.multiLayer = true;
		}
		else
		{
			brush.multiLayer = false;
			startLayer = editorData.layer;
			endLayer = startLayer + 1;
		}
		
		if (tileMap.data.tileType == tk2dTileMapData.TileType.Rectangular)
		{
			for (int layer = startLayer; layer < endLayer; ++layer)
			{
				for (int y = numTilesY - 1; y >= 0; --y)
				{
					for (int x = 0; x < numTilesX; ++x)
					{
						int tile = tileMap.Layers[layer].GetTile(x0 + x, y0 + y);
						tiles.Add(tile);
						sparseTile.Add(new tk2dSparseTile(x, y, allLayers?layer:0, tile));
					}
				}
			}
		}
		else if (tileMap.data.tileType == tk2dTileMapData.TileType.Isometric)
		{
			int xOffset = 0;
			int yOffset = 0;
			if ((y0 & 1) != 0)
				yOffset -= 1;
			
			for (int layer = startLayer; layer < endLayer; ++layer)
			{
				for (int y = numTilesY - 1; y >= 0; --y)
				{
					for (int x = 0; x < numTilesX; ++x)
					{
						int tile = tileMap.Layers[layer].GetTile(x0 + x, y0 + y);
						tiles.Add(tile);
						sparseTile.Add(new tk2dSparseTile(x + xOffset, y + yOffset, allLayers?layer:0, tile));
					}
				}
			}
		}
		
		
		brush.type = tk2dTileMapEditorBrush.Type.Custom;
		
		if (numTilesX == 1 && numTilesY == 3) brush.edgeMode = tk2dTileMapEditorBrush.EdgeMode.Vertical;
		else if (numTilesX == 3 && numTilesY == 1) brush.edgeMode = tk2dTileMapEditorBrush.EdgeMode.Horizontal;
		else if (numTilesX == 3 && numTilesY == 3) brush.edgeMode = tk2dTileMapEditorBrush.EdgeMode.Square;
		else brush.edgeMode = tk2dTileMapEditorBrush.EdgeMode.None;
		
		brush.tiles = sparseTile.ToArray();
		brush.multiSelectTiles = tiles.ToArray();
		brush.UpdateBrushHash();
		
		// Make the inspector update
		EditorUtility.SetDirty(tileMap);
	}
	
	void CommitBrushSplats(tk2dTileMapEditorBrush brush)
	{
		if (brush.paintMode != tk2dTileMapEditorBrush.PaintMode.Random &&
			brush.paintMode != tk2dTileMapEditorBrush.PaintMode.Edged)
		{
			return;
		}

		int x0 = Mathf.Min(cursorX, cursorX0);
		int x1 = Mathf.Max(cursorX, cursorX0);
		int y0 = Mathf.Min(cursorY, cursorY0);
		int y1 = Mathf.Max(cursorY, cursorY0);
		int tilesX = x1 - x0 + 1;
		int tilesY = y1 - y0 + 1;
		
		if (brush.paintMode == tk2dTileMapEditorBrush.PaintMode.Edged && brush.edgeMode != tk2dTileMapEditorBrush.EdgeMode.None)
		{
			int numSlicedTilesX = (brush.edgeMode == tk2dTileMapEditorBrush.EdgeMode.Horizontal 
				|| brush.edgeMode == tk2dTileMapEditorBrush.EdgeMode.Square)?3:1;
			int numSlicedTilesY = (brush.edgeMode == tk2dTileMapEditorBrush.EdgeMode.Vertical 
				|| brush.edgeMode == tk2dTileMapEditorBrush.EdgeMode.Square)?3:1;
		
			for (int x = 0; x < tilesX; ++x)
			{
				for (int y = 0; y < tilesY; ++y)
				{
					// local y, tilemap space is y up, brush is y down
					int ly = (tilesY - 1 - y);
					
					// collapse tiles
					int tx;
					if (numSlicedTilesX == 1 || x == 0) tx = 0;
					else if (x == tilesX - 1) tx = 2;
					else tx = 1;
					
					int ty;
					if (numSlicedTilesY == 1 || ly == 0) ty = 0;
					else if (ly == tilesY - 1) ty = 2;
					else ty = 1;
					
					//int spriteId = brush.multiSelectTiles[0];
					int spriteId = brush.multiSelectTiles[ty * numSlicedTilesX + tx];
					SplatTile(x0 + x, y0 + y, editorData.layer, spriteId);
				}
			}
			
			host.BuildIncremental();
		}
		else
		{
			var rng = new System.Random(randomSeed);
			
			for (int x = 0; x < tilesX; ++x)
			{
				for (int y = 0; y < tilesY; ++y)
				{
					if (brush.type == tk2dTileMapEditorBrush.Type.MultiSelect || 
						brush.type == tk2dTileMapEditorBrush.Type.Single ||
						brush.type == tk2dTileMapEditorBrush.Type.Custom)
					{
						var spriteId = brush.multiSelectTiles[rng.Next(brush.multiSelectTiles.Length)];
						SplatTile(x0 + x, y0 + y, editorData.layer, spriteId);
					}
				}
			}
			
			host.BuildIncremental();
		}
	}
	
	void PaintAt(int cx, int cy)
	{
		int brushRadiusExtents = (int)Mathf.Ceil(editorData.brushRadius);
		int y0 = Mathf.Max(cy - brushRadiusExtents, 0);
		int y1 = Mathf.Min(cy + brushRadiusExtents, tileMap.height + 1);
		int x0 = Mathf.Max(cx - brushRadiusExtents, 0);
		int x1 = Mathf.Min(cx + brushRadiusExtents, tileMap.width + 1);
		for (int y = y0; y < y1; ++y)
		{
			for (int x = x0; x < x1; ++x)
			{
				Color color = editorData.brushColor;
				float blendAmount = 1.0f - Mathf.Clamp01(Mathf.Sqrt((y - cy) * (y - cy) + (x - cx) * (x - cx)) / editorData.brushRadius);
				blendAmount *= editorData.blendStrength;
				
				var colors = tileMap.ColorChannel;
				Color c = colors.GetColor(x, y);
				
				switch (editorData.blendMode)
				{
				case tk2dTileMapEditorData.BlendMode.Blend:
					colors.SetColor(x, y, Color.Lerp(c, color, blendAmount));
					break;
				case tk2dTileMapEditorData.BlendMode.Add:
					colors.SetColor(x, y, c + color * blendAmount);
					break;
				}
			}
		}
	}
	
	void PickUpColor()
	{
		editorData.brushColor = tileMap.ColorChannel.GetColor(vertexCursorX, vertexCursorY);
	}
}
