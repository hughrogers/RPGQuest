
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BattleArena))]
public class BattleArenaInspector : BaseInspector
{
	private string[] gizmoModes = new string[] {"Standard battle", "Party advantage", "Enemy advantage"};
	private int tmpMode = 0;
	
	public override void OnInspectorGUI()
	{
		// data settings
		if(((BattleArena)target).partySpot.Length != DataHolder.GameSettings().maxBattleParty)
		{
			((BattleArena)target).partySpot = new Transform[DataHolder.GameSettings().maxBattleParty];
		}
		if(((BattleArena)target).partySpotPA.Length != DataHolder.GameSettings().maxBattleParty)
		{
			((BattleArena)target).partySpotPA = new Transform[DataHolder.GameSettings().maxBattleParty];
		}
		if(((BattleArena)target).partySpotEA.Length != DataHolder.GameSettings().maxBattleParty)
		{
			((BattleArena)target).partySpotEA = new Transform[DataHolder.GameSettings().maxBattleParty];
		}
		if(((BattleArena)target).enemySpotPA.Length != ((BattleArena)target).enemySpot.Length)
		{
			((BattleArena)target).enemySpotPA = new Transform[((BattleArena)target).enemySpot.Length];
		}
		if(((BattleArena)target).enemySpotEA.Length != ((BattleArena)target).enemySpot.Length)
		{
			((BattleArena)target).enemySpotEA = new Transform[((BattleArena)target).enemySpot.Length];
		}
		
		
		EditorGUILayout.BeginVertical();
		tmpMode = ((BattleArena)target).gizmoMode;
		((BattleArena)target).gizmoMode = EditorGUILayout.Popup("Gizmo mode", ((BattleArena)target).gizmoMode, this.gizmoModes);
		if(tmpMode != ((BattleArena)target).gizmoMode)
		{
			EditorUtility.SetDirty(((BattleArena)target).gameObject);
		}
		EditorGUILayout.Separator();
		
		EditorGUILayout.BeginVertical("box");
		fold1 = EditorGUILayout.Foldout(fold1, "Battle setup");
		if(fold1)
		{
			this.EventStartSettings((BattleArena)target);
			
			((BattleArena)target).deactivateAfter = EditorGUILayout.Toggle("Deactivate after battle", ((BattleArena)target).deactivateAfter);
			((BattleArena)target).canEscape = EditorGUILayout.Toggle("Can escape", ((BattleArena)target).canEscape);
			((BattleArena)target).noGameOver = EditorGUILayout.Toggle("No game over", ((BattleArena)target).noGameOver);
			((BattleArena)target).removeParty = EditorGUILayout.Toggle("Remove party", ((BattleArena)target).removeParty);
			((BattleArena)target).resetPartyPosition = EditorGUILayout.Toggle("Reset party pos", ((BattleArena)target).resetPartyPosition);
			
			// party/enemy settings
			EditorGUILayout.Separator();
			GUILayout.Label("Party spots", EditorStyles.boldLabel);
			for(int i=0; i<((BattleArena)target).partySpot.Length; i++)
			{
				if(((BattleArena)target).partySpot[i] == null &&
					i >= DataHolder.BattleSystem().partySpot.Length)
				{
					GUILayout.Label("Warning! No default party spot found!", EditorStyles.boldLabel);
				}
				((BattleArena)target).partySpot[i] = (Transform)EditorGUILayout.ObjectField("Spot "+i, 
						((BattleArena)target).partySpot[i], typeof(Transform), true);
				if(DataHolder.BattleSystem().enablePASpots && 
					((BattleArena)target).enablePartyAdvantage)
				{
					((BattleArena)target).partySpotPA[i] = (Transform)EditorGUILayout.ObjectField("PA spot "+i, 
							((BattleArena)target).partySpotPA[i], typeof(Transform), true);
				}
				if(DataHolder.BattleSystem().enableEASpots && 
					((BattleArena)target).enableEnemiesAdvantage)
				{
					((BattleArena)target).partySpotEA[i] = (Transform)EditorGUILayout.ObjectField("EA spot "+i, 
							((BattleArena)target).partySpotEA[i], typeof(Transform), true);
				}
			}
			EditorGUILayout.Separator();
			
			if(GUILayout.Button("Add enemy", GUILayout.Width(150)))
			{
				((BattleArena)target).AddEnemy();
			}
			for(int i=0; i<((BattleArena)target).enemy.Length; i++)
			{
				EditorGUILayout.BeginVertical("box");
				GUILayout.BeginHorizontal();
				GUILayout.Label("Enemy "+i.ToString(), EditorStyles.boldLabel);
				if(((BattleArena)target).enemy.Length > 1) 
				{
					if(GUILayout.Button("Remove", GUILayout.Width(75)))
					{
						((BattleArena)target).RemoveEnemy(i);
						return;
					}
				}
				GUILayout.EndHorizontal();
				((BattleArena)target).enemy[i] = EditorGUILayout.Popup("Select enemy", 
						((BattleArena)target).enemy[i], DataHolder.Enemies().GetNameList(true));
				
				if(((BattleArena)target).enemySpot[i] == null &&
					i >= DataHolder.BattleSystem().enemySpot.Length)
				{
					GUILayout.Label("Warning! No default enemy spot found!", EditorStyles.boldLabel);
				}
				((BattleArena)target).enemySpot[i] = (Transform)EditorGUILayout.ObjectField("Spot", 
						((BattleArena)target).enemySpot[i], typeof(Transform), true);
				if(DataHolder.BattleSystem().enablePASpots && 
					((BattleArena)target).enablePartyAdvantage)
				{
					((BattleArena)target).enemySpotPA[i] = (Transform)EditorGUILayout.ObjectField("PA spot", 
							((BattleArena)target).enemySpotPA[i], typeof(Transform), true);
				}
				if(DataHolder.BattleSystem().enableEASpots && 
					((BattleArena)target).enableEnemiesAdvantage)
				{
					((BattleArena)target).enemySpotEA[i] = (Transform)EditorGUILayout.ObjectField("EA spot", 
							((BattleArena)target).enemySpotEA[i], typeof(Transform), true);
				}
				
				((BattleArena)target).spawnEnemy[i] = EditorGUILayout.Toggle("Spawn enemy", 
						((BattleArena)target).spawnEnemy[i]);
				if(!((BattleArena)target).spawnEnemy[i])
				{
					((BattleArena)target).enemyObject[i] = (GameObject)EditorGUILayout.ObjectField("Enemy object", 
							((BattleArena)target).enemyObject[i], typeof(GameObject), true);
				}
				EditorGUILayout.Separator();
				EditorGUILayout.EndVertical();
			}
			EditorGUILayout.Separator();
		}
		EditorGUILayout.EndVertical();
		
		
		if(DataHolder.BattleSystem().partyAdvantage.enabled || 
			DataHolder.BattleSystem().enemiesAdvantage.enabled)
		{
			EditorGUILayout.BeginVertical("box");
			fold7 = EditorGUILayout.Foldout(fold7, "Battle advantage settings");
			if(fold7)
			{
				if(DataHolder.BattleSystem().partyAdvantage.enabled)
				{
					GUILayout.Label("Party advantage", EditorStyles.boldLabel);
					((BattleArena)target).enablePartyAdvantage = EditorGUILayout.Toggle("Enable", 
							((BattleArena)target).enablePartyAdvantage);
					if(((BattleArena)target).enablePartyAdvantage)
					{
						((BattleArena)target).overridePAChance = EditorGUILayout.Toggle("Override chance", 
								((BattleArena)target).overridePAChance);
						if(((BattleArena)target).overridePAChance)
						{
							((BattleArena)target).paChance = EditorGUILayout.FloatField("Chance (%)", 
									((BattleArena)target).paChance);
							FloatHelper.ChanceLimit(ref ((BattleArena)target).paChance);
						}
					}
					EditorGUILayout.Separator();
				}
				
				if(DataHolder.BattleSystem().enemiesAdvantage.enabled)
				{
					GUILayout.Label("Enemies advantage", EditorStyles.boldLabel);
					((BattleArena)target).enableEnemiesAdvantage = EditorGUILayout.Toggle("Enable", 
							((BattleArena)target).enableEnemiesAdvantage);
					if(((BattleArena)target).enableEnemiesAdvantage)
					{
						((BattleArena)target).overrideEAChance = EditorGUILayout.Toggle("Override chance", 
								((BattleArena)target).overrideEAChance);
						if(((BattleArena)target).overrideEAChance)
						{
							((BattleArena)target).eaChance = EditorGUILayout.FloatField("Chance (%)", 
									((BattleArena)target).eaChance);
							FloatHelper.ChanceLimit(ref ((BattleArena)target).eaChance);
						}
					}
					EditorGUILayout.Separator();
				}
			}
			EditorGUILayout.EndVertical();
		}
		
		
		EditorGUILayout.BeginVertical("box");
		fold6 = EditorGUILayout.Foldout(fold6, "Additional battle gains");
		if(fold6)
		{
			((BattleArena)target).moneyGain = EditorGUILayout.IntField("Money", ((BattleArena)target).moneyGain);
			
			if(GUILayout.Button("Add gain", GUILayout.Width(150)))
			{
				((BattleArena)target).AddGain();
			}
			for(int i=0; i<((BattleArena)target).gainType.Length; i++)
			{
				EditorGUILayout.BeginVertical("box");
				if(GUILayout.Button("Remove", GUILayout.Width(75)))
				{
					((BattleArena)target).RemoveGain(i);
					break;
				}
				
				((BattleArena)target).gainType[i] = (ItemDropType)EditorGUILayout.EnumPopup("Type", ((BattleArena)target).gainType[i]);
				if(ItemDropType.ITEM.Equals(((BattleArena)target).gainType[i]))
				{
					((BattleArena)target).gainID[i] = EditorGUILayout.Popup("Item", ((BattleArena)target).gainID[i], 
							DataHolder.Items().GetNameList(true));
				}
				else if(ItemDropType.WEAPON.Equals(((BattleArena)target).gainType[i]))
				{
					((BattleArena)target).gainID[i] = EditorGUILayout.Popup("Weapon", ((BattleArena)target).gainID[i], 
							DataHolder.Weapons().GetNameList(true));
				}
				else if(ItemDropType.ARMOR.Equals(((BattleArena)target).gainType[i]))
				{
					((BattleArena)target).gainID[i] = EditorGUILayout.Popup("Armor", ((BattleArena)target).gainID[i], 
							DataHolder.Armors().GetNameList(true));
				}
				
				((BattleArena)target).gainQuantity[i] = EditorGUILayout.IntField("Quantity", ((BattleArena)target).gainQuantity[i]);
				if(((BattleArena)target).gainQuantity[i] <= 0) ((BattleArena)target).gainQuantity[i] = 1;
				
				((BattleArena)target).gainChance[i] = EditorGUILayout.FloatField("Chance (%)", ((BattleArena)target).gainChance[i]);
				FloatHelper.ChanceLimit(ref ((BattleArena)target).gainChance[i]);
				EditorGUILayout.Separator();
				EditorGUILayout.EndVertical();
			}
			EditorGUILayout.Separator();
		}
		EditorGUILayout.EndVertical();
		
		
		// variable settings
		EditorGUILayout.BeginVertical("box");
		fold5 = EditorGUILayout.Foldout(fold5, "Variable setup");
		if(fold5)
		{
			this.VariableSettings((BattleArena)target);
			EditorGUILayout.Separator();
			
			GUILayout.Label("Set variable after battle", EditorStyles.boldLabel);
			if(GUILayout.Button("Add", GUILayout.Width(150)))
			{
				((BattleArena)target).AddVariableSet();
			}
			for(int i=0; i<((BattleArena)target).setVariableKey.Length; i++)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Separator();
				GUILayout.Label(i.ToString()+":");
				((BattleArena)target).setVariableKey[i] = EditorGUILayout.TextField(((BattleArena)target).setVariableKey[i]);
				GUILayout.Label(" = ");
				((BattleArena)target).setVariableValue[i] = EditorGUILayout.TextField(((BattleArena)target).setVariableValue[i]);
				EditorGUILayout.Separator();
				if(GUILayout.Button("Remove", GUILayout.Width(75)))
				{
					((BattleArena)target).RemoveVariableSet(i);
					return;
				}
				EditorGUILayout.EndHorizontal();
			}
			
			if(GUILayout.Button("Add Number", GUILayout.Width(150)))
			{
				((BattleArena)target).AddNumberVariableSet();
			}
			for(int i=0; i<((BattleArena)target).setNumberVarKey.Length; i++)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Separator();
				GUILayout.Label(i.ToString()+":");
				((BattleArena)target).setNumberVarKey[i] = EditorGUILayout.TextField(((BattleArena)target).setNumberVarKey[i]);
				((BattleArena)target).setNumberOperator[i] = (SimpleOperator)EditorGUILayout.EnumPopup(((BattleArena)target).setNumberOperator[i]);
				((BattleArena)target).setNumberVarValue[i] = EditorGUILayout.FloatField(((BattleArena)target).setNumberVarValue[i]);
				EditorGUILayout.Separator();
				if(GUILayout.Button("Remove", GUILayout.Width(75)))
				{
					((BattleArena)target).RemoveNumberVariableSet(i);
					return;
				}
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.Separator();
		}
		EditorGUILayout.EndVertical();
		
		
		EditorGUILayout.BeginVertical("box");
		fold4 = EditorGUILayout.Foldout(fold4, "Audio setup");
		if(fold4)
		{
			((BattleArena)target).startSound = (AudioClip)EditorGUILayout.ObjectField("Start sound", ((BattleArena)target).startSound, typeof(AudioClip), true);
			if(((BattleArena)target).startSound)
			{
				((BattleArena)target).startVolume = EditorGUILayout.FloatField("Volume", ((BattleArena)target).startVolume);
			}
			EditorGUILayout.Separator();
			
			((BattleArena)target).playBattleMusic = EditorGUILayout.Toggle("Battle music", ((BattleArena)target).playBattleMusic);
			if(((BattleArena)target).playBattleMusic)
			{
				((BattleArena)target).bmPlayType = (MusicPlayType)EditorGUILayout.EnumPopup("Play type", ((BattleArena)target).bmPlayType);
				if(((BattleArena)target).bmPlayType.Equals(MusicPlayType.STOP) || ((BattleArena)target).bmPlayType.Equals(MusicPlayType.FADE_OUT))
				{
					((BattleArena)target).bmPlayType = MusicPlayType.PLAY;
				}
				if(!((BattleArena)target).bmPlayType.Equals(MusicPlayType.FADE_OUT) && !((BattleArena)target).bmPlayType.Equals(MusicPlayType.STOP))
				{
					((BattleArena)target).battleMusic = EditorGUILayout.Popup("Music clip", ((BattleArena)target).battleMusic, DataHolder.Music().GetNameList(true));
				}
				if(!((BattleArena)target).bmPlayType.Equals(MusicPlayType.PLAY) && !((BattleArena)target).bmPlayType.Equals(MusicPlayType.STOP))
				{
					((BattleArena)target).bmFadeTime = EditorGUILayout.FloatField("Fade time (s)", ((BattleArena)target).bmFadeTime);
					((BattleArena)target).bmInterpolate = (EaseType)EditorGUILayout.EnumPopup("Interpolation", ((BattleArena)target).bmInterpolate);
				}
				EditorGUILayout.Separator();
			}
			
			((BattleArena)target).playVictoryMusic = EditorGUILayout.Toggle("Victory music", ((BattleArena)target).playVictoryMusic);
			if(((BattleArena)target).playVictoryMusic)
			{
				((BattleArena)target).vPlayType = (MusicPlayType)EditorGUILayout.EnumPopup("Play type", ((BattleArena)target).vPlayType);
				if(((BattleArena)target).vPlayType.Equals(MusicPlayType.STOP) || ((BattleArena)target).vPlayType.Equals(MusicPlayType.FADE_OUT))
				{
					((BattleArena)target).vPlayType = MusicPlayType.PLAY;
				}
				if(!((BattleArena)target).vPlayType.Equals(MusicPlayType.FADE_OUT) && !((BattleArena)target).vPlayType.Equals(MusicPlayType.STOP))
				{
					((BattleArena)target).victoryMusic = EditorGUILayout.Popup("Music clip", ((BattleArena)target).victoryMusic, DataHolder.Music().GetNameList(true));
				}
				if(!((BattleArena)target).vPlayType.Equals(MusicPlayType.PLAY) && !((BattleArena)target).vPlayType.Equals(MusicPlayType.STOP))
				{
					((BattleArena)target).vFadeTime = EditorGUILayout.FloatField("Fade time (s)", ((BattleArena)target).vFadeTime);
					((BattleArena)target).vInterpolate = (EaseType)EditorGUILayout.EnumPopup("Interpolation", ((BattleArena)target).vInterpolate);
				}
				EditorGUILayout.Separator();
			}
			
			((BattleArena)target).restoreLastMusic = EditorGUILayout.Toggle("Restore music", ((BattleArena)target).restoreLastMusic);
			if(((BattleArena)target).restoreLastMusic)
			{
				((BattleArena)target).lPlayType = (MusicPlayType)EditorGUILayout.EnumPopup("Play type", ((BattleArena)target).lPlayType);
				if(((BattleArena)target).lPlayType.Equals(MusicPlayType.STOP) || ((BattleArena)target).lPlayType.Equals(MusicPlayType.FADE_OUT))
				{
					((BattleArena)target).lPlayType = MusicPlayType.PLAY;
				}
				if(!((BattleArena)target).lPlayType.Equals(MusicPlayType.PLAY) && !((BattleArena)target).lPlayType.Equals(MusicPlayType.STOP))
				{
					((BattleArena)target).lFadeTime = EditorGUILayout.FloatField("Fade time (s)", ((BattleArena)target).lFadeTime);
					((BattleArena)target).lInterpolate = (EaseType)EditorGUILayout.EnumPopup("Interpolation", ((BattleArena)target).lInterpolate);
				}
				EditorGUILayout.Separator();
			}
			EditorGUILayout.Separator();
		}
		EditorGUILayout.EndVertical();
		
		
		EditorGUILayout.BeginVertical("box");
		fold2 = EditorGUILayout.Foldout(fold2, "Start cam setup");
		if(fold2)
		{
			((BattleArena)target).baseCamPos = EditorGUILayout.Popup("Base cam position", ((BattleArena)target).baseCamPos, DataHolder.CameraPositions().GetNameList(true));
			((BattleArena)target).startWait = EditorGUILayout.FloatField("Wait", ((BattleArena)target).startWait);
			((BattleArena)target).startFadeScreen = EditorGUILayout.Toggle("Fade screen", ((BattleArena)target).startFadeScreen);
			if(((BattleArena)target).startFadeScreen)
			{
				((BattleArena)target).startFadeOutTime = EditorGUILayout.FloatField("Fade out time", ((BattleArena)target).startFadeOutTime);
				((BattleArena)target).startFadeInTime = EditorGUILayout.FloatField("Fade in time", ((BattleArena)target).startFadeInTime);
				((BattleArena)target).startFadeInterpolation = (EaseType)EditorGUILayout.EnumPopup("Interpolation", ((BattleArena)target).startFadeInterpolation);
				
				EditorGUILayout.BeginHorizontal();
				((BattleArena)target).startFadeAlpha = EditorGUILayout.Toggle("Alpha from/to", ((BattleArena)target).startFadeAlpha);
				if(((BattleArena)target).startFadeAlpha)
				{
					((BattleArena)target).startFadeAStart = EditorGUILayout.FloatField(((BattleArena)target).startFadeAStart);
					((BattleArena)target).startFadeAEnd = EditorGUILayout.FloatField(((BattleArena)target).startFadeAEnd);
				}
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				((BattleArena)target).startFadeRed = EditorGUILayout.Toggle("Red from/to", ((BattleArena)target).startFadeRed);
				if(((BattleArena)target).startFadeRed)
				{
					((BattleArena)target).startFadeRStart = EditorGUILayout.FloatField(((BattleArena)target).startFadeRStart);
					((BattleArena)target).startFadeREnd = EditorGUILayout.FloatField(((BattleArena)target).startFadeREnd);
				}
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				((BattleArena)target).startFadeGreen = EditorGUILayout.Toggle("Green from/to", ((BattleArena)target).startFadeGreen);
				if(((BattleArena)target).startFadeGreen)
				{
					((BattleArena)target).startFadeGStart = EditorGUILayout.FloatField(((BattleArena)target).startFadeGStart);
					((BattleArena)target).startFadeGEnd = EditorGUILayout.FloatField(((BattleArena)target).startFadeGEnd);
				}
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				((BattleArena)target).startFadeBlue = EditorGUILayout.Toggle("Blue from/to", ((BattleArena)target).startFadeBlue);
				if(((BattleArena)target).startFadeBlue)
				{
					((BattleArena)target).startFadeBStart = EditorGUILayout.FloatField(((BattleArena)target).startFadeBStart);
					((BattleArena)target).startFadeBEnd = EditorGUILayout.FloatField(((BattleArena)target).startFadeBEnd);
				}
				EditorGUILayout.EndHorizontal();
			}
			((BattleArena)target).fadeCamPos = EditorGUILayout.Toggle("Fade cam pos", ((BattleArena)target).fadeCamPos);
			if(((BattleArena)target).fadeCamPos)
			{
				EditorGUILayout.BeginHorizontal();
				((BattleArena)target).setStartCamPos = EditorGUILayout.Toggle("Set start pos", ((BattleArena)target).setStartCamPos);
				if(((BattleArena)target).setStartCamPos)
				{
					((BattleArena)target).startCamPos = EditorGUILayout.Popup(((BattleArena)target).startCamPos, DataHolder.CameraPositions().GetNameList(true));
				}
				EditorGUILayout.EndHorizontal();
				((BattleArena)target).startPosTime = EditorGUILayout.FloatField("Time", ((BattleArena)target).startPosTime);
				((BattleArena)target).startCamInterpolation = (EaseType)EditorGUILayout.EnumPopup("Interpolation", ((BattleArena)target).startCamInterpolation);
			}
			EditorGUILayout.Separator();
		}
		EditorGUILayout.EndVertical();
		
		
		EditorGUILayout.BeginVertical("box");
		fold3 = EditorGUILayout.Foldout(fold3, "End cam setup");
		if(fold3)
		{
			((BattleArena)target).endWait = EditorGUILayout.FloatField("Wait", ((BattleArena)target).endWait);
			((BattleArena)target).endFadeScreen = EditorGUILayout.Toggle("Fade screen", ((BattleArena)target).endFadeScreen);
			if(((BattleArena)target).endFadeScreen)
			{
				((BattleArena)target).endFadeOutTime = EditorGUILayout.FloatField("Fade out time", ((BattleArena)target).endFadeOutTime);
				((BattleArena)target).endFadeInTime = EditorGUILayout.FloatField("Fade in time", ((BattleArena)target).endFadeInTime);
				((BattleArena)target).endFadeInterpolation = (EaseType)EditorGUILayout.EnumPopup("Interpolation", ((BattleArena)target).endFadeInterpolation);
				
				EditorGUILayout.BeginHorizontal();
				((BattleArena)target).endFadeAlpha = EditorGUILayout.Toggle("Alpha from/to", ((BattleArena)target).endFadeAlpha);
				if(((BattleArena)target).endFadeAlpha)
				{
					((BattleArena)target).endFadeAStart = EditorGUILayout.FloatField(((BattleArena)target).endFadeAStart);
					((BattleArena)target).endFadeAEnd = EditorGUILayout.FloatField(((BattleArena)target).endFadeAEnd);
				}
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				((BattleArena)target).endFadeRed = EditorGUILayout.Toggle("Red from/to", ((BattleArena)target).endFadeRed);
				if(((BattleArena)target).endFadeRed)
				{
					((BattleArena)target).endFadeRStart = EditorGUILayout.FloatField(((BattleArena)target).endFadeRStart);
					((BattleArena)target).endFadeREnd = EditorGUILayout.FloatField(((BattleArena)target).endFadeREnd);
				}
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				((BattleArena)target).endFadeGreen = EditorGUILayout.Toggle("Green from/to", ((BattleArena)target).endFadeGreen);
				if(((BattleArena)target).endFadeGreen)
				{
					((BattleArena)target).endFadeGStart = EditorGUILayout.FloatField(((BattleArena)target).endFadeGStart);
					((BattleArena)target).endFadeGEnd = EditorGUILayout.FloatField(((BattleArena)target).endFadeGEnd);
				}
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				((BattleArena)target).endFadeBlue = EditorGUILayout.Toggle("Blue from/to", ((BattleArena)target).endFadeBlue);
				if(((BattleArena)target).endFadeBlue)
				{
					((BattleArena)target).endFadeBStart = EditorGUILayout.FloatField(((BattleArena)target).endFadeBStart);
					((BattleArena)target).endFadeBEnd = EditorGUILayout.FloatField(((BattleArena)target).endFadeBEnd);
				}
				EditorGUILayout.EndHorizontal();
			}
			((BattleArena)target).fadeEndCamPos = EditorGUILayout.Toggle("Fade cam pos", ((BattleArena)target).fadeEndCamPos);
			if(((BattleArena)target).fadeEndCamPos)
			{
				EditorGUILayout.BeginHorizontal();
				((BattleArena)target).setEndCamPos = EditorGUILayout.Toggle("Set start pos", ((BattleArena)target).setEndCamPos);
				if(((BattleArena)target).setEndCamPos)
				{
					((BattleArena)target).endCamPos = EditorGUILayout.Popup(((BattleArena)target).endCamPos, DataHolder.CameraPositions().GetNameList(true));
				}
				EditorGUILayout.EndHorizontal();
				((BattleArena)target).endPosTime = EditorGUILayout.FloatField("Time", ((BattleArena)target).endPosTime);
				((BattleArena)target).endCamInterpolation = (EaseType)EditorGUILayout.EnumPopup("Interpolation", ((BattleArena)target).endCamInterpolation);
			}
			EditorGUILayout.Separator();
		}
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndVertical();
		
		if(GUI.changed)
            EditorUtility.SetDirty(target);
	}
}