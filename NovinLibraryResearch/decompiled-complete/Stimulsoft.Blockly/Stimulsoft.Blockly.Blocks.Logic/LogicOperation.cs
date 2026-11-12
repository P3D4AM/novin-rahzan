using System;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Logic;

public class LogicOperation : IronBlock
{
	public override object Evaluate(Context context)
	{
		bool flag = (bool)(base.Values.Evaluate("A", context) ?? ((object)false));
		bool flag2 = (bool)(base.Values.Evaluate("B", context) ?? ((object)false));
		string text = base.Fields.Get("OP");
		if (!(text == "AND"))
		{
			if (text == "OR")
			{
				return flag || flag2;
			}
			throw new ApplicationException("Unknown OP " + text);
		}
		return flag && flag2;
	}
}
