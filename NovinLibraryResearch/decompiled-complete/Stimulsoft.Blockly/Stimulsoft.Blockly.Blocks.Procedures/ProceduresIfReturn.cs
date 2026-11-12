using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Procedures;

public class ProceduresIfReturn : IronBlock
{
	public override object Evaluate(Context context)
	{
		object obj = base.Values.Evaluate("CONDITION", context);
		if ((bool)obj)
		{
			return base.Values.Evaluate("VALUE", context);
		}
		return base.Evaluate(context);
	}
}
