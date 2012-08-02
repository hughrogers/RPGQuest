
using System;
using System.Collections;
using UnityEngine;

public class ArrayHelper
{
	// array init helper
	public static int[] Create(int count, int initialValue)
	{
		int[] list = new int[count];
		for(int i=0; i<count; i++) list[i] = initialValue;
		return list;
	}
	
	// base types
	public static string[] Add(string n, string[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(string str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(string)) as string[];
	}
	
	public static string[] Remove(int index, string[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(string str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(string)) as string[];
	}
	
	public static bool[] Add(bool n, bool[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(bool str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(bool)) as bool[];
	}
	
	public static bool[] Remove(int index, bool[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(bool str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(bool)) as bool[];
	}
	
	public static int[] Add(int n, int[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(int str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(int)) as int[];
	}
	
	public static int[] Remove(int index, int[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(int str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(int)) as int[];
	}
	
	public static float[] Add(float n, float[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(float str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(float)) as float[];
	}
	
	public static float[] Remove(int index, float[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(float str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(float)) as float[];
	}
	
	public static Color[] Add(Color n, Color[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Color str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(Color)) as Color[];
	}
	
	public static Color[] Remove(int index, Color[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Color str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(Color)) as Color[];
	}
	
	public static Vector2[] Add(Vector2 n, Vector2[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Vector2 str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(Vector2)) as Vector2[];
	}
	
	public static Vector2[] Remove(int index, Vector2[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Vector2 str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(Vector2)) as Vector2[];
	}
	
	public static Vector3[] Add(Vector3 n, Vector3[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Vector3 str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(Vector3)) as Vector3[];
	}
	
	public static Vector3[] Remove(int index, Vector3[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Vector3 str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(Vector3)) as Vector3[];
	}
	
	// enum types
	public static SkillEffect[] Add(SkillEffect n, SkillEffect[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(SkillEffect str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(SkillEffect)) as SkillEffect[];
	}
	
	public static SkillEffect[] Remove(int index, SkillEffect[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(SkillEffect str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(SkillEffect)) as SkillEffect[];
	}
	
	public static SimpleOperator[] Add(SimpleOperator n, SimpleOperator[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(SimpleOperator str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(SimpleOperator)) as SimpleOperator[];
	}
	
	public static SimpleOperator[] Remove(int index, SimpleOperator[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(SimpleOperator str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(SimpleOperator)) as SimpleOperator[];
	}
	
	public static AttackSelection[] Add(AttackSelection n, AttackSelection[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(AttackSelection str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(AttackSelection)) as AttackSelection[];
	}
	
	public static AttackSelection[] Remove(int index, AttackSelection[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(AttackSelection str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(AttackSelection)) as AttackSelection[];
	}
	
	public static EquipSet[] Add(EquipSet n, EquipSet[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(EquipSet str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(EquipSet)) as EquipSet[];
	}
	
	public static EquipSet[] Remove(int index, EquipSet[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(EquipSet str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(EquipSet)) as EquipSet[];
	}
	
	public static ItemDropType[] Add(ItemDropType n, ItemDropType[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(ItemDropType str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(ItemDropType)) as ItemDropType[];
	}
	
	public static ItemDropType[] Remove(int index, ItemDropType[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(ItemDropType str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(ItemDropType)) as ItemDropType[];
	}
	
	// special types
	public static Language[] Add(Language n, Language[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Language str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(Language)) as Language[];
	}
	
	public static Language[] Remove(int index, Language[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Language str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(Language)) as Language[];
	}
	
	public static StatusDevelopment[] Add(StatusDevelopment n, StatusDevelopment[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(StatusDevelopment str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(StatusDevelopment)) as StatusDevelopment[];
	}
	
	public static StatusDevelopment[] Remove(int index, StatusDevelopment[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(StatusDevelopment str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(StatusDevelopment)) as StatusDevelopment[];
	}
	
	public static Armor[] Add(Armor n, Armor[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Armor str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(Armor)) as Armor[];
	}
	
	public static Armor[] Remove(int index, Armor[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Armor str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(Armor)) as Armor[];
	}
	
	public static BattleAnimation[] Add(BattleAnimation n, BattleAnimation[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(BattleAnimation str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(BattleAnimation)) as BattleAnimation[];
	}
	
	public static BattleAnimation[] Remove(int index, BattleAnimation[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(BattleAnimation str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(BattleAnimation)) as BattleAnimation[];
	}
	
	public static CameraPosition[] Add(CameraPosition n, CameraPosition[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(CameraPosition str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(CameraPosition)) as CameraPosition[];
	}
	
	public static CameraPosition[] Remove(int index, CameraPosition[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(CameraPosition str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(CameraPosition)) as CameraPosition[];
	}
	
	public static Character[] Add(Character n, Character[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Character str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(Character)) as Character[];
	}
	
	public static Character[] Remove(int index, Character[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Character str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(Character)) as Character[];
	}
	
	public static Character[] Remove(Character c, Character[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Character str in list) tmp.Add(str);
		tmp.Remove(c);
		return tmp.ToArray(typeof(Character)) as Character[];
	}
	
	public static Class[] Add(Class n, Class[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Class str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(Class)) as Class[];
	}
	
	public static Class[] Remove(int index, Class[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Class str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(Class)) as Class[];
	}
	
	public static DialoguePosition[] Add(DialoguePosition n, DialoguePosition[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(DialoguePosition str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(DialoguePosition)) as DialoguePosition[];
	}
	
	public static DialoguePosition[] Remove(int index, DialoguePosition[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(DialoguePosition str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(DialoguePosition)) as DialoguePosition[];
	}
	
	public static DialoguePosition[] Remove(DialoguePosition c, DialoguePosition[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(DialoguePosition str in list) tmp.Add(str);
		tmp.Remove(c);
		return tmp.ToArray(typeof(DialoguePosition)) as DialoguePosition[];
	}
	
	public static BattleAI[] Add(BattleAI n, BattleAI[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(BattleAI str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(BattleAI)) as BattleAI[];
	}
	
	public static BattleAI[] Remove(int index, BattleAI[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(BattleAI str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(BattleAI)) as BattleAI[];
	}
	
	public static AICondition[] Add(AICondition n, AICondition[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(AICondition str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(AICondition)) as AICondition[];
	}
	
	public static AICondition[] Remove(int index, AICondition[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(AICondition str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(AICondition)) as AICondition[];
	}
	
	public static Enemy[] Add(Enemy n, Enemy[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Enemy str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(Enemy)) as Enemy[];
	}
	
	public static Enemy[] Remove(int index, Enemy[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Enemy str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(Enemy)) as Enemy[];
	}
	
	public static Enemy[] Remove(Enemy c, Enemy[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Enemy str in list) tmp.Add(str);
		tmp.Remove(c);
		return tmp.ToArray(typeof(Enemy)) as Enemy[];
	}
	
	public static ValueChange[] Add(ValueChange n, ValueChange[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(ValueChange str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(ValueChange)) as ValueChange[];
	}
	
	public static ValueChange[] Remove(int index, ValueChange[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(ValueChange str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(ValueChange)) as ValueChange[];
	}
	
	public static ItemDrop[] Add(ItemDrop n, ItemDrop[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(ItemDrop str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(ItemDrop)) as ItemDrop[];
	}
	
	public static ItemDrop[] Remove(int index, ItemDrop[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(ItemDrop str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(ItemDrop)) as ItemDrop[];
	}
	
	public static Formula[] Add(Formula n, Formula[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Formula str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(Formula)) as Formula[];
	}
	
	public static Formula[] Remove(int index, Formula[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Formula str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(Formula)) as Formula[];
	}
	
	public static StatusValue[] Add(StatusValue n, StatusValue[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(StatusValue str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(StatusValue)) as StatusValue[];
	}
	
	public static StatusValue[] Remove(int index, StatusValue[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(StatusValue str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(StatusValue)) as StatusValue[];
	}
	
	public static EquipmentSkill[] Add(EquipmentSkill n, EquipmentSkill[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(EquipmentSkill str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(EquipmentSkill)) as EquipmentSkill[];
	}
	
	public static EquipmentSkill[] Remove(int index, EquipmentSkill[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(EquipmentSkill str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(EquipmentSkill)) as EquipmentSkill[];
	}
	
	public static FormulaPiece[] Add(FormulaPiece n, FormulaPiece[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(FormulaPiece str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(FormulaPiece)) as FormulaPiece[];
	}
	
	public static FormulaPiece[] Remove(int index, FormulaPiece[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(FormulaPiece str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(FormulaPiece)) as FormulaPiece[];
	}
	
	public static HUDElement[] Add(HUDElement n, HUDElement[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(HUDElement str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(HUDElement)) as HUDElement[];
	}
	
	public static HUDElement[] Remove(int index, HUDElement[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(HUDElement str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(HUDElement)) as HUDElement[];
	}
	
	public static HUD[] Add(HUD n, HUD[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(HUD str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(HUD)) as HUD[];
	}
	
	public static HUD[] Remove(int index, HUD[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(HUD str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(HUD)) as HUD[];
	}
	
	public static Item[] Add(Item n, Item[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Item str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(Item)) as Item[];
	}
	
	public static Item[] Remove(int index, Item[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Item str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(Item)) as Item[];
	}
	
	public static ChoiceContent[] Add(ChoiceContent n, ChoiceContent[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(ChoiceContent str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(ChoiceContent)) as ChoiceContent[];
	}
	
	public static ChoiceContent[] Remove(int index, ChoiceContent[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(ChoiceContent str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(ChoiceContent)) as ChoiceContent[];
	}
	
	public static MusicClip[] Add(MusicClip n, MusicClip[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(MusicClip str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(MusicClip)) as MusicClip[];
	}
	
	public static MusicClip[] Remove(int index, MusicClip[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(MusicClip str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(MusicClip)) as MusicClip[];
	}
	
	public static Skill[] Add(Skill n, Skill[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Skill str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(Skill)) as Skill[];
	}
	
	public static Skill[] Remove(int index, Skill[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Skill str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(Skill)) as Skill[];
	}
	
	public static StatusEffect[] Add(StatusEffect n, StatusEffect[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(StatusEffect str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(StatusEffect)) as StatusEffect[];
	}
	
	public static StatusEffect[] Remove(int index, StatusEffect[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(StatusEffect str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(StatusEffect)) as StatusEffect[];
	}
	
	public static StatusEffect[] Remove(StatusEffect e, StatusEffect[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(StatusEffect str in list) tmp.Add(str);
		tmp.Remove(e);
		return tmp.ToArray(typeof(StatusEffect)) as StatusEffect[];
	}
	
	public static StatusCondition[] Add(StatusCondition n, StatusCondition[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(StatusCondition str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(StatusCondition)) as StatusCondition[];
	}
	
	public static StatusCondition[] Remove(int index, StatusCondition[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(StatusCondition str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(StatusCondition)) as StatusCondition[];
	}
	
	public static Weapon[] Add(Weapon n, Weapon[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Weapon str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(Weapon)) as Weapon[];
	}
	
	public static Weapon[] Remove(int index, Weapon[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Weapon str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(Weapon)) as Weapon[];
	}
	
	public static AnimationStep[] Add(AnimationStep n, AnimationStep[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(AnimationStep str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(AnimationStep)) as AnimationStep[];
	}
	
	public static AnimationStep[] Remove(int index, AnimationStep[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(AnimationStep str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(AnimationStep)) as AnimationStep[];
	}
	
	public static Transform[] Add(Transform n, Transform[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Transform str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(Transform)) as Transform[];
	}
	
	public static Transform[] Remove(int index, Transform[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Transform str in list) tmp.Add(str);
		if(tmp[index] == null) tmp[index] = "";
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(Transform)) as Transform[];
	}
	
	public static GameObject[] Add(GameObject n, GameObject[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(GameObject str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(GameObject)) as GameObject[];
	}
	
	public static GameObject[] Remove(int index, GameObject[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(GameObject str in list) tmp.Add(str);
		if(tmp[index] == null) tmp[index] = "";
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(GameObject)) as GameObject[];
	}
	
	public static AudioClip[] Add(AudioClip n, AudioClip[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(AudioClip str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(AudioClip)) as AudioClip[];
	}
	
	public static AudioClip[] Remove(int index, AudioClip[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(AudioClip str in list) tmp.Add(str);
		if(tmp[index] == null) tmp[index] = "";
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(AudioClip)) as AudioClip[];
	}
	
	public static EventStep[] Add(EventStep n, EventStep[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(EventStep str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(EventStep)) as EventStep[];
	}
	
	public static EventStep[] Remove(int index, EventStep[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(EventStep str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(EventStep)) as EventStep[];
	}
	
	public static EventActor[] Add(EventActor n, EventActor[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(EventActor str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(EventActor)) as EventActor[];
	}
	
	public static EventActor[] Remove(int index, EventActor[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(EventActor str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(EventActor)) as EventActor[];
	}
	
	public static LabelContent[] Add(LabelContent n, LabelContent[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(LabelContent str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(LabelContent)) as LabelContent[];
	}
	
	public static LabelContent[] Remove(int index, LabelContent[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(LabelContent str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(LabelContent)) as LabelContent[];
	}
	
	public static StatusBar[] Add(StatusBar n, StatusBar[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(StatusBar str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(StatusBar)) as StatusBar[];
	}
	
	public static StatusBar[] Remove(int index, StatusBar[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(StatusBar str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(StatusBar)) as StatusBar[];
	}
	
	public static ValueCheck[] Add(ValueCheck n, ValueCheck[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(ValueCheck str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(ValueCheck)) as ValueCheck[];
	}
	
	public static ValueCheck[] Remove(int index, ValueCheck[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(ValueCheck str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(ValueCheck)) as ValueCheck[];
	}
	
	public static ItemRecipe[] Add(ItemRecipe n, ItemRecipe[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(ItemRecipe str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(ItemRecipe)) as ItemRecipe[];
	}
	
	public static ItemRecipe[] Remove(int index, ItemRecipe[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(ItemRecipe str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(ItemRecipe)) as ItemRecipe[];
	}
	
	public static ItemShort[] Add(ItemShort n, ItemShort[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(ItemShort str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(ItemShort)) as ItemShort[];
	}
	
	public static ItemShort[] Remove(int index, ItemShort[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(ItemShort str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(ItemShort)) as ItemShort[];
	}
	
	public static GUIFont[] Add(GUIFont n, GUIFont[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(GUIFont str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(GUIFont)) as GUIFont[];
	}
	
	public static GUIFont[] Remove(int index, GUIFont[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(GUIFont str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(GUIFont)) as GUIFont[];
	}
	
	public static StatusRequirement[] Add(StatusRequirement n, StatusRequirement[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(StatusRequirement str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(StatusRequirement)) as StatusRequirement[];
	}
	
	public static StatusRequirement[] Remove(int index, StatusRequirement[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(StatusRequirement str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(StatusRequirement)) as StatusRequirement[];
	}
	
	public static EffectPrefab[] Add(EffectPrefab n, EffectPrefab[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(EffectPrefab str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(EffectPrefab)) as EffectPrefab[];
	}
	
	public static EffectPrefab[] Remove(int index, EffectPrefab[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(EffectPrefab str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(EffectPrefab)) as EffectPrefab[];
	}
	
	public static SkillBlock[] Add(SkillBlock n, SkillBlock[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(SkillBlock str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(SkillBlock)) as SkillBlock[];
	}
	
	public static SkillBlock[] Remove(int index, SkillBlock[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(SkillBlock str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(SkillBlock)) as SkillBlock[];
	}
	
	public static SkillBlock[] Remove(SkillBlock c, SkillBlock[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(SkillBlock str in list) tmp.Add(str);
		tmp.Remove(c);
		return tmp.ToArray(typeof(SkillBlock)) as SkillBlock[];
	}
	
	public static SkillLevel[] Add(SkillLevel n, SkillLevel[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(SkillLevel str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(SkillLevel)) as SkillLevel[];
	}
	
	public static SkillLevel[] Remove(int index, SkillLevel[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(SkillLevel str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(SkillLevel)) as SkillLevel[];
	}
	
	public static SkillLearn[] Add(SkillLearn n, SkillLearn[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(SkillLearn str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(SkillLearn)) as SkillLearn[];
	}
	
	public static SkillLearn[] Remove(int index, SkillLearn[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(SkillLearn str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(SkillLearn)) as SkillLearn[];
	}
	
	public static SkillLearn[] Remove(SkillLearn c, SkillLearn[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(SkillLearn str in list) tmp.Add(str);
		tmp.Remove(c);
		return tmp.ToArray(typeof(SkillLearn)) as SkillLearn[];
	}
	
	public static BattleAction[] Add(BattleAction n, BattleAction[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(BattleAction str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(BattleAction)) as BattleAction[];
	}
	
	public static BattleAction[] Remove(int index, BattleAction[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(BattleAction str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(BattleAction)) as BattleAction[];
	}
	
	public static BattleAction[] Remove(BattleAction c, BattleAction[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(BattleAction str in list) tmp.Add(str);
		tmp.Remove(c);
		return tmp.ToArray(typeof(BattleAction)) as BattleAction[];
	}
	
	public static Combatant[] Add(Combatant n, Combatant[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Combatant str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(Combatant)) as Combatant[];
	}
	
	public static Combatant[] Remove(int index, Combatant[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Combatant str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(Combatant)) as Combatant[];
	}
	
	public static Combatant[] Remove(Combatant c, Combatant[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Combatant str in list) tmp.Add(str);
		tmp.Remove(c);
		return tmp.ToArray(typeof(Combatant)) as Combatant[];
	}
	
	public static Combatant[] Change(Combatant c, Combatant[] list, int index)
	{
		ArrayList tmp = new ArrayList();
		foreach(Combatant str in list) tmp.Add(str);
		tmp.Remove(c);
		tmp.Insert(index, c);
		return tmp.ToArray(typeof(Combatant)) as Combatant[];
	}
	
	public static BattleMenuItem[] Add(BattleMenuItem n, BattleMenuItem[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(BattleMenuItem str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(BattleMenuItem)) as BattleMenuItem[];
	}
	
	public static BattleMenuItem[] Remove(int index, BattleMenuItem[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(BattleMenuItem str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(BattleMenuItem)) as BattleMenuItem[];
	}
	
	public static AIBehaviour[] Add(AIBehaviour n, AIBehaviour[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(AIBehaviour str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(AIBehaviour)) as AIBehaviour[];
	}
	
	public static AIBehaviour[] Remove(int index, AIBehaviour[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(AIBehaviour str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(AIBehaviour)) as AIBehaviour[];
	}
	
	public static BaseAttack[] Add(BaseAttack n, BaseAttack[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(BaseAttack str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(BaseAttack)) as BaseAttack[];
	}
	
	public static BaseAttack[] Remove(int index, BaseAttack[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(BaseAttack str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(BaseAttack)) as BaseAttack[];
	}
	
	public static GlobalEvent[] Add(GlobalEvent n, GlobalEvent[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(GlobalEvent str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(GlobalEvent)) as GlobalEvent[];
	}
	
	public static GlobalEvent[] Remove(int index, GlobalEvent[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(GlobalEvent str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(GlobalEvent)) as GlobalEvent[];
	}
	
	public static TeleportTarget[] Add(TeleportTarget n, TeleportTarget[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(TeleportTarget str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(TeleportTarget)) as TeleportTarget[];
	}
	
	public static TeleportTarget[] Remove(int index, TeleportTarget[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(TeleportTarget str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(TeleportTarget)) as TeleportTarget[];
	}
	
	public static VariableCondition[] Add(VariableCondition n, VariableCondition[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(VariableCondition str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(VariableCondition)) as VariableCondition[];
	}
	
	public static VariableCondition[] Remove(int index, VariableCondition[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(VariableCondition str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(VariableCondition)) as VariableCondition[];
	}
	
	public static AnimationData[] Add(AnimationData n, AnimationData[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(AnimationData str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(AnimationData)) as AnimationData[];
	}
	
	public static AnimationData[] Remove(int index, AnimationData[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(AnimationData str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(AnimationData)) as AnimationData[];
	}
	
	public static Difficulty[] Add(Difficulty n, Difficulty[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Difficulty str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(Difficulty)) as Difficulty[];
	}
	
	public static Difficulty[] Remove(int index, Difficulty[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(Difficulty str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(Difficulty)) as Difficulty[];
	}
	
	public static DifficultyBonus[] Add(DifficultyBonus n, DifficultyBonus[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(DifficultyBonus str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(DifficultyBonus)) as DifficultyBonus[];
	}
	
	public static DifficultyBonus[] Remove(int index, DifficultyBonus[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(DifficultyBonus str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(DifficultyBonus)) as DifficultyBonus[];
	}
	
	public static DifficultyBonus[] Remove(DifficultyBonus c, DifficultyBonus[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(DifficultyBonus str in list) tmp.Add(str);
		tmp.Remove(c);
		return tmp.ToArray(typeof(DifficultyBonus)) as DifficultyBonus[];
	}
	
	public static StatusTimeChange[] Add(StatusTimeChange n, StatusTimeChange[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(StatusTimeChange str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(StatusTimeChange)) as StatusTimeChange[];
	}
	
	public static StatusTimeChange[] Remove(int index, StatusTimeChange[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(StatusTimeChange str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(StatusTimeChange)) as StatusTimeChange[];
	}
	
	public static EnemyTeam[] Add(EnemyTeam n, EnemyTeam[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(EnemyTeam str in list) tmp.Add(str);
		tmp.Add(n);
		return tmp.ToArray(typeof(EnemyTeam)) as EnemyTeam[];
	}
	
	public static EnemyTeam[] Remove(int index, EnemyTeam[] list)
	{
		ArrayList tmp = new ArrayList();
		foreach(EnemyTeam str in list) tmp.Add(str);
		tmp.RemoveAt(index);
		return tmp.ToArray(typeof(EnemyTeam)) as EnemyTeam[];
	}
}
