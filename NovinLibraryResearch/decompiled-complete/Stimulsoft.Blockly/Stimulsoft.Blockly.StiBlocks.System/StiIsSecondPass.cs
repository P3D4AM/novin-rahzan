using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.StiBlocks.System;

public class StiIsSecondPass : IronBlock
{
	public override object Evaluate(Context context)
	{
		return context.Report.IsSecondPass;
	}
}
