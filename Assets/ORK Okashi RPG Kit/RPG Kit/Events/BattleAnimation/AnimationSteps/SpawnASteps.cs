
using System.Collections;
using UnityEngine;

public class SpawnPrefabAStep : AnimationStep
{
	public SpawnPrefabAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		Vector3 spawnPosition = Vector3.zero;
		Transform mountTo = null;
		
		GameObject actor = battleAnimation.GetAnimationObject(this.animationObject, this.prefabID);
		if(actor != null && battleAnimation.prefab[this.prefabID] != null)
		{
			if(this.pathToChild != "")
			{
				Transform t = actor.transform.Find(this.pathToChild);
				if(t != null) actor = t.gameObject;
			}
			
			spawnPosition = actor.transform.position;
			Quaternion spawnRotation = Quaternion.identity;
			if(this.show3) spawnRotation = actor.transform.rotation;
			mountTo = actor.transform;
			
			if(battleAnimation.spawnedPrefabs.ContainsKey(this.number))
			{
				GameObject.Destroy((GameObject)battleAnimation.spawnedPrefabs[this.number]);
			}
			battleAnimation.spawnedPrefabs[this.number] = (GameObject)GameObject.Instantiate(
					battleAnimation.prefab[this.prefabID], spawnPosition+this.v3, spawnRotation);
			if(this.show2 && battleAnimation.spawnedPrefabs[this.number] != null && mountTo != null)
			{
				((GameObject)battleAnimation.spawnedPrefabs[this.number]).transform.parent = mountTo;
			}
		}
		battleAnimation.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("prefab", this.prefabID.ToString());
		ht.Add("show2", this.show2.ToString());
		ht.Add("show3", this.show3.ToString());
		ht.Add("number", this.number.ToString());
		
		ht.Add("animationobject", this.animationObject.ToString());
		ht.Add("prefab2", this.prefabID2);
		
		ArrayList subs = new ArrayList();
		Hashtable s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "vector3");
		s.Add("x", this.v3.x);
		s.Add("y", this.v3.y);
		s.Add("z", this.v3.z);
		subs.Add(s);
		
		subs.Add(HashtableHelper.GetContentHashtable("pathtochild", this.pathToChild));
		
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}

public class DestroyPrefabAStep : AnimationStep
{
	public DestroyPrefabAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		if(battleAnimation.spawnedPrefabs[this.number] != null)
		{
			GameObject.Destroy((GameObject)battleAnimation.spawnedPrefabs[this.number]);
		}
		battleAnimation.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("number", this.number.ToString());
		return ht;
	}
}