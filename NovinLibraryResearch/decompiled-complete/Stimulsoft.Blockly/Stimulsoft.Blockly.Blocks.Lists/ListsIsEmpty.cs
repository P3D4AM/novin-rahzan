using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Lists;

public class ListsIsEmpty : IronBlock
{
	public override object Evaluate(Context context)
	{
		if (!(base.Values.Evaluate("VALUE", context) is IEnumerable<object> source))
		{
			return true;
		}
		return !source.Any();
	}
}
