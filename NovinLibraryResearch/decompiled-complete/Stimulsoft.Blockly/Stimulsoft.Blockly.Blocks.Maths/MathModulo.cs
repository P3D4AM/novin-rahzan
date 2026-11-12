using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Maths;

public class MathModulo : IronBlock
{
	public override object Evaluate(Context context)
	{
		double num = StiObjConverter.ToDouble(base.Values.Evaluate("DIVIDEND", context));
		double num2 = StiObjConverter.ToDouble(base.Values.Evaluate("DIVISOR", context));
		return num % num2;
	}
}
