using Stimulsoft.Base.Blocks;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.StiBlocks.Objects;

public class StiGetCurrentValue : IronBlock
{
	public override object Evaluate(Context context)
	{
		if (context.EventArgs is IStiBlocklyValueEventArgs)
		{
			dynamic eventArgs = context.EventArgs;
			return eventArgs.Value;
		}
		return base.Evaluate(context);
	}
}
