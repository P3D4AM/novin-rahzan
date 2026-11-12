using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Text;

public class TextBlock : IronBlock
{
	public override object Evaluate(Context context)
	{
		return base.Fields.Get("TEXT");
	}
}
