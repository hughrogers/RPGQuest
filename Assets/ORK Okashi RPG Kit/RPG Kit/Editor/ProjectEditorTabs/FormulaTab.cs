
using UnityEditor;
using UnityEngine;

public class FormulaTab : BaseTab
{
	private float lastCalc = 0;
	private int groups = 0;
	
	public FormulaTab(ProjectWindow pw) : base(pw)
	{
		this.Reload();
	}
	
	new public void Reload()
	{
		DataHolder.Formulas().ResetTestData(pw.GetStatusValueCount());
		selection = 0;
	}
	
	public void ShowTab()
	{
		this.groups = 0;
		EditorGUILayout.BeginVertical();
		
		// buttons
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Add Formula", GUILayout.Width(pw.mWidth2)))
		{
			DataHolder.Formulas().AddFormula("New Formula", pw.GetStatusValueCount());
			selection = DataHolder.Formulas().GetDataCount()-1;
			pw.AddFormula(selection);
			GUI.FocusControl ("ID");
		}
		if(this.ShowCopyButton(DataHolder.Formulas()))
		{
			pw.AddFormula(selection);
		}
		if(DataHolder.Formulas().GetDataCount() > 1)
		{
			if(this.ShowRemButton("Remove Formula", DataHolder.Formulas()))
			{
				pw.RemoveFormula(selection);
			}
		}
		this.CheckSelection(DataHolder.Formulas());
		EditorGUILayout.EndHorizontal();
		
		// status value list
		this.AddItemList(DataHolder.Formulas());
		
		// value settings
		EditorGUILayout.BeginVertical();
		SP2 = EditorGUILayout.BeginScrollView(SP2);
		if(DataHolder.Formulas().GetDataCount() > 0)
		{
			this.AddID("Formula ID");
			DataHolder.Formulas().name[selection] = EditorGUILayout.TextField("Name", DataHolder.Formulas().name[selection]);
			this.Separate();
			
			EditorGUILayout.BeginVertical("box");
			fold3 = EditorGUILayout.Foldout(fold3, "Settings");
			if(fold3)
			{
				DataHolder.Formula(selection).useMinValue = EditorGUILayout.Toggle("Use min value", 
						DataHolder.Formula(selection).useMinValue, GUILayout.Width(pw.mWidth));
				if(DataHolder.Formula(selection).useMinValue)
				{
					DataHolder.Formula(selection).minValue = EditorGUILayout.FloatField("Min value", 
							DataHolder.Formula(selection).minValue, GUILayout.Width(pw.mWidth));
				}
				
				DataHolder.Formula(selection).useMaxValue = EditorGUILayout.Toggle("Use max value", 
						DataHolder.Formula(selection).useMaxValue, GUILayout.Width(pw.mWidth));
				if(DataHolder.Formula(selection).useMaxValue)
				{
					DataHolder.Formula(selection).maxValue = EditorGUILayout.FloatField("Max value", 
							DataHolder.Formula(selection).maxValue, GUILayout.Width(pw.mWidth));
				}
				if(DataHolder.Formula(selection).maxValue < DataHolder.Formula(selection).minValue)
				{
					DataHolder.Formula(selection).maxValue = DataHolder.Formula(selection).minValue;
				}
				this.Separate();
			}
			EditorGUILayout.EndVertical();
			
			fold1 = EditorGUILayout.Foldout(fold1, "Calculation");
			if(fold1)
			{	
				for(int i=0; i<DataHolder.Formula(selection).piece.Length; i++)
				{
					EditorGUILayout.BeginHorizontal();
					
					int space = groups*20;
					if(DataHolder.Formula(selection).piece[i].beginGroup) space += 20;
					if(space > 0) GUILayout.Space(space);
					
					
					EditorGUILayout.BeginVertical("box");
					GUILayout.Label("Step "+(i+1).ToString(), EditorStyles.boldLabel);
					
					// group settings
					if(i > 0)
					{
						DataHolder.Formula(selection).piece[i].beginGroup = EditorGUILayout.Toggle("Begin group", 
								DataHolder.Formula(selection).piece[i].beginGroup, GUILayout.Width(pw.mWidth));
						if(groups > 0 && !DataHolder.Formula(selection).piece[i].beginGroup)
						{
							DataHolder.Formula(selection).piece[i].endGroup = EditorGUILayout.Toggle("End group", 
									DataHolder.Formula(selection).piece[i].endGroup, GUILayout.Width(pw.mWidth));
						}
						else DataHolder.Formula(selection).piece[i].endGroup = false;
						
						if(DataHolder.Formula(selection).piece[i].beginGroup) groups++;
						else if(DataHolder.Formula(selection).piece[i].endGroup) groups--;
					}
					else
					{
						DataHolder.Formula(selection).piece[i].beginGroup = false;
						DataHolder.Formula(selection).piece[i].endGroup = false;
					}
					
					
					DataHolder.Formula(selection).piece[i].chooser = (FormulaChooser)this.EnumToolbar("", 
							(int)DataHolder.Formula(selection).piece[i].chooser, typeof(FormulaChooser), (int)(pw.mWidth*1.5f));
					EditorGUILayout.Separator();
					
					if(i > 0 && 
						!DataHolder.Formula(selection).piece[i].beginGroup && 
						!DataHolder.Formula(selection).piece[i].endGroup)
					{
						EditorGUILayout.BeginHorizontal();
						DataHolder.Formula(selection).piece[i].isCheck = EditorGUILayout.Toggle("Check value", 
								DataHolder.Formula(selection).piece[i].isCheck, GUILayout.Width(pw.mWidth));
						if(DataHolder.Formula(selection).piece[i].isCheck)
						{
							DataHolder.Formula(selection).piece[i].checkValue = EditorGUILayout.FloatField(DataHolder.Formula(selection).piece[i].checkValue, 
									GUILayout.Width(pw.mWidth*0.5f));
							DataHolder.Formula(selection).piece[i].check = (ValueCheck)this.EnumToolbar("", 
									(int)DataHolder.Formula(selection).piece[i].check, typeof(ValueCheck));
							DataHolder.Formula(selection).piece[i].endCalc = EditorGUILayout.Toggle("End calculation", 
									DataHolder.Formula(selection).piece[i].endCalc, GUILayout.Width(pw.mWidth));
						}
						else DataHolder.Formula(selection).piece[i].endCalc = false;
						GUILayout.FlexibleSpace();
						EditorGUILayout.EndHorizontal();
						EditorGUILayout.Separator();
					}
					else DataHolder.Formula(selection).piece[i].isCheck = false;
					
					if(FormulaChooser.VALUE.Equals(DataHolder.Formula(selection).piece[i].chooser))
					{
						DataHolder.Formula(selection).piece[i].useNumberVariable = EditorGUILayout.Toggle("Number variable",
								DataHolder.Formula(selection).piece[i].useNumberVariable, GUILayout.Width(pw.mWidth));
						if(DataHolder.Formula(selection).piece[i].useNumberVariable)
						{
							DataHolder.Formula(selection).piece[i].numberVariableKey = EditorGUILayout.TextField("Variable key",
									DataHolder.Formula(selection).piece[i].numberVariableKey, GUILayout.Width(pw.mWidth*1.2f));
						}
						else
						{
							DataHolder.Formula(selection).piece[i].value = EditorGUILayout.FloatField("Value", 
									DataHolder.Formula(selection).piece[i].value, GUILayout.Width(pw.mWidth));
						}
					}
					else if(FormulaChooser.STATUS.Equals(DataHolder.Formula(selection).piece[i].chooser))
					{
						DataHolder.Formula(selection).piece[i].statusOrigin = (StatusOrigin)this.EnumToolbar("", 
								(int)DataHolder.Formula(selection).piece[i].statusOrigin, typeof(StatusOrigin));
						DataHolder.Formula(selection).piece[i].useLevel = EditorGUILayout.Toggle("Use level",
								DataHolder.Formula(selection).piece[i].useLevel, GUILayout.Width(pw.mWidth));
						if(DataHolder.Formula(selection).piece[i].useLevel)
						{
							DataHolder.Formula(selection).piece[i].useClassLevel = EditorGUILayout.Toggle("Class level",
									DataHolder.Formula(selection).piece[i].useClassLevel, GUILayout.Width(pw.mWidth));
						}
						else
						{
							DataHolder.Formula(selection).piece[i].status = EditorGUILayout.Popup("Status Value", 
									DataHolder.Formula(selection).piece[i].status, pw.GetStatusValues(), GUILayout.Width(pw.mWidth));
						}
					}
					else if(FormulaChooser.FORMULA.Equals(DataHolder.Formula(selection).piece[i].chooser))
					{
						DataHolder.Formula(selection).piece[i].formula = EditorGUILayout.Popup("Formula", 
								DataHolder.Formula(selection).piece[i].formula, DataHolder.Formulas().GetNameList(true), GUILayout.Width(pw.mWidth));
						// safety check
						if(selection == DataHolder.Formula(selection).piece[i].formula)
						{
							if(selection == 0 && DataHolder.Formulas().GetDataCount() > 1)
							{
								DataHolder.Formula(selection).piece[i].formula = 1;
							}
							else if(selection == 0)
							{
								DataHolder.Formula(selection).piece[i].formula = -1;
							}
							else
							{
								DataHolder.Formula(selection).piece[i].formula = 0;
							}
						}
					}
					else if(FormulaChooser.RANDOM.Equals(DataHolder.Formula(selection).piece[i].chooser))
					{
						DataHolder.Formula(selection).piece[i].randomFormula = EditorGUILayout.Toggle("Random formula",
								DataHolder.Formula(selection).piece[i].randomFormula, GUILayout.Width(pw.mWidth));
						if(DataHolder.Formula(selection).piece[i].randomFormula)
						{
							if(!DataHolder.Formula(selection).piece[i].getMax)
							{
								DataHolder.Formula(selection).piece[i].getMin = EditorGUILayout.Toggle("Get minimum",
										DataHolder.Formula(selection).piece[i].getMin, GUILayout.Width(pw.mWidth));
							}
							if(!DataHolder.Formula(selection).piece[i].getMin)
							{
								DataHolder.Formula(selection).piece[i].getMax = EditorGUILayout.Toggle("Get maximum",
										DataHolder.Formula(selection).piece[i].getMax, GUILayout.Width(pw.mWidth));
							}
							
							DataHolder.Formula(selection).piece[i].randomMinFormula = EditorGUILayout.Popup("Minimum", 
									DataHolder.Formula(selection).piece[i].randomMinFormula, 
									DataHolder.Formulas().GetNameList(true), GUILayout.Width(pw.mWidth));
							DataHolder.Formula(selection).piece[i].randomMaxFormula = EditorGUILayout.Popup("Maximum", 
									DataHolder.Formula(selection).piece[i].randomMaxFormula, 
									DataHolder.Formulas().GetNameList(true), GUILayout.Width(pw.mWidth));
						}
						else
						{
							DataHolder.Formula(selection).piece[i].randomMin = EditorGUILayout.FloatField("Minimum", 
									DataHolder.Formula(selection).piece[i].randomMin, GUILayout.Width(pw.mWidth));
							DataHolder.Formula(selection).piece[i].randomMax = EditorGUILayout.FloatField("Maximum", 
									DataHolder.Formula(selection).piece[i].randomMax, GUILayout.Width(pw.mWidth));
						}
					}
					EditorGUILayout.Separator();
					
					DataHolder.Formula(selection).piece[i].rounding = (Rounding)this.EnumToolbar("Rounding", 
							(int)DataHolder.Formula(selection).piece[i].rounding, typeof(Rounding), (int)(pw.mWidth*1.2f));
					if(!Rounding.NONE.Equals(DataHolder.Formula(selection).piece[i].rounding))
					{
						DataHolder.Formula(selection).piece[i].doSum = EditorGUILayout.Toggle("Sum (1+...+n)",
								DataHolder.Formula(selection).piece[i].doSum, GUILayout.Width(pw.mWidth));
					}
					else DataHolder.Formula(selection).piece[i].doSum = false;
					EditorGUILayout.Separator();
					
					DataHolder.Formula(selection).piece[i].useMinValue = EditorGUILayout.Toggle("Use min value", 
							DataHolder.Formula(selection).piece[i].useMinValue, GUILayout.Width(pw.mWidth));
					if(DataHolder.Formula(selection).piece[i].useMinValue)
					{
						DataHolder.Formula(selection).piece[i].minValue = EditorGUILayout.FloatField("Min value", 
								DataHolder.Formula(selection).piece[i].minValue, GUILayout.Width(pw.mWidth));
					}
					
					DataHolder.Formula(selection).piece[i].useMaxValue = EditorGUILayout.Toggle("Use max value", 
							DataHolder.Formula(selection).piece[i].useMaxValue, GUILayout.Width(pw.mWidth));
					if(DataHolder.Formula(selection).piece[i].useMaxValue)
					{
						DataHolder.Formula(selection).piece[i].maxValue = EditorGUILayout.FloatField("Max value", 
								DataHolder.Formula(selection).piece[i].maxValue, GUILayout.Width(pw.mWidth));
					}
					if(DataHolder.Formula(selection).piece[i].maxValue < DataHolder.Formula(selection).piece[i].minValue)
					{
						DataHolder.Formula(selection).piece[i].maxValue = DataHolder.Formula(selection).piece[i].minValue;
					}
					EditorGUILayout.Separator();
					
					DataHolder.Formula(selection).piece[i].formulaOperator = (FormulaOperator)this.EnumToolbar("Operator", 
							(int)DataHolder.Formula(selection).piece[i].formulaOperator, typeof(FormulaOperator), (int)(pw.mWidth*3.2f));
					
					if(!FormulaOperator.DONE.Equals(DataHolder.Formula(selection).piece[i].formulaOperator) &&
						i == DataHolder.Formula(selection).piece.Length-1)
					{
						DataHolder.Formula(selection).AddPiece();
						GUI.FocusControl ("ID");
					}
					else if(FormulaOperator.DONE.Equals(DataHolder.Formula(selection).piece[i].formulaOperator) &&
							i != DataHolder.Formula(selection).piece.Length-1)
					{
						DataHolder.Formula(selection).RemovePieceAfter(i);
						GUI.FocusControl ("ID");
					}
					EditorGUILayout.Separator();
					EditorGUILayout.EndVertical();
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.Separator();
				}
			}
			
			fold2 = EditorGUILayout.Foldout(fold2, "Test Formula");
			if(fold2)
			{
				int svCount = pw.GetStatusValueCount();
				EditorGUILayout.BeginHorizontal(GUILayout.Width(pw.mWidth*2));
				EditorGUILayout.BeginVertical();
				GUILayout.Label ("User Values", EditorStyles.boldLabel);
				DataHolder.Formulas().user.currentLevel = EditorGUILayout.IntField("Level",
						DataHolder.Formulas().user.currentLevel, GUILayout.Width(pw.mWidth));
				if(DataHolder.Formulas().user.status == null ||
					DataHolder.Formulas().user.status.Length != svCount)
				{
					DataHolder.Formulas().ResetTestData(pw.GetStatusValueCount());
				}
				for(int i=0; i<svCount; i++)
				{
					DataHolder.Formulas().user.status[i].SetValue(EditorGUILayout.IntField(pw.GetStatusValue(i), 
							DataHolder.Formulas().user.status[i].GetValue(), 
							GUILayout.Width(pw.mWidth)), false, false, false);
				}
				EditorGUILayout.EndVertical();
				
				EditorGUILayout.BeginVertical();
				GUILayout.Label ("Target Values", EditorStyles.boldLabel);
				DataHolder.Formulas().target.currentLevel = EditorGUILayout.IntField("Level",
						DataHolder.Formulas().target.currentLevel, GUILayout.Width(pw.mWidth));
				for(int i=0; i<svCount; i++)
				{
					DataHolder.Formulas().target.status[i].SetValue(EditorGUILayout.IntField(pw.GetStatusValue(i), 
							DataHolder.Formulas().target.status[i].GetValue(), 
							GUILayout.Width(pw.mWidth)), false, false, false);
				}
				EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.Separator();
				EditorGUILayout.BeginHorizontal();
				if(GUILayout.Button("Test", GUILayout.Width(pw.mWidth2)))
				{
					this.lastCalc = DataHolder.Formula(selection).Calculate(DataHolder.Formulas().user, DataHolder.Formulas().target);
				}
				EditorGUILayout.PrefixLabel(this.lastCalc.ToString());
				EditorGUILayout.EndHorizontal();
				this.Separate();
			}
			
			EditorGUILayout.EndVertical();
		}
		this.EndTab();
	}
}