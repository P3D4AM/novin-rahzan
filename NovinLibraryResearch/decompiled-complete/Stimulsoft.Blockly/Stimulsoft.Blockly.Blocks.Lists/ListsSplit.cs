using System;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Lists;

public class ListsSplit : IronBlock
{
	public override object Evaluate(Context context)
	{
		string text = base.Fields.Get("MODE");
		object obj = base.Values.Evaluate("INPUT", context);
		object obj2 = base.Values.Evaluate("DELIM", context);
		if (!(text == "SPLIT"))
		{
			if (text == "JOIN")
			{
				return string.Join(obj2.ToString(), (obj as IEnumerable<object>).Select((object x) => x.ToString()));
			}
			throw new NotSupportedException("unknown mode: " + text);
		}
		return ((IEnumerable<string>)obj.ToString().Split(new string[1] { obj2.ToString() }, StringSplitOptions.None)).Select((Func<string, object>)((string x) => x)).ToList();
	}
}
