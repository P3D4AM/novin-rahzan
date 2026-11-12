using Stimulsoft.Blockly.Blocks;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.StiBlocks.Objects;

public class StiGetStyleByName : IronBlock
{
	public override object Evaluate(Context context)
	{
		string name = base.Values.Evaluate("VALUE", context).ToString();
		return context.Report.Styles[name];
	}
}
