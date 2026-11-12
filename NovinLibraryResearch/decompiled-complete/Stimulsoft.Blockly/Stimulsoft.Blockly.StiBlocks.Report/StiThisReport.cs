using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.StiBlocks.Report;

public class StiThisReport : IronBlock
{
	public override object Evaluate(Context context)
	{
		return context.Report;
	}
}
