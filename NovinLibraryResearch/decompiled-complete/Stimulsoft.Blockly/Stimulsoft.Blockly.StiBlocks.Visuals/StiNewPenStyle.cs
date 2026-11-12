using Stimulsoft.Base.Drawing;
using Stimulsoft.Blockly.Blocks;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.StiBlocks.Visuals;

public class StiNewPenStyle : IronBlock
{
	public override object Evaluate(Context context)
	{
		return base.Fields.Get("VALUE") switch
		{
			"SOLID" => StiPenStyle.Solid, 
			"DASH" => StiPenStyle.Dash, 
			"DASHDOT" => StiPenStyle.DashDot, 
			"DASHDOTDOT" => StiPenStyle.DashDotDot, 
			"DOT" => StiPenStyle.Dot, 
			"DOUBLE" => StiPenStyle.Double, 
			"NONE" => StiPenStyle.None, 
			_ => StiPenStyle.Solid, 
		};
	}
}
