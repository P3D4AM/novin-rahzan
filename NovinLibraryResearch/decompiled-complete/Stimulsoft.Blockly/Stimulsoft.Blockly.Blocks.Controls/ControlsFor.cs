using System.Linq;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Controls;

public class ControlsFor : IronBlock
{
	public override object Evaluate(Context context)
	{
		string key = base.Fields.Get("VAR");
		double num = StiObjConverter.ToDouble(base.Values.Evaluate("FROM", context));
		double num2 = StiObjConverter.ToDouble(base.Values.Evaluate("TO", context));
		double num3 = StiObjConverter.ToDouble(base.Values.Evaluate("BY", context));
		Statement statement = base.Statements.FirstOrDefault();
		if (context.Variables.ContainsKey(key))
		{
			context.Variables[key] = num;
		}
		else
		{
			context.Variables.Add(key, num);
		}
		while ((double)context.Variables[key] <= num2)
		{
			statement.Evaluate(context);
			context.Variables[key] = (double)context.Variables[key] + num3;
		}
		return base.Evaluate(context);
	}
}
