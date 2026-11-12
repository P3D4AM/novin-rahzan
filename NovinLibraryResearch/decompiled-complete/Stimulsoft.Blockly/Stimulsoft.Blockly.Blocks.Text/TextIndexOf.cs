using System;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Text;

public class TextIndexOf : IronBlock
{
	public override object Evaluate(Context context)
	{
		string text = base.Fields.Get("END");
		string text2 = (base.Values.Evaluate("VALUE", context) ?? "").ToString();
		string value = (base.Values.Evaluate("FIND", context) ?? "").ToString();
		if (!(text == "FIRST"))
		{
			if (text == "LAST")
			{
				return StiObjConverter.ToDouble(text2.LastIndexOf(value)) + 1.0;
			}
			throw new ApplicationException("unknown mode");
		}
		return StiObjConverter.ToDouble(text2.IndexOf(value)) + 1.0;
	}
}
