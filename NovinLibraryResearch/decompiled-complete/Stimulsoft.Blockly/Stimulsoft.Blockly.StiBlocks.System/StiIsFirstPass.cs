using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.StiBlocks.System;

public class StiIsFirstPass : IronBlock
{
	public override object Evaluate(Context context)
	{
		return context.Report.IsFirstPass;
	}
}
