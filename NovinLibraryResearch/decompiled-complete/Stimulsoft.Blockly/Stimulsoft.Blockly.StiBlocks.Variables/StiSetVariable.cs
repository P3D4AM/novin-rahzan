using Stimulsoft.Blockly.Blocks;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.StiBlocks.Variables;

public class StiSetVariable : IronBlock
{
	public override object Evaluate(Context context)
	{
		string name = base.Values.Evaluate("NAME", context).ToString();
		object value = base.Values.Evaluate("VALUE", context);
		context.Report[name] = value;
		return base.Evaluate(context);
	}
}
