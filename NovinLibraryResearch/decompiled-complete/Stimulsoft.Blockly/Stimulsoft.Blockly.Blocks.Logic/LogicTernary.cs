using System.Linq;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Logic;

public class LogicTernary : IronBlock
{
	public override object Evaluate(Context context)
	{
		if ((bool)base.Values.Evaluate("IF", context) && base.Values.Any((Value x) => x.Name == "THEN"))
		{
			return base.Values.Evaluate("THEN", context);
		}
		return null;
	}
}
