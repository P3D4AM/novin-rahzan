using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Procedures;

public class ValueBlock : IronBlock
{
	private Value value;

	public override object Evaluate(Context context)
	{
		return value.Evaluate(context);
	}

	public ValueBlock(Value value)
	{
		this.value = value;
	}
}
