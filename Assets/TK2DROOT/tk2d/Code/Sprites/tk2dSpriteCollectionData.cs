using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
/// <summary>
/// Sprite Definition.
/// </summary>
public class tk2dSpriteDefinition
{
	/// <summary>
	/// Collider type.
	/// </summary>
	public enum ColliderType
	{
		/// <summary>
		/// Do not create or destroy anything.
		/// </summary>
		Unset,
		
		/// <summary>
		/// If a collider exists, it will be destroyed. The sprite will be responsible in making sure there are no other colliders attached.
		/// </summary>
		None,
		
		/// <summary>
		/// Create a box collider.
		/// </summary>
		Box,
		
		/// <summary>
		/// Create a mesh collider.
		/// </summary>
		Mesh,
	}
	
	/// <summary>
	/// Name
	/// </summary>
	public string name;
	
	public Vector3[] boundsData;
	public Vector3[] untrimmedBoundsData;
	
	public Vector2 texelSize;
	
	/// <summary>
	/// Array of positions for sprite geometry.
	/// </summary>
    public Vector3[] positions;
	
	/// <summary>
	/// Array of normals for sprite geometry, zero length array if they dont exist
	/// </summary>
	public Vector3[] normals;
	
	/// <summary>
	/// Array of tangents for sprite geometry, zero length array if they dont exist
	/// </summary>
	public Vector4[] tangents;
	
	/// <summary>
	/// Array of UVs for sprite geometry, will match the position count.
	/// </summary>
    public Vector2[] uvs;
	/// <summary>
	/// Array of indices for sprite geometry.
	/// </summary>
    public int[] indices = new int[] { 0, 3, 1, 2, 3, 0 };
	/// <summary>
	/// The material used by this sprite. This is generally the same on all sprites in a colletion, but this is not
	/// true when multi-atlas spanning is enabled.
	/// </summary>
	public Material material;
	/// <summary>
	/// The material id used by this sprite. This is an index into the materials array and corresponds to the 
	/// material flag above.
	/// </summary>
	public int materialId;
	
	
	/// <summary>
	/// Source texture GUID - this is used by the inspector to find the source image without adding a unity dependency.
	/// </summary>
	public string sourceTextureGUID;
	/// <summary>
	/// Speficies if this texture is extracted from a larger texture source, for instance an atlas. This is used in the inspector.
	/// </summary>
	public bool extractRegion;
	public int regionX, regionY, regionW, regionH;
	
	/// <summary>
	/// Specifies if this texture is flipped to its side (rotated) in the atlas
	/// </summary>
	public bool flipped;
	
	/// <summary>
	/// Specifies if this texture has complex geometry
	/// </summary>
	public bool complexGeometry = false;
	
	/// <summary>
	/// Collider type
	/// </summary>
	public ColliderType colliderType = ColliderType.None;
	
	/// <summary>
	/// v0 and v1 are center and size respectively for box colliders when colliderType is Box.
	/// It is an array of vertices, and the geometry defined by indices when colliderType is Mesh.
	/// </summary>
	public Vector3[] colliderVertices; 
	public int[] colliderIndicesFwd;
	public int[] colliderIndicesBack;
	public bool colliderConvex;
	public bool colliderSmoothSphereCollisions;
	
	public bool Valid { get { return name.Length != 0; } }
}

[AddComponentMenu("2D Toolkit/Backend/tk2dSpriteCollectionData")]
/// <summary>
/// Sprite Collection Data.
/// </summary>
public class tk2dSpriteCollectionData : MonoBehaviour 
{
	public const int CURRENT_VERSION = 3;
	
	[HideInInspector]
	public int version;
	public bool materialIdsValid = false;
	
    [HideInInspector]
	/// <summary>
	/// An array of sprite definitions.
	/// </summary>
    public tk2dSpriteDefinition[] spriteDefinitions;
	
	/// <summary>
	/// Dictionary to look up sprite names. This will be initialized on first call to GetSpriteIdByName.
	/// </summary>
	Dictionary<string, int> spriteNameLookupDict = null;
	
	/// <summary>
	/// Whether premultiplied alpha is enabled on this sprite collection. This affects how tint colors are computed.
	/// </summary>
    [HideInInspector]
    public bool premultipliedAlpha;
	
	/// <summary>
	/// Only exists for backwards compatibility. Do not use or rely on this.
	/// </summary>
    [HideInInspector]
	public Material material;	
	
	/// <summary>
	/// An array of all materials used by this sprite collection.
	/// </summary>
	public Material[] materials;
	
	/// <summary>
	/// An array of all textures used by this sprite collection.
	/// </summary>
	public Texture[] textures;
	
	/// <summary>
	/// Specifies if sprites span multiple atlases.
	/// </summary>
	[HideInInspector]
	public bool allowMultipleAtlases;
	
	/// <summary>
	/// The sprite collection GUI.
	/// </summary>
	[HideInInspector]
	public string spriteCollectionGUID;
	
	[HideInInspector]
	/// <summary>
	/// The name of the sprite collection.
	/// </summary>
	public string spriteCollectionName;
	
	[HideInInspector]
	/// <summary>
	/// The size of the inv ortho size used to generate the sprite collection.
	/// </summary>
	public float invOrthoSize = 1.0f;
	
	[HideInInspector]
	/// <summary>
	/// Target height used to generate the sprite collection.
	/// </summary>
	public float halfTargetHeight = 1.0f;
	
	[HideInInspector]
	public int buildKey = 0;
	
	[HideInInspector]
	/// <summary>
	/// GUID of this object, used with <see cref="tk2dIndex"/>
	/// </summary>
	public string dataGuid = "";

	/// <summary>
	/// Returns the number of sprite definitions in this sprite collection.
	/// </summary>
    public int Count { get { return spriteDefinitions.Length; } }

	/// <summary>
	/// Resolves a sprite name and returns a unique id for the sprite.
	/// </summary>
	/// <returns>
	/// Unique Sprite Id. 0 if sprite isn't found.
	/// </returns>
	/// <param name='name'>Case sensitive sprite name, as defined in the sprite collection. This is usually the source filename excluding the extension</param>
	public int GetSpriteIdByName(string name)
	{
		InitDictionary();
		int returnValue = 0;
		spriteNameLookupDict.TryGetValue(name, out returnValue);
		return returnValue; // default to first sprite
	}
	
	/// <summary>
	/// Initializes the lookup dictionary
	/// </summary>
	public void InitDictionary()
	{
		if (spriteNameLookupDict == null)
		{
			spriteNameLookupDict = new Dictionary<string, int>(spriteDefinitions.Length);
			for (int i = 0; i < spriteDefinitions.Length; ++i)
			{
				spriteNameLookupDict[spriteDefinitions[i].name] = i;
			}
		}
	}
	
	/// <summary>
	/// Returns the first valid sprite definition
	/// </summary>
	public tk2dSpriteDefinition FirstValidDefinition
	{
		get 
		{
			foreach (var v in spriteDefinitions)
			{
				if (v.Valid)
					return v;
			}
			return null;
		}
	}
	
	/// <summary>
	/// Returns the index of the first valid sprite definition
	/// </summary>
	public int FirstValidDefinitionIndex
	{
		get 
		{
			for (int i = 0; i < spriteDefinitions.Length; ++i)
				if (spriteDefinitions[i].Valid)
					return i;
			return -1;
		}
	}
	
	/// <summary>
	/// Internal function to make sure all material Ids are valid. Used in the tilemap editor
	/// </summary>
	public void InitMaterialIds()
	{
		if (materialIdsValid)
			return;
		
		int firstValidIndex = -1;
		Dictionary<Material, int> materialLookupDict = new Dictionary<Material, int>();
		for (int i = 0; i < materials.Length; ++i)
		{
			if (firstValidIndex == -1 && materials[i] != null)
				firstValidIndex = i;
			materialLookupDict[materials[i]] = i;
		}
		if (firstValidIndex == -1)
		{
			Debug.LogError("Init material ids failed.");
		}
		else
		{
			foreach (var v in spriteDefinitions)			
			{
				if (!materialLookupDict.TryGetValue(v.material, out v.materialId))
					v.materialId = firstValidIndex;
			}
			materialIdsValid = true;
		}
	}
}
