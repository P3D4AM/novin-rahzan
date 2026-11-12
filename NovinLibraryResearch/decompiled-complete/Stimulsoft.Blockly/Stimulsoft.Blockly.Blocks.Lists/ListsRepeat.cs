using System.Collections.Generic;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Lists;

public class ListsRepeat : IronBlock
{
	public override object Evaluate(Context context)
	{
		object item = base.Values.Evaluate("ITEM", context);
		double num = StiObjConverter.ToDouble(base.Values.Evaluate("NUM", context));
		List<object> list = new List<object>();
		for (int i = 0; (double)i < num; i++)
		{
			list.Add(item);
		}
		return list;
	}
}
