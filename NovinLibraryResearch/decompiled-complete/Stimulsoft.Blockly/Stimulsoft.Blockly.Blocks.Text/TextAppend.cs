using System.Collections.Generic;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Text;

public class TextAppend : IronBlock
{
	public override object Evaluate(Context context)
	{
		IDictionary<string, object> variables = context.Variables;
		string key = base.Fields.Get("VAR");
		string text = (base.Values.Evaluate("TEXT", context) ?? "").ToString();
		if (!variables.ContainsKey(key))
		{
			variables.Add(key, "");
		}
		string text2 = variables[key].ToString();
		variables[key] = text2 + text;
		return base.Evaluate(context);
	}
}
