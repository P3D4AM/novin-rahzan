using Stimulsoft.Blockly.Blocks;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.StiBlocks.Variables;

public class StiGetVariableByName : IronBlock
{
	public override object Evaluate(Context context)
	{
		string name = base.Fields.Get("VALUE");
		return context.Report[name];
	}
}
