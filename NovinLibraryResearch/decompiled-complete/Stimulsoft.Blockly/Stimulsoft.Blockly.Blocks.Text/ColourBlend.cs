using System;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Text;

public class ColourBlend : IronBlock
{
	public override object Evaluate(Context context)
	{
		string text = (base.Values.Evaluate("COLOUR1", context) ?? "").ToString();
		string text2 = (base.Values.Evaluate("COLOUR2", context) ?? "").ToString();
		double num = Math.Min(Math.Max((double)base.Values.Evaluate("RATIO", context), 0.0), 1.0);
		if (string.IsNullOrWhiteSpace(text) || text.Length != 7)
		{
			return null;
		}
		if (string.IsNullOrWhiteSpace(text2) || text2.Length != 7)
		{
			return null;
		}
		byte b = (byte)((double)(int)Convert.ToByte(text.Substring(1, 2), 16) * (1.0 - num) + (double)(int)Convert.ToByte(text2.Substring(1, 2), 16) * num);
		byte b2 = (byte)((double)(int)Convert.ToByte(text.Substring(3, 2), 16) * (1.0 - num) + (double)(int)Convert.ToByte(text2.Substring(3, 2), 16) * num);
		byte b3 = (byte)((double)(int)Convert.ToByte(text.Substring(5, 2), 16) * (1.0 - num) + (double)(int)Convert.ToByte(text2.Substring(5, 2), 16) * num);
		return $"#{b:x2}{b2:x2}{b3:x2}";
	}
}
