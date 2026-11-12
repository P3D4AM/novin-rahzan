using System;
using Stimulsoft.Blockly.Blocks;
using Stimulsoft.Blockly.Model;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Blockly.StiBlocks.Visuals;

public class StiNewMargin : IronBlock
{
	public override object Evaluate(Context context)
	{
		double left = Convert.ToDouble(base.Values.Evaluate("LEFT", context));
		double top = Convert.ToDouble(base.Values.Evaluate("TOP", context));
		double right = Convert.ToDouble(base.Values.Evaluate("RIGHT", context));
		double bottom = Convert.ToDouble(base.Values.Evaluate("BOTTOM", context));
		return new StiMargins(left, right, top, bottom);
	}
}
