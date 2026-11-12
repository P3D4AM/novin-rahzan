using System.ComponentModel;
using System.Globalization;

namespace Stimulsoft.Base.Drawing.Design;

public class StiExpressionColorConverter : StiColorConverter
{
	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		if (value is string && StiAppExpressionHelper.IsExpressionSpecified(context.Instance, context.PropertyDescriptor.Name))
		{
			StiAppExpressionHelper.SetExpression(context.Instance, context.PropertyDescriptor.Name, value as string);
			return context.PropertyDescriptor.GetValue(context.Instance);
		}
		return base.ConvertFrom(context, culture, value);
	}
}
