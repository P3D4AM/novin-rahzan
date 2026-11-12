using System;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Maths;

public class MathConstrain : IronBlock
{
	public override object Evaluate(Context context)
	{
		double val = StiObjConverter.ToDouble(base.Values.Evaluate("VALUE", context));
		double val2 = StiObjConverter.ToDouble(base.Values.Evaluate("LOW", context));
		double val3 = StiObjConverter.ToDouble(base.Values.Evaluate("HIGH", context));
		return Math.Min(Math.Max(val, val2), val3);
	}
}
