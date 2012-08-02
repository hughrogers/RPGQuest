
using UnityEditor;
using UnityEngine;

public class BattleSystemTab : EditorTab
{
	private BattleSystemWindow pw;
	
	public BattleSystemTab(BattleSystemWindow pw) : base()
	{
		this.pw = pw;
	}
	
	public void ShowTab()
	{
		int count = DataHolder.StatusValueCount;
		EditorGUILayout.BeginVertical();
		SP1 = EditorGUILayout.BeginScrollView(SP1);
		
		this.Separate();
		EditorGUILayout.BeginVertical("box");
		fold1 = EditorGUILayout.Foldout(fold1, "System settings");
		if(fold1)
		{
			DataHolder.BattleSystem().type = (BattleSystemType)this.EnumToolbar("System type", 
					(int)DataHolder.BattleSystem().type, typeof(BattleSystemType), (int)(pw.mWidth*1.5f));
			EditorGUILayout.Separator();
			
			if(DataHolder.BattleSystem().IsRealTime())
			{
				DataHolder.BattleSystem().enemyCounting = EnemyCounting.NONE;
			}
			else 
			{
				DataHolder.BattleSystem().enemyCounting = (EnemyCounting)this.EnumToolbar("Enemy counting", 
						(int)DataHolder.BattleSystem().enemyCounting, typeof(EnemyCounting), (int)(pw.mWidth*1.5f));
				EditorGUILayout.Separator();
			}
			
			if(DataHolder.BattleSystem().IsTurnBased())
			{
				DataHolder.BattleSystem().turnCalc = EditorGUILayout.Popup("Turn calculation", DataHolder.BattleSystem().turnCalc, 
						DataHolder.Formulas().GetNameList(true), GUILayout.Width(pw.mWidth*1.5f));
				DataHolder.BattleSystem().activeCommand = EditorGUILayout.Toggle("Active command", DataHolder.BattleSystem().activeCommand, GUILayout.Width(pw.mWidth));
				
				if(DataHolder.BattleSystem().activeCommand)
				{
					this.Separate();
					DataHolder.BattleSystem().dynamicCombat = EditorGUILayout.Toggle("Dynamic combat", 
							DataHolder.BattleSystem().dynamicCombat, GUILayout.Width(pw.mWidth));
					if(DataHolder.BattleSystem().dynamicCombat)
					{
						DataHolder.BattleSystem().minTimeBetween = EditorGUILayout.FloatField("Time between actions",
								DataHolder.BattleSystem().minTimeBetween, GUILayout.Width(pw.mWidth));
						if(DataHolder.BattleSystem().minTimeBetween < 0)
						{
							DataHolder.BattleSystem().minTimeBetween = 0;
						}
						DataHolder.BattleSystem().playDamageAnim = EditorGUILayout.Toggle("Play damage animation", 
								DataHolder.BattleSystem().playDamageAnim, GUILayout.Width(pw.mWidth));
						DataHolder.BattleSystem().blockAutoAttackMenu = EditorGUILayout.Toggle("Menu block autoatk", 
								DataHolder.BattleSystem().blockAutoAttackMenu, GUILayout.Width(pw.mWidth));
					}
					else DataHolder.BattleSystem().playDamageAnim = false;
				}
				else DataHolder.BattleSystem().dynamicCombat = false;
			}
			else if(DataHolder.BattleSystem().IsActiveTime())
			{
				DataHolder.BattleSystem().turnCalc = EditorGUILayout.Popup("Time calculation", DataHolder.BattleSystem().turnCalc, 
						DataHolder.Formulas().GetNameList(true), GUILayout.Width(pw.mWidth*1.5f));
				DataHolder.BattleSystem().startTimeCalc = EditorGUILayout.Popup("Start time calc.", DataHolder.BattleSystem().startTimeCalc, 
						DataHolder.Formulas().GetNameList(true), GUILayout.Width(pw.mWidth*1.5f));
				EditorGUILayout.Separator();
				
				DataHolder.BattleSystem().actionPause = EditorGUILayout.Toggle("Pause on menu", 
						DataHolder.BattleSystem().actionPause, GUILayout.Width(pw.mWidth));
				
				DataHolder.BattleSystem().maxTimebar = EditorGUILayout.IntField("Max timebar", 
					DataHolder.BattleSystem().maxTimebar, GUILayout.Width(pw.mWidth));
				DataHolder.BattleSystem().actionBorder = EditorGUILayout.IntField("Action border", 
						DataHolder.BattleSystem().actionBorder, GUILayout.Width(pw.mWidth));
				DataHolder.BattleSystem().menuBorder = EditorGUILayout.IntField("Menu border", 
					DataHolder.BattleSystem().menuBorder, GUILayout.Width(pw.mWidth));
				
				if(DataHolder.BattleSystem().menuBorder > DataHolder.BattleSystem().actionBorder)
				{
					DataHolder.BattleSystem().menuBorder = DataHolder.BattleSystem().actionBorder;
				}
				if(DataHolder.BattleSystem().actionBorder > DataHolder.BattleSystem().maxTimebar)
				{
					DataHolder.BattleSystem().actionBorder = DataHolder.BattleSystem().maxTimebar;
				}
				
				DataHolder.BattleSystem().enableMultiChoice = EditorGUILayout.Toggle("Multi choice", 
						DataHolder.BattleSystem().enableMultiChoice, GUILayout.Width(pw.mWidth));
				if(DataHolder.BattleSystem().enableMultiChoice)
				{
					DataHolder.BattleSystem().useAllActions = EditorGUILayout.Toggle("Use all actions", 
						DataHolder.BattleSystem().useAllActions, GUILayout.Width(pw.mWidth));
					DataHolder.BattleSystem().useTimebarAction = (UseTimebarAction)EditorGUILayout.EnumPopup("Use actions", 
							DataHolder.BattleSystem().useTimebarAction, GUILayout.Width(pw.mWidth));
				}
				else
				{
					DataHolder.BattleSystem().useAllActions = false;
					DataHolder.BattleSystem().useTimebarAction = UseTimebarAction.ACTION_BORDER;
				}
				
				EditorGUILayout.Separator();
				DataHolder.BattleSystem().attackEndTurn = EditorGUILayout.Toggle("Attack end turn", DataHolder.BattleSystem().attackEndTurn, GUILayout.Width(pw.mWidth));
				if(!DataHolder.BattleSystem().attackEndTurn)
				{
					DataHolder.BattleSystem().attackTimebarUse = EditorGUILayout.FloatField("Timebar use", DataHolder.BattleSystem().attackTimebarUse, GUILayout.Width(pw.mWidth));
					if(DataHolder.BattleSystem().attackTimebarUse <= 0) DataHolder.BattleSystem().attackTimebarUse = 1;
				}
				EditorGUILayout.Separator();
				DataHolder.BattleSystem().itemEndTurn = EditorGUILayout.Toggle("Item end turn", DataHolder.BattleSystem().itemEndTurn, GUILayout.Width(pw.mWidth));
				if(!DataHolder.BattleSystem().itemEndTurn)
				{
					DataHolder.BattleSystem().itemTimebarUse = EditorGUILayout.FloatField("Timebar use", DataHolder.BattleSystem().itemTimebarUse, GUILayout.Width(pw.mWidth));
					if(DataHolder.BattleSystem().itemTimebarUse <= 0) DataHolder.BattleSystem().itemTimebarUse = 1;
				}
				EditorGUILayout.Separator();
				DataHolder.BattleSystem().defendEndTurn = EditorGUILayout.Toggle("Defend end turn", DataHolder.BattleSystem().defendEndTurn, GUILayout.Width(pw.mWidth));
				if(!DataHolder.BattleSystem().defendEndTurn)
				{
					DataHolder.BattleSystem().defendTimebarUse = EditorGUILayout.FloatField("Timebar use", DataHolder.BattleSystem().defendTimebarUse, GUILayout.Width(pw.mWidth));
					if(DataHolder.BattleSystem().defendTimebarUse <= 0) DataHolder.BattleSystem().defendTimebarUse = 1;
				}
				EditorGUILayout.Separator();
				DataHolder.BattleSystem().escapeEndTurn = EditorGUILayout.Toggle("Escape end turn", DataHolder.BattleSystem().escapeEndTurn, GUILayout.Width(pw.mWidth));
				if(!DataHolder.BattleSystem().escapeEndTurn)
				{
					DataHolder.BattleSystem().escapeTimebarUse = EditorGUILayout.FloatField("Timebar use", DataHolder.BattleSystem().escapeTimebarUse, GUILayout.Width(pw.mWidth));
					if(DataHolder.BattleSystem().escapeTimebarUse <= 0) DataHolder.BattleSystem().escapeTimebarUse = 1;
				}
				
				this.Separate();
				DataHolder.BattleSystem().dynamicCombat = EditorGUILayout.Toggle("Dynamic combat", 
					DataHolder.BattleSystem().dynamicCombat, GUILayout.Width(pw.mWidth));
				if(DataHolder.BattleSystem().dynamicCombat)
				{
					DataHolder.BattleSystem().minTimeBetween = EditorGUILayout.FloatField("Time between actions",
							DataHolder.BattleSystem().minTimeBetween, GUILayout.Width(pw.mWidth));
					if(DataHolder.BattleSystem().minTimeBetween < 0)
					{
						DataHolder.BattleSystem().minTimeBetween = 0;
					}
					DataHolder.BattleSystem().playDamageAnim = EditorGUILayout.Toggle("Play damage animation", 
							DataHolder.BattleSystem().playDamageAnim, GUILayout.Width(pw.mWidth));
					DataHolder.BattleSystem().blockAutoAttackMenu = EditorGUILayout.Toggle("Menu block autoatk", 
							DataHolder.BattleSystem().blockAutoAttackMenu, GUILayout.Width(pw.mWidth));
				}
				else DataHolder.BattleSystem().playDamageAnim = false;
			}
			else if(DataHolder.BattleSystem().IsRealTime())
			{
				DataHolder.BattleSystem().dynamicCombat = false;
				DataHolder.BattleSystem().battleRange = EditorGUILayout.FloatField("Battle range",
						DataHolder.BattleSystem().battleRange, GUILayout.Width(pw.mWidth));
				DataHolder.BattleSystem().aiRange = EditorGUILayout.FloatField("AI range",
						DataHolder.BattleSystem().aiRange, GUILayout.Width(pw.mWidth));
				DataHolder.BattleSystem().aiRecheckTime = EditorGUILayout.FloatField("AI recheck time",
						DataHolder.BattleSystem().aiRecheckTime, GUILayout.Width(pw.mWidth));
				EditorGUILayout.Separator();
				
				DataHolder.BattleSystem().actionPause = EditorGUILayout.Toggle("Pause on menu", 
						DataHolder.BattleSystem().actionPause, GUILayout.Width(pw.mWidth));
				if(DataHolder.BattleSystem().actionPause)
				{
					DataHolder.BattleSystem().blockControlMenu = true;
					DataHolder.BattleSystem().freezeAction = EditorGUILayout.Toggle("Freeze action",
							DataHolder.BattleSystem().freezeAction, GUILayout.Width(pw.mWidth));
				}
				else
				{
					DataHolder.BattleSystem().freezeAction = false;
					DataHolder.BattleSystem().blockControlMenu = EditorGUILayout.Toggle("Menu no control",
							DataHolder.BattleSystem().blockControlMenu, GUILayout.Width(pw.mWidth));
				}
				DataHolder.BattleSystem().blockControlAction = EditorGUILayout.Toggle("Action no control",
						DataHolder.BattleSystem().blockControlAction, GUILayout.Width(pw.mWidth));
				DataHolder.BattleSystem().blockMSE = EditorGUILayout.Toggle("Block MSE",
						DataHolder.BattleSystem().blockMSE, GUILayout.Width(pw.mWidth));
				DataHolder.BattleSystem().playDamageAnim = EditorGUILayout.Toggle("Play damage animation", 
						DataHolder.BattleSystem().playDamageAnim, GUILayout.Width(pw.mWidth));
			}
			this.Separate();
			
			DataHolder.BattleSystem().defendFormula = EditorGUILayout.Popup("Defend (%)", DataHolder.BattleSystem().defendFormula, 
					DataHolder.Formulas().GetNameList(true), GUILayout.Width(pw.mWidth*1.5f));
			if(!DataHolder.BattleSystem().IsRealTime())
			{
				DataHolder.BattleSystem().escapeFormula = EditorGUILayout.Popup("Escape (%)", DataHolder.BattleSystem().escapeFormula, 
						DataHolder.Formulas().GetNameList(true), GUILayout.Width(pw.mWidth*1.5f));
			}
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		
		if(BattleSystemType.REALTIME.Equals(DataHolder.BattleSystem().type))
		{
			EditorGUILayout.BeginVertical("box");
			fold7 = EditorGUILayout.Foldout(fold7, "Control settings");
			if(fold7)
			{
				GUILayout.Label("Key settings", EditorStyles.boldLabel);
				DataHolder.BattleControl().attackKey = EditorGUILayout.TextField("Attack key",
						DataHolder.BattleControl().attackKey, GUILayout.Width(pw.mWidth*1.2f));
				if(DataHolder.BattleControl().usePartyTarget)
				{
					DataHolder.BattleControl().attackPartyTarget = EditorGUILayout.Toggle("Attack party target",
							DataHolder.BattleControl().attackPartyTarget, GUILayout.Width(pw.mWidth));
					if(DataHolder.BattleControl().attackPartyTarget)
					{
						DataHolder.BattleControl().aptRange = EditorGUILayout.Toggle("Only in range",
								DataHolder.BattleControl().aptRange, GUILayout.Width(pw.mWidth));
					}
					else DataHolder.BattleControl().aptRange = false;
				}
				DataHolder.BattleControl().battleMenuKey = EditorGUILayout.TextField("Battle menu key",
						DataHolder.BattleControl().battleMenuKey, GUILayout.Width(pw.mWidth*1.2f));
				DataHolder.BattleControl().skillMenuKey = EditorGUILayout.TextField("Skill menu key",
						DataHolder.BattleControl().skillMenuKey, GUILayout.Width(pw.mWidth*1.2f));
				DataHolder.BattleControl().itemMenuKey = EditorGUILayout.TextField("Item menu key",
						DataHolder.BattleControl().itemMenuKey, GUILayout.Width(pw.mWidth*1.2f));
				DataHolder.BattleControl().keyCloses = EditorGUILayout.Toggle("2nd press closes",
						DataHolder.BattleControl().keyCloses, GUILayout.Width(pw.mWidth));
				DataHolder.BattleControl().allowMenuSwitch = EditorGUILayout.Toggle("Allow switching",
						DataHolder.BattleControl().allowMenuSwitch, GUILayout.Width(pw.mWidth));
				
				EditorGUILayout.Separator();
				GUILayout.Label("Party target settings", EditorStyles.boldLabel);
				DataHolder.BattleControl().usePartyTarget = EditorGUILayout.Toggle("Use party target",
						DataHolder.BattleControl().usePartyTarget, GUILayout.Width(pw.mWidth));
				if(DataHolder.BattleControl().usePartyTarget)
				{
					DataHolder.BattleControl().onlyInBattleRange = EditorGUILayout.Toggle("In battle range",
							DataHolder.BattleControl().onlyInBattleRange, GUILayout.Width(pw.mWidth));
					DataHolder.BattleControl().nextTargetKey = EditorGUILayout.TextField("Next target key",
							DataHolder.BattleControl().nextTargetKey, GUILayout.Width(pw.mWidth*1.2f));
					DataHolder.BattleControl().previousTargetKey = EditorGUILayout.TextField("Previous target key",
							DataHolder.BattleControl().previousTargetKey, GUILayout.Width(pw.mWidth*1.2f));
					DataHolder.BattleControl().nearestTargetKey = EditorGUILayout.TextField("Nearest target key",
							DataHolder.BattleControl().nearestTargetKey, GUILayout.Width(pw.mWidth*1.2f));
					if(!DataHolder.BattleControl().autoSelectTarget)
					{
						DataHolder.BattleControl().clearTargetKey = EditorGUILayout.TextField("Clear target key",
								DataHolder.BattleControl().clearTargetKey, GUILayout.Width(pw.mWidth*1.2f));
					}
					DataHolder.BattleControl().mouseTouch = EditorHelper.MouseTouchControlSettings(
							DataHolder.BattleControl().mouseTouch, true);
					DataHolder.BattleControl().allowTargetRemove = EditorGUILayout.Toggle("Allow remove",
							DataHolder.BattleControl().allowTargetRemove, GUILayout.Width(pw.mWidth));
					DataHolder.BattleControl().ptNoActionOnly = EditorGUILayout.Toggle("No action only",
							DataHolder.BattleControl().ptNoActionOnly, GUILayout.Width(pw.mWidth));
					DataHolder.BattleControl().autoSelectTarget = EditorGUILayout.Toggle("Auto select",
							DataHolder.BattleControl().autoSelectTarget, GUILayout.Width(pw.mWidth));
					DataHolder.BattleControl().autoUseOnTarget = EditorGUILayout.Toggle("Auto use on target",
							DataHolder.BattleControl().autoUseOnTarget, GUILayout.Width(pw.mWidth));
					EditorGUILayout.Separator();
					
					GUILayout.Label("Character auto attack on party target settings", EditorStyles.boldLabel);
					DataHolder.BattleControl().autoAttackTarget = EditorGUILayout.Toggle("Auto attack target",
							DataHolder.BattleControl().autoAttackTarget, GUILayout.Width(pw.mWidth));
					if(DataHolder.BattleControl().autoAttackTarget)
					{
						DataHolder.BattleControl().aaPlayerOnly = EditorGUILayout.Toggle("Player only",
								DataHolder.BattleControl().aaPlayerOnly, GUILayout.Width(pw.mWidth));
						DataHolder.BattleSystem().blockAutoAttackMenu = EditorGUILayout.Toggle("Menu block autoatk", 
								DataHolder.BattleSystem().blockAutoAttackMenu, GUILayout.Width(pw.mWidth));
					}
					EditorGUILayout.Separator();
					
					GUILayout.Label("Party target cursor settings", EditorStyles.boldLabel);
					DataHolder.BattleControl().useTargetCursor = EditorGUILayout.Toggle("Use cursor",
							DataHolder.BattleControl().useTargetCursor, GUILayout.Width(pw.mWidth));
					if(DataHolder.BattleControl().useTargetCursor)
					{
						if(DataHolder.BattleControl().cursorInstance == null && 
							"" != DataHolder.BattleControl().cursorPrefabName)
						{
							DataHolder.BattleControl().cursorInstance = (GameObject)Resources.Load(
									BattleSystemData.PREFAB_PATH+
									DataHolder.BattleControl().cursorPrefabName, typeof(GameObject));
						}
						DataHolder.BattleControl().cursorInstance = (GameObject)EditorGUILayout.ObjectField(
								"Cursor prefab", DataHolder.BattleControl().cursorInstance, 
								typeof(GameObject), false, GUILayout.Width(pw.mWidth*2));
						if(DataHolder.BattleControl().cursorInstance != null)
						{
							DataHolder.BattleControl().cursorPrefabName = DataHolder.BattleControl().cursorInstance.name;
						}
						else DataHolder.BattleControl().cursorPrefabName = "";
						
						DataHolder.BattleControl().cursorChildName = EditorGUILayout.TextField("Path to child", 
								DataHolder.BattleControl().cursorChildName, GUILayout.Width(pw.mWidth*2));
						
						DataHolder.BattleControl().cursorOffset = EditorGUILayout.Vector3Field("Cursor offset", 
								DataHolder.BattleControl().cursorOffset, GUILayout.Width(pw.mWidth));
					}
				}
				
				EditorGUILayout.Separator();
			}
			EditorGUILayout.EndVertical();
		}
		else
		{
			EditorGUILayout.BeginVertical("box");
			fold8 = EditorGUILayout.Foldout(fold8, "Default battle spot settings");
			if(fold8)
			{
				DataHolder.BattleSystem().spotOnGround = EditorGUILayout.Toggle("Place on ground", 
						DataHolder.BattleSystem().spotOnGround, GUILayout.Width(pw.mWidth));
				if(DataHolder.BattleSystem().spotOnGround)
				{
					DataHolder.BattleSystem().spotDistance = EditorGUILayout.FloatField("Distance", 
							DataHolder.BattleSystem().spotDistance, GUILayout.Width(pw.mWidth));
					DataHolder.BattleSystem().spotLayer = EditorGUILayout.LayerField("Layer", 
							DataHolder.BattleSystem().spotLayer, GUILayout.Width(pw.mWidth));
					DataHolder.BattleSystem().spotOffset = EditorGUILayout.Vector3Field("Offset", 
							DataHolder.BattleSystem().spotOffset, GUILayout.Width(pw.mWidth));
				}
				EditorGUILayout.Separator();
				
				if(DataHolder.BattleSystem().partyAdvantage.enabled)
				{
					DataHolder.BattleSystem().enablePASpots = EditorGUILayout.Toggle("Party adv. spots", 
							DataHolder.BattleSystem().enablePASpots, GUILayout.Width(pw.mWidth));
				}
				else DataHolder.BattleSystem().enablePASpots = false;
				if(DataHolder.BattleSystem().enemiesAdvantage.enabled)
				{
					DataHolder.BattleSystem().enableEASpots = EditorGUILayout.Toggle("Enemies adv. spots", 
							DataHolder.BattleSystem().enableEASpots, GUILayout.Width(pw.mWidth));
				}
				else DataHolder.BattleSystem().enableEASpots = false;
				EditorGUILayout.Separator();
				
				if(DataHolder.BattleSystem().partySpot.Length != DataHolder.GameSettings().maxBattleParty)
				{
					Vector3[] tmpSpots = DataHolder.BattleSystem().partySpot;
					DataHolder.BattleSystem().partySpot = new Vector3[DataHolder.GameSettings().maxBattleParty];
					System.Array.Copy(tmpSpots, DataHolder.BattleSystem().partySpot, 
							Mathf.Min(tmpSpots.Length, DataHolder.GameSettings().maxBattleParty));
					tmpSpots = DataHolder.BattleSystem().partySpotPA;
					DataHolder.BattleSystem().partySpotPA = new Vector3[DataHolder.GameSettings().maxBattleParty];
					System.Array.Copy(tmpSpots, DataHolder.BattleSystem().partySpotPA, 
							Mathf.Min(tmpSpots.Length, DataHolder.GameSettings().maxBattleParty));
					tmpSpots = DataHolder.BattleSystem().partySpotEA;
					DataHolder.BattleSystem().partySpotEA = new Vector3[DataHolder.GameSettings().maxBattleParty];
					System.Array.Copy(tmpSpots, DataHolder.BattleSystem().partySpotEA, 
							Mathf.Min(tmpSpots.Length, DataHolder.GameSettings().maxBattleParty));
				}
				GUILayout.Label("Party spots (in local space of the BattleArena)", EditorStyles.boldLabel);
				for(int i=0; i<DataHolder.BattleSystem().partySpot.Length; i++)
				{
					EditorGUILayout.BeginVertical("box");
					EditorGUILayout.BeginHorizontal();
					GUILayout.Label("Spot "+i, EditorStyles.boldLabel);
					EditorGUILayout.Separator();
					DataHolder.BattleSystem().partySpot[i] = EditorGUILayout.Vector3Field("Standard battle", 
							DataHolder.BattleSystem().partySpot[i], GUILayout.Width(pw.mWidth));
					
					if(DataHolder.BattleSystem().enablePASpots)
					{
						EditorGUILayout.Separator();
						DataHolder.BattleSystem().partySpotPA[i] = EditorGUILayout.Vector3Field("Party advantage", 
								DataHolder.BattleSystem().partySpotPA[i], GUILayout.Width(pw.mWidth));
					}
					if(DataHolder.BattleSystem().enableEASpots)
					{
						EditorGUILayout.Separator();
						DataHolder.BattleSystem().partySpotEA[i] = EditorGUILayout.Vector3Field("Enemies advantage", 
								DataHolder.BattleSystem().partySpotEA[i], GUILayout.Width(pw.mWidth));
					}
					GUILayout.FlexibleSpace();
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.Separator();
					EditorGUILayout.EndVertical();
				}
				EditorGUILayout.Separator();
				
				GUILayout.Label("Enemy spots (in local space of the BattleArena)", EditorStyles.boldLabel);
				if(GUILayout.Button("Add", GUILayout.Width(100)))
				{
					DataHolder.BattleSystem().enemySpot = ArrayHelper.Add(Vector3.zero, 
							DataHolder.BattleSystem().enemySpot);
					DataHolder.BattleSystem().enemySpotPA = ArrayHelper.Add(Vector3.zero, 
							DataHolder.BattleSystem().enemySpotPA);
					DataHolder.BattleSystem().enemySpotEA = ArrayHelper.Add(Vector3.zero, 
							DataHolder.BattleSystem().enemySpotEA);
				}
				for(int i=0; i<DataHolder.BattleSystem().enemySpot.Length; i++)
				{
					EditorGUILayout.BeginVertical("box");
					EditorGUILayout.BeginHorizontal();
					GUILayout.Label("Spot "+i, EditorStyles.boldLabel);
					EditorGUILayout.Separator();
					DataHolder.BattleSystem().enemySpot[i] = EditorGUILayout.Vector3Field("Standard battle", 
							DataHolder.BattleSystem().enemySpot[i], GUILayout.Width(pw.mWidth));
					if(DataHolder.BattleSystem().enablePASpots)
					{
						EditorGUILayout.Separator();
						DataHolder.BattleSystem().enemySpotPA[i] = EditorGUILayout.Vector3Field("Party advantage", 
								DataHolder.BattleSystem().enemySpotPA[i], GUILayout.Width(pw.mWidth));
					}
					if(DataHolder.BattleSystem().enableEASpots)
					{
						EditorGUILayout.Separator();
						DataHolder.BattleSystem().enemySpotEA[i] = EditorGUILayout.Vector3Field("Enemies advantage", 
								DataHolder.BattleSystem().enemySpotEA[i], GUILayout.Width(pw.mWidth));
					}
					EditorGUILayout.Separator();
					
					if(GUILayout.Button("Remove", GUILayout.Width(100)))
					{
						DataHolder.BattleSystem().enemySpot = ArrayHelper.Remove(i, 
								DataHolder.BattleSystem().enemySpot);
						DataHolder.BattleSystem().enemySpotPA = ArrayHelper.Remove(i, 
								DataHolder.BattleSystem().enemySpotPA);
						DataHolder.BattleSystem().enemySpotEA = ArrayHelper.Remove(i, 
								DataHolder.BattleSystem().enemySpotEA);
						break;
					}
					GUILayout.FlexibleSpace();
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.Separator();
					EditorGUILayout.EndVertical();
				}
				
				EditorGUILayout.Separator();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold9 = EditorGUILayout.Foldout(fold9, "Party advantage settings");
			if(fold9)
			{
				EditorHelper.BattleAdvantage(ref DataHolder.BattleSystem().partyAdvantage);
				EditorGUILayout.Separator();
			}
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical("box");
			fold10 = EditorGUILayout.Foldout(fold10, "Enemy advantage settings");
			if(fold10)
			{
				EditorHelper.BattleAdvantage(ref DataHolder.BattleSystem().enemiesAdvantage);
				EditorGUILayout.Separator();
			}
			EditorGUILayout.EndVertical();
		}
		
		EditorGUILayout.BeginVertical("box");
		fold2 = EditorGUILayout.Foldout(fold2, "Turn bonuses");
		if(fold2)
		{
			DataHolder.BattleSystem().turnBonuses = EditorGUILayout.Toggle("Use bonuses", DataHolder.BattleSystem().turnBonuses, GUILayout.Width(pw.mWidth));
			if(DataHolder.BattleSystem().turnBonuses)
			{
				if(DataHolder.BattleSystem().statusBonus == null)
				{
					DataHolder.BattleSystem().statusBonus = new int[count];
				}
				for(int i=0; i<DataHolder.BattleSystem().statusBonus.Length; i++)
				{
					if(DataHolder.StatusValue(i).IsConsumable())
					{
						DataHolder.BattleSystem().statusBonus[i] = EditorGUILayout.IntField(DataHolder.StatusValues().GetName(i), 
								DataHolder.BattleSystem().statusBonus[i], GUILayout.Width(pw.mWidth));
					}
					else
					{
						DataHolder.BattleSystem().statusBonus[i] = 0;
					}
				}
			}
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical("box");
		fold3 = EditorGUILayout.Foldout(fold3, "Revive after battle");
		if(fold3)
		{
			DataHolder.BattleSystem().reviveAfterBattle = EditorGUILayout.Toggle("Revive", DataHolder.BattleSystem().reviveAfterBattle, GUILayout.Width(pw.mWidth));
			if(DataHolder.BattleSystem().reviveAfterBattle)
			{
				if(DataHolder.BattleSystem().reviveSetStatus == null)
				{
					DataHolder.BattleSystem().reviveSetStatus = new bool[count];
				}
				if(DataHolder.BattleSystem().reviveStatus == null)
				{
					DataHolder.BattleSystem().reviveStatus = new int[count];
				}
				for(int i=0; i<DataHolder.BattleSystem().reviveSetStatus.Length; i++)
				{
					if(DataHolder.StatusValue(i).IsConsumable())
					{
						EditorGUILayout.BeginHorizontal();
						DataHolder.BattleSystem().reviveSetStatus[i] = EditorGUILayout.Toggle("Set "+DataHolder.StatusValues().GetName(i),
								DataHolder.BattleSystem().reviveSetStatus[i], GUILayout.Width(pw.mWidth));
						if(DataHolder.BattleSystem().reviveSetStatus[i])
						{
							DataHolder.BattleSystem().reviveStatus[i] = EditorGUILayout.IntField(
									DataHolder.BattleSystem().reviveStatus[i], GUILayout.Width(pw.mWidth*0.5f));
						}
						EditorGUILayout.EndHorizontal();
					}
					else
					{
						DataHolder.BattleSystem().reviveSetStatus[i] = false;
						DataHolder.BattleSystem().reviveStatus[i] = 0;
					}
				}
			}
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical("box");
		fold4 = EditorGUILayout.Foldout(fold4, "Start battle status settings");
		if(fold4)
		{
			DataHolder.BattleSystem().startBattleStatusSettings = EditorGUILayout.Toggle("Use b.s. status", DataHolder.BattleSystem().startBattleStatusSettings, GUILayout.Width(pw.mWidth));
			if(DataHolder.BattleSystem().startBattleStatusSettings)
			{
				if(DataHolder.BattleSystem().startSetStatus == null)
				{
					DataHolder.BattleSystem().startSetStatus = new bool[count];
				}
				if(DataHolder.BattleSystem().startStatus == null)
				{
					DataHolder.BattleSystem().startStatus = new int[count];
				}
				for(int i=0; i<DataHolder.BattleSystem().startSetStatus.Length; i++)
				{
					if(DataHolder.StatusValue(i).IsConsumable())
					{
						EditorGUILayout.BeginHorizontal();
						DataHolder.BattleSystem().startSetStatus[i] = EditorGUILayout.Toggle("Set "+DataHolder.StatusValues().GetName(i),
								DataHolder.BattleSystem().startSetStatus[i], GUILayout.Width(pw.mWidth));
						if(DataHolder.BattleSystem().startSetStatus[i])
						{
							DataHolder.BattleSystem().startStatus[i] = EditorGUILayout.IntField(
									DataHolder.BattleSystem().startStatus[i], GUILayout.Width(pw.mWidth*0.5f));
						}
						EditorGUILayout.EndHorizontal();
					}
					else
					{
						DataHolder.BattleSystem().startSetStatus[i] = false;
						DataHolder.BattleSystem().startStatus[i] = 0;
					}
				}
			}
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical("box");
		fold5 = EditorGUILayout.Foldout(fold5, "End battle status settings");
		if(fold5)
		{
			DataHolder.BattleSystem().endBattleStatusSettings = EditorGUILayout.Toggle("Use b.e. status", DataHolder.BattleSystem().endBattleStatusSettings, GUILayout.Width(pw.mWidth));
			if(DataHolder.BattleSystem().endBattleStatusSettings)
			{
				if(DataHolder.BattleSystem().endSetStatus == null)
				{
					DataHolder.BattleSystem().endSetStatus = new bool[count];
				}
				if(DataHolder.BattleSystem().endStatus == null)
				{
					DataHolder.BattleSystem().endStatus = new int[count];
				}
				for(int i=0; i<DataHolder.BattleSystem().endSetStatus.Length; i++)
				{
					if(DataHolder.StatusValue(i).IsConsumable())
					{
						EditorGUILayout.BeginHorizontal();
						DataHolder.BattleSystem().endSetStatus[i] = EditorGUILayout.Toggle("Set "+DataHolder.StatusValues().GetName(i),
								DataHolder.BattleSystem().endSetStatus[i], GUILayout.Width(pw.mWidth));
						if(DataHolder.BattleSystem().endSetStatus[i])
						{
							DataHolder.BattleSystem().endStatus[i] = EditorGUILayout.IntField(
									DataHolder.BattleSystem().endStatus[i], GUILayout.Width(pw.mWidth*0.5f));
						}
						EditorGUILayout.EndHorizontal();
					}
					else
					{
						DataHolder.BattleSystem().endSetStatus[i] = false;
						DataHolder.BattleSystem().endStatus[i] = 0;
					}
				}
			}
			this.Separate();
		}
		EditorGUILayout.EndVertical();
		
		if(!DataHolder.BattleSystem().IsRealTime())
		{
			EditorGUILayout.BeginVertical("box");
			fold6 = EditorGUILayout.Foldout(fold6, "Battle cam settings");
			if(fold6)
			{
				DataHolder.BattleCam().blockAnimationCams = EditorGUILayout.BeginToggleGroup("Block animation cams",
						DataHolder.BattleCam().blockAnimationCams);
				EditorGUILayout.EndToggleGroup();
				if(DataHolder.BattleCam().blockAnimationCams)
				{
					DataHolder.BattleCam().useCombatantCenter = EditorGUILayout.Toggle("Combatant center",
							DataHolder.BattleCam().useCombatantCenter, GUILayout.Width(pw.mWidth));
					DataHolder.BattleCam().rememberPosition = EditorGUILayout.Toggle("Remember position",
							DataHolder.BattleCam().rememberPosition, GUILayout.Width(pw.mWidth));
					DataHolder.BattleCam().rotationAxis = EditorGUILayout.Vector3Field("Rotation axis",
							DataHolder.BattleCam().rotationAxis, GUILayout.Width(pw.mWidth));
					DataHolder.BattleCam().rotationSpeed = EditorGUILayout.FloatField("Rotation speed",
							DataHolder.BattleCam().rotationSpeed, GUILayout.Width(pw.mWidth));
					DataHolder.BattleCam().limitRotation = EditorGUILayout.Toggle("Limit rotation", 
							DataHolder.BattleCam().limitRotation, GUILayout.Width(pw.mWidth));
					if(DataHolder.BattleCam().limitRotation)
					{
						DataHolder.BattleCam().minRotation = EditorGUILayout.Vector3Field("Min rotation (-180 - 0)",
								DataHolder.BattleCam().minRotation, GUILayout.Width(pw.mWidth));
						VectorHelper.LimitVector3(ref DataHolder.BattleCam().minRotation, -180, 0);
						DataHolder.BattleCam().maxRotation = EditorGUILayout.Vector3Field("Max rotation (0 - 180)",
								DataHolder.BattleCam().maxRotation, GUILayout.Width(pw.mWidth));
						VectorHelper.LimitVector3(ref DataHolder.BattleCam().maxRotation, 0, 180);
					}
					
					EditorGUILayout.Separator();
					DataHolder.BattleCam().lookAtDamping = EditorGUILayout.FloatField("Look at damping",
							DataHolder.BattleCam().lookAtDamping, GUILayout.Width(pw.mWidth));
					
					EditorGUILayout.Separator();
					GUILayout.Label("Look at latest damage", EditorStyles.boldLabel);
					DataHolder.BattleCam().lookAtLatestDamage = EditorGUILayout.Toggle("Simple look",
							DataHolder.BattleCam().lookAtLatestDamage, GUILayout.Width(pw.mWidth));
					if(DataHolder.BattleCam().lookAtLatestDamage)
					{
						DataHolder.BattleCam().damageLookAtChild = EditorGUILayout.TextField("Look at child", 
								DataHolder.BattleCam().damageLookAtChild, GUILayout.Width(pw.mWidth*2));
					}
					else
					{
						if(GUILayout.Button("Add", GUILayout.Width(100)))
						{
							DataHolder.BattleCam().AddLatestDamage();
						}
						for(int i=0; i<DataHolder.BattleCam().latestDamageID.Length; i++)
						{
							EditorGUILayout.BeginVertical("box");
							EditorGUILayout.BeginHorizontal();
							if(GUILayout.Button("Remove", GUILayout.Width(100)))
							{
								DataHolder.BattleCam().RemoveLatestDamage(i);
								break;
							}
							DataHolder.BattleCam().latestDamageID[i] = EditorGUILayout.Popup("Camera position",
									DataHolder.BattleCam().latestDamageID[i],  DataHolder.CameraPositions().GetNameList(true), 
									GUILayout.Width(pw.mWidth));
							DataHolder.BattleCam().ldRotationAxis[i] = EditorGUILayout.Vector3Field("Rotation axis",
									DataHolder.BattleCam().ldRotationAxis[i], GUILayout.Width(pw.mWidth));
							DataHolder.BattleCam().ldRotationSpeed[i] = EditorGUILayout.FloatField("Speed",
									DataHolder.BattleCam().ldRotationSpeed[i], GUILayout.Width(pw.mWidth));
							GUILayout.FlexibleSpace();
							EditorGUILayout.EndHorizontal();
							EditorGUILayout.Separator();
							EditorGUILayout.EndVertical();
						}
					}
					
					EditorGUILayout.Separator();
					GUILayout.Label("Look at latest user", EditorStyles.boldLabel);
					DataHolder.BattleCam().lookAtLatestUser = EditorGUILayout.Toggle("Simple look",
							DataHolder.BattleCam().lookAtLatestUser, GUILayout.Width(pw.mWidth));
					if(DataHolder.BattleCam().lookAtLatestUser)
					{
						DataHolder.BattleCam().userLookAtChild = EditorGUILayout.TextField("Look at child", 
								DataHolder.BattleCam().userLookAtChild, GUILayout.Width(pw.mWidth*2));
					}
					else
					{
						if(GUILayout.Button("Add", GUILayout.Width(100)))
						{
							DataHolder.BattleCam().AddLatestUser();
						}
						for(int i=0; i<DataHolder.BattleCam().latestUserID.Length; i++)
						{
							EditorGUILayout.BeginVertical("box");
							EditorGUILayout.BeginHorizontal();
							if(GUILayout.Button("Remove", GUILayout.Width(100)))
							{
								DataHolder.BattleCam().RemoveLatestUser(i);
								break;
							}
							DataHolder.BattleCam().latestUserID[i] = EditorGUILayout.Popup("Camera position",
									DataHolder.BattleCam().latestUserID[i],  DataHolder.CameraPositions().GetNameList(true), 
									GUILayout.Width(pw.mWidth));
							DataHolder.BattleCam().luRotationAxis[i] = EditorGUILayout.Vector3Field("Rotation axis",
									DataHolder.BattleCam().luRotationAxis[i], GUILayout.Width(pw.mWidth));
							DataHolder.BattleCam().luRotationSpeed[i] = EditorGUILayout.FloatField("Speed",
									DataHolder.BattleCam().luRotationSpeed[i], GUILayout.Width(pw.mWidth));
							GUILayout.FlexibleSpace();
							EditorGUILayout.EndHorizontal();
							EditorGUILayout.Separator();
							EditorGUILayout.EndVertical();
						}
					}
					
					EditorGUILayout.Separator();
					GUILayout.Label("Look at menu user", EditorStyles.boldLabel);
					DataHolder.BattleCam().lookAtMenuUser = EditorGUILayout.Toggle("Simple look",
							DataHolder.BattleCam().lookAtMenuUser, GUILayout.Width(pw.mWidth));
					if(DataHolder.BattleCam().lookAtMenuUser)
					{
						DataHolder.BattleCam().menuLookAtChild = EditorGUILayout.TextField("Look at child", 
								DataHolder.BattleCam().menuLookAtChild, GUILayout.Width(pw.mWidth*2));
					}
					else
					{
						if(GUILayout.Button("Add", GUILayout.Width(100)))
						{
							DataHolder.BattleCam().AddMenuUser();
						}
						for(int i=0; i<DataHolder.BattleCam().menuUserID.Length; i++)
						{
							EditorGUILayout.BeginVertical("box");
							EditorGUILayout.BeginHorizontal();
							if(GUILayout.Button("Remove", GUILayout.Width(100)))
							{
								DataHolder.BattleCam().RemoveMenuUser(i);
								break;
							}
							DataHolder.BattleCam().menuUserID[i] = EditorGUILayout.Popup("Camera position",
									DataHolder.BattleCam().menuUserID[i],  DataHolder.CameraPositions().GetNameList(true), 
									GUILayout.Width(pw.mWidth));
							DataHolder.BattleCam().muRotationAxis[i] = EditorGUILayout.Vector3Field("Rotation axis",
									DataHolder.BattleCam().muRotationAxis[i], GUILayout.Width(pw.mWidth));
							DataHolder.BattleCam().muRotationSpeed[i] = EditorGUILayout.FloatField("Speed",
									DataHolder.BattleCam().muRotationSpeed[i], GUILayout.Width(pw.mWidth));
							GUILayout.FlexibleSpace();
							EditorGUILayout.EndHorizontal();
							EditorGUILayout.Separator();
							EditorGUILayout.EndVertical();
						}
					}
					
					EditorGUILayout.Separator();
					GUILayout.Label("Look at selection", EditorStyles.boldLabel);
					DataHolder.BattleCam().lookAtSelection = EditorGUILayout.Toggle("Simple look",
							DataHolder.BattleCam().lookAtSelection, GUILayout.Width(pw.mWidth));
					if(DataHolder.BattleCam().lookAtSelection)
					{
						DataHolder.BattleCam().selectionLookAtChild = EditorGUILayout.TextField("Look at child", 
								DataHolder.BattleCam().selectionLookAtChild, GUILayout.Width(pw.mWidth*2));
					}
					else
					{
						if(GUILayout.Button("Add", GUILayout.Width(100)))
						{
							DataHolder.BattleCam().AddSelection();
						}
						for(int i=0; i<DataHolder.BattleCam().selectionID.Length; i++)
						{
							EditorGUILayout.BeginVertical("box");
							EditorGUILayout.BeginHorizontal();
							if(GUILayout.Button("Remove", GUILayout.Width(100)))
							{
								DataHolder.BattleCam().RemoveSelection(i);
								break;
							}
							DataHolder.BattleCam().selectionID[i] = EditorGUILayout.Popup("Camera position",
									DataHolder.BattleCam().selectionID[i],  DataHolder.CameraPositions().GetNameList(true), 
									GUILayout.Width(pw.mWidth));
							DataHolder.BattleCam().sRotationAxis[i] = EditorGUILayout.Vector3Field("Rotation axis",
									DataHolder.BattleCam().sRotationAxis[i], GUILayout.Width(pw.mWidth));
							DataHolder.BattleCam().sRotationSpeed[i] = EditorGUILayout.FloatField("Speed",
									DataHolder.BattleCam().sRotationSpeed[i], GUILayout.Width(pw.mWidth));
							GUILayout.FlexibleSpace();
							EditorGUILayout.EndHorizontal();
							EditorGUILayout.Separator();
							EditorGUILayout.EndVertical();
						}
					}
					
					EditorGUILayout.Separator();
					GUILayout.Label("Block exceptions", EditorStyles.boldLabel);
					if(GUILayout.Button("Add", GUILayout.Width(100)))
					{
						DataHolder.BattleCam().AddNoBlock();
					}
					for(int i=0; i<DataHolder.BattleCam().noBlockAnimation.Length; i++)
					{
						EditorGUILayout.BeginHorizontal();
						DataHolder.BattleCam().noBlockAnimation[i] = EditorGUILayout.Popup("Animation",
								DataHolder.BattleCam().noBlockAnimation[i], 
								DataHolder.BattleAnimations().GetNameList(true), GUILayout.Width(pw.mWidth));
						if(GUILayout.Button("Remove", GUILayout.Width(100)))
						{
							DataHolder.BattleCam().RemoveNoBlock(i);
							break;
						}
						GUILayout.FlexibleSpace();
						EditorGUILayout.EndHorizontal();
					}
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
		}
		
		EditorGUILayout.Separator();
		
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}
}