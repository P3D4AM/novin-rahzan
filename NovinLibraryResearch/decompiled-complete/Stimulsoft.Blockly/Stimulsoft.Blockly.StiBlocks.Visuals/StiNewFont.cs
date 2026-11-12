using System;
using System.Drawing;
using Stimulsoft.Blockly.Blocks;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.StiBlocks.Visuals;

public class StiNewFont : IronBlock
{
	public override object Evaluate(Context context)
	{
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		string text = base.Fields.Get("NAME").ToString();
		float num = (float)Convert.ToDouble(base.Values.Evaluate("SIZE", context));
		bool flag = Convert.ToBoolean(base.Fields.Get("BOLD"));
		bool flag2 = Convert.ToBoolean(base.Fields.Get("ITALIC"));
		bool flag3 = Convert.ToBoolean(base.Fields.Get("UNDERLINE"));
		bool flag4 = Convert.ToBoolean(base.Fields.Get("STRIKEOUT"));
		FontStyle val = (FontStyle)0;
		if (flag)
		{
			val = (FontStyle)(val | 1);
		}
		if (flag2)
		{
			val = (FontStyle)(val | 2);
		}
		if (flag3)
		{
			val = (FontStyle)(val | 4);
		}
		if (flag4)
		{
			val = (FontStyle)(val | 8);
		}
		return (object)new Font(text, num, val);
	}
}
