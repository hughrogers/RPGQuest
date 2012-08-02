
using UnityEngine;
using System.Collections;

public class TargetRaycast
{
	public bool active = false;
	
	public float distance = 100.0f;
	public int layerMask = 1;
	public bool ignoreUser = false;
	
	public MouseTouchControl mouseTouch = new MouseTouchControl();
	
	public TargetRayOrigin rayOrigin = TargetRayOrigin.USER;
	
	public string pathToChild = "";
	public Vector3 offset = Vector3.zero;
	
	public Vector3 rayDirection = Vector3.forward;
	
	public string pathToTarget = "";
	public Vector3 targetOffset = Vector3.zero;
	
	private static string CHILD = "child";
	private static string TARGETCHILD = "targetchild";
	
	public TargetRaycast()
	{
		
	}
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public Hashtable GetData(Hashtable ht)
	{
		if(this.active)
		{
			ArrayList s = new ArrayList();
			ht.Add("distance", this.distance.ToString());
			ht.Add("layermask", this.layerMask.ToString());
			if(this.ignoreUser) ht.Add("ignoreuser", "true");
			
			ht = this.mouseTouch.GetData(ht);
			
			ht.Add("rayorigin", this.rayOrigin.ToString());
			VectorHelper.ToHashtable(ref ht, this.offset);
			if(TargetRayOrigin.USER.Equals(this.rayOrigin))
			{
				s.Add(HashtableHelper.GetContentHashtable(TargetRaycast.CHILD, this.pathToChild));
				if(!this.mouseTouch.Active())
				{
					VectorHelper.ToHashtable(ref ht, this.rayDirection, "dx", "dy", "dz");
				}
			}
			s.Add(HashtableHelper.GetContentHashtable(TargetRaycast.TARGETCHILD, this.pathToTarget));
			VectorHelper.ToHashtable(ref ht, this.targetOffset, "tx", "ty", "tz");
			if(s.Count > 0) ht.Add(XMLHandler.NODES, s);
		}
		return ht;
	}
	
	public void SetData(Hashtable ht)
	{
		if(ht.ContainsKey("distance"))
		{
			this.active = true;
			
			this.distance = float.Parse((string)ht["distance"]);
			this.layerMask = int.Parse((string)ht["layermask"]);
			if(ht.ContainsKey("ignoreuser")) this.ignoreUser = true;
			this.mouseTouch.SetData(ht);
			this.rayOrigin = (TargetRayOrigin)System.Enum.Parse(typeof(TargetRayOrigin), (string)ht["rayorigin"]);
			this.offset = VectorHelper.FromHashtable(ht);
			if(ht.ContainsKey("dx"))
			{
				this.rayDirection = VectorHelper.FromHashtable(ht, "dx", "dy", "dz");
			}
			if(ht.ContainsKey("tx"))
			{
				this.targetOffset = VectorHelper.FromHashtable(ht, "tx", "ty", "tz");
			}
			if(ht.ContainsKey(XMLHandler.NODES))
			{
				ArrayList s = ht[XMLHandler.NODES] as ArrayList;
				foreach(Hashtable ht2 in s)
				{
					if(ht2[XMLHandler.NODE_NAME] as string == TargetRaycast.CHILD)
					{
						this.pathToChild = ht2[XMLHandler.CONTENT] as string;
					}
					else if(ht2[XMLHandler.NODE_NAME] as string == TargetRaycast.TARGETCHILD)
					{
						this.pathToTarget = ht2[XMLHandler.CONTENT] as string;
					}
				}
			}
		}
	}
	
	/*
	============================================================================
	Ingame functions
	============================================================================
	*/
	public bool NeedInteraction()
	{
		return this.active && this.mouseTouch.Active();
	}
	
	public void Tick(ActiveBattleMenu menu, GameObject user)
	{
		Vector3 point = Vector3.zero;
		if(this.mouseTouch.Interacted(ref point))
		{
			if(TargetRayOrigin.USER.Equals(this.rayOrigin))
			{
				point = this.ScreenRayPoint(point);
			}
			menu.SetRayPoint(this.GetRayPoint(user, point));
		}
	}
	
	public Vector3 ScreenRayPoint(Vector3 point)
	{
		Ray ray = Camera.main.ScreenPointToRay(point);
		RaycastHit hit;
		LayerMask lm = 1 << this.layerMask;
		if(Physics.Raycast(ray, out hit, this.distance, lm))
		{
			point = hit.point;
		}
		return point;
	}
	
	public Vector3 GetRayPoint(GameObject user, Vector3 target)
	{
		Vector3 point = Vector3.zero;
		if(this.active && user != null)
		{
			Ray ray;
			if(TargetRayOrigin.USER.Equals(this.rayOrigin))
			{
				if(this.pathToChild != "")
				{
					Transform tr = user.transform.Find(this.pathToChild);
					if(tr != null) user = tr.gameObject;
				}
				Vector3 origin = user.transform.position+offset;
				Vector3 dir = Vector3.zero;
				if(this.mouseTouch.Active()) dir = VectorHelper.GetDirection(origin, target);
				else dir = user.transform.TransformDirection(this.rayDirection);
				ray = new Ray(origin, dir);
			}
			else
			{
				Camera cam = Camera.main;
				ray = cam.ScreenPointToRay(target+this.offset);
			}
			LayerMask lm = 1 << this.layerMask;
			RaycastHit[] hit = Physics.RaycastAll(ray, this.distance, lm);
			if(hit.Length > 0)
			{
				for(int i=0; i<hit.Length; i++)
				{
					if(!this.ignoreUser || user.transform.root != hit[i].transform.root)
					{
						point = hit[i].point;
						break;
					}
				}
			}
			else
			{
				point = ray.GetPoint(this.distance);
			}
		}
		return point;
	}
	
	public Vector3 GetAIPoint(GameObject user, GameObject target)
	{
		if(this.pathToTarget != "" && target != null)
		{
			Transform t = target.transform.Find(this.pathToChild);
			if(t != null) target = t.gameObject;
		}
		Vector3 pos = target.transform.position+this.targetOffset;
		if(TargetRayOrigin.SCREEN.Equals(this.rayOrigin) && Camera.main != null)
		{
			pos = Camera.main.WorldToScreenPoint(pos);
		}
		return this.GetRayPoint(user, pos);
	}
}
