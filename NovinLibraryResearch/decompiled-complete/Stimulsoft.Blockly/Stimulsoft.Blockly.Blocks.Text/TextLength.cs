using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Text;

public class TextLength : IronBlock
{
	public override object Evaluate(Context context)
	{
		string text = (base.Values.Evaluate("VALUE", context) ?? "").ToString();
		return (double)text.Length;
	}
}
