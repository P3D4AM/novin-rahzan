using System;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Controls;

public class ControlsFlowStatement : IronBlock
{
	public override object Evaluate(Context context)
	{
		string text = base.Fields.Get("FLOW");
		if (text == "CONTINUE")
		{
			context.EscapeMode = EscapeMode.Continue;
			return null;
		}
		if (text == "BREAK")
		{
			context.EscapeMode = EscapeMode.Break;
			return null;
		}
		throw new NotSupportedException(text + " flow is not supported");
	}
}
