using System.Collections.Generic;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Lists;

public class ListsCreateWith : IronBlock
{
	public override object Evaluate(Context context)
	{
		List<object> list = new List<object>();
		foreach (Value value in base.Values)
		{
			list.Add(value.Evaluate(context));
		}
		return list;
	}
}
