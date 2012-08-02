
public class BaseData
{
	public string[] name;
	
	public string skinPath = "HUD/";
	
	protected string dir = "ProjectSettings/";
	
	protected static string NAME = "name";
	
	public BaseData()
	{
		
	}
	
	public int GetDataCount()
	{
		int val = 0;
		if(name != null)
		{
			val = name.Length;
		}
		return val;
	}
	
	public string GetName(int index)
	{
		return name[index];
	}
	
	public string[] GetNameList(bool showIDs)
	{
		string[] result = new string[0];
		if(name != null)
		{
			result = new string[name.Length];
			for(int i=0; i<name.Length; i++)
			{
				if(showIDs)
				{
					result[i] = i.ToString() + ": " + name[i];
				}
				else
				{
					result[i] = name[i];
				}
			}
		}
		return result;
	}
	
	public virtual void RemoveData(int index)
	{
		
	}
	
	public virtual void Copy(int index)
	{
		
	}
	
	public int CheckForIndex(int index, int check)
	{
		if(check == index) check = 0;
		else if(check > index) check -= 1;
		return check;
	}
}