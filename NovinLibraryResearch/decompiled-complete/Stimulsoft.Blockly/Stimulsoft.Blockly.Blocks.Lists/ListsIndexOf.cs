using System;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Lists;

public class ListsIndexOf : IronBlock
{
	public override object Evaluate(Context context)
	{
		string text = base.Fields.Get("END");
		IEnumerable<object> source = base.Values.Evaluate("VALUE", context) as IEnumerable<object>;
		object item = base.Values.Evaluate("FIND", context);
		if (!(text == "FIRST"))
		{
			if (text == "LAST")
			{
				return source.ToList().LastIndexOf(item) + 1;
			}
			throw new NotSupportedException("$Unknown end: {direction}");
		}
		return source.ToList().IndexOf(item) + 1;
	}
}
