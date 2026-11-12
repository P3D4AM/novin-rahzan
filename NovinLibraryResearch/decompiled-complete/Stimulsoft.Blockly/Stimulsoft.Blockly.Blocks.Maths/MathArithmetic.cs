using System;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Maths;

public class MathArithmetic : IronBlock
{
	public override object Evaluate(Context context)
	{
		double num = StiObjConverter.ToDouble(base.Values.Evaluate("A", context));
		double num2 = StiObjConverter.ToDouble(base.Values.Evaluate("B", context));
		string text = base.Fields.Get("OP");
		return text switch
		{
			"MULTIPLY" => num * num2, 
			"DIVIDE" => num / num2, 
			"ADD" => num + num2, 
			"MINUS" => num - num2, 
			"POWER" => Math.Pow(num, num2), 
			_ => throw new ApplicationException("Unknown OP " + text), 
		};
	}
}
