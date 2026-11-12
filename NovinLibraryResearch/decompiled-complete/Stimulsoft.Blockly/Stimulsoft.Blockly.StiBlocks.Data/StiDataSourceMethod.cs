using System.Reflection;
using Stimulsoft.Blockly.Blocks;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.StiBlocks.Data;

public class StiDataSourceMethod : IronBlock
{
	public override object Evaluate(Context context)
	{
		object obj = base.Values.Evaluate("OBJECT", context);
		if (obj != null)
		{
			string name = base.Fields.Get("NAME").ToString();
			MethodInfo method = obj.GetType().GetMethod(name);
			return method.Invoke(obj, new object[0]);
		}
		return base.Evaluate(context);
	}
}
