using Stimulsoft.Blockly.Blocks;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.StiBlocks.Visuals;

public class StiColorHex : IronBlock
{
	public override object Evaluate(Context context)
	{
		return base.Values.Evaluate("TEXT", context).ToString();
	}
}
