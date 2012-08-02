
using System.Collections;
using UnityEngine;

public class FormulaPiece
{
	public bool beginGroup = false;
	public bool endGroup = false;
	
	public FormulaChooser chooser = FormulaChooser.VALUE;
	public bool useNumberVariable = false;
	public string numberVariableKey = "";
	public float value = 0;
	
	public StatusOrigin statusOrigin = StatusOrigin.USER;
	public bool useLevel = false;
	public bool useClassLevel = false;
	public int status = 0;
	
	public int formula = 0;
	
	public bool randomFormula = false;
	public bool getMin = false;
	public bool getMax = false;
	public float randomMin = 0;
	public float randomMax = 0;
	public int randomMinFormula = 0;
	public int randomMaxFormula = 0;
	
	public Rounding rounding = Rounding.NONE;
	public bool doSum = false;
	
	public FormulaOperator formulaOperator = FormulaOperator.DONE;
	
	public bool isCheck = false;
	public float checkValue = 0;
	public ValueCheck check = ValueCheck.EQUALS;
	public bool endCalc = false;
	
	public bool useMinValue = false;
	public float minValue = 0;
	
	public bool useMaxValue = false;
	public float maxValue = 0;
	
	public FormulaPiece()
	{
		
	}
	
	public bool IsCheckOK(float calc)
	{
		bool ok = true;
		if(this.isCheck)
		{
			ok = false;
			if(ValueCheck.EQUALS.Equals(this.check) && calc == this.checkValue)
			{
				ok = true;
			}
			else if(ValueCheck.LESS.Equals(this.check) && calc < this.checkValue)
			{
				ok = true;
			}
			else if(ValueCheck.GREATER.Equals(this.check) && calc > this.checkValue)
			{
				ok = true;
			}
		}
		return ok;
	}
	
	public float GetRoundedValue(float calc)
	{
		if(Rounding.CEIL.Equals(this.rounding))
		{
			calc = Mathf.Ceil(calc);
		}
		else if(Rounding.FLOOR.Equals(this.rounding))
		{
			calc = Mathf.Floor(calc);
		}
		else if(Rounding.ROUND.Equals(this.rounding))
		{
			calc = Mathf.Round(calc);
		}
		if(!Rounding.NONE.Equals(this.rounding) && this.doSum)
		{
			int sum = (int)calc;
			sum = (sum*(sum+1))/2;
			calc = sum;
		}
		return calc;
	}
}
