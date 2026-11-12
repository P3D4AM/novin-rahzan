using System;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Maths;

public class MathRandomFloat : IronBlock
{
	private static Random rand = new Random();

	public override object Evaluate(Context context)
	{
		return rand.NextDouble();
	}
}
