
using System.Collections;

public class BattleAnimationData : BaseData
{
	public BattleAnimation[] animation = new BattleAnimation[0];
	
	// XML data
	private string filename  = "battleAnimations";
	
	private static string BATTLEANIMATIONS = "battleanimations";
	private static string BATTLEANIMATION = "battleanimation";
	private static string BATTLEANIMATIONNAME = "battleanimationname";

	public BattleAnimationData()
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
				if(entry[XMLHandler.NODE_NAME] as string == BattleAnimationData.BATTLEANIMATIONS)
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList subs = entry[XMLHandler.NODES] as ArrayList;
						name = new string[subs.Count];
						animation = new BattleAnimation[subs.Count];
						foreach(Hashtable val in subs)
						{
							if(val[XMLHandler.NODE_NAME] as string == BattleAnimationData.BATTLEANIMATION)
							{
								int i = int.Parse((string)val["id"]);
								animation[i] = new BattleAnimation();
								animation[i].LoadEventData(val);
								ArrayList s = val[XMLHandler.NODES] as ArrayList;
								foreach(Hashtable ht in s)
								{
									if(ht[XMLHandler.NODE_NAME] as string == BattleAnimationData.BATTLEANIMATIONNAME)
									{
										name[i] = ht[XMLHandler.CONTENT] as string;
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
		
		sv.Add(XMLHandler.NODE_NAME, BattleAnimationData.BATTLEANIMATIONS);
		
		if(name != null)
		{
			for(int i=0; i<name.Length; i++)
			{
				Hashtable ht = this.animation[i].GetEventData();
				ht.Add(XMLHandler.NODE_NAME, BattleAnimationData.BATTLEANIMATION);
				ht.Add("id", i.ToString());
				
				Hashtable n = new Hashtable();
				n.Add(XMLHandler.NODE_NAME, BattleAnimationData.BATTLEANIMATIONNAME);
				n.Add(XMLHandler.CONTENT, name[i]);
				((ArrayList)ht[XMLHandler.NODES]).Add(n);
				subs.Add(ht);
			}
			sv.Add(XMLHandler.NODES, subs);
		}
		
		data.Add(sv);
		XMLHandler.SaveXML(dir, filename, data);
	}
	
	public void AddBattleAnimation(string n)
	{
		if(name == null)
		{
			name = new string[] {n};
			animation = new BattleAnimation[] {new BattleAnimation()};
		}
		else
		{
			name = ArrayHelper.Add(n, name);
			animation = ArrayHelper.Add(new BattleAnimation(), animation);
		}
	}
	
	public override void RemoveData(int index)
	{
		name = ArrayHelper.Remove(index, name);
		if(name.Length == 0) name = null;
		
		animation = ArrayHelper.Remove(index, animation);
		for(int i=0; i<animation.Length; i++)
		{
			for(int j=0; j<animation[i].step.Length; j++)
			{
				if(animation[i].step[j] is CallAnimationAStep)
				{
					animation[i].step[j].number = this.CheckForIndex(index, animation[i].step[j].number);
				}
			}
		}
	}
	
	public override void Copy(int index)
	{
		name = ArrayHelper.Add(name[index], name);
		animation = ArrayHelper.Add(this.GetCopy(index), animation);
	}
	
	public BattleAnimation GetCopy(int index)
	{
		BattleAnimation ba = new BattleAnimation();
		ba.realID = index;
		
		ba.step = new AnimationStep[animation[index].step.Length];
		for(int i=0; i<ba.step.Length; i++)
		{
			ba.step[i] = animation[index].step[i].GetCopy(ba);
		}
		
		ba.hideButtons = animation[index].hideButtons;
		ba.returnToBaseCamPos = animation[index].returnToBaseCamPos;
		ba.returnLooks = animation[index].returnLooks;
		ba.calculationNeeded = animation[index].calculationNeeded;
		
		ba.autoDestroyPrefabs = animation[index].autoDestroyPrefabs;
		ba.prefabName = new string[animation[index].prefabName.Length];
		System.Array.Copy(animation[index].prefabName, ba.prefabName, ba.prefabName.Length);
		for(int i=0; i<ba.prefabName.Length; i++) ba.prefab = ArrayHelper.Add(null, ba.prefab);
		
		ba.audioName = new string[animation[index].audioName.Length];
		System.Array.Copy(animation[index].audioName, ba.audioName, ba.audioName.Length);
		for(int i=0; i<ba.audioName.Length; i++) ba.audioClip = ArrayHelper.Add(null, ba.audioClip);
		
		return ba;
	}
}