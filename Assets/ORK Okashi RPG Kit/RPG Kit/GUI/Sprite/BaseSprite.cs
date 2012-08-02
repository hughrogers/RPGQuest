
using UnityEngine;
using System.Collections;

public class BaseSprite : MonoBehaviour
{
	public int spriteID = -1;
	
	public Material material;
	protected MeshFilter meshFilter;
	protected MeshRenderer meshRenderer;
	protected Mesh mesh;
	protected Color startColor = Color.white;
	
	void Awake()
	{
		DontDestroyOnLoad(transform);
		GameObject.Destroy(collider);
		gameObject.layer = 31;
	}
	
	void Start()
	{	
		this.InitMesh();
		this.UpdateGUISize(false, GameHandler.GUIHandler().GetScreenRatio());
	}
	
	public void InitMesh()
	{
		transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
		
		meshFilter = gameObject.GetComponent<MeshFilter>();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
		meshRenderer.castShadows = false;
		meshRenderer.receiveShadows = false;

        meshRenderer.renderer.material = this.material;
        mesh = meshFilter.mesh;
	}
	
	public virtual void UpdateGUISize(bool updateChilds, Vector2 ratio)
	{
		
	}
	
	public virtual void UpdateTextures()
	{
		
	}
	
	public void UpdateBaseMesh(Vector2 ratio)
	{
		if(this.renderer.material.mainTexture == null) return;
		float w = (this.renderer.material.mainTexture.width/2)*ratio.x;
		float h = (this.renderer.material.mainTexture.height/2)*ratio.y;
		
		Vector3[] vertices = new Vector3[4];
		Color[] colors = new Color[4];
		Vector2[] uvs = new Vector2[4];
		int[] triangles = new int[6];
		
		vertices[0] = new Vector3(-w, 0, -h);
		vertices[1] = new Vector3(-w, 0, h);
		vertices[2] = new Vector3(w, 0, -h);
		vertices[3] = new Vector3(w, 0, h);
		
		uvs[0] = new Vector2(0.0f, 1.0f);
		uvs[1] = new Vector2(0.0f, 0.0f);
		uvs[2] = new Vector2(1.0f, 1.0f);
		uvs[3] = new Vector2(1.0f, 0.0f);
		
		for(int i=0; i<4; i++)
		{
			colors[i] = this.startColor;
		}
		
		triangles[0] = 0;
		triangles[1] = 2;
		triangles[2] = 1;
		triangles[3] = 2;
		triangles[4] = 3;
		triangles[5] = 1;

		if(mesh != null)
		{
			mesh.Clear();
			mesh.vertices = vertices;
			mesh.colors = colors;
			mesh.uv = uvs;
			mesh.triangles = triangles;
		}
	}
	
	public void RemoveSprite()
	{
		GameHandler.GUIHandler().RemoveSprite(this);
		GameObject.Destroy(this.gameObject);
	}
	
	/*
	============================================================================
	Interact functions
	============================================================================
	*/
	public virtual bool IsClicked(Vector2 mousePosition, Vector2 ratio)
	{
		return false;
	}
	
	public virtual void ReleaseClick(Vector2 mousePosition)
	{
		
	}
	
	public virtual void Drag()
	{
		
	}
}
