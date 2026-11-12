using Stimulsoft.Blockly.Blocks;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.StiBlocks.Data;

public class StiDataSourceProperty : IronBlock
{
	public override object Evaluate(Context context)
	{
		object obj = base.Values.Evaluate("OBJECT", context);
		if (obj != null)
		{
			string name = base.Fields.Get("NAME").ToString();
			return obj.GetType().GetProperty(name).GetValue(obj, null);
		}
		return base.Evaluate(context);
	}
}
