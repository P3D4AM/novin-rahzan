using Stimulsoft.Base;
using Stimulsoft.Blockly.Blocks;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.StiBlocks.Process;

public class StiOpenLink : IronBlock
{
	public override object Evaluate(Context context)
	{
		string text = base.Values.Evaluate("VALUE", context).ToString();
		StiProcess.Start("\"" + text + "\"");
		return base.Evaluate(context);
	}
}
