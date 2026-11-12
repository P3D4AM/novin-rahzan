using System;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Blockly.Blocks;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.StiBlocks.Visuals;

public class StiNewBorder : IronBlock
{
	public override object Evaluate(Context context)
	{
		Color color = ColorTranslator.FromHtml(base.Values.Evaluate("COLOR", context).ToString());
		double size = Convert.ToDouble(base.Values.Evaluate("SIZE", context));
		StiPenStyle style = (StiPenStyle)base.Values.Evaluate("STYLE", context);
		bool flag = Convert.ToBoolean(base.Fields.Get("TOP"));
		bool flag2 = Convert.ToBoolean(base.Fields.Get("LEFT"));
		bool flag3 = Convert.ToBoolean(base.Fields.Get("RIGHT"));
		bool flag4 = Convert.ToBoolean(base.Fields.Get("BOTTOM"));
		StiBorderSides stiBorderSides = StiBorderSides.None;
		if (flag)
		{
			stiBorderSides |= StiBorderSides.Top;
		}
		if (flag2)
		{
			stiBorderSides |= StiBorderSides.Left;
		}
		if (flag3)
		{
			stiBorderSides |= StiBorderSides.Right;
		}
		if (flag4)
		{
			stiBorderSides |= StiBorderSides.Bottom;
		}
		return new StiBorder(stiBorderSides, color, size, style);
	}
}
