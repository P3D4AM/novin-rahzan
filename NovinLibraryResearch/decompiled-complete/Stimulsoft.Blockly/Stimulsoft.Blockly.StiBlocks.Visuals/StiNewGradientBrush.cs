using System;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Blockly.Blocks;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.StiBlocks.Visuals;

public class StiNewGradientBrush : IronBlock
{
	public override object Evaluate(Context context)
	{
		string htmlColor = base.Values.Evaluate("STARTCOLOR", context).ToString();
		string htmlColor2 = base.Values.Evaluate("ENDCOLOR", context).ToString();
		object value = base.Values.Evaluate("ANGLE", context);
		return new StiGradientBrush(ColorTranslator.FromHtml(htmlColor), ColorTranslator.FromHtml(htmlColor2), Convert.ToDouble(value));
	}
}
