using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.StiBlocks.Report;

public class StiAllComponents : IronBlock
{
	public override object Evaluate(Context context)
	{
		if (!context.Report.IsRendered)
		{
			return context.Report.GetComponents().ToList();
		}
		return context.Report.GetRenderedComponents().ToList();
	}
}
