using System;
using Stimulsoft.Base.Blocks;
using Stimulsoft.Blockly.Blocks;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.StiBlocks.Objects;

public class StiSetCurrentValue : IronBlock
{
	public override object Evaluate(Context context)
	{
		if (context.EventArgs is IStiBlocklyValueEventArgs)
		{
			object obj = base.Values.Evaluate("VALUE", context);
			dynamic eventArgs = context.EventArgs;
			Type type = ((object)eventArgs.Value).GetType();
			if (!(type == typeof(string)))
			{
				eventArgs.Value = obj;
			}
			else
			{
				eventArgs.Value = obj.ToString();
			}
		}
		return base.Evaluate(context);
	}
}
