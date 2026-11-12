using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Blockly.Blocks;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.StiBlocks.Visuals;

public class StiNewSolidBrush : IronBlock
{
	public override object Evaluate(Context context)
	{
		string htmlColor = base.Values.Evaluate("COLOR", context).ToString();
		return new StiSolidBrush(ColorTranslator.FromHtml(htmlColor));
	}
}
