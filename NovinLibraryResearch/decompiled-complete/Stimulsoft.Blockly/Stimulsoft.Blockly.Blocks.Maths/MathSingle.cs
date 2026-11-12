using System;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Maths;

public class MathSingle : IronBlock
{
	public override object Evaluate(Context context)
	{
		string text = base.Fields.Get("OP");
		double num = StiObjConverter.ToDouble(base.Values.Evaluate("NUM", context));
		return text switch
		{
			"ROOT" => Math.Sqrt(num), 
			"ABS" => Math.Abs(num), 
			"NEG" => -1.0 * num, 
			"LN" => Math.Log(num), 
			"LOG10" => Math.Log10(num), 
			"EXP" => Math.Exp(num), 
			"POW10" => Math.Pow(10.0, num), 
			"SIN" => Math.Sin(num / 180.0 * Math.PI), 
			"COS" => Math.Cos(num / 180.0 * Math.PI), 
			"TAN" => Math.Tan(num / 180.0 * Math.PI), 
			"ASIN" => Math.Asin(num / 180.0 * Math.PI), 
			"ACOS" => Math.Acos(num / 180.0 * Math.PI), 
			"ATAN" => Math.Atan(num / 180.0 * Math.PI), 
			_ => throw new ApplicationException("Unknown OP " + text), 
		};
	}
}
