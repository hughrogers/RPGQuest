
using UnityEngine;
using System.Collections;

public class EffectPrefab
{
	public string prefabName = "";
	public GameObject prefabInstance;
	public string childName = "";
	public bool targetRotation = false;
	public bool localSpace = false;
	public Vector3 offset = Vector3.zero;
	public Vector3 rotationOffset = Vector3.zero;
	
	public string prefabPath = "Prefabs/StatusEffects/";
	
	private static string NAME = "name";
	private static string CHILD = "child";
	
	public EffectPrefab()
	{
		
	}
	
	public EffectPrefab(Hashtable ht)
	{
		this.SetData(ht);
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		VectorHelper.ToHashtable(ref ht, this.offset);
		VectorHelper.ToHashtable(ref ht, this.rotationOffset, "rx", "ry", "rz");
		if(this.localSpace) ht.Add("localspace", "true");
		if(this.targetRotation) ht.Add("targetrotation", "true");
		
		ArrayList s = new ArrayList();
		s.Add(HashtableHelper.GetContentHashtable(EffectPrefab.NAME, this.prefabName));
		if(this.childName != "") s.Add(HashtableHelper.GetContentHashtable(EffectPrefab.CHILD, this.childName));
		ht.Add(XMLHandler.NODES, s);
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		if(ht.ContainsKey("localspace")) this.localSpace = true;
		if(ht.ContainsKey("targetrotation")) this.targetRotation = true;
		this.offset = VectorHelper.FromHashtable(ht);
		if(ht.ContainsKey("rx"))
		{
			this.rotationOffset = VectorHelper.FromHashtable(ht, "rx", "ry", "rz");
		}
		
		ArrayList s = (ArrayList)ht[XMLHandler.NODES];
		foreach(Hashtable ht2 in s)
		{
			if(ht2[XMLHandler.NODE_NAME] as string == EffectPrefab.NAME)
			{
				this.prefabName = (string)ht2[XMLHandler.CONTENT];
			}
			else if(ht2[XMLHandler.NODE_NAME] as string == EffectPrefab.CHILD)
			{
				this.childName = (string)ht2[XMLHandler.CONTENT];
			}
		}
	}
	
	/*
	============================================================================
	Ingame functions
	============================================================================
	*/
	public void Create(Combatant c)
	{
		if(this.prefabInstance == null && "" != this.prefabName &&
			c.prefabInstance != null)
		{
			GameObject tmp = (GameObject)Resources.Load(this.prefabPath+this.prefabName, typeof(GameObject));
			if(tmp)
			{
				this.prefabInstance = (GameObject)GameObject.Instantiate(tmp);
			}
			TransformHelper.Mount(TransformHelper.GetChild(this.childName, c.prefabInstance.transform), this.prefabInstance.transform, 
					true, this.localSpace, this.offset, this.targetRotation, this.rotationOffset);
		}
		
	}
	
	public void Destroy()
	{
		if(this.prefabInstance) GameObject.Destroy(this.prefabInstance);
	}
}
