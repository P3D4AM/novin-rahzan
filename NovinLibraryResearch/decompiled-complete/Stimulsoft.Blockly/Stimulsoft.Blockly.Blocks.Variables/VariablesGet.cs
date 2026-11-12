using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Variables;

public class VariablesGet : IronBlock
{
	public override object Evaluate(Context context)
	{
		string key = base.Fields.Get("VAR");
		if (!context.Variables.ContainsKey(key))
		{
			return null;
		}
		return context.Variables[key];
	}
}
