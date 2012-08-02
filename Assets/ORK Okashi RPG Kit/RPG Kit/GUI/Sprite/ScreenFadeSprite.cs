
using UnityEngine;
using System.Collections;

public class ScreenFadeSprite : BaseSprite
{
	public ScreenFader screenFader;
	
	void Start()
	{	
		this.InitMesh();
		this.renderer.material.mainTexture = this.screenFader.tex;
		transform.position = new Vector3(0, -0.5f, 0);
		this.UpdateGUISize(false, GameHandler.GUIHandler().GetScreenRatio());
	}
	
	void LateUpdate()
	{
		this.renderer.material.mainTexture = this.screenFader.tex;
		if(this.renderer.material.mainTexture == null)
		{
			GameObject.Destroy(this.gameObject);
		}
	}
	
	public override void UpdateGUISize(bool updateChilds, Vector2 ratio)
	{
		float w = ((Screen.width+10)/2);
		float h = ((Screen.height+10)/2);
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
			colors[i] = Color.white;
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
	
	// unregister sprite
	void OnDestroy()
	{
		GameHandler.GUIHandler().RemoveSprite(this);
	}
}
