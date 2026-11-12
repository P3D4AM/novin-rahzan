using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Logic;

public class LogicNegate : IronBlock
{
	public override object Evaluate(Context context)
	{
		return !(bool)(base.Values.Evaluate("BOOL", context) ?? ((object)false));
	}
}
