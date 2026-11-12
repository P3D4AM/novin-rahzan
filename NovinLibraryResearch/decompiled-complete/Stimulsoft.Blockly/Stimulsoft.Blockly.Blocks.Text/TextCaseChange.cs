using System;
using System.Globalization;
using System.Linq;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Text;

public class TextCaseChange : IronBlock
{
	public override object Evaluate(Context context)
	{
		string text = base.Fields.Get("CASE").ToString();
		string text2 = (base.Values.Evaluate("TEXT", context) ?? "").ToString();
		switch (text)
		{
		case "UPPERCASE":
			return text2.ToUpper();
		case "LOWERCASE":
			return text2.ToLower();
		case "TITLECASE":
		{
			if (text2.Replace(" ", "").All(char.IsUpper))
			{
				text2 = text2.ToLower();
			}
			TextInfo textInfo = new CultureInfo("en-US", useUserOverride: false).TextInfo;
			return textInfo.ToTitleCase(text2);
		}
		default:
			throw new NotSupportedException("unknown case");
		}
	}
}
