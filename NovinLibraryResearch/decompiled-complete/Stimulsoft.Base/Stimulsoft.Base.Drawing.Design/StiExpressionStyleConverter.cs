using System;
using System.ComponentModel;
using System.Globalization;

namespace Stimulsoft.Base.Drawing.Design;

public class StiExpressionStyleConverter : TypeConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			string expressionFromInstance = StiAppExpressionHelper.GetExpressionFromInstance(context);
			if (!string.IsNullOrWhiteSpace(expressionFromInstance))
			{
				return expressionFromInstance;
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		if (value is string)
		{
			if (StiAppExpressionHelper.IsExpressionSpecified(context.Instance, context.PropertyDescriptor.Name))
			{
				StiAppExpressionHelper.SetExpression(context.Instance, context.PropertyDescriptor.Name, value as string);
				return context.PropertyDescriptor.GetValue(context.Instance);
			}
			return value;
		}
		return base.ConvertFrom(context, culture, value);
	}

	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
	{
		if (sourceType == typeof(string))
		{
			return true;
		}
		return base.CanConvertFrom(context, sourceType);
	}

	public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
	{
		return false;
	}
}
