using System.Globalization;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Maths;

public class MathNumber : IronBlock
{
	public override object Evaluate(Context context)
	{
		return double.Parse(base.Fields.Get("NUM"), CultureInfo.InvariantCulture);
	}
}
