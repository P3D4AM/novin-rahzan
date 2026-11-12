using System;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Text;

public class TextTrim : IronBlock
{
	public override object Evaluate(Context context)
	{
		string text = base.Fields.Get("MODE");
		string text2 = (base.Values.Evaluate("TEXT", context) ?? "").ToString();
		return text switch
		{
			"BOTH" => text2.Trim(), 
			"LEFT" => text2.TrimStart(new char[0]), 
			"RIGHT" => text2.TrimEnd(new char[0]), 
			_ => throw new ApplicationException("unknown mode"), 
		};
	}
}
