// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

// EditorWindow classes can't be called from a dll 
// so create a thin wrapper class as a workaround
// TODO: move to dll when Unity supports it

class FsmLogWindow : HutongGames.PlayMakerEditor.FsmLogger
{
}
