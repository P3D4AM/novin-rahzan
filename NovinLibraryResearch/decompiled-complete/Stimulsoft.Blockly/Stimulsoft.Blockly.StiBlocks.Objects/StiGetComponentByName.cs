using Stimulsoft.Blockly.Blocks;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.StiBlocks.Objects;

public class StiGetComponentByName : IronBlock
{
	public override object Evaluate(Context context)
	{
		string componentName = base.Fields.Get("NAME");
		if (!context.Report.IsRendered)
		{
			return context.Report.GetComponentByName(componentName);
		}
		return context.Report.RenderedPages.GetComponentByName(componentName);
	}
}
