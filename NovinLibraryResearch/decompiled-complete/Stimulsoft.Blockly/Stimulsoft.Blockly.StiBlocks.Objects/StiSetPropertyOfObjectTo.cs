using System.Reflection;
using Stimulsoft.Base;
using Stimulsoft.Blockly.Blocks;
using Stimulsoft.Blockly.Model;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Blockly.StiBlocks.Objects;

public class StiSetPropertyOfObjectTo : IronBlock
{
	public override object Evaluate(Context context)
	{
		object obj = base.Values.Evaluate("OBJECT", context);
		if (obj != null)
		{
			string name = base.Values.Evaluate("PROPERTY", context).ToString();
			object obj2 = base.Values.Evaluate("VALUE", context);
			PropertyInfo property = obj.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public);
			if (null != property && property.CanWrite)
			{
				if (obj2 is StiExpression stiExpression)
				{
					obj2 = stiExpression.Value;
				}
				if (property.PropertyType == typeof(StiExpression))
				{
					string value = (string)StiConvert.ChangeType(obj2, typeof(string));
					obj2 = new StiExpression
					{
						Value = value
					};
				}
				object value2 = StiConvert.ChangeType(obj2, property.PropertyType);
				property.SetValue(obj, value2, null);
			}
		}
		context.Report.Invalidate();
		return base.Evaluate(context);
	}
}
