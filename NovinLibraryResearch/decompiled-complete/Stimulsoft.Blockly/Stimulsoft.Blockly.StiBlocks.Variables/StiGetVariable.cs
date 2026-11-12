using Stimulsoft.Blockly.Blocks;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.StiBlocks.Variables;

public class StiGetVariable : IronBlock
{
	public override object Evaluate(Context context)
	{
		string name = base.Fields.Get("VALUE").ToString();
		return context.Report[name];
	}
}
