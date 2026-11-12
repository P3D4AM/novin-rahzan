using System;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Maths;

public class MathConstant : IronBlock
{
	public override object Evaluate(Context context)
	{
		string constant = base.Fields.Get("CONSTANT");
		return GetValue(constant);
	}

	private static double GetValue(string constant)
	{
		return constant switch
		{
			"PI" => Math.PI, 
			"E" => Math.E, 
			"GOLDEN_RATIO" => (1.0 + Math.Sqrt(5.0)) / 2.0, 
			"SQRT2" => Math.Sqrt(2.0), 
			"SQRT1_2" => Math.Sqrt(0.5), 
			"INFINITY" => double.PositiveInfinity, 
			_ => throw new ApplicationException("Unknown CONSTANT " + constant), 
		};
	}
}
