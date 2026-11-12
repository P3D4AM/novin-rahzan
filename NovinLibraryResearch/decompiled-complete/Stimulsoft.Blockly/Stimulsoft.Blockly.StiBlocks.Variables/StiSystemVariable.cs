using Stimulsoft.Blockly.Blocks;
using Stimulsoft.Blockly.Model;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Blockly.StiBlocks.Variables;

public class StiSystemVariable : IronBlock
{
	public override object Evaluate(Context context)
	{
		string text = base.Fields.Get("NAME").ToString();
		return StiReportParser.Parse("{" + text + "}", context.Report);
	}
}
