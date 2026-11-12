using System;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Maths;

public class MathRandomInt : IronBlock
{
	private static Random rand = new Random();

	public override object Evaluate(Context context)
	{
		double val = StiObjConverter.ToDouble(base.Values.Evaluate("FROM", context));
		double val2 = StiObjConverter.ToDouble(base.Values.Evaluate("TO", context));
		return (double)rand.Next((int)Math.Min(val, val2), (int)Math.Max(val, val2));
	}
}
