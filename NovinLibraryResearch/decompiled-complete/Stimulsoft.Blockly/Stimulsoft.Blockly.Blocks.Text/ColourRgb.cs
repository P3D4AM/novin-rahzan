using System;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Text;

public class ColourRgb : IronBlock
{
	public override object Evaluate(Context context)
	{
		byte b = Convert.ToByte(base.Values.Evaluate("RED", context));
		byte b2 = Convert.ToByte(base.Values.Evaluate("GREEN", context));
		byte b3 = Convert.ToByte(base.Values.Evaluate("BLUE", context));
		return $"#{b:x2}{b2:x2}{b3:x2}";
	}
}
