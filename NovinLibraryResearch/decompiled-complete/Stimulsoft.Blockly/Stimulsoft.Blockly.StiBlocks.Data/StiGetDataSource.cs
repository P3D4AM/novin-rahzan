using Stimulsoft.Blockly.Blocks;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.StiBlocks.Data;

public class StiGetDataSource : IronBlock
{
	public override object Evaluate(Context context)
	{
		string name = base.Fields.Get("NAME").ToString();
		return context.Report.Dictionary.DataSources[name];
	}
}
