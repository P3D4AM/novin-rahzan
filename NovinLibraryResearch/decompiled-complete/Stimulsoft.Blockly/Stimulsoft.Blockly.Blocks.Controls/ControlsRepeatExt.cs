using System.Linq;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Controls;

public class ControlsRepeatExt : IronBlock
{
	public override object Evaluate(Context context)
	{
		double num = StiObjConverter.ToDouble(base.Values.Evaluate("TIMES", context));
		if (!base.Statements.Any((Statement x) => x.Name == "DO"))
		{
			return base.Evaluate(context);
		}
		Statement statement = base.Statements.Get("DO");
		for (int i = 0; (double)i < num; i++)
		{
			statement.Evaluate(context);
			if (context.EscapeMode == EscapeMode.Break)
			{
				context.EscapeMode = EscapeMode.None;
				break;
			}
			context.EscapeMode = EscapeMode.None;
		}
		context.EscapeMode = EscapeMode.None;
		return base.Evaluate(context);
	}
}
