using System.Collections.Generic;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Variables;

public class VariablesSet : IronBlock
{
	public override object Evaluate(Context context)
	{
		IDictionary<string, object> variables = context.Variables;
		object value = base.Values.Evaluate("VALUE", context);
		string key = base.Fields.Get("VAR");
		if (variables.ContainsKey(key))
		{
			variables[key] = value;
		}
		else
		{
			variables.Add(key, value);
		}
		return base.Evaluate(context);
	}
}
