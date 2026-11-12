using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Base.Drawing.Design;

public class StiExpressionFontConverter : FontConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			if (value == null && StiVisualStatePermissionAttribute.IsFromDefaultStateAllowed(context))
			{
				return Loc.GetReport("FromDefaultState");
			}
			string expressionFromInstance = StiAppExpressionHelper.GetExpressionFromInstance(context);
			if (!string.IsNullOrWhiteSpace(expressionFromInstance))
			{
				return expressionFromInstance;
			}
		}
		return ((FontConverter)this).ConvertTo(context, culture, value, destinationType);
	}

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		if (value is string && StiAppExpressionHelper.IsExpressionSpecified(context.Instance, context.PropertyDescriptor.Name))
		{
			StiAppExpressionHelper.SetExpression(context.Instance, context.PropertyDescriptor.Name, value as string);
			return context.PropertyDescriptor.GetValue(context.Instance);
		}
		return ((FontConverter)this).ConvertFrom(context, culture, value);
	}
}
