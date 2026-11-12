using Stimulsoft.Blockly.Blocks;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.StiBlocks.Objects;

public class StiGetPropertyOfObject : IronBlock
{
	public override object Evaluate(Context context)
	{
		object obj = base.Values.Evaluate("OBJECT", context);
		if (obj != null)
		{
			string name = base.Values.Evaluate("PROPERTY", context).ToString();
			return obj.GetType().GetProperty(name).GetValue(obj, null);
		}
		return base.Evaluate(context);
	}
}
