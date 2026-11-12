using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Logic;

public class LogicBoolean : IronBlock
{
	public override object Evaluate(Context context)
	{
		return bool.Parse(base.Fields.Get("BOOL"));
	}
}
