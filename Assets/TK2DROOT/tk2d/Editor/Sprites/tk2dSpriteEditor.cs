using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(tk2dSprite))]
class tk2dSpriteEditor : Editor
{
    public override void OnInspectorGUI()
    {
        tk2dBaseSprite sprite = (tk2dBaseSprite)target;
		DrawSpriteEditorGUI(sprite);
    }
	
	void OnDestroy()
	{
		tk2dSpriteThumbnailCache.ReleaseSpriteThumbnailCache();
	}
	

	protected void DrawSpriteEditorGUI(tk2dBaseSprite sprite)
	{
		var newCollection = tk2dSpriteGuiUtility.SpriteCollectionPopup("Collection", sprite.collection, true, sprite.spriteId);
		if (sprite.collection != newCollection)
		{
			if (sprite.collection == null)
				sprite.collection = newCollection;
			
			int spriteId = sprite.spriteId;
			if (sprite.spriteId < 0 || sprite.spriteId >= sprite.collection.Count 
				|| !sprite.collection.spriteDefinitions[sprite.spriteId].Valid)
				spriteId = sprite.collection.FirstValidDefinitionIndex;
			sprite.SwitchCollectionAndSprite(newCollection, spriteId);
			sprite.ForceBuild();
		}
		
        if (sprite.collection)
        {
            int newSpriteId = sprite.spriteId;

			// sanity check sprite id
			if (sprite.spriteId < 0 || sprite.spriteId >= sprite.collection.Count 
				|| !sprite.collection.spriteDefinitions[sprite.spriteId].Valid)
			{
				newSpriteId = sprite.collection.FirstValidDefinitionIndex;
			}
			
			newSpriteId = tk2dSpriteGuiUtility.SpriteSelectorPopup("Sprite", sprite.spriteId, sprite.collection);
			if (tk2dPreferences.inst.displayTextureThumbs)
			{
				if (sprite.collection.version < 1 || sprite.collection.dataGuid == tk2dSpriteGuiUtility.TransientGUID)
				{
					string message = "";
					
					message = "No thumbnail data.";
					if (sprite.collection.version < 1 && sprite.collection.dataGuid != tk2dSpriteGuiUtility.TransientGUID)
						message += "\nPlease rebuild Sprite Collection.";
					
					tk2dGuiUtility.InfoBox(message, tk2dGuiUtility.WarningLevel.Info);
				}
				else
				{
					var tex = tk2dSpriteThumbnailCache.GetThumbnailTexture(sprite.collection, sprite.spriteId);
					if (tex) 
					{
						float w = tex.width;
						float h = tex.height;
						float maxSize = 128.0f;
						if (w > maxSize)
						{
							h = h / w * maxSize;
							w = maxSize;
						}
						
						Rect r = GUILayoutUtility.GetRect(w, h);
						GUI.DrawTexture(r, tex, ScaleMode.ScaleToFit);
					}
				}
			}

			if (newSpriteId != sprite.spriteId)
			{
				sprite.spriteId = newSpriteId;
				sprite.EditMode__CreateCollider();
				GUI.changed = true;
			}

            sprite.color = EditorGUILayout.ColorField("Color", sprite.color);
			Vector3 newScale = EditorGUILayout.Vector3Field("Scale", sprite.scale);
			if (newScale != sprite.scale)
			{
				sprite.scale = newScale;
				sprite.EditMode__CreateCollider();
			}
			
			EditorGUILayout.BeginHorizontal();
			
			if (GUILayout.Button("HFlip"))
			{
				Vector3 s = sprite.scale;
				s.x *= -1.0f;
				sprite.scale = s;
				GUI.changed = true;
			}
			if (GUILayout.Button("VFlip"))
			{
				Vector3 s = sprite.scale;
				s.y *= -1.0f;
				sprite.scale = s;
				GUI.changed = true;
			}
			
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			
			if (GUILayout.Button(new GUIContent("Reset Scale", "Set scale to 1")))
			{
				Vector3 s = sprite.scale;
				s.x = Mathf.Sign(s.x);
				s.y = Mathf.Sign(s.y);
				s.z = Mathf.Sign(s.z);
				sprite.scale = s;
				GUI.changed = true;
			}
			
			if (GUILayout.Button(new GUIContent("Bake Scale", "Transfer scale from transform.scale -> sprite")))
			{
				tk2dScaleUtility.Bake(sprite.transform);
				GUI.changed = true;
			}
			
			GUIContent pixelPerfectButton = new GUIContent("1:1", "Make Pixel Perfect");
			if ( GUILayout.Button(pixelPerfectButton ))
			{
				if (tk2dPixelPerfectHelper.inst) tk2dPixelPerfectHelper.inst.Setup();
				sprite.MakePixelPerfect();
				GUI.changed = true;
			}
			
			sprite.pixelPerfect = GUILayout.Toggle(sprite.pixelPerfect, new GUIContent("Always", "Always keep pixel perfect"), GUILayout.Width(60.0f));
			EditorGUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.IntSlider("Need a collection bound", 0, 0, 1);
        }
		
		bool needUpdatePrefabs = false;
		if (GUI.changed)
		{
			EditorUtility.SetDirty(sprite);
#if !(UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4)
			if (PrefabUtility.GetPrefabType(sprite) == PrefabType.Prefab)
				needUpdatePrefabs = true;
#endif
		}
		
		// This is a prefab, and changes need to be propagated. This isn't supported in Unity 3.4
		if (needUpdatePrefabs)
		{
#if !(UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4)
			// Rebuild prefab instances
			tk2dBaseSprite[] allSprites = Resources.FindObjectsOfTypeAll(sprite.GetType()) as tk2dBaseSprite[];
			foreach (var spr in allSprites)
			{
				if (PrefabUtility.GetPrefabType(spr) == PrefabType.PrefabInstance &&
					PrefabUtility.GetPrefabParent(spr.gameObject) == sprite.gameObject)
				{
					// Reset all prefab states
					var propMod = PrefabUtility.GetPropertyModifications(spr);
					PrefabUtility.ResetToPrefabState(spr);
					PrefabUtility.SetPropertyModifications(spr, propMod);
					
					spr.ForceBuild();
				}
			}
#endif
		}
	}
	
    [MenuItem("GameObject/Create Other/tk2d/Sprite", false, 12900)]
    static void DoCreateSpriteObject()
    {
		tk2dSpriteCollectionData sprColl = null;
		if (sprColl == null)
		{
			// try to inherit from other Sprites in scene
			tk2dSprite spr = GameObject.FindObjectOfType(typeof(tk2dSprite)) as tk2dSprite;
			if (spr)
			{
				sprColl = spr.collection;
			}
		}

		if (sprColl == null)
		{
			tk2dSpriteCollectionIndex[] spriteCollections = tk2dEditorUtility.GetOrCreateIndex().GetSpriteCollectionIndex();
			foreach (var v in spriteCollections)
			{
				GameObject scgo = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(v.spriteCollectionDataGUID), typeof(GameObject)) as GameObject;
				var sc = scgo.GetComponent<tk2dSpriteCollectionData>();
				if (sc != null && sc.spriteDefinitions != null && sc.spriteDefinitions.Length > 0)
				{
					sprColl = sc;
					break;
				}
			}

			if (sprColl == null)
			{
				EditorUtility.DisplayDialog("Create Sprite", "Unable to create sprite as no SpriteCollections have been found.", "Ok");
				return;
			}
		}

		GameObject go = tk2dEditorUtility.CreateGameObjectInScene("Sprite");
		tk2dSprite sprite = go.AddComponent<tk2dSprite>();
		sprite.collection = sprColl;
		sprite.renderer.material = sprColl.FirstValidDefinition.material;
		sprite.Build();
		
		Selection.activeGameObject = go;
		Undo.RegisterCreatedObjectUndo(go, "Create Sprite");
    }
}

