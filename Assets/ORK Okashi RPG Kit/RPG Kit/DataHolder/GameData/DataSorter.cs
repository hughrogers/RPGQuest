
using System.Collections;

public class TurnSorter : IComparer
{
	public int Compare(System.Object x , System.Object y)
	{
        int sort = 0;
		
		Hashtable ht1 = (Hashtable)x;
        Hashtable ht2 = (Hashtable)y;
		
		float f1 =float.Parse((string)ht1["value"]);
		float f2=float.Parse((string)ht2["value"]);
		
		if(f1 == f2) sort = 0;
		else if(f1 < f2) sort = 1;
		else if(f1 > f2) sort = -1;
		
		return sort;
    } 
}

// skill sorter
public class SkillTypeSorter : IComparer
{
	public int Compare(System.Object x , System.Object y)
	{
		int i1 = (int)x;
		int i2 = (int)y;
		string n1 = DataHolder.SkillType(i1);
		string n2 = DataHolder.SkillType(i2);
		return n1.CompareTo(n2);
	}
}

public class SkillNameSorter : IComparer
{
	public int Compare(System.Object x , System.Object y)
	{
		int i1 = (int)x;
		int i2 = (int)y;
		string n1 = DataHolder.Skills().GetName(i1);
		string n2 = DataHolder.Skills().GetName(i2);
		return n1.CompareTo(n2);
	}
}

// item sorter
public class ItemTypeSorter : IComparer
{
	public int Compare(System.Object x , System.Object y)
	{
		int i1 = (int)x;
		int i2 = (int)y;
		string n1 = DataHolder.ItemType(i1);
		string n2 = DataHolder.ItemType(i2);
		return n1.CompareTo(n2);
	}
}

public class ItemNameSorter : IComparer
{
	public int Compare(System.Object x , System.Object y)
	{
		int i1 = (int)x;
		int i2 = (int)y;
		string n1 = DataHolder.Items().GetName(i1);
		string n2 = DataHolder.Items().GetName(i2);
		return n1.CompareTo(n2);
	}
}