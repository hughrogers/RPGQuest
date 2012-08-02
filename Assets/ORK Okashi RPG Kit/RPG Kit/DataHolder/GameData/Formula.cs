
using System.Collections.Generic;
using UnityEngine;

public class Formula
{
	public bool useMinValue = false;
	public float minValue = 0;
	
	public bool useMaxValue = false;
	public float maxValue = 0;
	
	public FormulaPiece[] piece = new FormulaPiece[] {new FormulaPiece()};
	
	// calc
	private Stack<float> valueStack;
	private Stack<FormulaOperator> operatorStack;

	public Formula()
	{
		// create new stacks
		this.valueStack = new Stack<float>();
		this.operatorStack = new Stack<FormulaOperator>();
	}
	
	public float Calculate(Combatant user, Combatant target)
	{
		this.valueStack.Clear();
		this.operatorStack.Clear();
		
		float calc = 0;
		if(piece.Length > 0)
		{
			calc = this.piece[0].GetRoundedValue(this.GetPieceValue(0, user, target));
			for(int i=1; i<piece.Length; i++)
			{
				if(this.piece[i].beginGroup)
				{
					valueStack.Push(calc);
					operatorStack.Push(piece[i-1].formulaOperator);
					calc = this.piece[i].GetRoundedValue(this.GetPieceValue(i, user, target));
				}
				else if(piece[i].IsCheckOK(calc))
				{
					if(FormulaOperator.DONE.Equals(piece[i-1].formulaOperator))
					{
						break;
					}
					else if(FormulaOperator.ADD.Equals(piece[i-1].formulaOperator))
					{
						calc += this.GetPieceValue(i, user, target);
					}
					else if(FormulaOperator.SUB.Equals(piece[i-1].formulaOperator))
					{
						calc -= this.GetPieceValue(i, user, target);
					}
					else if(FormulaOperator.MULTIPLY.Equals(piece[i-1].formulaOperator))
					{
						calc *= this.GetPieceValue(i, user, target);
					}
					else if(FormulaOperator.DIVIDE.Equals(piece[i-1].formulaOperator))
					{
						calc /= this.GetPieceValue(i, user, target);
					}
					else if(FormulaOperator.MODULO.Equals(piece[i-1].formulaOperator))
					{
						calc = calc % this.GetPieceValue(i, user, target);
					}
					else if(FormulaOperator.POWER_OF.Equals(piece[i-1].formulaOperator))
					{
						calc = Mathf.Pow(calc, this.GetPieceValue(i, user, target));
					}
					else if(FormulaOperator.LOG.Equals(piece[i-1].formulaOperator))
					{
						calc = Mathf.Log(calc, this.GetPieceValue(i, user, target));
					}
					calc = this.piece[i].GetRoundedValue(calc);
					
					if(this.piece[i].endGroup)
					{
						this.EndGroupCalc(ref calc);
					}
					
					if(piece[i].endCalc) break;
				}
			}
		}
		
		// close groups
		while(this.valueStack.Count > 0)
		{
			this.EndGroupCalc(ref calc);
		}
		
		if(this.useMinValue && calc < this.minValue) calc = this.minValue;
		else if(this.useMaxValue && calc > this.maxValue) calc = this.maxValue;
		return calc;
	}
	
	private float GetPieceValue(int i, Combatant user, Combatant target)
	{
		float calc = 0;
		if(FormulaChooser.VALUE.Equals(piece[i].chooser))
		{
			if(piece[i].useNumberVariable && piece[i].numberVariableKey != "")
			{
				calc = GameHandler.GetNumberVariable(piece[i].numberVariableKey);
			}
			else calc = piece[i].value;
		}
		else if(FormulaChooser.STATUS.Equals(piece[i].chooser))
		{
			if(StatusOrigin.USER.Equals(piece[i].statusOrigin))
			{
				if(piece[i].useLevel)
				{
					if(piece[i].useClassLevel) calc = user.currentClassLevel;
					else calc = user.currentLevel;
				}
				else calc = user.status[piece[i].status].GetValue();
			}
			else if(StatusOrigin.TARGET.Equals(piece[i].statusOrigin))
			{
				if(piece[i].useLevel) calc = target.currentLevel;
				else calc = target.status[piece[i].status].GetValue();
			}
		}
		else if(FormulaChooser.FORMULA.Equals(piece[i].chooser) && piece[i].formula >= 0)
		{
			calc = DataHolder.Formulas().formula[piece[i].formula].Calculate(user, target);
		}
		else if(FormulaChooser.RANDOM.Equals(piece[i].chooser))
		{
			if(piece[i].randomFormula)
			{
				if(piece[i].getMin)
				{
					calc = Mathf.Min(
						DataHolder.Formulas().formula[piece[i].randomMinFormula].Calculate(user, target), 
						DataHolder.Formulas().formula[piece[i].randomMaxFormula].Calculate(user, target));
				}
				else if(piece[i].getMax)
				{
					calc = Mathf.Max(
						DataHolder.Formulas().formula[piece[i].randomMinFormula].Calculate(user, target), 
						DataHolder.Formulas().formula[piece[i].randomMaxFormula].Calculate(user, target));
				}
				else
				{
					calc = Random.Range(
							DataHolder.Formulas().formula[piece[i].randomMinFormula].Calculate(user, target), 
							DataHolder.Formulas().formula[piece[i].randomMaxFormula].Calculate(user, target));
				}
			}
			else
			{
				calc = Random.Range(piece[i].randomMin, piece[i].randomMax);
			}
		}
		if(this.piece[i].useMinValue && calc < this.piece[i].minValue) calc = this.piece[i].minValue;
		else if(this.piece[i].useMaxValue && calc > this.piece[i].maxValue) calc = this.piece[i].maxValue;
		return calc;
	}
	
	private void EndGroupCalc(ref float calc)
	{
		float tmp = valueStack.Pop();
		FormulaOperator op = operatorStack.Pop();
		
		if(FormulaOperator.ADD.Equals(op)) calc = tmp+calc;
		else if(FormulaOperator.SUB.Equals(op)) calc = tmp-calc;
		else if(FormulaOperator.MULTIPLY.Equals(op)) calc = tmp*calc;
		else if(FormulaOperator.DIVIDE.Equals(op)) calc = tmp/calc;
		else if(FormulaOperator.MODULO.Equals(op)) calc = tmp % calc;
		else if(FormulaOperator.POWER_OF.Equals(op)) calc = Mathf.Pow(tmp, calc);
		else if(FormulaOperator.LOG.Equals(op)) calc = Mathf.Log(tmp, calc);
	}
	
	public void AddPiece()
	{
		piece = ArrayHelper.Add(new FormulaPiece(), piece);
	}
	
	public void RemovePieceAfter(int index)
	{
		while(piece.Length-1 > index)
		{
			piece = ArrayHelper.Remove(piece.Length-1, piece);
		}
	}
}
