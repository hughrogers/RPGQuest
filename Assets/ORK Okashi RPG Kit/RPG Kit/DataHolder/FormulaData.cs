
using System.Collections;

public class FormulaData : BaseData
{
	public Formula[] formula = new Formula[0];
	
	// test data
	public Combatant user = new Combatant();
	public Combatant target = new Combatant();
	
	// XML data
	private string filename = "formulas";
	
	private static string FORMULAS = "formulas";
	private static string FORMULA = "formula";
	private static string PIECE = "piece";

	public FormulaData()
	{
		LoadData();
	}
	
	public void LoadData()
	{
		ArrayList data = XMLHandler.LoadXML(dir+filename);
		
		if(data.Count > 0)
		{
			foreach(Hashtable entry in data)
			{
				if(entry[XMLHandler.NODE_NAME] as string == FormulaData.FORMULAS)
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList subs = entry[XMLHandler.NODES] as ArrayList;
						name = new string[subs.Count];
						formula = new Formula[subs.Count];
						foreach(Hashtable val in subs)
						{
							if(val[XMLHandler.NODE_NAME] as string == FormulaData.FORMULA)
							{
								int i = int.Parse((string)val["id"]);
								formula[i] = new Formula();
								formula[i].piece = new FormulaPiece[int.Parse((string)val["pieces"])];
								
								if(val.ContainsKey("minvalue"))
								{
									formula[i].useMinValue = true;
									formula[i].minValue = float.Parse((string)val["minvalue"]);
								}
								if(val.ContainsKey("maxvalue"))
								{
									formula[i].useMaxValue = true;
									formula[i].maxValue = float.Parse((string)val["maxvalue"]);
								}
								
								ArrayList s = val[XMLHandler.NODES] as ArrayList;
								foreach(Hashtable ht in s)
								{
									if(ht[XMLHandler.NODE_NAME] as string == FormulaData.NAME)
									{
										name[i] = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == FormulaData.PIECE)
									{
										int j = int.Parse((string)ht["id"]);
										formula[i].piece[j] = new FormulaPiece();
										formula[i].piece[j].formulaOperator = (FormulaOperator)System.Enum.Parse(typeof(FormulaOperator), (string)ht["operator"]);
										formula[i].piece[j].chooser = (FormulaChooser)System.Enum.Parse(typeof(FormulaChooser), (string)ht["chooser"]);
										
										if(ht.ContainsKey("begingroup")) formula[i].piece[j].beginGroup = true;
										if(ht.ContainsKey("endgroup")) formula[i].piece[j].endGroup = true;
										
										if(ht.ContainsKey("minvalue"))
										{
											formula[i].piece[j].useMinValue = true;
											formula[i].piece[j].minValue = float.Parse((string)ht["minvalue"]);
										}
										if(ht.ContainsKey("maxvalue"))
										{
											formula[i].piece[j].useMaxValue = true;
											formula[i].piece[j].maxValue = float.Parse((string)ht["maxvalue"]);
										}
										
										if(ht.ContainsKey("checkvalue"))
										{
											formula[i].piece[j].isCheck = true;
											formula[i].piece[j].checkValue = float.Parse((string)ht["checkvalue"]);
											formula[i].piece[j].check = (ValueCheck)System.Enum.Parse(typeof(ValueCheck), (string)ht["checktype"]);
											if(ht.ContainsKey("endcalc"))
											{
												formula[i].piece[j].endCalc = true;
											}
										}
										
										if(ht.ContainsKey("rounding"))
										{
											formula[i].piece[j].rounding = (Rounding)System.Enum.Parse(typeof(Rounding), (string)ht["rounding"]);
										}
										if(ht.ContainsKey("dosum")) formula[i].piece[j].doSum = true;
										
										if(FormulaChooser.VALUE.Equals(formula[i].piece[j].chooser))
										{
											if(ht.ContainsKey("usenumvar")) formula[i].piece[j].useNumberVariable = true;
											if(ht.ContainsKey("numvarkey")) formula[i].piece[j].numberVariableKey = ht["numvarkey"] as string;
											if(ht.ContainsKey("value")) formula[i].piece[j].value = float.Parse((string)ht["value"]);
										}
										else if(FormulaChooser.STATUS.Equals(formula[i].piece[j].chooser))
										{
											formula[i].piece[j].statusOrigin = (StatusOrigin)System.Enum.Parse(typeof(StatusOrigin), (string)ht["statusorigin"]);
											if(ht.ContainsKey("uselevel")) formula[i].piece[j].useLevel = true;
											if(ht.ContainsKey("useclasslevel")) formula[i].piece[j].useClassLevel = true;
											if(ht.ContainsKey("status")) formula[i].piece[j].status = int.Parse((string)ht["status"]);
										}
										else if(FormulaChooser.FORMULA.Equals(formula[i].piece[j].chooser))
										{
											formula[i].piece[j].formula = int.Parse((string)ht["formula"]);
										}
										else if(FormulaChooser.RANDOM.Equals(formula[i].piece[j].chooser))
										{
											if(ht.ContainsKey("randomformula"))
											{
												formula[i].piece[j].randomFormula = true;
												formula[i].piece[j].randomMinFormula = int.Parse((string)ht["min"]);
												formula[i].piece[j].randomMaxFormula = int.Parse((string)ht["max"]);
												if(ht.ContainsKey("getmin")) formula[i].piece[j].getMin = true;
												if(ht.ContainsKey("getmax")) formula[i].piece[j].getMax = true;
											}
											else
											{
												formula[i].piece[j].randomMin = float.Parse((string)ht["min"]);
												formula[i].piece[j].randomMax = float.Parse((string)ht["max"]);
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}
	
	public void SaveData()
	{
		if(name == null) return;
		ArrayList data = new ArrayList();
		ArrayList subs = new ArrayList();
		Hashtable sv = new Hashtable();
		
		sv.Add(XMLHandler.NODE_NAME, FormulaData.FORMULAS);
		
		for(int i=0; i<name.Length; i++)
		{
			Hashtable ht = new Hashtable();
			ArrayList s = new ArrayList();
			
			ht.Add(XMLHandler.NODE_NAME, FormulaData.FORMULA);
			ht.Add("id", i.ToString());
			ht.Add("pieces", formula[i].piece.Length.ToString());
			if(formula[i].useMinValue)
			{
				ht.Add("minvalue", formula[i].minValue.ToString());
			}
			if(formula[i].useMaxValue)
			{
				ht.Add("maxvalue", formula[i].maxValue.ToString());
			}
			
			Hashtable n = new Hashtable();
			n.Add(XMLHandler.NODE_NAME, FormulaData.NAME);
			n.Add(XMLHandler.CONTENT, name[i]);
			s.Add(n);
			
			for(int j=0; j<formula[i].piece.Length; j++)
			{
				Hashtable p = new Hashtable();
				p.Add(XMLHandler.NODE_NAME, FormulaData.PIECE);
				p.Add("id", j.ToString());
				p.Add("operator", formula[i].piece[j].formulaOperator.ToString());
				p.Add("chooser", formula[i].piece[j].chooser.ToString());
				
				if(formula[i].piece[j].beginGroup) p.Add("begingroup", "true");
				if(formula[i].piece[j].endGroup) p.Add("endgroup", "true");
				
				if(formula[i].piece[j].doSum) p.Add("dosum", "true");
				
				if(formula[i].piece[j].isCheck)
				{
					p.Add("checkvalue", formula[i].piece[j].checkValue.ToString());
					p.Add("checktype", formula[i].piece[j].check.ToString());
					if(formula[i].piece[j].endCalc)
					{
						p.Add("endcalc", "true");
					}
				}
				
				if(!Rounding.NONE.Equals(formula[i].piece[j].rounding))
				{
					p.Add("rounding", formula[i].piece[j].rounding.ToString());
				}
				
				if(formula[i].piece[j].useMinValue)
				{
					p.Add("minvalue", formula[i].piece[j].minValue.ToString());
				}
				if(formula[i].piece[j].useMaxValue)
				{
					p.Add("maxvalue", formula[i].piece[j].maxValue.ToString());
				}
				
				if(FormulaChooser.VALUE.Equals(formula[i].piece[j].chooser))
				{
					if(formula[i].piece[j].useNumberVariable)
					{
						p.Add("usenumvar", "true");
						if(formula[i].piece[j].numberVariableKey != "")
						{
							p.Add("numvarkey", formula[i].piece[j].numberVariableKey);
						}
					}
					else p.Add("value", formula[i].piece[j].value.ToString());
				}
				else if(FormulaChooser.STATUS.Equals(formula[i].piece[j].chooser))
				{
					p.Add("statusorigin", formula[i].piece[j].statusOrigin.ToString());
					if(formula[i].piece[j].useLevel) p.Add("uselevel", "true");
					if(formula[i].piece[j].useClassLevel) p.Add("useclasslevel", "true");
					else p.Add("status", formula[i].piece[j].status.ToString());
				}
				else if(FormulaChooser.FORMULA.Equals(formula[i].piece[j].chooser))
				{
					p.Add("formula", formula[i].piece[j].formula.ToString());
				}
				else if(FormulaChooser.RANDOM.Equals(formula[i].piece[j].chooser))
				{
					if(formula[i].piece[j].randomFormula)
					{
						p.Add("randomformula", "true");
						if(formula[i].piece[j].getMin) p.Add("getmin", "true");
						if(formula[i].piece[j].getMax) p.Add("getmax", "true");
						p.Add("min", formula[i].piece[j].randomMinFormula.ToString());
						p.Add("max", formula[i].piece[j].randomMaxFormula.ToString());
					}
					else
					{
						p.Add("min", formula[i].piece[j].randomMin.ToString());
						p.Add("max", formula[i].piece[j].randomMax.ToString());
					}
				}
				s.Add(p);
			}
			
			ht.Add(XMLHandler.NODES, s);
			subs.Add(ht);
		}
		sv.Add(XMLHandler.NODES, subs);
		
		data.Add(sv);
		XMLHandler.SaveXML(dir, filename, data);
	}
	
	public void ResetTestData(int svCount)
	{
		user.status = new StatusValue[svCount];
		target.status = new StatusValue[svCount];
		for(int i=0; i<svCount; i++)
		{
			user.status[i] = DataHolder.StatusValues().GetCopy(i);
			user.status[i].SetOwner(user);
			target.status[i] = DataHolder.StatusValues().GetCopy(i);
			target.status[i].SetOwner(user);
		}
	}
	
	public void AddFormula(string n, int svCount)
	{
		if(name == null)
		{
			name = new string[] {n};
			formula = new Formula[] {new Formula()};
		}
		else
		{
			name = ArrayHelper.Add(n, name);
			formula = ArrayHelper.Add(new Formula(), formula);
		}
		
		if(user.status == null)
		{
			this.ResetTestData(svCount);
		}
	}
	
	public override void RemoveData(int index)
	{
		name = ArrayHelper.Remove(index, name);
		if(name.Length == 0) name = null;
		formula = ArrayHelper.Remove(index, formula);
		
		for(int i=0; i<formula.Length; i++)
		{
			for(int j=0; j<formula[i].piece.Length; j++)
			{
				if(formula[i].piece[j].formula == index)
				{
					formula[i].piece[j].formula = 0;
				}
				else if(formula[i].piece[j].formula > index)
				{
					formula[i].piece[j].formula -= 1;
				}
				if(formula[i].piece[j].randomMinFormula == index)
				{
					formula[i].piece[j].randomMinFormula = 0;
				}
				else if(formula[i].piece[j].randomMinFormula > index)
				{
					formula[i].piece[j].randomMinFormula -= 1;
				}
				if(formula[i].piece[j].randomMaxFormula == index)
				{
					formula[i].piece[j].randomMaxFormula = 0;
				}
				else if(formula[i].piece[j].randomMaxFormula > index)
				{
					formula[i].piece[j].randomMaxFormula -= 1;
				}
			}
		}
	}
	
	public override void Copy(int index)
	{
		this.AddFormula(name[index], DataHolder.StatusValueCount);
		
		formula[formula.Length-1].piece = new FormulaPiece[formula[index].piece.Length];
		formula[formula.Length-1].useMinValue = formula[index].useMinValue;
		formula[formula.Length-1].minValue = formula[index].minValue;
		formula[formula.Length-1].useMaxValue = formula[index].useMaxValue;
		formula[formula.Length-1].maxValue = formula[index].maxValue;
		
		for(int i=0; i<formula[index].piece.Length; i++)
		{
			formula[formula.Length-1].piece[i] = new FormulaPiece();
			formula[formula.Length-1].piece[i].chooser = formula[index].piece[i].chooser;
			formula[formula.Length-1].piece[i].value = formula[index].piece[i].value;
			formula[formula.Length-1].piece[i].statusOrigin = formula[index].piece[i].statusOrigin;
			formula[formula.Length-1].piece[i].status = formula[index].piece[i].status;
			formula[formula.Length-1].piece[i].formula = formula[index].piece[i].formula;
			formula[formula.Length-1].piece[i].randomFormula = formula[index].piece[i].randomFormula;
			formula[formula.Length-1].piece[i].randomMin = formula[index].piece[i].randomMin;
			formula[formula.Length-1].piece[i].randomMax = formula[index].piece[i].randomMax;
			formula[formula.Length-1].piece[i].randomMinFormula = formula[index].piece[i].randomMinFormula;
			formula[formula.Length-1].piece[i].randomMaxFormula = formula[index].piece[i].randomMaxFormula;
			formula[formula.Length-1].piece[i].rounding = formula[index].piece[i].rounding;
			formula[formula.Length-1].piece[i].formulaOperator = formula[index].piece[i].formulaOperator;
			formula[formula.Length-1].piece[i].isCheck = formula[index].piece[i].isCheck;
			formula[formula.Length-1].piece[i].checkValue = formula[index].piece[i].checkValue;
			formula[formula.Length-1].piece[i].check = formula[index].piece[i].check;
			formula[formula.Length-1].piece[i].endCalc = formula[index].piece[i].endCalc;
			formula[formula.Length-1].piece[i].useNumberVariable = formula[index].piece[i].useNumberVariable;
			formula[formula.Length-1].piece[i].numberVariableKey = formula[index].piece[i].numberVariableKey;
			formula[formula.Length-1].piece[i].useLevel = formula[index].piece[i].useLevel;
			formula[formula.Length-1].piece[i].useClassLevel = formula[index].piece[i].useClassLevel;
			formula[formula.Length-1].piece[i].doSum = formula[index].piece[i].doSum;
			formula[formula.Length-1].piece[i].getMin = formula[index].piece[i].getMin;
			formula[formula.Length-1].piece[i].getMax = formula[index].piece[i].getMax;
			formula[formula.Length-1].piece[i].useMinValue = formula[index].piece[i].useMinValue;
			formula[formula.Length-1].piece[i].minValue = formula[index].piece[i].minValue;
			formula[formula.Length-1].piece[i].useMaxValue = formula[index].piece[i].useMaxValue;
			formula[formula.Length-1].piece[i].maxValue = formula[index].piece[i].maxValue;
			formula[formula.Length-1].piece[i].beginGroup = formula[index].piece[i].beginGroup;
			formula[formula.Length-1].piece[i].endGroup = formula[index].piece[i].endGroup;
		}
	}
	
	public void AddStatusValue(int index)
	{
		this.ResetTestData(DataHolder.StatusValueCount);
	}
	
	public void RemoveStatusValue(int index)
	{
		this.ResetTestData(DataHolder.StatusValueCount);
	}
}