using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
	using UnityEditor;
#endif

[System.Serializable]
public class tk2dSpriteCollectionIndex
{
	public string name;
	public string spriteCollectionGUID;
	public string spriteCollectionDataGUID;
	public string[] spriteNames;
	public string[] spriteTextureGUIDs;
	public int version;
}

public class tk2dIndex : ScriptableObject
{
	[HideInInspector] public int version = 0;
	[SerializeField] List<tk2dSpriteAnimation> spriteAnimations = new List<tk2dSpriteAnimation>();
	[SerializeField] List<tk2dFont> fonts = new List<tk2dFont>();
	[SerializeField] List<tk2dSpriteCollectionIndex> spriteCollectionIndex = new List<tk2dSpriteCollectionIndex>();
	
	public tk2dSpriteCollectionIndex[] GetSpriteCollectionIndex()
	{
#if UNITY_EDITOR
		int i = 0;
		string assetsPath = Application.dataPath.Substring(0, Application.dataPath.Length - 6);
		foreach (var v in spriteCollectionIndex)
		{
			if (v != null)
			{
				string thisAssetPath = AssetDatabase.GUIDToAssetPath(v.spriteCollectionDataGUID);
				string p = assetsPath + thisAssetPath;
				if (thisAssetPath != null && !System.IO.File.Exists(p))
				{
					spriteCollectionIndex[i] = null;
				}
			}
			++i;
		}
#endif
		spriteCollectionIndex.RemoveAll(item => item == null);
		return spriteCollectionIndex.ToArray();
	}
	
	public void AddSpriteCollectionData(tk2dSpriteCollectionData sc)
	{
#if UNITY_EDITOR
		// prune list
		GetSpriteCollectionIndex(); 
		spriteCollectionIndex.RemoveAll(item => item == null);
		string guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(sc));
		
		bool existing = false;
		tk2dSpriteCollectionIndex indexEntry = null;
		foreach (var v in spriteCollectionIndex) 
		{
			if (v.spriteCollectionDataGUID == guid)
			{
				indexEntry = v;
				existing = true;
				break;
			}
		}
		if (indexEntry == null)
			indexEntry = new tk2dSpriteCollectionIndex();
			
		indexEntry.name = sc.spriteCollectionName;
		indexEntry.spriteCollectionDataGUID = guid;
		indexEntry.spriteCollectionGUID = sc.spriteCollectionGUID;
		indexEntry.spriteNames = new string[sc.spriteDefinitions.Length];
		indexEntry.spriteTextureGUIDs = new string[sc.spriteDefinitions.Length];
		indexEntry.version = sc.version;
		for (int i = 0; i < sc.spriteDefinitions.Length; ++i)
		{
			var s = sc.spriteDefinitions[i];
			if (s != null)
			{
				indexEntry.spriteNames[i] = sc.spriteDefinitions[i].name;
				indexEntry.spriteTextureGUIDs[i] = sc.spriteDefinitions[i].sourceTextureGUID;
			}
			else
			{
				indexEntry.spriteNames[i] = "";
				indexEntry.spriteTextureGUIDs[i] = "";
			}
		}
		
		if (!existing)
			spriteCollectionIndex.Add(indexEntry);
#endif
	}

	public tk2dSpriteAnimation[] GetSpriteAnimations()
	{
		spriteAnimations.RemoveAll(item => item == null);
		return spriteAnimations.ToArray();
	}
	
	public void AddSpriteAnimation(tk2dSpriteAnimation sc)
	{
		spriteAnimations.RemoveAll(item => item == null);
		foreach (var v in spriteAnimations) if (v == sc) return;
		spriteAnimations.Add(sc);
	}

	public tk2dFont[] GetFonts()
	{
		fonts.RemoveAll(item => item == null);
		return fonts.ToArray();
	}
	
	public void AddFont(tk2dFont sc)
	{
		fonts.RemoveAll(item => item == null);
		foreach (var v in fonts) if (v == sc) return;
		fonts.Add(sc);
	}
}

