using System.Collections;
using System.Linq;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Controls;

public class ControlsForEach : IronBlock
{
	public override object Evaluate(Context context)
	{
		string key = base.Fields.Get("VAR");
		IEnumerable enumerable = base.Values.Evaluate("LIST", context) as IEnumerable;
		Statement statement = base.Statements.Where((Statement x) => x.Name == "DO").FirstOrDefault();
		if (statement == null)
		{
			return base.Evaluate(context);
		}
		foreach (object item in enumerable)
		{
			if (context.Variables.ContainsKey(key))
			{
				context.Variables[key] = item;
			}
			else
			{
				context.Variables.Add(key, item);
			}
			statement.Evaluate(context);
		}
		return base.Evaluate(context);
	}
}
