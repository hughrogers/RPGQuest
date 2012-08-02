
using System.Collections;
using UnityEngine;

public class SendMessageAStep : AnimationStep
{
	public SendMessageAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		GameObject[] list = battleAnimation.GetAnimationObjects(this.animationObject, this.prefabID2);
		if(list != null && list.Length > 0)
		{
			for(int i=0; i<list.Length; i++)
			{
				if(list[i] != null)
				{
					if(this.pathToChild != "")
					{
						Transform t = list[i].transform.Find(this.pathToChild);
						if(t != null) list[i] = t.gameObject;
					}
					list[i].SendMessage(this.key, this.value, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
		battleAnimation.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("animationobject", this.animationObject.ToString());
		ht.Add("prefab2", this.prefabID2);
		
		ArrayList subs = new ArrayList();
		Hashtable s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "value");
		s.Add(XMLHandler.CONTENT, this.value);
		subs.Add(s);
		
		s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "key");
		s.Add(XMLHandler.CONTENT, this.key);
		subs.Add(s);
		
		subs.Add(HashtableHelper.GetContentHashtable("pathtochild", this.pathToChild));
		
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}

public class BroadcastMessageAStep : AnimationStep
{
	public BroadcastMessageAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		GameObject[] list = battleAnimation.GetAnimationObjects(this.animationObject, this.prefabID2);
		if(list != null && list.Length > 0)
		{
			for(int i=0; i<list.Length; i++)
			{
				if(list[i] != null)
				{
					list[i].BroadcastMessage(this.key, this.value, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
		battleAnimation.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("animationobject", this.animationObject.ToString());
		ht.Add("prefab2", this.prefabID2);
		
		ArrayList subs = new ArrayList();
		Hashtable s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "value");
		s.Add(XMLHandler.CONTENT, this.value);
		subs.Add(s);
		
		s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "key");
		s.Add(XMLHandler.CONTENT, this.key);
		subs.Add(s);
		
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}

public class AddComponentAStep : AnimationStep
{
	public AddComponentAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		GameObject[] list = battleAnimation.GetAnimationObjects(this.animationObject, this.prefabID2);
		if(list != null && list.Length > 0)
		{
			for(int i=0; i<list.Length; i++)
			{
				if(list[i] != null)
				{
					if(this.pathToChild != "")
					{
						Transform t = list[i].transform.Find(this.pathToChild);
						if(t != null) list[i] = t.gameObject;
					}
					list[i].AddComponent(this.key);
				}
			}
		}
		battleAnimation.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("animationobject", this.animationObject.ToString());
		ht.Add("prefab2", this.prefabID2);
		
		ArrayList subs = new ArrayList();
		Hashtable s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "key");
		s.Add(XMLHandler.CONTENT, this.key);
		subs.Add(s);
		
		subs.Add(HashtableHelper.GetContentHashtable("pathtochild", this.pathToChild));
		
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}

public class RemoveComponentAStep : AnimationStep
{
	public RemoveComponentAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		GameObject[] list = battleAnimation.GetAnimationObjects(this.animationObject, this.prefabID2);
		if(list != null && list.Length > 0)
		{
			for(int i=0; i<list.Length; i++)
			{
				if(list[i] != null)
				{
					if(this.pathToChild != "")
					{
						Transform t = list[i].transform.Find(this.pathToChild);
						if(t != null) list[i] = t.gameObject;
					}
					Component comp = list[i].GetComponent(this.key);
					if(comp != null)
					{
						GameObject.Destroy(comp);
					}
				}
			}
		}
		battleAnimation.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("animationobject", this.animationObject.ToString());
		ht.Add("prefab2", this.prefabID2);
		
		ArrayList subs = new ArrayList();
		Hashtable s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "key");
		s.Add(XMLHandler.CONTENT, this.key);
		subs.Add(s);
		
		subs.Add(HashtableHelper.GetContentHashtable("pathtochild", this.pathToChild));
		
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}
