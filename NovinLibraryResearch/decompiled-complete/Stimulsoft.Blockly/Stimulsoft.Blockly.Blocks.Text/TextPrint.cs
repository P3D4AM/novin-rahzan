using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Text;

public class TextPrint : IronBlock
{
	public override object Evaluate(Context context)
	{
		object obj = base.Values.Evaluate("TEXT", context);
		return base.Evaluate(context);
	}
}
