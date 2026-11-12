using Stimulsoft.Blockly.Blocks;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.StiBlocks.Data;

public class StiGetDataSourceByName : IronBlock
{
	public override object Evaluate(Context context)
	{
		string name = base.Values.Evaluate("NAME", context).ToString();
		return context.Report.Dictionary.DataSources[name];
	}
}
