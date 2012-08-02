
using UnityEngine;
using System.Collections;

public abstract class ChangeNotifier
{
	public abstract void ChangeHappened(int id, int info, int info2);
}
