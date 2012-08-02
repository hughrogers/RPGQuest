
// game settings
public enum GameControl {BOTH, MOUSE, KEYBOARD};
public enum PlayerControlType {NONE, DEFAULT, MOBILE};
public enum CameraControlType {NONE, FOLLOW, LOOK, MOBILE, FIRST_PERSON};
public enum InputHandling {BUTTON_DOWN, BUTTON_UP, KEY_DOWN, KEY_UP};
public enum ButtonPosition {TOP_LEFT, TOP_CENTER, TOP_RIGHT, BOTTOM_LEFT, BOTTOM_CENTER, BOTTOM_RIGHT};
public enum ControlType {NONE, FIELD, BATTLE, EVENT, MENU, SAVE};
public enum GUISystemType {UNITY, ORK};
public enum GUIImageStretch {POINT, BILINEAR};
public enum TextureDelete {NONE, CLOSE, SCENE};
public enum SaveGameType {PLAYER_PREFS, FILE};
public enum GUIScaleMode {STRETCH_TO_FILL, SCALE_AND_CROP, SCALE_TO_FIT, NO_SCALE};
public enum Selector {NONE, SELECT, ALL};

// div
public enum ValueSetter {PERCENT, VALUE};
public enum ItemSkillType {NONE, LEARN, USE};
public enum ItemVariableType {NONE, SET, REMOVE};
public enum ItemDropType {ITEM, WEAPON, ARMOR};
public enum ActiveSelection {INACTIVE, ACTIVE};
public enum ColumnFill {VERTICAL, HORIZONTAL};
public enum SavePointType {SAVE_POINT, AUTO_SAVE, RETRY_POINT}
public enum MouseTouch {START, MOVE, END}

// status values
public enum StatusValueType {NORMAL, CONSUMABLE, EXPERIENCE};

// status effect
public enum StatusEffectEnd {NONE, TURN, TIME};
public enum StatusConditionExecution {CAST, TURN, TIME};
public enum StatusNeeded {STATUS_VALUE, ELEMENT, SKILL, RACE, SIZE, LEVEL};

// skill
public enum SkillEffect {NONE, ADD, REMOVE};
public enum TargetType {ALLY, ENEMY, SELF};
public enum SkillTarget {SINGLE, GROUP, NONE};

// formula
public enum SimpleOperator {ADD, SUB, SET};
public enum FormulaChooser {VALUE, STATUS, FORMULA, RANDOM};
public enum StatusOrigin {USER, TARGET};
public enum FormulaOperator {DONE, ADD, SUB, MULTIPLY, DIVIDE, MODULO, POWER_OF, LOG};
public enum Rounding {NONE, CEIL, FLOOR, ROUND};

// equipment
public enum EquipType {SINGLE, MULTI};
public enum ValueCheck {EQUALS, LESS, GREATER};
public enum EquipSet  {NONE, WEAPON, ARMOR};

// battle ai
public enum AIConditionNeeded {ALL, ONE};
public enum AIConditionTarget {SELF, ALLY, ENEMY};
public enum AIConditionType {STATUS, EFFECT, ELEMENT, TURN, CHANCE, DEATH, RACE, SIZE};

// battle system
public enum BattleSystemType {TURN, ACTIVE, REALTIME};
public enum AttackSelection {ATTACK, SKILL, ITEM, DEFEND, ESCAPE, DEATH, COUNTER, NONE};
public enum CombatantAnimation {NONE, IDLE, WALK, RUN, SPRINT, JUMP, FALL, LAND, ATTACK, DEFEND, ITEM, SKILL, DAMAGE, EVADE, DEATH, REVIVE};
public enum BattleMoveToTarget {TARGET, BASE, CENTER};
public enum BattleMenuMode {NONE, BASE, SKILL, ITEM, TARGET};
public enum BattleAnimationObject {USER, TARGET, ARENA, PREFAB};
public enum DamageDealerType {TRIGGER_ENTER, TRIGGER_EXIT, TRIGGER_STAY, COLLISION_ENTER, COLLISION_EXIT, COLLISION_STAY};
public enum TargetRayOrigin {USER, SCREEN};
public enum AggressiveType {ALWAYS, DAMAGE, SELECTION, ACTION};
public enum EnemyCounting {NONE, LETTERS, NUMBERS};
public enum UseTimebarAction {ACTION_BORDER, MAX_TIMEBAR, END_TURN};

// event
public enum EventStartType {INTERACT, AUTOSTART, TRIGGER_ENTER, TRIGGER_EXIT, NONE, KEY_PRESS, DROP};
public enum GlobalEventType {CALL, AUTO};

// music
public enum MusicPlayType {PLAY, STOP, FADE_IN, FADE_OUT, FADE_TO};

// hud
public enum HUDElementType {TEXT, IMAGE, NAME, STATUS, TIMEBAR, USED_TIMEBAR, CASTTIME, EFFECT, VARIABLE};
public enum HUDDisplayType {TEXT, BAR};
public enum HUDContentType {TEXT, ICON, BOTH};
public enum HUDNameType {CHARACTER, CLASS, STATUS};
public enum HUDClick {NONE, INTERACT, MENU, MENUSCREEN, BATTLEMENU};

// drag
public enum DragType {NONE, ITEM, WEAPON, ARMOR, EQUIP_PART, SKILL, ATTACK};
public enum DragOrigin {NONE, INVENTORY, EQUIP, SHOP, BATTLE_MENU};
public enum DropType {NONE, EQUIP_PART};
