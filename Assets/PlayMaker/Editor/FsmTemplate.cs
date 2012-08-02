// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using System;
using HutongGames.PlayMaker;
using UnityEngine;

/// <summary>
/// Simple container for an Fsm so it can be saved as an asset in the project.
/// This asset can then be pasted into FSMs.
/// TODO: Create a new PlayMakerFSM from FsmTemplate and add to Game Object.
/// </summary>

[Serializable]
public class FsmTemplate : ScriptableObject
{
    [SerializeField]
    private string category;
    public Fsm fsm;

    public string Category
    {
        get { return category; }
        set { category = value; }
    }
}


