using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.StiBlocks.Report;

public class StiThisComponent : IronBlock
{
	public override object Evaluate(Context context)
	{
		return context.Sender;
	}
}
