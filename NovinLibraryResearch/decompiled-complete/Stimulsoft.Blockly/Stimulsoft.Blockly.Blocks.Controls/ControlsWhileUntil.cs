using System.Linq;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Controls;

public class ControlsWhileUntil : IronBlock
{
	public override object Evaluate(Context context)
	{
		string text = base.Fields.Get("MODE");
		Value value = base.Values.FirstOrDefault((Value x) => x.Name == "BOOL");
		if (!base.Statements.Any((Statement x) => x.Name == "DO") || value == null)
		{
			return base.Evaluate(context);
		}
		Statement statement = base.Statements.Get("DO");
		if (text == "WHILE")
		{
			while ((bool)value.Evaluate(context))
			{
				statement.Evaluate(context);
			}
		}
		else
		{
			while (!(bool)value.Evaluate(context))
			{
				statement.Evaluate(context);
			}
		}
		return base.Evaluate(context);
	}
}
