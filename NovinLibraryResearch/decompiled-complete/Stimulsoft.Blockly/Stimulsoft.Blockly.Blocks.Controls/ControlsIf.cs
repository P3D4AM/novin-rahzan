using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Controls;

public class ControlsIf : IronBlock
{
	public override object Evaluate(Context context)
	{
		int num = 1;
		if (base.Mutations.GetValue("elseif") != null)
		{
			string value = base.Mutations.GetValue("elseif");
			num = int.Parse(value) + 1;
		}
		bool flag = false;
		for (int i = 0; i < num; i++)
		{
			if ((bool)base.Values.Evaluate($"IF{i}", context))
			{
				Statement statement = base.Statements.Get($"DO{i}");
				statement.Evaluate(context);
				flag = true;
				break;
			}
		}
		if (!flag && base.Mutations.GetValue("else") != null)
		{
			string value2 = base.Mutations.GetValue("else");
			if (value2 == "1")
			{
				Statement statement2 = base.Statements.Get("ELSE");
				statement2.Evaluate(context);
			}
		}
		return base.Evaluate(context);
	}
}
