using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Text;

public class ColourPicker : IronBlock
{
	public override object Evaluate(Context context)
	{
		return base.Fields.Get("COLOUR") ?? "#000000";
	}
}
