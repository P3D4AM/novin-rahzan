using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Text;

public class TextIsEmpty : IronBlock
{
	public override object Evaluate(Context context)
	{
		string value = (base.Values.Evaluate("VALUE", context) ?? "").ToString();
		return string.IsNullOrEmpty(value);
	}
}
