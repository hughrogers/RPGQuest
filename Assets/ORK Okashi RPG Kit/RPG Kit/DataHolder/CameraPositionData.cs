
using System.Collections;

public class CameraPositionData : BaseData
{
	public CameraPosition[] cam = new CameraPosition[0];
	
	// XML data
	private string filename = "cameraPositions";
	
	private static string CAMERAPOSITIONS = "camerapositions";
	private static string CAMERAPOSITION = "cameraposition";
	private static string POSITION = "position";
	private static string ROTATION = "rotation";

	public CameraPositionData()
	{
		LoadData();
	}
	
	public void LoadData()
	{
		ArrayList data = XMLHandler.LoadXML(dir+filename);
		
		if(data.Count > 0)
		{
			foreach(Hashtable entry in data)
			{
				if(entry[XMLHandler.NODE_NAME] as string == CameraPositionData.CAMERAPOSITIONS)
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList subs = entry[XMLHandler.NODES] as ArrayList;
						name = new string[subs.Count];
						cam = new CameraPosition[subs.Count];
						foreach(Hashtable val in subs)
						{
							if(val[XMLHandler.NODE_NAME] as string == CameraPositionData.CAMERAPOSITION)
							{
								int i = int.Parse((string)val["id"]);
								cam[i] = new CameraPosition();
								if(val.ContainsKey("lookat"))
								{
									cam[i].lookAt = true;
								}
								if(val.ContainsKey("local"))
								{
									cam[i].localPoint = true;
								}
								if(val.ContainsKey("child"))
								{
									cam[i].targetChild = true;
									cam[i].childName = val["child"] as string;
								}
								if(val.ContainsKey("ignorexrotation")) cam[i].ignoreXRotation = true;
								if(val.ContainsKey("ignoreyrotation")) cam[i].ignoreYRotation = true;
								if(val.ContainsKey("ignorezrotation")) cam[i].ignoreZRotation = true;
								if(val.ContainsKey("fov"))
								{
									cam[i].setFoV = true;
									cam[i].fieldOfView = float.Parse((string)val["fov"]);
								}
								
								ArrayList s = val[XMLHandler.NODES] as ArrayList;
								foreach(Hashtable ht in s)
								{
									if(ht[XMLHandler.NODE_NAME] as string == CameraPositionData.NAME)
									{
										name[i] = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == CameraPositionData.POSITION)
									{
										cam[i].position.x = float.Parse((string)ht["x"]);
										cam[i].position.y = float.Parse((string)ht["y"]);
										cam[i].position.z = float.Parse((string)ht["z"]);
									}
									else if(ht[XMLHandler.NODE_NAME] as string == CameraPositionData.ROTATION)
									{
										cam[i].rotation.x = float.Parse((string)ht["x"]);
										cam[i].rotation.y = float.Parse((string)ht["y"]);
										cam[i].rotation.z = float.Parse((string)ht["z"]);
									}
								}
							}
						}
					}
				}
			}
		}
	}
	
	public void SaveData()
	{
		ArrayList data = new ArrayList();
		ArrayList subs = new ArrayList();
		Hashtable sv = new Hashtable();
		
		sv.Add(XMLHandler.NODE_NAME, CameraPositionData.CAMERAPOSITIONS);
		
		if(name != null)
		{
			for(int i=0; i<name.Length; i++)
			{
				Hashtable ht = new Hashtable();
				ArrayList s = new ArrayList();
				
				ht.Add(XMLHandler.NODE_NAME, CameraPositionData.CAMERAPOSITION);
				ht.Add("id", i.ToString());
				if(cam[i].localPoint) ht.Add("local", "true");
				if(cam[i].lookAt) ht.Add("lookat", "true");
				if(cam[i].ignoreXRotation) ht.Add("ignorexrotation", "true");
				if(cam[i].ignoreYRotation) ht.Add("ignoreyrotation", "true");
				if(cam[i].ignoreZRotation) ht.Add("ignorezrotation", "true");
				
				Hashtable n = new Hashtable();
				n.Add(XMLHandler.NODE_NAME, CameraPositionData.ROTATION);
				n.Add("x", cam[i].rotation.x.ToString());
				n.Add("y", cam[i].rotation.y.ToString());
				n.Add("z", cam[i].rotation.z.ToString());
				s.Add(n);
				
				if(cam[i].targetChild)
				{
					ht.Add("child", cam[i].childName);
				}
				if(cam[i].setFoV)
				{
					ht.Add("fov", cam[i].fieldOfView.ToString());
				}
				
				n = new Hashtable();
				n.Add(XMLHandler.NODE_NAME, CameraPositionData.NAME);
				n.Add(XMLHandler.CONTENT, name[i]);
				s.Add(n);
				
				n = new Hashtable();
				n.Add(XMLHandler.NODE_NAME, CameraPositionData.POSITION);
				n.Add("x", cam[i].position.x.ToString());
				n.Add("y", cam[i].position.y.ToString());
				n.Add("z", cam[i].position.z.ToString());
				s.Add(n);
				
				ht.Add(XMLHandler.NODES, s);
				subs.Add(ht);
			}
			sv.Add(XMLHandler.NODES, subs);
		}
		
		data.Add(sv);
		XMLHandler.SaveXML(dir, filename, data);
	}
	
	public void AddCamPos(string n)
	{
		if(name == null)
		{
			name = new string[] {n};
			cam = new CameraPosition[] {new CameraPosition()};
		}
		else
		{
			name = ArrayHelper.Add(n, name);
			cam = ArrayHelper.Add(new CameraPosition(), cam);
		}
	}
	
	public override void RemoveData(int index)
	{
		name = ArrayHelper.Remove(index, name);
		if (name.Length == 0) name = null;
		
		cam = ArrayHelper.Remove(index, cam);
	}
	
	public override void Copy(int index)
	{
		this.AddCamPos(name[index]);
		cam[cam.Length-1].position = cam[index].position;
		cam[cam.Length-1].rotation = cam[index].rotation;
		cam[cam.Length-1].lookAt = cam[index].lookAt;
		cam[cam.Length-1].localPoint = cam[index].localPoint;
		cam[cam.Length-1].targetChild = cam[index].targetChild;
		cam[cam.Length-1].childName = cam[index].childName;
		cam[cam.Length-1].ignoreXRotation = cam[index].ignoreXRotation;
		cam[cam.Length-1].ignoreYRotation = cam[index].ignoreYRotation;
		cam[cam.Length-1].ignoreZRotation = cam[index].ignoreZRotation;
		cam[cam.Length-1].setFoV = cam[index].setFoV;
		cam[cam.Length-1].fieldOfView = cam[index].fieldOfView;
	}
}