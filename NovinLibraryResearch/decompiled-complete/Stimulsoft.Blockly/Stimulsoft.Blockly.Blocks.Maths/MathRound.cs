using System;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Maths;

public class MathRound : IronBlock
{
	public override object Evaluate(Context context)
	{
		string text = base.Fields.Get("OP");
		double num = StiObjConverter.ToDouble(base.Values.Evaluate("NUM", context));
		return text switch
		{
			"ROUND" => Math.Round(num), 
			"ROUNDUP" => Math.Ceiling(num), 
			"ROUNDDOWN" => Math.Floor(num), 
			_ => throw new ApplicationException("Unknown OP " + text), 
		};
	}
}
