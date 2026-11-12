using System;
using System.Linq;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Procedures;

public class ProceduresCallReturn : ProceduresCallNoReturn
{
	public override object Evaluate(Context context)
	{
		string value = base.Mutations.GetValue("name");
		if (!context.Functions.ContainsKey(value))
		{
			throw new MissingMethodException("Method '" + value + "' not defined");
		}
		IFragment fragment = (IFragment)context.Functions[value];
		Context context2 = new Context
		{
			Parent = context
		};
		context2.Functions = context.Functions;
		int num = 0;
		foreach (Mutation item in base.Mutations.Where((Mutation x) => x.Domain == "arg" && x.Name == "name"))
		{
			object value2 = base.Values.Evaluate($"ARG{num}", context);
			context2.Variables.Add(item.Value, value2);
			num++;
		}
		return fragment.Evaluate(context2);
	}
}
