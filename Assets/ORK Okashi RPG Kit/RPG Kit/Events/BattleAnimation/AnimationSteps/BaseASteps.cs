
using System.Collections;
using UnityEngine;

public class AnimationStep
{
	public bool fold = true; 
	public BattleAnimationType type = BattleAnimationType.WAIT;
	public int next = 0;
	public int nextFail = 0;
	public bool stepEnabled = true;
	
	public string[] playOptions = new string[] {"Play", "CrossFade", "Blend", "PlayQueued", "CrossFadeQueued"};
	
	// vars
	public float time = 1;
	public float intensity = 0;
	public float speed = 0;
	public float volume = 1;
	public float float1 = 0;
	public float float2 = 0;
	public float float3 = 0;
	public float float4 = 0;
	public float float5 = 0;
	public float float6 = 0;
	public float float7 = 0;
	public float float8 = 0;
	
	public int actorID = 0;
	public int posID = 0;
	public int spawnID = 0;
	public int min = 0;
	public int max = 0;
	public int itemID = 0;
	public int weaponID = 0;
	public int armorID = 0;
	public int number = 0;
	public int characterID = 0;
	public int waypointID = 0;
	public int prefabID = 0;
	public int prefabID2 = 0;
	public int skillID = 0;
	public int audioID = 0;
	public int formulaID = 0;
	
	public bool wait = true;
	public bool useDefault = true;
	public bool useDefault2 = true;
	public bool show = false;
	public bool show2 = false;
	public bool show3 = false;
	public bool show4 = false;
	public bool show5 = false;
	public bool show6 = false;
	public bool show7 = false;
	public bool show8 = false;
	public bool show9 = false;
	public bool show10 = false;
	public bool show11 = false;
	public bool showShadow = true;
	public bool controller = false;
	
	public EaseType interpolate = EaseType.Linear;
	public PlayMode playMode = PlayMode.StopSameLayer;
	public QueueMode queueMode = QueueMode.CompleteOthers;
	public StatusOrigin statusOrigin = StatusOrigin.TARGET;
	public StatusOrigin statusOrigin2 = StatusOrigin.TARGET;
	public CombatantAnimation combatantAnimation = CombatantAnimation.DAMAGE;
	public BattleMoveToTarget moveToTarget = BattleMoveToTarget.TARGET;
	public AudioRolloffMode audioRolloffMode = AudioRolloffMode.Linear;
	
	public Rect rect = new Rect(0, 0, 100, 100);
	public Rect rect2 = new Rect(0, 0, 100, 100);
	public Vector2 v2 = new Vector2(0, 0);
	public Vector3 v3 = new Vector3(0, 0, 0);
	public Vector3 v3_2 = new Vector3(0, 0, 0);
	public Vector4 v4 = new Vector4(0, 0, 0, 0);
	public Vector4 v4_2 = new Vector4(0, 0, 0, 0);
	
	public string[] message = new string[DataHolder.Languages().GetDataCount()];
	public string key = "";
	public string value = "";
	public SkillEffect[] effect = new SkillEffect[DataHolder.Effects().GetDataCount()];
	
	public SimpleOperator simpleOperator = SimpleOperator.ADD;
	public ValueCheck valueCheck = ValueCheck.EQUALS;
	public BattleAnimationObject animationObject = BattleAnimationObject.USER;
	
	public string pathToChild = "";
	
	public string materialProperty = "_Color";
	
	// editor
	public int moveTo = 0;
	
	public AnimationStep(BattleAnimationType t)
	{
		this.type = t;
	}
	
	public void SetNextIndex(int index)
	{
		this.next = index;
	}
	
	public virtual void Execute(BattleAnimation battleAnimation) {}
	public virtual Hashtable GetData()
	{
		Hashtable ht = new Hashtable();
		ht.Add("type", this.type.ToString());
		if(!this.stepEnabled) ht.Add("stepenabled", "false");
		ht.Add("next", this.next.ToString());
		return ht;
	}
	
	public AnimationStep GetCopy(BattleAnimation battleAnimation)
	{
		AnimationStep s = battleAnimation.TypeToStep(this.type);
		
		s.fold = this.fold;
		s.next = this.next;
		s.nextFail = this.nextFail;
		s.stepEnabled = this.stepEnabled;
		
		s.time = this.time;
		s.intensity = this.intensity;
		s.speed = this.speed;
		s.volume = this.volume;
		s.float1 = this.float1;
		s.float2 = this.float2;
		s.float3 = this.float3;
		s.float4 = this.float4;
		s.float5 = this.float5;
		s.float6 = this.float6;
		s.float7 = this.float7;
		s.float8 = this.float8;
		
		s.actorID = this.actorID;
		s.posID = this.posID;
		s.spawnID = this.spawnID;
		s.min = this.min;
		s.max = this.max;
		s.itemID = this.itemID;
		s.weaponID = this.weaponID;
		s.armorID = this.armorID;
		s.number = this.number;
		s.characterID = this.characterID;
		s.waypointID = this.waypointID;
		s.prefabID = this.prefabID;
		s.prefabID2 = this.prefabID2;
		s.skillID = this.skillID;
		s.audioID = this.audioID;
		s.formulaID = this.formulaID;
		
		s.wait = this.wait;
		s.useDefault = this.useDefault;
		s.useDefault2 = this.useDefault2;
		s.show = this.show;
		s.show2 = this.show2;
		s.show3 = this.show3;
		s.show4 = this.show4;
		s.show5 = this.show5;
		s.show6 = this.show6;
		s.show7 = this.show7;
		s.showShadow = this.showShadow;
		s.controller = this.controller;
		
		s.interpolate = this.interpolate;
		s.playMode = this.playMode;
		s.queueMode = this.queueMode;
		s.statusOrigin = this.statusOrigin;
		s.statusOrigin2 = this.statusOrigin2;
		s.combatantAnimation = this.combatantAnimation;
		s.moveToTarget = this.moveToTarget;
		s.audioRolloffMode = this.audioRolloffMode;
		
		s.rect = this.rect;
		s.rect2 = this.rect2;
		s.v2 = this.v2;
		s.v3 = this.v3;
		s.v3_2 = this.v3_2;
		s.v4 = this.v4;
		s.v4_2 = this.v4_2;
		
		s.message = new string[this.message.Length];
		System.Array.Copy(this.message, s.message, this.message.Length);
		s.key = this.key;
		s.value = this.value;
		s.effect = new SkillEffect[this.effect.Length];
		System.Array.Copy(this.effect, s.effect, this.effect.Length);
		
		s.simpleOperator = this.simpleOperator;
		s.valueCheck = this.valueCheck;
		s.animationObject = this.animationObject;
		
		s.pathToChild = this.pathToChild;
		
		return s;
	}
}

public class WaitAStep : AnimationStep
{
	public WaitAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		battleAnimation.StartTime(this.time, this.next);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("time", this.time.ToString());
		return ht;
	}
}

public class GoToAStep : AnimationStep
{
	public GoToAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		battleAnimation.StepFinished(this.next);
	}
	
	public override Hashtable GetData()
	{
		return base.GetData();
	}
}

public class RandomAStep : AnimationStep
{
	public RandomAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		battleAnimation.StepFinished(Random.Range(this.min, this.max+1));
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("min", this.min.ToString());
		ht.Add("max", this.max.ToString());
		return ht;
	}
}

public class CheckRandomAStep : AnimationStep
{
	public CheckRandomAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		if(DataHolder.GameSettings().GetRandom() <= this.float1) battleAnimation.StepFinished(this.next);
		else battleAnimation.StepFinished(this.nextFail);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("nextfail", this.nextFail.ToString());
		ht.Add("float1", this.float1.ToString());
		return ht;
	}
}

public class CheckFormulaAStep : AnimationStep
{
	public CheckFormulaAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		Character c = GameHandler.Party().GetCharacter(this.characterID);
		if(c != null)
		{
			if(DataHolder.GameSettings().GetRandom() <= DataHolder.Formula(this.formulaID).Calculate(c, c)) battleAnimation.StepFinished(this.next);
			else battleAnimation.StepFinished(this.nextFail);
		}
		else battleAnimation.StepFinished(this.nextFail);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("formula", this.formulaID.ToString());
		ht.Add("character", this.characterID.ToString());
		ht.Add("nextfail", this.nextFail.ToString());
		return ht;
	}
}

public class WaitForButtonAStep : AnimationStep
{
	public WaitForButtonAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		if(this.show && battleAnimation.battleAction.user is Enemy)
		{
			battleAnimation.StartTime(this.time, this.nextFail);
		}
		else
		{
			battleAnimation.WaitForButton(this.key, this.time, this.next, this.nextFail);
		}
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("show", this.show.ToString());
		ht.Add("time", this.time.ToString());
		ht.Add("nextfail", this.nextFail.ToString());
		
		ArrayList subs = new ArrayList();
		Hashtable s = new Hashtable();
		s.Add(XMLHandler.NODE_NAME, "key");
		s.Add(XMLHandler.CONTENT, this.key);
		subs.Add(s);
		
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}

public class CheckDifficultyAStep : AnimationStep
{
	public CheckDifficultyAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		int difID = GameHandler.GetDifficulty();
		if((ValueCheck.EQUALS.Equals(this.valueCheck) && difID == this.number) ||
			(ValueCheck.LESS.Equals(this.valueCheck) && difID < this.number) ||
			(ValueCheck.GREATER.Equals(this.valueCheck) && difID > this.number))
		{
			battleAnimation.StepFinished(this.next);
		}
		else
		{
			battleAnimation.StepFinished(this.nextFail);
		}
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("number", this.number.ToString());
		ht.Add("valuecheck", this.valueCheck.ToString());
		ht.Add("nextfail", this.nextFail.ToString());
		return ht;
	}
}

public class CheckUserAStep : AnimationStep
{
	public CheckUserAStep(BattleAnimationType t) : base(t)
	{
		
	}
	
	public override void Execute(BattleAnimation battleAnimation)
	{
		if((this.show && battleAnimation.battleAction.user is Character) ||
			(!this.show && battleAnimation.battleAction.user is Enemy))
		{
			battleAnimation.StepFinished(this.next);
		}
		else
		{
			battleAnimation.StepFinished(this.nextFail);
		}
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("show", this.show.ToString());
		ht.Add("nextfail", this.nextFail.ToString());
		return ht;
	}
}