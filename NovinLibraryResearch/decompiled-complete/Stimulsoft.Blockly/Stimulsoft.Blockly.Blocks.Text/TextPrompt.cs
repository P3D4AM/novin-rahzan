using System;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Text;

public class TextPrompt : IronBlock
{
	public override object Evaluate(Context context)
	{
		string text = base.Mutations.GetValue("type") ?? "TEXT";
		string text2 = (base.Values.Evaluate("TEXT", context) ?? "").ToString();
		if (!string.IsNullOrWhiteSpace(text2))
		{
			Console.Write(text2 + ": ");
		}
		string text3 = Console.ReadLine();
		if (text == "NUMBER")
		{
			return int.Parse(text3);
		}
		return text3;
	}
}
