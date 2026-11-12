using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Logic;

public class LogicNull : IronBlock
{
	public override object Evaluate(Context context)
	{
		return null;
	}
}
